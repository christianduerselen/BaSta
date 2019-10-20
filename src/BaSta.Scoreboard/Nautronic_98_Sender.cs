using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace BaSta.Scoreboard
{
  public class Nautronic_98_Sender : IScoreBoard
  {
    private Sportart _sport = Sportart.Waterpolo;
    private int _refresh_interval = 100;
    private System.IO.Ports.SerialPort _ser_out = new System.IO.Ports.SerialPort();
    private short _serial_port = 1;
    private int _baudrate = 57600;
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

    public Nautronic_98_Sender()
    {
    }

    public Nautronic_98_Sender(int SerialPort, int RefreshInterval)
    {
      _refresh_interval = RefreshInterval;
      _serial_port = Convert.ToInt16(SerialPort);
      _ser_out = new System.IO.Ports.SerialPort("COM" + _serial_port.ToString(), 57600, Parity.Mark, 8, StopBits.One);
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
      if (_output_thread != null && _output_thread.IsAlive)
        _output_thread.Abort();
      if (_ser_out == null || !_ser_out.IsOpen)
        return;
      _ser_out.Close();
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
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      while (true)
      {
        if (_sending_active)
        {
          if (!_ser_out.IsOpen)
          {
            try
            {
              _ser_out.Open();
            }
            catch
            {
            }
          }
          else
          {
            byte[] _out = new byte[18];
            _out[0] = (byte) 11;
            _out[1] = (byte) 0;
            string str1 = GetFieldValue("ScoreHome").PadLeft(3);
            if (str1[1] == ' ')
            {
              _out[2] = (byte) 15;
            }
            else
            {
              _out[2] = (byte) ((uint) str1[1] & 15U);
              if (str1[0] != ' ')
                _out[2] = (byte) ((uint) _out[2] | 16U);
            }
            _out[3] = (byte) ((uint) str1[2] & 15U);
            string str2 = GetFieldValue("Period").PadLeft(1);
            _out[4] = (byte) ((uint) str2[0] & 15U);
            string str3 = GetFieldValue("ScoreGuest").PadLeft(3);
            if (str3[1] == ' ')
            {
              _out[5] = (byte) 15;
            }
            else
            {
              _out[5] = (byte) ((uint) str3[1] & 15U);
              if (str3[0] != ' ')
                _out[5] = (byte) ((uint) _out[5] | 16U);
            }
            _out[6] = (byte) ((uint) str3[2] & 15U);
            _out[7] = (byte) 15;
            _out[8] = (byte) 15;
            _out[9] = (byte) 15;
            _out[10] = (byte) 15;
            _out[11] = (byte) 15;
            _out[12] = (byte) 15;
            _out[13] = (byte) 15;
            string fieldValue = GetFieldValue("GameTime");
            string str4 = !fieldValue.Contains(":") ? fieldValue.PadRight(5) : fieldValue.PadLeft(5);
            _out[14] = str4[0] != ' ' ? (byte) ((uint) str4[0] & 15U) : (byte) 15;
            _out[15] = (byte) ((uint) str4[1] & 15U);
            _out[16] = (byte) ((uint) str4[3] & 15U);
            _out[17] = str4[4] != ' ' ? (byte) ((uint) str4[4] & 15U) : (byte) 15;
            _send_command(_out);
          }
        }
        Thread.Sleep(_refresh_interval);
      }
    }

    private void _send_command(byte[] _out)
    {
      byte[] buffer = new byte[_out.Length + 1];
      for (int index = 0; index < _out.Length; ++index)
        buffer[index] = _out[index];
      int length = _out.Length;
      buffer[length] = (byte) ((uint) buffer[0] * (uint) buffer[1]);
      for (int index = 2; index < buffer.Length - 1; ++index)
        buffer[length] = (byte) ((uint) buffer[length] * (uint) buffer[index]);
      buffer[length] = (byte) ((uint) buffer[length] | 128U);
      _ser_out.Parity = Parity.Mark;
      _ser_out.Write(buffer, 0, 1);
      _ser_out.Parity = Parity.Space;
      _ser_out.Write(buffer, 1, buffer.Length - 1);
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

    private enum Adresses
    {
      AllUnits = 0,
      GroupScoreboards = 10, // 0x0000000A
      FirstScoreboard = 11, // 0x0000000B
      Group24Sec = 40, // 0x00000028
      First24Sec = 41, // 0x00000029
    }

    private enum Subadresses
    {
      HomeScore10s_100 = 0,
      HomeScore1s = 1,
      Periode = 2,
      GuestScore10s_100 = 3,
      GuestScore1s = 4,
      HomeTimeoutMinutes = 5,
      HomeTimeoutSec10 = 6,
      HomeTimeoutSec1 = 7,
      GuestTimeoutMinutes = 8,
      GuestTimeoutSec10 = 9,
      GuestTimeoutSec1 = 10, // 0x0000000A
      GameTimeMinutes10 = 12, // 0x0000000C
      GameTimeMinutes1 = 13, // 0x0000000D
      GameTimeSec10 = 14, // 0x0000000E
      GameTimeSec1 = 15, // 0x0000000F
    }
  }
}
