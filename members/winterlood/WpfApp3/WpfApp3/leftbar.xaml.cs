using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// leftbar.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class leftbar : Page
    {
        public leftbar()
        {
            InitializeComponent();

            Refresh();
        }

        public void Refresh()
        {
            setSpoolerStatus();
            string deFaultPrinter = GetDefaultPrinter();
            Application.Current.Properties["dp"] = deFaultPrinter;
            defaultPrinter.Text = deFaultPrinter;
            defaultPrinter.Foreground = new SolidColorBrush(Colors.Green);
            string dp = Application.Current.Properties["dp"].ToString();

            PrintServer myPrintServer = new PrintServer(dp);
            // List the print server's queues
            PrintQueueCollection myPrintQueues = myPrintServer.GetPrintQueues();
            String printQueueNames = "My Print Queues:\n\n";
            noj.Text = GetNumberOfPrintJobs(deFaultPrinter).ToString();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                this.cboPrinter.Items.Add(printer.ToString());
                PrinterSettings p = new PrinterSettings();
                p.PrinterName = printer;
                if (p.IsDefaultPrinter)
                {
                    cboPrinter.Text = printer.ToString();
                }
            }
        }


        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(string name);
        private int GetNumberOfPrintJobs(string dep)
        {
            LocalPrintServer server = new LocalPrintServer();
            PrintQueueCollection queueCollection = server.GetPrintQueues();
            PrintQueue printQueue = null;

            foreach (PrintQueue pq in queueCollection)
            {
                if (pq.FullName.Equals(dep))
                    printQueue = pq;
            }

            int numberOfJobs = 0;
            if (printQueue != null)
                numberOfJobs = printQueue.NumberOfJobs;
            string jobList = null;

            foreach (PrintQueue pq in queueCollection)
            {
                pq.Refresh();
                PrintJobInfoCollection jobs = pq.GetPrintJobInfoCollection();
                foreach (PrintSystemJobInfo job in jobs)
                {
                    // Since the user may not be able to articulate which job is problematic,
                    // present information about each job the user has submitted.
             
                        jobList = jobList + "\n\tQueue:" + pq.Name;
                        jobList = jobList + "\n\tLocation:" + pq.Location;
                        jobList = jobList + "\n\t\tJob: " + job.JobName + " ID: " + job.JobIdentifier;
                    
                }// end for each print job    

            }// end for each print queue
            Console.Write(jobList);
            return numberOfJobs;
        }
        public string GetDefaultPrinter()
        {
            PrinterSettings settings = new PrinterSettings();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                settings.PrinterName = printer;
                if (settings.IsDefaultPrinter)   // 기본 설정 여부
                    return printer;
            }
            return string.Empty;
        }
        public void setSpoolerStatus()
        {
            ServiceController service = new ServiceController("Spooler");
            if ((service.Status.Equals(ServiceControllerStatus.Running)))
            {
                SpoolerStatus.Text = "Running";
                SpoolerStatus.Foreground = new SolidColorBrush(Colors.Green);
                Start.Content = "Stop";
            }
            else
            {
                SpoolerStatus.Text = "Stopped";
                SpoolerStatus.Foreground = new SolidColorBrush(Colors.Red);
                Start.Content = "Start";
            }
        }
        public string ID
        {
            get;
            set;
        }
        public string PW
        {
            get;
            set;
        }
        public void Start_Click(object sender, RoutedEventArgs e)
        {
            ServiceController service = new ServiceController("Spooler");
            if ((!service.Status.Equals(ServiceControllerStatus.Running)))
            {
                service.Start();
                MessageBox.Show("Spooler is  Successfully Run!");
                setSpoolerStatus();
            }
            else if ((!service.Status.Equals(ServiceControllerStatus.Stopped)))
            {
                service.Stop();
                MessageBox.Show("Spooler is  Successfully Stopped!");
                setSpoolerStatus();
            }
            else
            {
                MessageBox.Show("This is Error");
            }

        }

        private void set_DP(object sender, RoutedEventArgs e)
        {
            SetDefaultPrinter(cboPrinter.Text);
            Refresh();
            UpdateLayout();
        }
    }
}
