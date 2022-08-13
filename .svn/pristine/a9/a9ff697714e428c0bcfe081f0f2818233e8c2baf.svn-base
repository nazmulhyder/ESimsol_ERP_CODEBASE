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

   
    public class rptSalarySheetF04
    {
        #region Declaration
        private int _nColumn = 1;
        private Document _oDocument;
        private iTextSharp.text.Font _oFontStyle;
        private PdfPTable _oPdfPTable = new PdfPTable(1);
        private PdfPCell _oPdfPCell;
        PdfPCell otempPdfPCell;
        private iTextSharp.text.Image _oImag;
        private MemoryStream _oMemoryStream = new MemoryStream();
        List<EmployeeSalaryV2> _oAMGSalarySheets = new List<EmployeeSalaryV2>();
        private Company _oCompany = new Company();
        string _salaryMonth = string.Empty;
        DateTime _StartDate = DateTime.Now;
        #endregion

        public byte[] PrepareReport(List<EmployeeSalaryV2> oAMGSalarySheets, Company oCompany, string salaryMonth,DateTime StartDate)
        {
            _oAMGSalarySheets = oAMGSalarySheets;
            _oCompany = oCompany;
            _salaryMonth = salaryMonth;
            _StartDate = StartDate;
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

            PdfPCell oPdfPCell;
            iTextSharp.text.Font fontBoldHeader = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
          //  ESimSolItexSharp.SetCellValue(ref oPdfPTable, sTitle, 0, oPdfPTable.NumberOfColumns, Element.ALIGN_LEFT, BaseColor.WHITE, false, 0, 0, fontBoldHeader);
            oPdfPCell = new PdfPCell(new Phrase(sTitle, fontBoldHeader)); oPdfPCell.Colspan = oPdfPTable.NumberOfColumns; oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            iTextSharp.text.Font fontBoldNormal = FontFactory.GetFont("Tahoma", 5f, iTextSharp.text.Font.BOLD);
            #region Column Print
           
            foreach (TableHeader listItem in table_header)
            {

                oPdfPCell = new PdfPCell(new Phrase(listItem.Header, fontBoldNormal)); oPdfPCell.Rotation = ((listItem.IsRotate) ? 90 : 0);
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

              //  ESimSolItexSharp.SetCellValue(ref oPdfPTable, listItem.Header, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, true, 0, ((listItem.IsRotate) ? 90 : 0), fontBoldNormal);
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
            PdfPCell oPdfPCell;


            #region Body Print
            float cellHeight = 60f;
            int nCount = 0;
            int nBreakCount = 0;
            _oAMGSalarySheets = _oAMGSalarySheets.OrderBy(x => x.BUName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
            var data = _oAMGSalarySheets.GroupBy(x => new { x.BUName, x.LocationName, x.DepartmentName }, (key, grp) => new
            {
                BUName = key.BUName,
                LocName = key.LocationName,
                DptName = key.DepartmentName,
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


                    oPdfPCell = new PdfPCell(new Phrase((++nCount).ToString("#,##0"), fontNormal));oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                 
                    oPdfPCell = new PdfPCell(new Phrase(obj.EmployeeCode, fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

              
                    oPdfPCell = new PdfPCell(new Phrase(obj.EmployeeName, fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(obj.DesignationName, fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(obj.JoiningDate.ToString("dd MMM yyyy"), fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    int days = DateTime.DaysInMonth(_StartDate.Year, _StartDate.Month);
                    if(obj.JoiningDate>_StartDate)
                    {
                        days = days - Convert.ToInt32(obj.JoiningDate.ToString("dd"));
                    }
                 
                    oPdfPCell = new PdfPCell(new Phrase(days.ToString(), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    int NTotalLeave = (obj.CL + obj.ML + obj.EL + obj.LWP);
                    
                    int nTPresentDays = days - (obj.TotalDayOff + obj.TotalHoliday + NTotalLeave + obj.TotalAbsent);
                  
                    oPdfPCell = new PdfPCell(new Phrase(nTPresentDays.ToString(), fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase((obj.TotalDayOff + obj.TotalHoliday).ToString(), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(obj.TotalAbsent.ToString(), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                

                    oPdfPCell = new PdfPCell(new Phrase(obj.CL.ToString(), fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(obj.ML.ToString(), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

    
                    oPdfPCell = new PdfPCell(new Phrase(obj.EL.ToString(), fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(obj.LWP.ToString(), fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase((obj.CL + obj.ML + obj.EL + obj.LWP).ToString(), fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    int nTWorkingDay = nTPresentDays + obj.TotalDayOff + obj.TotalHoliday + NTotalLeave - obj.LWP;
               

                    oPdfPCell = new PdfPCell(new Phrase(nTWorkingDay.ToString(), fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(obj.GrossAmount.ToString(("#,###")), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(obj.AttendanceBonus.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    double GrossEarnings = obj.BasicAmount + obj.HouseRentAmount + obj.ConveyanceAmount + obj.FoodAmount + obj.MedicalAmount;

                    oPdfPCell = new PdfPCell(new Phrase(GrossEarnings.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(obj.BasicAmount.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(obj.HouseRentAmount.ToString("#,##0"), fontNormal));  oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(obj.ConveyanceAmount.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(obj.FoodAmount.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(obj.MedicalAmount.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(obj.AbsentAmount.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(obj.StampAmount.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

          
                    double nTotalDeductionAmt = obj.StampAmount + obj.AbsentAmount;

                    oPdfPCell = new PdfPCell(new Phrase(nTotalDeductionAmt.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(obj.NetAmount.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);



                    if (string.IsNullOrEmpty(obj.AccountNo))
                    {

                        oPdfPCell = new PdfPCell(new Phrase("", fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);



                        oPdfPCell = new PdfPCell(new Phrase(obj.NetAmount.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    }
                    else
                    {


                        oPdfPCell = new PdfPCell(new Phrase(obj.NetAmount.ToString("#,##0"), fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                        oPdfPCell = new PdfPCell(new Phrase("", fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                        oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    }

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


                    oPdfPCell = new PdfPCell(new Phrase(obj.AccountNo, fontNormal));oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);



                    oPdfPCell = new PdfPCell(new Phrase(obj.BankName, fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase("", fontNormal)); oPdfPCell.FixedHeight = cellHeight;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPTable.CompleteRow();


                    if (nDeptTotal == deptCounter)
                    {
                        deptCounter = 0;
                        continue;
                    }
                    if (nCount % 7 != 0 && flag)
                    {
                        otempPdfPCell = new PdfPCell(oPdfPTable);
                        otempPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        otempPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        otempPdfPCell.Border = 0;
                        otempPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(otempPdfPCell);
                        _oPdfPTable.CompleteRow();
                        //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

                        if (lstFlag)
                        {
                            oPdfPCell = new PdfPCell(new Phrase("Total", fontBold)); oPdfPCell.FixedHeight = 10; oPdfPCell.Colspan = 15;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.GrossAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20; 
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nGross += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.GrossAmount);

                            oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AttendanceBonus).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nAttBonus += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AttendanceBonus);
                            double nSubTotalEarnings = oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.BasicAmount + x.HouseRentAmount + x.ConveyanceAmount + x.FoodAmount + x.MedicalAmount);
                   

                            oPdfPCell = new PdfPCell(new Phrase(nSubTotalEarnings.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nGrossEarning += nSubTotalEarnings;

                            oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.BasicAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nBasics += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.BasicAmount);
                          

                            oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.HouseRentAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nHR += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.HouseRentAmount);
                    
                            oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.ConveyanceAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nConv += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.ConveyanceAmount);
                    
                            oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.FoodAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nFood += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.FoodAmount);
                    
                            oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.MedicalAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nMed += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.MedicalAmount);
                   
                            oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AbsentAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nAbsentAmount += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AbsentAmount);


                            oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.StampAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nStemp += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.StampAmount);
                            double SubTotalDeduc = oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AbsentAmount + x.StampAmount);
            

                            oPdfPCell = new PdfPCell(new Phrase(SubTotalDeduc.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nTotalDeduction += SubTotalDeduc;
                

                            oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.NetAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            nNetAmount += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.NetAmount);



                            oPdfPCell = new PdfPCell(new Phrase(sBank.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(sCash.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPTable.CompleteRow();


                            oPdfPCell = new PdfPCell(new Phrase("Grand Total", fontBold)); oPdfPCell.FixedHeight = 10; oPdfPCell.Colspan = 15;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

         
                            oPdfPCell = new PdfPCell(new Phrase(nGross.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(nAttBonus.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(nGrossEarning.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(nBasics.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(nHR.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(nConv.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(nFood.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(nMed.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(nAbsentAmount.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(nStemp.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(nTotalDeduction.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(nNetAmount.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                            oPdfPCell = new PdfPCell(new Phrase(dTBank.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                            oPdfPCell = new PdfPCell(new Phrase(dTCash.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                            oPdfPTable.CompleteRow();
                            lstFlag = true;
                        }
                    }
                    if (nCount % 7 == 0 && nCount!=0)
                    {
                         otempPdfPCell = new PdfPCell(oPdfPTable);
                        otempPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        otempPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        otempPdfPCell.Border = 0;
                        otempPdfPCell.BackgroundColor = BaseColor.WHITE;
                        _oPdfPTable.AddCell(otempPdfPCell);
                        _oPdfPTable.CompleteRow();
                       // ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();
                    }
                }
                if (lstFlag == false)
                {

                    oPdfPCell = new PdfPCell(new Phrase("Total", fontBold)); oPdfPCell.FixedHeight = 10; oPdfPCell.Colspan = 15;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.GrossAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nGross += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.GrossAmount);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AttendanceBonus).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nAttBonus += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AttendanceBonus);
                    double nSubTotalEarnings = oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.BasicAmount + x.HouseRentAmount + x.ConveyanceAmount + x.FoodAmount + x.MedicalAmount);


                    oPdfPCell = new PdfPCell(new Phrase(nSubTotalEarnings.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nGrossEarning += nSubTotalEarnings;

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.BasicAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nBasics += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.BasicAmount);


                    oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.HouseRentAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nHR += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.HouseRentAmount);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.ConveyanceAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nConv += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.ConveyanceAmount);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.FoodAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nFood += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.FoodAmount);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.MedicalAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nMed += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.MedicalAmount);

                    oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AbsentAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nAbsentAmount += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AbsentAmount);


                    oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.StampAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nStemp += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.StampAmount);
                    double SubTotalDeduc = oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.AbsentAmount + x.StampAmount);


                    oPdfPCell = new PdfPCell(new Phrase(SubTotalDeduc.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nTotalDeduction += SubTotalDeduc;


                    oPdfPCell = new PdfPCell(new Phrase(oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.NetAmount).ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    nNetAmount += oItem.Results.Where(x => x.DepartmentID == nDeptID).Sum(x => x.NetAmount);



                    oPdfPCell = new PdfPCell(new Phrase(sBank.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                    oPdfPCell = new PdfPCell(new Phrase(sCash.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                    oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                    oPdfPTable.CompleteRow();
                } 
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();

                nCount = 0;

                otempPdfPCell = new PdfPCell(oPdfPTable);
                otempPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                otempPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                otempPdfPCell.Border = 0;
                otempPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(otempPdfPCell);
                _oPdfPTable.CompleteRow();
                //ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);

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

                oPdfPCell = new PdfPCell(new Phrase("Grand Total", fontBold)); oPdfPCell.FixedHeight = 10; oPdfPCell.Colspan = 15;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nGross.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nAttBonus.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nGrossEarning.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nBasics.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nHR.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nConv.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nFood.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(nMed.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nAbsentAmount.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nStemp.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nTotalDeduction.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


                oPdfPCell = new PdfPCell(new Phrase(nNetAmount.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(dTBank.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(dTCash.ToString("#,##0"), fontBold)); oPdfPCell.FixedHeight = 20;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
            }

            otempPdfPCell = new PdfPCell(oPdfPTable);
            otempPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            otempPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            otempPdfPCell.Border = 0;
            otempPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(otempPdfPCell);
            _oPdfPTable.CompleteRow();
           // ESimSolItexSharp.PushTableInCell(ref _oPdfPTable, oPdfPTable, 0, 0, Element.ALIGN_CENTER, BaseColor.WHITE, false, true);
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            #endregion
        }
        #endregion

    }
}
