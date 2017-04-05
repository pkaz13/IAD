using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analiza_lab_3
{
    public class Siec
    {
        public int IloscEpok { get; set; }
        public double Epsilon { get; set; }
        public int IloscWejsc { get; set; }
        public int IloscWyjsc { get; set; }
        public List<Warstwa> Warstwy { get; set; }

        public Siec(int ileWejsc,int ileWyjsc)
        {
            Warstwy = new List<Warstwa>();
            IloscWejsc = ileWejsc;
            IloscWyjsc = ileWyjsc;
        }

        public Siec()
        {

        }

        public void DodajWarstwe(Warstwa warstwa)
        {
            Warstwy.Add(warstwa);
        }

        public double LiczEpoka(List<DanaTestowa> dane,string pathTofile)
        {
            double blad = 0;
            foreach (var item in dane)
            {
              
                List<double> temp = new List<double>();
                for (int i = 0; i < Warstwy.Count; i++)
                {
                    if (i == 0)
                    {
                        temp = Warstwy[i].SumujNeurony(item.Wejscia);
                        continue;
                    }
                    else
                    {
                        temp = Warstwy[i].SumujNeurony(temp);

                    }
                }
                if (!string.IsNullOrEmpty(pathTofile))
                {
                    string log = "Wartości spodziewane: ";
                    foreach (var danaWejsciowa in item.Wyjscia)
                    {
                        log += danaWejsciowa + ",";
                    }
                    log += "    Wartości otrzymane: ";
                    foreach (var wartoscObliczone in temp)
                    {
                        log += wartoscObliczone + ",";
                    }
                    File.AppendAllText(pathTofile, log + Environment.NewLine);
                }
                ObliczBladDlaPoszcegolnychNeuronow(item);
                ZmienWagi();                               
                blad+= LiczBladSredni();
            }
            return blad;
        }

        public void TestujSiec(List<DanaTestowa> dane, string pathTofile)
        {
            foreach (var item in dane)
            {
                string log = "Wartości spodziewane: ";
                foreach (var danaWejsciowa in item.Wyjscia)
                {
                    log += danaWejsciowa + ",";
                }
                log += "    Wartości otrzymane: ";
                List<double> temp = new List<double>();
                for (int i = 0; i < Warstwy.Count; i++)
                {
                    if (i == 0)
                    {
                        temp = Warstwy[i].SumujNeurony(item.Wejscia);
                        continue;
                    }
                    else
                    {
                        temp = Warstwy[i].SumujNeurony(temp);

                    }
                }
                foreach (var wartoscObliczone in temp)
                {
                    log += wartoscObliczone + ",";
                }
                File.AppendAllText(pathTofile, log + Environment.NewLine);
            }
        }

        public List<double> TestujSiec2(List<DanaTestowa> dane)
        {
            List<double> result = new List<double>();
            foreach (var item in dane)
            {
                List<double> temp = new List<double>();
                for (int i = 0; i < Warstwy.Count; i++)
                {
                    if (i == 0)
                    {
                        temp = Warstwy[i].SumujNeurony(item.Wejscia);
                        continue;
                    }
                    else
                    {
                        temp = Warstwy[i].SumujNeurony(temp);

                    }
                }
                foreach (var wynik in temp)
                {
                    result.Add(wynik);
                }
            }
            return result;
        }



        private void ObliczBladDlaPoszcegolnychNeuronow(DanaTestowa dane)
        {
            {
                for(int j= Warstwy.Count-1; j>=0;j--)
                {
                    for(int i=0;i< Warstwy[j].Neurony.Count;i++)
                    {
                        if(Warstwy[j].rodzajWarstwy==Warstwa.RodzajWarstwy.Wyjsciowa)
                        {
                            Warstwy[j].Neurony[i].ObliczBlad(Warstwy[j], dane.Wyjscia[i]);
                        }
                        else
                        {
                            Warstwy[j].Neurony[i].ObliczBlad(Warstwy[j]);
                        }

                    }
                }
            }
        }

        private void ZmienWagi()
        {
            foreach (var warstwa in Warstwy)
            {
                foreach (var neuron in warstwa.Neurony)
                {
                    neuron.ZmianaWag();
                }
            }
        }

        public double LiczBladSredni()
        {
            double sumaBledow = 0;
            var warstwaWyjsciowa = Warstwy.FirstOrDefault(x => x.rodzajWarstwy == Warstwa.RodzajWarstwy.Wyjsciowa);
            foreach (var item in warstwaWyjsciowa.Neurony)
            {
                sumaBledow += item.BladRoznicy;
            }
            return sumaBledow ;
        }
    }
}
