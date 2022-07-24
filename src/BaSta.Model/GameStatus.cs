namespace BaSta.Model;

public class GameStatus : IGameStatus
{
    private GameState _gamestate;

    GameState IGameStatus.Status => _gamestate;

    void IGameStatus.SetGameState(GameState gamestate)
    {
        _gamestate = gamestate;
    }
}