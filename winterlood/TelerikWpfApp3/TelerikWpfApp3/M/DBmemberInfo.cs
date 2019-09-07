using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelerikWpfApp3.VM;
using TelerikWpfApp3.M;

namespace TelerikWpfApp3.M
{
    class DBmemberInfo : INotifyPropertyChanged
    {
        private string id;
        private string passwd;
        private string email;

        public DBmemberInfo(string id, string passwd, string email)
        {
            Id = id;
            Passwd = passwd;
            Email = email;
        }

        public string Id { get => id; set => id = value; }
        public string Passwd { get => passwd; set => passwd = value; }
        public string Email { get => email; set => email = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
