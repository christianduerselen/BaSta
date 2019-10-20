using System.Collections;

namespace BaSta.Game
{
  public class BasketballGameData : IGameData
  {
    private Hashtable _game_parameters = new Hashtable();
    private int _period = 1;
    private ITimeCounter _game_timer = (ITimeCounter) new DownCounter(nameof (GameTimer));
    private KindOfGame _game_kind;
    private Team _home_team;
    private Team _guest_team;
    private bool _service_home;
    private bool _service_guest;
    private long _gametime_milliseconds;
    private long _shottime_milliseconds;
    private int _score_home;
    private int _score_guest;
    private int _timeouts_home;
    private int _timeouts_guest;
    private long _timeout_time_milliseconds_home;
    private long _timeout_time_milliseconds_guest;
    private int _teamfouls_home;
    private int _teamfouls_guest;
    private Player[] _referees;
    private Player[] _player_home;
    private Player[] _player_guest;
    private DownCounter _shot_timer;
    private DownCounter _timeout_timer_home;
    private DownCounter _timeout_timer_guest;
    private DownCounter _break_timer;

    public event GameTimeChangedDelegate GameTimeChanged;

    public event GameTimeEndReachedDelegate GameTimeEnded;

    public event BreakTimeChangedDelegate BreakTimeChanged;

    public event BreakTimeEndReachedDelegate BreakTimeEnded;

    public event ShotTimeChangedDelegate ShotTimeChanged;

    public event ShotTimeEndReachedDelegate ShotTimeEnded;

    public event TimeoutTimeChangedDelegate TimeoutTimeChanged;

    public event TimeoutTimeEndReachedDelegate TimeoutTimeEnded;

    public DataBaseFunctions DbFunctions { get; set; }

    public Hashtable GameParameters
    {
      get
      {
        return _game_parameters;
      }
      set
      {
        _game_parameters = value;
      }
    }

    public KindOfGame GameKind
    {
      get
      {
        return _game_kind;
      }
      set
      {
        _game_kind = value;
      }
    }

    public Team HomeTeam
    {
      get
      {
        return _home_team;
      }
      set
      {
        _home_team = value;
      }
    }

    public Team GuestTeam
    {
      get
      {
        return _guest_team;
      }
      set
      {
        _guest_team = value;
      }
    }

    public int Period
    {
      get
      {
        return _period;
      }
      set
      {
        _period = value;
      }
    }

    public bool ServiceHome
    {
      get
      {
        return _service_home;
      }
      set
      {
        _service_home = value;
      }
    }

    public bool ServiceGuest
    {
      get
      {
        return _service_guest;
      }
      set
      {
        _service_guest = value;
      }
    }

    public long GameTimeMilliseconds
    {
      get
      {
        return _gametime_milliseconds;
      }
      set
      {
        _gametime_milliseconds = value;
      }
    }

    public long ShotTimeMilliseconds
    {
      get
      {
        return _shottime_milliseconds;
      }
      set
      {
        _shottime_milliseconds = value;
      }
    }

    public int ScoreHome
    {
      get
      {
        return _score_home;
      }
      set
      {
        _score_home = value;
      }
    }

    public int ScoreGuest
    {
      get
      {
        return _score_guest;
      }
      set
      {
        _score_guest = value;
      }
    }

    public int SetScoreHome
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    public int SetScoreGuest
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    public int[] SetResultHome
    {
      get
      {
        return (int[]) null;
      }
      set
      {
      }
    }

    public int[] SetResultGuest
    {
      get
      {
        return (int[]) null;
      }
      set
      {
      }
    }

    public int TimeoutsHome
    {
      get
      {
        return _timeouts_home;
      }
      set
      {
        _timeouts_home = value;
      }
    }

    public int TimeoutsGuest
    {
      get
      {
        return _timeouts_guest;
      }
      set
      {
        _timeouts_guest = value;
      }
    }

    public long TimeoutTimeMillisecondsHome
    {
      get
      {
        return _timeout_time_milliseconds_home;
      }
      set
      {
        _timeout_time_milliseconds_home = value;
      }
    }

    public long TimeoutTimeMillisecondsGuest
    {
      get
      {
        return _timeout_time_milliseconds_guest;
      }
      set
      {
        _timeout_time_milliseconds_guest = value;
      }
    }

    public int TeamFoulsHome
    {
      get
      {
        return _teamfouls_home;
      }
      set
      {
        _teamfouls_home = value;
      }
    }

    public int TeamFoulsGuest
    {
      get
      {
        return _teamfouls_guest;
      }
      set
      {
        _teamfouls_guest = value;
      }
    }

    public int PlayerChangesHome
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    public int PlayerChangesGuest
    {
      get
      {
        return 0;
      }
      set
      {
      }
    }

    public int[] PenaltyNumberHome
    {
      get
      {
        return (int[]) null;
      }
      set
      {
      }
    }

    public long[] PenaltyMillisecondsHome
    {
      get
      {
        return (long[]) null;
      }
      set
      {
      }
    }

    public int[] PenaltyNumberGuest
    {
      get
      {
        return (int[]) null;
      }
      set
      {
      }
    }

    public long[] PenaltyMillisecondsGuest
    {
      get
      {
        return (long[]) null;
      }
      set
      {
      }
    }

    public Player[] Referees
    {
      get
      {
        return _referees;
      }
      set
      {
        _referees = value;
      }
    }

    public Player[] PlayerHome
    {
      get
      {
        return _player_home;
      }
      set
      {
        _player_home = value;
      }
    }

    public Player[] PlayerGuest
    {
      get
      {
        return _player_guest;
      }
      set
      {
        _player_guest = value;
      }
    }

