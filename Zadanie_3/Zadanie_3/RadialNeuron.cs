using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_3
{
    public class RadialNeuron
    {
        public int Id { get; set; }
        public List<double> Wagi { get; set; }
        public double Dystans { get; set; }
        public List<double> Wejscia { get; set; }
        public double Wyjscie { get; set; }
        public double WspolczynnikNauki { get; set; }
        public double Promien { get; set; }


        public RadialNeuron(int id, int iloscWejsc, double wspolczynnikNauki, List<double> wagi)
        {
            Id = id;
            WspolczynnikNauki = wspolczynnikNauki;
            Wagi =wagi;
        }

        public double LiczDystansDoWejscia(List<double> punkt)
        {
            Wejscia = punkt;
            double dystans = 0;
            for (int i = 0; i < Wagi.Count; i++)
            {
                dystans += (Wagi[i] - Wejscia[i]) * (Wagi[i] - Wejscia[i]);
            }
            Dystans = Math.Sqrt(dystans);
            return Dystans;
        }

        public double LiczWyjscie(List<double> punkt)
        {
            LiczDystansDoWejscia(punkt);
            Wyjscie = Math.Exp(-1 * ((Dystans * Dystans) / (2 * Promien * Promien)));
            return Wyjscie;
        }

        public void ZmianaWag()
        {
            for (int i = 0; i < Wagi.Count; i++)
            {
                Wagi[i] += WspolczynnikNauki * Wyjscie * (Wejscia[i] - Wagi[i]);
            }
        }
    }
}
