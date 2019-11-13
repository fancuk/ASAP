using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class FriendsItem : INotifyPropertyChanged
    {
        private string _user;
        private string _status;
        private string _lastMesseage;


        public FriendsItem(string user, string lastMesseage, string _status)
        {
            User = user;
            LastMesseage = lastMesseage;
            this._status = _status;
        }

        public string User
        {
            get
            {
                return this._user;
            }
            set
            {
                this._user = value;
                OnPropertyChanged("User");
            }
        }
        public string LastMesseage
        {
            get
            {
                return this._lastMesseage;
            }
            set
            {
                this._lastMesseage = value;
                OnPropertyChanged("LastMesseage");
            }
        }
        public string Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
                OnPropertyChanged("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}