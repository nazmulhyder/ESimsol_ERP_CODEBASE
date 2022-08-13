﻿using System;
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
using ICS.Core.Framework;

namespace ESimSol.Reports
{
    public class rptGoldenSalarySheet_Corporate
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        bool bComp;

        Company _oCompany = new Company();
        List<RPTSalarySheet> _oEmployeeSalarys = new List<RPTSalarySheet>();
        List<RPTSalarySheet> _oSummaryEmployeeSalarys = new List<RPTSalarySheet>();
        List<Employee> _oEmployees = new List<Employee>();
        List<AttendanceDaily> _oAttendanceDailys = new List<AttendanceDaily>();
        List<RPTSalarySheetDetail> _oEmployeeSalaryDetails = new List<RPTSalarySheetDetail>();
        List<RPTSalarySheetDetail> _oDEmployeeSalaryDetails = new List<RPTSalarySheetDetail>();
        List<EmployeeSalaryDetailDisciplinaryAction> _oEmployeeSalaryDetailDisciplinaryActions = new List<EmployeeSalaryDetailDisciplinaryAction>();
        List<LeaveHead> _oLeaveHeads = new List<LeaveHead>();
        List<EmployeeLeaveOnAttendance> _oELOnAttendances = new List<EmployeeLeaveOnAttendance>();
        List<PayrollProcessManagement> _oPayrollProcessManagements = new List<PayrollProcessManagement>();

        string _sStartDate = "";
        string _sEndDate = "";
        int _nTotalRow = 0;
        bool _bHasOTAllowance = false;
        #endregion
        public byte[] PrepareReport(EmployeeSalary oEmployeeSalary, List<LeaveHead> oLeaveHeads, List<EmployeeLeaveOnAttendance> oELOnAttendances, List<PayrollProcessManagement> oPayrollProcessManagements, List<SalarySheetProperty> oSalarySheetPropertys, List<SalarySheetSignature> oSalarySheetSignatures)
        {
            _oEmployeeSalarys = oEmployeeSalary.EmployeeSalarySheets;
            _oEmployees = oEmployeeSalary.Employees;
            _oAttendanceDailys = oEmployeeSalary.AttendanceDailys;
            _oEmployeeSalaryDetails = oEmployeeSalary.EmployeeSalarySheetDetails;
            _oEmployeeSalaryDetailDisciplinaryActions = oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions;
            _oCompany = oEmployeeSalary.Company;
            _oLeaveHeads = oLeaveHeads;
            _oELOnAttendances = oELOnAttendances;
            _oPayrollProcessManagements = oPayrollProcessManagements;
            DateTime sStartDate = Convert.ToDateTime(oEmployeeSalary.ErrorMessage.Split(',')[0]);
            DateTime sEndDate = Convert.ToDateTime(oEmployeeSalary.ErrorMessage.Split(',')[1]);
            _sStartDate = sStartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");

            _bHasOTAllowance = oSalarySheetPropertys.Any(x => x.PropertyFor == 2 && x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.OTAllowance);

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(1000, 700), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(40f, 40f, 5f, 60f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();
            //PageEventHandler.signatures = new List<string>(new string[]{"Approve BY", "Receive By"});
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            oPDFWriter.PageEvent = PageEventHandler;

            //if (_oCompany.BaseAddress == "dachser")
            //{
            //    ESimSolFooter_Dachser PageEventHandler = new ESimSolFooter_Dachser();
            //    oPDFWriter.PageEvent = PageEventHandler;
            //}
            //else
            //{
            //    ESimSolFooter_APS PageEventHandler = new ESimSolFooter_APS();
            //    PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            //    oPDFWriter.PageEvent = PageEventHandler;
            //}
            
            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 20f, 200f, 80f, 50f, 55f, 65f, 80f, 60f, 55f });

            #endregion
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 6;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header

        private void PrintHeader()
        {
            #region CompanyHeader
            PdfPTable oPdfPTableHeader = new PdfPTable(3);
            oPdfPTableHeader.SetWidths(new float[] { 430f, 220f, 350f });
            PdfPCell oPdfPCellHearder;
            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(20f, 15f);
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCellHearder = new PdfPCell(new Phrase("" , _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            oPdfPCellHearder.Colspan = 3;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Salary Sheet", _oFontStyle));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Salary Month-" + (_oPayrollProcessManagements.Count > 0 ? _oPayrollProcessManagements[0].MonthIDInString : ""), _oFontStyle));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 10;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion

        #region Report Body
        private void PrintHaedRow(RPTSalarySheet oES)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Unit:" + oES.LocationName + "   ||   " + "Department:" + oES.DepartmentName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Employee", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Salary Position", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Att. Status", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Leave Status", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Allowance", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Deduction", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Net Salary", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }

