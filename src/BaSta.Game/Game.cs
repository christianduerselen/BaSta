using System;
using System.Drawing;

namespace BaSta.Game
{
    public class Game
    {
        private int _id = -1;
        private int _gamekind_id = -1;
        private DateTime _date_time = DateTime.Now;
        private int _period = 1;
        private int _home_team_id = -1;
        private string _home_team_name = string.Empty;
        private int _guest_team_id = -1;
        private string _guest_team_name = string.Empty;
        private bool _started;
        private Image _home_team_logo;
        private Image _guest_team_logo;

        public int ID
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

        public int GameKindID
        {
            get
            {
                return _gamekind_id;
            }
            set
            {
                _gamekind_id = value;
            }
        }

        public DateTime DateTime
        {
            get
            {
                return _date_time;
            }
            set
            {
                _date_time = value;
            }
        }

        public bool Started
        {
            get
            {
                return _started;
            }
            set
            {
                _started = value;
            }
        }

        public int Period
        {
            get
            {
                return _period;
            }
            set
            {
                _period = value;
            }
        }

        public int HomeTeamID
        {
            get
            {
                return _home_team_id;
            }
            set
            {
                _home_team_id = value;
            }
        }

        public string HomeTeamName
        {
            get
            {
                return _home_team_name;
            }
            set
            {
                _home_team_name = value;
            }
        }

        public Image HomeTeamLogo
        {
            get
            {
                return _home_team_logo;
            }
            set
            {
                _home_team_logo = value;
            }
        }

        public int GuestTeamID
        {
            get
            {
                return _guest_team_id;
            }
            set
            {
                _guest_team_id = value;
            }
        }

        public string GuestTeamName
        {
            get
            {
                return _guest_team_name;
            }
            set
            {
                _guest_team_name = value;
            }
        }

        public Image GuestTeamLogo
        {
            get
            {
                return _guest_team_logo;
            }
            set
            {
                _guest_team_logo = value;
            }
        }
    }
}