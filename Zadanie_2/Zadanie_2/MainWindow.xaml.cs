using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<KeyValuePair<double, double>> Seria1 = new ObservableCollection<KeyValuePair<double, double>>();
        public static Random random = new Random();


        public MainWindow()
        {        
            InitializeComponent();
            seria1.DataContext = Seria1;
            for (int i = 0; i < 10; i++)
            {
                Seria1.Add(new KeyValuePair<double, double>(i, i));
            }
        }
    }
}
