using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BaSta.Game.Broadcast;
using BaSta.Game.Properties;

namespace BaSta.Game
{
  public class Basketball : Form
  {
    private InvokeFunctions _invoke = new InvokeFunctions();
    private long _max_teamfouls = 5;
    private long _max_playerfouls = 5;
    private bool _auto_horn_at_timeouts = true;
    private GUI_Functions _f = new GUI_Functions();
    private int _sc_horn_length = 3000;
    private int _horn_length = 3000;
    private int _timeouts_10s_horn_length = 3000;
    private int _timeouts_end_horn_length = 3000;
    private string _program_version = string.Empty;
    private string ServiceHomeSymbol = "";
    private string ServiceGuestSymbol = "";
    private UDP_Broadcast_Sender _sender;
    private DataBaseFunctions _dbfunc;
    private Game _act_game;
    private Dictionary<string, PeriodParameter> _presets;
    private Dictionary<string, GameData> _game_data;
    private List<GamePlayer> _game_player_home;
    private List<GamePlayer> _game_player_guest;
    private DownCounter _gametime_timer;
    private DownCounter _breaktime_timer;
    private DownCounter _shottime_timer;
    private DownCounter _timeout_timer_home;
    private DownCounter _timeout_timer_guest;
    private long _team_fouls_home;
    private long _team_fouls_guest;
    private long _timeouts_home;
    private long _timeouts_guest;
    private GamePlayer _act_player;
    private Label _act_label;
    private SerialConsole _console;
    private bool _countdown_active;
    private bool _horn_has_ringed;
    private long _TimeoutValue;
    private bool _console_shotclock_reset24_pressed;
    private bool _console_shotclock_reset14_pressed;
    private bool _console_gametime_running;
    private bool _precisiontime_gametime_running;
    private bool _gametime_run;
    private IContainer components;
    private Label label4;
    private Label TimeOutsHome;
    private Label TimeOutsGuest;
    private CheckBox cbStopShotclock;
    private Label Period;
    private Button btnReset24;
    private Button btnReset14;
    private Label TeamNameHome;
    private Label ShotTime;
    private Button btnStartStop;
    private Label GameTime;
    private Label TeamFoulsGuest;
    private ContextMenuStrip cmsEditTeamFouls;
    private ToolStripMenuItem tmsTeamFoulsPlus1;
    private ToolStripMenuItem tmsTeamFoulsMinus1;
    private Label TeamFoulsHome;
    private Label FoulsG15;
    private Label PointsG15;
    private Label NameG15;
    private Label NoG15;
    private Label FoulsG14;
    private Label ScoreGuest;
    private Label TeamNameGuest;
    private Label PointsG14;
    private Label NameG14;
    private Label ScoreHome;
    private Label NoG14;
    private Label FoulsG13;
    private Label PointsG13;
    private Label NameG13;
    private Label NoG13;
    private Label FoulsG12;
    private Label PointsG12;
    private Label NameG12;
    private Label NoG12;
    private Label FoulsG11;
    private Label PointsG11;
    private Label NameG11;
    private Label NoG11;
    private Label FoulsG10;
    private Label PointsG10;
    private Label NameG10;
    private Label NoG10;
    private Label FoulsG9;
    private Label PointsG9;
    private Label NameG9;
    private Label NoG9;
    private Label FoulsG8;
    private Label PointsG8;
    private Label NameG8;
    private Label NoG8;
    private Label FoulsG7;
    private Label PointsG7;
    private Label NameG7;
    private Label NoG7;
    private Label FoulsG6;
    private Label PointsG6;
    private Label NameG6;
    private Label NoG6;
    private Label FoulsG5;
    private Label PointsG5;
    private Label NameG5;
    private Label NoG5;
    private Label FoulsG4;
    private Label PointsG4;
    private Label NameG4;
    private Label NoG4;
    private Label FoulsG3;
    private Label PointsG3;
    private Label NameG3;
    private Label NoG3;
    private Label FoulsG2;
    private Label PointsG2;
    private Label NameG2;
    private Label NoG2;
    private Label FoulsG1;
    private Label PointsG1;
    private Label NameG1;
    private Label NoG1;
    private Label FoulsH15;
    private Label PointsH15;
    private Label NameH15;
    private Label NoH15;
    private Label FoulsH14;
    private Label PointsH14;
    private Label NameH14;
    private Label NoH14;
    private Label FoulsH13;
    private Label PointsH13;
    private Label NameH13;
    private Label NoH13;
    private Label FoulsH12;
    private Label PointsH12;
    private Label NameH12;
    private Label NoH12;
    private Label FoulsH11;
    private Label PointsH11;
    private Label NameH11;
    private Label NoH11;
    private Label FoulsH10;
    private Label PointsH10;
    private Label NameH10;
    private Label NoH10;
    private Label FoulsH9;
    private Label PointsH9;
    private Label NameH9;
    private Label NoH9;
    private Label FoulsH8;
    private Label PointsH8;
    private Label NameH8;
    private Label NoH8;
    private Label FoulsH7;
    private Label PointsH7;
    private Label NameH7;
    private Label NoH7;
    private Label FoulsH6;
    private Label PointsH6;
    private Label NameH6;
    private Label NoH6;
    private Label FoulsH5;
    private Label PointsH5;
    private Label NameH5;
    private Label NoH5;
    private Label FoulsH4;
    private Label PointsH4;
    private Label NameH4;
    private Label NoH4;
    private Label FoulsH3;
    private Label PointsH3;
    private Label NameH3;
    private Label NoH3;
    private Label FoulsH2;
    private Label PointsH2;
    private Label NameH2;
    private Label NoH2;
    private Label FoulsH1;
    private Label PointsH1;
    private Label NameH1;
    private Label NoH1;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label5;
    private Label label6;
    private ContextMenuStrip cmsPlayerData;
    private ToolStripMenuItem tmsPlayerPointsPlus3;
    private ToolStripMenuItem tmsPlayerPointsPlus2;
    private ToolStripMenuItem tmsPlayerPointsPlus1;
    private ToolStripMenuItem tmsPlayerPointsMinus1;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem tmsPlayerCorrPointsPlus1;
    private ToolStripMenuItem tmsPlayerCorrPointsMinus1;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripMenuItem tmsPlayerFoulsPlus1;
    private ToolStripMenuItem tmsPlayerFoulsMinus1;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem tmsPlayerEdit;
    private ContextMenuStrip cmsEditPeriod;
    private ToolStripMenuItem tmsNextPeriod;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem tmsPeriodPlus1;
    private ToolStripMenuItem tmsPeriodMinus1;
    private ContextMenuStrip cmsEditTime;
    private ToolStripMenuItem tsmEditGameTime;
    private ContextMenuStrip cmsEditScore;
    private ToolStripMenuItem tmsScorePlus1;
    private ToolStripMenuItem tmsScoreMinus1;
    private ContextMenuStrip cmsEditTimeouts;
    private ToolStripMenuItem tsmTimeoutPlus1;
    private ToolStripMenuItem tmsTimeoutMinus1;
    private Label TimeoutTimeGuest;
    private Label TimeoutTimeHome;
    private Label labelCaution;
    private Label labelTitlePeriodTimeNotElapsed;
    private Label labelChangePeriodAnyway;
    private ToolStripMenuItem tsmPlayerName;
    private ToolStripSeparator toolStripSeparator5;
    private Button btnHorn;
    private CheckBox cbShotclockDark;
    private Label Horn;
    private Timer HornTimer;
    private Label SC_Horn;
    private Label ActFoulPlayer;
    private Timer actFoulPlayerTimer;
    private Label TeamID_Home;
    private Label TeamID_Guest;
    private Label ServiceHome;
    private Label ServiceGuest;
    private Label label7;
    private Label label8;
    private Label label9;
    private ToolStripMenuItem tmsStartBreakTime;
    private ToolStripSeparator toolStripSeparator6;
    private Label labelStartBreaktimeAnyway;
    private Label labelStopBreaktimeFirst;
    private ToolStripSeparator toolStripSeparator7;
    private ToolStripMenuItem tmsScoreDirectInput;
    private Label ActFoulPlayerNumberHome;
    private Label ActFoulHome;
    private Label ActFoulGuest;
    private Label ActFoulPlayerNumberGuest;
    private ToolStripSeparator toolStripSeparator8;
    private ToolStripMenuItem showTimeoutTimeToolStripMenuItem;
    private ToolStripMenuItem tmsPointsPlus3;
    private ToolStripMenuItem tmsPointsPlus2;
    private ToolStripSeparator toolStripSeparator9;
    private Label lblShotclockTenth;
    private Label lblGameTimeTenth;
    private Label TimeRunning;
    private Label GameTimeStopped;
    private CheckBox cbEdgeLightAtShotclockZero;
    private Label SC_EdgeLightOn;
    private CheckBox cbShotclockIndependentStart;
    private Label label10;
    private Label label11;
    private Label label12;
    private Label label13;
    private Button btnLeft;
    private Button btnRight;
    private Label label14;
    private Label label15;
    private ToolStripMenuItem tsmTimeoutPlus60s;
    private ToolStripMenuItem tsmTimeoutPlus30s;
    private ToolStripSeparator toolStripSeparator10;
    private GroupBox GBPause;
    private Button BtnStartPause;
    private Button BtnAbortStartPause;
    private Label LbPauseBeginnen;

    public Basketball(
      string ProgramVersion,
      DataBaseFunctions DbFunc,
      Game ActGame,
      List<GamePlayer> HomePlayer,
      List<GamePlayer> GuestPlayer)
    {
      InitializeComponent();
      _gametime_timer = new DownCounter("BreakTimer", ref GameTime);
      _timeout_timer_home = new DownCounter("TimeOutTimerHome", ref TimeoutTimeHome);
      _timeout_timer_guest = new DownCounter("TimeOutTimerGuest", ref TimeoutTimeGuest);
      _shottime_timer = new DownCounter("ShotTimer", ref ShotTime);
      _program_version = ProgramVersion;
      Settings.Default.Reload();
      SC_EdgeLightOn.Text = cbEdgeLightAtShotclockZero.Checked ? "2" : "1";
      SC_EdgeLightOn.Tag = (object) SC_EdgeLightOn.Text;
      _sc_horn_length = Settings.Default.ShotclockHornLength;
      _horn_length = Settings.Default.HornLength;
      HornTimer.Tag = (object) "0";
      GBPause.Visible = false;
      _sender = new UDP_Broadcast_Sender(_program_version, nameof (Basketball), (Form) this, Settings.Default.LAN, Settings.Default.UDP_Port);
      _f.NormalBackColor = GameTime.BackColor;
      _dbfunc = DbFunc;
      _game_player_home = HomePlayer;
      _game_player_guest = GuestPlayer;
      _act_game = ActGame;
      _act_game.Started = true;
      _dbfunc.SaveGame(ActGame);
      TeamNameHome.Text = _act_game.HomeTeamName;
      TeamNameGuest.Text = _act_game.GuestTeamName;
      _game_data = _dbfunc.LoadGameData((Form) this, _act_game.ID);
      _load_settings(false);
      _team_fouls_home = _dbfunc.LoadTeamFouls(_act_game.ID, true);
      if (_team_fouls_home <= Convert.ToInt64(TeamFoulsHome.Tag))
        TeamFoulsHome.Text = _team_fouls_home.ToString();
      else
        TeamFoulsHome.Text = TeamFoulsHome.Tag.ToString();
      _team_fouls_guest = _dbfunc.LoadTeamFouls(_act_game.ID, false);
      if (_team_fouls_guest <= Convert.ToInt64(TeamFoulsGuest.Tag))
        TeamFoulsGuest.Text = _team_fouls_guest.ToString();
      else
        TeamFoulsGuest.Text = TeamFoulsGuest.Tag.ToString();
      _dbfunc.ClearGamePlayersList((Form) this, _game_player_home, _game_player_guest, 15L);
      _dbfunc.ShowGamePlayers((Form) this, _game_player_home, _game_player_guest, _max_playerfouls);
      for (int index = 0; index < 15; ++index)
      {
        if (index < _game_player_home.Count)
        {
          _game_player_home[index].DoBlink += new BlinkDelegate(Fouls_DoBlink);
          _game_player_home[index].DontBlink();
        }
        if (index < _game_player_guest.Count)
        {
          _game_player_guest[index].DoBlink += new BlinkDelegate(Fouls_DoBlink);
          _game_player_guest[index].DontBlink();
        }
      }
      if (Period.Text.Trim() == string.Empty)
        Period.Text = "1";
      if (ScoreHome.Text.Trim() == string.Empty)
        ScoreHome.Text = "0";
      if (ScoreGuest.Text.Trim() == string.Empty)
        ScoreGuest.Text = "0";
      _game_data = _dbfunc.LoadGameData((Form) this, _act_game.ID);
      _gametime_timer = new DownCounter("GameTimer", ref GameTime, _presets["PeriodTime"].ParameterIntValue, true);
      _gametime_timer.TimeChanged += new TimeChangedDelegate(_gametime_timer_TimeChanged);
      _gametime_timer.EndTimeReached += new EndTimeReachedDelegate(_gametime_timer_EndTimeReached);
      if (_game_data.ContainsKey(nameof (GameTime)))
        _gametime_timer.Milliseconds = _game_data[nameof (GameTime)].Value;
      _shottime_timer = new DownCounter("ShotTimer", ref ShotTime, _presets["ShotTimeLong"].ParameterIntValue, true);
      _shottime_timer.Milliseconds = !_game_data.ContainsKey(nameof (ShotTime)) ? _presets["ShotTimeLong"].ParameterIntValue : _game_data[nameof (ShotTime)].Value;
      _shottime_timer.TimeChanged += new TimeChangedDelegate(_shottime_timer_TimeChanged);
      _shottime_timer.EndTimeReached += new EndTimeReachedDelegate(_shottime_timer_EndTimeReached);
      if (_game_data.ContainsKey(nameof (TimeoutTimeHome)))
      {
        _timeout_timer_home = new DownCounter("TimeOutTimerHome", ref TimeoutTimeHome, _game_data[nameof (TimeoutTimeHome)].Value, false);
        _timeout_timer_home.ShowTimeInSecondsOnly = true;
        _timeout_timer_home.ShowTenth = false;
        _timeout_timer_home.TimeChanged += new TimeChangedDelegate(_timeout_timer_home_TimeChanged);
        _timeout_timer_home.EndTimeReached += new EndTimeReachedDelegate(_timeout_timer_home_EndTimeReached);
      }
      if (_game_data.ContainsKey(nameof (TimeoutTimeGuest)))
      {
        _timeout_timer_guest = new DownCounter("TimeOutTimerGuest", ref TimeoutTimeGuest, _game_data[nameof (TimeoutTimeGuest)].Value, false);
        _timeout_timer_guest.ShowTimeInSecondsOnly = true;
        _timeout_timer_guest.ShowTenth = false;
        _timeout_timer_guest.TimeChanged += new TimeChangedDelegate(_timeout_timer_guest_TimeChanged);
        _timeout_timer_guest.EndTimeReached += new EndTimeReachedDelegate(_timeout_timer_guest_EndTimeReached);
      }
      TeamID_Home.Text = ActGame.HomeTeamID.ToString();
      TeamID_Guest.Text = ActGame.GuestTeamID.ToString();
      _console = new SerialConsole(Settings.Default.ConsolePort);
      _console.ShowInTaskbar = false;
      _console.WindowState = FormWindowState.Minimized;
      _console.Show();
      _console.Visible = false;
      _precisiontime_gametime_running = _console.PrecisionTimeStatusGameTimeRunning;
      _console_gametime_running = _console.StatusGameTimeRunning;
      _console.StatusChanged += new SerialConsole.StatusChangedDelegate(_console_StatusChanged);
      _invoke.setLabelTextAsync(ServiceHome, string.Empty);
      _invoke.setLabelTextAsync(ServiceGuest, string.Empty);
      _invoke.setLabelTextAsync(GameTimeStopped, " ");
    }

    private void _StopCountDownPeriod()
    {
      _countdown_active = false;
      cbShotclockDark.Checked = false;
    }

    private void _breaktime_timer_EndTimeReached(ITimeCounter sender)
    {
      Horn.Text = "On";
      HornTimer.Interval = 3000;
      HornTimer.Start();
      btnStartStop.Enabled = true;
      btnStartStop.Text = "Start";
      if (_act_game.Period == 1 && _countdown_active)
      {
        _StopCountDownPeriod();
        _act_game.Period = 1;
      }
      _act_label = Period;
      tmsNextPeriod_Click((object) this, (EventArgs) null);
      _gametime_run = false;
      if (_gametime_timer != null)
      {
        while (_console.StatusGameTimeRunning && !_gametime_timer.IsRunning)
        {
          int num1 = (int) MessageBox.Show("Bitte den Schalter für die Spielzeit in Stellung Stop bringen!");
        }
      }
      else
      {
        while (_console.StatusGameTimeRunning)
        {
          int num2 = (int) MessageBox.Show("Bitte den Schalter für die Spielzeit in Stellung Stop bringen!");
        }
      }
    }

    private void _breaktime_timer_TimeChanged(ITimeCounter sender, string time, long milliseconds)
    {
      if (milliseconds / 60000L > 0L)
      {
        GameTime.Text = time;
        GameTime.Update();
        lblGameTimeTenth.Text = (milliseconds % 1000L / 100L).ToString();
      }
      else
      {
        if (time.Substring(3).StartsWith("0"))
          GameTime.Text = time.Substring(4);
        else
          GameTime.Text = time.Substring(3);
        GameTime.Update();
        lblGameTimeTenth.Text = string.Empty;
      }
    }

    private void _console_StatusChanged(SerialConsole sender, bool Init)
    {
      if (Init)
      {
        _precisiontime_gametime_running = sender.PrecisionTimeStatusGameTimeRunning;
        _console_gametime_running = sender.StatusGameTimeRunning;
      }
      else
      {
        if (sender.PrecisionTimeStatusGameTimeRunning != _precisiontime_gametime_running)
        {
          _precisiontime_gametime_running = sender.PrecisionTimeStatusGameTimeRunning;
          _console_gametime_running = sender.StatusGameTimeRunning;
          _gametime_run = _precisiontime_gametime_running;
        }
        else if (sender.StatusGameTimeRunning != _console_gametime_running)
        {
          _precisiontime_gametime_running = sender.PrecisionTimeStatusGameTimeRunning;
          _console_gametime_running = sender.StatusGameTimeRunning;
          _gametime_run = _console_gametime_running;
        }
        if (_breaktime_timer != null)
        {
          if (_breaktime_timer.Milliseconds < 99L)
          {
            _breaktime_timer.Dispose();
            _breaktime_timer = (DownCounter) null;
          }
          else if (_console_gametime_running)
          {
            _start_game_time();
            _invoke.setButtonTextAsync(btnStartStop, "Stop");
          }
          else
          {
            _stop_game_time();
            _invoke.setButtonTextAsync(btnStartStop, "Start");
          }
        }
        if (_breaktime_timer == null)
        {
          if (_gametime_run)
          {
            if (!_gametime_timer.IsRunning)
            {
              _start_game_time();
              _invoke.setButtonTextAsync(btnStartStop, "Stop");
            }
          }
          else if (_gametime_timer.IsRunning)
          {
            _stop_game_time();
            _invoke.setButtonTextAsync(btnStartStop, "Start");
          }
        }
      }
      if (sender.StatusHornButtonPressed && Horn.Text == "Off")
      {
        HornTimer.Stop();
        HornTimer.Tag = (object) "0";
        _invoke.setLabelTextAsync(Horn, "On");
      }
      if (!sender.StatusHornButtonPressed && Horn.Text == "On")
      {
        HornTimer.Stop();
        HornTimer.Tag = (object) "0";
        _invoke.setLabelTextAsync(Horn, "Off");
      }
      _invoke.setCheckBoxCheckedAsync(cbStopShotclock, sender.StatusShotClockRunning);
      bool clockReset24Pressed = sender.StatusShotClockReset24Pressed;
      bool clockReset14Pressed = sender.StatusShotClockReset14Pressed;
      if (clockReset24Pressed)
      {
        _console_shotclock_reset24_pressed = clockReset24Pressed;
        if (_shottime_timer.IsRunning)
          _shottime_timer.Stop();
        _invoke.setLabelTextAsync(ShotTime, string.Empty);
        _invoke.setLabelTextAsync(lblShotclockTenth, string.Empty);
      }
      if (clockReset14Pressed)
      {
        _console_shotclock_reset14_pressed = clockReset14Pressed;
        if (_shottime_timer.IsRunning)
          _shottime_timer.Stop();
        _invoke.setLabelTextAsync(ShotTime, string.Empty);
        _invoke.setLabelTextAsync(lblShotclockTenth, string.Empty);
      }
      if (!clockReset24Pressed && _console_shotclock_reset24_pressed)
      {
        _console_shotclock_reset24_pressed = false;
        btnResetShotclock_MouseUp((object) btnReset24, (MouseEventArgs) null);
      }
      if (!clockReset14Pressed && _console_shotclock_reset14_pressed)
      {
        _console_shotclock_reset14_pressed = false;
        btnResetShotclock_MouseUp((object) btnReset14, (MouseEventArgs) null);
      }
      _console.Busy = false;
    }

    private void _set_shotclock(Button source)
    {
      _shottime_timer.Milliseconds = Convert.ToInt64(source.Tag);
      if (_gametime_timer.Milliseconds >= Convert.ToInt64(source.Tag))
      {
        if (_gametime_timer.Milliseconds / 1000L != Convert.ToInt64(source.Tag) / 1000L)
        {
            if (!cbStopShotclock.Checked && _gametime_timer.IsRunning)
            _shottime_timer.Start();
        }

        _invoke.setLabelTextAsync(ShotTime, (Convert.ToInt64(source.Tag) / 1000L).ToString());
      }
      else
      {
          _shottime_timer.Stop();
        _invoke.setLabelTextAsync(ShotTime, string.Empty);
      }
    }

    private void Fouls_DoBlink()
    {
      for (int index = 0; index < 15; ++index)
      {
        try
        {
          if (index < _game_player_home.Count)
          {
            if (_game_player_home[index].FoulsVisible)
            {
              if (_game_player_home[index].Fouls < 1)
                Controls["FoulsH" + (index + 1).ToString()].Text = "0";
              else
                Controls["FoulsH" + (index + 1).ToString()].Text = _game_player_home[index].Fouls.ToString();
            }
            else
              Controls["FoulsH" + (index + 1).ToString()].Text = string.Empty;
          }
          if (index < _game_player_guest.Count)
          {
            if (_game_player_guest[index].FoulsVisible)
            {
              if (_game_player_guest[index].Fouls < 1)
                Controls["FoulsG" + (index + 1).ToString()].Text = "0";
              else
                Controls["FoulsG" + (index + 1).ToString()].Text = _game_player_guest[index].Fouls.ToString();
            }
            else
              Controls["FoulsG" + (index + 1).ToString()].Text = string.Empty;
          }
        }
        catch
        {
        }
      }
    }

    private void _timeout_timer_guest_EndTimeReached(ITimeCounter sender)
    {
      TimeoutTimeGuest.Text = string.Empty;
    }

