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

namespace RestaurantBestelSysteem
{

   

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class Orders()
        {
            public string dish = "";
            public int amount = 0;
        }

        List<Orders> orderItemsList = new List<Orders>();
        List<Orders> orderedItemsList = new List<Orders>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void newOrderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            orderGroupBox.Visibility = Visibility.Visible;
            inOrderGroupBox.Visibility = Visibility.Collapsed;
            statisticsGroupBox.Visibility = Visibility.Collapsed;
        }

        private void showOrderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            orderGroupBox.Visibility = Visibility.Collapsed;
            inOrderGroupBox.Visibility = Visibility.Visible;
            statisticsGroupBox.Visibility = Visibility.Collapsed;
        }

        private void statisticsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            orderGroupBox.Visibility = Visibility.Collapsed;
            inOrderGroupBox.Visibility = Visibility.Collapsed;
            statisticsGroupBox.Visibility = Visibility.Visible;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //orderItemsList.Add(dish,amount);
            Orders item1 = new Orders();

            item1.dish = "Pizza";
            item1.amount = 0;

            orderItemsList.Add(item1);

            Orders item2 = new Orders();

            item2.dish = "Pasta";
            item2.amount = 0;

            orderItemsList.Add(item2);

            Orders item3 = new Orders();

            item3.dish = "Hamburger";
            item3.amount = 0;

            orderItemsList.Add(item3);

            for (int i = 1; i <= 10; i++)
            {
                countOrdersComboBox.Items.Add(i);
            }

            foreach (var order in orderItemsList)
            {
                dishesListBox.Items.Add(order.dish);
                inOrderListBox.Items.Add(order.dish + $"\t{order.amount}");

            }

            int X = 0;
        }
    }
}