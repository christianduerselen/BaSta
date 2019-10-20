using System.Collections;

namespace BaSta.Scoreboard
{
  public class MatchData
  {
    public interface IGame
    {
      int MaxNormalPeriods { get; set; }

      int MaxProlongationPeriods { get; set; }

      long StartTime { get; set; }

      long StopTime { get; set; }
    }

    public interface IPlayer
    {
      int Number { get; set; }

      string Name { get; set; }

      int Points { get; set; }

      int Fouls { get; set; }
    }

    public class BasketballPlayer : MatchData.IPlayer
    {
      private string _name = string.Empty;
      private int _number;
      private int _points;
      private int _fouls;

      public BasketballPlayer(int Number, string Name, int Points, int Fouls)
      {
        _number = Number;
        _name = Name;
        _points = Points;
        _fouls = Fouls;
      }

      public int Number
      {
        get
        {
          return _number;
        }
        set
        {
          _number = value;
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

      public int Points
      {
        get
        {
          return _points;
        }
        set
        {
          _points = value;
        }
      }

      public int Fouls
      {
        get
        {
          return _fouls;
        }
        set
        {
          _fouls = value;
        }
      }
    }

    public class HandballPlayer : MatchData.IPlayer
    {
      private string _name = string.Empty;
      private int _number;
      private int _points;

      public int Number
      {
        get
        {
          return _number;
        }
        set
        {
          _number = value;
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

      public int Points
      {
        get
        {
          return _points;
        }
        set
        {
          _points = value;
        }
      }

      public int Fouls
      {
        get
        {
          return 0;
        }
        set
        {
        }
      }
    }

    public class VolleyballPlayer : MatchData.IPlayer
    {
      private string _name = string.Empty;
      private int _number;

      public int Number
      {
        get
        {
          return _number;
        }
        set
        {
          _number = value;
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

      public int Points
      {
        get
        {
          return 0;
        }
        set
        {
        }
      }

      public int Fouls
      {
        get
        {
          return 0;
        }
        set
        {
        }
      }
    }

    public enum Mannschaft
    {
      Heim,
      Gast,
    }

    public interface ITeam
    {
      MatchData.Mannschaft Team { get; set; }

      int MaxPlayers { get; set; }

      string Name { get; set; }

      ArrayList Players { get; set; }

      int Points { get; set; }

      int TeamFouls { get; set; }

      int Timeouts { get; set; }

      bool Bonus { get; set; }
    }

    public class BasketballTeam : MatchData.ITeam
    {
      private int _max_players = 12;
      private string _name = string.Empty;
      private ArrayList _players = new ArrayList();
      private MatchData.Mannschaft _team;
      private int _points;
      private int _teamfouls;
      private int _timeouts;
      private bool _bonus;

      public BasketballTeam()
      {
      }

      public BasketballTeam(MatchData.Mannschaft Team, string Name, ArrayList Players)
      {
        _team = Team;
        _name = Name;
        _players = Players;
      }

      public MatchData.Mannschaft Team
      {
        get
        {
          return _team;
        }
        set
        {
          _team = value;
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

      public ArrayList Players
      {
        get
        {
          return _players;
        }
        set
        {
          _players = value;
        }
      }

      public int MaxPlayers
      {
        get
        {
          return _max_players;
        }
        set
        {
          _max_players = value;
        }
      }

      public int Points
      {
        get
        {
          return _points;
        }
        set
        {
          _points = value;
        }
      }

      public int TeamFouls
      {
        get
        {
          return _teamfouls;
        }
        set
        {
          _teamfouls = value;
        }
      }

      public int Timeouts
      {
        get
        {
          return _timeouts;
        }
        set
        {
          _timeouts = value;
        }
      }

      public bool Bonus
      {
        get
        {
          return _bonus;
        }
        set
        {
          _bonus = value;
        }
      }
    }

    public class HandballTeam : MatchData.ITeam
    {
      private string _name = string.Empty;
      private int _max_players = 14;
      private ArrayList _players = new ArrayList();
      private MatchData.Mannschaft _team;
      private int _points;
      private int _timeouts;
      private bool _bonus;

      public HandballTeam()
      {
      }

      public HandballTeam(MatchData.Mannschaft Team, string Name, ArrayList Players)
      {
        _team = Team;
        _name = Name;
        _players = Players;
      }

      public int MaxPlayers
      {
        get
        {
          return _max_players;
        }
        set
        {
          _max_players = value;
        }
      }

      public MatchData.Mannschaft Team
      {
        get
        {
          return _team;
        }
        set
        {
          _team = value;
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

      public ArrayList Players
      {
        get
        {
          return _players;
        }
        set
        {
          _players = value;
        }
      }

      public int Points
      {
        get
        {
          return _points;
        }
        set
        {
          _points = value;
        }
      }

      public int TeamFouls
      {
        get
        {
          return 0;
        }
        set
        {
        }
      }

      public int Timeouts
      {
        get
        {
          return _timeouts;
        }
        set
        {
          _timeouts = value;
        }
      }

      public bool Bonus
      {
        get
        {
          return _bonus;
        }
        set
        {
          _bonus = value;
        }
      }
    }

    public class VolleyballTeam : MatchData.ITeam
    {
      private string _name = string.Empty;
      private int _max_players = 12;
      private ArrayList _players = new ArrayList();
      private MatchData.Mannschaft _team;
      private int _points;
      private int _timeouts;
      private bool _bonus;

      public VolleyballTeam()
      {
      }

      public VolleyballTeam(MatchData.Mannschaft Team, string Name, ArrayList Players)
      {
        _team = Team;
        _name = Name;
        _players = Players;
      }

      public int MaxPlayers
      {
        get
        {
          return _max_players;
        }
        set
        {
          _max_players = value;
        }
      }

      public MatchData.Mannschaft Team
      {
        get
        {
          return _team;
        }
        set
        {
          _team = value;
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

      public ArrayList Players
      {
        get
        {
          return _players;
        }
        set
        {
          _players = value;
        }
      }

      public int Points
      {
        get
        {
          return _points;
        }
        set
        {
          _points = value;
        }
      }

      public int TeamFouls
      {
        get
        {
          return 0;
        }
        set
        {
        }
      }

      public int Timeouts
      {
        get
        {
          return _timeouts;
        }
        set
        {
          _timeouts = value;
        }
      }

      public bool Bonus
      {
        get
        {
          return _bonus;
        }
        set
        {
          _bonus = value;
        }
      }
    }
  }
}
