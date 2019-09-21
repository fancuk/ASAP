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

namespace 실전
{
    /// <summary>
    /// Log_In.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Log_In : Window
    {
        public Log_In()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }
    }
}
