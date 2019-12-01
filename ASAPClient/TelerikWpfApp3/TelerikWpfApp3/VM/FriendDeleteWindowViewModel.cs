using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TelerikWpfApp3.M;
using TelerikWpfApp3.Service;

namespace TelerikWpfApp3.VM
{
    public class FriendDeleteWindowViewModel : INotifyPropertyChanged
    {
        private string friendID = "";
        NetworkManager networkManager = ((App)Application.Current).networkManager;
        public string FriendID
        {
            get
            {
                return friendID;
            }
            set
            {
                friendID = value;
                OnPropertyChanged("FriendID");
            }
        }
        public FriendDeleteWindowViewModel()
        {
            FriendDeleteButton = new Command(DeleteFriend, CanExecute);
            CancelButton = new Command(Cancel, CanExecute);
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Handle closing logic, set e.Cancel as needed
            e.Cancel = true;
            FriendsUserControlViewModel.Instance.CloseDeleteWindow();
        }
        public ICommand FriendDeleteButton { get; set; }
        public ICommand CancelButton { get; set; }
        public void DeleteFriend(object e)
        {
            string myID = networkManager.MyId;
            if (friendID == "")
            {
                MessageBox.Show("친구가 선택되지 않았습니다.");
            }
            else
            {
                string text = myID + "/" + FriendID;
                FriendsUserControlViewModel.Instance.DelteFriend(FriendID);
                networkManager.SendData("<FRD>", text);
            }
            
        }
        public void Cancel(object e)
        {
            FriendsUserControlViewModel.Instance.CloseDeleteWindow();
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
