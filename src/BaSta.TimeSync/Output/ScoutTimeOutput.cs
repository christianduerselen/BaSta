using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BaSta.TimeSync.Output
{
    internal class ScoutTimeOutput : TimeSyncTaskBase, ITimeSyncOutputTask
    {
        internal const string TypeName = "ActionScout";

        private Socket _socket;
        private IPEndPoint _localEndpoint;
        private IPEndPoint _destinationEndpoint;
        
        protected override void LoadSettings(ITimeSyncSettingsGroup settings)
        {
            int sourcePort = settings.GetValue("SourcePort", int.Parse, 2101);
            _localEndpoint = new IPEndPoint(IPAddress.Any, sourcePort);
            
            int destinationPort = settings.GetValue("DestinationPort", int.Parse, 2111);
            IPAddress destinationAddress = settings.GetValue("DestinationAddress", IPAddress.Parse, IPAddress.Loopback);
            _destinationEndpoint = new IPEndPoint(destinationAddress, destinationPort);
        }

        protected override void OnStartSync()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.Bind(_localEndpoint);
        }

        private static readonly TimeSpan SyncInterval = TimeSpan.FromMilliseconds(100);
        private DateTime LastSync = DateTime.MinValue;

        protected override void OnProcess(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(1);

                if (!_value.HasValue)
                    continue;

                DateTime now = DateTime.UtcNow;
                if (LastSync + SyncInterval > now)
                    continue;

                LastSync = now;

                string text = $"TMT{_value:hh\\:mm\\:ss}";

                Logger.Debug(text);

                byte[] textBytes = Encoding.ASCII.GetBytes(text);

                byte[] sendBytes = new byte[textBytes.Length + 3];

                sendBytes[0] = 0x02; // STX
                Array.Copy(textBytes, 0, sendBytes, 1, textBytes.Length);
                sendBytes[^2] = 0xCE;
                sendBytes[^1] = 0x03;

                _socket.SendTo(sendBytes, _destinationEndpoint);
            }
        }

        protected override void OnStopSync()
        {
            _socket.Close();
            _socket = null;
        }

        private TimeSpan? _value;

        public void Push(TimeSpan timeSpan)
        {
            _value = timeSpan;
        }
    }
}