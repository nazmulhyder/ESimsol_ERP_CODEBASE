using System;
using System.IO;
using System.Data;
using ESimSol.BusinessObjects;
using System.Linq;
using System.Collections.Generic;
using ICS.Core.Utility;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ESimSol.Reports
{
    public class rptLoanRequest
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        public iTextSharp.text.Image _oImag { get; set; }
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        LoanRequest _oLoanRequest = new LoanRequest();
        EmployeeLoan _oEmployeeLoan = new EmployeeLoan();
        List<EmployeeLoanInstallment> _oELIs = new List<EmployeeLoanInstallment>();
        List<EmployeeLoanAmount> _oELAs = new List<EmployeeLoanAmount>();
        List<EmployeeLoanRefund> _oEmployeeLoanRefunds = new List<EmployeeLoanRefund>();
        Company _oCompany = new Company();

        string sWStatus = "";
        #endregion


        #region Constructor

        public rptLoanRequest() { }

        #endregion


        public byte[] PrepareReport(LoanRequest oLoanRequest, EmployeeLoan oEmployeeLoan, List<EmployeeLoanInstallment> oELIs, List<EmployeeLoanAmount> oELAs, List<EmployeeLoanRefund> oEmployeeLoanRefunds, Company oCompany)
        {
            _oLoanRequest = oLoanRequest;
            _oEmployeeLoan = oEmployeeLoan;
            _oELIs = oELIs;
            _oELAs = oELAs;
            _oEmployeeLoanRefunds = oEmployeeLoanRefunds;
            _oCompany = oCompany;

            sWStatus = oEmployeeLoan.EmployeeWorkingStatus;
            #region Page Setup

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 95;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);

            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();

            _oPdfPTable.SetWidths(new float[] { 25f, 70f, 90f, 90f, 90f, 110f, 40f }); //height:842   width:595

            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 2;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }


        #region Report Header
        private void PrintHeader()
        {

            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 170f, 270f });
            PdfPCell oPdfPCellHearder;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(40f, 20f);
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
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region ReportHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("Employee Loan Info " + ((_oLoanRequest.IsAdjustable)?" (Adjustment Loan)":""), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 8f;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

        }
        #endregion


        #region Report Body

        private void PrintBody()
        {


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
            PdfPTable oPdfPTable = new PdfPTable(6);
            oPdfPTable.SetWidths(new float[] { 65f, 5f ,250f, 70f, 5f, 150f});

            #region 

            /*----------------------------*/
            _oPdfPCell = new PdfPCell(new Phrase((_oEmployeeLoan.EmployeeLoanID>0)?"Loan Code": "Request No", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oEmployeeLoan.EmployeeLoanID>0)? _oEmployeeLoan.Code :_oLoanRequest.RequestNo, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oEmployeeLoan.EmployeeLoanID > 0) ? "Approve Date" : "Request Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
           
            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oEmployeeLoan.EmployeeLoanID > 0) ? _oEmployeeLoan.ApproveDateStr : _oLoanRequest.RequestDateStr, _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            /*----------------------------*/
            _oPdfPCell = new PdfPCell(new Phrase("Employee Code", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(((_oEmployeeLoan.EmployeeLoanID > 0) ? _oEmployeeLoan.EmployeeCode + "   Employee Name : " + _oEmployeeLoan.EmployeeName : _oLoanRequest.EmployeeNameCode + "   Employee Name : " + _oLoanRequest.EmployeeName) + "   W. Status : " + sWStatus, _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            /*----------------------------*/
            _oPdfPCell = new PdfPCell(new Phrase("Official Info", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oEmployeeLoan.EmployeeLoanID > 0) ? _oEmployeeLoan.OfficialInfo : _oLoanRequest.OfficialInfo, _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            /*----------------------------*/
            if (!_oEmployeeLoan.IsPFLoan)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Salary Info", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((_oEmployeeLoan.EmployeeLoanID > 0) ? _oEmployeeLoan.SalaryInfo : _oLoanRequest.SalaryInfo, _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }
            else
            {
                /*----------------------------*/
                _oPdfPCell = new PdfPCell(new Phrase("PF Info", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((_oEmployeeLoan.EmployeeLoanID > 0) ? _oEmployeeLoan.PFInfo : _oLoanRequest.PFInfo, _oFontStyle));
                _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            /*----------------------------*/

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 4f; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Loan Info", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0;  oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0;  oPdfPTable.AddCell(_oPdfPCell);

            PdfPTable oPdfPTableLoan = new PdfPTable(5);
            oPdfPTableLoan.SetWidths(new float[] { 70f, 100f, 110f, 110f, 120f });

            _oPdfPCell = new PdfPCell(new Phrase("Disburse Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLoan.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Loan Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLoan.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLoan.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Refund Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLoan.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Current Bal. Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLoan.AddCell(_oPdfPCell);
            oPdfPTableLoan.CompleteRow();

            bool bIsSpan = true;
            foreach (EmployeeLoanAmount oItem in _oELAs)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.LoanDisburseDateStr, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLoan.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLoan.AddCell(_oPdfPCell);
                
                if (bIsSpan)
                {
                    bIsSpan = false;
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oELAs.Sum(x => x.Amount)), _oFontStyle));
                    _oPdfPCell.Rowspan = _oELAs.Count(); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLoan.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oEmployeeLoanRefunds.Sum(x => x.Amount)), _oFontStyle));
                    _oPdfPCell.Rowspan = _oELAs.Count(); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLoan.AddCell(_oPdfPCell);
                
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oELIs.Where(x=>x.ESDetailID<=0 && x.Type==1).Sum(x=>x.InstallmentAmount)), _oFontStyle));
                    _oPdfPCell.Rowspan = _oELAs.Count(); _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTableLoan.AddCell(_oPdfPCell);

                }
                oPdfPTableLoan.CompleteRow();
            }
            _oPdfPCell = new PdfPCell(oPdfPTableLoan);
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0;  oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 4f; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();
            /*----------------------------*/
            //_oPdfPCell = new PdfPCell(new Phrase("Purpose", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
          
            //_oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
          
            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
            //oPdfPTable.CompleteRow();

            /*----------------------------*/
            _oPdfPCell = new PdfPCell(new Phrase("Purpose", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0;  oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oEmployeeLoan.EmployeeLoanID > 0) ? _oEmployeeLoan.Purpose : _oLoanRequest.Purpose, _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0;  oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            /*----------------------------*/
            _oPdfPCell = new PdfPCell(new Phrase("Disburse Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0;  oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(((_oELIs.Count()>0)?_oELIs.FirstOrDefault().InstallmentDateStr:"") , _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0;  oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("No of Installment", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0;  oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oEmployeeLoan.EmployeeLoanID > 0) ? _oEmployeeLoan.NoOfTotalInstallment.ToString() : _oLoanRequest.NoOfInstallment.ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            /*----------------------------*/
            _oPdfPCell = new PdfPCell(new Phrase("Inst. Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((_oEmployeeLoan.EmployeeLoanID > 0) ? _oEmployeeLoan.InstallmentAmount : _oLoanRequest.InstallmentAmount), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Interest Rate", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat((_oEmployeeLoan.EmployeeLoanID > 0) ? _oEmployeeLoan.InterestRate : _oLoanRequest.InterestRate), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();


            /*----------------------------*/

            _oPdfPCell = new PdfPCell(new Phrase("Remarks", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oLoanRequest.Remarks, _oFontStyle));
            _oPdfPCell.Colspan = 4; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            /*----------------------------*/
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 6; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 10f; _oPdfPCell.Border = 0; oPdfPTable.AddCell(_oPdfPCell);
            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion


            #region Grid

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Installment Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Installment", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Interest", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Balance", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Realized", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oELIs = _oELIs.Where(x => x.Type == 1).ToList();
            if (_oELIs.Count() > 0)
            {
                
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 0);
                int nCount = 0;
                foreach (EmployeeLoanInstallment oItem in _oELIs)
                {
                    _oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.InstallmentDateStr, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.InstallmentAmount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.InterestPerInstallment), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.TotalAmount), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Balance), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Encash, _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();
                }

                #region Total
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
                _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oELIs.Sum(x => x.InstallmentAmount)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oELIs.Sum(x => x.InterestPerInstallment)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oELIs.Sum(x => x.TotalAmount)), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.Border = 0; _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            #endregion


            #region
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 7; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPCell.FixedHeight = 30f; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("___________________________", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("___________________________", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("___________________________", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Checked by", _oFontStyle));
            _oPdfPCell.Colspan = 3; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Received by", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approved by", _oFontStyle));
            _oPdfPCell.Colspan = 2; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.Border = 0; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        #endregion
    }


}

