using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analiza_lab_3
{
    public class Siec
    {
        public int IloscEpok { get; set; }

        public double Epsilon { get; set; }

        public List<Warstwa> Warstwy { get; set; }

        public Siec(int ileEpok,double epsilon)
        {
            IloscEpok = ileEpok;
            Epsilon = epsilon;
            Warstwy = new List<Warstwa>();
        }

        public void DodajWarstwe(Warstwa warstwa)
        {
            Warstwy.Add(warstwa);
        }

    }
}
