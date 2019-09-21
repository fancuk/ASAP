using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;
        Action<object> excute;
        Func<object, bool> canexecute;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            excute(parameter);
        }
        public Command(Action<object> excuteMethod,Func<object,bool> canexecuteMethod)
        {
            this.excute = excuteMethod;
            this.canexecute = canexecuteMethod;
        }
    }
}
