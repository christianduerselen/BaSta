using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BaSta.Scoreboard
{
    public class UdpReceiver
    {
        private readonly UdpClient _receiveClient;

        public UdpReceiver(int receivePort)
        {
            _receiveClient = new UdpClient(receivePort);
            _receiveClient.EnableBroadcast = true;
            _receiveClient.DontFragment = true;
            new Thread(_receiveMSG) 
            {
                IsBackground = true
            }.Start();
        }

        public delegate void DataReceive(UdpReceiver sender, string Data);
        public event DataReceive DataReceived;
        
        public delegate void BytesReceive(UdpReceiver sender, byte[] Data);
        public event BytesReceive BytesReceived;

        private void _receiveMSG()
        {
            while (true)
            {
                IPEndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);
                byte[] numArray = _receiveClient.Receive(ref remoteEp);
                try
                {
                    string Data = Encoding.UTF8.GetString(numArray);
                    if (Data.StartsWith("*") || Data.StartsWith("!"))
                        DataReceived(this, Data);
                    else
                        BytesReceived(this, numArray);
                }
                catch
                {
                    // Ignored
                }
            }
        }
    }
}