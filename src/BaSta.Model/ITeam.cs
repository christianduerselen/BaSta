using System.Collections.Generic;

namespace BaSta.Model;

public interface ITeam
{
    public string Name { get; }
    
    public int Points { get; }
    void SetPoints(int points);
    
    public int Fouls { get; }
    void SetFouls(int fouls);
    
    public int Timeouts { get; }
    void SetTimeouts(int timeouts);

    public IReadOnlyList<IPlayer> Players { get; }
    IPlayer? GetPlayer(int index);
}