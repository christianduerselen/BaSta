namespace BaSta.Model;

public class Period : IPeriod
{
    private int _period;

    int IPeriod.Period => _period;

    void IPeriod.SetPeriod(int period)
    {
        _period = period;
    }
}