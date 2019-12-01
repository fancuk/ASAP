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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
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
            this.DataContext = new StartWindowViewModel();
            StartWindowViewModel stw = new StartWindowViewModel();
            Closing += stw.OnWindowClosing;
            Loaded += Window1_Loaded;
        }

        private void Banner_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }



        public bool IsImage1 { get; set; }
        public List<BitmapImage> ImageList { get; set; }

        public DispatcherTimer Timer = new DispatcherTimer();
        public Random Ran = new Random();

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            Timer.Interval = TimeSpan.FromSeconds(3);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
            GetImage();
        }
        void GetImage()
        {
            ImageList = new List<BitmapImage>();
            //string[] Files = System.IO.Directory.GetFiles(@"\BannerImage");
            //foreach (string Path in Files)
            //{
            //    try
            //    {
            //        ImageList.Add(new BitmapImage(new Uri(Path, UriKind.Absolute)));
            //    }
            //    catch (Exception e) {
            //        MessageBox.Show(e.ToString());
            //    }
            //}
            //Img1.Source = ImageList[0];
            Uri uri = GetResourceURI("TelerikWpfApp3", "/Image/banner4.png");
            ImageList.Add(new BitmapImage(uri));
            uri = GetResourceURI("TelerikWpfApp3", "/Image/banner5.png");
            ImageList.Add(new BitmapImage(uri));
            uri = GetResourceURI("TelerikWpfApp3", "/Image/banner6.png");
            ImageList.Add(new BitmapImage(uri));
            uri = GetResourceURI("TelerikWpfApp3", "/Image/banner4.png");
            ImageList.Add(new BitmapImage(uri));
            uri = GetResourceURI("TelerikWpfApp3", "/Image/banner5.png");
            ImageList.Add(new BitmapImage(uri));
            uri = GetResourceURI("TelerikWpfApp3", "/Image/banner6.png");
            ImageList.Add(new BitmapImage(uri));
            uri = GetResourceURI("TelerikWpfApp3", "/Image/banner6.png");
            ImageList.Add(new BitmapImage(uri));
            Img1.Source = ImageList[0];
        }
        public Uri GetResourceURI(string assemblyName, string resourcePath)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                return new Uri(string.Format("pack://application:,,,/{0}", resourcePath));
            }

            else

            {

                return new Uri(string.Format("pack://application:,,,/{0};component/{1}", assemblyName, resourcePath));

            }

        }
        void Timer_Tick(object sender, EventArgs e)
        {
            if (IsImage1)
            {
                Img1.Source = ImageList[Ran.Next(0, ImageList.Count - 1)];
                (Resources["Img1Animation"] as Storyboard).Begin(this);
            }
            else
            {
                Img2.Source = ImageList[Ran.Next(0, ImageList.Count - 1)];
                (Resources["Img2Animation"] as Storyboard).Begin(this);
            }
            IsImage1 = !IsImage1;
        }
    }
}