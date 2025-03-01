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

namespace TweeSchermen
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {

        public static string UserInput;

        public StartWindow()
        {
            InitializeComponent();
        }

        private void UpdateSchermButton_Click(object sender, RoutedEventArgs e)
        {
            Data.MyTekst = TekstTextBox.Text;

            UpdateScherm updateScherm = new UpdateScherm(TekstTextBox.Text);
            updateScherm.ShowDialog();

        }
    }
}
