﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Analiza_lab_3
{
    public class Neuron
    {
        ////////////////////////////////////////////Ustawienia Sieci
        public double Momentum { get; set; }
        public double KrokNauki { get; set; }
        public bool CzyBias { get; set; }
        public bool CzySigmoidalnaAktywacja { get; set; }
        /////////////////////////////////////////////////////////////

        public List<double> Wagi { get; set; }
        public List<double> PoprzednieWagi { get; set; }
        private double wagaBiasu = 0;

        public int IloscWejsc { get; set; }
        private List<double> wejscia;
        [XmlIgnore]
        public double Blad { get; set; }
        [XmlIgnore]
        public double Suma { get; set; }
        [XmlIgnore]
        public double Wyjscie { get; set; }

        public Neuron()
        {

        }

        public Neuron(int iloscWejsc, double krok, bool czyBias,double momentum)
        {
            CzySigmoidalnaAktywacja = true;
            IloscWejsc = iloscWejsc;
            KrokNauki = krok;
            Momentum = momentum;
            CzyBias = czyBias;
            Wagi = new List<double>(iloscWejsc);
            for (int i = 0; i < Wagi.Capacity; i++)
            {

                Wagi.Add(MainWindow.random.NextDouble() * 2.0 - 1);
            }

        }

        public double ObliczWyjscie(List<double> daneWejsciowe)
        {
            if (daneWejsciowe.Count == Wagi.Count)
            {
                wejscia = daneWejsciowe;
                double suma = 0;
                for (int i = 0; i < Wagi.Count; i++)
                {
                    suma += Wagi[i] * daneWejsciowe[i];
                }
                if (CzyBias == true)
                {
                    suma += wagaBiasu + 1;
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

        public void ObliczBlad(Warstwa warstwa, double wartoscOczekiwana = 0)
        {
            if (warstwa.rodzajWarstwy == Warstwa.RodzajWarstwy.Wyjsciowa)
            {
                Blad = 1 / 2.0 * (wartoscOczekiwana - Wyjscie) * (wartoscOczekiwana - Wyjscie);
            }
            else
            {
                double sumaBledow = 0;
                for (int i = 0; i < warstwa.NastepnaWarstwa.Neurony.Count; i++)
                {
                    sumaBledow += warstwa.NastepnaWarstwa.Neurony[i].PropagacjaBledu(warstwa.Neurony.IndexOf(this));
                }
                Blad = sumaBledow * PochodnafunkcjiAktywacji(Suma);
            }

        }

        public void ZmianaWag()
        {
            for (int i = 0; i < wejscia.Count; i++)
            {
                Wagi[i] += (KrokNauki * Blad * wejscia[i]);
            }
            if (CzyBias)
            {
                wagaBiasu += (KrokNauki * Blad);
            }

        }

        private double FunkcjaAktywacji(double x)
        {
            if (CzySigmoidalnaAktywacja == true)
            {
                return 1 / (1 + Math.Exp(-x));
            }
            else
            {
                return x;
            }
        }

        private double PochodnafunkcjiAktywacji(double x)
        {
            if (CzySigmoidalnaAktywacja == true)
            {
                return FunkcjaAktywacji(x) * (1 - FunkcjaAktywacji(x));
            }
            else
            {
                return 1;
            }
        }

        public double PropagacjaBledu(int index)
        {
            return Wagi[index] * Blad;
        }

    }
}
