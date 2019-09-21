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
        public void Login(object sender, RoutedEventArgs e)
        {
            string Uid = idbox.Text;
            string Upw = pwbox.Password.ToString();
            ((App)Application.Current).setmyID(Uid);
            string parameter = Uid + "/" + Upw;
            if (Uid == "" || Upw == "")
            {
                Properties.Settings.Default.idSaveCheck = false;
                MessageBox.Show("아이디 비번중에 하나를 안쳤네요.");
            }
            else
            {
                ((App)Application.Current).StartSocket();
                if (((App)Application.Current).nowConnect == true)
                {
                    ((App)Application.Current).SendData("<LOG>", parameter);
                }
                if (Properties.Settings.Default.loginOK == true)//로그인 성공
                {
                    if (rememberID.IsChecked == true) //check면
                    {
                        Properties.Settings.Default.idSaveCheck = true; //checkbox 체크
                        Properties.Settings.Default.loginIdSave = Uid; //id 저장
                        Properties.Settings.Default.Save();
                    }

                    else
                    {
                        Properties.Settings.Default.idSaveCheck = false;
                        Properties.Settings.Default.loginIdSave = "";
                        Properties.Settings.Default.Save();
                    }
                }
                else
                {
                    Properties.Settings.Default.loginIdSave = "";
                    Properties.Settings.Default.Save();
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
