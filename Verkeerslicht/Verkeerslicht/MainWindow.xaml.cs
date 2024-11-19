using System.Drawing;
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

namespace Verkeerslicht
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void redButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeLight("red");
        }

        private void orangeButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeLight("orange");
        }

        private void greenButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeLight("green");
        }

        private void ChangeLight(string color)
        {
            if (color == "red")
            {
                if (orangeLight.Visibility == Visibility.Visible)
                {
                    redLight.Visibility = Visibility.Visible;
                    orangeLight.Visibility = Visibility.Hidden;
                    greenLight.Visibility = Visibility.Hidden;
                }
                else
                {
                    if (greenLight.Visibility == Visibility.Visible)
                    {
                        MessageBox.Show("Klik eerst op 'ORANJE'", "Foute actie");
                    }
                }
            }
            else if(color == "orange")
            {
                if(greenLight.Visibility == Visibility.Visible)
                {
                    redLight.Visibility = Visibility.Hidden;
                    orangeLight.Visibility = Visibility.Visible;
                    greenLight.Visibility = Visibility.Hidden;
                }
                else
                {
                    if (redLight.Visibility == Visibility.Visible)
                    {
                        MessageBox.Show("Klik eerst op 'GROEN'", "Foute actie");
                    }
                }
            }
            else if (color == "green")
            {
                if (redLight.Visibility == Visibility.Visible)
                {
                    redLight.Visibility = Visibility.Hidden;
                    orangeLight.Visibility = Visibility.Hidden;
                    greenLight.Visibility = Visibility.Visible;
                }
                else
                {
                    if (orangeLight.Visibility == Visibility.Visible)
                    {
                        MessageBox.Show("Klik eerst op 'ROOD'", "Foute actie");
                    }
                }
                
            }

        }
    }
}