
using System;
using System.Data;
using System.Linq;
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
    public class rptLoanRefund
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        EmployeeLoanRefund _oEmployeeLoanRefund = new EmployeeLoanRefund();
        Company _oCompany = new Company();
        string _sLoanDetails, _sEmpInfo ="";
        #endregion

        public byte[] PrepareReport(EmployeeLoanRefund oEmployeeLoanRefund, Company oCompany, string sLoanDetails, string sEmpInfo)
        {
            _oEmployeeLoanRefund = oEmployeeLoanRefund;
            _oCompany = oCompany;
            _sLoanDetails = sLoanDetails;
            _sEmpInfo = sEmpInfo;
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(5f, 5f, 10f, 40f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 555f });
            #endregion

            this.PrintBody();
            //_oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }


        #region Report Body


        private void PrintBody()
        {

            int nColumn = 9;
            PdfPTable oPdfPTable = new PdfPTable(9);
            oPdfPTable.SetWidths(new float[] { 26f, 50f, 4f, 80f, 20f, 80f, 80f, 70f, 80f });


            #region CompanyHeader

            oPdfPTable.AddCell(this.SetCellValue("", 0, nColumn, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 10f));
            oPdfPTable.CompleteRow();

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 170f, 270f });
            PdfPCell oPdfPCellHearder;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(60f, 20f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellHearder.PaddingBottom = 2;
                oPdfPCellHearder.Border = 0;

                oPdfPTableHeader.AddCell(oPdfPCellHearder);

            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);


            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = nColumn;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.FixedHeight = 40;
            oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region Title
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            oPdfPTable.AddCell(this.SetCellValue("", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 20f));
            oPdfPTable.AddCell(this.SetCellValue("Money Receipt", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, 1, 20f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 20f));
            oPdfPTable.CompleteRow();
            #endregion

            oPdfPTable.AddCell(this.SetCellValue("", 0, nColumn, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 20f));
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Receipt No", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(_oEmployeeLoanRefund.RefundNo, 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Loan Details :", 0, 2, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(_sLoanDetails, 0, 3, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Receipt Date", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(_oEmployeeLoanRefund.RefundDateStr, 0, 6, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("", 0, nColumn, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Received From", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(_sEmpInfo, 0, 6, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();


            oPdfPTable.AddCell(this.SetCellValue("", 0, nColumn, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();



            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Received For", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 16f));

            var oCell = this.SetCellValue("", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f);
            oCell.BorderWidthBottom = 1;
            oPdfPTable.AddCell(oCell);

            oPdfPTable.AddCell(this.SetCellValue("Refund Amount:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_oEmployeeLoanRefund.Amount), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();


            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 6, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Service Charge:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_oEmployeeLoanRefund.ServiceCharge), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 6, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Total:", 0, 0, Element.ALIGN_RIGHT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(Global.MillionFormat(_oEmployeeLoanRefund.Amount + _oEmployeeLoanRefund.ServiceCharge), 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Amount In Word", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(Global.TakaWords(_oEmployeeLoanRefund.Amount + _oEmployeeLoanRefund.ServiceCharge), 0, 6, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();


            oPdfPTable.AddCell(this.SetCellValue("", 0, nColumn, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 20f));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Paid By", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue(":", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Cash", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 16f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("", 0, nColumn, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 6f));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Cheque", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 16f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("", 0, nColumn, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 6f));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 2, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("Money Order", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 0, Element.ALIGN_LEFT, BaseColor.WHITE, 1, 16f));
            oPdfPTable.AddCell(this.SetCellValue("", 0, 4, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 16f));
            oPdfPTable.CompleteRow();



            oPdfPTable.AddCell(this.SetCellValue("", 0, nColumn, Element.ALIGN_LEFT, BaseColor.WHITE, 0, 30f));
            oPdfPTable.CompleteRow();


            oPdfPTable.AddCell(this.SetCellValue("____________________", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            oPdfPTable.AddCell(this.SetCellValue("____________________", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            oPdfPTable.AddCell(this.SetCellValue("____________________", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            oPdfPTable.CompleteRow();

            oPdfPTable.AddCell(this.SetCellValue("Prepared By", 0, 4, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            oPdfPTable.AddCell(this.SetCellValue("Checked By", 0, 3, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            oPdfPTable.AddCell(this.SetCellValue("Received By", 0, 2, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 0));
            oPdfPTable.CompleteRow();



            oPdfPTable.AddCell(this.SetCellValue("", 0, nColumn, Element.ALIGN_CENTER, BaseColor.WHITE, 0, 10f));
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }

        private PdfPCell SetCellValue(string sName, int nRowSpan, int nColumnSpan, int align, BaseColor color, int border, float height)
        {
            PdfPCell oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            oPdfPCell.HorizontalAlignment = align;
            oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            oPdfPCell.BackgroundColor = color;
            if (border == 0)
                oPdfPCell.Border = 0;
            if (height > 0)
                oPdfPCell.FixedHeight = height;
            return oPdfPCell;
        }

        #endregion
    }
}
