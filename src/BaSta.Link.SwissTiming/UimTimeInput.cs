using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace BaSta.Link.SwissTiming
{
    public class UimTimeInput : TimeSyncTaskBase, ITimeSyncInputTask
    {
        public const string TypeName = "UIM";

        private IPEndPoint _localEndpoint;
        private Socket _socket;

        protected override void LoadSettings(ITimeSyncSettingsGroup settings)
        {
            int sourcePort = settings.GetValue("Port", int.Parse, 2111);
            _localEndpoint = new IPEndPoint(IPAddress.Any, sourcePort);
        }

        protected override void OnStartSync()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(_localEndpoint);
        }

        private readonly byte[] _receiveBuffer = new byte[ushort.MaxValue];

        private const string ReceivePattern = "TMT[0-9]{2}:[0-9]{2}:[0-9]{2}";

        protected override void OnProcess(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(1);

                if (_socket.Available <= 0)
                    continue;

                int length = _socket.Receive(_receiveBuffer);
                byte[] data = new byte[length-3];
                Array.Copy(_receiveBuffer, 1, data, 0, data.Length);

                string receivedString = Encoding.ASCII.GetString(data);
                
                if (!Regex.IsMatch(receivedString, ReceivePattern))
                    continue;

                string[] numberStrings = Regex.Matches(receivedString, "[0-9]{2}").Select(x => x.Value).ToArray();
                _lastUsedTime = new TimeSpan(int.Parse(numberStrings[0]), int.Parse(numberStrings[1]), int.Parse(numberStrings[2]));

                StateChanged?.Invoke(this, new DataEventArgs<TimeSpan>(_lastUsedTime));
            }
        }

        protected override void OnStopSync()
        {
            _socket.Close();
            _socket = null;
        }

        private TimeSpan _lastUsedTime;

        public TimeSpan Pull()
        {
            return _lastUsedTime;
        }

        public event EventHandler<DataEventArgs<TimeSpan>> StateChanged;
    }
}