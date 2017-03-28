using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analiza_lab_3
{
    public static class Generic
    {
        public static T[,] JaggedToMultidimensional<T>(T[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int columns = jaggedArray.Max(subArray => subArray.Length);
            T[,] array = new T[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    array[i, j] = jaggedArray[i][j];
                }
            }

            return array;
        }
    }
}
