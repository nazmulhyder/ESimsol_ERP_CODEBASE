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
    public class rptExportIncentive
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
        ExportIncentive _oExportIncentive = new ExportIncentive();
        ExportLC _oExportLC = new ExportLC();
        ImportLetterSetup _oImportLetterSetup = new ImportLetterSetup();
        string sMessage = "";
        string _sMargins = "";
        string _sLCPaymentType = "";
        string _sPurchaseContactNo = "";
        #endregion

        #region Report Header
        private void PrintHeader()
        {
            _oFontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8f, 0);
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

            _oFontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9f, iTextSharp.text.Font.BOLD);
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
            _oFontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 20f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 6f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));

            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 7f, 0);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.PringReportHead, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            #region ReportHeader
            #region Blank Space
            _oFontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 5f, 1);
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
            _oFontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Colspan = 9;
            _oPdfPCell.FixedHeight = 145f; _oPdfPCell.BorderWidth = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
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
        public byte[] PreparePrintLetter(ExportIncentive oExportIncentive, ImportLetterSetup oImportLetterSetup, ImportLetterSetup oImportLetterSetupFord, Company oCompany)
        {
            _oExportIncentive = oExportIncentive;
           
            _oCompany = oCompany;
            _oImportLetterSetup = oImportLetterSetup;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(50f, 50f, 10f, 05f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            this.PrintBody_Letter();

            this.NewPageDeclaration();

            _oImportLetterSetup = oImportLetterSetupFord;
            this.PrintHeader();
            this.PrintBody_Letter();

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #endregion

        #region Report Body
        private void PrintBody_Letter()
        {

            _oFontStyleBold = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 13f, iTextSharp.text.Font.NORMAL);
            ESimSolPdfHelper.FontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12f, iTextSharp.text.Font.NORMAL);
            BlankSpec(8f);
            BlankSpec(8f);

            PdfPTable oPdfPTable_RefDate = new PdfPTable(2);
            oPdfPTable_RefDate.SetWidths(new float[] { 430f, 165f });

            #region RefNo
            if (!String.IsNullOrEmpty(_oImportLetterSetup.RefNo))
            {
                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                string sRefNo = _oImportLetterSetup.RefNo;
                FindAndReplace(ref sRefNo, "@YEAR", _oExportIncentive.ApplicationDate.ToString("yyyy"));
                FindAndReplace(ref sRefNo, "@SLNO", _oExportIncentive.SLNo.ToString());//"0000"

                _oPdfPCell = new PdfPCell(new Phrase(sRefNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable_RefDate.AddCell(_oPdfPCell);
            }
            #endregion

            #region Date
            if (_oImportLetterSetup.IsPrintDateObject)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oExportIncentive.ApplicationDate.ToString("MMM dd , yyyy"), _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPTable_RefDate.AddCell(_oPdfPCell); oPdfPTable_RefDate.CompleteRow();
            }
            else if (_oImportLetterSetup.IsPrintDateCurrentDate)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("MMM dd , yyyy"), _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable_RefDate.AddCell(_oPdfPCell);
                oPdfPTable_RefDate.CompleteRow();
            }
            else 
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable_RefDate.AddCell(_oPdfPCell);
                oPdfPTable_RefDate.CompleteRow();
            }
            #endregion

            ESimSolPdfHelper.AddTable(ref _oPdfPTable, oPdfPTable_RefDate, 0, 0, 0);

            BlankSpec(20);

            #region To
            if (!String.IsNullOrEmpty(_oImportLetterSetup.To))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.To, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Manager Print
            if (!String.IsNullOrEmpty(_oImportLetterSetup.ToName))
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.ToName, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            if (_oImportLetterSetup.IssueToType == (int)EnumImportLetterIssueTo.Bank)
            {
                #region Advice Bank Name
                _oPdfPCell = new PdfPCell(new Phrase(_oExportIncentive.BankName_Advice, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Advice Bank Branch
                _oPdfPCell = new PdfPCell(new Phrase(_oExportIncentive.BBranchName_Advice, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Advice Bank Address
                _oPdfPCell = new PdfPCell(new Phrase(_oExportIncentive.BBranchAddress_Advice, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                PdfPTable oPdfPTable = new PdfPTable(2);
                oPdfPTable.SetWidths(new float[] { 200f, 180f });

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
            if (_oImportLetterSetup.IssueToType == (int)EnumImportLetterIssueTo.IssueBank)
            {
                #region ISSUE Bank Name
                _oPdfPCell = new PdfPCell(new Phrase(_oExportIncentive.BankName_Issue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region ISSUE Bank Branch
                _oPdfPCell = new PdfPCell(new Phrase(_oExportIncentive.BBranchName_Issue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region ISSUE Bank Address
                _oPdfPCell = new PdfPCell(new Phrase(_oExportIncentive.BBranchAddress_Issue, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
               
            
                PdfPTable oPdfPTable = new PdfPTable(2);
                oPdfPTable.SetWidths(new float[] { 200f, 180f });

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
                #region ApplicantName
                _oPdfPCell = new PdfPCell(new Phrase(_oExportIncentive.ApplicantName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Negotiate Bank Branch
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Negotiate Bank Address
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

            }

            BlankSpec(12f);

            #region Subject

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Subject))
            {
                string sSubject = _oImportLetterSetup.Subject;
                FindAndReplace(ref sSubject, "@LCNO", _oExportIncentive.ExportLCNo + " DT:" + _oExportIncentive.LCRecivedDateST);
                FindAndReplace(ref sSubject, "@MASTERLCNO",  _oExportIncentive.MasterLCNo);
                FindAndReplace(ref sSubject, "@AMOUNT", _oExportIncentive.Amount_ST);
                FindAndReplace(ref sSubject, "@APPLICANTNAME", _oExportIncentive.ApplicantName);
                _oImportLetterSetup.Subject = sSubject;

                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk("Subject: ", _oFontStyle));
                _oPhrase.Add(new Chunk(_oImportLetterSetup.Subject + " " + _oImportLetterSetup.SubjectTwo, _oFontStyleBold));
                ESimSolPdfHelper.AddParagraph(ref _oPdfPTable, _oPhrase, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 0, 0);
                //_oPdfPCell = new PdfPCell(_oPhrase);
                //_oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion


            BlankSpec(10f);

            #region Dear sir
            ESimSolPdfHelper.AddParagraph(ref _oPdfPTable, new Phrase(_oImportLetterSetup.DearSir, _oFontStyle), Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 0, 0);
           _oPdfPTable.CompleteRow();
            #endregion

            BlankSpec(8f);

            #region BODY1
            if (!String.IsNullOrEmpty(_oImportLetterSetup.Body1))
            {
                _oPhrase = new Phrase();

                if(_oImportLetterSetup.Body1.Contains("@ISSUEBANK"))
                {
                      var rx = new System.Text.RegularExpressions.Regex("@ISSUEBANK");
                    var array = rx.Split(_oImportLetterSetup.Body1);
      
                    _oPhrase.Add(new Chunk(array[0], _oFontStyle));
                    if (array.Length > 1)
                    {
                        _oPhrase.Add(new Chunk(_oExportIncentive.BankName_Issue
                                                + (string.IsNullOrEmpty(_oExportIncentive.BBranchName_Issue) ? "" : ", " + _oExportIncentive.BBranchName_Issue)
                                                + (string.IsNullOrEmpty(_oExportIncentive.BBranchAddress_Issue) ? "" : ", " + _oExportIncentive.BBranchAddress_Issue)
                                                , _oFontStyleBold));

                        _oPhrase.Add(new Chunk(array[1], _oFontStyle));
                        _oPhrase.Add(new Chunk(_sPurchaseContactNo, _oFontStyleBold));
                    }
                }
                else if (_oImportLetterSetup.Body1.Contains("@MASTERLCNO"))
                {
                    var rx = new System.Text.RegularExpressions.Regex("@MASTERLCNO");
                    var array = rx.Split(_oImportLetterSetup.Body1);
      
                    _oPhrase.Add(new Chunk(array[0], _oFontStyle));
                    if (array.Length > 1)
                    {
                        _oPhrase.Add(new Chunk((string.IsNullOrEmpty(_oExportIncentive.MasterLCNo) ? "" : _oExportIncentive.MasterLCNo)
                                                , _oFontStyleBold));

                        _oPhrase.Add(new Chunk(array[1], _oFontStyle));
                        _oPhrase.Add(new Chunk(_sPurchaseContactNo, _oFontStyleBold));
                    }
                }
                ESimSolPdfHelper.AddParagraph(ref _oPdfPTable, _oPhrase, Element.ALIGN_LEFT, Element.ALIGN_TOP, BaseColor.WHITE, 0, 0, 0, 0);
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
                _oPdfPCell = new PdfPCell(new Phrase(_oExportLC.ExportLCNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
               
            }
            #endregion
            #region Master LC No
            if (!String.IsNullOrEmpty(_oImportLetterSetup.MasterLCNo))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 10f, 120f, 10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.MasterLCNo, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.MasterLCs, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
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
                _oPdfPCell = new PdfPCell(new Phrase(_oExportLC.ApplicantName, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();

                if (_oImportLetterSetup.IsPrintSupplierAddress)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase("Address", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.ContractorAddress, _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                    oPdfPTable.CompleteRow();
                }

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
                if (_oExportLC.ExportLCID > 0)
                {
                    _oPhrase = new Phrase();
                    _oPhrase.Add(new Chunk(_oExportLC.BankName_Issue, _oFontStyleBold));
                    if (!String.IsNullOrEmpty(_oImportLetterSetup.PIBankAddress))
                    {
                        _oPhrase.Add(new Chunk("\n" + _oExportLC.BBranchName_Issue, _oFontStyle));
                       
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
                _oPdfPCell = new PdfPCell(new Phrase(_oExportLC.ExportLCNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                //_oPhrase = new Phrase();
                //_oPhrase.Add(new Chunk(_oImportLetterSetup.LCNo, _oFontStyle));
                //_oPhrase.Add(new Chunk(_oExportIncentive.ExportIncentiveNo, _oFontStyleBold));

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
                _oPdfPCell = new PdfPCell(new Phrase(_oExportLC.AmountSt, _oFontStyleBold));
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
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] { 40f, 280f, 150f, 60f });

                //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("PI No", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase("Value", _oFontStyleBold));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                int nCount = 0;
            
              
                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Blank spacc
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();
            #endregion

            #region Clauses
            #region Clause
            if (!String.IsNullOrEmpty(_oImportLetterSetup.Clause))
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oImportLetterSetup.Clause, _oFontStyle));

                _oPdfPCell = new PdfPCell(_oPhrase);
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion
            float nBSHight = 0;
            if (_oImportLetterSetup.IsPrinTnC)
            {
                //_oFontStyle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9f, 0);
                nBSHight = 25;
                #region Blank space
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nBSHight; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();
                #endregion

            }
            else
            {
                nBSHight = 25;
                #region Blank space
                //_oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nBSHight; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();
                #endregion
            }
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
            if (!String.IsNullOrEmpty(_oImportLetterSetup.Body3) && _oExportIncentive.IsCopyTo)
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk(_oImportLetterSetup.Body3, _oFontStyle));
                _oParagraph = new Paragraph(_oPhrase);
                _oParagraph.SetLeading(0f, 1.5f);
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
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 8f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ThankingOne
            if (!String.IsNullOrEmpty(_oImportLetterSetup.ThankingOne) && _oImportLetterSetup.Authorize1IsAuto)
            {
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.ThankingOne, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = nBSHight; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 2f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ThankingTwo
            if (!String.IsNullOrEmpty(_oImportLetterSetup.ThankingTwo) && _oImportLetterSetup.Authorize2IsAuto)
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
            oPdfPTableTwo.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            int nRowHeight = 20;
            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11f, 0)));
            oPdfPCell.Border = 0; ;
            oPdfPCell.FixedHeight = nRowHeight;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11f, 2)));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = nRowHeight;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11f, 2)));
            oPdfPCell.Border = 0;
            oPdfPCell.FixedHeight = nRowHeight;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPTableTwo.CompleteRow();

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Authorize1) && _oImportLetterSetup.Authorize1IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase("--------------------", _oFontStyle));
                oPdfPCell.Border = 0; ;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTableTwo.AddCell(oPdfPCell);
            }

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Authorize2) && _oImportLetterSetup.Authorize2IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase("--------------------", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = (_oImportLetterSetup.Authorize1IsAuto ? Element.ALIGN_CENTER : Element.ALIGN_LEFT);
                oPdfPTableTwo.AddCell(oPdfPCell);
            }

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Authorize3) && _oImportLetterSetup.Authorize3IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase("--------------------", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPTableTwo.AddCell(oPdfPCell);
            }
           

            oPdfPTableTwo.CompleteRow();

            if (_oImportLetterSetup.Authorize1IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0; ;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTableTwo.AddCell(oPdfPCell);
            }
            

            if (_oImportLetterSetup.Authorize2IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = (String.IsNullOrEmpty(_oImportLetterSetup.Authorize1) ? Element.ALIGN_LEFT : Element.ALIGN_CENTER);
                oPdfPTableTwo.AddCell(oPdfPCell);
            }
            

            if (_oImportLetterSetup.Authorize3IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell.Border = 0;
                //oPdfPCell.FixedHeight = 35f;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPTableTwo.AddCell(oPdfPCell);
            }
           

            oPdfPTableTwo.CompleteRow();

            if (_oImportLetterSetup.Authorize1IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.Authorize1, _oFontStyle));
                oPdfPCell.Border = 0; ;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTableTwo.AddCell(oPdfPCell);
            }
            if (_oImportLetterSetup.Authorize2IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.Authorize2, _oFontStyle));
                oPdfPCell.Border = 0; ;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTableTwo.AddCell(oPdfPCell);
            }
            if (_oImportLetterSetup.Authorize3IsAuto)
            {
                oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.Authorize3, _oFontStyle));
                oPdfPCell.Border = 0; ;
                oPdfPCell.BackgroundColor = BaseColor.WHITE;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                oPdfPTableTwo.AddCell(oPdfPCell);
            }
            oPdfPTableTwo.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableTwo);
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        private void FindAndReplace(ref string sData, string sFind, string sReplace) 
        {
            if (sData.Contains(sFind))
            {
                sData = sData.Replace(sFind, sReplace);
            }
        }
        public void BlankSpec(float FixedHeight)
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = FixedHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public void NewPageDeclaration()
        {
            #region New Page Declare
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            #endregion
        }
        #endregion
    }
}
