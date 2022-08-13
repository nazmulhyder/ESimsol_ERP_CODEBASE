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
    public class rptExportPI
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        int _nTotalColumn = 1;
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        bool _bIsInKg = true;
        int _nTitleTypeInImg = 0;
        ExportPI _oExportPI = new ExportPI();
        List<ExportPIDetail> _oExportPIDetails = new List<ExportPIDetail>();
        ExportPIPrintSetup _oExportPIPrintSetup = new ExportPIPrintSetup();
        List<ExportPITandCClause> _oExportPITandCClauses = new List<ExportPITandCClause>();
        List<PISizerBreakDown> _oPISizerBreakDowns = new List<PISizerBreakDown>();
        List<SizeCategory> _oSizeCategories = new List<SizeCategory>();
        List<ColorCategory> _oColorCategories = new List<ColorCategory>();

        BankBranch _oBankBranch = new BankBranch();
        Phrase _oPhrase = new Phrase();
        BusinessUnit _oBusinessUnit = new BusinessUnit();        
        #endregion

        public byte[] PrepareReport(ExportPI oExportPI, Company oCompany, bool bPrintFormat, int nTitleTypeInImg, BusinessUnit oBusinessUnit)
        {
            _oBusinessUnit = oBusinessUnit;
            _oExportPI = oExportPI;
            _oExportPIDetails = oExportPI.ExportPIDetails;
            _oPISizerBreakDowns = oExportPI.PISizerBreakDowns;
            _oColorCategories = oExportPI.ColorCategories;
            _oSizeCategories = oExportPI.SizeCategories;
            _oExportPITandCClauses = oExportPI.ExportPITandCClauses;
            _oExportPIPrintSetup = oExportPI.ExportPIPrintSetup;
            _oCompany = oCompany;
            _bIsInKg = bPrintFormat;
            _nTitleTypeInImg = nTitleTypeInImg;            

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 30f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 
                                                    595f //Articale
                                              });
            #endregion

            this.PrintHeader();
            this.ReporttHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Header
        private void ReporttHeader()
        {
            #region Proforma Invoice Heading Print
            if (_oExportPI.ApprovedBy == 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unauthorised ", FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLDITALIC)));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.MinimumHeight = 10; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion
        }
        #endregion
        #region Report Header
        private void PrintHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            if (_nTitleTypeInImg == 1)//normal
            {
                _oPdfPCell = new PdfPCell(this.PrintHeader_Common());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else if (_nTitleTypeInImg == 2)//pad
            {
                PrintHeader_Blank();
            }
            else if (_nTitleTypeInImg == 3)//imge
            {
                _oPdfPCell = new PdfPCell(this.LoadCompanyLogo());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }


            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("PROFORMA INVOICE", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 1f; _oPdfPCell.FixedHeight = 25; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private void PrintHeader_Blank()
        {
            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 9;
            _oPdfPCell.FixedHeight = 150f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 262.5f, 262.5f });

            #region PI No & Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("P.I.No : " + _oExportPI.PINo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 9.0f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oExportPI.IssueDate.ToString("MMMM dd, yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 262.5f, 262.5f });

            #region Buyer & PI Information
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("APPLICANT/BUYER NAME:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BENEFICIARY:", _oFontStyle));
            _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ContractorAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Concern : " + _oExportPI.ContractorContactPersonName + ", Cell: " + _oExportPI.ContractorContactPersonPhone, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Concern : " + _oExportPI.MKTPName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0.5f; _oPdfPCell.BorderWidthLeft = 1f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Detalil Print
            if (_oExportPI.ProductNature == EnumProductNature.Poly)
            {
                #region Poly
                PdfPTable oDetailPdfPTable = new PdfPTable(8);
                oDetailPdfPTable.SetWidths(new float[] {
                                                    20f,//SL: 
                                                    110f, //Item Name
                                                    125f,//measeurement
                                                    110f, //Item description
                                                    38f, //Color Qty
                                                    40f,//qty
                                                    45f,//Price
                                                    47f});//amount


                #region Heading
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Item Name", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Measurement", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Style Desc", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Color Qty", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty (" + _oExportPIDetails[0].MUName + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.RateUnit > 1 ? "Price/" + _oExportPI.RateUnit + " (" + _oExportPIDetails[0].MUName + ")" : "Price/" + _oExportPIDetails[0].MUName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportPI.Currency + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                oDetailPdfPTable.CompleteRow();
                #endregion

                if (_oExportPIDetails.Count > 0)
                {
                    int nTempCount = 0; double nTotalAmount = 0, nTotalQty = 0;

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    foreach (ExportPIDetail oItem in _oExportPIDetails)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase((++nTempCount).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.Measurement, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ColorQty.ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + " " + oItem.UnitPrice.ToString("0.0000"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        nTotalQty += oItem.Qty;
                        nTotalAmount += oItem.Amount;
                        oDetailPdfPTable.CompleteRow();
                    }
                    #region Total
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                    _oPdfPCell.Colspan = 5; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + " " + Global.MillionFormat(nTotalAmount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                    oDetailPdfPTable.CompleteRow();
                    #endregion
                    #region amount in word
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    string sAmountInWord = "Amount In Word : ";
                    if (_oExportPI.Currency == "$")
                    {
                        sAmountInWord += "US " + Global.DollarWords(nTotalAmount);
                    }
                    else if (_oExportPI.Currency == "TK")
                    {
                        sAmountInWord += Global.TakaWords(nTotalAmount);
                    }
                    else if (_oExportPI.Currency == "GBP")
                    {
                        sAmountInWord += Global.PoundWords(nTotalAmount);
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(sAmountInWord, _oFontStyle));
                    _oPdfPCell.Colspan = 8; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                    oDetailPdfPTable.CompleteRow();
                    #endregion
                }
                //insert into Main table
                _oPdfPCell = new PdfPCell(oDetailPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion
            }
            else
            {
                #region Hanger
                PdfPTable oDetailPdfPTable = new PdfPTable(6);
                oDetailPdfPTable.SetWidths(new float[] { 20f, 190f, 190f, 40f, 50f, 50f });


                #region Heading
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Description Of Goods", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Style Desc", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty (" + _oExportPIDetails[0].MUName + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.RateUnit > 1 ? "Price/" + _oExportPI.RateUnit + " (" + _oExportPIDetails[0].MUName + ")" : "Price/" + _oExportPIDetails[0].MUName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount(" + _oExportPI.Currency + ")", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                oDetailPdfPTable.CompleteRow();

                #region Insert into Main Table
                _oPdfPCell = new PdfPCell(oDetailPdfPTable);
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #endregion

                if (_oExportPIDetails.Count > 0)
                {
                    int nTempCount = 0; double nTotalAmount = 0, nTotalQty = 0;
                    string sTempGoodDescription = "";
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    foreach (ExportPIDetail oItem in _oExportPIDetails)
                    {
                        #region Table Initialize
                        oDetailPdfPTable = new PdfPTable(6);
                        oDetailPdfPTable.SetWidths(new float[] { 20f, 190f, 190f, 40f, 50f, 50f });
                        #endregion

                        _oPdfPCell = new PdfPCell(new Phrase((++nTempCount).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                        //make dynamic good description
                        if (oItem.IsApplySizer)
                        {
                            sTempGoodDescription = oItem.ProductName;
                        }
                        else
                        {
                            if (oItem.ProductDescription != null)
                            {
                                sTempGoodDescription = oItem.ProductDescription + "\n";
                            }
                            sTempGoodDescription += oItem.ReferenceCaption+"# " + oItem.ProductName;
                            if (oItem.SizeName != "") { sTempGoodDescription += ", SIZE: " + oItem.SizeName; }
                            if (oItem.ColorID != 0) { sTempGoodDescription += "\nCOLOR: " + oItem.ColorName; }
                            if (oItem.ModelReferenceID != 0) { sTempGoodDescription += ", MODEL: " + oItem.ModelReferenceName; }
                        }
                        _oPdfPCell = new PdfPCell(new Phrase(sTempGoodDescription, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.StyleNo, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(oItem.Qty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + " " + oItem.UnitPrice.ToString("0.0000"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + " " + Global.MillionFormat(oItem.Amount), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                        nTotalQty += oItem.Qty;
                        nTotalAmount += oItem.Amount;
                        oDetailPdfPTable.CompleteRow();

                        #region Insert into Main Table
                        _oPdfPCell = new PdfPCell(oDetailPdfPTable);
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        #endregion

                    }

                    #region Table Initialize
                    oDetailPdfPTable = new PdfPTable(6);
                    oDetailPdfPTable.SetWidths(new float[] { 20f, 190f, 190f, 40f, 50f, 50f });
                    #endregion

                    #region Total
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                    _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(nTotalQty), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.Currency + " " + Global.MillionFormat(nTotalAmount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                    oDetailPdfPTable.CompleteRow();
                    #endregion
                    #region amount in word
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    string sAmountInWord = "Amount In Word : ";
                    if (_oExportPI.Currency == "$")
                    {
                        sAmountInWord += "US " + Global.DollarWords(nTotalAmount);
                    }
                    else if (_oExportPI.Currency == "TK")
                    {
                        sAmountInWord += Global.TakaWords(nTotalAmount);
                    }
                    else if (_oExportPI.Currency == "GBP")
                    {
                        sAmountInWord += Global.PoundWords(nTotalAmount);
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(sAmountInWord, _oFontStyle));
                    _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oDetailPdfPTable.AddCell(_oPdfPCell);
                    oDetailPdfPTable.CompleteRow();
                    #endregion

                    #region Insert into Main Table
                    _oPdfPCell = new PdfPCell(oDetailPdfPTable);
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion
                }
                #endregion
            }
            #endregion

            if (_oPISizerBreakDowns.Count > 0)
            {
                #region Color Size Ratio
                _oPdfPCell = new PdfPCell(new Phrase("Sizer BreakDown :", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(GetColorSizeTable());
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Transport Mode, Place of Shipment & Place of Destination
            oPdfPTable = new PdfPTable(4);
            oPdfPTable.SetWidths(new float[] { 186.66f, 186.66f, 186.67f, 186.67f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SHIPMENT TERM", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF SHIPMENT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MODE OF TRANSPORT", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PLACE OF DESTINATION", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ShipmentTermSt, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfShipment, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.ShipmentBy, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPIPrintSetup.PlaceOfDelivery, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 3f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Terms & Conditions Data :
            _oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(GetTermsAndConditionTable()));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ADVISING BANK
            #region ADVISING BANK
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("ADVISING BANK:", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion

            #region Bank Data
            _oPdfPCell = new PdfPCell(_oPdfPCell = new PdfPCell(GetAdviseBankTable()));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            #endregion
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Authorized Signature
            oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 200f, 100f, 2f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.AcceptanceBy, _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.ExportPIPrintSetup.For, _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.FixedHeight = 40f; _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            if (_oExportPI.Signature == null)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oExportPI.Signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 30f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8.0f, iTextSharp.text.Font.NORMAL);
            if (_oExportPI.ExportPIPrintSetup.AcceptanceBy == "" || _oExportPI.ExportPIPrintSetup.AcceptanceBy == "N/A")
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();



            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            this.Note();
        }

        #endregion

        #region Function
        private PdfPTable GetTermsAndConditionTable()
        {
            PdfPTable oTermsAndConditionTable = new PdfPTable(7);
            oTermsAndConditionTable.WidthPercentage = 100;
            oTermsAndConditionTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oTermsAndConditionTable.SetWidths(new float[] {                                                                 
                                                                100f, //Style Name 
                                                                40f,  // Dept
                                                                145f, //Description/Composition 
                                                                50f,  //Shipment Date
                                                                50f,  //Quantity
                                                                40f,  //Unit Price
                                                                50f,  //Amount 
                                                          });

            _oFontStyle = FontFactory.GetFont("Tahoma", 10.0f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("TERMS & CONDITIONS :", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0;
            oTermsAndConditionTable.AddCell(_oPdfPCell);
            oTermsAndConditionTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int i = 0;

            #region Deafult PI Terms
            string sTerms = "";
            if (_oExportPI.LCTermID > 0)
            {
                sTerms = "Payment Mode : Should be in Irrevocable Letter of credit " + _oExportPI.LCTermsName + "/Acceptance";
            }
            if (!string.IsNullOrEmpty(sTerms))
            {
                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString() + ") " + sTerms, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);
                oTermsAndConditionTable.CompleteRow();
            }
            sTerms = "";
            if (_oExportPI.IsBBankFDD)
            {
                sTerms = sTerms + "Payment should be made in US Dollar by FDD Drawn on Bangladesh Bank, Dhaka, Bangladesh";
            }

            if (!string.IsNullOrEmpty(sTerms))
            {
                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString() + ") " + sTerms, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);
                oTermsAndConditionTable.CompleteRow();
            }
            sTerms = "";
            if (_oExportPI.IsLIBORRate)
            {
                sTerms = "Interest has to be paid at LIBOR for Usance Period.";
                if (_oExportPI.OverdueRate > 0)
                {
                    sTerms = sTerms + " and ";
                }
                else
                {
                    sTerms = sTerms + ".";
                }

            }
            if (_oExportPI.OverdueRate > 0)
            {
                sTerms = sTerms + " Overdue interest has to be paid at @" + _oExportPI.OverdueRate.ToString() + " % P.A. for overdue period.";
            }
            if (!string.IsNullOrEmpty(sTerms))
            {
                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString() + ") " + sTerms, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);
                oTermsAndConditionTable.CompleteRow();
            }
            #endregion


            foreach (ExportPITandCClause oItem in _oExportPITandCClauses)
            {
                i++;
                _oPdfPCell = new PdfPCell(new Phrase(i.ToString() + ") " + oItem.TermsAndCondition, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.Colspan = 7;
                _oPdfPCell.Border = 0;
                oTermsAndConditionTable.AddCell(_oPdfPCell);
                oTermsAndConditionTable.CompleteRow();
            }
            return oTermsAndConditionTable;
        }
        private PdfPTable GetAdviseBankTable()
        {
            PdfPTable oAdviseBankTable = new PdfPTable(1);
            oAdviseBankTable.WidthPercentage = 100;
            oAdviseBankTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oAdviseBankTable.SetWidths(new float[] {                                                                 
                                                        530f, //Style Name                                    
                                                   });
            #region Bank Name
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.BankName, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oAdviseBankTable.AddCell(_oPdfPCell);
            #endregion

            #region Branch
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            BankBranch oBankBranch = new BankBranch();
            oBankBranch = BankBranch.Get(_oExportPI.BankBranchID, _oExportPI.CurrentUserId);
            _oPdfPCell = new PdfPCell(new Phrase(_oExportPI.BranchName + ", " + _oExportPI.BranchAddress, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oAdviseBankTable.AddCell(_oPdfPCell);
            oAdviseBankTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            string sBankAccountAndSwiftCode = "";
            if (_oExportPI.IsPrintAC)
            {
                sBankAccountAndSwiftCode = "A/C No : " + _oExportPI.BankAccountNo + " & ";
            }
            sBankAccountAndSwiftCode = sBankAccountAndSwiftCode + "Swift Code :" + oBankBranch.SwiftCode;
            _oPdfPCell = new PdfPCell(new Phrase(sBankAccountAndSwiftCode, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.Border = 0;
            oAdviseBankTable.AddCell(_oPdfPCell);
            oAdviseBankTable.CompleteRow();
            #endregion
            return oAdviseBankTable;
        }
        #endregion


        #region Color Size Ratio function
        private PdfPTable GetColorSizeTable()
        {
            PdfPTable oPdfPTable = new PdfPTable(11);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] {  70f, //Product Name
                                                65f, //Color Name
                                                50f, //Paton No
                                                45f, //Size
                                                50f, //Qty
                                                10f, //Blank Column
                                                65f, //Color Name
                                                50f, //Paton No
                                                45f, //Size
                                                45f,  //Qty
                                                50f  //Total Qty
                                            }); //total with = 540

            #region Header Row
            int nProductID = 0; int nRowSpan = 1;
            List<PISizerBreakDown> oPISizerBreakDowns = new List<PISizerBreakDown>();
            //foreach (PISizerBreakDown oItem in _oPISizerBreakDowns)
            //{
            //    if (nProductID != oItem.ProductID)
            //    {
            //        oPISizerBreakDowns = new List<PISizerBreakDown>();
            //        oPISizerBreakDowns = this.GetsByProduct(oItem.ProductID);
            //        double nTempRowSpan = Convert.ToDouble(oPISizerBreakDowns.Count) / 2;
            //        nRowSpan = nRowSpan+Convert.ToInt32(Math.Ceiling(nTempRowSpan));
            //    }
            //}

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Product", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Panton No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.BorderWidthBottom = 0f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Color", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Panton No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Size", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Qty", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion

            #region Sizer Breakdown
            nProductID = 0; nRowSpan = 1; bool bFlag = true;
            oPISizerBreakDowns = new List<PISizerBreakDown>();
            foreach (PISizerBreakDown oItem in _oPISizerBreakDowns)
            {
                if (nProductID != oItem.ProductID)
                {
                    bFlag = true;
                    oPISizerBreakDowns = new List<PISizerBreakDown>();
                    oPISizerBreakDowns = this.GetsByProduct(oItem.ProductID);
                    double nTempRowSpan = Convert.ToDouble(oPISizerBreakDowns.Count) / 2;
                    nRowSpan = Convert.ToInt32(Math.Ceiling(nTempRowSpan));
                    double nTotalQty = this.GetsTotalQty(oPISizerBreakDowns);
                    for (int i = 0; i < oPISizerBreakDowns.Count; i = i + 2)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                        if (bFlag)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(oPISizerBreakDowns[i].ProductName, _oFontStyle));
                            _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }

                        _oPdfPCell = new PdfPCell(new Phrase(oPISizerBreakDowns[i].ColorName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oPISizerBreakDowns[i].PantonNo, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oPISizerBreakDowns[i].SizeName, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Convert.ToInt32(oPISizerBreakDowns[i].Quantity).ToString(), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        if (bFlag)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                        }

                        _oPdfPCell = new PdfPCell(new Phrase((i + 1 < oPISizerBreakDowns.Count) ? oPISizerBreakDowns[i + 1].ColorName : "", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase((i + 1 < oPISizerBreakDowns.Count) ? oPISizerBreakDowns[i + 1].PantonNo : "", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase((i + 1 < oPISizerBreakDowns.Count) ? oPISizerBreakDowns[i + 1].SizeName : "", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase((i + 1 < oPISizerBreakDowns.Count) ? (Convert.ToInt32(oPISizerBreakDowns[i + 1].Quantity)).ToString() : "", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        if (bFlag)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(Convert.ToInt32(nTotalQty).ToString(), _oFontStyle));
                            _oPdfPCell.Rowspan = nRowSpan; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                            bFlag = false;
                        }
                        oPdfPTable.CompleteRow();
                    }
                }
                nProductID = oItem.ProductID;
            }
            #endregion

            return oPdfPTable;
        }

        private List<PISizerBreakDown> GetsByProduct(int nProductID)
        {
            List<PISizerBreakDown> oPISizerBreakDowns = new List<PISizerBreakDown>();
            foreach (PISizerBreakDown oItem in _oPISizerBreakDowns)
            {
                if (oItem.ProductID == nProductID)
                {
                    oPISizerBreakDowns.Add(oItem);
                }
            }
            return oPISizerBreakDowns;
        }
        private double GetsTotalQty(List<PISizerBreakDown> oPISizerBreakDowns)
        {
            double nTotalQty = 0;
            foreach (PISizerBreakDown oItem in oPISizerBreakDowns)
            {
                nTotalQty = nTotalQty + oItem.Quantity;
            }
            return nTotalQty;
        }

        #endregion

        private PdfPTable LoadCompanyLogo()
        {
            PdfPTable oPdfPTable1 = new PdfPTable(1);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 100f });
            iTextSharp.text.Image oImag;

            if (_oCompany.CompanyLogo != null)
            {
                oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                oImag.ScaleAbsolute(70f, 25f);
                oPdfPCell1 = new PdfPCell(oImag);
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell1.FixedHeight = 25f;
                oPdfPCell1.Border = 0;
                oPdfPCell1.PaddingRight = 0f;
                oPdfPCell1.BackgroundColor = BaseColor.WHITE;
                oPdfPTable1.AddCell(oPdfPCell1);
            }
            return oPdfPTable1;
        }
        private PdfPTable PrintHeader_Common()
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
                _oImag.ScaleAbsolute(60f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
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

            //_oPdfPCell = new PdfPCell(oPdfPTable);
            //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthBottom = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
            return oPdfPTable;
        }
        private void Note()
        {
            float nUsagesHeight = 0;
            nUsagesHeight = CalculatePdfPTableHeight(_oPdfPTable);


            /// Total Hight -Margian -Uses Height
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.FixedHeight = 842 - 40 - 30 - nUsagesHeight; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(this.PrintNote());
            _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private PdfPTable PrintNote()
        {
            PdfPTable oPdfPTable1 = new PdfPTable(2);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 300f, 300f });
            if (!String.IsNullOrEmpty(_oExportPI.Note))
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
                oPdfPCell1 = new PdfPCell(new Phrase("Note : " + _oExportPI.Note, _oFontStyle));
                oPdfPCell1.Border = 0; oPdfPCell1.BorderWidthBottom = 0;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPCell1.BackgroundColor = BaseColor.WHITE;
                oPdfPCell1.Colspan = 2;
                oPdfPTable1.AddCell(oPdfPCell1);
                oPdfPTable1.CompleteRow();
            }

            //_oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            //if (!String.IsNullOrEmpty(_oExportPI.ContractorContactPersonName))
            //{
            //    oPdfPCell1 = new PdfPCell(new Phrase("Buyer Concern Person: " + _oExportPI.ContractorContactPersonName, _oFontStyle));
            //}
            //else
            //{
            //    oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle));
            //}
            //_oPdfPCell.Border = 0; _oPdfPCell.BorderWidthBottom = 0;
            //oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT;
            //oPdfPCell1.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell1.Border = 0;
            //oPdfPTable1.AddCell(oPdfPCell1);
            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            //oPdfPCell1.Border = 0; oPdfPCell1.BorderWidthBottom = 0;
            //oPdfPCell1 = new PdfPCell(new Phrase(_oBusinessUnit.ShortName + " Concern Person: " + _oExportPI.MKTPName, _oFontStyle));
            //oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
            //oPdfPCell1.BackgroundColor = BaseColor.WHITE;
            //oPdfPCell1.Border = 0;
            //oPdfPTable1.AddCell(oPdfPCell1);
            //oPdfPTable1.CompleteRow();

            return oPdfPTable1;
        }
        public static float CalculatePdfPTableHeight(PdfPTable table)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Document doc = new Document(PageSize.TABLOID))
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        table.TotalWidth = 500f;
                        table.WriteSelectedRows(0, table.Rows.Count, 0, 0, w.DirectContent);

                        doc.Close();
                        return table.TotalHeight;
                    }
                }
            }
        }
    }

}