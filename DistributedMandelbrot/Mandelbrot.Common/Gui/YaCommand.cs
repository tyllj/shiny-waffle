using System;
using System.Windows.Input;

namespace Mandelbrot.Common.Gui
{
    public class YaCommand : ICommand
    {
        private Func<object, bool> _canExecute;

        private Action<object> _execute;

        public YaCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public YaCommand(Action<object> execute) : this(execute, _ => true) {}
        

        public bool CanExecute(object parameter) => _canExecute.Invoke(parameter);

        public void Execute(object parameter) => _execute.Invoke(parameter);

        public event EventHandler CanExecuteChanged;
    }
}