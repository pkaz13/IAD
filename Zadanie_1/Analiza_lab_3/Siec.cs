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

        public double LiczEpoka(List<DanaTestowa> dane,string pathTofile,bool czyOstatniaEpoka=false)
        {
            int counterTrue1 = 0, counterFalse1 = 0, counterTrue2 = 0, counterTrue3=0, counterFalse2=0, counterFalse3=0;
            string path = @"../../../Logi/wyniki__.txt";
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
                if(czyOstatniaEpoka==true)
                {
                    bool falga = true;
                    string log = "";
                    
                    

                    for (int i=0;i<item.Wyjscia.Count;i++)
                    {
                        log="";
                        foreach (var danaWejsciowa in item.Wyjscia)
                        {
                            log += danaWejsciowa + ",";
                        }
                        log += "    Wartości otrzymane: ";
                        foreach (var wartoscObliczone in temp)
                        {
                            log += wartoscObliczone + ",";
                        }
                        falga = true;
                        if (Math.Abs(item.Wyjscia[i]-temp[i])>0.1)
                        {
                            falga = false;
                        }
                        
                    }
                    if (falga == true)
                    {
                        log += "    SUKCES !!!";
                        if(item.Wyjscia[0]==1)
                        {
                            counterTrue1++;
                        }
                        else if (item.Wyjscia[1] == 1)
                        {
                            counterTrue2++;
                        }
                        else
                        {
                            counterTrue3++;
                        }
                        
                    }
                    else
                    {
                        log += "    PORAZKA !!!";
                        if (item.Wyjscia[0] == 1)
                        {
                            counterFalse1++;
                        }
                        else if (item.Wyjscia[1] == 1)
                        {
                            counterFalse2++;
                        }
                        else
                        {
                            counterFalse3++;
                        }
                    }
                    File.AppendAllText(path, log + Environment.NewLine);
                }
                else
                {
                    ObliczBladDlaPoszcegolnychNeuronow(item);
                    ZmienWagi();
                }
                                       
                blad+= LiczBladSredni();
            }
            if(czyOstatniaEpoka)
            {
                string result = string.Format("Sukcesy 1 : {0} , Sukcesy 2 : {1} , Sukcesy 3 : {2} {3} Porazki 1 : {4} , Porazki 2 : {5} , Porazki 3 : {6}", counterTrue1, counterTrue2, counterTrue3, Environment.NewLine, counterFalse1, counterFalse2, counterFalse3);
                //File.AppendAllText(path, string.Format("Ilosc sukcesow : {0} , Ilosc porazek : {1} , Procent sukcesów : {2}",counterTrue1,counterFalse1,(double)(counterTrue1/(counterTrue1+counterFalse1*1.0))*100) + Environment.NewLine);
                File.AppendAllText(path, result);
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
