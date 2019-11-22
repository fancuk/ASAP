using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace TelerikWpfApp3.View.Alert
{
    /// <summary>
    /// MessageToast.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MessageToast : Window
    {
        private static MessageToast _instnace;

        public static ToastItem toastItem = new ToastItem();

        public void getToastInfo(string sender, string time, string plain)
        {
            toastItem.Sender = sender;
            toastItem.Time = time;
            toastItem.Plain = plain;
            Sender.Text = sender;
            Time.Text = time;
            Plain.Text = plain;
        }
        public static MessageToast instance {
            get
            {
                if(_instnace == null)
                {
                    _instnace = new MessageToast();
                }
                else
                {
                   // _instnace.Hide();
                    _instnace = null;
                    _instnace = new MessageToast();
                    toastItem = new ToastItem();
                }
                StartCloseTimer();
                return _instnace;
            }
         }


        private MessageToast()
        {
            InitializeComponent();
            this.DataContext = toastItem;
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                this.Left = corner.X - this.ActualWidth - 10;
                this.Top = corner.Y - this.ActualHeight;
            }));
        }

        private static void StartCloseTimer()
        {
            _instnace.Show();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5d);
            timer.Tick += TimerTick;
            timer.Start();
        }

        private static void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;
            _instnace.Hide();
        }
    }
}
