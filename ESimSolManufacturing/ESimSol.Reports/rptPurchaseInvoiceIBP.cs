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
//using ICS.Printing;
using System.Linq;


namespace ESimSol.Reports
{

    public class rptPurchaseInvoiceIBP
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();

        List<PurchaseInvoiceIBP> _oPurchaseInvoiceIBPs = new List<PurchaseInvoiceIBP>();
        Company _oCompany = new Company();
        List<Currency> _oCurrencys = new List<Currency>();
        List<BankBranch> _oBankBranchs = new List<BankBranch>();
        string _sMessage = "";
        string _sCurrency = "";
        double _nGrandTotalAmount = 0;
        double _nTotalBankAmount = 0;
        double _nTotalAmount = 0;
        double _nAmount = 0;
        int _nBankBranchID = 0;
        int _nYear = 0;
        int _nMonth = 0;
        int _nCurrencyID = 0;
        string _sCurrencyName = "";
        string _sCurrencySymbol = "";
        string _sMonthName = "";
        int _nCurrencyCount = 0;
        int _nCount = 0;
        #endregion

        public byte[] PrepareReport(List<PurchaseInvoiceIBP> oPurchaseInvoiceIBPs, Company oCompany, List<Currency> oCurrencys, List<BankBranch> oBankBranchs)
        {

            _oCompany = oCompany;
            _oPurchaseInvoiceIBPs = oPurchaseInvoiceIBPs;

            _oCurrencys = oCurrencys;
            _oBankBranchs = oBankBranchs;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 20f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });//height:842   width:595
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public byte[] PrepareReport_ABP(List<PurchaseInvoiceIBP> oPurchaseInvoiceIBPs, Company oCompany, List<Currency> oCurrencys, List<BankBranch> oBankBranchs)
        {

            _oCompany = oCompany;
            _oPurchaseInvoiceIBPs = oPurchaseInvoiceIBPs;

            _oCurrencys = oCurrencys;
            _oBankBranchs = oBankBranchs;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//595*842
            _oDocument.SetMargins(40f, 40f, 20f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });//height:842   width:595
            #endregion

            this.PrintHeader();
            this.PrintBody_ABP();
            _oPdfPTable.HeaderRows = 5;
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
            oPdfPTable.SetWidths(new float[] { 100f, 267.5f, 100f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Rowspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, 0);

            _oPdfPCell = new PdfPCell(new Phrase("Print Date:" + DateTime.Now.ToString("dd MMM yy :HH:mm") + "", _oFontStyle));

            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();




            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Blank Space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("Import Bill payable (IBP) Report", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion
          
        }

        #endregion

        #region Report Body
        private void PrintBody()
        {
         
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(7);
            oPdfPTable = new PdfPTable(7);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            oPdfPTable.SetWidths(new float[] { 30.5f, 77.2f, 77.5f, 100.5f, 50.5f, 50.5f, 50.5f });


            foreach (PurchaseInvoiceIBP oitem in _oPurchaseInvoiceIBPs)
            {
                /// Get month wise Total
                if ((_nBankBranchID != oitem.BankBranchID || _nYear != oitem.DateofMaturity.Year || _nMonth != oitem.DateofMaturity.Month) && _nYear > 0)
                {


                    _nCurrencyCount = 0;

                    foreach (Currency oCUrrency in _oCurrencys)
                    {
                        _nAmount = PurchaseInvoiceIBP.TotalMonth_Currency(_nBankBranchID, _nYear, _nMonth, oCUrrency.CurrencyID, _oPurchaseInvoiceIBPs);
                        if (_nAmount > 0)
                        {
                            _nCurrencyCount++;
                            if (_nCurrencyCount > 1)
                            {
                                _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
                                _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                            }
                            else
                            {
                                _oPdfPCell = new PdfPCell(new Phrase(_sMonthName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
                                _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                            }
                            _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.CurrencyName, _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.Symbol + "" + Global.MillionFormat(_nAmount), _oFontStyle));
                            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                         
                            oPdfPTable.CompleteRow();
                        }
                    }
                    //  _nAmount = 0;

                }
                if (_nBankBranchID != oitem.BankBranchID)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(oitem.BankName, FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.Colspan = 7; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("LC Value", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Invoice Value", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                    _nCount = 0;

                }
                _nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.PurchaseLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.PurchaseInvoiceNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.SupplierName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(oitem.Currency + "" + Global.MillionFormat(oitem.AmountLC), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.Currency + "" + Global.MillionFormat(oitem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.DateofMaturity.ToString("dd MMM yyyy"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _nBankBranchID = oitem.BankBranchID;
                _nYear = oitem.DateofMaturity.Year;
                _nMonth = oitem.DateofMaturity.Month;
                _nCurrencyID = oitem.CurrencyID;
                _sCurrencyName = oitem.CurrencyName;
                _sCurrencySymbol = oitem.Currency;
                _sMonthName = oitem.DateofMaturity.ToString("MMMM yy");
            }



            #region Sub Total again for loor end
            _nCurrencyCount = 0;
            foreach (Currency oCUrrency in _oCurrencys)
            {
                _nAmount = PurchaseInvoiceIBP.TotalMonth_Currency(_nBankBranchID, _nYear, _nMonth, oCUrrency.CurrencyID, _oPurchaseInvoiceIBPs);
                if (_nAmount > 0)
                {
                    _nCurrencyCount++;
                    if (_nCurrencyCount > 1)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
                        _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    else
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(_sMonthName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
                        _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.CurrencyName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.Symbol + "" + Global.MillionFormat(_nAmount), _oFontStyle));
                    _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                 
                    oPdfPTable.CompleteRow();
                }
            }
            /////////// For  print last Line
            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            //  _nAmount = 0;


            #endregion
            #region Summary
            _oPdfPCell = new PdfPCell(new Phrase("Summary" ,FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.BorderWidthLeft =0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

           
            oPdfPTable.CompleteRow();
            

            foreach (BankBranch oBankBranch in _oBankBranchs)
            {
                _nCurrencyCount = 0;
                foreach (Currency oCUrrency in _oCurrencys)
                {
                    _nAmount = PurchaseInvoiceIBP.TotalBank_Currency(oBankBranch.BankBranchID, oCUrrency.CurrencyID, _oPurchaseInvoiceIBPs);
                    if (_nAmount > 0)
                    {
                        _nCurrencyCount++;
                        if (_nCurrencyCount > 1)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
                            _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oBankBranch.BankName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
                            _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }
                        _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.CurrencyName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.Symbol + "" + Global.MillionFormat(_nAmount), _oFontStyle));
                        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                      
                        oPdfPTable.CompleteRow();
                    }
                }
            }
            /////////// For  print last Line
                 _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
                 _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                  oPdfPTable.CompleteRow();
            #endregion
            //  _nAmount = 0;

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        private void PrintBody_ABP()
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(15);
            oPdfPTable = new PdfPTable(15);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            oPdfPTable.SetWidths(new float[] { 30.5f, 77.2f, 77.5f, 100.5f, 50.5f, 50.5f, 50.5f, 50.5f, 50.5f, 50.5f, 50.5f, 50.5f, 50.5f, 50.5f, 80f });


            foreach (PurchaseInvoiceIBP oitem in _oPurchaseInvoiceIBPs)
            {
                /// Get month wise Total
                //if ((_nBankBranchID != oitem.BankBranchID || _nYear != oitem.DateofMaturity.Year || _nMonth != oitem.DateofMaturity.Month) && _nYear > 0)
                //{


                //    _nCurrencyCount = 0;

                //    foreach (Currency oCUrrency in _oCurrencys)
                //    {
                //        _nAmount = PurchaseInvoiceIBP.TotalMonth_Currency(_nBankBranchID, _nYear, _nMonth, oCUrrency.CurrencyID, _oPurchaseInvoiceIBPs);
                //        if (_nAmount > 0)
                //        {
                //            _nCurrencyCount++;
                //            if (_nCurrencyCount > 1)
                //            {
                //                _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
                //                _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //            }
                //            else
                //            {
                //                _oPdfPCell = new PdfPCell(new Phrase(_sMonthName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
                //                _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //            }
                //            _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.CurrencyName, _oFontStyle));
                //            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                //            _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.Symbol + "" + Global.MillionFormat(_nAmount), _oFontStyle));
                //            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                //            oPdfPTable.CompleteRow();
                //        }
                //    }
                //    //  _nAmount = 0;

                //}
                if (_nBankBranchID != oitem.BankBranchID)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(oitem.BankName, FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD)));
                    _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.Colspan =15; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();

                    _oPdfPCell = new PdfPCell(new Phrase("SL No", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Issuing Bank", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("L/C Amount", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 1; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    
                    _oPdfPCell = new PdfPCell(new Phrase("L/C Date", _oFontStyle));
                    _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 1; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                 
                    _oPdfPCell = new PdfPCell(new Phrase("Name of\n The Drawer", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("ABP No", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Tenor of Bills", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase("Date of Maturity.", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Foreign Amount", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Currency Code", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Rate", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Equivalent Taka", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                    oPdfPTable.CompleteRow();
                    _nCount = 0;

                }
                _nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.BankNickName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.PurchaseLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oitem.Amount_LC), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.PurchaseLCDateSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.SupplierName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.BUName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.ABPNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.Tenor.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.DateofAcceptanceSt, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.DateofMaturityST, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oitem.CurrencyName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat( oitem.CCRate), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oitem.Amount * oitem.CCRate), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _nBankBranchID = oitem.BankBranchID;
                _nYear = oitem.DateofMaturity.Year;
                _nMonth = oitem.DateofMaturity.Month;
                _nCurrencyID = oitem.CurrencyID;
                _sCurrencyName = oitem.CurrencyName;
                _sCurrencySymbol = oitem.Currency;
                _sMonthName = oitem.DateofMaturity.ToString("MMMM yy");

                _nTotalBankAmount = _nTotalBankAmount + (oitem.Amount * oitem.CCRate);
                _nTotalAmount = _nTotalAmount+oitem.Amount;
            }



            #region Sub Total again for loor end
            _nCurrencyCount = 0;
            //foreach (Currency oCUrrency in _oCurrencys)
            //{
            //    _nAmount = PurchaseInvoiceIBP.TotalMonth_Currency(_nBankBranchID, _nYear, _nMonth, oCUrrency.CurrencyID, _oPurchaseInvoiceIBPs);
            //    if (_nAmount > 0)
            //    {
            //        _nCurrencyCount++;
            //        if (_nCurrencyCount > 1)
            //        {
            //            _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
            //            _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //        }
            //        else
            //        {
            //            _oPdfPCell = new PdfPCell(new Phrase(_sMonthName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            //            _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //        }
            //        _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.CurrencyName, _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //        _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.Symbol + "" + Global.MillionFormat(_nAmount), _oFontStyle));
            //        _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


            //        oPdfPTable.CompleteRow();
            //    }
            //}
            /////////// For  print last Line
            //_oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
            //_oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();
            //  _nAmount = 0;


            #endregion
            #region Summary
            _oPdfPCell = new PdfPCell(new Phrase("Total", FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.Colspan = 11; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalAmount) , FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalBankAmount), FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            _oPdfPCell.BorderWidthLeft = 0.5f; _oPdfPCell.BorderWidthRight = 0.5f; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            //foreach (BankBranch oBankBranch in _oBankBranchs)
            //{
            //    _nCurrencyCount = 0;
            //    foreach (Currency oCUrrency in _oCurrencys)
            //    {
            //        _nAmount = PurchaseInvoiceIBP.TotalBank_Currency(oBankBranch.BankBranchID, oCUrrency.CurrencyID, _oPurchaseInvoiceIBPs);
            //        if (_nAmount > 0)
            //        {
            //            //_nCurrencyCount++;
            //            if (_nCurrencyCount > 1)
            //            {
            //                _oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
            //                _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //            }
            //            else
            //            {
            //                _oPdfPCell = new PdfPCell(new Phrase(oBankBranch.BankName, FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD)));
            //                _oPdfPCell.BorderWidthLeft = 1; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //            }
            //            //_oPdfPCell = new PdfPCell(new Phrase(oCUrrency.CurrencyName, _oFontStyle));
            //            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //            _oPdfPCell = new PdfPCell(new Phrase(oCUrrency.Symbol + "" + Global.MillionFormat(_nAmount), _oFontStyle));
            //            _oPdfPCell.Colspan = 15; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

            //            oPdfPTable.CompleteRow();
            //        }
            //    }
            //}
            /////////// For  print last Line
            //_oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD)));
            //_oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthTop = 1; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();
            #endregion
            //  _nAmount = 0;

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        #endregion


    }
}