    public ITimeCounter GameTimer
    {
      get
      {
        return _game_timer;
      }
      set
      {
        _game_timer = value;
      }
    }

    public DownCounter ShotTimer
    {
      get
      {
        return _shot_timer;
      }
      set
      {
        _shot_timer = value;
      }
    }

    public DownCounter TimeoutTimerHome
    {
      get
      {
        return _timeout_timer_home;
      }
      set
      {
        _timeout_timer_home = value;
      }
    }

    public DownCounter TimeoutTimerGuest
    {
      get
      {
        return _timeout_timer_guest;
      }
      set
      {
        _timeout_timer_guest = value;
      }
    }

    public DownCounter[] PenaltyTimerHome
    {
      get
      {
        return (DownCounter[]) null;
      }
      set
      {
      }
    }

    public DownCounter[] PenaltyTimerGuest
    {
      get
      {
        return (DownCounter[]) null;
      }
      set
      {
      }
    }

    public DownCounter BreakTimer
    {
      get
      {
        return _break_timer;
      }
      set
      {
        _break_timer = value;
      }
    }

    public BasketballGameData(DataBaseFunctions DbFunctions)
    {
      this.DbFunctions = DbFunctions;
      _game_timer = (ITimeCounter) new DownCounter(nameof (GameTimer));
      _game_timer.TimeChanged += new TimeChangedDelegate(_game_timer_TimeChanged);
      _game_timer.EndTimeReached += new EndTimeReachedDelegate(_game_timer_EndTimeReached);
      _shot_timer = new DownCounter(nameof (ShotTimer));
      _shot_timer.TimeChanged += new TimeChangedDelegate(_shot_timer_TimeChanged);
      _shot_timer.EndTimeReached += new EndTimeReachedDelegate(_shot_timer_EndTimeReached);
      _timeout_timer_home = new DownCounter("TimeOutTimerHome");
      _timeout_timer_home.TimeChanged += new TimeChangedDelegate(_timeout_timer_home_TimeChanged);
      _timeout_timer_home.EndTimeReached += new EndTimeReachedDelegate(_timeout_timer_home_EndTimeReached);
      _timeout_timer_guest = new DownCounter("TimeOutTimerGuest");
      _timeout_timer_guest.TimeChanged += new TimeChangedDelegate(_timeout_timer_guest_TimeChanged);
      _timeout_timer_guest.EndTimeReached += new EndTimeReachedDelegate(_timeout_timer_guest_EndTimeReached);
      _break_timer = new DownCounter(nameof (BreakTimer));
      _break_timer.TimeChanged += new TimeChangedDelegate(_break_timer_TimeChanged);
      _break_timer.EndTimeReached += new EndTimeReachedDelegate(_break_timer_EndTimeReached);
    }

    public void Dispose()
    {
      _game_timer.Dispose();
      _game_timer.TimeChanged -= new TimeChangedDelegate(_game_timer_TimeChanged);
      _game_timer.EndTimeReached -= new EndTimeReachedDelegate(_game_timer_EndTimeReached);
      _shot_timer.Dispose();
      _shot_timer.TimeChanged -= new TimeChangedDelegate(_shot_timer_TimeChanged);
      _shot_timer.EndTimeReached -= new EndTimeReachedDelegate(_shot_timer_EndTimeReached);
      _timeout_timer_home.Dispose();
      _timeout_timer_home.TimeChanged -= new TimeChangedDelegate(_timeout_timer_home_TimeChanged);
      _timeout_timer_home.EndTimeReached -= new EndTimeReachedDelegate(_timeout_timer_home_EndTimeReached);
      _timeout_timer_guest.Dispose();
      _timeout_timer_guest.TimeChanged -= new TimeChangedDelegate(_timeout_timer_guest_TimeChanged);
      _timeout_timer_guest.EndTimeReached -= new EndTimeReachedDelegate(_timeout_timer_guest_EndTimeReached);
      _break_timer.Dispose();
      _break_timer.TimeChanged -= new TimeChangedDelegate(_break_timer_TimeChanged);
      _break_timer.EndTimeReached -= new EndTimeReachedDelegate(_break_timer_EndTimeReached);
    }

    private void _break_timer_EndTimeReached(ITimeCounter sender)
    {
      BreakTimeEnded((IGameData) this);
    }

    private void _break_timer_TimeChanged(ITimeCounter sender, string time, long milliseconds)
    {
      BreakTimeChanged((IGameData) this, time);
    }

    private void _timeout_timer_guest_EndTimeReached(ITimeCounter sender)
    {
      TimeoutTimeEnded((IGameData) this, false);
    }

    private void _timeout_timer_guest_TimeChanged(
      ITimeCounter sender,
      string time,
      long milliseconds)
    {
      TimeoutTimeChanged((IGameData) this, false, time);
    }

    private void _timeout_timer_home_EndTimeReached(ITimeCounter sender)
    {
      TimeoutTimeEnded((IGameData) this, true);
    }

    private void _timeout_timer_home_TimeChanged(
      ITimeCounter sender,
      string time,
      long milliseconds)
    {
      TimeoutTimeChanged((IGameData) this, true, time);
    }

    private void _shot_timer_EndTimeReached(ITimeCounter sender)
    {
      ShotTimeEnded((IGameData) this);
    }

    private void _shot_timer_TimeChanged(ITimeCounter sender, string time, long milliseconds)
    {
      ShotTimeChanged((IGameData) this, time);
    }

    private void _game_timer_EndTimeReached(ITimeCounter sender)
    {
      GameTimeEnded((IGameData) this);
    }

    private void _game_timer_TimeChanged(ITimeCounter sender, string time, long milliseconds)
    {
      GameTimeChanged((IGameData) this, time);
    }
  }
}
