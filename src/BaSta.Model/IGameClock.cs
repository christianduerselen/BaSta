using System;
using System.ComponentModel;

namespace BaSta.Model;

public interface IGameClock : INotifyPropertyChanged
{
    TimeSpan Clock { get; }

    void SetGameClock(TimeSpan clock);

    bool GameClockRunning { get; }

    void SetGameClockRunning(bool running);
}