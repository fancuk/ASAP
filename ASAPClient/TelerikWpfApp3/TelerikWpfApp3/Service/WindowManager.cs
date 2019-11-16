using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TelerikWpfApp3.Service
{
    public class WindowManager
    {
        public void ShowLoginView()
        {
            Window s = TelerikWpfApp3.StartWindow.Instance;
            Window n = TelerikWpfApp3.viewtest.Instance;
            n.Show();
            s.Hide();
        }
        public void StartMainWindow()
        {
            Window s = TelerikWpfApp3.viewtest.Instance;
            Window m = TelerikWpfApp3.StartWindow.Instance;
            m.Show();
            s.Hide();
        }
        public void RegisterComplete()
        {
            Window Rg = TelerikWpfApp3.Register.Instance;
            Rg.Hide();
        }

    }
}
