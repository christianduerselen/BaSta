namespace BaSta.Scoreboard
{
  public class KindOfGame
  {
    private int mID = -1;
    private string mGameKindName = string.Empty;

    public KindOfGame(int ID, string GameKindName)
    {
      mID = ID;
      mGameKindName = GameKindName;
    }

    public int ID
    {
      get
      {
        return mID;
      }
      set
      {
        mID = value;
      }
    }

    public string GameKindName
    {
      get
      {
        return mGameKindName;
      }
      set
      {
        mGameKindName = value;
      }
    }
  }
}
