using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace WpfTeam04
{
    /// <summary>
    /// Interaction logic for SqlWindow.xaml
    /// </summary>
    public partial class SqlWindow : Window
    {

        string connectionString;

        public SqlWindow()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (privateCheckBox.IsChecked == true)
                {
                    connectionString = "Integrated Security=SSPI;Persist Security Info=False;User ID=5CD421FDX8\\12402109;Initial Catalog=" + DbTextBox.Text + ";Data Source=5CD421FDX8\\SQLEXPRESS";
                }
                else
                {
                    connectionString = "Trusted_Connection=True;";
                    connectionString += "User ID=PxLUser_04;";
                    connectionString += "Password = 160CFv2!;";
                    connectionString += $@"Server=10.128.4.7;";
                    connectionString += $"Database=Db2025Team_04";
                }
                
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                conn.Close();
                MessageBox.Show("connection ok!");
                GetDataButton.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private SqlConnection GetConnection()
        {
            SqlConnection conn;
            try
            {
                //string connectionString = "Trusted_Connection=True;";

                if(connectionString == "")
                {
                    connectionString = "user id = pxluser;";
                    connectionString += "Password = pxluser;";
                    connectionString += $@"Server={ServerTextBox.Text};";
                    connectionString += $"Database={DbTextBox.Text}";
                }
                conn = new SqlConnection(connectionString);
                conn.Open();
                conn.Close();
                return conn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cmd = $"Select * from {TableTextBox.Text}";
                SqlCommand sql = new SqlCommand(cmd, GetConnection());
                SqlDataAdapter da = new SqlDataAdapter(sql);
                DataTable dt = new DataTable();
                da.Fill(dt);
                SqlDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

}
