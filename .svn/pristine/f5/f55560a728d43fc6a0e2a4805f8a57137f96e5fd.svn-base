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
    public class rptLabDipSubCard
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(8);
        int _nColumn = 8;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        #endregion
        public byte[] PrepareReport(List<LabDipDetail> oLabDipDetails, Company oCompany, BusinessUnit oBusinessUnit, LabDip oLabDip, List<LabdipShade> oLabdipShades)
        {
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(20f, 20f, 10f, 40f);
            _oPdfPTable.WidthPercentage = 90;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 20f, 
                                                87f, 5f, 185f, 
                                                102f, 5f, 170f, 20f });
            #endregion

            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBusinessUnit, oCompany, "LAB-DIP SUBMISSION CARD",_nColumn);
            
            //this.PrintHeader();
            this.ReportInfo(oLabDip, oLabDipDetails);
            this.MiddlePart(oLabdipShades);
            this.SignaturePart();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void ReportInfo(LabDip oLabDip, List<LabDipDetail> oLabDipDetails)
        {
            float cellHight = 15f;
            _oPdfPTable.AddCell(this.SetCellValue("", 0, _nColumn, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 15f));
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

            string[] sTop = new string[] { "TOP" };
            string[] sTopLeft = new string[] { "TOP", "LEFT" };
            string[] sTopRight = new string[] { "TOP", "RIGHT" };
            string[] sTopBottom = new string[] { "TOP", "BOTTOM" };
            string[] sTopBottomLeft = new string[] { "TOP", "BOTTOM", "LEFT" };
            string[] sTopBottomRight = new string[] { "TOP", "BOTTOM", "RIGHT" };

            _oPdfPTable.AddCell(this.SetCellValue("Buyer", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oLabDip.ContractorName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue("LabDip Quality", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oLabDip.LabdipNo + " " + oLabDipDetails[0].ProductName + " " + oLabDip.LabDipFormatStr, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopRight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Ref No", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oLabDip.BuyerRefNo, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue("Twisted", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oLabDip.TwistedStr, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopRight));
            _oPdfPTable.CompleteRow();


            _oPdfPTable.AddCell(this.SetCellValue("Light Source", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oLabDip.LightSourceName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue("Color Name", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oLabDipDetails[0].ColorName, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopRight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Seeking Date", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottomLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottom));
            _oPdfPTable.AddCell(this.SetCellValue(oLabDip.SeekingDateStr, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottom));
            _oPdfPTable.AddCell(this.SetCellValue("Lab-Dip No", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottomLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottom));
            _oPdfPTable.AddCell(this.SetCellValue(oLabDipDetails[0].LabdipNo, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottomRight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("", 0, _nColumn, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 15f));
            _oPdfPTable.CompleteRow();
        }

        private void MiddlePart(List<LabdipShade> oLabdipShades)
        {
            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            //_oPdfPTable.AddCell(this.SetCellValue("Special Instruction", 0, _nColumn, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 1, 20));
            //_oPdfPTable.CompleteRow();

            float nHeight = 105f;
            foreach (var oItem in oLabdipShades)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, nHeight));

                _oPdfPTable.AddCell(this.SetCellValue("\n Option: " + oItem.ShadeStr + "\n\n Approved: \n \t\t\t\t ☒ YES \n \t\t\t\t ☒ NO", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
                
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, nHeight));
                //_oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, nHeight));
                _oPdfPTable.CompleteRow();
            }
        }

        private void SignaturePart()
        {
             #region Signature Part
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 45f;
            _oPdfPCell.Colspan = _nColumn; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("________________\n     Prepared By ", _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("________________\nApproved By\t\t\t", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));_oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

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
                oPdfPCell.MinimumHeight = height;
            return oPdfPCell;
        }
        private PdfPCell SetCellValue(string sName, int nRowSpan, int nColumnSpan, int halign, int valign, BaseColor color, int border, float height, string [] BorderWidth)
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
                oPdfPCell.MinimumHeight = height;

            foreach (string oItem in BorderWidth)
            {
                switch (oItem.ToUpper()) 
                {
                    case "TOP"      :   oPdfPCell.BorderWidthTop = 1; break;
                    case "BOTTOM"   :   oPdfPCell.BorderWidthBottom = 1; break;
                    case "LEFT"     :   oPdfPCell.BorderWidthLeft = 1; break;
                    case "RIGHT"    :   oPdfPCell.BorderWidthRight = 1; break;
                }
            }

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
