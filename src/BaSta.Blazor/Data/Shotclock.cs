// using System;
// using System.Drawing;
// using System.Timers;
// using OpenMacroBoard.Examples.CommonStuff;
// using OpenMacroBoard.SDK;

// namespace OpenMacroBoard.Examples.Basketball24
// {
    // internal class Shotclock
    // {
        // private static readonly TimeSpan Interval = TimeSpan.FromMilliseconds(100);
        // private readonly Timer timer;
        
        // public Shotclock()
        // {
            // timer = new Timer(Interval.Milliseconds);
            // timer.AutoReset = true;
            // timer.Enabled = true;
            // timer.Elapsed += OnTimer;
            // Clock = TimeSpan.Zero;
        // }

        // public TimeSpan Clock { get; private set; }

        // public bool IsRunning {get; private set; }

        // private void OnTimer(object state, ElapsedEventArgs elapsedEventArgs)
        // {
            // if (!IsRunning)
                // return;

            // TimeSpan newClock = Clock - Interval;
            
            // if (newClock <= TimeSpan.Zero)
            // {
                // SetState(false);
                // newClock = TimeSpan.Zero;
            // }

            // Set(newClock);
        // }

        // public void SetState(bool active)
        // {
            // if (IsRunning == active)
                // return;

            // IsRunning = active;

            // if (!IsRunning)
                // timer.Stop();
            // else
                // timer.Start();
            
            // ClockChanged?.Invoke(this, new ShotClockStateChangedEventArgs(this));
        // }

        // public void Set(TimeSpan timeSpan)
        // {
            // if (timeSpan < TimeSpan.Zero)
                // return;

            // Clock = timeSpan;
            // ClockChanged?.Invoke(this, new ShotClockStateChangedEventArgs(this));
        // }

        // public void Set14() => Set(TimeSpan.FromSeconds(14));
        // public void Set24() => Set(TimeSpan.FromSeconds(24));

        // public event EventHandler<ShotClockStateChangedEventArgs> ClockChanged;
    // }

    // internal class ShotClockStateChangedEventArgs : EventArgs
    // {
        // public ShotClockStateChangedEventArgs(Shotclock clock)
        // {
            // Clock = clock;
        // }

        // public Shotclock Clock { get; }
    // }
// }