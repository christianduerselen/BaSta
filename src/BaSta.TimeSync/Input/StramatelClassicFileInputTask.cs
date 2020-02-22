using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Timers;

namespace BaSta.TimeSync.Input
{
    internal class StramatelClassicFileInputTask : TimeSyncTaskBase, ITimeSyncInputTask
    {
        internal const string TypeName = "StramatelClassicFile";

        private StramatelClassicTimeParser _parser;
        private string _filePath;
        private IntervalActionHelper _readTimer;
        private string _fileText;
        private int _fileTextIndex;

        protected override void LoadSettings(ITimeSyncSettingsGroup settings)
        {
            //File = [Path]
            //ReadDelay = [ms]
            
            _filePath = settings.GetValue("File", s => s, null);
            int readDelay = settings.GetValue("ReadDelay", int.Parse, 10);

            _readTimer = new IntervalActionHelper(TimeSpan.FromMilliseconds(readDelay), OnTimer);
        }

        private void OnTimer()
        {
            while (true)
            {
                string hex = _fileText.Substring(_fileTextIndex, 2);

                _fileTextIndex += 2;

                if (hex == Environment.NewLine)
                    break;

                int value = Convert.ToInt32(hex, 16);
                _parser.Push((byte)value);
            }
        }

        public override void Start()
        {
            _parser = new StramatelClassicTimeParser($"{Name}_Parser");

            _fileText = File.ReadAllText(_filePath);
            _fileTextIndex = 0;

            _parser.TimeReceived += OnTimeReceived;

            new Thread(Do).Start();

            //_readTimer.IsEnabled = true;
        }

        private DateTime? _lastData;

        private void Do()
        {
            using (StreamReader file = new StreamReader(_filePath))
            {
                while (true)
                {
                    string line = file.ReadLine();

                    if (string.IsNullOrEmpty(line))
                        break;

                    if (line.Contains(':'))
                    {
                        DateTime time = DateTime.ParseExact(line, "HH:mm:ss.fff:", CultureInfo.InvariantCulture);
                        line = file.ReadLine();

                        if (!_lastData.HasValue)
                            _lastData = time;

                        if (time - _lastData.Value < TimeSpan.Zero)
                            _lastData = time;

                        Thread.Sleep((time - _lastData.Value)/10);

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
        }

        public override void Stop()
        {
            _parser.TimeReceived -= OnTimeReceived;

            _readTimer.Finish(false);
        }

        private static readonly TimeSpan ShiftThreshold = TimeSpan.FromMilliseconds(50);
        private DateTime _lastReceivedTimestamp = DateTime.MinValue;
        private TimeSpan _lastReceivedTime;
        private TimeSpan _lastUsedTime;
        private bool _shift;

        private void OnTimeReceived(object? sender, DataEventArgs<TimeSpan> e)
        {
            DateTime currentTimestamp = DateTime.UtcNow;
            TimeSpan timeDifference = currentTimestamp - _lastReceivedTimestamp;

            // If time goes up or has low transfer schedule it is an indicator that milliseconds are not transferred any longer
            if (e.Data > _lastReceivedTime)
                _shift = false;
            
            _lastUsedTime = _lastReceivedTime = e.Data;
            
            // If minute count is larger than 20 or time is transferred fast transferred
            if (!_shift && (_lastReceivedTime.Minutes > 20 || ( _lastReceivedTime.Minutes != 10 && timeDifference < ShiftThreshold)))
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