using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace BaSta.Game.Broadcast
{
    public class UDP_Server
    {
        private int _search_pointer_min = 0;
        private int _search_pointer_max = (int)byte.MaxValue;
        private string _search_base = "172.20.31";
        private string _client_file_name = "ClientIPs.ini";
        private ArrayList _client_ips = new ArrayList();
        private ArrayList _connected_clients = new ArrayList();
        private Thread _search_clients_thread = (Thread)null;
        private Thread _send_information_thread = (Thread)null;
        private UDP_Receiver _receiver = (UDP_Receiver)null;
        private bool _do_send = false;
        private string _sport = string.Empty;
        private Form _source_form = (Form)null;
        private int _score_home = 0;
        private int _score_guest = 0;
        private int _period = 0;
        private const string _ask_connection = "?";
        private const string _confirm_connection = "!";
        private const string _close_connection = "#";
        private const string _start_of_information = "*";
        private const string _information_separator = "|";
        private const string _data_separator = "=";

        public event ClientConnect ClientConnected;

        public event ClientDisconnect ClientDisconnected;

        public string SearchBase
        {
            set
            {
                _search_base = value;
            }
        }

        public bool DoSend
        {
            set
            {
                _do_send = value;
            }
        }

        public string Sport
        {
            get
            {
                return _sport;
            }
            set
            {
                _sport = value;
            }
        }

        public int ScoreHome
        {
            set
            {
                _score_home = value;
            }
        }

        public int ScoreGuest
        {
            set
            {
                _score_guest = value;
            }
        }

        public int Period
        {
            set
            {
                _period = value;
            }
        }

        public UDP_Server(string Sport, Form SourceForm)
        {
            _source_form = SourceForm;
            _sport = Sport;
            _receiver = new UDP_Receiver(8001);
            _receiver.DataReceived += new DataReceive(_receiver_DataReceived);
            _search_clients_thread = new Thread(new ThreadStart(_search_clients));
            _search_clients_thread.IsBackground = true;
            _search_clients_thread.Start();
            _send_information_thread = new Thread(new ThreadStart(_send_information));
            _send_information_thread.IsBackground = true;
            _send_information_thread.Start();
        }

        public void Dispose()
        {
            _search_clients_thread.Abort();
            _send_information_thread.Abort();
            _receiver = (UDP_Receiver)null;
        }

        public void DoSaveClients()
        {
            if (_client_ips.Count <= 0)
                return;
            _save_clients();
        }

        public void DoReadClients()
        {
            _read_clients();
        }

        private void _receiver_DataReceived(UDP_Receiver sender, string Data)
        {
            if (Data.StartsWith("!") && !_client_ips.Contains((object)Data.Substring(1)))
            {
                _client_ips.Add((object)Data.Substring(1));
                UDP_Sender udpSender = new UDP_Sender(Data.Substring(1));
                if (udpSender.IsConnected)
                    _connected_clients.Add((object)udpSender);
                ClientConnected(this, Data.Substring(1));
            }
            if (!Data.StartsWith("#"))
                return;
            _client_ips.Remove((object)Data.Substring(1));
            _connected_clients.Clear();
            for (int index = 0; index < _client_ips.Count; ++index)
            {
                UDP_Sender udpSender = new UDP_Sender(_client_ips[index].ToString());
                if (udpSender.IsConnected)
                    _connected_clients.Add((object)udpSender);
            }
            ClientDisconnected(this, Data.Substring(1));
        }

        private void _search_clients()
        {
            int searchPointerMax = _search_pointer_max;
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            while (true)
            {
                string IP = _search_base + "." + searchPointerMax.ToString();
                --searchPointerMax;
                if (searchPointerMax < _search_pointer_min)
                    searchPointerMax = _search_pointer_max;
                if (!_client_ips.Contains((object)IP))
                {
                    UDP_Sender udpSender = new UDP_Sender(IP, 8000);
                    if (udpSender.IsConnected)
                        udpSender.SendMSG("?" + (object)hostEntry.AddressList[0]);
                }
                Thread.Sleep(10);
            }
        }

        private void _read_clients()
        {
            if (!System.IO.File.Exists(_client_file_name))
                return;
            StreamReader streamReader = new StreamReader(_client_file_name);
            for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
            {
                if (!_client_ips.Contains((object)str))
                    _client_ips.Add((object)str);
            }
            streamReader.Close();
            _connected_clients.Clear();
            for (int index = 0; index < _client_ips.Count; ++index)
            {
                UDP_Sender udpSender = new UDP_Sender(_client_ips[index].ToString());
                if (udpSender.IsConnected)
                {
                    _connected_clients.Add((object)udpSender);
                    ClientConnected(this, _client_ips[index].ToString());
                }
            }
        }

        private void _save_clients()
        {
            StreamWriter streamWriter = new StreamWriter(_client_file_name);
            for (int index = 0; index < _client_ips.Count; ++index)
                streamWriter.WriteLine(_client_ips[index].ToString());
            streamWriter.Close();
        }

        private void _send_information()
        {
            bool flag = true;
            StringBuilder stringBuilder1 = new StringBuilder();
            while (true)
            {
                try
                {
                    StringBuilder stringBuilder2 = new StringBuilder("*" + _sport);
                    foreach (Control control in (ArrangedElementCollection)_source_form.Controls)
                    {
                        if (control.GetType() == typeof(Label) && !control.Name.StartsWith("label"))
                        {
                            stringBuilder2.Append("|");
                            stringBuilder2.Append(control.Name);
                            stringBuilder2.Append("=");
                            stringBuilder2.Append(control.Text.Trim());
                            stringBuilder2.Append("=1");
                        }
                    }
                    flag = !flag;
                    for (int index = 0; index < _connected_clients.Count; ++index)
                        ((UDP_Sender)_connected_clients[index]).SendMSG(stringBuilder2.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(20);
            }
        }
    }
}