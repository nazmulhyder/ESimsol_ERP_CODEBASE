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

namespace ESimSol.Reports
{
    public class rptPaySlipF03
    {


        #region Declaration
        PdfWriter _oWriter;
        Document _oDocument;
        string DateRange = "";
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        List<EmployeeSalaryV2> _oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
        List<EmployeeSalaryV2> _oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
        List<LeaveHead> _oLeaveHeads = new List<LeaveHead>();
        List<EmployeeSalaryDetailV2> _oEmployeeSalaryBasics = new List<EmployeeSalaryDetailV2>();
        List<EmployeeSalaryDetailV2> _oEmployeeSalaryAllowances = new List<EmployeeSalaryDetailV2>();
        List<EmployeeSalaryDetailV2> _oEmployeeSalaryDeductions = new List<EmployeeSalaryDetailV2>();
        DateTime _StartDate = DateTime.Now;
        string _sStartDate = "";
        string _sEndDate = "";
        #endregion

        public byte[] PrepareReport(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<LeaveHead> oLeaveHeads, List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, List<EmployeeSalaryDetailV2> oEmployeeSalaryAllowances, List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions, Company oCompany, SalaryField oSalaryField, EnumPageOrientation ePageOrientation, List<SalarySheetSignature> oSalarySheetSignatures, string StartDate, string EndDate, bool IsOT)
        {
            _oCompany = oCompany;
            _oEmployeeSalaryV2s = oEmployeeSalaryV2s;
            _oLeaveHeads = oLeaveHeads;
            _oEmployeeSalaryBasics = oEmployeeSalaryBasics;
            _oEmployeeSalaryAllowances = oEmployeeSalaryAllowances;
            _oEmployeeSalaryDeductions = oEmployeeSalaryDeductions;
            _StartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            _sStartDate = _StartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");
            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4); //595X842
            _oDocument.SetMargins(20f, 20f, 5f, 20f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oWriter.ViewerPreferences = PdfWriter.PageModeUseOutlines;
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.PrintDocumentGenerator = true;
            PageEventHandler.PrintPrintingDateTime = true;
            _oWriter.PageEvent = PageEventHandler; //Footer print with page event handeler            
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 802 });
            #endregion

