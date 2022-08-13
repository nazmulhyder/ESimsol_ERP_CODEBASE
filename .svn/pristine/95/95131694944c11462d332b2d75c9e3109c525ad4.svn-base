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
using ICS.Core.Framework;
using System.Linq;

namespace ESimSol.Reports
{
    public class rptEmployeeBonus_F3
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        
        PdfPTable _oPdfPTable;

        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        EmployeeBonus _oEmployeeBonus = new EmployeeBonus();
        List<EmployeeBonus> _oEmployeeBonuss = new List<EmployeeBonus>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        List<EmployeeBonus> _oTempEmployeeBonuss = new List<EmployeeBonus>();
        bool _bPercent;
        #endregion

        public byte[] PrepareReport(EmployeeBonus oEmployeeBonus,List<SalarySheetSignature> oSalarySheetSignatures,bool bPercent)
        {
            _bPercent = bPercent;
            _oEmployeeBonuss = oEmployeeBonus.EmployeeBonuss.OrderBy(x=>x.DepartmentName).ThenBy(x=>x.EmployeeCode).ToList();
            _oBusinessUnits = oEmployeeBonus.BusinessUnits;

            #region Page Setup

            int nColumn = 0;
            int _nColumns = 10;
            if (_bPercent)
            {
                _nColumns = 11;
            }
            float[] tablecolumns = new float[_nColumns];

            tablecolumns[nColumn++] = 30f;
            tablecolumns[nColumn++] = 65f;
            tablecolumns[nColumn++] = 150f;
            tablecolumns[nColumn++] = 137f;
            tablecolumns[nColumn++] = 75f;
            tablecolumns[nColumn++] = 70f;
            tablecolumns[nColumn++] = 75f;
            tablecolumns[nColumn++] = 75f;
            if (_bPercent)
            {
                tablecolumns[nColumn++] = 40f;
            }
            tablecolumns[nColumn++] = 75f;
            tablecolumns[nColumn++] = 100f;

            _oPdfPTable = new PdfPTable(_nColumns);

            _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            //_oDocument.SetPageSize(new iTextSharp.text.Rectangle(350, 230));
            _oDocument.SetMargins(40f, 40f, 5f, 60f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            //_oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            //PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            //ESimSolFooter_Signature PageEventHandler = new ESimSolFooter_Signature();
            //oPDFWriter.PageEvent = PageEventHandler;

            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            oPDFWriter.PageEvent = PageEventHandler;

            #endregion

            if (_oEmployeeBonuss.Count() > 0)
            { 
                this.PrintHeader(_oEmployeeBonuss[0], _oEmployeeBonuss[0].BusinessUnitID); 
            }
            else
            {
                var oEB = new EmployeeBonus();
                this.PrintHeader(oEB, oEB.BusinessUnitID);
            }
            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);
            //this.PrintBody();
            _oPdfPTable.HeaderRows = 6;
            this.PrintEncashedEL();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        public byte[] PrepareReportF5(EmployeeBonus oEmployeeBonus, List<SalarySheetSignature> oSalarySheetSignatures, bool bPercent)
        {
            _bPercent = bPercent;
            _oEmployeeBonuss = oEmployeeBonus.EmployeeBonuss.OrderBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
            _oBusinessUnits = oEmployeeBonus.BusinessUnits;

            #region Page Setup

            int nColumn = 0;
            int _nColumns = 12;
            if (_bPercent)
            {
                _nColumns = 13;
            }
            float[] tablecolumns = new float[_nColumns];

            tablecolumns[nColumn++] = 30f;
            tablecolumns[nColumn++] = 65f;
            tablecolumns[nColumn++] = 150f;
            tablecolumns[nColumn++] = 137f;
            tablecolumns[nColumn++] = 75f;
            tablecolumns[nColumn++] = 70f;
            tablecolumns[nColumn++] = 75f;            
            tablecolumns[nColumn++] = 75f;
            if (_bPercent)
            {
                tablecolumns[nColumn++] = 40f;
            }
            tablecolumns[nColumn++] = 75f;
            tablecolumns[nColumn++] = 75f;
            tablecolumns[nColumn++] = 75f;
            tablecolumns[nColumn++] = 100f;

            _oPdfPTable = new PdfPTable(_nColumns);

            _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());           
            _oDocument.SetMargins(40f, 40f, 5f, 60f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                       
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            oPDFWriter.PageEvent = PageEventHandler;

            #endregion

            if (_oEmployeeBonuss.Count() > 0)
            {
                this.PrintHeaderF5(_oEmployeeBonuss[0], _oEmployeeBonuss[0].BusinessUnitID);
            }
            else
            {
                var oEB = new EmployeeBonus();
                this.PrintHeaderF5(oEB, oEB.BusinessUnitID);
            }
            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);            
            _oPdfPTable.HeaderRows = 6;
            this.PrintEncashedELF5();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(EmployeeBonus oEmployeeBonus, int BusinessUnitID)
        {
            #region CompanyHeader

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnits = _oBusinessUnits.Where(x => x.BusinessUnitID == BusinessUnitID).ToList();
            if (oBusinessUnits.Count > 0) { oBusinessUnit = oBusinessUnits[0]; }

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Colspan = (_bPercent)?11:10;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Colspan = (_bPercent) ? 11 : 10;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = (_bPercent) ? 11 : 10;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oEmployeeBonus.Note, _oFontStyle));
            _oPdfPCell.Colspan = (_bPercent) ? 11 : 10;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = (_bPercent) ? 11 : 10;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Joining Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Service Year", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gross", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Basic", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            if (_bPercent)
            {
                _oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("Bonus Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            #endregion

        }

        private void PrintHeaderF5(EmployeeBonus oEmployeeBonus, int BusinessUnitID)
        {
            #region CompanyHeader

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnits = _oBusinessUnits.Where(x => x.BusinessUnitID == BusinessUnitID).ToList();
            if (oBusinessUnits.Count > 0) { oBusinessUnit = oBusinessUnits[0]; }

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oBusinessUnit.Name, _oFontStyle));
            _oPdfPCell.Colspan = (_bPercent) ? 13 : 12;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Colspan = (_bPercent) ? 13 : 12;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = (_bPercent) ? 13 : 12;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oEmployeeBonus.Note, _oFontStyle));
            _oPdfPCell.Colspan = (_bPercent) ? 13 : 12;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = (_bPercent) ? 13 : 12;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Joining Date", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Service Year", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Gross", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Basic", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            if (_bPercent)
            {
                _oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyle));
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("Bonus Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bank Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cash Amount", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body
        private void PrintEncashedEL()
        {
            if (_oEmployeeBonuss.Count <= 0 )
            {
                string masg = "Nothing to print";
                //if (_oEmployeeBonuss[0].ErrorMessage != "") { masg = ""; masg = _oEmployeeBonuss[0].ErrorMessage; }
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(masg));
                _oPdfPCell.Colspan = (_bPercent) ? 11 : 10;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                var grpEmpBonus = _oEmployeeBonuss.GroupBy(x => new { x.DepartmentID, x.DepartmentName }, (key, grp) => new
                {
                    DepartmentID = key.DepartmentID,
                    DepartmentName = key.DepartmentName,
                    EmpBonusCount = grp.Count(),
                    EmpBonusList = grp,
                    

                }).ToList();


                int nCount = 0;
                foreach (var oItem in grpEmpBonus)
                {
                    double TotalBonus = 0.0;
                    //Print Department
                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

                    _oPdfPCell = new PdfPCell(new Phrase("Department : " + oItem.DepartmentName, _oFontStyle)); _oPdfPCell.Colspan = (_bPercent) ? 11 : 10; _oPdfPCell.FixedHeight = 25;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPTable.CompleteRow();
                    

                    int RowHeight = 25;
                    foreach (EmployeeBonus data in oItem.EmpBonusList.OrderBy(x=>x.DepartmentName).ThenBy(x => x.EmployeeCode))
                    {
                        nCount++;
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(data.EmployeeCode, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(data.EmployeeName, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(data.DesignationName, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(data.JoiningDateInString, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        DateTime DeclarationDate = data.BonusDeclarationDate.AddDays(1);
                        DateDifference oService = new DateDifference(data.JoiningDate, DeclarationDate);

                        double nTotalDayCount = (DeclarationDate - data.JoiningDate).TotalDays;

                        _oPdfPCell = new PdfPCell(new Phrase((nTotalDayCount < 365) ? oService.ToString() + "\n(" + nTotalDayCount.ToString() + "d)" : oService.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(data.GrossAmount), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(data.BasicAmount), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        if (_bPercent)
                        {
                            double Percent = data.BonusAmount / data.GrossAmount * 100;
                            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(Percent, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        }

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(data.BonusAmount), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        TotalBonus += data.BonusAmount;
                        _oPdfPTable.CompleteRow();
                    }

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);

                    _oPdfPCell = new PdfPCell(new Phrase("TOTAL", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = (_bPercent)?9:8;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalBonus, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();


                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                }

            }
        }

        private void PrintEncashedELF5()
        {
            if (_oEmployeeBonuss.Count <= 0)
            {
                string masg = "Nothing to print";
                //if (_oEmployeeBonuss[0].ErrorMessage != "") { masg = ""; masg = _oEmployeeBonuss[0].ErrorMessage; }
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(masg));
                _oPdfPCell.Colspan = (_bPercent) ? 13 : 12;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                var grpEmpBonus = _oEmployeeBonuss.GroupBy(x => new { x.DepartmentID, x.DepartmentName }, (key, grp) => new
                {
                    DepartmentID = key.DepartmentID,
                    DepartmentName = key.DepartmentName,
                    EmpBonusCount = grp.Count(),
                    EmpBonusList = grp,


                }).ToList();


                double GrandTotalBonus = 0.0;
                double GrandTotalBonusBankAmount = 0;
                double GrandTotalBonusCashAmount = 0;
                int nCount = 0; int nDeptPrint = 0;
                foreach (var oItem in grpEmpBonus)
                {                    
                    double TotalBonus = 0.0;
                    double TotalBonusBankAmount = 0;
                    double TotalBonusCashAmount = 0;
                    nDeptPrint = nDeptPrint + 1;

                    _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("Department : " + oItem.DepartmentName, _oFontStyle)); _oPdfPCell.Colspan = (_bPercent) ? 13 : 12; _oPdfPCell.FixedHeight = 25;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    int RowHeight = 25;
                    foreach (EmployeeBonus data in oItem.EmpBonusList.OrderBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode))
                    {
                        nCount++;
                        _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

                        _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(data.EmployeeCode, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(data.EmployeeName, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(data.DesignationName, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(data.JoiningDateInString, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        DateTime DeclarationDate = data.BonusDeclarationDate.AddDays(1);
                        DateDifference oService = new DateDifference(data.JoiningDate, DeclarationDate);

                        double nTotalDayCount = (DeclarationDate - data.JoiningDate).TotalDays;

                        _oPdfPCell = new PdfPCell(new Phrase((nTotalDayCount < 365) ? oService.ToString() + "\n(" + nTotalDayCount.ToString() + "d)" : oService.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(data.GrossAmount), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(data.BasicAmount), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        if (_bPercent)
                        {
                            double Percent = data.BonusAmount / data.GrossAmount * 100;
                            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(Percent, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        }

                        _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(data.BonusAmount), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                        if (data.BankAccountNo != "")
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(data.BonusBankAmount), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(data.BonusCashAmount), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            TotalBonusBankAmount += data.BonusBankAmount;
                            TotalBonusCashAmount += data.BonusCashAmount;

                            GrandTotalBonusBankAmount += data.BonusBankAmount;
                            GrandTotalBonusCashAmount += data.BonusCashAmount;
                        }
                        else
                        {
                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit(0.0), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormatActualDigit((data.BonusCashAmount + data.BonusBankAmount)), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                            _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            TotalBonusBankAmount += 0;
                            TotalBonusCashAmount += (data.BonusCashAmount + data.BonusBankAmount);
                            
                            GrandTotalBonusBankAmount += 0;
                            GrandTotalBonusCashAmount += (data.BonusCashAmount + data.BonusBankAmount);
                        }

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        TotalBonus += data.BonusAmount;
                        GrandTotalBonus += data.BonusAmount;
                        _oPdfPTable.CompleteRow();
                    }

                    #region Sub Total
                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase("SUB TOTAL", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = (_bPercent) ? 9 : 8;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalBonus, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalBonusBankAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalBonusCashAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();
                    #endregion

                    #region Grand Total
                    if (grpEmpBonus.Count() == nDeptPrint)
                    {                        
                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
                        _oPdfPCell = new PdfPCell(new Phrase("GRAND TOTAL", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = (_bPercent) ? 9 : 8;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(GrandTotalBonus, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(GrandTotalBonusBankAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(GrandTotalBonusCashAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                        _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                    }
                    #endregion

                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                }

            }
        }
        #endregion

        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount) : amount;
            return (bWithPrecision) ? Global.MillionFormatActualDigit(amount) : Global.MillionFormatActualDigit(amount).Split('.')[0];
        }
    }

}
