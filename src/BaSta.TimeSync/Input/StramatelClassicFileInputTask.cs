using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace BaSta.TimeSync.Input
{
    internal class StramatelClassicFileInputTask : TimeSyncTaskBase, ITimeSyncInputTask
    {
        internal const string TypeName = "StramatelClassicFile";

        private StramatelClassicTimeParser _parser;
        private string _filePath;
        private StreamReader _fileReader;

        private DateTime? _lastData;
        private static readonly TimeSpan ShiftThreshold = TimeSpan.FromMilliseconds(50);
        private DateTime _lastReceivedTimestamp = DateTime.MinValue;
        private TimeSpan _lastReceivedTime;
        private TimeSpan _lastUsedTime;
        private bool _shift;
        
        protected override void LoadSettings(ITimeSyncSettingsGroup settings)
        {
            //File = [Path]
            //ReadDelay = [ms]

            _filePath = settings.GetValue("File", s => s, null);
        }

        protected override void OnStartSync()
        {
            _fileReader = new StreamReader(_filePath);

            _parser = new StramatelClassicTimeParser($"{Name}_Parser");
            _parser.TimeReceived += OnTimeReceived;
        }

        protected override void OnProcess(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                string line = _fileReader.ReadLine();

                if (string.IsNullOrEmpty(line))
                    break;

                if (line.Contains(':'))
                {
                    DateTime time = DateTime.ParseExact(line, "HH:mm:ss.fff:", CultureInfo.InvariantCulture);
                    line = _fileReader.ReadLine();

                    if (!_lastData.HasValue)
                        _lastData = time;

                    if (time - _lastData.Value < TimeSpan.Zero)
                        _lastData = time;

                    Thread.Sleep((time - _lastData.Value) / 10);

                    _lastData = time;

                    Logger.Debug(line);

                    for (int i = 0; i < line.Length; i = i + 2)
                    {
                        string hex = line.Substring(i, 2);
                        int value = Convert.ToInt32(hex, 16);
                        _parser.Push((byte)value);
                    }
                }
            }
        }

        protected override void OnStopSync()
        {
            _parser.TimeReceived -= OnTimeReceived;
            _parser = null;

            _fileReader?.Dispose();
            _fileReader = null;
        }

        private void OnTimeReceived(object? sender, DataEventArgs<TimeSpan> e)
        {
            DateTime currentTimestamp = DateTime.UtcNow;
            TimeSpan timeDifference = currentTimestamp - _lastReceivedTimestamp;

            // If time goes up or has low transfer schedule it is an indicator that milliseconds are not transferred any longer
            if (e.Data > _lastReceivedTime)
                _shift = false;

            _lastUsedTime = _lastReceivedTime = e.Data;

            // If minute count is larger than 20 or time is transferred fast transferred
            if (!_shift && (_lastReceivedTime.Minutes > 20 || (_lastReceivedTime.Minutes != 10 && timeDifference < ShiftThreshold)))
                _shift = true;

            if (_shift)
                _lastUsedTime = TimeSpan.FromSeconds(_lastReceivedTime.Minutes);

            _lastReceivedTimestamp = currentTimestamp;
            StateChanged?.Invoke(this, null);
        }

        public TimeSpan Pull()
        {
            return _lastUsedTime;
        }

        public event EventHandler StateChanged;
    }
}