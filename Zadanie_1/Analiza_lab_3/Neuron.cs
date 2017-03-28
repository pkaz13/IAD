using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analiza_lab_3
{
    public class Neuron
    {
        public int IloscWejsc { get; set; }

        private List<double> wagi;

        private double wagaBiasu;

        public bool CzyBias { get; set; }

        public double KrokNauki { get; set; }

        public Neuron(int iloscWejsc,double krok,bool czyBias)
        {
            //ustawic wagi
            IloscWejsc = iloscWejsc;
            KrokNauki = krok;
            CzyBias = czyBias;
        }

        public double Sumuj(List<double> daneWejsciowe)
        {
            if (daneWejsciowe.Count == wagi.Count)
            {
                double suma = 0;
                for (int i = 0; i < wagi.Count; i++)
                {
                    suma += wagi[i] * daneWejsciowe[i];
                }
                if (CzyBias == true)
                {
                    suma += wagaBiasu + 1;
                }
                return suma;
            }
            else
            {
                ///Ilosc wag i wejsc jest niezgodna
                return 0;
            }
        }

    }
}
