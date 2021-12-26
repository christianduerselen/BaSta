using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaSta.Core
{
    /// <summary>
    /// Abstract base class for process instances.
    /// </summary>
    public abstract class ProcessBase : ProcessTaskBase
    {
        private Task _task;
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// Called when this instance is about to start the processing task.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Called when this instance is has stopped the processing task.
        /// </summary>
        protected abstract void OnStop();

        /// <inheritdoc />
        protected override void OnInitialize()
        {
            OnStart();

            _cancellationTokenSource = new CancellationTokenSource();
            _task = Task.Factory.StartNew(Process);
        }

        /// <inheritdoc />
        protected override void OnTerminate()
        {
            _cancellationTokenSource?.Cancel();
            _task?.Wait(TimeSpan.FromSeconds(60));

            _task = null;
            _cancellationTokenSource = null;

            OnStop();
        }

        /// <summary>
        /// Called sequentially when the processing task is running until cancellation is requested.
        /// </summary>
        /// <param name="token">The cancellation token for the process.</param>
        protected abstract void OnProcess(CancellationToken token);

        private void Process()
        {
            try
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    OnProcess(_cancellationTokenSource.Token);
                }
            }
            catch
            {
                // Ignored
            }
        }
    }
}