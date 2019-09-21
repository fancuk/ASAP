using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Http.Headers;

namespace MSGProject_Server
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string logid = "";
        private string logpwd = "";
        private string newid = "";
        private string newpwd = "";
        private string newpwdcheck = "";
        private string newemail = "";
        private string newname = "";
        public bool pwdcheck = false;
        public bool idcheck = false;

        protected void OnpropertyChanged(string str)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }
        //로그인정보
        public string logId
        {
            get { return logid; }
            set
            {
                logid = value;
                OnpropertyChanged(logid);
            }
        }
        public string logPwd
        {
            get { return logpwd; }
            set
            {
                logpwd = value;
                OnpropertyChanged(logpwd);
            }
        }
        //회원가입정보
        public string newId
        {
            get { return this.newid; }
            set
            {
                this.newid = value;
                OnpropertyChanged(newid);
            }
        }
        public string newPwd
        {
            get { return this.newpwd; }
            set
            {
                this.newpwd = value;
                OnpropertyChanged(newpwd);
            }
        }
        public string newPwdcheck
        {
            get { return this.newpwdcheck; }
            set
            {
                this.newpwdcheck = value;
                OnpropertyChanged(newpwdcheck);
            }
        }
        public string newEmail
        {
            get { return this.newemail; }
            set
            {
                this.newemail = value;
                OnpropertyChanged(newemail);
            }
        }
        public string newName
        {
            get { return this.newname; }
            set
            {
                this.newname = value;
                OnpropertyChanged(newname);
            }
        }

        public ICommand newPage_open { get; set; }//회원가입 page
        public ICommand loginPage { get; set; }//로그인 page
        public ICommand pwdcheckButton { get; set; }//비밀번호확인
        public ICommand idcheckButton { get; set; }//아이디 중복확인
        public ICommand newPage { get; set; }//아이디 생성
       // public ICommand kakaoButton { get; set; }

        private MySqlConnection getConn()
        {
            string strConn = "Server=localhost;Database=loginout;Uid=root;Pwd=1234";
            MySqlConnection conn = new MySqlConnection(strConn);
            return conn;
        }
        //회원가입
        private void makeNewid(object obj)
        {
            MySqlConnection connectt = getConn();
            connectt.Open();
            if (newname == "")
            {
                MessageBox.Show("이름을 입력하세요");
                return;
            }
            else if (newemail == "")
            {
                MessageBox.Show("email을 입력하세요!!");
                return;
            }
            else if (newid == "")
            {
                MessageBox.Show("ID를 입력하세요!!");
                return;
            }
            else if (newpwd == "")
            {
                MessageBox.Show("PWD를 입력하세요!!");
                return;
            }
            else if (pwdcheck == false)
            {
                MessageBox.Show("비밀번호 확인을 해주세요");
                return;
            }
            else if(idcheck == false)
            {
                MessageBox.Show("아이디 중복확인을 해주세요");
                return;
            }
            string insertQuery = "INSERT INTO info(id,password,name,age) VALUES('" + newid + "','" + newpwd + "','" + newname + "','" + newemail + "')";
            MySqlCommand cmd = new MySqlCommand(insertQuery, connectt);
            if (idcheck == true)
            {
                int len = cmd.ExecuteNonQuery();
                connectt.Close();
                MessageBox.Show("회원가입 완료");
            }
        }
        private void idCheck(object obj)
        {
            MySqlConnection conn = getConn();
            conn.Open();
            string sql1 = "SELECT * FROM info WHERE id='" + newid + "'";
            MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
            MySqlDataReader idread = cmd1.ExecuteReader();
            System.Console.Write("%s\n", newid);
            if (idread.Read())
            {
                MessageBox.Show("중복된 아이디 입니다");
            }
            else
            {
                idcheck = true;
                MessageBox.Show("사용 가능한 아이디입니다");
            }
            conn.Close();
        }
        private void pwdCheck(object obj)
        {
            if (newpwd != "" && newpwdcheck != "")
            {
                if (newpwd == newpwdcheck)
                {
                    MessageBox.Show("비밀번호 확인 완료");
                    pwdcheck = true;
                }
                else
                {
                    MessageBox.Show("비밀번호가 일치하지 않습니다");
                    pwdcheck = false;
                }
            }
        }
        private void dbLogin(object obj)
        {
            if (logid == "")
            {
                MessageBox.Show("ID를 입력하세요!!");
                return;
            }
            if (logpwd == "") 
            {
                MessageBox.Show("PWD를 입력하세요!!");
                return;
            }
            MySqlConnection connect = getConn();
            connect.Open();
            string ConnectQurey = "SELECT * FROM info WHERE id='" + logid + "'AND password='" + logpwd + "'";
            MySqlCommand command = new MySqlCommand(ConnectQurey, connect);
            MySqlDataReader read = command.ExecuteReader();
            System.Console.Write("%s\n", logid);
            System.Console.Write("%s\n", logpwd);
            if (read.Read())
            {
                MessageBox.Show("로그인 완료");
                Application.Current.Properties["id"] = logid;
                Window loginpage = new Login();
                loginpage.ShowDialog();
            }
            else MessageBox.Show("아이디 또는 비밀번호를 다시 입력하세요");
        }
      /*
        private async void signInAsync(object obj)
        {
            //await Application.Current.MainWindow.Navigation.PushAsync(new New());

        }
        */
        public ViewModel()
        {
            loginPage = new Command(dbLogin, canExcute);
            idcheckButton = new Command(idCheck, canExcute);
            newPage = new Command(makeNewid, canExcute);
           // newPage_open = new Command(signIn, canExcute);
            pwdcheckButton = new Command(pwdCheck, canExcute);
           // kakaoButton = new Command(kakaoLogin, canExcute);
        }
       /* private void kakaoLogin(object obj)
        {
            HttpClient client = new HttpClient();
            client.GetAsync
        }
        */
        public bool canExcute(object obj)
        {
            return true;
        }
     
    }
}