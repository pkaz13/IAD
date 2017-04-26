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
        public double WspolczynnikZmianyNauki { get; set; } = 0.9999;
        public double WspolczynnikZmianyPromienia { get; set; } = 0.99;
        public double PromienSasiedztwa { get; set; } = 2;
        public bool CzyZmeczenie { get; set; }
        public double Blad { get; set; } = 0;

        public Siec(int iloscNeuronow,double wspolczynnikNauki, RodzajAlgorytmu rodzaj ,int losowanieOd, int losowanieDo,double promien,bool czyMeczenie=false)
        {
            Rodzaj = rodzaj;
            Neurony = new List<Neuron>();
            CzyZmeczenie = czyMeczenie;
            PromienSasiedztwa = promien;
            for (int i=0;i<iloscNeuronow;i++)
            {
                Neurony.Add(new Neuron(2, wspolczynnikNauki,rodzaj,i,losowanieOd,losowanieDo, CzyZmeczenie));
            }
        }

        public double LiczEpoka(List<KeyValuePair<double,double>> punkty)
        {
            Blad = 0;
            int idWygranego=0;
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
                ZmianaWag(idWygranego);
                foreach (var neuron in Neurony)
                {
                    neuron.WspolczynnikNauki *= WspolczynnikZmianyNauki;
                }
                PromienSasiedztwa *= WspolczynnikZmianyPromienia;
            }
            return Blad/punkty.Count;   
        }

        private int KohonenZnajdzZwyciezce(KeyValuePair<double, double> punkt)
        {
            int idWygrany = Neurony[0].Id;
            for (int i = 0; i < Neurony.Count; i++)
            {
                Neurony[i].LiczDystansDoWejscia(punkt);
                if (Neurony[i].Dystans< Neurony.FirstOrDefault(x=>x.Id== idWygrany).Dystans && Neurony[i].CzyZmeczony==false) 
                {
                    idWygrany = Neurony[i].Id;
                }
            }
            Blad += Neurony.FirstOrDefault(x => x.Id == idWygrany).Dystans;
            Neurony.FirstOrDefault(x => x.Id == idWygrany).CzyWygrany = true;
            return idWygrany;
        }

        private int GazNeuronowyZnajdzZwyciezce(KeyValuePair<double, double> punkt)
        {
            for (int i = 0; i < Neurony.Count; i++)
            {
                Neurony[i].LiczDystansDoWejscia(punkt);
            }
            Neurony = Neurony.OrderBy(x => x.Dystans).ToList();
            Blad += Neurony.First().Dystans;
            Neurony.First().CzyWygrany = true;
            return Neurony.First().Id;
        }

        private void ZmianaWag(int wygranyId)
        {
            if(Rodzaj==RodzajAlgorytmu.Kohonen)
            {
                foreach (var neuron in Neurony)
                {
                    neuron.LiczDystansDoZwyciezcy(Neurony.FirstOrDefault(x => x.Id == wygranyId).Wagi);
                    if (neuron.DystansDoZwyciezcy <= PromienSasiedztwa)
                    {
                        neuron.ZmianaWag(Neurony.Count, PromienSasiedztwa);
                    }
                }
            }
            else
            {
                for(int i=0;i<Neurony.Count;i++)
                {
                    Neurony[i].ZmianaWag(Neurony.Count,PromienSasiedztwa, i);
                }
            }
            Neurony.FirstOrDefault(x => x.Id == wygranyId).CzyWygrany = false;
        }
    }

}
