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
namespace ESimSol.Reports
{
     public class rptSalesComissionRegisters
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
        List<SalesComissionRegister> _oSalesComissionRegisters = new List<SalesComissionRegister>();
        List<ExportBill> _oExportBills = new List<ExportBill>();
        Company _oCompany = new Company();
        EnumReportLayout _eReportLayout;
        string _sMohterBuyerName ="";
        #endregion
        public byte[] PrepareReport(List<SalesComissionRegister> oSalesComissionRegisters, Company oCompany, EnumReportLayout eReportLayout, List<ExportBill> oExportBills, string sDateRange, string sMohterBuyerName)
        {
            _oSalesComissionRegisters = oSalesComissionRegisters;
            _oCompany = oCompany;
            _oExportBills = oExportBills;
            _sMohterBuyerName = sMohterBuyerName;
            _eReportLayout = eReportLayout;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            if (eReportLayout != EnumReportLayout.Month_Wise)//not Month wise
            {
                if (eReportLayout == EnumReportLayout.SalesCommissionDetails)
                {
                    _oDocument.SetPageSize(new iTextSharp.text.Rectangle(1150,595)); //595X842
                }
                else 
                {
                    _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate()); //595X842
                }
            }

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
           
            if (eReportLayout == EnumReportLayout.TOP_Sheet)
            {
                _oPdfPTable = new PdfPTable(5);
                _oPdfPTable.WidthPercentage = 100;
                _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPTable.SetWidths(new float[] { 
                                                    40f,  //SL 
                                                    230f,  //Buyer Name
                                                    140f,  //qty
                                                    140f,  //LC
                                                    140f  //Month
                                             });
            }
            else
            {
                _oPdfPTable.SetWidths(new float[] { 802 });
            }
            #endregion

            #region Report Body & Header
            if (eReportLayout == EnumReportLayout.Month_Wise)
            {
                this.PrintHeader(_sMohterBuyerName+" Month Wise Register", sDateRange);
                this.PrintBody();
                _oPdfPTable.HeaderRows = 4;
            }
            else if (eReportLayout == EnumReportLayout.PartyWise)
            {
                this.PrintHeader(_sMohterBuyerName+" Party Wise Sales Commission Register", sDateRange);
                this.PrintBodyPartyWise();
                _oPdfPTable.HeaderRows = 3;
            }
            else if (eReportLayout == EnumReportLayout.PIWise)
            {
                this.PrintHeader(_sMohterBuyerName+" PI Wise Sales Commission Register", sDateRange);
                this.PrintBodyPIWise();
                _oPdfPTable.HeaderRows = 3;
            }

