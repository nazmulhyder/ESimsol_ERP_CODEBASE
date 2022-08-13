using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class CellRowSpan
    {
        public CellRowSpan()
        {
            FieldName = "";
            mergerCell = new int[1, 2];
        }
        public string FieldName { get; set; }
        public int[,] mergerCell { get; set; }

    }

    public class MakeSpan
    {
        public static CellRowSpan GenerateRowSpan(string fieldName, int index, int rowSpan)
        {
            CellRowSpan oSRSpan = new CellRowSpan();
            int[,] mergerCell2D = new int[1, 2];
            mergerCell2D[0, 0] = index; mergerCell2D[0, 1] = rowSpan;

            oSRSpan.FieldName = fieldName;
            oSRSpan.mergerCell = mergerCell2D;

            return oSRSpan;
        }
    }
}
