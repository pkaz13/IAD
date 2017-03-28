using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analiza_lab_3
{
    public class DanaTestowa
    {
        public int IloscWejsc { get; set; }

        public int IloscWyjsc { get; set; }

        public List<double> Wejscia { get; set; }

        public List<double> Wyjscia { get; set; }

        public DanaTestowa()
        {
            Wejscia = new List<double>();
            Wyjscia = new List<double>();
        }
    }
}
