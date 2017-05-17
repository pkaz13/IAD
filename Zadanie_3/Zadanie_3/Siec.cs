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

        public void UtworzWarstweUkryta(int liczbaNeuronow,List<Dana> punkty)
        {
            WarstwaUkryta = new List<RadialNeuron>();
            List<List<double>> temp = new List<List<double>>(); 
            foreach (var item in punkty)
            {
                temp.Add(item.Wejscia);
            }
            for (int i = 0; i < liczbaNeuronow; i++)
            {
                int index = MainWindow.random.Next(0, temp.Count - 1);
                WarstwaUkryta.Add(new RadialNeuron(i, punkty[i].IloscWejsc, 0.2, temp[index]));
                temp.RemoveAt(index);
            }
        }

        public void UtworzWarstweWyjsciowa(int liczbaNeuronow)
        {
            WarstwaWyjsciowa = new List<Neuron>();
            for (int i = 0; i < liczbaNeuronow; i++)
            {
                WarstwaWyjsciowa.Add(new Neuron(i, WarstwaUkryta.Count, 0.2, true));
            }
        }

    }
}
