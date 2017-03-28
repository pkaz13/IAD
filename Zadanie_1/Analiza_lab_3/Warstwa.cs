using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analiza_lab_3
{
    public class Warstwa
    {
        public enum RodzajWarstwy
        {
            Ukryta,
            Wyjsciowa
        }

        public int Id { get; set; }

        public int IloscNeuronow { get; set; }

        public List<Neuron> Neurony { get; set; }

        public RodzajWarstwy rodzajWarstwy { get; set; }

        public Warstwa PoprzedniaWarstwa { get; set; }

        public Warstwa NastepnaWarstwa { get; set; }

        public Warstwa(int id,int iloscNeurnow)
        {
            Id = id;
            IloscNeuronow = iloscNeurnow;
            Neurony = new List<Neuron>();
        }

        public void DodajNeuron(Neuron neuron)
        {
            Neurony.Add(neuron);
        }
    }
}
