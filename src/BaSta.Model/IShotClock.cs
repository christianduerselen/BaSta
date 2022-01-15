using System;

namespace BaSta.Model;

public interface IShotClock
{
    TimeSpan? ShotClock { get; }

    void SetShotClock(TimeSpan clock);

    bool ShotClockRunning { get; }

    void SetShotClockRunning(bool running);

    bool ShotClockVisible { get; }

    void SetShotClockVisible(bool visible);

    bool ShotClockHorn { get; }

    void SetShotClockHorn(bool horn);
}