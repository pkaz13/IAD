using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_2
{
    public class SiecK_Srednie
    {
        public List<NeuronK_Srednie> Neurony { get; set; }

        public SiecK_Srednie(int iloscNeuronow, int losowanieOd, int losowanieDo)
        {
            Neurony = new List<NeuronK_Srednie>();
            for (int i = 0; i < iloscNeuronow; i++)
            {
                Neurony.Add(new NeuronK_Srednie(2,  i, losowanieOd, losowanieDo));
            }
        }

        public double LiczEpoka(List<KeyValuePair<double, double>> punkty)
        {
            double blad = 0;
            ZnajdzPolaczenie(punkty);
            ZmienWagi();
            for (int i = 0; i < Neurony.Count(); i++)
            {
                Neurony[i].PunktyNalezace.Clear();
                blad += Neurony[i].Dystans;
            }
            return blad*1.0 / Neurony.Count();
        }

        public void ZnajdzPolaczenie(List<KeyValuePair<double, double>> punkty)
        {
            int neuronId= Neurony[0].Id; ;
            foreach (var punkt in punkty)
            {
                foreach (var neuron in Neurony)
                {
                    neuron.LiczDystansDoWejscia(punkt);
                    if (neuron.Dystans < Neurony.FirstOrDefault(x => x.Id == neuronId).Dystans)
                    {
                        neuronId = neuron.Id;
                    }
                }
                Neurony.FirstOrDefault(x => x.Id == neuronId).PunktyNalezace.Add(punkt);
            }
        }

        public void ZmienWagi()
        {
            for (int i = 0; i < Neurony.Count(); i++)
            {
                Neurony[i].ZmianaWag();
            }
        }
    }
}
