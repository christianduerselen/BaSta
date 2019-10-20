using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace BaSta.Scoreboard
{
  public class RG96_ShotclockSender : IScoreBoard
  {
    private string _info = string.Empty;
    private Sportart _sport = Sportart.Waterpolo;
    private int _refresh_interval = 100;
    private System.IO.Ports.SerialPort _ser_out = new System.IO.Ports.SerialPort();
    private short _serial_port = 1;
    private int _baudrate = 2400;
    private Dictionary<string, string> _fields = new Dictionary<string, string>();
    private Thread _output_thread;
    private bool _time_running;
    private bool _horn;
    private bool _shotclock_horn;
    private bool _redlight;
    private bool _sending_active;

    public bool RefreshTeams
    {
      set
      {
      }
    }

    public string Info
    {
      set
      {
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

    public bool TimeRunning
    {
      get
      {
        return _time_running;
      }
      set
      {
        _time_running = value;
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
        _ser_out.DtrEnable = _horn;
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
        _ser_out.RtsEnable = _shotclock_horn;
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

    public RG96_ShotclockSender()
    {
    }

    public RG96_ShotclockSender(int SerialPort, int RefreshInterval)
    {
      _refresh_interval = RefreshInterval;
      _serial_port = Convert.ToInt16(SerialPort);
      _ser_out = new System.IO.Ports.SerialPort("COM" + _serial_port.ToString(), 2400, Parity.None, 8, StopBits.One);
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

    public void Dispose()
    {
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

    public string SendString
    {
      set
      {
      }
    }

    public void Refresh()
    {
    }

    public void Clear()
    {
    }

    private string GetFieldValue(string FieldName)
    {
      if (_fields.ContainsKey(FieldName))
        return _fields[FieldName].ToString();
      return string.Empty;
    }

    private void _output()
    {
      ASCIIEncoding asciiEncoding = new ASCIIEncoding();
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
      while (true)
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (_sending_active)
        {
          try
          {
            string str1 = GetFieldValue("GameTime").Trim();
            string str2 = GetFieldValue("ShotTime").Trim();
            if (!str1.Contains(".") && !str1.Contains(":"))
              str1 = "0:" + str1.Trim().PadLeft(2, '0');
            stringBuilder.Append('\x0002');
            if (str2 == "0" && str1 != "10:00" && str2 != string.Empty || str1 == "0.0")
            {
              stringBuilder.Append('\x0012');
              stringBuilder.Append('ò');
              stringBuilder.Append('\x000F');
            }
            else if (str1 != "10:00")
            {
              stringBuilder.Append(' ');
              stringBuilder.Append('\x000F');
            }
            else
              stringBuilder.Append('\x000F');
            if (str1.Contains("."))
            {
              if (str1.Length < 4)
              {
                stringBuilder.Append(' ');
                stringBuilder.Append((char) ((int) str1[1] & 15 | 192));
                stringBuilder.Append((char) ((int) str1[3] & 15 | 160));
                stringBuilder.Append(' ');
              }
              else
              {
                stringBuilder.Append((char) ((int) str1[0] & 15 | 160));
                stringBuilder.Append((char) ((int) str1[1] & 15 | 192));
                stringBuilder.Append((char) ((int) str1[3] & 15 | 160));
                stringBuilder.Append(' ');
              }
            }
            else if (str1.Length < 5)
            {
              stringBuilder.Append(' ');
              try
              {
                stringBuilder.Append((char) ((int) str1[1] & 15 | 192));
                stringBuilder.Append((char) ((int) str1[3] & 15 | 160));
                stringBuilder.Append((char) ((int) str1[4] & 15 | 176));
              }
              catch
              {
                stringBuilder.Append(32);
              }
            }
            else
            {
              stringBuilder.Append((char) ((int) str1[0] & 15 | 160));
              stringBuilder.Append((char) ((int) str1[1] & 15 | 192));
              stringBuilder.Append((char) ((int) str1[3] & 15 | 160));
              stringBuilder.Append((char) ((int) str1[4] & 15 | 176));
            }
            stringBuilder.Append('\x0012');
            if (str2 != string.Empty)
              stringBuilder.Append(64);
            else
              stringBuilder.Append(str2.PadLeft(2));
            stringBuilder.Append('\x0003');
            byte[] bytes = asciiEncoding.GetBytes(stringBuilder.ToString());
            _ser_out.Write(bytes, 0, bytes.Length);
          }
          catch
          {
          }
        }
        Thread.Sleep(_refresh_interval);
      }
    }
  }
}
