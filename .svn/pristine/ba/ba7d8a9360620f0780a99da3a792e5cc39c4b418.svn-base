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

    public class rptImportPaymentRequest
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
        ImportLetterSetup _oImportLetterSetup = new ImportLetterSetup();
        Company _oCompany = new Company();
        ImportPaymentRequest _oImportPR = new ImportPaymentRequest();
        List<ImportPaymentRequestDetail> _oImportPaymentRequestDetails = new List<ImportPaymentRequestDetail>();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        double _nTotalValue = 0;
        int nCount = 0;
        #endregion

        public byte[] PrepareReport(ImportPaymentRequest oImportPaymentRequest, ImportLetterSetup oImportLetterSetup, Company oCompany,  BusinessUnit oBusinessUnit)
        {
            _oImportPR = oImportPaymentRequest;
            _oImportPaymentRequestDetails = oImportPaymentRequest.ImportPaymentRequestDetails;
            _oCompany = oCompany;
            _oImportLetterSetup = oImportLetterSetup;
            _oBusinessUnit = oBusinessUnit;
            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle( 595f,842f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(60f, 60f, 30f, 30f);
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
            _oPdfPTable.HeaderRows = 6;
           // _oPdfPTable.FooterRows = 2;
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
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.RefNo + _oImportPR.RefNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Date
            if (_oImportLetterSetup.IsPrintDateObject)
            {
                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oImportPR.LetterIssueDate.ToString("MMM dd , yyyy"), _oFontStyleBold));
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
                _oPdfPCell = new PdfPCell(new Phrase(_oImportPR.BankName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Negotiate Bank Branch
                _oPdfPCell = new PdfPCell(new Phrase(_oImportPR.BankBranchName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Negotiate Bank Address
                _oPdfPCell = new PdfPCell(new Phrase(_oImportPR.BranchAddress, _oFontStyle));
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
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCNO","");
                }
                if (_oImportLetterSetup.Subject.Contains("@AMOUNT"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@AMOUNT", "");
                }
                if (_oImportLetterSetup.Subject.Contains("@AMOUNTLC"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@AMOUNTLC", "");
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
                // _oImportLetterSetup.Body1 = _oImportLetterSetup.Body1.Replace("@PINO", _sPurchaseContactNo);
                _oPhrase = new Phrase();
                var rx = new System.Text.RegularExpressions.Regex("@LVNO");
                var array = rx.Split(_oImportLetterSetup.Body1);

             
                nCount = array.Length;

                _oPhrase.Add(new Chunk(array[0], _oFontStyle));
                //if (_oImportLetterSetup.IsPrintPINo)
                //{

                //}
                if (nCount > 1)
                {
                    _oPhrase.Add(new Chunk(array[1], _oFontStyle));
                    _oPhrase.Add(new Chunk("", _oFontStyleBold));
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
            #region Invoice Purchase Detail
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 40f, 240f, 140f, 110f, 100f, 100f });

            //if (_oImportPaymentRequest.CurrencyType == true)
            //{
            /////////For Local Discount////////////
            #region Detail Table header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Maturity Dat", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            foreach (ImportPaymentRequestDetail oItem in _oImportPaymentRequestDetails)
            {
                nCount++;

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString("00"), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ImportLCNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.ImportInvoiceNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oItem.Currency + " " + (Global.MillionFormat(oItem.Amount)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DateofMaturityST, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _nTotalValue += oItem.Amount;
                oPdfPTable.CompleteRow();


            }
      

            #region Total Print

            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalValue), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion
        


            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
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

            #region Account No Message
            if (_oImportLetterSetup.IsPrintPINo)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                if (_oImportPR.LiabilityType == EnumLiabilityType.Accounttransfer)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Please arrange to make the payment from our Foreign Currency Account No- " + _oImportPR.AccountNo + ".", _oFontStyle));
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Please arrange to make payment by creating " + _oImportPR.LiabilityType.ToString(), _oFontStyle));
                }
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion


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
        private void PrintBody()
        {
            
            #region Blank spacc
                 if(_oImportPR.ApprovedBy==0)
            { 
                   
            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Not authorize", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
                 }
            #endregion
            

            int nELDBPCount = _oImportPaymentRequestDetails.Count;
            string sBills = "bill";
            if (nELDBPCount > 1)
            {
                sBills = "bills";
            }
            else
            {
                sBills = "bill";
            }

            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            
            _oFontStyle = FontFactory.GetFont("Free 3 of 9 Extended", 9f, 0);
            #region Date RF No
            _oPdfPCell = new PdfPCell(new Phrase("Ref No: " + _oImportPR.RefNo, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Date Print
            _oPdfPCell = new PdfPCell(new Phrase("Date: " + _oImportPR.LetterIssueDateInString, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight= 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
           
            #region Manager Print
            _oPdfPCell = new PdfPCell(new Phrase("The Manager ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            #region Advice Bank Name
            _oPdfPCell = new PdfPCell(new Phrase(_oImportPR.BankName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Branch Name
            _oPdfPCell = new PdfPCell(new Phrase(_oImportPR.BranchName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Branch Address
            _oPdfPCell = new PdfPCell(new Phrase(_oImportPR.BranchAddress, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
             _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Subject 
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            if (_oImportPR.LiabilityType == EnumLiabilityType.Accounttransfer)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Subject : Request for import bill settlement.", _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Subject: Request for Local Bill Purchased.", _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Dear sir
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Dear Sir,", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Message 

        
            _oPdfPCell = new PdfPCell(new Phrase("With through this letter, we would like to request you to settle the following import " + sBills + "", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
           
            #endregion

            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Invoice Purchase Detail
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 40f, 240f, 140f, 110f, 100f, 100f });

            //if (_oImportPaymentRequest.CurrencyType == true)
            //{
                /////////For Local Discount////////////
                #region Detail Table header
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("L/C No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Supplier Name", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("Invoice No", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("Maturity Dat", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
                #endregion

                int nCount = 0;
                
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                foreach (ImportPaymentRequestDetail oItem in _oImportPaymentRequestDetails)
                {
                    nCount++;

                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString("00"), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ImportLCNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ContractorName, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ImportInvoiceNo, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                

                    _oPdfPCell = new PdfPCell(new Phrase( oItem.Currency+" "+(Global.MillionFormat(oItem.Amount)), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DateofMaturityST, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _nTotalValue += oItem.Amount;
                    oPdfPTable.CompleteRow();

                 
                }
                //for (int i = 0; i <= (8 - nCount); i++)
                //{
                //    nCount++;
                  
                //    _oPdfPCell = new PdfPCell(new Phrase(".", _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);



                //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                  
                //    oPdfPTable.CompleteRow();

                //}

                #region Total Print

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalValue), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                #endregion
                ///////// end Local///////////////
            //}
            //else
            //{
                ///////////For Local Discount////////////
                //#region Detail Table header
                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                //_oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Issueing Bank", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Value in US$", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("LDBC#", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);


                //_oPdfPCell = new PdfPCell(new Phrase("Maturity Date", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //oPdfPTable.CompleteRow();
                //#endregion

                //int nCount = 0;
                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                //foreach (ImportPaymentRequestDetail oItem in _oImportPaymentRequestDetails)
                //{
                //    nCount++;

                //    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString("00"), _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //    _oPdfPCell = new PdfPCell(new Phrase(oItem.IssueBankName, _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //    _oPdfPCell = new PdfPCell(new Phrase(oItem.LDBCNo, _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //    _oPdfPCell = new PdfPCell(new Phrase(oItem.MaturityDateStr, _oFontStyle));
                //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //    _nTotalValue += oItem.Amount;
                //    oPdfPTable.CompleteRow();
                //}

                //#region Total Print

                //_oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.BOLD);
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_nTotalValue), _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                //oPdfPTable.CompleteRow();

                //#endregion
                /////////// end Local///////////////
            //}



            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;   _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

            #region Account No Message
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            if (_oImportPR.LiabilityType == EnumLiabilityType.Accounttransfer)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Please arrange to make the payment from our Foreign Currency Account No- " + _oImportPR.AccountNo + ".", _oFontStyle));
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Please arrange to make payment by creating " + _oImportPR.LiabilityType.ToString(), _oFontStyle));
            }
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

         
            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            #region Thank you Messge
            _oPdfPCell = new PdfPCell(new Phrase("Thanking You", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.Border = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region Blank Row
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 40f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Signature Panel 
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("................................                                             ...........................................                              ....................", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature                                           Manager Trade and Finance                       Accounts Executive ", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            #endregion
        
            //#endregion

        }
        #endregion

    }
    
    

}
