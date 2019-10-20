using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace BaSta.Scoreboard
{
  internal class MTVisualSender : IScoreBoard
  {
    private byte[] commBuf0 = new byte[16];
    private byte[] commBuf1 = new byte[16];
    private bool _isBoardOn = true;
    private bool _thread_active = true;
    private Sportart _sport = Sportart.Waterpolo;
    private int _refresh_interval = 100;
    private System.IO.Ports.SerialPort _ser_out = new System.IO.Ports.SerialPort();
    private short _serial_port = 1;
    private int _baudrate = 19200;
    private Dictionary<string, string> _fields = new Dictionary<string, string>();
    private bool _sending_active;
    private Thread _output_thread;
    private bool _horn;
    private bool _shotclock_horn;
    private bool _redlight;

    public bool RefreshTeams
    {
      set
      {
      }
    }

    public MTVisualSender()
    {
    }

    public MTVisualSender(int SerialPort, int RefreshInterval)
    {
      commBuf0[0] = (byte) 128;
      commBuf1[0] = (byte) 240;
      for (int index = 1; index < commBuf0.Length; ++index)
      {
        commBuf0[index] = (byte) 10;
        commBuf1[index] = (byte) 10;
      }
      commBuf0[15] = (byte) 1;
      _refresh_interval = RefreshInterval;
      _serial_port = Convert.ToInt16(SerialPort);
      _ser_out = new System.IO.Ports.SerialPort("COM" + _serial_port.ToString(), 19200, Parity.None, 8, StopBits.One);
      _output_thread = new Thread(new ThreadStart(_output));
      _output_thread.Start();
    }

    public bool StartSending
    {
      set
      {
        _sending_active = value;
      }
    }

    public void SetFieldValue(string FieldName, string Value)
    {
      if (_fields.ContainsKey(FieldName))
        _fields[FieldName] = Value;
      else
        _fields.Add(FieldName, Value);
    }

    private string GetFieldValue(string FieldName)
    {
      if (_fields.ContainsKey(FieldName))
        return _fields[FieldName].ToString();
      return string.Empty;
    }

    public void Dispose()
    {
      try
      {
        _isBoardOn = false;
        Thread.Sleep(150);
        _thread_active = false;
        if (_output_thread != null)
        {
          if (_output_thread.IsAlive)
            _output_thread.Abort();
        }
        try
        {
          if (_ser_out == null || !_ser_out.IsOpen)
            return;
          _ser_out.Close();
        }
        catch
        {
        }
      }
      catch
      {
      }
    }

    public void Refresh()
    {
    }

    public void Clear()
    {
      _isBoardOn = false;
    }

    public bool DoRefreshHomeTeam
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public bool DoRefreshGuestTeam
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    private void _output()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (!_ser_out.IsOpen)
      {
        try
        {
          _ser_out.Open();
          _ser_out.DtrEnable = _horn;
          _ser_out.RtsEnable = _shotclock_horn;
        }
        catch
        {
        }
      }
      while (_thread_active)
      {
        try
        {
          if (_ser_out.IsOpen)
          {
            if (_sending_active)
            {
              string fieldValue = GetFieldValue("ShotTime");
              string str1 = GetFieldValue("GameTime");
              if (!str1.Contains(".") && !str1.Contains(":"))
                str1 = "0:" + str1.Trim().PadLeft(2, '0');
              StringBuilder stringBuilder = new StringBuilder();
              if (str1.Contains(":"))
              {
                string str2 = str1.PadLeft(5);
                commBuf0[11] = (byte) ((uint) Convert.ToByte(str2[4]) & 15U);
                commBuf0[12] = (byte) ((uint) Convert.ToByte(str2[3]) & 15U);
                commBuf0[13] = (byte) (((int) Convert.ToByte(str2[1]) & 15) + 32);
                commBuf0[14] = str2[0] == ' ' ? (byte) 10 : (byte) ((uint) Convert.ToByte(str2[0]) & 15U);
              }
              else
              {
                string str2 = str1.PadLeft(4).PadRight(5);
                commBuf0[11] = (byte) 10;
                commBuf0[12] = (byte) ((uint) Convert.ToByte(str2[3]) & 15U);
                commBuf0[13] = (byte) (((int) Convert.ToByte(str2[1]) & 15) + 32);
                commBuf0[14] = str2[0] == ' ' ? (byte) 10 : (byte) ((uint) Convert.ToByte(str2[0]) & 15U);
              }
              _ser_out.Write(commBuf0, 0, commBuf0.Length);
              if (fieldValue.Trim() != string.Empty)
              {
                int int32 = Convert.ToInt32(fieldValue);
                string str2 = fieldValue.PadLeft(2);
                commBuf1[2] = (byte) (int32 % 10);
                commBuf1[3] = int32 <= 9 ? (byte) 10 : (byte) (int32 / 10);
                int num = str2.Trim() == "0" ? 1 : 0;
                if (_redlight)
                  commBuf1[3] = (byte) ((uint) commBuf1[3] | 32U);
              }
              else
              {
                commBuf1[2] = (byte) 10;
                commBuf1[3] = (byte) 10;
              }
              commBuf1[5] = _isBoardOn ? (byte) 1 : (byte) 0;
              commBuf1[6] = (byte) 9;
              commBuf1[7] = _shotclock_horn ? (byte) 2 : (byte) 0;
              _ser_out.Write(commBuf1, 0, commBuf1.Length);
            }
          }
        }
        catch
        {
        }
        Thread.Sleep(_refresh_interval);
      }
    }

    public Sportart Sport
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

    public int RefreshInterval
    {
      get
      {
        return _refresh_interval;
      }
      set
      {
        _refresh_interval = value;
      }
    }

    public short SerialPort
    {
      get
      {
        return _serial_port;
      }
      set
      {
        _serial_port = value;
        if (_ser_out.IsOpen)
          _ser_out.Close();
        _ser_out.PortName = "COM" + _serial_port.ToString();
      }
    }

    public int Baudrate
    {
      get
      {
        return _baudrate;
      }
      set
      {
        _baudrate = value;
        if (_ser_out.IsOpen)
          _ser_out.Close();
        _ser_out.BaudRate = _baudrate;
      }
    }

    public byte Brightness
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    public byte BrightnessShotclock
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    public byte Hornlevel
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    public byte HornlevelShotclock
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    public bool TimeRunning
    {
      get
      {
        return false;
      }
      set
      {
      }
    }

    public bool Horn
    {
      get
      {
        return _horn;
      }
      set
      {
        _horn = value;
        if (!_ser_out.IsOpen)
          return;
        _ser_out.RtsEnable = _horn;
      }
    }

    public bool ShotclockHorn
    {
      get
      {
        return _shotclock_horn;
      }
      set
      {
        _shotclock_horn = value;
        if (!_ser_out.IsOpen)
          return;
        _ser_out.DtrEnable = _shotclock_horn;
      }
    }

    public bool RedLight
    {
      get
      {
        return _redlight;
      }
      set
      {
        _redlight = value;
      }
    }

    public Dictionary<string, string> Fields
    {
      get
      {
        return _fields;
      }
      set
      {
        _fields = value;
      }
    }
  }
}
