using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TelerikWpfApp3.ViewModel.Command
{
    public class MessageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action _execute;

        public MessageCommand(Action execute)
        {
            this._execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}
