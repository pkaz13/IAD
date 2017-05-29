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

        public double Promien { get; set; }
        public double WspolczynnikSkalujacy { get; set; }


        public RadialNeuron(int id, int iloscWejsc, List<double> wagi,double wspolczynnikSkaulujacyOd,double wspolczynnikSkalujacyDo)
        {
            Id = id;
            Wagi =wagi;
            WspolczynnikSkalujacy = MainWindow.random.NextDouble() * (wspolczynnikSkalujacyDo - wspolczynnikSkaulujacyOd) + wspolczynnikSkaulujacyOd;
        }

        public double LiczDystansDoNeuronu(List<double> wagi)
        {
            double dystans = 0;
            for (int i = 0; i < Wagi.Count; i++)
            {
                dystans += (Wagi[i] - wagi[i]) * (Wagi[i] - wagi[i]);
            }
            return Math.Sqrt(dystans);
        }


        public double LiczDystansDoWejscia(List<double> punkt)
        {
            Wejscia = punkt;
            double dystans = 0;
            for (int i = 0; i < Wagi.Count; i++)
            {
                dystans += (Wagi[i] - Wejscia[i]) * (Wagi[i] - Wejscia[i]);
            }
            Dystans = Math.Sqrt(dystans);// *WspolczynnikSkalujacy;
            return Dystans;
        }

        public double LiczWyjscie(List<double> punkt)
        {
            LiczDystansDoWejscia(punkt);
            Wyjscie = Math.Exp(-1 * ((Dystans * Dystans) / (2 * Promien * Promien)));
            return Wyjscie;
        }

        public void UstawPromien(List<RadialNeuron> neurony)
        {
            List<double> dystanse = new List<double>();
            foreach (var item in neurony)
            {
                if (item.Id == this.Id)
                    continue;
                else
                {
                    dystanse.Add(LiczDystansDoNeuronu(item.Wagi));
                }
            }
            double average = dystanse.Average();
            Promien = average * 2;
        }

    }
}
