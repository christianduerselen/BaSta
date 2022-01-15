namespace BaSta.Model;

public interface IGame
{
    IGameClock GameClock { get; }
    IHorn Horn { get; }
    IPeriod Period { get; }
    IShotClock ShotClock { get; }
    IPossession Possession { get; }
    ITeam Home { get; }
    ITeam Guest { get; }
    ITimeoutClock TimeoutClock { get; }
}