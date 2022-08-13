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
    public class rptImportClaimLetterIssue
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
        ImportClaim _oImportClaim = new ImportClaim();
        List<ImportPI> _oImportPIs = new List<ImportPI>();
        ImportLetterSetup _oImportLetterSetup = new ImportLetterSetup();
        string sMessage = "";
        string _sMargins = "";
        string _sLCPaymentType = "";
        string _sPurchaseContactNo = "";
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        #endregion

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

        #region Request For LC Letter(From Setup)
        public byte[] PreparePrintLetter(ImportClaim oImportClaim, ImportLetterSetup oImportLetterSetup, List<ImportPI> oImportPIs, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oImportClaim = oImportClaim;
            _oImportPIs = oImportPIs;
            _oCompany = oCompany;
            _oImportLetterSetup = oImportLetterSetup;
            _oBusinessUnit = oBusinessUnit;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(60f, 60f, 10f, 05f);
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
            //int nELDBPCount = _oImportClaim_Clauses.Count;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 8, iTextSharp.text.Font.NORMAL);


            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            //_oFontStyle = FontFactory.GetFont("Free 3 of 9 Extended", 9f, 0);
            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region RefNo
            if (!String.IsNullOrEmpty(_oImportLetterSetup.RefNo))
            {
                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.RefNo +" "+ _oImportClaim.ClaimNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Date
            if (_oImportLetterSetup.IsPrintDateObject)
            {
                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oImportClaim.ImportLCDate.ToString("MMM dd , yyyy"), _oFontStyleBold));
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
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
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


            if (_oImportLetterSetup.IssueToType == (int)EnumImportLetterIssueTo.Supplier)
            {
                #region Supplier Name
                _oPdfPCell = new PdfPCell(new Phrase(_oImportClaim.ContractorName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Supplier Branch
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));//_oImportClaim.BBranchName_Nego
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Supplier Address
                //_oPdfPCell = new PdfPCell(new Phrase(_oImportClaim.BankAddress_Nego, _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();


                PdfPTable oPdfPTable = new PdfPTable(2);
                oPdfPTable.SetWidths(new float[] { 200f, 180f });

                //_oImportClaim.BankAddress_Nego
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            if (_oImportLetterSetup.IssueToType == (int)EnumImportLetterIssueTo.Supplier)
            {
                #region Supplier Name
                _oPdfPCell = new PdfPCell(new Phrase(_oImportClaim.ContractorName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Supplier Branch
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Supplier Address
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

            #region Blank spacc

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Subject

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Subject))
            {

                //if (_oImportClaim.LCMargin <= 0)
                //{
                //    _sMargins = "Zero margin";
                //}
                //if (_oImportClaim.LCMargin > 0)
                //{
                //    _sMargins = Math.Round(_oImportClaim.LCMargin, 0) + " % margin";
                //}

                //if (_oImportClaim.LCPaymentType > EnumLCPaymentType.REGULAR)
                //{
                //    _sLCPaymentType = _oImportClaim.LCPaymentTypeSt;
                //}

                if (_oImportLetterSetup.Subject.Contains("@LCPAYMENTTYPE"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCPAYMENTTYPE", _sLCPaymentType);
                }
                if (_oImportLetterSetup.Subject.Contains("@LCNO"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCNO", _oImportClaim.ClaimNo + " DT:" + _oImportClaim.IssueDateST);
                }
                _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@AMOUNT", _oImportClaim.ImportInvoiceAmountST);
                _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCTERM", "");
                _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCMARGIN", "");
                _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@BALANCEAMOUNT", "" ); //+ Global.MillionFormat(_oImportClaim.));
                //_oImportLetterSetup.Ip
                //LCPaymentType

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
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
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

            foreach (ImportPI oItem in _oImportPIs)
            {
                _sPurchaseContactNo += oItem.ImportPINo + " Dt. " + oItem.IssueDate.ToString("dd MMM yyyy");
                if (_oImportPIs.Count == 2)
                {
                    _sPurchaseContactNo += " & ";
                }
                else if (_oImportPIs.Count > 2)
                {
                    _sPurchaseContactNo += ",";
                }
            }

            if (_oImportPIs.Count > 1)
            {
                _sPurchaseContactNo = _sPurchaseContactNo.Substring(0, _sPurchaseContactNo.Length - 1);
            }


            #region BODY1
            if (!String.IsNullOrEmpty(_oImportLetterSetup.Body1))
            {
                // _oImportLetterSetup.Body1 = _oImportLetterSetup.Body1.Replace("@PINO", _sPurchaseContactNo);
                _oPhrase = new Phrase();
                var rx = new System.Text.RegularExpressions.Regex("@PINO");
                var array = rx.Split(_oImportLetterSetup.Body1);

                int nCount = 0;
                nCount = array.Length;

                _oPhrase.Add(new Chunk(array[0], _oFontStyle));
                //if (_oImportLetterSetup.IsPrintPINo)
                //{

                //}
                if (nCount > 1)
                {
                    _oPhrase.Add(new Chunk(array[1], _oFontStyle));
                    _oPhrase.Add(new Chunk(_sPurchaseContactNo, _oFontStyleBold));
                }

                //_oParagraph = new Paragraph(_oPhrase);
                //_oParagraph.SetLeading(0f, 2f);
                //_oParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                _oPdfPCell = new PdfPCell();
                //_oPdfPCell.AddElement(_oParagraph);
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


            }
            #endregion
            #region Blank spacc

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 5f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
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
                _oPdfPCell = new PdfPCell(new Phrase(_oImportClaim.ClaimNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                //_oPhrase = new Phrase();
                //_oPhrase.Add(new Chunk(_oImportLetterSetup.LCNo, _oFontStyle));
                //_oPhrase.Add(new Chunk(_oImportClaim.ClaimNo, _oFontStyleBold));

                //_oPdfPCell = new PdfPCell(_oPhrase);
                //_oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();
            }
            #endregion
            #region Master LC No
            //if (!String.IsNullOrEmpty(_oImportLetterSetup.MasterLCNo) && _oImportClaim.LCAppType == EnumLCAppType.B2BLC)
            //{
            //    PdfPTable oPdfPTable = new PdfPTable(4);
            //    oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.MasterLCNo, _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.MasterLCs, _oFontStyleBold));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            //    oPdfPTable.CompleteRow();

            //    _oPdfPCell = new PdfPCell(oPdfPTable);
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //    //_oPhrase = new Phrase();
            //    //_oPhrase.Add(new Chunk(_oImportLetterSetup.LCNo, _oFontStyle));
            //    //_oPhrase.Add(new Chunk(_oImportClaim.ClaimNo, _oFontStyleBold));

            //    //_oPdfPCell = new PdfPCell(_oPhrase);
            //    //_oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    //_oPdfPTable.CompleteRow();
            //}
            //#endregion
            //#region Supplier Name
            //if (!String.IsNullOrEmpty(_oImportLetterSetup.SupplierName))
            //{
            //    PdfPTable oPdfPTable = new PdfPTable(4);
            //    oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

            //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.SupplierName, _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPCell = new PdfPCell(new Phrase(_oImportClaim.ContractorName, _oFontStyleBold));
            //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //    oPdfPTable.CompleteRow();

            //    if (_oImportLetterSetup.IsPrintSupplierAddress)
            //    {
            //        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPCell = new PdfPCell(new Phrase("Address", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.ContractorAddress, _oFontStyle));
            //        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            //        oPdfPTable.CompleteRow();
            //    }

            //    _oPdfPCell = new PdfPCell(oPdfPTable);
            //    _oPdfPCell.Border = 0;
            //    _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();


            //}
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
                if (_oImportPIs.Count > 0)
                {
                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(_oImportPIs[0].BankName, _oFontStyleBold));
                    if (!String.IsNullOrEmpty(_oImportLetterSetup.PIBankAddress))
                    {
                        _oPhrase.Add(new Chunk("\n" + _oImportPIs[0].BranchName, _oFontStyle));
                        if (!String.IsNullOrEmpty(_oImportPIs[0].SwiftCode))
                        {
                            _oPhrase.Add(new Chunk(" \nSwiftCode:" + _oImportPIs[0].SwiftCode, _oFontStyleBold));
                        }
                    }
                    _oPdfPCell = new PdfPCell(_oPhrase);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                }
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
                _oPdfPCell = new PdfPCell(new Phrase(_oImportClaim.ClaimNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                //_oPhrase = new Phrase();
                //_oPhrase.Add(new Chunk(_oImportLetterSetup.LCNo, _oFontStyle));
                //_oPhrase.Add(new Chunk(_oImportClaim.ClaimNo, _oFontStyleBold));

                //_oPdfPCell = new PdfPCell(_oPhrase);
                //_oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();
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
                _oPdfPCell = new PdfPCell(new Phrase(_oImportClaim.ImportInvoiceAmountST, _oFontStyleBold));
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

            #region Detail PI Info
            if (_oImportLetterSetup.IsPrintPINo)
            {
                PdfPTable oPdfPTable = new PdfPTable(5);
                oPdfPTable.SetWidths(new float[] { 40f, 280f, 70f, 70f, 70f });

                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Commodity", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Qty", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);
                
                _oPdfPCell = new PdfPCell(new Phrase("Unit Price", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                int nCount = 0;
                if (_oImportClaim.ImportClaimDetails.Count > 0)
                {
                    foreach (ImportClaimDetail oItem in _oImportClaim.ImportClaimDetails)
                    {
                        nCount++;
                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString("00"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                       // _oPdfPCell = new PdfPCell(new Phrase(oItem.ImportPINo + " DT: " + oItem.IssueDate.ToString("dd MMM yy"), _oFontStyle));
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ProductName + " ", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.MUnit + "" + Global.MillionFormat(oItem.Qty), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.UnitPrice), _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                    }
                }
                else
                {
                    //foreach (ImportPI oItem in _oImportPIs)
                    //{
                    //    nCount++;
                    //    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString("00"), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(oItem.ImportPINo + " DT: " + oItem.IssueDateSt, _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase(_oImportClaim.CurrencyName + "" + Global.MillionFormat(oItem.TotalValue), _oFontStyle));
                    //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    //    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    //    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    //    oPdfPTable.CompleteRow();
                    //}
                }
                if (nCount > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    double nValue = _oImportClaim.ImportClaimDetails.Select(x => x.Qty).Sum();
                    _oPdfPCell = new PdfPCell(new Phrase(_oImportClaim.ImportClaimDetails[0].MUnit + "" + Global.MillionFormat(nValue), _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    nValue = _oImportClaim.ImportClaimDetails.Select(x => x.Amount).Sum();
                    _oPdfPCell = new PdfPCell(new Phrase("" + Global.MillionFormat(nValue), _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    oPdfPTable.CompleteRow();
                }

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Clauses
            //#region Clause
            //if (!String.IsNullOrEmpty(_oImportLetterSetup.Clause))
            //{
            //    _oPhrase = new Phrase();
            //    _oPhrase.Add(new Chunk(_oImportLetterSetup.Clause, _oFontStyle));

            //    _oPdfPCell = new PdfPCell(_oPhrase);
            //    _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //}
            //#endregion
            
            float nBSHight = 0;
            
            //if (_oImportLetterSetup.IsPrinTnC)
            //{
            //    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            //    foreach (ImportClaim_Clause oItem in _oImportClaim_Clauses)
            //    {
            //        _oPhrase = new Phrase();
            //        if (!String.IsNullOrEmpty(oItem.Caption))
            //        {
            //            _oPhrase.Add(new Chunk("(*) " + oItem.Caption + " ", _oFontStyleBold));
            //        }
            //        else
            //        {
            //            _oPhrase.Add(new Chunk("(*) ", _oFontStyleBold));
            //        }

            //        _oPhrase.Add(new Chunk(oItem.Clause, _oFontStyle));
            //        _oParagraph = new Paragraph(_oPhrase);
            //        _oParagraph.SetLeading(0f, 0f);
            //        _oParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            //        _oPdfPCell = new PdfPCell();
            //        _oPdfPCell.AddElement(_oParagraph);
            //        _oPdfPCell = new PdfPCell(new Phrase("(*) " + oItem.Clause, _oFontStyle));
            //        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //        _oPdfPCell.Border = 0;
            //        //_oPdfPCell.MinimumHeight = 10;
            //        _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //        _oPdfPTable.AddCell(_oPdfPCell);
            //        _oPdfPTable.CompleteRow();
            //    }
            //    #region Blank space
            //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //    #endregion
            //    if (_oImportClaim_Clauses.Count > 8 && _oImportClaim_Clauses.Count < 10)
            //    {
            //        nBSHight = 5;
            //    }
            //    else if (_oImportClaim_Clauses.Count > 10)
            //    {
            //        nBSHight = 1;
            //    }

            //}
            //else
            //{
            //    nBSHight = 25;
            //    #region Blank space
            //    _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            //    _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nBSHight; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //    _oPdfPTable.CompleteRow();
            //    #endregion
            //}
            #endregion

            #region BODY2
            //_oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.Body2, _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            if (!String.IsNullOrEmpty(_oImportLetterSetup.Body2))
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oImportLetterSetup.Body2, _oFontStyle));
                //_oParagraph = new Paragraph(_oPhrase);
                //_oParagraph.SetLeading(0f, 2f);
                //_oParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                //_oPdfPCell = new PdfPCell();
                //_oPdfPCell.AddElement(_oParagraph);
                //_oPdfPCell.AddElement(_oParagraph);
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = nBSHight; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
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
                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = nBSHight; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
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
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = nBSHight; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ThankingTwo
            if (!String.IsNullOrEmpty(_oImportLetterSetup.ThankingTwo))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.ThankingTwo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = nBSHight; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Signature
            PdfPTable oPdfPTableTwo = new PdfPTable(3);
            PdfPCell oPdfPCell;
            oPdfPTableTwo.SetWidths(new float[] { 197f, 197f, 197f });


            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 9f, 0)));
            oPdfPCell.Border = 0; ;
            oPdfPCell.FixedHeight = 35;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 35;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 8f, 2)));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = 35;
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

        #endregion
        #endregion
    }
}