    private void _timeout_timer_guest_TimeChanged(
      ITimeCounter sender,
      string time,
      long milliseconds)
    {
      if (showTimeoutTimeToolStripMenuItem.Checked)
      {
        if (time.Contains(":"))
        {
          string str = time.Substring(time.IndexOf(':') + 1);
          if (str.StartsWith("0"))
            str = str.Substring(1);
          TimeoutTimeGuest.Text = str;
        }
        else
          TimeoutTimeGuest.Text = time;
        if (TimeoutTimeGuest.Text == "11")
          _horn_has_ringed = false;
        if (TimeoutTimeGuest.Text == "10" && _auto_horn_at_timeouts && !_horn_has_ringed)
        {
          _horn_has_ringed = true;
          Horn.Text = "On";
          HornTimer.Interval = _timeouts_10s_horn_length;
          HornTimer.Start();
        }
        if (!(TimeoutTimeGuest.Text.Trim() == "0"))
          return;
        TimeoutTimeGuest.Text = string.Empty;
        if (!_auto_horn_at_timeouts)
          return;
        Horn.Text = "On";
        HornTimer.Interval = _timeouts_end_horn_length;
        HornTimer.Start();
      }
      else
        TimeoutTimeGuest.Text = string.Empty;
    }

    private void _timeout_timer_home_EndTimeReached(ITimeCounter sender)
    {
      TimeoutTimeHome.Text = string.Empty;
    }

    private void _timeout_timer_home_TimeChanged(
      ITimeCounter sender,
      string time,
      long milliseconds)
    {
      if (showTimeoutTimeToolStripMenuItem.Checked)
      {
        if (time.Contains(":"))
        {
          string str = time.Substring(time.IndexOf(':') + 1);
          if (str.StartsWith("0"))
            str = str.Substring(1);
          TimeoutTimeHome.Text = str;
        }
        else
          TimeoutTimeHome.Text = time;
        if (TimeoutTimeHome.Text == "11")
          _horn_has_ringed = false;
        if (TimeoutTimeHome.Text == "10" && _auto_horn_at_timeouts && !_horn_has_ringed)
        {
          _horn_has_ringed = true;
          Horn.Text = "On";
          HornTimer.Interval = _timeouts_10s_horn_length;
          HornTimer.Start();
        }
        if (!(TimeoutTimeHome.Text.Trim() == "0"))
          return;
        TimeoutTimeHome.Text = string.Empty;
        if (!_auto_horn_at_timeouts)
          return;
        Horn.Text = "On";
        HornTimer.Interval = _timeouts_end_horn_length;
        HornTimer.Start();
      }
      else
        TimeoutTimeHome.Text = string.Empty;
    }

    private void _shottime_timer_EndTimeReached(ITimeCounter sender)
    {
      if (cbShotclockDark.Checked)
        return;
      if (SC_EdgeLightOn.Tag.ToString() != "1")
      {
        SC_EdgeLightOn.Tag = (object) "R";
        SC_EdgeLightOn.Text = "R";
        GameTime.Update();
      }
      _invoke.setLabelTextAsync(GameTimeStopped, GameTimeStopped.Tag.ToString());
      SC_Horn.Text = "On";
      HornTimer.Interval = _sc_horn_length;
      HornTimer.Start();
    }

    private void _shottime_timer_TimeChanged(ITimeCounter sender, string time, long milliseconds)
    {
      long num = milliseconds;
      if (!cbShotclockDark.Checked && !_console.StatusShotClockReset14Pressed && !_console.StatusShotClockReset24Pressed)
      {
        lblShotclockTenth.Text = (num % 1000L / 100L).ToString();
        if (cbShotclockDark.Checked && _breaktime_timer == null)
          return;
        string str = (num / 1000L).ToString().PadLeft(2);
        if (num >= 5000L && num < 9999L)
          str = (num / 1000L).ToString().PadLeft(1) + ".";
        if (num < 5000L && num > 0L)
          str = (num / 1000L).ToString().PadLeft(1) + (object) '.' + lblShotclockTenth.Text;
        ShotTime.Text = str;
      }
      else
      {
        ShotTime.Text = string.Empty;
        lblShotclockTenth.Text = string.Empty;
        _invoke.setLabelTextAsync(GameTimeStopped, " ");
      }
    }

    private void _gametime_timer_EndTimeReached(ITimeCounter sender)
    {
      if (SC_EdgeLightOn.Tag.ToString() != "1")
      {
        SC_EdgeLightOn.Tag = (object) "2";
        SC_EdgeLightOn.Text = "2";
        GameTime.Update();
      }
      if (_gametime_timer.IsRunning)
        clickStartStopButtonAsync();
      _invoke.setLabelTextAsync(GameTimeStopped, GameTimeStopped.Tag.ToString());
      GameTime.Text = "0.0";
      GameTime.Update();
      Horn.Text = "On";
      if (HornTimer != null)
      {
        HornTimer.Interval = _horn_length;
        HornTimer.Start();
      }
      GBPause.Visible = true;
    }

    private void _gametime_timer_TimeChanged(ITimeCounter sender, string time, long milliseconds)
    {
      if (milliseconds / 60000L > 0L)
      {
        GameTime.Text = time;
        GameTime.Update();
        lblGameTimeTenth.Text = (milliseconds % 1000L / 100L).ToString();
      }
      else
      {
        if (time.Substring(3).StartsWith("0"))
          GameTime.Text = time.Substring(4);
        else
          GameTime.Text = time.Substring(3);
        GameTime.Update();
        lblGameTimeTenth.Text = string.Empty;
      }
    }

    private void Basketball_FormClosing(object sender, FormClosingEventArgs e)
    {
      Control.CheckForIllegalCrossThreadCalls = false;
      try
      {
        if (SC_EdgeLightOn.Tag.ToString() != "1")
        {
          SC_EdgeLightOn.Tag = (object) "2";
          SC_EdgeLightOn.Text = "2";
          GameTime.Update();
        }
        _stop_game_time();
        _dbfunc.SaveGameData((Form) this, _act_game.ID);
        _shottime_timer.Dispose();
        _shottime_timer = (DownCounter) null;
        _gametime_timer.Dispose();
        _gametime_timer = (DownCounter) null;
        _dbfunc.SaveGameData("TeamFoulsHome", _team_fouls_home, _act_game.ID);
        _dbfunc.SaveGameData("TeamFoulsGuest", _team_fouls_guest, _act_game.ID);
        _console.Close();
        _sender.Dispose();
        _sender = (UDP_Broadcast_Sender) null;
        Settings.Default.ShowBasketballTimeoutTime = showTimeoutTimeToolStripMenuItem.Checked;
        Settings.Default.Save();
      }
      catch
      {
      }
      Control.CheckForIllegalCrossThreadCalls = true;
    }

    private bool _load_settings(bool IsNewPeriod)
    {
      bool flag = false;
      try
      {
        _presets = _dbfunc.PeriodParameterDictionary(_act_game.GameKindID, _act_game.Period);
        btnReset14.Tag = (object) _presets["ShotTimeShort"].ParameterIntValue;
        btnReset14.Text = "Reset " + (_presets["ShotTimeShort"].ParameterIntValue / 1000L).ToString() + "'";
        btnReset24.Tag = (object) _presets["ShotTimeLong"].ParameterIntValue;
        btnReset24.Text = "Reset " + (_presets["ShotTimeLong"].ParameterIntValue / 1000L).ToString() + "'";
        GameTime.Tag = (object) _presets["PeriodTime"].ParameterIntValue;
        if (IsNewPeriod)
        {
          if (_presets["DoResetTeamFoulsAtPeriodStart"].ParameterIntValue > 0L)
          {
            _team_fouls_home = 0L;
            TeamFoulsHome.ForeColor = Color.Lime;
            TeamFoulsHome.Text = _team_fouls_home.ToString();
            _team_fouls_guest = 0L;
            TeamFoulsGuest.ForeColor = Color.Lime;
            TeamFoulsGuest.Text = _team_fouls_guest.ToString();
          }
          if (_presets["DoResetTimeoutsAtPeriodstart"].ParameterIntValue > 0L)
          {
            TimeOutsHome.Text = string.Empty;
            _timeouts_home = 0L;
            TimeOutsGuest.Text = string.Empty;
            _timeouts_guest = 0L;
          }
        }
        TeamFoulsHome.Tag = (object) _presets["MaxTeamfouls"].ParameterIntValue;
        TeamFoulsGuest.Tag = (object) _presets["MaxTeamfouls"].ParameterIntValue;
        TimeoutTimeHome.Tag = (object) _presets["TimeoutTime"].ParameterIntValue;
        TimeoutTimeGuest.Tag = (object) _presets["TimeoutTime"].ParameterIntValue;
        _max_teamfouls = _presets["MaxTeamfouls"].ParameterIntValue;
        _max_playerfouls = _presets["MaxPlayerFouls"].ParameterIntValue;
        flag = true;
      }
      catch
      {
        int num = (int) MessageBox.Show("Die Spielperiode wurde in den Einstellungen nicht angelegt!", "Fehler:");
      }
      return flag;
    }

    private void TimeTextChanged(object sender, EventArgs e)
    {
      if (_act_game == null)
        return;
      Label label = (Label) sender;
      switch (label.Name)
      {
        case "GameTime":
          if (_gametime_timer == null)
            break;
          _dbfunc.SaveGameData(label.Name, _gametime_timer.Milliseconds, _act_game.ID);
          break;
        case "ShotTime":
          if (_shottime_timer == null)
            break;
          _dbfunc.SaveGameData(label.Name, _shottime_timer.Milliseconds, _act_game.ID);
          break;
      }
    }

    private void SimpleTextChanged(object sender, EventArgs e)
    {
      if (_act_game == null || !((Control) sender).Name.StartsWith("Score") && !((Control) sender).Name.StartsWith("TimeOuts") && !((Control) sender).Name.StartsWith("Period"))
        return;
      if (((Control) sender).Name == "Period")
      {
        if (((Control) sender).Text == "E")
        {
          _dbfunc.SaveGameData("Period", 5L, _act_game.ID);
          _act_game.Period = 5;
        }
        else
        {
          _act_game.Period = Convert.ToInt32(((Control) sender).Text);
          _dbfunc.SaveGameData((Label) sender, _act_game.ID);
        }
      }
      else
      {
        if (((Control) sender).Name == "TimeOutsHome")
          _timeouts_home = (long) ((Control) sender).Text.Length;
        if (((Control) sender).Name == "TimeOutsGuest")
          _timeouts_guest = (long) ((Control) sender).Text.Length;
        _dbfunc.SaveGameData((Label) sender, _act_game.ID);
      }
    }

    private void Fouls_TextChanged(object sender, EventArgs e)
    {
      if (_act_game == null)
        return;
      Label label = (Label) sender;
      try
      {
        if (label.Name.Contains("Team"))
        {
          if (label.Name.Contains("Home"))
          {
            _dbfunc.SaveGameData("TeamFoulsHome", _team_fouls_home, _act_game.ID);
            _sender.IsMaxHomeTeamFoul = _team_fouls_home >= _max_teamfouls;
          }
          if (label.Name.Contains("Guest"))
          {
            _dbfunc.SaveGameData("TeamFoulsGuest", _team_fouls_guest, _act_game.ID);
            _sender.IsMaxGuestTeamFoul = _team_fouls_guest >= _max_teamfouls;
          }
          if ((long) Convert.ToInt32(label.Text) < _max_teamfouls)
            label.ForeColor = Color.Lime;
          else
            label.ForeColor = Color.Red;
        }
        else
        {
          if (label.Text.Trim() != string.Empty)
          {
            if ((long) Convert.ToInt32(label.Text) < _max_playerfouls)
              label.ForeColor = Color.Lime;
            else
              label.ForeColor = Color.Red;
          }
          Label control1 = (Label) Controls["No" + label.Name.Substring(5)];
          Label control2 = (Label) Controls["Name" + label.Name.Substring(5)];
          bool HomeTeam = label.Name.Contains("H");
          int Index = Convert.ToInt32(label.Name.Substring(6)) - 1;
          if (label.Text.Trim() != string.Empty && (long) Convert.ToInt32(label.Text) >= _max_playerfouls)
          {
            control1.ForeColor = Color.Red;
            control2.ForeColor = Color.Red;
            _sender.IsMaxPlayerFoul(HomeTeam, Index, true);
          }
          else
          {
            control1.ForeColor = Color.White;
            control2.ForeColor = Color.White;
            _sender.IsMaxPlayerFoul(HomeTeam, Index, false);
          }
        }
      }
      catch
      {
      }
    }

    private void Label_MouseDown(object sender, MouseEventArgs e)
    {
      _act_label = (Label) sender;
    }

    private void tmsTeamFoulsPlus1_Click(object sender, EventArgs e)
    {
      if (_act_label != null)
      {
        int int32 = Convert.ToInt32(((ToolStripItem) sender).Tag);
        if (_act_label.Name == "TeamFoulsHome")
        {
          if (int32 < 0 && _team_fouls_home > _max_teamfouls)
            _team_fouls_home = _max_teamfouls;
          _team_fouls_home += (long) Convert.ToInt32(((ToolStripItem) sender).Tag);
          if (_team_fouls_home < 0L)
            _team_fouls_home = 0L;
          if (_team_fouls_home <= _max_teamfouls)
            _act_label.Text = _team_fouls_home.ToString();
          else
            _act_label.Text = _max_teamfouls.ToString();
        }
        if (_act_label.Name == "TeamFoulsGuest")
        {
          if (int32 < 0 && _team_fouls_guest > _max_teamfouls)
            _team_fouls_guest = _max_teamfouls;
          _team_fouls_guest += (long) Convert.ToInt32(((ToolStripItem) sender).Tag);
          if (_team_fouls_guest < 0L)
            _team_fouls_guest = 0L;
          if (_team_fouls_guest <= _max_teamfouls)
            _act_label.Text = _team_fouls_guest.ToString();
          else
            _act_label.Text = _max_teamfouls.ToString();
        }
      }
      _act_label = (Label) null;
    }

    private void tmsEditScore_Click(object sender, EventArgs e)
    {
      if (_act_label != null)
      {
        long num = Convert.ToInt64(_act_label.Text) + Convert.ToInt64(((ToolStripItem) sender).Tag);
        if (num > -1L)
          _act_label.Text = num.ToString();
        else
          _act_label.Text = "0";
      }
      _act_label = (Label) null;
    }

    private void tmsEditPeriod_Click(object sender, EventArgs e)
    {
      if (_act_label != null)
      {
        long num1 = 5;
        if (_act_label.Text != "E")
          num1 = Convert.ToInt64(_act_label.Text);
        long num2 = num1 + Convert.ToInt64(((ToolStripItem) sender).Tag);
        if (num2 < 1L)
          num2 = 1L;
        _act_game.Period = (int) num2;
        if (_load_settings(false))
        {
          if (num2 > 4L)
            _act_label.Text = "E";
          else
            _act_label.Text = num2.ToString();
        }
        else
          --_act_game.Period;
      }
      _act_label = (Label) null;
    }

    private void tmsNextPeriod_Click(object sender, EventArgs e)
    {
      if (_act_label != null)
      {
        DialogResult dialogResult = DialogResult.Yes;
        if (_gametime_timer != null && _gametime_timer.Milliseconds > 0L)
          dialogResult = MessageBox.Show(labelChangePeriodAnyway.Text, labelTitlePeriodTimeNotElapsed.Text, MessageBoxButtons.YesNo);
        if (dialogResult == DialogResult.Yes)
        {
          _gametime_run = false;
          btnStartStop.Enabled = true;
          long num1 = 5;
          if (_act_label.Text != "E")
            num1 = Convert.ToInt64(_act_label.Text);
          if (!_countdown_active && _breaktime_timer == null)
            ++num1;
          _StopCountDownPeriod();
          if (_breaktime_timer != null)
          {
            if (_breaktime_timer.IsRunning)
              _breaktime_timer.Stop();
            _breaktime_timer.Dispose();
            _breaktime_timer.TimeChanged -= new TimeChangedDelegate(_breaktime_timer_TimeChanged);
            _breaktime_timer.EndTimeReached -= new EndTimeReachedDelegate(_breaktime_timer_EndTimeReached);
            _breaktime_timer = (DownCounter) null;
          }
          cbShotclockDark.Checked = false;
          _act_game.Period = (int) num1;
          if (_load_settings(true))
          {
            if (num1 > 4L)
              _act_label.Text = "E";
            else
              _act_label.Text = num1.ToString();
            if (_gametime_timer != null)
              _gametime_timer.Milliseconds = _presets["PeriodTime"].ParameterIntValue;
            if (_shottime_timer != null)
              _shottime_timer.Milliseconds = Convert.ToInt64(btnReset24.Tag);
            if (_gametime_timer != null && _shottime_timer != null)
            {
              if (!cbStopShotclock.Checked && _gametime_timer.IsRunning)
                _shottime_timer.Start();
              else
                _shottime_timer.Stop();
            }
          }
          else
            --_act_game.Period;
          tmsPeriodPlus1.Enabled = true;
          tmsPeriodMinus1.Enabled = true;
          while (_console.StatusGameTimeRunning)
          {
            int num2 = (int) MessageBox.Show("Bitte den Schalter für die Spielzeit in Stellung Stop bringen!");
          }
          _invoke.setButtonTextAsync(btnStartStop, "Start");
          _invoke.setLabelColorAsync(GameTime, Color.Red);
        }
      }
      _act_label = (Label) null;
    }

    private void tsmEditTimeout_Click(object sender, EventArgs e)
    {
      if (_act_label != null)
      {
        if (_act_label.Name == "TimeOutsHome" && _timeouts_home <= (long) Convert.ToInt32(_presets["MaxTimeouts"].ParameterIntValue) && _timeouts_home >= 0L)
        {
          while (_console.StatusGameTimeRunning)
          {
            int num1 = (int) MessageBox.Show("Bitte den Schalter für die Spielzeit in Stellung Stop bringen!");
          }
          if (_timeout_timer_home.IsRunning)
            _timeout_timer_home.Stop();
          if (_gametime_timer.IsRunning)
            btnStartStop_Click((object) this, (EventArgs) null);
          if (Convert.ToInt32(((ToolStripItem) sender).Tag) > 0)
            ++_timeouts_home;
          else
            _timeouts_home += (long) Convert.ToInt32(((ToolStripItem) sender).Tag);
          if (_timeouts_home > (long) Convert.ToInt32(_presets["MaxTimeouts"].ParameterIntValue))
          {
            _timeouts_home = (long) Convert.ToInt32(_presets["MaxTimeouts"].ParameterIntValue);
            int num2 = (int) MessageBox.Show("Keine Auszeiten mehr verfügbar!", "Max.Timeouts=" + _timeouts_home.ToString());
          }
          else
          {
            if (_timeouts_home < 0L)
              _timeouts_home = 0L;
            TimeOutsHome.Text = string.Empty.PadLeft((int) _timeouts_home, 'o');
            if (_timeout_timer_home != null)
            {
              _timeout_timer_home.Stop();
              _timeout_timer_home.TimeChanged -= new TimeChangedDelegate(_timeout_timer_home_TimeChanged);
              _timeout_timer_home.EndTimeReached -= new EndTimeReachedDelegate(_timeout_timer_home_EndTimeReached);
              _timeout_timer_home.Dispose();
            }
            if (Convert.ToInt32(((ToolStripItem) sender).Tag) > 0)
            {
              _TimeoutValue = (long) (int) _presets["TimeoutTime"].ParameterIntValue;
              if (Convert.ToInt64(((ToolStripItem) sender).Tag) > 5L)
                _TimeoutValue = 1000L * Convert.ToInt64(((ToolStripItem) sender).Tag);
              _timeout_timer_home = new DownCounter("TimeOutTimerHome", ref TimeoutTimeHome, _TimeoutValue + 999L, false);
              _timeout_timer_home.ShowTimeInSecondsOnly = true;
              _timeout_timer_home.ShowTenth = false;
              _timeout_timer_home.TimeChanged += new TimeChangedDelegate(_timeout_timer_home_TimeChanged);
              _timeout_timer_home.EndTimeReached += new EndTimeReachedDelegate(_timeout_timer_home_EndTimeReached);
              _timeout_timer_home.Start();
            }
            else
              TimeoutTimeHome.Text = string.Empty;
          }
        }
        if (_act_label.Name == "TimeOutsGuest" && _timeouts_guest <= (long) Convert.ToInt32(_presets["MaxTimeouts"].ParameterIntValue) && _timeouts_guest >= 0L)
        {
          while (_console.StatusGameTimeRunning)
          {
            int num1 = (int) MessageBox.Show("Bitte den Schalter für die Spielzeit in Stellung Stop bringen!");
          }
          if (_timeout_timer_guest.IsRunning)
            _timeout_timer_guest.Stop();
          if (_gametime_timer.IsRunning)
            btnStartStop_Click((object) this, (EventArgs) null);
          if (Convert.ToInt32(((ToolStripItem) sender).Tag) > 0)
            ++_timeouts_guest;
          else
            _timeouts_guest += (long) Convert.ToInt32(((ToolStripItem) sender).Tag);
          if (_timeouts_guest > (long) Convert.ToInt32(_presets["MaxTimeouts"].ParameterIntValue))
          {
            _timeouts_guest = (long) Convert.ToInt32(_presets["MaxTimeouts"].ParameterIntValue);
            int num2 = (int) MessageBox.Show("Keine Auszeiten mehr verfügbar!", "Max.Timeouts=" + _timeouts_guest.ToString());
          }
          else
          {
            if (_timeouts_guest < 0L)
              _timeouts_guest = 0L;
            TimeOutsGuest.Text = string.Empty.PadLeft((int) _timeouts_guest, 'o');
            if (_timeout_timer_guest != null)
            {
              _timeout_timer_guest.Stop();
              _timeout_timer_guest.TimeChanged -= new TimeChangedDelegate(_timeout_timer_guest_TimeChanged);
              _timeout_timer_guest.EndTimeReached -= new EndTimeReachedDelegate(_timeout_timer_guest_EndTimeReached);
              _timeout_timer_guest.Dispose();
            }
            if (Convert.ToInt64(((ToolStripItem) sender).Tag) > 0L)
            {
              _TimeoutValue = (long) (int) _presets["TimeoutTime"].ParameterIntValue;
              if (Convert.ToInt64(((ToolStripItem) sender).Tag) > 5L)
                _TimeoutValue = 1000L * Convert.ToInt64(((ToolStripItem) sender).Tag);
              _timeout_timer_guest = new DownCounter("TimeOutTimerGuest", ref TimeoutTimeGuest, _TimeoutValue + 999L, false);
              _timeout_timer_guest.ShowTimeInSecondsOnly = true;
              _timeout_timer_guest.ShowTenth = false;
              _timeout_timer_guest.TimeChanged += new TimeChangedDelegate(_timeout_timer_guest_TimeChanged);
              _timeout_timer_guest.EndTimeReached += new EndTimeReachedDelegate(_timeout_timer_guest_EndTimeReached);
              _timeout_timer_guest.Start();
            }
            else
              TimeoutTimeGuest.Text = string.Empty;
          }
        }
      }
      _act_label = (Label) null;
    }

