using System;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ESimSol.BusinessObjects.ReportingObject;
using System.Collections;

namespace ESimSol.Reports
{

   
    public class rptAMGSalarySheet
    {
        #region Declaration
        private int _nColumn = 1;
        private Document _oDocument;
        private iTextSharp.text.Font _oFontStyle;
        private PdfPTable _oPdfPTable = new PdfPTable(1);
        private PdfPCell _oPdfPCell;
        private iTextSharp.text.Image _oImag;
        private MemoryStream _oMemoryStream = new MemoryStream();
        List<AMGSalarySheet> _oAMGSalarySheets = new List<AMGSalarySheet>();
        private Company _oCompany = new Company();
        string _salaryMonth = string.Empty;
        #endregion

        public byte[] PrepareReport(List<AMGSalarySheet> oAMGSalarySheets, Company oCompany, string salaryMonth)
        {
            _oAMGSalarySheets = oAMGSalarySheets;
            _oCompany = oCompany;
            _salaryMonth = salaryMonth;

            #region Page Setup

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            _oDocument.SetMargins(10f, 10f, 5f, 5f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter PDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            PDFWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.signatures =  (new string[]{"Prepared By", "Checked By", "HR & Compliance", "Chief Accountant", "Director"}).ToList();//_oSalarySheetSignatures.Select(x=>x.SignatureName).ToList();
            PDFWriter.PageEvent = PageEventHandler; //Footer print with page event handler
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 842f });

            #endregion

            this.PrintBody();
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private PdfPTable PrintHeader(string unitName)
        {
            PdfPCell oPdfPCell;
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 825f });

            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(PrintHeaderWithLogo(unitName));
            oPdfPCell.Colspan = _nColumn;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.ExtraParagraphSpace = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        private PdfPTable PrintCompanyInfo(string unitName)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.SetWidths(new float[] { 400f });
            PdfPCell oPdfPCell;
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase(unitName, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.ExtraParagraphSpace = 0;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Staff Salary Sheet For The M/O - " + _salaryMonth, _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPCell.FixedHeight = 10f;
            oPdfPTable.CompleteRow();

            return oPdfPTable;
        }
        private PdfPTable PrintHeaderWithLogo(string unitName)
        {
            PdfPTable oPdfPTable = new PdfPTable(2);
            oPdfPTable.SetWidths(new float[] { 10f, 200f });
            PdfPCell oPdfPCell;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(160f, 35f);
                oPdfPCell = new PdfPCell(_oImag);
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCell.Border = 0;
                oPdfPCell.FixedHeight = 35;
                oPdfPTable.AddCell(oPdfPCell);
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 13f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(PrintCompanyInfo(unitName));
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCell.Border = 0;
            oPdfPCell.BackgroundColor = BaseColor.WHITE;
            oPdfPCell.ExtraParagraphSpace = 0;
            if (_oCompany.CompanyLogo == null)
            {
                oPdfPCell.Colspan = 2;
            }
            oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            return oPdfPTable;
        }

        #endregion


        private PdfPTable PDFTableDeclaration(ArrayList table_header)
        {
            PdfPTable oPdfPTable = new PdfPTable(table_header.Count);
            oPdfPTable.SetWidths(table_header.Cast<TableHeader>().Select(x => x.Width).ToArray());
            return oPdfPTable;
        }

        private void HeaderSetUp(ArrayList table_header, string unitName, ref PdfPTable oPdfPTable, string sTitle)
        {
            ESimSolItexSharp.PushTableInCell(ref oPdfPTable, this.PrintHeader(unitName), 0, oPdfPTable.NumberOfColumns, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);


            iTextSharp.text.Font fontBoldHeader = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            ESimSolItexSharp.SetCellValue(ref oPdfPTable, sTitle, 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, 0, fontBoldHeader);
            oPdfPTable.CompleteRow();

            iTextSharp.text.Font fontBoldNormal = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.BOLD);
            #region Column Print

