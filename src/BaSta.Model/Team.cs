using System.Collections.Generic;

namespace BaSta.Model;

public class Team : ITeam
{
    private string _name;
    private int _points;
    private int _fouls;
    private int _timeouts;
    private List<Player> _players;

    public Team()
    {
        _players = new List<Player>();
    }

    string ITeam.Name => _name;

    int ITeam.Points => _points;

    void ITeam.SetPoints(int points)
    {
        _points = points;
    }

    int ITeam.Fouls => _fouls;

    void ITeam.SetFouls(int fouls)
    {
        _fouls = fouls;
    }

    int ITeam.Timeouts => _timeouts;

    void ITeam.SetTimeouts(int timeouts)
    {
        _timeouts = timeouts;
    }

    IReadOnlyList<IPlayer> ITeam.Players => _players;
    
    public IPlayer? GetPlayer(int index)
    {
        return index > _players.Count - 1 ? null : _players[index];
    }
}