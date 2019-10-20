using System;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class GamePlayer
    {
        private Timer _blink_timer = new Timer();
        private int mGameID = -1;
        private int mTeamID = -1;
        private string mName = string.Empty;
        private bool _fouls_visible = true;
        private const int _blink_cycles = 10;
        private int _blink_intervals;
        private int mPlayerNumber;
        private int mPoints;
        private int mFouls;
        private byte[] mPlayerImage;
        private bool mIsReservePlayer;
        private bool mIsGoalkeeper;
        private bool mIsCoach;
        private int mGamePlayerCards;

        public event BlinkDelegate DoBlink;

        public bool FoulsVisible
        {
            get
            {
                return _fouls_visible;
            }
            set
            {
                _fouls_visible = value;
            }
        }

        public int GameID
        {
            get
            {
                return mGameID;
            }
            set
            {
                mGameID = value;
            }
        }

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

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        public int Points
        {
            get
            {
                return mPoints;
            }
            set
            {
                mPoints = value;
            }
        }

        public int Fouls
        {
            get
            {
                return mFouls;
            }
            set
            {
                if (value > mFouls)
                {
                    _blink_intervals = 10;
                    _blink_timer.Start();
                }
                mFouls = value;
            }
        }

        public int GamePlayerCards
        {
            get
            {
                return mGamePlayerCards;
            }
            set
            {
                mGamePlayerCards = value;
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

        public bool IsReservePlayer
        {
            get
            {
                return mIsReservePlayer;
            }
            set
            {
                mIsReservePlayer = value;
            }
        }

        public bool IsTrainer
        {
            get
            {
                return mPlayerNumber == -1;
            }
            set
            {
                if (!value)
                    return;
                mPlayerNumber = -1;
            }
        }

        public bool IsCoTrainer
        {
            get
            {
                return mPlayerNumber < -1;
            }
            set
            {
                if (!value)
                    return;
                mPlayerNumber = -2;
            }
        }

        public bool IsReferee
        {
            get
            {
                return mTeamID == -1;
            }
            set
            {
                mTeamID = -1;
            }
        }

        public bool IsGoalkeeper
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

        public bool IsCoach
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

        public GamePlayer(
          int GameId,
          int TeamID,
          int PlayerNumber,
          string Name,
          int Points,
          int Fouls,
          bool IsReservePlayer,
          int Cards,
          bool IsGoalKeeper,
          bool IsCoach)
        {
            mGameID = GameId;
            mTeamID = TeamID;
            mPlayerNumber = PlayerNumber;
            mName = Name;
            mPoints = Points;
            mFouls = Fouls;
            mIsReservePlayer = IsReservePlayer;
            mGamePlayerCards = Cards;
            mIsGoalkeeper = IsGoalKeeper;
            mIsCoach = IsCoach;
            _blink_timer = new Timer();
            _blink_timer.Interval = 250;
            _blink_timer.Tick += new EventHandler(_blink_timer_Tick);
        }

        private void _blink_timer_Tick(object sender, EventArgs e)
        {
            if (_blink_intervals > 0 && DoBlink != null)
            {
                if (_fouls_visible)
                {
                    _fouls_visible = false;
                }
                else
                {
                    _fouls_visible = true;
                    --_blink_intervals;
                }
                if (DoBlink == null)
                    return;
                DoBlink();
            }
            else
                _blink_timer.Stop();
        }

        public GamePlayer()
        {
            _blink_timer = new Timer();
            _blink_timer.Interval = 500;
            _blink_timer.Tick += new EventHandler(_blink_timer_Tick);
        }

        public void Dispose()
        {
            _blink_timer.Tick -= new EventHandler(_blink_timer_Tick);
            _blink_timer.Stop();
        }

        public void DontBlink()
        {
            _blink_intervals = 0;
            _fouls_visible = true;
            if (DoBlink == null)
                return;
            DoBlink();
        }
    }
}