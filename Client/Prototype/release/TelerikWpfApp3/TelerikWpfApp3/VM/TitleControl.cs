using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
            Process.GetCurrentProcess().Kill();
        }
        private void ExecuteCloseWindow(object obj)
        {
            //Task t1 = new Task(new Action(SendClose));
            //t1.Start(); t1.Wait();
            SystemCommands.CloseWindow(obj as Window);
            //Process.GetCurrentProcess().Kill();
        }

        private void SendClose()
        {
            ((App)Application.Current).CloseSocket();
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
