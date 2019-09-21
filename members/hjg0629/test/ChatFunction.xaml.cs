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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Controls.Primitives;

namespace test
{
    /// <summary>
    /// ChatFunction.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChatFunction : UserControl
    {
        public ListCollectionView MyCollectionView;

        public Emp emp;
        public ChatFunction()
        {
            InitializeComponent();
            //make_defaultView();
        }
        public void DCChange(object sender, DependencyPropertyChangedEventArgs args)

        {

            // StackPanel의 DataContext로 지정된 emps 컬렉션을 소스로 해서 ListCollectionView 생성

            // 이를 이용하여 정렬, 탐색, 필터링 기능 등을 구현한다.

            MyCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(ClientList.DataContext);
            ClientList.SelectedIndex = ClientList.Items.Count - 1;
            ClientList.ScrollIntoView(ClientList.SelectedItem);
        }
        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("he");
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                TreeViewItem t = null;
                MessageBox.Show(e.OriginalSource.ToString());
                string[] result = e.OriginalSource.ToString().Split(new char[] { ' ' });
                MessageBox.Show(result[0].ToString());
                MessageBox.Show(result[1].ToString());

                // Starts the Edit on the row;

            }
        }

        public void OnTreeItemDoubleClick(object sender, EventArgs args)
        {

            MyCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(ClientList.DataContext);
        }

        private void Wp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            tx.Text = null;
            string ct = (((sender as StackPanel).FindName("hoh") as TextBlock).Text);
            tx.Text += ct;
            MessageBox.Show(ct);
        }
        /*

public void defaultCategory()
{

}
public void OnTreeItemDoubleClick(object sender, EventArgs args)
{
   TreeViewItem tvi = (TreeViewItem)sender;
}
public void make_defaultView() // - 1
{
   DBConn dbconn = new DBConn();

   DataTable dt = new DataTable();
   dt.Clear();
   dt.Columns.Add("Project", typeof(string));

   try
   {
       dbconn.DBOpen();
       MySqlCommand cmd = new MySqlCommand();
       cmd.Connection = dbconn.myconn;
       cmd.CommandText = "SELECT * FROM groupcomponent WHERE Name = @Name ORDER BY TopGroupNumber ASC";
       cmd.Parameters.Add("@Name", MySqlDbType.VarChar, 50);
       cmd.Parameters[0].Value = "hjg0629";
       MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
       DataSet ds = new DataSet();

       adp.Fill(ds);

       foreach (DataRow TableComponent in ds.Tables[0].Rows)
       {
           DataRow dr = dt.NewRow();
           MessageBox.Show((string)TableComponent["TopGroupName"]);
           dr["Project"] = (string)TableComponent["TopGroupName"];
           dt.Rows.Add(dr);
           ClientList.ItemsSource = dt.DefaultView;
       }

   }
   catch (MySqlException MSE)
   {
       MessageBox.Show(MSE.ToString());
   }
   finally
   {
       dbconn.myconn.Close();
   }
}

public void make_obj() // 2
{
   Myobj dr = new Myobj();
   dr.Frist = "A";

   ClientList.Items.Add(dr);
}

public class Myobj // 2-2
{
   public string Frist { get; set; }
}
private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
{
   // Lookup for the source to be DataGridCell
   if (e.OriginalSource.GetType() == typeof(DataGridCell))
   {
       TreeViewItem t = null;
       MessageBox.Show(e.OriginalSource.ToString());
       string[] result = e.OriginalSource.ToString().Split(new char[] { ' ' });
       MessageBox.Show(result[0].ToString());
       MessageBox.Show(result[1].ToString());
       clickGroup(result[1], t);

       // Starts the Edit on the row;

   }
}
public void clickGroup(string Group, TreeViewItem TVI)
{


}
*/
    }
}
