﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.Reports
{
    public class RoundedBorder : IPdfPCellEvent
    {
        public void CellLayout(PdfPCell cell, Rectangle rect, PdfContentByte[] canvas)
        {
            PdfContentByte cb = canvas[PdfPTable.BACKGROUNDCANVAS];
            cb.RoundRectangle(
              rect.Left + 1.5f,
              rect.Bottom + 1.5f,
              rect.Width - 3,
              rect.Height - 3, 4
            );
            cb.Stroke();
        }
    }
}
