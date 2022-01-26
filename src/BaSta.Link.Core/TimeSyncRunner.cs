using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace BaSta.Link;

public class TimeSyncRunner : ITimeSyncTask
{
    private const int TaskFinishTimeout = 5000;

    private Logger Logger = LogManager.GetCurrentClassLogger();

    private readonly ITimeSyncInputTask _inputTask;
    private readonly ITimeSyncOutputTask[] _outputTasks;
    private CancellationTokenSource _cancellationTokenSource;
    private Task _acceptTask;
    private ConcurrentQueue<TimeSpan> _inputTimeQueue = new();

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

    private void OnInputStateChanged(object sender, DataEventArgs<TimeSpan> dataEventArgs)
    {
        //_stateChanged.Set();
        _inputTimeQueue.Enqueue(dataEventArgs.Data);
    }

    private void SyncTask()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            try
            {
                //if (!_stateChanged.WaitOne(1))
                //    continue;
                if (!_inputTimeQueue.TryDequeue(out TimeSpan time))
                {
                    Thread.Sleep(1);
                    continue;
                }

                // Pull the current time value from the input
                //var time = _inputTask.Pull();

                Logger.Info($"Synchronizing time: {time:hh\\:mm\\:ss\\.fff}");

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