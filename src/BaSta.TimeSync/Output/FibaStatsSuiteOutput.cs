using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BaSta.TimeSync.Output
{
    internal class FibaStatsSuiteOutput : TimeSyncTaskBase, ITimeSyncOutputTask
    {
        internal const string TypeName = "FibaStatsSuite";

        private IPEndPoint _localEndpoint;
        private TcpListener _listener;
        private TcpClient _client;

        protected override void LoadSettings(ITimeSyncSettingsGroup settings)
        {
            int port = settings.GetValue("Port", int.Parse, 4000);
            _localEndpoint = new IPEndPoint(IPAddress.Any, port);
        }

        protected override void OnStartSync()
        {
            _listener = new TcpListener(_localEndpoint);
            _listener.Start();
        }

        private TimeSpan? _value;
        private static readonly TimeSpan SyncInterval = TimeSpan.FromMilliseconds(100);
        private DateTime LastSync = DateTime.MinValue;

        protected override void OnProcess(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(1);

                // If currently no client is connected and listener has a pending request accept client
                if (_client == null && _listener.Pending())
                    _client = _listener.AcceptTcpClient();

                if (_client == null)
                    continue;

                if (!_value.HasValue)
                    continue;

                DateTime now = DateTime.UtcNow;
                if (LastSync + SyncInterval > now)
                    continue;

                LastSync = now;

                string output = $"<Template Name =\"SEMAPHORE\"><PARAM Name=\"MINUTES\" Value=\"{_value:mm}\"/><PARAM Name=\"SECONDS\" Value=\"{_value:ss}\"/></Template>";

                _client.GetStream().Write(Encoding.ASCII.GetBytes(output));
            }
        }

        protected override void OnStopSync()
        {
            _client?.Close();
            _client = null;

            _listener?.Stop();
            _listener = null;
        }

        public void Push(TimeSpan time)
        {
            _value = time;
        }
    }
}