    private void tmsEditPlayerPoints_Click(object sender, EventArgs e)
    {
      if (_act_player != null)
      {
        _act_player.Points += Convert.ToInt32(((ToolStripItem) sender).Tag);
        if (_act_player.Points < 0)
        {
          _act_player.Points = 0;
        }
        else
        {
          if (_act_player.TeamID == _act_game.HomeTeamID)
          {
            long num = Convert.ToInt64(ScoreHome.Text) + (long) Convert.ToInt32(((ToolStripItem) sender).Tag);
            if (num > -1L)
              ScoreHome.Text = num.ToString();
            else
              ScoreHome.Text = "0";
          }
          if (_act_player.TeamID == _act_game.GuestTeamID)
          {
            long num = Convert.ToInt64(ScoreGuest.Text) + (long) Convert.ToInt32(((ToolStripItem) sender).Tag);
            if (num > -1L)
              ScoreGuest.Text = num.ToString();
            else
              ScoreGuest.Text = "0";
          }
          _dbfunc.SaveGamePlayer(_act_player);
          _dbfunc.ShowGamePlayers((Form) this, _game_player_home, _game_player_guest, _max_playerfouls);
        }
      }
      _act_player = (GamePlayer) null;
    }

    private void tmsPlayerCorrPoints_Click(object sender, EventArgs e)
    {
      if (_act_player != null)
      {
        if (_act_player != null)
        {
          _act_player.Points += Convert.ToInt32(((ToolStripItem) sender).Tag);
          if (_act_player.Points < 0)
            _act_player.Points = 0;
        }
        _dbfunc.SaveGamePlayer(_act_player);
        _dbfunc.ShowGamePlayers((Form) this, _game_player_home, _game_player_guest, _max_playerfouls);
      }
      _act_player = (GamePlayer) null;
    }

    private void tmsEditPlayerFouls_Click(object sender, EventArgs e)
    {
      if (_act_label == null)
        return;
      if (_act_player != null)
      {
        int num = _act_player.Fouls + Convert.ToInt32(((ToolStripItem) sender).Tag);
        if (num < 0)
          num = 0;
        if ((long) num <= _max_playerfouls)
        {
          _act_player.Fouls = num;
          _dbfunc.SaveGamePlayer(_act_player);
          _dbfunc.ShowGamePlayers((Form) this, _game_player_home, _game_player_guest, _max_playerfouls);
          if (_act_player.TeamID == _act_game.HomeTeamID)
          {
            _team_fouls_home += (long) Convert.ToInt32(((ToolStripItem) sender).Tag);
            if (_team_fouls_home < 0L)
              _team_fouls_home = 0L;
            _dbfunc.SaveGameData("TeamFoulsHome", _team_fouls_home, _act_game.ID);
            if (_team_fouls_home <= _max_teamfouls)
              TeamFoulsHome.Text = _team_fouls_home.ToString();
            else
              TeamFoulsHome.Text = _max_teamfouls.ToString();
          }
          if (_act_player.TeamID == _act_game.GuestTeamID)
          {
            _team_fouls_guest += (long) Convert.ToInt32(((ToolStripItem) sender).Tag);
            if (_team_fouls_guest < 0L)
              _team_fouls_guest = 0L;
            _dbfunc.SaveGameData("TeamFoulsGuest", _team_fouls_guest, _act_game.ID);
            if (_team_fouls_guest <= _max_teamfouls)
              TeamFoulsGuest.Text = _team_fouls_guest.ToString();
            else
              TeamFoulsGuest.Text = _max_teamfouls.ToString();
          }
          if (Convert.ToInt32(((ToolStripItem) sender).Tag) > 0)
          {
            ActFoulPlayer.Text = _act_player.PlayerNumber.ToString().PadLeft(2) + " - " + _act_player.Fouls.ToString();
            if (_act_player.TeamID == _act_game.HomeTeamID)
            {
              ActFoulHome.Text = _act_player.Fouls.ToString();
              ActFoulPlayerNumberHome.Text = _act_player.PlayerNumber.ToString().PadLeft(2);
            }
            if (_act_player.TeamID == _act_game.GuestTeamID)
            {
              ActFoulGuest.Text = _act_player.Fouls.ToString();
              ActFoulPlayerNumberGuest.Text = _act_player.PlayerNumber.ToString().PadLeft(2);
            }
            actFoulPlayerTimer.Interval = 10000;
            actFoulPlayerTimer.Start();
          }
        }
      }
      _act_player = (GamePlayer) null;
      _act_label = (Label) null;
    }

    private void tmsPlayerEdit_Click(object sender, EventArgs e)
    {
      if (_act_player != null)
      {
        _dbfunc.DeleteGamePlayer(_act_player);
      }
      else
      {
        _act_player = !_act_label.Name.Contains("H") ? new GamePlayer(_act_game.ID, _act_game.GuestTeamID, 0, string.Empty, 0, 0, false, 0, false, false) : new GamePlayer(_act_game.ID, _act_game.HomeTeamID, 0, string.Empty, 0, 0, false, 0, false, false);
        _act_player.DoBlink += new BlinkDelegate(Fouls_DoBlink);
      }
      PlayerEditForm playerEditForm = new PlayerEditForm(_act_player, (int) _max_playerfouls);
      playerEditForm.Size = new Size(424, 116);
      int num = (int) playerEditForm.ShowDialog();
      if (playerEditForm.Player != null)
        _dbfunc.SaveGamePlayer(playerEditForm.Player);
      _game_player_home = _dbfunc.GamePlayers(_act_game.ID, _act_game.HomeTeamID);
      _game_player_guest = _dbfunc.GamePlayers(_act_game.ID, _act_game.GuestTeamID);
      _dbfunc.ClearGamePlayersList((Form) this, _game_player_home, _game_player_guest, 15L);
      _dbfunc.ShowGamePlayers((Form) this, _game_player_home, _game_player_guest, _max_playerfouls);
      Fouls_TextChanged((object) Controls["TeamFoulsHome"], (EventArgs) null);
      Fouls_TextChanged((object) Controls["TeamFoulsGuest"], (EventArgs) null);
      _act_player = (GamePlayer) null;
      _act_label = (Label) null;
      for (int index = 0; index < 15; ++index)
      {
        if (index < _game_player_home.Count)
        {
          try
          {
            _game_player_home[index].DoBlink -= new BlinkDelegate(Fouls_DoBlink);
          }
          catch
          {
          }
          _game_player_home[index].DoBlink += new BlinkDelegate(Fouls_DoBlink);
          _game_player_home[index].DontBlink();
        }
        if (index < _game_player_guest.Count)
        {
          try
          {
            _game_player_guest[index].DoBlink -= new BlinkDelegate(Fouls_DoBlink);
          }
          catch
          {
          }
          _game_player_guest[index].DoBlink += new BlinkDelegate(Fouls_DoBlink);
          _game_player_guest[index].DontBlink();
        }
      }
    }

    private void Player_MouseDown(object sender, MouseEventArgs e)
    {
      _act_label = (Label) sender;
      try
      {
        if (((Control) sender).Name.Contains("H"))
          _act_player = _game_player_home[Convert.ToInt32(((Control) sender).Tag)];
        if (!((Control) sender).Name.Contains("G"))
          return;
        _act_player = _game_player_guest[Convert.ToInt32(((Control) sender).Tag)];
      }
      catch
      {
        _act_player = (GamePlayer) null;
      }
    }

    private void _start_game_time()
    {
      _invoke.setLabelTextAsync(TimeRunning, "0");
      HornTimer.Tag = (object) "0";
      if (_breaktime_timer != null)
      {
        _gametime_timer.Stop();
        if (!_breaktime_timer.IsRunning)
          _breaktime_timer.Start();
        if (!_breaktime_timer.IsRunning)
          return;
        _invoke.setButtonTextAsync(btnStartStop, "Stop");
        _invoke.setLabelColorAsync(GameTime, Color.Lime);
        if (ShotTime.Text != string.Empty && ShotTime.Text.Trim() != "0" && (!cbShotclockDark.Checked && !cbStopShotclock.Checked) && (!_console.StatusShotClockReset14Pressed && !_console.StatusShotClockReset24Pressed))
          _shottime_timer.Start();
        _invoke.setLabelTextAsync(TimeRunning, "0");
      }
      else
      {
        if (_timeout_timer_home.IsRunning)
          _timeout_timer_home.Stop();
        _invoke.setLabelTextAsync(TimeoutTimeHome, string.Empty);
        if (_timeout_timer_guest.IsRunning)
          _timeout_timer_guest.Stop();
        _invoke.setLabelTextAsync(TimeoutTimeGuest, string.Empty);
        if (!_gametime_timer.IsRunning)
        {
          _gametime_timer.Start();
          if (_gametime_timer.IsRunning)
          {
            if (ShotTime.Text != string.Empty && ShotTime.Text.Trim() != "0" && (!cbShotclockDark.Checked && !cbStopShotclock.Checked) && (!_console.StatusShotClockReset14Pressed && !_console.StatusShotClockReset24Pressed))
              _shottime_timer.Start();
            _sender.GameTimeRunning = true;
            _invoke.setButtonTextAsync(btnStartStop, "Stop");
            _invoke.setLabelColorAsync(GameTime, Color.Lime);
            if (ServiceHome.Text != string.Empty)
            {
              int num = ServiceGuest.Text != string.Empty ? 1 : 0;
            }
          }
        }
        Tag = (object) (_gametime_timer.IsRunning ? 1 : 0);
        _invoke.setLabelTextAsync(TimeRunning, _gametime_timer.IsRunning ? "1" : "0");
        _invoke.setLabelTextAsync(GameTimeStopped, _gametime_timer.IsRunning ? " " : GameTimeStopped.Tag.ToString());
      }
    }

    private void _stop_game_time()
    {
      try
      {
        if (_breaktime_timer != null)
        {
          if (_breaktime_timer.IsRunning)
            _breaktime_timer.Stop();
          _invoke.setButtonTextAsync(btnStartStop, "Start");
          _invoke.setLabelColorAsync(GameTime, Color.Red);
        }
        else
        {
          if (_gametime_timer.IsRunning)
          {
            _gametime_timer.Stop();
            _sender.GameTimeRunning = false;
            if (_timeout_timer_home != null)
              _timeout_timer_home.Stop();
            if (_timeout_timer_guest != null)
              _timeout_timer_guest.Stop();
            TimeTextChanged((object) GameTime, (EventArgs) null);
            _invoke.setButtonTextAsync(btnStartStop, "Start");
            _invoke.setLabelColorAsync(GameTime, Color.Red);
          }
          Tag = (object) 0;
        }
        if (Tag != null)
          _invoke.setLabelTextAsync(TimeRunning, Tag.ToString());
      }
      catch
      {
      }
      if (_shottime_timer == null || !_shottime_timer.IsRunning)
        return;
      _shottime_timer.Stop();
    }

    private void btnStartStop_Click(object sender, EventArgs e)
    {
      if (btnStartStop.Text == "Start")
        _start_game_time();
      else
        _stop_game_time();
      _gametime_run = _gametime_timer.IsRunning;
    }

    private void cmsEditTime_Opening(object sender, CancelEventArgs e)
    {
      tsmEditGameTime.Enabled = !_gametime_timer.IsRunning;
    }

    private void tsmEditGameTime_Click(object sender, EventArgs e)
    {
      if (_breaktime_timer != null)
        _edit_break_time();
      else
        _edit_game_and_shot_time();
    }

    private void _edit_break_time()
    {
      _breaktime_timer.Stop();
      EditBreaktime editBreaktime = new EditBreaktime(_breaktime_timer.Milliseconds);
      int num = (int) editBreaktime.ShowDialog();
      _breaktime_timer.Milliseconds = editBreaktime.BreakMilliSeconds;
      _breaktime_timer.Start();
    }

    private void _edit_game_and_shot_time()
    {
      EditTime editTime = new EditTime(_gametime_timer.Milliseconds, _shottime_timer.Milliseconds);
      int num = (int) editTime.ShowDialog();
      _gametime_timer.Milliseconds = editTime.GameMilliSeconds;
      _shottime_timer.Milliseconds = editTime.ShotMilliseconds;
      if (!cbShotclockDark.Checked)
      {
        if (_gametime_timer.Milliseconds / 100L >= _shottime_timer.Milliseconds / 100L)
        {
          if (_gametime_timer.Milliseconds / 1000L != _shottime_timer.Milliseconds / 1000L)
          {
              ShotTime.Text = (_shottime_timer.Milliseconds / 1000L).ToString();
          }
          else
          {
              ShotTime.Text = (_gametime_timer.Milliseconds / 1000L).ToString();
          }
        }
        else
        {
            _shottime_timer.Stop();
          ShotTime.Text = string.Empty;
          _invoke.setLabelTextAsync(GameTimeStopped, " ");
        }
      }
      else
      {
        ShotTime.Text = string.Empty;
        _invoke.setLabelTextAsync(GameTimeStopped, " ");
      }
    }

    private void cmsEditTimeouts_Opening(object sender, CancelEventArgs e)
    {
      tsmTimeoutPlus1.Enabled = !_gametime_timer.IsRunning;
    }

    private void cbStopShotclock_CheckedChanged(object sender, EventArgs e)
    {
      if (cbStopShotclock.Checked)
      {
        cbStopShotclock.Text = "Angriffszeit steht";
        if (!_shottime_timer.IsRunning)
          return;
        _shottime_timer.Stop();
      }
      else
      {
        cbStopShotclock.Text = "Angriffszeit läuft";
        if (_breaktime_timer != null)
        {
          if ((!_breaktime_timer.IsRunning || _shottime_timer.Milliseconds <= 0L) && (!_countdown_active || _shottime_timer.Milliseconds <= 0L))
            return;
          _shottime_timer.Start();
        }
        else
        {
          if (!_gametime_timer.IsRunning && !cbShotclockIndependentStart.Checked || _shottime_timer.Milliseconds <= 0L)
            return;
          _shottime_timer.Start();
        }
      }
    }

    private void btnResetShotclock_MouseDown(object sender, MouseEventArgs e)
    {
      SC_Horn.Text = "Off";
      if (_shottime_timer.IsRunning)
        _shottime_timer.Stop();
      ShotTime.Text = string.Empty;
      lblShotclockTenth.Text = string.Empty;
      _invoke.setLabelTextAsync(GameTimeStopped, " ");
    }

    private void btnResetShotclock_MouseUp(object sender, MouseEventArgs e)
    {
      if (!_console.StatusShotClockReset14Pressed && !_console.StatusShotClockReset24Pressed)
      {
        Button button = (Button) sender;
        if (_breaktime_timer != null)
        {
          if (_breaktime_timer.Milliseconds / 1000L >= Convert.ToInt64(button.Tag) / 1000L)
          {
            _shottime_timer.Milliseconds = Convert.ToInt64(button.Tag);
            if (!cbStopShotclock.Checked && _breaktime_timer.IsRunning || !cbStopShotclock.Checked && _countdown_active)
            {
              _shottime_timer.Stop();
              _shottime_timer.StartIndependendFromActTime();
            }
            else
              _shottime_timer.Stop();
            _invoke.setCheckBoxCheckedAsync(cbShotclockDark, false);
          }
          else
          {
            _shottime_timer.Stop();
            _invoke.setCheckBoxCheckedAsync(cbShotclockDark, true);
          }
        }
        else if (_gametime_timer.Milliseconds / 1000L >= Convert.ToInt64(button.Tag) / 1000L)
        {
          _shottime_timer.Milliseconds = Convert.ToInt64(button.Tag);
          _invoke.setCheckBoxCheckedAsync(cbShotclockDark, false);
          Console.WriteLine(_console.StatusShotClockRunning.ToString());
          if ((_gametime_timer.IsRunning || cbShotclockIndependentStart.Checked) && (!cbStopShotclock.Checked && !_console.StatusShotClockRunning))
          {
            _shottime_timer.Stop();
            _shottime_timer.StartIndependendFromActTime();
          }
          _invoke.setLabelTextAsync(ShotTime, (Convert.ToInt64(button.Tag) / 1000L).ToString());
          _invoke.setLabelTextAsync(lblShotclockTenth, "9");
        }
        else
        {
            _shottime_timer.Stop();
          _invoke.setCheckBoxCheckedAsync(cbShotclockDark, true);
        }
      }
      _invoke.setLabelTextAsync(SC_Horn, "Off");
      _invoke.setLabelTextAsync(GameTimeStopped, " ");
    }

    private void Player_MouseEnter(object sender, EventArgs e)
    {
      Label label = (Label) sender;
      Label[] TargetLabel = new Label[4];
      string str = string.Empty;
      if (label.Name.Contains("H"))
        str = label.Name.Substring(label.Name.LastIndexOf('H'));
      if (label.Name.Contains("G"))
        str = label.Name.Substring(label.Name.LastIndexOf('G'));
      TargetLabel[0] = (Label) Controls["No" + str];
      TargetLabel[1] = (Label) Controls["Name" + str];
      TargetLabel[2] = (Label) Controls["Points" + str];
      TargetLabel[3] = (Label) Controls["Fouls" + str];
      _f._show_highlighted_selection(TargetLabel);
      _f._show_edged_selection((Form) this, TargetLabel);
    }

    private void Player_MouseLeave(object sender, EventArgs e)
    {
      Label label = (Label) sender;
      Label[] TargetLabel = new Label[4];
      string str = string.Empty;
      if (label.Name.Contains("H"))
        str = label.Name.Substring(label.Name.LastIndexOf('H'));
      if (label.Name.Contains("G"))
        str = label.Name.Substring(label.Name.LastIndexOf('G'));
      TargetLabel[0] = (Label) Controls["No" + str];
      TargetLabel[1] = (Label) Controls["Name" + str];
      TargetLabel[2] = (Label) Controls["Points" + str];
      TargetLabel[3] = (Label) Controls["Fouls" + str];
      _f._reset_highlighted_selection(TargetLabel);
      _f._reset_edged_selection((Form) this);
    }

    private void Label_MouseEnter(object sender, EventArgs e)
    {
      Label[] TargetLabel = new Label[1]
      {
        (Label) sender
      };
      _f._show_highlighted_selection(TargetLabel);
      _f._show_edged_selection((Form) this, TargetLabel);
    }

    private void Label_MouseLeave(object sender, EventArgs e)
    {
      _f._reset_highlighted_selection(new Label[1]
      {
        (Label) sender
      });
      _f._reset_edged_selection((Form) this);
    }

    private void cmsPlayerData_Opening(object sender, CancelEventArgs e)
    {
      if (_act_player != null)
      {
        tsmPlayerName.Visible = true;
        tmsPlayerPointsPlus3.Visible = true;
        tmsPlayerPointsPlus2.Visible = true;
        tmsPlayerPointsPlus1.Visible = true;
        tmsPlayerPointsMinus1.Visible = true;
        tmsPlayerCorrPointsPlus1.Visible = true;
        tmsPlayerCorrPointsMinus1.Visible = true;
        tmsPlayerFoulsPlus1.Visible = true;
        tmsPlayerFoulsMinus1.Visible = true;
        toolStripSeparator1.Visible = true;
        toolStripSeparator3.Visible = true;
        toolStripSeparator4.Visible = true;
        toolStripSeparator5.Visible = true;
        tsmPlayerName.Text = _act_player.PlayerNumber.ToString() + ", " + _act_player.Name;
      }
      else
      {
        tsmPlayerName.Visible = false;
        tmsPlayerPointsPlus3.Visible = false;
        tmsPlayerPointsPlus2.Visible = false;
        tmsPlayerPointsPlus1.Visible = false;
        tmsPlayerPointsMinus1.Visible = false;
        tmsPlayerCorrPointsPlus1.Visible = false;
        tmsPlayerCorrPointsMinus1.Visible = false;
        tmsPlayerFoulsPlus1.Visible = false;
        tmsPlayerFoulsMinus1.Visible = false;
        toolStripSeparator1.Visible = false;
        toolStripSeparator3.Visible = false;
        toolStripSeparator4.Visible = false;
        toolStripSeparator5.Visible = false;
      }
    }

    private void btnHorn_MouseDown(object sender, MouseEventArgs e)
    {
      HornTimer.Stop();
      HornTimer.Tag = (object) "0";
      Horn.Text = "On";
    }

    private void btnHorn_MouseUp(object sender, MouseEventArgs e)
    {
      HornTimer.Stop();
      HornTimer.Tag = (object) "0";
      Horn.Text = "Off";
    }

    private void cbShotclockDark_CheckedChanged(object sender, EventArgs e)
    {
      if (!cbShotclockDark.Checked && _shottime_timer != null)
      {
        ShotTime.Text = (_shottime_timer.Milliseconds / 1000L).ToString();
        Label lblShotclockTenth1 = lblShotclockTenth;
        Label lblShotclockTenth2 = lblShotclockTenth;
        long num = _shottime_timer.Milliseconds % 1000L / 100L;
        string str1;
        string str2 = str1 = num.ToString();
        lblShotclockTenth2.Text = str1;
        string str3 = str2;
        lblShotclockTenth1.Text = str3;
      }
      else
      {
        ShotTime.Text = string.Empty;
        lblShotclockTenth.Text = string.Empty;
        _invoke.setLabelTextAsync(GameTimeStopped, " ");
      }
    }

    private void HornTimer_Tick(object sender, EventArgs e)
    {
      HornTimer.Stop();
      HornTimer.Tag = (object) "0";
      Horn.Text = "Off";
      SC_Horn.Text = "Off";
      _invoke.setLabelTextAsync(GameTimeStopped, " ");
    }

    private void actFoulPlayerTimer_Tick(object sender, EventArgs e)
    {
      ActFoulPlayer.Text = string.Empty;
      ActFoulPlayerNumberHome.Text = string.Empty;
      ActFoulPlayerNumberGuest.Text = string.Empty;
      ActFoulHome.Text = string.Empty;
      ActFoulGuest.Text = string.Empty;
      actFoulPlayerTimer.Stop();
    }

    public void clickStartStopButtonAsync()
    {
      if (btnStartStop.InvokeRequired)
        btnStartStop.Invoke((Delegate) new Basketball.clickStartStopButtonAsyncDelegate(clickStartStopButtonAsync), new object[0]);
      else
        btnStartStop_Click((object) this, (EventArgs) null);
    }

    private void ServiceGuest_Click(object sender, EventArgs e)
    {
      if ((Label) sender == ServiceHome)
      {
        if (ServiceHome.Text == ServiceHomeSymbol)
          ServiceHome.Text = string.Empty;
        else
          ServiceHome.Text = ServiceHomeSymbol;
      }
      if ((Label) sender != ServiceGuest)
        return;
      if (ServiceGuest.Text == ServiceGuestSymbol)
        ServiceGuest.Text = string.Empty;
      else
        ServiceGuest.Text = ServiceGuestSymbol;
    }

    private void cmsEditPeriod_Opening_1(object sender, CancelEventArgs e)
    {
      tmsNextPeriod.Enabled = !_gametime_timer.IsRunning;
      tmsStartBreakTime.Enabled = !_gametime_timer.IsRunning && _breaktime_timer == null;
      tmsPeriodPlus1.Enabled = !_gametime_timer.IsRunning && _breaktime_timer != null;
      tmsPeriodMinus1.Enabled = !_gametime_timer.IsRunning && _breaktime_timer != null;
    }

