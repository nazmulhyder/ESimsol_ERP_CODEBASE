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
using System.Collections;

namespace ESimSol.Reports
{
    public class rptTaxAssessment
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(6);
        PdfPTable _oPdfPNextTable = new PdfPTable(11);
        PdfPCell _oPdfPCell;
        PdfPCell _oPdfPNextCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        List<ITaxLedger> _oITaxLedgers = new List<ITaxLedger>();
        List<SalaryHead> _oSalaryHeads = new List<SalaryHead>();
        List<EmployeeSalary> _oEmployeeSalarys = new List<EmployeeSalary>();
        List<EmployeeSalaryDetail> _oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
        List<ITaxRateSlab> _oITaxRateSlabs = new List<ITaxRateSlab>();
        List<ITaxRebatePayment> _oITaxRebatePayments = new List<ITaxRebatePayment>();
        List<ITaxRebateItem> _oITaxRebateItems = new List<ITaxRebateItem>();
        List<ITaxLedgerSalaryHead> _oITaxLedgerSalaryHeads = new List<ITaxLedgerSalaryHead>();
        ITaxLedger _oITaxLedger = new ITaxLedger();
        ITaxAssessmentYear _oITaxAssessmentYear = new ITaxAssessmentYear();
        Company _oCompany = new Company();
        Employee _oEmployee = new Employee();
        ITaxRateScheme _oITaxRateScheme = new ITaxRateScheme();
        List<EmployeeSalaryStructureDetail> _oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
        EmployeeTINInformation _oEmployeeTINInformation = new EmployeeTINInformation();
        List<ITaxHeadConfiguration> _oITaxHeadConfigurations = new List<ITaxHeadConfiguration>();

        #endregion
        public byte[] PrepareReport(ITaxLedger oITaxLedger)
        {
            _oITaxLedger = oITaxLedger;

            _oEmployeeSalarys = _oITaxLedger.EmployeeSalarys;
            _oEmployeeSalaryDetails = _oITaxLedger.EmployeeSalaryDetails;
            _oSalaryHeads = _oITaxLedger.SalaryHeads;

            _oITaxLedgerSalaryHeads = _oITaxLedger.ITaxLedgerSalaryHeads;
            _oITaxAssessmentYear = _oITaxLedger.ITaxAssessmentYear;
            _oCompany = oITaxLedger.Company;
            _oEmployee = oITaxLedger.Employee;
            _oITaxRateScheme = oITaxLedger.ITaxRateScheme;
            _oEmployeeTINInformation = oITaxLedger.EmployeeTINInformation;
            _oEmployeeSalaryStructureDetails = oITaxLedger.EmployeeSalaryStructureDetails;
            _oITaxHeadConfigurations = oITaxLedger.ITaxHeadConfigurations;
            _oITaxRateSlabs = oITaxLedger.ITaxRateSlabs;
            _oITaxRebateItems = oITaxLedger.ITaxRebateItems;
            _oITaxRebatePayments = oITaxLedger.ITaxRebatePayments;

            #region Page Setup
            //_oDocument = new Document(new iTextSharp.text.Rectangle(370, 380), 0f, 0f, 0f, 0f);
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(60f, 60f, 15f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 80f, 118f, 90f, 80f, 118f, 109f });

            #endregion

