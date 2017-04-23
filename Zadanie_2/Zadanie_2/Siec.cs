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
        public RodzajAlgorytmu Rodzaj { get; set; }
        public double WspolczynnikZmianyNauki { get; set; } = 0.999999;
        public double WspolczynnikZmianyPromienia { get; set; } = 0.9;

        public Siec(int iloscNeuronow,double wspolczynnikNauki, RodzajAlgorytmu rodzaj)
        {
            Rodzaj = rodzaj;
            Neurony = new List<Neuron>();
            for(int i=0;i<iloscNeuronow;i++)
            {
                Neurony.Add(new Neuron(2, wspolczynnikNauki,rodzaj,i));
            }
        }

        public double LiczEpoka(List<KeyValuePair<double,double>> punkty)
        {
            double bladKwantyzacji = 0;
            int idWygranego;
            foreach (var item in punkty)
            {
                if(Rodzaj==RodzajAlgorytmu.Kohonen)
                {
                    idWygranego = KohonenZnajdzZwyciezce(item);
                }
                else if(Rodzaj==RodzajAlgorytmu.GazNeuronowy)
                {
                    idWygranego = GazNeuronowyZnajdzZwyciezce(item);
                }
                ////tu zmiana wag !!!
                foreach (var neuron in Neurony)
                {
                    neuron.WspolczynnikNauki *= WspolczynnikZmianyNauki;
                    ///tu zmiana promienia !!!
                }
               
            }
            return bladKwantyzacji;   
        }

        public int KohonenZnajdzZwyciezce(KeyValuePair<double, double> punkt)
        {
            int idWygrany = Neurony[0].Id;
            for (int i = 0; i < Neurony.Count; i++)
            {
                Neurony[i].LiczDystansDoWejscia(punkt);
                if (Neurony[i].Dystans< Neurony.FirstOrDefault(x=>x.Id== idWygrany).Dystans ) ///tired 
                {
                    idWygrany = Neurony[i].Id;
                }
            }
            Neurony.FirstOrDefault(x => x.Id == idWygrany).CzyWygrany = true;
            return idWygrany;
        }

        public int GazNeuronowyZnajdzZwyciezce(KeyValuePair<double, double> punkt)
        {
            for (int i = 0; i < Neurony.Count; i++)
            {
                Neurony[i].LiczDystansDoWejscia(punkt);
            }
            var sortedNeurons=Neurony.OrderByDescending(x => x.Dystans);
            return sortedNeurons.First().Id;
        }
    }

}
