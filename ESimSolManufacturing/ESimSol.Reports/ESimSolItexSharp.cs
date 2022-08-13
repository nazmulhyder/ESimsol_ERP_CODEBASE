using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.Reports
{
    public class ESimSolItexSharp
    {
        private static PdfPCell SetValue(string name, int rowSpan, int columnSpan, int align, BaseColor color, bool hasBorder, float height, int rotation, Font font)
        {
            PdfPCell oPdfPCell = new PdfPCell(new Phrase(name, font));
            oPdfPCell.Rowspan = (rowSpan > 0) ? rowSpan : 1;
            oPdfPCell.Colspan = (columnSpan > 0) ? columnSpan : 1;
            oPdfPCell.HorizontalAlignment = align;
            //oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = color;
            if (!hasBorder)
                oPdfPCell.Border = 0;
            if (height > 0)
                oPdfPCell.FixedHeight = height;

            if (rotation != 0)
            {
                oPdfPCell.Rotation = rotation;
            }
            return oPdfPCell;
        }
        public static PdfPCell SetCellValue(string name, int rowSpan, int columnSpan, int align, BaseColor color, bool hasBorder, float height, Font font)
        {
            return SetValue(name, rowSpan, columnSpan, align, color, hasBorder, height, 0, font);
        }

        public static void SetCellValue(ref PdfPTable oPdfPTable, string name, int rowSpan, int columnSpan, int align, BaseColor color, bool hasBorder, float height, int rotation, Font font)
        {
            PdfPCell oPdfPCell = SetValue(name, rowSpan, columnSpan, align, color, hasBorder, height, rotation, font);
            oPdfPTable.AddCell(oPdfPCell);
        }

        public static void SetCellValue(ref PdfPTable oPdfPTable, string name, int rowSpan, int columnSpan, int align, BaseColor color, bool hasBorder, float height, Font font)
        {
            PdfPCell oPdfPCell = SetValue(name, rowSpan, columnSpan, align, color, hasBorder, height, 0, font);
            oPdfPTable.AddCell(oPdfPCell);
        }
        public static void SetCellValue(ref PdfPTable oPdfPTable, string name, int rowSpan, int columnSpan, int align, BaseColor color, bool hasBorder, float height, Font font, bool hasRowComplete)
        {
            PdfPCell oPdfPCell = SetValue(name, rowSpan, columnSpan, align, color, hasBorder, height, 0, font);
            oPdfPTable.AddCell(oPdfPCell);
            if (hasRowComplete)
                oPdfPTable.CompleteRow();
        }
        public static void PushTableInCell(ref PdfPTable oPdfPTable, PdfPTable oChildTable, int rowSpan, int columnSpan, int align, BaseColor color, bool hasBorder, bool hasRowComplete)
        {
            PdfPCell oPdfPCell = new PdfPCell(oChildTable);
            oPdfPCell.Rowspan = (rowSpan > 0) ? rowSpan : 1;
            oPdfPCell.Colspan = (columnSpan > 0) ? columnSpan : 1;
            oPdfPCell.HorizontalAlignment = align;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = color;
            if (!hasBorder)
                oPdfPCell.Border = 0;

            oPdfPTable.AddCell(oPdfPCell);

            if (hasRowComplete)
                oPdfPTable.CompleteRow();
        }

    }
}
