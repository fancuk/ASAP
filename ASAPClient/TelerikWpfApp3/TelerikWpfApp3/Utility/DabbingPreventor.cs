using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using System.Windows;

namespace TelerikWpfApp3.Utility
{
    public class DabbingPreventor
    {
        private static DabbingPreventor instance = new DabbingPreventor();
        private DabbingPreventor()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
            dabbingCount = 0;
        }
        public static DabbingPreventor Instance { get => instance; }
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dabbingCount = 0;
        }
        public static int dabbingCount;
        public bool isDabbing()
        {
            if (dabbingCount > 5)
            {
                return true;
            }
            else
            {
                dabbingCount += 1;
                return false;
            }
        }
    }
}
