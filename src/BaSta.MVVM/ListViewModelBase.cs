using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace BaSta.MVVM
{
    public abstract class ListViewModelBase<TList, TItem> : ViewModelBase
        where TList : ListViewModelBase<TList, TItem>
        where TItem : ListItemViewModelBase<TList, TItem>
    {
        #region Items

        private ObservableCollection<TItem> _items;

        public ObservableCollection<TItem> Items
        {
            get => _items;
            set
            {
                if (value == _items)
                    return;

                if (_items != null)
                {
                    _items.CollectionChanged -= OnItemsCollectionChanged;
                    foreach (var item in _items)
                        item.List = null;
                }

                ++SuppressSelectedItemsChangedNotifications;

                _items = value;

                if (_items != null)
                {
                    foreach (var item in _items)
                    {
                        if (item.List != null)
                            throw new InvalidOperationException("Item from new Items already is in another List");
                        item.List = (TList)this;
                        if (item.IsSelected)
                            SelectionChanged = true;
                    }
                    _items.CollectionChanged += OnItemsCollectionChanged;
                }

                OnPropertyChanged();
                OnItemsChanged();
                if (SelectedItem != null && SelectedItem.List != this)
                    SelectedItem = null;

                --SuppressSelectedItemsChangedNotifications;

                UpdateHasItems();
            }
        }

        protected virtual void OnItemsChanged() { }

        #endregion

        #region HasItems

        public bool HasItems
        {
            get => _hasItems;
            private set
            {
                if (value == _hasItems)
                    return;

                _hasItems = value;
                OnPropertyChanged();
            }
        }

        private void UpdateHasItems()
        {
            HasItems = Items != null && Items.Count > 0;
        }

        private bool _hasItems;

        #endregion

        #region SelectionMode

        public ListSelectionMode SelectionMode
        {
            get => _selectionMode;
            set
            {
                if (value == _selectionMode)
                    return;

                _selectionMode = value;
                OnPropertyChanged();
            }
        }

        private ListSelectionMode _selectionMode;

        #endregion

        #region SelectedItem

        public event EventHandler SelectedItemChanged;

        public TItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == _selectedItem)
                    return;

                if (value != null && value.List != this)
                    throw new InvalidOperationException("Trying to select Item that is not in this List.");

                ++SuppressSelectedItemsChangedNotifications;

                var oldSelectedItem = _selectedItem;
                if (_selectedItem != null && _selectedItem.List == this && SelectionMode == ListSelectionMode.Single)
                {
                    _selectedItem = null;
                    oldSelectedItem.IsSelected = false;
                }

                _selectedItem = value;
                SelectionChanged = true;

                if (_selectedItem != null)
                    _selectedItem.IsSelected = true;

                OnPropertyChanged();
                OnSelectedItemChanged(oldSelectedItem);
                InvokeSelectedItemChanged();

                --SuppressSelectedItemsChangedNotifications;

                UpdateCommands(ClearSelectionCommand);
            }
        }

        protected virtual void OnSelectedItemChanged(TItem oldSelectedItem) { UpdateCommands(); }

        private void InvokeSelectedItemChanged() => SelectedItemChanged?.Invoke(this, EventArgs.Empty);

        private TItem _selectedItem;

        #endregion

        #region SelectedItems

        public event EventHandler SelectedItemsChanged;

        public IEnumerable<TItem> SelectedItems { get { return Items?.Where(x => x.IsSelected) ?? Enumerable.Empty<TItem>(); } }

        protected virtual void OnSelectedItemsChanged() { }

        private void InvokeSelectedItemsChanged() => SelectedItemsChanged?.Invoke(this, EventArgs.Empty);

        internal int SuppressSelectedItemsChangedNotifications
        {
            get => _suppressSelectedItemsChangedNotifications;
            set
            {
                if (_suppressSelectedItemsChangedNotifications == value)
                    return;

                _suppressSelectedItemsChangedNotifications = value;

                if (_suppressSelectedItemsChangedNotifications == 0 && SelectionChanged)
                {
                    SelectionChanged = false;

                    //if (SelectedItem == null && SelectedItems.Any() || SelectedItem != null && !SelectedItems.Any() && (!SelectedItem.IsSelected || SelectedItem.List != this))
                    //    SelectedItem = SelectedItems.FirstOrDefault();

                    OnPropertyChanged("SelectedItems");
                    OnSelectedItemsChanged();
                    InvokeSelectedItemsChanged();
                }
            }
        }

        internal bool SelectionChanged;
        private int _suppressSelectedItemsChangedNotifications;

        #endregion

        #region CurrentItem

        public TItem CurrentItem
        {
            get => _currentItem;
            set
            {
                if (value == _currentItem)
                    return;

                _currentItem = value;
                OnPropertyChanged();
            }
        }

        private TItem _currentItem;

        #endregion

        #region Commands

        public ICommand ClearSelectionCommand { get; }

        public void ClearSelection()
        {
            ++SuppressSelectedItemsChangedNotifications;

            foreach (var item in SelectedItems)
                item.IsSelected = false;

            --SuppressSelectedItemsChangedNotifications;
        }

        private bool ClearSelectionCanExecute()
        {
            return SelectedItems.Any();
        }

        #endregion

        protected ListViewModelBase()
        {
            //ClearSelectionCommand = new RelayCommand(ClearSelection, ClearSelectionCanExecute);
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ++SuppressSelectedItemsChangedNotifications;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // Set this as List on all added items
                    foreach (TItem item in e.NewItems)
                    {
                        if (item.List != null)
                            throw new InvalidOperationException("Item from new Items already is in another List");
                        item.List = (TList)this;
                        if (item.IsSelected)
                            SelectionChanged = true;
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    // Unset List on all removed items
                    foreach (TItem item in e.OldItems)
                    {
                        item.List = null;
                        if (item.IsSelected)
                            SelectionChanged = true;
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Reset:
                    // We cannot unset List on the possibly removed items because they aren't given
                    // in the Reset case => Not supported yet.
                    throw new NotSupportedException("Not supported yet. Set Items property instead.");
            }

            --SuppressSelectedItemsChangedNotifications;

            UpdateHasItems();
        }
    }
}
