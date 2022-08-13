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
using ICS.Core.Framework;

namespace ESimSol.Reports
{
    public class rptFinalSettlementSalaryAMG
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        List<EmployeeSettlement> _oEmployeeSettlements = new List<EmployeeSettlement>();
        List<EmployeeSettlementSalary> _oEmployeeSettlementSalarys = new List<EmployeeSettlementSalary>();
        List<EmployeeSettlementSalary> _oSummaryEmployeeSettlementSalarys = new List<EmployeeSettlementSalary>();
        List<EmployeeSettlementSalaryDetail> _oEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
        List<EmployeeSettlementSalaryDetail> _oTEmployeeSettlementSalaryDetails = new List<EmployeeSettlementSalaryDetail>();
        List<AttendanceDaily> _oAttendanceDailys = new List<AttendanceDaily>();
        List<LeaveHead> _oLeaveHeads = new List<LeaveHead>();
        List<Employee> _oEmployees = new List<Employee>();
        List<EmployeeLeaveOnAttendance> _oELOnAttendances = new List<EmployeeLeaveOnAttendance>();
        double nExceptSalaryHead = 0.0;

        string _sStartDate = "";
        string _sEndDate = "";
        int _nTotalRow = 0;
        bool _bHasOTAllowance = true;
        int _nGroupDept = 1;
        #endregion
        public byte[] PrepareReport(List<EmployeeSettlement> oEmployeeSettlements, List<EmployeeSettlementSalary> oEmployeeSettlementSalarys, List<EmployeeSettlementSalaryDetail> oEmployeeSettlementSalaryDetails, List<LeaveHead> oLeaveHeads, List<AttendanceDaily> oAttendanceDailys, Company oCompany, List<Employee> oEmployees, List<EmployeeLeaveOnAttendance> oELOnAttendances, List<SalarySheetSignature> oSalarySheetSignatures, int nGroupDept)
        {
            _oEmployeeSettlements = oEmployeeSettlements;
            _oEmployeeSettlementSalarys = oEmployeeSettlementSalarys;
            _oAttendanceDailys = oAttendanceDailys;
            _oEmployeeSettlementSalaryDetails = oEmployeeSettlementSalaryDetails;
            _oTEmployeeSettlementSalaryDetails = oEmployeeSettlementSalaryDetails;

            var ColDeductions = _oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == 3 && (x.SalaryHeadID == 8 || x.SalaryHeadID == 20 || x.SalaryHeadID == 25 || x.SalaryHeadID == 26))).ToList();
            //var allCols = _oEmployeeSettlementSalaryDetails.Where(x => (x.SalaryHeadType == 3)).ToList();

            _oEmployeeSettlementSalaryDetails = _oEmployeeSettlementSalaryDetails.Except(ColDeductions.Where(x => (x.SalaryHeadType == 3 && (x.SalaryHeadID == 8 || x.SalaryHeadID == 20 || x.SalaryHeadID == 25 || x.SalaryHeadID == 26))).ToList()).ToList();
            

            _oLeaveHeads = oLeaveHeads;
            _oEmployees = oEmployees;
            _oCompany = oCompany;
            _oELOnAttendances = oELOnAttendances;
            _nGroupDept = nGroupDept;
            DateTime sStartDate;
            DateTime sEndDate;
            if (_oEmployeeSettlements.Count > 0)
            {
                sStartDate = Convert.ToDateTime(_oEmployeeSettlements[0].ErrorMessage.Split(',')[0]);
                sEndDate = Convert.ToDateTime(_oEmployeeSettlements[0].ErrorMessage.Split(',')[1]);
                _sStartDate = sStartDate.ToString("dd MMM yyyy");
                _sEndDate = sEndDate.ToString("dd MMM yyyy");
            }


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
            _oPdfPCell = new PdfPCell(new Phrase("Employee Settlement Salary Sheet", _oFontStyle));
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
        private void PrintHaedRow(EmployeeSettlementSalary oES, int nGroupDept)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            if (nGroupDept == 1)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unit:" + oES.LocationName + ", " + "Department:" + oES.DepartmentName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unit:" + oES.LocationName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }

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
            if (_oEmployeeSettlementSalarys.Count <= 0)
            {
                this.PrintHeader();
                _oPdfPCell = new PdfPCell(new Phrase("Nothing to print !!", _oFontStyle)); _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
            }
            else
            {
                _nTotalRow = _oEmployeeSettlementSalarys.Count;
                if (_nGroupDept == 0)
                {
                    _oEmployeeSettlementSalarys = _oEmployeeSettlementSalarys.OrderBy(x => x.LocationName).ToList();
                    _oEmployeeSettlementSalarys.ForEach(x => _oSummaryEmployeeSettlementSalarys.Add(x));
                    while (_oEmployeeSettlementSalarys.Count > 0)
                    {
                        List<EmployeeSettlementSalary> oTempEmployeeSettlementSalarys = new List<EmployeeSettlementSalary>();
                        oTempEmployeeSettlementSalarys = _oEmployeeSettlementSalarys.Where(x => x.LocationName == _oEmployeeSettlementSalarys[0].LocationName).ToList();
                        string sLocationName = oTempEmployeeSettlementSalarys.Count > 0 ? oTempEmployeeSettlementSalarys[0].LocationName : "";
                        while (oTempEmployeeSettlementSalarys.Count > 0)
                        {
                            List<EmployeeSettlementSalary> oTempEmpSs = new List<EmployeeSettlementSalary>();
                            oTempEmpSs = oTempEmployeeSettlementSalarys.Where(x => x.LocationName == oTempEmployeeSettlementSalarys[0].LocationName).ToList();

                            this.PrintHeader();
                            PrintHaedRow(oTempEmpSs[0], _nGroupDept);

                            PrintSalarySheet(oTempEmpSs);
                            oTempEmployeeSettlementSalarys.RemoveAll(x => x.LocationName == oTempEmpSs[0].LocationName);
                        }
                        _oEmployeeSettlementSalarys.RemoveAll(x => x.LocationName == sLocationName);
                    }
                    this.Footer();
                }
                else
                {
                    _oEmployeeSettlementSalarys = _oEmployeeSettlementSalarys.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
                    _oEmployeeSettlementSalarys.ForEach(x => _oSummaryEmployeeSettlementSalarys.Add(x));
                    while (_oEmployeeSettlementSalarys.Count > 0)
                    {
                        List<EmployeeSettlementSalary> oTempEmployeeSettlementSalarys = new List<EmployeeSettlementSalary>();
                        oTempEmployeeSettlementSalarys = _oEmployeeSettlementSalarys.Where(x => x.LocationName == _oEmployeeSettlementSalarys[0].LocationName).ToList();
                        string sLocationName = oTempEmployeeSettlementSalarys.Count > 0 ? oTempEmployeeSettlementSalarys[0].LocationName : "";
                        while (oTempEmployeeSettlementSalarys.Count > 0)
                        {
                            List<EmployeeSettlementSalary> oTempEmpSs = new List<EmployeeSettlementSalary>();
                            oTempEmpSs = oTempEmployeeSettlementSalarys.Where(x => x.DepartmentName == oTempEmployeeSettlementSalarys[0].DepartmentName).ToList();
                            
                            this.PrintHeader();
                            PrintHaedRow(oTempEmpSs[0], _nGroupDept);

                            PrintSalarySheet(oTempEmpSs);
                            oTempEmployeeSettlementSalarys.RemoveAll(x => x.DepartmentName == oTempEmpSs[0].DepartmentName);
                        }
                        _oEmployeeSettlementSalarys.RemoveAll(x => x.LocationName == sLocationName);
                    }
                    this.Footer();
                }
            }
        }

        int nCount = 0;
        int nPageCount = 0;
        int nTotalCount = 0;
        int nTotalRowCount = 0;

        double nTotalGrossPerPage = 0;
        double nTotalGross = 0;

        double nTotalAllowancePerPage = 0;
        double nTotalAllowance = 0;

        double nTotalDeductionPerPage = 0;
        double nTotalDeduction = 0;

        double nTotalNetSalaryPerPage = 0;
        double nTotalNetSalary = 0;
        bool flag = true;

        public void PrintSalarySheet(List<EmployeeSettlementSalary> oEmployeeSalarys)
        {
            nCount = 0;
            nPageCount = 0;
            foreach (EmployeeSettlementSalary oEmpSalaryItem in oEmployeeSalarys)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                nCount++;
                nTotalRowCount++;
                nTotalCount++;
                _oPdfPCell = new PdfPCell(new Phrase((_nGroupDept==1)?nCount.ToString():nTotalRowCount.ToString(), _oFontStyle));
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
                        nAttTotal = oEmpSalaryItem.Present + oEmpSalaryItem.TotalAbsent + oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalHoliday + oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave;
                        _oPdfPCell = new PdfPCell(new Phrase("Present: " + oEmpSalaryItem.Present +"\nAbsent: " + oEmpSalaryItem.TotalAbsent + "\nOff day: " + oEmpSalaryItem.TotalDayOff + "\nLeave: " + (oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave) + "\nHoliday: " + oEmpSalaryItem.TotalHoliday + "\nTotal=" + nAttTotal, _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    
                        _oPdfPCell = new PdfPCell(new Phrase(GetLeaveStatus(oEmpSalaryItem.EmployeeID), _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(GetAdditionSalary(oEmpSalaryItem));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(GetDeductionSalary(oEmpSalaryItem));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
                        oEmpSalaryItem.NetAmount += (_bHasOTAllowance) ? 0 : - (oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour);

                        //double nESH = GetExceptDeductionSalary(oEmpSalaryItem.EmployeeSalaryID);

                        //oEmpSalaryItem.NetAmount = oEmpSalaryItem.NetAmount + nESH;
                        _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.NetAmount > 0 ? this.GetAmountInStr(oEmpSalaryItem.NetAmount, true, false) : "-", _oFontStyle));
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        nExceptSalaryHead = 0;

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
                        PrintHaedRow(oEmployeeSalarys[0],_nGroupDept);
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



            //_oDocument.Add(_oPdfPTable);
            //_oDocument.NewPage();
            //_oPdfPTable.DeleteBodyRows();
            //this.PrintHeader();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9; _oPdfPCell.FixedHeight = 35;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

           
            //_oPdfPCell = new PdfPCell(Summary()); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }

        #endregion
        
        public PdfPTable GetEmpOffical(Employee oEmp)
        {
            PdfPTable oPdfPTable1 = new PdfPTable(3);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 20f, 80f, 100f });

            oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Rowspan = 7; oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
            

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
            if (_nGroupDept == 0)
            {
                oPdfPCell1 = new PdfPCell(new Phrase("Department : ", _oFontStyle)); oPdfPCell1.Border = 0;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(oEmp.DepartmentName, _oFontStyle)); oPdfPCell1.Border = 0;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPTable1.CompleteRow();
            }
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

            foreach (EmployeeSettlementSalaryDetail oESDItem in _oEmployeeSettlementSalaryDetails)
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
        public PdfPTable GetAdditionSalary(EmployeeSettlementSalary oEmpSalaryItem)
        {
            double nLeaveAllowancePerEmployee = 0;

            PdfPTable oPdfPTable_Addition = new PdfPTable(2);
            PdfPCell oPdfPCell_Addition;

            oPdfPTable_Addition.SetWidths(new float[] { 70f, 50f });

            foreach (EmployeeSettlementSalaryDetail oESDItem in _oEmployeeSettlementSalaryDetails)
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

        public PdfPTable GetDeductionSalary(EmployeeSettlementSalary oEmpSalaryItem)
        {
            PdfPTable oPdfPTable_Deduction = new PdfPTable(2);
            PdfPCell oPdfPCell_Deduction;

            oPdfPTable_Deduction.SetWidths(new float[] { 70f, 50f });

            foreach (EmployeeSettlementSalaryDetail oESDItem in _oEmployeeSettlementSalaryDetails)
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
        private double GetExceptDeductionSalary(int EmployeeSalaryID)
        {
            foreach (EmployeeSettlementSalaryDetail oESDItem in _oTEmployeeSettlementSalaryDetails)
            {
                if (oESDItem.SalaryHeadType == 3 && oESDItem.EmployeeSalaryID == EmployeeSalaryID)
                {
                    if (oESDItem.SalaryHeadID == 8 || oESDItem.SalaryHeadID == 20 || oESDItem.SalaryHeadID == 25 || oESDItem.SalaryHeadID == 26)
                    {
                        nExceptSalaryHead += oESDItem.Amount;
                    }
                }
            }
            return nExceptSalaryHead;
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

    }

}

