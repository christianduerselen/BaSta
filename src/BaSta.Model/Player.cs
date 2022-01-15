namespace BaSta.Model;

internal class Player : IPlayer
{
    private string _name;
    private int _number;
    private int _points;
    private int _fouls;

    string IPlayer.Name => _name;

    void IPlayer.SetName(string name)
    {
        _name = name;
    }

    int IPlayer.Number => _number;

    void IPlayer.SetNumber(int number)
    {
        _number = number;
    }

    int IPlayer.Points => _points;

    void IPlayer.SetPoints(int points)
    {
        _points = points;
    }

    int IPlayer.Fouls => _fouls;

    void IPlayer.SetFouls(int fouls)
    {
        _fouls = fouls;
    }
}