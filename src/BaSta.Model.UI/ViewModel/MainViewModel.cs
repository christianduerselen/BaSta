using BaSta.MVVM;

namespace BaSta.Model.UI.ViewModel;

public class MainViewModel : ViewModelBase
{
    public MainViewModel(IGame game)
    {
        Home = new TeamViewModel(game.Home);
        Guest = new TeamViewModel(game.Guest);
    }

    public TeamViewModel Home { get; }
    public TeamViewModel Guest { get; }
}