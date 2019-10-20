using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
  public class UpCounter : ITimeCounter
  {
    private Stopwatch _watch = new Stopwatch();
    private long _end_milliseconds = 1800000;
    private long _old_time_in_tenth = -1;
    private Timer _watch_timer = new Timer();
    private string _name = string.Empty;
    private bool _show_tenth;
    private bool _show_time_in_seconds_only;
    private bool _running;
    private long _elapsed_milliseconds;
    private long _corr_milliseconds;

    public event EndTimeReachedDelegate EndTimeReached;

    public event TimeChangedDelegate TimeChanged;

    public long Milliseconds
    {
      get
      {
        return _elapsed_milliseconds;
      }
      set
      {
        _corr_milliseconds = _corr_milliseconds + value - _elapsed_milliseconds;
      }
    }

    public bool ShowTimeInSecondsOnly
    {
      get
      {
        return _show_time_in_seconds_only;
      }
      set
      {
        _show_time_in_seconds_only = value;
      }
    }

    public bool IsRunning
    {
      get
      {
        return _running;
      }
    }

    public bool ShowTenth
    {
      get
      {
        return _show_tenth;
      }
      set
      {
        _show_tenth = value;
      }
    }

    public void Start()
    {
      if (_running || _elapsed_milliseconds / 100L == _end_milliseconds / 100L)
        return;
      _running = true;
      _watch.Start();
    }

    public void Stop()
    {
      _running = false;
      _watch.Stop();
    }

    public void Reset()
    {
      _watch.Reset();
      _running = false;
    }

    public string Name
    {
      get
      {
        return _name;
      }
    }

    public void SetTime(string NewTime)
    {
      _corr_milliseconds = _corr_milliseconds + (Convert.ToInt64(NewTime.Substring(0, 2)) * 60000L + Convert.ToInt64(NewTime.Substring(3, 2)) * 1000L + Convert.ToInt64(NewTime.Substring(6, 1)) * 100L) - _elapsed_milliseconds;
    }

    public UpCounter(string Name)
    {
      _name = Name;
    }

    public UpCounter(string Name, int EndMinutes)
    {
      _name = Name;
      _end_milliseconds = (long) (60000 * EndMinutes);
      _watch_timer.Interval = 1;
      _watch_timer.Tick += new EventHandler(_watch_timer_Tick);
      _watch_timer.Start();
    }

    private void _watch_timer_Tick(object sender, EventArgs e)
    {
      _elapsed_milliseconds = _watch.ElapsedMilliseconds + _corr_milliseconds;
      if (_elapsed_milliseconds / 100L != _old_time_in_tenth)
      {
        int num1 = (int) _elapsed_milliseconds / 60000;
        long num2 = _elapsed_milliseconds % 60000L;
        int num3 = (int) num2 / 1000;
        int num4 = (int) (num2 % 1000L) / 100;
        string time = num1.ToString() + ":" + num3.ToString("00");
        if (_show_time_in_seconds_only)
          time = (num1 * 60 + num3).ToString();
        if (_show_tenth && num1 < 1)
          time = time + "." + num4.ToString();
        _old_time_in_tenth = _elapsed_milliseconds / 100L;
        TimeChanged((ITimeCounter) this, time, _elapsed_milliseconds);
      }
      if (_elapsed_milliseconds / 100L != _end_milliseconds / 100L)
        return;
      _watch.Stop();
      if (!_running)
        return;
      EndTimeReached((ITimeCounter) this);
    }

    public void Dispose()
    {
      _watch_timer.Stop();
      _watch_timer.Tick -= new EventHandler(_watch_timer_Tick);
      _watch_timer.Dispose();
      GC.Collect();
    }
  }
}
