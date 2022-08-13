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
   

    public class rptSalarySheetF02
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        int _nColumns = 0;
        int _nPageWidth = 2800;
        int _npageHeight = 1125; //1125 pixels x 1950 pixels
        PdfPTable _oPdfPTable;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        List<EmployeeSalaryV2> _oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
        List<EmployeeSalaryV2> _oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
        List<LeaveHead> _oLeaveHeads = new List<LeaveHead>();
        List<EmployeeSalaryDetailV2> _oEmployeeSalaryBasics = new List<EmployeeSalaryDetailV2>();
        List<EmployeeSalaryDetailV2> _oEmployeeSalaryAllowances = new List<EmployeeSalaryDetailV2>();
        List<EmployeeSalaryDetailV2> _oEmployeeSalaryDeductions = new List<EmployeeSalaryDetailV2>();
        SalaryField _oSalaryField = new SalaryField();
        EnumPageOrientation _ePageOrientation = EnumPageOrientation.None;
        string _sStartDate = "";
        string _sEndDate = "";
        bool _IsOT = false,_isDynamic=false;
        bool _bIsSerialWise = false;
        double _SubTOTAllowance = 0, _GrandTOTAllowance = 0;
        double _SubTLastGross = 0, _GrandTLastGross = 0, _SubTLastIncrement = 0, _GrandTLastIncrement = 0, _SubTPresentSalary = 0, _GrandTPresentSalary = 0, _SubTGrossEarnings = 0, _GrandTGrossEarnings = 0, _GrandTGrossDeduction = 0, _SubTGrossDeduction = 0, _GrandTNetAmount = 0, _SubTNetAmount = 0, _GrandTBankAmt = 0, _SubTBankAmt = 0, _GrandTCashAmt = 0, _SubTCashAmt = 0;
        double _TGrandTLastGross = 0, _TGrandTLastIncrement = 0, _TGrandTPresentSalary = 0, _TGrandTGrossEarnings = 0, _TGrandTGrossDeduction = 0, _TGrandTNetAmount = 0, _TGrandTBankAmt = 0, _TGrandTCashAmt = 0, _TGrandTOTAllowance=0;
        DateTime _StartDate = DateTime.Now;
        float _nHeight = 200f, _nFontSize = 0f, _nFontHeader = 0f, _nFooterFont = 7f, _nBUFont = 15f, _nBUlocafont = 8f, _nSalarySheetFont = 12f, _DateFont = 10f, _UnitDeptFont = 12f;
        int _nTotalColumns = 0;
        int _nCount = 0,_nTotalLeave=0;
        int _nGroupBySerial = 0;
        #endregion
        public byte[] PrepareReport(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<LeaveHead> oLeaveHeads, List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, List<EmployeeSalaryDetailV2> oEmployeeSalaryAllowances, List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions, Company oCompany, SalaryField oSalaryField, EnumPageOrientation ePageOrientation, List<SalarySheetSignature> oSalarySheetSignatures, string StartDate, string EndDate, bool IsOT)
        {
            _oCompany = oCompany;
            _oEmployeeSalaryV2s = oEmployeeSalaryV2s;
            _oLeaveHeads = oLeaveHeads;
            _oEmployeeSalaryBasics = oEmployeeSalaryBasics;
            _oEmployeeSalaryAllowances = oEmployeeSalaryAllowances;
            _oEmployeeSalaryDeductions = oEmployeeSalaryDeductions;
            _oSalaryField = oSalaryField;
            _ePageOrientation = ePageOrientation;
            _StartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            _bIsSerialWise = nGroupBySerial ==2 ? true : false;
            _IsOT = IsOT;
            _sStartDate = _StartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");
            _nGroupBySerial = nGroupBySerial;
            int nColumn = 0;

        
            _oSalaryField.FixedColumn = _oSalaryField.FixedColumn + 5;//SL,Gross Earnings,Gross Deductions,Net Amount,Signature

            if(oSalaryField.LeaveDetail)
            {
                _nTotalLeave = _oLeaveHeads.Count();
            }
            _nTotalColumns = _oEmployeeSalaryBasics.Count() + _oEmployeeSalaryAllowances.Count() + _oEmployeeSalaryDeductions.Count() + _nTotalLeave + _oSalaryField.FixedColumn;
            _nColumns = _nTotalColumns;
            float[] tablecolumns = new float[_nTotalColumns];
            tablecolumns[nColumn++] = 40f;


            for (var i = 0; i < _oSalaryField.TotalEmpInfoCol; i++)//Employee Informations
            {
                tablecolumns[nColumn++] = 90f;
            }
            for (var i = 0; i < _oSalaryField.TotalAttDetailColPrev; i++)
            {
                tablecolumns[nColumn++] = 50f;
            }

            for (var i = 0; i < _nTotalLeave; i++)//Leave Heads
            {
                tablecolumns[nColumn++] = 25f;
            }
            for (var i = 0; i < _oSalaryField.TotalAttDetailColPost; i++)
            {
                tablecolumns[nColumn++] = 55f;
            }
            for (var i = 0; i < _oSalaryField.TotalIncrementDetailCol + _oSalaryField.PresentSalaryCount; i++)
            {
                tablecolumns[nColumn++] = 65f;
            }
            int nBasic = _oEmployeeSalaryBasics.Count();
            for (var i = 0; i < nBasic; i++)//Basic
            {
                tablecolumns[nColumn++] = 70f;
            }
            int nAllowance = 0;
            if(_IsOT)
            {
                nAllowance = _oEmployeeSalaryAllowances.Count() + _oSalaryField.OTAllowanceCount;
            }
            else
            {
                nAllowance = _oEmployeeSalaryAllowances.Count();
            }
            
            for (var i = 0; i < nAllowance; i++)
            {
                tablecolumns[nColumn++] = 70f;
            }
            tablecolumns[nColumn++] = 75f;//Gross Earnings

            int nDeduction = _oEmployeeSalaryDeductions.Count();

            for (var i = 0; i < nDeduction; i++)
            {
                tablecolumns[nColumn++] = 70f;
            }
            tablecolumns[nColumn++] = 70f;///Gross Deduc
            tablecolumns[nColumn++] = 85f;//Net Amount
            int TCol = _oSalaryField.BankAmountCount + _oSalaryField.CashAmountCount + _oSalaryField.AccountNoCount + _oSalaryField.BankNameCount;
            for (var i = 0; i < TCol; i++)
            {
                tablecolumns[nColumn++] = 65f;
            }
            tablecolumns[nColumn++] = 150f;//Signature
            #region Page Setup
            _oPdfPTable = new PdfPTable(_nTotalColumns);
            if (_ePageOrientation == EnumPageOrientation.A4_Landscape)
            {
                _oDocument = new Document(PageSize.A4_LANDSCAPE, 0f, 0f, 0f, 0f);
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4_LANDSCAPE);
                _nHeight = 100f;
                _nFontSize = 6f;
                _nFontHeader = 6f;
            }
            else if (_ePageOrientation == EnumPageOrientation.Legal_LandScape)
            {
                _oDocument = new Document(PageSize.LEGAL_LANDSCAPE, 0f, 0f, 0f, 0f);
                _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL_LANDSCAPE);
                _nHeight = 100f;
                _nFontSize = 6f;
                _nFontHeader = 6f;
            }
            else if (_ePageOrientation == EnumPageOrientation.Legal_Portrait)
            {
                _oDocument = new Document(PageSize.LEGAL_LANDSCAPE, 0f, 0f, 0f, 0f);
                _oDocument.SetPageSize(iTextSharp.text.PageSize.LEGAL);
                _nHeight = 100f;
                _nFontSize = 6f;
                _nFontHeader = 6f;
            }
            else if (_ePageOrientation == EnumPageOrientation.Letter_LandScape)
            {
                _oDocument = new Document(PageSize.LETTER_LANDSCAPE, 0f, 0f, 0f, 0f);
                _oDocument.SetPageSize(iTextSharp.text.PageSize.LETTER_LANDSCAPE);
                _nHeight = 100f;
                _nFontSize = 6f;
                _nFontHeader = 6f;
            }
            else if (_ePageOrientation == EnumPageOrientation.Dynamic)
            {
                _npageHeight = 1600;
                _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
                _nFontSize = 20f;
                _nFontHeader = 18f;
                _nFooterFont = 18f;
                _nBUFont = 40f;
                _nBUlocafont = 20f;
                _nSalarySheetFont = 35f;
                _DateFont = 25f;
                _UnitDeptFont = 25f;
                _nHeight = 180f;
                _isDynamic = true;

            }

            //  _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);

            _oDocument.SetMargins(25f, 25f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.nFontSize = (int)_nFooterFont;
            oPDFWriter.PageEvent = PageEventHandler;
            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);

            #endregion
            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Header

        private void PrintHeader(string BUName,bool IsSummary)
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 200f, 230f });
            PdfPCell oPdfPCellHearder;


            _oFontStyle = FontFactory.GetFont("Tahoma", _nBUFont, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(BUName, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);


            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", _nBUlocafont, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            string sTitle = "Salary Sheet";
            if(IsSummary==true)
            {
                sTitle = "Salary Summary";
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", _nSalarySheetFont, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sTitle, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", _DateFont, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body
        int nAlredayPrintGrandTotal = 0;
        private void PrintBody()
        {


            _oEmployeeSalaryV2s.ForEach(x => _oTempEmployeeSalaryV2s.Add(x));
            string sPrevBUName = "";

            while (_oEmployeeSalaryV2s.Count > 0)
            {
                var oResults = new List<EmployeeSalaryV2>();
                if (_bIsSerialWise)
                {
                    oResults = _oEmployeeSalaryV2s.Where(x => x.BUID == _oEmployeeSalaryV2s[0].BUID).OrderBy(x=>x.LocationName).ThenBy(x=>x.DepartmentName).ThenBy(x=>x.EmployeeCode).ToList();
                }
                else
                {
                    oResults = _oEmployeeSalaryV2s.Where(x => x.BUID == _oEmployeeSalaryV2s[0].BUID && x.LocationID == _oEmployeeSalaryV2s[0].LocationID && x.DepartmentID == _oEmployeeSalaryV2s[0].DepartmentID).OrderBy(x=>x.LocationName).ThenBy(x=>x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                }

                PrintHeader(oResults[0].BUName,false);
                _oFontStyle = FontFactory.GetFont("Tahoma", _UnitDeptFont, iTextSharp.text.Font.BOLD);
                int nSpan = 0;
                if (_bIsSerialWise)
                {
                    nSpan = _nTotalColumns;
                }
                else
                {
                    nSpan = 8;

                }
                _oPdfPCell = new PdfPCell(new Phrase("Unit : " + oResults[0].BUShortName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = nSpan; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                if (!_bIsSerialWise)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Department : " + oResults.FirstOrDefault().DepartmentName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = _nTotalColumns - 8; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                }
                else
                {
                    if (_bIsSerialWise)
                    {
                        _oPdfPTable.CompleteRow();
                        sPrevBUName = oResults[0].BUShortName;
                    }
                }



                this.ColumnSetup();

                this.SalarySheet(oResults);
                if (_bIsSerialWise)
                {
                    _oEmployeeSalaryV2s.RemoveAll(x => x.BUID == oResults[0].BUID);
                }
                else
                {
                    _oEmployeeSalaryV2s.RemoveAll(x => x.BUID == oResults[0].BUID && x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                }

                if (((_oEmployeeSalaryV2s.Count <= 0) || (_bIsSerialWise == true && _oEmployeeSalaryV2s[0].BUShortName != sPrevBUName)) || _bIsSerialWise == false)
                {
                    if (nAlredayPrintGrandTotal == 0 && _nGroupBySerial!=2)
                    {
                       
                        this.GrandTotal();
                        ResetGrandTotal();
                    }

                   // _nCount = 0;
                    nAlredayPrintGrandTotal = 0;
                }
                if((_oEmployeeSalaryV2s.Count <= 0))
                {
                    this.FinalGrandTotal();
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                }
                else
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                }

            }
            if(_IsOT==true)
            {
                this.Summary();
            }
            else
            {
                this.SummaryWithoutOT();
            }
            
        }

        private void SalarySheet(List<EmployeeSalaryV2> oEmpSalarys)
        {
            if (!_bIsSerialWise&&_nGroupBySerial!=3)
            {
                _nCount = 0;
            }
            int nCount = 0;

            int nTotalRowPage = 0;

            foreach (EmployeeSalaryV2 oEmpSalaryItem in oEmpSalarys)
            {
                nTotalRowPage++;
                _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
                _nCount++;
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                if (_oSalaryField.EmployeeCode)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.EmployeeCode, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.NORMAL);
                if (_oSalaryField.EmployeeName)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.EmployeeName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.Designation)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.DesignationName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.Grade)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.Grade, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.JoiningDate)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.JoiningDate.ToString("dd MMM yyyy"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                if (_oSalaryField.EmployeeType)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.EmployeeTypeName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                if (_oSalaryField.PaymentType)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.PaymentType, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                int days = DateTime.DaysInMonth(_StartDate.Year, _StartDate.Month);
                if(oEmpSalaryItem.JoiningDate>_StartDate)
                {
                    days = days - Convert.ToInt32(oEmpSalaryItem.JoiningDate.ToString("dd"));
                }
                if (_oSalaryField.TotalDays)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(days.ToString(), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                int NTotalLeave = oEmpSalaryItem.EmployeeWiseLeaveStatus.Sum(x => x.LeaveDays);
                int nTPresentDays = days - (oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalHoliday + NTotalLeave + oEmpSalaryItem.TotalAbsent);
                if (_oSalaryField.PresentDay)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nTPresentDays.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                int nDayOffHoliday = oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalHoliday;
                if (_oSalaryField.Day_off_Holidays)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nDayOffHoliday.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.AbsentDays)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.TotalAbsent.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }


                int nTotalLeave = 0;
                int nLeave = 0, nLWP = 0;
             
                    foreach (LeaveHead oEmpLeave in _oLeaveHeads)
                    {
                        nLeave = oEmpSalaryItem.EmployeeWiseLeaveStatus.Where(x => x.LeaveHeadID == oEmpLeave.LeaveHeadID).Select(x => x.LeaveDays).FirstOrDefault();
                        if (oEmpLeave.IsLWP)
                        {
                            nLWP += nLeave;
                        }
                        if (_oSalaryField.LeaveDetail)
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(nLeave.ToString(), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        }
                        nTotalLeave += nLeave;
                    }
                
               

                if (_oSalaryField.LeaveDays)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nTotalLeave.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                int nTWorkingDay = nTPresentDays + nDayOffHoliday + nTotalLeave - nLWP;
                if (_oSalaryField.Employee_Working_Days)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nTWorkingDay.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.Early_Out_Days)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.TotalEarlyLeaving.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.Early_Out_Mins)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.EarlyOutInMin.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;//Early Out Mins
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.LateDays)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.TotalLate.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.LateHrs)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MinInHourMin(oEmpSalaryItem.LateInMin), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
               

                if (_oSalaryField.LastGross)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.LastGross, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }


                _SubTLastGross += oEmpSalaryItem.LastGross;
                if (_oSalaryField.LastIncrement)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.LastIncrement, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;//Last Increment
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                _SubTLastIncrement += oEmpSalaryItem.LastIncrement;
                if (_oSalaryField.Increment_Effect_Date)
                {
                    
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.IncrementEffectDate!=DateTime.MinValue?oEmpSalaryItem.IncrementEffectDate.ToString("dd MMM yyyy"):"", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;//Increment Effect Date
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.PresentSalary)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.PresentSalary, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                _SubTPresentSalary += oEmpSalaryItem.PresentSalary;

                foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
                {
                    double nBasic = oEmpSalaryItem.EmployeeSalaryBasics.Where(x => x.SalaryHeadID == oEmpBasics.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                    oEmpBasics.SubTotalAmount = oEmpBasics.SubTotalAmount + nBasic;
                    oEmpBasics.GrandTotalAmount = oEmpBasics.GrandTotalAmount + nBasic;
                    oEmpBasics.TGrandTotalAmount = oEmpBasics.TGrandTotalAmount + nBasic;
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nBasic, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                foreach (EmployeeSalaryDetailV2 oEmpAllowances in _oEmployeeSalaryAllowances)
                {
                    double nAllowance = oEmpSalaryItem.EmployeeSalaryAllowances.Where(x => x.SalaryHeadID == oEmpAllowances.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                    oEmpAllowances.SubTotalAmount = oEmpAllowances.SubTotalAmount + nAllowance;
                    oEmpAllowances.GrandTotalAmount = oEmpAllowances.GrandTotalAmount + nAllowance;
                    oEmpAllowances.TGrandTotalAmount = oEmpAllowances.TGrandTotalAmount + nAllowance;
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nAllowance, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                double TotalOT = 0;
                if(_IsOT)
                {
                     TotalOT = oEmpSalaryItem.OTHour * oEmpSalaryItem.OTRatePerHour;
                }
                double TotalEarnings = oEmpSalaryItem.EmployeeSalaryAllowances.Sum(x => x.Amount) + oEmpSalaryItem.EmployeeSalaryBasics.Sum(x => x.Amount) + TotalOT;
                _SubTGrossEarnings += TotalEarnings;
                double nTOT = 0;
                if (_IsOT)
                {
                    
                    nTOT = TotalOT;

                    if (_oSalaryField.DefineOTHour)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.DefineOTHour.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    }
                    if (_oSalaryField.ExtraOTHour)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.ExtraOTHour.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    }

                    if (_oSalaryField.OTHours)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.OTHour.ToString("#,##0"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    }

                    if (_oSalaryField.OTRate)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.OTRatePerHour.ToString("#,##0.00"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    }
                    if (_oSalaryField.OTAllowance)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalOT, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    }
                    _SubTOTAllowance += TotalOT;
                    oEmpSalaryItem.GrossAmount += TotalOT;
                }
               

                //oEmpSalaryItem.GrossAmount
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalEarnings, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                foreach (EmployeeSalaryDetailV2 oEmpDeductions in _oEmployeeSalaryDeductions)
                {
                    double nDeduction = oEmpSalaryItem.EmployeeSalaryDeductions.Where(x => x.SalaryHeadID == oEmpDeductions.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                    oEmpDeductions.SubTotalAmount = oEmpDeductions.SubTotalAmount + nDeduction;
                    oEmpDeductions.GrandTotalAmount = oEmpDeductions.GrandTotalAmount + nDeduction;
                    oEmpDeductions.TGrandTotalAmount = oEmpDeductions.TGrandTotalAmount + nDeduction;
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nDeduction, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                double TotalDeduction = oEmpSalaryItem.EmployeeSalaryDeductions.Sum(x => x.Amount);
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalDeduction, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _SubTGrossDeduction += TotalDeduction;

   
                _oFontStyle = FontFactory.GetFont("Tahoma", 22, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.NetAmount + nTOT, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _SubTNetAmount += oEmpSalaryItem.NetAmount + nTOT;
                _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.NORMAL);
                if (_oSalaryField.BankAmount)
                {
                    if(oEmpSalaryItem.BankAmount>0)
                    {
                        oEmpSalaryItem.BankAmount += nTOT;
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.BankAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _SubTBankAmt += oEmpSalaryItem.BankAmount;
                }

                if (_oSalaryField.CashAmount)
                {
                    if(oEmpSalaryItem.CashAmount>0)
                    {
                        oEmpSalaryItem.CashAmount += nTOT;
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.CashAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _SubTCashAmt += oEmpSalaryItem.CashAmount;
                }

                if (_oSalaryField.AccountNo)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.AccountNo, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.BankName)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.BankName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                _oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;//Signature
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                if (oEmpSalarys.Count == nCount && nTotalRowPage % 6 != 0)
                {
                    this.SubTotal();
                    nAlredayPrintGrandTotal = 1;
                    if(_nGroupBySerial!=2)
                    {
                        this.GrandTotal();
                        this.ResetGrandTotal();
                    }
                   
                    _SubTBankAmt = 0;
                    _SubTCashAmt = 0;
                    _SubTNetAmount = 0;
                    _SubTGrossDeduction = 0;
                    _SubTLastGross = 0;
                    _SubTLastIncrement = 0;
                    _SubTPresentSalary = 0;
                    _SubTOTAllowance = 0;
                    _SubTGrossEarnings = 0;
                }
                else if (oEmpSalarys.Count == nCount)
                {
                    this.SubTotal();
                    nAlredayPrintGrandTotal = 1;
                    if(_nGroupBySerial!=2)
                    {
                        this.GrandTotal();
                        this.ResetGrandTotal();
                    }
                  
                    _SubTBankAmt = 0;
                    _SubTCashAmt = 0;
                    _SubTNetAmount = 0;
                    _SubTGrossDeduction = 0;
                    _SubTLastGross = 0;
                    _SubTLastIncrement = 0;
                    _SubTPresentSalary = 0;
                    _SubTOTAllowance = 0;
                    _SubTGrossEarnings = 0;
                }
               
                else if (nTotalRowPage % 6 == 0)
                {
                    this.SubTotal();
                    _SubTBankAmt = 0;
                    _SubTCashAmt = 0;
                    _SubTNetAmount = 0;
                    _SubTGrossDeduction = 0;
                    _SubTLastGross = 0;
                    _SubTLastIncrement = 0;
                    _SubTPresentSalary = 0;
                    _SubTOTAllowance = 0;
                    _SubTGrossEarnings = 0;
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    this.ResetSubTotal();
                    
                    PrintHeader(oEmpSalaryItem.BUName,false);
                    _oFontStyle = FontFactory.GetFont("Tahoma", _UnitDeptFont, iTextSharp.text.Font.BOLD);

                    int nSpan = 0;
                    if (_bIsSerialWise)
                    {
                        nSpan = _nColumns;
                    }
                    else
                    {
                        nSpan = 8;
                    }
                    _oPdfPCell = new PdfPCell(new Phrase("Unit : " + oEmpSalaryItem.BUShortName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = nSpan; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    if (!_bIsSerialWise)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Department : " + oEmpSalaryItem.DepartmentName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = _nColumns - 8; _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    }

                    _oPdfPTable.CompleteRow();
                    this.ColumnSetup();
                }

            }


            //_oDocument.Add(_oPdfPTable);
            //_oDocument.NewPage();
            //_oPdfPTable.DeleteBodyRows();

        }

        private void FinalGrandTotal()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("GrandTotal", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.Colspan = _oSalaryField.TotalEmpInfoCol + _oSalaryField.TotalAttDetailColPrev + _oSalaryField.TotalAttDetailColPost + _nTotalLeave + 1;//1 For SL
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            if (_oSalaryField.LastGross)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_TGrandTLastGross, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.LastIncrement)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_TGrandTLastIncrement, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.Increment_Effect_Date)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.PresentSalary)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_TGrandTPresentSalary, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpBasics.TGrandTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            foreach (EmployeeSalaryDetailV2 oEmpAllowances in _oEmployeeSalaryAllowances)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpAllowances.TGrandTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if (_IsOT)
            {
                if (_oSalaryField.DefineOTHour)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.ExtraOTHour)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.OTHours)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.OTRate)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.OTAllowance)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_TGrandTOTAllowance, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
            }


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_TGrandTGrossEarnings, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            foreach (EmployeeSalaryDetailV2 oEmpDeductions in _oEmployeeSalaryDeductions)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpDeductions.TGrandTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_TGrandTGrossDeduction, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_TGrandTNetAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            if (_oSalaryField.BankAmount)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_TGrandTBankAmt, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.CashAmount)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_TGrandTCashAmt, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.AccountNo)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.BankName)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void GrandTotal()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Department Wise GrandTotal", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.Colspan = _oSalaryField.TotalEmpInfoCol + _oSalaryField.TotalAttDetailColPrev + _oSalaryField.TotalAttDetailColPost + _nTotalLeave + 1;//1 For SL
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            if (_oSalaryField.LastGross)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTLastGross, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.LastIncrement)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTLastIncrement, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.Increment_Effect_Date)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.PresentSalary)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTPresentSalary, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpBasics.GrandTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            foreach (EmployeeSalaryDetailV2 oEmpAllowances in _oEmployeeSalaryAllowances)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpAllowances.GrandTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if(_IsOT)
            {
                if (_oSalaryField.DefineOTHour)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.ExtraOTHour)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.OTHours)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.OTRate)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.OTAllowance)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTOTAllowance, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
            }
          

            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTGrossEarnings, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            foreach (EmployeeSalaryDetailV2 oEmpDeductions in _oEmployeeSalaryDeductions)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpDeductions.GrandTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTGrossDeduction, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTNetAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            if (_oSalaryField.BankAmount)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTBankAmt, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.CashAmount)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTCashAmt, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.AccountNo)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.BankName)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _GrandTNetAmount = 0;
            _GrandTBankAmt = 0;
            _GrandTCashAmt = 0;
            _GrandTGrossDeduction = 0;
            _GrandTLastGross = 0;
            _GrandTLastIncrement = 0;
            _GrandTPresentSalary = 0;
            _GrandTOTAllowance = 0;
            _GrandTGrossEarnings = 0;
            _oPdfPTable.CompleteRow();
        }
        private void ResetSubTotal()
        {
            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
            {
                oEmpBasics.SubTotalAmount = 0;
            }
            foreach (EmployeeSalaryDetailV2 oEmpAllowances in _oEmployeeSalaryAllowances)
            {
                oEmpAllowances.SubTotalAmount = 0;
            }
            foreach (EmployeeSalaryDetailV2 oEmpDeductions in _oEmployeeSalaryDeductions)
            {
                oEmpDeductions.SubTotalAmount = 0;
            }
        }
        private void ResetGrandTotal()
        {
            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
            {
                oEmpBasics.GrandTotalAmount = 0;
            }
            foreach (EmployeeSalaryDetailV2 oEmpAllowances in _oEmployeeSalaryAllowances)
            {
                oEmpAllowances.GrandTotalAmount = 0;
            }
            foreach (EmployeeSalaryDetailV2 oEmpDeductions in _oEmployeeSalaryDeductions)
            {
                oEmpDeductions.GrandTotalAmount = 0;
            }
        }
        private void SubTotal()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.Colspan = _oSalaryField.TotalEmpInfoCol + _oSalaryField.TotalAttDetailColPrev + _oSalaryField.TotalAttDetailColPost + _nTotalLeave + 1;//1 For SL
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            if (_oSalaryField.LastGross)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTLastGross, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _GrandTLastGross += _SubTLastGross;
                _TGrandTLastGross += _SubTLastGross;
            }

            if (_oSalaryField.LastIncrement)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTLastIncrement, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _GrandTLastIncrement += _SubTLastIncrement;
                _TGrandTLastIncrement += _SubTLastIncrement;
            }

            if (_oSalaryField.Increment_Effect_Date)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.PresentSalary)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTPresentSalary, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _GrandTPresentSalary += _SubTPresentSalary;
                _TGrandTPresentSalary += _SubTPresentSalary;
            }

            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpBasics.SubTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            foreach (EmployeeSalaryDetailV2 oEmpAllowances in _oEmployeeSalaryAllowances)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpAllowances.SubTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if(_IsOT)
            {
                if (_oSalaryField.DefineOTHour)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.ExtraOTHour)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.OTHours)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }

                if (_oSalaryField.OTRate)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; 
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.OTAllowance)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTOTAllowance, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _GrandTOTAllowance += _SubTOTAllowance;
                    _TGrandTOTAllowance += _SubTOTAllowance;
                }

            }
          

            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTGrossEarnings, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _GrandTGrossEarnings += _SubTGrossEarnings;
            _TGrandTGrossEarnings += _SubTGrossEarnings;
            foreach (EmployeeSalaryDetailV2 oEmpDeductions in _oEmployeeSalaryDeductions)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpDeductions.SubTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTGrossDeduction, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _GrandTGrossDeduction += _SubTGrossDeduction;
            _TGrandTGrossDeduction += _SubTGrossDeduction;

            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTNetAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _GrandTNetAmount += _SubTNetAmount;
            _TGrandTNetAmount += _SubTNetAmount;

            if (_oSalaryField.BankAmount)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTBankAmt, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _GrandTBankAmt += _SubTBankAmt;
                _TGrandTBankAmt += _SubTBankAmt;
            }


            if (_oSalaryField.CashAmount)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTCashAmt, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _GrandTCashAmt += _SubTCashAmt;
                _TGrandTCashAmt += _SubTCashAmt;
            }

            if (_oSalaryField.AccountNo)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (_oSalaryField.BankName)
            {
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        private void ColumnSetup()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", _nFontHeader, iTextSharp.text.Font.BOLD);
            int nSpan = 2;
            if(_oSalaryField.LeaveDetail)
            {
                nSpan = 3;
            }
           

            _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            if (_oSalaryField.TotalEmpInfoCol > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Employee Information", _oFontStyle)); _oPdfPCell.Colspan = _oSalaryField.TotalEmpInfoCol; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if (_oSalaryField.TotalAttDetailColPrev + _oSalaryField.TotalAttDetailColPost + _nTotalLeave>0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Att. Detail", _oFontStyle)); _oPdfPCell.Colspan = _oSalaryField.TotalAttDetailColPrev + _oSalaryField.TotalAttDetailColPost + _nTotalLeave; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }

          

            if (_oSalaryField.TotalIncrementDetailCol > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Increment Detail", _oFontStyle)); _oPdfPCell.Colspan = _oSalaryField.TotalIncrementDetailCol; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.PresentSalary)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Present Salary", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oEmployeeSalaryBasics.Count > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Basic", _oFontStyle)); _oPdfPCell.Colspan = _oEmployeeSalaryBasics.Count(); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
           
            if(_IsOT)
            {
                if (_oEmployeeSalaryAllowances.Count() + _oSalaryField.OTAllowanceCount>0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Allowance", _oFontStyle)); _oPdfPCell.Colspan = _oEmployeeSalaryAllowances.Count() + _oSalaryField.OTAllowanceCount; _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                }
                
            }
            else
            {
                if (_oEmployeeSalaryAllowances.Count()>0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Allowance", _oFontStyle)); _oPdfPCell.Colspan = _oEmployeeSalaryAllowances.Count(); _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                }
              
            }
           
            _oPdfPCell = new PdfPCell(new Phrase("Gross Earnings", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            if (_oEmployeeSalaryDeductions.Count()>0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Deduction", _oFontStyle)); _oPdfPCell.Colspan = _oEmployeeSalaryDeductions.Count(); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
           
            _oPdfPCell = new PdfPCell(new Phrase("Gross Deductions", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Net Amount", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            if (_oSalaryField.BankAmount)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Bank", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if (_oSalaryField.CashAmount)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Cash", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.AccountNo)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Account No", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if (_oSalaryField.BankName)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Bank Name", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            if (_oSalaryField.EmployeeCode)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Employee Code", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (_oSalaryField.EmployeeName)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Employee Name", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (_oSalaryField.Designation)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if (_oSalaryField.Grade)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Grade", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if (_oSalaryField.JoiningDate)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Joining Date", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.EmployeeType)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Employee Type", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.PaymentType)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Payment Type", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if (_oSalaryField.TotalDays)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Total Days", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.PresentDay)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Present Day", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.Day_off_Holidays)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Day Off Hoildays", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.AbsentDays)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Absent Days", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if(_oSalaryField.LeaveDetail)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Leave Head", _oFontStyle)); _oPdfPCell.Colspan = _oLeaveHeads.Count(); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
           
            if (_oSalaryField.LeaveDays)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Leave Day", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.Employee_Working_Days)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Employee Working Day", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.Early_Out_Days)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Early Out Day", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.Early_Out_Mins)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Early Out Mins", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.LateDays)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Late Days", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            if (_oSalaryField.LateHrs)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Late Hrs", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

          

            if (_oSalaryField.LastGross)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Last Gross", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if (_oSalaryField.LastIncrement)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Last Increment", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if (_oSalaryField.Increment_Effect_Date)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Increment Effect Day", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }


            foreach (EmployeeSalaryDetailV2 oItem in _oEmployeeSalaryBasics)
            {

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }

            foreach (EmployeeSalaryDetailV2 oItem in _oEmployeeSalaryAllowances)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            if(_IsOT)
            {
                string sOTHour = "OT Hour";
                if (_oSalaryField.DefineOTHour)
                {
                    sOTHour = "Total OT Hour";
                    _oPdfPCell = new PdfPCell(new Phrase("OT Hour", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.ExtraOTHour)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Extra OT Hour", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.OTHours)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(sOTHour, _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.OTRate)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("OT Rate", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                }
                if (_oSalaryField.OTAllowance)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("O T Allowance", _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                }
            }
           


            foreach (EmployeeSalaryDetailV2 oItem in _oEmployeeSalaryDeductions)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle)); _oPdfPCell.Rowspan = nSpan - 1; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPTable.CompleteRow();

            if(_oSalaryField.LeaveDetail)
            {
                foreach (LeaveHead oItem in _oLeaveHeads)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.ShortName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

                }
                _oPdfPTable.CompleteRow();
            }
          
        }


        public void Summary()
        {
            float FixedHeight = 0f;
            iTextSharp.text.Font _oFontStyle;
            if (_isDynamic)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma",25, iTextSharp.text.Font.BOLD);
                FixedHeight = 30f;
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8, iTextSharp.text.Font.BOLD);
            }
            
            PdfPTable oPdfPTable1 = new PdfPTable(16);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 18f,65f, 45f, 50f, 43f, 55f, 60f, 60f, 60f, 80f, 60f, 60f, 50f, 40f,40f,10f });

            double nGTotalEmpCount = 0, nGTotalOTHour = 0, nGTotalTwoHourOT = 0, nGTotalExtraOTHour = 0, nGTotalGrossAmount = 0, nGTotalTotalOTAmt = 0, nGTotalTwoHourOTAmt = 0, nGTotalExtraOTAmt = 0, nGTotalAdditionAmt = 0, nGTotalDeductionAmnt = 0, nGTotalNetAmnt = 0, nGTotalBankAmnt = 0, nGTotalCashAmnt = 0;
            _oTempEmployeeSalaryV2s = _oTempEmployeeSalaryV2s.OrderBy(x => x.BUName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
            while (_oTempEmployeeSalaryV2s.Count > 0)
            {

                string BUName = _oTempEmployeeSalaryV2s[0].BUName;
                this.PrintHeader(BUName,true);
                List<EmployeeSalaryV2> oEmployeeSalaryV2s = _oTempEmployeeSalaryV2s.Where(x => x.BUName == BUName).ToList();



                var oDBMonthlyAttendanceReports = oEmployeeSalaryV2s.GroupBy(x => new { x.DepartmentName }, (key, grp) => new
                {
                    DepartmentName = key.DepartmentName,
                    EmpCount = grp.ToList().Count(),
                    GrossAmount = grp.ToList().Sum(y => y.GrossAmount),
                    TwoHourOT = grp.ToList().Sum(y => y.DefineOTHour),
                    OTHour = grp.ToList().Sum(y => y.OTHour),
                    ExtraOtHour = grp.ToList().Sum(y => y.ExtraOTHour),
                    TotalOT = grp.ToList().Sum(y => y.OTHour * y.OTRatePerHour),
                    TotalExtraOT = grp.ToList().Sum(y => y.ExtraOTHour * y.OTRatePerHour),
                    TotalTwoHourOT = grp.ToList().Sum(y => y.DefineOTHour * y.OTRatePerHour),
                    NetAmount = grp.ToList().Sum(y => y.NetAmount),
                    BankAmount = grp.ToList().Sum(y => y.BankAmount),
                    CashAmount = grp.ToList().Sum(y => y.CashAmount),
                    Results = grp.ToList(),
                    AdditionAmount = grp.ToList().Sum(x => x.EmployeeSalaryAllowances.Sum(y => y.Amount)),
                    DeductionAmount = grp.ToList().Sum(x => x.EmployeeSalaryDeductions.Sum(y => y.Amount))
                });

                int nCount = 0;
                oPdfPTable1 = new PdfPTable(16);
                oPdfPTable1.SetWidths(new float[] { 18f, 65f, 45f, 50f, 43f, 55f, 60f, 60f, 60f, 80f, 60f, 60f, 50f, 40f, 40f, 10f });


               
                oPdfPCell1 = new PdfPCell(new Phrase("Unit : " + BUName, _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = 16;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPTable1.CompleteRow();

                oPdfPCell1 = new PdfPCell(new Phrase("SL#", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("Department", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("ManPower", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Gross", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("Addition", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Deduction", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("OT Two Hour", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Extra OT Hour", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Total OT Hour", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Two Hour OT Amount", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight; oPdfPCell1.VerticalAlignment = Element.ALIGN_CENTER;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Extra OT Amount", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("Total OT Amount", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("Net", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Bank", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Cash", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 15; oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPTable1.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                double nTotalEmpCount = 0, nTotalOTHour = 0, nTotalTwoHourOT = 0, nTotalExtraOTHour = 0, nTotalGrossAmount = 0, nTotalTotalOTAmt = 0, nTotalTwoHourOTAmt = 0, nTotalExtraOTAmt = 0, nTotalAdditionAmt = 0, nTotalDeductionAmnt = 0, nTotalNetAmnt = 0, nTotalBankAmnt=0, nTotalCashAmnt=0;
                foreach (var oBUEmployeeSalary in oDBMonthlyAttendanceReports)
                {
                    oPdfPTable1 = new PdfPTable(16);
                    oPdfPTable1.SetWidths(new float[] { 18f, 65f, 45f, 50f, 43f, 55f, 60f, 60f, 60f, 80f, 60f, 60f, 50f, 40f, 40f, 10f });
                    nCount++;

                    oPdfPCell1 = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.DepartmentName, _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.EmpCount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalEmpCount += oBUEmployeeSalary.EmpCount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.GrossAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalGrossAmount += oBUEmployeeSalary.GrossAmount;


                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.AdditionAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalAdditionAmt += oBUEmployeeSalary.AdditionAmount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.DeductionAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalDeductionAmnt += oBUEmployeeSalary.DeductionAmount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.TwoHourOT.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalTwoHourOT += oBUEmployeeSalary.TwoHourOT;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.ExtraOtHour.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalExtraOTHour += oBUEmployeeSalary.ExtraOtHour;


                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.OTHour.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalOTHour += oBUEmployeeSalary.OTHour;


                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.TotalTwoHourOT.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalTwoHourOTAmt += oBUEmployeeSalary.TotalTwoHourOT;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.TotalExtraOT.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalExtraOTAmt += oBUEmployeeSalary.TotalExtraOT;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.TotalOT.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalTotalOTAmt += oBUEmployeeSalary.TotalOT;


                    oPdfPCell1 = new PdfPCell(new Phrase((oBUEmployeeSalary.NetAmount+oBUEmployeeSalary.TotalOT).ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalNetAmnt += oBUEmployeeSalary.NetAmount+oBUEmployeeSalary.TotalOT;

                    oPdfPCell1 = new PdfPCell(new Phrase((oBUEmployeeSalary.BankAmount + oBUEmployeeSalary.TotalOT).ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalBankAmnt += oBUEmployeeSalary.BankAmount + oBUEmployeeSalary.TotalOT;

                    oPdfPCell1 = new PdfPCell(new Phrase((oBUEmployeeSalary.CashAmount + oBUEmployeeSalary.TotalOT).ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalCashAmnt += oBUEmployeeSalary.CashAmount + oBUEmployeeSalary.TotalOT;

                    oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 15; oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPTable1.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns; oPdfPCell1.FixedHeight = FixedHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                #region SubTotal
                oPdfPTable1 = new PdfPTable(16);
                oPdfPTable1.SetWidths(new float[] { 18f, 65f, 45f, 50f, 43f, 55f, 60f, 60f, 60f, 80f, 60f, 60f, 50f, 40f, 40f, 10f });

                oPdfPCell1 = new PdfPCell(new Phrase("SubTotal:", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight; oPdfPCell1.Colspan = 2;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalEmpCount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalEmpCount += nTotalEmpCount;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalGrossAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalGrossAmount += nTotalGrossAmount;


                oPdfPCell1 = new PdfPCell(new Phrase(nTotalAdditionAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalAdditionAmt += nTotalAdditionAmt;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalDeductionAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalDeductionAmnt += nTotalDeductionAmnt;


                oPdfPCell1 = new PdfPCell(new Phrase(nTotalTwoHourOT.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalTwoHourOT += nTotalTwoHourOT;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalExtraOTHour.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalExtraOTHour += nTotalExtraOTHour;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalOTHour.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalOTHour += nTotalOTHour;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalTwoHourOTAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalTwoHourOTAmt += nTotalTwoHourOTAmt;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalExtraOTAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalExtraOTAmt += nTotalExtraOTAmt;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalTotalOTAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalTotalOTAmt += nTotalTotalOTAmt;


                oPdfPCell1 = new PdfPCell(new Phrase(nTotalNetAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalNetAmnt += nTotalNetAmnt;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalBankAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalBankAmnt += nTotalBankAmnt;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalCashAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalCashAmnt += nTotalCashAmnt;

               

                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 15;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPTable1.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns; oPdfPCell1.FixedHeight = FixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                #endregion
                _oTempEmployeeSalaryV2s.RemoveAll(x => x.BUName == BUName);


            }
            #region GrandTotal
            oPdfPTable1 = new PdfPTable(16);
            oPdfPTable1.SetWidths(new float[] { 18f, 65f, 45f, 50f, 43f, 55f, 60f, 60f, 60f, 80f, 60f, 60f, 50f, 40f, 40f, 10f });
            oPdfPCell1 = new PdfPCell(new Phrase("Grand Total:", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight; oPdfPCell1.Colspan = 2;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalEmpCount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalGrossAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalAdditionAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalDeductionAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalTwoHourOT.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalExtraOTHour.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalOTHour.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalTwoHourOTAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalExtraOTAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalTotalOTAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalNetAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalBankAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalCashAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 15; oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPTable1.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            #endregion

        }

        public void SummaryWithoutOT()
        {
            float FixedHeight = 0f;
            iTextSharp.text.Font _oFontStyle;
            if (_isDynamic)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 25, iTextSharp.text.Font.BOLD);
                FixedHeight = 30f;
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8, iTextSharp.text.Font.BOLD);
            }

            PdfPTable oPdfPTable1 = new PdfPTable(10);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 18f, 65f, 45f, 50f, 43f, 55f, 40f, 40f, 40f, 20f });

            double nGTotalEmpCount = 0, nGTotalOTHour = 0, nGTotalTwoHourOT = 0, nGTotalExtraOTHour = 0, nGTotalGrossAmount = 0, nGTotalTotalOTAmt = 0, nGTotalTwoHourOTAmt = 0, nGTotalExtraOTAmt = 0, nGTotalAdditionAmt = 0, nGTotalDeductionAmnt = 0, nGTotalNetAmnt = 0, nGTotalBankAmnt = 0, nGTotalCashAmnt = 0;
            _oTempEmployeeSalaryV2s = _oTempEmployeeSalaryV2s.OrderBy(x => x.BUName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
            while (_oTempEmployeeSalaryV2s.Count > 0)
            {

                string BUName = _oTempEmployeeSalaryV2s[0].BUName;
                this.PrintHeader(BUName,true);
                List<EmployeeSalaryV2> oEmployeeSalaryV2s = _oTempEmployeeSalaryV2s.Where(x => x.BUName == BUName).ToList();



                var oDBMonthlyAttendanceReports = oEmployeeSalaryV2s.GroupBy(x => new { x.DepartmentName }, (key, grp) => new
                {
                    DepartmentName = key.DepartmentName,
                    EmpCount = grp.ToList().Count(),
                    GrossAmount = grp.ToList().Sum(y => y.GrossAmount),
                    TwoHourOT = grp.ToList().Sum(y => y.DefineOTHour),
                    OTHour = grp.ToList().Sum(y => y.OTHour),
                    ExtraOtHour = grp.ToList().Sum(y => y.ExtraOTHour),
                    TotalOT = grp.ToList().Sum(y => y.OTHour * y.OTRatePerHour),
                    TotalExtraOT = grp.ToList().Sum(y => y.ExtraOTHour * y.OTRatePerHour),
                    TotalTwoHourOT = grp.ToList().Sum(y => y.DefineOTHour * y.OTRatePerHour),
                    NetAmount = grp.ToList().Sum(y => y.NetAmount),
                    BankAmount = grp.ToList().Sum(y => y.BankAmount),
                    CashAmount = grp.ToList().Sum(y => y.CashAmount),
                    Results = grp.ToList(),
                    AdditionAmount = grp.ToList().Sum(x => x.EmployeeSalaryAllowances.Sum(y => y.Amount)),
                    DeductionAmount = grp.ToList().Sum(x => x.EmployeeSalaryDeductions.Sum(y => y.Amount))
                });

                int nCount = 0;

                oPdfPTable1 = new PdfPTable(10);
                oPdfPTable1.SetWidths(new float[] { 18f, 65f, 45f, 50f, 43f, 55f, 40f, 40f, 40f, 20f });



                oPdfPCell1 = new PdfPCell(new Phrase("Unit : " + BUName, _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = 10;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPTable1.CompleteRow();


                oPdfPCell1 = new PdfPCell(new Phrase("SL#", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Department", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("ManPower", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Gross", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("Addition", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Deduction", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);




                oPdfPCell1 = new PdfPCell(new Phrase("Net", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("Bank", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("Cash", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 9; oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPTable1.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                double nTotalEmpCount = 0, nTotalOTHour = 0, nTotalTwoHourOT = 0, nTotalExtraOTHour = 0, nTotalGrossAmount = 0, nTotalTotalOTAmt = 0, nTotalTwoHourOTAmt = 0, nTotalExtraOTAmt = 0, nTotalAdditionAmt = 0, nTotalDeductionAmnt = 0, nTotalNetAmnt = 0, nTotalBankAmnt = 0, nTotalCashAmnt = 0;
                foreach (var oBUEmployeeSalary in oDBMonthlyAttendanceReports)
                {
                    oPdfPTable1 = new PdfPTable(10);
                    oPdfPTable1.SetWidths(new float[] { 18f, 65f, 45f, 50f, 43f, 55f, 40f, 40f, 40f, 20f });
                    nCount++;
                    oPdfPCell1 = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.DepartmentName, _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.EmpCount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalEmpCount += oBUEmployeeSalary.EmpCount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.GrossAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalGrossAmount += oBUEmployeeSalary.GrossAmount;


                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.AdditionAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalAdditionAmt += oBUEmployeeSalary.AdditionAmount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.DeductionAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalDeductionAmnt += oBUEmployeeSalary.DeductionAmount;

                   
                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.NetAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalNetAmnt += oBUEmployeeSalary.NetAmount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.BankAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalBankAmnt += oBUEmployeeSalary.BankAmount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.CashAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalCashAmnt += oBUEmployeeSalary.CashAmount;

                    oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 9; oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPTable1.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns; oPdfPCell1.FixedHeight = FixedHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                #region SubTotal
                oPdfPTable1 = new PdfPTable(10);
                oPdfPTable1.SetWidths(new float[] { 18f, 65f, 45f, 50f, 43f, 55f, 40f, 40f, 40f, 20f });

                oPdfPCell1 = new PdfPCell(new Phrase("SubTotal:", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight; oPdfPCell1.Colspan = 2;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalEmpCount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalEmpCount += nTotalEmpCount;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalGrossAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalGrossAmount += nTotalGrossAmount;


                oPdfPCell1 = new PdfPCell(new Phrase(nTotalAdditionAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalAdditionAmt += nTotalAdditionAmt;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalDeductionAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalDeductionAmnt += nTotalDeductionAmnt;


                oPdfPCell1 = new PdfPCell(new Phrase(nTotalNetAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalNetAmnt += nTotalNetAmnt;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalBankAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalBankAmnt += nTotalBankAmnt;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalCashAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalCashAmnt += nTotalCashAmnt;


                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns -9;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPTable1.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns; oPdfPCell1.FixedHeight = FixedHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                #endregion
                _oTempEmployeeSalaryV2s.RemoveAll(x => x.BUName == BUName);


            }
            #region GrandTotal
            oPdfPTable1 = new PdfPTable(10);
            oPdfPTable1.SetWidths(new float[] { 18f, 65f, 45f, 50f, 43f, 55f, 40f, 40f, 40f, 20f });
            oPdfPCell1 = new PdfPCell(new Phrase("Grand Total:", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight; oPdfPCell1.Colspan = 2;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalEmpCount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalGrossAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalAdditionAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalDeductionAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalNetAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalBankAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalCashAmnt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 9; oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPTable1.CompleteRow();
            _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            #endregion

        }

        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount) : amount;
            return (bWithPrecision) ? Global.MillionFormat(amount) : Global.MillionFormat(amount).Split('.')[0];
        }

        #endregion
    }
}
