using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TelerikWpfApp3
{
    class FriendsList
    {
        public List<string> friends = new List<string>();
        string str = Application.Current.Properties["id"].ToString() + "/";
        //((App) Application.Current).SendData("<FRD>", str);
    }
}
