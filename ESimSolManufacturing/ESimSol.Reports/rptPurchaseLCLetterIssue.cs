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
    public class rptImportLCLetterIssue
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
        ImportLC _oImportLC = new ImportLC();
        List<ImportLC_Clause> _oImportLC_Clauses = new List<ImportLC_Clause>();
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
        public byte[] PreparePrintLetter(ImportLC oImportLC, ImportLetterSetup oImportLetterSetup, List<ImportPI> oImportPIs, Company oCompany, BusinessUnit oBusinessUnit)
        {
            _oImportLC = oImportLC;
            _oImportPIs = oImportPIs;
            _oImportLC_Clauses = oImportLC.ImportLC_Clauses;
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
            int nELDBPCount = _oImportLC_Clauses.Count;

            _oFontStyleBold = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10, iTextSharp.text.Font.NORMAL);


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
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.RefNo + _oImportLC.FileNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion

            #region Date
            if (_oImportLetterSetup.IsPrintDateObject)
            { 
            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Date : " + _oImportLC.LCRequestDate.ToString("MMM dd , yyyy"), _oFontStyleBold));
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
            

            if (_oImportLetterSetup.IssueToType == (int)EnumImportLetterIssueTo.Bank)
            {
                #region Negotiate Bank Name
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BankName_Nego, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Negotiate Bank Branch
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BBranchName_Nego, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Negotiate Bank Address
                //_oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BankAddress_Nego, _oFontStyle));
                //_oPdfPCell.Border = 0; _oPdfPCell.MinimumHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();


                PdfPTable oPdfPTable = new PdfPTable(2);
                oPdfPTable.SetWidths(new float[] { 200f, 180f });

                _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BankAddress_Nego, _oFontStyle));
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
                #region Negotiate Bank Name
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.ContractorName, _oFontStyle));
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

            #region Blank spacc
            
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Subject

            if (!String.IsNullOrEmpty(_oImportLetterSetup.Subject))
            {

                if (_oImportLC.LCMargin<=0)
                {
                    _sMargins = "Zero margin";
                }
                if (_oImportLC.LCMargin>0)
                {
                    _sMargins = Math.Round(_oImportLC.LCMargin,0)+" % margin";
                }

                if (_oImportLC.LCPaymentType > EnumLCPaymentType.REGULAR)
                {
                    _sLCPaymentType = _oImportLC.LCPaymentTypeSt;
                }

                if (_oImportLetterSetup.Subject.Contains("@LCPAYMENTTYPE"))
                {
                     _oImportLetterSetup.Subject =_oImportLetterSetup.Subject.Replace("@LCPAYMENTTYPE", _sLCPaymentType);
                }
                if (_oImportLetterSetup.Subject.Contains("@LCNO"))
                {
                    _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCNO", _oImportLC.ImportLCNo + " DT:" + _oImportLC.ImportLCDateInString);
                }
                _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@AMOUNT", _oImportLC.AmountSt);
                _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCTERM", _oImportLC.LCTermsName);
                _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@LCMARGIN", _sMargins);
                _oImportLetterSetup.Subject = _oImportLetterSetup.Subject.Replace("@BALANCEAMOUNT", _oImportLC.Currency + "" + Global.MillionFormat(_oImportLC.Balance));
                //_oImportLetterSetup.Ip
                //LCPaymentType

                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk("Subject: ", _oFontStyle));
                _oPhrase.Add(new Chunk(_oImportLetterSetup.Subject +" "+_oImportLetterSetup.SubjectTwo, _oFontStyleBold));
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
                    _sPurchaseContactNo += oItem.ImportPINo+" Dt. "+oItem.IssueDate.ToString("dd MMM yyyy");
                    if (_oImportPIs.Count==2)
                    {
                        _sPurchaseContactNo += " & ";
                    }
                    else if(_oImportPIs.Count>2)
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
                _oPdfPCell.Border = 0;  _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
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
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.ImportLCNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                //_oPhrase = new Phrase();
                //_oPhrase.Add(new Chunk(_oImportLetterSetup.LCNo, _oFontStyle));
                //_oPhrase.Add(new Chunk(_oImportLC.ImportLCNo, _oFontStyleBold));

                //_oPdfPCell = new PdfPCell(_oPhrase);
                //_oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();
            }
            #endregion
            #region Master LC No
            if (!String.IsNullOrEmpty(_oImportLetterSetup.MasterLCNo) && _oImportLC.LCAppType == EnumLCAppType.B2BLC)
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
                //_oPhrase = new Phrase();
                //_oPhrase.Add(new Chunk(_oImportLetterSetup.LCNo, _oFontStyle));
                //_oPhrase.Add(new Chunk(_oImportLC.ImportLCNo, _oFontStyleBold));

                //_oPdfPCell = new PdfPCell(_oPhrase);
                //_oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                //_oPdfPTable.CompleteRow();
            }
            #endregion
            #region Supplier Name
            if (!String.IsNullOrEmpty(_oImportLetterSetup.SupplierName))
            {
                PdfPTable oPdfPTable = new PdfPTable(4);
                oPdfPTable.SetWidths(new float[] {10f, 120f,10f, 455f });

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLetterSetup.SupplierName, _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.ContractorName, _oFontStyleBold));
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
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.ImportLCNo, _oFontStyleBold));
                _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                //_oPhrase = new Phrase();
                //_oPhrase.Add(new Chunk(_oImportLetterSetup.LCNo, _oFontStyle));
                //_oPhrase.Add(new Chunk(_oImportLC.ImportLCNo, _oFontStyleBold));

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
                _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.AmountSt, _oFontStyleBold));
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
                if (_oImportLC.ImportLCDetails.Count > 0)
                {
                    foreach (ImportLCDetail oItem in _oImportLC.ImportLCDetails)
                    {
                        nCount++;
                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString("00"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ImportPINo + " DT: " + oItem.IssueDate.ToString("dd MMM yy"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.CurrencyName + "" + Global.MillionFormat(oItem.Amount), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                    }
                }
                else
                {
                    foreach (ImportPI oItem in _oImportPIs)
                    {
                        nCount++;
                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString("00"), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(oItem.ImportPINo + " DT: " + oItem.IssueDateSt, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.CurrencyName + "" + Global.MillionFormat(oItem.TotalValue), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                        oPdfPTable.CompleteRow();
                    }
                }
                if (nCount>0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.CurrencyName + "" + Global.MillionFormat(_oImportLC.Amount), _oFontStyleBold));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

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
                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
                foreach (ImportLC_Clause oItem in _oImportLC_Clauses)
                {
                    _oPhrase = new Phrase();
                    if (!String.IsNullOrEmpty(oItem.Caption))
                    {
                        _oPhrase.Add(new Chunk("(*) " + oItem.Caption+" ", _oFontStyleBold));
                    }
                    else
                    {
                        _oPhrase.Add(new Chunk("(*) ", _oFontStyleBold));
                    }

                    _oPhrase.Add(new Chunk(oItem.Clause, _oFontStyle));
                    _oParagraph = new Paragraph(_oPhrase);
                    _oParagraph.SetLeading(0f, 0f);
                    _oParagraph.Alignment = Element.ALIGN_JUSTIFIED;
                    _oPdfPCell = new PdfPCell();
                    _oPdfPCell.AddElement(_oParagraph);
                    _oPdfPCell = new PdfPCell(new Phrase("(*) " + oItem.Clause, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
                    _oPdfPCell.Border = 0;
                    //_oPdfPCell.MinimumHeight = 10;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                    _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                }
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
                if (_oImportLC_Clauses.Count > 8 && _oImportLC_Clauses.Count < 10)
                {
                nBSHight = 5;
                }
                else if (_oImportLC_Clauses.Count>10)
                {
                nBSHight = 1;
                }
                
            }
            else
            {
                nBSHight = 25;
                #region Blank space
                _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
                _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = nBSHight; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
          

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
            if (_oImportLC.LCChargeType != EnumLCChargeType.Charge_Free)
            {
                _oPhrase = new Phrase();
                _oPhrase.Add(new Chunk("The amendment charges will bear by the " + _oImportLC.LCChargeType, _oFontStyle));
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
            oPdfPTableTwo.SetWidths(new float[] { 197f, 197f, 197f});


            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 10f, 0)));
            oPdfPCell.Border = 0;;
            oPdfPCell.FixedHeight = 35;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 10f, 2)));
            oPdfPCell.Border = 0; 
            oPdfPCell.FixedHeight = 35;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPTableTwo.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("", FontFactory.GetFont("Tahoma", 10f, 2)));
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
            oPdfPCell.Border = 0;;
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

        #region Request For LC Open
        public byte[] PrepareReport(ImportLC oImportLC, Company oCompany)
        {
            _oImportLC = oImportLC;
            _oImportLC_Clauses = oImportLC.ImportLC_Clauses;
            _oCompany = oCompany;
            //oImportLC.PurchasePaymentContractDetails

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(842f, 595f), 0f, 0f, 0f, 0f);
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
            this.PrintBody();     
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody()
        {
            int nELDBPCount = _oImportLC_Clauses.Count;
    
            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Free 3 of 9 Extended", 10f, 0);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f,0);
                   
            #region Date
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("MMM dd , yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            
            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 25f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            

            #region Manager Print
            _oPdfPCell = new PdfPCell(new Phrase("The Manager ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

            #region Negotiate Bank Name
            _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BankName_Nego, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Negotiate Bank Branch
            _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BBranchName_Nego, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Negotiate Bank Address
            _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BankAddress_Nego, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

                #region Subject
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD|iTextSharp.text.Font.UNDERLINE);
            _oPdfPCell = new PdfPCell(new Phrase("Sub:  Request opening "+_oImportLC.LCTermsName+" For "+_oImportLC.AmountSt+" Under E.D.F Facility From Bangladesh Bank.", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Dear sir
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Dear Sir,", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Message   
            
            _oPhrase.Add(new Chunk("Enclosed please find herewith Indent/Proforma Invoice No. Or Contract No.", _oFontStyle));
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPhrase.Add(new Chunk("", _oFontStyle));
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPhrase.Add(new Chunk(" We hereby requested to open the L/C at sight for 0% Cash Margin and rest 100%(Hundred Percent) margin within ............Days and to arrange Tk. ........... lac post import finance under EDF facilities from Bangladesh Bank at the time of Documents retired. In this regard we assure you tha we shall bear rate fluctuation of Foreign Currency fund, if any.", _oFontStyle));
            _oParagraph = new Paragraph(_oPhrase);
            _oParagraph.SetLeading(0f, 2f);
            _oParagraph.Alignment = Element.ALIGN_JUSTIFIED;
            _oPdfPCell = new PdfPCell();
            _oPdfPCell.AddElement(_oParagraph);
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 90f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            
            #region Thanks
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("Thanking you", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

         
            #region Sincerely
            _oPdfPCell = new PdfPCell(new Phrase("Sincerely yours.", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }

        #endregion
        #endregion
        #region Request For LC Open
        public byte[] PrepareReport_ImportLCAmendmentRequestLetter(ImportLC oImportLC, Company oCompany)
        {
            _oImportLC = oImportLC;
          
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(595f, 842f), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);//842*595
            _oDocument.SetMargins(60f, 60f, 30f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            //_oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);



            _oDocument.Open();


            _oPdfPTable.SetWidths(new float[] { 595f });
            #endregion

            this.PrintHeader();
            
            this.PrintBody_AmendmentRequestLetter();

            _oPdfPTable.HeaderRows = 6;
         //   _oPdfPTable.FooterRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody_AmendmentRequestLetter()
        {
            int nELDBPCount = _oImportLC_Clauses.Count;


            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Free 3 of 9 Extended", 10f, 0);
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

            #region Blank spacc
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("MMM dd , yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);

            #region Manager Print
            _oPdfPCell = new PdfPCell(new Phrase("The Manager ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 0);

            #region Negotiate Bank Name
            _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BankName_Nego, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Negotiate Bank Branch
            _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BBranchName_Nego, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Negotiate Bank Address
            _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BankAddress_Nego, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Subject
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Subject:  Request to Amendment on LC No "+_oImportLC.ImportLCNo+" Dated " +_oImportLC.ImportLCDate.ToString("dd MMM yyyy")+".", _oFontStyle));
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
            _oPdfPCell = new PdfPCell(new Phrase("We would like to inform you that, we had opened the subject mention LC against supplier " + _oImportLC .ContractorName+". Now time we want to amend the LC as follows:", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion



            //#region Clauses
            //_oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            //foreach (ImportLC_AmendmentDetail oItem in _oImportLCAmendment.ImportLC_AmendmentDetails)
            //{
            //    _oPdfPCell = new PdfPCell(new Phrase("(*) " + oItem.CaluseofAmendment, _oFontStyle));
            //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    _oPdfPCell.Border = 0;
            //    _oPdfPCell.FixedHeight = 15f;
            //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //    _oPdfPTable.AddCell(_oPdfPCell);

            //    _oPdfPTable.CompleteRow();
            //}
            //#endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Message
            _oPdfPCell = new PdfPCell(new Phrase("Please arrange to amend the LC as per requirement.", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Message
            _oPdfPCell = new PdfPCell(new Phrase("Thanking you for your continuous support & service towards us.", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 40f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Message
            _oPdfPCell = new PdfPCell(new Phrase("Your's Truly", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 180f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


          



            //#endregion


        }

        #endregion
        #endregion
        #region Request For LC Cancel
        public byte[] PrepareReport_ImportLCCancelRequestLetter(ImportLC oImportLC,  Company oCompany)
        {
            _oImportLC = oImportLC;
       
            _oCompany = oCompany;

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(595f, 842f), 0f, 0f, 0f, 0f);
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

            this.PrintBody_CancelRequestLetter();

            _oPdfPTable.HeaderRows = 6;
            //   _oPdfPTable.FooterRows = 1;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody_CancelRequestLetter()
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


            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 12f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion


            #region Date
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString("MMM dd , yyyy"), _oFontStyle));
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

            #region Manager Print
            _oPdfPCell = new PdfPCell(new Phrase("The Manager ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            #region Negotiate Bank Name
            _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BankName_Nego, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Negotiate Bank Branch
            _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BBranchName_Nego, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Negotiate Bank Address
            _oPdfPCell = new PdfPCell(new Phrase(_oImportLC.BankAddress_Nego, _oFontStyle));
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
            if (_oImportLC.LCCurrentStatus == EnumLCCurrentStatus.Req_For_Cancel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Subject : Request for cancelation of LC No " + _oImportLC.ImportLCNo + " Dated " + _oImportLC.ImportLCDate.ToString("dd MMM yyyy") + ".", _oFontStyle));
            }
            else if (_oImportLC.LCCurrentStatus == EnumLCCurrentStatus.Req_For_Partial_Cancel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Subject :  Request for cancelation of balance amount of LC No " + _oImportLC.ImportLCNo + " Dated " + _oImportLC.ImportLCDate.ToString("dd MMM yyyy") + ".", _oFontStyle));
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
            _oPdfPCell = new PdfPCell(new Phrase("With due respect, we would like to inform you that, you have opened the following L/C in favor of following supplier/beneficiary :", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank spacc
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 15f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Detail Table header
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 2f, 80f, 320f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Supplier Name ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oImportLC.ContractorName, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
              
            _oPdfPCell = new PdfPCell(new Phrase("LC No. & Date  ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oImportLC.ImportLCNo + ";" + _oImportLC.ImportLCDate.ToString("dd MMM yyyy"), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("LC Value  ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _oPdfPCell = new PdfPCell(new Phrase(": " + _oImportLC.Currency + "" + Global.MillionFormat(_oImportLC.Amount), _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();

        
            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0; 
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, 0);

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 4f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Message
            if (_oImportLC.LCCurrentStatus == EnumLCCurrentStatus.Req_For_Cancel)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Now we are requesting you to cancel the above L/C's as the supplier is not in a position to ship out the goods in time.", _oFontStyle));
            }
            else if (_oImportLC.LCCurrentStatus == EnumLCCurrentStatus.Req_For_Partial_Cancel)
            {

                ///Please get Invoice

                _oPdfPCell = new PdfPCell(new Phrase("The beneficiary has effected shipment for us " + _oImportLC.Currency + " " + Global.MillionFormat(_oImportLC.Amount_Invoice) + " and they are not in a position to effect shipment balance quantity of goods for us " + _oImportLC.Currency + " " + Global.MillionFormat(_oImportLC.Amount - _oImportLC.Amount_Invoice) + "./nSo we are requesting you to cancel the balance amount  of the L/C for us " + _oImportLC.Currency + " " + Global.MillionFormat(_oImportLC.Amount - _oImportLC.Amount_Invoice) + " at the earliest. (It may be mentioned here that the L/C  has already been expired on " + _oImportLC.ExpireDate.ToString("dd MMM yyyy") + ")", _oFontStyle));
            }

            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Message
            _oPdfPCell = new PdfPCell(new Phrase("Your earlier co-operation regarding this matter will be highly appreciated.\n\n Thanking You", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Blank space
            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 40f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Message
            _oPdfPCell = new PdfPCell(new Phrase("For "+_oCompany.Name, _oFontStyle));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 180f; _oPdfPCell.VerticalAlignment = Element.ALIGN_LEFT; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
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


            _oPdfPCell = new PdfPCell(new Phrase("Authorized Signature                                           Manager-Trade Finace                       Finance Executive ", _oFontStyle));
            _oPdfPCell.Border = 0;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            #endregion

            //#endregion

        }

        #endregion
        #endregion
    }
}
