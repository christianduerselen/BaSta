using System;
using System.Diagnostics;
using System.Threading;

namespace BaSta.TimeSync
{
    /// <summary>
    /// Class providing interval-based execution of an action by encapsulating a timer.
    /// </summary>
    public class IntervalActionHelper
    {
        private readonly bool _startImmediately;
        private Timer _timer;
        private readonly object _timerLock = new object();
        private readonly TimeSpan _interval;
        private readonly object _executionLock = new object();
        private readonly Action _onElapsed;
        private bool _isEnabled;
        private bool _onElapsedErrorLogged;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalActionHelper"/> class.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="onElapsed">The on elapsed.</param>
        public IntervalActionHelper(TimeSpan interval, Action onElapsed) : this(interval, onElapsed, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalActionHelper"/> class.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="onElapsed">The on elapsed.</param>
        /// <param name="startImmediately">Whether the interval shall hit immediately.</param>
        public IntervalActionHelper(TimeSpan interval, Action onElapsed, bool startImmediately)
        {
            _startImmediately = startImmediately;
            _interval = interval;
            _onElapsed = onElapsed;

            _timer = new Timer(OnTimerElapsed);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="IntervalActionHelper"/> class.
        /// </summary>
        ~IntervalActionHelper()
        {
            lock (_timerLock)
            {
                _timer?.Dispose();
                _timer = null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled, otherwise <c>false</c>.</value>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value)
                    return;

                _isEnabled = value;
                lock (_timerLock)
                {
                    _timer?.Change(_isEnabled ? _startImmediately ? 0 : (int)_interval.TotalMilliseconds : Timeout.Infinite, Timeout.Infinite);
                }
            }
        }

        /// <summary>
        /// Finishes the specified execute action.
        /// </summary>
        /// <param name="executeAction">if set to <c>true</c> executes the action once before cleanup.</param>
        public void Finish(bool executeAction)
        {
            IsEnabled = false;

            // Lock ensures end of OnElapsed before exiting Finish
            lock (_executionLock)
                if (executeAction)
                    OnElapsed();
        }

        protected void OnElapsed()
        {
            // As the timer's thread is created in unmanaged code, any exception would terminate the application
            // So we catch everything by default
            try
            {
                _onElapsed?.Invoke();
            }
            catch (Exception err)
            {
                if (!_onElapsedErrorLogged)
                {
                    _onElapsedErrorLogged = true;
                    Debug.Fail("IntervalActionHelper", err.Message);
                }
            }
        }

        private void OnTimerElapsed(object stateInfo)
        {
            lock (_timerLock)
            {
                _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            }

            lock (_executionLock)
            {
                if (_isEnabled)
                    OnElapsed();
            }

            lock (_timerLock)
            {
                if (_isEnabled)
                    _timer?.Change((int)_interval.TotalMilliseconds, Timeout.Infinite);
            }
        }
    }
}