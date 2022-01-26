using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using BaSta.Link;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace BaSta.TimeSync;

/// <summary>
/// Program class for main entry point.
/// </summary>
public static class Program
{
    private const string ApplicationID = "3FD67FB7-ADAE-40DB-8122-089C41E6A2A4";

    private static readonly ApplicationInfo Info = new(Assembly.GetExecutingAssembly());
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
            DebuggerTarget debugTarget = new DebuggerTarget();
            config.AddRule(LogLevel.Info, LogLevel.Fatal, debugTarget);
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
            TimeSyncSettings settings = TimeSyncSettingsFactory.Load("BaSta.TimeSync" + ".ini");

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