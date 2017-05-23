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

        public double BladSredniokwadratowy { get; set; }

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
                WarstwaUkryta.Add(new RadialNeuron(i, punkty[i].IloscWejsc, temp[index],0,3));
                temp.RemoveAt(index);
            }
            foreach (var item in WarstwaUkryta)
            {
                item.UstawPromien(WarstwaUkryta);
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

        public void UstawNoweWagiWarstyUkrytej(List<Dana> punkty)
        {
            List<List<double>> temp = new List<List<double>>();
            foreach (var item in punkty)
            {
                temp.Add(item.Wejscia);
            }
            foreach (var item in WarstwaUkryta)
            {
                int index = MainWindow.random.Next(0, temp.Count - 1);
                item.Wagi = temp[index];
                temp.RemoveAt(index);
            }
        }

        public List<KeyValuePair<double,double>> LiczEpoka(List<Dana> punkty)
        {
            double bladSredniokwadratowy = 0;
            List<KeyValuePair<double, double>> temp = new List<KeyValuePair<double, double>>();
            foreach (var punkt in punkty)
            {
                
                List<double> wynikiWarstywUkrytej = new List<double>();
                foreach (var item in WarstwaUkryta)
                {
                    wynikiWarstywUkrytej.Add(item.LiczWyjscie(punkt.Wejscia));
                }

                List<double> wynikiSieci = new List<double>();
                foreach (var item in WarstwaWyjsciowa)
                {
                    wynikiSieci.Add(item.ObliczWyjscie(wynikiWarstywUkrytej));
                    item.ObliczBlad(punkt.Wyjscia[0]);
                    bladSredniokwadratowy += item.ObliczBladKwadratowy(punkt.Wyjscia[0]);
                    item.ZmianaWag();
                }
                temp.Add(new KeyValuePair<double, double>(punkt.Wejscia[0], wynikiSieci[0]));
                //UstawNoweWagiWarstyUkrytej(punkty);
            }
            BladSredniokwadratowy = bladSredniokwadratowy;
            return temp;
        }

    }
}
