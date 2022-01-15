namespace BaSta.Model;

public interface IPossession
{
    public PossessionState Possession { get; }

    public void SetPossession(PossessionState possession);
}