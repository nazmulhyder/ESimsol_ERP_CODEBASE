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
    public class rptSalarySheetF01
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(9);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        List<EmployeeSalaryV2> _oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
        List<EmployeeSalaryV2> _oSummaryEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
        string _sStartDate = "";
        string _sEndDate = "";
        int _nTotalRow = 0;
        bool _bHasOTAllowance = false;
        bool _isComp = false;
        bool _IsOT = false;
        DateTime _StartDate = DateTime.Now;
        int _nGroupBySerial = 0;

        int nCount = 0,_nCount=0;
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
        #endregion

        public byte[] PrepareReport(int nGroupBySerial,List<EmployeeSalaryV2> oEmployeeSalaryV2s, Company oCompany,List<SalarySheetSignature>oSalarySheetSignatures, string StartDate, string EndDate,bool IsOT)
        {
            _oCompany = oCompany;
            _oEmployeeSalaryV2s = oEmployeeSalaryV2s;
             _StartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            bool bIsSerialWise = nGroupBySerial ==2 ? true : false ;
            _IsOT = IsOT;
            _sStartDate = _StartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");
            _nGroupBySerial = nGroupBySerial;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(40f, 40f, 20f, 55f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            oPDFWriter.PageEvent = PageEventHandler;
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 20f, 260f, 90f, 70f, 55f, 85f, 80f, 80f, 75f });
          
            #endregion
            this.PrintBody(bIsSerialWise);
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        private void PrintBody(bool bIsSerialWise)
        {
            
            if(_oEmployeeSalaryV2s.Count>0)
            {
                //bool bIsSerialWises = true;
                _nTotalRow = _oEmployeeSalaryV2s.Count;
                if (bIsSerialWise)
                {
                    _oEmployeeSalaryV2s.ForEach(x => _oSummaryEmployeeSalaryV2s.Add(x));
                    _oSummaryEmployeeSalaryV2s = _oSummaryEmployeeSalaryV2s.OrderBy(x => x.DepartmentName).ToList();

                    if (_oCompany.BaseAddress.ToUpper() == "WANGS")
                    {
                        foreach (EmployeeSalaryV2 oItem in _oEmployeeSalaryV2s)
                        {
                            oItem.EmployeeCodeSL = Convert.ToInt32(oItem.EmployeeCode);
                        }
                        _oEmployeeSalaryV2s = _oEmployeeSalaryV2s.OrderBy(x => x.EmployeeCodeSL).ToList();
                    }
                    PrintSalarySheet(_oEmployeeSalaryV2s, bIsSerialWise);


                }
                else
                {
                    _oEmployeeSalaryV2s = _oEmployeeSalaryV2s.OrderBy(x => x.BUName).ThenBy(x=>x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    _oEmployeeSalaryV2s.ForEach(x => _oSummaryEmployeeSalaryV2s.Add(x));
                    while (_oEmployeeSalaryV2s.Count > 0)
                    {
                        List<EmployeeSalaryV2> oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
                        oTempEmployeeSalaryV2s = _oEmployeeSalaryV2s.Where(x => x.BUName == _oEmployeeSalaryV2s[0].BUName).ToList();
                        string sBUNName = oTempEmployeeSalaryV2s.Count > 0 ? oTempEmployeeSalaryV2s[0].BUName : "";
                        while (oTempEmployeeSalaryV2s.Count > 0)
                        {
                            List<EmployeeSalaryV2> oTempEmpSs = new List<EmployeeSalaryV2>();
                            oTempEmpSs = oTempEmployeeSalaryV2s.Where(x => x.DepartmentName == oTempEmployeeSalaryV2s[0].DepartmentName).ToList();

                            PrintSalarySheet(oTempEmpSs, bIsSerialWise);
                           

                            oTempEmployeeSalaryV2s.RemoveAll(x => x.DepartmentName == oTempEmpSs[0].DepartmentName);
                        }
                        _oEmployeeSalaryV2s.RemoveAll(x => x.BUName == sBUNName);
                    }
                }
                this.Footer();
            }

        }

        public void DepartmentWiseSubTotal(List<EmployeeSalaryV2> oEmployeeSalaryV2s)
        {

            double TotalSalaryPos = oEmployeeSalaryV2s.Sum(x => x.EmployeeSalaryBasics.Sum(y=>y.Amount));
            double TotalAlloweance = oEmployeeSalaryV2s.Sum(x => x.EmployeeSalaryAllowances.Sum(y => y.Amount));
            double TotalDeduction = oEmployeeSalaryV2s.Sum(x => x.EmployeeSalaryDeductions.Sum(y => y.Amount));
            double TotalNetAmount = oEmployeeSalaryV2s.Sum(x => x.NetAmount);
            if(_IsOT==true)
            {
                TotalNetAmount += oEmployeeSalaryV2s.Sum(x => x.OTHour * x.OTRatePerHour);
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Department Wise Sub Total : ", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(TotalSalaryPos > 0 ? this.GetAmountInStr(TotalSalaryPos, true, false) : "-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(TotalAlloweance > 0 ? this.GetAmountInStr(TotalAlloweance, true, false) : "-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(TotalDeduction > 0 ? this.GetAmountInStr(TotalDeduction, true, false) : "-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(TotalNetAmount > 0 ? this.GetAmountInStr(TotalNetAmount, true, false) : "-", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }
        public void Footer()
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 20; _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);

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

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();



            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            //this.PrintHeader();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9; _oPdfPCell.FixedHeight = 35;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            if(_IsOT==true)
            {
                this.Summary();
            }
            else
            {
                this.SummaryWithoutOT();
            }
            
            _oPdfPTable.CompleteRow();


        }
   

        public void PrintSalarySheet(List<EmployeeSalaryV2> oEmployeeSalaryV2s, bool bIsSerialWise)
        {
          
                nCount = 0;
                nPageCount = 0;
      

            this.PrintHeader(oEmployeeSalaryV2s[0].BUName,false);
            PrintHaedRow(oEmployeeSalaryV2s[0], bIsSerialWise);
            List<EmployeeSalaryV2> oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
            oTempEmployeeSalaryV2s = oEmployeeSalaryV2s;
            foreach (EmployeeSalaryV2 oEmpSalaryItem in oEmployeeSalaryV2s)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                nCount++;
                nTotalCount++;
                _nCount++;
                if(_nGroupBySerial==3)//For Serial Continue
                {
                    _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
               

                _oPdfPCell = new PdfPCell(GetEmpOffical(oEmpSalaryItem)); _oPdfPCell.PaddingLeft = -20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetEmpSalary(oEmpSalaryItem.EmployeeSalaryBasics,oEmpSalaryItem.GrossAmount));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                double nAttTotal = 0;
                int days = DateTime.DaysInMonth(_StartDate.Year, _StartDate.Month);
                if(oEmpSalaryItem.JoiningDate>_StartDate)
                {
                    days = days - Convert.ToInt32(oEmpSalaryItem.JoiningDate.ToString("dd"));
                }
                int pDay = days - (oEmpSalaryItem.TotalAbsent + oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalHoliday + oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave);
                nAttTotal = pDay + oEmpSalaryItem.TotalAbsent + oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalHoliday + oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave;
                _oPdfPCell = new PdfPCell(new Phrase("Present: " + pDay + "\nAbsent: " + oEmpSalaryItem.TotalAbsent + "\nOff day: " + oEmpSalaryItem.TotalDayOff + "\nLeave: " + (oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave) + "\nHoliday: " + oEmpSalaryItem.TotalHoliday + "\nTotal=" + nAttTotal, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(GetLeaveStatus(oEmpSalaryItem.EmployeeWiseLeaveStatus), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetAdditionSalary(oEmpSalaryItem));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(GetDeductionSalary(oEmpSalaryItem.EmployeeSalaryDeductions));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
                double nTotalNetAmount = _IsOT == true ? oEmpSalaryItem.NetAmount + (oEmpSalaryItem.OTRatePerHour * oEmpSalaryItem.OTHour) : oEmpSalaryItem.NetAmount;
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalNetAmount, true, false), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase((oEmpSalaryItem.BankAmount > 0) ? "Bank: " + oEmpSalaryItem.BankName + "\nAcc. No: " + oEmpSalaryItem.AccountNo : "", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
              

                nTotalGrossPerPage = nTotalGrossPerPage + oEmpSalaryItem.GrossAmount;
                nTotalGross = nTotalGross + oEmpSalaryItem.GrossAmount;
                nTotalNetSalaryPerPage = nTotalNetSalaryPerPage + nTotalNetAmount;
                nTotalNetSalary = nTotalNetSalary + nTotalNetAmount;

                   if (nCount == 6)
                    {
                        nPageCount = 1;
                    }

                   int nAlreadyPrintTotal = 0;
                if (nCount == 6 * nPageCount)
                {
                    nPageCount++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
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
                    nAlreadyPrintTotal = 1;
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

                    if (oEmployeeSalaryV2s.Count != nCount)
                    {

                        this.PrintHeader(oEmployeeSalaryV2s[0].BUName,false);
                        PrintHaedRow(oEmployeeSalaryV2s[0], bIsSerialWise);
                    }
                }
                if (oEmployeeSalaryV2s.Count == nCount)
                {
                    if (nAlreadyPrintTotal==0)
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
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
                    }
                   
                    if(_nGroupBySerial!=2)
                    {
                        this.DepartmentWiseSubTotal(oTempEmployeeSalaryV2s);
                    }
                   
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
        public PdfPTable GetAdditionSalary(EmployeeSalaryV2 oEmployeeSalaryV2)
        {
            double nLeaveAllowancePerEmployee = 0;

            PdfPTable oPdfPTable_Addition = new PdfPTable(2);
            PdfPCell oPdfPCell_Addition;

            oPdfPTable_Addition.SetWidths(new float[] { 70f, 50f });

            if(_IsOT)
            {
                if (oEmployeeSalaryV2.OTRatePerHour > 0)
                {
                    oPdfPCell_Addition = new PdfPCell(new Phrase("OT Rate: ", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                    oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                    double nOTAmt = oEmployeeSalaryV2.OTHour * oEmployeeSalaryV2.OTRatePerHour;

                    oPdfPCell_Addition = new PdfPCell(new Phrase(oEmployeeSalaryV2.OTRatePerHour.ToString(), _oFontStyle)); oPdfPCell_Addition.Border = 0;
                    oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                    oPdfPTable_Addition.CompleteRow();
                }

                if (oEmployeeSalaryV2.OTHour > 0)
                {
                    oPdfPCell_Addition = new PdfPCell(new Phrase("OT Hr.: ", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                    oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);



                    oPdfPCell_Addition = new PdfPCell(new Phrase(oEmployeeSalaryV2.OTHour.ToString(), _oFontStyle)); oPdfPCell_Addition.Border = 0;
                    oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                    oPdfPTable_Addition.CompleteRow();

                    if (oEmployeeSalaryV2.OTRatePerHour > 0)
                    {
                        oPdfPCell_Addition = new PdfPCell(new Phrase("OT All. : ", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                        oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);


                        oPdfPCell_Addition = new PdfPCell(new Phrase(Math.Round((oEmployeeSalaryV2.OTHour * oEmployeeSalaryV2.OTRatePerHour)).ToString("#,##0"), _oFontStyle)); oPdfPCell_Addition.Border = 0;
                        oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                        oPdfPTable_Addition.CompleteRow();
                    }
                }

            }
          
           

           

            nTotalAllowancePerPage += oEmployeeSalaryV2.OTHour * oEmployeeSalaryV2.OTRatePerHour;
            nTotalAllowance += oEmployeeSalaryV2.OTHour * oEmployeeSalaryV2.OTRatePerHour;


            foreach (EmployeeSalaryDetailV2 oESDItem in oEmployeeSalaryV2.EmployeeSalaryAllowances)
            {
                oPdfPCell_Addition = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                oPdfPCell_Addition = new PdfPCell(new Phrase(oESDItem.Amount > 0 ? this.GetAmountInStr(oESDItem.Amount, true, false) : "-", _oFontStyle)); oPdfPCell_Addition.Border = 0;
                oPdfPCell_Addition.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Addition.BackgroundColor = BaseColor.WHITE; oPdfPTable_Addition.AddCell(oPdfPCell_Addition);

                oPdfPTable_Addition.CompleteRow();

                nTotalAllowancePerPage += oESDItem.Amount;
                nTotalAllowance += oESDItem.Amount;
            }


            return oPdfPTable_Addition;
        }

        public PdfPTable GetDeductionSalary(List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions)
        {
            PdfPTable oPdfPTable_Deduction = new PdfPTable(2);
            PdfPCell oPdfPCell_Deduction;

            oPdfPTable_Deduction.SetWidths(new float[] { 70f, 50f });

            foreach (EmployeeSalaryDetailV2 oESDItem in oEmployeeSalaryDeductions)
            {
                oPdfPCell_Deduction = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle)); oPdfPCell_Deduction.Border = 0;
                oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

                oPdfPCell_Deduction = new PdfPCell(new Phrase(oESDItem.Amount > 0 ? this.GetAmountInStr(oESDItem.Amount, true, false) : "-", _oFontStyle)); oPdfPCell_Deduction.Border = 0;
                oPdfPCell_Deduction.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell_Deduction.BackgroundColor = BaseColor.WHITE; oPdfPTable_Deduction.AddCell(oPdfPCell_Deduction);

                oPdfPTable_Deduction.CompleteRow();
                nTotalDeductionPerPage += oESDItem.Amount;
                nTotalDeduction += oESDItem.Amount;
            }
            return oPdfPTable_Deduction;
        }
        private string GetLeaveStatus(List<LeaveStatus> oLeaveStatus)
        {
            string Status = "";
            foreach (LeaveStatus oItemLeaveStatus in oLeaveStatus)
            {

                Status += oItemLeaveStatus.LeaveHeadShortName + "-" + oItemLeaveStatus.LeaveDays + ", ";
            }
            if (!string.IsNullOrEmpty(Status)) { Status = Status.Remove(Status.Length - 2); };
            return Status;
        }
        public PdfPTable GetEmpSalary(List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, double nGrossAmount)
        {
            PdfPTable oPdfPTable2 = new PdfPTable(2);
            PdfPCell oPdfPCell2;

            oPdfPTable2.SetWidths(new float[] { 70f, 50f });


            foreach (EmployeeSalaryDetailV2 oESDItem in oEmployeeSalaryBasics)
            {
                oPdfPCell2 = new PdfPCell(new Phrase(oESDItem.SalaryHeadName + " : ", _oFontStyle)); oPdfPCell2.Border = 0;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPCell2 = new PdfPCell(new Phrase(oESDItem.Amount > 0 ? this.GetAmountInStr(oESDItem.Amount, true, false) : "-", _oFontStyle)); oPdfPCell2.Border = 0;
                oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

                oPdfPTable2.CompleteRow();
            }

            oPdfPCell2 = new PdfPCell(new Phrase("Gross Salary : ", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);

            oPdfPCell2 = new PdfPCell(new Phrase(nGrossAmount > 0 ? this.GetAmountInStr(nGrossAmount, true, false) : "-", _oFontStyle)); oPdfPCell2.Border = 0;
            oPdfPCell2.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell2.BackgroundColor = BaseColor.WHITE; oPdfPTable2.AddCell(oPdfPCell2);
            oPdfPTable2.CompleteRow();

            return oPdfPTable2;
        }
        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount) : amount;
            return (bWithPrecision) ? Global.MillionFormat(amount) : Global.MillionFormat(amount).Split('.')[0];
        }
        public PdfPTable GetEmpOffical(EmployeeSalaryV2 oEmp)
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

            oPdfPCell1 = new PdfPCell(new Phrase(oEmp.EmployeeCode, _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            oPdfPCell1 = new PdfPCell(new Phrase("Name : ", _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(oEmp.EmployeeName, _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            oPdfPCell1 = new PdfPCell(new Phrase("Designation : ", _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(oEmp.DesignationName, _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            oPdfPCell1 = new PdfPCell(new Phrase("Date Of Join : ", _oFontStyle)); oPdfPCell1.Border = 0;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(oEmp.JoiningDate.ToString("dd MMM yyyy"), _oFontStyle)); oPdfPCell1.Border = 0;
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

        #region Report Header

        private void PrintHeader(string BuName,bool IsSummary)
        {
            #region CompanyHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(BuName, _oFontStyle));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            string sTitle = "Salary Sheet || " + "Salary Month-" + (_StartDate.ToString("MMM"));

            if(IsSummary==true)
            {
                sTitle = "Salary Summary|| " + "Salary Month-" + (_StartDate.ToString("MMM"));
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sTitle, _oFontStyle));
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
            #endregion
        }
        #endregion

        private void PrintHaedRow(EmployeeSalaryV2 oES, bool bIsSerialWise)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);

            if (bIsSerialWise)
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unit:" + oES.BUShortName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("Unit:" + oES.BUShortName + "   ||   " + "Department:" + oES.DepartmentName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }


            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
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

        public void Summary()
        {

          
            PdfPTable oPdfPTable1 = new PdfPTable(11);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 18f,60f, 45f, 45f,60f,45f,60f,60f,50f,40f,40f });

            double GTotalManPower = 0, GTotalOTHour = 0, GTotalGross = 0, GTotalOTAmount = 0, GTotalAddition = 0, GTotalDeduction = 0, GTotalNetAmount = 0, GTotalBankAmount = 0, GTotalCashAmount=0;
            while (_oSummaryEmployeeSalaryV2s.Count > 0)
            {
                
                string BUName = _oSummaryEmployeeSalaryV2s[0].BUName;
                this.PrintHeader(BUName,true);
                List<EmployeeSalaryV2> oEmployeeSalaryV2s = _oSummaryEmployeeSalaryV2s.Where(x => x.BUName == BUName).ToList();
                                      


                var oDBMonthlyAttendanceReports = oEmployeeSalaryV2s.GroupBy(x => new { x.DepartmentName }, (key, grp) => new
                {
                    DepartmentName = key.DepartmentName, 
                    EmpCount = grp.ToList().Count(),
                    GrossAmount = grp.ToList().Sum(y => y.GrossAmount),
                    OTHour = grp.ToList().Sum(y => y.OTHour),
                    TotalOT = grp.ToList().Sum(y => y.OTHour * y.OTRatePerHour),
                    NetAmount = grp.ToList().Sum(y => y.NetAmount),
                    BankAmount=grp.ToList().Sum(y=>y.BankAmount),
                    CashAmount=grp.ToList().Sum(y=>y.CashAmount),
                    Results = grp.ToList(),
                    AdditionAmount = grp.ToList().Sum(x => x.EmployeeSalaryAllowances.Sum(y => y.Amount)),
                    DeductionAmount = grp.ToList().Sum(x => x.EmployeeSalaryDeductions.Sum(y => y.Amount))
                });


                oPdfPTable1 = new PdfPTable(11);
                oPdfPTable1.SetWidths(new float[] { 18f, 60f, 45f, 45f, 60f, 45f, 60f, 60f, 50f, 40f, 40f });


                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                oPdfPCell1 = new PdfPCell(new Phrase("Unit : " + BUName, _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = 11;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPTable1.CompleteRow();

                oPdfPCell1 = new PdfPCell(new Phrase("SL#", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("Department", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("ManPower", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("OT Hour", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Gross", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("OT", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Addition", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Deduction", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Net", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Bank", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Cash", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPTable1.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                double TotalManPower = 0, TotalOTHour = 0, TotalGross = 0, TotalOTAmount = 0, TotalAddition = 0, TotalDeduction = 0, TotalNetAmount = 0, TotalBankAmount = 0, TotalCashAmount = 0;
                int nCount = 0;
                foreach (var oBUEmployeeSalary in oDBMonthlyAttendanceReports)
                {
                    oPdfPTable1 = new PdfPTable(11);
                    oPdfPTable1.SetWidths(new float[] { 18f, 60f, 45f, 45f, 60f, 45f, 60f, 60f, 50f, 40f, 40f });

                    nCount++;

                    oPdfPCell1 = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.DepartmentName, _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.EmpCount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalManPower += oBUEmployeeSalary.EmpCount;
                    GTotalManPower += oBUEmployeeSalary.EmpCount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.OTHour.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalOTHour += oBUEmployeeSalary.OTHour;
                    GTotalOTHour += oBUEmployeeSalary.OTHour;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.GrossAmount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalGross += oBUEmployeeSalary.GrossAmount;
                    GTotalGross += oBUEmployeeSalary.GrossAmount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.TotalOT.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalOTAmount += oBUEmployeeSalary.TotalOT;
                    GTotalOTAmount += oBUEmployeeSalary.TotalOT;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.AdditionAmount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalAddition += oBUEmployeeSalary.AdditionAmount;
                    GTotalAddition += oBUEmployeeSalary.AdditionAmount;


                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.DeductionAmount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalDeduction += oBUEmployeeSalary.DeductionAmount;
                    GTotalDeduction += oBUEmployeeSalary.DeductionAmount;


                    oPdfPCell1 = new PdfPCell(new Phrase((oBUEmployeeSalary.NetAmount+oBUEmployeeSalary.TotalOT).ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalNetAmount += oBUEmployeeSalary.NetAmount + oBUEmployeeSalary.TotalOT;
                    GTotalNetAmount += oBUEmployeeSalary.NetAmount + oBUEmployeeSalary.TotalOT;


                    oPdfPCell1 = new PdfPCell(new Phrase((oBUEmployeeSalary.BankAmount + oBUEmployeeSalary.TotalOT).ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalBankAmount += oBUEmployeeSalary.BankAmount + oBUEmployeeSalary.TotalOT;
                    GTotalBankAmount += oBUEmployeeSalary.BankAmount + oBUEmployeeSalary.TotalOT;


                    oPdfPCell1 = new PdfPCell(new Phrase((oBUEmployeeSalary.CashAmount + oBUEmployeeSalary.TotalOT).ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalCashAmount += oBUEmployeeSalary.CashAmount + oBUEmployeeSalary.TotalOT;
                    GTotalCashAmount += oBUEmployeeSalary.CashAmount + oBUEmployeeSalary.TotalOT;

                    oPdfPTable1.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }


                oPdfPTable1 = new PdfPTable(11);
                oPdfPTable1.SetWidths(new float[] { 18f, 60f, 45f, 45f, 60f, 45f, 60f, 60f, 50f, 40f, 40f });

                oPdfPCell1 = new PdfPCell(new Phrase("SubTotal", _oFontStyle)); oPdfPCell1.Colspan = 2;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(TotalManPower.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(TotalOTHour.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase(TotalGross.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase(TotalOTAmount.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase(TotalAddition.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);



                oPdfPCell1 = new PdfPCell(new Phrase(TotalDeduction.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);



                oPdfPCell1 = new PdfPCell(new Phrase(TotalNetAmount.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(TotalBankAmount.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(TotalCashAmount.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                oPdfPTable1.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oSummaryEmployeeSalaryV2s.RemoveAll(x => x.BUName == BUName);

                if (_oSummaryEmployeeSalaryV2s.Count > 0)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                }
            }


            oPdfPTable1 = new PdfPTable(11);
            oPdfPTable1.SetWidths(new float[] { 18f, 60f, 45f, 45f, 60f, 45f, 60f, 60f, 50f, 40f, 40f });

            oPdfPCell1 = new PdfPCell(new Phrase("GrandTotal:", _oFontStyle)); oPdfPCell1.Colspan = 2;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(GTotalManPower.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(GTotalOTHour.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(GTotalGross.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(GTotalOTAmount.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(GTotalAddition.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);



            oPdfPCell1 = new PdfPCell(new Phrase(GTotalDeduction.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);



            oPdfPCell1 = new PdfPCell(new Phrase(GTotalNetAmount.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(GTotalBankAmount.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(GTotalCashAmount.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

        }

        public void SummaryWithoutOT()
        {


            PdfPTable oPdfPTable1 = new PdfPTable(9);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 18f,40f, 40f,  40f,  40f, 40f, 40f,40f,40f });

            double GTotalManPower = 0, GTotalGross = 0, GTotalAddition = 0, GTotalDeduction = 0, GTotalNetAmount = 0, GTotalBankAmount = 0, GTotalCashAmount = 0;
            while (_oSummaryEmployeeSalaryV2s.Count > 0)
            {

                string BUName = _oSummaryEmployeeSalaryV2s[0].BUName;
                this.PrintHeader(BUName,true);
                List<EmployeeSalaryV2> oEmployeeSalaryV2s = _oSummaryEmployeeSalaryV2s.Where(x => x.BUName == BUName).ToList();



                var oDBMonthlyAttendanceReports = oEmployeeSalaryV2s.GroupBy(x => new { x.DepartmentName }, (key, grp) => new
                {
                    DepartmentName = key.DepartmentName,
                    EmpCount = grp.ToList().Count(),
                    GrossAmount = grp.ToList().Sum(y => y.GrossAmount),
                    OTHour = grp.ToList().Sum(y => y.OTHour),
                    TotalOT = grp.ToList().Sum(y => y.OTHour * y.OTRatePerHour),
                    NetAmount = grp.ToList().Sum(y => y.NetAmount),
                    BankAmount=grp.ToList().Sum(y=>y.BankAmount),
                    CashAmount=grp.ToList().Sum(y=>y.CashAmount),
                    Results = grp.ToList(),
                    AdditionAmount = grp.ToList().Sum(x => x.EmployeeSalaryAllowances.Sum(y => y.Amount)),
                    DeductionAmount = grp.ToList().Sum(x => x.EmployeeSalaryDeductions.Sum(y => y.Amount))
                });


                oPdfPTable1 = new PdfPTable(9);
                oPdfPTable1.SetWidths(new float[] { 18f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f });


                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                oPdfPCell1 = new PdfPCell(new Phrase("Unit : " + BUName, _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = 9;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPTable1.CompleteRow();
                oPdfPCell1 = new PdfPCell(new Phrase("SL#", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Department", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("ManPower", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("Gross", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("Addition", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Deduction", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Net", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Bank", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Cash", _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPTable1.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                double TotalManPower = 0, TotalGross = 0, TotalAddition = 0, TotalDeduction = 0, TotalNetAmount = 0, TotalBankAmount = 0, TotalCashAmount = 0;
                int nCount = 0;
                foreach (var oBUEmployeeSalary in oDBMonthlyAttendanceReports)
                {
                    oPdfPTable1 = new PdfPTable(9);
                    oPdfPTable1.SetWidths(new float[] { 18f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f });
                    nCount++;
                    oPdfPCell1 = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.DepartmentName, _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.EmpCount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalManPower += oBUEmployeeSalary.EmpCount;
                    GTotalManPower += oBUEmployeeSalary.EmpCount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.GrossAmount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalGross += oBUEmployeeSalary.GrossAmount;
                    GTotalGross += oBUEmployeeSalary.GrossAmount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.AdditionAmount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalAddition += oBUEmployeeSalary.AdditionAmount;
                    GTotalAddition += oBUEmployeeSalary.AdditionAmount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.DeductionAmount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalDeduction += oBUEmployeeSalary.DeductionAmount;
                    GTotalDeduction += oBUEmployeeSalary.DeductionAmount;


                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.NetAmount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalNetAmount += oBUEmployeeSalary.NetAmount;
                    GTotalNetAmount += oBUEmployeeSalary.NetAmount;


                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.BankAmount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalBankAmount += oBUEmployeeSalary.BankAmount;
                    GTotalBankAmount += oBUEmployeeSalary.BankAmount;


                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.CashAmount.ToString("#,##0"), _oFontStyle));
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    TotalCashAmount += oBUEmployeeSalary.CashAmount;
                    GTotalCashAmount += oBUEmployeeSalary.CashAmount;
                    oPdfPTable1.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }


                oPdfPTable1 = new PdfPTable(9);
                oPdfPTable1.SetWidths(new float[] { 18f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f });

                oPdfPCell1 = new PdfPCell(new Phrase("SubTotal", _oFontStyle)); oPdfPCell1.Colspan = 2;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(TotalManPower.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);



                oPdfPCell1 = new PdfPCell(new Phrase(TotalGross.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase(TotalAddition.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);



                oPdfPCell1 = new PdfPCell(new Phrase(TotalDeduction.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);



                oPdfPCell1 = new PdfPCell(new Phrase(TotalNetAmount.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(TotalBankAmount.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(TotalCashAmount.ToString("#,##0"), _oFontStyle));
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                oPdfPTable1.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oSummaryEmployeeSalaryV2s.RemoveAll(x => x.BUName == BUName);

                if(_oSummaryEmployeeSalaryV2s.Count>0)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                }
            }

            oPdfPTable1 = new PdfPTable(9);
            oPdfPTable1.SetWidths(new float[] { 18f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 40f });

            oPdfPCell1 = new PdfPCell(new Phrase("GrandTotal:", _oFontStyle)); oPdfPCell1.Colspan = 2;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(GTotalManPower.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(GTotalGross.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(GTotalAddition.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);



            oPdfPCell1 = new PdfPCell(new Phrase(GTotalDeduction.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);



            oPdfPCell1 = new PdfPCell(new Phrase(GTotalNetAmount.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(GTotalBankAmount.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(GTotalCashAmount.ToString("#,##0"), _oFontStyle));
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

        }

     
    }
}
