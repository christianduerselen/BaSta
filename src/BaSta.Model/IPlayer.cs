namespace BaSta.Model;

public interface IPlayer
{
    public string Name { get; }
    void SetName(string name);

    public int Number { get; }
    void SetNumber(int number);

    public int Points { get; }
    void SetPoints(int points);
    
    public int Fouls { get; }
    void SetFouls(int fouls);
}