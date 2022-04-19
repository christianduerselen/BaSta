using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BaSta.MVVM
{
    public abstract class TreeViewModelBase<TTree, TItem> : ViewModelBase
        where TTree : TreeViewModelBase<TTree, TItem>
        where TItem : TreeItemViewModelBase<TTree, TItem>
    {
        #region Items

        //public UIObservableCollection<TItem> Items - VO 2020
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
                        item.Tree = null;
                }

                _items = value;

                if (_items != null)
                {
                    foreach (var item in _items)
                    {
                        if (item.Tree != null)
                            throw new InvalidOperationException("Item from new Items already is in another Tree");
                        if (item.ParentItem != null)
                            throw new InvalidOperationException("Item from new Items has a ParentItem");
                        item.Tree = (TTree)this;
                    }
                    _items.CollectionChanged += OnItemsCollectionChanged;
                }

                OnPropertyChanged();
                if (SelectedItem != null && SelectedItem.Tree != this)
                    SelectedItem = null;
            }
        }

        /// <summary>
        /// Enumerates all items in the tree using a depth first traversal.
        /// </summary>
        public IEnumerable<TItem> EnumerateItemsDepthFirst()
        {
            if (Items != null)
                foreach (var item in Items.SelectMany(i => i.EnumerateItemsDepthFirst()))
                    yield return item;
        }

        /// <summary>
        /// Enumerates all items in the tree using a breadth first traversal.
        /// </summary>
        public IEnumerable<TItem> EnumerateItemsBreadthFirst()
        {
            if (Items != null)
                foreach (var item in Items.SelectMany(i => i.EnumerateItemsBreadthFirst()))
                    yield return item;
        }

        //private UIObservableCollection<TItem> _items;
        private ObservableCollection<TItem> _items;

        #endregion

        #region SelectedItem

        public TItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == _selectedItem)
                    return;

                if (value != null && value.Tree != this)
                    throw new InvalidOperationException("Trying to select Item that is not in this Tree.");

                var oldSelectedItem = _selectedItem;
                if (_selectedItem != null && _selectedItem.Tree == this)
                {
                    _selectedItem = null;
                    oldSelectedItem.IsSelected = false;
                }

                _selectedItem = value;

                if (_selectedItem != null)
                    _selectedItem.IsSelected = true;

                OnPropertyChanged();
                OnSelectedItemChanged(_selectedItem, oldSelectedItem);
                InvokeSelectedItemChanged();
            }
        }

        public event EventHandler SelectedItemChanged;

        protected virtual void OnSelectedItemChanged(TItem newItem, TItem oldItem) { UpdateCommands(); }

        private void InvokeSelectedItemChanged() => SelectedItemChanged?.Invoke(this, EventArgs.Empty);

        private TItem _selectedItem;

        #endregion

        public void ExpandTo(TItem item)
        {
            if (item != null)
                for (item = item.ParentItem; item != null; item = item.ParentItem)
                    item.IsExpanded = true;
        }

        public void SelectAndExpandTo(TItem item)
        {
            SelectedItem = item;

            ExpandTo(item);
        }

        #region ExpansionMode

        public TreeExpansionMode ExpansionMode
        {
            get => _expansionMode;
            set
            {
                if (value == _expansionMode)
                    return;

                _expansionMode = value;
                OnPropertyChanged();

                if (Items != null)
                    foreach (var item in Items.Where(i => i.ExpansionMode == TreeExpansionMode.Inherit))
                        item.UpdateEffectiveExpansionMode();
            }
        }

        private TreeExpansionMode _expansionMode = TreeExpansionMode.Tree;

        #endregion

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TItem addedSelectedItem = null;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    // We may have to fire IsFirstAtItsLevel or IsLastAtItsLevel changes, depending on where new items were added
                    // and whether SubItems had items before
                    if (Items.Count > e.NewItems.Count)
                    {
                        if (e.NewStartingIndex == 0)
                            Items[e.NewStartingIndex + e.NewItems.Count].OnIsFirstAtItsLevelChanged();
                        else if (e.NewStartingIndex == Items.Count - e.NewItems.Count)
                            Items[e.NewStartingIndex - 1].OnIsLastAtItsLevelChanged();
                    }

                    // Set this as Tree on all added items
                    foreach (TItem item in e.NewItems)
                    {
                        if (item.Tree != null)
                            throw new InvalidOperationException("Added Item already is in another Tree");
                        if (item.ParentItem != null)
                            throw new InvalidOperationException("Added Item already has a ParentItem");
                        item.Tree = (TTree)this;
                        if (item.IsSelected)
                            addedSelectedItem = item;
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    // We may have to fire IsFirstAtItsLevel or IsLastAtItsLevel changes, depending on where the items were moved
                    if (e.OldStartingIndex == 0)
                    {
                        ((TItem)e.OldItems[0]).OnIsFirstAtItsLevelChanged();
                        Items[0].OnIsFirstAtItsLevelChanged();
                    }
                    else if (e.NewStartingIndex == 0)
                    {
                        Items[0].OnIsFirstAtItsLevelChanged();
                        Items[e.OldItems.Count].OnIsFirstAtItsLevelChanged();
                    }

                    if (e.OldStartingIndex == Items.Count - e.OldItems.Count)
                    {
                        ((TItem)e.OldItems[e.OldItems.Count - 1]).OnIsLastAtItsLevelChanged();
                        Items.Last().OnIsLastAtItsLevelChanged();
                    }
                    else if (e.NewStartingIndex == Items.Count - e.OldItems.Count)
                    {
                        Items.Last().OnIsLastAtItsLevelChanged();
                        Items[Items.Count - e.OldItems.Count - 1].OnIsLastAtItsLevelChanged();
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    // We may have to fire IsFirstAtItsLevel or IsLastAtItsLevel changes, depending on where items were removed
                    // and whether SubItems has items now
                    if (Items.Count > 0)
                    {
                        if (e.OldStartingIndex == 0)
                            Items[0].OnIsFirstAtItsLevelChanged();
                        else if (Items.Count == e.OldStartingIndex)
                            Items.Last().OnIsLastAtItsLevelChanged();
                    }

                    // Unset Tree on all removed items
                    foreach (TItem item in e.OldItems)
                        item.Tree = null;

                    break;
                case NotifyCollectionChangedAction.Replace:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Reset:
                    // We cannot unset Tree on the possibly removed items because they aren't given
                    // in the Reset case => Not supported yet.
                    throw new NotSupportedException("Not supported yet. Set Items property instead.");
            }

            if (addedSelectedItem != null)
                SelectedItem = addedSelectedItem;
            else if (SelectedItem != null && SelectedItem.Tree != this)
                SelectedItem = null;
        }
    }
}
