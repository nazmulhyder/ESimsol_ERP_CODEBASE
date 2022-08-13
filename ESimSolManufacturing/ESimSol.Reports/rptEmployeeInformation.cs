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
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Reports
{
    public class rptEmployeeInformation
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(3);
        PdfPCell _oPdfPCell;

        MemoryStream _oMemoryStream = new MemoryStream();
        Employee _oEmployee = new Employee();
        List<Employee> _oEmployees = new List<Employee>();
        List<Department> _oDepartments = new List<Department>();
        List<EmployeeSettlement> _oEmployeeSettlements = new List<EmployeeSettlement>();
        List<MonthlyAttendanceReport> _oMonthlyAttendanceReports = new List<MonthlyAttendanceReport>();
        List<LeaveApplication> _oLeaveApplications = new List<LeaveApplication>();
        Company _oCompany = new Company();


        string sHeader = "";

        #endregion

        public byte[] PrepareReport(Employee oEmployee, List<MonthlyAttendanceReport> MonthlyAttendanceReports, List<LeaveApplication> oLeaveApplications, Company oCompany)
        {
            _oEmployee = oEmployee;
            _oMonthlyAttendanceReports = MonthlyAttendanceReports;
            _oLeaveApplications = oLeaveApplications;
            _oCompany = oEmployee.Company;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            //_oDocument.SetPageSize(new iTextSharp.text.Rectangle(350, 230));
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 416f, 10f, 416f });
            #endregion

            //this.PrintHeader();
            this.PrintBody();

            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        //private void PrintHeader()
        //{
        //    #region CompanyHeader

        //    PdfPTable oPdfPTableHeader = new PdfPTable(3);
        //    oPdfPTableHeader.SetWidths(new float[] { 160f, 130f, 125f });
        //    PdfPCell oPdfPCellHearder;

        //    if (_oCompany.CompanyLogo != null)
        //    {
        //        _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        _oImag.ScaleAbsolute(20f, 15f);
        //        oPdfPCellHearder = new PdfPCell(_oImag);
        //        oPdfPCellHearder.FixedHeight = 15;
        //        oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
        //        oPdfPCellHearder.PaddingBottom = 2;
        //        oPdfPCellHearder.Border = 0;

        //        oPdfPTableHeader.AddCell(oPdfPCellHearder);

        //    }
        //    else
        //    {
        //        oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.Colspan = 4; oPdfPCellHearder.FixedHeight = 15;
        //        oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);

        //    }

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
        //    oPdfPCellHearder = new PdfPCell(new Phrase(_oEmployees.Count>0? _oEmployees[0].BUName : _oCompany.Name, _oFontStyle));
        //    oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
        //    oPdfPCellHearder.Border = 0;
        //    oPdfPCellHearder.Colspan = 4;
        //    oPdfPCellHearder.FixedHeight = 15;
        //    oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCellHearder.ExtraParagraphSpace = 0;
        //    oPdfPTableHeader.AddCell(oPdfPCellHearder);

        //    /*_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
        //    oPdfPCellHearder = new PdfPCell(new Phrase("BGMEA Reg. No.-" + _oCompany.CompanyRegNo, _oFontStyle));
        //    oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
        //    oPdfPCellHearder.Border = 0;
        //    oPdfPCellHearder.FixedHeight = 15;
        //    oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCellHearder.ExtraParagraphSpace = 0;
        //    oPdfPTableHeader.AddCell(oPdfPCellHearder);
        //    */
        //    oPdfPTableHeader.CompleteRow();

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
        //    oPdfPCellHearder = new PdfPCell(new Phrase(_oEmployees.Count > 0 ? _oEmployees[0].BUAddress : _oCompany.Address, _oFontStyle));
        //    //oPdfPCellHearder.Colspan = 4;
        //    oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
        //    oPdfPCellHearder.Border = 0;
        //    oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
        //    oPdfPCellHearder.ExtraParagraphSpace = 0;
        //    oPdfPTableHeader.AddCell(oPdfPCellHearder);
        //    oPdfPTableHeader.CompleteRow();

        //    _oPdfPCell = new PdfPCell(oPdfPTableHeader);
        //    //_oPdfPCell.Colspan = 7;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();


        //    _oPdfPCell = new PdfPCell(new Phrase(" "));
        //    //_oPdfPCell.Colspan = 7;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPCell.FixedHeight = 7;
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
        //    _oPdfPCell = new PdfPCell(new Phrase("Employee Profile", _oFontStyle));
        //    //_oPdfPCell.Colspan = 7;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPCell.ExtraParagraphSpace = 0;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();

        //    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
        //    _oPdfPCell = new PdfPCell(new Phrase(sHeader, _oFontStyle));
        //    //_oPdfPCell.Colspan = 7;
        //    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPCell.ExtraParagraphSpace = 0;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();

        //    _oPdfPCell = new PdfPCell(new Phrase(" "));
        //    //_oPdfPCell.Colspan = 7;
        //    _oPdfPCell.Border = 0;
        //    _oPdfPCell.FixedHeight = 7;
        //    _oPdfPCell.BackgroundColor = BaseColor.WHITE;
        //    _oPdfPTable.AddCell(_oPdfPCell);
        //    _oPdfPTable.CompleteRow();

        //    #endregion

        //}
        #endregion

        #region Report Body
        private void PrintBody()
        {

            if (_oEmployee.EmployeeID > 0)
            {

                #region Top Header

                PdfPTable oPdfPTableTopHeader = new PdfPTable(1);
                oPdfPTableTopHeader.SetWidths(new float[] { 842f });
                PdfPCell oPdfPCellTopHearder;

                _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

                oPdfPCellTopHearder = new PdfPCell(new Phrase("Business Unit : " + _oEmployee.BusinessUnitName, _oFontStyle));
                oPdfPCellTopHearder.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellTopHearder.Border = 0; oPdfPCellTopHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableTopHeader.AddCell(oPdfPCellTopHearder);

                oPdfPCellTopHearder = new PdfPCell(new Phrase("Location Unit : " + _oEmployee.LocationName, _oFontStyle));
                oPdfPCellTopHearder.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellTopHearder.Border = 0; oPdfPCellTopHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableTopHeader.AddCell(oPdfPCellTopHearder);

                oPdfPCellTopHearder = new PdfPCell(new Phrase("Business Unit Address : " + _oEmployee.BUAddress, _oFontStyle));
                oPdfPCellTopHearder.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellTopHearder.Border = 0; oPdfPCellTopHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableTopHeader.AddCell(oPdfPCellTopHearder);

                oPdfPCellTopHearder = new PdfPCell(new Phrase("Current Date : " + DateTime.Now.ToString("dd MMM yyyy"), _oFontStyle));
                oPdfPCellTopHearder.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCellTopHearder.Border = 0; oPdfPCellTopHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableTopHeader.AddCell(oPdfPCellTopHearder);
                
                _oPdfPCell = new PdfPCell(oPdfPTableTopHeader);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                #region Emp Basic Info

                #region Part 1

                PdfPTable oPdfPTableEmpBasicHeader1 = new PdfPTable(2);
                oPdfPTableEmpBasicHeader1.SetWidths(new float[] { 150f, 266f });
                PdfPCell oPdfPCellEmpBasicHearder1;

                oPdfPCellEmpBasicHearder1 = new PdfPCell(new Phrase("Employee Code :", _oFontStyle));
                oPdfPCellEmpBasicHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellEmpBasicHearder1.Border = 0; oPdfPCellEmpBasicHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader1.AddCell(oPdfPCellEmpBasicHearder1);

                oPdfPCellEmpBasicHearder1 = new PdfPCell(new Phrase(_oEmployee.Code, _oFontStyle));
                oPdfPCellEmpBasicHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEmpBasicHearder1.Border = 0; oPdfPCellEmpBasicHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader1.AddCell(oPdfPCellEmpBasicHearder1);

                oPdfPCellEmpBasicHearder1 = new PdfPCell(new Phrase("Authentication No :", _oFontStyle));
                oPdfPCellEmpBasicHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellEmpBasicHearder1.Border = 0; oPdfPCellEmpBasicHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader1.AddCell(oPdfPCellEmpBasicHearder1);

                oPdfPCellEmpBasicHearder1 = new PdfPCell(new Phrase(_oEmployee.CardNo, _oFontStyle));
                oPdfPCellEmpBasicHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEmpBasicHearder1.Border = 0; oPdfPCellEmpBasicHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader1.AddCell(oPdfPCellEmpBasicHearder1);

                oPdfPCellEmpBasicHearder1 = new PdfPCell(new Phrase("Bank Info :", _oFontStyle));
                oPdfPCellEmpBasicHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellEmpBasicHearder1.Border = 0; oPdfPCellEmpBasicHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader1.AddCell(oPdfPCellEmpBasicHearder1);

                oPdfPCellEmpBasicHearder1 = new PdfPCell(new Phrase((_oEmployee.EmployeeBankAccounts.Count > 0) ? _oEmployee.EmployeeBankAccounts[0].BankBranchName : "", _oFontStyle));
                oPdfPCellEmpBasicHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEmpBasicHearder1.Border = 0; oPdfPCellEmpBasicHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader1.AddCell(oPdfPCellEmpBasicHearder1);

                oPdfPCellEmpBasicHearder1 = new PdfPCell(new Phrase("TIN No :", _oFontStyle));
                oPdfPCellEmpBasicHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellEmpBasicHearder1.Border = 0; oPdfPCellEmpBasicHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader1.AddCell(oPdfPCellEmpBasicHearder1);

                oPdfPCellEmpBasicHearder1 = new PdfPCell(new Phrase(_oEmployee.EmployeeTINInformations.TIN, _oFontStyle));
                oPdfPCellEmpBasicHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEmpBasicHearder1.Border = 0; oPdfPCellEmpBasicHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader1.AddCell(oPdfPCellEmpBasicHearder1);


                _oPdfPCell = new PdfPCell(oPdfPTableEmpBasicHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);

                #region Part 2
                PdfPTable oPdfPTableEmpBasicHeader2 = new PdfPTable(2);
                oPdfPTableEmpBasicHeader2.SetWidths(new float[] { 150f, 266f });
                PdfPCell oPdfPCellEmpBasicHearder2;

                oPdfPCellEmpBasicHearder2 = new PdfPCell(new Phrase("Employee Code :", _oFontStyle));
                oPdfPCellEmpBasicHearder2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellEmpBasicHearder2.Border = 0; oPdfPCellEmpBasicHearder2.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader2.AddCell(oPdfPCellEmpBasicHearder2);

                oPdfPCellEmpBasicHearder2 = new PdfPCell(new Phrase(_oEmployee.Code, _oFontStyle));
                oPdfPCellEmpBasicHearder2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEmpBasicHearder2.Border = 0; oPdfPCellEmpBasicHearder2.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader2.AddCell(oPdfPCellEmpBasicHearder2);

                oPdfPCellEmpBasicHearder2 = new PdfPCell(new Phrase("Authentication No :", _oFontStyle));
                oPdfPCellEmpBasicHearder2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellEmpBasicHearder2.Border = 0; oPdfPCellEmpBasicHearder2.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader2.AddCell(oPdfPCellEmpBasicHearder2);

                oPdfPCellEmpBasicHearder2 = new PdfPCell(new Phrase(_oEmployee.CardNo, _oFontStyle));
                oPdfPCellEmpBasicHearder2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEmpBasicHearder2.Border = 0; oPdfPCellEmpBasicHearder2.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader2.AddCell(oPdfPCellEmpBasicHearder2);

                oPdfPCellEmpBasicHearder2 = new PdfPCell(new Phrase("Bank Info :", _oFontStyle));
                oPdfPCellEmpBasicHearder2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellEmpBasicHearder2.Border = 0; oPdfPCellEmpBasicHearder2.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader2.AddCell(oPdfPCellEmpBasicHearder2);

                oPdfPCellEmpBasicHearder2 = new PdfPCell(new Phrase((_oEmployee.EmployeeBankAccounts.Count > 0) ? _oEmployee.EmployeeBankAccounts[0].BankBranchName : "", _oFontStyle));
                oPdfPCellEmpBasicHearder2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEmpBasicHearder2.Border = 0; oPdfPCellEmpBasicHearder2.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader2.AddCell(oPdfPCellEmpBasicHearder2);

                oPdfPCellEmpBasicHearder2 = new PdfPCell(new Phrase("TIN No :", _oFontStyle));
                oPdfPCellEmpBasicHearder2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellEmpBasicHearder2.Border = 0; oPdfPCellEmpBasicHearder2.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader2.AddCell(oPdfPCellEmpBasicHearder2);

                oPdfPCellEmpBasicHearder2 = new PdfPCell(new Phrase(_oEmployee.EmployeeTINInformations.TIN, _oFontStyle));
                oPdfPCellEmpBasicHearder2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEmpBasicHearder2.Border = 0; oPdfPCellEmpBasicHearder2.BackgroundColor = BaseColor.WHITE; oPdfPTableEmpBasicHeader2.AddCell(oPdfPCellEmpBasicHearder2);


                _oPdfPCell = new PdfPCell(oPdfPTableEmpBasicHeader2);
                _oPdfPCell.Border = 15;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #endregion

                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                #region Scheme & Gross


                _oPdfPCell = new PdfPCell(new Phrase("Salary Scheme : " + (_oEmployee.EmployeeSalaryStructures.Count > 0 ? _oEmployee.EmployeeSalaryStructures[0].SalarySchemeName : ""), _oFontStyle));
                _oPdfPCell.Border = 15;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(""));
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Gross : " + (_oEmployee.EmployeeSalaryStructures.Count > 0 ?_oEmployee.EmployeeSalaryStructures[0].GrossAmount.ToString():""), _oFontStyle));
                _oPdfPCell.Border = 15;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                #region Emp LeaveLedger

                #region Part 1

                PdfPTable oPdfPTableLeaveLedgerHeader1 = new PdfPTable(4);
                oPdfPTableLeaveLedgerHeader1.SetWidths(new float[] { 150f, 88f, 88f, 88f });
                PdfPCell oPdfPCellLeaveLedgerHearder1;

                oPdfPCellLeaveLedgerHearder1 = new PdfPCell(new Phrase("LeaveShortName", _oFontStyle));
                oPdfPCellLeaveLedgerHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLeaveLedgerHearder1.Border = 0; oPdfPCellLeaveLedgerHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableLeaveLedgerHeader1.AddCell(oPdfPCellLeaveLedgerHearder1);

                oPdfPCellLeaveLedgerHearder1 = new PdfPCell(new Phrase("TotalLeave", _oFontStyle));
                oPdfPCellLeaveLedgerHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLeaveLedgerHearder1.Border = 0; oPdfPCellLeaveLedgerHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableLeaveLedgerHeader1.AddCell(oPdfPCellLeaveLedgerHearder1);

                oPdfPCellLeaveLedgerHearder1 = new PdfPCell(new Phrase("EnjoyedInfo", _oFontStyle));
                oPdfPCellLeaveLedgerHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLeaveLedgerHearder1.Border = 0; oPdfPCellLeaveLedgerHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableLeaveLedgerHeader1.AddCell(oPdfPCellLeaveLedgerHearder1);

                oPdfPCellLeaveLedgerHearder1 = new PdfPCell(new Phrase("BalanceInfo", _oFontStyle));
                oPdfPCellLeaveLedgerHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLeaveLedgerHearder1.Border = 0; oPdfPCellLeaveLedgerHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableLeaveLedgerHeader1.AddCell(oPdfPCellLeaveLedgerHearder1);
                oPdfPTableLeaveLedgerHeader1.CompleteRow();

                foreach (LeaveLedgerReport oItem in _oEmployee.LeaveLedgerReports)
                {
                    oPdfPCellLeaveLedgerHearder1 = new PdfPCell(new Phrase(oItem.LeaveShortName, _oFontStyle));
                    oPdfPCellLeaveLedgerHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLeaveLedgerHearder1.Border = 0; oPdfPCellLeaveLedgerHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableLeaveLedgerHeader1.AddCell(oPdfPCellLeaveLedgerHearder1);

                    oPdfPCellLeaveLedgerHearder1 = new PdfPCell(new Phrase(oItem.TotalLeave.ToString(), _oFontStyle));
                    oPdfPCellLeaveLedgerHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLeaveLedgerHearder1.Border = 0; oPdfPCellLeaveLedgerHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableLeaveLedgerHeader1.AddCell(oPdfPCellLeaveLedgerHearder1);

                    oPdfPCellLeaveLedgerHearder1 = new PdfPCell(new Phrase(oItem.EnjoyedInfo, _oFontStyle));
                    oPdfPCellLeaveLedgerHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLeaveLedgerHearder1.Border = 0; oPdfPCellLeaveLedgerHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableLeaveLedgerHeader1.AddCell(oPdfPCellLeaveLedgerHearder1);

                    oPdfPCellLeaveLedgerHearder1 = new PdfPCell(new Phrase(oItem.BalanceInfo, _oFontStyle));
                    oPdfPCellLeaveLedgerHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLeaveLedgerHearder1.Border = 0; oPdfPCellLeaveLedgerHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableLeaveLedgerHeader1.AddCell(oPdfPCellLeaveLedgerHearder1);

                }



                _oPdfPCell = new PdfPCell(oPdfPTableLeaveLedgerHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);

                #region Part 2 Salary Structure
                PdfPTable oPdfPTableSSHeader2 = new PdfPTable(2);
                oPdfPTableSSHeader2.SetWidths(new float[] { 150f, 266f });
                PdfPCell oPdfPCellSSHearder2;


                oPdfPCellSSHearder2 = new PdfPCell(new Phrase("SalaryHeadName", _oFontStyle));
                oPdfPCellSSHearder2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellSSHearder2.Border = 0; oPdfPCellSSHearder2.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableSSHeader2.AddCell(oPdfPCellSSHearder2);

                oPdfPCellSSHearder2 = new PdfPCell(new Phrase("Amount", _oFontStyle));
                oPdfPCellSSHearder2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellSSHearder2.Border = 0; oPdfPCellSSHearder2.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableSSHeader2.AddCell(oPdfPCellSSHearder2);
                oPdfPTableSSHeader2.CompleteRow();

                foreach (EmployeeSalaryStructureDetail oItem in _oEmployee.EmployeeSalaryStructureDetails)
                {
                    oPdfPCellSSHearder2 = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle));
                    oPdfPCellSSHearder2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellSSHearder2.Border = 0; oPdfPCellSSHearder2.BackgroundColor = BaseColor.WHITE; oPdfPTableSSHeader2.AddCell(oPdfPCellSSHearder2);

                    oPdfPCellSSHearder2 = new PdfPCell(new Phrase(oItem.Amount.ToString(), _oFontStyle));
                    oPdfPCellSSHearder2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellSSHearder2.Border = 0; oPdfPCellSSHearder2.BackgroundColor = BaseColor.WHITE; oPdfPTableSSHeader2.AddCell(oPdfPCellSSHearder2);

                }

                _oPdfPCell = new PdfPCell(oPdfPTableSSHeader2);
                _oPdfPCell.Border = 15;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
                #endregion

                #endregion

                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                #region Bank & Cash


                _oPdfPCell = new PdfPCell(new Phrase("Bank Amount : " + (_oEmployee.EmployeeSalaryStructures.Count > 0 ? (_oEmployee.EmployeeSalaryStructures[0].IsCashFixed == false) ? _oEmployee.EmployeeSalaryStructures[0].CashAmount.ToString() : "" : ""), _oFontStyle));
                _oPdfPCell.Border = 15;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(""));
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Cash Amount : " + (_oEmployee.EmployeeSalaryStructures.Count > 0 ? (_oEmployee.EmployeeSalaryStructures[0].IsCashFixed == true) ? _oEmployee.EmployeeSalaryStructures[0].CashAmount.ToString() : "" : ""), _oFontStyle));
                _oPdfPCell.Border = 15;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion


                _oPdfPCell = new PdfPCell(new Phrase("Personal Detail", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Personal Detail

                PdfPTable oPdfPTablePersonalDetailHeader1 = new PdfPTable(4);
                oPdfPTablePersonalDetailHeader1.SetWidths(new float[] { 120f, 300f, 120f, 280f });
                PdfPCell oPdfPCellPersonalDetailHearder1;

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Father's Name:", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.FatherName, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Gender :", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.Gender, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPTablePersonalDetailHeader1.CompleteRow();

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Mother's Name :", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.MotherName, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);


                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Marital Status :", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.MaritalStatus, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);


                oPdfPTablePersonalDetailHeader1.CompleteRow();

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Present Address :", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.PresentAddress, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);


                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Nationalism :", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.Nationalism, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPTablePersonalDetailHeader1.CompleteRow();


                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Permanent Address:", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.ParmanentAddress, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);


                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Blood Group:", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.BloodGroup, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPTablePersonalDetailHeader1.CompleteRow();

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("DOB:", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.DateOfBirthInString, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);


                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Cell:", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.ContactNo, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPTablePersonalDetailHeader1.CompleteRow();


                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Religion:", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.Religious, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);


                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase("Email:", _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPCellPersonalDetailHearder1 = new PdfPCell(new Phrase(_oEmployee.Email, _oFontStyle));
                oPdfPCellPersonalDetailHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPersonalDetailHearder1.Border = 0; oPdfPCellPersonalDetailHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePersonalDetailHeader1.AddCell(oPdfPCellPersonalDetailHearder1);

                oPdfPTablePersonalDetailHeader1.CompleteRow();



                _oPdfPCell = new PdfPCell(oPdfPTablePersonalDetailHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion


                _oPdfPCell = new PdfPCell(new Phrase("Nominee", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Nominee
                PdfPTable oPdfPTableNomineeHeader1 = new PdfPTable(5);
                oPdfPTableNomineeHeader1.SetWidths(new float[] { 200f, 220f, 120f, 180f, 100f });
                PdfPCell oPdfPCellNomineeHearder1;

                oPdfPCellNomineeHearder1 = new PdfPCell(new Phrase("Name", _oFontStyle));
                oPdfPCellNomineeHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellNomineeHearder1.Border = 0; oPdfPCellNomineeHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableNomineeHeader1.AddCell(oPdfPCellNomineeHearder1);

                oPdfPCellNomineeHearder1 = new PdfPCell(new Phrase("Address", _oFontStyle));
                oPdfPCellNomineeHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellNomineeHearder1.Border = 0; oPdfPCellNomineeHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableNomineeHeader1.AddCell(oPdfPCellNomineeHearder1);

                oPdfPCellNomineeHearder1 = new PdfPCell(new Phrase("ContactNo", _oFontStyle));
                oPdfPCellNomineeHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellNomineeHearder1.Border = 0; oPdfPCellNomineeHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableNomineeHeader1.AddCell(oPdfPCellNomineeHearder1);

                oPdfPCellNomineeHearder1 = new PdfPCell(new Phrase("Relation", _oFontStyle));
                oPdfPCellNomineeHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellNomineeHearder1.Border = 0; oPdfPCellNomineeHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableNomineeHeader1.AddCell(oPdfPCellNomineeHearder1);

                oPdfPCellNomineeHearder1 = new PdfPCell(new Phrase("Percentage", _oFontStyle));
                oPdfPCellNomineeHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellNomineeHearder1.Border = 0; oPdfPCellNomineeHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableNomineeHeader1.AddCell(oPdfPCellNomineeHearder1);

                oPdfPTableNomineeHeader1.CompleteRow();

                foreach (EmployeeNominee oItem in _oEmployee.EmployeeNominees)
                {
                    oPdfPCellNomineeHearder1 = new PdfPCell(new Phrase(oItem.Name, _oFontStyle));
                    oPdfPCellNomineeHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellNomineeHearder1.Border = 0; oPdfPCellNomineeHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableNomineeHeader1.AddCell(oPdfPCellNomineeHearder1);

                    oPdfPCellNomineeHearder1 = new PdfPCell(new Phrase(oItem.Address.ToString(), _oFontStyle));
                    oPdfPCellNomineeHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellNomineeHearder1.Border = 0; oPdfPCellNomineeHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableNomineeHeader1.AddCell(oPdfPCellNomineeHearder1);

                    oPdfPCellNomineeHearder1 = new PdfPCell(new Phrase(oItem.ContactNo, _oFontStyle));
                    oPdfPCellNomineeHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellNomineeHearder1.Border = 0; oPdfPCellNomineeHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableNomineeHeader1.AddCell(oPdfPCellNomineeHearder1);

                    oPdfPCellNomineeHearder1 = new PdfPCell(new Phrase(oItem.Relation, _oFontStyle));
                    oPdfPCellNomineeHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellNomineeHearder1.Border = 0; oPdfPCellNomineeHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableNomineeHeader1.AddCell(oPdfPCellNomineeHearder1);

                    oPdfPCellNomineeHearder1 = new PdfPCell(new Phrase(oItem.Percentage.ToString(), _oFontStyle));
                    oPdfPCellNomineeHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellNomineeHearder1.Border = 0; oPdfPCellNomineeHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableNomineeHeader1.AddCell(oPdfPCellNomineeHearder1);

                }

                _oPdfPCell = new PdfPCell(oPdfPTableNomineeHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion


                _oPdfPCell = new PdfPCell(new Phrase("Experience", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Experience
                PdfPTable oPdfPTableExperienceHeader1 = new PdfPTable(6);
                oPdfPTableExperienceHeader1.SetWidths(new float[] { 200f, 120f, 100f, 120f, 180f, 100f });
                PdfPCell oPdfPCellExperienceHearder1;

                oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase("Name", _oFontStyle));
                oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase("Address", _oFontStyle));
                oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase("Designation", _oFontStyle));
                oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase("StartDate", _oFontStyle));
                oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase("EndDate", _oFontStyle));
                oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase("MajorResponsibility", _oFontStyle));
                oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                oPdfPTableExperienceHeader1.CompleteRow();

                foreach (EmployeeExperience oItem in _oEmployee.EmployeeExperiences)
                {
                    oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase(oItem.Organization, _oFontStyle));
                    oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                    oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase(oItem.Address.ToString(), _oFontStyle));
                    oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                    oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase(oItem.Designation, _oFontStyle));
                    oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                    oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase(oItem.StartDate.ToString("dd MMM yyyy"), _oFontStyle));
                    oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                    oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase(oItem.EndDate.ToString("dd MMM yyyy"), _oFontStyle));
                    oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                    oPdfPCellExperienceHearder1 = new PdfPCell(new Phrase(oItem.MajorResponsibility.ToString(), _oFontStyle));
                    oPdfPCellExperienceHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellExperienceHearder1.Border = 0; oPdfPCellExperienceHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableExperienceHeader1.AddCell(oPdfPCellExperienceHearder1);

                }

                _oPdfPCell = new PdfPCell(oPdfPTableExperienceHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion

                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("Education", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Education
                PdfPTable oPdfPTableEducationHeader1 = new PdfPTable(6);
                oPdfPTableEducationHeader1.SetWidths(new float[] { 200f, 120f, 100f, 120f, 180f, 100f });
                PdfPCell oPdfPCellEducationHearder1;

                oPdfPCellEducationHearder1 = new PdfPCell(new Phrase("Degree", _oFontStyle));
                oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                oPdfPCellEducationHearder1 = new PdfPCell(new Phrase("Institution", _oFontStyle));
                oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                oPdfPCellEducationHearder1 = new PdfPCell(new Phrase("PassingYear", _oFontStyle));
                oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                oPdfPCellEducationHearder1 = new PdfPCell(new Phrase("Result", _oFontStyle));
                oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                oPdfPCellEducationHearder1 = new PdfPCell(new Phrase("BoardUniversity", _oFontStyle));
                oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                oPdfPCellEducationHearder1 = new PdfPCell(new Phrase("Country", _oFontStyle));
                oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                oPdfPTableEducationHeader1.CompleteRow();

                foreach (EmployeeEducation oItem in _oEmployee.EmployeeEducations)
                {
                    oPdfPCellEducationHearder1 = new PdfPCell(new Phrase(oItem.Degree, _oFontStyle));
                    oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                    oPdfPCellEducationHearder1 = new PdfPCell(new Phrase(oItem.Institution.ToString(), _oFontStyle));
                    oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                    oPdfPCellEducationHearder1 = new PdfPCell(new Phrase(oItem.PassingYear.ToString(), _oFontStyle));
                    oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                    oPdfPCellEducationHearder1 = new PdfPCell(new Phrase(oItem.Result, _oFontStyle));
                    oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                    oPdfPCellEducationHearder1 = new PdfPCell(new Phrase(oItem.BoardUniversity, _oFontStyle));
                    oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                    oPdfPCellEducationHearder1 = new PdfPCell(new Phrase(oItem.Country, _oFontStyle));
                    oPdfPCellEducationHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellEducationHearder1.Border = 0; oPdfPCellEducationHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableEducationHeader1.AddCell(oPdfPCellEducationHearder1);

                }

                _oPdfPCell = new PdfPCell(oPdfPTableEducationHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion


                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("Training", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Training
                PdfPTable oPdfPTableTrainingHeader1 = new PdfPTable(8);
                oPdfPTableTrainingHeader1.SetWidths(new float[] { 110f, 100f, 110f, 100f, 110f, 90f, 100f, 100f });
                PdfPCell oPdfPCellTrainingHearder1;

                oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase("CourseName", _oFontStyle));
                oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase("StartDate", _oFontStyle));
                oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase("EndDate", _oFontStyle));
                oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase("PassingDate", _oFontStyle));
                oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase("Result", _oFontStyle));
                oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase("Institution", _oFontStyle));
                oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase("CertifyBodyVendor", _oFontStyle));
                oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase("Country", _oFontStyle));
                oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                oPdfPTableTrainingHeader1.CompleteRow();

                foreach (EmployeeTraining oItem in _oEmployee.EmployeeTrainings)
                {
                    oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase(oItem.CourseName, _oFontStyle));
                    oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                    oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase(oItem.StartDate.ToString("dd MMM yyyy"), _oFontStyle));
                    oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                    oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase(oItem.EndDate.ToString("dd MMM yyyy"), _oFontStyle));
                    oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                    oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase(oItem.PassingDate.ToString("dd MMM yyyy"), _oFontStyle));
                    oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                    oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase(oItem.Result, _oFontStyle));
                    oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                    oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase(oItem.Institution, _oFontStyle));
                    oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                    oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase(oItem.CertifyBodyVendor, _oFontStyle));
                    oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                    oPdfPCellTrainingHearder1 = new PdfPCell(new Phrase(oItem.Country, _oFontStyle));
                    oPdfPCellTrainingHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTrainingHearder1.Border = 0; oPdfPCellTrainingHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableTrainingHeader1.AddCell(oPdfPCellTrainingHearder1);

                }

                _oPdfPCell = new PdfPCell(oPdfPTableTrainingHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion


                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("Attendnace History", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region AttHistory
                PdfPTable oPdfPTableAttHisHeader1 = new PdfPTable(15);
                oPdfPTableAttHisHeader1.SetWidths(new float[] { 56f, 56f, 56f, 56f, 56f, 56f, 56f, 56f, 56f, 56f, 56f, 56f, 56f, 56f, 56f });
                PdfPCell oPdfPCellAttHisHearder1;

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("W.Day", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("Present", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("Absent", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("FullLeave", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("HalfLeave", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("S.Leave", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("OSD", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("Late", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("Early", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("OTHour", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("DayOff", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("Holiday", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("PDayOff", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("PHoliday", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("BOA", _oFontStyle));
                oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                oPdfPTableAttHisHeader1.CompleteRow();

                foreach (MonthlyAttendanceReport oItem in _oMonthlyAttendanceReports)
                {
                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase(oItem.TotalWorkingDaySt, _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase(oItem.PresentDaySt, _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase(oItem.AbsentDaySt, _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase(oItem.OvertimeInhourSt, _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase(oItem.HoliDaySt, _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                    oPdfPCellAttHisHearder1 = new PdfPCell(new Phrase("", _oFontStyle));
                    oPdfPCellAttHisHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellAttHisHearder1.Border = 0; oPdfPCellAttHisHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableAttHisHeader1.AddCell(oPdfPCellAttHisHearder1);

                }

                _oPdfPCell = new PdfPCell(oPdfPTableAttHisHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion


                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("Leave History", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Leave History
                PdfPTable oPdfPTableLHHeader1 = new PdfPTable(3);
                oPdfPTableLHHeader1.SetWidths(new float[] { 110f, 100f, 110f });
                PdfPCell oPdfPCellLHHearder1;

                oPdfPCellLHHearder1 = new PdfPCell(new Phrase("Session", _oFontStyle));
                oPdfPCellLHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLHHearder1.Border = 0; oPdfPCellLHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableLHHeader1.AddCell(oPdfPCellLHHearder1);

                oPdfPCellLHHearder1 = new PdfPCell(new Phrase("CL", _oFontStyle));
                oPdfPCellLHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLHHearder1.Border = 0; oPdfPCellLHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableLHHeader1.AddCell(oPdfPCellLHHearder1);

                oPdfPCellLHHearder1 = new PdfPCell(new Phrase("SL", _oFontStyle));
                oPdfPCellLHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLHHearder1.Border = 0; oPdfPCellLHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableLHHeader1.AddCell(oPdfPCellLHHearder1);

                oPdfPTableLHHeader1.CompleteRow();

                foreach (EmployeeLeaveLedger oItem in _oEmployee.EmployeeLeaveLedgers)
                {
                    oPdfPCellLHHearder1 = new PdfPCell(new Phrase(oItem.Session, _oFontStyle));
                    oPdfPCellLHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLHHearder1.Border = 0; oPdfPCellLHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableLHHeader1.AddCell(oPdfPCellLHHearder1);

                    oPdfPCellLHHearder1 = new PdfPCell(new Phrase(oItem.CLDetailsInStr, _oFontStyle));
                    oPdfPCellLHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLHHearder1.Border = 0; oPdfPCellLHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableLHHeader1.AddCell(oPdfPCellLHHearder1);

                    oPdfPCellLHHearder1 = new PdfPCell(new Phrase(oItem.SLDetailsInStr, _oFontStyle));
                    oPdfPCellLHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellLHHearder1.Border = 0; oPdfPCellLHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableLHHeader1.AddCell(oPdfPCellLHHearder1);

                }

                _oPdfPCell = new PdfPCell(oPdfPTableLHHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion


                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("Increment History", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Increment History
                PdfPTable oPdfPTableIHHeader1 = new PdfPTable(4);
                oPdfPTableIHHeader1.SetWidths(new float[] { 110f, 100f, 110f, 110f });
                PdfPCell oPdfPCellIHHearder1;

                oPdfPCellIHHearder1 = new PdfPCell(new Phrase("EffectDate", _oFontStyle));
                oPdfPCellIHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellIHHearder1.Border = 0; oPdfPCellIHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableIHHeader1.AddCell(oPdfPCellIHHearder1);

                oPdfPCellIHHearder1 = new PdfPCell(new Phrase("PreviousSalary", _oFontStyle));
                oPdfPCellIHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellIHHearder1.Border = 0; oPdfPCellIHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableIHHeader1.AddCell(oPdfPCellIHHearder1);

                oPdfPCellIHHearder1 = new PdfPCell(new Phrase("PresentSalary", _oFontStyle));
                oPdfPCellIHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellIHHearder1.Border = 0; oPdfPCellIHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableIHHeader1.AddCell(oPdfPCellIHHearder1);

                oPdfPCellIHHearder1 = new PdfPCell(new Phrase("DesignationWhileIncrement", _oFontStyle));
                oPdfPCellIHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellIHHearder1.Border = 0; oPdfPCellIHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableIHHeader1.AddCell(oPdfPCellIHHearder1);

                oPdfPTableIHHeader1.CompleteRow();

                foreach (TransferPromotionIncrement oItem in _oEmployee.IncrementHistorys)
                {
                    oPdfPCellIHHearder1 = new PdfPCell(new Phrase(oItem.EffectedDateInString, _oFontStyle));
                    oPdfPCellIHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellIHHearder1.Border = 0; oPdfPCellIHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableIHHeader1.AddCell(oPdfPCellIHHearder1);

                    oPdfPCellIHHearder1 = new PdfPCell(new Phrase(oItem.GrossSalary.ToString(), _oFontStyle));
                    oPdfPCellIHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellIHHearder1.Border = 0; oPdfPCellIHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableIHHeader1.AddCell(oPdfPCellIHHearder1);

                    oPdfPCellIHHearder1 = new PdfPCell(new Phrase(oItem.TPIGrossSalary.ToString(), _oFontStyle));
                    oPdfPCellIHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellIHHearder1.Border = 0; oPdfPCellIHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableIHHeader1.AddCell(oPdfPCellIHHearder1);

                    oPdfPCellIHHearder1 = new PdfPCell(new Phrase(oItem.DesignationName, _oFontStyle));
                    oPdfPCellIHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellIHHearder1.Border = 0; oPdfPCellIHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableIHHeader1.AddCell(oPdfPCellIHHearder1);

                }

                _oPdfPCell = new PdfPCell(oPdfPTableIHHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion


                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("Transfer History", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Transfer History
                PdfPTable oPdfPTableTHHeader1 = new PdfPTable(2);
                oPdfPTableTHHeader1.SetWidths(new float[] { 110f, 100f });
                PdfPCell oPdfPCellTHHearder1;

                oPdfPCellTHHearder1 = new PdfPCell(new Phrase("PreviousLocation", _oFontStyle));
                oPdfPCellTHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTHHearder1.Border = 0; oPdfPCellTHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableTHHeader1.AddCell(oPdfPCellTHHearder1);

                oPdfPCellTHHearder1 = new PdfPCell(new Phrase("PresentLocation", _oFontStyle));
                oPdfPCellTHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTHHearder1.Border = 0; oPdfPCellTHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableTHHeader1.AddCell(oPdfPCellTHHearder1);

                oPdfPTableTHHeader1.CompleteRow();

                foreach (TransferPromotionIncrement oItem in _oEmployee.TransferHistorys)
                {
                    oPdfPCellTHHearder1 = new PdfPCell(new Phrase(oItem.LocationName, _oFontStyle));
                    oPdfPCellTHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTHHearder1.Border = 0; oPdfPCellTHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableTHHeader1.AddCell(oPdfPCellTHHearder1);

                    oPdfPCellTHHearder1 = new PdfPCell(new Phrase(oItem.TPILocationName, _oFontStyle));
                    oPdfPCellTHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellTHHearder1.Border = 0; oPdfPCellTHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableTHHeader1.AddCell(oPdfPCellTHHearder1);

                }

                _oPdfPCell = new PdfPCell(oPdfPTableTHHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion



                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("Promotion History", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Promotion History
                PdfPTable oPdfPTablePHHeader1 = new PdfPTable(2);
                oPdfPTablePHHeader1.SetWidths(new float[] { 110f, 100f });
                PdfPCell oPdfPCellPHHearder1;

                oPdfPCellPHHearder1 = new PdfPCell(new Phrase("PreviousDesignation", _oFontStyle));
                oPdfPCellPHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPHHearder1.Border = 0; oPdfPCellPHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTablePHHeader1.AddCell(oPdfPCellPHHearder1);

                oPdfPCellPHHearder1 = new PdfPCell(new Phrase("PresentLocation", _oFontStyle));
                oPdfPCellPHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPHHearder1.Border = 0; oPdfPCellPHHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTablePHHeader1.AddCell(oPdfPCellPHHearder1);

                oPdfPTablePHHeader1.CompleteRow();

                foreach (TransferPromotionIncrement oItem in _oEmployee.PromotionHistorys)
                {
                    oPdfPCellPHHearder1 = new PdfPCell(new Phrase(oItem.DesignationName, _oFontStyle));
                    oPdfPCellPHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPHHearder1.Border = 0; oPdfPCellPHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePHHeader1.AddCell(oPdfPCellPHHearder1);

                    oPdfPCellPHHearder1 = new PdfPCell(new Phrase(oItem.TPIDesignationName, _oFontStyle));
                    oPdfPCellPHHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellPHHearder1.Border = 0; oPdfPCellPHHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTablePHHeader1.AddCell(oPdfPCellPHHearder1);

                }

                _oPdfPCell = new PdfPCell(oPdfPTablePHHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion



                #region Blank

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #endregion

                _oPdfPCell = new PdfPCell(new Phrase("Disciplinary Action/Activity", _oFontStyle));
                _oPdfPCell.Border = 0;
                _oPdfPCell.FixedHeight = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                #region Promotion History
                PdfPTable oPdfPTableDAHeader1 = new PdfPTable(3);
                oPdfPTableDAHeader1.SetWidths(new float[] { 110f, 100f, 100f });
                PdfPCell oPdfPCellDAHearder1;

                oPdfPCellDAHearder1 = new PdfPCell(new Phrase("ActivityDate", _oFontStyle));
                oPdfPCellDAHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellDAHearder1.Border = 0; oPdfPCellDAHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableDAHeader1.AddCell(oPdfPCellDAHearder1);

                oPdfPCellDAHearder1 = new PdfPCell(new Phrase("Note", _oFontStyle));
                oPdfPCellDAHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellDAHearder1.Border = 0; oPdfPCellDAHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableDAHeader1.AddCell(oPdfPCellDAHearder1);

                oPdfPCellDAHearder1 = new PdfPCell(new Phrase("Approve By", _oFontStyle));
                oPdfPCellDAHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellDAHearder1.Border = 0; oPdfPCellDAHearder1.BackgroundColor = BaseColor.LIGHT_GRAY; oPdfPTableDAHeader1.AddCell(oPdfPCellDAHearder1);

                oPdfPTableDAHeader1.CompleteRow();

                foreach (EmployeeActivityNote oItem in _oEmployee.AcitivityNotes)
                {
                    oPdfPCellDAHearder1 = new PdfPCell(new Phrase(oItem.ActivityDateInStr, _oFontStyle));
                    oPdfPCellDAHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellDAHearder1.Border = 0; oPdfPCellDAHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableDAHeader1.AddCell(oPdfPCellDAHearder1);

                    oPdfPCellDAHearder1 = new PdfPCell(new Phrase(oItem.Note, _oFontStyle));
                    oPdfPCellDAHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellDAHearder1.Border = 0; oPdfPCellDAHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableDAHeader1.AddCell(oPdfPCellDAHearder1);

                    oPdfPCellDAHearder1 = new PdfPCell(new Phrase(oItem.ApproveByDateInStr, _oFontStyle));
                    oPdfPCellDAHearder1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellDAHearder1.Border = 0; oPdfPCellDAHearder1.BackgroundColor = BaseColor.WHITE; oPdfPTableDAHeader1.AddCell(oPdfPCellDAHearder1);

                }

                _oPdfPCell = new PdfPCell(oPdfPTableDAHeader1);
                _oPdfPCell.Border = 15;
                _oPdfPCell.Colspan = 3;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                #endregion



            }

        }
        #endregion

    }

}
