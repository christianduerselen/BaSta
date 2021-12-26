using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace BaSta.TimeSync.Input
{
    internal class StramatelClassicSerialInputTask : TimeSyncTaskBase, ITimeSyncInputTask
    {
        internal const string TypeName = "StramatelClassic";

        private StramatelClassicTimeParser _parser;
        private SerialPort _port;

        protected override void LoadSettings(ITimeSyncSettingsGroup settings)
        {
            // PortName=COM1
            // BaudRate=19200
            // # 0 = None | 1 = Odd | 2 = Even | 3 = Mark | 4 = Space
            // Parity=0
            // DataBits=8
            // # 1 = One | 2 = Two | 3 = OnePointFive
            // StopBits=1
            // # 0 = None | 1 = XOnXOff | 2 = RequestToSend | 3 = RequestToSendXOnXOff
            // Handshake=0

            _port = new SerialPort
            {
                PortName = settings.GetValue("PortName", s =>
                {
                    if (!SerialPort.GetPortNames().Contains(s))
                        throw new Exception($"Serial port '{s}' not available on this system!");

                    return s;
                }, null),
                BaudRate = settings.GetValue("BaudRate", int.Parse, 19200),
                Parity = settings.GetValue("Parity", Enum.Parse<Parity>, _port.Parity),
                DataBits = settings.GetValue("DataBits", int.Parse, _port.DataBits),
                StopBits = settings.GetValue("StopBits", Enum.Parse<StopBits>, _port.StopBits),
                Handshake = settings.GetValue("Handshake", Enum.Parse<Handshake>, _port.Handshake)
            };
        }

        protected override void OnStartSync()
        {
            _parser = new StramatelClassicTimeParser($"{Name}_Parser");
            _parser.TimeReceived += OnTimeReceived;

            _port.Open();
        }

        protected override void OnProcess(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(1);

                if (_port.BytesToRead <= 0)
                    continue;

                int inputLength = _port.BytesToRead;
                byte[] input = new byte[inputLength];
                _port.Read(input, 0, inputLength);

                Logger.Debug(string.Join("", input.Select(x => $"{x:X2}")));

                for (int i = 0; i < inputLength; i++)
                    _parser.Push(input[i]);
            }
        }

        protected override void OnStopSync()
        {
            _port.Close();

            _parser.TimeReceived -= OnTimeReceived;
        }

        private static readonly TimeSpan ShiftThreshold = TimeSpan.FromMilliseconds(500);
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