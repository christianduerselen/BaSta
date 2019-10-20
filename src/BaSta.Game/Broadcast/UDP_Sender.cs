using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BaSta.Game.Broadcast
{
    public class UDP_Sender
    {
        private int _send_port = 8000;
        private string _ip = "127.0.0.1";
        private IPEndPoint _endpoint = (IPEndPoint)null;
        private UdpClient _send_client = (UdpClient)null;
        private bool _is_connected = false;

        public bool IsConnected
        {
            get
            {
                return _is_connected;
            }
        }

        public UDP_Sender()
        {
            _connect();
        }

        public UDP_Sender(string IP)
        {
            _ip = IP;
            _connect();
        }

        public UDP_Sender(string IP, int SendPort)
        {
            _ip = IP;
            _send_port = SendPort;
            _connect();
        }

        private void _connect()
        {
            try
            {
                _endpoint = new IPEndPoint(IPAddress.Parse(_ip), _send_port);
                _send_client = new UdpClient();
                _send_client.EnableBroadcast = false;
                _send_client.DontFragment = false;
                _send_client.Connect(_endpoint);
                _is_connected = true;
            }
            catch (Exception)
            {
                _is_connected = false;
            }
        }

        public void SendMSG(string Text)
        {
            try
            {
                do
                {
                    if (Text != "")
                    {
                        try
                        {
                            byte[] bytes = Encoding.UTF8.GetBytes(Text);
                            _send_client.Send(bytes, bytes.Length);
                            Text = "";
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                while (Text != "");
            }
            catch
            {
            }
        }
    }
}