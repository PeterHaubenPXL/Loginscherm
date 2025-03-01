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

namespace WpfTeam04
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void StudentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            StudentWindow studentWindow = new StudentWindow();

            studentWindow.Show();
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SQLMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SqlWindow sqlwin = new SqlWindow();

            sqlwin.ShowDialog();
        }
    }
}
