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

namespace WisselgeldCalculator
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

        private void calculateChange_Click(object sender, RoutedEventArgs e)
        {
            bool isTotal = decimal.TryParse(totalTextBox.Text, out decimal total);
            bool isCash = decimal.TryParse(cashPaidTextBox.Text, out decimal cash);

            if (isTotal && isCash)
            {
                if (cash > total)
                {
                    CalculateChange(cash - total);
                }
            }

        }

        private void CalculateChange(decimal change)
        {
            change = change * 100;

            int twoEuro = (int)change / 200;
            change = change % 200;
            twoEuroTextBox.Text = twoEuro.ToString();

            int oneEuro = (int)change / 100;
            change = change % 100;
            oneEuoTextBox.Text = oneEuro.ToString();

            int halfEuro = (int)change / 50;
            change = change % 50;
            halfEuroTextBox.Text = halfEuro.ToString();

            int Cent20Euro = (int)change / 20;
            change = change % 20;
            cent20TextBox.Text = Cent20Euro.ToString();

            int Cent10Euro = (int)change / 10;
            change = change % 10;
            cent10TextBox.Text = Cent10Euro.ToString();

            int Cent5Euro = (int)change / 5;
            change = change % 5;
            cent5TextBox.Text = Cent5Euro.ToString();

            int Cent2Euro = (int)change / 2;
            change = change % 2;
            cent2TextBox.Text = Cent2Euro.ToString();

            int Cent1Euro = (int)change;
            //change = change % 1;
            cent1TextBox.Text = Cent1Euro.ToString();

        }
    }
}