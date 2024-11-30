using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
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

namespace Bibliotheek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string userfirstName = "";
        string userName = "";

        string bookCode = "";

        List<string> borrowedList = new List<string>();
        List<string> availableList = new List<string>();
        List<string> totalList = new List<string>();

        //string[,] availableArray = new string[100, 2];   //Kolommen : boekcode,titel
        //string[,] borrowedArray = new string[100, 4];    //Kolommen : boekcode,titel,Naam,Voornaam
        //string[,] totalArray = new string[100, 2];       //Kolommen : boekcode, title

        public MainWindow()
        {
            InitializeComponent();
        }

        private void saveUser_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text.Length > 0 && firstNameTextBox.Text.Length > 0)
            {
                userfirstName = firstNameTextBox.Text;
                userName = nameTextBox.Text;

                MessageBox.Show($"Hallo {userfirstName} {userName}\nblij je terug te zien", "Begroeting", MessageBoxButton.OK, MessageBoxImage.Information);
                userStackPanel.Visibility = Visibility.Collapsed;
                selectionStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Vul de onbrekende gegevens in", "Niet volledig", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (firstNameTextBox.Text.Length == 0)
                {
                    firstNameTextBox.Focus();
                }
                else
                {
                    nameTextBox.Focus();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            userStackPanel.Visibility = Visibility.Visible;
            borrowBookGroupBox.Visibility = Visibility.Collapsed;
            addBookGroupBox.Visibility = Visibility.Collapsed;
            returnBookGroupBox.Visibility = Visibility.Collapsed;

            string temp;
            for (int i = 0; i < 25; i++)
            {
                temp = (i + 1).ToString();
                while (temp.Length < 3)
                {
                    temp = "0" + temp;
                }
                string newBookCode = $"B{temp}";
                string newBookTitle = "Book " + (i + 1).ToString();

                availableList.Add($"{newBookTitle}\t{newBookCode}");
                totalList.Add($"{newBookTitle}\t{newBookCode}");
            }

            fillAvailable();

            //Moet verwijdert worden
            //firstNameTextBox.Text = userfirstName = "Peter";
            //nameTextBox.Text = userName = "Hauben";
        }


        private void addBookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            addBookGroupBox.Visibility = Visibility.Visible;
            menuMenu.IsEnabled = false;
        }

        private void borrowBookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            borrowBookGroupBox.Visibility = Visibility.Visible;
            borrowBookButton.IsEnabled = false;
            menuMenu.IsEnabled = false;

            firstNameBorrowTextBox.Text = userfirstName;
            nameBorrowTextBox.Text = userName;

            fillAvailable();

        }


        private void fillAvailable()
        {
            availableListBox.Items.Clear();

            availableList.Sort();

            foreach (var item in availableList)
            {
                availableListBox.Items.Add(item);
            }
            int X = 0;

        }

        private void fillBorrowed()
        {
            borrowedListBox.Items.Clear();

            foreach (var item in borrowedList)
            {
                if (item != null)
                {
                    borrowedListBox.Items.Add(item);
                }
            }
        }

        private void returnBookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            returnBookGroupBox.Visibility = Visibility.Visible;
            returnBookButton.IsEnabled = false;
            menuMenu.IsEnabled = false;

            firstNameReturnTextBox.Text = userfirstName;
            nameReturnTextBox.Text = userName;

            fillBorrowed();
        }

        private void addBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (bookTitleTextBox.Text.Length > 0)
            {
                int counter = 0;
                int index = 1;
                foreach (var item in totalList)
                {
                    string temp = item.ToString();
                    string[] tempArray = temp.Split('\t');

                    if (tempArray[0].ToUpper() == bookTitleTextBox.Text.ToUpper())
                    {
                        MessageBox.Show("Dit boek bestaat al\ngeef ander boek in", "Boek bestaat reeds!", MessageBoxButton.OK);

                        bookTitleTextBox.Focus();
                        bookTitleTextBox.Text = "";

                        return;
                    }
                }

                if (index % 2 == 1)
                {
                    index++;
                }

                bookCodeTextBox.Text = GenerateCode(); 

                availableList.Add($"{bookTitleTextBox.Text}\t{bookCode}");
                totalList.Add($"{bookTitleTextBox.Text}\t{bookCode}");

                fillAvailable();

                MessageBoxResult result = MessageBox.Show("Boek is opgeslagen,\nWil je nog een boek toevoegen?", "Opslag geslaagd", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

                if (result == MessageBoxResult.Yes)
                {
                    bookTitleTextBox.Focus();
                    bookTitleTextBox.Text = "";

                    bookCodeTextBox.Text = "";
                }
                else
                {
                    addBookGroupBox.Visibility = Visibility.Collapsed;
                    menuMenu.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Vul de titel van de nieuwe boek in", "Vul gegevens in", MessageBoxButton.OK);
                bookTitleTextBox.Focus();
                bookTitleTextBox.Text = "";
            }
        }

        private string GenerateCode()
        {
            bookCode = "B";

            int bookNumber = totalList.Count() + 1;

            string bookString = bookNumber.ToString();

            while (bookString.Length < 3)
            {
                bookString = "0" + bookString;
            }

            bookCode += bookString;

            return bookCode;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            menuMenu.IsEnabled = true;
            addBookGroupBox.Visibility = Visibility.Collapsed;
        }

        private void borrowBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (availableListBox.SelectedIndex > -1)
            {
                string selectedItem = availableListBox.SelectedItem.ToString();

                borrowedList.Add(selectedItem + $"\t{userfirstName}\t{userName}");

                borrowedList.Sort();

                fillBorrowed();

                availableList.Remove(selectedItem);

                availableList.Sort();

                fillAvailable();
            }
            else
            {
                MessageBox.Show("Selecteer een boek", "Geen boek geselecteerd", MessageBoxButton.OK);
            }
        }

        private void borrowCancelButton_Click(object sender, RoutedEventArgs e)
        {
            borrowBookGroupBox.Visibility = Visibility.Collapsed;
            menuMenu.IsEnabled = true;
        }

        private void returnBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (borrowedListBox.SelectedIndex > -1)
            {
                if (borrowedListBox.SelectedIndex > -1)
                {
                    string selectedItem = borrowedListBox.SelectedItem.ToString();

                    borrowedList.Remove(selectedItem);

                    string[] split = selectedItem.Split('\t');
                    string bookCode = split[0];
                    string bookTitle = split[1];
                    string userFirstName = split[2];
                    string userLastName = split[3];

                    selectedItem = $"{bookCode}\t{bookTitle}";

                    availableList.Add(selectedItem);
                }

                fillBorrowed();

                int X = 0;
            }
            else
            {
                MessageBox.Show("Selecteer een boek", "Geen boek geselecteerd", MessageBoxButton.OK);
            }
        }

        private void returnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            returnBookGroupBox.Visibility = Visibility.Collapsed;
            menuMenu.IsEnabled = true;
        }

        private void checkUserBorrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (firstNameBorrowTextBox.Text.Length == 0 || nameBorrowTextBox.Text.Length == 0)
            {
                MessageBox.Show("Geef eerst je gegevens in", "Gegevens invullen", MessageBoxButton.OK);

                if (firstNameBorrowTextBox.Text.Length == 0)
                {
                    firstNameBorrowTextBox.Focus();
                }
                else
                {
                    nameBorrowTextBox.Focus();
                }
            }
            else
            {
                firstNameBorrowTextBox.IsEnabled = false;
                nameBorrowTextBox.IsEnabled = false;

                borrowBookButton.IsEnabled = true;
                checkUserBorrowButton.IsEnabled = true;
            }
        }

        private void checkUserReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (firstNameReturnTextBox.Text.Length == 0 || nameReturnTextBox.Text.Length == 0)
            {
                MessageBox.Show("Geef eerst je gegevens in", "Gegevens invullen", MessageBoxButton.OK);

                if (firstNameReturnTextBox.Text.Length == 0)
                {
                    firstNameReturnTextBox.Focus();
                }
                else
                {
                    nameReturnTextBox.Focus();
                }
            }
            else
            {
                firstNameReturnTextBox.IsEnabled = false;
                nameReturnTextBox.IsEnabled = false;

                returnBookButton.IsEnabled = true;
                checkUserReturnButton.IsEnabled = true;
            }
        }

        private void borrowSortButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (string item in availableListBox.Items)
            {
                availableList.Add(item);
            }

            fillAvailable();
        }

        private void returnSortButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (string item in borrowedListBox.Items)
            {
                borrowedList.Add(item);
            }

            borrowedList.Sort();

            fillBorrowed();
        }

        private void searchBorrowedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp;

            temp = searchBorrowedTextBox.Text;
            temp = borrowedList.Find(element => element.ToUpper().StartsWith(temp.ToUpper()));

            borrowedListBox.SelectedIndex = borrowedListBox.Items.IndexOf(temp);

            borrowedListBox.ScrollIntoView(borrowedListBox.SelectedItem);
        }

        private void searchAvailableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp;

            temp = searchAvailableTextBox.Text;
            temp = availableList.Find(element => element.ToUpper().StartsWith(temp.ToUpper()));

            availableListBox.SelectedIndex = availableListBox.Items.IndexOf(temp);

            availableListBox.ScrollIntoView(availableListBox.SelectedItem);
        }
    }

}