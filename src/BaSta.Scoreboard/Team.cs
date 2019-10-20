namespace BaSta.Scoreboard
{
  public class Team
  {
    private int mTeamID = -1;
    private int mTeamGameKindID = -1;
    private string mTeamName = string.Empty;
    private byte[] mTeamLogo;

    public int TeamID
    {
      get
      {
        return mTeamID;
      }
      set
      {
        mTeamID = value;
      }
    }

    public int TeamGameKindID
    {
      get
      {
        return mTeamGameKindID;
      }
      set
      {
        mTeamGameKindID = value;
      }
    }

    public byte[] TeamLogo
    {
      get
      {
        return mTeamLogo;
      }
      set
      {
        mTeamLogo = value;
      }
    }

    public string TeamName
    {
      get
      {
        return mTeamName;
      }
      set
      {
        mTeamName = value;
      }
    }

    public Team(int teamid, string teamname)
    {
      mTeamID = teamid;
      mTeamName = teamname;
    }

    public Team()
    {
    }
  }
}
