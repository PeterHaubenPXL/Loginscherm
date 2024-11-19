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

namespace Mastermind
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            generateLabels();
        }

        private void generateLabels()
        {
            int Aantal = 5;

            //Textboxen genereren
            for (int i = 1; i <= 12; i++)
            {
                if (i == 1)
                {
                    for (int j = 1; j <= 6; j++)
                    {
                        Label lbl = new Label();
                        lbl.Name = $"colorLabel{j}";
                        lbl.Width = 54;
                        lbl.Height = 54;
                        lbl.Margin = new Thickness(8);

                        Grid.SetRow(lbl, i);
                        Grid.SetColumn(lbl, j);

                        switch (j)
                        {
                            case 1:
                                lbl.Background = Brushes.Red;
                                break;
                            case 2:
                                lbl.Background = Brushes.Yellow;
                                break;
                            case 3:
                                lbl.Background = Brushes.Orange;
                                break;
                            case 4:
                                lbl.Background = Brushes.White;
                                break;
                            case 5:
                                lbl.Background = Brushes.Green;
                                break;
                            case 6:
                                lbl.Background = Brushes.Blue;
                                break;
                        }

                        mainGrid.Children.Add(lbl);
                    }
                }
                else if (i >= 3)
                {
                    for (int j = 1; j <= Aantal; j++)
                    {
                        Label lbl = new Label();
                        lbl.Name = $"serie{i - 2}{j - 1}";
                        lbl.Width = 54;
                        lbl.Height = 54;
                        lbl.Margin = new Thickness(8);
                        lbl.ToolTip = lbl.Name;
                        //lbl.Visibility = Visibility.Hidden;

                        if (j == 1)
                        {
                            lbl.FontSize = 28;
                            lbl.Content = (i - 2).ToString();
                            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                            lbl.VerticalContentAlignment = VerticalAlignment.Center;
                        }

                        Grid.SetRow(lbl, i);
                        Grid.SetColumn(lbl, j);

                        lbl.Background = Brushes.WhiteSmoke;

                        switch (i)
                        {
                            case 3: 
                                serie1StackPanel.Children.Add(lbl);
                                break;
                            case 4: 
                                serie2StackPanel.Children.Add(lbl);
                                break;
                            case 5:
                                serie3StackPanel.Children.Add(lbl);
                                break;
                            case 6:
                                serie4StackPanel.Children.Add(lbl);
                                break;
                            case 7:
                                serie5StackPanel.Children.Add(lbl);
                                break;
                            case 8:
                                serie6StackPanel.Children.Add(lbl);
                                break;
                            case 9:
                                serie7StackPanel.Children.Add(lbl);
                                break;
                            case 10:
                                serie8StackPanel.Children.Add(lbl);
                                break;
                            case 11:
                                serie9StackPanel.Children.Add(lbl);
                                break;
                            case 12:
                                serie10StackPanel.Children.Add(lbl);
                                break;
                        }

                        
                    }
                }
            }
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Code genereren

            // Labels Visible maken

            
        }

        private void ControlButton_Click(object sender, RoutedEventArgs e)
        {
            // Code checken
        }
    }

}