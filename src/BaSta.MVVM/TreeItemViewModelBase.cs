using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace BaSta.MVVM
{
    public abstract class TreeItemViewModelBase<TTree, TItem> : ViewModelBase
        where TTree : TreeViewModelBase<TTree, TItem>
        where TItem : TreeItemViewModelBase<TTree, TItem>
    {
        #region Tree

        public TTree Tree
        {
            get => _tree ?? ParentItem?.Tree;
            internal set
            {
                if (value == _tree)
                    return;

                _tree = value;
                UpdateEffectiveExpansionMode();
            }
        }

        private TTree _tree;

        #endregion

        #region ParentItem

        public TItem ParentItem
        {
            get => _parentItem;
            private set
            {
                if (value == _parentItem)
                    return;

                _parentItem = value;
                UpdateEffectiveExpansionMode();
            }
        }

        private TItem _parentItem;

        #endregion

        #region SubItems

        //public UIObservableCollection<TItem> SubItems - VO 2020
        public ObservableCollection<TItem> SubItems
        {
            get => _subItems;
            set
            {
                if (value == _subItems)
                    return;

                if (_subItems != null)
                {
                    _subItems.CollectionChanged -= OnSubItemsCollectionChanged;
                    foreach (var subItem in _subItems)
                        subItem.ParentItem = null;
                }

                var oldHasItems = HasItems;

                _subItems = value;

                if (_subItems != null)
                {
                    foreach (var subItem in _subItems)
                    {
                        if (subItem.ParentItem != null)
                            throw new InvalidOperationException("Item from new SubItems already has a ParentItem");
                        subItem.ParentItem = (TItem)this;
                    }
                    _subItems.CollectionChanged += OnSubItemsCollectionChanged;
                }

                OnPropertyChanged();
                if (oldHasItems != HasItems)
                    OnPropertyChanged("HasItems");
            }
        }

        /// <summary>
        /// Enumerates this and all sub items using a depth first traversal.
        /// </summary>
        public IEnumerable<TItem> EnumerateItemsDepthFirst()
        {
            yield return (TItem)this;

            if (SubItems != null)
                foreach (var subItem in SubItems.SelectMany(i => i.EnumerateItemsDepthFirst()))
                    yield return subItem;
        }

        /// <summary>
        /// Enumerates this and all sub items using a breadth first traversal.
        /// </summary>
        public IEnumerable<TItem> EnumerateItemsBreadthFirst()
        {
            if (SubItems != null)
                foreach (var subItem in SubItems.SelectMany(i => i.EnumerateItemsBreadthFirst()))
                    yield return subItem;

            yield return (TItem)this;
        }

        //private UIObservableCollection<TItem> _subItems;
        private ObservableCollection<TItem> _subItems;

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
                if (Tree != null)
                    Tree.SelectedItem = _isSelected ? (TItem)this : null;

                OnPropertyChanged();
                OnIsSelectedChanged();
            }
        }

        protected virtual void OnIsSelectedChanged() { }

        private bool _isSelected;

        #endregion

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

                UpdateEffectiveExpansionMode();
            }
        }

        private TreeExpansionMode _expansionMode = TreeExpansionMode.Inherit;

        #endregion

        #region IsExpanded

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (value == _isExpanded || EffectiveExpansionMode == TreeExpansionMode.Disabled)
                    return;

                _isExpanded = value;
                OnPropertyChanged();
                OnIsExpandedChanged();

                if (EffectiveExpansionMode == TreeExpansionMode.Accordeon)
                    if (_isExpanded)
                        foreach (var sibling in Siblings)
                            sibling.IsExpanded = false;
            }
        }

        protected virtual void OnIsExpandedChanged() { }

        private bool _isExpanded;

        #endregion

        #region Derived Properties

        #region IsTopLevel

        public bool IsTopLevel => ParentItem == null;

        #endregion

        #region HasItems

        public bool HasItems => SubItems != null && SubItems.Any();

        #endregion

        #region IsFirstAtItsLevel

        public bool IsFirstAtItsLevel => ParentItem?.SubItems != null && ParentItem.SubItems.First() == this || Tree?.Items != null && Tree.Items.First() == this;

        internal void OnIsFirstAtItsLevelChanged() { OnPropertyChanged("IsFirstAtItsLevel"); }

        #endregion

        #region IsLastAtItsLevel

        public bool IsLastAtItsLevel => ParentItem?.SubItems != null && ParentItem.SubItems.Last() == this || Tree?.Items != null && Tree.Items.Last() == this;

        internal void OnIsLastAtItsLevelChanged() { OnPropertyChanged("IsLastAtItsLevel"); }

        #endregion

        #region Siblings

        public IEnumerable<TItem> Siblings
        {
            get
            {
                if (ParentItem != null)
                    return ParentItem.SubItems.Where(item => item != this);
                if (Tree != null)
                    return Tree.Items.Where(item => item != this);

                return Enumerable.Empty<TItem>();
            }
        }

        #endregion

        #region EffectiveExpansionMode

        public TreeExpansionMode EffectiveExpansionMode
        {
            get => _effectiveExpansionMode;
            private set
            {
                if (value == _effectiveExpansionMode)
                    return;

                _effectiveExpansionMode = value;
                OnPropertyChanged();

                if (SubItems != null)
                    foreach (var item in SubItems.Where(i => i.ExpansionMode == TreeExpansionMode.Inherit))
                        item.UpdateEffectiveExpansionMode();

                UpdateCommands(ExpandCommand);
            }
        }

        internal void UpdateEffectiveExpansionMode()
        {
            EffectiveExpansionMode = ExpansionMode != TreeExpansionMode.Inherit ? ExpansionMode : Tree?.ExpansionMode ?? (ParentItem?.EffectiveExpansionMode ?? ExpansionMode);
        }

        private TreeExpansionMode _effectiveExpansionMode;

        #endregion

        #endregion

        #region Commands

        #region LeftClickCommand

        public ICommand LeftClickCommand
        {
            get => _leftClickCommand;
            set
            {
                if (value == _leftClickCommand)
                    return;

                _leftClickCommand = value;
                OnPropertyChanged();
            }
        }

        private ICommand _leftClickCommand;

        #endregion

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

        #region ExpandCommand

        public ICommand ExpandCommand { get; }

        private void Expand()
        {
            IsExpanded = true;
            IsSelected = true;
        }

        private bool ExpandCanExecute()
        {
            return EffectiveExpansionMode != TreeExpansionMode.Disabled;
        }

        #endregion

        #endregion

        public void RemoveFromParentOrTree()
        {
            if (ParentItem != null)
                ParentItem.SubItems.Remove((TItem)this);
            else
                Tree?.Items.Remove((TItem)this);
        }

        protected TreeItemViewModelBase()
        {
            ExpandCommand = new RelayCommand(Expand, ExpandCanExecute);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private void OnSubItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender == SubItems)
            {
                TItem addedSelectedItem = null;

                var args = e;
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        // We may have to fire IsFirstAtItsLevel or IsLastAtItsLevel changes, depending on where new items were added
                        // and whether SubItems had items before
                        if (SubItems.Count > args.NewItems.Count)
                        {
                            if (args.NewStartingIndex == 0)
                                SubItems[args.NewStartingIndex + args.NewItems.Count].OnIsFirstAtItsLevelChanged();
                            else if (args.NewStartingIndex == SubItems.Count - args.NewItems.Count)
                                SubItems[args.NewStartingIndex - 1].OnIsLastAtItsLevelChanged();
                        }
                        // There were no items in SubItems before => HasItems changed
                        else
                            OnPropertyChanged("HasItems");

                        // Set this as ParentItem on all added items
                        foreach (TItem item in args.NewItems)
                        {
                            if (item.ParentItem != null)
                                throw new InvalidOperationException("Added Item already has a ParentItem");
                            item.ParentItem = (TItem)this;
                            if (item.IsSelected)
                                addedSelectedItem = item;
                        }

                        break;
                    case NotifyCollectionChangedAction.Move:
                        // We may have to fire IsFirstAtItsLevel or IsLastAtItsLevel changes, depending on where the items were moved
                        if (args.OldStartingIndex == 0)
                        {
                            ((TItem)args.OldItems[0]).OnIsFirstAtItsLevelChanged();
                            SubItems[0].OnIsFirstAtItsLevelChanged();
                        }
                        else if (args.NewStartingIndex == 0)
                        {
                            SubItems[0].OnIsFirstAtItsLevelChanged();
                            SubItems[args.OldItems.Count].OnIsFirstAtItsLevelChanged();
                        }

                        if (args.OldStartingIndex == SubItems.Count - args.OldItems.Count)
                        {
                            ((TItem)args.OldItems[args.OldItems.Count - 1]).OnIsLastAtItsLevelChanged();
                            SubItems.Last().OnIsLastAtItsLevelChanged();
                        }
                        else if (args.NewStartingIndex == SubItems.Count - args.OldItems.Count)
                        {
                            SubItems.Last().OnIsLastAtItsLevelChanged();
                            SubItems[SubItems.Count - args.OldItems.Count - 1].OnIsLastAtItsLevelChanged();
                        }

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        // We may have to fire IsFirstAtItsLevel or IsLastAtItsLevel changes, depending on where items were removed
                        // and whether SubItems has items now
                        if (SubItems.Count > 0)
                        {
                            if (args.OldStartingIndex == 0)
                                SubItems[0].OnIsFirstAtItsLevelChanged();
                            else if (SubItems.Count == args.OldStartingIndex)
                                SubItems.Last().OnIsLastAtItsLevelChanged();
                        }
                        // There are no items in SubItems now => HasItems changed
                        else
                            OnPropertyChanged("HasItems");

                        // Unset ParentItem on all removed items
                        foreach (TItem item in args.OldItems)
                            item.ParentItem = null;

                        break;
                    case NotifyCollectionChangedAction.Replace:
                        throw new NotImplementedException();

                    case NotifyCollectionChangedAction.Reset:
                        // We cannot unset ParentItem on the possibly removed items because they aren't given
                        // in the Reset case => Not supported yet.
                        throw new NotSupportedException("Not supported yet. Replace with new ItemsCollection instead.");
                }

                if (Tree != null)
                    if (addedSelectedItem != null)
                        Tree.SelectedItem = addedSelectedItem;
                    else if (Tree.SelectedItem != null && Tree.SelectedItem.Tree != Tree)
                        Tree.SelectedItem = null;
            }
        }
    }
}
