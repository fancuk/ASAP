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
namespace 실전
{
    // 로그인 되면, 로그인 페이지 닫기,회원가입 또한.
    class ViewModel : INotifyPropertyChanged
    {
        bool idchanged = false;
        #region 회원가입,로그인 아이디 비밀번호
        private string already_id = "";
        private string already_password = "";
        private string already_age = "";
        private string log_In_Id = "";
        private string log_In_PASSWORD = "";
        #endregion
        #region 회원 가입 아이디
        public string already_ID
        {
            get
            {
                return this.already_id;
            }
            set
            {
                this.already_id = value;
                OnpropertyChanged("already_ID");
                idchanged = false;
            }
        }
        #endregion
        #region 회원 가입 비밀번호
        public string already_Password
        {
            get { return this.already_password; }
            set
            {
                this.already_password = value;
                OnpropertyChanged(already_password);
            }
        }
        #endregion
        #region 회원 가입 나이
        public string already_Age
        {
            get {
                return this.already_age;
            }
            set {
                this.already_age = value;
                OnpropertyChanged(already_Age);
            }
        }
        #endregion
        #region 로그인 아이디
        public string log_In_ID
        {
            get { return log_In_Id; }
            set
            {
                log_In_Id = value;
                OnpropertyChanged(log_In_Id);
            }
        }
        #endregion
        #region 로그인 비밀번호
        public string log_In_Password
        {
            get { return log_In_PASSWORD; }
            set
            {
                log_In_PASSWORD = value;
                OnpropertyChanged(log_In_Password);
            }
        }
        #endregion
        public ICommand _sign_Up_Page { get; set; }
        public ICommand _sign_Up { get; set; }
        public ICommand already { get; set; }
        public ICommand _log_In_Page { get; set; }
        public ViewModel()
        {
            _sign_Up_Page = new Command(_sign_Up_Page_Open,_canExecute);
            already = new Command(dbIDRead, _canExecute);
            _sign_Up = new Command(dbSign_Up, _canExecute);
            _log_In_Page = new Command(dbLogIn, _canExecute);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #region INotifyPropertyChanged Members
        protected void OnpropertyChanged(string str)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));
        }
        #endregion
        #region DB연결
        private MySqlConnection db()
        {
            string conn = "Server=localhost; Port=3306; Database=member; Uid=root; Pwd=emforhsqhf1";
            MySqlConnection mySql = new MySqlConnection(conn);
            return mySql;
        }

        private void dbSign_Up(object obj)
        {
            
            MySqlConnection mysql = db();
            mysql.Open();
            string Sign_Up_query = "INSERT INTO members(id,password,age) VALUES('" + already_ID + "','" + already_Password + "','" + already_Age + "')";
            MySqlCommand mySqlCommand = new MySqlCommand(Sign_Up_query, mysql);
            if (already_Age == "" || already_ID == "" || already_Password == "") 
            {
                MessageBox.Show("아이디, 비밀번호, 나이는 빈칸일 수 없습니다.");
            }

            else
            {
                if (idchanged)
                {
                    int len = mySqlCommand.ExecuteNonQuery();
                    MessageBox.Show("회원 가입 완료", "성공");
                }
                else
                {
                    MessageBox.Show("중복 확인 해주세요", "실패");
                }
            }
            mysql.Close();
        }

        private void dbIDRead(object obj)
        {
            if (already_ID == null)
            {
                MessageBox.Show("아이디는 빈칸일 수 없습니다.");
                return;
            }
            MySqlConnection mysql = db();
            mysql.Open();
            string idquery = "SELECT id FROM members WHERE id='" + already_ID + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(idquery, mysql);
            MySqlDataReader idreader = mySqlCommand.ExecuteReader();
            if (idreader.Read()) // 해당 ID가 존재한다면
            {
                MessageBox.Show("사용 불가");
            }
            else
            {
                MessageBox.Show("사용 가능");
                idchanged = true;
            }
            mysql.Close();
        }
        private void dbLogIn(object obj)
        {
            MySqlConnection mysql = db();
            mysql.Open();
            string query = "SELECT password FROM members WHERE id='" + log_In_ID + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(query, mysql);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            if (mySqlDataReader.Read())
            {
                if (log_In_PASSWORD.Equals(mySqlDataReader["password"].ToString()))
                {
                    MessageBox.Show("로그인 성공.");
                    _log_In_Page_Open();
                }
                else
                {
                    MessageBox.Show("비밀번호 틀림.");
                }
            }
            else
            {
                MessageBox.Show("로그인 실패.");
            }
        }
        #endregion
        #region 회원가입 창 띄우기
        private void _sign_Up_Page_Open(object obj)
        {
            Window _sign_up_Page = new sign_up();
            _sign_up_Page.Show();
        }
        #endregion
        #region 로그인 창 띄우기
        private void _log_In_Page_Open()
        {
            Window _log_In_Page = new Log_In();
            _log_In_Page.Show();
        }
        #endregion

        private bool _canExecute(object obj)
        {
            return true;
        }
    }
}
