using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_3
{
    public class Neuron
    {
        public int Id { get; set; }
        ////////////////////////////////////////////Ustawienia Sieci
        public double KrokNauki { get; set; }
        public bool CzyBias { get; set; }
        public double Momentum { get; set; }
        /////////////////////////////////////////////////////////////
        public List<double> Wejscia { get; set; }
        public List<double> Wagi { get; set; }
        public double WagaBiasu { get; set; }

        public List<double> PoprzednieWagi { get; set; }
        public double PoprzedniaWagaBiasu { get; set; }


        public double Blad { get; set; }
        public double Suma { get; set; }
        public double Wyjscie { get; set; }


        public Neuron(int id,int iloscWejsc, double krok, bool czyBias)
        {
            Id = id;
            KrokNauki = krok;
            CzyBias = czyBias;
            Wagi = new List<double>(iloscWejsc);
            PoprzednieWagi = new List<double>(iloscWejsc);
            for (int i = 0; i < PoprzednieWagi.Capacity; i++)
            {
                PoprzednieWagi.Add(0);
            }
            for (int i = 0; i < Wagi.Capacity; i++)
            {
                Wagi.Add(MainWindow.random.NextDouble() * 2.0 - 1);
            }
            if (czyBias == true)
            {
                WagaBiasu = MainWindow.random.NextDouble() * 2.0 - 1;
            }

        }

        public double ObliczWyjscie(List<double> daneWejsciowe)
        {
            if (daneWejsciowe.Count == Wagi.Count)
            {
                Wejscia = daneWejsciowe;
                double suma = 0;
                for (int i = 0; i < Wagi.Count; i++)
                {
                    suma += Wagi[i] * daneWejsciowe[i];
                }
                if (CzyBias == true)
                {
                    suma += WagaBiasu;
                }
                Suma = suma;
                Wyjscie = FunkcjaAktywacji(suma);
                return Wyjscie;
            }
            else
            {
                ///Ilosc wag i wejsc jest niezgodna
                return 0;
            }
        }

        public void ZmianaWag()
        {
            for (int i = 0; i < Wejscia.Count; i++)
            {
                double temp = Wagi[i];
                Wagi[i] += (KrokNauki * Blad * Wejscia[i] + Momentum * PoprzednieWagi[i]);
                PoprzednieWagi[i] = Wagi[i] - temp;
            }
            if (CzyBias)
            {
                double temp = WagaBiasu;
                WagaBiasu += (KrokNauki * Blad + Momentum * PoprzedniaWagaBiasu);
                PoprzedniaWagaBiasu = WagaBiasu - temp;
            }
        }

        private double FunkcjaAktywacji(double x)
        {
            return x;
        }

        public void ObliczBlad( double wartoscOczekiwana = 0)
        {
            Blad = 1.0 * (wartoscOczekiwana - Wyjscie);        
        }

        public double ObliczBladKwadratowy(double wartoscOczekiwana)
        {
            return 1 / 2.0 * (wartoscOczekiwana - Wyjscie) * (wartoscOczekiwana - Wyjscie);
        }
    }
}
