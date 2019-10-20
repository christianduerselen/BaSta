using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace BaSta.Scoreboard
{
  internal class ExtHorn : IScoreBoard
  {
    private bool _thread_active = true;
    private Sportart _sport = Sportart.Waterpolo;
    private int _refresh_interval = 100;
    private System.IO.Ports.SerialPort _ser_out = new System.IO.Ports.SerialPort();
    private short _serial_port = 1;
    private int _baudrate = 9600;
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

    public ExtHorn()
    {
    }

    public ExtHorn(int SerialPort, int RefreshInterval)
    {
      _refresh_interval = RefreshInterval;
      _serial_port = Convert.ToInt16(SerialPort);
      _ser_out = new System.IO.Ports.SerialPort("COM" + _serial_port.ToString(), 9600, Parity.None, 8, StopBits.One);
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

    public void Refresh()
    {
    }

    public void Clear()
    {
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
      string empty = string.Empty;
      bool flag = false;
      while (_thread_active)
      {
        try
        {
          if (!_ser_out.IsOpen)
          {
            if (_sending_active)
            {
              try
              {
                _ser_out.Open();
                _ser_out.NewLine = string.Empty + (object) '\r';
                _ser_out.WriteLine("!Timeout=05");
              }
              catch
              {
              }
            }
          }
          if (_ser_out.IsOpen)
          {
            if (_horn != flag)
            {
              flag = _horn;
              if (_ser_out.IsOpen)
              {
                if (flag)
                  _ser_out.WriteLine("!On");
                else
                  _ser_out.WriteLine("!Off");
              }
            }
            else
              Thread.Sleep(_refresh_interval);
          }
          else
            Thread.Sleep(_refresh_interval);
        }
        catch
        {
        }
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
