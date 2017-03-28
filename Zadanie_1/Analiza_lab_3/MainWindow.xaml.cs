using Microsoft.Win32;
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

namespace Analiza_lab_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string filePath { get; set; }

        public Siec siec { get; set; }

        public FileHelper helper;

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
                helper = new FileHelper(filePath);
            }

        }

        private void stworzSiecButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ileEpok = int.Parse(epokiTextBox.Text);
                double epsilon = double.Parse(epsilonTextBox.Text);
                int ileWarstw = int.Parse(warstwyTextBox.Text);
                var ileNeuronowNaWarstwy = iloscNeuronowTextBox.Text.Split(';').Select(Int32.Parse).ToList();
                siec = new Siec(ileEpok, epsilon);
                for (int i = 0; i < ileWarstw; i++)
                {
                    Warstwa warstwa = new Warstwa(i, ileNeuronowNaWarstwy[i]);
                    if (i == 0)
                    {
                        warstwa.PoprzedniaWarstwa = null;/////////pierwsza warstwa
                                                         /*
                                                          * Pobrac dane testowe z pliku i okreslic ile wejsc bedzie mial neuron
                                                          */
                    }
                    else
                    {
                        warstwa.PoprzedniaWarstwa = siec.Warstwy.FirstOrDefault(x => x.Id == i - 1);
                        for (int j = 0; i < warstwa.IloscNeuronow; i++)
                        {
                            Neuron neuron = new Neuron(warstwa.PoprzedniaWarstwa.IloscNeuronow, 0.2, true);
                            warstwa.DodajNeuron(neuron);

                        }
                        if (i == ileWarstw - 1)
                        {
                            warstwa.NastepnaWarstwa = null;
                            warstwa.rodzajWarstwy = Warstwa.RodzajWarstwy.Wyjsciowa;
                        }
                    }
                }
            }
            catch(Exception)
            {
                
            }
        }

        private void testBtn_Click(object sender, RoutedEventArgs e)
        {
            helper.ReadFromFile();
        }
    }
}
