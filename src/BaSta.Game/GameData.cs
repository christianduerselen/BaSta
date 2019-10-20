namespace BaSta.Game
{
    public class GameData
    {
        private int _id = -1;
        private string _name = string.Empty;
        private string _text = string.Empty;
        private long _value;

        public int GameID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public long Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        public GameData()
        {
        }

        public GameData(int GameID, string Name, long Value)
        {
            _id = GameID;
            _name = Name;
            _value = Value;
            _text = string.Empty;
        }

        public GameData(int GameID, string Name, long Value, string Text)
        {
            _id = GameID;
            _name = Name;
            _value = Value;
            _text = Text;
        }
    }
}