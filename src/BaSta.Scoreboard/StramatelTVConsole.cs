using System;
using System.Text;

namespace BaSta.Scoreboard
{
  internal class StramatelTVConsole
  {
    private static string _period = string.Empty;
    private string _game_time = string.Empty;
    private string _horn = string.Empty;
    private string _temp_inbuffer = string.Empty;
    private string _endstring = string.Concat((object) '\r');
    private string _startstring = string.Concat((object) 'ø');
    private string _datagram_game_time = string.Empty;
    private string _datagram33 = string.Empty;
    private string _datagram37 = string.Empty;
    private string _datagram38 = string.Empty;
    private short _datagram37_timeout = 1;
    private short _datagram38_timeout = 2;
    private string _temp_buffer = string.Empty;
    private Sportart _sport;
    private short _datagram33_timeout;
    private int _error_counter;

    public event StramatelTVConsole.MessageReceivedDelegate MessageReceived;

    public event StramatelTVConsole.BufferErrorDelegate BufferError;

    public Sportart Sportart
    {
      set
      {
        _sport = value;
      }
    }

    public void EvaluateStramatelData(string Buffer)
    {
      _temp_buffer = Buffer;
      if (_temp_buffer.Length <= 53)
        return;
      if ((byte) _temp_buffer[0] == (byte) 248 && (byte) _temp_buffer[53] == (byte) 13)
      {
        if (_temp_buffer.Length > 54)
          _temp_buffer = _temp_buffer.Substring(0, 54);
        if ((byte) _temp_buffer[1] != (byte) 102 && (byte) _temp_buffer[1] != (byte) 98 && ((byte) _temp_buffer[1] != (byte) 119 && (byte) _temp_buffer[1] != (byte) 54))
          _game_time = (byte) _temp_buffer[7] == (byte) 32 ? ((byte) _temp_buffer[4] != (byte) 48 ? _temp_buffer.Substring(4, 2) + "." + _temp_buffer.Substring(6, 2) : " " + _temp_buffer.Substring(5, 1) + "." + _temp_buffer.Substring(6, 2)) : _temp_buffer.Substring(4, 2) + ":" + _temp_buffer.Substring(6, 2);
        if ((byte) _temp_buffer[1] != (byte) 51 && (byte) _temp_buffer[1] != (byte) 53 && ((byte) _temp_buffer[1] != (byte) 54 && (byte) _temp_buffer[1] != (byte) 57) && (byte) _temp_buffer[1] != (byte) 58)
        {
          if ((byte) _temp_buffer[1] != (byte) 108)
            goto label_9;
        }
        StramatelTVConsole._period = _temp_buffer.Substring(14, 1);
label_9:
        try
        {
          switch ((byte) _temp_buffer[1])
          {
            case 51:
              try
              {
                ++_datagram33_timeout;
                if (!(_temp_buffer != _datagram33))
                {
                  if (_datagram33_timeout <= (short) 5)
                    break;
                }
                _datagram33_timeout = (short) 0;
                _datagram33 = _temp_buffer;
                _sport = Sportart.Basketball;
                StringBuilder stringBuilder = new StringBuilder("*Basketball|GameTime=" + _game_time + "=" + ((byte) _temp_buffer[20] == (byte) 49 ? "1" : "0") + "|Period=" + StramatelTVConsole._period + "=0|Horn=" + ((byte) _temp_buffer[19] == (byte) 49 ? "On=0" : "Off=0"));
                if ((byte) _temp_buffer[3] == (byte) 49)
                {
                  stringBuilder.Append("|ServiceHome=o=0");
                  stringBuilder.Append("|ServiceGust= =0");
                }
                else if ((byte) _temp_buffer[3] == (byte) 50)
                {
                  stringBuilder.Append("|ServiceHome= =0");
                  stringBuilder.Append("|ServiceGuest=o=0");
                }
                else
                {
                  stringBuilder.Append("|ServiceHome= =0");
                  stringBuilder.Append("|ServiceGuest= =0");
                }
                stringBuilder.Append("|ScoreHome=" + _temp_buffer.Substring(8, 3) + "=0");
                stringBuilder.Append("|ScoreGuest=" + _temp_buffer.Substring(11, 3) + "=0");
                stringBuilder.Append("|TeamFoulsHome=" + _temp_buffer.Substring(15, 1) + "=" + (Convert.ToInt32(_temp_buffer.Substring(15, 1)) > 4 ? "1" : "0"));
                stringBuilder.Append("|TeamFoulsGuest=" + _temp_buffer.Substring(16, 1) + "=" + (Convert.ToInt32(_temp_buffer.Substring(16, 1)) > 4 ? "1" : "0"));
                if (_temp_buffer.Substring(17, 1) != " ")
                  stringBuilder.Append("|TimeOutsHome=" + "".PadLeft(Convert.ToInt32(_temp_buffer.Substring(17, 1)), 'o') + "=0");
                else
                  stringBuilder.Append("|TimeOutsHome= =0");
                if (_temp_buffer.Substring(18, 1) != " ")
                  stringBuilder.Append("|TimeOutsGuest=" + "".PadLeft(Convert.ToInt32(_temp_buffer.Substring(18, 1)), 'o') + "=0");
                else
                  stringBuilder.Append("|TimeOutsGuest= =0");
                for (int index = 0; index < 12; ++index)
                {
                  stringBuilder.Append("|FoulsH" + (index + 1).ToString() + "=" + _temp_buffer.Substring(22 + index, 1) + "=" + (_temp_buffer[22 + index] > '4' ? "1" : "0"));
                  stringBuilder.Append("|FoulsG" + (index + 1).ToString() + "=" + _temp_buffer.Substring(34 + index, 1) + "=" + (_temp_buffer[34 + index] > '4' ? "1" : "0"));
                }
                stringBuilder.Append("|TimeoutTimeHome=" + _temp_buffer.Substring(46, 2) + "=0");
                stringBuilder.Append("|ShotTime=" + _temp_buffer.Substring(48, 2) + "=0");
                stringBuilder.Append("|SC_Horn=" + (_temp_buffer[50] == '1' ? "On" : "Off") + "=0");
                _datagram_game_time = _temp_buffer.Substring(4, 4);
                MessageReceived(this, stringBuilder.ToString());
                break;
              }
              catch
              {
                break;
              }
            case 55:
              try
              {
                ++_datagram37_timeout;
                if (!(_temp_buffer.Substring(8) != _datagram37) && !(_temp_buffer.Substring(4, 4) != _datagram_game_time))
                {
                  if (_datagram37_timeout <= (short) 5)
                    break;
                }
                _datagram37_timeout = (short) 0;
                _datagram37 = _temp_buffer.Substring(8);
                StringBuilder stringBuilder = new StringBuilder("*" + _sport.ToString() + "|GameTime=" + _game_time + "=" + ((byte) _temp_buffer[20] == (byte) 49 ? "1" : "0") + "|Period=" + StramatelTVConsole._period + "=0|Horn=" + ((byte) _temp_buffer[19] == (byte) 49 ? "On=0" : "Off=0"));
                for (int index = 0; index < 13; ++index)
                {
                  if (_temp_buffer.Substring(22 + index * 2, 2).Trim() != string.Empty)
                    stringBuilder.Append("|PointsG" + (index + 1).ToString() + "=" + _temp_buffer.Substring(22 + index * 2, 2) + "=0");
                  else
                    stringBuilder.Append("|PointsG" + (index + 1).ToString() + "= 0=0");
                }
                stringBuilder.Append("|PointsG14=" + _temp_buffer.Substring(11, 2) + "=0");
                stringBuilder.Append("|ShotTime=" + _temp_buffer.Substring(48, 2) + "=0");
                stringBuilder.Append("|SC_Horn=" + (_temp_buffer[50] == '1' ? "On" : "Off") + "=0");
                _datagram_game_time = _temp_buffer.Substring(4, 4);
                MessageReceived(this, stringBuilder.ToString());
                break;
              }
              catch
              {
                break;
              }
            case 56:
              try
              {
                ++_datagram38_timeout;
                if (!(_temp_buffer.Substring(8) != _datagram38) && !(_temp_buffer.Substring(4, 4) != _datagram_game_time))
                {
                  if (_datagram38_timeout <= (short) 5)
                    break;
                }
                _datagram38_timeout = (short) 0;
                _datagram38 = _temp_buffer.Substring(8);
                StringBuilder stringBuilder = new StringBuilder("*" + _sport.ToString() + "|GameTime=" + _game_time + "=" + ((byte) _temp_buffer[20] == (byte) 49 ? "1" : "0") + "|Period=" + StramatelTVConsole._period + "=0|Horn=" + ((byte) _temp_buffer[19] == (byte) 49 ? "On=0" : "Off=0"));
                for (int index = 0; index < 13; ++index)
                {
                  if (_temp_buffer.Substring(22 + index * 2, 2).Trim() != string.Empty)
                    stringBuilder.Append("|PointsH" + (index + 1).ToString() + "=" + _temp_buffer.Substring(22 + index * 2, 2) + "=0");
                  else
                    stringBuilder.Append("|PointsH" + (index + 1).ToString() + "= 0=0");
                }
                stringBuilder.Append("|PointsH14=" + _temp_buffer.Substring(11, 2) + "=0");
                stringBuilder.Append("|ShotTime=" + _temp_buffer.Substring(48, 2) + "=0");
                stringBuilder.Append("|SC_Horn=" + (_temp_buffer[50] == '1' ? "On" : "Off") + "=0");
                _datagram_game_time = _temp_buffer.Substring(4, 4);
                MessageReceived(this, stringBuilder.ToString());
                break;
              }
              catch
              {
                break;
              }
            case 98:
              try
              {
                StringBuilder stringBuilder = new StringBuilder("*none");
                if (_temp_buffer[4] == ' ' && _temp_buffer[5] == ' ')
                {
                  stringBuilder.Append("|TeamNameHome=" + _temp_buffer.Substring(6, 9) + "=0");
                  stringBuilder.Append("|TeamNameGuest=" + _temp_buffer.Substring(18, 9) + "=0");
                  MessageReceived(this, stringBuilder.ToString());
                  break;
                }
                stringBuilder.Append("|NoG" + Convert.ToInt16(_temp_buffer.Substring(4, 2)).ToString() + "=" + _temp_buffer.Substring(16, 2) + "=0");
                stringBuilder.Append("|NameG" + Convert.ToInt16(_temp_buffer.Substring(4, 2)).ToString() + "=" + _temp_buffer.Substring(6, 10).Trim() + "=0");
                MessageReceived(this, stringBuilder.ToString());
                break;
              }
              catch
              {
                break;
              }
            case 119:
              try
              {
                StringBuilder stringBuilder = new StringBuilder("*none");
                stringBuilder.Append("|NoH" + Convert.ToInt16(_temp_buffer.Substring(4, 2)).ToString() + "=" + _temp_buffer.Substring(16, 2) + "=0");
                stringBuilder.Append("|NameH" + Convert.ToInt16(_temp_buffer.Substring(4, 2)).ToString() + "=" + _temp_buffer.Substring(6, 10).Trim() + "=0");
                MessageReceived(this, stringBuilder.ToString());
                break;
              }
              catch
              {
                break;
              }
          }
        }
        catch
        {
        }
      }
      else
      {
        ++_error_counter;
        Console.WriteLine("Fehler" + _error_counter.ToString());
        BufferError(this);
      }
      GC.Collect();
    }

    public delegate void MessageReceivedDelegate(StramatelTVConsole Sender, string Message);

    public delegate void BufferErrorDelegate(StramatelTVConsole Sender);
  }
}