            #region Report Body & Header
            this.PrintBody();
            #endregion
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Body
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            _oEmployeeSalaryV2s = _oEmployeeSalaryV2s.OrderBy(x => x.EmployeeCode).ToList();
            foreach (EmployeeSalaryV2 oItem in _oEmployeeSalaryV2s)
            {
                _oPdfPCell = new PdfPCell(PrintHeader(oItem)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(PrintBodyConfiden(oItem)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oPdfPCell = new PdfPCell(PrintBodyDetail(oItem)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                

                #region New Page Declare
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();
                #endregion

            }


        }
        #endregion

        #region Report Header
        private PdfPTable PrintHeader(EmployeeSalaryV2 oEmployeeSalaryV2)
        {
            #region Company & Report Header
            PdfPCell oPdfPCell = null;
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 100f, 450f, 250f });

            #region Company Name & Report Header
           
          
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle)); oPdfPCell.Colspan = 3;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion

            #region Company Address
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle)); oPdfPCell.Colspan = 3;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
            oPdfPCell = new PdfPCell(new Phrase("Pay Slip", _oFontStyle)); oPdfPCell.Colspan = 3;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("For The Month of "+_StartDate.ToString("MMMM")+", "+_StartDate.Year, _oFontStyle)); oPdfPCell.Colspan = 3;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            #endregion
            return oPdfPTable;
        }
        #endregion

        #region Report Body Confiden
        private PdfPTable PrintBodyConfiden(EmployeeSalaryV2 oEmployeeSalaryV2)
        {
            #region Company & Report Header
            PdfPCell oPdfPCell = null;
            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] {50f, 6f, 250f });

            #region Company Name & Report Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);

            oPdfPCell = new PdfPCell(new Phrase("CONFIDENTIAL", _oFontStyle)); oPdfPCell.Colspan = 3;
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #endregion

            #region Employee Code
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Employee Code", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase(oEmployeeSalaryV2.EmployeeCode, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Employee Name
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Employee Name", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oEmployeeSalaryV2.EmployeeName, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Designation
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oEmployeeSalaryV2.DesignationName, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Date Of Joining
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Date of Joining", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oEmployeeSalaryV2.JoiningDate.ToString("dd MMM yyyy"), _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region Location
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Location", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oEmployeeSalaryV2.LocationName, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            #region ETIN
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("ETIN", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(oEmployeeSalaryV2.ETIN, _oFontStyle));
            oPdfPCell.Border = 0; oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion
            #endregion
            return oPdfPTable;
        }
        #endregion

        #region Report BodyDetail
        private PdfPTable PrintBodyDetail(EmployeeSalaryV2 oEmployeeSalaryV2)
        {
            #region Company & Report Header
            PdfPCell oPdfPCell = null;
            PdfPTable oPdfPTable = new PdfPTable(4);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.SetWidths(new float[] { 50f, 50f, 50f,50f });

            #region Header
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Earnings", _oFontStyle)); oPdfPCell.Colspan = 2;
             oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Deductions", _oFontStyle)); oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Heads", _oFontStyle)); oPdfPCell.BorderWidthTop= 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Amount(BDT)", _oFontStyle)); oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthRight = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Heads", _oFontStyle)); oPdfPCell.BorderWidthTop = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Amount(BDT)", _oFontStyle)); oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion

            #region ReportData
            int nDeductionRow = 0;
            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                double nBasic = oEmployeeSalaryV2.EmployeeSalaryBasics.Where(x => x.SalaryHeadID == oEmpBasics.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                oPdfPCell = new PdfPCell(new Phrase(oEmpBasics.SalaryHeadName, _oFontStyle)); oPdfPCell.BorderWidthTop = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(nBasic.ToString("#,##0.00"), _oFontStyle)); oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthRight = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(getDeducHead(nDeductionRow, oEmployeeSalaryV2.EmployeeSalaryDeductions), _oFontStyle)); oPdfPCell.BorderWidthTop = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(getDeducAmount(nDeductionRow, oEmployeeSalaryV2.EmployeeSalaryDeductions)>0?getDeducAmount(nDeductionRow, oEmployeeSalaryV2.EmployeeSalaryDeductions).ToString("#,##0.00"):getDeducAmount(nDeductionRow, oEmployeeSalaryV2.EmployeeSalaryDeductions).ToString("#,###"), _oFontStyle)); oPdfPCell.BorderWidthLeft = 0; oPdfPCell.BorderWidthTop = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                nDeductionRow++;
            }

            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryAllowances)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                double nBasic = oEmployeeSalaryV2.EmployeeSalaryAllowances.Where(x => x.SalaryHeadID == oEmpBasics.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                oPdfPCell = new PdfPCell(new Phrase(oEmpBasics.SalaryHeadName, _oFontStyle)); oPdfPCell.BorderWidthTop = 0; 
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(nBasic.ToString("#,##0.00"), _oFontStyle)); oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthLeft = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(getDeducHead(nDeductionRow, oEmployeeSalaryV2.EmployeeSalaryDeductions), _oFontStyle)); oPdfPCell.BorderWidthTop = 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

                oPdfPCell = new PdfPCell(new Phrase(getDeducAmount(nDeductionRow, oEmployeeSalaryV2.EmployeeSalaryDeductions) > 0 ? getDeducAmount(nDeductionRow, oEmployeeSalaryV2.EmployeeSalaryDeductions).ToString("#,##0.00") : getDeducAmount(nDeductionRow, oEmployeeSalaryV2.EmployeeSalaryDeductions).ToString("#,###"), _oFontStyle));
                oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft= 0;
                oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
                oPdfPTable.CompleteRow();
                nDeductionRow++;
            }

            #region TotalEarnings
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase("Total Earnings", _oFontStyle)); oPdfPCell.BorderWidthTop = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            double nTotalEarnings = oEmployeeSalaryV2.EmployeeSalaryBasics.Sum(x => x.Amount)+oEmployeeSalaryV2.EmployeeSalaryAllowances.Sum(x=>x.Amount);
            oPdfPCell = new PdfPCell(new Phrase(Math.Round(nTotalEarnings).ToString("#,##0"), _oFontStyle)); oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthRight = 0; oPdfPCell.BorderWidthLeft = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);


            oPdfPCell = new PdfPCell(new Phrase("Total Deductions", _oFontStyle)); oPdfPCell.BorderWidthTop = 0; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            double nTotalDeductions = oEmployeeSalaryV2.EmployeeSalaryDeductions.Sum(x => x.Amount);
            oPdfPCell = new PdfPCell(new Phrase(Math.Round(nTotalDeductions).ToString("#,##0"), _oFontStyle)); oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Colspan = 4; oPdfPCell.Border = 0; oPdfPCell.FixedHeight = 30;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            #region Net Payable
            oPdfPCell = new PdfPCell(new Phrase("Net Payable", _oFontStyle)); oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            double nNetPayable = nTotalEarnings - nTotalDeductions;
            oPdfPCell = new PdfPCell(new Phrase(Math.Round(nNetPayable).ToString("#,##0"), _oFontStyle)); oPdfPCell.Colspan = 2;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell.Colspan = 4; oPdfPCell.Border = 0; oPdfPCell.FixedHeight = 30;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("MODE OF PAYMENT", _oFontStyle)); oPdfPCell.Colspan = 4;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            string sModeMsg = "";
            if(oEmployeeSalaryV2.AccountNo!="")
            {
                sModeMsg = "Through Bank Account:(" + oEmployeeSalaryV2.AccountNo + ")";
            }
            else
            {
                sModeMsg = "Through Cash";
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
            oPdfPCell = new PdfPCell(new Phrase(sModeMsg, _oFontStyle)); oPdfPCell.Colspan = 2; oPdfPCell.BorderWidthTop = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCell = new PdfPCell(new Phrase(Math.Round(nNetPayable).ToString("#,##0"), _oFontStyle)); oPdfPCell.Colspan = 2; oPdfPCell.BorderWidthTop = 0; oPdfPCell.BorderWidthLeft = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);
            oPdfPTable.CompleteRow();
            #endregion
            #endregion

            #endregion
            return oPdfPTable;
        }

        private string getDeducHead(int nStartIndex, List<EmployeeSalaryDetailV2> EmployeeSalaryDeductions)
        {
            if (nStartIndex <= EmployeeSalaryDeductions.Count() - 1)
            {

                return EmployeeSalaryDeductions[nStartIndex].SalaryHeadName;
            }
            else
            {
                return "";
            }
               
        }

        private double getDeducAmount(int nStartIndex, List<EmployeeSalaryDetailV2> EmployeeSalaryDeductions)
        {
            if (nStartIndex <= EmployeeSalaryDeductions.Count() - 1)
            {

                return EmployeeSalaryDeductions[nStartIndex].Amount;
            }
            else
            {
                return 0f;
            }

        }
        #endregion
    }
}
