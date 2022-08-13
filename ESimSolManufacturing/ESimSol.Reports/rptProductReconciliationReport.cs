using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ESimSol.Reports
{
    public class rptProductReconciliationReport
    {
        #region Declaration
        int _nTotalColumn = 1;
        float _nfixedHight = 5f;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleDetail;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        //ProductReconciliationReport _oProductReconciliationReport = new ProductReconciliationReport();
        List<ProductReconciliationReport> _oProductReconciliationReports = new List<ProductReconciliationReport>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();

        Company _oCompany = new Company();
        string _sMUnit = "";
        string _sCurrency = "";
        string _sDateRange = "";
        #endregion
        public byte[] PrepareReport_Import(List<ProductReconciliationReport> oProductReconciliationReports, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oProductReconciliationReports = oProductReconciliationReports;
            _oBusinessUnit = oBusinessUnit;
          
            _oCompany = oCompany;
            if (_oProductReconciliationReports.Count > 0)
            {
                //_sMUnit = _oProductReconciliationReportDetails[0].MUnit;
                _sMUnit = "LBS";
            }
           
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(20f, 20f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 842 });
            #endregion

            this.PrintHeader();
            this.ReporttHeader();

            this.Print_Body_Import();
            _oPdfPTable.HeaderRows = 3;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Print Header
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
                _oImag.ScaleAbsolute(70f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            //_oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
        }
        private void PrintHeader(string sReportHeader)
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 350f, 350f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(95f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.Rowspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Product Reconciliation Report", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            //_oPdfPCell = new PdfPCell(new Phrase(sDateRange, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase( _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_sDateRange, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion

        #region Report Header
        private void ReporttHeader()
        {
            string sHeaderName = "";
            #region Proforma Invoice Heading Print
            sHeaderName = "Product Reconciliation Report";

            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase( _sDateRange, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            

            #endregion
        }
        #endregion
        #region Report Body Import
        private void Print_Body_Import()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 20f, 170f,40f,40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f,40f });


            _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("StockInHand", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Goods in Trasit", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Goods Release", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);
         

            _oPdfPCell = new PdfPCell(new Phrase("Doc In CnF", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Doc Receive", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shipment Done", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Copy Doc Recvd", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C Open", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("P/I Receive", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 20f, 170f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f });
            #endregion

            int nProductID = 0; int nCount = 0; int nRowSpan = 0;
            double nTotalQty = 0;
            foreach (ProductReconciliationReport oItem in _oProductReconciliationReports)
            {

                    #region Initialize Table
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 20f, 170f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f });
                    #endregion

                    nCount++;
                    #region PrintDetail

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;

                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                    ////_oPdfPCell.FixedHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.StockInHand), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.GoodsinTrasit), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.GoodsRelease), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DocInCnF), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DocReceive), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ShipmentDone), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.InvoiceWithoutBL), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.LCOpen), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.POReceive), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Total_Import), _oFontStyle));
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }



            #region Grand Total
            #region Initialize Table
            oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 20f, 170f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Colspan = 2;
            oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);
            nTotalQty = 0;
            nTotalQty = _oProductReconciliationReports.Select(c => c.StockInHand).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Select(c => c.GoodsinTrasit).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Select(c => c.GoodsRelease).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Select(c => c.DocInCnF).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Select(c => c.DocReceive).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Select(c => c.ShipmentDone).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Select(c => c.InvoiceWithoutBL).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Select(c => c.LCOpen).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Select(c => c.POReceive).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Select(c => c.Total_Import).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
       
        #endregion
        #region Report Body
        public byte[] PrepareReportPR(List<ProductReconciliationReport> oProductReconciliationReports, Company oCompany, BusinessUnit oBusinessUnit, string sDateRange)
        {
            _oProductReconciliationReports = oProductReconciliationReports;
            _oBusinessUnit = oBusinessUnit;
            _sDateRange = sDateRange;
            _oCompany = oCompany;
            if (_oProductReconciliationReports.Count > 0)
            {
                _sMUnit = "LBS";
            }

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(10f, 10f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 842 });
            #endregion

            //this.PrintHeader();
            //this.ReporttHeader();
            this.PrintHeader("");
            this.Print_Body();
            _oPdfPTable.HeaderRows = 2;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void Print_Body()
        {
          
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(14);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  16f, 104f, 40f, 40f,40f, 40f, 40f,40f,40f,40f ,40f,40f,40f ,40f 
                                             });


            _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("StockInHand", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Pending Prod", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Pending Order(L/C Reced)", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Pending Order(PI Issue)", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Current Salable", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Goods in Trasit", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DocInCnF", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DocReceive", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ShipmentDone", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

         
            _oPdfPCell = new PdfPCell(new Phrase("L/C Open", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("P/I Receive", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Net Salable", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Initialize Table
            oPdfPTable = new PdfPTable(14);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                      16f, 104f, 40f, 40f,40f, 40f, 40f,40f,40f,40f ,40f,40f,40f ,40f 
                                             });
            #endregion

            int nProductID = 0; int nCount = 0; int nRowSpan = 0;
            double nTotalQty = 0;
            BaseColor oBaseColor = new BaseColor(Color.White);
            oBaseColor = BaseColor.WHITE;

            foreach (ProductReconciliationReport oItem in _oProductReconciliationReports)
            {
                if (oItem.ProductID<=0)
                {
                    oBaseColor = BaseColor.LIGHT_GRAY;
                    _oFontStyleDetail = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.ITALIC | iTextSharp.text.Font.BOLD);
                }
                else
                {
                    nCount++;
                    oBaseColor = BaseColor.WHITE;
                    _oFontStyleDetail = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                }

                #region Initialize Table
                oPdfPTable = new PdfPTable(14);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                      16f, 104f, 40f, 40f,40f, 40f, 40f,40f,40f,40f ,40f,40f,40f ,40f 
                                             });
                #endregion

             
                #region PrintDetail
                if (oItem.ProductID <= 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleDetail));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                }
               
                _oPdfPCell.BackgroundColor = oBaseColor;

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);
                if (oItem.ProductID <= 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CategoryName+":"+oItem.ProductName, _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.Colspan = 13;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("[" + oItem.ProductCode + "]" + oItem.ProductName, _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                    ////_oPdfPCell.FixedHeight = 40f;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.StockInHand), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ProductionYetTo), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.LCReceiveDONoReceive), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.PIIssueLCnDONotReceive), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.CurrentSalable), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.GoodsinTrasit + oItem.GoodsRelease), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DocInCnF), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.DocReceive), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ShipmentDone), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ShipmentInTransit), _oFontStyleDetail)); // LCOpen+InvoiceWithOutBL
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.POReceive), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.NetSalable), _oFontStyleDetail));
                    _oPdfPCell.BackgroundColor = oBaseColor;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                }
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }



            #region Grand Total
            #region Initialize Table
            oPdfPTable = new PdfPTable(14);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  20f, 100f, 40f, 40f,40f, 40f, 40f,40f,40f,40f ,40f,40f,40f ,40f 
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Colspan = 2;
            oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);
            nTotalQty = 0;
            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.StockInHand);
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

        

            nTotalQty = 0;
            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.ProductionYetTo); //_oProductReconciliationReports.Select(c => c.ProductionYetTo).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = 0;
            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.LCReceiveDONoReceive);// _oProductReconciliationReports.Select(c => c.LCReceiveDONoReceive).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = 0;
            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.PIIssueLCnDONotReceive); //_oProductReconciliationReports.Select(c => c.PIIssueLCnDONotReceive).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = 0;
            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.CurrentSalable); //_oProductReconciliationReports.Select(c => c.CurrentSalable).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.GoodsinTrasit); //_oProductReconciliationReports.Select(c => (c.GoodsRelease + c.GoodsinTrasit)).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.DocInCnF); //_oProductReconciliationReports.Select(c => c.DocInCnF).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.DocReceive); //_oProductReconciliationReports.Select(c => c.DocReceive).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.ShipmentDone); //_oProductReconciliationReports.Select(c => c.ShipmentDone).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.ShipmentInTransit); //_oProductReconciliationReports.Select(c => c.ShipmentInTransit).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.POReceive); //_oProductReconciliationReports.Select(c => c.POReceive).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotalQty = _oProductReconciliationReports.Where(x => (x.ProductID > 0)).ToList().Sum(x => x.NetSalable); // _oProductReconciliationReports.Select(c => c.NetSalable).Sum();
            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalQty), _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion

        #region Report For all
        public byte[] PrepareReportPRDetail(ProductReconciliationReport oPRR, List<ProductReconciliationReportDetail> oPRDs_StockInHand, List<ProductReconciliationReportDetail> oPRDs_ProYetToWithoutLC, List<ProductReconciliationReportDetail> oPRDs_ProYetToWithLC, List<ProductReconciliationReportDetail> oPRDs_YetToOrderLC, List<ProductReconciliationReportDetail> oPRDs_YetToOrderPI, List<ProductReconciliationReportDetail> oPRDs_DocInCnF, List<ProductReconciliationReportDetail> oPRDs_DocReceived, List<ProductReconciliationReportDetail> oPRDs_ShipmentDone, List<ProductReconciliationReportDetail> oPRDs_InvoiceWithOutShipment, List<ProductReconciliationReportDetail> oPRDs_LCOpen, List<ProductReconciliationReportDetail> oPRDs_PIReceived, List<ProductReconciliationReportDetail> oPRDs_GoodsRelease , List<ProductReconciliationReportDetail> oPRDs_GoodsinTrasit, Company oCompany, BusinessUnit oBusinessUnit)
        {
            //_oProductReconciliationReports = oProductReconciliationReports;
            _oBusinessUnit = oBusinessUnit;

            _oCompany = oCompany;
            _sMUnit = "LBS";
            _sCurrency = "$";

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(10f, 10f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 802 });
            #endregion
            this.PrintHeader();
            this.ReporttHeader(oPRR);
            this.Report_StockInHand(oPRDs_StockInHand);
            //this.Report_SubTotal("Stock In Hand:", oPRR.StockInHand);
            if (oPRDs_ProYetToWithoutLC.Count > 0)
            {
                this.Report_ProYetToWithoutLC(oPRDs_ProYetToWithoutLC, "Dyeing Order Issue Production Pending(Without L/C)");
            }
            if (oPRDs_ProYetToWithLC.Count > 0)
            {
                this.Report_ProYetToWithoutLC(oPRDs_ProYetToWithLC, "Dyeing Order Issue Production Pending(With L/C)");
            }
            //this.Report_SubTotal("Production Yet To:", oPRR.ProductionYetTo);
            if (oPRDs_YetToOrderLC.Count > 0)
            {
                this.Report_ProYetOrder(oPRDs_YetToOrderLC, "Production Order Pending(L/C)");
            }
            //this.Report_SubTotal("Order Yet To(L/C):", oPRR.LCReceiveDONoReceive);
            if (oPRDs_YetToOrderPI.Count > 0)
            {
                this.Report_ProYetOrder(oPRDs_YetToOrderPI, "Production Order Pending(P/I)");
            }
            //this.Report_SubTotal("Order Yet To(P/I):", oPRR.PIIssueLCnDONotReceive);

            if (oPRDs_PIReceived.Count > 0)
            {
                this.Report_ImportPIRecd(oPRDs_PIReceived, "Import P/I Receive");
            }
            if (oPRDs_DocInCnF.Count > 0)
            {
                this.Report_ShipmentDone(oPRDs_DocInCnF, "Doc In CnF");
            }
            if (oPRDs_DocReceived.Count > 0)
            {
                this.Report_ShipmentDone(oPRDs_DocReceived, "Doc Received");
            }
            //oPRDs_InvoiceWithOutShipment
            if (oPRDs_ShipmentDone.Count > 0)
            {
                this.Report_ShipmentDone(oPRDs_ShipmentDone, "Shipment Done");
            }
            if (oPRDs_InvoiceWithOutShipment.Count > 0)
            {
                this.Report_DocInCnF(oPRDs_InvoiceWithOutShipment, "Copy Doc Receive");
            }
            if (oPRDs_LCOpen.Count > 0)
            {
                this.Report_LCOpen(oPRDs_LCOpen, " Import L/C Open");
            }
            if (oPRDs_GoodsRelease.Count > 0)
            {
                this.Report_GoodsRelease(oPRDs_GoodsRelease, "Goods Release");
            }
            if (oPRDs_GoodsinTrasit.Count > 0)
            {
                this.Report_GoodsinTrasit(oPRDs_GoodsinTrasit, "Goods in Trasit");
            }
            this.Report_SubTotal(oPRR);
            _oPdfPTable.HeaderRows = 4;

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void ReporttHeader(ProductReconciliationReport oPRR)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            string sHeaderName = "";
            #region Proforma Invoice Heading Print
            sHeaderName = "Yarn:" + "["+oPRR.ProductCode + "]" + oPRR.ProductName;

            _oPdfPCell = new PdfPCell(new Phrase(sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPCell = new PdfPCell(new Phrase("Date:" + DateTime.Now.ToString("dd MMM yyyy: HH:mm"), FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.BOLD)));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("Date Range:" + oPRR.StartDate.ToString("dd MMM yyyy") + " to " + oPRR.EndDate.ToString("dd MMM yyyy") + "          Print Date" + DateTime.Now.ToString("dd MMM yyyy: HH:mm"), FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        private void Report_StockInHand(List<ProductReconciliationReportDetail> oPRDs_StockInHand)
        {

            double nTotal = 0;
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  20f, 80f, 80f,130f,40f,70f,60f, 80f, 70f, 50f
                                             });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 10;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Stock In Hand", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 10;
            oPdfPTable.AddCell(_oPdfPCell);

            //nTotal = oPRDs_StockInHand.Select(c => c.Qty).Sum();
            //_oPdfPCell = new PdfPCell(new Phrase("Qty:"+Global.MillionFormat(nTotal)+""+_sMUnit, _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);
            //nTotal = oPRDs_StockInHand.Select(c => (c.Qty*c.Rate)).Sum();
            //_oPdfPCell = new PdfPCell(new Phrase("Value:" + _sCurrency + Global.MillionFormat(nTotal), _oFontStyleBold));
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GRN No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("GRN Date", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Store Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase("UP", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

         

            int nProductID = 0; int nCount = 0; 
           
            foreach (ProductReconciliationReportDetail oItem in oPRDs_StockInHand)
            {
                #region Initialize Table
                 oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                   20f, 80f, 80f,130f,40f,70f,60f, 80f, 70f, 50f
                                             });
                #endregion
                nCount++;
                #region PrintDetail

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.GRNNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.IssueDateSt, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.WUName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }


             oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                 20f, 80f, 80f,130f,40f,70f,60f, 80f, 70f, 50f
                                             });

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 8;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotal = oPRDs_StockInHand.Select(c => c.Qty).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        private void Report_ProYetToWithoutLC(List<ProductReconciliationReportDetail> oPRDs_ProYetToWithoutLC, string sTemp)
        {

            double nTotal = 0;
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                   20f, 65, 15f,120f,120f, 70f, 80f, 50f,50f,50f
                                             });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 10;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

         

            _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 10;
            oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            
            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.Colspan=3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Buyering House", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OrderQty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Prod Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Pending Pro", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

         


            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

          

            int nCount = 0;
           
            foreach (ProductReconciliationReportDetail oItem in oPRDs_ProYetToWithoutLC)
            {

                #region Initialize Table
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                      20f, 65, 15f,120f,120f, 70f, 80f, 50f,50f,50f
                                             });
                #endregion

                nCount++;
                #region PrintDetail

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                //////_oPdfPCell.FixedHeight = 40f;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OrderQty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty_Prod), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.YetToProduction), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                      20f, 65, 15f,120f,120f, 70f, 80f, 50f,50f,50f
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 8;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotal = oPRDs_ProYetToWithoutLC.Select(c => c.YetToProduction).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        private void Report_ProYetOrder(List<ProductReconciliationReportDetail> oPRDs_ProYetOrder, string sTemp)
        {

            double nTotal = 0;
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  20f, 60f, 20f,120f,10f, 140f, 80f, 50f,50f,50f
                                             });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 10;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 7;
            oPdfPTable.AddCell(_oPdfPCell);

   

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Buyering House", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("P/I Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Order Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

          
          

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

       
            int nProductID = 0; int nCount = 0;

            foreach (ProductReconciliationReportDetail oItem in oPRDs_ProYetOrder)
            {

                #region Initialize Table
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    20f, 60f, 20f,120f,10f, 140f, 80f, 50f,50f,50f
                                             });
                #endregion

                nCount++;
                #region PrintDetail

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                //////_oPdfPCell.FixedHeight = 40f;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceNo, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                //////_oPdfPCell.FixedHeight = 40f;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.PIQty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OrderQty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Rate), _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Value), _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

             
                oPdfPTable.CompleteRow();
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    20f, 60f, 20f,120f,10f, 140f, 80f, 50f,50f,50f
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 8;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotal = oPRDs_ProYetOrder.Select(c => c.Qty).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


        }
        private void Report_ShipmentDone(List<ProductReconciliationReportDetail> oPRDs_ShipmentDone, string sTemp)
        {

            double nTotal = 0;
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  20f, 100f, 100f,80f, 50f,20f,50f,55f,70f,70f
                                             });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 10;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 6;
            oPdfPTable.AddCell(_oPdfPCell);

     

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BL No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BL Date", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);
       

        
         


            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        

            int nProductID = 0; int nCount = 0;

            foreach (ProductReconciliationReportDetail oItem in oPRDs_ShipmentDone)
            {

                #region Initialize Table
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                        20f, 100f, 100f,80f, 50f,20f,50f,55f,70f,70f
                                             });
                #endregion

                nCount++;
                #region PrintDetail

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.Colspan = 4;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BLNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BLDateSt, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Rate), _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Value), _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                       20f, 100f, 100f,80f, 50f,20f,50f,55f,70f,70f
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 8;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotal = oPRDs_ShipmentDone.Select(c => c.Qty).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }

        private void Report_DocInCnF(List<ProductReconciliationReportDetail> oPRDs_DocInCnF, string sTemp)
        {

            double nTotal = 0;
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  20f, 100f, 100f,80f, 50f,20f,100f,55f,20f,70f
                                             });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 10;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 6;
            oPdfPTable.AddCell(_oPdfPCell);



            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Agent Name", _oFontStyleBold));
            //_oPdfPCell.Colspan = 3;
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase("UP", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);




            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion



            int nProductID = 0; int nCount = 0;

            foreach (ProductReconciliationReportDetail oItem in oPRDs_DocInCnF)
            {

                #region Initialize Table
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                       20f, 100f, 100f,80f, 50f,20f,100f,55f,20f,70f
                                             });
                #endregion

                nCount++;
                #region PrintDetail

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.Colspan = 6;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = 3;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);




                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Rate), _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Value), _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                      20f, 100f, 100f,80f, 50f,20f,100f,55f,20f,70f
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 8;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotal = oPRDs_DocInCnF.Select(c => c.Qty).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        private void Report_LCOpen(List<ProductReconciliationReportDetail> oPRDs_LCOpen, string sTemp)
        {

            double nTotal = 0;
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  20f, 93f, 93f,100f,30f, 45f, 58f, 52f,52f,50f
                                             });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 11;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 10;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();




            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shipment DT", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Invoice Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

       

            int nProductID = 0; int nCount = 0;

            foreach (ProductReconciliationReportDetail oItem in oPRDs_LCOpen)
            {

                #region Initialize Table
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                     20f, 93f, 93f,100f,30f, 45f, 58f, 52f,52f,50f
                                             });
                #endregion

                nCount++;
                #region PrintDetail

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ShipmentDateSt, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);
              

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.PIQty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.POQty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                       20f, 93f, 93f,100f,30f, 45f, 58f, 52f,52f,50f
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 8;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotal = oPRDs_LCOpen.Select(c => c.Qty).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        private void Report_ImportPIRecd(List<ProductReconciliationReportDetail> oPRDs_PIrecd, string sTemp)
        {

            double nTotal = 0;
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(8);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  20f, 100f, 100f,80f, 50f,20f,100f,80f
                                             });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 8;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 10;
            oPdfPTable.AddCell(_oPdfPCell);

            //nTotal = oPRDs_PIrecd.Select(c => c.Qty).Sum();
            //_oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);
            //nTotal = oPRDs_PIrecd.Select(c => (c.Qty * c.Rate)).Sum();
            //_oPdfPCell = new PdfPCell(new Phrase("Value:" + _sCurrency + Global.MillionFormat(nTotal), _oFontStyleBold));
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Agent Name", _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


         



            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion



            int nProductID = 0; int nCount = 0;

            foreach (ProductReconciliationReportDetail oItem in oPRDs_PIrecd)
            {

                #region Initialize Table
                oPdfPTable = new PdfPTable(8);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                       20f, 100f, 100f,80f, 50f,20f,100f,80f
                                             });
                #endregion

                nCount++;
                #region PrintDetail

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceNo, _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                ////_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                //////_oPdfPCell.FixedHeight = 40f;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);




                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Rate), _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Value), _oFontStyle));
                //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //_oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(8);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                      20f, 100f, 100f,80f, 50f,20f,100f,80f
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 7;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotal = oPRDs_PIrecd.Select(c => c.Qty).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
           
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void Report_GoodsRelease(List<ProductReconciliationReportDetail> oPRDs_GoodsRelease, string sTemp)
        {

            double nTotal = 0;
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  20f, 100f, 100f,80f, 30f,20f,10f,30f,60f,60f,60f
                                             });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 11;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 11;
            oPdfPTable.AddCell(_oPdfPCell);

    

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Invoice Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase("UP", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);




            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            int nProductID = 0; int nCount = 0;

            foreach (ProductReconciliationReportDetail oItem in oPRDs_GoodsRelease)
            {

                #region Initialize Table
                oPdfPTable = new PdfPTable(11);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                   20f, 100f, 100f,80f, 30f,20f,10f,30f,60f,60f,60f
                                             });
                #endregion

                nCount++;
                #region PrintDetail

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;

                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.Colspan = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.POQty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OrderQty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(11);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                       20f, 100f, 100f,80f, 30f,20f,10f,30f,60f,60f,60f
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 9;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotal = oPRDs_GoodsRelease.Select(c => c.Qty).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }

        private void Report_GoodsinTrasit(List<ProductReconciliationReportDetail> oPRDs_GoodsinTrasit, string sTemp)
        {

            double nTotal = 0;
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                  20f, 100f, 100f,80f, 20f,20f,100f,50f,80f,60f
                                             });
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 10;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase(sTemp, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 6;
            oPdfPTable.AddCell(_oPdfPCell);

            //nTotal = oPRDs_GoodsRelease.Select(c => c.Qty).Sum();
            //_oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);
            //nTotal = oPRDs_GoodsRelease.Select(c => (c.Qty * c.Rate)).Sum();
            //_oPdfPCell = new PdfPCell(new Phrase("Value:" + _sCurrency + Global.MillionFormat(nTotal), _oFontStyleBold));
            //_oPdfPCell.Colspan = 2;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Challan No", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTable.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase("UP", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.GRAY;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            int nProductID = 0; int nCount = 0;

            foreach (ProductReconciliationReportDetail oItem in oPRDs_GoodsinTrasit)
            {
                #region Initialize Table
                oPdfPTable = new PdfPTable(10);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                        20f, 100f, 100f,80f, 20f,20f,100f,50f,80f,60f
                                             });
                #endregion

                nCount++;
                #region PrintDetail

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.InvoiceNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.Colspan = 5;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.GRNNo, _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0;
                ////_oPdfPCell.FixedHeight = 40f;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                oPdfPTable.AddCell(_oPdfPCell);


                oPdfPTable.CompleteRow();
                #endregion

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

            #region Initialize Table
            oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                      20f, 100f, 100f,80f, 20f,20f,100f,50f,80f,60f
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 8;
            oPdfPTable.AddCell(_oPdfPCell);

            nTotal = oPRDs_GoodsinTrasit.Select(c => c.Qty).Sum();
            _oPdfPCell = new PdfPCell(new Phrase("Qty:" + Global.MillionFormat(nTotal) + "" + _sMUnit, _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        private void Report_SubTotal(ProductReconciliationReport oPRR)
        {
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                 100f, 100f, 80f, 50f,100f,80f,100f
                                             });

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 15f;
            _oPdfPCell.Colspan = 7;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

             oPdfPTable = new PdfPTable(7);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                 40f, 150f, 80f, 40f,130f,100f,40f
                                             });

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 10f;
            _oPdfPCell.Colspan = 7;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Summary", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 7;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(oPRR.ProductName, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 7;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Stock In Hand:", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.StockInHand) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Import PI Receive:", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.POReceive) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

        
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Order Issue but Production Pending:", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.ProductionYetTo) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Import L/C Open:", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.LCOpen + oPRR.InvoiceWithoutBL) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Production Order Pending(L/C):", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.LCReceiveDONoReceive) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shipment Done(BL) Qty:", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.ShipmentDone + oPRR.DocReceive) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Production Order Pending(P/I):", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.PIIssueLCnDONotReceive) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Doc In CnF:", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.DocInCnF ) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            ///-------------------
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Goods In Trasit:", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.GoodsRelease + oPRR.GoodsinTrasit) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            ///-------------------


            ////
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Balance:", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.CurrentSalable) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Pipeline Qty:", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oPRR.LCOpen + oPRR.InvoiceWithoutBL + oPRR.ShipmentDone + oPRR.DocReceive + oPRR.DocInCnF + oPRR.GoodsRelease + oPRR.GoodsinTrasit) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            ////
          


            _oPdfPCell = new PdfPCell(new Phrase("Ultimate Balance:", _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.Colspan = 4;
            oPdfPTable.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //_oPdfPCell.Border = 0;
            //oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" "+Global.MillionFormat(oPRR.NetSalable) + " " + _sMUnit, _oFontStyleBold));
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Colspan = 3;
            oPdfPTable.AddCell(_oPdfPCell);


            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            ////_oPdfPCell.Border = 0;
            //oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion
        private void Blank(int nFixedHeight)
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

    }
}
