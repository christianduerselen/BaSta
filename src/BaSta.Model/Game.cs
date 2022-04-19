namespace BaSta.Model;

public interface IGameControl
{

}

public class Game : IGame
{
    public Game()
    {
        GameClock = new GameClock();
        Horn = new Horn();
        Period = new Period();
        ShotClock = new ShotClock();
        Possession = new Possession();
        Home = new Team();
        Guest = new Team();
        TimeoutClock = new TimeoutClock();
    }

    public IGameClock GameClock { get; }
    public IHorn Horn { get; }
    public IPeriod Period { get; }
    public IShotClock ShotClock { get; }
    public IPossession Possession { get; }
    public ITeam Home { get; }
    public ITeam Guest { get; }
    public ITimeoutClock TimeoutClock { get; }
}