        private void PrintBody()
        {
            if (_oEmployeeSalarys.Count <= 0)
            {
                this.PrintHeader();
                _oPdfPCell = new PdfPCell(new Phrase("Nothing to print !!", _oFontStyle)); _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }
            else
            {
                _nTotalRow = _oEmployeeSalarys.Count;
                _oEmployeeSalarys = _oEmployeeSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                _oEmployeeSalarys.ForEach(x => _oSummaryEmployeeSalarys.Add(x));
                while (_oEmployeeSalarys.Count > 0)
                {
                    List<RPTSalarySheet> oTempEmployeeSalarys = new List<RPTSalarySheet>();
                    oTempEmployeeSalarys = _oEmployeeSalarys.Where(x => x.LocationName == _oEmployeeSalarys[0].LocationName).ToList();
                    string sLocationName = oTempEmployeeSalarys.Count > 0 ? oTempEmployeeSalarys[0].LocationName : "";
                    while (oTempEmployeeSalarys.Count > 0)
                    {
                        List<RPTSalarySheet> oTempEmpSs = new List<RPTSalarySheet>();
                        oTempEmpSs = oTempEmployeeSalarys.Where(x => x.DepartmentName == oTempEmployeeSalarys[0].DepartmentName).ToList();
                        PrintSalarySheet(oTempEmpSs);
                        oTempEmployeeSalarys.RemoveAll(x => x.DepartmentName == oTempEmpSs[0].DepartmentName);
                    }
                    _oEmployeeSalarys.RemoveAll(x => x.LocationName == sLocationName);
                }
                this.Footer();
            }
        }

        int nCount = 0;
        int nPageCount = 0;
        int nTotalCount = 0;

        double nTotalGrossPerPage = 0;
        double nTotalGross = 0;

        double nTotalAllowancePerPage = 0;
        double nTotalAllowance = 0;

        double nTotalDeductionPerPage = 0;
        double nTotalDeduction = 0;

        double nTotalNetSalaryPerPage = 0;
        double nTotalNetSalary = 0;

