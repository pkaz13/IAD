using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_2
{
    public class Neuron
    {
        public int IloscWejsc { get; set; }
        public List<double> Wagi { get; set; }

        public Neuron()
        {

        }

        public Neuron(int iloscWejsc)
        {
            IloscWejsc = iloscWejsc;
            Wagi = new List<double>(iloscWejsc);
            for (int i = 0; i < Wagi.Capacity; i++)
            {

                Wagi.Add(MainWindow.random.NextDouble() * 2.0 - 1);
            }
        }
    }
}
