using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Linq;

namespace ESimSol.Reports
{
    public class rptDUProductionStatus
    {

        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(13);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<DUProductionStatus> _oDUProductionStatusList = new List<DUProductionStatus>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit;
        int _nColspan = 13;
        int _nReportType = 0;
        #endregion

        #region Order Sheet
        public byte[] PrepareReport(List<DUProductionStatus> oDUProductionStatusList, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sStartEndDate, int nReportType)
        {
            _oDUProductionStatusList = oDUProductionStatusList;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;
            _nReportType = nReportType;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);

            _oDocument.SetMargins(15f, 15f, 20f, 15f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            
            _oPdfPTable.SetWidths(new float[] { 
                                                    60f,  //SL 
                                                    40f,  //InHouse
                                                    40f, //OutSide
                                                    40f,  //Sweater
                                                    40f,  //Knit                                                
                                                    47f,  //Qty                                               
                                                    47f,  //%
                                                    50f,  //Total
                                                    47f,  //InHouse
                                                    47f,  //OutSide
                                                    47f,  //Total
                                                    40f,  //%
                                                    50f,  //C/p
                                             });


            if (_nReportType == 3) // Product Wise
            {
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                _oPdfPTable.SetWidths(new float[] { 
                                                    120f,  //SL 
                                                    40f,  //InHouse
                                                    40f, //OutSide
                                                    40f,  //Sweater
                                                    40f,  //Knit                                                
                                                    47f,  //Qty                                               
                                                    47f,  //%
                                                    50f,  //Total
                                                    47f,  //InHouse
                                                    47f,  //OutSide
                                                    47f,  //Total
                                                    40f,  //%
                                                    50f,  //C/p
                                             });
            }

            _oDocument.Open();
            #endregion

            this.PrintHeader();
            this.PrintBody();
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
                _oImag.ScaleAbsolute(60f, 25f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));//_oBusinessUnit.PringReportHead
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = _nColspan; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Production Status", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Colspan = _nColspan; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body

        #region Report Body
        private void PrintBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            #region HEADER

            if (_nReportType < 2)
                this.AddCell("Date", "CENTER", 0, 2);
            if (_nReportType == 3)
                this.AddCell("Product Name", "CENTER", 0, 2);
            if (_nReportType == 4)
                this.AddCell("Machine", "CENTER", 0, 2);

            this.AddCell("Production (Kg)", "CENTER", 4, 0);
            this.AddCell("White", "CENTER", 2, 0);
            this.AddCell( "Total (Kg)", "CENTER", 0, 2);
            this.AddCell("Re-Processs", "CENTER", 4, 0);
            this.AddCell( "Remarks", "CENTER", 0, 2);
            this.AddCell("In-House", "CENTER", false);
            this.AddCell("Out-Side", "CENTER", false);
            this.AddCell("Sweater", "CENTER", false);
            this.AddCell("Knit", "CENTER", false);
            this.AddCell("Qty", "CENTER", false);
            this.AddCell("%", "CENTER", false);
            this.AddCell("In-House", "CENTER", false);
            this.AddCell("Out-Side", "CENTER", false);
            this.AddCell("Total", "CENTER", false);
            this.AddCell("%", "CENTER", false);
            _oPdfPTable.CompleteRow();

            #endregion

            int nSL = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (DUProductionStatus oItem in _oDUProductionStatusList)
            {
                nSL++;
                if (_nReportType == 1) 
                    this.AddCell(oItem.StartDateStr_MMYY, "LEFT", false);
                else if (_nReportType == 2)
                    this.AddCell(oItem.StartDateStr, "LEFT", false);
                else
                    this.AddCell(oItem.RefName, "LEFT", false);

                this.AddCell(Global.MillionFormat(oItem.QtyDyeing), "RIGHT", false);
                this.AddCell(Global.MillionFormat(oItem.QtyDyeing), "RIGHT", false);
                this.AddCell(Global.MillionFormat(oItem.QtyDyeing_Sweater), "RIGHT", false);
                this.AddCell(Global.MillionFormat(oItem.QtyDyeing_Knit), "RIGHT", false);
                this.AddCell(Global.MillionFormat(oItem.QtyDyeing_White), "RIGHT", false);
                this.AddCell(Global.MillionFormat(oItem.White_Percentage) + "%", "RIGHT", false);
                this.AddCell(Global.MillionFormat(oItem.QtyDyeing_Total), "RIGHT", false);
                this.AddCell(Global.MillionFormat(oItem.ReProcess_InHouse), "RIGHT", false);
                this.AddCell(Global.MillionFormat(oItem.ReProcess_OutSide), "RIGHT", false);
                this.AddCell(Global.MillionFormat(oItem.ReProcess_Total), "RIGHT", false);
                this.AddCell(Global.MillionFormat(oItem.ReProcess_Percentage) + "%", "RIGHT", false);
                this.AddCell(oItem.Remarks, "Left", false);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            this.AddCell("Total", "CENTER", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.QtyDyeing)), "RIGHT", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.QtyDyeing)), "RIGHT", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.QtyDyeing_Sweater)), "RIGHT", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.QtyDyeing_Knit)), "RIGHT", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.QtyDyeing_White)), "RIGHT", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.White_Percentage)) + "%", "RIGHT", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.QtyDyeing_Total)), "RIGHT", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.ReProcess_InHouse)), "RIGHT", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.ReProcess_OutSide)), "RIGHT", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.ReProcess_Total)), "RIGHT", false);
            this.AddCell(Global.MillionFormat(_oDUProductionStatusList.Sum(x => x.ReProcess_Percentage)) + "%", "RIGHT", false);
            this.AddCell("", "Left", false);
            _oPdfPTable.CompleteRow();
            #endregion

        }

        #endregion

        #region PDF HELPER
        public void AddCell(string sHeader, string sAlignment, int nColSpan, int nRowSpan)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            if (nColSpan > 0)
                _oPdfPCell.Colspan = nColSpan;
            if (nRowSpan > 0)
                _oPdfPCell.Rowspan = nRowSpan;

            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void AddCell(string sHeader, string sAlignment, bool isGray)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;//Default
            if (sAlignment == "LEFT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (sAlignment == "RIGHT")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            if (sAlignment == "CENTER")
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;

            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
        }
        public void PrintRow(string sHeader)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nColspan;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #endregion
        #endregion
    }
}