using System.Collections.Generic;

namespace BaSta.Model;

public interface ITeam
{
    public byte[]? Logo { get; }
    void SetLogo(byte[]? logo);

    public string Name { get; }
    void SetName(string name);

    public string NameShort { get; }
    void SetNameShort(string nameShort);

    public string NameInitials { get; }
    void SetNameInitials(string nameInitials);

    public int Points { get; }
    void SetPoints(int points);
    
    public int Fouls { get; }
    void SetFouls(int fouls);
    
    public int Timeouts { get; }
    void SetTimeouts(int timeouts);

    public IReadOnlyList<IPlayer> Players { get; }
    IPlayer? GetPlayer(int index);
    void AddPlayer(IPlayer player);
    void RemovePlayer(IPlayer player);
    void UpdateTeamPoints();
    void UpdateTeamFouls();
}