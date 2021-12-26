using System;
using System.Threading;

namespace BaSta.Core
{
    /// <summary>
    /// Process implementation providing the capability to an external action.
    /// </summary>
    public class ExternalActionProcess : ProcessBase
    {
        private readonly Action<CancellationToken> _process;

        /// <summary>
        /// Prevents a default instance of the <see cref="ExternalActionProcess"/> class from being created.
        /// </summary>
        public ExternalActionProcess(Action<CancellationToken> process)
        {
            _process = process;
        }

        /// <summary>
        /// Called when this instance is about to start the processing task.
        /// </summary>
        protected override void OnStart()
        {
        }

        /// <summary>
        /// Called when this instance is about to stop the processing task.
        /// </summary>
        protected override void OnStop()
        {
        }

        /// <summary>
        /// Called sequentially when the processing task is running until cancellation is requested.
        /// </summary>
        /// <param name="token">The cancellation token for the process.</param>
        protected override void OnProcess(CancellationToken token)
        {
            _process(token);
        }
    }
}