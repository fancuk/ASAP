using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace 실전
{
    class Model
    {

    }
    public class Command : ICommand
    {
        Action<object> _execute;
        Func<object, bool> _canexecute;

        public Command(Action<object> executeMethod, Func<object, bool> canexecuteMethod)
        {
            this._execute = executeMethod;
            this._canexecute = canexecuteMethod;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
