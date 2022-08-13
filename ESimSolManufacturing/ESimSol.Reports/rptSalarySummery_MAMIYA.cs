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
    public class rptSalarySummery_MAMIYA
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(34);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        List<EmployeeSalary_MAMIYA> _oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();

        string _sStartDate = "";
        string _sEndDate = "";

        #endregion

        public byte[] PrepareReport(EmployeeSalary_MAMIYA oEmployeeSalary_MAMIYA)
        {
            _oEmployeeSalary_MAMIYAs = oEmployeeSalary_MAMIYA.EmployeeSalary_MAMIYAs;

            _oCompany = oEmployeeSalary_MAMIYA.Company;
            DateTime sStartDate = Convert.ToDateTime(oEmployeeSalary_MAMIYA.ErrorMessage.Split(',')[0]);
            DateTime sEndDate = Convert.ToDateTime(oEmployeeSalary_MAMIYA.ErrorMessage.Split(',')[1]);
            _sStartDate = sStartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");

            #region Page Setup
            _oDocument = new Document(new iTextSharp.text.Rectangle(1600, 1600), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 20f, 85f, 50f, 50f, 50f, 45f, 45f, 55f, 40f, 40f, 40f, 50f, 40f, 40f, 40f, 40f, 50f, 45f, 40f, 55f, 45f, 45f, 45f, 45f, 45f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 45f, 55f });
            #endregion

            this.PrintHeader();
            this.PrintBody();
            _oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 190f, 260f });
            PdfPCell oPdfPCellHearder;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(180f, 45f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.FixedHeight = 35;
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 30f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 35;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = 34;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Salary Summary", _oFontStyle));
            _oPdfPCell.Colspan = 34;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = 34;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = 34;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion

        #region Report Body
        private void PrintHaedRow()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("No Of Emp.", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("BASIC", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("H RENT", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("MED", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("CONV.", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GROSS SALARY", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ABSENT HR", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TOTAL ABSENT AMOUNT", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("EARNED PAY", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("SHIFT AMT", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OVERTIME", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ATT BONUS", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ADJ CR", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OTH.", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("FB", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("M. Bill", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Holiday All.", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("GROSS PAY", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DEUCTION", _oFontStyle)); _oPdfPCell.Colspan = 9;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("NET PAY", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Sick", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("W Pay", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("N HR", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("H HR", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("F HR", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("PF", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TRNS", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("DORM", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ADV", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ADJ DR", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("STAMP", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("P LOAN", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("TAX", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }

        int nCount = 0;
        double nBAsic_GT = 0;
        double nHRent_GT = 0;
        double nMed_GT = 0;
        double nCONV_GT = 0;
        double nGrossSalary_GT = 0;
        double nAbsentHr_Sick_GT = 0;
        double nAbsentHr_WOPay_GT = 0;
        double nTotalAbsentAmount_GT = 0;
        double nEarnedPay_GT = 0;
        double nShiftAmount_GT = 0;
        double nOT_NHR_GT = 0;
        double nOT_HHR_GT = 0;
        double nFHOT_GT = 0;
        double nOTAmount_GT = 0;
        double nAttendanceBonus_GT = 0;
        double nADJCR_GT = 0;
        double nOTHER_GT = 0;
        double nFB_GT = 0;
        double nMBill_GT = 0;
        double nHolidayAll_GT = 0;
        double nGrossPay_GT = 0;
        double nPF_GT = 0;
        double nTRNS_GT = 0;
        double nDORM_GT = 0;
        double nADV_GT = 0;
        double nADJDR_GT = 0;
        double nRS_GT = 0;
        double nPLoan_GT = 0;
        double nTAX_GT = 0;
        double nDeductionTotal_GT = 0;

        //double nFracretained_GT = 0;

        double nNetPay_GT = 0;
   
        private void PrintBody()
        {
            _oEmployeeSalary_MAMIYAs = _oEmployeeSalary_MAMIYAs.OrderBy(x => x.DepartmentName).ToList();
            PrintSalarySheet(_oEmployeeSalary_MAMIYAs);
            GT();
            Footer();
        }

        public void PrintSalarySheet(List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs)
        {
            nCount = 0;
            PrintHaedRow();
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);

            foreach (EmployeeSalary_MAMIYA oEmpSalaryItem in oEmployeeSalary_MAMIYAs)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
               
                _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.DepartmentName, _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.NoOfEmp.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                if (oEmpSalaryItem.Basic > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.Basic), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nBAsic_GT += oEmpSalaryItem.Basic;

                if (oEmpSalaryItem.HRent > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.HRent), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nHRent_GT += oEmpSalaryItem.HRent;
             

                if (oEmpSalaryItem.Med > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.Med), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nMed_GT += oEmpSalaryItem.Med;

                if (oEmpSalaryItem.Conveyance > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.Conveyance), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nCONV_GT += oEmpSalaryItem.Conveyance;


                if (oEmpSalaryItem.GrossSalary > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.GrossSalary), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nGrossSalary_GT += oEmpSalaryItem.GrossSalary;
              

                if (oEmpSalaryItem.AbsentHr_Sick > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.AbsentHr_Sick), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nAbsentHr_Sick_GT += oEmpSalaryItem.AbsentHr_Sick;
             

                if (oEmpSalaryItem.AbsentHr_WOPay > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.AbsentHr_WOPay), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nAbsentHr_WOPay_GT += oEmpSalaryItem.AbsentHr_WOPay;
             

                if (oEmpSalaryItem.TotalAbsentAmount > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.TotalAbsentAmount), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nTotalAbsentAmount_GT += oEmpSalaryItem.TotalAbsentAmount;
               

                if (oEmpSalaryItem.EarnedPay > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.EarnedPay), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nEarnedPay_GT += oEmpSalaryItem.EarnedPay;
              

                if (oEmpSalaryItem.ShiftAmount > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.ShiftAmount), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nShiftAmount_GT += oEmpSalaryItem.ShiftAmount;
                

                if (oEmpSalaryItem.OT_NHR_HR > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.OT_NHR_HR), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nOT_NHR_GT += oEmpSalaryItem.OT_NHR_HR;
              

                if (oEmpSalaryItem.OT_HHR_HR > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.OT_HHR_HR), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nOT_HHR_GT += oEmpSalaryItem.OT_HHR_HR;

                if (oEmpSalaryItem.FHOT > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.FHOT_HR), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nFHOT_GT += oEmpSalaryItem.FHOT_HR;

                if (oEmpSalaryItem.OTAmount > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.OTAmount), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nOTAmount_GT += oEmpSalaryItem.OTAmount;
                

                if (oEmpSalaryItem.AttendanceBonus > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.AttendanceBonus), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nAttendanceBonus_GT += oEmpSalaryItem.AttendanceBonus;
              

                if (oEmpSalaryItem.ADJCR > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.ADJCR), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nADJCR_GT += oEmpSalaryItem.ADJCR;

                if (oEmpSalaryItem.OtherAll > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.OtherAll), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nOTHER_GT += oEmpSalaryItem.OtherAll;

                if (oEmpSalaryItem.FB > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.FB), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nFB_GT += oEmpSalaryItem.FB;

                if (oEmpSalaryItem.MobileBill > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.MobileBill), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nMBill_GT += oEmpSalaryItem.MobileBill;

                if (oEmpSalaryItem.HolidayAll > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.HolidayAll), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nHolidayAll_GT += oEmpSalaryItem.HolidayAll;

                if (oEmpSalaryItem.GrossPay > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.GrossPay), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nGrossPay_GT += oEmpSalaryItem.GrossPay;
               

                if (oEmpSalaryItem.PF > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.PF), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nPF_GT += oEmpSalaryItem.PF;
           

                if (oEmpSalaryItem.TRNS > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.TRNS), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nTRNS_GT += oEmpSalaryItem.TRNS;
            

                if (oEmpSalaryItem.DORM > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.DORM), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nDORM_GT += oEmpSalaryItem.DORM;
          

                if (oEmpSalaryItem.ADV > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.ADV), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nADV_GT += oEmpSalaryItem.ADV;
              

                if (oEmpSalaryItem.ADJDR > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.ADJDR), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nADJDR_GT += oEmpSalaryItem.ADJDR;
               

                if (oEmpSalaryItem.RS > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.RS), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nRS_GT += oEmpSalaryItem.RS;
               

                if (oEmpSalaryItem.PLoan > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.PLoan), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nPLoan_GT += oEmpSalaryItem.PLoan;

                if (oEmpSalaryItem.IncomeTax > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.IncomeTax), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nTAX_GT += oEmpSalaryItem.IncomeTax;

                if (oEmpSalaryItem.DeductionTotal > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.DeductionTotal), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nDeductionTotal_GT += oEmpSalaryItem.DeductionTotal;
               
                if (oEmpSalaryItem.NetPay > 0)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.NetPay), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                else
                {
                    _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                nNetPay_GT += oEmpSalaryItem.NetPay;

                _oPdfPTable.CompleteRow();

            }
        }

        public void GT()//Grand Total
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 30; _oPdfPCell.Colspan = 34; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("Grand Total :", _oFontStyle)); _oPdfPCell.Colspan = 3; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            if (nBAsic_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nBAsic_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nHRent_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nHRent_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nMed_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nMed_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nCONV_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nCONV_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nGrossSalary_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrossSalary_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nAbsentHr_Sick_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAbsentHr_Sick_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nAbsentHr_WOPay_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAbsentHr_WOPay_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }


            if (nTotalAbsentAmount_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalAbsentAmount_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nEarnedPay_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nEarnedPay_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nShiftAmount_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nShiftAmount_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nOT_NHR_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nOT_NHR_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nOT_HHR_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nOT_HHR_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nFHOT_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nFHOT_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nOTAmount_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nOTAmount_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nAttendanceBonus_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nAttendanceBonus_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nADJCR_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nADJCR_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }


            if (nOTHER_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nOTHER_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nFB_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nFB_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nMBill_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nMBill_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nHolidayAll_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nHolidayAll_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nGrossPay_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nGrossPay_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nPF_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nPF_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nTRNS_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTRNS_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nDORM_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDORM_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nADV_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nADV_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nADJDR_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nADJDR_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nRS_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nRS_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nPLoan_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nPLoan_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nTAX_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTAX_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nDeductionTotal_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nDeductionTotal_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            if (nNetPay_GT > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nNetPay_GT), _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            else
            {
                _oPdfPCell = new PdfPCell(new Phrase("-", _oFontStyle)); _oPdfPCell.FixedHeight = 20;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 34; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            //GT end
        }

        public void Footer()
        {
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = 30; _oPdfPCell.Colspan = 34; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 11; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 11; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("____________________", _oFontStyle)); _oPdfPCell.Colspan = 12; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Prepared By", _oFontStyle)); _oPdfPCell.Colspan = 11; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Checked By", _oFontStyle)); _oPdfPCell.Colspan = 11; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Approved By", _oFontStyle)); _oPdfPCell.Colspan = 12; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 34; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }

        #endregion

    }

}
