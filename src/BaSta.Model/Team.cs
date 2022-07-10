using System;
using System.Collections.Generic;
using System.Linq;

namespace BaSta.Model;

public class Team : ITeam
{
    private string _name = "";
    private string _nameShort = "";
    private string _nameInitials = "";
    private int _points;
    private int _fouls;
    private int _timeouts;
    private List<IPlayer> _players;
    private byte[]? _logo;
    
    public Team()
    {
        _players = new List<IPlayer>();
    }

    public byte[]? Logo => _logo;

    public void SetLogo(byte[]? logo)
    {
        _logo = logo;
    }

    string ITeam.Name => _name;

    public void SetName(string name)
    {
        _name = name;
    }

    public string NameShort => _nameShort;

    public void SetNameShort(string nameShort)
    {
        _nameShort = nameShort;
    }

    public string NameInitials => _nameInitials;

    public void SetNameInitials(string nameInitials)
    {
        _nameInitials = nameInitials;
    }

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

    public void AddPlayer(IPlayer player)
    {
        _players.Add(player);
        _players = _players.OrderBy(player1 => player1.Number).ToList();
    }

    public void RemovePlayer(IPlayer player)
    {
        _players.Remove(player);
    }

    void ITeam.UpdateTeamPoints()
    {
        _points = _players.Sum(x => x.Points);
    }

    void ITeam.UpdateTeamFouls()
    {
            
        if(_players.Sum(x => x.Fouls) > 5)
        {
            _fouls = 5;
        }
        else
        {
            _fouls = _players.Sum(x => x.Fouls);
        }
    }
}