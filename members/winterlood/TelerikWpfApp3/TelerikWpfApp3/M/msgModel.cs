using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    class msgModel:INotifyPropertyChanged
    {
        private string _msg;
        public string msg
        {
            get
            {
                return this._msg;
            }
            set
            {
                this._msg = value; OnPropertyChanged("msg");
            }
        }
        public msgModel(string parameter)
        {
            msg = parameter;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
