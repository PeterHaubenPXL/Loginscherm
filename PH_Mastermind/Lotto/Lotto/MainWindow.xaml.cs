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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lotto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Globale Variabelen
        int Reeks = 1;
        int Seed = (int)DateTime.Now.Millisecond;

        public MainWindow()
        {
            InitializeComponent();

            int Aantal = 7;

            //Textboxen genereren
            for (int i = 1; i <= 15; i++)
            {
                for (int j = 1; j <= Aantal; j++)
                {
                    //TextBox textBox = new TextBox();
                    TextBox txt = new TextBox();
                    txt.Name = $"txt{i}{j}";
                    txt.Width = 40;
                    txt.Margin = new Thickness(3);
                    txt.TextAlignment = TextAlignment.Center;

                    if (i > 1)
                    {
                        Grid.SetRow(txt, i + 1);
                    }
                    else    // i == 1
                    {
                        Grid.SetRow(txt, i);
                        if (j < 7)
                        {
                            txt.Background = Brushes.LightGreen;
                        }
                        else    // j == 7
                        {
                            txt.Background = Brushes.LightPink;
                        }

                    }
                    Grid.SetColumn(txt, j);

                    GrdMain.Children.Add(txt);
                }
                Aantal = 6;

                
            }
            //chkVasteNummers.IsChecked = true;
            //btnGenereren_Click(null, null);

        }

        private void ClearTXTs()
        {
            int i = 0;
            int j = 0;

            foreach (TextBox txt in FindVisualChildren<TextBox>(this))
            {
                txt.Text = string.Empty;
                if (i==0)
                {
                    if (j<6)
                    {
                        txt.Background = Brushes.LightGreen;
                    }
                    else // j = 6 reserve getal
                    {
                        txt.Background = Brushes.LightPink;
                        i++;
                    }
                    j++;     
                }
                else // i > 0
                {
                    txt.Background = Brushes.White;
                }

                lbl01.Content = "";
                lbl01.Background = Brushes.White;

                lbl02.Content = "";
                lbl02.Background = Brushes.White;

                lbl03.Content = "";
                lbl03.Background = Brushes.White;

                lbl04.Content = "";
                lbl04.Background = Brushes.White;

                lbl05.Content = "";
                lbl05.Background = Brushes.White;

                lbl06.Content = "";
                lbl06.Background = Brushes.White;

                lbl07.Content = "";
                lbl07.Background = Brushes.White;

                lbl08.Content = "";
                lbl08.Background = Brushes.White;

                lbl09.Content = "";
                lbl09.Background = Brushes.White;

                lbl10.Content = "";
                lbl10.Background = Brushes.White;

                lbl11.Content = "";
                lbl11.Background = Brushes.White;

                lbl12.Content = "";
                lbl12.Background = Brushes.White;

                lbl13.Content = "";
                lbl13.Background = Brushes.White;

                lbl14.Content = "";
                lbl14.Background = Brushes.White;

            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void GetrokkenNummers(int[,] ArrayGen) {
            int i = 0;
            int j = 0;

            foreach (TextBox txt in FindVisualChildren<TextBox>(this))
            {
                if (i == 0)
                {
                    if (chkGetrokkenGen.IsChecked  == false)
                    {
                        if (txt.Text != "")
                        {
                            ArrayGen[i, j] = Convert.ToInt32(txt.Text);
                        }
                        else
                        {
                            ArrayGen[i, j] = 0;
                        }
                        

                        j++;
                        if (j == 7)
                        {
                            i++;
                            j = 0;
                        }
                    }
                }
                else 
                {
                    break;                    
                }
            }
        }

        private void TextVakkenOpvullen(int[,] Lotto)
        {
            int i = 0;
            int j = 0;
            int Resultaat = 0;
            char Reserve = ' ';
            bool Toon;

            var mijnConverter = new System.Windows.Media.BrushConverter();
            var mijnBrush = (Brush)mijnConverter.ConvertFromString("#FFffbb80"); //#FFFF7800    #FFffbb80

            foreach (TextBox txt in FindVisualChildren<TextBox>(this))
            {
                if (Lotto[i, j].ToString() == "0" || Lotto[i, j] > 45)
                {
                    txt.Text = "";
                }
                else
                {
                    txt.Text = Lotto[i, j].ToString();
                }
                

                if (j != 6 && (Lotto[i, j] == Lotto[0, 0] || Lotto[i, j] == Lotto[0, 1] || Lotto[i, j] == Lotto[0, 2]
                    || Lotto[i, j] == Lotto[0, 3] || Lotto[i, j] == Lotto[0, 4] || Lotto[i, j] == Lotto[0, 5]))
                {
                    txt.Background = Brushes.LightGreen;
                    if (i != 0)
                    {
                        Resultaat++;
                    }
                }
                else if (Lotto[i, j] == Lotto[0, 6] || (Lotto[i, j] == 0 && j == 6 && i == 0))
                {
                    txt.Background = Brushes.LightPink;
                    if (i != 0)
                    {
                        Reserve = '+';
                    }
                }

                if (i == 0)
                {
                    if (j == 6)
                    {
                        ++i;
                        j = 0;
                    }
                    else
                    {
                        j++;
                    }
                }
                else if (i > 0)
                {
                    if (j == 5)
                    {
                        Toon = false;
                        if (Resultaat > 0)
                        {
                            if (Reserve == '+')
                            {
                                Toon = true;
                            }
                            if (Resultaat > 1)
                            {
                                if (Reserve == '+')
                                {
                                    Toon = true;
                                }
                                if (Resultaat > 2)
                                {
                                    Toon = true;
                                }
                            }
                        }
                        if (Toon == true)
                        {
                            switch (i)
                            {
                                case 1:
                                    lbl01.Background = Brushes.Yellow;
                                    if (Reserve == '+')
                                    {
                                        lbl01.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl01.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 2:
                                    lbl02.Background = Brushes.Yellow;
                                    if (Reserve == '+')
                                    {
                                        lbl02.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl02.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 3:
                                    lbl03.Background = Brushes.Yellow;
                                    if (Reserve == '+')
                                    {
                                        lbl03.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl03.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 4:
                                    lbl04.Background = Brushes.Yellow;
                                    if (Reserve == '+')
                                    {
                                        lbl04.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl04.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 5:
                                    lbl05.Background = Brushes.Yellow;
                                    if (Reserve == '+')
                                    {
                                        lbl05.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl05.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 6:
                                    lbl06.Background = Brushes.Yellow;
                                    if (Reserve == '+')
                                    {
                                        lbl06.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl06.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 7:
                                    lbl07.Background = Brushes.Yellow;
                                    if (Reserve == '+')
                                    {
                                        lbl07.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl07.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 8:
                                    lbl08.Background = Brushes.Yellow;
                                    if (Reserve == '+')
                                    {
                                        lbl08.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl08.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 9:
                                    lbl09.Background = mijnBrush ;
                                    if (Reserve == '+')
                                    {
                                        lbl09.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl09.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 10:
                                    lbl10.Background = mijnBrush;
                                    if (Reserve == '+')
                                    {
                                        lbl10.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl10.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 11:
                                    
                                    lbl11.Background = Brushes.Coral;
                                    if (Reserve == '+')
                                    {
                                        lbl11.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl11.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 12:
                                    lbl12.Background = Brushes.Coral;
                                    if (Reserve == '+')
                                    {
                                        lbl12.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl12.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 13:
                                    lbl13.Background = Brushes.Coral;
                                    if (Reserve == '+')
                                    {
                                        lbl13.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl13.Content = Resultaat.ToString();
                                    }
                                    break;
                                case 14:
                                    lbl14.Background = Brushes.Coral;
                                    if (Reserve == '+')
                                    {
                                        lbl14.Content = Resultaat.ToString() + Reserve;
                                    }
                                    else
                                    {
                                        lbl14.Content = Resultaat.ToString();
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else    // Toon == false
                        {
                            switch (i)
                            {
                                case 1:
                                    lbl01.Content = "";
                                    lbl01.Background = Brushes.White;
                                    break;
                                case 2:
                                    lbl02.Content = "";
                                    lbl02.Background = Brushes.White;
                                    break;
                                case 3:
                                    lbl03.Content = "";
                                    lbl03.Background = Brushes.White;
                                    break;
                                case 4:
                                    lbl04.Content = "";
                                    lbl04.Background = Brushes.White;
                                    break;
                                case 5:
                                    lbl05.Content = "";
                                    lbl05.Background = Brushes.White;
                                    break;
                                case 6:
                                    lbl06.Content = "";
                                    lbl06.Background = Brushes.White;
                                    break;
                                case 7:
                                    lbl07.Content = "";
                                    lbl07.Background = Brushes.White;
                                    break;
                                case 8:
                                    lbl08.Content = "";
                                    lbl08.Background = Brushes.White;
                                    break;
                                case 9:
                                    lbl09.Content = "";
                                    lbl09.Background = Brushes.White;
                                    break;
                                case 10:
                                    lbl10.Content = "";
                                    lbl10.Background = Brushes.White;
                                    break;
                                case 11:
                                    lbl11.Content = "";
                                    lbl11.Background = Brushes.White;
                                    break;
                                case 12:
                                    lbl12.Content = "";
                                    lbl12.Background = Brushes.White;
                                    break;
                                case 13:
                                    lbl13.Content = "";
                                    lbl13.Background = Brushes.White;
                                    break;
                                case 14:
                                    lbl14.Content = "";
                                    lbl14.Background = Brushes.White;
                                    break;
                                default:
                                    break;
                            }
                        }

                        i++;
                        j = 0;
                        Resultaat = 0;
                        Reserve = ' ';
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            if (i==15 && (Lotto[0, 0] == 0 || Lotto[0, 1] == 0 || Lotto[0, 2] == 0 || Lotto[0, 3] == 0 || Lotto[0, 4] == 0 || Lotto[0, 5] == 0 || Lotto[0, 6] == 0 ||
                    Lotto[0, 0] > 45 || Lotto[0, 1] > 45 || Lotto[0, 2] > 45 || Lotto[0, 3] > 45 || Lotto[0, 4] > 45 || Lotto[0, 5] > 45 || Lotto[0, 6] > 45))
            {
                MessageBox.Show("Vul alle getrokken nummers in,\nin de gekleurde vakken\n\nof \n\nVink Getrokken Genereren aan\n\nKlik dan op Controleren", "Getrokken nummers");
            }

        }


        private void Bubblesort(int[] Array)
        {
            int Temp;
            int Teller = 5; //Het reserve getal wordt niet in de bubblesort gecontrolleerd

            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < Teller; i++)
                {
                    if (Array[i] > Array[i + 1])
                    {
                        Temp = Array[i];
                        Array[i] = Array[i + 1];
                        Array[i + 1] = Temp;
                    }
                }
                Teller--;
            }
        }

        private void ArraysVullen(int[] Array)
        {

            int Aantal;
            int Gegenereerd;

            if (Reeks == 1)
            {
                Aantal = 7;
            }
            else
            {
                Aantal = 6;
            }

            for (int i = 0; i < Aantal; i++)
            {
                Random rnd = new Random(Seed);

                Gegenereerd = rnd.Next(1, 46);

                if (Array.Contains(Gegenereerd) == true)
                {
                    i--;    // Dubbel Gegenereerd Getal => Teller wordt terug gezet
                }
                else
                {
                    Array[i] = Gegenereerd;
                }
                Seed = rnd.Next(0, 2147483647);
            }
        }

        private void btnGenereren_Click(object sender, RoutedEventArgs e)
        {
            int[] Cijfer = new int[7];
            
            int Aantal;
            Reeks = 1;


            if(chkVasteNummers.IsChecked == false)
            {
                ClearTXTs();

                int[,] ArrayGen = new int[15, 7];

                for (int i = 0; i < 15; i++)
                {
                    if (i == 0)
                    {
                        Aantal = 7;
                    }
                    else    // i != 0
                    {
                        Aantal = 6;
                        Cijfer[6] = 0; /*Reserve Getal wordt op 0 gezet anders blijft
                                     dat ingevuld in de waarde van de eerste reeks
                                     en dan wordt het reserve getal gezien als 
                                     dubbel gegenereerd, en wordt dus niet aanvaardt*/
                    }

                    ArraysVullen(Cijfer);
                    Bubblesort(Cijfer);

                    Reeks++;

                    for (int j = 0; j < Aantal; j++)
                    {
                        ArrayGen[i, j] = Cijfer[j];
                    }
                    TextVakkenOpvullen(ArrayGen);
                }
            }
            else // chkVasteNummers.IsChecked == true
            {
                int[,] ArrayGen = { { 0,0,0,0,0,0,0 },{ 1,6,12,17,26,37,0 },{ 10,16,17,19,23,43,0},{ 7,8,14,17,35,41,0} ,{ 5,8,18,35,40,41,0} ,
                            { 5,14,18,31,34,42,0},{8,10,11,13,27,28,0 },{10,23,34,37,40,43,0 },{ 23,25,29,32,38,40,0},{ 1,5,7,10,11,21,0},
                            { 2,8,12,29,30,31,0},{3,6,9,12,15,18,0 },{ 7,13,21,33,35,41,0},{6,10,11,17,27,28,0 },{6,12,23,33,41,45,0 }};

                GetrokkenNummers(ArrayGen);

                if (ArrayGen[0,6]!=0)
                {
                    ClearTXTs();
                    TextVakkenOpvullen(ArrayGen);
                }
                else    // ArrayGen[0,6] == 0
                {
                    if (chkGetrokkenGen.IsChecked==true)
                    {
                        Reeks = 1;
                        
                        ArraysVullen(Cijfer);
                        Bubblesort(Cijfer);

                        for (int j = 0; j < 7; j++)
                        {
                            ArrayGen[0, j] = Cijfer[j];
                        }
                        
                        ClearTXTs();
                        TextVakkenOpvullen(ArrayGen);
                    }
                    else
                    {
                        ClearTXTs();
                        TextVakkenOpvullen(ArrayGen);
                    }
                }
            }
        }

        private void chkVasteNummers_Click(object sender, RoutedEventArgs e)
        {
            if (chkVasteNummers.IsChecked == true)
            {
                btnGenereren.Content = "Controleren";
                chkGetrokkenGen.Visibility = Visibility.Visible;
                
                ClearTXTs();

                btnGenereren_Click(sender,e);
            }
            else
            {
                btnGenereren.Content = "Genereren";
            }
        }

        private void lblHidden_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (chkVasteNummers.Visibility == Visibility.Hidden)
            {
                chkVasteNummers.Visibility = Visibility.Visible;    
            }
            else
            {
                chkVasteNummers.Visibility = Visibility.Hidden;
                chkVasteNummers.IsChecked = false;

                chkGetrokkenGen.Visibility = Visibility.Hidden;
                chkGetrokkenGen.IsChecked = false;

                btnGenereren.Content = "Generereren";
            }

        }

    }
}