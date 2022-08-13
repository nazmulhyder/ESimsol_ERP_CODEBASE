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
using ESimSol.Reports;

namespace ESimSol.BusinessObjects
{
    public class rptGRNRegisters
    {
        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        iTextSharp.text.Font _oFontStyleBoldUnderLine;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<GRNRegister> _oGRNRegisters = new List<GRNRegister>();
        Company _oCompany = new Company();
        bool _bIsRateView = false;
        #endregion

        public byte[] PrepareReport(List<GRNRegister> oGRNRegisters, Company oCompany, EnumReportLayout eReportLayout, string sDateRange, bool bIsRateView)
        {
            _oGRNRegisters = oGRNRegisters;
            _oCompany = oCompany;
            _bIsRateView = bIsRateView;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(20f, 20f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 802 });
            #endregion

            #region Report Body & Header
            if (eReportLayout == EnumReportLayout.DateWiseDetails)
            {
                this.PrintHeader("Date Wise GRN Register(Details)", sDateRange);
                this.PrintBodyDateWiseDetails();
                _oPdfPTable.HeaderRows = 3;
            }

            else if (eReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                this.PrintHeader("Party Wise GRN Register(Details)", sDateRange);
                this.PrintBodyPartyWiseDetails();
                _oPdfPTable.HeaderRows = 3;
            }

            else if (eReportLayout == EnumReportLayout.ProductWise)
            {
                this.PrintHeader("Product Wise GRN Register(Details)", sDateRange);
                this.PrintBodyProductWiseDetails();
                _oPdfPTable.HeaderRows = 3;
            }

            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public PdfPTable PrepareExcel(List<GRNRegister> oGRNRegisters, Company oCompany, EnumReportLayout eReportLayout, string sDateRange)
        {
            _oGRNRegisters = oGRNRegisters;
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(20f, 20f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 802 });
            #endregion

            #region Report Body & Header
            if (eReportLayout == EnumReportLayout.DateWiseDetails)
            {
                this.PrintHeader("Date Wise GRN Register(Details)", sDateRange);
                this.PrintBodyDateWiseDetails();
                _oPdfPTable.HeaderRows = 3;
            }

            else if (eReportLayout == EnumReportLayout.PartyWiseDetails)
            {
                this.PrintHeader("Party Wise GRN Register(Details)", sDateRange);
                this.PrintBodyPartyWiseDetails();
                _oPdfPTable.HeaderRows = 3;
            }

            else if (eReportLayout == EnumReportLayout.ProductWise)
            {
                this.PrintHeader("Product Wise GRN Register(Details)", sDateRange);
                this.PrintBodyProductWiseDetails();
                _oPdfPTable.HeaderRows = 3;
            }

            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oPdfPTable;
        }

        #region Report Header
        private void PrintHeader(string sReportHeader, string sDateRange)
        {
            #region Company & Report Header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 450f, 250f });

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
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase(sReportHeader, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Address & Date Range
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(sDateRange, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Company Phone Number
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBodyProductWiseDetails()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            _oGRNRegisters = _oGRNRegisters.OrderBy(x => x.ProductID).ThenBy(x => x.GRNID).ToList();
            
            

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(13);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f,    //SL 
                                                    130f,   //GRN No //GLDate
                                                    50f,    //GRN Status
                                                    100f,   //SupplierName
                                                    85f,    //ApprovedByName
                                                    40f,    //LCNo
                                                    70f,    //GRNDate
                                                    40f,    //M Unit
                                                    40f,    //REf qty
                                                    40f,    //Qty
                                                    40f,    //Extra qt6y
                                                    45f,    //Rate
                                                    60f,    //Amount
                                             });
            #endregion

            int nGRNID = 0; int dProductID = 0; DateTime dGRNDate = DateTime.MinValue; int nCount = 0; int nRowSpan = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nTotalQty = 0, nPartyWiseQty = 0, nPartyWiseAmount = 0, nGrandTotalAmount = 0, nGrandTotalQty = 0;
            foreach (GRNRegister oItem in _oGRNRegisters)
            {
              
                if (dProductID != oItem.ProductID)
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region SubTotal
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        if (_bIsRateView) { _oPdfPCell.Colspan = 5; } else { _oPdfPCell.Colspan = 7; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        if (_bIsRateView)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Party Wise Total
                        _oPdfPCell = new PdfPCell(new Phrase("Product Wise Total :", _oFontStyleBold));
                        if (_bIsRateView) { _oPdfPCell.Colspan = 9; } else { _oPdfPCell.Colspan = 11; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nPartyWiseQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        if (_bIsRateView)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nPartyWiseAmount), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                        nGRNID = 0;
                        nTotalQty = 0; nTotalAmount = 0; nPartyWiseQty = 0; nPartyWiseAmount = 0;
                    }

                    #region Header

                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(13);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    130f,  //GRN No //GLDate
                                                    50f,  //GRN Status
                                                    100f,  //GRN Date
                                                    85f,  //ApprovedByName
                                                    40f,   //Code
                                                    70f,  //Product Name
                                                    40f,   //M Unit
                                                    40f,  //REf qty
                                                    40f,   //Qty
                                                    40f,  //Extra qt6y
                                                    45f,   //Rate
                                                    60f,   //Amount
                                             });
                    #endregion

