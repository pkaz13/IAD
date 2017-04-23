﻿using System;
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
        public double[] Wejscia { get; set; }



        public Neuron()
        {

        }

        public Neuron(int iloscWejsc,double wspolczynnikNauki, RodzajAlgorytmu rodzaj,int id)
        {
            Id = id;
            IloscWejsc = iloscWejsc;
            WspolczynnikNauki = wspolczynnikNauki;
            Algorytm = rodzaj;
            Wagi = new List<double>(iloscWejsc);
            for (int i = 0; i < Wagi.Capacity; i++)
            {

                Wagi.Add(MainWindow.random.NextDouble() * 2.0 - 1);
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

        public void ZmianaWag(double radius,int numerWKolejce=0)
        {
            double neigbourhoodFunction;
            if (CzyWygrany)
            {
                neigbourhoodFunction = 1;
            }
            else
            {
                if(Algorytm==RodzajAlgorytmu.Kohonen)
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

