using System;

namespace BaSta.Model;

internal class GameClock : IGameClock
{
    private TimeSpan _clock;
    private bool _gameClockRunning;

    TimeSpan IGameClock.Clock => _clock;

    void IGameClock.SetGameClock(TimeSpan clock)
    {
        _clock = clock;
    }

    bool IGameClock.GameClockRunning => _gameClockRunning;

    void IGameClock.SetGameClockRunning(bool running)
    {
        _gameClockRunning = running;
    }
}