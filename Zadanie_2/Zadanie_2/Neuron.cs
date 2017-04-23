using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_2
{
    public class Neuron
    {
        public enum RodzajAlgorytmu
        {
            Kohonen,
            GazNeuronowy
        };

        public int Id { get; set; }
        public int IloscWejsc { get; set; }
        public List<double> Wagi { get; set; }
        public double Dystans { get; set; }
        public double DystansDoZwyciezcy { get; set; }
        public double WspolczynnikNauki { get; set; }
        public RodzajAlgorytmu Algorytm { get; set; }
        public bool CzyWygrany { get; set; } = false;
        public bool CzyZmeczony { get; set; }
        public double[] Wejscia { get; set; }

        public bool AktywneMeczenie { get; set; } = false;
        public double Potencjal { get; set; } = 1;
        public double ZmianaPotencjalu { get; set; } = 0.75;



        public Neuron()
        {

        }

        public Neuron(int iloscWejsc,double wspolczynnikNauki, RodzajAlgorytmu rodzaj,int id, int losowanieOd, int losowanieDo,bool czyMeczenie=false)
        {
            Id = id;
            AktywneMeczenie = czyMeczenie;
            IloscWejsc = iloscWejsc;
            WspolczynnikNauki = wspolczynnikNauki;
            Algorytm = rodzaj;
            Wagi = new List<double>(iloscWejsc);
            for (int i = 0; i < Wagi.Capacity; i++)
            {

                Wagi.Add(MainWindow.random.NextDouble()* (Math.Abs(losowanieOd)+Math.Abs(losowanieDo)) - losowanieOd);
            }
        }

        public double LiczDystansDoWejscia(KeyValuePair<double, double> punkt)
        {
            Wejscia = new double[] { punkt.Key, punkt.Value };
            double dystans = 0;
            for (int i = 0; i < Wagi.Count; i++)
            {
                dystans += (Wagi[i] - Wejscia[i]) * (Wagi[i] - Wejscia[i]);
            }
            Dystans= Math.Sqrt(dystans);
            return Dystans;
        }

        public double LiczDystansDoZwyciezcy(List<double> wagiZwyciezcy)
        {
            double dystans = 0;
            for (int i = 0; i < Wagi.Count; i++)
            {
                dystans += (Wagi[i] - wagiZwyciezcy[i]) * (Wagi[i] - wagiZwyciezcy[i]);
            }
            DystansDoZwyciezcy = Math.Sqrt(dystans);
            return DystansDoZwyciezcy;
        }

        public void ZmianaWag(int iloscNeuronow,double radius,int numerWKolejce=0)
        {
            if(AktywneMeczenie && CzyZmeczony ==true)
            {
                if(Potencjal>ZmianaPotencjalu)
                {
                    CzyZmeczony = false;
                }
            }
            double neigbourhoodFunction;
            if (CzyWygrany)
            {
                neigbourhoodFunction = 1;
                if (AktywneMeczenie)
                {
                    Potencjal -= ZmianaPotencjalu;
                    if (Potencjal < ZmianaPotencjalu)
                    {
                        CzyZmeczony = true;
                    }
                }
            }
            else
            {
                if(AktywneMeczenie && Potencjal<1)
                    Potencjal += 1.0 / iloscNeuronow;

                if (Algorytm==RodzajAlgorytmu.Kohonen)
                {
                    neigbourhoodFunction = Math.Exp(-1 * ((DystansDoZwyciezcy * DystansDoZwyciezcy) / (2 * radius * radius)));
                }
                else
                {
                    neigbourhoodFunction = Math.Exp(-1 * (numerWKolejce / radius));
                }
            }
            for (int i = 0; i < Wagi.Count; i++)
            {
                Wagi[i] += WspolczynnikNauki * neigbourhoodFunction * (Wejscia[i] - Wagi[i]);
            }
        }

    }
    }

