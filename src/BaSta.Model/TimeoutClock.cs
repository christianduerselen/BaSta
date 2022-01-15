using System;

namespace BaSta.Model;

public class TimeoutClock : ITimeoutClock
{
    private TimeSpan? _timeoutClock;

    TimeSpan? ITimeoutClock.TimeoutClock => _timeoutClock;

    void ITimeoutClock.SetTimeoutClock(TimeSpan? clock)
    {
        _timeoutClock = clock;
    }
}