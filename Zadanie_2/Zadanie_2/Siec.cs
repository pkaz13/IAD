using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Zadanie_2.Neuron;

namespace Zadanie_2
{
    public class Siec
    {
        public List<Neuron> Neurony { get; set; }

        public Siec(int iloscNeuronow,double wspolczynnikNauki, RodzajAlgorytmu rodzaj)
        {
            Neurony = new List<Neuron>();
            for(int i=0;i<iloscNeuronow;i++)
            {
                Neurony.Add(new Neuron(2, wspolczynnikNauki,rodzaj));
            }
        }

        public double LiczEpoka()
        {
            double bladKwantyzacji = 0;
            return bladKwantyzacji;   
        }
    }
}
