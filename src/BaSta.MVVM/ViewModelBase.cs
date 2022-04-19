using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BaSta.MVVM
{
    /// <summary>
    /// Basic ViewModel class.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when instance property with given name changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Calls Update on given CommandBase or derived non-null commands or on all CommandBase or derived typed public properties.
        /// </summary>
        public virtual void UpdateCommands(params ICommand[] commands)
        {
          //  foreach (var command in commands.Length == 0 ? ViewModelEnumerationHelper.EnumerateCommandsFromProperties(this) : commands.OfType<CommandBase>())
          //  command.Update();
        }

        /// <summary>
        /// Sets all command properties to null to prevent executing them in future.
        /// </summary>
        public virtual void UnsetCommands()
        {
            foreach (var command in GetType().GetProperties().Where(p => typeof(ICommand).IsAssignableFrom(p.PropertyType)).Where(p => p.CanWrite))
                command.SetValue(this, null, null);
        }

        /// <summary>
        /// Returns all ViewModels referenced by this ViewModel recursively in breadth-first order and each only once.
        /// </summary>
        public IEnumerable<ViewModelBase> EnumerateReferencedViewModelsBreadthFirst()
        {
            var toVisit = new Queue<ViewModelBase>(EnumerateReferencedViewModelsNonRecursive());
            var visited = new HashSet<ViewModelBase>();

            while (toVisit.Any())
            {
                var current = toVisit.Dequeue();
                if (!visited.Add(current))
                    continue;

                yield return current;

                foreach (var viewModel in current.EnumerateReferencedViewModelsNonRecursive())
                    toVisit.Enqueue(viewModel);
            }
        }

        /// <summary>
        /// Returns all ViewModels referenced by this ViewModel recursively in depth-first order and each only once.
        /// </summary>
        public IEnumerable<ViewModelBase> EnumerateReferencedViewModelsDepthFirst()
        {
            var recursionStack = new Stack<IEnumerator<ViewModelBase>>();
            recursionStack.Push(EnumerateReferencedViewModelsNonRecursive().GetEnumerator());
            var visited = new HashSet<ViewModelBase>();

            while (recursionStack.Any())
            {
                var peek = recursionStack.Peek();

                if (!peek.MoveNext())
                    recursionStack.Pop();
                else if (visited.Add(peek.Current))
                {
                    yield return peek.Current;
                    recursionStack.Push(peek.Current.EnumerateReferencedViewModelsNonRecursive().GetEnumerator());
                }
            }
        }

        /// <summary>
        /// Returns all ViewModels referenced by this ViewModel non recursive and each possibly more than once.
        /// </summary>
        protected virtual IEnumerable<ViewModelBase> EnumerateReferencedViewModelsNonRecursive()
        {
            // TODO: Vladimir 2020
            return null;
            //return ViewModelEnumerationHelper.EnumerateViewModelsFromProperties(this, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public virtual string ViewId => GetViewModelName(GetType());

        public static string GetViewModelName(Type type) => type.Name.Replace("ViewModel", "");
    }
}