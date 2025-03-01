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
            orderGroupBox.Visibility = Visibility.Visible;
            inOrderGroupBox.Visibility = Visibility.Collapsed;
            statisticsGroupBox.Visibility = Visibility.Collapsed;

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
                countDeleteOrdersComboBox.Items.Add(i);
            }

            foreach (var order in orderItemsList)
            {
                dishesListBox.Items.Add(order.dish);
            }

            int X = 0;
        }

        private void addOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (countOrdersComboBox.SelectedIndex >= 0 && dishesListBox.SelectedIndex >= 0) 
            {
                PlaceOrder(dishesListBox.SelectedItem.ToString(), countOrdersComboBox.SelectedIndex + 1);
            }

            countOrdersComboBox.SelectedIndex = -1;
        }

        private void PlaceOrder(string selectedDish, int amountOrdered)
        {
            foreach (var item in orderedItemsList)
            {
                if (item.dish == dishesListBox.SelectedItem.ToString())
                {
                    item.amount += amountOrdered;
                    RefreshOrders();
                    return;
                }
            }

            Orders newOrder = new Orders();

            newOrder.dish = dishesListBox.SelectedItem.ToString();
            newOrder.amount = amountOrdered;

            orderedItemsList.Add(newOrder);

            RefreshOrders();
        }

        private void RefreshOrders()
        {
            inOrderListBox.Items.Clear();
            foreach (var item in orderedItemsList)
            {
                inOrderListBox.Items.Add($"{item.dish} - {item.amount}");
            }
        }

        private void deleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (inOrderListBox.SelectedIndex >= 0 && countDeleteOrdersComboBox.SelectedIndex >= 0)
            {
                string temp = inOrderListBox.SelectedItem.ToString();
                temp = temp.Substring(0, temp.IndexOf(" - "));

                foreach (var item in orderedItemsList)
                {
                    if (item.dish == temp)
                    {
                        item.amount -= countDeleteOrdersComboBox.SelectedIndex + 1;

                        countDeleteOrdersComboBox.SelectedIndex = -1;

                        if (item.amount <= 0)
                        {
                            orderedItemsList.Remove(item);
                            RefreshOrders();
                            return;
                        }
                        else
                        {
                            RefreshOrders();
                        }
                    }
                }
            }
        }

        private void inOrderListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int X = 0;
        }

    }
}