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

    public class rptSalarySheet_First
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(12);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        List<EmployeeSalary> _oEmployeeSalarys = new List<EmployeeSalary>();
        List<Employee> _oEmployees = new List<Employee>();
        List<AttendanceDaily> _oAttendanceDailys = new List<AttendanceDaily>();
        List<EmployeeSalaryDetail> _oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
        List<EmployeeSalaryDetail> _oDEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
        List<EmployeeSalaryDetailDisciplinaryAction> _oEmployeeSalaryDetailDisciplinaryActions = new List<EmployeeSalaryDetailDisciplinaryAction>();

        string _sStartDate = "";
        string _sEndDate = "";
        int _nMonthCycle = 0;

        #endregion
        public byte[] PrepareReport(EmployeeSalary oEmployeeSalary)
        {
            _oEmployeeSalarys = oEmployeeSalary.EmployeeSalarys;
            _oEmployees = oEmployeeSalary.Employees;
            _oAttendanceDailys = oEmployeeSalary.AttendanceDailys;
            _oEmployeeSalaryDetails = oEmployeeSalary.EmployeeSalaryDetails;
            _oEmployeeSalaryDetailDisciplinaryActions = oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions;
            _oCompany = oEmployeeSalary.Company;
            _sStartDate = oEmployeeSalary.ErrorMessage.Split(',')[0];
            _sEndDate = oEmployeeSalary.ErrorMessage.Split(',')[1];

            DateTime dDateFrom = Convert.ToDateTime(_sStartDate);
            DateTime dDateTo = Convert.ToDateTime(_sEndDate);

            TimeSpan ts = dDateTo - dDateFrom;
            _nMonthCycle = ts.Days + 1;

            #region Page Setup

            _oDocument = new Document(new iTextSharp.text.Rectangle(1200, 700), 0f, 0f, 0f, 0f);
            //_oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 20f, 250f, 110f, 70f, 88f, 70f, 60f, 70f, 60f, 65f, 60f, 75f });

            #endregion
            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 5;
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
                _oImag.ScaleAbsolute(200f, 50f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.FixedHeight = 55;
                oPdfPCellHearder.Colspan = 3;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellHearder.PaddingBottom = 2;
                oPdfPCellHearder.Border = 0;

                oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 15; oPdfPCellHearder.Colspan = 3;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }

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
            _oPdfPCell.Colspan = 12;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.FixedHeight = 75;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Salary Sheet", _oFontStyle));
            _oPdfPCell.Colspan = 12;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = 12;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = 12;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 12;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 10;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion

        #region Report Body
      
        //col-5
        double nSalaryDepartmentWise = 0;
        double nSalaryTotal = 0;

        //col-6
        //double nAllowanceEmployeeWise = 0;
        //double nAllowanceDepartmentWise = 0;
        //double nAllowanceTotal = 0;

        //col-7
        double nNoWorkDepartmentWise = 0;
        double nNoWorkTotal = 0;

        //col-8
        double nOverTimeDepartmentWise = 0;
        double nOverTimeTotal = 0;

        //col-9
        double nEarnSalaryEmployeeWise = 0;
        double nEarnSalaryDepartmentWise = 0;
        double nEarnSalaryTotal = 0;

        //col-10
        double nSubsidyAmountDepartmentWise = 0;
        double nSubsidyAmountTotal = 0;
        //col-11
        double nDeductionEmployeeWise = 0;
        double nDeductionDepartmentWise = 0;
        double nDeductionTotal = 0;

        //col-12
        double nNetSalaryDepartmentWise = 0;
        double nNetSalaryTotal = 0;

        double nSalary = 0;

        double nTotalProduction = 0;
        double nTotalProductionBonus = 0;
        double nTotalLeaveallowance = 0;
        double nTotalRevenueStamp = 0;
        double nTotalAttendanceBonus = 0;
        int nCount = 0;

        private void PrintHaedRow(EmployeeSalary oES)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Department:" + oES.DepartmentName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 12;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Employee", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Salary Structure", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Att. Status", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Salary/Pro./Bonus", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Allowance", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Remark", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("No Work", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Over Time", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Earn Salary", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Subsidy Amount", _oFontStyle));
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
            _oEmployeeSalarys = _oEmployeeSalarys.OrderBy(x => x.DepartmentName).ToList();
            while (_oEmployeeSalarys.Count > 0)
            {
                List<EmployeeSalary> oTempEmployeeSalarys = new List<EmployeeSalary>();
                oTempEmployeeSalarys = _oEmployeeSalarys.Where(x => x.DepartmentName == _oEmployeeSalarys[0].DepartmentName).OrderBy(x=>x.EmployeeCode).ToList();

                PrintSalarySheet(oTempEmployeeSalarys);
                DepartmentWiseTotal();
                InitializeDeptWise();
                _oEmployeeSalarys.RemoveAll(x => x.DepartmentName == oTempEmployeeSalarys[0].DepartmentName);
            }
            Footer();
        }

        public void PrintSalarySheet(List<EmployeeSalary> oEmployeeSalarys)
        {
            PrintHaedRow(oEmployeeSalarys[0]);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            nCount = 0;
            foreach (EmployeeSalary oEmpSalaryItem in oEmployeeSalarys)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                foreach (Employee OEmpItem in _oEmployees)
                {
                    if (OEmpItem.EmployeeID == oEmpSalaryItem.EmployeeID)
                    {
                       
                        InitializeEmployeeWise();
                        _oPdfPCell = new PdfPCell(GetEmpOffical(OEmpItem));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(GetEmpSalary(oEmpSalaryItem.EmployeeSalaryID, oEmpSalaryItem.GrossAmount));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        double nAttTotal = 0;
                        //double nTotalDays = 0;
                        nAttTotal = oEmpSalaryItem.TotalPresent+ oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave+ oEmpSalaryItem.TotalHoliday;
                        //nTotalDays = oEmpSalaryItem.TotalPresent + oEmpSalaryItem.TotalAbsent + oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave + oEmpSalaryItem.TotalHoliday;
                        _oPdfPCell = new PdfPCell(new Phrase("Present: " + oEmpSalaryItem.TotalPresent + "\nAbsent: " + oEmpSalaryItem.TotalAbsent + "\nOff day: " + oEmpSalaryItem.TotalDayOff + "\nHoly day:" + oEmpSalaryItem.TotalHoliday + "\nLeave: " + (oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave) + "\nTotal=" + nAttTotal + "\nDay=" + _nMonthCycle, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        
                        //GetAll(oEmpSalaryItem.EmployeeSalaryID);//allowance
                        GetDeduction(oEmpSalaryItem.EmployeeSalaryID, oEmpSalaryItem.RevenueStemp);
                        if (oEmpSalaryItem.IsProductionBase)
                        {
                            nEarnSalaryEmployeeWise = oEmpSalaryItem.NetAmount_ZN + nDeductionEmployeeWise;
                            nSalary = nEarnSalaryEmployeeWise - oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour - oEmpSalaryItem.TotalNoWorkDayAllowance;
                            //oEmpSalaryItem.ProductionAmount
                            //nEarnSalaryEmployeeWise = oEmpSalaryItem.ProductionAmount + nAllowanceEmployeeWise + oEmpSalaryItem.TotalNoWorkDayAllowance + oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour;
                            nSalaryDepartmentWise = nSalaryDepartmentWise + oEmpSalaryItem.ProductionAmount + oEmpSalaryItem.ProductionBonus;
                            _oPdfPCell = new PdfPCell(new Phrase("Pro.: " + Global.MillionFormat(nSalary), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            nTotalProduction += oEmpSalaryItem.ProductionAmount;
                            nTotalProductionBonus += oEmpSalaryItem.ProductionBonus;
                        }
                        else
                        {
                            nEarnSalaryEmployeeWise = oEmpSalaryItem.NetAmount_ZN + nDeductionEmployeeWise;
                            nSalary = nEarnSalaryEmployeeWise - oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour - oEmpSalaryItem.TotalNoWorkDayAllowance;
                            //nEarnSalaryEmployeeWise = nSalary + nAllowanceEmployeeWise + oEmpSalaryItem.TotalNoWorkDayAllowance + oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour;
                            nSalaryDepartmentWise = nSalaryDepartmentWise + nSalary;
                            _oPdfPCell = new PdfPCell(new Phrase("Salary: " + Global.MillionFormat(nSalary), _oFontStyle));
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }

                        //_oPdfPCell = new PdfPCell(GetAll_Table(oEmpSalaryItem.EmployeeSalaryID));
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        //_oPdfPCell = new PdfPCell(new Phrase(AllowanceNameWithDay(oEmpSalaryItem), _oFontStyle));
                        //_oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("No work: " + oEmpSalaryItem.TotalNoWorkDay + "\nAllow.: " + Global.MillionFormat(oEmpSalaryItem.TotalNoWorkDayAllowance), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("H: " +Global.MillionFormat( oEmpSalaryItem.OTHour) + "\nR: " + Global.MillionFormat(oEmpSalaryItem.OTRatePerHour) + "\nA: " + Global.MillionFormat(oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nEarnSalaryEmployeeWise), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("0.00", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(GetDeduction_Table(oEmpSalaryItem.EmployeeSalaryID, oEmpSalaryItem.RevenueStemp));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        nTotalRevenueStamp += oEmpSalaryItem.RevenueStemp;

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.NetAmount_ZN), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPTable.CompleteRow();

                    }
                }

              
                nNoWorkDepartmentWise = nNoWorkDepartmentWise + oEmpSalaryItem.TotalNoWorkDayAllowance;
                nOverTimeDepartmentWise = nOverTimeDepartmentWise + oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour;
                nEarnSalaryDepartmentWise = nEarnSalaryDepartmentWise + nEarnSalaryEmployeeWise;
                nSubsidyAmountDepartmentWise = nSubsidyAmountDepartmentWise + 0;
                nNetSalaryDepartmentWise = nNetSalaryDepartmentWise + oEmpSalaryItem.NetAmount_ZN;

                nSalaryTotal += nSalary;
                //nAllowanceTotal += nAllowanceEmployeeWise;
                nNoWorkTotal = nNoWorkTotal + oEmpSalaryItem.TotalNoWorkDayAllowance;
                nOverTimeTotal= nOverTimeTotal+ oEmpSalaryItem.OTHour;
                nEarnSalaryTotal = nEarnSalaryTotal + nEarnSalaryEmployeeWise;
                nSubsidyAmountTotal = nSubsidyAmountTotal + 0;
                nDeductionTotal = nDeductionTotal + nDeductionEmployeeWise;
                nNetSalaryTotal = nNetSalaryTotal + oEmpSalaryItem.NetAmount_ZN;

                nTotalProduction = nTotalProduction + oEmpSalaryItem.ProductionAmount;
                nTotalProductionBonus = nTotalProductionBonus + oEmpSalaryItem.ProductionBonus;
                nTotalRevenueStamp = nTotalRevenueStamp + oEmpSalaryItem.RevenueStemp;
           
            }
        }

        public void DepartmentWiseTotal()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Total :", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSalaryDepartmentWise), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAllowanceDepartmentWise), _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nNoWorkDepartmentWise), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nOverTimeDepartmentWise), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nEarnSalaryDepartmentWise), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSubsidyAmountDepartmentWise), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDeductionDepartmentWise), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nNetSalaryDepartmentWise), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 40; _oPdfPCell.Colspan = 12; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }
        public void Footer()
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Colspan = 12; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total : ", _oFontStyle)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSalaryTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAllowanceTotal), _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nNoWorkTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nOverTimeTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nEarnSalaryTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nSubsidyAmountTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDeductionTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nNetSalaryTotal), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            //....................
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Colspan = 12; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            //_oPdfPCell = new PdfPCell(new Phrase("Production/Bonus", _oFontStyle)); _oPdfPCell.Colspan = 3;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Allowance", _oFontStyle)); _oPdfPCell.Colspan = 4;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("Deduction", _oFontStyle)); _oPdfPCell.Colspan = 4;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            //List<EmployeeSalaryDetail> oESDs = new List<EmployeeSalaryDetail>();
            //oESDs = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadType == 2 || x.SalaryHeadType == 4).ToList();
            //oESDs = oESDs.GroupBy(x => x.SalaryHeadID).Select(x => x.First()).ToList();

            //List<EmployeeSalaryDetail> oESDs_Ded = new List<EmployeeSalaryDetail>();
            //oESDs_Ded = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadType == 3).ToList();
            //oESDs_Ded = oESDs_Ded.GroupBy(x => x.SalaryHeadID).Select(x => x.First()).ToList();


            //_oPdfPCell = new PdfPCell(ProductionGTTable()); _oPdfPCell.Colspan = 3; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; 
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(AllowanceGTTable(oESDs)); _oPdfPCell.Colspan = 4; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; 
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPCell = new PdfPCell(DeductionGTTable(oESDs_Ded)); _oPdfPCell.Colspan = 4; _oPdfPCell.VerticalAlignment = Element.ALIGN_TOP;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            //_oPdfPTable.CompleteRow();

            //..................
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 60; _oPdfPCell.Colspan = 12; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Recommended By", _oFontStyle)); _oPdfPCell.Colspan = 4; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

        }
        #endregion

        public PdfPTable GetEmpOffical(Employee oEmp)
        {
            PdfPTable oPdfPTable1 = new PdfPTable(3);
            PdfPCell oPdfPCell1;

            oPdfPTable1.SetWidths(new float[] { 40f, 40f, 100f });

            if (oEmp.EmployeePhoto != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(oEmp.EmployeePhoto, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(55f, 65f);
                oPdfPCell1 = new PdfPCell(_oImag);
                oPdfPCell1.Rowspan = 5;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCell1.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell1.PaddingTop = 30;
                oPdfPCell1.Border = 0;
                oPdfPCell1.FixedHeight = 65;
                oPdfPTable1.AddCell(oPdfPCell1);
            }
            else
            {
                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Rowspan = 5; oPdfPCell1.Border = 0;
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

            oPdfPCell1 = new PdfPCell(new Phrase("Grade : ", _oFontStyle)); oPdfPCell1.Border = 0;
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

            oPdfPTable2.SetWidths(new float[] { 65f, 60f });

            foreach (EmployeeSalaryDetail oESDItem in _oEmployeeSalaryDetails)
            {
                if (oESDItem.SalaryHeadType == 1 && oESDItem.EmployeeSalaryID == nEmpSalaryID)
                {
                    oPdfPCell2 = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle)); oPdfPCell2.Border = 0;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                    oPdfPCell2 = new PdfPCell(new Phrase(Global.MillionFormat(oESDItem.Amount), _oFontStyle)); oPdfPCell2.Border = 0;
                    oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                    oPdfPTable2.CompleteRow();
                }
            }

            oPdfPCell2 = new PdfPCell(new Phrase("Gross Salary : ", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(Global.MillionFormat(nGrossAmount), _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            return oPdfPTable2;
        }

        //double nAttBonus = 0;
        //double nLeaveBonus = 0;
        //public void GetAll(int nEmpSalaryID)
        //{
        //    nAttBonus = 0;
        //    nLeaveBonus = 0;

        //    foreach (EmployeeSalaryDetailDisciplinaryAction oESDDA in _oEmployeeSalaryDetailDisciplinaryActions)
        //    {
        //        if (nEmpSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Addition" && oESDDA.Note == "AttBonus")
        //        {
        //            nAttBonus = nAttBonus + oESDDA.Amount;
        //        }
        //        if (nEmpSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Addition" && oESDDA.Note == "PaidLeave")
        //        {
        //            nLeaveBonus = nLeaveBonus + oESDDA.Amount;
                   
        //        }
        //    }
        //    if (nAttBonus > 0)
        //    {
        //        nAllowanceEmployeeWise += nAttBonus;
        //        nAllowanceDepartmentWise += nAttBonus;
        //        nTotalAttendanceBonus+=nAttBonus;
        //    }

        //    if (nLeaveBonus > 0)
        //    {
        //        nAllowanceEmployeeWise += nLeaveBonus;
        //        nAllowanceDepartmentWise += nLeaveBonus;
        //        nTotalLeaveallowance += nLeaveBonus;
        //    }

        //    //foreach (EmployeeSalaryDetail oESDItem in _oEmployeeSalaryDetails)
        //    //{
        //    //    if ((oESDItem.SalaryHeadType == 2 || oESDItem.SalaryHeadType == 4) && oESDItem.EmployeeSalaryID == nEmpSalaryID)
        //    //    {
        //    //        nAllowanceEmployeeWise += oESDItem.Amount;
        //    //        nAllowanceDepartmentWise += oESDItem.Amount;
        //    //    }
        //    //}
            
        //}

        //public PdfPTable GetAll_Table(int nEmpSalaryID)
        //{
        //    PdfPTable oPdfPTable_All = new PdfPTable(2);//allowance
        //    PdfPCell oPdfPCell_All;
        //    oPdfPTable_All.SetWidths(new float[] { 60f, 60f });
        //    if (nAttBonus>0)
        //    {
        //        oPdfPCell_All = new PdfPCell(new Phrase("Att.:", _oFontStyle)); oPdfPCell_All.Border = 0;
        //        oPdfPCell_All.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //        oPdfPCell_All = new PdfPCell(new Phrase(Global.MillionFormat(nAttBonus), _oFontStyle)); oPdfPCell_All.Border = 0;
        //        oPdfPCell_All.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //        oPdfPTable_All.CompleteRow();
        //    }

        //    if (nLeaveBonus > 0)
        //    {
        //        oPdfPCell_All = new PdfPCell(new Phrase("Leave:", _oFontStyle)); oPdfPCell_All.Border = 0;
        //        oPdfPCell_All.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //        oPdfPCell_All = new PdfPCell(new Phrase(Global.MillionFormat(nLeaveBonus), _oFontStyle)); oPdfPCell_All.Border = 0;
        //        oPdfPCell_All.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //        oPdfPTable_All.CompleteRow();

        //    }

        //    //foreach (EmployeeSalaryDetail oESDItem in _oEmployeeSalaryDetails)
        //    //{
        //    //    if ((oESDItem.SalaryHeadType == 2 || oESDItem.SalaryHeadType == 4) && oESDItem.EmployeeSalaryID == nEmpSalaryID)
        //    //    {
        //    //        oPdfPCell_All = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle)); oPdfPCell_All.Border = 0;
        //    //        oPdfPCell_All.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //    //        oPdfPCell_All = new PdfPCell(new Phrase(Global.MillionFormat(oESDItem.Amount), _oFontStyle)); oPdfPCell_All.Border = 0;
        //    //        oPdfPCell_All.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //    //        oPdfPTable_All.CompleteRow();
        //    //    }
        //    //}

        //    oPdfPTable_All.CompleteRow();

        //    return oPdfPTable_All;
        //}

        double nAbsentDeduction = 0;
        double nAdv = 0;
        public void GetDeduction(int nEmpSalaryID, double nRs)
        {
            nAbsentDeduction = 0;
            nAdv = 0;

            foreach (EmployeeSalaryDetailDisciplinaryAction oESDDA in _oEmployeeSalaryDetailDisciplinaryActions)
            {
                if (nEmpSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Deduction" && oESDDA.Note == "UnPaidLeave")
                {
                    nAbsentDeduction = nAbsentDeduction + oESDDA.Amount;
                }
                if (nEmpSalaryID == oESDDA.EmployeeSalaryID && oESDDA.ActionName == "Deduction" && oESDDA.Note == "Advance Payment")
                {
                    nAdv = nAdv + oESDDA.Amount;
                }
            }

            nDeductionEmployeeWise+=nRs;
            nDeductionDepartmentWise+=nRs;

            if (nAbsentDeduction > 0)
            {
                nDeductionEmployeeWise += nAbsentDeduction;
                nDeductionDepartmentWise += nAbsentDeduction;
            }
            if (nAdv > 0)
            {
                nDeductionEmployeeWise += nAdv;
                nDeductionDepartmentWise += nAdv;
            }

            foreach (EmployeeSalaryDetail oESDItem in _oEmployeeSalaryDetails)
            {
                if ((oESDItem.SalaryHeadType == 3) && oESDItem.EmployeeSalaryID == nEmpSalaryID)
                {
                    nDeductionEmployeeWise+=oESDItem.Amount;
                    nDeductionDepartmentWise+=oESDItem.Amount;
                }
            }
        }
        public PdfPTable GetDeduction_Table(int nEmpSalaryID, double nRs)
        {
            PdfPTable oPdfPTable_Deduction = new PdfPTable(2);
            PdfPCell oPdfPCell_Deduction;
            oPdfPTable_Deduction.SetWidths(new float[] { 70f, 50f });

            oPdfPCell_Deduction = new PdfPCell(new Phrase("R. Sta.:", _oFontStyle)); oPdfPCell_Deduction.Border = 0;
            oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

            oPdfPCell_Deduction = new PdfPCell(new Phrase(Global.MillionFormat(nRs), _oFontStyle)); oPdfPCell_Deduction.Border = 0;
            oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

            oPdfPTable_Deduction.CompleteRow();

            if (nAbsentDeduction > 0)
            {
                oPdfPCell_Deduction = new PdfPCell(new Phrase("Absent:", _oFontStyle)); oPdfPCell_Deduction.Border = 0;
                oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

                oPdfPCell_Deduction = new PdfPCell(new Phrase(Global.MillionFormat(nAbsentDeduction), _oFontStyle)); oPdfPCell_Deduction.Border = 0;
                oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

                oPdfPTable_Deduction.CompleteRow();
            }
            if (nAdv > 0)
            {
                oPdfPCell_Deduction = new PdfPCell(new Phrase("Adv.:", _oFontStyle)); oPdfPCell_Deduction.Border = 0;
                oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

                oPdfPCell_Deduction = new PdfPCell(new Phrase(Global.MillionFormat(nAdv), _oFontStyle)); oPdfPCell_Deduction.Border = 0;
                oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

                oPdfPTable_Deduction.CompleteRow();
            }

            foreach (EmployeeSalaryDetail oESDItem in _oEmployeeSalaryDetails)
            {
                if ((oESDItem.SalaryHeadType == 3) && oESDItem.EmployeeSalaryID == nEmpSalaryID)
                {
                    oPdfPCell_Deduction = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle)); oPdfPCell_Deduction.Border = 0;
                    oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

                    oPdfPCell_Deduction = new PdfPCell(new Phrase(Global.MillionFormat(oESDItem.Amount), _oFontStyle)); oPdfPCell_Deduction.Border = 0;
                    oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

                    oPdfPTable_Deduction.CompleteRow();
                }
            }

            oPdfPTable_Deduction.CompleteRow();

            return oPdfPTable_Deduction;
        }
        //public PdfPTable AllowanceGTTable(List<EmployeeSalaryDetail> oESDs)
        //{
        //    PdfPTable oPdfPTable_All = new PdfPTable(2);
        //    PdfPCell oPdfPCell_All;
        //    oPdfPTable_All.SetWidths(new float[] { 60f, 60f });
        //    //if (nTotalAttendanceBonus > 0)
        //    //{
        //        oPdfPCell_All = new PdfPCell(new Phrase("Att. Bonus:", _oFontStyle));
        //        oPdfPCell_All.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //        oPdfPCell_All = new PdfPCell(new Phrase(Global.MillionFormat(nAttBonus), _oFontStyle)); 
        //        oPdfPCell_All.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //        oPdfPTable_All.CompleteRow();
        //    //}

        //    //if (nTotalLeaveallowance > 0)
        //    //{
        //        oPdfPCell_All = new PdfPCell(new Phrase("Leave:", _oFontStyle)); 
        //        oPdfPCell_All.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //        oPdfPCell_All = new PdfPCell(new Phrase(Global.MillionFormat(nLeaveBonus), _oFontStyle)); 
        //        oPdfPCell_All.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //        oPdfPTable_All.CompleteRow();

        //    //}
            
        //    //foreach (EmployeeSalaryDetail oESDItem in oESDs)
        //    //{
        //    //    double nAmount = 0;
        //    //    nAmount = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadID == oESDItem.SalaryHeadID).Sum(x => x.Amount);
        //    //    oPdfPCell_All = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle));
        //    //    oPdfPCell_All.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //    //    oPdfPCell_All = new PdfPCell(new Phrase(Global.MillionFormat(nAmount), _oFontStyle));
        //    //    oPdfPCell_All.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_All.BackgroundColor = BaseColor.WHITE; oPdfPTable_All.AddCell(oPdfPCell_All);

        //    //    oPdfPTable_All.CompleteRow();
        //    //}

        //    oPdfPTable_All.CompleteRow();

        //    return oPdfPTable_All;
        //}

        //public PdfPTable DeductionGTTable(List<EmployeeSalaryDetail> oESDs)
        //{
        //    PdfPTable oPdfPTable_Deduction = new PdfPTable(2);
        //    PdfPCell oPdfPCell_Deduction;
        //    oPdfPTable_Deduction.SetWidths(new float[] { 70f, 50f });

        //    oPdfPCell_Deduction = new PdfPCell(new Phrase("R. Sta.:", _oFontStyle)); 
        //    oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

        //    oPdfPCell_Deduction = new PdfPCell(new Phrase(Global.MillionFormat(nTotalRevenueStamp), _oFontStyle)); oPdfPCell_Deduction.Border = 0;
        //    oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

        //    oPdfPTable_Deduction.CompleteRow();

        //    //if (nAbsentDeduction > 0)
        //    //{
        //        oPdfPCell_Deduction = new PdfPCell(new Phrase("Absent:", _oFontStyle)); 
        //        oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

        //        oPdfPCell_Deduction = new PdfPCell(new Phrase(Global.MillionFormat(nAbsentDeduction), _oFontStyle)); 
        //        oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

        //        oPdfPTable_Deduction.CompleteRow();
        //    //}
        //    //if (nAdv > 0)
        //    //{
        //        oPdfPCell_Deduction = new PdfPCell(new Phrase("Adv.:", _oFontStyle));
        //        oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

        //        oPdfPCell_Deduction = new PdfPCell(new Phrase(Global.MillionFormat(nAdv), _oFontStyle));
        //        oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

        //        oPdfPTable_Deduction.CompleteRow();
        //    //}

        //        foreach (EmployeeSalaryDetail oESDItem in oESDs)
        //        {
        //            double nAmount = 0;
        //            nAmount = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadID == oESDItem.SalaryHeadID).Sum(x => x.Amount);

        //            oPdfPCell_Deduction = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle));
        //            oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

        //            oPdfPCell_Deduction = new PdfPCell(new Phrase(Global.MillionFormat(nAmount), _oFontStyle));
        //            oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

        //            oPdfPTable_Deduction.CompleteRow();
        //        }

        //    oPdfPTable_Deduction.CompleteRow();

        //    return oPdfPTable_Deduction;
        //}

        //public PdfPTable ProductionGTTable()
        //{
        //    PdfPTable oPdfPTable_Production = new PdfPTable(2);
        //    PdfPCell oPdfPCell_Production;
        //    oPdfPTable_Production.SetWidths(new float[] { 70f, 50f });

        //    oPdfPCell_Production = new PdfPCell(new Phrase("Production ", _oFontStyle));
        //    oPdfPCell_Production.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Production.BackgroundColor = BaseColor.WHITE; oPdfPTable_Production.AddCell(oPdfPCell_Production);

        //    oPdfPCell_Production = new PdfPCell(new Phrase(Global.MillionFormat(nTotalProduction), _oFontStyle));
        //    oPdfPCell_Production.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Production.BackgroundColor = BaseColor.WHITE; oPdfPTable_Production.AddCell(oPdfPCell_Production);

        //    oPdfPCell_Production = new PdfPCell(new Phrase("Production Bonus ", _oFontStyle)); _oPdfPCell.Colspan = 2;
        //    oPdfPCell_Production.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Production.BackgroundColor = BaseColor.WHITE; oPdfPTable_Production.AddCell(oPdfPCell_Production);

        //    oPdfPCell_Production = new PdfPCell(new Phrase(Global.MillionFormat(nTotalProductionBonus), _oFontStyle));
        //    oPdfPCell_Production.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Production.BackgroundColor = BaseColor.WHITE; oPdfPTable_Production.AddCell(oPdfPCell_Production);

        //    oPdfPTable_Production.CompleteRow();

        //    return oPdfPTable_Production;
        //}

        public string AllowanceNameWithDay(EmployeeSalary oES)
        {
            string S = "";
            string[] sANWDs;
            sANWDs = oES.AllowanceNameWithDay.Split('~');
            foreach (string sANWD in sANWDs)
            {
                S += sANWD + "\n";
            }
            return S;
        }
        public void InitializeDeptWise()
        {
            nSalaryDepartmentWise = 0;
            //nAllowanceDepartmentWise = 0;
            nNoWorkDepartmentWise = 0;
            nOverTimeDepartmentWise = 0;
            nEarnSalaryDepartmentWise = 0;
            nSubsidyAmountDepartmentWise = 0;
            nDeductionDepartmentWise = 0;
            nNetSalaryDepartmentWise = 0;
        }
        public void InitializeEmployeeWise()
        {
            //nAllowanceEmployeeWise = 0;
            nEarnSalaryEmployeeWise = 0;
            nDeductionEmployeeWise = 0;
        }
    }

}
