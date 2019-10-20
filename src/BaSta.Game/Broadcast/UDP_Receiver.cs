using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BaSta.Game.Broadcast
{
    public class UDP_Receiver
    {
        private UdpClient _receive_client = (UdpClient)null;
        private int _receive_port = 8001;

        public UDP_Receiver()
        {
            _new();
        }

        public UDP_Receiver(int ReceivePort)
        {
            _receive_port = ReceivePort;
            _new();
        }

        private void _new()
        {
            _receive_client = new UdpClient(_receive_port);
            _receive_client.EnableBroadcast = true;
            _receive_client.DontFragment = true;
            new Thread(new ThreadStart(_receiveMSG))
            {
                IsBackground = true
            }.Start();
        }

        public event DataReceive DataReceived;

        private void _receiveMSG()
        {
            while (true)
            {
                try
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                    DataReceived(this, Encoding.UTF8.GetString(_receive_client.Receive(ref remoteEP)));
                }
                catch
                {
                }
            }
        }
    }
}