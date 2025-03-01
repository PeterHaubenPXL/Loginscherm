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

        public MainWindow()
        {
            InitializeComponent();

            clearButton_Click(null, null);
        }

        private void calculateBtn_Click(object sender, RoutedEventArgs e)
        {
            // Input user 
            if (!double.TryParse(radiusTextBox.Text, out double radius))
            {
                MessageBox.Show("Voer een decimaal getal in.", "Foutieve ingave", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // Berekeningen & Tonen
                double surface = Math.PI * radius * radius;
                surfaceTextBox.Text = Math.Round(surface, 2).ToString() + " cm²";

                double circumference = 2 * Math.PI * radius;
                circumferenceTextBox.Text = Math.Round(circumference, 2).ToString() + " cm";
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            circumferenceTextBox.Text = "0 cm";
            radiusTextBox.Text = "0";
            surfaceTextBox.Text = "0 cm²";

            // Focus Input Txt-field
            radiusTextBox.Focus();
            radiusTextBox.SelectAll();

        }

    }

}