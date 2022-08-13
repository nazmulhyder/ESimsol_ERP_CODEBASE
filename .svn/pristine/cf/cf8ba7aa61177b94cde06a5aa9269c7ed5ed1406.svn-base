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

namespace ESimSol.Reports
{
    public class rptVoucherBillConfigure
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        int _nColumns = 7;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        VoucherBill _oVoucherBill = new VoucherBill();
        List<VoucherBill> _oVoucherBills = new List<VoucherBill>();

        Company _oCompany = new Company();
        Contractor _oContractor = new Contractor();
        ContactPersonnel _oContactPersonnel = new ContactPersonnel();
        ACCostCenter _oACCostCenter = new ACCostCenter();
        string _sMessage = ""; string _sUserName = "";
        #endregion

        public byte[] PrepareReport(VoucherBill oVoucherBill, string sMessage, bool bIsLandscap, string sUserName = "")
        {
            _oVoucherBill = oVoucherBill;
            _oVoucherBills = oVoucherBill.VoucherBills;
            _oCompany = oVoucherBill.Company;            
            _sMessage = sMessage;
            _sUserName = sUserName;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            if (bIsLandscap)
            {
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                _oDocument.SetMargins(40f, 40f, 5f, 40f);
                _oPdfPTable.WidthPercentage = 100;
                _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
                PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                ESimSolFooter PageEventHandler = new ESimSolFooter();
                PDFWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
                _oDocument.Open();
                _oPdfPTable.SetWidths(new float[] { 
                                                    30f, //SL
                                                    171f, //Bill No
                                                    171f,  //Subledger
                                                    171f,  //Account Head
                                                    75f, //Bill Date
                                                    75f, //Due Date
                                                    94f   //Bill Amount
                                                });
            }
            else
            {
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
                _oDocument.SetMargins(20f, 20f, 5f, 30f);
                _oPdfPTable.WidthPercentage = 100;
                _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
                PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
                PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
                ESimSolFooter PageEventHandler = new ESimSolFooter();
                PDFWriter.PageEvent = PageEventHandler; //Footer print wiht page event handeler            
                _oDocument.Open();

                _oPdfPTable.SetWidths(new float[] { 
                                                     30f, //SL
                                                    171f, //Bill No
                                                    171f,  //Subledger
                                                    171f,  //Account Head
                                                    75f, //Bill Date
                                                    75f, //Due Date
                                                    94f   //Bill Amount
                                                });
            }
            #endregion

            this.PrintHeader();
            this.PrintBody(bIsLandscap);
            if (_oVoucherBill.PartyAddress != "")
            {
                _oPdfPTable.HeaderRows = 5;
            }
            else
            {
                _oPdfPTable.HeaderRows = 4;
            }
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                _oPdfPCell = new PdfPCell(_oImag);
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 35;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
                _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPCell.ExtraParagraphSpace = 0;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address + "\n" + _oCompany.Phone + ";  " + _oCompany.Email + ";  " + _oCompany.WebAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;           
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(_sMessage, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BorderWidthTop = 1f;
            _oPdfPCell.FixedHeight = 25f;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;            
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;         
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody(bool bIsLanscape)
        {
            if (bIsLanscape)
            {
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                _oFontStyle = FontFactory.GetFont("Tahoma", 8.5f, iTextSharp.text.Font.NORMAL);
            }
            else
            {
                _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oFontStyle = FontFactory.GetFont("Tahoma", 7.0f, iTextSharp.text.Font.NORMAL);
            }
            
            #region Table Header   
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bill No", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Subledger Name", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Account Head", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bill Date", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Due Date", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bill Amount", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            int nCount = 0; double nAmount = 0;
            foreach (VoucherBill oItem in _oVoucherBills)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BillNo, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SubLedgerName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AccountHeadName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                                
                _oPdfPCell = new PdfPCell(new Phrase(oItem.BillDateInString, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DueDateInString, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.AmountInMillionFormat, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);                                
                nAmount = nAmount + oItem.Amount;                
                _oPdfPTable.CompleteRow();
            }            
            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Colspan = _nColumns - 1; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);                        
            _oPdfPTable.CompleteRow();

            #region Blank 
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Prepare By
            _oPdfPCell = new PdfPCell(new Phrase(_sUserName + "\n------------------\nPrepare By", _oFontStyleBold));
            _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion
    }
}