                    #region Date Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Product : " + oItem.ProductName + "[" + oItem .ProductCode+ "]", _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 13; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN No/GL Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN Status/ Store Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Approved By ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN Date", _oFontStyleBold));
                    if (!_bIsRateView) { _oPdfPCell.Colspan = 3; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Ref Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Extra Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    if (_bIsRateView)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(13);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    130f,  //GRN No //GLDate
                                                    50f,  //GRN Status
                                                    100f,  //GRN Date
                                                    85f,  //ApprovedByName
                                                    40f,   //Code
                                                    70f,  //Product Name
                                                    40f,   //M Unit
                                                    40f,  //REf qty
                                                    40f,   //Qty
                                                    40f,  //Extra qt6y
                                                    45f,   //Rate
                                                    60f,   //Amount
                                             });
                    #endregion

                    #endregion

                    nCount = 0;
                }


                if (nGRNID != oItem.GRNID)
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region SubTotal
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        if (_bIsRateView) { _oPdfPCell.Colspan = 5; } else { _oPdfPCell.Colspan = 7; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        if (_bIsRateView)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalQty = 0; nTotalAmount = 0;
                    }

                    #region Initialize Table
                    oPdfPTable = new PdfPTable(13);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    130f,  //GRN No //GLDate
                                                    50f,  //GRN Status
                                                    100f,  //GRN Date
                                                    85f,  //ApprovedByName
                                                    40f,   //Code
                                                    70f,  //Product Name
                                                    40f,   //M Unit
                                                    40f,  //REf qty
                                                    40f,   //Qty
                                                    40f,  //Extra qt6y
                                                    45f,   //Rate
                                                    60f,   //Amount
                                             });
                    #endregion

                    nCount++;
                    nRowSpan = _oGRNRegisters.Where(GRNR => GRNR.GRNID == oItem.GRNID && GRNR.ProductID == oItem.ProductID).ToList().Count + 1;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.GRNNo + "/n" + oItem.GLDateSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.GRNStatusSt + Environment.NewLine + oItem.StoreName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SupplierName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
                _oPdfPCell = new PdfPCell(new Phrase(oItem.ApprovedByName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.GRNDateSt, _oFontStyle));
                if (!_bIsRateView) { _oPdfPCell.Colspan = 3; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.RefQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.ReceivedQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExtraQty < 0 ? "-" : Global.MillionFormatActualDigit(oItem.ExtraQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (_bIsRateView)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CurrencySymbol + " " + oItem.UnitPriceSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                oPdfPTable.CompleteRow();

                nGRNID = oItem.GRNID;
                dProductID = oItem.ProductID;
                sCurrencySymbol = oItem.CurrencySymbol;
                nTotalQty = nTotalQty + oItem.ReceivedQty;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nPartyWiseQty = nPartyWiseQty + oItem.ReceivedQty;
                nPartyWiseAmount = nPartyWiseAmount + oItem.Amount;
                nGrandTotalQty = nGrandTotalQty + oItem.ReceivedQty;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region SubTotal
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            if (_bIsRateView) { _oPdfPCell.Colspan = 5; } else { _oPdfPCell.Colspan = 7; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (_bIsRateView)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Date Wise Total
            _oPdfPCell = new PdfPCell(new Phrase("Product Wise Total :", _oFontStyleBold));
            if (_bIsRateView) { _oPdfPCell.Colspan = 9; } else { _oPdfPCell.Colspan = 11; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nPartyWiseQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_bIsRateView)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nPartyWiseAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            if (_bIsRateView) { _oPdfPCell.Colspan = 9; } else { _oPdfPCell.Colspan = 11; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_bIsRateView)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodyDateWiseDetails()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(15);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f,    //SL 
                                                    65f,    //GRN No/GLDate
                                                    50f,    //GRN Status
                                                    80f,    //Supplier Name
                                                   65f,    //ApprovedByName
                                                    35f,    //Code
                                                   100f,    //Product Name
                                                    45f,    //LotNo
                                                    55f,    //ColorName
                                                    35f,    //M Unit
                                                    50f,    //Ref qty
                                                    40f,    //Qty
                                                    40f,    //Extra Qty
                                                    40f,    //Rate
                                                    50f,    //Amount
                                             });
            #endregion

            int nGRNID = 0; DateTime dGRNDate = DateTime.MinValue; int nCount = 0; int nRowSpan = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nTotalQty = 0, nDateWiseQty = 0, nDateWiseAmount = 0, nGrandTotalAmount = 0, nGrandTotalQty = 0;
            foreach (GRNRegister oItem in _oGRNRegisters)
            {
                if (dGRNDate.ToString("dd MMM yyyy") != oItem.GRNDate.ToString("dd MMM yyyy"))
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region SubTotal
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        if (_bIsRateView) { _oPdfPCell.Colspan = 6; } else { _oPdfPCell.Colspan = 8; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        if (_bIsRateView)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Date Wise Total
                        _oPdfPCell = new PdfPCell(new Phrase("Date Wise Total :", _oFontStyleBold));
                        if (_bIsRateView) { _oPdfPCell.Colspan = 11; } else { _oPdfPCell.Colspan = 13; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nDateWiseQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        if (_bIsRateView)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nDateWiseAmount), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalQty = 0; nTotalAmount = 0; nDateWiseQty = 0; nDateWiseAmount = 0;
                    }

                    #region Header

                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(15);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f,    //SL 
                                                    65f,    //GRN No/GLDate
                                                    50f,    //GRN Status
                                                    80f,    //Supplier Name
                                                   65f,    //ApprovedByName
                                                    35f,    //Code
                                                   100f,    //Product Name
                                                    45f,    //LotNo
                                                    55f,    //ColorName
                                                    35f,    //M Unit
                                                    50f,    //Ref qty
                                                    40f,    //Qty
                                                    40f,    //Extra Qty
                                                    40f,    //Rate
                                                    50f,    //Amount
                                             });
                    #endregion

                    #region Date Heading
                    _oPdfPCell = new PdfPCell(new Phrase("GRN Date @ " + oItem.GRNDateSt, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 15; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN No/GL Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN Status/ Store Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Approved By ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
                    if (!_bIsRateView) { _oPdfPCell.Colspan = 3; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Color Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Ref Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Extra Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    if (_bIsRateView)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(15);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f,    //SL 
                                                    65f,    //GRN No/GLDate
                                                    50f,    //GRN Status
                                                    80f,    //Supplier Name
                                                   65f,    //ApprovedByName
                                                    35f,    //Code
                                                   100f,    //Product Name
                                                    45f,    //LotNo
                                                    55f,    //ColorName
                                                    35f,    //M Unit
                                                    50f,    //Ref qty
                                                    40f,    //Qty
                                                    40f,    //Extra Qty
                                                    40f,    //Rate
                                                    50f,    //Amount
                                             });
                    #endregion

                    #endregion

                    nCount = 0;
                }


                if (nGRNID != oItem.GRNID)
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region SubTotal
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        if (_bIsRateView) { _oPdfPCell.Colspan = 6; } else { _oPdfPCell.Colspan = 8; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        if (_bIsRateView)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalQty = 0; nTotalAmount = 0;
                    }

                    #region Initialize Table
                    oPdfPTable = new PdfPTable(15);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f,    //SL 
                                                    65f,    //GRN No/GLDate
                                                    50f,    //GRN Status
                                                    80f,    //Supplier Name
                                                   65f,    //ApprovedByName
                                                    35f,    //Code
                                                   100f,    //Product Name
                                                    45f,    //LotNo
                                                    55f,    //ColorName
                                                    35f,    //M Unit
                                                    50f,    //Ref qty
                                                    40f,    //Qty
                                                    40f,    //Extra Qty
                                                    40f,    //Rate
                                                    50f,    //Amount
                                             });
                    #endregion

                    nCount++;
                    nRowSpan = _oGRNRegisters.Where(GRNR => GRNR.GRNID == oItem.GRNID).ToList().Count + 1;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.GRNNo + "\n" + oItem.GLDateSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.GRNStatusSt + Environment.NewLine + oItem.StoreName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.SupplierName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ApprovedByName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                if (!_bIsRateView) { _oPdfPCell.Colspan = 3; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.RefQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.ReceivedQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExtraQty < 0 ? "-" : Global.MillionFormatActualDigit(oItem.ExtraQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (_bIsRateView)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CurrencySymbol + " " + oItem.UnitPriceSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                oPdfPTable.CompleteRow();

                nGRNID = oItem.GRNID;
                dGRNDate = oItem.GRNDate;
                sCurrencySymbol = oItem.CurrencySymbol;
                nTotalQty = nTotalQty + oItem.ReceivedQty;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nDateWiseQty = nDateWiseQty + oItem.ReceivedQty;
                nDateWiseAmount = nDateWiseAmount + oItem.Amount;
                nGrandTotalQty = nGrandTotalQty + oItem.ReceivedQty;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region SubTotal
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            if (_bIsRateView) { _oPdfPCell.Colspan = 6; } else { _oPdfPCell.Colspan = 8; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_bIsRateView)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Date Wise Total
            _oPdfPCell = new PdfPCell(new Phrase("Date Wise Total :", _oFontStyleBold));
            if (_bIsRateView) { _oPdfPCell.Colspan = 11; } else { _oPdfPCell.Colspan = 13; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nDateWiseQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_bIsRateView)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nDateWiseAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            if (_bIsRateView) { _oPdfPCell.Colspan = 11; } else { _oPdfPCell.Colspan = 13; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_bIsRateView)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodyPartyWiseDetails()
        {
            _oGRNRegisters = _oGRNRegisters.OrderBy(x => x.SupplierName).ToList();
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(15);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f,    //SL 
                                                    70f,    //GRN No //GLDate
                                                    50f,    //GRN Status
                                                    50f,    //GRN Date
                                                    62f,    //ApprovedByName
                                                    35f,    //Code
                                                   100f,    //Product Name
                                                    45f,    //LotNo
                                                    53f,    //ColorName
                                                    45f,    //M Unit
                                                    53f,    //Ref qty
                                                    40f,    //Qty
                                                    40f,    //Extra Qty
                                                    42f,    //Rate
                                                    70f,    //Amount
                                             });
            #endregion

            int nGRNID = 0; string sSupplierName = ""; int nCount = 0; int nRowSpan = 0; string sCurrencySymbol = "";
            double nTotalAmount = 0, nTotalQty = 0, nPartyWiseQty = 0, nPartyWiseAmount = 0, nGrandTotalAmount = 0, nGrandTotalQty = 0;
            foreach (GRNRegister oItem in _oGRNRegisters)
            {
                if (sSupplierName != oItem.SupplierName)
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region SubTotal
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        if (_bIsRateView) { _oPdfPCell.Colspan = 6; } else { _oPdfPCell.Colspan = 8; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        if (_bIsRateView)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Party Wise Total
                        _oPdfPCell = new PdfPCell(new Phrase("Party Wise Total :", _oFontStyleBold));
                        if (_bIsRateView) { _oPdfPCell.Colspan = 11; } else { _oPdfPCell.Colspan = 13; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nPartyWiseQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        if (_bIsRateView)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nPartyWiseAmount), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                        nGRNID = 0;
                        nTotalQty = 0; nTotalAmount = 0; nPartyWiseQty = 0; nPartyWiseAmount = 0;
                    }

                    #region Header

                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(15);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f,    //SL 
                                                    70f,    //GRN No //GLDate
                                                    50f,    //GRN Status
                                                    50f,    //GRN Date
                                                    62f,    //ApprovedByName
                                                    35f,    //Code
                                                   100f,    //Product Name
                                                    45f,    //LotNo
                                                    53f,    //ColorName
                                                    45f,    //M Unit
                                                    53f,    //Ref qty
                                                    40f,    //Qty
                                                    40f,    //Extra Qty
                                                    42f,    //Rate
                                                    70f,    //Amount
                                             });
                    #endregion

                    #region Date Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.SupplierName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 15; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN No/GL Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN Status/ Store Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Approved By ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
                    if (!_bIsRateView) { _oPdfPCell.Colspan = 3; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Lot No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Color Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Ref Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("GRN Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Extra Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    if (_bIsRateView)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(15);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f,    //SL 
                                                    70f,    //GRN No //GLDate
                                                    50f,    //GRN Status
                                                    50f,    //GRN Date
                                                    62f,    //ApprovedByName
                                                    35f,    //Code
                                                   100f,    //Product Name
                                                    45f,    //LotNo
                                                    53f,    //ColorName
                                                    45f,    //M Unit
                                                    53f,    //Ref qty
                                                    40f,    //Qty
                                                    40f,    //Extra Qty
                                                    42f,    //Rate
                                                    70f,    //Amount
                                             });
                    #endregion

                    #endregion

                    nCount = 0;
                }


                if (nGRNID != oItem.GRNID)
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region SubTotal
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        if (_bIsRateView) { _oPdfPCell.Colspan = 6; } else { _oPdfPCell.Colspan = 8; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        if (_bIsRateView)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalQty = 0; nTotalAmount = 0;
                    }

                    #region Initialize Table
                    oPdfPTable = new PdfPTable(15);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f,    //SL 
                                                    70f,    //GRN No //GLDate
                                                    50f,    //GRN Status
                                                    50f,    //GRN Date
                                                    62f,    //ApprovedByName
                                                    35f,    //Code
                                                   100f,    //Product Name
                                                    45f,    //LotNo
                                                    53f,    //ColorName
                                                    45f,    //M Unit
                                                    53f,    //Ref qty
                                                    40f,    //Qty
                                                    40f,    //Extra Qty
                                                    42f,    //Rate
                                                    70f,    //Amount
                                             });
                    #endregion

                    nCount++;
                    nRowSpan = _oGRNRegisters.Where(GRNR => GRNR.GRNID == oItem.GRNID).ToList().Count + 1;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.GRNNo + "\n" + oItem.GLDateSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.GRNStatusSt + Environment.NewLine + oItem.StoreName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.GRNDateSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ApprovedByName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                if (!_bIsRateView) { _oPdfPCell.Colspan = 3; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LotNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.RefQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.ReceivedQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExtraQty < 0 ? "-" : Global.MillionFormatActualDigit(oItem.ExtraQty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (_bIsRateView)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CurrencySymbol + " " + oItem.UnitPriceSt, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                oPdfPTable.CompleteRow();

                nGRNID = oItem.GRNID;
                sSupplierName = oItem.SupplierName;
                sCurrencySymbol = oItem.CurrencySymbol;
                nTotalQty = nTotalQty + oItem.ReceivedQty;
                nTotalAmount = nTotalAmount + oItem.Amount;
                nPartyWiseQty = nPartyWiseQty + oItem.ReceivedQty;
                nPartyWiseAmount = nPartyWiseAmount + oItem.Amount;
                nGrandTotalQty = nGrandTotalQty + oItem.ReceivedQty;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region SubTotal
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            if (_bIsRateView) { _oPdfPCell.Colspan = 6; } else { _oPdfPCell.Colspan = 8; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (_bIsRateView)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Date Wise Total
            _oPdfPCell = new PdfPCell(new Phrase("Party Wise Total :", _oFontStyleBold));
            if (_bIsRateView) { _oPdfPCell.Colspan = 11; } else { _oPdfPCell.Colspan = 13; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nPartyWiseQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_bIsRateView)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nPartyWiseAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            if (_bIsRateView) { _oPdfPCell.Colspan = 11; } else { _oPdfPCell.Colspan = 13; } _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            if (_bIsRateView)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }


        #endregion

        #region endregion
        public byte[] PrepareReportTwo(List<GRNRegister> oGRNRegisters, Company oCompany, EnumReportLayout eReportLayout, string sDateRange, bool bIsRateView)
        {
            _oGRNRegisters = oGRNRegisters;
            _oCompany = oCompany;
            _bIsRateView = bIsRateView;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
            _oDocument.SetMargins(20f, 20f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 802 });
            #endregion

      
                this.PrintHeader("GRN Register", sDateRange);
                this.BoddyTwo();
                _oPdfPTable.HeaderRows = 3;
            //}

            //else if (eReportLayout == EnumReportLayout.PartyWiseDetails)
            //{
            //    this.PrintHeader("Party Wise GRN Register(Details)", sDateRange);
            //    this.PrintBodyPartyWiseDetails();
            //    _oPdfPTable.HeaderRows = 3;
            //}

            //else if (eReportLayout == EnumReportLayout.ProductWise)
            //{
            //    this.PrintHeader("Product Wise GRN Register(Details)", sDateRange);
            //    this.PrintBodyProductWiseDetails();
            //    _oPdfPTable.HeaderRows = 3;
            //}

            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void BoddyTwo()
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
          
            PdfPTable oPdfPTable = new PdfPTable(14);
            oPdfPTable.SetWidths(new float[] { 20f, 35f,100f, 80f, 42f, 42f, 32f,13f, 42f, 40f, 35f, 30f, 25f, 30f  });

       // string sTemp=  _oGRNRegisters.Where(p=>p.ProductTypeSt!='').FirstOrDefault().ProductTypeSt;
         string sTemp=  _oGRNRegisters.Where(p=>p.ProductTypeSt!="").ToList().FirstOrDefault().ProductTypeSt;

            #region Fabric Info
            oPdfPTable = new PdfPTable(14);
            oPdfPTable.SetWidths(new float[] { 20f, 35f, 100f, 80f, 42f, 42f, 32f, 13f, 42f, 40f, 35f, 30f, 25f, 30f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL#", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "GRN Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Name of Supplier", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sTemp, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Lot", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PI No.", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PI Qty.", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Unit", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "L/C No.", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Challan No.", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Challan Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Received Qty", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "PI Rate(USD)", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Amount (USD)", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, _oFontStyleBold);
            

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            int nSLNo = 0;

            foreach (var oItem1 in _oGRNRegisters)
            {
                nSLNo++;
                oPdfPTable = new PdfPTable(14);
                oPdfPTable.SetWidths(new float[] { 20f, 35f, 100f, 80f, 42f, 42f, 32f, 13f, 42f, 40f, 35f, 30f, 25f, 30f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.GRNDateSt, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.SupplierName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ProductName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.LotNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.PINo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.PIQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.MUSymbol, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.LCNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ChallanNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.RefQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.ReceivedQty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.CurrencySymbol+""+ Global.MillionFormat(oItem1.UnitPrice), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.CurrencySymbol + "" + Global.MillionFormat((oItem1.UnitPrice * oItem1.ReceivedQty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            oPdfPTable = new PdfPTable(14);
            oPdfPTable.SetWidths(new float[] { 20f, 35f, 100f, 80f, 42f, 42f, 32f, 13f, 42f, 40f, 35f, 30f, 25f, 30f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 10, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oGRNRegisters.Sum(x => x.RefQty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(_oGRNRegisters.Sum(x => x.ReceivedQty)), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable,"", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

        }
        #endregion
    }
}
