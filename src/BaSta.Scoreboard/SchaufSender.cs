using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace BaSta.Scoreboard
{
  internal class SchaufSender : IScoreBoard
  {
    private string _horn_on = string.Empty + (object) '\x001B' + (object) '"' + (object) '"' + (object) 'E' + (object) '1' + (object) '\b' + (object) '1' + (object) 'E';
    private string _horn_off = string.Empty + (object) '\x001B' + (object) '"' + (object) '"' + (object) 'C' + (object) '1' + (object) '\b' + (object) '3' + (object) 'E';
    private string _old_game_time = string.Empty;
    private string _score_home = string.Empty;
    private string _score_guest = string.Empty;
    private string _timeouts_home = string.Empty;
    private string _timeouts_guest = string.Empty;
    private string _period = string.Empty;
    private string _act_foul = string.Empty;
    private string _team_fouls_home = string.Empty;
    private string _team_fouls_guest = string.Empty;
    private List<string> _pers_fouls_home = new List<string>();
    private List<string> _pers_fouls_guest = new List<string>();
    private Sportart _sport = Sportart.Waterpolo;
    private int _refresh_interval = 30;
    private short _serial_port = 1;
    private int _baudrate = 19200;
    private System.IO.Ports.SerialPort _ser_out = new System.IO.Ports.SerialPort();
    private string _teamname_home = string.Empty;
    private string _teamname_guest = string.Empty;
    private Dictionary<string, string> _fields = new Dictionary<string, string>();
    private const char ESC = '\x001B';
    private const char AdrGameTime = '!';
    private const char AdrTimeOuts = '"';
    private const char AdrHorn = '$';
    private const char HornOn = 'a';
    private const char HornOff = 'A';
    private const char AdrFouls = ')';
    private const char AdrTeamFouls = '(';
    private const char AdrScore = '#';
    private const char AdrPeriod = '%';
    private bool _shotclock_horn;
    private bool _redlight;
    private bool _horn_changed;
    private bool _horn;
    private bool _time_running;
    private Thread _output_thread;
    private bool _sendin_active;

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
        _horn_changed = _horn != value;
        _horn = value;
        if (!_ser_out.IsOpen)
          return;
        _ser_out.DtrEnable = _horn;
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

    private void _open_ser_out()
    {
      if (_ser_out.IsOpen)
        _ser_out.Close();
      _ser_out.PortName = "COM" + _serial_port.ToString();
      try
      {
        _ser_out.Open();
      }
      catch
      {
      }
    }

    private string GetFieldValue(string FieldName)
    {
      if (_fields.ContainsKey(FieldName))
        return _fields[FieldName].ToString();
      return string.Empty;
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

    public void Clear()
    {
    }

    public SchaufSender()
    {
    }

    public SchaufSender(int SerialPort, int RefreshInterval)
    {
      for (int index = 0; index < 15; ++index)
      {
        _pers_fouls_home.Add(string.Empty);
        _pers_fouls_guest.Add(string.Empty);
      }
      _refresh_interval = 50;
      _serial_port = Convert.ToInt16(SerialPort);
      _ser_out = new System.IO.Ports.SerialPort("COM" + _serial_port.ToString(), 1200, Parity.None, 8, StopBits.One);
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
        _sendin_active = value;
      }
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
      while (true)
      {
        string text = (string) null;
        try
        {
          if (_ser_out.IsOpen)
          {
            if (_sendin_active)
            {
              if (_horn_changed)
              {
                _horn_changed = false;
                if (_horn)
                  _ser_out.Write(_horn_on);
                else
                  _ser_out.Write(_horn_off);
              }
              string str1 = GetFieldValue("GameTime");
              if (str1.Contains("."))
                str1 = str1.PadLeft(4).PadRight(5).Substring(0, 2);
              if (str1 != _old_game_time)
              {
                _old_game_time = str1;
                text = _gametime_str_builder().ToString();
              }
              else
              {
                string fieldValue1 = GetFieldValue("ScoreHome");
                if (fieldValue1 != _score_home)
                {
                  _score_home = fieldValue1;
                  text = _score_telegram();
                }
                else
                {
                  string fieldValue2 = GetFieldValue("ScoreGuest");
                  if (fieldValue2 != _score_guest)
                  {
                    _score_guest = fieldValue2;
                    text = _score_telegram();
                  }
                  else
                  {
                    string fieldValue3 = GetFieldValue("Period");
                    if (fieldValue3 != _period)
                    {
                      _period = fieldValue3;
                      text = _period_telegram();
                    }
                    else
                    {
                      string fieldValue4 = GetFieldValue("TeamFoulsHome");
                      if (fieldValue4 != _team_fouls_home)
                      {
                        _team_fouls_home = fieldValue4;
                        text = _actfoul_teamfouls_telegram();
                      }
                      else
                      {
                        string fieldValue5 = GetFieldValue("TeamFoulsGuest");
                        if (fieldValue5 != _team_fouls_guest)
                        {
                          _team_fouls_guest = fieldValue5;
                          text = _actfoul_teamfouls_telegram();
                        }
                        else
                        {
                          string str2 = GetFieldValue("ActFoulPlayer").PadLeft(4);
                          if (str2 != _act_foul)
                          {
                            _act_foul = str2;
                            text = _actfoul_teamfouls_telegram();
                          }
                          else
                          {
                            string fieldValue6 = GetFieldValue("TimeOutsHome");
                            if (fieldValue6 != _timeouts_home)
                            {
                              _timeouts_home = fieldValue6;
                              text = _timeouts_home_telegram();
                            }
                            else
                            {
                              string fieldValue7 = GetFieldValue("TimeOutsGuest");
                              if (fieldValue7 != _timeouts_guest)
                              {
                                _timeouts_guest = fieldValue7;
                                text = _timeouts_guest_telegram();
                              }
                              else
                              {
                                bool flag = false;
                                for (int ix = 0; ix < 12; ++ix)
                                {
                                  if (!flag)
                                  {
                                    string fieldValue8 = GetFieldValue("FoulsH" + (ix + 1).ToString());
                                    if (fieldValue8.Trim() != string.Empty && fieldValue8 != _pers_fouls_home[ix])
                                    {
                                      flag = true;
                                      _pers_fouls_home[ix] = fieldValue8;
                                      text = _fouls_home_telegram(ix);
                                    }
                                  }
                                  if (!flag)
                                  {
                                    string fieldValue8 = GetFieldValue("FoulsG" + (ix + 1).ToString());
                                    if (fieldValue8.Trim() != string.Empty && fieldValue8 != _pers_fouls_guest[ix])
                                    {
                                      flag = true;
                                      _pers_fouls_guest[ix] = fieldValue8;
                                      text = _fouls_guest_telegram(ix);
                                    }
                                  }
                                }
                                if (!flag)
                                  text = _gametime_str_builder().ToString();
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
              if (text != null)
              {
                _ser_out.Write(text);
                Thread.Sleep(text.Length * 10);
              }
              else
                Thread.Sleep(10);
            }
          }
        }
        catch
        {
        }
        Thread.Sleep(1);
      }
    }

    private StringBuilder _gametime_str_builder()
    {
      string empty = string.Empty;
      string str1 = GetFieldValue("GameTime").Trim();
      string str2;
      if (str1.Contains("."))
      {
        string str3 = str1.PadLeft(4).PadRight(5);
        string str4 = str3.Substring(3);
        string str5 = str3.Substring(0, 2);
        str2 = !(GetFieldValue("ServiceGuest") != string.Empty) || !(GetFieldValue("ServiceHome") != string.Empty) ? str5.PadRight(4) : str5 + str4;
      }
      else
      {
        string str3 = str1.PadLeft(5);
        str2 = str3.Substring(0, 2) + str3.Substring(3);
      }
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x001B');
      stringBuilder.Append('!');
      stringBuilder.Append(' ');
      stringBuilder.Append(str2);
      return stringBuilder;
    }

    private string _horn_telegram()
    {
      return _gametime_str_builder().ToString();
    }

    private string _period_telegram()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x001B');
      stringBuilder.Append('%');
      stringBuilder.Append(' ');
      stringBuilder.Append(_period);
      return stringBuilder.ToString();
    }

    private string _score_telegram()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x001B');
      stringBuilder.Append('#');
      stringBuilder.Append(' ');
      stringBuilder.Append(_score_home.PadLeft(3));
      stringBuilder.Append(_score_guest.PadLeft(3));
      return stringBuilder.ToString();
    }

    private string _actfoul_teamfouls_telegram()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x001B');
      stringBuilder.Append('(');
      stringBuilder.Append('!');
      stringBuilder.Append(_team_fouls_home.PadLeft(1));
      string str = GetFieldValue("ActFoulPlayer").PadLeft(4);
      stringBuilder.Append(str.Substring(0, 2).Trim().PadLeft(2));
      stringBuilder.Append(str.Substring(str.Length - 1));
      stringBuilder.Append('5');
      stringBuilder.Append(_team_fouls_guest.PadLeft(1));
      return stringBuilder.ToString();
    }

    private string _timeouts_home_telegram()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Empty + (object) '\x001B' + (object) '"' + (object) ' ' + (object) '4' + (object) '7');
      stringBuilder.Append('\x001B');
      stringBuilder.Append('"');
      stringBuilder.Append(' ');
      switch (_timeouts_home.Length)
      {
        case 0:
          stringBuilder.Append('4');
          stringBuilder.Append('7');
          break;
        case 1:
          stringBuilder.Append('C');
          stringBuilder.Append('1');
          break;
        case 2:
          stringBuilder.Append('C');
          stringBuilder.Append('3');
          break;
        case 3:
          stringBuilder.Append('C');
          stringBuilder.Append('7');
          break;
        default:
          stringBuilder.Append('4');
          stringBuilder.Append('7');
          break;
      }
      return stringBuilder.ToString();
    }

    private string _timeouts_guest_telegram()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Empty + (object) '\x001B' + (object) '"' + (object) '!' + (object) '4' + (object) '7');
      stringBuilder.Append('\x001B');
      stringBuilder.Append('"');
      stringBuilder.Append('!');
      switch (_timeouts_guest.Length)
      {
        case 0:
          stringBuilder.Append('4');
          stringBuilder.Append('7');
          break;
        case 1:
          stringBuilder.Append('C');
          stringBuilder.Append('1');
          break;
        case 2:
          stringBuilder.Append('C');
          stringBuilder.Append('3');
          break;
        case 3:
          stringBuilder.Append('C');
          stringBuilder.Append('7');
          break;
        default:
          stringBuilder.Append('4');
          stringBuilder.Append('7');
          break;
      }
      return stringBuilder.ToString();
    }

    private string _fouls_home_telegram(int ix)
    {
      string empty = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x001B');
      stringBuilder.Append(')');
      stringBuilder.Append((char) (35 + ix));
      switch (_pers_fouls_home[ix])
      {
        case "0":
          stringBuilder.Append('@');
          break;
        case "1":
          stringBuilder.Append('B');
          break;
        case "2":
          stringBuilder.Append('F');
          break;
        case "3":
          stringBuilder.Append('N');
          break;
        case "4":
          stringBuilder.Append('^');
          break;
        case "5":
          stringBuilder.Append('~');
          break;
      }
      return stringBuilder.ToString();
    }

    private string _fouls_guest_telegram(int ix)
    {
      string empty = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x001B');
      stringBuilder.Append(')');
      stringBuilder.Append((char) (51 + ix));
      switch (_pers_fouls_guest[ix])
      {
        case "0":
          stringBuilder.Append('@');
          break;
        case "1":
          stringBuilder.Append('B');
          break;
        case "2":
          stringBuilder.Append('F');
          break;
        case "3":
          stringBuilder.Append('N');
          break;
        case "4":
          stringBuilder.Append('^');
          break;
        case "5":
          stringBuilder.Append('~');
          break;
      }
      return stringBuilder.ToString();
    }
  }
}
