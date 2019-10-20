using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
  public class DownCounter : ITimeCounter
  {
    private Stopwatch _watch = new Stopwatch();
    private long _start_milliseconds = 1800000;
    private long _actual_time = 1800000;
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
        return _actual_time;
      }
      set
      {
        _corr_milliseconds = _corr_milliseconds + _actual_time - value;
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
      if (_running || _actual_time / 100L == 0L)
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
      _actual_time = _start_milliseconds;
      _running = false;
    }

    public void SetTime(string NewTime)
    {
      _corr_milliseconds = _corr_milliseconds + _actual_time - (Convert.ToInt64(NewTime.Substring(0, 2)) * 60000L + Convert.ToInt64(NewTime.Substring(3, 2)) * 1000L + Convert.ToInt64(NewTime.Substring(6, 1)) * 100L);
    }

    public string Name
    {
      get
      {
        return _name;
      }
    }

    public DownCounter(string Name)
    {
      _name = Name;
      _watch_timer.Interval = 1;
      _watch_timer.Tick += new EventHandler(_watch_timer_Tick);
      _watch_timer.Start();
    }

    public DownCounter(string Name, int StartMinutes, bool ShowTenth)
    {
      _name = Name;
      _show_tenth = ShowTenth;
      _start_milliseconds = (long) (60000 * StartMinutes);
      _actual_time = _start_milliseconds;
      _watch_timer.Interval = 1;
      _watch_timer.Tick += new EventHandler(_watch_timer_Tick);
      _watch_timer.Start();
    }

    public DownCounter(string Name, long Milliseconds, bool ShowTenth)
    {
      _name = Name;
      _show_tenth = ShowTenth;
      _start_milliseconds = Milliseconds;
      _actual_time = _start_milliseconds;
      _watch_timer.Interval = 1;
      _watch_timer.Tick += new EventHandler(_watch_timer_Tick);
      _watch_timer.Start();
    }

    private void _watch_timer_Tick(object sender, EventArgs e)
    {
      _elapsed_milliseconds = _watch.ElapsedMilliseconds + _corr_milliseconds;
      _actual_time = _start_milliseconds - _elapsed_milliseconds;
      if (_actual_time / 100L != _old_time_in_tenth)
      {
        int num1 = (int) _actual_time / 60000;
        long num2 = _actual_time % 60000L;
        int num3 = (int) num2 / 1000;
        int num4 = (int) (num2 % 1000L) / 100;
        string time = num1 <= 0 ? num3.ToString() : (!_show_time_in_seconds_only ? num1.ToString() + ":" + num3.ToString("00") : (num1 * 60 + num3).ToString());
        if (_show_tenth && num1 < 1)
          time = time + "." + num4.ToString();
        _old_time_in_tenth = _actual_time / 100L;
        if (TimeChanged != null)
          TimeChanged((ITimeCounter) this, time, _actual_time);
      }
      if ((!_show_tenth || _actual_time / 100L != 0L) && (_show_tenth || _actual_time / 1000L != 0L))
        return;
      _watch.Stop();
      if (_running && EndTimeReached != null)
        EndTimeReached((ITimeCounter) this);
      _running = false;
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
