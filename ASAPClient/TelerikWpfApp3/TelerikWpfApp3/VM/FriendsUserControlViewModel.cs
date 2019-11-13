﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TelerikWpfApp3.M;
using TelerikWpfApp3.View;
using TelerikWpfApp3.Collection;
using TelerikWpfApp3.VM.DBControl;

namespace TelerikWpfApp3.VM
{
    class FriendsUserControlViewModel : INotifyPropertyChanged
    {
        private static FriendsUserControlViewModel instance = null; // 다민
        private string _myID;
        public string MyID {
            get => _myID;
            set {
                this._myID = value;
                OnPropertyChanged("MyID");
            } }

        public static FriendsUserControlViewModel Instance // 다민
        {
            get
            {
                if (instance == null)
                {
                    instance = new FriendsUserControlViewModel();
                }
                return instance;
            }
        }
        public FullyObservableCollection<FriendsItem> fl;
        public ItemsChangeObservableCollection<FriendsItem> ico;
        private ObservableCollection<FriendsItem> _FriendsList; // 다민

        public ObservableCollection<FriendsItem> FriendsList
        {
            get
            {
                return _FriendsList;
            }
        }

        public ItemsChangeObservableCollection<FriendsItem> getFriends() // 다민
        {
            return ico;
        }

        public void AddFriend(string user, string _status) // 다민
        {
            if (_status == "true")
            {
                FriendsList.Add(new FriendsItem(user, null, "true"));
                ico.Add(new FriendsItem(user, null, "true"));
            }
            else
            {
                FriendsList.Add(new FriendsItem(user, null, "false"));
                ico.Add(new FriendsItem(user, null, "false"));
            }
        }

        public void ChangeStatus(string User, string _status)// 다민
        {
            for (int i = 0; i < ico.Count; i++)
            {
                if (ico[i].User == User)
                {
                    ico[i].Status = _status;
                }
            }
        }

        public bool FriendDoubleCheck(string user) //다민
        {
            for (int i = 0; i < ico.Count; i++)
            {
                if (ico[i].User == user)
                    return true;
            }
            return false;
        }
        public ICommand showFriendModal { get; set; }

     

        /*public FriendsUserControlViewModel()
        {
            showFriendModal = new Command(showModal, CanExecute);
        }다민*/

        private bool _loadAllChk = false; // 다민

        public bool loadAllChk // 다민
        {
            get
            {
                return _loadAllChk;
            }
            set
            {
                _loadAllChk = value;
            }
        }


        private FriendsUserControlViewModel() //다민
        {
            MyID = ((App)Application.Current).myID;
            showFriendModal = new Command(showModal, CanExecute);
            _FriendsList = new ObservableCollection<FriendsItem>();
            fl = new FullyObservableCollection<FriendsItem>();
            ico = new ItemsChangeObservableCollection<FriendsItem>();
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
