namespace LIFXGUI.ViewModels
{
    using System;
    using System.Windows.Input;

    public class ParameteredCommand<T> : ICommand//WindowsUniversalCommand, IGenericCommand<T>
    {
        private readonly Action<T> _action;

        public ParameteredCommand(Action<T> action)
        {
            _action = action;
        }

        public void Execute(T parameter)
        {
            _action(parameter);
        }

        public event System.EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(T parameter)
        {
            return true;
        }


        public bool CanExecute(object parameter) => parameter is T ? CanExecute((T)parameter) : false;

        public void Execute(object parameter) => Execute((T)parameter);

#pragma warning disable 67
        //public event EventHandler CanExecuteChanged { add { } remove { } }
#pragma warning restore 67
    }

    /*

    public abstract class WindowsUniversalCommand : IWindowsUniversalCommand
    {
        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);

#if WINDOWS_UWP
    // The .NET version on Windows RT (Universal Apps) does not support the `CommandManager` trick below, so we just make a regular event and fire it.
    public event EventHandler CanExecuteChanged;
#else
        // Programmes that are not Windows Universal Apps support the `CommandManager` trick.
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
#endif

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            var handler = CanExecuteChanged;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// This method is required for Windows Universal Apps (WinRT). Non-WinRT programmes may simply ignore this method.
        /// </summary>
        public void RaiseCanExecuteChanged() => OnCanExecuteChanged(EventArgs.Empty);
    }

    /// <summary>
    /// Requires the implementation of <code>System.Windows.Input.ICommand</code> as well.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericCommand<T> : ICommand
    {
        /// <summary>
        /// A generic version of <code>System.Windows.Input.ICommand.CanExecute(object)</code>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool CanExecute(T parameter);
        /// <summary>
        /// A generic version of <code>System.Windows.Input.ICommand.Execute(object)</code>.
        /// </summary>
        /// <param name="parameter"></param>
        void Execute(T parameter);
    }

    /// <summary>
    /// Requires the implementation of <code>System.Windows.Input.ICommand</code> as well.
    /// </summary>
    interface IWindowsUniversalCommand : ICommand
    {
        /// <summary>
        /// Should refresh whether or not the <code>ICommand</code> can execute, or call <code>System.Windows.Input.ICommand.CanExecuteChanged</code>.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
    */
}
