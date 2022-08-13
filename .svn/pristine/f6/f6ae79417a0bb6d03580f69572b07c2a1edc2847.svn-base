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
    public class rptPurchaseTakeOutOrginalDoc
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        Phrase _oPhrase = new Phrase();
        Paragraph _oParagraph;
        Chunk _oChunk = new Chunk();
        iTextSharp.text.Image _oImag;
        PdfWriter _oWriter;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        ImportInvoice _oImportInvoice = new ImportInvoice();
        ImportLC _oImportLC = new ImportLC();
        ImportLetterSetup _oImportLetterSetup = new ImportLetterSetup();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        Contractor _oContractor = new Contractor();
        List<ImportPack> _oImportPacks = new List<ImportPack>();
        List<ImportPackDetail> _oImportPackDetails = new List<ImportPackDetail>();
        ImportCnf _oImportCnf = new ImportCnf();
        int nCount = 0;
        string _sMUnit = "";
        double _nTotalValue = 0;
 

        #endregion


        #region Request For LC Letter(From Setup)
        public byte[] PreparePrintLetter(ImportLetterSetup oImportLetterSetup, ImportInvoice oImportInvoice, Company oCompany, BusinessUnit oBusinessUnit,  ImportLC oImportLC)
        {
            _oBusinessUnit = oBusinessUnit;
            _oImportInvoice = oImportInvoice;
            _oCompany = oCompany;
            _oImportLC = oImportLC;
            _oImportLetterSetup = oImportLetterSetup;
            
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(30f, 40f, 20f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion
            this.PrintHeader();
            this.PrintBody_Letter();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Body
        private void PrintBody_Letter()
        {
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10, iTextSharp.text.Font.NORMAL);

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region RefNo
            if (!String.IsNullOrEmpty(_oImportLetterSetup.RefNo))
            {
                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.RefNo + _oImportInvoice.FileNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Date
            if (_oImportLetterSetup.IsPrintDateObject)
            {
                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oImportInvoice.DateofBankInfo.ToString("MMM dd , yyyy"), _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region Date Current
            if (_oImportLetterSetup.IsPrintDateCurrentDate)
            {
                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("MMM dd , yyyy"), _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion


            #region Blank spacc
            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region To
            if (!String.IsNullOrEmpty(_oImportLetterSetup.To))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.To, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Manager Print
            if (!String.IsNullOrEmpty(_oImportLetterSetup.ToName))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.ToName, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion


            if (_oImportLetterSetup.IssueToType == (int)EnumImportLetterIssueTo.Bank)
            {
                #region Negotiate Bank Name
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.BankName_Nego, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Negotiate Bank Branch
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.BBranchName_Nego, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Negotiate Bank Address
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.BankAddress_Nego, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            if (_oImportLetterSetup.IssueToType == (int)EnumImportLetterIssueTo.Supplier)
            {
                #region Negotiate Bank Name
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.ContractorName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Negotiate Bank Branch
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.ContractorAddress, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Negotiate Bank Address
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }

            #region Blank spacc

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Subject

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Subject))
            {
                if (_oImportLetterSetup.Subject.Contains("@LCNO"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCNO", _oImportInvoice.ImportLCNo + " DT:" + _oImportInvoice.ImportLCDateInString);
                }
                if (_oImportLetterSetup.Subject.Contains("@AMOUNT"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@AMOUNT", _oImportInvoice.AmountSt);
                }
                if (_oImportLetterSetup.Subject.Contains("@AMOUNTLC"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@AMOUNTLC", _oImportInvoice.AmountLCInSt);
                }
                if (_oImportLetterSetup.Subject.Contains("@LCPAYMENTTYPE"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCPAYMENTTYPE", _oImportInvoice.LCPaymentType);
                }
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk("Subject: ", _oFontStyle));
                _oPhrase.Add(new Chunk(_oImportLetterSetup.Subject + " " + _oImportLetterSetup.SubjectTwo, _oFontStyleBold));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion


            #region Blank spacc

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Dear sir

            _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.DearSir, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            //#region Message
            //_oPdfPCell = new PdfPCell(new Phrase("To meet the requirement of demand for raw material to meet the upcoming export order, we need to open the LC for raw material against the PI's mention below:", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

            #region BODY1

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Body1))
            {
                if (_oImportLetterSetup.Body1.Contains("@LCPAYMENTTYPE"))
                {
                    _oImportLetterSetup.Body1 = _oImportLetterSetup.Body1.Replace("@LCPAYMENTTYPE", _oImportInvoice.LCPaymentType);
                }

                // _oImportLetterSetup.Body1 = _oImportLetterSetup.Body1.Replace("@PINO", _sPurchaseContactNo);
                _oPhrase = new Phrase();
                var rx = new System.Text.RegularExpressions.Regex("@AGENT");
                var array = rx.Split(_oImportLetterSetup.Body1);

                int nCount = 0;
                nCount = array.Length;

                _oPhrase.Add(new Chunk(array[0], _oFontStyle));
                if (_oImportLetterSetup.IsPrintPINo)
                {

                }
                if (nCount > 1)
                {
                    _oPhrase.Add(new Chunk(array[1], _oFontStyle));
                    _oPhrase.Add(new Chunk(_oImportLetterSetup.ContractorName, _oFontStyleBold));
                }

                _oParagraph = new Paragraph(_oPhrase);
                _oParagraph.SetLeading(0f, 2f);
                _oParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(_oParagraph);
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


            }
            #endregion
            #region Blank spacc

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Supplier Name
            if (!String.IsNullOrEmpty(_oImportLetterSetup.SupplierName))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.SupplierName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.ContractorName, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

          
            }
            #endregion
            
            #region LC No
            if (!String.IsNullOrEmpty(_oImportLetterSetup.LCNo))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.LCNo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.ImportLCNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
              
            }
            #endregion
            #region LC Value
            if (!String.IsNullOrEmpty(_oImportLetterSetup.LCValue))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.LCValue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.AmountLCInSt, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region InvoiceNo
            if (!String.IsNullOrEmpty(_oImportLetterSetup.InvoiceNo))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.InvoiceNo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.ImportInvoiceNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region InvoiceValue
            if (!String.IsNullOrEmpty(_oImportLetterSetup.InvoiceValue))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.InvoiceValue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.AmountSt, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            #region PI Bank
            if (!String.IsNullOrEmpty(_oImportLetterSetup.PIBank))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.PIBank, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
               /// if need Gets PI Bank and Show
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.BankAddress_Nego, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            #endregion
            #region Show Payment Type
            if (!String.IsNullOrEmpty(_oImportLetterSetup.LCPayType))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.LCPayType, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.LCPaymentType + " " + _oImportInvoice.LCTermsName, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("Beneficiary will be paid :" + " " + _oImportLC.LCTermsName_Bene, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.LCPaymentType + " " + _oImportInvoice.LCTermsName, _oFontStyleBold));
                //_oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            #endregion
            #region DateofMaturity
            if (_oImportLetterSetup.IsPrintMaturityDate)
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.DateofMaturityST, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            #endregion
            #region Cal culate Maturity
            if (_oImportLetterSetup.IsCalMaturityDate)
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });
                if (_oImportLC.PaymentInstructionInt == (int)EnumPaymentInstruction.BL)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase("BL No & Date", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.BLNo + " DT:" + _oImportInvoice.BLDateSt, _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                else  if (_oImportLC.PaymentInstructionInt == (int)EnumPaymentInstruction.Negotiation)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase("Negotiation Date", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.DateofNegotiationInST, _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                 else   if (_oImportLC.PaymentInstructionInt == (int)EnumPaymentInstruction.LCOpen)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase("L/C Open", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.ImportLCDate.ToString("dd MMM yyyy"), _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }
                else 
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.DateofMaturityST, _oFontStyleBold));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            #endregion
            #region BL No and Date
            if (!String.IsNullOrEmpty(_oImportLetterSetup.BLNo))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.BLNo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportInvoice.BLNo + " DT:" + _oImportInvoice.BLDateSt, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }

            #endregion

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 2f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Clauses
            #region Clause
            if (!String.IsNullOrEmpty(_oImportLetterSetup.Clause))
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oImportLetterSetup.Clause, _oFontStyle));
                //_oPhrase.Add(new Chunk(_oImportLC.ImportLCNo, _oFontStyleBold));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
           
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region BODY2
            //_oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.Body2, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            if (!String.IsNullOrEmpty(_oImportLetterSetup.Body2))
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oImportLetterSetup.Body2, _oFontStyle));
                _oParagraph = new Paragraph(_oPhrase);
                _oParagraph.SetLeading(0f, 2f);
                _oParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(_oParagraph);
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            #endregion
            #region BODY3
            //_oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.Body3, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            if (!String.IsNullOrEmpty(_oImportLetterSetup.Body3))
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oImportLetterSetup.Body3, _oFontStyle));
                _oParagraph = new Paragraph(_oPhrase);
                _oParagraph.SetLeading(0f, 2f);
                _oParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                _oPdfPCell = new PdfPCell();
                _oPdfPCell.AddElement(_oParagraph);
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            //#region Message
            //_oPdfPCell = new PdfPCell(new Phrase("We are obligated for the liabilities against the L/C opened as per our request and will arrange the required fund upon maturity of the import bill. Please arrange to open the L/C. Your earliest response & action will be highly appreciated.", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            //#endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ThankingOne
            if (!String.IsNullOrEmpty(_oImportLetterSetup.ThankingOne))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.ThankingOne, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ThankingTwo
            if (!String.IsNullOrEmpty(_oImportLetterSetup.ThankingTwo))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.ThankingTwo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 20f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Signature
            PdfPTable oPdfPTableTwo = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTableTwo.SetWidths(new float[] { 197f, 197f, 197f });


            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 9f, 0)));
            oPdfPCell.Border = 0; ;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 45f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPTableTwo.CompleteRow();

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Authorize1))
            {
                oPdfPCell = new PdfPCell(new Phrase("--------------------", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            oPdfPCell.Border = 0; ;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTableTwo.AddCell(oPdfPCell);

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Authorize2))
            {
                oPdfPCell = new PdfPCell(new Phrase("--------------------", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Authorize3))
            {
                oPdfPCell = new PdfPCell(new Phrase("--------------------", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            oPdfPCell.Border = 0; //oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            //oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPTableTwo.CompleteRow();

            if (_oImportLetterSetup.Authorize1IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            oPdfPCell.Border = 0; ;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            if (_oImportLetterSetup.Authorize2IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            if (_oImportLetterSetup.Authorize3IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            else
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            oPdfPCell.Border = 0;
            //oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPTableTwo.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.Authorize1, _oFontStyle));
            oPdfPCell.Border = 0; ;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.Authorize2, _oFontStyle));
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.Authorize3, _oFontStyle));
            oPdfPCell.Border = 0; //oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0.5f;
            //oPdfPCell.FixedHeight = 35f;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPTableTwo.CompleteRow();


            _oPdfPCell = new PdfPCell(oPdfPTableTwo);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        private void PrintBodyForPackingList()
        {


            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

            PdfPTable oPdfPTable = new PdfPTable(4);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 106f, 190f, 220f, 80f });

            if (_oImportCnf.ImportCnfID > 0)
            {
                oPdfPCell = new PdfPCell(new Phrase("Date", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportCnf.SendDateSt, _oFontStyle));
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("Document No  ", _oFontStyle));
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportCnf.FileNo, _oFontStyle));
                //oPdfPCell.Colspan = 3;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            oPdfPCell = new PdfPCell(new Phrase("Invoice No & Date", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.MinimumHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.ImportInvoiceNo + " DT: " + _oImportInvoice.InvoiceDateInString, _oFontStyleBold));
            //oPdfPCell.Colspan = 3;
            oPdfPCell.MinimumHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("Invoice Value ", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.MinimumHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.AmountSt, _oFontStyleBold));
            //oPdfPCell.Colspan = 3;
            oPdfPCell.MinimumHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("L/C No & Date", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.MinimumHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.ImportLCNo + " DT: " + _oImportInvoice.ImportLCDateInString, _oFontStyleBold));
            oPdfPCell.Colspan = 3;
            oPdfPCell.MinimumHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("SUPPLIER NAME", _oFontStyle));
            //oPdfPCell.Border = 0;
            oPdfPCell.MinimumHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(" " + _oImportInvoice.ContractorName, _oFontStyleBold));
            //oPdfPCell.Border = 0;
            oPdfPCell.Colspan = 3;
            oPdfPCell.MinimumHeight = 15;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Address_Supplier))
            {
                oPdfPCell = new PdfPCell(new Phrase("Address", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.MinimumHeight = 15;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportLetterSetup.Address_Supplier, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.MinimumHeight = 15;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            if (!String.IsNullOrEmpty(_oImportLetterSetup.Origin))
            {
                oPdfPCell = new PdfPCell(new Phrase("Country of Origin", _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.MinimumHeight = 15;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(" " + _oImportLetterSetup.Origin, _oFontStyle));
                //oPdfPCell.Border = 0;
                oPdfPCell.Colspan = 3;
                oPdfPCell.MinimumHeight = 15;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            



            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank Rows
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        private void SetPackDetail()
        {
            #region Balnk Space
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);


            PdfPTable oPdfPTable = new PdfPTable(7);
            PdfPCell oPdfPCell;
            oPdfPTable.SetWidths(new float[] { 25f, 210f, 100f, 75f, 65f, 75f, 60f });
          
            string sPack = "Pack";
            if (_oImportInvoice.ImportInvoiceDetails.Count > 0)
            {
                if (_oImportPacks.Count > 0)
                {
                    sPack = _oImportPacks[0].PackCountByInString;
                    _sMUnit = _oImportPackDetails[0].MUName;
                }
            }
            if (_oImportInvoice.ImportInvoiceDetails.Count > 0)
            {
                _sMUnit = _oImportInvoice.ImportInvoiceDetails[0].MUSymbol;
            }

            #region Grid Column
            oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("DESCRIPTION", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("LOT NO", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("NO of " + sPack, _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("QTY/" + sPack, _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("QTY(" + _sMUnit + ")", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("REMARKS", _oFontStyleBold));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            int nCount = 0;
            int nLotCount = 0;
            int nProductID = 0;
            int nRowSpan = 0;
            double nTempInvQty = 0;


            if (_oImportPackDetails.Count > 0)
            {
                foreach (ImportPackDetail oImportPackDetail in _oImportPackDetails)
                {

                    if (nProductID != oImportPackDetail.ProductID)
                    {
                        nTempInvQty = 0;

                        nRowSpan = _oImportPackDetails.Where(P => P.ProductID == oImportPackDetail.ProductID).ToList().Count;


                        if (nProductID > 0 && nProductID != oImportPackDetail.ProductID && nLotCount > 1)
                        {
                            #region Sub Total

                            oPdfPCell = new PdfPCell(new Phrase(" Total", _oFontStyleBold));
                            oPdfPCell.Colspan = 3;
                            //oPdfPCell.Rowspan = nRowSpan;
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            nTempInvQty = (_oImportPackDetails.Where(c => c.ProductID == nProductID).Sum(x => x.NumberOfPack));

                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyleBold));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            nTempInvQty = (_oImportPackDetails.Where(c => c.ProductID == nProductID).Sum(x => x.Qty));

                            oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyleBold));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                            oPdfPCell.BackgroundColor = BaseColor.WHITE;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            oPdfPTable.AddCell(oPdfPCell);

                            oPdfPTable.CompleteRow();
                            #endregion
                         
                        }
                        nLotCount = 0;
                        /////
                        nCount++;
                        oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                        oPdfPCell.Rowspan = nRowSpan; oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                        oPdfPCell = new PdfPCell(new Phrase(oImportPackDetail.ProductName, _oFontStyle));
                        //oPdfPCell.Colspan = 3;
                        oPdfPCell.Rowspan = nRowSpan;
                        oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                        oPdfPCell.BackgroundColor = BaseColor.WHITE;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                        oPdfPTable.AddCell(oPdfPCell);

                    }

                    nLotCount++;
                    oPdfPCell = new PdfPCell(new Phrase(oImportPackDetail.LotNo, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oImportPackDetail.NumberOfPack), _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oImportPackDetail.QtyPerPack), _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oImportPackDetail.Qty), _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(oImportPackDetail.Remarks, _oFontStyle));
                    //oPdfPCell.Colspan = 2;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    nProductID = oImportPackDetail.ProductID;
                }

                if (nLotCount > 1)
                {

                    #region Sub Total

                    oPdfPCell = new PdfPCell(new Phrase(" Total", _oFontStyleBold));
                    oPdfPCell.Colspan = 3;
                    //oPdfPCell.Rowspan = nRowSpan;
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    nTempInvQty = (_oImportPackDetails.Where(c => c.ProductID == nProductID).Sum(x => x.NumberOfPack));

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyleBold));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    nTempInvQty = (_oImportPackDetails.Where(c => c.ProductID == nProductID).Sum(x => x.Qty));

                    oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty), _oFontStyleBold));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                    oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();
                    #endregion

                }
                #region Grand Total

                oPdfPCell = new PdfPCell(new Phrase(" Grand Total", _oFontStyleBold));
                oPdfPCell.Colspan = 3;
                //oPdfPCell.Rowspan = nRowSpan;
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                nTempInvQty = _oImportPackDetails.Sum(x => x.NumberOfPack);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty) + "\n" + sPack, _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                nTempInvQty = _oImportPackDetails.Sum(x => x.Qty);
                oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTempInvQty) + "\n" + _sMUnit, _oFontStyleBold));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; oPdfPCell.BorderWidthTop = 0.5f; oPdfPCell.BorderWidthLeft = 0.5f; oPdfPCell.BorderWidthRight = 0.5f; oPdfPCell.BorderWidthBottom = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTable.AddCell(oPdfPCell);

                oPdfPTable.CompleteRow();
                #endregion


            }

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


        }
        #endregion
        #endregion

    
        public byte[] PrepareReport_SendToCnFLetter(ImportCnf oImportCnf, ImportLetterSetup oImportLetterSetup, ImportInvoice oPurchaseInvoice, Company oCompany, BusinessUnit oBusinessUnit, Contractor oContractor, ImportLC oImportLC, List<ImportPack> oImportPacks, List<ImportPackDetail> oImportPackDetails)
        {
            _oImportInvoice = oPurchaseInvoice;
            _oBusinessUnit = oBusinessUnit;
            _oImportLetterSetup = oImportLetterSetup;
            _oCompany = oCompany;
            _oContractor = oContractor;
            _oImportPacks = oImportPacks;
            _oImportPackDetails = oImportPackDetails;
            _oImportCnf = oImportCnf;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(20f, 20f, 30f,30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.PrintBody_SendToCnFLetter();
            this.PrintBodyForPackingList();
            this.SetPackDetail();
            _oPdfPTable.HeaderRows = 5;
            //_oPdfPTable.FooterRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            if (_oImportLetterSetup.HeaderType == 1 || _oImportLetterSetup.HeaderType == 0)//normal
            {
                _oPdfPCell = new PdfPCell(this.PrintHeader_Common());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else if (_oImportLetterSetup.HeaderType == 2)//pad
            {
                PrintHeader_Blank();
            }
            else if (_oImportLetterSetup.HeaderType == 3)//imge
            {
                _oPdfPCell = new PdfPCell(this.LoadCompanyTitle());
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                //_oPdfPCell.Colspan = _nTotalColumn;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }


          
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
                _oImag.ScaleAbsolute(62f, 40f);
                _oPdfPCell = new PdfPCell(_oImag);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.Rowspan = 2; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
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
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont("Tahoma", 5f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.BorderWidthTop = 0; _oPdfPCell.BorderWidthRight = 0; _oPdfPCell.BorderWidthLeft = 0; _oPdfPCell.BorderWidthBottom = 0.5f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
            #endregion

            #endregion
            return oPdfPTable;
        }
        private void PrintHeader_Blank()
        {
            #region Proforma Invoice Heading Print
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 9;
            _oPdfPCell.FixedHeight = 115f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.Colspan = _nTotalColumn;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        private PdfPTable LoadCompanyTitle()
        {
            PdfPTable oPdfPTable1 = new PdfPTable(1);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 400f });
            iTextSharp.text.Image oImag;

            if (_oCompany.CompanyTitle != null)
            {
                oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyTitle, System.Drawing.Imaging.ImageFormat.Jpeg);
                oImag.ScaleAbsolute(530f, 65f);
                oPdfPCell1 = new PdfPCell(oImag);
                oPdfPCell1.Border = 0;
                oPdfPCell1.FixedHeight = 100f;
                oPdfPCell1.BackgroundColor = BaseColor.WHITE;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPTable1.AddCell(oPdfPCell1);
            }
            return oPdfPTable1;
        }
        #endregion
      
        #region Report Body
      
        private void PrintBody_SendToCnFLetter()
        {


            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Free 3 of 9 Extended", 9f, 0);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
           
            #region Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oImportCnf.SendDateSt, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);

        
            #region Name
            _oPdfPCell = new PdfPCell(new Phrase(_oContractor.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Address
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oContractor.Address, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Subject

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Subject))
            {
                if (_oImportLetterSetup.Subject.Contains("@LCNO"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCNO", _oImportInvoice.ImportLCNo + " DT:" + _oImportInvoice.ImportLCDateInString);
                }
                if (_oImportLetterSetup.Subject.Contains("@AMOUNT"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@AMOUNT", _oImportInvoice.AmountSt);
                }
                if (_oImportLetterSetup.Subject.Contains("@AMOUNTLC"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@AMOUNTLC", _oImportInvoice.AmountLCInSt);
                }
                if (_oImportLetterSetup.Subject.Contains("@LCPAYMENTTYPE"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCPAYMENTTYPE", _oImportInvoice.LCPaymentType);
                }
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk("Subject: ", _oFontStyle));
                _oPhrase.Add(new Chunk(_oImportLetterSetup.Subject + " " + _oImportLetterSetup.SubjectTwo, _oFontStyleBold));
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion


            #region Blank spacc

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Dear sir

            _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.DearSir, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


        }
      
        #endregion
    }
}
