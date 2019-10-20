using System.Collections.Generic;

namespace BaSta.Scoreboard
{
  public interface IScoreBoard
  {
    Sportart Sport { get; set; }

    int RefreshInterval { get; set; }

    bool RefreshTeams { set; }

    short SerialPort { get; set; }

    int Baudrate { get; set; }

    byte Brightness { get; set; }

    byte BrightnessShotclock { get; set; }

    byte Hornlevel { get; set; }

    byte HornlevelShotclock { get; set; }

    bool TimeRunning { get; set; }

    bool DoRefreshHomeTeam { get; set; }

    bool DoRefreshGuestTeam { get; set; }

    Dictionary<string, string> Fields { get; set; }

    bool ShotclockHorn { get; set; }

    bool RedLight { get; set; }

    bool Horn { get; set; }

    bool StartSending { set; }

    void SetFieldValue(string FieldName, string Value);

    void Dispose();

    void Clear();
  }
}
