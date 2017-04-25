using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_2
{
    public class NeuronK_Srednie
    {
        public int Id { get; set; }
        public List<double> Wagi { get; set; }
        public double Dystans { get; set; }
        public double[] Wejscia { get; set; }
        public List<KeyValuePair<double, double>> PunktyNalezace { get; set; }
        public KeyValuePair<double,double> PoprzedniePolozenie { get; set; }

        public NeuronK_Srednie()
        {

        }

        public NeuronK_Srednie(int iloscWejsc, int id, int losowanieOd, int losowanieDo)
        {
            Id = id;
            PunktyNalezace = new List<KeyValuePair<double, double>>();
            Wagi = new List<double>(iloscWejsc);
            for (int i = 0; i < Wagi.Capacity; i++)
            {
                Wagi.Add(MainWindow.random.NextDouble() * (Math.Abs(losowanieOd) + Math.Abs(losowanieDo)) - losowanieOd);
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
            Dystans = Math.Sqrt(dystans);
            return Dystans;
        }

        public void ZmianaWag()
        {
            if(PunktyNalezace.Count>0)
            {
                Wagi[0] = PunktyNalezace.Average(x => x.Key);
                Wagi[1] = PunktyNalezace.Average(x => x.Value);
                PoprzedniePolozenie = new KeyValuePair<double, double>(Wagi[0], Wagi[1]);
            }
        }
    }
}