    private void tmsStartBreakTime_Click(object sender, EventArgs e)
    {
      DialogResult dialogResult = DialogResult.Yes;
      if (_gametime_timer.Milliseconds > 0L)
        dialogResult = MessageBox.Show(labelStartBreaktimeAnyway.Text, labelTitlePeriodTimeNotElapsed.Text, MessageBoxButtons.YesNo);
      if (dialogResult != DialogResult.Yes)
        return;
      cbShotclockDark.Checked = true;
      _gametime_timer.Milliseconds = 0L;
      if (_breaktime_timer != null)
        _breaktime_timer.Dispose();
      _breaktime_timer = (DownCounter) null;
      _breaktime_timer = new DownCounter("BreakTimer", ref GameTime, _presets["BreakTime"].ParameterIntValue + 999L, true);
      _breaktime_timer.TimeChanged += new TimeChangedDelegate(_breaktime_timer_TimeChanged);
      _breaktime_timer.EndTimeReached += new EndTimeReachedDelegate(_breaktime_timer_EndTimeReached);
      _breaktime_timer.Start();
      long num1 = 5;
      if (_act_label.Text != "E")
        num1 = Convert.ToInt64(_act_label.Text);
      long num2 = num1 + 1L;
      _StopCountDownPeriod();
      _act_game.Period = (int) num2;
      if (_load_settings(true))
      {
        if (num2 > 4L)
          Period.Text = "E";
        else
          Period.Text = num2.ToString();
      }
      else
        --_act_game.Period;
      _team_fouls_home = 0L;
      _team_fouls_guest = 0L;
      TeamFoulsHome.Text = "0";
      TeamFoulsGuest.Text = "0";
    }

    private void Basketball_Shown(object sender, EventArgs e)
    {
      showTimeoutTimeToolStripMenuItem.Checked = Settings.Default.ShowBasketballTimeoutTime;
      if (_act_game.Period != 1)
        return;
      if (MessageBox.Show("Start countdown ?", "Countdown", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        if (!_console.StatusGameTimeRunning)
        {
          int num = (int) MessageBox.Show("Bitte Schalter für Spielzeit in Stellung 'Start' bringen!");
        }
        cbShotclockDark.Checked = false;
        _gametime_timer.Stop();
        _gametime_timer.Milliseconds = 0L;
        _breaktime_timer = new DownCounter("BreakTimer", ref GameTime, Settings.Default.CountdownMinutesBeforeGame, false);
        _breaktime_timer.TimeChanged += new TimeChangedDelegate(_breaktime_timer_TimeChanged);
        _breaktime_timer.EndTimeReached += new EndTimeReachedDelegate(_breaktime_timer_EndTimeReached);
        _breaktime_timer.Milliseconds += 999L;
        _breaktime_timer.Start();
        _countdown_active = true;
        _invoke.setButtonTextAsync(btnStartStop, "Stop");
        _invoke.setLabelColorAsync(GameTime, Color.Lime);
      }
      else
      {
        if (_breaktime_timer == null)
          return;
        _breaktime_timer.Dispose();
        _breaktime_timer = (DownCounter) null;
      }
    }

    private void tmsScoreDirectInput_Click(object sender, EventArgs e)
    {
      if (_act_label == ScoreHome)
      {
        ScoreInput scoreInput = new ScoreInput(Convert.ToInt32(ScoreHome.Text));
        int num = (int) scoreInput.ShowDialog();
        ScoreHome.Text = scoreInput.Score.ToString();
        scoreInput.Dispose();
      }
      if (_act_label != ScoreGuest)
        return;
      ScoreInput scoreInput1 = new ScoreInput(Convert.ToInt32(ScoreGuest.Text));
      int num1 = (int) scoreInput1.ShowDialog();
      ScoreGuest.Text = scoreInput1.Score.ToString();
      scoreInput1.Dispose();
    }

    private void showTimeoutTimeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      showTimeoutTimeToolStripMenuItem.Checked = !showTimeoutTimeToolStripMenuItem.Checked;
      Settings.Default.ShowBasketballTimeoutTime = showTimeoutTimeToolStripMenuItem.Checked;
      Settings.Default.Save();
    }

    private void cbEdgeLightAtShotclockZero_CheckedChanged(object sender, EventArgs e)
    {
      SC_EdgeLightOn.Text = cbEdgeLightAtShotclockZero.Checked ? "2" : "1";
      SC_EdgeLightOn.Tag = (object) SC_EdgeLightOn.Text;
    }

    private void btnLeft_Click(object sender, EventArgs e)
    {
      ServiceHomeSymbol = "<";
      ServiceGuestSymbol = ServiceGuest.Text.Length <= 0 ? "" : "";
      ServiceHome.Text = ServiceHomeSymbol;
      ServiceGuest.Text = ServiceGuestSymbol;
    }

    private void btnRight_Click(object sender, EventArgs e)
    {
      ServiceGuestSymbol = ">";
      ServiceHomeSymbol = ServiceHome.Text.Length <= 0 ? "" : "";
      ServiceHome.Text = ServiceHomeSymbol;
      ServiceGuest.Text = ServiceGuestSymbol;
    }

    private void cmsEditTimeouts_Opening_1(object sender, CancelEventArgs e)
    {
    }

    private void tsmTimeoutPlus60s_Click(object sender, EventArgs e)
    {
    }

    private void BtnAbortStartPause_Click(object sender, EventArgs e)
    {
      GBPause.Visible = false;
      if (!_console.StatusGameTimeRunning)
        return;
      int num = (int) MessageBox.Show("Bitte den Schalter für die Spielzeit in Stellung Stop bringen!");
    }

    private void BtnStartPause_Click(object sender, EventArgs e)
    {
      GBPause.Visible = false;
      _act_label = Period;
      cbShotclockDark.Checked = true;
      _gametime_timer.Milliseconds = 0L;
      if (_breaktime_timer != null)
        _breaktime_timer.Dispose();
      _breaktime_timer = (DownCounter) null;
      _breaktime_timer = new DownCounter("BreakTimer", ref GameTime, _presets["BreakTime"].ParameterIntValue + 999L, true);
      _breaktime_timer.TimeChanged += new TimeChangedDelegate(_breaktime_timer_TimeChanged);
      _breaktime_timer.EndTimeReached += new EndTimeReachedDelegate(_breaktime_timer_EndTimeReached);
      _breaktime_timer.Start();
      _invoke.setLabelColorAsync(GameTime, Color.Lime);
      long num1 = 5;
      if (_act_label.Text != "E")
        num1 = Convert.ToInt64(_act_label.Text);
      long num2 = num1 + 1L;
      _countdown_active = false;
      _act_game.Period = (int) num2;
      if (_load_settings(true))
      {
        if (num2 > 4L)
          Period.Text = "E";
        else
          Period.Text = num2.ToString();
      }
      else
        --_act_game.Period;
      if (_console.StatusGameTimeRunning)
        return;
      int num3 = (int) MessageBox.Show("Bitte den Schalter für die Spielzeit in Stellung Start bringen!");
    }

