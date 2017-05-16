using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_3
{
    public class Siec
    {
        public List<Neuron> WarstwaWyjsciowa { get; set; } = new List<Neuron>();
        public List<RadialNeuron> WarstwaUkryta { get; set; } = new List<RadialNeuron>();

    }
}
