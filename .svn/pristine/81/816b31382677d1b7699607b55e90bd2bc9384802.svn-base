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
    public class rptExportLCRegisters
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
        List<ExportLCRegister> _oExportLCRegisters = new List<ExportLCRegister>();
        Company _oCompany = new Company();
        string _sExportLCType = "";
        #endregion

        public byte[] PrepareReport(List<ExportLCRegister> oExportLCRegisters, Company oCompany,int nReportLayout, string sDateRange,string sExportLCType)
        {
            _oExportLCRegisters = oExportLCRegisters;
            _oCompany = oCompany;
            _sExportLCType = sExportLCType;
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
          
            if (nReportLayout == (int)EnumReportLayout.PartyWise)
            {
                this.PrintHeader("Party Wise ExportLC Register", sDateRange);
                this.PrintBodyPartyWise();
                _oPdfPTable.HeaderRows = 3;
            }
            else if (nReportLayout == (int)EnumReportLayout.ProductWise)
            {
                this.PrintHeader("Product Wise ExportLC Register", sDateRange);
                this.PrintBodyProductWise();
                _oPdfPTable.HeaderRows = 3;
            }
            else if(nReportLayout == (int)EnumReportLayout.BankWise)
            {
                this.PrintHeader("Bank Wise ExportLC Register", sDateRange);
                this.PrintBodyBankWise();
                _oPdfPTable.HeaderRows = 4;
            }
            else if (nReportLayout == (int)EnumReportLayout.LCWithAmendment)
            {
                this.PrintHeader("Month Wise ExportLC Register", sDateRange);
                this.PrintBodyExportLC();
                _oPdfPTable.HeaderRows = 3;
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

            _oPdfPCell = new PdfPCell(new Phrase(" " , _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (_sExportLCType != "")
            {
                _oPdfPCell = new PdfPCell(new Phrase("Export LC Type: " + _sExportLCType, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
           
          

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

        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);

            #region Heading
            PdfPTable oPdfPTable = new PdfPTable(14);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    70f,  //LC No
                                                    50f,  //LC date
                                                    50f,  //Receive date
                                                    100f,  //Supplier Name
                                                    85f,  //Negotiation Bank
                                                    50f,  //Shipment date
                                                    50f,   //Expire DATe
                                                    50f,   //LC Terms
                                                    50f,  //PI No
                                                    45f,   //PI DAte
                                                    40f,   //PI Value
                                                    40f,   //Invoice amont
                                                    60f,   //Yet to Invoice amount
                                             });
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC No ", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC Date ", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Receive Date ", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Negotiation Bank", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shipment Date", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Expire Date", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC Terms", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PI Date", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PI Value", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Invoice\n Amount", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Due\n Amount", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
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
                                                    30f,  //SL 
                                                    70f,  //LC No
                                                    50f,  //LC date
                                                    50f,  //Receive date
                                                    100f,  //Supplier Name
                                                    85f,  //Negotiation Bank
                                                    50f,  //Shipment date
                                                    50f,   //Expire DATe
                                                    50f,   //LC Terms
                                                    50f,  //PI No
                                                    45f,   //PI DAte
                                                    40f,   //PI Value
                                                    40f,   //Invoice amont
                                                    60f,   //Yet to Invoice amount
                                             });
            #endregion

            int nExportLCID = 0; int nCount = 0; int nRowSpan = 0, nRowSpanPI=1; int nImportPIID = 0; string sCurrency = "";
            double nTotalAmount = 0, nTotalInvoAmount = 0, nTotalYetToAmount = 0, nGrandTotalAmount = 0, nGrandTotalInvoAmount = 0, nGrandTotalYetToAmount = 0;
            foreach (ExportLCRegister oItem in _oExportLCRegisters)
            {
                if (nExportLCID != oItem.ExportLCID)
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region SubTotal

                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalInvoAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalYetToAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                        nTotalAmount = nTotalInvoAmount = nTotalYetToAmount = 0;
                    }

                    #region Initialize Table
                    oPdfPTable = new PdfPTable(14);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    70f,  //LC No
                                                    50f,  //LC date
                                                    50f,  //Receive date
                                                    100f,  //Supplier Name
                                                    85f,  //Negotiation Bank
                                                    50f,  //Shipment date
                                                    50f,   //Expire DATe
                                                    50f,   //LC Terms
                                                    50f,  //PI No
                                                    45f,   //PI DAte
                                                    40f,   //PI Value
                                                    40f,   //Invoice amont
                                                    60f,   //Yet to Invoice amount
                                             });
                    #endregion

                    nCount++;
                    nRowSpan = _oExportLCRegisters.Where(PIR => PIR.ExportLCID == oItem.ExportLCID).ToList().Count + 1;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpenDateSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCReceiveDateSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ApplicantName , _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.NegoBankName , _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.UDRecDateSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDateSt , _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCStatusSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }

                if (nImportPIID != oItem.ExportPIID)
                {
                    nRowSpanPI = _oExportLCRegisters.Where(PIR => PIR.ExportLCID == oItem.ExportLCID && PIR.ExportPIID == oItem.ExportPIID).ToList().Count;
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpanPI; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PIDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.LCValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormatActualDigit(oItem.Qty * oItem.UnitPrice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormatActualDigit(oItem.Amount - (oItem.Qty * oItem.UnitPrice)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                nImportPIID = oItem.ExportPIID;
                nExportLCID = oItem.ExportLCID;
                sCurrency = oItem.Currency;
                nTotalAmount = nTotalAmount + oItem.LCValue;
                nTotalInvoAmount = nTotalInvoAmount + (oItem.Qty * oItem.UnitPrice);
                nTotalYetToAmount = nTotalAmount - nTotalInvoAmount;
                nGrandTotalAmount = nGrandTotalAmount + oItem.LCValue;
                nGrandTotalInvoAmount = nGrandTotalInvoAmount + (oItem.Qty * oItem.UnitPrice);
                nGrandTotalYetToAmount = nGrandTotalAmount - nGrandTotalInvoAmount;
            }

            #region SubTotal


            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalInvoAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nTotalYetToAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //double nVal = _oExportLCRegisters.Sum(PIR => PIR.ExportLCID).ToList().Count + 1;
            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nGrandTotalInvoAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(nGrandTotalYetToAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #region AmendmentWise

        private void PrintBodyExportLC()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(12);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    55f,  //LC No
                                                    40f,//LC Date
                                                    50f,//PI No
                                                    100f, //Supplier Name                      
                                                    55f,  //LC VAlue  
                                                    60f,//Amendment Date
                                                    70f,  // PIN No    
                                                    65f,//Product Name
                                                    45f,//Qty
                                                    25f,//U/D
                                                    60f  //Amendment Amount
                                             });
            #endregion

            DateTime dIssueDate = DateTime.MinValue; int nCount = 0; string sCurrencySymbol = "";

            int nExportPIID = 0; int nExportLCID = 0; DateTime dPIDate = DateTime.MinValue; int nRowSpan = 0;
            double nTotalAmount = 0, nTotalQty = 0, nDateWiseQty = 0, nDateWiseAmount = 0, nGrandTotalAmount = 0, nGrandTotalQty = 0;
            foreach (ExportLCRegister oItem in _oExportLCRegisters)
            {
                if (dIssueDate == DateTime.MinValue || dIssueDate.Month != oItem.AmendmentDate.Month)
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region SubTotal
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty) , _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Date Wise Total
                        _oPdfPCell = new PdfPCell(new Phrase("Month Wise Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nDateWiseQty) , _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nDateWiseAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
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
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    55f,  //LC No
                                                    40f,//LC Date
                                                    50f,//Export LC No
                                                    100f, //Supplier Name                      
                                                    55f,  //LC VAlue  
                                                    60f,//Amendment Date
                                                    70f,  // PIN No    
                                                    65f,//Product Name
                                                    45f,//Qty
                                                    25f,//U/P
                                                    60f  //Amendment Amount
                                             });
                    #endregion

                    #region Date Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Month @ " + oItem.AmendmentDate.ToString("MMM yyyy"), _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 12; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LC No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LC Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("C/P", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("BuyerName", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LC Amount", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Amendment Date", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("UD", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("New LC/Amendment Amount", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    55f,  //LC No
                                                    40f,//LC Date
                                                    50f,//Export LC No
                                                    100f, //Supplier Name                      
                                                    55f,  //LC VAlue  
                                                    60f,//Amendment Date
                                                    70f,  // PIN No    
                                                    65f,//Product Name
                                                    45f,//Qty
                                                    25f,//U/P
                                                    60f  //Amendment Amount
                                             });
                    #endregion

                    #endregion

                    nCount = 0;
                }
                if (nExportLCID != oItem.ExportLCID)
                {
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region SubTotal
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty) , _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
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
                    oPdfPTable = new PdfPTable(12);
                    oPdfPTable.WidthPercentage = 100;
                    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    55f,  //LC No
                                                    40f,//LC Date
                                                    50f,//Export LC No
                                                    100f, //Supplier Name                      
                                                    55f,  //LC VAlue  
                                                    60f,//Amendment Date
                                                    70f,  // PIN No    
                                                    65f,//Product Name
                                                    45f,//Qty
                                                    25f,//U/P
                                                    60f  //Amendment Amount
                                             });
                    #endregion

                    nCount++;
                    nRowSpan = _oExportLCRegisters.Where(PIR => PIR.ExportLCID == oItem.ExportLCID && PIR.AmendmentDate.Month == oItem.AmendmentDate.Month).ToList().Count + 1;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    //if (oItem.ExportLCType == EnumExportLCType.TTForeign || oItem.ImportPIType == EnumImportPIType.TTLocal) { oItem.ImportLCNo = "TT " + oItem.ImportLCNo; }
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpenDateSt, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPersonName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);



                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BuyerName, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat(oItem.Amount), _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }

                string sAmendmenDate = ""; if (oItem.AmendmentDate != DateTime.MinValue && oItem.AmendmentDate != oItem.LCOpenDate) { sAmendmenDate = oItem.AmendmentDate.ToString("dd MMM yyyy"); }
                _oPdfPCell = new PdfPCell(new Phrase(sAmendmenDate, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (nExportPIID != oItem.ExportPIID)
                {
                    nRowSpan = _oExportLCRegisters.Where(PIR => PIR.ExportLCID == oItem.ExportLCID && PIR.ExportPIID == oItem.ExportPIID).ToList().Count;
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }


                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty) , _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                string sUDRcv = (oItem.UDRcvType == EnumUDRcvType.No_Receive ? "No" : "Yes");
                _oPdfPCell = new PdfPCell(new Phrase(sUDRcv, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase( Global.MillionFormat(oItem.Qty * oItem.UnitPrice), _oFontStyle));  //changed here Amount
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                nExportPIID = oItem.ExportPIID;
                nExportLCID = oItem.ExportLCID;
                dIssueDate = oItem.AmendmentDate;
                nTotalQty = nTotalQty + oItem.Qty;
                nTotalAmount = nTotalAmount + (oItem.Qty * oItem.UnitPrice);//
                nDateWiseQty = nDateWiseQty + oItem.Qty;
                nDateWiseAmount = nDateWiseAmount + (oItem.Qty * oItem.UnitPrice);//(oItem.Qty*oItem.UnitPrice)
                nGrandTotalQty = nGrandTotalQty + oItem.Qty;
                nGrandTotalAmount = nGrandTotalAmount + (oItem.Qty * oItem.UnitPrice);//
            }

            #region SubTotal
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty),  _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Date Wise Total
            _oPdfPCell = new PdfPCell(new Phrase("Month Wise Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nDateWiseQty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nDateWiseAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 9; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nGrandTotalQty) , _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrencySymbol + " " + Global.MillionFormat(nGrandTotalAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region Party Wise Body
        //private PdfPTable GetPartyWiseTable()
        //{
        //    #region Initialize Table
        //    PdfPTable oPdfPTable = new PdfPTable(13);
        //    oPdfPTable.WidthPercentage = 100;
        //    oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPTable.SetWidths(new float[] { 
        //                                            30f,  //SL 
        //                                            65f,  //LC No
        //                                            50f, //P.I No
        //                                            160f,  //Product
        //                                            60f,  //Qty                                                
        //                                            60f,  //Value                                                  
        //                                            55f,  //Shipment Date
        //                                            55f,  //Expire Date
        //                                            25f,  //Acc
        //                                            25f,  //Acc
        //                                            25f,  //UD
        //                                            115f,  //Query
        //                                            50f //C.P
        //                                     });
        //    #endregion
        //    return oPdfPTable;
        //}
        //private void PrintBodyPartyWise()
        //{
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
        //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
        //    _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

        //    #region Initialize Table
        //    PdfPTable oPdfPTable = GetPartyWiseTable();
        //    #endregion

        //    //_oExportLCRegisters = _oExportLCRegisters.OrderBy(x => x.ApplicantID).ThenBy(x=>x.ApplicantName).ToList();
        //    string sApplicantName = ""; int nCount = 0, nApplicantID = 0, nExportLCID = 0; string sCurrency = "";

        //    foreach (ExportLCRegister oItem in _oExportLCRegisters)
        //    {
        //        if (nApplicantID != oItem.ApplicantID)
        //        {
        //            #region Supplier Wise Sub Total
        //            if (oPdfPTable.Rows.Count > 0)
        //            {
        //                #region Initialize Table
        //                oPdfPTable = GetPartyWiseTable();
        //                #endregion
        //                #region Date Wise Sub Total
        //                _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
        //                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.ApplicantID == nApplicantID).Sum(x => x.Qty)), _oFontStyleBold));
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.ApplicantID == nApplicantID).Sum(x => x.Amount)), _oFontStyleBold));
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //                oPdfPTable.CompleteRow();
        //                #endregion

        //                #region Insert Into Main Table
        //                _oPdfPCell = new PdfPCell(oPdfPTable);
        //                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //                _oPdfPTable.CompleteRow();
        //                #endregion

        //            }
        //            #endregion

        //            #region Header
        //            #region Blank Row
        //            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
        //            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //            _oPdfPTable.CompleteRow();
        //            #endregion

        //            #region Table Initialize
        //            oPdfPTable = GetPartyWiseTable();
        //            #endregion

        //            #region Supplier Heading
        //            _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ApplicantName, _oFontStyleBoldUnderLine));
        //            _oPdfPCell.Colspan = 13; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //            oPdfPTable.CompleteRow();
        //            #endregion

        //            #region Header Row
        //            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("LC No ", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("PI No ", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Shipment Date", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Expired Date", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("ACC", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("ACC", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("UD", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Doc. Send To Bank", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("C/P", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            oPdfPTable.CompleteRow();
        //            #endregion

        //            #region Insert Into Main Table
        //            _oPdfPCell = new PdfPCell(oPdfPTable);
        //            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //            _oPdfPTable.CompleteRow();
        //            #endregion
        //            nCount = 0;
        //            #endregion
        //        }

        //        #region Initialize Table
        //        oPdfPTable = GetPartyWiseTable();
        //        #endregion


        //        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.ShipmentDateSt, _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.ExpiryDateSt, _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Party + " Part", _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Bank + " Part", _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        string sUDRcv = (oItem.UDRcvType == EnumUDRcvType.No_Receive ? "No" : "Yes");
        //        _oPdfPCell = new PdfPCell(new Phrase(sUDRcv, _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.NoteQuery, _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPersonName, _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        oPdfPTable.CompleteRow();


        //        #region Insert Into Main Table
        //        _oPdfPCell = new PdfPCell(oPdfPTable);
        //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //        _oPdfPTable.CompleteRow();
        //        #endregion

        //        nExportLCID = oItem.ExportLCID;
        //        sApplicantName = oItem.ApplicantName;
        //        sCurrency = oItem.Currency;
        //        nApplicantID = oItem.ApplicantID;
        //    }

        //    oPdfPTable = GetPartyWiseTable();
        //    #region Party Wise Sub Total
        //    _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
        //    _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.ApplicantID == nApplicantID).Sum(x => x.Qty)), _oFontStyleBold));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.ApplicantID == nApplicantID).Sum(x => x.Amount)), _oFontStyleBold));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //    oPdfPTable.CompleteRow();
        //    #endregion

        //    #region Grand Total
        //    _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
        //    _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Sum(x => x.Qty)), _oFontStyleBold));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Sum(x => x.Amount)), _oFontStyleBold));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //    oPdfPTable.CompleteRow();
        //    #endregion

        //    #region Insert Into Main Table
        //    _oPdfPCell = new PdfPCell(oPdfPTable);
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion
        //}
        //private void PrintBodyPartyWiseOFF()
        //{
        //    _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
        //    _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
        //    _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

        //    #region Initialize Table
        //    PdfPTable oPdfPTable = GetPartyWiseTable();
        //    #endregion

        //    //_oExportLCRegisters = _oExportLCRegisters.OrderBy(x => x.ApplicantID).ThenBy(x=>x.ApplicantName).ToList();
        //    string sApplicantName = ""; int nCount = 0, nApplicantID = 0, nExportLCID = 0; string sCurrency = "";

        //    foreach (ExportLCRegister oItem in _oExportLCRegisters)
        //    {
        //        if (nApplicantID != oItem.ApplicantID)
        //        {
        //            #region Supplier Wise Sub Total
        //            if (oPdfPTable.Rows.Count > 0)
        //            {
        //                #region Initialize Table
        //                oPdfPTable = GetPartyWiseTable();
        //                #endregion
        //                #region Date Wise Sub Total
        //                _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
        //                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.ApplicantID == nApplicantID).Sum(x => x.Qty)), _oFontStyleBold));
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.ApplicantID == nApplicantID).Sum(x => x.Amount)), _oFontStyleBold));
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //                oPdfPTable.CompleteRow();
        //                #endregion

        //                #region Insert Into Main Table
        //                _oPdfPCell = new PdfPCell(oPdfPTable);
        //                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //                _oPdfPTable.CompleteRow();
        //                #endregion
        //                nCount = 0;
        //                nExportLCID = 0;
        //            }
        //            #endregion

        //            #region Header
        //            #region Blank Row
        //            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
        //            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //            _oPdfPTable.CompleteRow();
        //            #endregion

        //            #region Table Initialize
        //            oPdfPTable = GetPartyWiseTable();
        //            #endregion

        //            #region Supplier Heading
        //            _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oItem.ApplicantName, _oFontStyleBoldUnderLine));
        //            _oPdfPCell.Colspan = 13; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //            oPdfPTable.CompleteRow();
        //            #endregion

        //            #region Header Row
        //            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("LC No ", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("PI No ", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Shipment Date", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Expired Date", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("ACC", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("ACC", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("UD", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("Doc. Send To Bank", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase("C/P", _oFontStyleBold));
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

        //            oPdfPTable.CompleteRow();
        //            #endregion

        //            #region Insert Into Main Table
        //            _oPdfPCell = new PdfPCell(oPdfPTable);
        //            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //            _oPdfPTable.CompleteRow();
        //            #endregion
        //            #endregion
        //        }

        //        int nRowSpan = 1;
        //        if (nExportLCID != oItem.ExportLCID)
        //        {
        //            if (oPdfPTable.Rows.Count > 0 && nCount > 0)
        //            {
        //                #region Sub Total
        //                _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.ApplicantID == nApplicantID && x.ExportLCID == nExportLCID).Sum(x => x.Qty)), _oFontStyleBold));
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.ApplicantID == nApplicantID && x.ExportLCID == nExportLCID).Sum(x => x.Amount)), _oFontStyleBold));
        //                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //                oPdfPTable.CompleteRow();
        //                #endregion

        //                #region Insert Into Main Table
        //                _oPdfPCell = new PdfPCell(oPdfPTable);
        //                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //                _oPdfPTable.CompleteRow();
        //                #endregion
        //            }

        //            #region Initialize Table
        //            oPdfPTable = GetPartyWiseTable();
        //            #endregion

        //            nCount++;
        //            nRowSpan = _oExportLCRegisters.Where(x => x.ExportLCID == oItem.ExportLCID && x.ApplicantID == oItem.ApplicantID).Count() + 1;

        //            _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        }

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
        //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //        if (nExportLCID != oItem.ExportLCID)
        //        {
        //            _oPdfPCell = new PdfPCell(new Phrase(oItem.ShipmentDateSt, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(oItem.ExpiryDateSt, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Party + " Part", _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Bank + " Part", _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            string sUDRcv = (oItem.UDRcvType == EnumUDRcvType.No_Receive ? "No" : "Yes");
        //            _oPdfPCell = new PdfPCell(new Phrase(sUDRcv, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(oItem.NoteQuery, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //            _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPersonName, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
        //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //        }
        //        oPdfPTable.CompleteRow();

        //        nExportLCID = oItem.ExportLCID;
        //        sApplicantName = oItem.ApplicantName;
        //        sCurrency = oItem.Currency;
        //        nApplicantID = oItem.ApplicantID;
        //    }
        //    #region Insert Into Main Table
        //    _oPdfPCell = new PdfPCell(oPdfPTable);
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion

        //    oPdfPTable = GetPartyWiseTable();
        //    #region Party Wise Sub Total
        //    _oPdfPCell = new PdfPCell(new Phrase("Party Wise Sub Total :", _oFontStyleBold));
        //    _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.ApplicantID == nApplicantID).Sum(x => x.Qty)), _oFontStyleBold));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.ApplicantID == nApplicantID).Sum(x => x.Amount)), _oFontStyleBold));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //    oPdfPTable.CompleteRow();
        //    #endregion

        //    #region Grand Total
        //    _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
        //    _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Sum(x => x.Qty)), _oFontStyleBold));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

        //    _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Sum(x => x.Amount)), _oFontStyleBold));
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
        //    oPdfPTable.CompleteRow();
        //    #endregion

        //    #region Insert Into Main Table
        //    _oPdfPCell = new PdfPCell(oPdfPTable);
        //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();
        //    #endregion
        //}
        #endregion
        #region Product Wise Body
        private PdfPTable GetProductWiseTable() 
        {
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(13);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    125f,  //Buyer Name 
                                                    65f,  //LC No
                                                    50f, //P.I No
                                                    60f,  //Qty                                                
                                                    60f,  //Value                                                  
                                                    55f,  //Shipment Date
                                                    55f,  //Expire Date
                                                    35f,  //Acc
                                                    35f,  //Acc
                                                    35f,  //UD
                                                    100f,  //Query
                                                    70f //C.P
                                             });
            #endregion
            return oPdfPTable;
        }
        private void PrintBodyProductWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = GetProductWiseTable();
            #endregion
            //_oExportLCRegisters = _oExportLCRegisters.OrderBy(x => x.ProductName).ThenBy(x => x.ApplicantName).ToList();
            string sProductName="@#$^&*{}[]()"; int nCount = 0; string sCurrency = "";
            foreach (ExportLCRegister oItem in _oExportLCRegisters)
            {
                if (sProductName != oItem.ProductName)
                {
                    #region Product Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = GetProductWiseTable();
                        #endregion

                        #region Product Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Product Wise Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.ProductName.Equals(sProductName)).Sum(x => x.Qty)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.ProductName.Equals(sProductName)).Sum(x => x.LCValue)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                        nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = GetProductWiseTable();
                    #endregion

                    #region Product Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Product Name : " + oItem.ProductName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 13; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Shipment", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Expiry", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Acc", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Acc", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("UD", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Doc. Send To Bank", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("C/P", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }

                #region Initialize Table
                oPdfPTable = GetProductWiseTable();
                #endregion

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ApplicantName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.LCValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCOpenDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmendmentDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Party + " Part", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Bank + " Part", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                string sUDRcv = (oItem.UDRcvType == EnumUDRcvType.No_Receive ? "No" : "Yes");
                _oPdfPCell = new PdfPCell(new Phrase(sUDRcv, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.NoteQuery, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPersonName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
               
                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                sProductName = oItem.ProductName;
                sCurrency = oItem.Currency;
            }

            #region Initialize Table
            oPdfPTable = GetProductWiseTable();
            #endregion

            #region Product Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Product Wise Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.ProductName.Equals(sProductName)).Sum(x => x.Qty)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportLCRegisters[0].Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.ProductName.Equals(sProductName)).Sum(x => x.LCValue)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(_oExportLCRegisters.Sum(x=>x.Qty)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Sum(x => x.LCValue)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion
        #region Bank Wise Body
        private PdfPTable GetBankWiseTable()
        {
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(16);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    100f,  //Buyer Name 
                                                    65f,  //LC No
                                                    50f, //P.I No
                                                    55f,  //Qty_LC                                               
                                                    55f,  //Qty_DO                                              
                                                    55f,  //Qty_DC                                             
                                                    55f,  //Value             
                                                    55f,  //Value_DO            
                                                    55f,  //Value_DC          
                                                    50f,  //Value_YetTo        
                                                    27f,  //Acc
                                                    27f,  //Acc
                                                    27f,  //UD
                                                    69f,  //Query
                                                    50f //C.P
                                             });
            #endregion
            return oPdfPTable;
        }
        private PdfPTable GetBankWiseTable2()
        {
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(14);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    100f,  //Buyer Name 
                                                    65f,  //LC No
                                                    50f, //P.I No
                                                    140f, //Commdoity                                            
                                                    60f,  //Qty          
                                                    60f,  //Value             
                                                    55f,  //SD             
                                                    55f,  //ED             
                                                    30f,  //Acc
                                                    30f,  //Acc
                                                    30f,  //UD
                                                    69f,  //Query
                                                    50f //C.P
                                             });
            #endregion
            return oPdfPTable;
        }
        private void PrintBodyBankWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = GetBankWiseTable();
            #endregion
            //_oExportLCRegisters = _oExportLCRegisters.OrderBy(x => x.NegoBankBranchID).ThenBy(x => x.NegoBankName).ToList();
            int nBankID = 0, nCount = 0, nExportLCID = 0; string sCurrency = "";
            foreach (ExportLCRegister oItem in _oExportLCRegisters)
            {
                if (nBankID != oItem.NegoBankBranchID)
                {
                    #region Bank Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = GetBankWiseTable();
                        #endregion

                        #region Bank Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Bank Wise Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Qty)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Qty_DO)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Qty_DC)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Amount)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Value_DO)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Value_DC)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Amount_YetToInvoice)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                        nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = GetBankWiseTable();
                    #endregion

                    #region Product Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.NegoBankName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 16; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("DO Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("DC Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("DO Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("DC Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Pending Inv.", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Acc", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Acc", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("UD", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Doc. Send To Bank", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("C/P", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }
                #region Initialize Table
                oPdfPTable = GetBankWiseTable();
                #endregion
                int nRowSpan = 1;
                nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = nRowSpan; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ApplicantName, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = nRowSpan; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty_DO), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty_DC), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Value_DO), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Value_DC), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Amount_YetToInvoice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
               
                _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Party + " Part", _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Bank + " Part", _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                string sUDRcv = (oItem.UDRcvType == EnumUDRcvType.No_Receive ? "No" : "Yes");
                _oPdfPCell = new PdfPCell(new Phrase(sUDRcv, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.NoteQuery, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPersonName, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                
                oPdfPTable.CompleteRow();

                #region Insert Into Main Table
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
                nExportLCID = oItem.ExportLCID;
                nBankID = oItem.NegoBankBranchID;
                sCurrency = oItem.Currency;
            }


            #region Initialize Table
            oPdfPTable = GetBankWiseTable();
            #endregion

            #region Product Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Bank Wise Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Qty)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportLCRegisters[0].Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Amount)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(_oExportLCRegisters.Sum(x => x.Qty)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Sum(x => x.Amount)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintBodyBankWiseMerge()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = GetBankWiseTable();
            #endregion

            //_oExportLCRegisters = _oExportLCRegisters.OrderBy(x => x.NegoBankBranchID).ThenBy(x => x.NegoBankName).ToList();
            int nBankID = 0, nCount = 0, nExportLCID = 0, nImportPIID=0; string sCurrency = "";
            foreach (ExportLCRegister oItem in _oExportLCRegisters.OrderBy(x=>x.ExportLCID).ThenBy(x=>x.ExportPIID))
            {
                if (nBankID != oItem.NegoBankBranchID)
                {
                    #region Bank Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = GetBankWiseTable();
                        #endregion

                        #region Bank Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Bank Wise Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Qty)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Qty_DO)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Qty_DC)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Amount)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Value_DO)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Value_DC)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Amount_YetToInvoice)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                       
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = GetBankWiseTable();
                    #endregion

                    #region Product Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.NegoBankName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 16; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("DO Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("DC Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("DO Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("DC Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Pending Inv.", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Acc", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Acc", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("UD", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Doc. Send To Bank", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("C/P", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    nCount = 0;
                    #endregion
                }

                int nRowSpan = 1, nRowSpanPI=1; //
                if (nExportLCID != oItem.ExportLCID)
                {
                    if (oPdfPTable.Rows.Count > 0 && nCount > 0)
                    {
                        #region SubTotal
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID && x.ExportLCID == nExportLCID).Sum(x => x.Qty)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID && x.ExportLCID == nExportLCID).Sum(x => x.Qty_DO)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID && x.ExportLCID == nExportLCID).Sum(x => x.Qty_DC)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID && x.ExportLCID == nExportLCID).Sum(x => x.Amount)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID && x.ExportLCID == nExportLCID).Sum(x => x.Value_DO)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID && x.ExportLCID == nExportLCID).Sum(x => x.Value_DC)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID && x.ExportLCID == nExportLCID).Sum(x => x.Amount_YetToInvoice)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                    }

                    #region Initialize Table
                    oPdfPTable = GetBankWiseTable();
                    #endregion

                    nCount++;
                    nRowSpan = _oExportLCRegisters.Where(x => x.ExportLCID == oItem.ExportLCID && x.NegoBankBranchID == oItem.NegoBankBranchID).Count() + 1;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = nRowSpan; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ApplicantName, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = nRowSpan; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                }

                if (nImportPIID != oItem.ExportPIID)
                {
                    nRowSpanPI = _oExportLCRegisters.Where(PIR => PIR.ExportLCID == oItem.ExportLCID && PIR.ExportPIID == oItem.ExportPIID).ToList().Count;
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpanPI; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                nImportPIID = oItem.ExportPIID;

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle)); //_oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty_DO), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty_DC), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Value_DO), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Value_DC), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Amount_YetToInvoice), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                if (nExportLCID != oItem.ExportLCID)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Party + " Part", _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Bank + " Part", _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    string sUDRcv = (oItem.UDRcvType == EnumUDRcvType.No_Receive ? "No" : "Yes");
                    _oPdfPCell = new PdfPCell(new Phrase(sUDRcv, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.NoteQuery, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPersonName, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                oPdfPTable.CompleteRow();

                nExportLCID = oItem.ExportLCID;
                nBankID = oItem.NegoBankBranchID;
                sCurrency = oItem.Currency;

            }

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Initialize Table
            oPdfPTable = GetBankWiseTable();
            #endregion

            #region Product Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Bank Wise Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Qty)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportLCRegisters[0].Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Amount)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(_oExportLCRegisters.Sum(x => x.Qty)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Sum(x => x.Amount)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        private void PrintBodyBankWise2()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            #region Initialize Table
            PdfPTable oPdfPTable = GetBankWiseTable2();
            #endregion
            //_oExportLCRegisters = _oExportLCRegisters.OrderBy(x => x.NegoBankBranchID).ThenBy(x => x.NegoBankName).ToList();
            int nBankID = 0, nCount = 0, nExportLCID = 0; string sCurrency = "";
            foreach (ExportLCRegister oItem in _oExportLCRegisters)
            {
                if (nBankID != oItem.NegoBankBranchID)
                {
                    #region Bank Wise Sub Total
                    if (oPdfPTable.Rows.Count > 0)
                    {
                        #region Initialize Table
                        oPdfPTable = GetBankWiseTable2();
                        #endregion

                        #region Bank Wise Sub Total
                        _oPdfPCell = new PdfPCell(new Phrase("Bank Wise Sub Total :", _oFontStyleBold));
                        _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Qty)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Amount)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                        nCount = 0;
                    }
                    #endregion

                    #region Header
                    #region Blank Row
                    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Table Initialize
                    oPdfPTable = GetBankWiseTable2();
                    #endregion

                    #region Product Heading
                    _oPdfPCell = new PdfPCell(new Phrase("Bank Name : " + oItem.NegoBankName, _oFontStyleBoldUnderLine));
                    _oPdfPCell.Colspan = 14; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Header Row
                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Buyer Name", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No ", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("P/I No", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Shipment", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Expiry", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Acc", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Acc", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("UD", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Doc. Send To Bank", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("C/P", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion

                    #region Insert Into Main Table
                    _oPdfPCell = new PdfPCell(oPdfPTable);
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                    #endregion
                }

                int nRowSpan = 1;
                if (nExportLCID != oItem.ExportLCID)
                {
                    if (oPdfPTable.Rows.Count > 0 && nCount > 0)
                    {
                        #region SubTotal
                        _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID && x.ExportLCID == nExportLCID).Sum(x => x.Qty)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID && x.ExportLCID == nExportLCID).Sum(x => x.Amount)), _oFontStyleBold));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                        #endregion

                        #region Insert Into Main Table
                        _oPdfPCell = new PdfPCell(oPdfPTable);
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion
                    }
                    #region Initialize Table
                    oPdfPTable = GetBankWiseTable2();
                    #endregion

                    nCount++;
                    nRowSpan = _oExportLCRegisters.Where(x => x.ExportLCID == oItem.ExportLCID && x.NegoBankBranchID == oItem.NegoBankBranchID).Count() + 1;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = nRowSpan; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ApplicantName, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Rowspan = nRowSpan; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.LCNo, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if (nExportLCID != oItem.ExportLCID)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ShipmentDateSt, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ExpiryDateSt, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Party + " Part", _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Acc_Bank + " Part", _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    string sUDRcv = (oItem.UDRcvType == EnumUDRcvType.No_Receive ? "No" : "Yes");
                    _oPdfPCell = new PdfPCell(new Phrase(sUDRcv, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.NoteQuery, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.MKTPersonName, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                oPdfPTable.CompleteRow();

                nExportLCID = oItem.ExportLCID;
                nBankID = oItem.NegoBankBranchID;
                sCurrency = oItem.Currency;

            }

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Initialize Table
            oPdfPTable = GetBankWiseTable2();
            #endregion

            #region Product Wise Sub Total
            _oPdfPCell = new PdfPCell(new Phrase("Bank Wise Sub Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Qty)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportLCRegisters[0].Currency + " " + Global.MillionFormat(_oExportLCRegisters.Where(x => x.NegoBankBranchID == nBankID).Sum(x => x.Amount)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyleBold));
            _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(_oExportLCRegisters.Sum(x => x.Qty)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(_oExportLCRegisters.Sum(x => x.Amount)), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Party Wise Body Test
        private PdfPTable GetPartyWiseTable()
        {
            #region Initialize Table
            PdfPTable oPdfPTable = new PdfPTable(13);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 
                                                    30f,  //SL 
                                                    65f,  //LC No
                                                    50f, //P.I No
                                                    160f,  //Product
                                                    60f,  //Qty                                                
                                                    60f,  //Value                                                  
                                                    55f,  //Shipment Date
                                                    55f,  //Expire Date
                                                    25f,  //Acc
                                                    25f,  //Acc
                                                    25f,  //UD
                                                    115f,  //Query
                                                    50f //C.P
                                             });
            #endregion
            return oPdfPTable;
        }
        private void PrintPartyWiseTotal(double Qty, double Amount, string sCurrency, string sHeader)
        {
            PdfPTable oPdfPTable = GetPartyWiseTable();
            #region Sub Total
            #region Party Wise Sub Total
            
            _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyleBold));
            _oPdfPCell.Colspan = 4; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(Qty), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(Amount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #endregion
        }
        private void PrintPartyWiseDetailMergeByLC(List<ExportLCRegister> oTempExportLCRegisters, int nCount, bool isPrintSubTotal) 
        {
            string sCurrency = oTempExportLCRegisters.Select(x=>x.Currency).FirstOrDefault();
            PdfPTable oPdfPTable = GetPartyWiseTable();//Initialize Table

             int nRowSpanPI = 1,  nRowSpan = 1, nImportPIID=0; bool IsPrint=true;
            if (oTempExportLCRegisters.Count() > 1)
                nRowSpan = oTempExportLCRegisters.Count()+1;

            int nProductCount = 0;
            foreach (ExportLCRegister oItem in oTempExportLCRegisters.OrderBy(x => x.ExportLCID).ThenBy(x => x.ExportPIID))
            {
                nProductCount++;
                if(IsPrint)
                {
                    #region nRowSpan Part
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTempExportLCRegisters.Select(x => x.LCNo).FirstOrDefault(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                                    #endregion
                }

                if (nImportPIID != oItem.ExportPIID)
                {
                    nRowSpanPI = oTempExportLCRegisters.Where(PIR => PIR.ExportLCID == oItem.ExportLCID && PIR.ExportPIID == oItem.ExportPIID).ToList().Count;
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));
                    _oPdfPCell.Rowspan = nRowSpanPI; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                }
                nImportPIID = oItem.ExportPIID;
                //_oPdfPCell = new PdfPCell(new Phrase(oItem.PINo, _oFontStyle));  _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Qty), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                if(IsPrint)
                {
                     #region nRowSpan Part2
                    _oPdfPCell = new PdfPCell(new Phrase(oTempExportLCRegisters.Select(x=>x.ShipmentDateSt).FirstOrDefault(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTempExportLCRegisters.Select(x=>x.ExpiryDateSt).FirstOrDefault(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTempExportLCRegisters.Select(x=>x.Acc_Party).FirstOrDefault()+ " Part", _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTempExportLCRegisters.Select(x=>x.Acc_Bank).FirstOrDefault() + " Part", _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    string sUDRcv = (oTempExportLCRegisters.Select(x=>x.UDRcvType).FirstOrDefault() == EnumUDRcvType.No_Receive ? "No" : "Yes");
                    _oPdfPCell = new PdfPCell(new Phrase(sUDRcv, _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTempExportLCRegisters.Select(x=>x.NoteQuery).FirstOrDefault(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oTempExportLCRegisters.Select(x=>x.MKTPersonName).FirstOrDefault(), _oFontStyle)); _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                    #endregion
                }
                IsPrint = false;
            }

            #region Sub Total
            if (isPrintSubTotal && nProductCount > 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Sub Total :", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oTempExportLCRegisters.Sum(x => x.Qty)), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(sCurrency + " " + Global.MillionFormat(oTempExportLCRegisters.Sum(x => x.Amount)), _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            #endregion
            
            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintPartyWiseDetails(List<ExportLCRegister> oExportLCRegisters, bool isPrintSubTotal ) 
        {
            int nCount = 0;
            
            //Table Initialize
            PdfPTable oPdfPTable = GetPartyWiseTable();
            #region Supplier Heading
            _oPdfPCell = new PdfPCell(new Phrase("Party Name : " + oExportLCRegisters[0].ApplicantName, _oFontStyleBoldUnderLine));
            _oPdfPCell.Colspan = 13; _oPdfPCell.FixedHeight = 14f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Header Row
            _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC No ", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PI No ", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Shipment Date", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Expired Date", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ACC", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ACC", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("UD", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Doc. Send To Bank", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("C/P", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Insert Into Main Table
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            if (oExportLCRegisters.Count() > 0)
            {
                while (oExportLCRegisters.Count() > 0)
                {
                    List<ExportLCRegister> oTempExportLCRegisterLCWise = new List<ExportLCRegister>();
                    oTempExportLCRegisterLCWise = oExportLCRegisters.Where(x => x.ExportLCID == oExportLCRegisters.First().ExportLCID).ToList();
                    oExportLCRegisters.RemoveAll(x => x.ExportLCID == oTempExportLCRegisterLCWise.First().ExportLCID);
                    this.PrintPartyWiseDetailMergeByLC(oTempExportLCRegisterLCWise, ++nCount, isPrintSubTotal);
                }
            }
        }
        private void PrintBodyPartyWise()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 7.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            string sCurrency = "";
            double dQty = _oExportLCRegisters.Sum(x => x.Qty), dAmount = _oExportLCRegisters.Sum(x => x.Amount);
            sCurrency = _oExportLCRegisters[0].Currency;

            if(_oExportLCRegisters.Count() > 0)
            {  
                while (_oExportLCRegisters.Count() > 0)
                {
                    List<ExportLCRegister> oTempExportLCRegisterPartyWise = new List<ExportLCRegister>();
                    oTempExportLCRegisterPartyWise = _oExportLCRegisters.Where(x => x.ApplicantID == _oExportLCRegisters.First().ApplicantID).ToList();
                    _oExportLCRegisters.RemoveAll(x => x.ApplicantID == oTempExportLCRegisterPartyWise.First().ApplicantID);

                    bool isPrintSubTotal = false;
                    if (oTempExportLCRegisterPartyWise.Select(x=>x.ExportLCID).Distinct().ToList().Count > 1)
                        isPrintSubTotal = true;
                    double dSubQty = oTempExportLCRegisterPartyWise.Sum(x => x.Qty), dSubAmount = oTempExportLCRegisterPartyWise.Sum(x => x.Amount);
                    this.PrintPartyWiseDetails(oTempExportLCRegisterPartyWise, isPrintSubTotal);
                    this.PrintPartyWiseTotal(dSubQty, dSubAmount, sCurrency, "Party Wise Sub Total :");
                }
            }
            PrintPartyWiseTotal(dQty, dAmount,sCurrency, "Grand Total :");
        }
        #endregion

        #region Blank Row
        private void PrintBlankRow() 
        {
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        #endregion
        #region Summart Top (Buyer)
        public byte[] PrepareReportSUmmary(List<ExportLCRegister> oExportLCRegisters, Company oCompany, int nReportLayout, string sDateRange,string sExportLCType)
        {
            _oExportLCRegisters = oExportLCRegisters;
            _oCompany = oCompany;
            _sExportLCType = sExportLCType;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4); //595X842
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
            this.PrintHeader("Party Wise Summary", sDateRange);
            this.PrintBodySummary();
            _oPdfPTable.HeaderRows = 3;
            #endregion

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        private void PrintBodySummary()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
            _oFontStyleBoldUnderLine = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);


            //PdfPTable oPdfPTable = new PdfPTable(5);
            //oPdfPTable.SetWidths(new float[] { 25f, 50f, 42f, 42f, 42f, 40f, 42f, 30f, 42f, 42f, 42f, 42f, 42f, 25f });

            //#region PO Info
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Summary(Order Type)", 0, 12, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL));
            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, _oFontStyleBold);
            //oPdfPTable.CompleteRow();
            //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            //#endregion
            //foreach (var oItem in oDUOrderType)
            //{
            //List<RSFreshDyedYarn> oRSFreshDyedYarns =new List<RSFreshDyedYarn>();// _oRSFreshDyedYarns.Where(x => x.OrderType == oItem.OrderType).ToList();
            #region Fabric Info
            PdfPTable oPdfPTable = new PdfPTable(5);
            oPdfPTable.SetWidths(new float[] { 20f, 100f, 30f, 50f, 50f });
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "SL", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Buyer Name", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "L/C Count", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "L/C Value", 0, 0, Element.ALIGN_CENTER, BaseColor.GRAY, true, 0, _oFontStyleBold);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Invoice Value", 0, 0, Element.ALIGN_LEFT, BaseColor.GRAY, true, 0, _oFontStyleBold);
           

            oPdfPTable.CompleteRow();
            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            #endregion

            //   NoOfBag = grp.Sum(p => p.NoOfBag),
            int nSLNo = 0;
            foreach (ExportLCRegister oItem1 in _oExportLCRegisters)
            {
                nSLNo++;
                oPdfPTable = new PdfPTable(5);
                oPdfPTable.SetWidths(new float[] { 20f, 100f, 30f, 50f, 50f });
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, nSLNo.ToString(), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.ApplicantName, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, (Math.Round( oItem1.Qty)).ToString(), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.Currency + "" + Global.MillionFormat(oItem1.LCValue), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, oItem1.Currency+""+ Global.MillionFormat(oItem1.Qty_Invoice), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyle);
              
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }
            var oLCSummary = _oExportLCRegisters.GroupBy(x => new { x.Currency }, (key, grp) =>
                                       new
                                       {
                                           Qty = grp.Sum(p => p.Qty),
                                           LCValue = grp.Sum(p => p.LCValue),
                                           Qty_Invoice = grp.Sum(p => p.Qty_Invoice),
                                           Currency = key.Currency

                                       }).ToList();
            foreach (var oItem1 in oLCSummary)
            {
                oPdfPTable = new PdfPTable(5);
                oPdfPTable.SetWidths(new float[] { 20f, 100f, 30f, 50f, 50f });
                
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.LCValue), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem1.Qty_Invoice), 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, true, 0, _oFontStyleBold);
                oPdfPTable.CompleteRow();
                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, _oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            }



        }
        #endregion
    }
}
