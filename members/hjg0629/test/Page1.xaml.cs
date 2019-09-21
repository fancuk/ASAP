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


namespace test
{
    /// <summary>
    /// Page1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Page1 : Page
    {


        public Page1()
        {
            InitializeComponent();
            make_defaultView();
            
            /*
            TreeViewItem tt = new TreeViewItem();
            tt.Header = "some item";
            tt.MouseDoubleClick += new MouseButtonEventHandler(OnTreeItemDoubleClick);
            

            TreeViewItem tvi = new TreeViewItem();
            tvi.Header = groupModel.Name;
            treeView.Items.Add(tvi);

            foreach (Model m in models)
            {
                TreeViewItem t = new TreeViewItem();
                t.Header = model.Name;
                tvi.Items.Add('a');
            }
            */
        }

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
                    DataGridGroup.ItemsSource = dt.DefaultView;
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

            DataGridGroup.Items.Add(dr);
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
                myTreeViewEvent.Items.Clear();
                clickGroup(result[1], t);
                
                // Starts the Edit on the row;

            }
        }
        public void clickGroup(string Group, TreeViewItem TVI)
        {
            bool LeafCheckFlag = false;
            bool IsEmptyGroup = true;
            DBConn dbconn = new DBConn();
            try
            {
                dbconn.DBOpen();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbconn.myconn;
                cmd.CommandText = "SELECT * FROM grouptable WHERE ParentGroup = @ParentGroup";
                //cmd.CommandText = "WITH CTE AS (SELECT code, parent_code FROM CODE_TABLE WHERE code = 'AA' UNION ALL SELECT a.code, a.parent_code FROM CODE_TABLE a INNER JOIN CTE b ON a.parent_code = b.code) " +
                //    "SELECT code, parent_code FROM CTE";
                cmd.Parameters.Add("@ParentGroup", MySqlDbType.VarChar, 50);
                cmd.Parameters[0].Value = Group;
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adp.Fill(ds);
                TreeViewItem Parent = new TreeViewItem();
                Parent.Background = new SolidColorBrush(Color.FromArgb(0xFF,00,0xBE,0xBE));
                Parent.Name = "_GroupName";
                Parent.Header = Group;
                if (TVI == null)
                {
                    myTreeViewEvent.Items.Add(Parent);
                }
                else
                {
                    TVI.Items.Add(Parent);
                }

                foreach (DataRow TableComponent in ds.Tables[0].Rows)
                {
                    {
                        clickGroup((string)TableComponent["GroupName"], Parent);
                        IsEmptyGroup = false;
                    }
                    MySqlCommand NewCommand = new MySqlCommand();
                    NewCommand.Connection = dbconn.myconn;
                    NewCommand.CommandText = "SELECT * FROM groupcomponent WHERE GroupName = @GroupName";
                    NewCommand.Parameters.Add("@GroupName", MySqlDbType.VarChar, 50);
                    NewCommand.Parameters[0].Value = Group;
                    MySqlDataAdapter NewAdp = new MySqlDataAdapter(NewCommand);
                    DataSet NewDS = new DataSet();

                    NewAdp.Fill(NewDS);
                    foreach (DataRow TreeComponent in NewDS.Tables[0].Rows)
                    {
                        {
                            TreeViewItem ChildComponent = new TreeViewItem();
                            string Name = (string)TreeComponent["Name"];
                            ChildComponent.Header = Name;
                            Parent.Items.Add(ChildComponent);
                        }
                    }
                    LeafCheckFlag = true;
                    IsEmptyGroup = false;
                }
                if(LeafCheckFlag == false)
                {
                    MySqlCommand NewCommand = new MySqlCommand();
                    NewCommand.Connection = dbconn.myconn;
                    NewCommand.CommandText = "SELECT * FROM groupcomponent WHERE GroupName = @GroupName";
                    NewCommand.Parameters.Add("@GroupName", MySqlDbType.VarChar, 50);
                    NewCommand.Parameters[0].Value = Group;
                    MySqlDataAdapter NewAdp = new MySqlDataAdapter(NewCommand);
                    DataSet NewDS = new DataSet();

                    NewAdp.Fill(NewDS);
                    foreach (DataRow TreeComponent in NewDS.Tables[0].Rows)
                    {
                        {
                            TreeViewItem ChildComponent = new TreeViewItem();
                            ChildComponent.Header = (string)TreeComponent["Name"];
                            Parent.Items.Add(ChildComponent);
                        }
                        IsEmptyGroup = false;
                    }
                }
                if(IsEmptyGroup == true)
                {
                    TreeViewItem EmptyGroup = new TreeViewItem();
                    EmptyGroup.Visibility = Visibility.Collapsed;
                    Parent.Items.Add(EmptyGroup);
                }
                /*

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbconn.myconn;
                cmd.CommandText = "SELECT * FROM groupcomponent WHERE GroupName = @GroupName";
                cmd.Parameters.Add("@GroupName", MySqlDbType.VarChar, 50);
                cmd.Parameters[0].Value = Group;
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adp.Fill(ds);
                myTreeViewEvent.Items.Clear();
                TreeViewItem parent = new TreeViewItem();
                parent.Name = "_GroupName";
                parent.Header = Group;
                myTreeViewEvent.Items.Add(parent);
                foreach (DataRow TableComponent in ds.Tables[0].Rows)
                {

                    //if ((string)TableComponent["Name"] == "hjg0629")
                    {
                        //myTreeViewEvent.Items.Add("jeonggu");
                        //myTreeViewEvent.Items.Insert(0, "je");
                        //myTreeViewEvent.Items.MoveCurrentTo(1);


                        TreeViewItem child = new TreeViewItem();
                        child.Header = (string)TableComponent["Name"];
                        parent.Items.Add(child);
                    }

                }
                */

                
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
    }
}
