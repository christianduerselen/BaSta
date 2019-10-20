using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace BaSta.Scoreboard
{
  public class OmegaSaturnSender : IScoreBoard
  {
    private Sportart _sport = Sportart.Waterpolo;
    private int _refresh_interval = 30;
    private Dictionary<string, string> _fields = new Dictionary<string, string>();
    private short _serial_port = 1;
    private int _baudrate = 9600;
    private string _teamname_home = string.Empty;
    private string _teamname_guest = string.Empty;
    private bool _is_active = true;
    private const char STX = '\x0002';
    private const char ETX = '\x0003';
    private bool _sending_active;
    private bool _shotclock_horn;
    private bool _redlight;
    private bool _horn;
    private bool _time_running;
    private Thread _output_thread;
    private System.IO.Ports.SerialPort _ser_out;

    public bool RefreshTeams
    {
      set
      {
      }
    }

    public OmegaSaturnSender()
    {
    }

    public OmegaSaturnSender(int SerialPort, int RefreshInterval)
    {
      _refresh_interval = RefreshInterval;
      _serial_port = Convert.ToInt16(SerialPort);
    }

    public bool StartSending
    {
      set
      {
        _sending_active = value;
        if (!_sending_active)
          return;
        if (_output_thread != null)
        {
          if (_output_thread.IsAlive)
            _output_thread.Abort();
          _output_thread = (Thread) null;
        }
        if (_ser_out != null)
        {
          if (_ser_out.IsOpen)
            _ser_out.Close();
          _ser_out.Dispose();
          Thread.Sleep(2000);
        }
        _ser_out = new System.IO.Ports.SerialPort("COM" + _serial_port.ToString(), 9600, Parity.None, 8, StopBits.One);
        try
        {
          _ser_out.Open();
          _send_init_telegrams();
          _output_thread = new Thread(new ThreadStart(_output));
          _output_thread.Start();
        }
        catch
        {
        }
      }
    }

    public void SetFieldValue(string FieldName, string Value)
    {
      string str = Value;
      if (FieldName == "TeamNameGuest")
        str = "GAST";
      if (FieldName.StartsWith("Fouls") && Value.Trim() == string.Empty)
        return;
      if (_fields.ContainsKey(FieldName))
        _fields[FieldName] = str;
      else
        _fields.Add(FieldName, str);
    }

    public void Dispose()
    {
      _is_active = false;
      if (_output_thread != null && _output_thread.IsAlive)
        _output_thread.Abort();
      _output_thread = (Thread) null;
      try
      {
        if (!_ser_out.IsOpen)
          return;
        _send_clear_telegrams();
        _ser_out.Close();
      }
      catch
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

    private void _send_init_telegrams()
    {
      try
      {
        if (!_ser_out.IsOpen)
          _ser_out.Open();
        if (!_ser_out.IsOpen)
          return;
        byte[] buffer1 = _append_checksum(_init_telegramm1());
        _ser_out.Write(buffer1, 0, buffer1.Length);
        Thread.Sleep(1000);
        byte[] buffer2 = _append_checksum(_init_telegramm2());
        _ser_out.Write(buffer2, 0, buffer2.Length);
        Thread.Sleep(1000);
        byte[] buffer3 = _append_checksum(_init_telegramm3());
        _ser_out.Write(buffer3, 0, buffer3.Length);
        Thread.Sleep(1000);
      }
      catch
      {
      }
    }

    private void _send_clear_telegrams()
    {
      try
      {
        if (!_ser_out.IsOpen)
          _ser_out.Open();
        if (!_ser_out.IsOpen)
          return;
        byte[] buffer1 = _append_checksum(_clear_telegramm1());
        _ser_out.Write(buffer1, 0, buffer1.Length);
        Thread.Sleep(1000);
        byte[] buffer2 = _append_checksum(_clear_telegramm2());
        _ser_out.Write(buffer2, 0, buffer2.Length);
        Thread.Sleep(1000);
        byte[] buffer3 = _append_checksum(_clear_telegramm3());
        _ser_out.Write(buffer3, 0, buffer3.Length);
        Thread.Sleep(1000);
      }
      catch
      {
      }
    }

    public void Clear()
    {
      _send_clear_telegrams();
    }

    private string _init_telegramm1()
    {
      if (_fields.ContainsKey("TeamNameHome"))
        _teamname_home = _fields["TeamNameHome"].ToString();
      if (_fields.ContainsKey("TeamNameGuest"))
        _teamname_guest = _fields["TeamNameGuest"].ToString();
      return 2.ToString() + "N" + _teamname_home.PadRight(9).Substring(0, 9).ToUpper() + "      GAST " + (object) char.MinValue + "                                       " + (object) char.MinValue + "               " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "                        " + (object) '\x0003';
    }

    private string _init_telegramm2()
    {
      return 2.ToString() + "N" + _teamname_home.PadRight(9).Substring(0, 9).ToUpper() + "      GAST " + (object) char.MinValue + (object) char.MinValue + "               " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + (object) char.MinValue + "            " + (object) '\x0003';
    }

    private string _init_telegramm3()
    {
      return 2.ToString() + "N" + _teamname_home.PadRight(9).Substring(0, 9).ToUpper() + "      GAST " + (object) char.MinValue + "                                       " + (object) char.MinValue + "                " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) '\x0003';
    }

    private string _clear_telegramm1()
    {
      if (_fields.ContainsKey("TeamNameHome"))
        _teamname_home = _fields["TeamNameHome"].ToString();
      if (_fields.ContainsKey("TeamNameGuest"))
        _teamname_guest = _fields["TeamNameGuest"].ToString();
      return 2.ToString() + "N                   " + (object) char.MinValue + "                                       " + (object) char.MinValue + "               " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "                        " + (object) '\x0003';
    }

    private string _clear_telegramm2()
    {
      return 2.ToString() + "N                   " + (object) char.MinValue + (object) char.MinValue + "               " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + (object) char.MinValue + "            " + (object) '\x0003';
    }

    private string _clear_telegramm3()
    {
      return 2.ToString() + "N                   " + (object) char.MinValue + "                                       " + (object) char.MinValue + "                " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "           " + (object) char.MinValue + "                       " + (object) '\x0003';
    }

    private void _output()
    {
      int num1 = 0;
      int num2 = 0;
      string text = (string) null;
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
      while (_sending_active)
      {
        try
        {
          switch (num1)
          {
            case 0:
              text = _gametime_telegram();
              break;
            case 1:
              text = _timeout_telegram();
              break;
            case 2:
              text = _daytime_telegram();
              break;
            case 3:
              text = _gametime_telegram();
              break;
            case 4:
              text = _fouls_and_player_no_telegram(HomeGuestTeam.Home);
              break;
            case 5:
              text = _gametime_telegram();
              break;
            case 6:
              text = _timeout_telegram();
              break;
            case 7:
              text = _daytime_telegram();
              break;
            case 8:
              text = _gametime_telegram();
              break;
            case 9:
              text = _fouls_and_player_no_telegram(HomeGuestTeam.Guest);
              break;
            case 10:
              text = _gametime_telegram();
              break;
            case 11:
              text = _timeout_telegram();
              break;
            case 12:
              text = _daytime_telegram();
              break;
            case 13:
              text = _gametime_telegram();
              break;
            case 14:
              text = _player_points_telegram(HomeGuestTeam.Home);
              break;
            case 15:
              text = _gametime_telegram();
              break;
            case 16:
              text = _timeout_telegram();
              break;
            case 17:
              text = _daytime_telegram();
              break;
            case 18:
              text = _gametime_telegram();
              break;
            case 19:
              text = _player_points_telegram(HomeGuestTeam.Guest);
              break;
          }
          byte[] buffer = _append_checksum(text);
          _ser_out.Write(buffer, 0, buffer.Length);
          ++num1;
          if (num1 > 19)
            num1 = 0;
        }
        catch
        {
        }
        ++num2;
        Thread.Sleep(_refresh_interval);
      }
    }

    private string GetFieldValue(string FieldName)
    {
      if (_fields.ContainsKey(FieldName))
        return _fields[FieldName].ToString();
      return string.Empty;
    }

    private byte[] _append_checksum(string text)
    {
      byte num = 0;
      byte[] numArray = new byte[text.Length + 1];
      byte[] bytes = Encoding.Default.GetBytes(text);
      for (int index = 0; index < bytes.Length; ++index)
      {
        num ^= bytes[index];
        numArray[index] = bytes[index];
      }
      numArray[numArray.Length - 1] = num;
      return numArray;
    }

    private string _gametime_telegram()
    {
      string empty = string.Empty;
      string str1 = !_time_running ? (!_shotclock_horn ? "300" : "322") : (!_shotclock_horn ? "010" : "032");
      string fieldValue1 = GetFieldValue("TimeoutTimeHome");
      if (fieldValue1 == string.Empty)
        fieldValue1 = GetFieldValue("TimeoutTimeGuest");
      string str2 = GetFieldValue("GameTime");
      if (str2.Trim() == string.Empty)
        str2 = "     ";
      if (str2.Trim() == "0.0")
        _horn = true;
      if (str2.Contains("."))
        str2 += " ";
      if (str2.Substring(1, 1) == "." || str2.Substring(1, 1) == ":")
        str2 = " " + str2;
      string fieldValue2 = GetFieldValue("ScoreHome");
      string fieldValue3 = GetFieldValue("ScoreGuest");
      string str3 = GetFieldValue("TeamFoulsHome").Trim();
      string str4 = GetFieldValue("TeamFoulsGuest").Trim();
      string str5 = GetFieldValue("TimeOutsHome").Trim();
      string str6 = GetFieldValue("TimeOutsGuest").Trim();
      string str7 = GetFieldValue("Period").Trim();
      string str8 = GetFieldValue("ShotTime").Trim();
      return 2.ToString() + "D" + str2.PadRight(5) + fieldValue2.PadLeft(3) + fieldValue3.PadLeft(3) + str3.PadLeft(1, '0') + str4.PadLeft(1, '0') + str5.Length.ToString() + str6.Length.ToString() + str7.PadLeft(1) + str1 + fieldValue1.PadLeft(2) + str8.PadLeft(2) + (object) '\x0003';
    }

    private string _timeout_telegram()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('\x0002');
      stringBuilder.Append("C");
      stringBuilder.Append(GetFieldValue("TeamFoulsHome"));
      stringBuilder.Append(" ");
      stringBuilder.Append(GetFieldValue("TimeoutTimeHome").PadLeft(2));
      stringBuilder.Append("    ");
      stringBuilder.Append(GetFieldValue("ActFoulPlayerNumberHome").PadLeft(2));
      stringBuilder.Append(GetFieldValue("ActFoulHome").PadLeft(2));
      stringBuilder.Append(GetFieldValue("TimeoutTimeGuest").PadLeft(2));
      stringBuilder.Append(" ");
      stringBuilder.Append(GetFieldValue("TeamFoulsGuest"));
      stringBuilder.Append("    ");
      stringBuilder.Append(GetFieldValue("ActFoulPlayerNumberGuest").PadLeft(2));
      stringBuilder.Append(GetFieldValue("ActFoulGuest").PadLeft(2));
      stringBuilder.Append("00");
      stringBuilder.Append('\x0003');
      return stringBuilder.ToString();
    }

    private string _daytime_telegram()
    {
      DateTime now = DateTime.Now;
      return 2.ToString() + "T" + (now.Day.ToString("00") + "/" + now.Month.ToString("00") + "/" + now.Year.ToString().Substring(2, 2)) + (now.Hour.ToString("00") + ":" + now.Minute.ToString("00") + "." + now.Second.ToString("00")) + "€0      " + (object) '\x0003';
    }

    private string _fouls_and_player_no_telegram(HomeGuestTeam _team)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(2.ToString() + "F");
      if (_team == HomeGuestTeam.Home)
      {
        stringBuilder.Append("1");
        for (int index = 0; index < 16; ++index)
        {
          string FieldName1 = "NoH" + (index + 1).ToString();
          string FieldName2 = "FoulsH" + (index + 1).ToString();
          stringBuilder.Append(GetFieldValue(FieldName1).PadLeft(2));
          string str = GetFieldValue(FieldName2).PadLeft(1);
          if (str == "0")
            str = " ";
          if (GetFieldValue(FieldName1).Trim() == string.Empty)
            str = " ";
          stringBuilder.Append(str);
        }
      }
      else
      {
        stringBuilder.Append("2");
        for (int index = 0; index < 16; ++index)
        {
          string FieldName1 = "NoG" + (index + 1).ToString();
          string FieldName2 = "FoulsG" + (index + 1).ToString();
          stringBuilder.Append(GetFieldValue(FieldName1).PadLeft(2));
          string str = GetFieldValue(FieldName2).PadLeft(1);
          if (str == "0")
            str = " ";
          if (GetFieldValue(FieldName1).Trim() == string.Empty)
            str = " ";
          stringBuilder.Append(str);
        }
      }
      stringBuilder.Append('\x0003');
      return stringBuilder.ToString();
    }

    private string _player_points_telegram(HomeGuestTeam _team)
    {
      string empty = string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(2.ToString() + "F");
      if (_team == HomeGuestTeam.Home)
      {
        stringBuilder.Append("3");
        for (int index = 0; index < 16; ++index)
        {
          string str = GetFieldValue("PointsH" + (index + 1).ToString()).PadLeft(2);
          if (str == " 0")
            str = "  ";
          stringBuilder.Append(str);
        }
      }
      else
      {
        stringBuilder.Append("4");
        for (int index = 0; index < 16; ++index)
        {
          string str = GetFieldValue("PointsG" + (index + 1).ToString()).PadLeft(2);
          if (str == " 0")
            str = "  ";
          stringBuilder.Append(str);
        }
      }
      stringBuilder.Append('\x0003');
      return stringBuilder.ToString();
    }
  }
}
