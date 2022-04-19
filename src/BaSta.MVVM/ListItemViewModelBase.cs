using System.Windows.Input;

namespace BaSta.MVVM
{
    public abstract class ListItemViewModelBase<TList, TItem> : ViewModelBase
        where TList : ListViewModelBase<TList, TItem>
        where TItem : ListItemViewModelBase<TList, TItem>
    {
        #region List

        public TList List { get; internal set; }

        #endregion
        
        #region IsSelected

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;

                OnIsSelectedChanged();

                var list = List;
                if (list != null)
                {
                    ++list.SuppressSelectedItemsChangedNotifications;

                    list.SelectedItem = _isSelected ? (TItem)this : null;
                    list.SelectionChanged = true;

                    --list.SuppressSelectedItemsChangedNotifications;
                }

                OnPropertyChanged();
            }
        }

        public void Select()
        {
            if (List != null)
                List.CurrentItem = (TItem)this;

            IsSelected = true;
        }

        public void ToggleSelection()
        {
            if (List != null)
                List.CurrentItem = (TItem)this;

            IsSelected = !IsSelected;
        }

        protected virtual void OnIsSelectedChanged()
        {
        }

        private bool _isSelected;

        #endregion

        #region ShowSeparator

        public bool ShowSeparator
        {
            get => _showSeparator;
            set
            {
                if (value == _showSeparator)
                    return;

                _showSeparator = value;
                OnPropertyChanged();
            }
        }

        private bool _showSeparator;

        #endregion
        
        #region Commands

        #region LeftDoubleClickCommand

        public ICommand LeftDoubleClickCommand
        {
            get => _leftDoubleClickCommand;
            set
            {
                if (value == _leftDoubleClickCommand)
                    return;

                _leftDoubleClickCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _leftDoubleClickCommand;

        #endregion

        #endregion
    }
}