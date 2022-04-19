using System;
using System.Windows.Input;

namespace BaSta.MVVM;

///<summary>
/// Base class for commands.
///</summary>
public abstract class CommandBase : ICommand
{
    ///<summary>
    /// Trigger refresh of can execute state by Command Manager.
    ///</summary>
    public void Update() { OnCanExecuteChanged(); }

    ///<summary>
    /// Trigger refresh of can execute state by Command Manager.
    ///</summary>
    public abstract void OnCanExecuteChanged();

    public abstract bool CanExecute(object parameter);

    public abstract void Execute(object parameter);

    public event EventHandler CanExecuteChanged;
}