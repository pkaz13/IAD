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
                //string result = string.Format("Sukcesy 1 : {0} , Sukcesy 2 : {1} , Sukcesy 3 : {2} {3} Porazki 1 : {4} , Porazki 2 : {5} , Porazki 3 : {6}", counterTrue1, counterTrue2, counterTrue3, Environment.NewLine, counterFalse1, counterFalse2, counterFalse3);
                //File.AppendAllText(path, string.Format("Ilosc sukcesow : {0} , Ilosc porazek : {1} , Procent sukcesów : {2}",counterTrue1,counterFalse1,(double)(counterTrue1/(counterTrue1+counterFalse1*1.0))*100) + Environment.NewLine);
                File.AppendAllText(path, Environment.NewLine+ "----------STATYSTYKI----------"+Environment.NewLine);
                File.AppendAllText(path, string.Format("Rodzaj pierwszy -  Ilość sukcesów: {0} , Ilość porażek: {1} , Procent sukcesów: {2} ",counterTrue1,counterFalse1,(double)(counterTrue1/(counterTrue1+counterFalse1*1.0))*100) + Environment.NewLine);
                File.AppendAllText(path, string.Format("Rodzaj drugi -  Ilość sukcesów: {0} , Ilość porażek: {1} , Procent sukcesów: {2} ",counterTrue2,counterFalse2,(double)(counterTrue2/(counterTrue2+counterFalse2*1.0))*100) + Environment.NewLine);
                File.AppendAllText(path, string.Format("Rodzaj trzeci -  Ilość sukcesów: {0} , Ilość porażek: {1} , Procent sukcesów: {2} ", counterTrue3, counterFalse3, (double)(counterTrue3 / (counterTrue3 + counterFalse3 * 1.0)) * 100) + Environment.NewLine);
                File.AppendAllText(path, string.Format("Podsumowanie -  Ilość sukcesów: {0} , Ilość porażek: {1} , Procent sukcesów: {2} ", counterTrue1+counterTrue2+counterTrue3, counterFalse1+counterFalse2+counterFalse3, (double)((counterTrue1 + counterTrue2 + counterTrue3) / (counterTrue1 + counterTrue2 + counterTrue3 + counterFalse1 + counterFalse2 + counterFalse3 * 1.0)) * 100) + Environment.NewLine);
            }
            return blad;
        }

        public void TestujSiec(List<DanaTestowa> dane, string pathTofile,bool czyWynikiKlasyfikacji=false)
        {
            string pathToMatrix = @"../../../Logi/Macierz_klasyfikacji.txt";
            int counterTrue1=0, counterTrue2=0, counterTrue3 = 0,counterFalse1_2=0, counterFalse1_3=0, counterFalse2_1=0, counterFalse2_3=0, counterFalse3_1=0, counterFalse3_2=0;
            if (czyWynikiKlasyfikacji)
            {              
                System.IO.File.Create(pathToMatrix).Close();
            }         
            
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

                if (czyWynikiKlasyfikacji == true)
                {
                    bool flaga = true;

                    for (int i = 0; i < item.Wyjscia.Count; i++)
                    {                       
                        if (Math.Abs(item.Wyjscia[i] - temp[i]) > 0.1)
                        {
                            flaga = false;
                        }
                    }
                    if (flaga == true)
                    {
                        if (item.Wyjscia[0] == 1)
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
                        if (item.Wyjscia[0] == 1)
                        {
                            if (temp[1] > temp[2])
                                counterFalse1_2++;
                            else
                                counterFalse1_3++;
                        }
                        else if (item.Wyjscia[1] == 1)
                        {
                            if (temp[0] > temp[2])
                                counterFalse2_1++;
                            else
                                counterFalse2_3++;
                        }
                        else
                        {
                            if (temp[0] > temp[1])
                                counterFalse3_1++;
                            else
                                counterFalse3_2++;
                        }
                    }
                }
            }
            StringBuilder result = new StringBuilder();
            result.AppendLine("        | Klasa 1 | Klasa 2 | Klasa 3");
            result.AppendLine("--------|----------------------------");
            result.AppendLine(string.Format("Klasa 1 |   {0}       {1}       {2}  ",counterTrue1,counterFalse1_2,counterFalse1_3));
            result.AppendLine(string.Format("Klasa 2 |   {0}       {1}       {2}  ", counterFalse2_1, counterTrue2, counterFalse2_3));
            result.AppendLine(string.Format("Klasa 3 |   {0}       {1}       {2}  ", counterFalse3_1,  counterFalse3_2,counterTrue3));
            File.AppendAllText(pathToMatrix, result.ToString());
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
