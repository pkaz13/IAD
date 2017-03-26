using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Laborki_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<KeyValuePair<double, double>> Seria1 = new ObservableCollection<KeyValuePair<double, double>>();
        ObservableCollection<KeyValuePair<double, double>> Seria2 = new ObservableCollection<KeyValuePair<double, double>>();
        ObservableCollection<KeyValuePair<double, double>> Seria3 = new ObservableCollection<KeyValuePair<double, double>>();

        private const int iloscPunktow = 9;
        public double[,] daneTreningowe;

        private Neuron neuron;

        private int count = 0;


        public MainWindow()
        {       
            InitializeComponent();
            seria1.DataContext = Seria1;
            seria2.DataContext = Seria2;
            seria3.DataContext = Seria3;
            neuron=new Neuron();
            daneTreningowe = new double[iloscPunktow, 3] 
            {
                {1, 1, 1},
                {2, -2, 0},
                { -1,-1.5,0},
                {-2,-1,0 },
                {-2,1,1 },
                {1.5,-0.5,1 },
                {-1,0.2,1 },
                {1,-0.6,0 },
                {0.5,-0.3,1 }

            };

            //Seria1.Add(new KeyValuePair<double, double>(1,1));
            //Seria1.Add(new KeyValuePair<double, double>(-2, 1));
            //Seria1.Add(new KeyValuePair<double, double>(1.5, -0.5));
            //Seria2.Add(new KeyValuePair<double, double>(2,-2));
            //Seria2.Add(new KeyValuePair<double, double>(-1,-1.5));
            //Seria2.Add(new KeyValuePair<double, double>(-2, -1));
            for (int i = -2; i < 3; i++)
            {
                Seria3.Add(new KeyValuePair<double, double>(i,neuron.fun(i)));
            }

            //for (int i = 0; i < 6; i++)
            //{
            //    neuron.Sumuj(daneTreningowe[i,0],daneTreningowe[i,1],(int)daneTreningowe[0,2]);
            //}
            //Seria3.Clear();
            //for (int i = -3; i < 4; i++)
            //{
            //    Seria3.Add(new KeyValuePair<double, double>(i, neuron.fun(i)));
            //}



        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (count == iloscPunktow)
                return;
            if((int)daneTreningowe[count, 2]==1)
            {
                Seria1.Add(new KeyValuePair<double, double>(daneTreningowe[count, 0], daneTreningowe[count, 1]));
            }
            else
            {
                Seria2.Add(new KeyValuePair<double, double>(daneTreningowe[count, 0], daneTreningowe[count, 1]));
            }
            neuron.Sumuj(daneTreningowe[count, 0], daneTreningowe[count, 1], (int)daneTreningowe[count, 2]);
            count++;
            
            Seria3.Clear();
            for (int i = -2; i < 3; i++)
            {
                Seria3.Add(new KeyValuePair<double, double>(i, neuron.fun(i)));
            }

        }
    }
}
