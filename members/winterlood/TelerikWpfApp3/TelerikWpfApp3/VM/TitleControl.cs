using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TelerikWpfApp3.M;

namespace TelerikWpfApp3.VM
{
    class TitleControl : INotifyPropertyChanged
    {

        public TitleControl()
        {
            test = new Command(TestExecute, CanExecute);
            minimize = new Command(ExecuteMinimize, CanExecute);
            close = new Command(ExecuteCloseWindow, CanExecute);
        }
        public ICommand test { get;set; }
        public ICommand minimize { get; set; }
        public ICommand close { get; set; }

        private void ExecuteMinimize (object obj)
        {
            SystemCommands.MinimizeWindow(obj as Window);

        }

        private void TestExecute(object obj)
        {
            MessageBox.Show("hi");
        }
        private void ExecuteCloseWindow(object obj)
        {
            SystemCommands.CloseWindow(obj as Window);
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
