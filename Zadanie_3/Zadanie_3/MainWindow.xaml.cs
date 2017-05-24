using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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

namespace Zadanie_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Random random = new Random();
        ObservableCollection<KeyValuePair<double, double>> Seria1 = new ObservableCollection<KeyValuePair<double, double>>();
        ObservableCollection<KeyValuePair<double, double>> Seria2 = new ObservableCollection<KeyValuePair<double, double>>();

        public string filePath { get; set; }
        public string filePathToTest { get; set; }

        public int IloscWejsc { get; set; }
        public int IloscWyjsc { get; set; }

        public Siec Siec { get; set; }
        public List<Dana> DaneTreningowe { get; set; }
        public List<Dana> DaneTestowe { get; set; }

        public int IloscEpok { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            seria1.DataContext = Seria1;
            seria2.DataContext = Seria2;
                
        }

        private void wczytajDane(int iloscWejsc, int iloscWyjsc)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                DaneTreningowe = new List<Dana>();
                foreach (var line in lines)
                {
                    var temp = line.Replace(".", ",");
                    var numbers = temp.Split(new Char[] { ';', ' ' }).Select(double.Parse).ToList();
                    if (numbers.Count == (iloscWejsc + iloscWyjsc)) ////sprawdzamy czy plik zgodny z tym co deklarowal uzytkownik
                    {
                        Dana dana = new Dana()
                        {
                            IloscWejsc = iloscWejsc,
                            IloscWyjsc = iloscWyjsc,
                            Wejscia = numbers.Take(iloscWejsc).ToList(),
                            Wyjscia = numbers.GetRange(iloscWejsc, iloscWyjsc)
                        };
                        DaneTreningowe.Add(dana);
                    }
                    else
                    {
                        string messageBoxText = "Błędny format pliku";
                        string caption = "Problem z plikiem";
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Error;
                        MessageBox.Show(messageBoxText, caption, button, icon);
                        Debug.WriteLine("Błędny format danych testowcyh w pliku");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error during reading from file " + ex);
            }
        }

        private void selectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select file with data";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == true)
            {
                filePath = openFileDialog1.FileName;
                selectedFileTextBox.Text = System.IO.Path.GetFileName(filePath);
            }
        }

        private void treningSieciButton_Click(object sender, RoutedEventArgs e)
        {
            if (Siec != null)
            {
                string path = @"../../Blad.txt";
                File.Create(path).Close();
                Seria1.Clear();
                Seria2.Clear();
                int iloscEpok = epokiTextBox.Value.Value;
                List<double> bledy = new List<double>();
                for (int i = 0; i < iloscEpok; i++)
                {
                    Siec.LiczEpoka(DaneTreningowe);
                    bledy.Add(Siec.BladSredniokwadratowy);
                    
                    if(i%50==0)
                    {
                        File.AppendAllText(path, "Epoka " + (i + 1) + " : " + Siec.BladSredniokwadratowy + Environment.NewLine);
                    }
                }
                for (int i = 0; i < bledy.Count;i=i+20)
                {
                    Seria1.Add(new KeyValuePair<double, double>(i + 1, bledy[i]));
                }

                string messageBoxText = "Trening ukończony !!!";
                string caption = "Trening";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
            
        }

        private void stworzSiecButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /////Wczytywanie danych z formularza
                double epsilon = epsilonTextBox.Value.Value;
                int iloscNeuronwoWarstwyUkrytej = iloscNeuronowUkrytychCounter.Value.Value;
                IloscWejsc = iloscWejscTextBox.Value.Value;
                IloscWyjsc = iloscWyjscTextBox.Value.Value;
                double momentum = momentumTextBox.Value.Value;
                double krokNauki = krokNaukiTextBox.Value.Value;
                bool czyBias = biasCheckBox.IsChecked.Value;
                //////////////////////////////////////////////////
               
                if(string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                Seria1.Clear();
                Seria2.Clear();
                wczytajDane(IloscWejsc, IloscWyjsc);
                Siec = new Siec();
                Siec.UtworzWarstweUkryta(iloscNeuronwoWarstwyUkrytej, DaneTreningowe);
                Siec.UtworzWarstweWyjsciowa(IloscWyjsc,momentum,krokNauki,czyBias);


                string messageBoxText = "Sieć została poprawnie utworzona !!!";
                string caption = "Sieć utworzona";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Nieznany błąd !!!" + ex);
            }


        }

        private void selectFileToTestButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select file with data";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == true)
            {
                filePathToTest = openFileDialog1.FileName;
                selectedFileToTestTextBox.Text = System.IO.Path.GetFileName(filePathToTest);
            }
        }

        private void testSieciButton_Click(object sender, RoutedEventArgs e)
        {
            if (Siec != null)
            {
                wczytajDaneTestowe(IloscWejsc, IloscWyjsc);
                Seria1.Clear();
                Seria2.Clear();
                var wyniki = Siec.LiczEpoka(DaneTestowe);
                foreach (var punkt in DaneTestowe)
                {
                    Seria1.Add(new KeyValuePair<double, double>(punkt.Wejscia[0], punkt.Wyjscia[0]));
                }
                foreach (var item in wyniki)
                {
                    Seria2.Add(new KeyValuePair<double, double>(item.Key, item.Value));
                }

                string messageBoxText = "Test zakończony.";
                string caption = "Test";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        private void wczytajDaneTestowe(int iloscWejsc, int iloscWyjsc)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePathToTest);
                DaneTestowe = new List<Dana>();
                foreach (var line in lines)
                {
                    var temp = line.Replace(".", ",");
                    var numbers = temp.Split(new Char[] { ';', ' ' }).Select(double.Parse).ToList();
                    if (numbers.Count == (iloscWejsc + iloscWyjsc)) ////sprawdzamy czy plik zgodny z tym co deklarowal uzytkownik
                    {
                        Dana dana = new Dana()
                        {
                            IloscWejsc = iloscWejsc,
                            IloscWyjsc = iloscWyjsc,
                            Wejscia = numbers.Take(iloscWejsc).ToList(),
                            Wyjscia = numbers.GetRange(iloscWejsc, iloscWyjsc)
                        };
                        DaneTestowe.Add(dana);
                    }
                    else
                    {
                        string messageBoxText = "Błędny format pliku";
                        string caption = "Problem z plikiem";
                        MessageBoxButton button = MessageBoxButton.OK;
                        MessageBoxImage icon = MessageBoxImage.Error;
                        MessageBox.Show(messageBoxText, caption, button, icon);
                        Debug.WriteLine("Błędny format danych testowcyh w pliku");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error during reading from file " + ex);
            }
        }
    }
}
