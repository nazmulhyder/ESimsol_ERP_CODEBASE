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
    public class rptInventoryTraking
    {
        #region Declaration
        int _nTotalColumn = 1;
        float _nfixedHight = 5f;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle_UnLine;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<InventoryTraking> _oInventoryTrakings = new List<InventoryTraking>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();

        Company _oCompany = new Company();
        string _sMUnit = "";
        string _sHeaderName = "";
        string _sDateRange = "";
        int _nCount = 0;
        double _nTOpeningQty = 0;
        double _nTInQty = 0;
        double _nTOutQty = 0;
        double _nTClosingQty = 0;
        int _nValueType = 0;
         string _sUnitP = "";
         string _sValueType = "";
         int _nColumns = 0;

        #endregion
        #region WU
        public byte[] PrepareReport(List<InventoryTraking> oInventoryTrakings, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sDateRange, int nValueType)
        {
            _oInventoryTrakings = oInventoryTrakings;
            _oBusinessUnit = oBusinessUnit;
            _sHeaderName = sHeaderName;
            _sDateRange = sDateRange;
            _oCompany = oCompany;
            _nValueType = nValueType;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

           
            this.PrintValueType();
            this.PrintHeader();
            this.ReporttHeader();
            this.Print_Body_WU();
            
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintValueType()
        {
            if (_nValueType == 1)
            {
                _sUnitP = "Unit/Currency";
                _sValueType = "Qty/Value";
            }
            else if (_nValueType == 2)
            {
                _sUnitP = "Currency";
                _sValueType = "Value";
            }
            else
            {
                _sUnitP = "Unit";
                _sValueType = "Qty";
            }

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


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

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
        #endregion
        #region Report Header
        private void ReporttHeader()
        {
         
            #region Proforma Invoice Heading Print

            _oPdfPCell = new PdfPCell(new Phrase(_sHeaderName, FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_sDateRange, FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.NORMAL)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
          

            #endregion
        }
        #endregion
        #region Report Body
        private void Print_Body_WU()
        {

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 160f, 22f, 75f, 65f, 65f,75f, });
             _nTOpeningQty = 0;
             _nTInQty = 0;
             _nTOutQty = 0;
             _nTClosingQty = 0;
            _nCount = 0;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            if (_oInventoryTrakings.Count > 0)
            {
                _oInventoryTrakings = _oInventoryTrakings.OrderBy(o => o.WorkingUnitID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Store Name", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(_sUnitP, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Opening Balance", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("In "+_sValueType, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Out " + _sValueType, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Closing Balance", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);
                
                oPdfPTable.CompleteRow();

                foreach (InventoryTraking oItem in _oInventoryTrakings)
                {
                    _nCount++;


                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.WUName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.MUnit, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OpeningQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.InQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OutQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ClosingQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);
            

                    oPdfPTable.CompleteRow();
                    #endregion
                    _nTOpeningQty = _nTOpeningQty+oItem.OpeningQty;
                    _nTInQty = _nTInQty + oItem.InQty;
                    _nTOutQty = _nTOutQty + oItem.OutQty;
                    _nTClosingQty = _nTClosingQty + oItem.ClosingQty;
                }

                if (_nValueType != 1)
                {
                    #region Total
                    _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                    _oPdfPCell.Colspan = 3;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTOpeningQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTInQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTOutQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTClosingQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                }
                
            }
          
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
             _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion
        #endregion
        #region Product
        public byte[] PrepareReportProduct(List<InventoryTraking> oInventoryTrakings, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sDateRange, int nValueType)
        {
            _oInventoryTrakings = oInventoryTrakings;
            _oBusinessUnit = oBusinessUnit;
            _sHeaderName = sHeaderName;
            _sDateRange = sDateRange;
            _oCompany = oCompany;
            _nValueType = nValueType;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            //_oDocument.SetMargins(20f, 20f, 5f, 20f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(20f, 10f, 10f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
            this.PrintValueType();
            this.PrintHeader();
            this.ReporttHeader();
            this.Print_Body_Product();

            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Body
        private void Print_Body_Product()
        {

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 160f, 22f, 75f, 65f, 65f, 75f, });
            _nTOpeningQty = 0;
            _nTInQty = 0;
            _nTOutQty = 0;
            _nTClosingQty = 0;
            _nCount = 0;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            if (_oInventoryTrakings.Count > 0)
            {
                _oInventoryTrakings = _oInventoryTrakings.OrderBy(o => o.WorkingUnitID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(_sUnitP, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Opening Balance", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("In "+_nValueType, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Out "+_nValueType, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Closing Balance", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                foreach (InventoryTraking oItem in _oInventoryTrakings)
                {
                    _nCount++;


                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.MUnit, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OpeningQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.InQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OutQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ClosingQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPTable.CompleteRow();
                    #endregion
                    _nTOpeningQty = _nTOpeningQty + oItem.OpeningQty;
                    _nTInQty = _nTInQty + oItem.InQty;
                    _nTOutQty = _nTOutQty + oItem.OutQty;
                    _nTClosingQty = _nTClosingQty + oItem.ClosingQty;
                }

                if (_nValueType != 1)
                {
                    #region Total
                    _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                    _oPdfPCell.Colspan = 3;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTOpeningQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTInQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTOutQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTClosingQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                }

            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion
        #endregion
        #region Lot
        public byte[] PrepareReportLot(List<InventoryTraking> oInventoryTrakings, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sDateRange,int nValueType)
        {
            _oInventoryTrakings = oInventoryTrakings;
            _oBusinessUnit = oBusinessUnit;
            _sHeaderName = sHeaderName;
            _sDateRange = sDateRange;
            _oCompany = oCompany;
            _nValueType = nValueType;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            //_oDocument.SetMargins(20f, 20f, 5f, 20f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(20f, 10f, 10f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
            this.PrintValueType();
            this.PrintHeader();
            this.ReporttHeader();
            this.Print_Body_Lot();

            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Body
        private void Print_Body_Lot()
        {

            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 200f, 25f, 100f, 70f, 70f, 100f, });
            _nTOpeningQty = 0;
            _nTInQty = 0;
            _nTOutQty = 0;
            _nTClosingQty = 0;
            _nCount = 0;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            if (_oInventoryTrakings.Count > 0)
            {
                _oInventoryTrakings = _oInventoryTrakings.OrderBy(o => o.WorkingUnitID).ToList();

                oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(_sUnitP, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Opening Balance", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("In "+_sValueType, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Out "+_sValueType, _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Closing Balance", _oFontStyleBold));
                oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();

                foreach (InventoryTraking oItem in _oInventoryTrakings)
                {
                    _nCount++;


                    #region PrintDetail

                    oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase( oItem.LotNo, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.MUnit, _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OpeningQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.InQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.OutQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.ClosingQty), _oFontStyle));
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    //oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthBottom = 0; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPTable.CompleteRow();
                    #endregion
                    _nTOpeningQty = _nTOpeningQty + oItem.OpeningQty;
                    _nTInQty = _nTInQty + oItem.InQty;
                    _nTOutQty = _nTOutQty + oItem.OutQty;
                    _nTClosingQty = _nTClosingQty + oItem.ClosingQty;
                }


                if (_nValueType != 1)
                {
                    #region Total
                    _oPdfPCell = new PdfPCell(new Phrase("Total: ", _oFontStyleBold));
                    _oPdfPCell.Colspan = 3;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTOpeningQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTInQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTOutQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTClosingQty), _oFontStyleBold));
                    _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    //_oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion
                }

            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion
        #endregion
        private void Blank(int nFixedHeight)
        {

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 1; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nFixedHeight; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }

        #region Dyeing
        public byte[] PrepareReportDyeing(List<InventoryTraking> oInventoryTrakings, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sDateRange, int nValueType)
        {
            _oInventoryTrakings = oInventoryTrakings;
            _oBusinessUnit = oBusinessUnit;
            _sHeaderName = sHeaderName;
            _sDateRange = sDateRange;
            _oCompany = oCompany;
            _nValueType = nValueType;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion


            this.PrintValueType();
            this.PrintHeader();
            this.ReporttHeader();
            this.Print_Body_WU_Dyeing();

            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void Print_Body_WU_Dyeing()
        {

            PdfPTable oPdfPTable = new PdfPTable(16);
            oPdfPTable.SetWidths(new float[] { 15f, 80f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
            //_nTOpeningQty = 0;
            //_nTInQty = 0;
            //_nTOutQty = 0;
            //_nTClosingQty = 0;
            //_nCount = 0;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);

            if (_oInventoryTrakings.Count > 0)
            {
                _oInventoryTrakings = _oInventoryTrakings.OrderBy(o => o.WorkingUnitID).ToList();


                oPdfPTable = new PdfPTable(16);
                oPdfPTable.SetWidths(new float[] { 15f, 80f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 3, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In Qty", 0, 6, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Out Qty", 0, 5, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Store Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Opening", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "GRN In ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Adj. In ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Pro. In", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Trans. In ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SW Req. In", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Return In", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total In ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Adj. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Pro. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Trans. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SW Req. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "DC/Con", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Closing ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                foreach (InventoryTraking oItem in _oInventoryTrakings)
                {
                    _nCount++;

                    #region PrintDetail
                    oPdfPTable = new PdfPTable(16);
                    oPdfPTable.SetWidths(new float[] { 15f, 80f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, _nCount.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.WUName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.MUnit, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OpeningQty > 0) ? Global.MillionFormat(oItem.OpeningQty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InGRN > 0) ? Global.MillionFormat(oItem.InGRN) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InAdj > 0) ? Global.MillionFormat(oItem.InAdj) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InRS > 0) ? Global.MillionFormat(oItem.InRS) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InTr > 0) ? Global.MillionFormat(oItem.InTr) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InTrSW > 0) ? Global.MillionFormat(oItem.InTrSW) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InRet > 0) ? Global.MillionFormat(oItem.InRet) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.InQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutAdj > 0) ? Global.MillionFormat(oItem.OutAdj) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutRS > 0) ? Global.MillionFormat(oItem.OutRS) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutTr > 0) ? Global.MillionFormat(oItem.OutTr) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutTrSW > 0) ? Global.MillionFormat(oItem.OutTrSW) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((oItem.OutCon + oItem.OutDC) > 0) ? Global.MillionFormat(oItem.OutCon + oItem.OutDC) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.OutQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.ClosingQty > 0) ? Global.MillionFormat(oItem.ClosingQty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                    #endregion
                 
                }

               

            }

     
        }


        public byte[] PrepareReportProductDyeing(List<InventoryTraking> oInventoryTrakings, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sDateRange, int nValueType)
        {
            _oInventoryTrakings = oInventoryTrakings;
            _oBusinessUnit = oBusinessUnit;
            _sHeaderName = sHeaderName;
            _sDateRange = sDateRange;
            _oCompany = oCompany;
            _nValueType = nValueType;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion


            this.PrintValueType();
            this.PrintHeader();
            this.ReporttHeader();
            this.Print_Body_Product_Dyeing();

            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void Print_Body_Product_Dyeing()
        {

            PdfPTable oPdfPTable = new PdfPTable(16);
            oPdfPTable.SetWidths(new float[] { 15f, 80f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
            //_nTOpeningQty = 0;
            _nTInQty = 0;
            //_nTOutQty = 0;
            //_nTClosingQty = 0;
            //_nCount = 0;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);

            if (_oInventoryTrakings.Count > 0)
            {
                _oInventoryTrakings = _oInventoryTrakings.OrderBy(o => o.WorkingUnitID).ToList();


                oPdfPTable = new PdfPTable(16);
                oPdfPTable.SetWidths(new float[] { 15f, 80f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 3, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In Qty", 0, 6, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Out Qty", 0, 5, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Opening", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "GRN In ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Adj. In ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Pro. In %", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Trans. In ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SW Req. In", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Return In", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total In ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Adj. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Pro. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Trans. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SW Req. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "DC/Con Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Closing ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                foreach (InventoryTraking oItem in _oInventoryTrakings)
                {
                    _nCount++;

                    #region PrintDetail
                    oPdfPTable = new PdfPTable(16);
                    oPdfPTable.SetWidths(new float[] { 15f, 80f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, _nCount.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.MUnit, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OpeningQty>0)? Global.MillionFormat(oItem.OpeningQty):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,(oItem.InGRN>0)? Global.MillionFormat(oItem.InGRN):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InAdj > 0) ? Global.MillionFormat(oItem.InAdj) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InRS > 0) ? Global.MillionFormat(oItem.InRS) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InTr > 0) ? Global.MillionFormat(oItem.InTr) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,(oItem.InTrSW > 0) ? Global.MillionFormat(oItem.InTrSW):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InRet > 0) ? Global.MillionFormat(oItem.InRet):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.InQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutAdj > 0) ?Global.MillionFormat(oItem.OutAdj):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutRS > 0) ?Global.MillionFormat(oItem.OutRS):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,(oItem.OutTr > 0) ? Global.MillionFormat(oItem.OutTr):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutTrSW > 0) ?Global.MillionFormat(oItem.OutTrSW):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutCon + oItem.OutDC > 0) ? Global.MillionFormat(oItem.OutCon + oItem.OutDC) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.OutQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable,(oItem.ClosingQty > 0) ? Global.MillionFormat(oItem.ClosingQty):"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);


                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                    #endregion

                }

                oPdfPTable = new PdfPTable(16);
                oPdfPTable.SetWidths(new float[] { 15f, 80f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.OpeningQty);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.InGRN);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.InAdj);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.InRS);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.InTr);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => x.InTrSW);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => x.InRet);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => x.OutAdj);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => x.OutRS);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);


                _nTInQty = _oInventoryTrakings.Sum(x => x.OutTr);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => x.OutTrSW);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => (x.OutDC+x.OutCon));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);


                _nTInQty = _oInventoryTrakings.Sum(x => ( x.ClosingQty));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);




            }


        }

        #endregion

        #region Dyeing
        public byte[] PrepareReportProductWise(List<InventoryTraking> oInventoryTrakings, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sDateRange)
        {
            _oInventoryTrakings = oInventoryTrakings;
            _oBusinessUnit = oBusinessUnit;
            _sHeaderName = sHeaderName;
            _sDateRange = sDateRange;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(PageSize.LEGAL, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());//842*595
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion


            this.PrintValueType();
            this.PrintHeader();
            this.ReporttHeader();
            this.Print_Body_Product_Wise();

            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void Print_Body_Product_Wise()
        {

            PdfPTable oPdfPTable = new PdfPTable(22);
            oPdfPTable.SetWidths(new float[] { 15f, 100f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
            _nTInQty = 0;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.UNDERLINE);

            var oPCategorys = _oInventoryTrakings.GroupBy(x => new { x.PCategoryID, x.PCategoryName }, (key, grp) =>
                                  new
                                  {
                                      PCategoryName = key.PCategoryName,
                                      PCategoryID = key.PCategoryID
                                  }).ToList();
            foreach (var oItemCat in oPCategorys)
            {
                var oInventoryTrakings = _oInventoryTrakings.Where(x => x.PCategoryID == oItemCat.PCategoryID && x.PCategoryName == oItemCat.PCategoryName).ToList();
            


                oPdfPTable = new PdfPTable(22);
                oPdfPTable.SetWidths(new float[] { 15f, 100f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

             //   ESimSolItexSharp.SetCellValue(ref oPdfPTable, " ", 0, 3, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItemCat.PCategoryName, 0, 4, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In Qty", 0, 9, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Out Qty", 0, 8, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Opening", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "GRN In ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Adj. In ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Pro. In %", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Trans. In ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SW Req. In", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Return In", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "WIP In", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In Recycle", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "In Wastage", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);

                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total In ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Adj. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Pro. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Trans. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SW Req. Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "DC/Con Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "WIP ", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Recycle", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Wastage", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total Out", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Closing ", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                foreach (var oItem in oInventoryTrakings)
                {
                    _nCount++;

                    #region PrintDetail
                    oPdfPTable = new PdfPTable(22);
                    oPdfPTable.SetWidths(new float[] { 15f, 100f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, _nCount.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.MUnit, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OpeningQty > 0) ? Global.MillionFormat(oItem.OpeningQty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InGRN > 0) ? Global.MillionFormat(oItem.InGRN) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InAdj > 0) ? Global.MillionFormat(oItem.InAdj) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InRS > 0) ? Global.MillionFormat(oItem.InRS) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InTr > 0) ? Global.MillionFormat(oItem.InTr) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InTrSW > 0) ? Global.MillionFormat(oItem.InTrSW) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InRet > 0) ? Global.MillionFormat(oItem.InRet) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InWIP > 0) ? Global.MillionFormat(oItem.InRet) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InRecycle > 0) ? Global.MillionFormat(oItem.InRet) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.InWastage > 0) ? Global.MillionFormat(oItem.InRet) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.InQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutAdj > 0) ? Global.MillionFormat(oItem.OutAdj) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutRS > 0) ? Global.MillionFormat(oItem.OutRS) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutTr > 0) ? Global.MillionFormat(oItem.OutTr) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutTrSW > 0) ? Global.MillionFormat(oItem.OutTrSW) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutCon + oItem.OutDC > 0) ? Global.MillionFormat(oItem.OutCon + oItem.OutDC) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutWIP > 0) ? Global.MillionFormat(oItem.InRet) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutRecycle > 0) ? Global.MillionFormat(oItem.InRet) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.OutWastage > 0) ? Global.MillionFormat(oItem.InRet) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);


                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.OutQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (oItem.ClosingQty > 0) ? Global.MillionFormat(oItem.ClosingQty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);


                    oPdfPTable.CompleteRow();
                    ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                    #endregion

                }
            }
                oPdfPTable = new PdfPTable(22);
                oPdfPTable.SetWidths(new float[] { 15f, 100f, 20f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f, 42f });

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.OpeningQty);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.InGRN);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.InAdj);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.InRS);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.InTr);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => x.InTrSW);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => x.InRet);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);


                _nTInQty = _oInventoryTrakings.Sum(x => x.InWIP);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.InRecycle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.InWastage);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => x.OutAdj);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => x.OutRS);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);


                _nTInQty = _oInventoryTrakings.Sum(x => x.OutTr);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => x.OutTrSW);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => (x.OutDC + x.OutCon));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);


                _nTInQty = _oInventoryTrakings.Sum(x => x.OutWIP);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.OutRecycle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                _nTInQty = _oInventoryTrakings.Sum(x => x.OutWastage);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                _nTInQty = _oInventoryTrakings.Sum(x => (x.ClosingQty));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

        }


    
        
        #endregion
        #region Current Stock
        public byte[] PrepareReportCurrentStock(List<Lot> oLots, Company oCompany, BusinessUnit oBusinessUnit, string sHeaderName, string sDateRange)
        {
            _oBusinessUnit = oBusinessUnit;
            _sHeaderName = sHeaderName;
            _sDateRange = sDateRange;
            _oCompany = oCompany;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(10f, 10f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion


            this.PrintValueType();
            this.PrintHeader();
            //is.ReporttHeader();
            this.Print_Body_Product_WiseCurrentStock(oLots);

            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void Print_Body_Product_WiseCurrentStock(List<Lot> oLots)
        {

            #region Proforma Invoice Heading Print

            _oPdfPCell = new PdfPCell(new Phrase("Current Stock Report Date :" + DateTime.Now.ToString("dd MMM yyyy hh:mm tt"), FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;

            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            _nTInQty = 0;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle_UnLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE);

            var oWUs = oLots.GroupBy(x => new { x.WorkingUnitID, x.OperationUnitName }, (key, grp) =>
                                  new
                                  {
                                      WorkingUnitID = key.WorkingUnitID,
                                      OperationUnitName = key.OperationUnitName
                                  }).ToList();


            var oProducts = oLots.GroupBy(x => new { x.ProductID, x.ProductName, x.ProductCode }, (key, grp) =>
                                  new
                                  {
                                      ProductID = key.ProductID,
                                      ProductName = key.ProductName,
                                      ProductCode = key.ProductCode
                                  }).ToList();


            _nColumns = oWUs.Count + 3;
            PdfPTable oPdfPTable = new PdfPTable(_nColumns);

            float[] tablecolumns = new float[_nColumns];
            tablecolumns[0] = 25f;
            tablecolumns[1] = 100f;
            for (int i = 2; i < _nColumns; i++)
            {
                tablecolumns[i] = 60f;
            }

            oPdfPTable = new PdfPTable(_nColumns);
            oPdfPTable.SetWidths(tablecolumns);


            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Product Name", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            foreach (var oWU in oWUs)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oWU.OperationUnitName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            }
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

            foreach (var oItem in oProducts)
            {
                _nCount++;

                #region PrintDetail
                oPdfPTable = new PdfPTable(_nColumns);
                oPdfPTable.SetWidths(tablecolumns);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, _nCount.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                foreach (var oWU in oWUs)
                {
                    _nTInQty = oLots.Where(x => x.ProductID == oItem.ProductID && x.WorkingUnitID == oWU.WorkingUnitID).Sum(x => (x.Balance));
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                }
                _nTInQty = oLots.Where(x => x.ProductID == oItem.ProductID).Sum(x => (x.Balance));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                #endregion

            }

            #region Total
            oPdfPTable = new PdfPTable(_nColumns);
            oPdfPTable.SetWidths(tablecolumns);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            foreach (var oWU in oWUs)
            {
                _nTInQty = oLots.Where(x => x.WorkingUnitID == oWU.WorkingUnitID).Sum(x => (x.Balance));
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            }
            _nTInQty = oLots.Sum(x => (x.Balance));
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, (_nTInQty > 0) ? Global.MillionFormat(_nTInQty) : "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion
        }
        #endregion
    }
}
