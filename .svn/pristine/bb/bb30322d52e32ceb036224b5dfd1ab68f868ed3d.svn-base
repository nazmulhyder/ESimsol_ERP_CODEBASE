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

    public class rptNPSalarySheetV1
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(11);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        EmployeeSalary _oEmployeeSalary = new EmployeeSalary();
        List<EmployeeSalary> _oEmployeeSalarys = new List<EmployeeSalary>();
        List<Employee> _oEmployees = new List<Employee>();
        List<AttendanceDaily> _oAttendanceDailys = new List<AttendanceDaily>();
        List<EmployeeSalaryDetail> _oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
        List<EmployeeSalaryDetail> _oDEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
        List<EmployeeSalaryDetailDisciplinaryAction> _oEmployeeSalaryDetailDisciplinaryActions = new List<EmployeeSalaryDetailDisciplinaryAction>();


        string _sStartDate = "";
        string _sEndDate = "";

        //for net salary calculation
        double _nBasic = 0;
        int _nTotalDay = 0;


        #endregion

        public byte[] PrepareReport(EmployeeSalary oEmployeeSalary)
        {
            _oEmployeeSalary = oEmployeeSalary;
            _oEmployeeSalarys = oEmployeeSalary.EmployeeSalarys;
            _oEmployees = oEmployeeSalary.Employees;
            _oAttendanceDailys = oEmployeeSalary.AttendanceDailys;
            _oEmployeeSalaryDetails = oEmployeeSalary.EmployeeSalaryDetails;
            _oEmployeeSalaryDetailDisciplinaryActions = oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions;
            _oCompany = oEmployeeSalary.Company;
            DateTime sStartDate = Convert.ToDateTime(oEmployeeSalary.ErrorMessage.Split(',')[0]);
            DateTime sEndDate = Convert.ToDateTime(oEmployeeSalary.ErrorMessage.Split(',')[1]);
            _sStartDate = sStartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");

            TimeSpan ts = sEndDate - sStartDate;
            _nTotalDay = ts.Days + 1;

            #region Page Setup

            _oDocument = new Document(new iTextSharp.text.Rectangle(1000, 700), 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 20f, 240f, 110f, 70f, 120f, 80f, 80f, 80f, 80f, 65f, 65f });

            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 6;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header

        private void PrintHeader()
        {
            #region CompanyHeader

            //_oFontStyle = FontFactory.GetFont("Tahoma", 14f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            //_oPdfPCell.Colspan = 11;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            //_oPdfPCell.Colspan = 11;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oPdfPCell = new PdfPCell(new Phrase(" "));
            //_oPdfPCell.Colspan = 11;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.FixedHeight = 7;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();


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
            oPdfPCellHearder = new PdfPCell(new Phrase("BGMEA Reg. No.-" + _oCompany.CompanyRegNo, _oFontStyle));
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
            _oPdfPCell.Colspan = 11;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Salary Sheet", _oFontStyle));
            _oPdfPCell.Colspan = 11;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = 11;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = 11;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 11;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 10;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body

        private void PrintBody()
        {

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Employee", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Salary Position", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Att. Status", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bonus", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Allowance", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Att. Deduction", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Over Time", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Deduction", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Net Salary", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();



            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            int nCount = 0;
            int nPageCount = 0;
            double nTotalGrossPerPage = 0;
            double nTotalBonusAndShortFallPerPage = 0;
            double nABonus = 0;
            double nAllowancePerEmployee = 0;
            double nAllowancePerPage = 0;
            double nAttDeductionPerEmployee = 0;
            double nAttDeductionPerPage = 0;
            double nOverTimeAmountPerPage = 0;
            double nDeductionPerPage = 0;
            double nNetSalaryPerEmployee = 0;
            double nNetSalaryPerPage = 0;

            double nTotalGross = 0;
            double nTotalBonusAndShortFall = 0;
            double nTotalAllowance = 0;
            double nTotalAttDeduction = 0;
            double nToatlOverTimeAmount = 0;
            double nTotalDeduction = 0;
            double nTotalNetSalary = 0;

            double nTotalProduction = 0;
            double nTotalProductionBonus = 0;
            double nTotalAttendanceBonus = 0;
            double nTotalAddShortFall = 0;
            double nTotalLeaveallowance = 0;
            double nToatlRevenueStamp = 0;



            foreach (EmployeeSalary oEmpSalaryItem in _oEmployeeSalarys)
            {

                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                foreach (Employee OEmpItem in _oEmployees)
                {
                    if (OEmpItem.EmployeeID == oEmpSalaryItem.EmployeeID)
                    {

                        _oPdfPCell = new PdfPCell(GetEmpOffical(OEmpItem));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(GetEmpSalary(oEmpSalaryItem.EmployeeSalaryID, oEmpSalaryItem.GrossAmount));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(GetAttendance(OEmpItem.EmployeeID));
                        double nAttTotal = 0;
                        nAttTotal = oEmpSalaryItem.TotalPresent + oEmpSalaryItem.TotalAbsent + oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave;
                        _oPdfPCell = new PdfPCell(new Phrase("Present: " + oEmpSalaryItem.TotalPresent + "\nAbsent: " + oEmpSalaryItem.TotalAbsent + "\nOff day: " + oEmpSalaryItem.TotalDayOff + "\nLeave: " + (oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave) + "\nTotal=" + nAttTotal, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        nABonus = 0;
                        nABonus = GetAttBonus(oEmpSalaryItem.EmployeeSalaryID);
                        _oPdfPCell = new PdfPCell(new Phrase("Att. Bonus: " + Global.MillionFormat(nABonus), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        nAllowancePerEmployee = 0;
                        DateTime dStartDate = Convert.ToDateTime(_oEmployeeSalary.ErrorMessage.Split(',')[0]);
                        if (dStartDate > OEmpItem.DateOfJoin)
                        {
                            nAllowancePerEmployee = _nBasic * oEmpSalaryItem.TotalPLeave / _nTotalDay;
                        }
                        else
                        {
                            nAllowancePerEmployee = GetLeaveAllowance(oEmpSalaryItem.EmployeeSalaryID);
                        }
                        _oPdfPCell = new PdfPCell(new Phrase("Leave: " + Global.MillionFormat(nAllowancePerEmployee) + "\nOthers: 0", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        nAttDeductionPerEmployee = 0;
                        nAttDeductionPerEmployee = GetAbsentDeduction(oEmpSalaryItem.EmployeeSalaryID);
                        _oPdfPCell = new PdfPCell(new Phrase("Absent: " + Global.MillionFormat(nAttDeductionPerEmployee), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("H: " + oEmpSalaryItem.OTHour + "\nR: " + oEmpSalaryItem.OTRatePerHour + "\nA: " + (oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("R. Sta.:" + oEmpSalaryItem.RevenueStemp + "\nAdvance: 0.00 \nTax: 0.00 \nOthers : 0", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        //DateTime dStartDate = Convert.ToDateTime(_oEmployeeSalary.ErrorMessage.Split(',')[0]);
                        if (dStartDate > OEmpItem.DateOfJoin)
                        {
                            nNetSalaryPerEmployee = (_nBasic / _nTotalDay) * (oEmpSalaryItem.TotalPresent + oEmpSalaryItem.TotalDayOff) + (oEmpSalaryItem.GrossAmount - _nBasic + oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour + nAllowancePerEmployee - 10);
                        }
                        else
                        {
                            nNetSalaryPerEmployee = oEmpSalaryItem.NetAmount;
                        }

                        //if (oEmpSalaryItem.NetAmount - oEmpSalaryItem.AddShort > 0)
                        //{
                        //    nNetSalaryPerEmployee = nNetSalaryPerEmployee - oEmpSalaryItem.AddShort;
                        //}
                        //else
                        //{
                        //    nNetSalaryPerEmployee = 0;
                        //}

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nNetSalaryPerEmployee), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPTable.CompleteRow();


                    }
                }

                nTotalGrossPerPage = nTotalGrossPerPage + oEmpSalaryItem.GrossAmount;
                //nTotalBonusAndShortFallPerPage = nTotalBonusAndShortFallPerPage + oEmpSalaryItem.ProductionAmount + oEmpSalaryItem.ProductionBonus + nABonus + oEmpSalaryItem.AddShort;
                nTotalBonusAndShortFallPerPage = nTotalBonusAndShortFallPerPage + nABonus;
                nAllowancePerPage = nAllowancePerPage + nAllowancePerEmployee;
                nAttDeductionPerPage = nAttDeductionPerPage + nAttDeductionPerEmployee;
                nOverTimeAmountPerPage = nOverTimeAmountPerPage + oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour;
                nDeductionPerPage = nDeductionPerPage + oEmpSalaryItem.RevenueStemp;
                nNetSalaryPerPage = nNetSalaryPerPage + nNetSalaryPerEmployee;

                nTotalGross = nTotalGross + oEmpSalaryItem.GrossAmount;
                //nTotalBonusAndShortFall = nTotalBonusAndShortFall + oEmpSalaryItem.ProductionAmount + oEmpSalaryItem.ProductionBonus + nABonus + oEmpSalaryItem.AddShort;
                nTotalBonusAndShortFall = nTotalBonusAndShortFall + nABonus;
                nTotalAllowance = nTotalAllowance + nAllowancePerEmployee;
                nTotalAttDeduction = nTotalAttDeduction + nAttDeductionPerEmployee;
                nToatlOverTimeAmount = nToatlOverTimeAmount + oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour;
                nTotalDeduction = nTotalDeduction + oEmpSalaryItem.RevenueStemp;
                nTotalNetSalary = nTotalNetSalary + nNetSalaryPerEmployee;

                //nTotalProduction = nTotalProduction + oEmpSalaryItem.ProductionAmount;
                nTotalProductionBonus = nTotalProductionBonus + oEmpSalaryItem.ProductionBonus;
                nTotalAttendanceBonus = nTotalAttendanceBonus + nABonus;
                nTotalAddShortFall = nTotalAddShortFall + oEmpSalaryItem.AddShort;
                nTotalLeaveallowance = nTotalLeaveallowance + nAllowancePerEmployee;
                nToatlRevenueStamp = nToatlRevenueStamp + oEmpSalaryItem.RevenueStemp;

                if (nCount == 8)
                {
                    nPageCount = 1;
                }
                if (nCount == 8 * nPageCount)
                {
                    nPageCount++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalGrossPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalBonusAndShortFallPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAllowancePerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAttDeductionPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nOverTimeAmountPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDeductionPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nNetSalaryPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();

                    nTotalGrossPerPage = 0;
                    nTotalBonusAndShortFallPerPage = 0;
                    nAllowancePerPage = 0;
                    nAttDeductionPerPage = 0;
                    nOverTimeAmountPerPage = 0;
                    nDeductionPerPage = 0;
                    nNetSalaryPerPage = 0;
                }
                if (_oEmployeeSalarys.Count == nCount)
                {

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyle)); _oPdfPCell.Colspan = 2;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalGrossPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalBonusAndShortFallPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAllowancePerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAttDeductionPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nOverTimeAmountPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDeductionPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nNetSalaryPerPage), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();
                }

            }

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Colspan = 11; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalGross), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalBonusAndShortFall), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalAllowance), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalAttDeduction), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nToatlOverTimeAmount), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalDeduction), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalNetSalary), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            //....................
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Colspan = 11; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Rowspan = 5; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Production/Bonus/Add Short Fall", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Allowance", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Deduction", _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Production ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalProduction), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Leave Allowance", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalLeaveallowance), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Revenue Stamp", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nToatlRevenueStamp), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Production Bonus ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalProductionBonus), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Others", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(0.00), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Advance", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("0.00", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Attendance Bonus ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalAttendanceBonus), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Tax", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("0.00", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Add Short Fall ", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalAddShortFall), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Others", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("0.00", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            //..................


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 40; _oPdfPCell.Colspan = 11; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Recommended By", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


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
                oPdfPCell1.Rowspan = 4;

                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell1.PaddingTop = 30;
                oPdfPCell1.Border = 0;
                oPdfPCell1.FixedHeight = 60;
                oPdfPTable1.AddCell(oPdfPCell1);

            }
            else
            {
                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Rowspan = 4; oPdfPCell1.Border = 0;
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

            return oPdfPTable1;

        }

        public PdfPTable GetEmpSalary(int nEmpSalaryID, double nGrossAmount)
        {

            _nBasic = 0;
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            PdfPCell oPdfPCell2;

            oPdfPTable2.SetWidths(new float[] { 70f, 50f });

            foreach (EmployeeSalaryDetail oESDItem in _oEmployeeSalaryDetails)
            {
                if (oESDItem.SalaryHeadType == 1 && oESDItem.EmployeeSalaryID == nEmpSalaryID)
                {
                    oPdfPCell2 = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle)); oPdfPCell2.Border = 0;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                    oPdfPCell2 = new PdfPCell(new Phrase(Global.MillionFormat(oESDItem.Amount), _oFontStyle)); oPdfPCell2.Border = 0;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                    oPdfPTable2.CompleteRow();

                    if (oESDItem.SalaryHeadName == "Basic")
                    {
                        _nBasic = oESDItem.Amount;
                    }
                }
            }

            oPdfPCell2 = new PdfPCell(new Phrase("Gross Salary : ", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(Global.MillionFormat(nGrossAmount), _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            return oPdfPTable2;

        }

        //public PdfPTable GetAttendance(int nEmpID)
        //{
        //    PdfPTable oPdfPTable3 = new PdfPTable(2);
        //    PdfPCell oPdfPCell3;
        //    oPdfPTable3.SetWidths(new float[] { 75f, 25f });

        //    int nTotalPresentDay = 0;
        //    int nTotalAbsentDay = 0;
        //    int nLeave = 0;
        //    int nDayOff = 0;
        //    int nTotal = 0;

        //    foreach (AttendanceDaily oATDItem in _oAttendanceDailys)
        //    {
        //        if (oATDItem.EmployeeID == nEmpID)
        //        {
        //            if (oATDItem.OutTime.ToString("HH:mm") == "00:00" && oATDItem.InTime.ToString("HH:mm") == "00:00")
        //            {
        //                if (oATDItem.IsDayOff == true)
        //                {
        //                    nDayOff++;
        //                }
        //                else if (oATDItem.IsLeave == true)
        //                {
        //                    nLeave++;
        //                }
        //                else
        //                {
        //                    nTotalAbsentDay++;
        //                }
        //            }
        //            else
        //            {
        //                if (oATDItem.IsDayOff == true)
        //                {
        //                    nDayOff++;
        //                }
        //                else
        //                {
        //                    nTotalPresentDay++;
        //                }
        //            }

        //        }

        //    }

        //    nTotal = nTotalPresentDay + nTotalAbsentDay + nDayOff + nLeave;

        //    oPdfPCell3 = new PdfPCell(new Phrase("Present : ", _oFontStyle)); oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPCell3 = new PdfPCell(new Phrase(nTotalPresentDay.ToString(), _oFontStyle)); oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPTable3.CompleteRow();

        //    oPdfPCell3 = new PdfPCell(new Phrase("Absent : ", _oFontStyle)); oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPCell3 = new PdfPCell(new Phrase(nTotalAbsentDay.ToString(), _oFontStyle)); oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPTable3.CompleteRow();

        //    oPdfPCell3 = new PdfPCell(new Phrase("Off Day : ", _oFontStyle)); oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPCell3 = new PdfPCell(new Phrase(nDayOff.ToString(), _oFontStyle)); oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPTable3.CompleteRow();

        //    oPdfPCell3 = new PdfPCell(new Phrase("Leave : ", _oFontStyle)); oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPCell3 = new PdfPCell(new Phrase(nLeave.ToString(), _oFontStyle)); oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPTable3.CompleteRow();

        //    oPdfPCell3 = new PdfPCell(new Phrase("_______________", _oFontStyle)); oPdfPCell3.Colspan = 2; oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPTable3.CompleteRow();

        //    oPdfPCell3 = new PdfPCell(new Phrase("Total : ", _oFontStyle)); oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPCell3 = new PdfPCell(new Phrase(nTotal.ToString(), _oFontStyle)); oPdfPCell3.Border = 0;
        //    oPdfPCell3.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell3.BackgroundColor = BaseColor.WHITE; oPdfPTable3.AddCell(oPdfPCell3);

        //    oPdfPTable3.CompleteRow();


        //    return oPdfPTable3;
        //}

        public double GetAttBonus(int nEmployeeSalaryID)
        {
            double nAttBonus = 0;// allowance is considered as Bonus

            foreach (EmployeeSalaryDetailDisciplinaryAction oESDDA in _oEmployeeSalaryDetailDisciplinaryActions)
            {
                if (nEmployeeSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Addition" && oESDDA.Note == "AttBonus")
                {
                    nAttBonus = nAttBonus + oESDDA.Amount;

                }


            }
            return nAttBonus;
        }

        public double GetLeaveAllowance(int nEmployeeSalaryID)
        {

            double nLeaveBonus = 0;// allowance is considered as Bonus.nLeaveBonus may be nLeaveAllowance

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

                if (nEmployeeSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Deduction" && oESDDA.Note != "UnPaidLeave")
                {
                    nAbsentDeduction = nAbsentDeduction + oESDDA.Amount;

                }

            }
            return nAbsentDeduction;
        }

    }

}
