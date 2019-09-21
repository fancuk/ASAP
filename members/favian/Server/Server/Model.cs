using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Server
{
    class Model
    {
        
    }
    public class Command : ICommand
    {
        Action<object> _excute;
        Func<object, bool> _canexceute;

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> excuteMethod, Func<object, bool> canexecute)
        {
            this._excute = excuteMethod;
            this._canexceute = canexecute;
        }
        public Command(Action<object> obj)
        {
            this._excute = obj;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _excute(parameter);
        }
    }
}
