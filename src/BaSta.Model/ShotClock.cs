using System;

namespace BaSta.Model;

public class ShotClock : IShotClock
{
    private TimeSpan? _shotClock;
    private bool _shotClockRunning;
    private bool _shotClockVisible;
    private bool _shotClockHorn;

    TimeSpan? IShotClock.ShotClock => _shotClock;

    void IShotClock.SetShotClock(TimeSpan clock)
    {
        _shotClock = clock;
    }

    bool IShotClock.ShotClockRunning => _shotClockRunning;

    void IShotClock.SetShotClockRunning(bool running)
    {
        _shotClockRunning = running;
    }

    bool IShotClock.ShotClockVisible => _shotClockVisible;

    void IShotClock.SetShotClockVisible(bool visible)
    {
        _shotClockVisible = visible;
    }

    bool IShotClock.ShotClockHorn => _shotClockHorn;

    void IShotClock.SetShotClockHorn(bool horn)
    {
        _shotClockHorn = horn;
    }
}