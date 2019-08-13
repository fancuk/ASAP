using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace viewModel
{
    class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action<object> execute;
        Func<object, bool> canExcute;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public Command(Action<object> executeMethod, Func<object, bool> canexecuteMethod)
        {
            this.canExcute = canexecuteMethod;
            this.execute = executeMethod;
        }
    }
}