        public void PrintSalarySheet(List<RPTSalarySheet> oEmployeeSalarys)
        {
            nCount = 0;
            nPageCount = 0;
            this.PrintHeader();
            PrintHaedRow(oEmployeeSalarys[0]);
            foreach (RPTSalarySheet oEmpSalaryItem in oEmployeeSalarys)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                nCount++;
                nTotalCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                foreach (Employee OEmpItem in _oEmployees)
                {
                    if (OEmpItem.EmployeeID == oEmpSalaryItem.EmployeeID)
                    {
                        _oPdfPCell = new PdfPCell(GetEmpOffical(OEmpItem)); _oPdfPCell.PaddingLeft = -20;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(GetEmpSalary(oEmpSalaryItem.EmployeeSalaryID, oEmpSalaryItem.GrossAmount));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        double nAttTotal = 0;

                        nAttTotal = oEmpSalaryItem.Present + oEmpSalaryItem.TotalAbsent + oEmpSalaryItem.TotalDOff + oEmpSalaryItem.TotalHoliday + oEmpSalaryItem.TotalLeave;
                        _oPdfPCell = new PdfPCell(new Phrase("Present: " + oEmpSalaryItem.Present + "\nAbsent: " + oEmpSalaryItem.TotalAbsent + "\nOff day: " + oEmpSalaryItem.TotalDOff + "\nLeave: " + (oEmpSalaryItem.TotalLeave) + "\nHoliday: " + oEmpSalaryItem.TotalHoliday + "\nTotal=" + nAttTotal, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    
                        _oPdfPCell = new PdfPCell(new Phrase(GetLeaveStatus(oEmpSalaryItem.EmployeeID), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(GetAdditionSalary(oEmpSalaryItem));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(GetDeductionSalary(oEmpSalaryItem));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
                        oEmpSalaryItem.NetAmount += (_bHasOTAllowance) ? 0 : - (oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour);
                        _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.NetAmount > 0 ? this.GetAmountInStr(oEmpSalaryItem.NetAmount, true, false) : "-", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPTable.CompleteRow();
                    }
                }

                nTotalGrossPerPage = nTotalGrossPerPage + oEmpSalaryItem.GrossAmount;
                nTotalGross = nTotalGross + oEmpSalaryItem.GrossAmount;
                nTotalNetSalaryPerPage = nTotalNetSalaryPerPage + oEmpSalaryItem.NetAmount;
                nTotalNetSalary = nTotalNetSalary + oEmpSalaryItem.NetAmount;
            
                if (nCount == 6)
                {
                    nPageCount = 1;
                }
                if (nCount == 6 * nPageCount)
                {
                    nPageCount++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nTotalGrossPerPage > 0 ? this.GetAmountInStr(nTotalGrossPerPage, true, false) : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nTotalAllowancePerPage > 0 ? this.GetAmountInStr(nTotalAllowancePerPage, true, false) : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nTotalDeductionPerPage > 0 ? this.GetAmountInStr(nTotalDeductionPerPage, true, false) : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(nTotalNetSalaryPerPage > 0 ? this.GetAmountInStr(nTotalNetSalaryPerPage, true, false) : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();

                    nTotalGrossPerPage = 0;
                    nTotalAllowancePerPage = 0;
                    nTotalDeductionPerPage = 0;
                    nTotalNetSalaryPerPage = 0;

                    if (_nTotalRow > nTotalCount)
                    {
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();
                    }

                    if (oEmployeeSalarys.Count != nCount)
                    {
                        this.PrintHeader();
                        PrintHaedRow(oEmployeeSalarys[0]);
                    }
                }
                if (oEmployeeSalarys.Count == nCount && oEmployeeSalarys.Count % 6 != 0)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nTotalGrossPerPage > 0 ? this.GetAmountInStr(nTotalGrossPerPage, true, false) : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nTotalAllowancePerPage > 0 ? this.GetAmountInStr(nTotalAllowancePerPage, true, false) : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(nTotalDeductionPerPage > 0 ? this.GetAmountInStr(nTotalDeductionPerPage, true, false) : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(nTotalNetSalaryPerPage> 0 ? this.GetAmountInStr(nTotalNetSalaryPerPage, true, false) : "-", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();

                    nTotalGrossPerPage = 0;
                    nTotalAllowancePerPage = 0;
                    nTotalDeductionPerPage = 0;
                    nTotalNetSalaryPerPage = 0;
                }
            }

            if (_nTotalRow > nTotalCount)
            {
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
            }
        }

        public void Footer()
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nTotalGross > 0 ? this.GetAmountInStr(nTotalGross, true, false) : "-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nTotalAllowance > 0 ? this.GetAmountInStr(nTotalAllowance, true, false) : "-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nTotalDeduction > 0 ? this.GetAmountInStr(nTotalDeduction, true, false) : "-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(nTotalNetSalary > 0 ? this.GetAmountInStr(nTotalNetSalary, true, false) : "-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();



            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            this.PrintHeader();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9; _oPdfPCell.FixedHeight = 35;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

           
            _oPdfPCell = new PdfPCell(Summary()); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }

        #endregion
        
        public PdfPTable GetEmpOffical(Employee oEmp)
        {
            PdfPTable oPdfPTable1 = new PdfPTable(3);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 60f, 60f, 80f });

            if (oEmp.EmployeePhoto != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oEmp.EmployeePhoto, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(55f, 60f);
                oPdfPCell1 = new PdfPCell(_oImag);
                oPdfPCell1.Rowspan = 6;

                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell1.PaddingTop = 30;
                oPdfPCell1.Border = 0;
                oPdfPCell1.FixedHeight = 60;
                oPdfPTable1.AddCell(oPdfPCell1);
            }
            else
            {
                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Rowspan = 6; oPdfPCell1.Border = 0;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
            }

            oPdfPCell1 = new PdfPCell(new Phrase("Card No. : ", _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(oEmp.Code, _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            oPdfPCell1 = new PdfPCell(new Phrase("Name : ", _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(oEmp.Name, _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            oPdfPCell1 = new PdfPCell(new Phrase("Designation : ", _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(oEmp.DesignationName, _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            oPdfPCell1 = new PdfPCell(new Phrase("Date Of Join : ", _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(oEmp.DateOfJoinInString, _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            oPdfPCell1 = new PdfPCell(new Phrase("Gender : ", _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(oEmp.Gender, _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            oPdfPCell1 = new PdfPCell(new Phrase("Employee Type : ", _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(oEmp.EmployeeTypeName, _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();


            return oPdfPTable1;
        }

        public PdfPTable GetEmpSalary(int nEmpSalaryID, double nGrossAmount)
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            PdfPCell oPdfPCell2;

            oPdfPTable2.SetWidths(new float[] { 70f, 50f });

            foreach (RPTSalarySheetDetail oESDItem in _oEmployeeSalaryDetails)
            {
                if (oESDItem.SalaryHeadType == 1 && oESDItem.EmployeeSalaryID == nEmpSalaryID)
                {
                    oPdfPCell2 = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle)); oPdfPCell2.Border = 0;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                    oPdfPCell2 = new PdfPCell(new Phrase(oESDItem.Amount > 0 ? this.GetAmountInStr(oESDItem.Amount, true, false) : "-", _oFontStyle)); oPdfPCell2.Border = 0;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                    oPdfPTable2.CompleteRow();
                }
            }

            oPdfPCell2 = new PdfPCell(new Phrase("Gross Salary : ", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(nGrossAmount > 0 ? this.GetAmountInStr(nGrossAmount, true, false) : "-", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            return oPdfPTable2;
        }

        public double GetLeaveAllowance(int nEmployeeSalaryID)
        {
            double nLeaveBonus = 0;
            foreach (EmployeeSalaryDetailDisciplinaryAction oESDDA in _oEmployeeSalaryDetailDisciplinaryActions)
            {
                if (nEmployeeSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Addition" && oESDDA.Note == "PaidLeave")
                {
                    nLeaveBonus = nLeaveBonus + oESDDA.Amount;
                }
            }
            return nLeaveBonus;
        }

        public double GetAbsentDeduction(int nEmployeeSalaryID)
        {
            double nAbsentDeduction = 0;
            foreach (EmployeeSalaryDetailDisciplinaryAction oESDDA in _oEmployeeSalaryDetailDisciplinaryActions)
            {
                if (nEmployeeSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Deduction")
                {
                    string sNote = ""; 
                    if (oESDDA.Note.Length > 6) { 
                        sNote = oESDDA.Note.Substring(0, 6);
                        if (sNote == "Absent")
                        {
                            nAbsentDeduction = nAbsentDeduction + oESDDA.Amount;
                        }
                    }                    
                }
            }
            return nAbsentDeduction;
        }

        public double GetLoan(int nEmployeeSalaryID)
        {
            double nLoanAmount = 0;
            foreach (EmployeeSalaryDetailDisciplinaryAction oESDDA in _oEmployeeSalaryDetailDisciplinaryActions)
            {
                if (nEmployeeSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Deduction" && oESDDA.Note == "Loan")
                {
                    nLoanAmount = nLoanAmount + oESDDA.Amount;
                }
            }
            return nLoanAmount;
        }
        public double GetAdvanceDeduction(int nEmployeeSalaryID)
        {
            double nAbsentDeduction = 0;
            foreach (EmployeeSalaryDetailDisciplinaryAction oESDDA in _oEmployeeSalaryDetailDisciplinaryActions)
            {
                if (nEmployeeSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Deduction" && oESDDA.Note == "Advance")
                {
                    nAbsentDeduction = nAbsentDeduction + oESDDA.Amount;
                }
            }
            return nAbsentDeduction;
        }
        public PdfPTable GetAdditionSalary(RPTSalarySheet oEmpSalaryItem)
        {
            double nLeaveAllowancePerEmployee = 0;
            //nLeaveAllowancePerEmployee = GetLeaveAllowance(oEmpSalaryItem.EmployeeSalaryID);

            PdfPTable oPdfPTable_Addition = new PdfPTable(2);
            PdfPCell oPdfPCell_Addition;

            oPdfPTable_Addition.SetWidths(new float[] { 70f, 50f });

            foreach (RPTSalarySheetDetail oESDItem in _oEmployeeSalaryDetails)
            {
                if (oESDItem.SalaryHeadType == 2 && oESDItem.EmployeeSalaryID == oEmpSalaryItem.EmployeeSalaryID)
                {
                    oPdfPCell_Addition = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                    oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                    oPdfPCell_Addition = new PdfPCell(new Phrase(oESDItem.Amount > 0 ? this.GetAmountInStr(oESDItem.Amount, true, false) : "-", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                    oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                    oPdfPTable_Addition.CompleteRow();

                    nTotalAllowancePerPage += oESDItem.Amount;
                    nTotalAllowance += oESDItem.Amount;
                }
            }

            if (_bHasOTAllowance)
            {
                oPdfPCell_Addition = new PdfPCell(new Phrase("OT Rate: ", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                oPdfPCell_Addition = new PdfPCell(new Phrase(oEmpSalaryItem.OTRatePerHour > 0 ? Global.MillionFormat(oEmpSalaryItem.OTRatePerHour) : "-", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                oPdfPTable_Addition.CompleteRow();

                oPdfPCell_Addition = new PdfPCell(new Phrase("OT Hr.: ", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                oPdfPCell_Addition = new PdfPCell(new Phrase(oEmpSalaryItem.OTHour > 0 ? oEmpSalaryItem.OTHour.ToString() : "-", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                oPdfPTable_Addition.CompleteRow();

                oPdfPCell_Addition = new PdfPCell(new Phrase("OT All. : ", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                oPdfPCell_Addition = new PdfPCell(new Phrase(oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour > 0 ? this.GetAmountInStr(oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour, true, false) : "-", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                oPdfPTable_Addition.CompleteRow();

                nTotalAllowancePerPage += oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour;
                nTotalAllowance += oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour;
            }

            return oPdfPTable_Addition;
        }

        public PdfPTable GetDeductionSalary(RPTSalarySheet oEmpSalaryItem)
        {
            PdfPTable oPdfPTable_Deduction = new PdfPTable(2);
            PdfPCell oPdfPCell_Deduction;

            oPdfPTable_Deduction.SetWidths(new float[] { 70f, 50f });

            foreach (RPTSalarySheetDetail oESDItem in _oEmployeeSalaryDetails)
            {
                if (oESDItem.SalaryHeadType == 3 && oESDItem.EmployeeSalaryID == oEmpSalaryItem.EmployeeSalaryID)
                {
                    oPdfPCell_Deduction = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle)); oPdfPCell_Deduction.Border = 0;
                    oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

                    oPdfPCell_Deduction = new PdfPCell(new Phrase(oESDItem.Amount > 0 ? this.GetAmountInStr(oESDItem.Amount, true, false) : "-", _oFontStyle)); oPdfPCell_Deduction.Border = 0;
                    oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

                    oPdfPTable_Deduction.CompleteRow();
                    nTotalDeductionPerPage += oESDItem.Amount;
                    nTotalDeduction += oESDItem.Amount;
                }
            }
            return oPdfPTable_Deduction;
        }

        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount) : amount;
            return (bWithPrecision) ? Global.MillionFormat(amount) : Global.MillionFormat(amount).Split('.')[0];
        }

        private string GetLeaveStatus(int nEmployeeID)
        {
            string Status = "";
            List<EmployeeLeaveOnAttendance> oELOnAtts = new List<EmployeeLeaveOnAttendance>();
            oELOnAtts = _oELOnAttendances.Where(x => x.EmployeeID == nEmployeeID).ToList();
            foreach (LeaveHead oLeaveHead in _oLeaveHeads)
            {
                string sValue="";
                if((oELOnAtts.Where(x => x.LeaveHeadID == oLeaveHead.LeaveHeadID).Any())){sValue = oELOnAtts.Where(x => x.LeaveHeadID == oLeaveHead.LeaveHeadID).FirstOrDefault().LeaveDays.ToString();}
                //if (!string.IsNullOrEmpty(sValue))
                //{
                //    Status += oLeaveHead.ShortName + "-" + sValue+",";
                //}
                Status += oLeaveHead.ShortName + "-" + sValue+", ";
            }
            if (!string.IsNullOrEmpty(Status)) {Status=Status.Remove(Status.Length - 2); };
            return Status;
        }

        public PdfPTable Summary()
        {
            List<RPTSalarySheetDetail> _oEmployeeSalaryDetailsAdd = new List<RPTSalarySheetDetail>();
            List<RPTSalarySheetDetail> _oEmployeeSalaryDetailsDed = new List<RPTSalarySheetDetail>();
            _oEmployeeSalaryDetailsAdd = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadType == (int)EnumSalaryHeadType.Addition).GroupBy(x => x.SalaryHeadID).Select(y => y.First()).ToList();
            _oEmployeeSalaryDetailsDed = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction).GroupBy(x => x.SalaryHeadID).Select(y => y.First()).ToList();

            int nColumn = _oEmployeeSalaryDetailsAdd.Count() + _oEmployeeSalaryDetailsDed.Count();
            PdfPTable oPdfPTable1 = new PdfPTable(nColumn+2);
            PdfPCell oPdfPCell1;
            float[] sCol = new float[nColumn+2];
            sCol[0] = 60;
            sCol[1] = 30;
            for (int i = 2; i<nColumn+2;i++)
            {
                sCol[i] = 30;
            }
            oPdfPTable1.SetWidths(sCol);


            List<RPTSalarySheet> _oBUEmployeeSalarys = new List<RPTSalarySheet>();
            _oBUEmployeeSalarys = _oSummaryEmployeeSalarys.GroupBy(x => x.BusinessUnitID).Select(y => y.First()).ToList();


            foreach (RPTSalarySheet oBUEmployeeSalary in _oBUEmployeeSalarys)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                oPdfPCell1 = new PdfPCell(new Phrase("Unit : " + oBUEmployeeSalary.BUName, _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = nColumn+2;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
               
                oPdfPTable1.CompleteRow();

                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                if(_oEmployeeSalaryDetailsAdd.Count>0)
                {
                    oPdfPCell1 = new PdfPCell(new Phrase("Addition", _oFontStyle)); oPdfPCell1.Colspan = _oEmployeeSalaryDetailsAdd.Count+1;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                }

                if (_oEmployeeSalaryDetailsDed.Count > 0)
                {
                    oPdfPCell1 = new PdfPCell(new Phrase("Deduction", _oFontStyle)); oPdfPCell1.Colspan = _oEmployeeSalaryDetailsDed.Count;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                }

                oPdfPTable1.CompleteRow();


                oPdfPCell1 = new PdfPCell(new Phrase("Department", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("OT", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                if (_oEmployeeSalaryDetailsAdd.Count > 0)
                {
                    foreach (RPTSalarySheetDetail oESD in _oEmployeeSalaryDetailsAdd)
                    {
                        oPdfPCell1 = new PdfPCell(new Phrase(oESD.SalaryHeadName, _oFontStyle)); 
                        oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    }
                }

                if (_oEmployeeSalaryDetailsDed.Count > 0)
                {
                    foreach (RPTSalarySheetDetail oESD in _oEmployeeSalaryDetailsDed)
                    {
                        oPdfPCell1 = new PdfPCell(new Phrase(oESD.SalaryHeadName, _oFontStyle));
                        oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    }
                }

                oPdfPTable1.CompleteRow();


                List<RPTSalarySheet> oDeptEmployeeSalarys = new List<RPTSalarySheet>();
                oDeptEmployeeSalarys = _oSummaryEmployeeSalarys.GroupBy(x => x.DepartmentID).Select(y => y.First()).ToList();

                List<RPTSalarySheet> _oBlockEmployeeSalarys = new List<RPTSalarySheet>();
                _oBlockEmployeeSalarys = _oSummaryEmployeeSalarys.GroupBy(x => x.BlockName).Select(y => y.First()).Where(x => x.BlockName != "").ToList();

                foreach (RPTSalarySheet oDeptEmployeeSalary in oDeptEmployeeSalarys)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

                    oPdfPCell1 = new PdfPCell(new Phrase(oDeptEmployeeSalary.DepartmentName, _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);
                    double nOTAmt = _oSummaryEmployeeSalarys.Where(x => x.DepartmentID == oDeptEmployeeSalary.DepartmentID && x.BusinessUnitID == oBUEmployeeSalary.BusinessUnitID).Sum(x => x.OTHour * x.OTRatePerHour);
                    oPdfPCell1 = new PdfPCell(new Phrase(Global.MillionFormat(nOTAmt), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                    List<RPTSalarySheet> oTempEmpSals = new List<RPTSalarySheet>();
                    oTempEmpSals = _oSummaryEmployeeSalarys.Where(x => x.DepartmentID == oDeptEmployeeSalary.DepartmentID && x.BusinessUnitID == oBUEmployeeSalary.BusinessUnitID).ToList();
                    foreach (RPTSalarySheetDetail oESD in _oEmployeeSalaryDetailsAdd)
                    {
                        double nAmt = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadID == oESD.SalaryHeadID).Where(x => oTempEmpSals.Select(y => y.EmployeeSalaryID).Contains(x.EmployeeSalaryID)).Sum(x => x.Amount);
                        oPdfPCell1 = new PdfPCell(new Phrase(Global.MillionFormat(nAmt), _oFontStyle)); 
                        oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    }
                    foreach (RPTSalarySheetDetail oESD in _oEmployeeSalaryDetailsDed)
                    {
                        double nAmt = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadID == oESD.SalaryHeadID).Where(x => oTempEmpSals.Select(y => y.EmployeeSalaryID).Contains(x.EmployeeSalaryID)).Sum(x => x.Amount);
                        oPdfPCell1 = new PdfPCell(new Phrase(Global.MillionFormat(nAmt), _oFontStyle)); 
                        oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    }
                    oPdfPTable1.CompleteRow();
                }

                oPdfPCell1 = new PdfPCell(new Phrase("" , _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = nColumn + 2;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPTable1.CompleteRow();

                if (_oBlockEmployeeSalarys.Count > 0)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                    oPdfPCell1 = new PdfPCell(new Phrase("Block", _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPCell1 = new PdfPCell(new Phrase("OT", _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    if (_oEmployeeSalaryDetailsAdd.Count > 0)
                    {
                        foreach (RPTSalarySheetDetail oESD in _oEmployeeSalaryDetailsAdd)
                        {
                            oPdfPCell1 = new PdfPCell(new Phrase(oESD.SalaryHeadName, _oFontStyle));
                            oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                        }
                    }

                    if (_oEmployeeSalaryDetailsDed.Count > 0)
                    {
                        foreach (RPTSalarySheetDetail oESD in _oEmployeeSalaryDetailsDed)
                        {
                            oPdfPCell1 = new PdfPCell(new Phrase(oESD.SalaryHeadName, _oFontStyle));
                            oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                        }
                    }

                    oPdfPTable1.CompleteRow();
                }

                foreach (RPTSalarySheet oBlockEmployeeSalary in _oBlockEmployeeSalarys)
                {
                    oPdfPCell1 = new PdfPCell(new Phrase("Block : " + oBlockEmployeeSalary.BlockName, _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);


                    double nOTAmt = _oSummaryEmployeeSalarys.Where(x => x.BlockName == oBlockEmployeeSalary.BlockName && x.BusinessUnitID == oBUEmployeeSalary.BusinessUnitID).Sum(x => x.OTHour * x.OTRatePerHour);
                    oPdfPCell1 = new PdfPCell(new Phrase(Global.MillionFormat(nOTAmt), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    List<RPTSalarySheet> oTempEmpSals = new List<RPTSalarySheet>();
                    oTempEmpSals = _oSummaryEmployeeSalarys.Where(x => x.BlockName == oBlockEmployeeSalary.BlockName && x.BusinessUnitID == oBUEmployeeSalary.BusinessUnitID).ToList();
                    foreach (RPTSalarySheetDetail oESD in _oEmployeeSalaryDetailsAdd)
                    {
                        double nAmt = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadID == oESD.SalaryHeadID).Where(x => oTempEmpSals.Select(y => y.EmployeeSalaryID).Contains(x.EmployeeSalaryID)).Sum(x => x.Amount);
                        oPdfPCell1 = new PdfPCell(new Phrase(Global.MillionFormat(nAmt), _oFontStyle));
                        oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    }
                    foreach (RPTSalarySheetDetail oESD in _oEmployeeSalaryDetailsDed)
                    {
                        double nAmt = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadID == oESD.SalaryHeadID).Where(x => oTempEmpSals.Select(y => y.EmployeeSalaryID).Contains(x.EmployeeSalaryID)).Sum(x => x.Amount);
                        oPdfPCell1 = new PdfPCell(new Phrase(Global.MillionFormat(nAmt), _oFontStyle));
                        oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    }
                }
            }
            return oPdfPTable1;
        }
    }

}

