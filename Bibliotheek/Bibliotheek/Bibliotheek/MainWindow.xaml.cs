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
        string userFirstname = "";
        string userName = "";

        string bookCode = "";

        List<string> borrowedList = new List<string>();
        List<string> availableList = new List<string>();
        List<string> totalList = new List<string>();

        // Vervangen door bovenstaande lijsten
        //string[,] availableArray = new string[100, 2];   //Kolommen : Boektitel,boekcode
        //string[,] borrowedArray = new string[100, 4];    //Kolommen : Boektitel,boekcode,Vooraam,Naam
        //string[,] totalArray = new string[100, 2];       //Kolommen : boekcode, title

        public MainWindow()
        {
            InitializeComponent();
        }

        private void saveUser_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text.Length > 0 && firstnameTextBox.Text.Length > 0)
            {
                userFirstname = firstnameTextBox.Text;
                userName = nameTextBox.Text;

                MessageBox.Show($"Hallo {userFirstname} {userName}\nblij je terug te zien", "Begroeting", MessageBoxButton.OK, MessageBoxImage.Information);
                userStackPanel.Visibility = Visibility.Collapsed;
                selectionStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Vul de onbrekende gegevens in", "Niet volledig", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (firstnameTextBox.Text.Length == 0)
                {
                    firstnameTextBox.Focus();
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
            selectionStackPanel.Visibility = Visibility.Collapsed;
#if DEBUG
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

            FillAvailable();

            firstnameTextBox.Text = userFirstname = "Peter";
            nameTextBox.Text = userName = "Hauben";
#endif
        }


        private void addBookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            makeVisible(sender);
            
            bookTitleTextBox.Focus();
        }

        private void borrowBookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            makeVisible(sender);

            borrowBookButton.IsEnabled = false;

            nameBorrowTextBox.IsEnabled = true;

            borrowBookButton.IsEnabled = false;

            FillAvailable();

            nameBorrowTextBox.Text = userName;
            firstnameBorrowTextBox.Text = userFirstname;

            firstnameBorrowTextBox.IsEnabled = true;
            firstnameBorrowTextBox.SelectAll();
            firstnameBorrowTextBox.Focus();
        }

        private void returnBookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            makeVisible(sender);

            returnBookButton.IsEnabled = false;  

            firstnameReturnTextBox.Text = userFirstname;
            nameReturnTextBox.Text = userName;

            nameReturnTextBox.IsEnabled = true;
            borrowBookButton.IsEnabled = false;

            FillBorrowed();

            firstnameReturnTextBox.IsEnabled = true;
            firstnameReturnTextBox.SelectAll();
            firstnameReturnTextBox.Focus();
        }

        private void makeVisible(object sender)
        {
            if(sender is MenuItem mni)
            {
                if (mni.Name == "addBookMenuItem")
                {
                    addBookGroupBox.Visibility = Visibility.Visible;
                }
                else
                {
                    addBookGroupBox.Visibility = Visibility.Collapsed;
                }

                if (mni.Name == "borrowBookMenuItem")
                {
                    borrowBookGroupBox.Visibility = Visibility.Visible;
                }
                else
                {
                    borrowBookGroupBox.Visibility = Visibility.Collapsed;
                }

                if (mni.Name == "returnBookMenuItem")
                {
                    returnBookGroupBox.Visibility = Visibility.Visible;
                }
                else
                {
                    returnBookGroupBox.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void FillAvailable()
        {
            availableListBox.Items.Clear();

            availableList.Sort();

            foreach (var item in availableList)
            {
                availableListBox.Items.Add(item);
            }
        }

        private void FillBorrowed()
        {
            borrowedListBox.Items.Clear();

            borrowedList.Sort();

            foreach (var item in borrowedList)
            {
                if (item != null)
                {
                    borrowedListBox.Items.Add(item);
                }
            }
        }

        private void addBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (bookTitleTextBox.Text.Length > 0)
            {
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

                bookCodeTextBox.Text = GenerateCode();

                availableList.Add($"{bookTitleTextBox.Text}\t{bookCode}");
                totalList.Add($"{bookTitleTextBox.Text}\t{bookCode}");

                FillAvailable();

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
            string bookString = (totalList.Count() + 1).ToString();

            while (bookString.Length < 3)
            {
                bookString = "0" + bookString;
            }

            bookCode += "B" + bookString;

            return bookCode;
        }

        //private void cancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    menuMenu.IsEnabled = true;
        //    addBookGroupBox.Visibility = Visibility.Collapsed;
        //}

        private void borrowBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (availableListBox.SelectedIndex > -1)
            {
                string selectedItem = availableListBox.SelectedItem.ToString();

                borrowedList.Add(selectedItem + $"\t{userFirstname}\t{userName}");

                FillBorrowed();

                availableList.Remove(selectedItem);

                FillAvailable();
            }
            else
            {
                MessageBox.Show("Selecteer een boek", "Geen boek geselecteerd", MessageBoxButton.OK);
            }
        }

        //private void borrowCancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    borrowBookGroupBox.Visibility = Visibility.Collapsed;
        //    menuMenu.IsEnabled = true;
        //}

        private void returnBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (borrowedListBox.SelectedIndex > -1)
            {
                string selectedItem = borrowedListBox.SelectedItem.ToString();

                borrowedList.Remove(selectedItem);

                string[] split = selectedItem.Split('\t');
                string bookTitle = split[0];
                string bookCode = split[1];
                string userFirstName = split[2];
                string userLastName = split[3];

                selectedItem = $"{bookTitle}\t{bookCode}";

                availableList.Add(selectedItem);

                FillBorrowed();
            }
            else
            {
                MessageBox.Show("Selecteer een boek", "Geen boek geselecteerd", MessageBoxButton.OK);
            }
        }

        //private void returnCancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    returnBookGroupBox.Visibility = Visibility.Collapsed;
        //    menuMenu.IsEnabled = true;
        //}

        private void checkUserBorrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (firstnameBorrowTextBox.Text.Length == 0 || nameBorrowTextBox.Text.Length == 0)
            {
                MessageBox.Show("Geef eerst je gegevens in", "Gegevens invullen", MessageBoxButton.OK);

                if (firstnameBorrowTextBox.Text.Length == 0)
                {
                    firstnameBorrowTextBox.Focus();
                }
                else
                {
                    nameBorrowTextBox.Focus();
                }
            }
            else
            {
                firstnameBorrowTextBox.IsEnabled = false;
                nameBorrowTextBox.IsEnabled = false;

                userFirstname = firstnameBorrowTextBox.Text;
                userName = nameBorrowTextBox.Text;

                borrowBookButton.IsEnabled = true;
                checkUserBorrowButton.IsEnabled = true;
            }
        }

        private void checkUserReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (firstnameReturnTextBox.Text.Length == 0 || nameReturnTextBox.Text.Length == 0)
            {
                MessageBox.Show("Geef eerst je gegevens in", "Gegevens invullen", MessageBoxButton.OK);

                if (firstnameReturnTextBox.Text.Length == 0)
                {
                    firstnameReturnTextBox.Focus();
                }
                else
                {
                    nameReturnTextBox.Focus();
                }
            }
            else
            {
                firstnameReturnTextBox.IsEnabled = false;
                nameReturnTextBox.IsEnabled = false;

                userFirstname = firstnameReturnTextBox.Text;
                userName = nameReturnTextBox.Text;

                returnBookButton.IsEnabled = true;
                checkUserReturnButton.IsEnabled = true;
            }
        }


        private void searchBorrowedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string? temp;

            temp = searchBorrowedTextBox.Text;
            temp = borrowedList.Find(element => element.ToUpper().StartsWith(temp.ToUpper()));

            borrowedListBox.SelectedIndex = borrowedListBox.Items.IndexOf(temp);

            borrowedListBox.ScrollIntoView(borrowedListBox.SelectedItem);
        }

        private void searchAvailableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string? temp;

            temp = searchAvailableTextBox.Text;
            temp = availableList.Find(element => element.ToUpper().StartsWith(temp.ToUpper()));

            availableListBox.SelectedIndex = availableListBox.Items.IndexOf(temp);

            availableListBox.ScrollIntoView(availableListBox.SelectedItem);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox txt)
            {
                txt.SelectAll();
            }
        }

    }

}