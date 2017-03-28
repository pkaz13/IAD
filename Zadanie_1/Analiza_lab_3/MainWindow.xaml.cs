using Microsoft.Win32;
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

namespace Analiza_lab_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string filePath { get; set; }

        public Siec siec { get; set; }

        public List<DanaTestowa> DaneTestowe { get; set; }

        public MainWindow()
        {
            InitializeComponent();
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
                int ileEpok = int.Parse(epokiTextBox.Text);
                double epsilon = double.Parse(epsilonTextBox.Text);
                int ileWarstw = int.Parse(warstwyTextBox.Text);
                var ileNeuronowNaWarstwy = iloscNeuronowTextBox.Text.Split(';').Select(Int32.Parse).ToList();
                int iloscWejsc = int.Parse(iloscWejscTextBox.Text);
                int iloscWyjsc = int.Parse(iloscWyjscTextBox.Text);
                //////////////////////////////////////////////////
                wczytajDane(iloscWejsc, iloscWyjsc);

                siec = new Siec(ileEpok, epsilon);
                for (int i = 0; i < ileWarstw; i++)
                {                  
                    if (i == 0) /// pierwsza warstwa ukryta
                    {
                        Warstwa warstwa = new Warstwa(i, ileNeuronowNaWarstwy[i]);
                        warstwa.PoprzedniaWarstwa = null;
                        for (int j = 0; j < warstwa.IloscNeuronow; j++)
                        {
                            Neuron neuron = new Neuron(iloscWejsc, 0.2, true);
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
                            Neuron neuron = new Neuron(warstwa.PoprzedniaWarstwa.IloscNeuronow, 0.2, true);
                            warstwa.DodajNeuron(neuron);

                        }
                        siec.Warstwy.Add(warstwa);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Nieznany błąd !!!"+ex);
            }
        }

        private void wczytajDane(int iloscWejsc,int iloscWyjsc)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                DaneTestowe = new List<DanaTestowa>();
                foreach (var line in lines)
                {
                    var numbers = line.Split(new Char[] { ';', ' '}).Select(double.Parse).ToList();
                    if(numbers.Count== (iloscWejsc+ iloscWyjsc)) ////sprawdzamy czy plik zgodny z tym co deklarowal uzytkownik
                    {
                        DanaTestowa dana = new DanaTestowa()
                        {
                            IloscWejsc = iloscWejsc,
                            IloscWyjsc = iloscWyjsc,
                            Wejscia = numbers.Take(iloscWejsc).ToList(),
                            Wyjscia=numbers.GetRange(iloscWejsc, iloscWyjsc)
                        };
                        DaneTestowe.Add(dana);
                    }
                    else
                    {
                        Debug.WriteLine("Błędny format danych testowcyh w pliku");
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error during reading from file " + ex);
            }
            
        }
    }
}
