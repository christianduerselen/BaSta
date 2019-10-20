using System;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
    public class GamePlayer
    {
        private readonly Timer _blink_timer = new Timer();
        
        private int mPlayerNumber;
        private int mFouls;

        public GamePlayer()
        {
            _blink_timer = new Timer { Interval = 500 };
            _blink_timer.Tick += _blink_timer_Tick;
        }

        public GamePlayer(int GameId, int TeamID, int PlayerNumber, string Name, int Points, int Fouls, bool IsReservePlayer, int Cards, bool IsGoalKeeper, bool IsCoach)
        {
            GameID = GameId;
            this.TeamID = TeamID;
            mPlayerNumber = PlayerNumber;
            this.Name = Name;
            this.Points = Points;
            mFouls = Fouls;
            this.IsReservePlayer = IsReservePlayer;
            GamePlayerCards = Cards;
            IsGoalkeeper = IsGoalKeeper;
            this.IsCoach = IsCoach;
            _blink_timer = new Timer();
            _blink_timer.Interval = 500;
            _blink_timer.Tick += _blink_timer_Tick;
        }

        public int GameID { get; set; } = -1;

        public int TeamID { get; set; } = -1;

        public int PlayerNumber
        {
            get => mPlayerNumber;
            set
            {
                mPlayerNumber = value;
                IsCoach = mPlayerNumber < 0;
            }
        }

        public string Name { get; set; } = string.Empty;

        public int Points { get; set; }

        public int Fouls
        {
            get => mFouls;
            set
            {
                if (value > mFouls)
                    _blink_timer.Start();

                mFouls = value;
            }
        }

        public int GamePlayerCards { get; set; }

        public bool IsReservePlayer { get; set; }

        public bool IsGoalkeeper { get; set; }

        public bool IsCoach { get; set; }

        private void _blink_timer_Tick(object sender, EventArgs e)
        {
            _blink_timer.Stop();
        }
    }
}