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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IdentityServer3.Core.Models;

namespace MSGProject_Server
{
    /// <summary>
    /// RealMainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RealMainWindow : Window
    {
        public RealMainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel();
        }
        
        //카카오 연동로그인
        private void Kakaobutton_Click(object sender, RoutedEventArgs e)
        {
            //HttpClient 인스턴스 생성하고 초기화
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://kauth.kakao.com");
            //JSON 형식에 대한 Accept 헤더를 추가
            //기본으로 초기화 한것, 데이터를 json 형식으로 전송할 것임을 서버에게 알림
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           //GET
            HttpResponseMessage response = client.GetAsync("/oauth/authorize?client_id=75d200a0406f48ab67540907d6028415&redirect_uri={redirect_uri}&response_type=code").Result;
        }
    }
}
