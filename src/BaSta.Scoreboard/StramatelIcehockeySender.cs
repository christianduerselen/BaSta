using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace BaSta.Scoreboard
{
  public class StramatelIcehockeySender : IScoreBoard
  {
    private byte[] _send_bytes = new byte[54];
    private Sportart _sport = Sportart.Waterpolo;
    private int _refresh_interval = 100;
    private short _serial_port = 1;
    private int _baudrate = 19200;
    private byte _brightness = byte.MaxValue;
    private byte _brightness_shotclock = byte.MaxValue;
    private byte _hornlevel = byte.MaxValue;
    private byte _hornlevel_shotclock = byte.MaxValue;
    private Dictionary<string, string> _fields = new Dictionary<string, string>();
    private const byte _start_byte = 248;
    private const byte _end_byte = 13;
    private Thread _output_thread;
    private System.IO.Ports.SerialPort _ser_out;
    private bool _refresh_home_team;
    private bool _refresh_guest_team;
    private bool _shotclock_horn;
    private bool _horn;
    private bool _time_running;
    private bool _redlight;
    private bool _sending_active;

    public bool RefreshTeams
    {
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
        return _refresh_home_team;
      }
      set
      {
        _refresh_home_team = value;
      }
    }

    public bool DoRefreshGuestTeam
    {
      get
      {
        return _refresh_guest_team;
      }
      set
      {
        _refresh_guest_team = value;
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
        return _brightness;
      }
      set
      {
        _brightness = value;
      }
    }

    public byte BrightnessShotclock
    {
      get
      {
        return _brightness_shotclock;
      }
      set
      {
        _brightness_shotclock = value;
      }
    }

    public byte Hornlevel
    {
      get
      {
        return _hornlevel;
      }
      set
      {
        _hornlevel = value;
      }
    }

    public byte HornlevelShotclock
    {
      get
      {
        return _hornlevel_shotclock;
      }
      set
      {
        _hornlevel_shotclock = value;
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

    public void Dispose()
    {
      if (_output_thread != null)
      {
        if (_output_thread.IsAlive)
          _output_thread.Abort();
      }
      try
      {
        if (_ser_out == null)
          return;
        _ser_out.Dispose();
      }
      catch
      {
      }
    }

    public void Clear()
    {
    }

    public void Refresh()
    {
    }

    public StramatelIcehockeySender()
    {
    }

    public StramatelIcehockeySender(int SerialPort, int RefreshInterval)
    {
      _refresh_interval = RefreshInterval;
      for (int index = 0; index < _send_bytes.Length; ++index)
        _send_bytes[index] = (byte) 0;
      _send_bytes[0] = (byte) 248;
      _send_bytes[2] = (byte) 32;
      _send_bytes[3] = (byte) 32;
      _send_bytes[53] = (byte) 13;
      _serial_port = Convert.ToInt16(SerialPort);
      _ser_out = new System.IO.Ports.SerialPort("COM" + _serial_port.ToString(), 19200, Parity.None, 8, StopBits.One);
      _ser_out.DtrEnable = false;
      _ser_out.RtsEnable = true;
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

    private void _output()
    {
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
      int num = 0;
      while (true)
      {
        if (_sending_active)
        {
          try
          {
            switch (num)
            {
              case 0:
                _make_telegram_icehockey1();
                break;
              case 1:
                _make_telegram_icehockey2();
                break;
              default:
                _make_telegram_icehockey1();
                break;
            }
            ++num;
            if (num > 1)
              num = 0;
            try
            {
              _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
            }
            catch
            {
            }
          }
          catch
          {
            try
            {
              _ser_out.Close();
            }
            catch
            {
            }
          }
        }
        Thread.Sleep(_refresh_interval);
      }
    }

    private string GetFieldValue(string FieldName)
    {
      if (_fields.ContainsKey(FieldName))
        return _fields[FieldName].ToString();
      return string.Empty;
    }

    private void _insert_gametime_points_timeouts()
    {
      string str1 = GetFieldValue("GameTime").Trim();
      for (int index = 1; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      if (!str1.Contains(".") && !str1.Contains(":"))
        str1 = "0:" + str1.Trim().PadLeft(2, '0');
      if (str1.Contains(":"))
        str1 = str1.PadLeft(5);
      else if (str1.Contains("."))
        str1 = str1.Trim() == "0.0" || _redlight ? " 0.00" : str1.Trim().PadLeft(4, '0').PadRight(5);
      _send_bytes[4] = Convert.ToByte(str1[0]);
      _send_bytes[5] = Convert.ToByte(str1[1]);
      _send_bytes[6] = Convert.ToByte(str1[3]);
      _send_bytes[7] = Convert.ToByte(str1[4]);
      string str2 = GetFieldValue("ScoreHome").PadLeft(2);
      _send_bytes[8] = (byte) str2[0];
      _send_bytes[9] = (byte) str2[1];
      string str3 = GetFieldValue("ScoreGuest").PadLeft(2);
      _send_bytes[10] = (byte) str3[0];
      _send_bytes[11] = (byte) str3[1];
      _send_bytes[12] = (byte) GetFieldValue("Period").PadLeft(1)[0];
      _send_bytes[13] = (byte) (48U + (uint) (byte) GetFieldValue("TimeOutsHome").Trim().Length);
      _send_bytes[14] = (byte) (48U + (uint) (byte) GetFieldValue("TimeOutsGuest").Trim().Length);
      _send_bytes[19] = Form1.UseBasketHorn >= 1 ? (_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      _send_bytes[20] = _time_running ? (byte) 48 : (byte) 49;
    }

    private void _make_telegram_icehockey1()
    {
      _insert_gametime_points_timeouts();
      _send_bytes[1] = (byte) 146;
      string str1 = GetFieldValue("PenaltyNoH1").PadLeft(2);
      _send_bytes[15] = (byte) str1[0];
      _send_bytes[16] = (byte) str1[1];
      string str2 = GetFieldValue("PenaltyNoH2").PadLeft(2);
      _send_bytes[17] = (byte) str2[0];
      _send_bytes[18] = (byte) str2[1];
      string str3 = GetFieldValue("PenaltyNoH3").PadLeft(2);
      _send_bytes[21] = (byte) str3[0];
      _send_bytes[22] = (byte) str3[1];
      string str4 = GetFieldValue("PenaltyNoG1").PadLeft(2);
      _send_bytes[25] = (byte) str4[0];
      _send_bytes[26] = (byte) str4[1];
      string str5 = GetFieldValue("PenaltyNoG2").PadLeft(2);
      _send_bytes[27] = (byte) str5[0];
      _send_bytes[28] = (byte) str5[1];
      string str6 = GetFieldValue("PenaltyNoG3").PadLeft(2);
      _send_bytes[29] = (byte) str6[0];
      _send_bytes[30] = (byte) str6[1];
    }

    private void _make_telegram_icehockey2()
    {
      _insert_gametime_points_timeouts();
      _send_bytes[1] = (byte) 147;
      string str1 = GetFieldValue("PenaltyTimeH1").PadLeft(5);
      if (str1.Trim() != string.Empty)
      {
        _send_bytes[15] = (byte) 49;
        _send_bytes[16] = (byte) str1[1];
        _send_bytes[17] = (byte) str1[3];
        _send_bytes[18] = (byte) str1[4];
      }
      string str2 = GetFieldValue("PenaltyTimeH2").PadLeft(5);
      if (str2.Trim() != string.Empty)
      {
        _send_bytes[21] = (byte) 49;
        _send_bytes[22] = (byte) str2[1];
        _send_bytes[23] = (byte) str2[3];
        _send_bytes[24] = (byte) str2[4];
      }
      string str3 = GetFieldValue("PenaltyTimeH3").PadLeft(5);
      if (str3.Trim() != string.Empty)
      {
        _send_bytes[25] = (byte) 49;
        _send_bytes[26] = (byte) str3[1];
        _send_bytes[27] = (byte) str3[3];
        _send_bytes[28] = (byte) str3[4];
      }
      string str4 = GetFieldValue("PenaltyTimeG1").PadLeft(5);
      if (str4.Trim() != string.Empty)
      {
        _send_bytes[33] = (byte) 49;
        _send_bytes[34] = (byte) str4[1];
        _send_bytes[35] = (byte) str4[3];
        _send_bytes[36] = (byte) str4[4];
      }
      string str5 = GetFieldValue("PenaltyTimeG2").PadLeft(5);
      if (str5.Trim() != string.Empty)
      {
        _send_bytes[37] = (byte) 49;
        _send_bytes[38] = (byte) str5[1];
        _send_bytes[39] = (byte) str5[3];
        _send_bytes[40] = (byte) str5[4];
      }
      string str6 = GetFieldValue("PenaltyTimeG3").PadLeft(5);
      if (!(str6.Trim() != string.Empty))
        return;
      _send_bytes[41] = (byte) 49;
      _send_bytes[42] = (byte) str6[1];
      _send_bytes[43] = (byte) str6[3];
      _send_bytes[44] = (byte) str6[4];
    }

    private enum SportCode
    {
      Basket_with_individual_Fouls = 51, // 0x00000033
      Handball_Soccer = 53, // 0x00000035
      Volleyball = 54, // 0x00000036
      Basket_with_individual_GuestPoints = 55, // 0x00000037
      Basket_with_individual_HomePoints = 56, // 0x00000038
      Tennis = 57, // 0x00000039
      TableTennis = 58, // 0x0000003A
      Guest_Team = 98, // 0x00000062
      LED_Test = 102, // 0x00000066
      Badminton = 108, // 0x0000006C
      Home_Team = 119, // 0x00000077
      Icehockey1 = 146, // 0x00000092
      Icehockey2 = 147, // 0x00000093
      ClockSet = 153, // 0x00000099
      SimpleTimer = 154, // 0x0000009A
      Training = 156, // 0x0000009C
    }
  }
}
