using System.Collections.ObjectModel;
using System.Windows.Input;
using BaSta.MVVM;

namespace BaSta.Model.UI.ViewModel;

public class TeamViewModel : ViewModelBase
{
    private readonly ITeam _team;

    public TeamViewModel(ITeam team)
    {
        _team = team;
        Players = new ObservableCollection<PlayerViewModel>(team.Players.Select(x => new PlayerViewModel(x,_team)));

        Remove1PointCommand = new RelayCommand(() => Points -= 1, () => Points > 0);
        Add1PointCommand = new RelayCommand(() => Points += 1);


        //
    }

    public string Name
    {
        get => _team.Name;
        set
        {
            if (_team.Name == value)
                return;

            _team.SetName(value);
            OnPropertyChanged();
        }
    }

    public string NameShort
    {
        get => _team.NameShort;
        set
        {
            if (_team.NameShort == value)
                return;

            _team.SetNameShort(value);
            OnPropertyChanged();
        }
    }

    public string NameInitials
    {
        get => _team.NameInitials;
        set
        {
            if (_team.NameInitials == value)
                return;

            _team.SetNameInitials(value);
            OnPropertyChanged();
        }
    }

    public byte[]? Logo
    {
        get => _team.Logo;
        set
        {
            if (_team.Logo == value)
                return;

            _team.SetLogo(value);
            OnPropertyChanged();
        }
    }

    public int Points
    {
        get => _team.Points;
        set
        {
            if (_team.Points == value)
                return;

            _team.SetPoints(value);
            OnPropertyChanged();
        }
    }

    public int Fouls
    {
        get => _team.Fouls;
        set
        {
            if (_team.Fouls == value)
                return;

            _team.SetFouls(value);
            OnPropertyChanged();
        }
    }

    public int Timeouts
    {
        get => _team.Timeouts;
        set
        {
            if (_team.Timeouts == value)
                return;

            _team.SetTimeouts(value);
            OnPropertyChanged();
        }
    }

    public ObservableCollection<PlayerViewModel> Players { get; }

    public ICommand Remove1PointCommand { get; }

    public ICommand Add1PointCommand { get; }
}