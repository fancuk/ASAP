using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MySql.Data;
using MySql.Data.MySqlClient;

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
        public bool check = false;

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
        public ICommand checkButton { get; set; } //비밀번호확인
        public ICommand newPage { get; set; }

        private MySqlConnection getConn()
        {
            string strConn = "Server=localhost;Database=loginout;Uid=root;Pwd=1234";
            MySqlConnection conn = new MySqlConnection(strConn);
            return conn;
        }

        private void makeNewid(object obj)
        {
            MySqlConnection connect = getConn();
            connect.Open();
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
            else if (check == false)
            {
                MessageBox.Show("비밀번호 확인을 해주세요");
                return;
            }
            string insertQuery = "INSERT INTO info(id,password,name,age) VALUES('" + newid + "','" + newpwd + "','" + newname + "','" + newemail + "')";
            connect.Open();
            MySqlCommand cmd = new MySqlCommand(insertQuery, connect);
            try
            {
                if (cmd.ExecuteNonQuery() == 1)//내가 처리한 mysql에 정상적으로 들어감
                {
                    MessageBox.Show("회원가입 완료");
                }
                else
                {
                    MessageBox.Show("오류");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("같은 아이디가 있습니다");
            }
            connect.Close();
        }
        private void pwdCheck(object obj)
        {
            if (newpwd != "" && newpwdcheck != "")
            {
                if (newpwd == newpwdcheck)
                {
                    MessageBox.Show("비밀번호 확인 완료");
                    check = true;
                }
                else
                {
                    MessageBox.Show("비밀번호가 일치하지 않습니다");
                    check = false;
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
        private void signIn(object obj)
        {
            Window signpage = new New();
            signpage.ShowDialog();
        }
        public ViewModel()
        {
            loginPage = new Command(dbLogin, canExcute);
            newPage = new Command(makeNewid, canExcute);
            newPage_open = new Command(signIn, canExcute);
            checkButton = new Command(pwdCheck, canExcute);
        }
        public bool canExcute(object obj)
        {
            return true;
        }
    }
}