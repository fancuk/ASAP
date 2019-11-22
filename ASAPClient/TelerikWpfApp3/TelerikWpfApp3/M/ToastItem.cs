using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp3.M
{
    public class ToastItem : INotifyPropertyChanged
    {
        private string sender;
        private string time;
        private string plain;
        public string Sender { get => sender; set
            {
                this.sender = value;
                OnPropertyChanged("Sender");
            }
        }
        public string Time { get => time; set
            {
                this.time = value;
                OnPropertyChanged("Time");
            }
        }
        public string Plain { get => plain; set {
                this.plain = value;
                OnPropertyChanged("Plain");
            } }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
