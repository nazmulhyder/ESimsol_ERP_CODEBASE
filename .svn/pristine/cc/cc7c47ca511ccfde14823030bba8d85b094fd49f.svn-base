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
    public class rptFNLabDipSubCard
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(8);
        int _nColumn = 8;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        FNLabDipDetail _oFNLabDipDetail = new FNLabDipDetail();
        FabricSCReport _oFabricSCReport = new FabricSCReport();
        #endregion

        public byte[] PrepareReport(List<FNLabDipDetail> oFNLabDipDetails, Company oCompany, BusinessUnit oBusinessUnit,   FabricSCReport oFabricSCReport , List<FNLabdipShade> oFNLabdipShades, FNLabDipDetail oFNLabDipDetail)
        {
            _oFNLabDipDetail = oFNLabDipDetail;
            _oFabricSCReport = oFabricSCReport;
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
            this.PrintEmptyRow(25);
            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBusinessUnit, oCompany, "",_nColumn);

            this.PrintHeaderNew("LAB-DIP SUBMISSION CARD", "(Factory Copy)");
            this.ReportInfo(oFabricSCReport, oFNLabDipDetails);
            this.MiddlePart(oFNLabdipShades);
            this.LastPara(oFabricSCReport);
            //this.SignaturePart();
            //_oPdfPTable.HeaderRows = 2;
            

            ESimSolPdfHelper.NewPageDeclaration(_oDocument, _oPdfPTable);
            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, oBusinessUnit, oCompany, " ", _nColumn);
            this.PrintHeaderNew("LAB-DIP SUBMISSION CARD", "(Buyer Copy)");
            this.ReportInfo(oFabricSCReport, oFNLabDipDetails);
            this.MiddlePart(oFNLabdipShades);
            this.LastPara(oFabricSCReport);
            //this.SignaturePart();
            //_oPdfPTable.HeaderRows = 2;



            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void ReportInfo(FabricSCReport oFabricSCReport, List<FNLabDipDetail> oFNLabDipDetails)
        {
            float cellHight = 18f;
            _oPdfPTable.AddCell(this.SetCellValue("", 0, _nColumn, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 15f));
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 0);

            string[] sTop = new string[] { "TOP" };
            string[] sTopLeft = new string[] { "TOP", "LEFT" };
            string[] sTopRight = new string[] { "TOP", "RIGHT" };
            string[] sTopBottom = new string[] { "TOP", "BOTTOM" };
            string[] sTopBottomLeft = new string[] { "TOP", "BOTTOM", "LEFT" };
            string[] sTopBottomRight = new string[] { "TOP", "BOTTOM", "RIGHT" };

            _oPdfPTable.AddCell(this.SetCellValue("Buyer Name", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oFabricSCReport.BuyerName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue("Lab Dip Ref", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oFNLabDipDetails[0].LabdipNo, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopRight));
            _oPdfPTable.CompleteRow();


            _oPdfPTable.AddCell(this.SetCellValue("Color Name", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oFNLabDipDetails[0].ColorName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue("Buyer Ref", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oFabricSCReport.BuyerReference, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopRight));
            _oPdfPTable.CompleteRow();

            _oPdfPTable.AddCell(this.SetCellValue("Weave Type", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oFabricSCReport.FabricWeaveName, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue("Finish Type", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTop));
            _oPdfPTable.AddCell(this.SetCellValue(oFabricSCReport.FinishTypeName, 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopRight));
            _oPdfPTable.CompleteRow();


            _oPdfPTable.AddCell(this.SetCellValue("Construction", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottomLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottom));
            _oPdfPTable.AddCell(this.SetCellValue(_oFNLabDipDetail.Construction, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottom));
            _oPdfPTable.AddCell(this.SetCellValue("Date", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottomLeft));
            _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottom));
            _oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottomRight));
            _oPdfPTable.CompleteRow();

            if (_oFNLabDipDetail.ContactPersonnelID > 0)
            {
                ContactPersonnel oContactPersonnel = new ContactPersonnel();
                oContactPersonnel = oContactPersonnel.Get(_oFNLabDipDetail.ContactPersonnelID, 0);

                _oPdfPTable.AddCell(this.SetCellValue("Concern Person", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottomLeft));
                _oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottom));
                _oPdfPTable.AddCell(this.SetCellValue(oContactPersonnel.Name, 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottom));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottomLeft));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottom));
                _oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, cellHight, sTopBottomRight));
                _oPdfPTable.CompleteRow();
            }
            

            _oPdfPTable.AddCell(this.SetCellValue("", 0, _nColumn, Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 15f));
            _oPdfPTable.CompleteRow();
        }

        private void MiddlePart(List<FNLabdipShade> oFNLabdipShades)
        {
            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            //_oPdfPTable.AddCell(this.SetCellValue("Special Instruction", 0, _nColumn, Element.ALIGN_CENTER, Element.ALIGN_TOP, BaseColor.WHITE, 1, 20));
            //_oPdfPTable.CompleteRow();

            int nHeight = 120;
            int nColorSet = (_oFNLabDipDetail.ColorSet <= 0) ? 3 : _oFNLabDipDetail.ColorSet;

            if (nColorSet <= 3) { nHeight = 145; }

            //foreach (var oItem in oFNLabdipShades)
            //for (int i = 0; i < 3; i++ )//
            for (int i = 0; i < nColorSet; i++)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
                //_oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, nHeight));

                PdfPTable oMarTable = new PdfPTable(1);
                oMarTable.SetWidths(new float[] { 100f });
                oMarTable.DefaultCell.Border = 0;

                ESimSolPdfHelper.FontStyle = _oFontStyle;
                ESimSolPdfHelper.AddCell(ref oMarTable, "\n APPROVAL ", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0, 0);
                ESimSolPdfHelper.CheckMark = new ZapfDingbatsList(111); //+ oItem.ShadeStr + "\n\n Approved: \n \t\t\t\t ☒ 
                ESimSolPdfHelper.AddCellMark(ref oMarTable, " YES", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0, 40);
                             
                ESimSolPdfHelper.AddCellMark(ref oMarTable, " NO", Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 0, 0, 100);

                ESimSolPdfHelper.AddTable(ref _oPdfPTable, oMarTable, 15, 0, 3, nHeight, false);
                //_oPdfPTable.AddCell(this.SetCellValue("\n YES \n \t\t\t\t ☒ NO", 0, 2, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));

                _oPdfPTable.AddCell(this.SetCellValue("", 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
                _oPdfPTable.AddCell(this.SetCellValue("\n Remarks:", 0, 3, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 1, nHeight));
                //_oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, nHeight));
                //_oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, nHeight));
                _oPdfPTable.CompleteRow();
            }
        }
        public void PrintHeaderNew(string str1, string str2)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            PdfPTable oMarTable = new PdfPTable(1);
            oMarTable.SetWidths(new float[] { 100f });
            oMarTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oMarTable.DefaultCell.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
            oMarTable.DefaultCell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            oMarTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            ESimSolPdfHelper.FontStyle = _oFontStyle;
            ESimSolPdfHelper.AddCell(ref oMarTable, str1, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 5, 0, 0);
            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oMarTable, 7, 0, 5, 30, false);


            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            ESimSolPdfHelper.FontStyle = _oFontStyle;
            oMarTable = new PdfPTable(1);
            
            ESimSolPdfHelper.AddCell(ref oMarTable, str2, Element.ALIGN_RIGHT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 3, 0, 0);
            oMarTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oMarTable.DefaultCell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
            oMarTable.DefaultCell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            oMarTable.DefaultCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oMarTable, 11, 0, 3, 30, false);
            _oPdfPTable.CompleteRow();

        }

        public void LastPara(FabricSCReport oFabricSCReport)
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 11f, 0); ;
            string sLightSource = "";
            sLightSource = oFabricSCReport.LightSourceName;
            if (!string.IsNullOrEmpty(oFabricSCReport.LightSourceNameTwo))
            {
                sLightSource = sLightSource + ", " + oFabricSCReport.LightSourceNameTwo;
            }
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 8, 20);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "All lab dips are matched under customer's specified light source  [ ] D-65/10', [ ] TL-84/10', [ ] TL-83/10', [ ] Inca-A/10', [ ] C.W.F/10' or Others................", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 8, 30);
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "Make sure bulk matched color standard / Approved lab dip within a tolerance of DE ___________", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 8, 0);
        
        
        
        }
        public void PrintEmptyRow(int nHeight)
        {
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont("Tahoma", 11f, 0); ;
            ESimSolPdfHelper.AddCell(ref _oPdfPTable, "", Element.ALIGN_LEFT, Element.ALIGN_MIDDLE, BaseColor.WHITE, 0, 0, 8, nHeight);
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
