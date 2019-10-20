namespace BaSta.Game
{
    public class PeriodParameter
    {
        private string mParameterName = string.Empty;
        private long mParameterIntValue;

        public PeriodParameter()
        {
        }

        public PeriodParameter(string ParameterName, long ParameterIntValue)
        {
            mParameterName = ParameterName;
            mParameterIntValue = ParameterIntValue;
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