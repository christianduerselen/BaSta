using NLog;
using System;

namespace BaSta.Core
{
    /// <summary>
    /// Abstract base class for an <see cref="IProcessTask"/> implementation.
    /// </summary>
    public abstract class ProcessTaskBase : IProcessTask
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly object _initLock = new object();

        private int _initCount;

        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc/>
        public bool IsInitialized => _initCount > 0;

        /// <inheritdoc/>
        public void Initialize()
        {
            lock (_initLock)
            {
                if (!IsEnabled)
                    return;

                if (!IsInitialized)
                {
                    try
                    {
                        OnInitialize();
                        _initCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                    }
                }
                else
                {
                    _initCount++;
                }
            }
        }

        /// <inheritdoc/>
        public void Terminate()
        {
            lock (_initLock)
            {
                if (!IsEnabled)
                    return;

                if (!IsInitialized)
                    return;

                _initCount--;

                if (_initCount > 0)
                    return;

                try
                {
                    OnTerminate();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }

                _initCount = 0;
            }
        }

        /// <summary>
        /// Called when the instance shall initialize.
        /// </summary>
        protected abstract void OnInitialize();

        /// <summary>
        /// Called when the instance shall terminate.
        /// </summary>
        protected abstract void OnTerminate();
    }
}