            foreach (TableHeader listItem in table_header)
            {
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, listItem.Header, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, ((listItem.IsRotate) ? 90 : 0), fontBoldNormal);
            }
            oPdfPTable.CompleteRow();
            #endregion
        }


        #region Body
        private void PrintBody()
        {
            var fontBold = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.BOLD);
            var fontBoldNet = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.BOLD);
            var fontBoldHeader = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            var fontNormal = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);

            ArrayList table_header = new ArrayList();
            table_header.Add(new TableHeader { Header = "#SL", Width = 30f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Employee Code", Width = 70f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "EMployee Name", Width = 90f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Designation", Width = 90f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Join Date", Width = 67f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Total Days", Width = 30f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Present Day", Width = 30f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Day Off Holiday", Width = 45f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Absent Days", Width = 30f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "CL", Width = 30f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "ML", Width = 30f, IsRotate = true });//Medical Leave
            table_header.Add(new TableHeader { Header = "EL", Width = 30f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "LWP", Width = 30f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Leave Days", Width = 30f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "EWD", Width = 30f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Gross Salary", Width = 55f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Att Bonous", Width = 50f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Gross Earnings", Width = 55f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Basic", Width = 55f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "House Rent", Width = 50f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Conveyance", Width = 40f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Food", Width = 40f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Medical", Width = 40f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Absent Amt", Width = 40f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Stamp", Width = 45f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Total Deduct", Width = 45f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Net Amount", Width = 55f, IsRotate = true });
            table_header.Add(new TableHeader { Header = "Bank", Width = 55f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Cash", Width = 55f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Account No", Width = 110f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Bank Name", Width = 103f, IsRotate = false });
            table_header.Add(new TableHeader { Header = "Signature", Width = 130f, IsRotate = false });


            PdfPTable oPdfPTable = PDFTableDeclaration(table_header);
       


            #region Body Print
            float cellHeight = 60f;
            int nCount = 0;
            int nBreakCount = 0;
            _oAMGSalarySheets = _oAMGSalarySheets.OrderBy(x => x.BUName).ThenBy(x => x.LocName).ThenBy(x => x.DptName).ThenBy(x => x.Code).ToList();
            var data = _oAMGSalarySheets.GroupBy(x => new { x.BUName, x.LocName, x.DptName }, (key, grp) => new
            {
                BUName = key.BUName,
                LocName = key.LocName,
                DptName = key.DptName,
                Results = grp.ToList()
            });


            int nEmpCount = 0;
            int nDeptID = 0;
            bool flag=true;
            bool lstFlag = false;

            double nGross = 0;
            double nAttBonus = 0;
            double nGrossEarning = 0;
            double nBasics = 0;
            double nHR = 0;
            double nConv = 0;
            double nFood = 0;
            double nMed = 0;
            double nAbsentAmount = 0;
            double nStemp = 0;
            double nTotalDeduction = 0;
            double nNetAmount = 0;

            double sBank = 0;
            double sCash = 0;
            double dTBank = 0;
            double dTCash = 0;

            int deptCounter = 0;
            int nTotalRowsForGToral = 0;
            int nDeptTotal = 0;
            foreach (var oItem in data)
            {
                sBank = 0;
                sCash = 0;

                nTotalRowsForGToral += oItem.Results.Count();
                nDeptTotal = oItem.Results.Count();
                foreach (var obj in oItem.Results)
                {
                    ++deptCounter;
                    ++nBreakCount;
                    nDeptID = obj.DepartmentID;
                    nEmpCount = oItem.Results.Count();
                    if (nCount % 7 == 0 || flag)
                    {
                        flag = false;
                        string sUnit = "Unit Name: " + oItem.LocName + ", Dept. Name: " + oItem.DptName;

                        oPdfPTable = PDFTableDeclaration(table_header);
                        HeaderSetUp(table_header, oItem.BUName, ref oPdfPTable, sUnit);
                    }

                    //oPdfPTable = PDFTableDeclaration(table_header);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, (++nCount).ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.Code, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.Name, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.DsgName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.DOJShort, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.TotalDays.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.Present.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.DayOffHoliday.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.Absent.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.CL.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.SL.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.EL.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.LWP.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.LeaveDays.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.EWD.ToString(), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.Gross, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.AttBonus, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.GrossEarning, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.Basics, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.HR, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.Conv, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.Food, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.Med, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.AbsentAmount, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.Stemp, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.TotalDeduction, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.NetAmount, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontBoldNet);


                    //Temp Solution But Confirm it with Mr. Al-Amin
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormatRound(obj.NetAmount, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);                    
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((string.IsNullOrEmpty(obj.AccountNo)) ? "" : Global.MillionFormatRound(obj.NetAmount, 0)), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);                    
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, ((string.IsNullOrEmpty(obj.AccountNo)) ? Global.MillionFormatRound(obj.NetAmount, 0) : ""), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);

                    if (obj.NetAmount > 0)
                    {
                        if (string.IsNullOrEmpty(obj.AccountNo))
                        {
                            sCash += Convert.ToDouble(Global.MillionFormatRound(obj.NetAmount, 0));
                            dTCash += Convert.ToDouble(Global.MillionFormatRound(obj.NetAmount, 0));
                        }
                        else
                        {
                            sBank += Convert.ToDouble(Global.MillionFormatRound(obj.NetAmount, 0));
                            dTBank += Convert.ToDouble(Global.MillionFormatRound(obj.NetAmount, 0));
                        }
                    }

                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.AccountNo, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, obj.BankName, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "-", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, cellHeight, fontNormal);
                    oPdfPTable.CompleteRow();


                    if (nDeptTotal == deptCounter)
                    {
                        deptCounter = 0;
                        continue;
                    }
                    if (nCount % 7 != 0 && flag)
                    {


                        ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                        if (lstFlag)
                        {
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 15, Element.ALIGN_CENTER, BaseColor.WHITE, true, 10, fontBold);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Gross), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nGross += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Gross);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AttBonus), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nAttBonus += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AttBonus);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.GrossEarning), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nGrossEarning += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.GrossEarning);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Basics), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nBasics += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Basics);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.HR), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nHR += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.HR);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Conv), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nConv += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Conv);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Food), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nFood += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Food);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Med), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nMed += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Med);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AbsentAmount), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nAbsentAmount += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AbsentAmount);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Stemp), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nStemp += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Stemp);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.TotalDeduction), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            nTotalDeduction += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.TotalDeduction);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.NetAmount), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBoldNet);
                            nNetAmount += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.NetAmount);

                            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(sBank, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(sCash, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.NetAmount), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBoldNet);
                            oPdfPTable.CompleteRow();


                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grand Total", 0, 15, Element.ALIGN_CENTER, BaseColor.WHITE, true, 10, fontBold);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGross, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nAttBonus, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrossEarning, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nBasics, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nHR, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nConv, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nFood, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nMed, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nAbsentAmount, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);


                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nStemp, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalDeduction, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nNetAmount, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(dTBank, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(dTCash, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nNetAmount, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                            oPdfPTable.CompleteRow();
                            lstFlag = true;
                        }

                        //_oDocument.Add(_oPdfPTable);
                        //_oDocument.NewPage();
                        //_oPdfPTable.DeleteBodyRows();
                    }
                    //if (nCount % 7 == 0 && (nDeptTotal != nBreakCount))
                    if (nCount % 7 == 0 && nCount!=0)
                    {
                        
                        ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();
                    }
                }
                if (lstFlag == false)
                {
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Total", 0, 15, Element.ALIGN_CENTER, BaseColor.WHITE, true, 10, fontBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Gross), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nGross += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Gross);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AttBonus), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nAttBonus += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AttBonus);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.GrossEarning), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nGrossEarning += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.GrossEarning);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Basics), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nBasics += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Basics);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.HR), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nHR += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.HR);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Conv), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nConv += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Conv);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Food), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nFood += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Food);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Med), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nMed += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Med);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AbsentAmount), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nAbsentAmount += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AbsentAmount);

                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Stemp), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nStemp += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.Stemp);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.TotalDeduction), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    nTotalDeduction += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.TotalDeduction);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.NetAmount), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBoldNet);
                    nNetAmount += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.NetAmount);


                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.NetAmount), 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBoldNet);

                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(sBank, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                    //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(sCash, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);


                    oPdfPTable.CompleteRow();
                } 
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();

                nCount = 0;

                ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                flag = true;
                if (nTotalRowsForGToral != nBreakCount)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();

                }

            }
            if (lstFlag == false)
            {
                oPdfPTable = PDFTableDeclaration(table_header);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "Grand Total", 0, 15, Element.ALIGN_CENTER, BaseColor.WHITE, true, 10, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGross, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nAttBonus, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nGrossEarning, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nBasics, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nHR, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nConv, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nFood, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nMed, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nAbsentAmount, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);


                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nStemp, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nTotalDeduction, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nNetAmount, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBoldNet);

                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(dTBank, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                //ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(dTCash, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                ESimSolItexSharp.SetCellValue(ref oPdfPTable, "", 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);
                ESimSolItexSharp.SetCellValue(ref oPdfPTable, Global.MillionFormat(nNetAmount, 0), 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 20, fontBold);

                oPdfPTable.CompleteRow();
            }

            ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            #endregion
        }
        #endregion

    }
}
