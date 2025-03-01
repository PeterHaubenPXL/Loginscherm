using ClassLibTeam04.Business;
using ClassLibTeam04.Data.Framework;
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

namespace WPFTeam04
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
            LoadData();
        }


        private void LoadData()
        {
            studentDataGrid.ItemsSource = null;
            studentDataGrid.ItemsSource = Students.List();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            newStudentStackPanel.Visibility = Visibility.Hidden;
        }

        private void newStudentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            newStudentStackPanel.Visibility = Visibility.Visible;
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

        private void closeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void BtnGetFrameworkData_Click(object sender, RoutedEventArgs e)
        {
            SelectResult result = Students.GetStudents();
            if (result.Succeeded)
            {
                studentDataGrid.ItemsSource = null;
                studentDataGrid.ItemsSource = result.DataTable.DefaultView;
            }
        }

        private void insertStudentButton_Click(object sender, RoutedEventArgs e)
        {
            InsertResult result = Students.Add(firstNameTextBox.Text, lastNameTextBox.Text);
        }

    }

}
