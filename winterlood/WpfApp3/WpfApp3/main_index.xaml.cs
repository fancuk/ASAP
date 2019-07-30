using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
using System.Runtime.InteropServices;
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
    /// main_index.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class main_index : Page
    {

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
            uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]

        static extern IntPtr CreateRemoteThread(IntPtr hProcess,
            IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        const int PROCESS_CREATE_THREAD = 0x0002;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_READ = 0x0010;

        // used for memory allocation
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;
        const uint PAGE_READWRITE = 4;
        public main_index()
        {
            InitializeComponent();

            // the target process - I'm using a dummy process for this
            // if you don't have one, open Task Manager and choose wisely
            Process targetProcess = Process.GetProcessesByName("WpfApp3")[0];

            // geting the handle of the process - with required privileges
            IntPtr procHandle = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, false, targetProcess.Id);

            // searching for the address of LoadLibraryA and storing it in a pointer
            IntPtr loadLibraryAdendr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            // name of the dll we want to inject
            string dllName = "dll1.dll";
            //HMODULE hm;
            //hm = LoadLibrary(TEXT("c:\\dll\\dll1.dll"));
            // alocating some memory on the target process ough to store the name of the dll
            // and storing its address in a pointer
            IntPtr allocMemAddress = VirtualAllocEx(procHandle, IntPtr.Zero, (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

            // writing the name of the dll there
            UIntPtr bytesWritten;
            WriteProcessMemory(procHandle, allocMemAddress, Encoding.Default.GetBytes(dllName), (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);

            // creating a thread that will call LoadLibraryA with allocMemAddress as argument

            //CreateRemoteThread(procHandle, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);

        }

        public object listBoxControl { get;  set; }


        [DllImport("user32")]
        static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32")]
        public static extern string GetWindowText(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]

        private static extern bool SetForegroundWindow(IntPtr hWnd);


        [DllImport("user32")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, string lParam);

        [DllImport("user32")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);


        public static bool IsValidChatWindow(IntPtr hWnd, string mess)
        {
            if (!IsWindow(hWnd)) return false;
            IntPtr hEdit, hList, hScrl;
            hEdit = FindWindowEx(hWnd, IntPtr.Zero, "RichEdit20W", null);
            hList = FindWindowEx(hWnd, IntPtr.Zero, "EVA_VH_ListControl", null);
            hScrl = FindWindowEx(hList, IntPtr.Zero, "_EVA_CustomScrollCtrl", null);
            SendText(hEdit,mess);
            return true;
        }

        public static bool IsValidKakaotalkWindow(IntPtr hWnd)
        {
            while (true)
            {
                Process[] p = Process.GetProcessesByName("kakaotalk");
                if(p.Length < 1)
                {
                    Process.Start("C:\\Program Files (x86)\\Kakao\\KakaoTalk\\KakaoTalk.exe");
                }
                else
                {
                    Console.WriteLine("kakao talk is running now!");
                    break;
                }
                System.Threading.Thread.Sleep(3000);
            }
            Process[] found = Process.GetProcessesByName("kakaotalk");
            Process kakao = found[0];

            string id = "01090117518";
            string pw = "marin1";
            IntPtr edit1 = FindWindowEx(kakao.MainWindowHandle, IntPtr.Zero, "Edit", null);

            Console.WriteLine("로그인 중...");

            IntPtr edit2 = FindWindowEx(kakao.MainWindowHandle, edit1, "Edit", null);

            SendMessage(edit1, 0xC, IntPtr.Zero, id);
            SendMessage(edit2, 0xC, IntPtr.Zero, pw);

            PostMessage(edit2, 0x100, new IntPtr(0x0D), null);
            PostMessage(edit2, 0x101, new IntPtr(0x0D), null);
            if (!IsWindow(hWnd)) return false;

            return true;
            // 윈도우의 제목을 가져온다.
            //string name = GetWindowText(hWnd);
            //if (name == null) return false;

            // 총 2개의 하위 다이얼로그가 있으므로
            // 핸들을 가져온다.
            IntPtr hChildDialog1 = FindWindowEx(hWnd, IntPtr.Zero, "#32770", null);
            IntPtr hChildDialog2 = FindWindowEx(hWnd, hChildDialog1, "#32770", null);

            // 두 개의 다이얼로그 중 하나의 값이라도 받아오지 못한 경우
            // 정상적인 카카오톡 창이 아니다.
            if (hChildDialog1 == IntPtr.Zero || hChildDialog2 == IntPtr.Zero) return false;

            // 이제 다이얼로그의 하위 구조를 확인한다.
            IntPtr hDialogChildDialog1 = FindWindowEx(hChildDialog1, IntPtr.Zero, "#32770", null);
            IntPtr hDialogChildDialog2 = FindWindowEx(hChildDialog1, hDialogChildDialog1, "#32770", null);
            IntPtr hDialogChildEvaWindow1 = FindWindowEx(hChildDialog1, IntPtr.Zero, "EVA_Window", null);
            IntPtr hDialogChildEvaWindow2 = FindWindowEx(hChildDialog1, hDialogChildEvaWindow1, "EVA_Window", null);

            // 네 개의 윈도우 핸들이 유효하다면 정상적인 카카오톡 창이다.
            // 더 깊게 들어가야 하지만, 이 정도만 검사하면 된다.
            return
                hDialogChildDialog1 != IntPtr.Zero &&
                hDialogChildDialog2 != IntPtr.Zero &&
                hDialogChildEvaWindow1 != IntPtr.Zero &&
                hDialogChildEvaWindow2 != IntPtr.Zero;
        }

        [DllImport("user32")]
        public static extern Int32 SendMessage(IntPtr hWnd, Int32 uMsg, IntPtr WParam, string builder);
        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static void SendText(IntPtr hEdit,string mess)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SETTEXT = 0x0C;
            const int WM_KEYUP = 0x101;
            const int VK_ENTER = 0x0D;
            SendMessage(hEdit, WM_SETTEXT, IntPtr.Zero, mess);
            PostMessage(hEdit, WM_KEYDOWN, VK_ENTER, IntPtr.Zero);
            PostMessage(hEdit, WM_KEYUP, VK_ENTER, IntPtr.Zero);
            return;
        }
        private void Send_Message(object sender, RoutedEventArgs e)
        {
             IntPtr hd01 = FindWindow("#32770" ,null);
            IntPtr hd02 = FindWindow(null, name.Text);
            if (!IsValidKakaotalkWindow(hd01))
            {
                MessageBox.Show("kakao Talk is closed");
                return;
            }
            else if (!IsValidChatWindow(hd02,mess.Text))
            {
                MessageBox.Show("Chatting window is closed");
                return;
            }
            return;
        }
        private void sm(object sender, RoutedEventArgs e)
        {
            IntPtr hd01 = FindWindow("#32770", null);
            IntPtr hd02 = FindWindow(null, name.Text);
            if (!IsValidKakaotalkWindow(hd01))
            {
                MessageBox.Show("kakao Talk is closed");
                return;
            }
            else if (!IsValidChatWindow(hd02, mess.Text))
            {
                MessageBox.Show("Chatting window is closed");
                return;
            }
            else
            {
                MessageBox.Show("Success");
                return;
            }
            
        }

        private void Start_Moniter_Click(object sender, RoutedEventArgs e)
        {
         //   NavigationService.Navigate(
         //new Uri("moniter.xaml", UriKind.Relative));
        }
        void HyperlikOnRequestNavigate(object sender, RequestNavigateEventArgs args)
        {
            NavigationService.Navigate(args.Uri);
            args.Handled = true;
        }
    }
}
