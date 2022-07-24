namespace BaSta.Model;

public interface IGameStatus
{
    public GameState Status { get; }

    public void SetGameState(GameState gamestate);
}