using System;

namespace BaSta.Model;

public interface IGameClock
{
    TimeSpan Clock { get; }

    void SetGameClock(TimeSpan clock);

    bool GameClockRunning { get; }

    void SetGameClockRunning(bool running);
}