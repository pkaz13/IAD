using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;

namespace Laborki_1
{
    public class Neuron
    {
        private double waga0; //bias
        private double waga1;
        private double waga2;

        private double krokNauki=0.5;

        public Neuron()
        {
            var random=new Random();
            waga0 = 0;
            waga1 = 1;//random.NextDouble() *2 -1;
            waga2 = 0.5; //random.NextDouble() * 2 - 1;
        }

        public void Sumuj(double wejscie1,double wejscie2,int wynik,double wejscie0=1)
        {
            double suma = 0;
            suma = wejscie1*waga1 + wejscie2*waga2 + wejscie0*waga0;
            double E =(double) 1 / 2 * (wynik - suma) * (wynik - suma);
            if (suma > 0)
            {
                if (wynik == 1)
                    return;
                else
                {
                    waga1 = waga1 - krokNauki*wejscie1*E;
                    waga2 = waga2 - krokNauki * wejscie2*E;
                    waga0 = waga0 - krokNauki * wejscie0*E;
                }
            }
            else
            {
                if (wynik == 0)
                {
                    return;
                }
                else
                {
                    waga1 = waga1 + krokNauki * wejscie1*E;
                    waga2 = waga2 + krokNauki * wejscie2*E;
                    waga0 = waga0 + krokNauki * wejscie0*E;
                }
            }
            return;
        }

        public double fun(double x)
        {
            return -(waga1/waga2)*x - waga0/waga2;
        }



    }
}
