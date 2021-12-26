using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BaSta;
using BaSta.Core;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace BaSta.TimeSync
{
    /// <summary>
    /// Program class for main entry point.
    /// </summary>
    public static class Program
    {
        private const string ApplicationID = "3FD67FB7-ADAE-40DB-8122-089C41E6A2A4";

        private static readonly ApplicationInfo Info = new ApplicationInfo(Assembly.GetExecutingAssembly());
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private enum ReturnCode
        {
            OK = 0,
            Error = -1
        }

        /// <summary>
        /// Main entry point.
        /// </summary>
        public static int Main()
        {
            try
            {
                // Logging setup
                LoggingConfiguration config = new LoggingConfiguration();
                FileTarget fileTarget = new FileTarget { FileName = $"{Environment.CurrentDirectory}\\{Info.ProductName}.log" };
                config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
                ConsoleTarget consoleTarget = new ConsoleTarget();
                config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);
                LogManager.Configuration = config;

                // Log all unhandled exceptions
                AppDomain.CurrentDomain.UnhandledException += (s, e) => Logger.Error((Exception)e.ExceptionObject);

                // Log program start
                Logger.Info($"{Info.ProductName} {Info.Version} ({Environment.CommandLine})");
                Logger.Info("Application start");

                // If an instance is already running, signal return code
                if (!ApplicationMutex.Open(ApplicationID))
                    return (int)ReturnCode.Error;

                // Load configuration file 
                TimeSyncSettings settings = TimeSyncSettingsFactory.Load(Info.ProductName + ".ini");

                // Create tasks and start runner
                var input = TimeSyncTaskFactory.CreateInput(settings.Input);
                var outputs = settings.Outputs.Select(TimeSyncTaskFactory.CreateOutput);
                var runner = new TimeSyncRunner(input, outputs);
                runner.Initialize();

                // Wait for user input from the console to stop the program
                AutoResetEvent consoleCancelledEvent = new AutoResetEvent(false);
                Console.CancelKeyPress += (sender, args) =>
                {
                    args.Cancel = true;
                    consoleCancelledEvent.Set();
                };

                Console.WriteLine("Press Ctl+C to stop the program...");
                consoleCancelledEvent.WaitOne();

                // Terminate runner
                runner.Terminate();

                return (int)ReturnCode.OK;
            }
            catch (ReturnCodeException ex)
            {
                // In the case of a definite return code exception return code
                Logger.Error(ex);
                return ex.ReturnCode;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                Console.ReadKey(true);
                return (int)ReturnCode.Error;
            }
            finally
            {
                // Log program end
                Logger.Info("Application end");

                // Release the application mutex
                ApplicationMutex.Close(ApplicationID);
            }
        }
    }

    internal interface ITimeSyncOutputTask : ITimeSyncTask
    {
        void Push(TimeSpan time);
    }

    internal interface ITimeSyncInputTask : ITimeSyncTask
    {
        TimeSpan Pull();

        event EventHandler StateChanged;
    }

    internal interface ITimeSyncTask : IProcessTask
    {
        void LoadSettings(string name, ITimeSyncSettingsGroup settings);
    }

    internal abstract class TimeSyncTaskBase : ProcessTaskBase, ITimeSyncTask
    {
        private ExternalActionProcess _process;

        protected string Name { get; private set; }
        
        protected Logger Logger { get; private set; }

        /// <inheritdoc/>
        public void LoadSettings(string name, ITimeSyncSettingsGroup settings)
        {
            Name = name;
            Logger = LogManager.GetLogger(name);

            LoadSettings(settings);
        }

        protected abstract void LoadSettings(ITimeSyncSettingsGroup settings);

        protected abstract void OnStartSync();

        protected abstract void OnProcess(CancellationToken cancellationToken);

        protected abstract void OnStopSync();

        /// <inheritdoc/>
        protected override void OnInitialize()
        {
            OnStartSync();

            _process = new ExternalActionProcess(OnProcess);
            _process.Initialize();
        }

        /// <inheritdoc/>
        protected override void OnTerminate()
        {
            _process?.Terminate();
            _process = null;

            OnStopSync();
        }
    }

    internal class TimeSyncRunner : ITimeSyncTask
    {
        private const int TaskFinishTimeout = 5000;

        private Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ITimeSyncInputTask _inputTask;
        private readonly ITimeSyncOutputTask[] _outputTasks;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _acceptTask;

        public TimeSyncRunner(ITimeSyncInputTask inputTask, IEnumerable<ITimeSyncOutputTask> outputTasks)
        {
            _inputTask = inputTask;
            _outputTasks = outputTasks.ToArray();
        }

        public bool IsEnabled { get; set; }

        public bool IsInitialized { get; }

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _acceptTask = Task.Factory.StartNew(SyncTask);

            foreach (ITimeSyncOutputTask output in _outputTasks)
                output.Initialize();

            _inputTask.Initialize();
            _inputTask.StateChanged += OnInputStateChanged;
        }

        private readonly AutoResetEvent _stateChanged = new AutoResetEvent(false);

        private void OnInputStateChanged(object? sender, EventArgs e)
        {
            _stateChanged.Set();
        }

        private void SyncTask()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    if (!_stateChanged.WaitOne(1))
                        continue;

                    // Pull the current time value from the input
                    var time = _inputTask.Pull();

                    Logger.Info($"Synchronizing time: {time:mm\\:ss}");

                    // Push the time to all outputs
                    foreach (var output in _outputTasks)
                        output.Push(time);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public void Terminate()
        {
            _inputTask.StateChanged -= OnInputStateChanged;
            _inputTask.Terminate();

            foreach (ITimeSyncOutputTask output in _outputTasks)
                output.Terminate();

            if (_acceptTask == null)
                return;

            _cancellationTokenSource.Cancel();

            _acceptTask.Wait(TaskFinishTimeout);
            _acceptTask = null;


        }

        public void LoadSettings(string name, ITimeSyncSettingsGroup settings) => throw new NotImplementedException();
    }
}