    private void btnReset24_Click(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Basketball));
      label4 = new Label();
      TimeOutsHome = new Label();
      cmsEditTimeouts = new ContextMenuStrip(components);
      tsmTimeoutPlus1 = new ToolStripMenuItem();
      tmsTimeoutMinus1 = new ToolStripMenuItem();
      toolStripSeparator8 = new ToolStripSeparator();
      tsmTimeoutPlus60s = new ToolStripMenuItem();
      tsmTimeoutPlus30s = new ToolStripMenuItem();
      toolStripSeparator10 = new ToolStripSeparator();
      showTimeoutTimeToolStripMenuItem = new ToolStripMenuItem();
      TimeOutsGuest = new Label();
      cbStopShotclock = new CheckBox();
      Period = new Label();
      cmsEditPeriod = new ContextMenuStrip(components);
      tmsNextPeriod = new ToolStripMenuItem();
      toolStripSeparator2 = new ToolStripSeparator();
      tmsStartBreakTime = new ToolStripMenuItem();
      toolStripSeparator6 = new ToolStripSeparator();
      tmsPeriodPlus1 = new ToolStripMenuItem();
      tmsPeriodMinus1 = new ToolStripMenuItem();
      btnReset24 = new Button();
      btnReset14 = new Button();
      TeamNameHome = new Label();
      ShotTime = new Label();
      cmsEditTime = new ContextMenuStrip(components);
      tsmEditGameTime = new ToolStripMenuItem();
      btnStartStop = new Button();
      GameTime = new Label();
      TeamFoulsGuest = new Label();
      cmsEditTeamFouls = new ContextMenuStrip(components);
      tmsTeamFoulsPlus1 = new ToolStripMenuItem();
      tmsTeamFoulsMinus1 = new ToolStripMenuItem();
      TeamFoulsHome = new Label();
      FoulsG15 = new Label();
      cmsPlayerData = new ContextMenuStrip(components);
      tsmPlayerName = new ToolStripMenuItem();
      toolStripSeparator5 = new ToolStripSeparator();
      tmsPlayerPointsPlus3 = new ToolStripMenuItem();
      tmsPlayerPointsPlus2 = new ToolStripMenuItem();
      tmsPlayerPointsPlus1 = new ToolStripMenuItem();
      tmsPlayerPointsMinus1 = new ToolStripMenuItem();
      toolStripSeparator1 = new ToolStripSeparator();
      tmsPlayerCorrPointsPlus1 = new ToolStripMenuItem();
      tmsPlayerCorrPointsMinus1 = new ToolStripMenuItem();
      toolStripSeparator4 = new ToolStripSeparator();
      tmsPlayerFoulsPlus1 = new ToolStripMenuItem();
      tmsPlayerFoulsMinus1 = new ToolStripMenuItem();
      toolStripSeparator3 = new ToolStripSeparator();
      tmsPlayerEdit = new ToolStripMenuItem();
      PointsG15 = new Label();
      NameG15 = new Label();
      NoG15 = new Label();
      FoulsG14 = new Label();
      ScoreGuest = new Label();
      cmsEditScore = new ContextMenuStrip(components);
      tmsPointsPlus3 = new ToolStripMenuItem();
      tmsPointsPlus2 = new ToolStripMenuItem();
      tmsScorePlus1 = new ToolStripMenuItem();
      toolStripSeparator9 = new ToolStripSeparator();
      tmsScoreMinus1 = new ToolStripMenuItem();
      toolStripSeparator7 = new ToolStripSeparator();
      tmsScoreDirectInput = new ToolStripMenuItem();
      TeamNameGuest = new Label();
      PointsG14 = new Label();
      NameG14 = new Label();
      ScoreHome = new Label();
      NoG14 = new Label();
      FoulsG13 = new Label();
      PointsG13 = new Label();
      NameG13 = new Label();
      NoG13 = new Label();
      FoulsG12 = new Label();
      PointsG12 = new Label();
      NameG12 = new Label();
      NoG12 = new Label();
      FoulsG11 = new Label();
      PointsG11 = new Label();
      NameG11 = new Label();
      NoG11 = new Label();
      FoulsG10 = new Label();
      PointsG10 = new Label();
      NameG10 = new Label();
      NoG10 = new Label();
      FoulsG9 = new Label();
      PointsG9 = new Label();
      NameG9 = new Label();
      NoG9 = new Label();
      FoulsG8 = new Label();
      PointsG8 = new Label();
      NameG8 = new Label();
      NoG8 = new Label();
      FoulsG7 = new Label();
      PointsG7 = new Label();
      NameG7 = new Label();
      NoG7 = new Label();
      FoulsG6 = new Label();
      PointsG6 = new Label();
      NameG6 = new Label();
      NoG6 = new Label();
      FoulsG5 = new Label();
      PointsG5 = new Label();
      NameG5 = new Label();
      NoG5 = new Label();
      FoulsG4 = new Label();
      PointsG4 = new Label();
      NameG4 = new Label();
      NoG4 = new Label();
      FoulsG3 = new Label();
      PointsG3 = new Label();
      NameG3 = new Label();
      NoG3 = new Label();
      FoulsG2 = new Label();
      PointsG2 = new Label();
      NameG2 = new Label();
      NoG2 = new Label();
      FoulsG1 = new Label();
      PointsG1 = new Label();
      NameG1 = new Label();
      NoG1 = new Label();
      FoulsH15 = new Label();
      PointsH15 = new Label();
      NameH15 = new Label();
      NoH15 = new Label();
      FoulsH14 = new Label();
      PointsH14 = new Label();
      NameH14 = new Label();
      NoH14 = new Label();
      FoulsH13 = new Label();
      PointsH13 = new Label();
      NameH13 = new Label();
      NoH13 = new Label();
      FoulsH12 = new Label();
      PointsH12 = new Label();
      NameH12 = new Label();
      NoH12 = new Label();
      FoulsH11 = new Label();
      PointsH11 = new Label();
      NameH11 = new Label();
      NoH11 = new Label();
      FoulsH10 = new Label();
      PointsH10 = new Label();
      NameH10 = new Label();
      NoH10 = new Label();
      FoulsH9 = new Label();
      PointsH9 = new Label();
      NameH9 = new Label();
      NoH9 = new Label();
      FoulsH8 = new Label();
      PointsH8 = new Label();
      NameH8 = new Label();
      NoH8 = new Label();
      FoulsH7 = new Label();
      PointsH7 = new Label();
      NameH7 = new Label();
      NoH7 = new Label();
      FoulsH6 = new Label();
      PointsH6 = new Label();
      NameH6 = new Label();
      NoH6 = new Label();
      FoulsH5 = new Label();
      PointsH5 = new Label();
      NameH5 = new Label();
      NoH5 = new Label();
      FoulsH4 = new Label();
      PointsH4 = new Label();
      NameH4 = new Label();
      NoH4 = new Label();
      FoulsH3 = new Label();
      PointsH3 = new Label();
      NameH3 = new Label();
      NoH3 = new Label();
      FoulsH2 = new Label();
      PointsH2 = new Label();
      NameH2 = new Label();
      NoH2 = new Label();
      FoulsH1 = new Label();
      PointsH1 = new Label();
      NameH1 = new Label();
      NoH1 = new Label();
      label1 = new Label();
      label2 = new Label();
      label3 = new Label();
      label5 = new Label();
      label6 = new Label();
      TimeoutTimeGuest = new Label();
      TimeoutTimeHome = new Label();
      labelCaution = new Label();
      labelTitlePeriodTimeNotElapsed = new Label();
      labelChangePeriodAnyway = new Label();
      btnHorn = new Button();
      cbShotclockDark = new CheckBox();
      Horn = new Label();
      HornTimer = new Timer(components);
      SC_Horn = new Label();
      ActFoulPlayer = new Label();
      actFoulPlayerTimer = new Timer(components);
      TeamID_Home = new Label();
      TeamID_Guest = new Label();
      ServiceHome = new Label();
      ServiceGuest = new Label();
      label7 = new Label();
      label8 = new Label();
      label9 = new Label();
      labelStartBreaktimeAnyway = new Label();
      labelStopBreaktimeFirst = new Label();
      ActFoulPlayerNumberHome = new Label();
      ActFoulHome = new Label();
      ActFoulGuest = new Label();
      ActFoulPlayerNumberGuest = new Label();
      lblShotclockTenth = new Label();
      lblGameTimeTenth = new Label();
      TimeRunning = new Label();
      GameTimeStopped = new Label();
      cbEdgeLightAtShotclockZero = new CheckBox();
      SC_EdgeLightOn = new Label();
      cbShotclockIndependentStart = new CheckBox();
      label10 = new Label();
      label11 = new Label();
      label12 = new Label();
      label13 = new Label();
      btnRight = new Button();
      btnLeft = new Button();
      label14 = new Label();
      label15 = new Label();
      GBPause = new GroupBox();
      BtnStartPause = new Button();
      BtnAbortStartPause = new Button();
      LbPauseBeginnen = new Label();
      cmsEditTimeouts.SuspendLayout();
      cmsEditPeriod.SuspendLayout();
      cmsEditTime.SuspendLayout();
      cmsEditTeamFouls.SuspendLayout();
      cmsPlayerData.SuspendLayout();
      cmsEditScore.SuspendLayout();
      GBPause.SuspendLayout();
      SuspendLayout();
      componentResourceManager.ApplyResources((object) label4, "label4");
      label4.ForeColor = Color.White;
      label4.Name = "label4";
      componentResourceManager.ApplyResources((object) TimeOutsHome, "TimeOutsHome");
      TimeOutsHome.BackColor = Color.FromArgb(64, 64, 64);
      TimeOutsHome.BorderStyle = BorderStyle.FixedSingle;
      TimeOutsHome.ContextMenuStrip = cmsEditTimeouts;
      TimeOutsHome.ForeColor = Color.Goldenrod;
      TimeOutsHome.Name = "TimeOutsHome";
      TimeOutsHome.Tag = (object) "o";
      TimeOutsHome.TextChanged += new EventHandler(SimpleTextChanged);
      TimeOutsHome.MouseDown += new MouseEventHandler(Label_MouseDown);
      TimeOutsHome.MouseEnter += new EventHandler(Label_MouseEnter);
      TimeOutsHome.MouseLeave += new EventHandler(Label_MouseLeave);
      componentResourceManager.ApplyResources((object) cmsEditTimeouts, "cmsEditTimeouts");
      cmsEditTimeouts.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) tsmTimeoutPlus1,
        (ToolStripItem) tmsTimeoutMinus1,
        (ToolStripItem) toolStripSeparator8,
        (ToolStripItem) tsmTimeoutPlus60s,
        (ToolStripItem) tsmTimeoutPlus30s,
        (ToolStripItem) toolStripSeparator10,
        (ToolStripItem) showTimeoutTimeToolStripMenuItem
      });
      cmsEditTimeouts.Name = "cmsEditScore";
      cmsEditTimeouts.Opening += new CancelEventHandler(cmsEditTimeouts_Opening_1);
      componentResourceManager.ApplyResources((object) tsmTimeoutPlus1, "tsmTimeoutPlus1");
      tsmTimeoutPlus1.Name = "tsmTimeoutPlus1";
      tsmTimeoutPlus1.Tag = (object) "1";
      tsmTimeoutPlus1.Click += new EventHandler(tsmEditTimeout_Click);
      componentResourceManager.ApplyResources((object) tmsTimeoutMinus1, "tmsTimeoutMinus1");
      tmsTimeoutMinus1.Name = "tmsTimeoutMinus1";
      tmsTimeoutMinus1.Tag = (object) "-1";
      tmsTimeoutMinus1.Click += new EventHandler(tsmEditTimeout_Click);
      componentResourceManager.ApplyResources((object) toolStripSeparator8, "toolStripSeparator8");
      toolStripSeparator8.Name = "toolStripSeparator8";
      componentResourceManager.ApplyResources((object) tsmTimeoutPlus60s, "tsmTimeoutPlus60s");
      tsmTimeoutPlus60s.Name = "tsmTimeoutPlus60s";
      tsmTimeoutPlus60s.Tag = (object) "60";
      tsmTimeoutPlus60s.Click += new EventHandler(tsmEditTimeout_Click);
      componentResourceManager.ApplyResources((object) tsmTimeoutPlus30s, "tsmTimeoutPlus30s");
      tsmTimeoutPlus30s.Name = "tsmTimeoutPlus30s";
      tsmTimeoutPlus30s.Tag = (object) "30";
      tsmTimeoutPlus30s.Click += new EventHandler(tsmEditTimeout_Click);
      componentResourceManager.ApplyResources((object) toolStripSeparator10, "toolStripSeparator10");
      toolStripSeparator10.Name = "toolStripSeparator10";
      componentResourceManager.ApplyResources((object) showTimeoutTimeToolStripMenuItem, "showTimeoutTimeToolStripMenuItem");
      showTimeoutTimeToolStripMenuItem.Name = "showTimeoutTimeToolStripMenuItem";
      showTimeoutTimeToolStripMenuItem.Click += new EventHandler(showTimeoutTimeToolStripMenuItem_Click);
      componentResourceManager.ApplyResources((object) TimeOutsGuest, "TimeOutsGuest");
      TimeOutsGuest.BackColor = Color.FromArgb(64, 64, 64);
      TimeOutsGuest.BorderStyle = BorderStyle.FixedSingle;
      TimeOutsGuest.ContextMenuStrip = cmsEditTimeouts;
      TimeOutsGuest.ForeColor = Color.Goldenrod;
      TimeOutsGuest.Name = "TimeOutsGuest";
      TimeOutsGuest.Tag = (object) "o";
      TimeOutsGuest.TextChanged += new EventHandler(SimpleTextChanged);
      TimeOutsGuest.MouseDown += new MouseEventHandler(Label_MouseDown);
      TimeOutsGuest.MouseEnter += new EventHandler(Label_MouseEnter);
      TimeOutsGuest.MouseLeave += new EventHandler(Label_MouseLeave);
      componentResourceManager.ApplyResources((object) cbStopShotclock, "cbStopShotclock");
      cbStopShotclock.ForeColor = Color.Black;
      cbStopShotclock.Name = "cbStopShotclock";
      cbStopShotclock.UseVisualStyleBackColor = true;
      cbStopShotclock.CheckedChanged += new EventHandler(cbStopShotclock_CheckedChanged);
      componentResourceManager.ApplyResources((object) Period, "Period");
      Period.BackColor = Color.FromArgb(64, 64, 64);
      Period.BorderStyle = BorderStyle.FixedSingle;
      Period.ContextMenuStrip = cmsEditPeriod;
      Period.ForeColor = Color.Goldenrod;
      Period.Name = "Period";
      Period.TextChanged += new EventHandler(SimpleTextChanged);
      Period.MouseDown += new MouseEventHandler(Label_MouseDown);
      Period.MouseEnter += new EventHandler(Label_MouseEnter);
      Period.MouseLeave += new EventHandler(Label_MouseLeave);
      componentResourceManager.ApplyResources((object) cmsEditPeriod, "cmsEditPeriod");
      cmsEditPeriod.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) tmsNextPeriod,
        (ToolStripItem) toolStripSeparator2,
        (ToolStripItem) tmsStartBreakTime,
        (ToolStripItem) toolStripSeparator6,
        (ToolStripItem) tmsPeriodPlus1,
        (ToolStripItem) tmsPeriodMinus1
      });
      cmsEditPeriod.Name = "cmsEditScore";
      cmsEditPeriod.Opening += new CancelEventHandler(cmsEditPeriod_Opening_1);
      componentResourceManager.ApplyResources((object) tmsNextPeriod, "tmsNextPeriod");
      tmsNextPeriod.Name = "tmsNextPeriod";
      tmsNextPeriod.Tag = (object) "1";
      tmsNextPeriod.Click += new EventHandler(tmsNextPeriod_Click);
      componentResourceManager.ApplyResources((object) toolStripSeparator2, "toolStripSeparator2");
      toolStripSeparator2.Name = "toolStripSeparator2";
      componentResourceManager.ApplyResources((object) tmsStartBreakTime, "tmsStartBreakTime");
      tmsStartBreakTime.Name = "tmsStartBreakTime";
      tmsStartBreakTime.Click += new EventHandler(tmsStartBreakTime_Click);
      componentResourceManager.ApplyResources((object) toolStripSeparator6, "toolStripSeparator6");
      toolStripSeparator6.Name = "toolStripSeparator6";
      componentResourceManager.ApplyResources((object) tmsPeriodPlus1, "tmsPeriodPlus1");
      tmsPeriodPlus1.Name = "tmsPeriodPlus1";
      tmsPeriodPlus1.Tag = (object) "1";
      tmsPeriodPlus1.Click += new EventHandler(tmsEditPeriod_Click);
      componentResourceManager.ApplyResources((object) tmsPeriodMinus1, "tmsPeriodMinus1");
      tmsPeriodMinus1.Name = "tmsPeriodMinus1";
      tmsPeriodMinus1.Tag = (object) "-1";
      tmsPeriodMinus1.Click += new EventHandler(tmsEditPeriod_Click);
      componentResourceManager.ApplyResources((object) btnReset24, "btnReset24");
      btnReset24.ForeColor = Color.Black;
      btnReset24.Name = "btnReset24";
      btnReset24.Tag = (object) "24";
      btnReset24.UseVisualStyleBackColor = true;
      btnReset24.Click += new EventHandler(btnReset24_Click);
      btnReset24.MouseDown += new MouseEventHandler(btnResetShotclock_MouseDown);
      btnReset24.MouseUp += new MouseEventHandler(btnResetShotclock_MouseUp);
      componentResourceManager.ApplyResources((object) btnReset14, "btnReset14");
      btnReset14.ForeColor = Color.Black;
      btnReset14.Name = "btnReset14";
      btnReset14.Tag = (object) "14";
      btnReset14.UseVisualStyleBackColor = true;
      btnReset14.MouseDown += new MouseEventHandler(btnResetShotclock_MouseDown);
      btnReset14.MouseUp += new MouseEventHandler(btnResetShotclock_MouseUp);
      componentResourceManager.ApplyResources((object) TeamNameHome, "TeamNameHome");
      TeamNameHome.ForeColor = Color.White;
      TeamNameHome.Name = "TeamNameHome";
      componentResourceManager.ApplyResources((object) ShotTime, "ShotTime");
      ShotTime.BackColor = Color.FromArgb(64, 64, 64);
      ShotTime.BorderStyle = BorderStyle.FixedSingle;
      ShotTime.ContextMenuStrip = cmsEditTime;
      ShotTime.ForeColor = Color.Goldenrod;
      ShotTime.Name = "ShotTime";
      ShotTime.TextChanged += new EventHandler(TimeTextChanged);
      ShotTime.MouseEnter += new EventHandler(Label_MouseEnter);
      ShotTime.MouseLeave += new EventHandler(Label_MouseLeave);
      componentResourceManager.ApplyResources((object) cmsEditTime, "cmsEditTime");
      cmsEditTime.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) tsmEditGameTime
      });
      cmsEditTime.Name = "cmsEditScore";
      componentResourceManager.ApplyResources((object) tsmEditGameTime, "tsmEditGameTime");
      tsmEditGameTime.Name = "tsmEditGameTime";
      tsmEditGameTime.Tag = (object) "1";
      tsmEditGameTime.Click += new EventHandler(tsmEditGameTime_Click);
      componentResourceManager.ApplyResources((object) btnStartStop, "btnStartStop");
      btnStartStop.ForeColor = Color.Black;
      btnStartStop.Name = "btnStartStop";
      btnStartStop.UseVisualStyleBackColor = true;
      btnStartStop.Click += new EventHandler(btnStartStop_Click);
      componentResourceManager.ApplyResources((object) GameTime, "GameTime");
      GameTime.BackColor = Color.FromArgb(64, 64, 64);
      GameTime.BorderStyle = BorderStyle.FixedSingle;
      GameTime.ContextMenuStrip = cmsEditTime;
      GameTime.ForeColor = Color.Red;
      GameTime.Name = "GameTime";
      GameTime.TextChanged += new EventHandler(TimeTextChanged);
      GameTime.MouseEnter += new EventHandler(Label_MouseEnter);
      GameTime.MouseLeave += new EventHandler(Label_MouseLeave);
      componentResourceManager.ApplyResources((object) TeamFoulsGuest, "TeamFoulsGuest");
      TeamFoulsGuest.BackColor = Color.FromArgb(64, 64, 64);
      TeamFoulsGuest.BorderStyle = BorderStyle.FixedSingle;
      TeamFoulsGuest.ContextMenuStrip = cmsEditTeamFouls;
      TeamFoulsGuest.ForeColor = Color.FromArgb(0, 192, 0);
      TeamFoulsGuest.Name = "TeamFoulsGuest";
      TeamFoulsGuest.TextChanged += new EventHandler(Fouls_TextChanged);
      TeamFoulsGuest.MouseDown += new MouseEventHandler(Label_MouseDown);
      TeamFoulsGuest.MouseEnter += new EventHandler(Label_MouseEnter);
      TeamFoulsGuest.MouseLeave += new EventHandler(Label_MouseLeave);
      componentResourceManager.ApplyResources((object) cmsEditTeamFouls, "cmsEditTeamFouls");
      cmsEditTeamFouls.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) tmsTeamFoulsPlus1,
        (ToolStripItem) tmsTeamFoulsMinus1
      });
      cmsEditTeamFouls.Name = "cmsEditScore";
      componentResourceManager.ApplyResources((object) tmsTeamFoulsPlus1, "tmsTeamFoulsPlus1");
      tmsTeamFoulsPlus1.Name = "tmsTeamFoulsPlus1";
      tmsTeamFoulsPlus1.Tag = (object) "1";
      tmsTeamFoulsPlus1.Click += new EventHandler(tmsTeamFoulsPlus1_Click);
      componentResourceManager.ApplyResources((object) tmsTeamFoulsMinus1, "tmsTeamFoulsMinus1");
      tmsTeamFoulsMinus1.Name = "tmsTeamFoulsMinus1";
      tmsTeamFoulsMinus1.Tag = (object) "-1";
      tmsTeamFoulsMinus1.Click += new EventHandler(tmsTeamFoulsPlus1_Click);
      componentResourceManager.ApplyResources((object) TeamFoulsHome, "TeamFoulsHome");
      TeamFoulsHome.BackColor = Color.FromArgb(64, 64, 64);
      TeamFoulsHome.BorderStyle = BorderStyle.FixedSingle;
      TeamFoulsHome.ContextMenuStrip = cmsEditTeamFouls;
      TeamFoulsHome.ForeColor = Color.FromArgb(0, 192, 0);
      TeamFoulsHome.Name = "TeamFoulsHome";
      TeamFoulsHome.TextChanged += new EventHandler(Fouls_TextChanged);
      TeamFoulsHome.MouseDown += new MouseEventHandler(Label_MouseDown);
      TeamFoulsHome.MouseEnter += new EventHandler(Label_MouseEnter);
      TeamFoulsHome.MouseLeave += new EventHandler(Label_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG15, "FoulsG15");
      FoulsG15.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG15.BorderStyle = BorderStyle.FixedSingle;
      FoulsG15.ContextMenuStrip = cmsPlayerData;
      FoulsG15.ForeColor = Color.White;
      FoulsG15.Name = "FoulsG15";
      FoulsG15.Tag = (object) "14";
      FoulsG15.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG15.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG15.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG15.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) cmsPlayerData, "cmsPlayerData");
      cmsPlayerData.Items.AddRange(new ToolStripItem[14]
      {
        (ToolStripItem) tsmPlayerName,
        (ToolStripItem) toolStripSeparator5,
        (ToolStripItem) tmsPlayerPointsPlus3,
        (ToolStripItem) tmsPlayerPointsPlus2,
        (ToolStripItem) tmsPlayerPointsPlus1,
        (ToolStripItem) tmsPlayerPointsMinus1,
        (ToolStripItem) toolStripSeparator1,
        (ToolStripItem) tmsPlayerCorrPointsPlus1,
        (ToolStripItem) tmsPlayerCorrPointsMinus1,
        (ToolStripItem) toolStripSeparator4,
        (ToolStripItem) tmsPlayerFoulsPlus1,
        (ToolStripItem) tmsPlayerFoulsMinus1,
        (ToolStripItem) toolStripSeparator3,
        (ToolStripItem) tmsPlayerEdit
      });
      cmsPlayerData.Name = "cmsEditPlayer";
      cmsPlayerData.Opening += new CancelEventHandler(cmsPlayerData_Opening);
      componentResourceManager.ApplyResources((object) tsmPlayerName, "tsmPlayerName");
      tsmPlayerName.BackColor = Color.Black;
      tsmPlayerName.ForeColor = Color.Red;
      tsmPlayerName.Name = "tsmPlayerName";
      componentResourceManager.ApplyResources((object) toolStripSeparator5, "toolStripSeparator5");
      toolStripSeparator5.Name = "toolStripSeparator5";
      componentResourceManager.ApplyResources((object) tmsPlayerPointsPlus3, "tmsPlayerPointsPlus3");
      tmsPlayerPointsPlus3.Name = "tmsPlayerPointsPlus3";
      tmsPlayerPointsPlus3.Tag = (object) "3";
      tmsPlayerPointsPlus3.Click += new EventHandler(tmsEditPlayerPoints_Click);
      componentResourceManager.ApplyResources((object) tmsPlayerPointsPlus2, "tmsPlayerPointsPlus2");
      tmsPlayerPointsPlus2.Name = "tmsPlayerPointsPlus2";
      tmsPlayerPointsPlus2.Tag = (object) "2";
      tmsPlayerPointsPlus2.Click += new EventHandler(tmsEditPlayerPoints_Click);
      componentResourceManager.ApplyResources((object) tmsPlayerPointsPlus1, "tmsPlayerPointsPlus1");
      tmsPlayerPointsPlus1.Name = "tmsPlayerPointsPlus1";
      tmsPlayerPointsPlus1.Tag = (object) "1";
      tmsPlayerPointsPlus1.Click += new EventHandler(tmsEditPlayerPoints_Click);
      componentResourceManager.ApplyResources((object) tmsPlayerPointsMinus1, "tmsPlayerPointsMinus1");
      tmsPlayerPointsMinus1.Name = "tmsPlayerPointsMinus1";
      tmsPlayerPointsMinus1.Tag = (object) "-1";
      tmsPlayerPointsMinus1.Click += new EventHandler(tmsEditPlayerPoints_Click);
      componentResourceManager.ApplyResources((object) toolStripSeparator1, "toolStripSeparator1");
      toolStripSeparator1.Name = "toolStripSeparator1";
      componentResourceManager.ApplyResources((object) tmsPlayerCorrPointsPlus1, "tmsPlayerCorrPointsPlus1");
      tmsPlayerCorrPointsPlus1.Name = "tmsPlayerCorrPointsPlus1";
      tmsPlayerCorrPointsPlus1.Tag = (object) "1";
      tmsPlayerCorrPointsPlus1.Click += new EventHandler(tmsPlayerCorrPoints_Click);
      componentResourceManager.ApplyResources((object) tmsPlayerCorrPointsMinus1, "tmsPlayerCorrPointsMinus1");
      tmsPlayerCorrPointsMinus1.Name = "tmsPlayerCorrPointsMinus1";
      tmsPlayerCorrPointsMinus1.Tag = (object) "-1";
      tmsPlayerCorrPointsMinus1.Click += new EventHandler(tmsPlayerCorrPoints_Click);
      componentResourceManager.ApplyResources((object) toolStripSeparator4, "toolStripSeparator4");
      toolStripSeparator4.Name = "toolStripSeparator4";
      componentResourceManager.ApplyResources((object) tmsPlayerFoulsPlus1, "tmsPlayerFoulsPlus1");
      tmsPlayerFoulsPlus1.Name = "tmsPlayerFoulsPlus1";
      tmsPlayerFoulsPlus1.Tag = (object) "1";
      tmsPlayerFoulsPlus1.Click += new EventHandler(tmsEditPlayerFouls_Click);
      componentResourceManager.ApplyResources((object) tmsPlayerFoulsMinus1, "tmsPlayerFoulsMinus1");
      tmsPlayerFoulsMinus1.Name = "tmsPlayerFoulsMinus1";
      tmsPlayerFoulsMinus1.Tag = (object) "-1";
      tmsPlayerFoulsMinus1.Click += new EventHandler(tmsEditPlayerFouls_Click);
      componentResourceManager.ApplyResources((object) toolStripSeparator3, "toolStripSeparator3");
      toolStripSeparator3.Name = "toolStripSeparator3";
      componentResourceManager.ApplyResources((object) tmsPlayerEdit, "tmsPlayerEdit");
      tmsPlayerEdit.Name = "tmsPlayerEdit";
      tmsPlayerEdit.Click += new EventHandler(tmsPlayerEdit_Click);
      componentResourceManager.ApplyResources((object) PointsG15, "PointsG15");
      PointsG15.BackColor = Color.FromArgb(64, 64, 64);
      PointsG15.BorderStyle = BorderStyle.FixedSingle;
      PointsG15.ContextMenuStrip = cmsPlayerData;
      PointsG15.ForeColor = Color.White;
      PointsG15.Name = "PointsG15";
      PointsG15.Tag = (object) "14";
      PointsG15.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG15.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG15.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG15.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG15, "NameG15");
      NameG15.BackColor = Color.FromArgb(64, 64, 64);
      NameG15.BorderStyle = BorderStyle.FixedSingle;
      NameG15.ContextMenuStrip = cmsPlayerData;
      NameG15.ForeColor = Color.White;
      NameG15.Name = "NameG15";
      NameG15.Tag = (object) "14";
      NameG15.TextChanged += new EventHandler(SimpleTextChanged);
      NameG15.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG15.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG15.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG15, "NoG15");
      NoG15.BackColor = Color.FromArgb(64, 64, 64);
      NoG15.BorderStyle = BorderStyle.FixedSingle;
      NoG15.ContextMenuStrip = cmsPlayerData;
      NoG15.ForeColor = Color.White;
      NoG15.Name = "NoG15";
      NoG15.Tag = (object) "14";
      NoG15.TextChanged += new EventHandler(SimpleTextChanged);
      NoG15.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG15.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG15.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG14, "FoulsG14");
      FoulsG14.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG14.BorderStyle = BorderStyle.FixedSingle;
      FoulsG14.ContextMenuStrip = cmsPlayerData;
      FoulsG14.ForeColor = Color.White;
      FoulsG14.Name = "FoulsG14";
      FoulsG14.Tag = (object) "13";
      FoulsG14.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG14.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG14.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG14.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) ScoreGuest, "ScoreGuest");
      ScoreGuest.BackColor = Color.FromArgb(64, 64, 64);
      ScoreGuest.BorderStyle = BorderStyle.FixedSingle;
      ScoreGuest.ContextMenuStrip = cmsEditScore;
      ScoreGuest.Name = "ScoreGuest";
      ScoreGuest.TextChanged += new EventHandler(SimpleTextChanged);
      ScoreGuest.MouseDown += new MouseEventHandler(Label_MouseDown);
      ScoreGuest.MouseEnter += new EventHandler(Label_MouseEnter);
      ScoreGuest.MouseLeave += new EventHandler(Label_MouseLeave);
      componentResourceManager.ApplyResources((object) cmsEditScore, "cmsEditScore");
      cmsEditScore.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) tmsPointsPlus3,
        (ToolStripItem) tmsPointsPlus2,
        (ToolStripItem) tmsScorePlus1,
        (ToolStripItem) toolStripSeparator9,
        (ToolStripItem) tmsScoreMinus1,
        (ToolStripItem) toolStripSeparator7,
        (ToolStripItem) tmsScoreDirectInput
      });
      cmsEditScore.Name = "cmsEditScore";
      componentResourceManager.ApplyResources((object) tmsPointsPlus3, "tmsPointsPlus3");
      tmsPointsPlus3.Name = "tmsPointsPlus3";
      tmsPointsPlus3.Tag = (object) "3";
      tmsPointsPlus3.Click += new EventHandler(tmsEditScore_Click);
      componentResourceManager.ApplyResources((object) tmsPointsPlus2, "tmsPointsPlus2");
      tmsPointsPlus2.Name = "tmsPointsPlus2";
      tmsPointsPlus2.Tag = (object) "2";
      tmsPointsPlus2.Click += new EventHandler(tmsEditScore_Click);
      componentResourceManager.ApplyResources((object) tmsScorePlus1, "tmsScorePlus1");
      tmsScorePlus1.Name = "tmsScorePlus1";
      tmsScorePlus1.Tag = (object) "1";
      tmsScorePlus1.Click += new EventHandler(tmsEditScore_Click);
      componentResourceManager.ApplyResources((object) toolStripSeparator9, "toolStripSeparator9");
      toolStripSeparator9.Name = "toolStripSeparator9";
      componentResourceManager.ApplyResources((object) tmsScoreMinus1, "tmsScoreMinus1");
      tmsScoreMinus1.Name = "tmsScoreMinus1";
      tmsScoreMinus1.Tag = (object) "-1";
      tmsScoreMinus1.Click += new EventHandler(tmsEditScore_Click);
      componentResourceManager.ApplyResources((object) toolStripSeparator7, "toolStripSeparator7");
      toolStripSeparator7.Name = "toolStripSeparator7";
      componentResourceManager.ApplyResources((object) tmsScoreDirectInput, "tmsScoreDirectInput");
      tmsScoreDirectInput.Name = "tmsScoreDirectInput";
      tmsScoreDirectInput.Click += new EventHandler(tmsScoreDirectInput_Click);
      componentResourceManager.ApplyResources((object) TeamNameGuest, "TeamNameGuest");
      TeamNameGuest.ForeColor = Color.White;
      TeamNameGuest.Name = "TeamNameGuest";
      componentResourceManager.ApplyResources((object) PointsG14, "PointsG14");
      PointsG14.BackColor = Color.FromArgb(64, 64, 64);
      PointsG14.BorderStyle = BorderStyle.FixedSingle;
      PointsG14.ContextMenuStrip = cmsPlayerData;
      PointsG14.ForeColor = Color.White;
      PointsG14.Name = "PointsG14";
      PointsG14.Tag = (object) "13";
      PointsG14.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG14.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG14.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG14.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG14, "NameG14");
      NameG14.BackColor = Color.FromArgb(64, 64, 64);
      NameG14.BorderStyle = BorderStyle.FixedSingle;
      NameG14.ContextMenuStrip = cmsPlayerData;
      NameG14.ForeColor = Color.White;
      NameG14.Name = "NameG14";
      NameG14.Tag = (object) "13";
      NameG14.TextChanged += new EventHandler(SimpleTextChanged);
      NameG14.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG14.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG14.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) ScoreHome, "ScoreHome");
      ScoreHome.BackColor = Color.FromArgb(64, 64, 64);
      ScoreHome.BorderStyle = BorderStyle.FixedSingle;
      ScoreHome.ContextMenuStrip = cmsEditScore;
      ScoreHome.Name = "ScoreHome";
      ScoreHome.TextChanged += new EventHandler(SimpleTextChanged);
      ScoreHome.MouseDown += new MouseEventHandler(Label_MouseDown);
      ScoreHome.MouseEnter += new EventHandler(Label_MouseEnter);
      ScoreHome.MouseLeave += new EventHandler(Label_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG14, "NoG14");
      NoG14.BackColor = Color.FromArgb(64, 64, 64);
      NoG14.BorderStyle = BorderStyle.FixedSingle;
      NoG14.ContextMenuStrip = cmsPlayerData;
      NoG14.ForeColor = Color.White;
      NoG14.Name = "NoG14";
      NoG14.Tag = (object) "13";
      NoG14.TextChanged += new EventHandler(SimpleTextChanged);
      NoG14.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG14.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG14.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG13, "FoulsG13");
      FoulsG13.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG13.BorderStyle = BorderStyle.FixedSingle;
      FoulsG13.ContextMenuStrip = cmsPlayerData;
      FoulsG13.ForeColor = Color.White;
      FoulsG13.Name = "FoulsG13";
      FoulsG13.Tag = (object) "12";
      FoulsG13.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG13.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG13.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG13.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG13, "PointsG13");
      PointsG13.BackColor = Color.FromArgb(64, 64, 64);
      PointsG13.BorderStyle = BorderStyle.FixedSingle;
      PointsG13.ContextMenuStrip = cmsPlayerData;
      PointsG13.ForeColor = Color.White;
      PointsG13.Name = "PointsG13";
      PointsG13.Tag = (object) "12";
      PointsG13.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG13.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG13.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG13.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG13, "NameG13");
      NameG13.BackColor = Color.FromArgb(64, 64, 64);
      NameG13.BorderStyle = BorderStyle.FixedSingle;
      NameG13.ContextMenuStrip = cmsPlayerData;
      NameG13.ForeColor = Color.White;
      NameG13.Name = "NameG13";
      NameG13.Tag = (object) "12";
      NameG13.TextChanged += new EventHandler(SimpleTextChanged);
      NameG13.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG13.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG13.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG13, "NoG13");
      NoG13.BackColor = Color.FromArgb(64, 64, 64);
      NoG13.BorderStyle = BorderStyle.FixedSingle;
      NoG13.ContextMenuStrip = cmsPlayerData;
      NoG13.ForeColor = Color.White;
      NoG13.Name = "NoG13";
      NoG13.Tag = (object) "12";
      NoG13.TextChanged += new EventHandler(SimpleTextChanged);
      NoG13.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG13.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG13.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG12, "FoulsG12");
      FoulsG12.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG12.BorderStyle = BorderStyle.FixedSingle;
      FoulsG12.ContextMenuStrip = cmsPlayerData;
      FoulsG12.ForeColor = Color.White;
      FoulsG12.Name = "FoulsG12";
      FoulsG12.Tag = (object) "11";
      FoulsG12.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG12.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG12.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG12.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG12, "PointsG12");
      PointsG12.BackColor = Color.FromArgb(64, 64, 64);
      PointsG12.BorderStyle = BorderStyle.FixedSingle;
      PointsG12.ContextMenuStrip = cmsPlayerData;
      PointsG12.ForeColor = Color.White;
      PointsG12.Name = "PointsG12";
      PointsG12.Tag = (object) "11";
      PointsG12.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG12.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG12.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG12.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG12, "NameG12");
      NameG12.BackColor = Color.FromArgb(64, 64, 64);
      NameG12.BorderStyle = BorderStyle.FixedSingle;
      NameG12.ContextMenuStrip = cmsPlayerData;
      NameG12.ForeColor = Color.White;
      NameG12.Name = "NameG12";
      NameG12.Tag = (object) "11";
      NameG12.TextChanged += new EventHandler(SimpleTextChanged);
      NameG12.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG12.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG12.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG12, "NoG12");
      NoG12.BackColor = Color.FromArgb(64, 64, 64);
      NoG12.BorderStyle = BorderStyle.FixedSingle;
      NoG12.ContextMenuStrip = cmsPlayerData;
      NoG12.ForeColor = Color.White;
      NoG12.Name = "NoG12";
      NoG12.Tag = (object) "11";
      NoG12.TextChanged += new EventHandler(SimpleTextChanged);
      NoG12.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG12.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG12.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG11, "FoulsG11");
      FoulsG11.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG11.BorderStyle = BorderStyle.FixedSingle;
      FoulsG11.ContextMenuStrip = cmsPlayerData;
      FoulsG11.ForeColor = Color.White;
      FoulsG11.Name = "FoulsG11";
      FoulsG11.Tag = (object) "10";
      FoulsG11.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG11.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG11.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG11.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG11, "PointsG11");
      PointsG11.BackColor = Color.FromArgb(64, 64, 64);
      PointsG11.BorderStyle = BorderStyle.FixedSingle;
      PointsG11.ContextMenuStrip = cmsPlayerData;
      PointsG11.ForeColor = Color.White;
      PointsG11.Name = "PointsG11";
      PointsG11.Tag = (object) "10";
      PointsG11.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG11.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG11.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG11.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG11, "NameG11");
      NameG11.BackColor = Color.FromArgb(64, 64, 64);
      NameG11.BorderStyle = BorderStyle.FixedSingle;
      NameG11.ContextMenuStrip = cmsPlayerData;
      NameG11.ForeColor = Color.White;
      NameG11.Name = "NameG11";
      NameG11.Tag = (object) "10";
      NameG11.TextChanged += new EventHandler(SimpleTextChanged);
      NameG11.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG11.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG11.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG11, "NoG11");
      NoG11.BackColor = Color.FromArgb(64, 64, 64);
      NoG11.BorderStyle = BorderStyle.FixedSingle;
      NoG11.ContextMenuStrip = cmsPlayerData;
      NoG11.ForeColor = Color.White;
      NoG11.Name = "NoG11";
      NoG11.Tag = (object) "10";
      NoG11.TextChanged += new EventHandler(SimpleTextChanged);
      NoG11.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG11.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG11.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG10, "FoulsG10");
      FoulsG10.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG10.BorderStyle = BorderStyle.FixedSingle;
      FoulsG10.ContextMenuStrip = cmsPlayerData;
      FoulsG10.ForeColor = Color.White;
      FoulsG10.Name = "FoulsG10";
      FoulsG10.Tag = (object) "9";
      FoulsG10.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG10.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG10.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG10.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG10, "PointsG10");
      PointsG10.BackColor = Color.FromArgb(64, 64, 64);
      PointsG10.BorderStyle = BorderStyle.FixedSingle;
      PointsG10.ContextMenuStrip = cmsPlayerData;
      PointsG10.ForeColor = Color.White;
      PointsG10.Name = "PointsG10";
      PointsG10.Tag = (object) "9";
      PointsG10.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG10.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG10.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG10.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG10, "NameG10");
      NameG10.BackColor = Color.FromArgb(64, 64, 64);
      NameG10.BorderStyle = BorderStyle.FixedSingle;
      NameG10.ContextMenuStrip = cmsPlayerData;
      NameG10.ForeColor = Color.White;
      NameG10.Name = "NameG10";
      NameG10.Tag = (object) "9";
      NameG10.TextChanged += new EventHandler(SimpleTextChanged);
      NameG10.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG10.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG10.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG10, "NoG10");
      NoG10.BackColor = Color.FromArgb(64, 64, 64);
      NoG10.BorderStyle = BorderStyle.FixedSingle;
      NoG10.ContextMenuStrip = cmsPlayerData;
      NoG10.ForeColor = Color.White;
      NoG10.Name = "NoG10";
      NoG10.Tag = (object) "9";
      NoG10.TextChanged += new EventHandler(SimpleTextChanged);
      NoG10.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG10.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG10.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG9, "FoulsG9");
      FoulsG9.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG9.BorderStyle = BorderStyle.FixedSingle;
      FoulsG9.ContextMenuStrip = cmsPlayerData;
      FoulsG9.ForeColor = Color.White;
      FoulsG9.Name = "FoulsG9";
      FoulsG9.Tag = (object) "8";
      FoulsG9.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG9.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG9.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG9.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG9, "PointsG9");
      PointsG9.BackColor = Color.FromArgb(64, 64, 64);
      PointsG9.BorderStyle = BorderStyle.FixedSingle;
      PointsG9.ContextMenuStrip = cmsPlayerData;
      PointsG9.ForeColor = Color.White;
      PointsG9.Name = "PointsG9";
      PointsG9.Tag = (object) "8";
      PointsG9.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG9.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG9.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG9.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG9, "NameG9");
      NameG9.BackColor = Color.FromArgb(64, 64, 64);
      NameG9.BorderStyle = BorderStyle.FixedSingle;
      NameG9.ContextMenuStrip = cmsPlayerData;
      NameG9.ForeColor = Color.White;
      NameG9.Name = "NameG9";
      NameG9.Tag = (object) "8";
      NameG9.TextChanged += new EventHandler(SimpleTextChanged);
      NameG9.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG9.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG9.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG9, "NoG9");
      NoG9.BackColor = Color.FromArgb(64, 64, 64);
      NoG9.BorderStyle = BorderStyle.FixedSingle;
      NoG9.ContextMenuStrip = cmsPlayerData;
      NoG9.ForeColor = Color.White;
      NoG9.Name = "NoG9";
      NoG9.Tag = (object) "8";
      NoG9.TextChanged += new EventHandler(SimpleTextChanged);
      NoG9.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG9.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG9.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG8, "FoulsG8");
      FoulsG8.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG8.BorderStyle = BorderStyle.FixedSingle;
      FoulsG8.ContextMenuStrip = cmsPlayerData;
      FoulsG8.ForeColor = Color.White;
      FoulsG8.Name = "FoulsG8";
      FoulsG8.Tag = (object) "7";
      FoulsG8.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG8.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG8.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG8.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG8, "PointsG8");
      PointsG8.BackColor = Color.FromArgb(64, 64, 64);
      PointsG8.BorderStyle = BorderStyle.FixedSingle;
      PointsG8.ContextMenuStrip = cmsPlayerData;
      PointsG8.ForeColor = Color.White;
      PointsG8.Name = "PointsG8";
      PointsG8.Tag = (object) "7";
      PointsG8.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG8.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG8.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG8.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG8, "NameG8");
      NameG8.BackColor = Color.FromArgb(64, 64, 64);
      NameG8.BorderStyle = BorderStyle.FixedSingle;
      NameG8.ContextMenuStrip = cmsPlayerData;
      NameG8.ForeColor = Color.White;
      NameG8.Name = "NameG8";
      NameG8.Tag = (object) "7";
      NameG8.TextChanged += new EventHandler(SimpleTextChanged);
      NameG8.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG8.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG8.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG8, "NoG8");
      NoG8.BackColor = Color.FromArgb(64, 64, 64);
      NoG8.BorderStyle = BorderStyle.FixedSingle;
      NoG8.ContextMenuStrip = cmsPlayerData;
      NoG8.ForeColor = Color.White;
      NoG8.Name = "NoG8";
      NoG8.Tag = (object) "7";
      NoG8.TextChanged += new EventHandler(SimpleTextChanged);
      NoG8.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG8.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG8.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG7, "FoulsG7");
      FoulsG7.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG7.BorderStyle = BorderStyle.FixedSingle;
      FoulsG7.ContextMenuStrip = cmsPlayerData;
      FoulsG7.ForeColor = Color.White;
      FoulsG7.Name = "FoulsG7";
      FoulsG7.Tag = (object) "6";
      FoulsG7.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG7.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG7.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG7.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG7, "PointsG7");
      PointsG7.BackColor = Color.FromArgb(64, 64, 64);
      PointsG7.BorderStyle = BorderStyle.FixedSingle;
      PointsG7.ContextMenuStrip = cmsPlayerData;
      PointsG7.ForeColor = Color.White;
      PointsG7.Name = "PointsG7";
      PointsG7.Tag = (object) "6";
      PointsG7.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG7.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG7.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG7.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG7, "NameG7");
      NameG7.BackColor = Color.FromArgb(64, 64, 64);
      NameG7.BorderStyle = BorderStyle.FixedSingle;
      NameG7.ContextMenuStrip = cmsPlayerData;
      NameG7.ForeColor = Color.White;
      NameG7.Name = "NameG7";
      NameG7.Tag = (object) "6";
      NameG7.TextChanged += new EventHandler(SimpleTextChanged);
      NameG7.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG7.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG7.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG7, "NoG7");
      NoG7.BackColor = Color.FromArgb(64, 64, 64);
      NoG7.BorderStyle = BorderStyle.FixedSingle;
      NoG7.ContextMenuStrip = cmsPlayerData;
      NoG7.ForeColor = Color.White;
      NoG7.Name = "NoG7";
      NoG7.Tag = (object) "6";
      NoG7.TextChanged += new EventHandler(SimpleTextChanged);
      NoG7.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG7.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG7.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG6, "FoulsG6");
      FoulsG6.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG6.BorderStyle = BorderStyle.FixedSingle;
      FoulsG6.ContextMenuStrip = cmsPlayerData;
      FoulsG6.ForeColor = Color.White;
      FoulsG6.Name = "FoulsG6";
      FoulsG6.Tag = (object) "5";
      FoulsG6.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG6.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG6.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG6.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG6, "PointsG6");
      PointsG6.BackColor = Color.FromArgb(64, 64, 64);
      PointsG6.BorderStyle = BorderStyle.FixedSingle;
      PointsG6.ContextMenuStrip = cmsPlayerData;
      PointsG6.ForeColor = Color.White;
      PointsG6.Name = "PointsG6";
      PointsG6.Tag = (object) "5";
      PointsG6.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG6.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG6.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG6.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG6, "NameG6");
      NameG6.BackColor = Color.FromArgb(64, 64, 64);
      NameG6.BorderStyle = BorderStyle.FixedSingle;
      NameG6.ContextMenuStrip = cmsPlayerData;
      NameG6.ForeColor = Color.White;
      NameG6.Name = "NameG6";
      NameG6.Tag = (object) "5";
      NameG6.TextChanged += new EventHandler(SimpleTextChanged);
      NameG6.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG6.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG6.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG6, "NoG6");
      NoG6.BackColor = Color.FromArgb(64, 64, 64);
      NoG6.BorderStyle = BorderStyle.FixedSingle;
      NoG6.ContextMenuStrip = cmsPlayerData;
      NoG6.ForeColor = Color.White;
      NoG6.Name = "NoG6";
      NoG6.Tag = (object) "5";
      NoG6.TextChanged += new EventHandler(SimpleTextChanged);
      NoG6.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG6.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG6.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG5, "FoulsG5");
      FoulsG5.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG5.BorderStyle = BorderStyle.FixedSingle;
      FoulsG5.ContextMenuStrip = cmsPlayerData;
      FoulsG5.ForeColor = Color.White;
      FoulsG5.Name = "FoulsG5";
      FoulsG5.Tag = (object) "4";
      FoulsG5.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG5.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG5.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG5.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG5, "PointsG5");
      PointsG5.BackColor = Color.FromArgb(64, 64, 64);
      PointsG5.BorderStyle = BorderStyle.FixedSingle;
      PointsG5.ContextMenuStrip = cmsPlayerData;
      PointsG5.ForeColor = Color.White;
      PointsG5.Name = "PointsG5";
      PointsG5.Tag = (object) "4";
      PointsG5.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG5.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG5.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG5.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG5, "NameG5");
      NameG5.BackColor = Color.FromArgb(64, 64, 64);
      NameG5.BorderStyle = BorderStyle.FixedSingle;
      NameG5.ContextMenuStrip = cmsPlayerData;
      NameG5.ForeColor = Color.White;
      NameG5.Name = "NameG5";
      NameG5.Tag = (object) "4";
      NameG5.TextChanged += new EventHandler(SimpleTextChanged);
      NameG5.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG5.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG5.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG5, "NoG5");
      NoG5.BackColor = Color.FromArgb(64, 64, 64);
      NoG5.BorderStyle = BorderStyle.FixedSingle;
      NoG5.ContextMenuStrip = cmsPlayerData;
      NoG5.ForeColor = Color.White;
      NoG5.Name = "NoG5";
      NoG5.Tag = (object) "4";
      NoG5.TextChanged += new EventHandler(SimpleTextChanged);
      NoG5.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG5.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG5.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG4, "FoulsG4");
      FoulsG4.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG4.BorderStyle = BorderStyle.FixedSingle;
      FoulsG4.ContextMenuStrip = cmsPlayerData;
      FoulsG4.ForeColor = Color.White;
      FoulsG4.Name = "FoulsG4";
      FoulsG4.Tag = (object) "3";
      FoulsG4.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG4.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG4.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG4.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG4, "PointsG4");
      PointsG4.BackColor = Color.FromArgb(64, 64, 64);
      PointsG4.BorderStyle = BorderStyle.FixedSingle;
      PointsG4.ContextMenuStrip = cmsPlayerData;
      PointsG4.ForeColor = Color.White;
      PointsG4.Name = "PointsG4";
      PointsG4.Tag = (object) "3";
      PointsG4.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG4.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG4.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG4.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG4, "NameG4");
      NameG4.BackColor = Color.FromArgb(64, 64, 64);
      NameG4.BorderStyle = BorderStyle.FixedSingle;
      NameG4.ContextMenuStrip = cmsPlayerData;
      NameG4.ForeColor = Color.White;
      NameG4.Name = "NameG4";
      NameG4.Tag = (object) "3";
      NameG4.TextChanged += new EventHandler(SimpleTextChanged);
      NameG4.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG4.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG4.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG4, "NoG4");
      NoG4.BackColor = Color.FromArgb(64, 64, 64);
      NoG4.BorderStyle = BorderStyle.FixedSingle;
      NoG4.ContextMenuStrip = cmsPlayerData;
      NoG4.ForeColor = Color.White;
      NoG4.Name = "NoG4";
      NoG4.Tag = (object) "3";
      NoG4.TextChanged += new EventHandler(SimpleTextChanged);
      NoG4.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG4.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG4.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG3, "FoulsG3");
      FoulsG3.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG3.BorderStyle = BorderStyle.FixedSingle;
      FoulsG3.ContextMenuStrip = cmsPlayerData;
      FoulsG3.ForeColor = Color.White;
      FoulsG3.Name = "FoulsG3";
      FoulsG3.Tag = (object) "2";
      FoulsG3.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG3.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG3.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG3.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG3, "PointsG3");
      PointsG3.BackColor = Color.FromArgb(64, 64, 64);
      PointsG3.BorderStyle = BorderStyle.FixedSingle;
      PointsG3.ContextMenuStrip = cmsPlayerData;
      PointsG3.ForeColor = Color.White;
      PointsG3.Name = "PointsG3";
      PointsG3.Tag = (object) "2";
      PointsG3.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG3.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG3.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG3.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG3, "NameG3");
      NameG3.BackColor = Color.FromArgb(64, 64, 64);
      NameG3.BorderStyle = BorderStyle.FixedSingle;
      NameG3.ContextMenuStrip = cmsPlayerData;
      NameG3.ForeColor = Color.White;
      NameG3.Name = "NameG3";
      NameG3.Tag = (object) "2";
      NameG3.TextChanged += new EventHandler(SimpleTextChanged);
      NameG3.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG3.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG3.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG3, "NoG3");
      NoG3.BackColor = Color.FromArgb(64, 64, 64);
      NoG3.BorderStyle = BorderStyle.FixedSingle;
      NoG3.ContextMenuStrip = cmsPlayerData;
      NoG3.ForeColor = Color.White;
      NoG3.Name = "NoG3";
      NoG3.Tag = (object) "2";
      NoG3.TextChanged += new EventHandler(SimpleTextChanged);
      NoG3.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG3.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG3.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG2, "FoulsG2");
      FoulsG2.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG2.BorderStyle = BorderStyle.FixedSingle;
      FoulsG2.ContextMenuStrip = cmsPlayerData;
      FoulsG2.ForeColor = Color.White;
      FoulsG2.Name = "FoulsG2";
      FoulsG2.Tag = (object) "1";
      FoulsG2.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG2.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG2.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG2.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG2, "PointsG2");
      PointsG2.BackColor = Color.FromArgb(64, 64, 64);
      PointsG2.BorderStyle = BorderStyle.FixedSingle;
      PointsG2.ContextMenuStrip = cmsPlayerData;
      PointsG2.ForeColor = Color.White;
      PointsG2.Name = "PointsG2";
      PointsG2.Tag = (object) "1";
      PointsG2.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG2.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG2.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG2.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG2, "NameG2");
      NameG2.BackColor = Color.FromArgb(64, 64, 64);
      NameG2.BorderStyle = BorderStyle.FixedSingle;
      NameG2.ContextMenuStrip = cmsPlayerData;
      NameG2.ForeColor = Color.White;
      NameG2.Name = "NameG2";
      NameG2.Tag = (object) "1";
      NameG2.TextChanged += new EventHandler(SimpleTextChanged);
      NameG2.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG2.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG2.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG2, "NoG2");
      NoG2.BackColor = Color.FromArgb(64, 64, 64);
      NoG2.BorderStyle = BorderStyle.FixedSingle;
      NoG2.ContextMenuStrip = cmsPlayerData;
      NoG2.ForeColor = Color.White;
      NoG2.Name = "NoG2";
      NoG2.Tag = (object) "1";
      NoG2.TextChanged += new EventHandler(SimpleTextChanged);
      NoG2.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG2.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG2.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsG1, "FoulsG1");
      FoulsG1.BackColor = Color.FromArgb(64, 64, 64);
      FoulsG1.BorderStyle = BorderStyle.FixedSingle;
      FoulsG1.ContextMenuStrip = cmsPlayerData;
      FoulsG1.ForeColor = Color.White;
      FoulsG1.Name = "FoulsG1";
      FoulsG1.Tag = (object) "0";
      FoulsG1.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsG1.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsG1.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsG1.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsG1, "PointsG1");
      PointsG1.BackColor = Color.FromArgb(64, 64, 64);
      PointsG1.BorderStyle = BorderStyle.FixedSingle;
      PointsG1.ContextMenuStrip = cmsPlayerData;
      PointsG1.ForeColor = Color.White;
      PointsG1.Name = "PointsG1";
      PointsG1.Tag = (object) "0";
      PointsG1.TextChanged += new EventHandler(SimpleTextChanged);
      PointsG1.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsG1.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsG1.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameG1, "NameG1");
      NameG1.BackColor = Color.FromArgb(64, 64, 64);
      NameG1.BorderStyle = BorderStyle.FixedSingle;
      NameG1.ContextMenuStrip = cmsPlayerData;
      NameG1.ForeColor = Color.White;
      NameG1.Name = "NameG1";
      NameG1.Tag = (object) "0";
      NameG1.TextChanged += new EventHandler(SimpleTextChanged);
      NameG1.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameG1.MouseEnter += new EventHandler(Player_MouseEnter);
      NameG1.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoG1, "NoG1");
      NoG1.BackColor = Color.FromArgb(64, 64, 64);
      NoG1.BorderStyle = BorderStyle.FixedSingle;
      NoG1.ContextMenuStrip = cmsPlayerData;
      NoG1.ForeColor = Color.White;
      NoG1.Name = "NoG1";
      NoG1.Tag = (object) "0";
      NoG1.TextChanged += new EventHandler(SimpleTextChanged);
      NoG1.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoG1.MouseEnter += new EventHandler(Player_MouseEnter);
      NoG1.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH15, "FoulsH15");
      FoulsH15.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH15.BorderStyle = BorderStyle.FixedSingle;
      FoulsH15.ContextMenuStrip = cmsPlayerData;
      FoulsH15.ForeColor = Color.Lime;
      FoulsH15.Name = "FoulsH15";
      FoulsH15.Tag = (object) "14";
      FoulsH15.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH15.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH15.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH15.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH15, "PointsH15");
      PointsH15.BackColor = Color.FromArgb(64, 64, 64);
      PointsH15.BorderStyle = BorderStyle.FixedSingle;
      PointsH15.ContextMenuStrip = cmsPlayerData;
      PointsH15.ForeColor = Color.White;
      PointsH15.Name = "PointsH15";
      PointsH15.Tag = (object) "14";
      PointsH15.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH15.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH15.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH15.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH15, "NameH15");
      NameH15.BackColor = Color.FromArgb(64, 64, 64);
      NameH15.BorderStyle = BorderStyle.FixedSingle;
      NameH15.ContextMenuStrip = cmsPlayerData;
      NameH15.ForeColor = Color.White;
      NameH15.Name = "NameH15";
      NameH15.Tag = (object) "14";
      NameH15.TextChanged += new EventHandler(SimpleTextChanged);
      NameH15.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH15.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH15.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH15, "NoH15");
      NoH15.BackColor = Color.FromArgb(64, 64, 64);
      NoH15.BorderStyle = BorderStyle.FixedSingle;
      NoH15.ContextMenuStrip = cmsPlayerData;
      NoH15.ForeColor = Color.White;
      NoH15.Name = "NoH15";
      NoH15.Tag = (object) "14";
      NoH15.TextChanged += new EventHandler(SimpleTextChanged);
      NoH15.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH15.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH15.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH14, "FoulsH14");
      FoulsH14.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH14.BorderStyle = BorderStyle.FixedSingle;
      FoulsH14.ContextMenuStrip = cmsPlayerData;
      FoulsH14.ForeColor = Color.Lime;
      FoulsH14.Name = "FoulsH14";
      FoulsH14.Tag = (object) "13";
      FoulsH14.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH14.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH14.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH14.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH14, "PointsH14");
      PointsH14.BackColor = Color.FromArgb(64, 64, 64);
      PointsH14.BorderStyle = BorderStyle.FixedSingle;
      PointsH14.ContextMenuStrip = cmsPlayerData;
      PointsH14.ForeColor = Color.White;
      PointsH14.Name = "PointsH14";
      PointsH14.Tag = (object) "13";
      PointsH14.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH14.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH14.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH14.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH14, "NameH14");
      NameH14.BackColor = Color.FromArgb(64, 64, 64);
      NameH14.BorderStyle = BorderStyle.FixedSingle;
      NameH14.ContextMenuStrip = cmsPlayerData;
      NameH14.ForeColor = Color.White;
      NameH14.Name = "NameH14";
      NameH14.Tag = (object) "13";
      NameH14.TextChanged += new EventHandler(SimpleTextChanged);
      NameH14.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH14.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH14.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH14, "NoH14");
      NoH14.BackColor = Color.FromArgb(64, 64, 64);
      NoH14.BorderStyle = BorderStyle.FixedSingle;
      NoH14.ContextMenuStrip = cmsPlayerData;
      NoH14.ForeColor = Color.White;
      NoH14.Name = "NoH14";
      NoH14.Tag = (object) "13";
      NoH14.TextChanged += new EventHandler(SimpleTextChanged);
      NoH14.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH14.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH14.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH13, "FoulsH13");
      FoulsH13.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH13.BorderStyle = BorderStyle.FixedSingle;
      FoulsH13.ContextMenuStrip = cmsPlayerData;
      FoulsH13.ForeColor = Color.Lime;
      FoulsH13.Name = "FoulsH13";
      FoulsH13.Tag = (object) "12";
      FoulsH13.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH13.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH13.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH13.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH13, "PointsH13");
      PointsH13.BackColor = Color.FromArgb(64, 64, 64);
      PointsH13.BorderStyle = BorderStyle.FixedSingle;
      PointsH13.ContextMenuStrip = cmsPlayerData;
      PointsH13.ForeColor = Color.White;
      PointsH13.Name = "PointsH13";
      PointsH13.Tag = (object) "12";
      PointsH13.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH13.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH13.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH13.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH13, "NameH13");
      NameH13.BackColor = Color.FromArgb(64, 64, 64);
      NameH13.BorderStyle = BorderStyle.FixedSingle;
      NameH13.ContextMenuStrip = cmsPlayerData;
      NameH13.ForeColor = Color.White;
      NameH13.Name = "NameH13";
      NameH13.Tag = (object) "12";
      NameH13.TextChanged += new EventHandler(SimpleTextChanged);
      NameH13.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH13.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH13.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH13, "NoH13");
      NoH13.BackColor = Color.FromArgb(64, 64, 64);
      NoH13.BorderStyle = BorderStyle.FixedSingle;
      NoH13.ContextMenuStrip = cmsPlayerData;
      NoH13.ForeColor = Color.White;
      NoH13.Name = "NoH13";
      NoH13.Tag = (object) "12";
      NoH13.TextChanged += new EventHandler(SimpleTextChanged);
      NoH13.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH13.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH13.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH12, "FoulsH12");
      FoulsH12.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH12.BorderStyle = BorderStyle.FixedSingle;
      FoulsH12.ContextMenuStrip = cmsPlayerData;
      FoulsH12.ForeColor = Color.Lime;
      FoulsH12.Name = "FoulsH12";
      FoulsH12.Tag = (object) "11";
      FoulsH12.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH12.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH12.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH12.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH12, "PointsH12");
      PointsH12.BackColor = Color.FromArgb(64, 64, 64);
      PointsH12.BorderStyle = BorderStyle.FixedSingle;
      PointsH12.ContextMenuStrip = cmsPlayerData;
      PointsH12.ForeColor = Color.White;
      PointsH12.Name = "PointsH12";
      PointsH12.Tag = (object) "11";
      PointsH12.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH12.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH12.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH12.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH12, "NameH12");
      NameH12.BackColor = Color.FromArgb(64, 64, 64);
      NameH12.BorderStyle = BorderStyle.FixedSingle;
      NameH12.ContextMenuStrip = cmsPlayerData;
      NameH12.ForeColor = Color.White;
      NameH12.Name = "NameH12";
      NameH12.Tag = (object) "11";
      NameH12.TextChanged += new EventHandler(SimpleTextChanged);
      NameH12.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH12.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH12.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH12, "NoH12");
      NoH12.BackColor = Color.FromArgb(64, 64, 64);
      NoH12.BorderStyle = BorderStyle.FixedSingle;
      NoH12.ContextMenuStrip = cmsPlayerData;
      NoH12.ForeColor = Color.White;
      NoH12.Name = "NoH12";
      NoH12.Tag = (object) "11";
      NoH12.TextChanged += new EventHandler(SimpleTextChanged);
      NoH12.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH12.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH12.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH11, "FoulsH11");
      FoulsH11.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH11.BorderStyle = BorderStyle.FixedSingle;
      FoulsH11.ContextMenuStrip = cmsPlayerData;
      FoulsH11.ForeColor = Color.Lime;
      FoulsH11.Name = "FoulsH11";
      FoulsH11.Tag = (object) "10";
      FoulsH11.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH11.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH11.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH11.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH11, "PointsH11");
      PointsH11.BackColor = Color.FromArgb(64, 64, 64);
      PointsH11.BorderStyle = BorderStyle.FixedSingle;
      PointsH11.ContextMenuStrip = cmsPlayerData;
      PointsH11.ForeColor = Color.White;
      PointsH11.Name = "PointsH11";
      PointsH11.Tag = (object) "10";
      PointsH11.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH11.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH11.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH11.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH11, "NameH11");
      NameH11.BackColor = Color.FromArgb(64, 64, 64);
      NameH11.BorderStyle = BorderStyle.FixedSingle;
      NameH11.ContextMenuStrip = cmsPlayerData;
      NameH11.ForeColor = Color.White;
      NameH11.Name = "NameH11";
      NameH11.Tag = (object) "10";
      NameH11.TextChanged += new EventHandler(SimpleTextChanged);
      NameH11.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH11.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH11.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH11, "NoH11");
      NoH11.BackColor = Color.FromArgb(64, 64, 64);
      NoH11.BorderStyle = BorderStyle.FixedSingle;
      NoH11.ContextMenuStrip = cmsPlayerData;
      NoH11.ForeColor = Color.White;
      NoH11.Name = "NoH11";
      NoH11.Tag = (object) "10";
      NoH11.TextChanged += new EventHandler(SimpleTextChanged);
      NoH11.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH11.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH11.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH10, "FoulsH10");
      FoulsH10.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH10.BorderStyle = BorderStyle.FixedSingle;
      FoulsH10.ContextMenuStrip = cmsPlayerData;
      FoulsH10.ForeColor = Color.Lime;
      FoulsH10.Name = "FoulsH10";
      FoulsH10.Tag = (object) "9";
      FoulsH10.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH10.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH10.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH10.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH10, "PointsH10");
      PointsH10.BackColor = Color.FromArgb(64, 64, 64);
      PointsH10.BorderStyle = BorderStyle.FixedSingle;
      PointsH10.ContextMenuStrip = cmsPlayerData;
      PointsH10.ForeColor = Color.White;
      PointsH10.Name = "PointsH10";
      PointsH10.Tag = (object) "9";
      PointsH10.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH10.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH10.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH10.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH10, "NameH10");
      NameH10.BackColor = Color.FromArgb(64, 64, 64);
      NameH10.BorderStyle = BorderStyle.FixedSingle;
      NameH10.ContextMenuStrip = cmsPlayerData;
      NameH10.ForeColor = Color.White;
      NameH10.Name = "NameH10";
      NameH10.Tag = (object) "9";
      NameH10.TextChanged += new EventHandler(SimpleTextChanged);
      NameH10.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH10.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH10.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH10, "NoH10");
      NoH10.BackColor = Color.FromArgb(64, 64, 64);
      NoH10.BorderStyle = BorderStyle.FixedSingle;
      NoH10.ContextMenuStrip = cmsPlayerData;
      NoH10.ForeColor = Color.White;
      NoH10.Name = "NoH10";
      NoH10.Tag = (object) "9";
      NoH10.TextChanged += new EventHandler(SimpleTextChanged);
      NoH10.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH10.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH10.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH9, "FoulsH9");
      FoulsH9.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH9.BorderStyle = BorderStyle.FixedSingle;
      FoulsH9.ContextMenuStrip = cmsPlayerData;
      FoulsH9.ForeColor = Color.Lime;
      FoulsH9.Name = "FoulsH9";
      FoulsH9.Tag = (object) "8";
      FoulsH9.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH9.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH9.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH9.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH9, "PointsH9");
      PointsH9.BackColor = Color.FromArgb(64, 64, 64);
      PointsH9.BorderStyle = BorderStyle.FixedSingle;
      PointsH9.ContextMenuStrip = cmsPlayerData;
      PointsH9.ForeColor = Color.White;
      PointsH9.Name = "PointsH9";
      PointsH9.Tag = (object) "8";
      PointsH9.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH9.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH9.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH9.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH9, "NameH9");
      NameH9.BackColor = Color.FromArgb(64, 64, 64);
      NameH9.BorderStyle = BorderStyle.FixedSingle;
      NameH9.ContextMenuStrip = cmsPlayerData;
      NameH9.ForeColor = Color.White;
      NameH9.Name = "NameH9";
      NameH9.Tag = (object) "8";
      NameH9.TextChanged += new EventHandler(SimpleTextChanged);
      NameH9.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH9.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH9.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH9, "NoH9");
      NoH9.BackColor = Color.FromArgb(64, 64, 64);
      NoH9.BorderStyle = BorderStyle.FixedSingle;
      NoH9.ContextMenuStrip = cmsPlayerData;
      NoH9.ForeColor = Color.White;
      NoH9.Name = "NoH9";
      NoH9.Tag = (object) "8";
      NoH9.TextChanged += new EventHandler(SimpleTextChanged);
      NoH9.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH9.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH9.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH8, "FoulsH8");
      FoulsH8.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH8.BorderStyle = BorderStyle.FixedSingle;
      FoulsH8.ContextMenuStrip = cmsPlayerData;
      FoulsH8.ForeColor = Color.Lime;
      FoulsH8.Name = "FoulsH8";
      FoulsH8.Tag = (object) "7";
      FoulsH8.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH8.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH8.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH8.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH8, "PointsH8");
      PointsH8.BackColor = Color.FromArgb(64, 64, 64);
      PointsH8.BorderStyle = BorderStyle.FixedSingle;
      PointsH8.ContextMenuStrip = cmsPlayerData;
      PointsH8.ForeColor = Color.White;
      PointsH8.Name = "PointsH8";
      PointsH8.Tag = (object) "7";
      PointsH8.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH8.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH8.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH8.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH8, "NameH8");
      NameH8.BackColor = Color.FromArgb(64, 64, 64);
      NameH8.BorderStyle = BorderStyle.FixedSingle;
      NameH8.ContextMenuStrip = cmsPlayerData;
      NameH8.ForeColor = Color.White;
      NameH8.Name = "NameH8";
      NameH8.Tag = (object) "7";
      NameH8.TextChanged += new EventHandler(SimpleTextChanged);
      NameH8.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH8.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH8.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH8, "NoH8");
      NoH8.BackColor = Color.FromArgb(64, 64, 64);
      NoH8.BorderStyle = BorderStyle.FixedSingle;
      NoH8.ContextMenuStrip = cmsPlayerData;
      NoH8.ForeColor = Color.White;
      NoH8.Name = "NoH8";
      NoH8.Tag = (object) "7";
      NoH8.TextChanged += new EventHandler(SimpleTextChanged);
      NoH8.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH8.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH8.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH7, "FoulsH7");
      FoulsH7.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH7.BorderStyle = BorderStyle.FixedSingle;
      FoulsH7.ContextMenuStrip = cmsPlayerData;
      FoulsH7.ForeColor = Color.Lime;
      FoulsH7.Name = "FoulsH7";
      FoulsH7.Tag = (object) "6";
      FoulsH7.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH7.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH7.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH7.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH7, "PointsH7");
      PointsH7.BackColor = Color.FromArgb(64, 64, 64);
      PointsH7.BorderStyle = BorderStyle.FixedSingle;
      PointsH7.ContextMenuStrip = cmsPlayerData;
      PointsH7.ForeColor = Color.White;
      PointsH7.Name = "PointsH7";
      PointsH7.Tag = (object) "6";
      PointsH7.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH7.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH7.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH7.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH7, "NameH7");
      NameH7.BackColor = Color.FromArgb(64, 64, 64);
      NameH7.BorderStyle = BorderStyle.FixedSingle;
      NameH7.ContextMenuStrip = cmsPlayerData;
      NameH7.ForeColor = Color.White;
      NameH7.Name = "NameH7";
      NameH7.Tag = (object) "6";
      NameH7.TextChanged += new EventHandler(SimpleTextChanged);
      NameH7.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH7.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH7.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH7, "NoH7");
      NoH7.BackColor = Color.FromArgb(64, 64, 64);
      NoH7.BorderStyle = BorderStyle.FixedSingle;
      NoH7.ContextMenuStrip = cmsPlayerData;
      NoH7.ForeColor = Color.White;
      NoH7.Name = "NoH7";
      NoH7.Tag = (object) "6";
      NoH7.TextChanged += new EventHandler(SimpleTextChanged);
      NoH7.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH7.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH7.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH6, "FoulsH6");
      FoulsH6.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH6.BorderStyle = BorderStyle.FixedSingle;
      FoulsH6.ContextMenuStrip = cmsPlayerData;
      FoulsH6.ForeColor = Color.Lime;
      FoulsH6.Name = "FoulsH6";
      FoulsH6.Tag = (object) "5";
      FoulsH6.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH6.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH6.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH6.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH6, "PointsH6");
      PointsH6.BackColor = Color.FromArgb(64, 64, 64);
      PointsH6.BorderStyle = BorderStyle.FixedSingle;
      PointsH6.ContextMenuStrip = cmsPlayerData;
      PointsH6.ForeColor = Color.White;
      PointsH6.Name = "PointsH6";
      PointsH6.Tag = (object) "5";
      PointsH6.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH6.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH6.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH6.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH6, "NameH6");
      NameH6.BackColor = Color.FromArgb(64, 64, 64);
      NameH6.BorderStyle = BorderStyle.FixedSingle;
      NameH6.ContextMenuStrip = cmsPlayerData;
      NameH6.ForeColor = Color.White;
      NameH6.Name = "NameH6";
      NameH6.Tag = (object) "5";
      NameH6.TextChanged += new EventHandler(SimpleTextChanged);
      NameH6.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH6.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH6.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH6, "NoH6");
      NoH6.BackColor = Color.FromArgb(64, 64, 64);
      NoH6.BorderStyle = BorderStyle.FixedSingle;
      NoH6.ContextMenuStrip = cmsPlayerData;
      NoH6.ForeColor = Color.White;
      NoH6.Name = "NoH6";
      NoH6.Tag = (object) "5";
      NoH6.TextChanged += new EventHandler(SimpleTextChanged);
      NoH6.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH6.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH6.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH5, "FoulsH5");
      FoulsH5.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH5.BorderStyle = BorderStyle.FixedSingle;
      FoulsH5.ContextMenuStrip = cmsPlayerData;
      FoulsH5.ForeColor = Color.Lime;
      FoulsH5.Name = "FoulsH5";
      FoulsH5.Tag = (object) "4";
      FoulsH5.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH5.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH5.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH5.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH5, "PointsH5");
      PointsH5.BackColor = Color.FromArgb(64, 64, 64);
      PointsH5.BorderStyle = BorderStyle.FixedSingle;
      PointsH5.ContextMenuStrip = cmsPlayerData;
      PointsH5.ForeColor = Color.White;
      PointsH5.Name = "PointsH5";
      PointsH5.Tag = (object) "4";
      PointsH5.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH5.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH5.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH5.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH5, "NameH5");
      NameH5.BackColor = Color.FromArgb(64, 64, 64);
      NameH5.BorderStyle = BorderStyle.FixedSingle;
      NameH5.ContextMenuStrip = cmsPlayerData;
      NameH5.ForeColor = Color.White;
      NameH5.Name = "NameH5";
      NameH5.Tag = (object) "4";
      NameH5.TextChanged += new EventHandler(SimpleTextChanged);
      NameH5.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH5.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH5.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH5, "NoH5");
      NoH5.BackColor = Color.FromArgb(64, 64, 64);
      NoH5.BorderStyle = BorderStyle.FixedSingle;
      NoH5.ContextMenuStrip = cmsPlayerData;
      NoH5.ForeColor = Color.White;
      NoH5.Name = "NoH5";
      NoH5.Tag = (object) "4";
      NoH5.TextChanged += new EventHandler(SimpleTextChanged);
      NoH5.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH5.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH5.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH4, "FoulsH4");
      FoulsH4.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH4.BorderStyle = BorderStyle.FixedSingle;
      FoulsH4.ContextMenuStrip = cmsPlayerData;
      FoulsH4.ForeColor = Color.Lime;
      FoulsH4.Name = "FoulsH4";
      FoulsH4.Tag = (object) "3";
      FoulsH4.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH4.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH4.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH4.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH4, "PointsH4");
      PointsH4.BackColor = Color.FromArgb(64, 64, 64);
      PointsH4.BorderStyle = BorderStyle.FixedSingle;
      PointsH4.ContextMenuStrip = cmsPlayerData;
      PointsH4.ForeColor = Color.White;
      PointsH4.Name = "PointsH4";
      PointsH4.Tag = (object) "3";
      PointsH4.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH4.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH4.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH4.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH4, "NameH4");
      NameH4.BackColor = Color.FromArgb(64, 64, 64);
      NameH4.BorderStyle = BorderStyle.FixedSingle;
      NameH4.ContextMenuStrip = cmsPlayerData;
      NameH4.ForeColor = Color.White;
      NameH4.Name = "NameH4";
      NameH4.Tag = (object) "3";
      NameH4.TextChanged += new EventHandler(SimpleTextChanged);
      NameH4.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH4.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH4.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH4, "NoH4");
      NoH4.BackColor = Color.FromArgb(64, 64, 64);
      NoH4.BorderStyle = BorderStyle.FixedSingle;
      NoH4.ContextMenuStrip = cmsPlayerData;
      NoH4.ForeColor = Color.White;
      NoH4.Name = "NoH4";
      NoH4.Tag = (object) "3";
      NoH4.TextChanged += new EventHandler(SimpleTextChanged);
      NoH4.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH4.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH4.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH3, "FoulsH3");
      FoulsH3.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH3.BorderStyle = BorderStyle.FixedSingle;
      FoulsH3.ContextMenuStrip = cmsPlayerData;
      FoulsH3.ForeColor = Color.Lime;
      FoulsH3.Name = "FoulsH3";
      FoulsH3.Tag = (object) "2";
      FoulsH3.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH3.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH3.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH3.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH3, "PointsH3");
      PointsH3.BackColor = Color.FromArgb(64, 64, 64);
      PointsH3.BorderStyle = BorderStyle.FixedSingle;
      PointsH3.ContextMenuStrip = cmsPlayerData;
      PointsH3.ForeColor = Color.White;
      PointsH3.Name = "PointsH3";
      PointsH3.Tag = (object) "2";
      PointsH3.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH3.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH3.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH3.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH3, "NameH3");
      NameH3.BackColor = Color.FromArgb(64, 64, 64);
      NameH3.BorderStyle = BorderStyle.FixedSingle;
      NameH3.ContextMenuStrip = cmsPlayerData;
      NameH3.ForeColor = Color.White;
      NameH3.Name = "NameH3";
      NameH3.Tag = (object) "2";
      NameH3.TextChanged += new EventHandler(SimpleTextChanged);
      NameH3.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH3.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH3.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH3, "NoH3");
      NoH3.BackColor = Color.FromArgb(64, 64, 64);
      NoH3.BorderStyle = BorderStyle.FixedSingle;
      NoH3.ContextMenuStrip = cmsPlayerData;
      NoH3.ForeColor = Color.White;
      NoH3.Name = "NoH3";
      NoH3.Tag = (object) "2";
      NoH3.TextChanged += new EventHandler(SimpleTextChanged);
      NoH3.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH3.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH3.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH2, "FoulsH2");
      FoulsH2.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH2.BorderStyle = BorderStyle.FixedSingle;
      FoulsH2.ContextMenuStrip = cmsPlayerData;
      FoulsH2.ForeColor = Color.Lime;
      FoulsH2.Name = "FoulsH2";
      FoulsH2.Tag = (object) "1";
      FoulsH2.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH2.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH2.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH2.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH2, "PointsH2");
      PointsH2.BackColor = Color.FromArgb(64, 64, 64);
      PointsH2.BorderStyle = BorderStyle.FixedSingle;
      PointsH2.ContextMenuStrip = cmsPlayerData;
      PointsH2.ForeColor = Color.White;
      PointsH2.Name = "PointsH2";
      PointsH2.Tag = (object) "1";
      PointsH2.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH2.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH2.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH2.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH2, "NameH2");
      NameH2.BackColor = Color.FromArgb(64, 64, 64);
      NameH2.BorderStyle = BorderStyle.FixedSingle;
      NameH2.ContextMenuStrip = cmsPlayerData;
      NameH2.ForeColor = Color.White;
      NameH2.Name = "NameH2";
      NameH2.Tag = (object) "1";
      NameH2.TextChanged += new EventHandler(SimpleTextChanged);
      NameH2.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH2.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH2.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH2, "NoH2");
      NoH2.BackColor = Color.FromArgb(64, 64, 64);
      NoH2.BorderStyle = BorderStyle.FixedSingle;
      NoH2.ContextMenuStrip = cmsPlayerData;
      NoH2.ForeColor = Color.White;
      NoH2.Name = "NoH2";
      NoH2.Tag = (object) "1";
      NoH2.TextChanged += new EventHandler(SimpleTextChanged);
      NoH2.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH2.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH2.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) FoulsH1, "FoulsH1");
      FoulsH1.BackColor = Color.FromArgb(64, 64, 64);
      FoulsH1.BorderStyle = BorderStyle.FixedSingle;
      FoulsH1.ContextMenuStrip = cmsPlayerData;
      FoulsH1.ForeColor = Color.Lime;
      FoulsH1.Name = "FoulsH1";
      FoulsH1.Tag = (object) "0";
      FoulsH1.TextChanged += new EventHandler(Fouls_TextChanged);
      FoulsH1.MouseDown += new MouseEventHandler(Player_MouseDown);
      FoulsH1.MouseEnter += new EventHandler(Player_MouseEnter);
      FoulsH1.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) PointsH1, "PointsH1");
      PointsH1.BackColor = Color.FromArgb(64, 64, 64);
      PointsH1.BorderStyle = BorderStyle.FixedSingle;
      PointsH1.ContextMenuStrip = cmsPlayerData;
      PointsH1.ForeColor = Color.White;
      PointsH1.Name = "PointsH1";
      PointsH1.Tag = (object) "0";
      PointsH1.TextChanged += new EventHandler(SimpleTextChanged);
      PointsH1.MouseDown += new MouseEventHandler(Player_MouseDown);
      PointsH1.MouseEnter += new EventHandler(Player_MouseEnter);
      PointsH1.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NameH1, "NameH1");
      NameH1.BackColor = Color.FromArgb(64, 64, 64);
      NameH1.BorderStyle = BorderStyle.FixedSingle;
      NameH1.ContextMenuStrip = cmsPlayerData;
      NameH1.ForeColor = Color.White;
      NameH1.Name = "NameH1";
      NameH1.Tag = (object) "0";
      NameH1.TextChanged += new EventHandler(SimpleTextChanged);
      NameH1.MouseDown += new MouseEventHandler(Player_MouseDown);
      NameH1.MouseEnter += new EventHandler(Player_MouseEnter);
      NameH1.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) NoH1, "NoH1");
      NoH1.BackColor = Color.FromArgb(64, 64, 64);
      NoH1.BorderStyle = BorderStyle.FixedSingle;
      NoH1.ContextMenuStrip = cmsPlayerData;
      NoH1.ForeColor = Color.White;
      NoH1.Name = "NoH1";
      NoH1.Tag = (object) "0";
      NoH1.TextChanged += new EventHandler(SimpleTextChanged);
      NoH1.MouseDown += new MouseEventHandler(Player_MouseDown);
      NoH1.MouseEnter += new EventHandler(Player_MouseEnter);
      NoH1.MouseLeave += new EventHandler(Player_MouseLeave);
      componentResourceManager.ApplyResources((object) label1, "label1");
      label1.ForeColor = Color.White;
      label1.Name = "label1";
      componentResourceManager.ApplyResources((object) label2, "label2");
      label2.ForeColor = Color.White;
      label2.Name = "label2";
      componentResourceManager.ApplyResources((object) label3, "label3");
      label3.ForeColor = Color.White;
      label3.Name = "label3";
      componentResourceManager.ApplyResources((object) label5, "label5");
      label5.ForeColor = Color.White;
      label5.Name = "label5";
      componentResourceManager.ApplyResources((object) label6, "label6");
      label6.ForeColor = Color.White;
      label6.Name = "label6";
      componentResourceManager.ApplyResources((object) TimeoutTimeGuest, "TimeoutTimeGuest");
      TimeoutTimeGuest.BackColor = Color.FromArgb(64, 64, 64);
      TimeoutTimeGuest.BorderStyle = BorderStyle.FixedSingle;
      TimeoutTimeGuest.ContextMenuStrip = cmsEditTimeouts;
      TimeoutTimeGuest.ForeColor = Color.Goldenrod;
      TimeoutTimeGuest.Name = "TimeoutTimeGuest";
      TimeoutTimeGuest.Tag = (object) "";
      TimeoutTimeGuest.TextChanged += new EventHandler(SimpleTextChanged);
      componentResourceManager.ApplyResources((object) TimeoutTimeHome, "TimeoutTimeHome");
      TimeoutTimeHome.BackColor = Color.FromArgb(64, 64, 64);
      TimeoutTimeHome.BorderStyle = BorderStyle.FixedSingle;
      TimeoutTimeHome.ContextMenuStrip = cmsEditTimeouts;
      TimeoutTimeHome.ForeColor = Color.Goldenrod;
      TimeoutTimeHome.Name = "TimeoutTimeHome";
      TimeoutTimeHome.Tag = (object) "";
      TimeoutTimeHome.TextChanged += new EventHandler(SimpleTextChanged);
      componentResourceManager.ApplyResources((object) labelCaution, "labelCaution");
      labelCaution.Name = "labelCaution";
      componentResourceManager.ApplyResources((object) labelTitlePeriodTimeNotElapsed, "labelTitlePeriodTimeNotElapsed");
      labelTitlePeriodTimeNotElapsed.Name = "labelTitlePeriodTimeNotElapsed";
      componentResourceManager.ApplyResources((object) labelChangePeriodAnyway, "labelChangePeriodAnyway");
      labelChangePeriodAnyway.Name = "labelChangePeriodAnyway";
      componentResourceManager.ApplyResources((object) btnHorn, "btnHorn");
      btnHorn.ForeColor = Color.Black;
      btnHorn.Name = "btnHorn";
      btnHorn.UseVisualStyleBackColor = true;
      btnHorn.MouseDown += new MouseEventHandler(btnHorn_MouseDown);
      btnHorn.MouseUp += new MouseEventHandler(btnHorn_MouseUp);
      componentResourceManager.ApplyResources((object) cbShotclockDark, "cbShotclockDark");
      cbShotclockDark.ForeColor = Color.White;
      cbShotclockDark.Name = "cbShotclockDark";
      cbShotclockDark.UseVisualStyleBackColor = true;
      cbShotclockDark.CheckedChanged += new EventHandler(cbShotclockDark_CheckedChanged);
      componentResourceManager.ApplyResources((object) Horn, "Horn");
      Horn.Name = "Horn";
      Horn.UseWaitCursor = true;
      HornTimer.Tick += new EventHandler(HornTimer_Tick);
      componentResourceManager.ApplyResources((object) SC_Horn, "SC_Horn");
      SC_Horn.ForeColor = Color.White;
      SC_Horn.Name = "SC_Horn";
      SC_Horn.UseWaitCursor = true;
      componentResourceManager.ApplyResources((object) ActFoulPlayer, "ActFoulPlayer");
      ActFoulPlayer.BackColor = Color.FromArgb(64, 64, 64);
      ActFoulPlayer.BorderStyle = BorderStyle.FixedSingle;
      ActFoulPlayer.ContextMenuStrip = cmsEditTime;
      ActFoulPlayer.ForeColor = Color.Red;
      ActFoulPlayer.Name = "ActFoulPlayer";
      actFoulPlayerTimer.Tick += new EventHandler(actFoulPlayerTimer_Tick);
      componentResourceManager.ApplyResources((object) TeamID_Home, "TeamID_Home");
      TeamID_Home.Name = "TeamID_Home";
      TeamID_Home.UseWaitCursor = true;
      componentResourceManager.ApplyResources((object) TeamID_Guest, "TeamID_Guest");
      TeamID_Guest.Name = "TeamID_Guest";
      TeamID_Guest.UseWaitCursor = true;
      componentResourceManager.ApplyResources((object) ServiceHome, "ServiceHome");
      ServiceHome.BackColor = Color.Transparent;
      ServiceHome.BorderStyle = BorderStyle.FixedSingle;
      ServiceHome.ContextMenuStrip = cmsEditTeamFouls;
      ServiceHome.ForeColor = Color.Red;
      ServiceHome.Name = "ServiceHome";
      ServiceHome.Click += new EventHandler(ServiceGuest_Click);
      componentResourceManager.ApplyResources((object) ServiceGuest, "ServiceGuest");
      ServiceGuest.BackColor = Color.Transparent;
      ServiceGuest.BorderStyle = BorderStyle.FixedSingle;
      ServiceGuest.ContextMenuStrip = cmsEditTeamFouls;
      ServiceGuest.ForeColor = Color.Red;
      ServiceGuest.Name = "ServiceGuest";
      ServiceGuest.Click += new EventHandler(ServiceGuest_Click);
      componentResourceManager.ApplyResources((object) label7, "label7");
      label7.ForeColor = Color.White;
      label7.Name = "label7";
      componentResourceManager.ApplyResources((object) label8, "label8");
      label8.ForeColor = Color.White;
      label8.Name = "label8";
      componentResourceManager.ApplyResources((object) label9, "label9");
      label9.Name = "label9";
      componentResourceManager.ApplyResources((object) labelStartBreaktimeAnyway, "labelStartBreaktimeAnyway");
      labelStartBreaktimeAnyway.Name = "labelStartBreaktimeAnyway";
      componentResourceManager.ApplyResources((object) labelStopBreaktimeFirst, "labelStopBreaktimeFirst");
      labelStopBreaktimeFirst.Name = "labelStopBreaktimeFirst";
      componentResourceManager.ApplyResources((object) ActFoulPlayerNumberHome, "ActFoulPlayerNumberHome");
      ActFoulPlayerNumberHome.BackColor = Color.FromArgb(64, 64, 64);
      ActFoulPlayerNumberHome.BorderStyle = BorderStyle.FixedSingle;
      ActFoulPlayerNumberHome.ContextMenuStrip = cmsEditTime;
      ActFoulPlayerNumberHome.ForeColor = Color.Red;
      ActFoulPlayerNumberHome.Name = "ActFoulPlayerNumberHome";
      componentResourceManager.ApplyResources((object) ActFoulHome, "ActFoulHome");
      ActFoulHome.BackColor = Color.FromArgb(64, 64, 64);
      ActFoulHome.BorderStyle = BorderStyle.FixedSingle;
      ActFoulHome.ContextMenuStrip = cmsEditTime;
      ActFoulHome.ForeColor = Color.Red;
      ActFoulHome.Name = "ActFoulHome";
      componentResourceManager.ApplyResources((object) ActFoulGuest, "ActFoulGuest");
      ActFoulGuest.BackColor = Color.FromArgb(64, 64, 64);
      ActFoulGuest.BorderStyle = BorderStyle.FixedSingle;
      ActFoulGuest.ContextMenuStrip = cmsEditTime;
      ActFoulGuest.ForeColor = Color.Red;
      ActFoulGuest.Name = "ActFoulGuest";
      componentResourceManager.ApplyResources((object) ActFoulPlayerNumberGuest, "ActFoulPlayerNumberGuest");
      ActFoulPlayerNumberGuest.BackColor = Color.FromArgb(64, 64, 64);
      ActFoulPlayerNumberGuest.BorderStyle = BorderStyle.FixedSingle;
      ActFoulPlayerNumberGuest.ContextMenuStrip = cmsEditTime;
      ActFoulPlayerNumberGuest.ForeColor = Color.Red;
      ActFoulPlayerNumberGuest.Name = "ActFoulPlayerNumberGuest";
      componentResourceManager.ApplyResources((object) lblShotclockTenth, "lblShotclockTenth");
      lblShotclockTenth.BackColor = Color.FromArgb(64, 64, 64);
      lblShotclockTenth.ForeColor = Color.Goldenrod;
      lblShotclockTenth.Name = "lblShotclockTenth";
      componentResourceManager.ApplyResources((object) lblGameTimeTenth, "lblGameTimeTenth");
      lblGameTimeTenth.BackColor = Color.FromArgb(64, 64, 64);
      lblGameTimeTenth.ForeColor = Color.Goldenrod;
      lblGameTimeTenth.Name = "lblGameTimeTenth";
      lblGameTimeTenth.Tag = (object) "0";
      componentResourceManager.ApplyResources((object) TimeRunning, "TimeRunning");
      TimeRunning.Name = "TimeRunning";
      TimeRunning.UseWaitCursor = true;
      componentResourceManager.ApplyResources((object) GameTimeStopped, "GameTimeStopped");
      GameTimeStopped.ForeColor = Color.Red;
      GameTimeStopped.Name = "GameTimeStopped";
      GameTimeStopped.Tag = (object) "●";
      GameTimeStopped.UseWaitCursor = true;
      componentResourceManager.ApplyResources((object) cbEdgeLightAtShotclockZero, "cbEdgeLightAtShotclockZero");
      cbEdgeLightAtShotclockZero.Checked = Settings.Default.BasketballEdgeLightAtShotclockZero;
      cbEdgeLightAtShotclockZero.CheckState = CheckState.Checked;
      cbEdgeLightAtShotclockZero.DataBindings.Add(new Binding("Checked", (object) Settings.Default, "BasketballEdgeLightAtShotclockZero", true, DataSourceUpdateMode.OnPropertyChanged));
      cbEdgeLightAtShotclockZero.DataBindings.Add(new Binding("Visible", (object) Settings.Default, "BasketballEdgeLightSettingsVisible", true, DataSourceUpdateMode.OnPropertyChanged));
      cbEdgeLightAtShotclockZero.ForeColor = Color.White;
      cbEdgeLightAtShotclockZero.Name = "cbEdgeLightAtShotclockZero";
      cbEdgeLightAtShotclockZero.UseVisualStyleBackColor = true;
      cbEdgeLightAtShotclockZero.Visible = Settings.Default.BasketballEdgeLightSettingsVisible;
      cbEdgeLightAtShotclockZero.CheckedChanged += new EventHandler(cbEdgeLightAtShotclockZero_CheckedChanged);
      componentResourceManager.ApplyResources((object) SC_EdgeLightOn, "SC_EdgeLightOn");
      SC_EdgeLightOn.Name = "SC_EdgeLightOn";
      SC_EdgeLightOn.UseWaitCursor = true;
      componentResourceManager.ApplyResources((object) cbShotclockIndependentStart, "cbShotclockIndependentStart");
      cbShotclockIndependentStart.Checked = Settings.Default.BasketballStartShotclockIndependent;
      cbShotclockIndependentStart.DataBindings.Add(new Binding("Checked", (object) Settings.Default, "BasketballStartShotclockIndependent", true, DataSourceUpdateMode.OnPropertyChanged));
      cbShotclockIndependentStart.FlatAppearance.BorderColor = Color.White;
      cbShotclockIndependentStart.ForeColor = Color.White;
      cbShotclockIndependentStart.Name = "cbShotclockIndependentStart";
      cbShotclockIndependentStart.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) label10, "label10");
      label10.Name = "label10";
      componentResourceManager.ApplyResources((object) label11, "label11");
      label11.Name = "label11";
      componentResourceManager.ApplyResources((object) label12, "label12");
      label12.Name = "label12";
      componentResourceManager.ApplyResources((object) label13, "label13");
      label13.Name = "label13";
      componentResourceManager.ApplyResources((object) btnRight, "btnRight");
      btnRight.BackColor = Color.FromArgb(42, 0, 0);
      btnRight.ForeColor = Color.Black;
      btnRight.Image = (Image) Resources.NextIcon;
      btnRight.Name = "btnRight";
      btnRight.UseVisualStyleBackColor = false;
      btnRight.Click += new EventHandler(btnRight_Click);
      componentResourceManager.ApplyResources((object) btnLeft, "btnLeft");
      btnLeft.ForeColor = Color.Black;
      btnLeft.Image = (Image) Resources.PreviousIcon;
      btnLeft.Name = "btnLeft";
      btnLeft.UseVisualStyleBackColor = true;
      btnLeft.Click += new EventHandler(btnLeft_Click);
      componentResourceManager.ApplyResources((object) label14, "label14");
      label14.ForeColor = Color.DarkGray;
      label14.Name = "label14";
      componentResourceManager.ApplyResources((object) label15, "label15");
      label15.ForeColor = Color.DarkGray;
      label15.Name = "label15";
      componentResourceManager.ApplyResources((object) GBPause, "GBPause");
      GBPause.BackColor = Color.IndianRed;
      GBPause.Controls.Add((Control) BtnStartPause);
      GBPause.Controls.Add((Control) BtnAbortStartPause);
      GBPause.Controls.Add((Control) LbPauseBeginnen);
      GBPause.FlatStyle = FlatStyle.Popup;
      GBPause.ForeColor = Color.White;
      GBPause.Name = "GBPause";
      GBPause.TabStop = false;
      componentResourceManager.ApplyResources((object) BtnStartPause, "BtnStartPause");
      BtnStartPause.ForeColor = Color.Maroon;
      BtnStartPause.Name = "BtnStartPause";
      BtnStartPause.UseVisualStyleBackColor = true;
      BtnStartPause.Click += new EventHandler(BtnStartPause_Click);
      componentResourceManager.ApplyResources((object) BtnAbortStartPause, "BtnAbortStartPause");
      BtnAbortStartPause.ForeColor = Color.Maroon;
      BtnAbortStartPause.Name = "BtnAbortStartPause";
      BtnAbortStartPause.UseVisualStyleBackColor = true;
      BtnAbortStartPause.Click += new EventHandler(BtnAbortStartPause_Click);
      componentResourceManager.ApplyResources((object) LbPauseBeginnen, "LbPauseBeginnen");
      LbPauseBeginnen.Name = "LbPauseBeginnen";
      componentResourceManager.ApplyResources((object) this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.FromArgb(42, 0, 0);
      Controls.Add((Control) GBPause);
      Controls.Add((Control) btnLeft);
      Controls.Add((Control) ActFoulPlayer);
      Controls.Add((Control) btnRight);
      Controls.Add((Control) label15);
      Controls.Add((Control) label14);
      Controls.Add((Control) label13);
      Controls.Add((Control) label12);
      Controls.Add((Control) label11);
      Controls.Add((Control) label10);
      Controls.Add((Control) lblShotclockTenth);
      Controls.Add((Control) lblGameTimeTenth);
      Controls.Add((Control) ActFoulGuest);
      Controls.Add((Control) ActFoulPlayerNumberGuest);
      Controls.Add((Control) ActFoulHome);
      Controls.Add((Control) ActFoulPlayerNumberHome);
      Controls.Add((Control) label8);
      Controls.Add((Control) label7);
      Controls.Add((Control) ServiceHome);
      Controls.Add((Control) TimeoutTimeGuest);
      Controls.Add((Control) label9);
      Controls.Add((Control) labelStopBreaktimeFirst);
      Controls.Add((Control) Period);
      Controls.Add((Control) TeamNameHome);
      Controls.Add((Control) ServiceGuest);
      Controls.Add((Control) TimeOutsHome);
      Controls.Add((Control) SC_EdgeLightOn);
      Controls.Add((Control) labelStartBreaktimeAnyway);
      Controls.Add((Control) TeamFoulsGuest);
      Controls.Add((Control) labelTitlePeriodTimeNotElapsed);
      Controls.Add((Control) TeamFoulsHome);
      Controls.Add((Control) labelCaution);
      Controls.Add((Control) TimeoutTimeHome);
      Controls.Add((Control) label4);
      Controls.Add((Control) TimeOutsGuest);
      Controls.Add((Control) cbShotclockDark);
      Controls.Add((Control) FoulsG15);
      Controls.Add((Control) PointsG15);
      Controls.Add((Control) NameG15);
      Controls.Add((Control) NoG15);
      Controls.Add((Control) labelChangePeriodAnyway);
      Controls.Add((Control) FoulsG14);
      Controls.Add((Control) ScoreGuest);
      Controls.Add((Control) TeamNameGuest);
      Controls.Add((Control) PointsG14);
      Controls.Add((Control) NameG14);
      Controls.Add((Control) ScoreHome);
      Controls.Add((Control) NoG14);
      Controls.Add((Control) FoulsG13);
      Controls.Add((Control) PointsG13);
      Controls.Add((Control) NameG13);
      Controls.Add((Control) NoG13);
      Controls.Add((Control) FoulsG12);
      Controls.Add((Control) PointsG12);
      Controls.Add((Control) NameG12);
      Controls.Add((Control) NoG12);
      Controls.Add((Control) FoulsG11);
      Controls.Add((Control) PointsG11);
      Controls.Add((Control) NameG11);
      Controls.Add((Control) NoG11);
      Controls.Add((Control) ShotTime);
      Controls.Add((Control) FoulsG10);
      Controls.Add((Control) PointsG10);
      Controls.Add((Control) NameG10);
      Controls.Add((Control) NoG10);
      Controls.Add((Control) TimeRunning);
      Controls.Add((Control) GameTimeStopped);
      Controls.Add((Control) FoulsG9);
      Controls.Add((Control) PointsG9);
      Controls.Add((Control) cbStopShotclock);
      Controls.Add((Control) NameG9);
      Controls.Add((Control) NoG9);
      Controls.Add((Control) PointsG8);
      Controls.Add((Control) btnReset24);
      Controls.Add((Control) btnReset14);
      Controls.Add((Control) NameG8);
      Controls.Add((Control) btnStartStop);
      Controls.Add((Control) NoG8);
      Controls.Add((Control) FoulsG7);
      Controls.Add((Control) PointsG7);
      Controls.Add((Control) Horn);
      Controls.Add((Control) NameG7);
      Controls.Add((Control) FoulsG8);
      Controls.Add((Control) NoG7);
      Controls.Add((Control) FoulsG6);
      Controls.Add((Control) PointsG6);
      Controls.Add((Control) NameG6);
      Controls.Add((Control) NoG6);
      Controls.Add((Control) FoulsG5);
      Controls.Add((Control) PointsG5);
      Controls.Add((Control) NameG5);
      Controls.Add((Control) NoG5);
      Controls.Add((Control) FoulsG4);
      Controls.Add((Control) PointsG4);
      Controls.Add((Control) btnHorn);
      Controls.Add((Control) NameG4);
      Controls.Add((Control) NoG4);
      Controls.Add((Control) FoulsG3);
      Controls.Add((Control) PointsG3);
      Controls.Add((Control) NameG3);
      Controls.Add((Control) TeamID_Guest);
      Controls.Add((Control) NoG3);
      Controls.Add((Control) FoulsG2);
      Controls.Add((Control) PointsG2);
      Controls.Add((Control) SC_Horn);
      Controls.Add((Control) NameG2);
      Controls.Add((Control) NoG2);
      Controls.Add((Control) FoulsG1);
      Controls.Add((Control) TeamID_Home);
      Controls.Add((Control) PointsG1);
      Controls.Add((Control) NameG1);
      Controls.Add((Control) NoG1);
      Controls.Add((Control) FoulsH15);
      Controls.Add((Control) PointsH15);
      Controls.Add((Control) NameH15);
      Controls.Add((Control) NoH15);
      Controls.Add((Control) FoulsH14);
      Controls.Add((Control) PointsH14);
      Controls.Add((Control) NameH14);
      Controls.Add((Control) NoH14);
      Controls.Add((Control) FoulsH13);
      Controls.Add((Control) PointsH13);
      Controls.Add((Control) NameH13);
      Controls.Add((Control) NoH13);
      Controls.Add((Control) FoulsH12);
      Controls.Add((Control) PointsH12);
      Controls.Add((Control) NameH12);
      Controls.Add((Control) NoH12);
      Controls.Add((Control) FoulsH11);
      Controls.Add((Control) PointsH11);
      Controls.Add((Control) NameH11);
      Controls.Add((Control) NoH11);
      Controls.Add((Control) FoulsH10);
      Controls.Add((Control) PointsH10);
      Controls.Add((Control) NameH10);
      Controls.Add((Control) NoH10);
      Controls.Add((Control) FoulsH9);
      Controls.Add((Control) PointsH9);
      Controls.Add((Control) NameH9);
      Controls.Add((Control) NoH9);
      Controls.Add((Control) FoulsH8);
      Controls.Add((Control) PointsH8);
      Controls.Add((Control) NameH8);
      Controls.Add((Control) NoH8);
      Controls.Add((Control) FoulsH7);
      Controls.Add((Control) PointsH7);
      Controls.Add((Control) NameH7);
      Controls.Add((Control) NoH7);
      Controls.Add((Control) FoulsH6);
      Controls.Add((Control) PointsH6);
      Controls.Add((Control) NameH6);
      Controls.Add((Control) NoH6);
      Controls.Add((Control) FoulsH5);
      Controls.Add((Control) PointsH5);
      Controls.Add((Control) NameH5);
      Controls.Add((Control) NoH5);
      Controls.Add((Control) FoulsH4);
      Controls.Add((Control) PointsH4);
      Controls.Add((Control) NameH4);
      Controls.Add((Control) NoH4);
      Controls.Add((Control) FoulsH3);
      Controls.Add((Control) PointsH3);
      Controls.Add((Control) NameH3);
      Controls.Add((Control) NoH3);
      Controls.Add((Control) FoulsH2);
      Controls.Add((Control) PointsH2);
      Controls.Add((Control) NameH2);
      Controls.Add((Control) NoH2);
      Controls.Add((Control) FoulsH1);
      Controls.Add((Control) PointsH1);
      Controls.Add((Control) NameH1);
      Controls.Add((Control) NoH1);
      Controls.Add((Control) label1);
      Controls.Add((Control) label2);
      Controls.Add((Control) label3);
      Controls.Add((Control) label5);
      Controls.Add((Control) GameTime);
      Controls.Add((Control) label6);
      Controls.Add((Control) cbShotclockIndependentStart);
      Controls.Add((Control) cbEdgeLightAtShotclockZero);
      DataBindings.Add(new Binding("Location", (object) Settings.Default, "StartLocationBasketball", true, DataSourceUpdateMode.OnPropertyChanged));
      ForeColor = Color.Lime;
      FormBorderStyle = FormBorderStyle.FixedSingle;
      KeyPreview = true;
      Location = Settings.Default.StartLocationBasketball;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = nameof (Basketball);
      FormClosing += new FormClosingEventHandler(Basketball_FormClosing);
      Shown += new EventHandler(Basketball_Shown);
      cmsEditTimeouts.ResumeLayout(false);
      cmsEditPeriod.ResumeLayout(false);
      cmsEditTime.ResumeLayout(false);
      cmsEditTeamFouls.ResumeLayout(false);
      cmsPlayerData.ResumeLayout(false);
      cmsEditScore.ResumeLayout(false);
      GBPause.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    private delegate void clickStartStopButtonAsyncDelegate();
  }
}
