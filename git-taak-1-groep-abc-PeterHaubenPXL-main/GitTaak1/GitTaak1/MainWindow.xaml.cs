using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GitTaak1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void blueImage_MouseEnter(object sender, MouseEventArgs e)
        {
            mainWindow.Background = Brushes.Blue;
        }

        private void greenImage_MouseEnter(object sender, MouseEventArgs e)
        {
            mainWindow.Background = Brushes.Green;
        }

        private void yellowImage_MouseEnter(object sender, MouseEventArgs e)
        {
            mainWindow.Background = Brushes.Yellow;
        }


        private void redImage_MouseEnter(object sender, MouseEventArgs e)
        {
            mainWindow.Background = Brushes.Red;
        }

        private void cleanImage_MouseLeave(object sender, MouseEventArgs e)
        {
            mainWindow.Background = Brushes.White;
        }

        
    }
}