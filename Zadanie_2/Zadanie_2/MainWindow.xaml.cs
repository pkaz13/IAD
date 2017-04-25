using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace Zadanie_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<KeyValuePair<double, double>> PunktyTreningowe = new ObservableCollection<KeyValuePair<double, double>>();
        ObservableCollection<KeyValuePair<double, double>> Neurony = new ObservableCollection<KeyValuePair<double, double>>();
        public static Random random = new Random();
        public string FilePath { get; set; }
        public Siec Siec { get; set; }


        public MainWindow()
        {        
            InitializeComponent();
            algorytmComboBox.Items.Add("Kohonen");
            algorytmComboBox.Items.Add("Gaz neuronowy");
            algorytmComboBox.Items.Add("K-średnie");
            algorytmComboBox.SelectedIndex = 0;

            seria1.DataContext = PunktyTreningowe;
            seria2.DataContext = Neurony;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            //////////WCZYTYWANIE DANYCH
            int iloscEpok = iloscEpokCounter.Value.Value;
            int iloscNeuronwo = iloscNeuronowCounter.Value.Value;
            double wspolczynnikNauki = wspolczynnikNaukiCounter.Value.Value;
            string algorytm = algorytmComboBox.SelectedValue.ToString();
            int losowanieWagOd = losowanieWagDoCounter.Value.Value;
            int losowanieWagDo = losowanieWagDoCounter.Value.Value;
            bool czyZmeczenie = zmeczenieCheckBox.IsChecked.Value;
            ////////////////////////////
            double blad = 0;
            if(algorytm== "Kohonen")
            {
                Siec = new Siec(iloscNeuronwo, wspolczynnikNauki, Neuron.RodzajAlgorytmu.Kohonen, losowanieWagOd, losowanieWagDo, czyZmeczenie);
            }               
            else if(algorytm== "Gaz neuronowy")
            {
                Siec = new Siec(iloscNeuronwo, wspolczynnikNauki, Neuron.RodzajAlgorytmu.GazNeuronowy,losowanieWagOd, losowanieWagDo, czyZmeczenie);
            }
            else
            {
                SiecK_Srednie siec= new SiecK_Srednie(iloscNeuronwo, losowanieWagOd, losowanieWagDo);
                for (int i = 0; i < iloscEpok; i++)
                {
                    blad=siec.LiczEpoka(PunktyTreningowe.ToList());
                }
                Neurony.Clear();
                foreach (var item in siec.Neurony)
                {
                    Neurony.Add(new KeyValuePair<double, double>(item.Wagi[0], item.Wagi[1]));
                }
                bladLabel.Content = blad.ToString();
                return;
            }
            Neurony.Clear();
            if(PunktyTreningowe.Count>0)
            {
                //foreach (var item in Siec.Neurony)
                //{
                //    Neurony.Add(new KeyValuePair<double, double>(item.Wagi[0], item.Wagi[1]));
                //}
                for (int i = 0; i < iloscEpok; i++)
                {
                    blad = Siec.LiczEpoka(PunktyTreningowe.ToList());
                }
                Neurony.Clear();
                foreach (var item in Siec.Neurony)
                {
                    Neurony.Add(new KeyValuePair<double, double>(item.Wagi[0], item.Wagi[1]));
                }
                bladLabel.Content = blad.ToString();
            }
            else
            {
                string messageBoxText = "Brak punktów treningowych";
                string caption = "Błąd";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
                Debug.WriteLine("Brak punktow treningowych");
            }

        }

        private void selectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select file with data";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == true)
            {
                FilePath = openFileDialog1.FileName;
                selectedFileTextBox.Text = System.IO.Path.GetFileName(FilePath);
                WczytajDane();
            }
        }

        private void WczytajDane()
        {
            try
            {
                PunktyTreningowe.Clear();
                string[] lines = System.IO.File.ReadAllLines(FilePath);
                foreach (var line in lines)
                {
                    var temp = line.Replace(",", ";");
                    temp = temp.Replace(".", ",");
                    var numbers = temp.Split(new Char[] { ';', ' ' }).Select(double.Parse).ToList();
                    if (numbers.Count == 2)
                    {
                        PunktyTreningowe.Add(new KeyValuePair<double, double>(numbers[0], numbers[1]));
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                string messageBoxText = "Błędny format pliku";
                string caption = "Problem z plikiem";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
                Debug.WriteLine("Error during reading from file " + ex);

            }
        }

        private void algorytmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(algorytmComboBox.SelectedValue.ToString()== "K-średnie")
            {
                wspolczynnikNaukiCounter.Visibility = Visibility.Collapsed;
                zmeczenieCheckBox.Visibility = Visibility.Collapsed;
                zmeczenieLabel.Visibility = Visibility.Collapsed;
                wspolczynnikLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                wspolczynnikNaukiCounter.Visibility = Visibility.Visible;
                zmeczenieCheckBox.Visibility = Visibility.Visible;
                zmeczenieLabel.Visibility = Visibility.Visible;
                wspolczynnikLabel.Visibility = Visibility.Visible;
            }
        }
    }
}
