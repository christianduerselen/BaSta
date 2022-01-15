namespace BaSta.Model;

public class Possession : IPossession
{
    private PossessionState _possession;

    PossessionState IPossession.Possession => _possession;

    void IPossession.SetPossession(PossessionState possession)
    {
        _possession = possession;
    }
}