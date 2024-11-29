using System.CodeDom.Compiler;
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

        //List<string> bookList = new List<string>();
        //List<string> bookCodeList = new List<string>();

        string[,] availableArray = new string[100, 2];   //Kolommen : boekcode,titel
        string[,] borrowedArray = new string[100, 4];    //Kolommen : Naam,Voornaam,boekcode,titel
        string[,] totalArray = new string[100, 2];

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
                availableArray[i, 0] = $"B{temp}";
                availableArray[i, 1] = "Book " + (i+1).ToString();
            }
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

            fillAvailable();
        }


        private void fillAvailable()
        {
            int counter = 0;

            string loadInListbox = "";

            availableListBox.Items.Clear();

            foreach (var item in availableArray)
            {
                if (item != null)
                {
                    if (counter % 2 == 0)
                    {
                        loadInListbox = item;
                        counter++;
                    }
                    else
                    {
                        loadInListbox += $"\t{item}";
                        availableListBox.Items.Add(loadInListbox);
                        counter++;
                    }
                }
                else
                {
                    return;
                }

            }
        }

        private void returnBookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            returnBookGroupBox.Visibility = Visibility.Visible;
            returnBookButton.IsEnabled = false;
            menuMenu.IsEnabled = false;
        }

        private void addBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (bookTitleTextBox.Text.Length > 0)
            {
                int counter = 0;
                foreach (var item in totalArray)
                {
                    if (item == null)
                    {
                        break;
                    }
                    else
                    {
                        if (counter % 2 == 1)
                        {
                            if (item.ToUpper() == bookTitleTextBox.Text.ToUpper())
                            {
                                MessageBox.Show("Dit boek bestaat al\ngeef ander boek in", "Boek bestaat reeds!", MessageBoxButton.OK);

                                bookTitleTextBox.Focus();
                                bookTitleTextBox.Text = "";
                                return;
                            }
                        }
                        counter++;
                    }
                }

                bookCodeTextBox.Text = GenerateCode();

                for (int i = 0; i < 100; i++)
                {
                    if (availableArray[i, 0] == null)
                    {
                        availableArray[i, 0] = "[" + bookCodeTextBox.Text + "]";
                        availableArray[i, 1] = bookTitleTextBox.Text;
                        totalArray[i,0] = "[" + bookCodeTextBox.Text + "]";
                        totalArray[i,1] = bookTitleTextBox.Text;
                        break;
                    }
                }

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
            }
        }

        private string GenerateCode()
        {
            bookCode = "B";

            int counter = 1;
            int bookNumber = 1;
            foreach (var item in availableArray)
            {
                if (item == null)
                {
                    string bookString = bookNumber.ToString();

                    while (bookString.Length < 3)
                    {
                        bookString = "0" + bookString;
                    }

                    bookCode += bookString;
                    return bookCode;
                }
                else
                {
                    counter++;
                }

                if (counter % 2 == 1)
                {
                    bookNumber++;
                }
            }

            return null;
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
                // ToDo
                MessageBox.Show("WIP", "ToDo!!!", MessageBoxButton.OK);

                string temp = availableListBox.SelectedItem.ToString();
                temp = temp.Substring(0, 6);

                // Data opslaan naam, voornaam, SelectedBook in uitgeleend

                int counter = 1;

                foreach (var item in availableArray)
                {
                    if (item == temp)
                    {
                        int X = 0;
                    }
                }

                // Book verwijderen uit availableListBox
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
                // ToDo
                MessageBox.Show("WIP", "ToDo!!!", MessageBoxButton.OK);

                // Boek verwijderen uit BorrowedLijst

                // Boek terug toevoegen aan Availeble List
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
                checkUserBorrowButton.IsEnabled = false;
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
                checkUserReturnButton.IsEnabled = false;
            }
        }
    }
}