namespace BaSta.Scoreboard
{
  public interface ITimeCounter
  {
    event EndTimeReachedDelegate EndTimeReached;

    event TimeChangedDelegate TimeChanged;

    string Name { get; }

    bool ShowTimeInSecondsOnly { get; set; }

    bool IsRunning { get; }

    bool ShowTenth { get; set; }

    void Start();

    void Stop();

    void Reset();

    void SetTime(string NewTime);

    void Dispose();
  }
}
