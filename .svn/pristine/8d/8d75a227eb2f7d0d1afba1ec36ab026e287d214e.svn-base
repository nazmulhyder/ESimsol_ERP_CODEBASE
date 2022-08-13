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
    public class rptTaxDetail_F1
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyle1;
        PdfPTable _oPdfPTable = new PdfPTable(7);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        List<ITaxLedger> _oITaxLedgers = new List<ITaxLedger>();
        List<ITaxLedgerSalaryHead> _oITaxLedgerSalaryHeads = new List<ITaxLedgerSalaryHead>();
        ITaxLedger _oITaxLedger = new ITaxLedger();
        ITaxRateScheme _oITaxRateScheme = new ITaxRateScheme();
        ITaxAssessmentYear _oITaxAssessmentYear = new ITaxAssessmentYear();
        List<ITaxHeadConfiguration> _oITaxHeadConfigurations = new List<ITaxHeadConfiguration>();
        List<EmployeeSalaryStructureDetail> _oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
        List<ITaxRebateScheme> _oITaxRebateSchemes = new List<ITaxRebateScheme>();
        Company _oCompany = new Company();

        #endregion
        public byte[] PrepareReport(ITaxLedger oITaxLedger)
        {
            _oITaxLedger = oITaxLedger;
            _oITaxLedgerSalaryHeads = _oITaxLedger.ITaxLedgerSalaryHeads;
            _oITaxAssessmentYear = _oITaxLedger.ITaxAssessmentYear;
            _oITaxHeadConfigurations = _oITaxLedger.ITaxHeadConfigurations;
            _oEmployeeSalaryStructureDetails = _oITaxLedger.EmployeeSalaryStructureDetails;
            _oITaxRebateSchemes = _oITaxLedger.ITaxRebateSchemes;
            _oITaxRateScheme = _oITaxLedger.ITaxRateScheme;
            _oCompany = oITaxLedger.Company;

            #region Page Setup
            //_oDocument = new Document(new iTextSharp.text.Rectangle(370, 380), 0f, 0f, 0f, 0f);
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(60f, 60f, 15f, 40f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _oFontStyle1 = FontFactory.GetFont("Tahoma", 7f, 1);
            PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 20f, 200f, 20f, 95f, 75f, 20f, 50f });

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

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Name, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(_oCompany.Address, _oFontStyle));
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

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("TAX DETAIL", _oFontStyle));
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

            PdfPTable oPdfPTable = new PdfPTable(3);
            oPdfPTable.SetWidths(new float[] { 120f, 20f, 250f});
            PdfPCell oPdfPCell;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);

            oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle)); oPdfPCell.Border = 0; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oITaxLedger.EmployeeNameCode, _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle)); oPdfPCell.Border = 0; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oITaxLedger.DepartmentName, _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();


            oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oITaxLedger.DesignationName, _oFontStyle)); oPdfPCell.Border = 0; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Income Year", _oFontStyle)); oPdfPCell.Border = 0; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(_oITaxAssessmentYear.AssessmentYear, _oFontStyle)); oPdfPCell.Border = 0; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            oPdfPCell = new PdfPCell(new Phrase("Assesment Year", _oFontStyle)); oPdfPCell.Border = 0; 
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            string assYear1 = _oITaxAssessmentYear.AssessmentYear.Split('-')[0];
            int assYearInt1 = Convert.ToInt32(assYear1)+1;
            string assYear2 = _oITaxAssessmentYear.AssessmentYear.Split('-')[1];
            int assYearInt2 = Convert.ToInt32(assYear2)+1;

            string newAssYear = Convert.ToString(assYearInt1) + "-" + Convert.ToString(assYearInt2);


            oPdfPCell = new PdfPCell(new Phrase(newAssYear, _oFontStyle)); oPdfPCell.Border = 0;
            oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(oPdfPCell);

            oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 4; 
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.FixedHeight = 20;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oITaxLedgerSalaryHeads = _oITaxLedgerSalaryHeads.OrderBy(x => x.ITaxLSHID).ToList();

            int nCount = 0;
            double nTotalSHAmount = 0;
            double nTotalTaxableAmount = 0;

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Salary Head", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Income From Salary", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("S. Head Amt.", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Taxable Amt.", _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            foreach (ITaxLedgerSalaryHead oItem in _oITaxLedgerSalaryHeads)
            {
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                List<ITaxHeadConfiguration>  oITHonfigs = new List<ITaxHeadConfiguration>();
                string sDescription ="";
                oITHonfigs = _oITaxHeadConfigurations.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).ToList();
                sDescription = oITHonfigs.Any()?oITHonfigs[0].DescriptionStr:"";

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName + "\n" + "(" +sDescription + ")", _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                List<EmployeeSalaryStructureDetail> oEmpSSDs = new List<EmployeeSalaryStructureDetail>();
                double SalaryHeadAmount = 0;
                string  sTimePeriod = "";
                oEmpSSDs = _oEmployeeSalaryStructureDetails.Where(x => x.SalaryHeadID == oItem.SalaryHeadID).ToList();
                SalaryHeadAmount = oEmpSSDs.Any() ? oEmpSSDs[0].Amount : 0;
                if (oEmpSSDs.Any())
                {
                    if (oEmpSSDs[0].Times == 0 && oEmpSSDs[0].Period == 0)
                    {
                        sTimePeriod = "12 Month";
                    }
                    else
                    {
                        sTimePeriod = oEmpSSDs[0].Times + " Times";
                    }
                }

                _oPdfPCell = new PdfPCell(new Phrase(SalaryHeadAmount + "  × " + sTimePeriod, _oFontStyle)); 
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.SalaryHeadAmount), _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oItem.TaxableAmount), _oFontStyle)); _oPdfPCell.Colspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();
                nTotalSHAmount += oItem.SalaryHeadAmount;
                nTotalTaxableAmount += oItem.TaxableAmount;
            }

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);


            _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalSHAmount), _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalTaxableAmount), _oFontStyle)); _oPdfPCell.Colspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = 7; _oPdfPCell.FixedHeight = 15;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Tax Amount", _oFontStyle)); _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oITaxLedger.TaxAmount), _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Investment All.", _oFontStyle)); _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(nTotalTaxableAmount/100*(_oITaxRebateSchemes.Any()?_oITaxRebateSchemes[0].PercentOfTaxIncome:0)), _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oITaxRebateSchemes.Any()?("(Max." + _oITaxRebateSchemes[0].MaxRebateAmountST + "(BDT) or " + _oITaxRebateSchemes[0].PercentOfTaxIncome.ToString() + "% of taxable income whichever is lower)"):"", _oFontStyle1)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Rebate", _oFontStyle)); _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oITaxLedger.RebateAmount), _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oITaxRebateSchemes.Any() ? ("(rebate is " + _oITaxRebateSchemes[0].RebateInPercent + "% of " + _oITaxRebateSchemes[0].ITaxRebateType.ToString()+")") : "", _oFontStyle1)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Advance Paid", _oFontStyle)); _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oITaxLedger.AdvancePaidAmount), _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Payable Tax", _oFontStyle)); _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oITaxLedger.TotalTax), _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(_oITaxRebateSchemes.Any() ? ("(Minimum Tax " + _oITaxRateScheme.MinimumTax + ")") : "", _oFontStyle1)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("Installment", _oFontStyle)); _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(":", _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(_oITaxLedger.InstallmentAmount), _oFontStyle)); _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.Colspan = 7; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 200;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oPdfPCell = new PdfPCell(new Phrase("______________________\nHead of Human Resources", _oFontStyle)); _oPdfPCell.Colspan = 7; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }
        #endregion
    }
}
