using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection;
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

namespace Mastermind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Globale variable

        Brush chosenColor;

        int attempts = 0;

        int colorCode1 = 0;
        int colorCode2 = 0;
        int colorCode3 = 0;
        int colorCode4 = 0;

        int chosenColorCode1 = 0;
        int chosenColorCode2 = 0;
        int chosenColorCode3 = 0;
        int chosenColorCode4 = 0;

        bool dissolved = false;
        bool gameStarted = false;

        DispatcherTimer timer = new DispatcherTimer();

        int points;
        int penaltyPoints;

        List<StackPanel> list = new List<StackPanel>();

        string codeString = "";


        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mastermindWindow.Top = 40;
            mastermindWindow.Left = 40;

            timer.Interval = new TimeSpan(0, 0, 10); // Voorlopig op 15 seconden gezet
            timer.Tick += Timer_Tick;

            debugStackPanel.Visibility = Visibility.Hidden;
            debugStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            foreach (var item in seriesStackPanel.Children)
            {
                if (item is StackPanel stack)
                {
                    list.Add(stack);
                }
            }

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
                        lbl.BorderThickness = new Thickness(8);

                        lbl.MouseDown += Color_MouseDown;

                        Grid.SetRow(lbl, i);
                        Grid.SetColumn(lbl, j);

                        switch (j)
                        {
                            case 1:
                                lbl.Background = Brushes.Red;
                                lbl.BorderBrush = lbl.Background;
                                chosenColor = lbl.Background;
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

                        colorsStackPanel.Children.Add(lbl);
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
                        lbl.ToolTip = lbl.Name;
                        lbl.Background = Brushes.DarkGray;
                        lbl.BorderThickness = new Thickness(8);
                        lbl.BorderBrush = Brushes.Transparent;

                        if (j == 1)
                        {
                            lbl.FontSize = 28;
                            lbl.Content = (i - 2).ToString();
                            lbl.Width = 60;
                            lbl.Height = 60;
                            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                            lbl.VerticalContentAlignment = VerticalAlignment.Center;
                            lbl.Background = Brushes.White;

                            lbl.MouseDown += Series_MouseDown;
                            lbl.MouseDoubleClick += Series_MouseDoubleClick;
                        }
                        else
                        {
                            lbl.Margin = new Thickness(8);

                            lbl.MouseDown += Label_MouseDown;
                            lbl.MouseDoubleClick += Label_MouseDoubleClick;
                        }

                        Grid.SetRow(lbl, i);
                        Grid.SetColumn(lbl, j);

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


        private void Timer_Tick(object sender, EventArgs e)
        {
            attempts++;

            points -= 8;
            scoreLabel.Content = $"Poging {attempts}/10 Score = {points}";

            if (attempts < 10)
            {
                // Als attempst aangepast wordt, moet ook de StackPanels aangepast worden
                // Hier en in timer_Tick
                makeStackPanelVisible();
            }
            else
            {
                // Spel einde na 10 beurten

                debugStackPanel.Visibility = Visibility.Visible;

                MessageBoxResult result = MessageBox.Show($"You failed! De correcte code was {codeString}.\nNog eens proberen?", "FAILED", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    newGameButton_Click(null, null);
                }
                else
                {
                    this.Close();
                }
            }

            makeStackPanelVisible();

            // eerst kleuren overzetten
            Series_MouseDoubleClick(null, null);

            // Dan Resetten
            ResetLabels();

            StopCountdown();
        }

        private void ResetLabels()
        {
            // Kleuren verloren beurt Gray maken

            int counter = 0;

            for (int i = list.Count - 1; i >= 1; i--)
            {
                if (list[i].Visibility == Visibility.Visible)
                {
                    counter = 0;
                    foreach (var item in list[i - 1].Children)
                    {
                        if (item is Label lbl)
                        {
                            if (counter > 0)
                            {
                                lbl.Background = Brushes.DarkGray;
                                lbl.BorderBrush = Brushes.Transparent;
                            }
                        }
                        counter++;
                    }
                    return;
                }
            }
        }

        private void StopCountdown()
        {
            /// <summary>
            /// The timer = stoped and
            /// attemps + 1
            /// </summary>

            timer.Stop();
        }

        private void StartCountdown()
        {
            /// <summary>
            /// The timer = started
            /// from in generateButton_Click or
            /// from in controlButton_Click
            /// </summary>

            timer.Start();
        }


        private void Label_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (!gameStarted)
            {
                MessageBoxResult result = MessageBox.Show("Wil je een spel starten?", "Spel starten", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes); ;
                if (result == MessageBoxResult.Yes)
                {
                    newGameButton_Click(null, null);
                }
                else
                {
                    return;
                }
            }

            if (sender is Label lbl)
            {
                int counter;
                int counter2;
                string temp = lbl.Name;
                temp = temp.Substring(temp.Length - 1, 1);
                counter = Convert.ToInt32(temp);

                // ToDo

                for (int i = list.Count - 1; i >= 1; i--)
                {
                    if (list[i].Visibility == Visibility.Visible)
                    {
                        counter2 = 0;
                        foreach (var item2 in list[i - 1].Children)
                        {
                            if (item2 is Label lbl2 && counter2 > 0)
                            {
                                if (counter2 == counter)
                                {
                                    if (lbl2.Background != Brushes.DarkGray)
                                    {
                                        lbl.Background = lbl2.Background;
                                        chosenColor = lbl2.Background;
                                        GetColorCode(lbl.Name);
                                    }
                                }
                            }
                            counter2++;
                        }
                        return;
                    }
                }
            }
        }

        private void Series_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (!gameStarted)
            {
                MessageBoxResult result = MessageBox.Show("Wil je een spel starten?", "Spel starten", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes); ;
                if (result == MessageBoxResult.Yes)
                {
                    newGameButton_Click(null, null);
                }
                else
                {
                    return;
                }
            }

            if (serie2StackPanel.Visibility == Visibility.Hidden)
            {
                return;
            }

            int counter;
            int counter2;

            for (int i = list.Count - 1; i >= 1; i--)
            {
                if (list[i].Visibility == Visibility.Visible)
                {
                    counter = 0;
                    counter2 = 0;

                    foreach (var item in list[i].Children)
                    {
                        if (item is Label lbl && counter > 0)
                        {
                            counter2 = 0;
                            foreach (var item2 in list[i - 1].Children)
                            {
                                if (item2 is Label lbl2 && counter2 > 0)
                                {
                                    if (counter2 == counter)
                                    {
                                        lbl.Background = lbl2.Background;
                                        chosenColor = lbl2.Background;
                                        GetColorCode(lbl.Name);
                                    }
                                }
                                counter2++;
                            }
                        }
                        counter++;
                    }
                    return;
                }
            }
        }

        private void Series_MouseDown(object sender, RoutedEventArgs e)
        {
            if (!gameStarted)
            {
                MessageBoxResult result = MessageBox.Show("Wil je een spel starten?", "Spel starten", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes); ;
                if (result == MessageBoxResult.Yes)
                {
                    newGameButton_Click(null, null);
                }
                else
                {
                    return;
                }
            }

            if (chosenColor == Brushes.Transparent)
            {
                // Standaard begin als er nog geen kleur gekozen is

                chosenColor = Brushes.Red;
                chosenColor_Changed();
            }

            int counter = 0;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Visibility == Visibility.Visible)
                {
                    counter = 0;
                    foreach (var item in list[i].Children)
                    {
                        if (item is Label lbl && counter > 0)
                        {
                            if (lbl.Background == Brushes.DarkGray)
                            {
                                lbl.Background = chosenColor;
                                GetColorCode(lbl.Name);
                            }
                        }
                        counter++;
                    }
                    return;
                }
            }
        }

        private void Color_MouseDown(object sender, RoutedEventArgs e)
        {
            if (sender is Label lbl)
            {
                string temp = lbl.Name;
                temp = temp.Substring(temp.Length - 1, 1);
                foreach (var item in colorsStackPanel.Children)
                {
                    if (item is Label lbl2)
                    {
                        if (lbl2.Name.Contains(temp))
                        {
                            lbl2.BorderBrush = lbl.Background;
                        }
                        else
                        {
                            lbl2.BorderBrush = Brushes.Transparent;
                        }
                    }
                }
                chosenColor = lbl.Background;
                chosenColor_Changed();
            }
        }

        private void Label_MouseDown(object sender, RoutedEventArgs e)
        {
            if (!gameStarted)
            {
                MessageBoxResult result = MessageBox.Show("Wil je een spel starten?", "Spel starten", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes); ;
                if (result == MessageBoxResult.Yes)
                {
                    newGameButton_Click(null, null);
                }
                else
                {
                    return;
                }
            }
            if (chosenColor == Brushes.Transparent)
            {
                chosenColor = Brushes.Red;
                chosenColor_Changed();
            }

            if (sender is Label lbl)
            {
                if (lbl.Name.Contains("serie"))
                {
                    string temp = lbl.Name;
                    temp = temp.Substring(temp.LastIndexOf("e") + 1);
                    int intTemp = Convert.ToInt32(temp);
                    intTemp = intTemp / 10;

                    if (attempts == intTemp - 1)
                    {
                        lbl.Background = chosenColor;
                        GetColorCode(lbl.Name);
                    }
                    else
                    {
                        if(lbl.Background != Brushes.DarkGray)
                        {
                            chosenColor = lbl.Background;
                            chosenColor_Changed();
                        }
                    }
                }
            }
        }

        private void chosenColor_Changed()
        {
            foreach (var item in colorsStackPanel.Children)
            {
                if (item is Label lbl)
                {
                    if (chosenColor == lbl.Background)
                    {
                        lbl.BorderBrush = lbl.Background;
                    }
                    else
                    {
                        lbl.BorderBrush = Brushes.Transparent;
                    }
                }
            }

            controlButton.IsDefault = true;
        }

        private void GetColorCode(string lblName)
        {
            char lastLetter = lblName.Last();
            switch (lastLetter)
            {
                case '1':
                    if (chosenColor == Brushes.Red)
                    {
                        chosenColorCode1 = 1;
                    }
                    else if (chosenColor == Brushes.Yellow)
                    {
                        chosenColorCode1 = 2;
                    }
                    else if (chosenColor == Brushes.Orange)
                    {
                        chosenColorCode1 = 3;
                    }
                    else if (chosenColor == Brushes.White)
                    {
                        chosenColorCode1 = 4;
                    }
                    else if (chosenColor == Brushes.Green)
                    {
                        chosenColorCode1 = 5;
                    }
                    else if (chosenColor == Brushes.Blue)
                    {
                        chosenColorCode1 = 6;
                    }
                    break;
                case '2':
                    if (chosenColor == Brushes.Red)
                    {
                        chosenColorCode2 = 1;
                    }
                    else if (chosenColor == Brushes.Yellow)
                    {
                        chosenColorCode2 = 2;
                    }
                    else if (chosenColor == Brushes.Orange)
                    {
                        chosenColorCode2 = 3;
                    }
                    else if (chosenColor == Brushes.White)
                    {
                        chosenColorCode2 = 4;
                    }
                    else if (chosenColor == Brushes.Green)
                    {
                        chosenColorCode2 = 5;
                    }
                    else if (chosenColor == Brushes.Blue)
                    {
                        chosenColorCode2 = 6;
                    }
                    break;
                case '3':
                    if (chosenColor == Brushes.Red)
                    {
                        chosenColorCode3 = 1;
                    }
                    else if (chosenColor == Brushes.Yellow)
                    {
                        chosenColorCode3 = 2;
                    }
                    else if (chosenColor == Brushes.Orange)
                    {
                        chosenColorCode3 = 3;
                    }
                    else if (chosenColor == Brushes.White)
                    {
                        chosenColorCode3 = 4;
                    }
                    else if (chosenColor == Brushes.Green)
                    {
                        chosenColorCode3 = 5;
                    }
                    else if (chosenColor == Brushes.Blue)
                    {
                        chosenColorCode3 = 6;
                    }
                    break;
                case '4':
                    if (chosenColor == Brushes.Red)
                    {
                        chosenColorCode4 = 1;
                    }
                    else if (chosenColor == Brushes.Yellow)
                    {
                        chosenColorCode4 = 2;
                    }
                    else if (chosenColor == Brushes.Orange)
                    {
                        chosenColorCode4 = 3;
                    }
                    else if (chosenColor == Brushes.White)
                    {
                        chosenColorCode4 = 4;
                    }
                    else if (chosenColor == Brushes.Green)
                    {
                        chosenColorCode4 = 5;
                    }
                    else if (chosenColor == Brushes.Blue)
                    {
                        chosenColorCode4 = 6;
                    }
                    break;
            }

            chosenColor_Changed();
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            controlButton.IsDefault = true;

            if (!dissolved && gameStarted)
            {
                MessageBoxResult result = MessageBox.Show("Wilt u het spel vroegtijdig beëindigen?", $"Poging {attempts + 1}/10", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }

            gameStarted = true;
            dissolved = false;

            chosenColor = Brushes.Red;
            chosenColor_Changed();

            // Code genereren

            attempts = 0;
            points = 100;
            penaltyPoints = 0;
            scoreLabel.Content = "";

            StartCountdown();

            debugStackPanel.Visibility = Visibility.Hidden;

            Random rnd = new Random();

            colorCode1 = rnd.Next(0, 24);
            colorCode2 = rnd.Next(0, 24);
            colorCode3 = rnd.Next(0, 24);
            colorCode4 = rnd.Next(0, 24);

            chosenColorCode1 = 0;
            chosenColorCode2 = 0;
            chosenColorCode3 = 0;
            chosenColorCode4 = 0;

            //chosenColor = Brushes.Transparent;

            switch (colorCode1 % 6)
            {
                case 0:
                    debugLabel1.Background = Brushes.Red;
                    colorCode1 = 1;
                    codeString = "Red, ";
                    break;
                case 1:
                    debugLabel1.Background = Brushes.Yellow;
                    colorCode1 = 2;
                    codeString = "Yellow, ";
                    break;
                case 2:
                    debugLabel1.Background = Brushes.Orange;
                    colorCode1 = 3;
                    codeString = "Orange, ";
                    break;
                case 3:
                    debugLabel1.Background = Brushes.White;
                    colorCode1 = 4;
                    codeString = "White, ";
                    break;
                case 4:
                    debugLabel1.Background = Brushes.Green;
                    colorCode1 = 5;
                    codeString = "Green, ";
                    break;
                case 5:
                    debugLabel1.Background = Brushes.Blue;
                    colorCode1 = 6;
                    codeString = "Blue, ";
                    break;
            }

            switch (colorCode2 % 6)
            {
                case 0:
                    debugLabel2.Background = Brushes.Red;
                    colorCode2 = 1;
                    codeString += "Red, ";
                    break;
                case 1:
                    debugLabel2.Background = Brushes.Yellow;
                    colorCode2 = 2;
                    codeString += "Yellow, ";
                    break;
                case 2:
                    debugLabel2.Background = Brushes.Orange;
                    colorCode2 = 3;
                    codeString += "Orange, ";
                    break;
                case 3:
                    debugLabel2.Background = Brushes.White;
                    colorCode2 = 4;
                    codeString += "White, ";
                    break;
                case 4:
                    debugLabel2.Background = Brushes.Green;
                    colorCode2 = 5;
                    codeString += "Green, ";
                    break;
                case 5:
                    debugLabel2.Background = Brushes.Blue;
                    colorCode2 = 6;
                    codeString += "Blue, ";
                    break;
            }

            switch (colorCode3 % 6)
            {
                case 0:
                    debugLabel3.Background = Brushes.Red;
                    colorCode3 = 1;
                    codeString += "Red, ";
                    break;
                case 1:
                    debugLabel3.Background = Brushes.Yellow;
                    colorCode3 = 2;
                    codeString += "Yellow, ";
                    break;
                case 2:
                    debugLabel3.Background = Brushes.Orange;
                    colorCode3 = 3;
                    codeString += "Orange, ";
                    break;
                case 3:
                    debugLabel3.Background = Brushes.White;
                    colorCode3 = 4;
                    codeString += "White, ";
                    break;
                case 4:
                    debugLabel3.Background = Brushes.Green;
                    colorCode3 = 5;
                    codeString += "Green, ";
                    break;
                case 5:
                    debugLabel3.Background = Brushes.Blue;
                    colorCode3 = 6;
                    codeString += "Blue, ";
                    break;
            }

            switch (colorCode4 % 6)
            {
                case 0:
                    debugLabel4.Background = Brushes.Red;
                    colorCode4 = 1;
                    codeString += "Red";
                    break;
                case 1:
                    debugLabel4.Background = Brushes.Yellow;
                    colorCode4 = 2;
                    codeString += "Yellow";
                    break;
                case 2:
                    debugLabel4.Background = Brushes.Orange;
                    colorCode4 = 3;
                    codeString += "Orange";
                    break;
                case 3:
                    debugLabel4.Background = Brushes.White;
                    colorCode4 = 4;
                    codeString += "White";
                    break;
                case 4:
                    debugLabel4.Background = Brushes.Green;
                    colorCode4 = 5;
                    codeString += "Green";
                    break;
                case 5:
                    debugLabel4.Background = Brushes.Blue;
                    colorCode4 = 6;
                    codeString += "Blue";
                    break;
            }

            int counter;

            foreach (var item in seriesStackPanel.Children)
            {
                if (item is StackPanel stack)
                {
                    counter = 0;

                    if (stack.Visibility == Visibility.Visible)
                    {
                        foreach (var item2 in stack.Children)
                        {
                            if (counter > 0)
                            {
                                if (item2 is Label lbl)
                                {
                                    lbl.Background = Brushes.DarkGray;
                                    lbl.BorderBrush = Brushes.Transparent;
                                }
                            }
                            counter++;
                        }
                    }
                }
            }

            controlButton.IsEnabled = true;

            // StackPanels Visible maken

            serie1StackPanel.Visibility = Visibility.Visible;
            serie2StackPanel.Visibility = Visibility.Hidden;
            serie3StackPanel.Visibility = Visibility.Hidden;
            serie4StackPanel.Visibility = Visibility.Hidden;
            serie5StackPanel.Visibility = Visibility.Hidden;
            serie6StackPanel.Visibility = Visibility.Hidden;
            serie7StackPanel.Visibility = Visibility.Hidden;
            serie8StackPanel.Visibility = Visibility.Hidden;
            serie9StackPanel.Visibility = Visibility.Hidden;
            serie10StackPanel.Visibility = Visibility.Hidden;
        }

        private void controlButton_Click(object sender, RoutedEventArgs e)
        {
            // Resetten booleans

            bool colorPosition1 = false;
            bool colorPosition2 = false;
            bool colorPosition3 = false;
            bool colorPosition4 = false;

            bool color1 = false;
            bool color2 = false;
            bool color3 = false;
            bool color4 = false;

            penaltyPoints = 0;

            //Controle of elke positie een kleur heeft

            if (chosenColorCode1 > 0 &&
                chosenColorCode2 > 0 &&
                chosenColorCode3 > 0 &&
                chosenColorCode4 > 0)
            {
                StopCountdown();

                //Controleren juiste kleur op juiste plaats

                if (colorCode1 == chosenColorCode1)
                {
                    //lbl.BorderBrush = Brushes.DarkRed;
                    colorPosition1 = true;
                    color1 = true;
                }

                if (colorCode2 == chosenColorCode2)
                {
                    //lbl.BorderBrush = Brushes.DarkRed;
                    colorPosition2 = true;
                    color2 = true;
                }

                if (colorCode3 == chosenColorCode3)
                {
                    //lbl.BorderBrush = Brushes.DarkRed;
                    colorPosition3 = true;
                    color3 = true;
                }

                if (colorCode4 == chosenColorCode4)
                {
                    //lbl.BorderBrush = Brushes.DarkRed;
                    colorPosition4 = true;
                    color4 = true;
                }

                if (colorPosition1 == true)
                {
                    ChangeBorder(attempts, 1, 1);
                }
                if (colorPosition2 == true)
                {
                    ChangeBorder(attempts, 2, 1);
                }
                if (colorPosition3 == true)
                {
                    ChangeBorder(attempts, 3, 1);
                }
                if (colorPosition4 == true)
                {
                    ChangeBorder(attempts, 4, 1);
                }

                if (colorPosition1 && colorPosition2 && colorPosition3 && colorPosition4)
                {
                    // Gewonnen spel

                    attempts++;
                    scoreLabel.Content = $"Poging {attempts}/10 Score = {points}";
                    debugStackPanel.Visibility = Visibility.Visible;
                    gameStarted = false;

                    MessageBoxResult result = MessageBox.Show($"Code is gekraakt in {attempts} pogingen. Wil je nog eens?", "WINNER", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        dissolved = true;
                        newGameButton_Click(null, null);
                    }
                    else
                    {
                        this.Close();
                    }
                    return;
                }
                else
                {
                    // Checken of de kleur ergens anders voorkomt

                    if (colorPosition1 == false)
                    {
                        if (chosenColorCode1 == colorCode2 && color2 == false)
                        {
                            ChangeBorder(attempts, 1);
                            color2 = true;
                        }
                        else if (chosenColorCode1 == colorCode3 && color3 == false)
                        {
                            ChangeBorder(attempts, 1);
                            color3 = true;
                        }
                        else if (chosenColorCode1 == colorCode4 && color4 == false)
                        {
                            ChangeBorder(attempts, 1);
                            color4 = true;
                        }
                    }

                    if (colorPosition2 == false)
                    {
                        if (chosenColorCode2 == colorCode1 && color1 == false)
                        {
                            ChangeBorder(attempts, 2);
                            color1 = true;
                        }
                        else if (chosenColorCode2 == colorCode3 && color3 == false)
                        {
                            ChangeBorder(attempts, 2);
                            color3 = true;
                        }
                        else if (chosenColorCode2 == colorCode4 && color4 == false)
                        {
                            ChangeBorder(attempts, 2);
                            color4 = true;
                        }
                    }

                    if (colorPosition3 == false)
                    {
                        if (chosenColorCode3 == colorCode1 && color1 == false)
                        {
                            ChangeBorder(attempts, 3);
                            color1 = true;
                        }
                        else if (chosenColorCode3 == colorCode2 && color2 == false)
                        {
                            ChangeBorder(attempts, 3);
                            color2 = true;
                        }
                        else if (chosenColorCode3 == colorCode4 && color4 == false)
                        {
                            ChangeBorder(attempts, 3);
                            color4 = true;
                        }
                    }

                    if (colorPosition4 == false)
                    {
                        if (chosenColorCode4 == colorCode1 && color1 == false)
                        {
                            ChangeBorder(attempts, 4);
                            color1 = true;
                        }
                        else if (chosenColorCode4 == colorCode2 && color2 == false)
                        {
                            ChangeBorder(attempts, 4);
                            color2 = true;
                        }
                        else if (chosenColorCode4 == colorCode3 && color3 == false)
                        {
                            ChangeBorder(attempts, 4);
                            color3 = true;
                        }
                    }

                    if (color1 && !colorPosition1)
                    {
                        penaltyPoints++;
                    }

                    if (color2 && !colorPosition2)
                    {
                        penaltyPoints++;
                    }

                    if (color3 && !colorPosition3)
                    {
                        penaltyPoints++;
                    }

                    if (color4 && !colorPosition4)
                    {
                        penaltyPoints++;
                    }

                    if (!color1)
                    {
                        penaltyPoints += 2;
                    }

                    if (!color2)
                    {
                        penaltyPoints += 2;
                    }

                    if (!color3)
                    {
                        penaltyPoints += 2;
                    }

                    if (!color4)
                    {
                        penaltyPoints += 2;
                    }

                    points -= penaltyPoints;

                    attempts++;

                    scoreLabel.Content = $"Poging {attempts}/10 Score = {points}";

                    if (attempts < 10)
                    {
                        // Als attempst aangepast wordt, moet ook de StackPanels aangepast worden
                        // Hier en in timer_Tick
                        makeStackPanelVisible();
                    }
                    else
                    {
                        // Spel einde na 10 beurten

                        gameStarted = false;

                        debugStackPanel.Visibility = Visibility.Visible;

                        MessageBoxResult result = MessageBox.Show($"You failed! De correcte code was {codeString}.\nNog eens proberen?", "FAILED", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            newGameButton_Click(null, null);
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Maak een keuze voor alle vakken", "Keuze");
            }
        }

        private void ChangeBorder(int attempst, int code, int colorPositie = 0)
        {
            int counter = 1;

            foreach (var item in seriesStackPanel.Children)
            {
                if (item is StackPanel stack)
                {
                    if (counter == attempst + 1)
                    {
                        int counter2 = 0;
                        foreach (var item2 in stack.Children)
                        {
                            if (counter2 == code)
                            {
                                if (item2 is Label lbl)
                                {
                                    if (colorPositie == 1)
                                    {
                                        lbl.BorderBrush = Brushes.DarkRed;
                                    }
                                    else
                                    {
                                        lbl.BorderBrush = Brushes.Wheat;
                                    }
                                    return;
                                }

                            }
                            counter2++;
                        }
                    }
                    counter++;
                }
            }
        }

        private void makeStackPanelVisible()
        {
            chosenColorCode1 = 0;
            chosenColorCode2 = 0;
            chosenColorCode3 = 0;
            chosenColorCode4 = 0;

            switch (attempts)
            {
                case 1:
                    serie2StackPanel.Visibility = Visibility.Visible;
                    break;
                case 2:
                    serie3StackPanel.Visibility = Visibility.Visible;
                    break;
                case 3:
                    serie4StackPanel.Visibility = Visibility.Visible;
                    break;
                case 4:
                    serie5StackPanel.Visibility = Visibility.Visible;
                    break;
                case 5:
                    serie6StackPanel.Visibility = Visibility.Visible;
                    break;
                case 6:
                    serie7StackPanel.Visibility = Visibility.Visible;
                    break;
                case 7:
                    serie8StackPanel.Visibility = Visibility.Visible;
                    break;
                case 8:
                    serie9StackPanel.Visibility = Visibility.Visible;
                    break;
                case 9:
                    serie10StackPanel.Visibility = Visibility.Visible;
                    break;
            }

            if (debugStackPanel.Visibility == Visibility.Hidden)
            {
                StartCountdown();
            }
        }

        private void mastermindWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12 && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                ToggleDebug();
            }
            else if (e.Key == Key.Enter)
            {
                controlButton_Click(null, null);
            }
        }

        ///<summary>
        ///Make  visible or hidden
        /// </summary>
        private void ToggleDebug()
        {
            
            if (debugStackPanel.Visibility == Visibility.Visible)
            {
                debugStackPanel.Visibility = Visibility.Hidden;
                StartCountdown();
            }
            else
            {
                debugStackPanel.Visibility = Visibility.Visible;
                StopCountdown();
            }
        }

    }

}