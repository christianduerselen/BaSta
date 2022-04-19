using System;
using System.Diagnostics;
using System.Windows.Input;

namespace BaSta.MVVM;

///<summary>
/// Command implementation for use in viewmodels, invoking a delegate with or without parameter when the command is executed
/// as well as another delegate with or without parameter when can execute is calculated.
///</summary>
public class RelayCommand : CommandBase
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="execute">The execute.</param>
    public RelayCommand(Action execute)
        : this(p => execute(), null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="execute">The execute.</param>
    public RelayCommand(Action<object> execute)
        : this(execute, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="execute">The execute.</param>
    /// <param name="canExecute">The can execute.</param>
    public RelayCommand(Action execute, Func<bool> canExecute)
        : this(p => execute(), p => canExecute())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="execute">The execute.</param>
    /// <param name="canExecute">The can execute.</param>
    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException("execute");
        _canExecute = canExecute;

        // force a refresh of our command if suggested by command manager
        // why we need a member variable for the handler, ee static weak events: http://www.codeproject.com/Articles/29922/Weak-Events-in-C#heading0011
        //EventHandler onCanExecuteChangedHandler = (s, e) => OnCanExecuteChanged();
        //CommandManager.RequerySuggested += onCanExecuteChangedHandler;
    }

    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public event EventHandler CanExecuteChanged;

    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    /// <returns><c>true</c> if this command can be executed, otherwise <c>false</c>.</returns>
    [DebuggerStepThrough]
    public override bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    public override void Execute(object parameter)
    {
        _execute(parameter);
    }

    /// <summary>
    /// Should be called to force a refresh of the commands <see cref="ICommand.CanExecute"/> method.
    /// </summary>
    public override void OnCanExecuteChanged()
    {
        //Application.Current?.Dispatcher?.BeginInvoke((Action)(() =>
        //{
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        // }));
    }
}