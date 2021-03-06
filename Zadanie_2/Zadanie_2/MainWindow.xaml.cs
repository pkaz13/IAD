﻿using Microsoft.Win32;
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
        List<KeyValuePair<double, double>> Punkty = new List<KeyValuePair<double, double>>();
        ObservableCollection<KeyValuePair<double, double>> Neurony = new ObservableCollection<KeyValuePair<double, double>>();
        public static Random random = new Random();
        public string FilePath { get; set; }
        public Siec Siec { get; set; }
        public SiecK_Srednie SiecK { get; set; }
        public int IloscEpok { get; set; }


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
            SiecK = null;
            Siec = null;
            //////////WCZYTYWANIE DANYCH
            int iloscEpok = iloscEpokCounter.Value.Value;
            int iloscNeuronwo = iloscNeuronowCounter.Value.Value;
            double wspolczynnikNauki = wspolczynnikNaukiCounter.Value.Value;
            string algorytm = algorytmComboBox.SelectedValue.ToString();
            int losowanieWagOd = losowanieWagDoCounter.Value.Value;
            int losowanieWagDo = losowanieWagDoCounter.Value.Value;
            bool czyZmeczenie = zmeczenieCheckBox.IsChecked.Value;
            double promien = promienCounter.Value.Value;
            double zmianaPromienia = promienZmianaCounter.Value.Value;
            double zmianaNauki = naukaZmianaCounter.Value.Value;
            double zmianaPotencjalu = zmianaPotencjaluCounter.Value.Value;
            IloscEpok = iloscEpok;
            ////////////////////////////
            double blad = 0;
            Neurony.Clear();
            seria2.Refresh();
            bladLabel.Content = "";
            if (algorytm == "Kohonen")
            {
                Siec = new Siec(iloscNeuronwo, wspolczynnikNauki, Neuron.RodzajAlgorytmu.Kohonen, losowanieWagOd, losowanieWagDo, promien, czyZmeczenie, zmianaPotencjalu);
                Siec.WspolczynnikZmianyNauki = zmianaNauki;
                Siec.WspolczynnikZmianyPromienia = zmianaPromienia;
                foreach (var item in Siec.Neurony)
                {
                    Neurony.Add(new KeyValuePair<double, double>(item.Wagi[0], item.Wagi[1]));
                }
            }
            else if (algorytm == "Gaz neuronowy")
            {
                Siec = new Siec(iloscNeuronwo, wspolczynnikNauki, Neuron.RodzajAlgorytmu.GazNeuronowy, losowanieWagOd, losowanieWagDo, promien, czyZmeczenie, zmianaPotencjalu);
                Siec.WspolczynnikZmianyNauki = zmianaNauki;
                Siec.WspolczynnikZmianyPromienia = zmianaPromienia;
                foreach (var item in Siec.Neurony)
                {
                    Neurony.Add(new KeyValuePair<double, double>(item.Wagi[0], item.Wagi[1]));
                }
            }
            else
            {
                SiecK = new SiecK_Srednie(iloscNeuronwo, losowanieWagOd, losowanieWagDo);
                foreach (var item in SiecK.Neurony)
                {
                    Neurony.Add(new KeyValuePair<double, double>(item.Wagi[0], item.Wagi[1]));
                }
            }
            string messageBoxText = "Sieć poprawnie utworzona";
            string caption = "Sukces !!!";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(messageBoxText, caption, button, icon);
            Debug.WriteLine("Siec utworzona");


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
                Punkty.Clear();
                PunktyTreningowe.Clear();
                string[] lines = System.IO.File.ReadAllLines(FilePath);
                foreach (var line in lines)
                {
                    var temp = line.Replace(",", ";");
                    temp = temp.Replace(".", ",");
                    var numbers = temp.Split(new Char[] { ';', ' ' }).Select(double.Parse).ToList();
                    if (numbers.Count == 2)
                    {
                        Punkty.Add(new KeyValuePair<double, double>(numbers[0], numbers[1]));
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                if (Punkty.Count > 1000)
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        PunktyTreningowe.Add(Punkty[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < Punkty.Count; i++)
                    {
                        PunktyTreningowe.Add(Punkty[i]);
                    }
                }

                //PunktyTreningowe = new ObservableCollection<KeyValuePair<double, double>>(Punkty.Take(1000));
                //seria1.DataContext = PunktyTreningowe;
                //seria1.Refresh();
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
            if (algorytmComboBox.SelectedValue.ToString() == "K-średnie")
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

        private void epokaButton_Click(object sender, RoutedEventArgs e)
        {
            double blad = 0;
            if (Siec == null && SiecK == null)
            {
                string messageBoxText = "Brak sieci";
                string caption = "Brak sieci";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
                Debug.WriteLine("Brak sieci");
                return;
            }
            else
            {
                if (Siec != null)
                {
                    Neurony.Clear();
                    if (Punkty.Count > 0)
                    {

                        blad = Siec.LiczEpoka(Punkty.ToList());
                        foreach (var item in Siec.Neurony)
                        {
                            Neurony.Add(new KeyValuePair<double, double>(item.Wagi[0], item.Wagi[1]));
                        }
                        seria2.Refresh();
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
                    return;
                }
                else
                {
                    Neurony.Clear();
                    if (Punkty.Count > 0)
                    {
                        blad = SiecK.LiczEpoka(Punkty.ToList());
                        foreach (var item in SiecK.Neurony)
                        {
                            Neurony.Add(new KeyValuePair<double, double>(item.Wagi[0], item.Wagi[1]));
                        }
                        seria2.Refresh();
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
                    return;
                }
            }

        }

        private void wszystkieEpokiButton_Click(object sender, RoutedEventArgs e)
        {
            double blad = 0;
            if (Siec == null && SiecK == null)
            {
                string messageBoxText = "Brak sieci";
                string caption = "Brak sieci";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon);
                Debug.WriteLine("Brak sieci");
                return;
            }
            else
            {
                if (Siec != null)
                {
                    Neurony.Clear();

                    if (Punkty.Count > 0)
                    {
                        for (int i = 0; i < IloscEpok; i++)
                        {
                            blad = Siec.LiczEpoka(Punkty.ToList());
                        }
                        foreach (var item in Siec.Neurony)
                        {
                            Neurony.Add(new KeyValuePair<double, double>(item.Wagi[0], item.Wagi[1]));
                        }
                        seria2.Refresh();
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
                    return;
                }
                else
                {
                    Neurony.Clear();
                    if (Punkty.Count > 0)
                    {
                        for (int i = 0; i < IloscEpok; i++)
                        {
                            blad = SiecK.LiczEpoka(Punkty.ToList());
                        }
                        foreach (var item in SiecK.Neurony)
                        {
                            Neurony.Add(new KeyValuePair<double, double>(item.Wagi[0], item.Wagi[1]));
                        }
                        seria2.Refresh();
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
                    return;
                }
            }

        }
    }
}
