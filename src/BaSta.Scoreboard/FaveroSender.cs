using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace BaSta.Scoreboard
{
  public class FaveroSender : IScoreBoard
  {
    private Sportart _sport = Sportart.Basketball;
    private int _refresh_interval = 30;
    private short _serial_port = 1;
    private int _baudrate = 19200;
    private byte _brightness = byte.MaxValue;
    private byte _brightness_shotclock = byte.MaxValue;
    private byte _hornlevel = byte.MaxValue;
    private byte _hornlevel_shotclock = byte.MaxValue;
    private Dictionary<string, string> _fields = new Dictionary<string, string>();
    private const int _max_player_send_index = 56;
    private const int _max_player_per_team = 14;
    private bool _time_running;
    private bool _shotclock_horn;
    private bool _redlight;
    private bool _horn;
    private Thread _output_thread;
    private System.IO.Ports.SerialPort _ser_out;
    private bool _sending_active;
    private int _player_send_index;
    private int _team_send_index;

    public bool RefreshTeams
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

    public FaveroSender()
    {
    }

    public FaveroSender(int SerialPort, int RefreshInterval)
    {
      _refresh_interval = 20;
      _serial_port = Convert.ToInt16(SerialPort);
      _ser_out = new System.IO.Ports.SerialPort("COM" + _serial_port.ToString(), 19200, Parity.Odd, 8, StopBits.One);
      try
      {
        _ser_out.Open();
      }
      catch
      {
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

    public void Clear()
    {
    }

    private void _output()
    {
      int num = 0;
      byte[] buffer = (byte[]) null;
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
        try
        {
          if (_ser_out.IsOpen)
          {
            if (_sending_active)
            {
              try
              {
                switch (num)
                {
                  case 0:
                    buffer = _game_clock_possession_timeout_telegramm();
                    break;
                  case 1:
                    buffer = _shot_clock_timeout_telegramm();
                    break;
                  case 2:
                    buffer = _horn_telegramm();
                    break;
                  case 3:
                    buffer = _playerno_penaltytime_home_telegramm(0);
                    break;
                  case 4:
                    buffer = _playerno_penaltytime_home_telegramm(1);
                    break;
                  case 5:
                    buffer = _playerno_penaltytime_home_telegramm(2);
                    break;
                  case 6:
                    buffer = _playerno_penaltytime_home_telegramm(3);
                    break;
                  case 7:
                    buffer = _playerno_penaltytime_guest_telegramm(0);
                    break;
                  case 8:
                    buffer = _playerno_penaltytime_guest_telegramm(1);
                    break;
                  case 9:
                    buffer = _playerno_penaltytime_guest_telegramm(2);
                    break;
                  case 10:
                    buffer = _playerno_penaltytime_guest_telegramm(3);
                    break;
                  case 11:
                    switch (_team_send_index)
                    {
                      case 0:
                        buffer = _teamname_home_telegramm();
                        break;
                      case 1:
                        buffer = _teamname_guest_telegramm();
                        break;
                      case 2:
                        buffer = _brightness_telegramm();
                        break;
                      case 3:
                        buffer = _teamscore_period_bonus_telegramm();
                        break;
                      case 4:
                        buffer = _team_fouls_aktfoul_telegramm();
                        break;
                    }
                    ++_team_send_index;
                    if (_team_send_index > 4)
                    {
                      _team_send_index = 0;
                      break;
                    }
                    break;
                  case 12:
                    try
                    {
                      buffer = _player_send_index >= 14 ? (_player_send_index >= 28 ? (_player_send_index >= 42 ? _playerno_fouls_points_onfieldstatus_guest_telegramm(_player_send_index - 42) : _playerno_fouls_points_onfieldstatus_home_telegramm(_player_send_index - 28)) : _player_name_guest_telegramm(_player_send_index - 14)) : _player_name_home_telegramm(_player_send_index);
                      ++_player_send_index;
                      if (_player_send_index > 55)
                      {
                        _player_send_index = 0;
                        break;
                      }
                      break;
                    }
                    catch
                    {
                      break;
                    }
                }
                _ser_out.Write(buffer, 0, buffer.Length);
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
              ++num;
              if (num > 12)
                num = 0;
            }
          }
        }
        catch
        {
        }
        if (num == 0)
          Thread.Sleep(50);
        else
          Thread.Sleep(20);
      }
    }

    private byte[] AppendCheckSum(string Text)
    {
      int num = 0;
      byte[] numArray1 = new byte[Text.Length];
      for (int index = 0; index < Text.Length; ++index)
        numArray1[index] = Convert.ToByte(Text[index]);
      byte[] numArray2 = new byte[numArray1.Length + 1];
      for (int index = 0; index < numArray1.Length; ++index)
      {
        num += (int) numArray1[index];
        numArray2[index] = numArray1[index];
      }
      numArray2[numArray2.Length - 1] = (byte) (num % 256 & (int) sbyte.MaxValue);
      return numArray2;
    }

    private byte[] _teamscore_period_bonus_telegramm()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x0082');
      stringBuilder.Append(GetFieldValue("ScoreHome").PadLeft(3));
      stringBuilder.Append(GetFieldValue("ScoreGuest").PadLeft(3));
      stringBuilder.Append(" ");
      stringBuilder.Append(GetFieldValue("Period"));
      stringBuilder.Append(" ");
      stringBuilder.Append(" ");
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _game_clock_possession_timeout_telegramm()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x0080');
      stringBuilder.Append(" ");
      string str1 = GetFieldValue("GameTime");
      if (!str1.Contains(".") && !str1.Contains(":"))
        str1 = "0:" + str1.Trim().PadLeft(2, '0');
      string str2 = !str1.Contains(":") ? str1.PadLeft(4).PadRight(5) : str1.PadLeft(5);
      if (_sport == Sportart.Volleyball)
        stringBuilder.Append("     ");
      else
        stringBuilder.Append(str2);
      if (GetFieldValue("ServiceHome").Trim() != string.Empty)
        stringBuilder.Append('p');
      else
        stringBuilder.Append(" ");
      if (GetFieldValue("TimeOutsHome") != string.Empty)
      {
        switch (GetFieldValue("TimeOutsHome").Length)
        {
          case 0:
            stringBuilder.Append(" ");
            break;
          case 1:
            stringBuilder.Append("A");
            break;
          case 2:
            stringBuilder.Append("B");
            break;
          case 3:
            stringBuilder.Append("C");
            break;
          default:
            stringBuilder.Append(" ");
            break;
        }
      }
      else
        stringBuilder.Append(" ");
      if (GetFieldValue("ServiceGuest").Trim() != string.Empty)
        stringBuilder.Append('p');
      else
        stringBuilder.Append(" ");
      if (GetFieldValue("TimeOutsGuest") != string.Empty)
      {
        switch (GetFieldValue("TimeOutsGuest").Length)
        {
          case 0:
            stringBuilder.Append(" ");
            break;
          case 1:
            stringBuilder.Append("A");
            break;
          case 2:
            stringBuilder.Append("B");
            break;
          case 3:
            stringBuilder.Append("C");
            break;
          default:
            stringBuilder.Append(" ");
            break;
        }
      }
      else
        stringBuilder.Append(" ");
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _shot_clock_timeout_telegramm()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x0081');
      string fieldValue1 = GetFieldValue("ShotTime");
      if (fieldValue1.Trim() != string.Empty && fieldValue1.Trim() == "0" || _redlight)
        stringBuilder.Append('1');
      else
        stringBuilder.Append(" ");
      stringBuilder.Append(fieldValue1.PadLeft(2));
      stringBuilder.Append(":");
      string fieldValue2 = GetFieldValue("TimeoutTime");
      stringBuilder.Append(fieldValue2.PadLeft(2));
      stringBuilder.Append("  ");
      stringBuilder.Append("  ");
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _team_fouls_aktfoul_telegramm()
    {
      string str1 = GetFieldValue("ActFoulPlayer").PadLeft(6);
      string str2 = str1.Substring(0, 2);
      string str3 = str1.Substring(5);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x0083');
      stringBuilder.Append("  ");
      stringBuilder.Append(str2);
      stringBuilder.Append(" ");
      stringBuilder.Append(str3);
      stringBuilder.Append(" ");
      if (_sport == Sportart.Volleyball)
        stringBuilder.Append(GetFieldValue("SetsWonHome"));
      else
        stringBuilder.Append(GetFieldValue("TeamFoulsHome"));
      stringBuilder.Append(" ");
      if (_sport == Sportart.Volleyball)
        stringBuilder.Append(GetFieldValue("SetsWonGuest"));
      else
        stringBuilder.Append(GetFieldValue("TeamFoulsGuest"));
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _horn_telegramm()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x0094');
      string str = " ";
      if (_horn)
        str = "1";
      if (Form1.UseBasketHorn < 1)
        str = " ";
      stringBuilder.Append(str);
      stringBuilder.Append("2");
      stringBuilder.Append(" ");
      if (_shotclock_horn)
        stringBuilder.Append('1');
      else
        stringBuilder.Append(" ");
      stringBuilder.Append("2");
      stringBuilder.Append("     ");
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _set_score_telegramm(int set)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 141 + set;
      stringBuilder.Append((char) num);
      stringBuilder.Append(" ");
      string str1 = GetFieldValue("SetScoreHome" + set.ToString()).PadLeft(2);
      string str2 = GetFieldValue("SetScoreGuest" + set.ToString()).PadLeft(2);
      stringBuilder.Append(str1);
      if (str1 == "  ")
        stringBuilder.Append(" ");
      else
        stringBuilder.Append(":");
      stringBuilder.Append(str2);
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _brightness_telegramm()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('ð');
      stringBuilder.Append(_brightness.ToString("000"));
      stringBuilder.Append("       ");
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _teamname_home_telegramm()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x0092');
      stringBuilder.Append(GetFieldValue("TeamNameHome").PadRight(10).Substring(0, 10));
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _teamname_guest_telegramm()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x0093');
      stringBuilder.Append(GetFieldValue("TeamNameGuest").PadRight(10).Substring(0, 10));
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _player_name_home_telegramm(int index)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 160 + index;
      stringBuilder.Append((char) num);
      stringBuilder.Append(GetFieldValue("NameH" + (index + 1).ToString().PadRight(10).Substring(0, 10)));
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _player_name_guest_telegramm(int index)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num = 160 + index;
      stringBuilder.Append((char) num);
      stringBuilder.Append(GetFieldValue("NameG" + (index + 1).ToString().PadRight(10).Substring(0, 10)));
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _playerno_fouls_points_onfieldstatus_home_telegramm(int index)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string fieldValue = GetFieldValue("FoulsH" + (index + 1).ToString());
      byte num1 = 0;
      if (fieldValue != string.Empty)
      {
        switch (Convert.ToInt32(fieldValue))
        {
          case 1:
            num1 = (byte) 1;
            break;
          case 2:
            num1 = (byte) 3;
            break;
          case 3:
            num1 = (byte) 7;
            break;
          case 4:
            num1 = (byte) 15;
            break;
          case 5:
            num1 = (byte) 31;
            break;
          default:
            num1 = (byte) 0;
            break;
        }
      }
      int num2 = 192 + index;
      stringBuilder.Append((char) num2);
      stringBuilder.Append(" ");
      stringBuilder.Append(GetFieldValue("FoulNoH" + (index + 1).ToString().PadLeft(2)));
      stringBuilder.Append("  ");
      stringBuilder.Append(GetFieldValue("PointsH" + (index + 1).ToString().PadLeft(2)));
      stringBuilder.Append(" ");
      stringBuilder.Append((char) num1);
      stringBuilder.Append("  ");
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _playerno_fouls_points_onfieldstatus_guest_telegramm(int index)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string fieldValue = GetFieldValue("FoulsG" + (index + 1).ToString());
      byte num1 = 0;
      if (fieldValue.Trim() != string.Empty)
      {
        switch (Convert.ToInt32(fieldValue))
        {
          case 1:
            num1 = (byte) 1;
            break;
          case 2:
            num1 = (byte) 3;
            break;
          case 3:
            num1 = (byte) 7;
            break;
          case 4:
            num1 = (byte) 15;
            break;
          case 5:
            num1 = (byte) 31;
            break;
          default:
            num1 = (byte) 0;
            break;
        }
      }
      int num2 = 208 + index;
      stringBuilder.Append((char) num2);
      stringBuilder.Append(" ");
      stringBuilder.Append(GetFieldValue("FoulNoG" + (index + 1).ToString().PadLeft(2)));
      stringBuilder.Append("  ");
      stringBuilder.Append(GetFieldValue("PointsG" + (index + 1).ToString().PadLeft(2)));
      stringBuilder.Append(" ");
      stringBuilder.Append((char) num1);
      stringBuilder.Append("  ");
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _playerno_penaltytime_home_telegramm(int index)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string fieldValue = GetFieldValue("PenaltyNoH" + index.ToString().PadLeft(2));
      string str = GetFieldValue("PenaltyTimeH" + index.ToString()).PadLeft(5);
      int num = 131 + index;
      stringBuilder.Append((char) num);
      stringBuilder.Append(" ");
      stringBuilder.Append(fieldValue);
      stringBuilder.Append(":");
      stringBuilder.Append(str.Substring(0, 2));
      stringBuilder.Append(str.Substring(3));
      stringBuilder.Append("  ");
      return AppendCheckSum(stringBuilder.ToString());
    }

    private byte[] _playerno_penaltytime_guest_telegramm(int index)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string fieldValue = GetFieldValue("PenaltyNoG" + index.ToString().PadLeft(2));
      string str = GetFieldValue("PenaltyTimeG" + index.ToString()).PadLeft(5);
      int num = 131 + index;
      stringBuilder.Append((char) num);
      stringBuilder.Append(" ");
      stringBuilder.Append(fieldValue);
      stringBuilder.Append(":");
      stringBuilder.Append(str.Substring(0, 2));
      stringBuilder.Append(str.Substring(3));
      stringBuilder.Append("  ");
      return AppendCheckSum(stringBuilder.ToString());
    }
  }
}
