namespace BaSta.Game
{
    public class KindOfGame
    {
        private int mID = -1;
        private string mGameKindName = string.Empty;
        private string mGameKindNameAlias = string.Empty;

        public KindOfGame(int ID, string GameKindName, string GameKindNameAlias)
        {
            mID = ID;
            mGameKindName = GameKindName;
            mGameKindNameAlias = GameKindNameAlias;
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

        public string GameKindNameAlias
        {
            get
            {
                return mGameKindNameAlias;
            }
            set
            {
                mGameKindNameAlias = value;
            }
        }
    }
}