using System;
using System.Timers;

namespace BaSta.Model;

public class ShotClockStateChangedEventArgs : EventArgs
{
    public ShotClockStateChangedEventArgs(DownClock clock)
    {
        Clock = clock;
    }

    public DownClock Clock { get; }
}

public class DownClock
{
    private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(25);
    
    private readonly Timer _timer;

    public DownClock()
        : this(TimeSpan.Zero)
    {
    }

    public DownClock(TimeSpan startClock)
    {
        Clock = startClock;
        _timer = new Timer(Interval.Milliseconds);
        _timer.AutoReset = true;
        _timer.Enabled = true;
        _timer.Elapsed += OnTimer;
    }

    public TimeSpan Clock { get; private set; }

    public bool IsRunning { get; private set; }

    private void OnTimer(object state, ElapsedEventArgs elapsedEventArgs)
    {
        if (!IsRunning)
            return;

        TimeSpan timeSpan = Clock - Interval;
        if (timeSpan <= TimeSpan.Zero)
        {
            SetState(false);
            timeSpan = TimeSpan.Zero;
        }
        Set(timeSpan);
    }

    public void SetState(bool active)
    {
        if (IsRunning == active)
            return;

        IsRunning = active;
        
        if (!IsRunning)
            _timer.Stop();
        else
            _timer.Start();
        
        ClockChanged?.Invoke(this, new ShotClockStateChangedEventArgs(this));
    }

    public void Set(TimeSpan timeSpan)
    {
        if (timeSpan < TimeSpan.Zero)
            return;
        
        Clock = timeSpan;

        ClockChanged?.Invoke(this, new ShotClockStateChangedEventArgs(this));
    }

    public event EventHandler<ShotClockStateChangedEventArgs>? ClockChanged;
}

public static class DownClockExtensions
{
    public static void Set14(this DownClock clock) => clock.Set(TimeSpan.FromSeconds(14));
    public static void Set24(this DownClock clock) => clock.Set(TimeSpan.FromSeconds(24));
}