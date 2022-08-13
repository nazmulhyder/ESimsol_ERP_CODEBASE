using System;
using System.Data;
using ESimSol.BusinessObjects;
using ICS.Core;
using System.Linq;
using ICS.Core.Utility;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.Text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;


namespace ESimSol.Reports
{

    public class rptIncrementApprisal
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(32);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        IncrementApprisal _oIncrementApprisal = new IncrementApprisal();
        List<IncrementApprisal> _oIncrementApprisals = new List<IncrementApprisal>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        Company _oCompany = new Company();
        double _nSumBankAmount = 0;
        string _sMessage = "";
        DateTime _UpToDate = DateTime.Now;
        int _nColumns;

        #endregion

        public byte[] PrepareReport(IncrementApprisal oIncrementApprisal, DateTime UpToDate)
        {
            _oIncrementApprisal = oIncrementApprisal;
            _oIncrementApprisals = oIncrementApprisal.IncrementApprisals;
            _oBusinessUnits = oIncrementApprisal.BusinessUnits;
            _oCompany = oIncrementApprisal.Company;
            _UpToDate = UpToDate;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());//842*595
            _oDocument.SetMargins(5f, 5f, 5f, 30f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = new List<string> { "Executive Director" };
            PageEventHandler.PrintPrintingDateTime = false;
            //PageEventHandler.nFontSize = 15;
            oPDFWriter.PageEvent = PageEventHandler;
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 40f
                                              , 70f
                                              , 65f
                                              , 60f
                                              , 27f
                                              , 27f
                                              , 27f
                                              , 27f
                                              , 40f
                                              , 30f
                                              , 18
                                              , 18f
                                              , 18f
                                              , 18f
                                              , 18f
                                              , 18f
                                              , 18f
                                              , 18f
                                              , 17f
                                              , 17f
                                              , 17f
                                              , 17f
                                              , 22f
                                              , 17f
                                              , 17f
                                              , 17f
                                              , 17f
                                              , 17f
                                              , 27f
                                              , 17f
                                              , 27f
                                              , 29f});
            _nColumns = 32;
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(int nBusinessUnitID)
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 200f, 230f });
            PdfPCell oPdfPCellHearder;

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = _oBusinessUnits.Where(x => x.BusinessUnitID == nBusinessUnitID).ToList();

            _oFontStyle = FontFactory.GetFont("Tahoma", 30f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oBusinessUnits.Count > 0 ? oBusinessUnits[0].Name : "", _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);


            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oBusinessUnits.Count > 0 ? oBusinessUnits[0].Address : "", _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);


            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Yearly Increment Sheet " + _UpToDate.ToString("MMMM") + " - " + _UpToDate.ToString("yyyy"), _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            if (_oIncrementApprisals.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Nothing to Print!"));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                List<IncrementApprisal> oIncrementApprisals = new List<IncrementApprisal>();
                _oIncrementApprisals.ForEach(x => oIncrementApprisals.Add(x));
                while (oIncrementApprisals.Count > 0)
                {
                    //List<EmployeeSalary> oTempEmployeeSs = new List<EmployeeSalary>();
                    var oResults = oIncrementApprisals.Where(x => x.LocationID == oIncrementApprisals[0].LocationID && x.DepartmentID == oIncrementApprisals[0].DepartmentID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    
                    PrintHeader(oResults[0].BusinessUnitID);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                    this.SetCellValue("Department Name : " + oResults.FirstOrDefault().DepartmentName, 0, _nColumns, Element.ALIGN_LEFT, 0, 30f, false);
                    _oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    this.ColumnSetup();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                    this.PrintIncrementApprisal(oResults);
                    oIncrementApprisals.RemoveAll(x => x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                    //}
                }
            }
        }
        private void PrintIncrementApprisal(List<IncrementApprisal> oIncrementApprisals)
        {
            int nHeight = 55;
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
            int rowCount = 0;

            foreach (IncrementApprisal oItem in oIncrementApprisals)
            {
                rowCount++;
                _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                this.SetCellValue(oItem.EmployeeCode, 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue(oItem.EmployeeName, 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue(oItem.DesignationName, 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue(oItem.JoiningDateInString, 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue(Global.MillionFormat(oItem.BeforeIncrement).ToString(), 0, 0, Element.ALIGN_CENTER, 1, nHeight, true);
                this.SetCellValue(oItem.BeforeEffectDateInString, 0, 0, Element.ALIGN_CENTER, 1, nHeight,true);
                this.SetCellValue(Global.MillionFormat(oItem.RecentIncrement).ToString(), 0, 0, Element.ALIGN_CENTER, 1, nHeight, true);
                
                //2nd increment is recent 
                this.SetCellValue(oItem.RecentEffectDateInString, 0, 0, Element.ALIGN_CENTER, 1, nHeight, true);
                this.SetCellValue(Global.MillionFormat(oItem.PresentSalary).ToString(), 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue(oItem.Education, 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue(oItem.TotalLate.ToString(), 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue(oItem.TotalLeave.ToString(), 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue(oItem.TotalAbsent.ToString(), 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue(oItem.AttendancePercent.ToString(), 0, 0, Element.ALIGN_CENTER, 1, nHeight, true);
                this.SetCellValue(oItem.Warning.ToString(), 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);
                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, nHeight, false);

                if (rowCount % 6 == 0)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    this.PrintHeader(oItem.BusinessUnitID);

                    this.SetCellValue("Department Name : " + oItem.DepartmentName, 0, _nColumns, Element.ALIGN_LEFT, 0, 30f, false);
                    _oPdfPTable.CompleteRow();
                    _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);
                    this.ColumnSetup();
                }
            }
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();

        }
        private void ColumnSetup()
        {
            int nHeight = 70;
            int nHeight1 = 50;
            _oFontStyle = FontFactory.GetFont("Tahoma", 6f, iTextSharp.text.Font.NORMAL);

            _oPdfPCell = new PdfPCell(new Phrase("EmployeeID.", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("JoiningDate", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("1st Increment", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("1st Increment Month", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("2nd Increment", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("2nd Increment Month", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Present Salary", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Education", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Last 1 Year", _oFontStyle)); _oPdfPCell.Colspan = 5; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Performance Numbering System (High=80-100, Medium=60-79, Normal=41-59, Low=up to 40)", _oFontStyle)); _oPdfPCell.Colspan = 8; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Average Marks", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Efficency(If Any)", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Performance Grade", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Proposed Increment Amount", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Board Approved Increment Amount", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Deduction(Seam Loss, Sea/Hot/Shot,Shipment, D.A)", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Final Approval", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Salary After Increment", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Promotion(If Any)", _oFontStyle)); _oPdfPCell.Rowspan = 2; _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Late", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Leave", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Absent", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Attendance %", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Warning", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Working Efficency Status", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Willing to the works", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cooperation", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Intelligence and Strategy", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Reliability", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Behavior", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Discipline and Obeys", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Congrolling Ability Subordinates", _oFontStyle)); _oPdfPCell.Rotation = 90; _oPdfPCell.FixedHeight = nHeight1;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void SetCellValue(string sName, int nRowSpan, int nColumnSpan, int align, int border, float height, bool IsRotate)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            _oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            _oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            if (height > 0)
                _oPdfPCell.FixedHeight = height;
            if (border == 0)
                _oPdfPCell.Border = 0;
            if (IsRotate)
            {
                _oPdfPCell.Rotation = 90;
            }
            _oPdfPCell.HorizontalAlignment = align; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        }
        #endregion       
    }
}

