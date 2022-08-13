using System;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ESimSol.Reports
{

    public class rptFNBatchCard
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        int _nColumn = 7;
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        FNBatch _oFNBatch = new FNBatch();
        Company _oCompany = new Company();
        #endregion

        public byte[] PrepareReport(FNBatch oFNBatch, Company oCompany)
        {
            _oFNBatch = oFNBatch;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(20f, 20f, 30f, 40f);
            _oPdfPTable.WidthPercentage = 90;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 60f, 5f, 200f, 50f, 70f, 5f, 140f });
            #endregion

            this.PrintHeader();
            this.ReportInfo();
            this.MiddlePart();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader


            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 80f, 300.5f, 80f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Fabric Process Route Card", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.ExtraParagraphSpace = 20f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion
        private void ReportInfo()
        {
            float cellHight = 15f;
            _oPdfPTable.AddCell(this.SetCellValue("", 0, _nColumn, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 15f));
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

            _oPdfPTable.AddCell(this.SetCellValue("Exe Order No", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(_oFNBatch.FNExONo, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue("Grey GSM", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(((_oFNBatch.GreyGSM > 0) ? Global.MillionFormat(_oFNBatch.GreyGSM) : "--        ") + "   grams", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.CompleteRow();


            _oPdfPTable.AddCell(this.SetCellValue("Order Qty", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(RemoveDecimalPoint(Global.MillionFormat(Math.Round(_oFNBatch.ExeQty)))+"   Yards", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue("GLM", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(((_oFNBatch.GLM > 0) ? Global.MillionFormat(_oFNBatch.GLM) : "--        ") + "   grams", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.CompleteRow();


            _oPdfPTable.AddCell(this.SetCellValue("Issue Qty", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(RemoveDecimalPoint(Global.MillionFormat(Math.Round(_oFNBatch.QtyInMtr))) + "   Meters", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue("Construction", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(_oFNBatch.Construction, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Customer", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(_oFNBatch.BuyerName, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue("Count", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(_oFNBatch.CountName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Issue Date", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(_oFNBatch.IssueDateStr, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue("Grey Width", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(((_oFNBatch.GreyWidth > 0) ? Global.MillionFormat(_oFNBatch.GreyWidth) : "--        ") + "   inches", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Delivery Date", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(_oFNBatch.ExpectedDeliveryDateStr, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue("Finish Width", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(((_oFNBatch.FinishWidth != "") ? _oFNBatch.FinishWidth : "--        ") + "   inches", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Finish Type", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(_oFNBatch.FinishTypeName, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue("Fabric Weave", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(_oFNBatch.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Batch No", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.AddCell(this.SetCellValue(_oFNBatch.BatchNo, 0, 5, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("", 0, _nColumn, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 15f));
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            string sDotted = "________________________________________________________________________________";
            _oPdfPTable.AddCell(this.SetCellValue("Dyeing Color Detail: " + sDotted, 0, _nColumn, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("", 0, _nColumn, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 15f));
            _oPdfPTable.CompleteRow();
        }

        private void MiddlePart()
        {
            float nHeight = 105f;
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPTable.AddCell(this.SetCellValue("Special Instruction", 0, _nColumn, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPTable.AddCell(this.SetCellValue("1. Singening & Desizing", 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
            _oPdfPTable.AddCell(this.SetCellValue("2. Washing & Bleaching", 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("3. Mercerizing", 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
            _oPdfPTable.AddCell(this.SetCellValue("4. Dyeing & Washing", 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("5. Stenter", 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
            _oPdfPTable.AddCell(this.SetCellValue("6. Sanforize", 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("7. Peaching", 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
            _oPdfPTable.AddCell(this.SetCellValue("8. Inspection", 0, 4, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
            _oPdfPTable.CompleteRow();
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

        private string RemoveDecimalPoint(string val)
        {
            if(val.Contains('.'))
                val = val.Split('.')[0];
            return val;
        }
    }


    
}
