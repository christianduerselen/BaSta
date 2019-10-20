namespace BaSta.Game
{
    public class Player
    {
        private int mPlayerID;
        private string mPlayerName;
        private int mPlayerNumber;
        private int mPlayerTeamID;
        private bool mPlayerIsActive;
        private byte[] mPlayerImage;
        private bool mPlayerIsReservePlayer;
        private bool mIsGoalkeeper;
        private bool mIsCoach;

        public int PlayerID
        {
            get
            {
                return mPlayerID;
            }
            set
            {
                mPlayerID = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return mPlayerName;
            }
            set
            {
                mPlayerName = value.Trim();
            }
        }

        public int PlayerNumber
        {
            get
            {
                return mPlayerNumber;
            }
            set
            {
                mPlayerNumber = value;
                mIsCoach = mPlayerNumber < 0;
            }
        }

        public int PlayerTeamID
        {
            get
            {
                return mPlayerTeamID;
            }
            set
            {
                mPlayerTeamID = value;
            }
        }

        public bool PlayerIsActive
        {
            get
            {
                return mPlayerIsActive;
            }
            set
            {
                mPlayerIsActive = value;
            }
        }

        public byte[] PlayerImage
        {
            get
            {
                return mPlayerImage;
            }
            set
            {
                mPlayerImage = value;
            }
        }

        public bool PlayerIsReservePlayer
        {
            get
            {
                return mPlayerIsReservePlayer;
            }
            set
            {
                mPlayerIsReservePlayer = value;
            }
        }

        public bool PlayerIsGoalkeeper
        {
            get
            {
                return mIsGoalkeeper;
            }
            set
            {
                mIsGoalkeeper = value;
            }
        }

        public bool PlayerIsCoach
        {
            get
            {
                return mIsCoach;
            }
            set
            {
                mIsCoach = value;
            }
        }

        public Player(
          int playerid,
          string playername,
          int playernumber,
          int playerteamid,
          bool playerisactive)
        {
            mPlayerID = playerid;
            mPlayerName = playername;
            mPlayerNumber = playernumber;
            mPlayerTeamID = playerteamid;
            mPlayerIsActive = playerisactive;
        }

        public Player()
        {
        }
    }
}