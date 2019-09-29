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

namespace TelerikWpfApp3.View.Alert
{
    /// <summary>
    /// ServiceErrorAlert.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ServiceErrorAlert : Window
    {
        public ServiceErrorAlert()
        {
            InitializeComponent();
        }
        public System.Windows.Forms.NotifyIcon notify;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
                // 아이콘 설정부분
                notify = new System.Windows.Forms.NotifyIcon();
                // notify.Icon = new System.Drawing.Icon(@"TiimeAlram.ico");  // 외부아이콘 사용 시
                // notify.Icon = Properties.Resources.greenchk;   // Resources 아이콘 사용 시
                notify.Visible = true;
                notify.ContextMenu = menu;
                notify.Text = "Test";

                // 아이콘 더블클릭 이벤트 설정
                notify.DoubleClick += Notify_DoubleClick;

                System.Windows.Forms.MenuItem item1 = new System.Windows.Forms.MenuItem();
                menu.MenuItems.Add(item1);
                item1.Index = 0;
                item1.Text = "프로그램 종료";
                item1.Click += delegate (object click, EventArgs eClick)
                {
                    System.Windows.Application.Current.Shutdown();
                    notify.Dispose();
                };

                System.Windows.Forms.MenuItem item2 = new System.Windows.Forms.MenuItem();
                menu.MenuItems.Add(item2);
                item2.Index = 0;
                item2.Text = "프로그램 설정";
                item2.Click += delegate (object click, EventArgs eClick)
                {
                    this.Close();
                };

                this.Close();   // 시작시 창 닫음 (아이콘만 띄우기 위함)
            }
            catch (Exception ee)
            {
            }
        }

        private void Notify_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
            //            notify.Visible = false;   // 트레이아이콘 숨기기
        }

        // X 버튼으로 종료 시를 위한 오버라이드
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }
    }
}
