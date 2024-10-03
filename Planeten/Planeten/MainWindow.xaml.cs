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

namespace Planeten
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Globale variables

        const double MercuriusDagenPerJaar = 88;
        const double VenusdagenDagenPerJaar = 225;
        const double AardeDagenPerJaar = 365;
        const double MarsDagenPerJaar = 687;
        const double JupiterDagenPerJaar = 4333;
        const double SaturnusDagenPerJaar = 10759;
        const double UranusDagenPerJaar = 30687;
        const double NeptunusDagenPerJaar = 60190;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isJaar = double.TryParse(TxtAantalJaren.Text, out double jaar);

            double AantalMercurius = jaar * MercuriusDagenPerJaar;
            double AantalVenus = jaar * VenusdagenDagenPerJaar;
            double AantalAarde = jaar * AardeDagenPerJaar;
            double AantalMars = jaar * MarsDagenPerJaar;
            double AantalJupiter = jaar * JupiterDagenPerJaar;
            double AantalSaturnus = jaar * SaturnusDagenPerJaar;
            double AantalUranus = jaar * UranusDagenPerJaar;
            double AantalNeptunus = jaar * NeptunusDagenPerJaar;

            LblMecuryDays.Content = AantalMercurius.ToString();
            LblVenusDays.Content = AantalVenus.ToString();
            LblEarthDays.Content = AantalAarde.ToString();
            LblMarsDays.Content = AantalMars.ToString();
            LblJupiterDays.Content = AantalJupiter.ToString();
            LblSaturnDays.Content = AantalSaturnus.ToString();
            LblUranusDays.Content = AantalUranus.ToString();
            LblNeptuneDays.Content = AantalNeptunus.ToString();
        }

    }

}