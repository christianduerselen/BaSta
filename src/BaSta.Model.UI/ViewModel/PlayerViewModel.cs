using System.Windows.Input;
using BaSta.MVVM;

namespace BaSta.Model.UI.ViewModel;

public class PlayerViewModel : ViewModelBase
{
    private readonly IPlayer _player;
    private ITeam _team;

    public PlayerViewModel(IPlayer player, ITeam team)
    {
        _player = player;
        _team = team;

        Remove1FoulCommand = new RelayCommand(() => Fouls -= 1, () => Fouls > 0);
        Add1FoulCommand = new RelayCommand(() => Fouls += 1, () => Fouls < 5);

        Remove1PointCommand = new RelayCommand(() => Points -= 1, () => Points > 0);
        Add1PointCommand = new RelayCommand(() => Points += 1);
        Add2PointCommand = new RelayCommand(() => Points += 2);
        Add3PointCommand = new RelayCommand(() => Points += 3);
        
    }

    public int Number
    {
        get => _player.Number;
        set
        {
            if (_player.Number == value)
                return;

            _player.SetNumber(value);
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _player.Name;
        set
        {
            if (_player.Name == value)
                return;

            _player.SetName(value);
            OnPropertyChanged();
        }
    }

    public int Points
    {
        get => _player.Points;
        set
        {
            if (_player.Points == value)
                return;

            _player.SetPoints(value);
            _team.UpdateTeamPoints();
            OnPropertyChanged();
        }
    }

    public int Fouls
    {
        get => _player.Fouls;
        set
        {
            if (_player.Fouls == value)
                return;

            _player.SetFouls(value);
            _team.UpdateTeamFouls();
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsDisqualified));
        }
    }

    public bool IsDisqualified => Fouls == 5;

    public ICommand Add1FoulCommand { get; }

    public ICommand Remove1FoulCommand { get; }

    public ICommand Remove1PointCommand { get; }

    public ICommand Add1PointCommand { get; }

    public ICommand Add2PointCommand { get; }

    public ICommand Add3PointCommand { get; }

}