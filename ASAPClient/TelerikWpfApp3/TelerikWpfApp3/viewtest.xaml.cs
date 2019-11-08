using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
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
    /// viewtest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class viewtest : Window
    {
        private static viewtest instance = null;

        public static viewtest Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new viewtest();
                }
                return instance;
            }
        }
        static viewtest()
        {
            instance = new viewtest();
        }
        private viewtest()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MoveWindow;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.PreviewKeyDown += new KeyEventHandler(OnEnterKeyDownHandler); // Enter 인식 하면 로그인 실행
            LoginViewModel lvm = new LoginViewModel();
            Closing += lvm.OnWindowClosing;
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        void MoveWindow(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }
        private void Login(object sender, RoutedEventArgs e)
        {
            string Uid = idbox.Text;
            string Upw = pwbox.Password.ToString();
            if (Uid == "" || Upw == "")
            {
                Properties.Settings.Default.idSaveCheck = false;
                MessageBox.Show("아이디 비번중에 하나를 안쳤네요.");
                return;
            }
            else
            {
                LoginViewModel login = new LoginViewModel();
                login.LogIn(Uid, Upw);
                if (rememberID.IsChecked == true)
                {
                    Properties.Settings.Default.idSaveCheck = true;
                }
                else
                {
                    Properties.Settings.Default.idSaveCheck = false;
                }
            }
        }
        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            idbox.Text = Properties.Settings.Default.loginIdSave;
        }

        // 여기는 메세지 박스내에서 엔터시 로그인 버튼 실행
        private void OnEnterKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(login);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
        }

        
    }
}
