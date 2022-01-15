using System;

namespace BaSta.Model;

public interface ITimeoutClock
{
    TimeSpan? TimeoutClock { get; }

    void SetTimeoutClock(TimeSpan? clock);
}