using System;

namespace OpenMacroBoard.Examples.Basketball24;

internal class ShotClockStateChangedEventArgs : EventArgs
{
    public ShotClockStateChangedEventArgs(Shotclock clock)
    {
        Clock = clock;
    }

    public Shotclock Clock { get; }
}