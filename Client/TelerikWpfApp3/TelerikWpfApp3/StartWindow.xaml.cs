using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TelerikWpfApp3.VM;

namespace TelerikWpfApp3
{
    /// <summary>
    /// StartWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StartWindow : Window
    {
        private static StartWindow instance = null;
        public static StartWindow Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StartWindow();
                }
                return instance;
            }
        }
        
        private StartWindow()
        {
            InitializeComponent();
            StartWindowViewModel stw = new StartWindowViewModel();
            Closing += stw.OnWindowClosing;
        }

        private void Banner_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

    }
}