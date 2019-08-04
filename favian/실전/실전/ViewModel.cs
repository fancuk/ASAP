using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace 실전
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnpropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public ICommand _sign_Up { get; set; }
        public ViewModel()
        {
            _sign_Up = new Command(_execute,_canExecute);
        }
        private void _execute(object obj)
        {
            Window _sign_up = new sign_up();
            _sign_up.Show();
        }
        private bool _canExecute(object obj)
        {
            return true;
        }
    }
    public class Command : ICommand
    {
        Action<object> _execute;
        Func<object, bool> _canexecute;

        public Command(Action<object>executeMethod,Func<object,bool> canexecuteMethod)
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
