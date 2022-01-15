namespace BaSta.Model;

public class Horn : IHorn
{
    private bool _horn;

    bool IHorn.Horn => _horn;

    void IHorn.SetHorn(bool horn)
    {
        _horn = horn;
    }
}