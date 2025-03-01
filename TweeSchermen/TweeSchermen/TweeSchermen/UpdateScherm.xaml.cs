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
    /// Interaction logic for UpdateScherm.xaml
    /// </summary>
    public partial class UpdateScherm : Window
    {
        

        public UpdateScherm(string label)
        {
            InitializeComponent();

            upDateLabel.Content = Data.MyTekst;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //upDateLabel.Content = 
        }
    }
}
