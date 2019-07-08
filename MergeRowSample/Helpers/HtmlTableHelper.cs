using System;
using System.Collections.Generic;

namespace MergeRowSample
{
    static class HtmlTableHelper
    {
        internal static Td[][] GetTdMatrix<T>(List<T> elements, Func<T, Td[]> mergingTdsFunc, Func<T, Td[]> otherTdsFunc)
        {
            Td[][] array = new Td[elements.Count][];
            for (int i = 0; i < elements.Count; i++)
            {
                T element = elements[i];
                Td[] mergingTds = mergingTdsFunc(element);
                Td[] otherTds = otherTdsFunc(element);
                List<Td> tds = new List<Td>();
                for (int j = 0; j < mergingTds.Length; j++)
                {
                    Td td = mergingTds[j];
                    Td mergedTd = FindMergedTd(array, i - 1, j);
                    if (mergedTd != null && td.Text == mergedTd.Text)
                    {
                        mergedTd.Rowspan++;
                        tds.Add(null);
                    }
                    else
                    {
                        tds.Add(td);
                    }
                }
                tds.AddRange(otherTdsFunc(element));
                array[i] = tds.ToArray();
            }
            return array;
        }

        private static Td FindMergedTd(Td[][] array, int row, int col)
        {
            while (row >= 0)
            {
                Td td = array[row][col];
                if (td != null) return td;
                else row--;
            }
            return null;
        }
    }
}
