using ClassLibTeam04.Business;
using ClassLibTeam04.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTeam04
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public StudentWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StudentDataGrid.ItemsSource = Students.List();

            LoadData();
        }


        private void LoadData()
        {
            StudentDataGrid.ItemsSource = null;
            StudentDataGrid.ItemsSource = Students.List();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            newStudentStackPanel.Visibility = Visibility.Hidden;
        }

        private void NewStudentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            newStudentStackPanel.Visibility = Visibility.Visible;
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SqlMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SqlWindow sqlwin = new SqlWindow();

            sqlwin.ShowDialog();
        }

        private void addStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (firstNameTextBox.Text.Length > 0 && lastNameTextBox.Text.Length > 0)
            {
                Students.Add(firstNameTextBox.Text, lastNameTextBox.Text);
                LoadData();
                newStudentStackPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Incorrect data!");
            }
        }

    }

}
