﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Xml.Serialization;
using Microsoft.Windows.Controls;
using System.Collections.ObjectModel;

namespace Analiza_lab_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<KeyValuePair<int, double>> Seria1 = new ObservableCollection<KeyValuePair<int, double>>();
        public string filePath { get; set; }

        public Siec siec { get; set; }

        public int IloscEpok { get; set; }

        public List<DanaTestowa> DaneTestowe { get; set; }

        public static Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            seria1.DataContext = Seria1;

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

        private void stworzSiecButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /////Wczytywanie danych z formularza
                IloscEpok = epokiTextBox.Value.Value;
                double epsilon = epsilonTextBox.Value.Value;
                int ileWarstw = warstwyTextBox.Value.Value + 1;
                var ileNeuronowNaWarstwy = iloscNeuronowTextBox.Text.Split(';').Select(Int32.Parse).ToList();
                int iloscWejsc = iloscWejscTextBox.Value.Value;
                int iloscWyjsc = iloscWyjscTextBox.Value.Value;
                double momentum = momentumTextBox.Value.Value;
                double krokNauki = krokNaukiTextBox.Value.Value;
                bool czyBias = biasCheckBox.IsChecked.Value;
                //////////////////////////////////////////////////

                siec = new Siec(IloscEpok, epsilon, iloscWejsc,iloscWyjsc);
                for (int i = 0; i < ileWarstw; i++)
                {
                    if (i == 0) /// pierwsza warstwa ukryta
                    {
                        Warstwa warstwa = new Warstwa(i, ileNeuronowNaWarstwy[i]);
                        warstwa.PoprzedniaWarstwa = null;
                        for (int j = 0; j < warstwa.IloscNeuronow; j++)
                        {
                            Neuron neuron = new Neuron(iloscWejsc, krokNauki, czyBias, momentum);
                            warstwa.DodajNeuron(neuron);

                        }
                        warstwa.rodzajWarstwy = Warstwa.RodzajWarstwy.Ukryta;
                        siec.Warstwy.Add(warstwa);
                        continue;
                    }
                    else
                    {
                        Warstwa warstwa = null;
                        if (i == ileWarstw - 1) ///ostatnia warstwa - wyjsciowa ktora daje wyniki i musi miec tyle neuronow ile wyjsc
                        {
                            warstwa = new Warstwa(i, iloscWyjsc);
                            warstwa.NastepnaWarstwa = null;
                            warstwa.rodzajWarstwy = Warstwa.RodzajWarstwy.Wyjsciowa;
                        }
                        else  ////// pozostałe warstwy - ukryte 
                        {
                            warstwa = new Warstwa(i, ileNeuronowNaWarstwy[i]);
                            warstwa.rodzajWarstwy = Warstwa.RodzajWarstwy.Ukryta;
                        }

                        var warstwaPoprzednia = siec.Warstwy.FirstOrDefault(x => x.Id == i - 1);
                        warstwaPoprzednia.NastepnaWarstwa = warstwa;
                        warstwa.PoprzedniaWarstwa = warstwaPoprzednia;
                        for (int j = 0; j < warstwa.IloscNeuronow; j++)
                        {
                            Neuron neuron = new Neuron(warstwa.PoprzedniaWarstwa.IloscNeuronow, krokNauki, czyBias, momentum);
                            warstwa.DodajNeuron(neuron);

                        }
                        siec.Warstwy.Add(warstwa);
                    }
                }
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

        private void wczytajDane(int iloscWejsc, int iloscWyjsc)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                DaneTestowe = new List<DanaTestowa>();
                foreach (var line in lines)
                {
                    var temp = line.Replace(".", ",");
                    var numbers = temp.Split(new Char[] { ';', ' ' }).Select(double.Parse).ToList();
                    if (numbers.Count == (iloscWejsc + iloscWyjsc)) ////sprawdzamy czy plik zgodny z tym co deklarowal uzytkownik
                    {
                        DanaTestowa dana = new DanaTestowa()
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

        private void PrzejdzWszystkieEpoki()
        {
            Array.ForEach(Directory.GetFiles(@"../../../Logi/"), File.Delete);
            string path = @"../../../Logi/Epoka_.txt";
            string pathToError = @"../../../Logi/Bledy.txt";
            System.IO.File.Create(path).Close();
            System.IO.File.Create(pathToError).Close();
            for (int i = 0; i < siec.IloscEpok; i++)
            {
                File.AppendAllText(path, "-------------------Epoka " + (i + 1) + Environment.NewLine);

                siec.LiczEpoka(DaneTestowe, path);

                double blad = siec.LiczBladSredni();
                Seria1.Add(new KeyValuePair<int, double>(i + 1, blad));
                File.AppendAllText(pathToError, "-------------------Epoka " + (i + 1) + Environment.NewLine);
                File.AppendAllText(pathToError, "Błąd sieci równy : " + blad + Environment.NewLine);
                if (blad < siec.Epsilon)
                    break;
            }
        }

        private void serializujSiecButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "NeuronalNetwork"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                XmlSerializer serializer = new XmlSerializer(typeof(Siec));
                using (var stream = new StreamWriter(filename))
                {
                    serializer.Serialize(stream, siec);
                    string messageBoxText = "Sieć poprawnie zapisana do pliku xml !!!";
                    string caption = "Sieć zapisana";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBox.Show(messageBoxText, caption, button, icon);
                }
            }
        }

        private void deserialuzujSiecButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Title = "Select file with network";
                openFileDialog1.DefaultExt = ".xml";
                if (openFileDialog1.ShowDialog() == true)
                {
                    String filename = openFileDialog1.FileName;
                    XmlSerializer serializer = new XmlSerializer(typeof(Siec));
                    FileStream fs = new FileStream(filename, FileMode.Open);
                    siec = (Siec)serializer.Deserialize(fs);
                    string messageBoxText = "Sieć poprawnie wczytana z pliku xml !!!";
                    string caption = "Sieć wczytana";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBox.Show(messageBoxText, caption, button, icon);
                }
            }
            catch (Exception)
            {

            }
        }

        private void treningSieciButton_Click(object sender, RoutedEventArgs e)
        {
            if(siec!=null)
            {
                siec.IloscEpok = epokiTextBox.Value.Value;
                Seria1.Clear();
                wczytajDane(siec.IloscWejsc, siec.IloscWyjsc);
                PrzejdzWszystkieEpoki();
                string messageBoxText = "Trening ukończony !!!";
                string caption = "Trening";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }
    }
}
