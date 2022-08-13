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
    public class rptDUSoftWindingReport
    {
        #region Declaration
        Document _oDocument;
        PdfWriter _oWriter;
        iTextSharp.text.Font _oFontStyle;

        int _nColspan = 13;
        PdfPTable _oPdfPTable = new PdfPTable(13);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<DUSoftWinding> _oDUSoftWindingList = new List<DUSoftWinding>();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit;
        #endregion

        #region Order Sheet
        public byte[] PrepareReport(List<DUSoftWinding> oDUSoftWindingList, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sStartEndDate)
        {
            _oDUSoftWindingList = oDUSoftWindingList;
            _oBusinessUnit = oBusinessUnit;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(30f, 30f, 45f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            #region ESimSolFooter
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            #endregion

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    50f,  //OrderDate
                                                    45f,  //OrderNo
                                                    100f, //Buyer                                          
                                                    120f, //Product   
                                                    50f,  //LotNo                                                  
                                                    30f,  //MUnit    
                                              
                                                    40f,  //OrderQty
                                                    40f,  //SRS
                                                    40f,  //SRM
                                                    40f,  //Rcv
                                                    40f,  //Issue
                                                    40f,  //SW
                                             });
            #endregion
            this.PrintHeader(sHeaderName);
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #endregion

        #region Report Header
        private void PrintHeader(string sHeaderName)
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
            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.Colspan = _nColspan; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintRow("");
            this.PrintRow("");
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            #region HEADER
            this.AddCell("SL#", "CENTER");
            this.AddCell("Order Date", "CENTER");
            this.AddCell("Order No", "CENTER");
            this.AddCell("Buyer", "CENTER");
            this.AddCell("Yarn Count", "CENTER");
            this.AddCell("Lot No", "CENTER");
            this.AddCell("Unit", "CENTER");

            this.AddCell("Order Qty", "CENTER");
            this.AddCell("SRS Qty", "CENTER");
            this.AddCell("SRM Qty", "CENTER");
            this.AddCell("Rcv. Qty", "CENTER");
            this.AddCell("Issue Qty", "CENTER");
            this.AddCell("Balance", "CENTER");
            _oPdfPTable.CompleteRow();

            #endregion

            int nSL = 0;
            #region Data
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            foreach (DUSoftWinding oItem in _oDUSoftWindingList)
            {
                nSL++;
                this.AddCell(nSL.ToString(), "RIGHT");
                this.AddCell(oItem.OrderDateST, "CENTER");
                this.AddCell(oItem.DyeingOrderNo, "LEFT");
                this.AddCell(oItem.ContractorName, "LEFT");
                this.AddCell(oItem.ProductName, "LEFT");
                this.AddCell(oItem.LotNo, "LEFT");
                this.AddCell(oItem.MUnit, "LEFT");

                this.AddCell(Global.MillionFormat(oItem.Qty_Order), "RIGHT");
                this.AddCell(Global.MillionFormat(oItem.QtySRS), "RIGHT");
                this.AddCell(Global.MillionFormat(oItem.QtySRM), "RIGHT");
                this.AddCell(Global.MillionFormat(oItem.Qty_Req), "RIGHT");
                this.AddCell(Global.MillionFormat(oItem.Qty_RSOut), "RIGHT");
                this.AddCell(Global.MillionFormat(oItem.Qty_SW), "RIGHT");
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region Total
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            this.AddCell("Total", "RIGHT",7,0);

            var oResult = _oDUSoftWindingList.GroupBy(x => new { x.DyeingOrderID, x.ProductID }, (key, grp)  =>  new {
                DyeingOrderID = key.DyeingOrderID,
                ProductID = key.ProductID,
                Qty = grp.First().Qty_Order
            });

            this.AddCell(Global.MillionFormat(oResult.Sum(x => x.Qty)), "RIGHT");
            this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.QtySRS)), "RIGHT");
            this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.QtySRM)), "RIGHT");
            this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.Qty_Req)), "RIGHT");
            this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.Qty_RSOut)), "RIGHT");
            this.AddCell(Global.MillionFormat(_oDUSoftWindingList.Sum(x => x.Qty_SW)), "RIGHT");
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
        public void AddCell(string sHeader, string sAlignment)
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
    }
}