using System;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ESimSol.Reports
{

    public class rptFNBatchCardDetail
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        MemoryStream _oMemoryStream = new MemoryStream();
        #endregion

        public byte[] PrepareReport()
        {
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(5f, 5f, 5f, 5f);
            _oPdfPTable.WidthPercentage = 90;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 535f });
            #endregion

            this.PrintHeader();
            this.ReportInfo();
            _oPdfPTable.HeaderRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 10f));
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 0);
            _oPdfPTable.AddCell(this.SetCellValue("Production Details", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 10f));
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion
        private void ReportInfo()
        {
            float cellHeight = 0f;

            #region Process
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.SetWidths(new float[] { 80f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 35f, 50f });

            List<string> tableHead = new List<string>(new string[] { "Process", "Batch Trolly No (In)", "Batch Trolly No (Out)", "Meter (In)","Meter (Out)", "Start Time", "End Time", "Date", "m/c Speed", "Before Width", "After Width", "Operator Name/ Signature" });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            foreach (string head in tableHead)
            {
                oPdfPTable.AddCell(this.SetCellValue(head, 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, 0));
            }
            oPdfPTable.CompleteRow();


            List<string> process = new List<string>(new string[] {
            "Singening + Desizing",
            "CBR(Washing Or S+B+W)",
            "QC Comments",
            "Mercerizer",
            "QC Comments",
            "CPB Dyeing",
            "Washing Range",
            "QC Comments",
            "Peaching",
            "QC Comments",
            "Stenter Finish",
            "Sanforizing",
            "Others-1",
            "Others-2",
            "Others-3"
            });

            cellHeight = 30f;
            bool IsComment = false;
            foreach (string processName in process)
            {
                oPdfPTable.AddCell(this.SetCellValue(processName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHeight));
                IsComment = (processName == "QC Comments") ? true : false;

                if (IsComment)
                {
                    oPdfPTable.AddCell(this.SetCellValue("", 0, tableHead.Count() - 1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHeight));
                }
                else
                {
                    for (int i = 0; i < tableHead.Count() - 1; i++)
                    {
                        oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHeight));
                    }
                }
                
                oPdfPTable.CompleteRow();
            }

            _oPdfPTable.AddCell(new PdfPCell(oPdfPTable));
            _oPdfPTable.CompleteRow();
            #endregion

            #region Quality Control & Assurance(At Final Stage)

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 10f));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Quality Control & Assurance(At Final Stage)", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 10f));
            _oPdfPTable.CompleteRow();


            cellHeight = 20f;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 50f, 120f, 120f, 120f, 120f });

            oPdfPTable.AddCell(this.SetCellValue("Shrinkage: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Warp......................%", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Weft: ......................%", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("PH: ......................%", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Finish Width: ...............Inches", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.CompleteRow();


            oPdfPTable.AddCell(this.SetCellValue("Fastness: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Dry Rub......................", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Wet Rub: ......................", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Water......................%", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("Strength: ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Tensile WP......................", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Tear WP: ......................", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Seam Slipage WT...................", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue(" ", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Tensile WT......................", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Tear WT: ......................", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Seam Slipage WT...................", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.CompleteRow();


            _oPdfPTable.AddCell(new PdfPCell(oPdfPTable));
            _oPdfPTable.CompleteRow();



            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 60f, 320f, 40f, 100f});

            oPdfPTable.AddCell(this.SetCellValue("QC Comments", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Sign", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 1, cellHeight));
            oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(new PdfPCell(oPdfPTable));
            _oPdfPTable.CompleteRow();

            #endregion

            #region Inspection

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 10f));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Inspection", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 10f));
            _oPdfPTable.CompleteRow();

            cellHeight = 20f;
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 80f, 180f, 80f, 180f });

            oPdfPTable.AddCell(this.SetCellValue("Folding Meter/Yards", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Fresh Meter/Yards", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("Total Meter/Yards", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Rags/ Fents", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("Inspection Date", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("Packing Date", 0, 0, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHeight));
            oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(new PdfPCell(oPdfPTable));
            _oPdfPTable.CompleteRow();
            #endregion

            #region Signature

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 175f, 175f, 175f });

            _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 35f));
            _oPdfPTable.CompleteRow();


            oPdfPTable.AddCell(this.SetCellValue("_________________", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0));
            oPdfPTable.AddCell(this.SetCellValue("_________________", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0));
            oPdfPTable.AddCell(this.SetCellValue("_________________", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("Prepared By", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0));
            oPdfPTable.AddCell(this.SetCellValue("QA & QC Incharge", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0));
            oPdfPTable.AddCell(this.SetCellValue("Inspection Incharge", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0));
            oPdfPTable.CompleteRow();

            PdfPCell oPdfPCell = new PdfPCell(oPdfPTable);
            oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion


        }


        private PdfPCell SetCellValue(string sName, int nRowSpan, int nColumnSpan, int halign, int valign, BaseColor color, int border, float height)
        {
            PdfPCell oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            oPdfPCell.HorizontalAlignment = halign;
            oPdfPCell.VerticalAlignment = valign;
            oPdfPCell.BackgroundColor = color;
            if (border == 0)
                oPdfPCell.Border = 0;
            if (height > 0)
                oPdfPCell.FixedHeight = height;
            return oPdfPCell;
        }
    }

}
