using System;

namespace BaSta.Model;

internal class GameClock : ModelBase, IGameClock
{
    private TimeSpan _clock;
    private bool _gameClockRunning;

    public TimeSpan Clock
    {
        get => _clock;
        private set => SetField(ref _clock, value);
    }

    public void SetGameClock(TimeSpan clock)
    {
        Clock = clock;
    }

    public bool GameClockRunning
    {
        get => _gameClockRunning;
        private set => SetField(ref _gameClockRunning, value);
    }

    void IGameClock.SetGameClockRunning(bool running)
    {
        GameClockRunning = running;
    }
}