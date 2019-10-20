using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class SerialConsole : Form
    {
        private string CR = string.Empty + (object)'\r';
        private string _received_data = string.Empty;
        private bool _just_opened = true;
        private byte[] _statusbyte = new byte[4];
        private byte[] _received_bytes = new byte[5];
        private int _port;
        private bool _busy;
        private IContainer components;
        private Panel pnlDataReceived;
        private Label lblByte0;
        private Label lblByte1;
        private Label lblByte3;
        private Label lblByte2;
        private SerialPort _ser;
        private Timer timer1;

        public event SerialConsole.StatusChangedDelegate StatusChanged;

        private byte StatusByte(int Index)
        {
            return _statusbyte[Index];
        }

        public bool StatusGameTimeRunning
        {
            get
            {
                return ((int)_statusbyte[0] & 1) > 0;
            }
        }

        public bool StatusHornButtonPressed
        {
            get
            {
                return ((int)_statusbyte[0] & 2) > 0;
            }
        }

        public bool StatusShotClockRunning
        {
            get
            {
                return ((int)_statusbyte[1] & 4) > 0;
            }
        }

        public bool PrecisionTimeStatusGameTimeRunning
        {
            get
            {
                return ((int)_statusbyte[0] & 4) > 0;
            }
        }

        public bool StatusShotClockReset24Pressed
        {
            get
            {
                return ((int)_statusbyte[1] & 1) > 0;
            }
        }

        public bool StatusShotClockReset14Pressed
        {
            get
            {
                return ((int)_statusbyte[1] & 2) > 0;
            }
        }

        public bool Busy
        {
            get
            {
                return _busy;
            }
            set
            {
                _busy = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                if (_ser != null)
                    return _ser.IsOpen;
                return false;
            }
        }

        public SerialConsole(int Port)
        {
            InitializeComponent();
            for (int index = 0; index < _statusbyte.Length; ++index)
                _statusbyte[index] = (byte)0;
            _port = Port;
            _ser.PortName = "COM" + _port.ToString();
            _open_connection();
        }

        private void _open_connection()
        {
            try
            {
                _ser.Open();
                _just_opened = true;
            }
            catch
            {
            }
        }

        private void SerialConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_ser != null)
            {
                try
                {
                    if (_ser.IsOpen)
                        _ser.Close();
                }
                catch
                {
                }
                try
                {
                    _ser.Dispose();
                }
                catch
                {
                }
            }
            _ser = (SerialPort)null;
        }

        private void _ser_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (_ser.IsOpen)
                {
                    while (_ser.IsOpen && _ser.BytesToRead > 0)
                    {
                        byte num = (byte)_ser.ReadByte();
                        if (num != (byte)13)
                        {
                            if (num < (byte)64)
                                _received_bytes[1] = num;
                            else if (num < (byte)80)
                                _received_bytes[2] = num;
                            else if (num < (byte)96)
                            {
                                _received_bytes[3] = num;
                            }
                            else
                            {
                                _received_bytes[4] = num;
                                _received_bytes[4] = num;
                                bool flag = (int)_statusbyte[0] != (int)_received_bytes[1] || (int)_statusbyte[1] != (int)_received_bytes[2] || (int)_statusbyte[2] != (int)_received_bytes[3] || (int)_statusbyte[3] != (int)_received_bytes[4];
                                _received_bytes[0] = (byte)0;
                                _statusbyte[0] = _received_bytes[1];
                                _statusbyte[1] = _received_bytes[2];
                                _statusbyte[2] = _received_bytes[3];
                                _statusbyte[3] = _received_bytes[4];
                                if (flag)
                                    StatusChanged(this, _just_opened);
                                _just_opened = false;
                            }
                        }
                    }
                }
                else
                    _open_connection();
            }
            catch
            {
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (components != null)
                        components.Dispose();
                }
            }
            catch
            {
            }
            try
            {
                base.Dispose(disposing);
            }
            catch
            {
            }
        }

        private void InitializeComponent()
        {
            components = (IContainer)new Container();
            pnlDataReceived = new Panel();
            lblByte0 = new Label();
            lblByte1 = new Label();
            lblByte3 = new Label();
            lblByte2 = new Label();
            _ser = new SerialPort(components);
            timer1 = new Timer(components);
            SuspendLayout();
            pnlDataReceived.BackColor = Color.FromArgb(0, 192, 0);
            pnlDataReceived.BorderStyle = BorderStyle.FixedSingle;
            pnlDataReceived.Location = new Point(12, 12);
            pnlDataReceived.Name = "pnlDataReceived";
            pnlDataReceived.Size = new Size(15, 15);
            pnlDataReceived.TabIndex = 0;
            lblByte0.BorderStyle = BorderStyle.FixedSingle;
            lblByte0.Location = new Point(48, 9);
            lblByte0.Name = "lblByte0";
            lblByte0.Size = new Size(22, 23);
            lblByte0.TabIndex = 1;
            lblByte0.Text = "00";
            lblByte0.TextAlign = ContentAlignment.MiddleCenter;
            lblByte1.BorderStyle = BorderStyle.FixedSingle;
            lblByte1.Location = new Point(76, 9);
            lblByte1.Name = "lblByte1";
            lblByte1.Size = new Size(22, 23);
            lblByte1.TabIndex = 2;
            lblByte1.Text = "00";
            lblByte1.TextAlign = ContentAlignment.MiddleCenter;
            lblByte3.BorderStyle = BorderStyle.FixedSingle;
            lblByte3.Location = new Point(132, 9);
            lblByte3.Name = "lblByte3";
            lblByte3.Size = new Size(22, 23);
            lblByte3.TabIndex = 4;
            lblByte3.Text = "00";
            lblByte3.TextAlign = ContentAlignment.MiddleCenter;
            lblByte2.BorderStyle = BorderStyle.FixedSingle;
            lblByte2.Location = new Point(104, 9);
            lblByte2.Name = "lblByte2";
            lblByte2.Size = new Size(22, 23);
            lblByte2.TabIndex = 3;
            lblByte2.Text = "00";
            lblByte2.TextAlign = ContentAlignment.MiddleCenter;
            _ser.BaudRate = 57600;
            _ser.DataReceived += new SerialDataReceivedEventHandler(_ser_DataReceived);
            timer1.Interval = 1000;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(164, 42);
            ControlBox = false;
            Controls.Add((Control)lblByte3);
            Controls.Add((Control)lblByte2);
            Controls.Add((Control)lblByte1);
            Controls.Add((Control)lblByte0);
            Controls.Add((Control)pnlDataReceived);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            Name = nameof(SerialConsole);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Serial Console";
            TopMost = true;
            FormClosing += new FormClosingEventHandler(SerialConsole_FormClosing);
            ResumeLayout(false);
        }

        public delegate void StatusChangedDelegate(SerialConsole sender, bool Init);
    }
}