            else if (eReportLayout == EnumReportLayout.PI_Wise_Details)
            {
                this.PrintHeader(_sMohterBuyerName+" PI Wise Sales Commission Register(Details)", sDateRange);
                this.PrintBodyPIWiseDetails();
                _oPdfPTable.HeaderRows = 4;
            }
            else if (eReportLayout == EnumReportLayout.SalesCommissionDetails)
            {
                this.PrintHeader("Sales Commission Details", sDateRange);
                this.PrintBodySalesCommmissionDetails();
                _oPdfPTable.HeaderRows = 4;
            }
            else if (eReportLayout == EnumReportLayout.TOP_Sheet)
            {
                this.PrintHeader(_sMohterBuyerName + " TOP Sheet", sDateRange);
                this.PrintBodyTOPSheetWise();
                _oPdfPTable.HeaderRows = 4;
            }
            #endregion
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
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
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
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
            if (_eReportLayout == EnumReportLayout.TOP_Sheet) { _oPdfPCell.Colspan = 5; }
             _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            if (_eReportLayout == EnumReportLayout.TOP_Sheet) { _oPdfPCell.Colspan = 5; } _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 1f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            if (_eReportLayout == EnumReportLayout.TOP_Sheet) { _oPdfPCell.Colspan = 5; } _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #endregion
        }
        #endregion

        #region Report Body Month wise
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    50f,  //SL 
                                                    330f,  //Name of Month
                                                    150f,  //Qty
                                                    150f  //Value
                                             });

            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Name Of Month", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty(Pcs)", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                        
            _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

          

            int nCount = 0; int nRowSpan = 0;
            double nTotalQty = 0,  nAmount = 0;
            foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
            {
                nCount++;
                #region table initalize
                oPdfPTable = new PdfPTable(4);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    50f,  //SL 
                                                    330f,  //Name of Month
                                                    150f,  //Qty
                                                    150f  //Value
                                             });
                #endregion
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PIDateMonthSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Amount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                     oPdfPTable.CompleteRow();

                nTotalQty += oItem.Qty;
                nAmount = nAmount + oItem.Amount;

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

          

            #region Grand Total
            #region table initalize
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    50f,  //SL 
                                                    330f,  //Name of Month
                                                    150f,  //Qty
                                                    150f  //Value
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodyPartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    40f,//SL 
                                                    305f,// customer Name
                                                    160f,//Export LC NO
                                                    110f,//Qty
                                                    110f //value
                                             });
            #endregion

             DateTime sPIDateMonth = DateTime.Now; int nCount = 0; int nRowSpan = 0;
            double nGrandTotalAmount = 0, nDateWiseQty = 0, nDateWiseAmount = 0, nGrandTotalQty = 0;
            foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
            {
                if (sPIDateMonth != oItem.PIDateMonth)
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {


                        #region Month Wise Total
                        _oPdfPCell = new PdfPCell(new Phrase("Month Wise Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nDateWiseQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nDateWiseAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nDateWiseQty = 0; nDateWiseAmount = 0; nCount=0;
                    }

                    #region Header

                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(5);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    40f,//SL 
                                                    305f,// customer Name
                                                    160f,//Export LC NO
                                                    110f,//Qty
                                                    110f //value
                                             });
                    #endregion

                    #region Date Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Month: " + oItem.PIDateMonthSt, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 5; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    #region Table Initialize
                    oPdfPTable = new PdfPTable(5);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    40f,//SL 
                                                    305f,// customer Name
                                                    160f,//Export LC NO
                                                    110f,//Qty
                                                    110f //value
                                             });
                    #endregion

                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Customer Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Export LC No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    
                    _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(5);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    40f,//SL 
                                                    305f,// customer Name
                                                    160f,//Export LC NO
                                                    110f,//Qty
                                                    110f //value
                                             });
                    #endregion

                    #endregion

                    nCount = 0;
                }
                  nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty)+" "+oItem.MUSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("$"+Global.MillionFormatActualDigit(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                sPIDateMonth = oItem.PIDateMonth;
                nDateWiseQty = nDateWiseQty + oItem.Qty;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                nDateWiseAmount = nDateWiseAmount + oItem.Amount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;
            }

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Date Wise Total
            #region Table Initialize
            oPdfPTable = new PdfPTable(5);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    40f,//SL 
                                                    305f,// customer Name
                                                    160f,//Export LC NO
                                                    110f,//Qty
                                                    110f //value
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Month Wise Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nDateWiseQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("$"+Global.MillionFormatActualDigit(nDateWiseAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            
            oPdfPTable.CompleteRow();
            #endregion
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            #region Table Initialize
            oPdfPTable = new PdfPTable(5);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    40f,//SL 
                                                    305f,// customer Name
                                                    160f,//Export LC NO
                                                    110f,//Qty
                                                    110f //value
                                             });
            #endregion
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalQty)+" "+_oSalesComissionRegisters[0].MUSymbol, _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("$" + Global.MillionFormatActualDigit(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodyPIWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    140f,  //PI No
                                                    100f,  //Date
                                                    200f,  //Buyer Name
                                                    100f,  //qty
                                                    100f  //Amount
                                             });
            #endregion

            #region Header Row
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Customer Name", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                       

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            DateTime dChallanDate = DateTime.MinValue; int nCount = 0;
            double nGrandAmount = 0,  nGrandTotalQty = 0;
            foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
            {
                #region Initialize Table
                 oPdfPTable = new PdfPTable(6);
                oPdfPTable.WidthPercentage = 100;
                oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    140f,  //PI No
                                                    100f,  //Date
                                                    200f,  //Buyer Name
                                                    100f,  //qty
                                                    100f  //Amount
                                             });
                #endregion
                 
                nCount++;    
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.IssueDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty)+" "+oItem.MUSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("$"+Global.MillionFormatActualDigit(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();
                nGrandAmount = nGrandAmount + oItem.Amount;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }

            #region Grand Total
            #region Initialize Table
            oPdfPTable = new PdfPTable(6);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    140f,  //PI No
                                                    100f,  //Date
                                                    200f,  //Buyer Name
                                                    100f,  //qty
                                                    100f  //Amount
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("$"+Global.MillionFormatActualDigit(nGrandAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodyPIWiseDetails()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
          oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    50f,  //pi No
                                                    50f,  //Status
                                                    150f,  //Buyer Name
                                                    55f,  //LC NO
                                                    80f,  //Issue date
                                                    40f,   //Code
                                                    90f,  //Product Name
                                                    30f,   //M Unit
                                                    40f,  //Qty
                                                    60f//Amount
                                                   
                                             });
            #endregion

            #region Header Row
          _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("PI No ", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Status ", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Issue Date", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


          _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Product Name", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("M. Unit", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          oPdfPTable.CompleteRow();
          #endregion

          #region Insert Into Main Table
          _oPdfPCell = new PdfPCell(oPdfPTable);
          _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
          _oPdfPTable.CompleteRow();
          #endregion

          #region Initialize Table
          oPdfPTable = new PdfPTable(11);
          oPdfPTable.WidthPercentage = 100;
          oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
          oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    50f,  //pi No
                                                    50f,  //Status
                                                    150f,  //Buyer Name
                                                    55f,  //LC NO
                                                    80f,  //Issue date
                                                    40f,   //Code
                                                    90f,  //Product Name
                                                    30f,   //M Unit
                                                    40f,  //Qty
                                                    60f//Amount
                                                   
                                             });
          #endregion

            int nExportPIID = 0;int nCount = 0; int nRowSpan = 0;
            double nTotalQty = 0, nTotalAmount = 0, nGrandTotalQty = 0, nGrandTotalAmount = 0;
            foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
            {

                if (nExportPIID != oItem.ExportPIID)
                {
                    if (oPdfPTable.Rows.Count > 0 )
                    {
                        if (nTotalAmount > 0)
                        {
                            #region SubTotal
                            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalAmount), _oFontStyleBold));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                        }
                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalQty = 0; nTotalAmount=0; 
                    }

                    #region Initialize Table
                    oPdfPTable = new PdfPTable(11);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    50f,  //pi No
                                                    50f,  //Status
                                                    150f,  //Buyer Name
                                                    55f,  //LC NO
                                                    80f,  //Issue date
                                                    40f,   //Code
                                                    90f,  //Product Name
                                                    30f,   //M Unit
                                                    40f,  //Qty
                                                    60f//Amount
                                                   
                                             });
                    #endregion

                    nCount++;
                    nRowSpan = _oSalesComissionRegisters.Where(ChallanR => ChallanR.ExportPIID == oItem.ExportPIID).ToList().Count + 1;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PIStatusSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(oItem.IssueDateSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductCode, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MUSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                nExportPIID = oItem.ExportPIID;
                
                nTotalQty = nTotalQty + oItem.Qty;
                nTotalAmount = nTotalAmount + oItem.Amount;
                
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                nGrandTotalAmount = nGrandTotalAmount + oItem.Amount;

            }

            #region SubTotal
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        
            #region Grand Total
            #region Initialize Table
            oPdfPTable = new PdfPTable(11);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f,  //SL 
                                                    50f,  //pi No
                                                    50f,  //Status
                                                    150f,  //Buyer Name
                                                    55f,  //LC NO
                                                    80f,  //Issue date
                                                    40f,   //Code
                                                    90f,  //Product Name
                                                    30f,   //M Unit
                                                    40f,  //Qty
                                                    60f//Amount
                                                   
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodySalesCommmissionDetails()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(21);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f, //SL 
                                                    130f, //Buyer Name
                                                    130f, //pi No
                                                    60f, //Issue date
                                                    90f,//Product Name
                                                    40f,//Qty
                                                    30f,//Unit Price
                                                    60f,//Amount
                                                    60f, //Challan No
                                                    80f,//Delivery Date
                                                    60f,//Delivery qty
                                                    50f,//Balance
                                                    50f,//LC NO
                                                    50f,//LC Rcv date
                                                    50f,//LC Value
                                                    50f,//Invoice No
                                                    60f,//Invoice date
                                                    60f,//Acceptance date
                                                    50f,//Matuaty  Date
                                                    60f,//Realization Date
                                                    50f//Remarks
                                                   
                                             });
            #endregion

            #region Header Row
          _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Curstomer Name", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("PI No ", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("PI Date", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Hanger Ref", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Unit Price", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Total Price", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


          _oPdfPCell = new PdfPCell(new Phrase("Ch. No", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


          _oPdfPCell = new PdfPCell(new Phrase("Delivery Date", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


          _oPdfPCell = new PdfPCell(new Phrase("Delivery Qty ", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("LC No", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("LC Rcv Date", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("LC Value", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Accepance Sub.Date", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Accepance RCV.Date", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("DOC.RCV Value", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Maturity RCV Date", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Realization Date", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyleBold));
          _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

          oPdfPTable.CompleteRow();
          #endregion

          #region Insert Into Main Table
          _oPdfPCell = new PdfPCell(oPdfPTable);
          _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
          _oPdfPTable.CompleteRow();
          #endregion

          #region Initialize Table
          oPdfPTable = new PdfPTable(21);
          oPdfPTable.WidthPercentage = 100;
          oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
          oPdfPTable.SetWidths(new float[] { 
                                                    25f, //SL 
                                                    130f, //Buyer Name
                                                    130f, //pi No
                                                    60f, //Issue date
                                                    90f,//Product Name
                                                    40f,//Qty
                                                    30f,//Unit Price
                                                    60f,//Amount
                                                    60f, //Challan No
                                                    80f,//Delivery Date
                                                    60f,//Delivery qty
                                                    50f,//Balance
                                                    50f,//LC NO
                                                    50f,//LC Rcv date
                                                    50f,//LC Value
                                                    50f,//Invoice No
                                                    60f,//Invoice date
                                                    60f,//Acceptance date
                                                    50f,//Matuaty  Date
                                                    60f,//Realization Date
                                                    50f//Remarks
                                                   
                                             });
          #endregion

          int nExportPIID = 0, nExportLCID = -1; int nCount = 0; int nRowSpan = 0, nRowSpanForLC = 0 ;
            double nGrandTotalQty = 0, nGrandTotalAmount = 0;
            foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
            {

                if (nExportLCID != oItem.ExportLCID)
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                        #region Total
                        #region Initialize Table
                        oPdfPTable = new PdfPTable(21);
                        oPdfPTable.WidthPercentage = 100;
                        oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.SetWidths(new float[] { 
                                                    25f, //SL 
                                                    130f, //Buyer Name
                                                    130f, //pi No
                                                    60f, //Issue date
                                                    90f,//Product Name
                                                    40f,//Qty
                                                    30f,//Unit Price
                                                    60f,//Amount
                                                    60f, //Challan No
                                                    80f,//Delivery Date
                                                    60f,//Delivery qty
                                                    50f,//Balance
                                                    50f,//LC NO
                                                    50f,//LC Rcv date
                                                    50f,//LC Value
                                                    50f,//Invoice No
                                                    60f,//Invoice date
                                                    60f,//Acceptance date
                                                    50f,//Matuaty  Date
                                                    60f,//Realization Date
                                                    50f//Remarks
                                                   
                                             });
                        #endregion

                        _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalQty), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("Total L/C Received:", _oFontStyleBold));
                        _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                        _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion
                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                        nGrandTotalQty = 0; nGrandTotalAmount = 0;
                    }

                    #region Initialize Table
                    oPdfPTable = new PdfPTable(21);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    25f, //SL 
                                                    130f, //Buyer Name
                                                    130f, //pi No
                                                    60f, //Issue date
                                                    90f,//Product Name
                                                    40f,//Qty
                                                    30f,//Unit Price
                                                    60f,//Amount
                                                    60f, //Challan No
                                                    80f,//Delivery Date
                                                    60f,//Delivery qty
                                                    50f,//Balance
                                                    50f,//LC NO
                                                    50f,//LC Rcv date
                                                    50f,//LC Value
                                                    50f,//Invoice No
                                                    60f,//Invoice date
                                                    60f,//Acceptance date
                                                    50f,//Matuaty  Date
                                                    60f,//Realization Date
                                                    50f//Remarks
                                                   
                                             });
                    #endregion

                    nCount++;
                    nRowSpanForLC = _oSalesComissionRegisters.Where(ChallanR => ChallanR.ExportLCID == oItem.ExportLCID).ToList().Count;
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }
                if(nExportPIID!=oItem.ExportPIID)
                {

                    nRowSpan = _oSalesComissionRegisters.Where(ChallanR => ChallanR.ExportPIID == oItem.ExportPIID).ToList().Count;
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.IssueDateSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.RateUnit > 1 ? Global.MillionFormat(oItem.UnitPrice) + "/" + oItem.RateUnit.ToString() : Global.MillionFormat(oItem.UnitPrice), _oFontStyle)); _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetDeliveryInfo(oItem.ChallanInfo));
                _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Balance), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (nExportLCID != oItem.ExportLCID)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExportLCNo, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCRecivedDateInString, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCValue!=0?Global.MillionFormat(oItem.LCValue):"", _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    List<ExportBill> oExportBills = new List<ExportBill>();
                    oExportBills = _oExportBills.Where(x => x.ExportLCID==oItem.ExportLCID).ToList();
                    if (oExportBills.Count > 0)
                    {
                        PdfPTable oTempPdfPTable = new PdfPTable(6);
                        oTempPdfPTable.SetWidths(new float[] { 
                                                    50f,//Invoice No
                                                    60f,//Invoice date
                                                    60f,//Acceptance date
                                                    50f,//Matuaty  Date
                                                    60f,//Realization Date
                                                    50f//Remarks
                                                   
                                             });
                        foreach (ExportBill BItem in oExportBills)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(BItem.SendToPartySt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(BItem.AcceptanceDateStr, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(BItem.AmountSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(BItem.MaturityReceivedDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(BItem.RelizationDateSt, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                        }
                        _oPdfPCell = new PdfPCell(oTempPdfPTable);
                        _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                        _oPdfPCell.Rowspan = nRowSpanForLC; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    }
                    nGrandTotalAmount = nGrandTotalAmount + oItem.LCValue;
                    
                 }
                oPdfPTable.CompleteRow();

                nExportPIID = oItem.ExportPIID;
                nExportLCID = oItem.ExportLCID;

                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                

            }

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        
            #region Total
            #region Initialize Table
            oPdfPTable = new PdfPTable(21);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    25f, //SL 
                                                    130f, //Buyer Name
                                                    130f, //pi No
                                                    60f, //Issue date
                                                    90f,//Product Name
                                                    40f,//Qty
                                                    30f,//Unit Price
                                                    60f,//Amount
                                                    60f, //Challan No
                                                    80f,//Delivery Date
                                                    60f,//Delivery qty
                                                    50f,//Balance
                                                    50f,//LC NO
                                                    50f,//LC Rcv date
                                                    50f,//LC Value
                                                    50f,//Invoice No
                                                    60f,//Invoice date
                                                    60f,//Acceptance date
                                                    50f,//Matuaty  Date
                                                    60f,//Realization Date
                                                    50f//Remarks
                                                   
                                             });
            #endregion

            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total L/C Received:", _oFontStyleBold));
            _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
            _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodyTOPSheetWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Header Row
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Customer Name", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC Value", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Month", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            DateTime dLCOpenDateMonth = DateTime.MinValue; int nCount = 0;
            double nGrandAmount = 0, nGrandTotalQty = 0;
            foreach (SalesComissionRegister oItem in _oSalesComissionRegisters)
            {

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty) + " " + oItem.MUSymbol, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("$" + Global.MillionFormatActualDigit(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                if (dLCOpenDateMonth != oItem.LCOpenDateMonth)
                {
                    int nRowSpan = _oSalesComissionRegisters.Where(x => x.LCOpenDateMonth == oItem.LCOpenDateMonth).Count();
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpenDateMonthSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment= Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    dLCOpenDateMonth = oItem.LCOpenDateMonth;
                }
                _oPdfPTable.CompleteRow();
                nGrandAmount = nGrandAmount + oItem.Amount;
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
            }

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("$" + Global.MillionFormatActualDigit(nGrandAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion

        #region Get info
         private PdfPTable GetDeliveryInfo(string ChallanInfo)
         {
             PdfPTable oTempPdfPTable = new PdfPTable(3);
             string  [] sAChallanInfo = !string.IsNullOrEmpty(ChallanInfo)?ChallanInfo.Split(','):new []{""};
             if (!string.IsNullOrEmpty(ChallanInfo) &&sAChallanInfo.Length>0)
             {
                 for(int i = 0; i<sAChallanInfo.Length;i++)
                 {
                     _oPdfPCell = new PdfPCell(new Phrase(sAChallanInfo[i].Split('D')[0], _oFontStyleBold)); //Challa no
                     _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);

                     _oPdfPCell = new PdfPCell(new Phrase(sAChallanInfo[i].Split('D')[1].Split('Q')[0], _oFontStyleBold));// date
                     _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                                          
                     _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(Convert.ToDouble(sAChallanInfo[i].Split('Q')[1])), _oFontStyleBold));//qty
                     _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                                      
                     oTempPdfPTable.CompleteRow();
                 }
                 
             }
             else
             {
                 _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
                 _oPdfPCell.Border = 0; _oPdfPCell.Colspan= 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oTempPdfPTable.AddCell(_oPdfPCell);
                 oTempPdfPTable.CompleteRow();
             }
             return oTempPdfPTable;
         }
        #endregion
    }
}
