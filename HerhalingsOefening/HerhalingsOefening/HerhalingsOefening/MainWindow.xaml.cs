using Microsoft.VisualBasic;
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
using System.Windows.Threading;

namespace HerhalingsOefening
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private DispatcherTimer timer = new DispatcherTimer();
       
        TimeSpan elapsed = new TimeSpan(0,0,0);

        DateTime startTime = new DateTime(2024,1,1,0,0,0);

        bool isFirstPeriod = true;

        int sec = 0;
        int min = 0;

        public MainWindow()
        {
            InitializeComponent();

            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            sec++;

            if (sec == 60)
            {
;                sec = 0;
                 min++;
            }

            string stringSec = sec.ToString();
            string stringMin = min.ToString();

            if(stringSec.Length == 1)
            {
                stringSec = "0" + stringSec;
            }

            if (stringMin.Length == 1) 
            { 
                stringMin = "0" + stringMin;
            }

            timerTextBox.Text = stringMin + ":" + stringSec;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Interval = new TimeSpan(0, 0, 1);

            if (isFirstPeriod)
            {
                sec = 0;
                min = 0;
            }
            else
            {
                sec = 0;
                min = 45;
            }

            timer.Start();

            homeRedButton.IsEnabled = true;
            homeYellowButton.IsEnabled = true;
            homeScoreButton.IsEnabled = true;

            outRedButton.IsEnabled = true;
            outYellowButton.IsEnabled = true;
            outScoreButton.IsEnabled = true;
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            resultButton.IsEnabled = true;

            isFirstPeriod = false;


        }

        private void resultButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Home\nScore: " + homeScoreButton.Content.ToString() +"\nGele kaarten: ");
        }

        private void homeRedButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                int.TryParse(btn.Content.ToString(), out int input);
                input++;

                btn.Content = input.ToString();
            }
        }

        private void homeYellowButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                int.TryParse(btn.Content.ToString(), out int input);
                input++;

                btn.Content = input.ToString();
            }
        }

        private void outYellowButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                int.TryParse(btn.Content.ToString(), out int input);
                input++;

                btn.Content = input.ToString();
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                int.TryParse(btn.Content.ToString(), out int input);
                input++;

                btn.Content = input.ToString();
            }
        }

        private void homeScoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                int.TryParse(btn.Content.ToString(), out int input);
                input++;

                btn.Content = input.ToString();
            }

            string backNumber = Interaction.InputBox("Geef rugnummer in:", "Rugnummer", "0", 50, 50);
        }

        private void outScoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                int.TryParse(btn.Content.ToString(), out int input);
                input++;

                btn.Content = input.ToString();
            }

            string backNumber = Interaction.InputBox("Geef rugnummer in:", "Rugnummer", "0", 50, 50);
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            // Alles resetten


        }
    }
}