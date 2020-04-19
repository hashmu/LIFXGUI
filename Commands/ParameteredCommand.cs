namespace LIFXGUI.ViewModels
{
    using System;
    using System.Windows.Input;

    public class ParameteredCommand<T> : ICommand
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
}
