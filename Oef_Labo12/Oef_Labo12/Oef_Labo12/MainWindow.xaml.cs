using System;
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

namespace Oef_Labo12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> list = new List<string>();
        string temp;
        List<string> selectedList = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            if(newProductTextBox.Text.Length > 0)
            {
                //availebaleListBox.Items.Add(newProductTextBox.Text);
                list.Add(newProductTextBox.Text);
                list.Sort();

                OrderInListBox();
            }
        }

        private void AvailebaleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addToStoreButton.IsEnabled = true;
            deleteFromStoreButton.IsEnabled = false;
        }

        private void addToStoreButton_Click(object sender, RoutedEventArgs e)
        {
            productsInStoreListBox.Items.Add(availebaleListBox.SelectedItem);
        }

        private void deleteFromStoreButton_Click(object sender, RoutedEventArgs e)
        {
            productsInStoreListBox.Items.Remove(productsInStoreListBox.SelectedItem);
        }

        private void productsInStoreListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addToStoreButton.IsEnabled = false;
            deleteFromStoreButton.IsEnabled = true;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            temp = searchTextBox.Text;
            temp = list.Find(element => element.StartsWith(temp));
            availebaleListBox.SelectedIndex = availebaleListBox.Items.IndexOf(temp);
            
            availebaleListBox.ScrollIntoView(availebaleListBox.SelectedItem);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for(int i=90; i >= 65; i--)
            {
                for (int j = 1; j < 10; j++)
                {
                    list.Add(Convert.ToChar(i) + j.ToString());
                }
            }

            for (int i = 97; i <= 122; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    list.Add(Convert.ToChar(i) + j.ToString());
                }
            }

            list.Sort();

            OrderInListBox();
        }


        private void OrderInListBox()
        {
            availebaleListBox.Items.Clear();

            foreach (var item in list)
            {
                availebaleListBox.Items.Add(item.ToString());
            }
        }

        // bijkomend Search
    }
}