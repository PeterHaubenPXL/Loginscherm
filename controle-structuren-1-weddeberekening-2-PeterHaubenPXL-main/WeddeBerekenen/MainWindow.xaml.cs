using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeddeBerekenen
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

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            double hourlyWage;
            int numberOfHours;
            string employeeName;

            bool isHourlyWage = double.TryParse(hourlyWageTextBox.Text, out hourlyWage);
            bool isNumberOfHours = int.TryParse(numberOfHoursTextBox.Text, out numberOfHours);

            if (isHourlyWage && isNumberOfHours)
            {
                double grossAnnualSalary = Math.Round(hourlyWage * numberOfHours, 2);
                double changedGrossAnnualSalary = grossAnnualSalary;
                double vat = 0;

                while (changedGrossAnnualSalary > 10000)
                {
                    if (changedGrossAnnualSalary > 50000)
                    {
                        vat += Math.Round((changedGrossAnnualSalary - 50000) * 0.5, 2);
                        changedGrossAnnualSalary = changedGrossAnnualSalary - (changedGrossAnnualSalary - 50000);
                    }
                    else if (changedGrossAnnualSalary > 25000)
                    {
                        vat += Math.Round((changedGrossAnnualSalary - 25000) * 0.4, 2);
                        changedGrossAnnualSalary = changedGrossAnnualSalary - (changedGrossAnnualSalary - 25000);
                    }
                    else if (changedGrossAnnualSalary > 15000)
                    {
                        vat += Math.Round((changedGrossAnnualSalary - 15000) * 0.3, 2);
                        changedGrossAnnualSalary = changedGrossAnnualSalary - (changedGrossAnnualSalary - 15000);
                    }
                    else if (changedGrossAnnualSalary > 10000)
                    {
                        vat += Math.Round((changedGrossAnnualSalary - 10000) * 0.2, 2);
                        changedGrossAnnualSalary = changedGrossAnnualSalary - (changedGrossAnnualSalary - 10000);
                    }
                }


                double netAnnualSalary = Math.Round(grossAnnualSalary - vat, 2);

                employeeName = employeeTextBox.Text;

                resultTextBox.Text = $"LOONFISCHE VAN {employeeName}\n" +
                                        $"Aantal gewerkte uren: {numberOfHours}\n" +
                                        $"Uurloon: € {hourlyWage}\n" +
                                        $"Brutojaarwerde: € {grossAnnualSalary}\n" +
                                        $"Belastingen: € {vat}\n" +
                                        $"Nettojaarwedde: € {netAnnualSalary}";
            }
            else
            {
                if (!isHourlyWage)
                {
                    MessageBox.Show("'Uurloon' is fout", "Foute ingave");
                }
                else
                {
                    MessageBox.Show("'Aantal uren' is fout", "Foute ingave");
                }
            }
        }

        private void cleanButton_Click(object sender, RoutedEventArgs e)
        {
            employeeTextBox.Text = "";
            employeeTextBox.Focus();

            hourlyWageTextBox.Text = "17,85";
            numberOfHoursTextBox.Text = "1686";
            resultTextBox.Text = "";
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}