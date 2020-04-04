using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Evolution.Presenter
{
    /// <summary>
    /// Copied from the MVVM article in MSDN magazine. Read more about the MVVM pattern there.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));

            _canExecute = canExecute;
        }

        public RelayCommand(Action<T> execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));

            if (canExecute != null)
                _canExecute = p => canExecute();
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = p => execute();
            if (canExecute != null)
                _canExecute = p => canExecute();
        }

        /// <author>PD-101105</author>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter = null)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter = null)
        {
            _execute((T)parameter);
        }
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> execute)
            : base(execute)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            : base(execute, canExecute)
        {
        }

        public RelayCommand(Action execute)
            : base(execute)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
            : base(execute, canExecute)
        {
        }
    }
}
