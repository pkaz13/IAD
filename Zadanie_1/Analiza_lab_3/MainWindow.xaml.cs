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
using System.Xml.Serialization;
using Microsoft.Windows.Controls;

namespace Analiza_lab_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string filePath { get; set; }

        public Siec siec { get; set; }

        public int IloscEpok { get; set; }

        public List<DanaTestowa> DaneTestowe { get; set; }

        public static Random random = new Random();

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
                IloscEpok = epokiTextBox.Value.Value;
                double epsilon = epsilonTextBox.Value.Value;
                int ileWarstw = warstwyTextBox.Value.Value+1;
                var ileNeuronowNaWarstwy = iloscNeuronowTextBox.Text.Split(';').Select(Int32.Parse).ToList();
                int iloscWejsc = iloscWejscTextBox.Value.Value;
                int iloscWyjsc = iloscWyjscTextBox.Value.Value;
                double momentum = momentumTextBox.Value.Value;
                double krokNauki = krokNaukiTextBox.Value.Value;
                bool czyBias = biasCheckBox.IsChecked.Value;
                //////////////////////////////////////////////////
                wczytajDane(iloscWejsc, iloscWyjsc);

                siec = new Siec(IloscEpok, epsilon);
                for (int i = 0; i < ileWarstw; i++)
                {                  
                    if (i == 0) /// pierwsza warstwa ukryta
                    {
                        Warstwa warstwa = new Warstwa(i, ileNeuronowNaWarstwy[i]);
                        warstwa.PoprzedniaWarstwa = null;
                        for (int j = 0; j < warstwa.IloscNeuronow; j++)
                        {
                            Neuron neuron = new Neuron(iloscWejsc, krokNauki, czyBias);
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
                            Neuron neuron = new Neuron(warstwa.PoprzedniaWarstwa.IloscNeuronow, krokNauki, czyBias);
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
            PrzejdzWszystkieEpoki();
            SerializujSiec();


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

        private void PrzejdzWszystkieEpoki()
        {
            Array.ForEach(Directory.GetFiles(@"../../../Logi/"), File.Delete);
            for (int i = 0; i < IloscEpok; i++)
            {
                string path = @"../../../Logi/Epoka_" + (i + 1) + ".txt";
                System.IO.File.Create(path).Close();
                siec.LiczEpoka(DaneTestowe,path);
            }
        }

        private void SerializujSiec()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Siec));
            using (var stream = new StreamWriter("../../../StrukturaSieci.xml"))
            {
                serializer.Serialize(stream, siec);
            }
        }

       
    }
}
