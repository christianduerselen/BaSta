using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace BaSta.Scoreboard
{
  public class StramatelSender : IScoreBoard
  {
    private int _version = 1;
    private byte[] _send_bytes = new byte[54];
    private Sportart _sport = Sportart.Waterpolo;
    private int _refresh_interval = 100;
    private bool _init_scb = true;
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
        if (!value)
          return;
        _refresh_home_team = true;
        _refresh_guest_team = true;
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
        if (_sport != Sportart.Handball && _sport != Sportart.Handball)
          return;
        _refresh_interval = 100;
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

    public StramatelSender()
    {
    }

    public StramatelSender(int SerialPort, int RefreshInterval, int Version)
    {
      _version = Version;
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
      try
      {
        _ser_out.Open();
        _ser_out.DtrEnable = _horn;
        _ser_out.RtsEnable = _shotclock_horn;
      }
      catch
      {
      }
      if (File.Exists("StramatelStartsequenz.txt"))
      {
        string empty = string.Empty;
        StreamReader streamReader = new StreamReader("StramatelStartsequenz.txt");
        for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
        {
          byte[] buffer = new byte[str.Length + 1];
          for (int index = 0; index < str.Length; ++index)
            buffer[index] = (byte) str[index];
          buffer[str.Length] = (byte) 13;
          Thread.Sleep(250);
          try
          {
            _ser_out.Write(buffer, 0, buffer.Length);
          }
          catch
          {
          }
        }
        streamReader.Close();
      }
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
      if (FieldName.StartsWith("Fouls") && Value.Trim() == string.Empty)
        return;
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
            if (_init_scb)
            {
              _init_scb = false;
              _make_telegram_erase_player_name();
              try
              {
                _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
              }
              catch
              {
              }
              Thread.Sleep(250);
            }
            else if (_refresh_home_team)
            {
              _refresh_home_team = false;
              Thread.Sleep(250);
              _make_telegram_team_name();
              try
              {
                _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
              }
              catch
              {
              }
              for (int index = 0; index < 14; ++index)
              {
                Thread.Sleep(250);
                _make_telegram_player_name(HomeGuestTeam.Home, index + 1);
                try
                {
                  _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
                }
                catch
                {
                }
              }
            }
            else if (_refresh_guest_team)
            {
              _refresh_guest_team = false;
              Thread.Sleep(250);
              _make_telegram_team_name();
              try
              {
                _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
              }
              catch
              {
              }
              for (int index = 0; index < 14; ++index)
              {
                Thread.Sleep(250);
                _make_telegram_player_name(HomeGuestTeam.Guest, index + 1);
                try
                {
                  _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
                }
                catch
                {
                }
              }
            }
            else
            {
              if (_sport == Sportart.Volleyball)
                _make_telegram_volley();
              else if (_sport == Sportart.Icehockey)
              {
                _make_telegram_hand_soccer();
              }
              else
              {
                switch (num)
                {
                  case 0:
                    if (_sport == Sportart.Basketball)
                      _make_telegram_basket_with_pers_fouls();
                    if (_sport == Sportart.Handball)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Soccer)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Icehockey)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Hockey)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Waterpolo)
                      _make_telegram_simple_timer();
                    if (_sport == Sportart.Indoorsoccer)
                    {
                      _make_telegram_hand_soccer();
                      break;
                    }
                    break;
                  case 1:
                    _make_telegram_basket_hand_hockey_foot_with_points(MatchData.Mannschaft.Heim);
                    break;
                  case 2:
                    if (_sport == Sportart.Basketball)
                      _make_telegram_basket_with_pers_fouls();
                    if (_sport == Sportart.Handball)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Soccer)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Icehockey)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Hockey)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Waterpolo)
                      _make_telegram_simple_timer();
                    if (_sport == Sportart.Indoorsoccer)
                    {
                      _make_telegram_hand_soccer();
                      break;
                    }
                    break;
                  case 3:
                    _make_telegram_basket_hand_hockey_foot_with_points(MatchData.Mannschaft.Gast);
                    break;
                  case 4:
                    if (_sport == Sportart.Basketball)
                      _make_telegram_basket_with_pers_fouls();
                    if (_sport == Sportart.Handball)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Soccer)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Icehockey)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Hockey)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Waterpolo)
                      _make_telegram_simple_timer();
                    if (_sport == Sportart.Indoorsoccer)
                    {
                      _make_telegram_hand_soccer();
                      break;
                    }
                    break;
                  case 5:
                    if (_sport == Sportart.Basketball)
                      _make_telegram_basket_with_pers_fouls();
                    if (_sport == Sportart.Handball)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Soccer)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Icehockey)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Hockey)
                      _make_telegram_hand_soccer();
                    if (_sport == Sportart.Waterpolo)
                      _make_telegram_simple_timer();
                    if (_sport == Sportart.Indoorsoccer)
                    {
                      _make_telegram_hand_soccer();
                      break;
                    }
                    break;
                  default:
                    if (_sport == Sportart.Basketball)
                    {
                      _make_telegram_basket_with_pers_fouls();
                      break;
                    }
                    break;
                }
                ++num;
                if (num > 5)
                  num = 0;
              }
              try
              {
                _ser_out.Write(_send_bytes, 0, _send_bytes.Length);
              }
              catch
              {
              }
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

    private void _insert_gametime()
    {
      _redlight = false;
      string str = GetFieldValue("GameTime").Trim();
      for (int index = 1; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      if (!str.Contains(".") && !str.Contains(":"))
        str = "0:" + str.Trim().PadLeft(2, '0');
      if (str.Contains(":"))
        str = str.PadLeft(5);
      else if (str.Contains("."))
        str = str.Trim() == "0.0" || _redlight ? " 0.00" : str.Trim().PadLeft(4, '0').PadRight(5);
      _send_bytes[4] = Convert.ToByte(str[0]);
      _send_bytes[5] = Convert.ToByte(str[1]);
      _send_bytes[6] = Convert.ToByte(str[3]);
      _send_bytes[7] = Convert.ToByte(str[4]);
    }

    private void _make_telegram_basket_with_pers_fouls()
    {
      _insert_gametime();
      _send_bytes[1] = (byte) 51;
      _send_bytes[3] = (byte) 48;
      string str1 = GetFieldValue("ScoreHome").PadLeft(3);
      _send_bytes[8] = (byte) str1[0];
      _send_bytes[9] = (byte) str1[1];
      _send_bytes[10] = (byte) str1[2];
      string str2 = GetFieldValue("ScoreGuest").PadLeft(3);
      _send_bytes[11] = (byte) str2[0];
      _send_bytes[12] = (byte) str2[1];
      _send_bytes[13] = (byte) str2[2];
      _send_bytes[14] = (byte) GetFieldValue("Period").PadLeft(1)[0];
      _send_bytes[15] = (byte) GetFieldValue("TeamFoulsHome").PadLeft(1)[0];
      _send_bytes[16] = (byte) GetFieldValue("TeamFoulsGuest").PadLeft(1)[0];
      _send_bytes[17] = (byte) (48 + GetFieldValue("TimeOutsHome").Trim().Length);
      _send_bytes[18] = (byte) (48 + GetFieldValue("TimeOutsGuest").Trim().Length);
      _send_bytes[19] = Form1.UseBasketHorn >= 1 ? (_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      _send_bytes[20] = (byte) GetFieldValue("TimeRunning")[0];
      if (_redlight)
        _send_bytes[20] = (byte) 82;
      string str3 = GetFieldValue("TimeoutTimeHome").PadLeft(3);
      if (str3.Trim() == string.Empty)
        str3 = GetFieldValue("TimeoutTimeGuest").PadLeft(3);
      _send_bytes[21] = (byte) str3[0];
      string empty = string.Empty;
      for (int index = 0; index < 14; ++index)
      {
        string str4 = GetFieldValue("FoulsH" + (index + 1).ToString()).PadLeft(1);
        _send_bytes[22 + index] = (byte) str4[0];
      }
      for (int index = 0; index < 14; ++index)
      {
        string str4 = GetFieldValue("FoulsG" + (index + 1).ToString()).PadLeft(1);
        _send_bytes[34 + index] = (byte) str4[0];
      }
      _send_bytes[46] = (byte) str3[1];
      _send_bytes[47] = (byte) str3[2];
      string str5 = GetFieldValue("ShotTime").PadLeft(2) + " ";
      if (str5.Contains("."))
      {
        if (str5[2] >= '0' && str5[1] < '5')
        {
          _send_bytes[48] = (byte) str5[0];
          _send_bytes[49] = (byte) ((uint) str5[2] + 16U);
        }
        else
        {
          _send_bytes[48] = (byte) 32;
          _send_bytes[49] = (byte) str5[0];
        }
      }
      else
      {
        _send_bytes[48] = (byte) str5[0];
        _send_bytes[49] = (byte) str5[1];
      }
      _send_bytes[50] = Form1.UseBasketHorn >= 1 ? (_shotclock_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      byte num = !(GetFieldValue("SC_EdgeLightOn") != string.Empty) ? (byte) 48 : (byte) GetFieldValue("SC_EdgeLightOn")[0];
      switch (num)
      {
        case 50:
          _send_bytes[50] = (byte) 50;
          break;
        case 82:
          num = (byte) 51;
          break;
      }
      _send_bytes[51] = _shotclock_horn ? num : (byte) 48;
      _send_bytes[52] = (byte) 49;
    }

    private void _make_telegram_basket_hand_hockey_foot_with_points(MatchData.Mannschaft Team)
    {
      _insert_gametime();
      string str1 = "PointsH";
      _send_bytes[1] = (byte) 56;
      if (Team == MatchData.Mannschaft.Gast)
      {
        str1 = "PointsG";
        _send_bytes[1] = (byte) 55;
      }
      string empty = string.Empty;
      for (int index = 0; index < 14; ++index)
      {
        string str2 = GetFieldValue(str1 + (index + 1).ToString()).PadLeft(2);
        if (index > 12)
        {
          _send_bytes[11] = (byte) str2[0];
          _send_bytes[12] = (byte) str2[1];
        }
        else
        {
          _send_bytes[22 + index * 2] = (byte) str2[0];
          _send_bytes[23 + index * 2] = (byte) str2[1];
        }
      }
      _send_bytes[19] = Form1.UseBasketHorn >= 1 ? (_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      _send_bytes[20] = !(GetFieldValue("TimeRunning") != string.Empty) ? (byte) 48 : (byte) GetFieldValue("TimeRunning")[0];
      if (_redlight)
        _send_bytes[20] = (byte) 82;
      string str3 = GetFieldValue("ShotTime").PadLeft(2) + " ";
      if (str3.Contains("."))
      {
        if (str3[2] >= '0' && str3[1] < '5')
        {
          _send_bytes[48] = (byte) str3[0];
          _send_bytes[49] = (byte) ((uint) str3[2] + 16U);
        }
        else
        {
          _send_bytes[48] = (byte) 32;
          _send_bytes[49] = (byte) str3[0];
        }
      }
      else
      {
        _send_bytes[48] = (byte) str3[0];
        _send_bytes[49] = (byte) str3[1];
      }
      _send_bytes[50] = Form1.UseBasketHorn >= 1 ? (_shotclock_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      byte num = !(GetFieldValue("SC_EdgeLightOn") != string.Empty) ? (byte) 48 : (byte) GetFieldValue("SC_EdgeLightOn")[0];
      switch (num)
      {
        case 50:
          _send_bytes[50] = (byte) 50;
          break;
        case 82:
          num = (byte) 51;
          break;
      }
      _send_bytes[51] = _shotclock_horn ? num : (byte) 48;
      _send_bytes[52] = (byte) 49;
      string str4 = GetFieldValue("ScoreHome").PadLeft(3);
      _send_bytes[8] = (byte) str4[0];
      _send_bytes[9] = (byte) str4[1];
      _send_bytes[10] = (byte) str4[2];
      string str5 = GetFieldValue("ScoreGuest").PadLeft(3);
      _send_bytes[11] = (byte) str5[0];
      _send_bytes[12] = (byte) str5[1];
      _send_bytes[13] = (byte) str5[2];
      _send_bytes[14] = (byte) GetFieldValue("Period").PadLeft(1)[0];
      _send_bytes[15] = (byte) GetFieldValue("TeamFoulsHome").PadLeft(1)[0];
      _send_bytes[16] = (byte) GetFieldValue("TeamFoulsGuest").PadLeft(1)[0];
      _send_bytes[17] = (byte) (48 + GetFieldValue("TimeOutsHome").Trim().Length);
      _send_bytes[18] = (byte) (48 + GetFieldValue("TimeOutsGuest").Trim().Length);
      string str6 = GetFieldValue("TimeoutTimeHome").PadLeft(3);
      if (str6.Trim() == string.Empty)
        str6 = GetFieldValue("TimeoutTimeGuest").PadLeft(3);
      _send_bytes[21] = (byte) str6[0];
      _send_bytes[46] = (byte) str6[1];
      _send_bytes[47] = (byte) str6[2];
    }

    private void _make_telegram_hand_soccer()
    {
      _insert_gametime();
      _send_bytes[1] = (byte) 53;
      _send_bytes[8] = (byte) 32;
      string str1 = GetFieldValue("ScoreHome").PadLeft(2);
      _send_bytes[9] = (byte) str1[0];
      _send_bytes[10] = (byte) str1[1];
      string str2 = GetFieldValue("ScoreGuest").PadLeft(2);
      _send_bytes[11] = (byte) 32;
      _send_bytes[12] = (byte) str2[0];
      _send_bytes[13] = (byte) str2[1];
      _send_bytes[14] = (byte) GetFieldValue("Period").PadLeft(1)[0];
      string str3 = GetFieldValue("TimeOutsHome").Trim();
      _send_bytes[17] = (byte) (48 + str3.Length);
      _send_bytes[15] = (byte) (48 + str3.Length);
      string str4 = GetFieldValue("TimeOutsGuest").Trim();
      _send_bytes[18] = (byte) (48 + str4.Length);
      _send_bytes[16] = (byte) (48 + str4.Length);
      _send_bytes[19] = Form1.UseBasketHorn >= 1 ? (_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      _send_bytes[20] = _time_running ? (byte) 49 : (byte) 48;
      _send_bytes[21] = (byte) 32;
      string str5 = GetFieldValue("PenaltyNoH1").PadLeft(2) + GetFieldValue("PenaltyTimeH1").PadLeft(4);
      string str6 = GetFieldValue("PenaltyNoH2").PadLeft(2) + GetFieldValue("PenaltyTimeH2").PadLeft(4);
      string str7 = GetFieldValue("PenaltyNoH3").PadLeft(2) + GetFieldValue("PenaltyTimeH3").PadLeft(4);
      GetFieldValue("PenaltyTimeH1").PadLeft(4);
      string str8 = GetFieldValue("PenaltyTimeH2").PadLeft(4);
      string str9 = GetFieldValue("PenaltyTimeH3").PadLeft(4);
      _send_bytes[25] = (byte) str8[0];
      _send_bytes[26] = (byte) str8[2];
      _send_bytes[27] = (byte) str8[3];
      _send_bytes[28] = (byte) str9[0];
      _send_bytes[29] = (byte) str9[2];
      _send_bytes[30] = (byte) str9[3];
      string str10 = GetFieldValue("PenaltyNoG1").PadLeft(2) + GetFieldValue("PenaltyTimeG1").PadLeft(4);
      str6 = GetFieldValue("PenaltyNoG2").PadLeft(2) + GetFieldValue("PenaltyTimeG2").PadLeft(4);
      str7 = GetFieldValue("PenaltyNoG3").PadLeft(2) + GetFieldValue("PenaltyTimeG3").PadLeft(4);
      GetFieldValue("PenaltyTimeG1").PadLeft(4);
      string str11 = GetFieldValue("PenaltyTimeG2").PadLeft(4);
      string str12 = GetFieldValue("PenaltyTimeG3").PadLeft(4);
      _send_bytes[38] = (byte) str11[0];
      _send_bytes[39] = (byte) str11[2];
      _send_bytes[40] = (byte) str11[3];
      _send_bytes[41] = (byte) str12[0];
      _send_bytes[42] = (byte) str12[2];
      _send_bytes[43] = (byte) str12[3];
      _send_bytes[52] = (byte) 32;
      Console.WriteLine();
      for (int index = 0; index < 54; ++index)
        Console.Write(string.Concat((object) (char) _send_bytes[index]));
    }

    private void _make_telegram_erase_player_name()
    {
      for (int index = 2; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      _send_bytes[1] = (byte) 116;
      _send_bytes[4] = (byte) 116;
      _send_bytes[5] = (byte) 116;
    }

    private void _make_telegram_team_name()
    {
      for (int index = 2; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      _send_bytes[1] = (byte) 98;
      string str1 = GetFieldValue("TeamNameHome").ToUpper().Trim().Replace("Ä", "AE").Replace("Ö", "OE").Replace("Ü", "UE").Replace("ß", "SS");
      if (str1.Length < 8)
        str1 = " " + str1;
      string str2 = str1.PadRight(9).Substring(0, 9);
      string str3 = GetFieldValue("TeamNameGuest").Trim().Replace("Ä", "AE").Replace("Ö", "OE").Replace("Ü", "UE").Replace("ß", "SS");
      if (str3.Length < 8)
        str3 += " ";
      string str4 = str3.PadLeft(9).Substring(0, 9);
      if (_version == 2)
      {
        int num = 0;
        for (int index = 0; index < 9; ++index)
        {
          _send_bytes[num + 6] = (byte) 0;
          _send_bytes[num + 7] = (byte) str2[index];
          _send_bytes[num + 30] = (byte) 0;
          _send_bytes[num + 31] = (byte) str4[index];
          num += 2;
        }
      }
      else
      {
        for (int index = 0; index < 9; ++index)
        {
          _send_bytes[index + 6] = (byte) str2[index];
          _send_bytes[index + 18] = (byte) str4[index];
        }
      }
      _send_bytes[53] = (byte) 13;
    }

    private void _make_telegram_team_name(HomeGuestTeam Team)
    {
      for (int index = 2; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      _send_bytes[1] = (byte) 119;
      string str = GetFieldValue("TeamNameHome").PadRight(20).Substring(0, 20);
      if (Team == HomeGuestTeam.Guest)
      {
        _send_bytes[1] = (byte) 98;
        str = GetFieldValue("TeamNameGuest").PadRight(20).Substring(0, 20);
      }
      for (int index = 0; index < 20; ++index)
        _send_bytes[index + 6] = (byte) str[index];
      _send_bytes[53] = (byte) 13;
    }

    private void _make_telegram_player_name(HomeGuestTeam Team, int Index)
    {
      string str1 = "NameH";
      string str2 = "NoH";
      for (int index = 2; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      _send_bytes[1] = (byte) 119;
      if (Team == HomeGuestTeam.Guest)
      {
        str1 = "NameG";
        str2 = "NoG";
        _send_bytes[1] = (byte) 98;
      }
      string str3 = GetFieldValue(str2 + Index.ToString()).PadLeft(2);
      if (_version == 2)
      {
        _send_bytes[2] = (byte) (48 + Index);
        _send_bytes[22] = (byte) 0;
        _send_bytes[51] = (byte) str3[0];
        _send_bytes[52] = (byte) str3[1];
        if (str3[1] == '0' && str3[0] == ' ')
        {
          _send_bytes[51] = (byte) 48;
          _send_bytes[52] = (byte) 48;
        }
      }
      else
      {
        _send_bytes[4] = (byte) Index.ToString().PadLeft(2, '0')[0];
        _send_bytes[5] = (byte) Index.ToString().PadLeft(2, '0')[1];
        string str4 = GetFieldValue(str1 + Index.ToString()).ToUpper().Trim().Replace("Ä", "AE").Replace("Ö", "OE").Replace("Ü", "UE").Replace("ß", "SS").PadLeft(10).Substring(0, 10);
        for (int index = 0; index < 10; ++index)
          _send_bytes[index + 6] = (byte) str4[index];
        _send_bytes[16] = (byte) str3[0];
        _send_bytes[17] = (byte) str3[1];
      }
      _send_bytes[53] = (byte) 13;
    }

    private string GetStringMinSize(string DBFieldName, int MinNoOfBytes)
    {
      string fieldValue = GetFieldValue(DBFieldName);
      while (fieldValue.Length < MinNoOfBytes)
        fieldValue += " ";
      return fieldValue;
    }

    private void _make_telegram_volley()
    {
      for (int index = 1; index < 53; ++index)
        _send_bytes[index] = (byte) 32;
      _send_bytes[1] = (byte) 54;
      string str1 = GetStringMinSize("ScoreHome", 1).PadLeft(2);
      _send_bytes[9] = (byte) str1[0];
      _send_bytes[10] = (byte) str1[1];
      string str2 = GetStringMinSize("ScoreGuest", 1).PadLeft(2);
      _send_bytes[12] = (byte) str2[0];
      _send_bytes[13] = (byte) str2[1];
      _send_bytes[14] = (byte) GetStringMinSize("Period", 1)[0];
      _send_bytes[15] = (byte) GetStringMinSize("SetsWonHome", 1).PadLeft(1)[0];
      _send_bytes[16] = (byte) GetStringMinSize("SetsWonGuest", 1).PadLeft(1)[0];
      _send_bytes[17] = (byte) GetStringMinSize("TimeOutsHome", 1).PadLeft(1)[0];
      _send_bytes[18] = (byte) GetStringMinSize("TimeOutsGuest", 1).PadLeft(1)[0];
      string str3 = GetStringMinSize("TimeOutsHome", 1).PadLeft(1);
      if (str3[0] > 'a')
        _send_bytes[17] = (byte) (48 + str3.Length);
      string str4 = GetStringMinSize("TimeOutsGuest", 1).PadLeft(1);
      if (str4[0] > 'a')
        _send_bytes[18] = (byte) (48 + str4.Length);
      _send_bytes[19] = Form1.UseBasketHorn >= 1 ? (_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      _send_bytes[20] = _time_running ? (byte) 49 : (byte) 48;
      string stringMinSize1 = GetStringMinSize("Set1Home", 0);
      string str5;
      if (stringMinSize1 != string.Empty)
      {
        string[] strArray = stringMinSize1.Split(':');
        str5 = strArray[0].PadLeft(2) + strArray[1].PadRight(2);
      }
      else
        str5 = "    ";
      while (str5.Length < 4)
        str5 += " ";
      _send_bytes[24] = (byte) str5[0];
      _send_bytes[25] = (byte) str5[1];
      _send_bytes[26] = (byte) str5[2];
      _send_bytes[27] = (byte) str5[3];
      string stringMinSize2 = GetStringMinSize("Set2Home", 0);
      string str6;
      if (stringMinSize2 != string.Empty)
      {
        string[] strArray = stringMinSize2.Split(':');
        str6 = strArray[0].PadLeft(2) + strArray[1].PadRight(2);
      }
      else
        str6 = "    ";
      while (str6.Length < 4)
        str6 += " ";
      _send_bytes[28] = (byte) str6[0];
      _send_bytes[29] = (byte) str6[1];
      _send_bytes[30] = (byte) str6[2];
      _send_bytes[31] = (byte) str6[3];
      string stringMinSize3 = GetStringMinSize("Set3Home", 0);
      string str7;
      if (stringMinSize3 != string.Empty)
      {
        string[] strArray = stringMinSize3.Split(':');
        str7 = strArray[0].PadLeft(2) + strArray[1].PadRight(2);
      }
      else
        str7 = "    ";
      while (str7.Length < 5)
        str7 += " ";
      _send_bytes[32] = (byte) str7[0];
      _send_bytes[33] = (byte) str7[1];
      _send_bytes[34] = (byte) str7[3];
      _send_bytes[35] = (byte) str7[4];
      while (str7.Length < 5)
        str7 += " ";
      _send_bytes[36] = (byte) str7[0];
      _send_bytes[37] = (byte) str7[1];
      _send_bytes[38] = (byte) str7[3];
      _send_bytes[39] = (byte) str7[4];
      _send_bytes[50] = GetStringMinSize("ServiceHome", 0).Trim() == string.Empty ? (byte) 51 : (byte) 49;
    }

    private void _make_telegram_simple_timer()
    {
      _insert_gametime();
      _send_bytes[1] = (byte) 154;
      _send_bytes[19] = Form1.UseBasketHorn >= 1 ? (_horn ? (byte) 49 : (byte) 48) : (byte) 48;
      _send_bytes[21] = _time_running ? (byte) 49 : (byte) 48;
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
      ErasePlayerNames = 116, // 0x00000074
      Home_Team = 119, // 0x00000077
      ClockSet = 153, // 0x00000099
      SimpleTimer = 154, // 0x0000009A
      Training = 156, // 0x0000009C
    }
  }
}
