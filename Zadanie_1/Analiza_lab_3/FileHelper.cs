using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analiza_lab_3
{
    public class FileHelper
    {
        public string FilePath { get; set; }

        private int[][] jaggedArray;

        public int[,] array;

        public FileHelper(string filePath)
        {
            this.FilePath = filePath; 
        }

        public void ReadFromFile()
        {
            try
            {
                using (StreamReader reader = File.OpenText(FilePath))
                {
                    //int numberOfLines = 0;

                    //while (reader.ReadLine() != null)
                    //{
                    //    numberOfLines++;
                    //}

                    jaggedArray = File.ReadAllLines(FilePath).Select(line => line.Split(' ').Select(i => int.Parse(i)).ToArray()).ToArray();
                    array = Generic.JaggedToMultidimensional(jaggedArray);
                    PrintArray(array);
                }
            }
            catch (Exception e)
            {

            }
        }

        public void PrintArray(int[,] array)
        {
            var rowCount = array.GetLength(0);
            var colCount = array.GetLength(1);

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                    Console.Write(String.Format("{0}\t", array[row, col]));
                Console.WriteLine();
            }
        }



    }
}
