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

namespace TryOut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Input();
        }

        private void Input()
        {
            //int nummer = Convert.ToInt32(Interaction.InputBox("Geef een getal in: ","Getal","50",10,10));
            //window.Content = nummer.ToString();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                // Test op numeriek toetsenbord.
                resultTextBox.Text = $"Numeriek {e.Key}";
            }
            else if (e.Key == Key.K)
            {
                resultTextBox.Text = "Kleine letter k of hoofdletter K";
            }
            else if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                resultTextBox.Text = "Shift";
            }
            else if (e.Key == Key.Return)
            {
                resultTextBox.Text = "Enter";
            }
            else if (e.Key >= Key.F1 && e.Key <= Key.F12)
            {
                resultTextBox.Text = $"Functietoets {e.Key}";
            }
            else
            {
                resultTextBox.Text = $"Key {e.Key} wordt niet ondersteund.";
                e.Handled = true;
            }

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt)
            {
                resultTextBox.Text = "ALT toets";
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                resultTextBox.Text = "Control toets";
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                resultTextBox.Text = "Shift toets";
            }

            if (e.Key >= Key.D0 && e.Key <= Key.D9 && e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                resultTextBox.Text = $"Numerieke toets {e.Key}";
            }
            if (e.Key == Key.K && e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                resultTextBox.Text = "Hoofdletter K";
            }
            if (e.Key == Key.K && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                resultTextBox.Text = "kleine k";
            }

            if (e.Key >= Key.D0 && e.Key <= Key.D9 && e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                resultTextBox.Text = $"Numerieke toets met shift: {e.Key}";
            }
            if (e.Key >= Key.D0 && e.Key <= Key.D9 && e.KeyboardDevice.Modifiers == ModifierKeys.None)
            {
                resultTextBox.Text = $"Gewone aanslag: {e.Key}";
            }
            // Alt Gr is een combinatie van Alt + Crtl
            if (e.Key >= Key.D0 && e.Key <= Key.D9 && e.KeyboardDevice.Modifiers == (ModifierKeys.Alt | ModifierKeys.Control))
            {
                resultTextBox.Text = $"Aanslag met Alt Gr: {e.Key}";
            }
        }
    }
}