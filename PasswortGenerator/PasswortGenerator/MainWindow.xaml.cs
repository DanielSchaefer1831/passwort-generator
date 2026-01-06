using System.CodeDom.Compiler;
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

namespace PasswortGenerator
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


        // Methode, um den Textinhalt zu kopieren:
        private void Copy_Button(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TxtPassword.Text);
        }


        // Methode, um das Passwort zu ändern / aktualisieren
        private void Refresh_Button(object sender, RoutedEventArgs e)
        {
            Random updatePassword = new Random();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";

            int length = (int)SldLength.Value;

            string passwordStart = "";

            for(int i = 0; i < length; i++) // Die Schleife wird so oft wiederholt, wie die Zahl auf dem Balken angibt.
            {
                int index = updatePassword.Next(0, characters.Length);
                passwordStart += characters[index]; // Der Rest wird drangehängt.
            }

            TxtPassword.Text = passwordStart;

            UpdateStrengthStatus(); // Ist die Methode in Zeile 55, die dann ausgeführt werden soll!
        }


        // Reagiert auf Bewegungen des Sliders und aktualisiert die Stärke-Anzeige:
        private void SldLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateStrengthStatus();
        }


        // Prüft die Passwortlänge und passt Text und Farbe der Statusanzeige und Balken an:
        private void UpdateStrengthStatus()
        {
            if (TxtStatus == null || SldLength == null || StrengthBar == null) return;

            int length = (int)SldLength.Value;

            if (length < 8)
            {
                TxtStatus.Text = "Status: Schwach";
                TxtStatus.Foreground = Brushes.Red;
                StrengthBar.Value = 25;
                StrengthBar.Foreground = Brushes.Red;
            }
            else if (length <= 11)
            {
                TxtStatus.Text = "Status: Mittel";
                TxtStatus.Foreground = Brushes.Orange;
                StrengthBar.Value = 50;
                StrengthBar.Foreground = Brushes.Orange;
            }
            else if (length <= 15)
            {
                TxtStatus.Text = "Status: Stark";
                TxtStatus.Foreground = Brushes.Green;
                StrengthBar.Value = 75;
                StrengthBar.Foreground = Brushes.Green;
            }
            else // Ab 16 Zeichen:
            {
                TxtStatus.Text = "Status: Sicher";
                TxtStatus.Foreground = Brushes.DarkGreen;
                StrengthBar.Value = 100;
                StrengthBar.Foreground = Brushes.DarkGreen;
            }
            
        }


        // Passwortlogik nach einer Änderung des Checkbox-Filters:
        private void GeneratePassword()
        {
            string charSet = "";

            //  Prüfen, welche Checkboxen einen Haken haben:
            if (CbUpper.IsChecked == true)
            {
                charSet += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            if (CbLower.IsChecked == true)
            {
                charSet += "abcdefghijklmnopqrstuvwxyz";
            }

            if (CbNumbers.IsChecked == true)
            {
                charSet += "0123456789";
            }

            if (CbSpecial.IsChecked == true)
            {
                charSet += "!@#$%^&*()-_=+<>?";
            }

            // Sicherheits-Check: Falls gar kein Haken gesetzt wurde:
            if (string.IsNullOrEmpty(charSet))
            {
                TxtPassword.Text = "Keine Checkbox ausgewählt.";
                return;
            }

            // Zufalls-Generator erstellen:
            Random random = new Random();
            int length = (int)SldLength.Value;
            char[] result = new char[length];

            // Passwort Buchstabe für Buchstabe aus dem charSet ziehen
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(charSet.Length);
                result[i] = charSet[index];
            }

            // Das fertige Passwort anzeigen
            TxtPassword.Text = new string(result);
        }


        // Generiert ein neues Passwort unter Berücksichtigung der gesetzten Filter beim Klicken auf den Update-Button:
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            GeneratePassword(); 
        }
    }
}