            //this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader()
        {
            #region CompanyHeader
            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 150f, 200f });
            PdfPCell oPdfPCellHearder;

            if (_oCompany.CompanyLogo != null)
            {
                _oImag = iTextSharp.text.Image.GetInstance(_oCompany.CompanyLogo, System.Drawing.Imaging.ImageFormat.Jpeg);
                _oImag.ScaleAbsolute(150f, 30f);
                oPdfPCellHearder = new PdfPCell(_oImag);
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder.VerticalAlignment = Element.ALIGN_BOTTOM;
                oPdfPCellHearder.PaddingBottom = -4;
                oPdfPCellHearder.Border = 0;

                oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }
            else
            {
                oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCellHearder.Border = 0; oPdfPCellHearder.FixedHeight = 15;
                oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCellHearder.BackgroundColor = BaseColor.WHITE; oPdfPTableHeader.AddCell(oPdfPCellHearder);
            }

            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyleBold));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPTableHeader.CompleteRow();

            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyleBold));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("TAX ASSESSMENT DETAILS FOR SALARY INCOME", _oFontStyleBold));
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            
            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 7;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            this.PrintHeader();

            _oPdfPCell = new PdfPCell(new Phrase("Code : ", _oFontStyle)); _oPdfPCell.Border = 0; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Code, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Name : ", _oFontStyle)); _oPdfPCell.Border = 0; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployee.Name, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Department : ", _oFontStyle)); _oPdfPCell.Border = 0; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oITaxLedger.DepartmentName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Designation : ", _oFontStyle)); _oPdfPCell.Border = 0; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oITaxLedger.DesignationName, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Tax Payer : ", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oITaxRateScheme.TaxPayerTypeString, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Tax Area : ", _oFontStyle)); _oPdfPCell.Border = 0; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oITaxRateScheme.TaxAreaString, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("E-TIN No. : ", _oFontStyle)); _oPdfPCell.Border = 0; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(_oEmployeeTINInformation.ETIN, _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 6; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Salary Head", _oFontStyleBold)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Salary Head", _oFontStyleBold));
             _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

             _oPdfPCell = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oEmployeeSalaryStructureDetails = _oEmployeeSalaryStructureDetails.OrderBy(x => x.SalaryHeadID).ToList();
            double nTotalSHAmount = 0;
            foreach (EmployeeSalaryStructureDetail oItem in _oEmployeeSalaryStructureDetails)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.Amount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
                nTotalSHAmount += oItem.Amount;
            }

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Total Gross", _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalSHAmount), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            if (_oCompany.BaseAddress == "mamiya")
            {
                _oPdfPCell = new PdfPCell(new Phrase("Note: Festival Bonus (100% of Basic, Yearly 2 time) & Monthly PF deduction (8.33% of Basic)", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 6;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 6;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("SALARY INCOME TAX ASSESSMENT FOR THE YEAR OF " + _oITaxLedger.AssessmentYear, _oFontStyleBold)); _oPdfPCell.Colspan = 6;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Tax Slab", _oFontStyleBold)); _oPdfPCell.Colspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Amt.", _oFontStyleBold));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Rebate Amt.", _oFontStyleBold));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Taxable Income.", _oFontStyleBold));
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            int nCount = 0;
            double nTotalTaxableAmount = 0.0;
            foreach (ITaxHeadConfiguration oitem in _oITaxHeadConfigurations)
            {
                _oPdfPCell = new PdfPCell(new Phrase((++nCount) + ". "+ oitem.SalaryHeadName + " ("+oitem.DescriptionStr+")", _oFontStyle)); _oPdfPCell.Colspan = 3;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(getSalaryFromHead(oitem.ITaxHeadConfigurationID)).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                double nRebateAmount = Math.Round(getSalaryFromHead(oitem.ITaxHeadConfigurationID)) - Math.Round(getTaxFromHead(oitem.ITaxHeadConfigurationID));

                _oPdfPCell = new PdfPCell(new Phrase(Math.Abs(nRebateAmount).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Math.Round(getTaxFromHead(oitem.ITaxHeadConfigurationID)).ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                nTotalTaxableAmount += Math.Round(getTaxFromHead(oitem.ITaxHeadConfigurationID));
                _oPdfPTable.CompleteRow();
            }
            _oPdfPCell = new PdfPCell(new Phrase("Total", _oFontStyleBold)); _oPdfPCell.Colspan = 5;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(nTotalTaxableAmount.ToString(), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            PdfPTable oPdfPTableHeader = new PdfPTable(4);
            oPdfPTableHeader.SetWidths(new float[] { 80f, 118f, 90f, 80f });
            PdfPCell oPdfPCellHearder;

            oPdfPCellHearder = new PdfPCell(new Phrase("Tax Slab", _oFontStyleBold));
            oPdfPCellHearder.Colspan = 4;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPTableHeader.CompleteRow();


            oPdfPCellHearder = new PdfPCell(new Phrase("Slab No.", _oFontStyleBold));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPCellHearder = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPCellHearder = new PdfPCell(new Phrase("Tax Rate (%)", _oFontStyleBold));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPCellHearder = new PdfPCell(new Phrase("Amount (BDT)", _oFontStyleBold));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            int nc = 0;
            double nDivideAmount = nTotalTaxableAmount;
            double nTotalTaxAmounts = 0.0;
            foreach (ITaxRateSlab oitem in _oITaxRateSlabs)
            {
                if (oitem.ITaxRateSchemeID == _oITaxRateScheme.ITaxRateSchemeID)
                {
                    oPdfPCellHearder = new PdfPCell(new Phrase((++nc).ToString(), _oFontStyle));
                    oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                    oPdfPCellHearder.ExtraParagraphSpace = 0;
                    oPdfPTableHeader.AddCell(oPdfPCellHearder);

                    oPdfPCellHearder = new PdfPCell(new Phrase((oitem.Amount).ToString(), _oFontStyle));
                    oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                    oPdfPCellHearder.ExtraParagraphSpace = 0;
                    oPdfPTableHeader.AddCell(oPdfPCellHearder);

                    oPdfPCellHearder = new PdfPCell(new Phrase((oitem.Percents).ToString(), _oFontStyle));
                    oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
                    oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                    oPdfPCellHearder.ExtraParagraphSpace = 0;
                    oPdfPTableHeader.AddCell(oPdfPCellHearder);

                    double nAmount = getTaxAmount(ref nDivideAmount, oitem.Amount, oitem.Percents);
                    nTotalTaxAmounts += Math.Floor(nAmount);
                    oPdfPCellHearder = new PdfPCell(new Phrase(Math.Floor(nAmount).ToString(), _oFontStyle));
                    oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
                    oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
                    oPdfPCellHearder.ExtraParagraphSpace = 0;
                    oPdfPTableHeader.AddCell(oPdfPCellHearder);
                }
            }
            oPdfPCellHearder = new PdfPCell(new Phrase("Total", _oFontStyleBold)); oPdfPCellHearder.Colspan = 3;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPCellHearder = new PdfPCell(new Phrase(nTotalTaxAmounts.ToString(), _oFontStyleBold));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();








            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = 4;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);


            PdfPTable oPdfPTableHeader1 = new PdfPTable(2);
            oPdfPTableHeader1.SetWidths(new float[] { 118f, 109f });
            PdfPCell oPdfPCellHearder1;

            oPdfPCellHearder1 = new PdfPCell(new Phrase("Investment Details", _oFontStyleBold));
            oPdfPCellHearder1.Colspan = 2;
            oPdfPCellHearder1.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder1.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder1.ExtraParagraphSpace = 0;
            oPdfPTableHeader1.AddCell(oPdfPCellHearder1);
            oPdfPTableHeader1.CompleteRow();


            oPdfPCellHearder1 = new PdfPCell(new Phrase("Investment Type", _oFontStyleBold));
            oPdfPCellHearder1.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder1.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder1.ExtraParagraphSpace = 0;
            oPdfPTableHeader1.AddCell(oPdfPCellHearder1);

            oPdfPCellHearder1 = new PdfPCell(new Phrase("Amount", _oFontStyleBold));
            oPdfPCellHearder1.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder1.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder1.ExtraParagraphSpace = 0;
            oPdfPTableHeader1.AddCell(oPdfPCellHearder1);
            oPdfPTableHeader1.CompleteRow();

            double nTotalRebateAmount = 0.0;
            foreach (ITaxRebateItem oitem in _oITaxRebateItems)
            {

                oPdfPCellHearder1 = new PdfPCell(new Phrase(oitem.Description, _oFontStyle));
                oPdfPCellHearder1.HorizontalAlignment = Element.ALIGN_CENTER;
                oPdfPCellHearder1.BackgroundColor = BaseColor.WHITE;
                oPdfPCellHearder1.ExtraParagraphSpace = 0;
                oPdfPTableHeader1.AddCell(oPdfPCellHearder1);

                nTotalRebateAmount += Math.Floor(getTaxRebatePayment(oitem.ITaxRebateItemID));

                oPdfPCellHearder1 = new PdfPCell(new Phrase(Math.Floor(getTaxRebatePayment(oitem.ITaxRebateItemID)).ToString(), _oFontStyle));
                oPdfPCellHearder1.HorizontalAlignment = Element.ALIGN_RIGHT;
                oPdfPCellHearder1.BackgroundColor = BaseColor.WHITE;
                oPdfPCellHearder1.ExtraParagraphSpace = 0;
                oPdfPTableHeader1.AddCell(oPdfPCellHearder1);
            }

            oPdfPCellHearder1 = new PdfPCell(new Phrase("Total", _oFontStyleBold));
            oPdfPCellHearder1.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCellHearder1.BackgroundColor = BaseColor.WHITE;
            oPdfPTableHeader1.AddCell(oPdfPCellHearder1);

            oPdfPCellHearder1 = new PdfPCell(new Phrase(nTotalRebateAmount.ToString(), _oFontStyleBold));
            oPdfPCellHearder1.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCellHearder1.BackgroundColor = BaseColor.WHITE;
            oPdfPTableHeader1.AddCell(oPdfPCellHearder1);
            oPdfPTableHeader1.CompleteRow();

            oPdfPCellHearder1 = new PdfPCell(new Phrase("Max.15,000,000 or 25% of taxable income", _oFontStyleBold));
            oPdfPCellHearder1.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCellHearder1.BackgroundColor = BaseColor.WHITE;
            oPdfPTableHeader1.AddCell(oPdfPCellHearder1);

            double rateIncomeTax = Math.Round((nTotalTaxableAmount * 25) / 100);
            oPdfPCellHearder1 = new PdfPCell(new Phrase(rateIncomeTax.ToString(), _oFontStyleBold));
            oPdfPCellHearder1.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCellHearder1.BackgroundColor = BaseColor.WHITE;
            oPdfPTableHeader1.AddCell(oPdfPCellHearder1);
            oPdfPTableHeader1.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader1);
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();




            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Less: Rebate on Investment", _oFontStyleBold));
            _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("Monthly Tax", _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Rebate 15% of Investment", _oFontStyle));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            double rebate = Math.Round((rateIncomeTax * 15) / 100);

            _oPdfPCell = new PdfPCell(new Phrase(rebate.ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase(_oITaxLedger.InstallmentAmount.ToString(), _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.Rowspan = 3;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Tax Liability", _oFontStyle));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Abs(rebate - nTotalTaxAmounts).ToString(), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Net Payable", _oFontStyleBold));
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oITaxLedger.InstallmentAmount * 12).ToString(), _oFontStyleBold));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.FixedHeight = 110;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oPdfPCell = new PdfPCell(new Phrase("Admin, HR and Compliance", _oFontStyle));
            _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = _oPdfPCell.BorderWidthLeft = _oPdfPCell.BorderWidthRight = 1;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 60;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Accounts", _oFontStyle));
            _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = _oPdfPCell.BorderWidthLeft = _oPdfPCell.BorderWidthRight = 1;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 60;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Factory Manager", _oFontStyle));
            _oPdfPCell.BorderWidthTop = _oPdfPCell.BorderWidthBottom = _oPdfPCell.BorderWidthLeft = _oPdfPCell.BorderWidthRight = 1;
            _oPdfPCell.Colspan = 2;
            _oPdfPCell.FixedHeight = 60;
            _oPdfPCell.VerticalAlignment = Element.ALIGN_BOTTOM;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();








            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
            this.PrintHeader();


            int[] SHIDs = _oEmployeeSalaryDetails.Select(x => x.SalaryHeadID).Distinct().ToArray();


            var data = _oSalaryHeads.Where(x => SHIDs.Contains(x.SalaryHeadID)).ToList();


            Dictionary<string, object> sumHeads = new Dictionary<string, object>();

            List<string> ColHeads = new List<string>();
            ColHeads = _oEmployeeSalaryDetails.Where(x => ((x.SalaryHeadType == (int)EnumSalaryHeadType.Basic) || (x.SalaryHeadType == (int)EnumSalaryHeadType.Addition)) || (x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction)).Select(x => x.SalaryHeadName).ToList();
            ColHeads = ColHeads.Distinct().ToList();


            foreach (string property in ColHeads)
            {
                sumHeads.Add(property, 0);
            }

            int columnswidth = 480 / ColHeads.Count();
            float[] n = new float[ColHeads.Count() + 3];
            n[0] = 25f;
            n[1] = 45f;
            n[2] = 45f;
            for (int i = 3; i < ColHeads.Count;i++)
            {
                n[i]= columnswidth;
            }
            _oPdfPNextTable = new PdfPTable(ColHeads.Count+3);
            _oPdfPNextTable.SetWidths(n);

            _oPdfPNextCell = new PdfPCell(new Phrase("Salary Details", _oFontStyle)); _oPdfPNextCell.Colspan = ColHeads.Count() + 3;
            _oPdfPNextCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);
            _oPdfPNextTable.CompleteRow();


            _oPdfPNextCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPNextCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);

            _oPdfPNextCell = new PdfPCell(new Phrase("Month", _oFontStyle));
            _oPdfPNextCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);

            _oPdfPNextCell = new PdfPCell(new Phrase("Year", _oFontStyle));
            _oPdfPNextCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);

            foreach (string item in ColHeads)
            {
                _oPdfPNextCell = new PdfPCell(new Phrase(item, _oFontStyle));
                _oPdfPNextCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);

            }
            _oPdfPNextTable.CompleteRow();

            Dictionary<string, object> dSumHeads = new Dictionary<string, object>();
            foreach (string property in sumHeads.Select(x => x.Key).ToArray())
            {
                dSumHeads.Add(property, 0);
            }

            int nSl = 0;
            List<dynamic> objs = new List<dynamic>();

            List<string> ColSum = new List<string>();
            foreach (EmployeeSalary oItem in _oEmployeeSalarys) {

                _oPdfPNextCell = new PdfPCell(new Phrase((++nSl).ToString(), _oFontStyle));
                _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);

                _oPdfPNextCell = new PdfPCell(new Phrase(oItem.SalaryEndMonth, _oFontStyle));
                _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);
                
                _oPdfPNextCell = new PdfPCell(new Phrase(oItem.SalaryEndYear, _oFontStyle));
                _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);


                double Amount;
               
                foreach (string sItem in ColHeads)
                {
                    var oESDs = _oEmployeeSalaryDetails.Where(x => x.SalaryHeadName == sItem && x.EmployeeSalaryID == oItem.EmployeeSalaryID).ToList();
                    Amount = (oESDs.Count() > 0) ? oESDs.Sum(x => x.Amount) : 0;
                    objs.Add(new { SalaryHeadName = sItem, AmountHead = Amount });
                    Amount = Math.Round(Amount);
                   
                    _oPdfPNextCell = new PdfPCell(new Phrase(Amount.ToString(), _oFontStyle));
                    _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);

                }
            }
            double sumAmount = 0.0;

            _oPdfPNextCell = new PdfPCell(new Phrase("Total".ToString(), _oFontStyleBold)); _oPdfPNextCell.Colspan = 3;
            _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);

            foreach (string sItem in ColHeads)
            {
                var oESDs = objs.Where(x => x.SalaryHeadName == sItem).ToList();
                sumAmount = (oESDs.Count() > 0) ? oESDs.Sum(x => Convert.ToInt32(x.AmountHead)) : 0;
             
                _oPdfPNextCell = new PdfPCell(new Phrase(Math.Round(sumAmount).ToString(), _oFontStyleBold));
                _oPdfPNextCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPNextCell.BackgroundColor = BaseColor.WHITE; _oPdfPNextTable.AddCell(_oPdfPNextCell);

            }

            _oPdfPCell = new PdfPCell(_oPdfPNextTable);
            _oPdfPCell.Colspan = 11;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            ///////////////////////////////////////

            //var data  = _oEmployeeSalarys.GroupBy(x => new { x.SalaryEndYear, x.SalaryEndMonth }, (key, grp) => new {
            //    SalaryEndYear = key.SalaryEndYear,
            //    SalaryEndMonth = key.SalaryEndMonth,
            //    Salarys = grp.ToList()
            //}).ToString();
            
            

        }


        public double getSalaryFromHead(int nITaxHeadConfigureID)
        {
            double nAmount = 0.0;
            foreach (ITaxLedgerSalaryHead oItem in _oITaxLedgerSalaryHeads)
            {
                if (oItem.ITaxHeadConfigureID == nITaxHeadConfigureID)
                {
                    nAmount = oItem.SalaryHeadAmount;
                    break;
                }
            }
            return nAmount;
        }
        public double getTaxFromHead(int nITaxHeadConfigureID)
        {
            double nAmount = 0.0;
            foreach (ITaxLedgerSalaryHead oItem in _oITaxLedgerSalaryHeads)
            {
                if (oItem.ITaxHeadConfigureID == nITaxHeadConfigureID)
                {
                    nAmount = oItem.TaxableAmount;
                    break;
                }
            }
            return nAmount;
        }
        public double getTaxRebatePayment(int nITaxRebateItemID)
        {
            double nAmount = 0.0;
            foreach (ITaxRebatePayment oItem in _oITaxRebatePayments)
            {
                if (oItem.ITaxRebateItemID == nITaxRebateItemID)
                {
                    nAmount = oItem.Amount;
                    break;
                }
            }
            return nAmount;
        }


        public double getTaxAmount(ref double nTotalTaxAmount, double nSlabAmount, double nPercent)
        {
            double nAmount = 0.0;

            if (nTotalTaxAmount > 0) 
            {
                if(nTotalTaxAmount>nSlabAmount)
                {
                    nAmount = (nTotalTaxAmount - nSlabAmount) * (nPercent / 100);
                    nTotalTaxAmount -= nSlabAmount;
                }
                else 
                {
                    nAmount = nTotalTaxAmount * (nPercent / 100);
                    nTotalTaxAmount =0;
                }
            }
            return nAmount;
        }

        #endregion

    }
}

