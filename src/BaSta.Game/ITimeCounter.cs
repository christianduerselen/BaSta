using System.Windows.Forms;

namespace BaSta.Game
{
    public interface ITimeCounter
    {
        event EndTimeReachedDelegate EndTimeReached;

        event TimeChangedDelegate TimeChanged;

        string Name { get; }

        Label Target { get; }

        bool ShowTimeInSecondsOnly { get; set; }

        bool IsRunning { get; }

        bool ShowTenth { get; set; }

        bool ShowLeadingZero { get; set; }

        bool IsWaiting { get; set; }

        void Start();

        void StartIfWaiting();

        void Stop();

        void Reset();

        void SetTime(string NewTime);

        void Dispose();
    }
}