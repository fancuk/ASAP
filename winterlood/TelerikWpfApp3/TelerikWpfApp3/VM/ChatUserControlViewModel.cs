using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TelerikWpfApp3.M;
using TelerikWpfApp3;
using System.Windows.Documents;
using System.Collections.ObjectModel;

namespace TelerikWpfApp3.VM
{
    class ChatUserControlViewModel : INotifyPropertyChanged
    {
        private string searchname;
        
        public string searchName
        {
            get { return searchname; }
            set
            {
                searchname = value;
                OnPropertyChanged("searchname");
            }
        }
        public ICommand friendPlus { get; set; }
        public ChatUserControlViewModel()
        {
            friendPlus = new Command(fpButton, CanExecuteMethod);
        }

        public void fpButton(object org)
        {
            string str = searchName + "/";
            string member = ((App)Application.Current).getmyID();
            ((App)Application.Current).SendData("<FRR>", str + member);
        }

        private bool CanExecuteMethod(object arg)
        {
            return true;
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}