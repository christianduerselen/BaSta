namespace BaSta.Game
{
    public class GameParameter
    {
        private int mGamekindID = -1;
        private int mPeriod = 1;
        private string mParameterName = string.Empty;
        private long mParameterIntValue;

        public GameParameter()
        {
        }

        public GameParameter(int GamekindID, int Period, string ParameterName, long ParameterIntValue)
        {
            mGamekindID = GamekindID;
            mPeriod = Period;
            mParameterName = ParameterName;
            mParameterIntValue = ParameterIntValue;
        }

        public int GameKindID
        {
            get
            {
                return mGamekindID;
            }
            set
            {
                mGamekindID = value;
            }
        }

        public int Period
        {
            get
            {
                return mPeriod;
            }
            set
            {
                mPeriod = value;
            }
        }

        public string ParameterName
        {
            get
            {
                return mParameterName;
            }
            set
            {
                mParameterName = value;
            }
        }

        public long ParameterIntValue
        {
            get
            {
                return mParameterIntValue;
            }
            set
            {
                mParameterIntValue = value;
            }
        }
    }
}