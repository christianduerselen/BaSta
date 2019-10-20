using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace BaSta.Game.Broadcast
{
  public class UDP_Broadcast_Sender
  {
    private string _programm_version = string.Empty;
    private bool _do_send_other_information = false;
    private bool _network_available = false;
    private UdpClient _sender = (UdpClient) null;
    private int _send_port = 8000;
    private Thread _send_information_thread = (Thread) null;
    private bool _stopped = false;
    private string _sport = string.Empty;
    private Form _source_form = (Form) null;
    private bool _gametime_running = false;
    private bool _is_max_home_team_foul = false;
    private bool _is_max_guest_team_foul = false;
    private bool[] _is_max_home_player_foul = new bool[15];
    private bool[] _is_max_guest_player_foul = new bool[15];
    private bool[] _is_penalty_time_home_running = new bool[3];
    private bool[] _is_penalty_time_guest_running = new bool[3];
    private bool _do_send = true;
    private string _old_send_string = string.Empty;
    private const string _ask_connection = "?";
    private const string _confirm_connection = "!";
    private const string _close_connection = "#";
    private const string _start_of_information = "*";
    private const string _information_separator = "|";
    private const string _data_separator = "=";

    public bool DoSendOtherInformation
    {
      get
      {
        return _do_send_other_information;
      }
      set
      {
        _do_send_other_information = value;
      }
    }

    public bool Stopped
    {
      get
      {
        return _stopped;
      }
      set
      {
        _stopped = value;
      }
    }

    public string Sport
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

    public bool GameTimeRunning
    {
      set
      {
        _gametime_running = value;
      }
    }

    public bool IsMaxHomeTeamFoul
    {
      set
      {
        _is_max_home_team_foul = value;
      }
    }

    public bool IsMaxGuestTeamFoul
    {
      set
      {
        _is_max_guest_team_foul = value;
      }
    }

    public void IsMaxPlayerFoul(bool HomeTeam, int Index, bool Value)
    {
      if (HomeTeam)
        _is_max_home_player_foul[Index] = Value;
      else
        _is_max_guest_player_foul[Index] = Value;
    }

    public void IsPenaltyTimeRunning(bool HomeTeam, int Index, bool Value)
    {
      if (HomeTeam)
        _is_penalty_time_home_running[Index] = Value;
      else
        _is_penalty_time_guest_running[Index] = Value;
    }

    public UDP_Broadcast_Sender(
      string ProgramVersion,
      string Sport,
      Form SourceForm,
      bool LanAvailable)
    {
      _programm_version = ProgramVersion;
      _source_form = SourceForm;
      _sport = Sport;
      _network_available = LanAvailable;
      _start_connection();
    }

    public UDP_Broadcast_Sender(
      string ProgramVersion,
      string Sport,
      Form SourceForm,
      bool LanAvailable,
      int Port)
    {
      _programm_version = ProgramVersion;
      _sport = Sport;
      _source_form = SourceForm;
      _send_port = Port;
      _network_available = LanAvailable;
      _start_connection();
    }

    private void _start_connection()
    {
      if (_network_available)
      {
        _sender = new UdpClient();
        _sender.Client.SendBufferSize = 256000;
        _sender.EnableBroadcast = true;
        try
        {
          _sender.Connect(IPAddress.Broadcast, _send_port);
          if (_send_information_thread != null)
            return;
          _send_information_thread = new Thread(new ThreadStart(_send_information));
          _send_information_thread.IsBackground = true;
          _send_information_thread.Start();
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show(ex.Message);
        }
      }
      else
      {
        _sender = new UdpClient();
        _sender.EnableBroadcast = false;
        _sender.Connect(IPAddress.Loopback, _send_port);
        if (_send_information_thread == null)
        {
          _send_information_thread = new Thread(new ThreadStart(_send_information));
          _send_information_thread.IsBackground = true;
          _send_information_thread.Start();
        }
      }
    }

    public void Dispose()
    {
      _do_send = false;
    }

    public void Send(string Text)
    {
      if (!(Text != string.Empty))
        return;
      byte[] bytes = Encoding.UTF8.GetBytes(Text);
      try
      {
        _sender.Send(bytes, bytes.Length);
      }
      catch
      {
      }
    }

    public void Send(byte[] data)
    {
      if (_sender == null)
        return;
      _sender.DontFragment = false;
      try
      {
        try
        {
          _sender.Send(data, data.Length);
        }
        catch
        {
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void _send_information()
    {
      bool flag1 = true;
      StringBuilder stringBuilder1 = new StringBuilder();
      while (_do_send)
      {
        if (!_do_send_other_information)
        {
          Exception exception;
          try
          {
            _programm_version = "00000000";
            StringBuilder stringBuilder2 = new StringBuilder("*" + _sport + "|Version=" + _programm_version + "=0");
            foreach (Control control in (ArrangedElementCollection) _source_form.Controls)
            {
              if (control.GetType() == typeof (Label) && !control.Name.StartsWith("label") && !control.Name.StartsWith("lbl"))
              {
                try
                {
                  stringBuilder2.Append("|");
                  stringBuilder2.Append(control.Name);
                  bool flag2 = false;
                  if (control.Name == "TeamFoulsHome")
                    flag2 = _is_max_home_team_foul;
                  if (control.Name == "TeamFoulsGuest")
                    flag2 = _is_max_guest_team_foul;
                  if (control.Name == "GameTime")
                    flag2 = !_gametime_running;
                  if (control.Name.StartsWith("FoulNoH") && _is_max_home_player_foul.Length > Convert.ToInt32(control.Name.Substring(7)) - 1)
                    flag2 = _is_max_home_player_foul[Convert.ToInt32(control.Name.Substring(7)) - 1];
                  if (control.Name.StartsWith("NameH") && _is_max_home_player_foul.Length > Convert.ToInt32(control.Name.Substring(5)) - 1)
                    flag2 = _is_max_home_player_foul[Convert.ToInt32(control.Name.Substring(5)) - 1];
                  if (control.Name.StartsWith("FoulsH") && _is_max_home_player_foul.Length > Convert.ToInt32(control.Name.Substring(6)) - 1)
                    flag2 = _is_max_home_player_foul[Convert.ToInt32(control.Name.Substring(6)) - 1];
                  if (control.Name.StartsWith("FoulNoG") && _is_max_guest_player_foul.Length > Convert.ToInt32(control.Name.Substring(7)) - 1)
                    flag2 = _is_max_guest_player_foul[Convert.ToInt32(control.Name.Substring(7)) - 1];
                  if (control.Name.StartsWith("NameG") && _is_max_guest_player_foul.Length > Convert.ToInt32(control.Name.Substring(5)) - 1)
                    flag2 = _is_max_guest_player_foul[Convert.ToInt32(control.Name.Substring(5)) - 1];
                  if (control.Name.StartsWith("FoulsG") && _is_max_guest_player_foul.Length > Convert.ToInt32(control.Name.Substring(6)) - 1)
                    flag2 = _is_max_guest_player_foul[Convert.ToInt32(control.Name.Substring(6)) - 1];
                  if (control.Name.StartsWith("PenaltyNoH") && _is_penalty_time_home_running.Length > Convert.ToInt32(control.Name.Substring(10)) - 1)
                    flag2 = _is_penalty_time_home_running[Convert.ToInt32(control.Name.Substring(10)) - 1];
                  if (control.Name.StartsWith("PenaltyTimeH") && _is_penalty_time_home_running.Length > Convert.ToInt32(control.Name.Substring(12)) - 1)
                    flag2 = _is_penalty_time_home_running[Convert.ToInt32(control.Name.Substring(12)) - 1];
                  if (control.Name.StartsWith("PenaltyNoG") && _is_penalty_time_guest_running.Length > Convert.ToInt32(control.Name.Substring(10)) - 1)
                    flag2 = _is_penalty_time_guest_running[Convert.ToInt32(control.Name.Substring(10)) - 1];
                  if (control.Name.StartsWith("PenaltyTimeG") && _is_penalty_time_guest_running.Length > Convert.ToInt32(control.Name.Substring(12)) - 1)
                    flag2 = _is_penalty_time_guest_running[Convert.ToInt32(control.Name.Substring(12)) - 1];
                  stringBuilder2.Append("=");
                  stringBuilder2.Append(control.Text.Trim());
                  stringBuilder2.Append("=");
                  stringBuilder2.Append(flag2 ? "1" : "0");
                }
                catch (Exception ex)
                {
                  exception = ex;
                }
              }
            }
            flag1 = !flag1;
            if (!_stopped)
            {
              Send(stringBuilder2.ToString());
              _old_send_string = stringBuilder2.ToString();
            }
          }
          catch (Exception ex)
          {
            exception = ex;
          }
          Thread.Sleep(50);
        }
      }
      _sender.Close();
      _do_send = true;
    }

    public void DoSendVersionInfo(string ProgramVersion)
    {
      _do_send_other_information = true;
      Thread.Sleep(250);
      Send("!V" + ProgramVersion);
      _do_send_other_information = false;
    }

    public void DoStartSequence(
      string GameKind,
      string EventName,
      string Title,
      int GameID,
      int TeamID,
      bool IsHomeTeam,
      bool ReservePlayerOnly,
      bool Coach,
      bool IsReferees,
      int PlayerNumber,
      string Playername,
      string GameTime,
      string ScoreHome,
      string ScoreGuest,
      int EffectIndex,
      bool GraphicInWindow,
      bool MakeImageOnly)
    {
      _do_send_other_information = true;
      Thread.Sleep(250);
      Send("!E" + EffectIndex.ToString("00"));
      if (GraphicInWindow)
        Send("!w");
      else
        Send("!W");
      string str1 = IsHomeTeam ? "1" : "0";
      string str2 = ReservePlayerOnly ? "1" : "0";
      string str3 = Coach ? "1" : "0";
      string str4 = IsReferees ? "1" : "0";
      string str5 = MakeImageOnly ? "1" : "0";
      Send("!S|" + GameKind.Trim() + "|" + EventName.Trim() + "|" + Title + "|" + GameID.ToString() + "|" + TeamID.ToString() + "|" + str1 + "|" + str2 + "|" + str3 + "|" + str4 + "|" + PlayerNumber.ToString() + "|" + Playername + "|" + GameTime + "|" + ScoreHome + "|" + ScoreGuest + "|" + str5);
      _do_send_other_information = false;
    }

    public void MakeImage(string GameKind, string ImageName, List<string> Source, bool DoShow)
    {
      _do_send_other_information = true;
      Thread.Sleep(250);
      string str = DoShow ? "1" : "0";
      string Text = "!M|" + GameKind.Trim() + "|" + ImageName + "|" + str;
      for (int index = 0; index < Source.Count; ++index)
        Text = Text + "|" + Source[index];
      Send(Text);
      _do_send_other_information = false;
    }

    public void DoShowImage(string ImageName, bool ShowInWindow, int EffectIndex)
    {
      _do_send_other_information = true;
      Thread.Sleep(250);
      string str = ShowInWindow ? "1" : "0";
      Send("!I|" + ImageName + "|" + str + "|" + EffectIndex.ToString());
      _do_send_other_information = false;
    }

    public void DoChangeGraphicWindow(bool IsCfgDefinedGraphicWindow)
    {
      _do_send_other_information = true;
      Thread.Sleep(250);
      Send("!G" + (IsCfgDefinedGraphicWindow ? "1" : "0"));
      _do_send_other_information = false;
    }

    public void DeleteImageFile(string FileName)
    {
      _do_send_other_information = true;
      Send("!C" + FileName);
    }
  }
}
