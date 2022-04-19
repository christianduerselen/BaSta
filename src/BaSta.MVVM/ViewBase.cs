using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.AspNetCore.Components;

namespace BaSta.MVVM;

public class ViewBase<T> : ComponentBase, IDisposable where T : ViewModelBase
{
    [Parameter]
    public T DataContext { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (DataContext is INotifyPropertyChanged inps)
            inps.PropertyChanged += InvokeStateChanged;

        if (DataContext is ICommand icmd)
            icmd.CanExecuteChanged += InvokeStateChanged;

        if (DataContext is INotifyCollectionChanged cc)
            cc.CollectionChanged += InvokeStateChanged;
    }

    /*protected override void OnParametersSet()
    {
        base.OnParametersSet();
    }*/

    private void InvokeStateChanged(object sender, EventArgs e) // (object? sender, EventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (DataContext is INotifyPropertyChanged inps)
                inps.PropertyChanged -= InvokeStateChanged;

            if (DataContext is ICommand icmd)
                icmd.CanExecuteChanged -= InvokeStateChanged;

            if (DataContext is INotifyCollectionChanged cc)
                cc.CollectionChanged -= InvokeStateChanged;
        }
    }

    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}