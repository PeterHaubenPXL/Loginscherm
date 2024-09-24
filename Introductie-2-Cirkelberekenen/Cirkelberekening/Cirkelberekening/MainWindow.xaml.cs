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

namespace Cirkelberekening
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Global variable
        const double PI = 3.1415;

        public MainWindow()
        {
            InitializeComponent();

            // 
            clearBtn_Click(null, null);
        }

        private void calculateBtn_Click(object sender, RoutedEventArgs e)
        {
            // Input user 
            double radius = Convert.ToDouble(radiusTxt.Text);
            
            // Berekeningen & Tonen
            double surface = Math.PI * radius * radius;
            surfaceTxt.Text = Math.Round(surface, 2).ToString() + " cm²";
            
            double circumference = 2 * Math.PI * radius;
            circumferenceTxt.Text = Math.Round(circumference, 2).ToString() + " cm";
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            circumferenceTxt.Text = "0 cm";
            radiusTxt.Text = "0 cm";
            surfaceTxt.Text = "0 cm²";

            // Focus Input Txt-field
            radiusTxt.Focus();
            radiusTxt.SelectAll();

        }

    }

}