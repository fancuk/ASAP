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
using TelerikWpfApp3.M;

namespace TelerikWpfApp3
{
    /// <summary>
    /// myMessageBox.xaml에 대한 상호 작용 논리
    /// </summary>
    ///
    public partial class SuccessMsgBox : Window
    {
        public SuccessMsgBox(string msg)
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MoveWindow;
            body.DataContext = new msgModel(msg);

        }
        void MoveWindow(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }
    }
}
