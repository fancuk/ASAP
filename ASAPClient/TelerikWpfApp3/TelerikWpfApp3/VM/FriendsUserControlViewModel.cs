using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TelerikWpfApp3.M;
using TelerikWpfApp3.View;

namespace TelerikWpfApp3.VM
{
    class FriendsUserControlViewModel : INotifyPropertyChanged
    {
        public ICommand showFriendModal { get; set; }
        public FriendsUserControlViewModel()
        {
            showFriendModal = new Command(showModal, CanExecute);
        }


        private void showModal(object e)
        {
            FriendAddWindow faw = new FriendAddWindow();
            faw.Owner = Application.Current.MainWindow;
            if(faw.ShowDialog() == true)
            {

            }
        }
        private bool CanExecute(object obj)
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
