using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TelerikWpfApp3.M
{
    public class Command : ICommand
    {
        Action<object> _executeMethod;
        Action<object, object> _executeMethod2;
        Func<object, bool> _canexecuteMethod;

        public Command(Action<object> executeMethod, Func<object,bool> canexecuteMethod)
        {
            this._executeMethod = executeMethod;
            this._canexecuteMethod = canexecuteMethod;
        }
        public Command(Action<object, object> executeMethod, Func<object, bool> canexecuteMethod)
        {
            this._executeMethod2 = executeMethod;
            this._canexecuteMethod = canexecuteMethod;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
           return true;
        }

        public void Execute(object parameter)
        {
            _executeMethod(parameter);
        }
    }
}
