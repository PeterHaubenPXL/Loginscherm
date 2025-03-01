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

namespace WPFTeam04
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

        private void closeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void studentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            StudentWindow win = new StudentWindow();
            win.ShowDialog();
        }

        private void sqlMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SqlWindow win = new SqlWindow();
            win.ShowDialog();
        }
    }
}
