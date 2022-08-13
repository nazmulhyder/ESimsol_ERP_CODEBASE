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
    public class rptEmployeeBonus_F2
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable = new PdfPTable(14);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        EmployeeBonus _oEmployeeBonus = new EmployeeBonus();
        List<EmployeeBonus> _oEmployeeBonuss = new List<EmployeeBonus>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        List<EmployeeBonus> _oTempEmployeeBonuss = new List<EmployeeBonus>();
        Company _oCompany = new Company();
        int _nRow = 0;
        int _nRowCount = 0;
        string _sBlockIDs = "";
        #endregion

        public byte[] PrepareReport(EmployeeBonus oEmployeeBonus,List<SalarySheetSignature> oSalarySheetSignatures, Company oCompany, string blockIDs)
        {
            _sBlockIDs = blockIDs;
            _oCompany = oCompany;
            _oEmployeeBonuss = oEmployeeBonus.EmployeeBonuss.OrderBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
            _oBusinessUnits = oEmployeeBonus.BusinessUnits;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            //_oDocument.SetPageSize(new iTextSharp.text.Rectangle(350, 230));
            _oDocument.SetMargins(40f, 40f, 5f, 40f);
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


            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 25f, 80f, 110f, 75f, 70f, 60f, 70f,70f, 70f, 30f, 60f, 35f, 70f, 70f});
            #endregion

            //this.PrintHeader();
            //this.PrintBody();
            //_oPdfPTable.HeaderRows = 5;
            this.PrintEncashedEL();
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
            _oPdfPCell.Colspan = 14;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oBusinessUnit.Address, _oFontStyle));
            _oPdfPCell.Colspan = 14;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 14;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oEmployeeBonus.Note, _oFontStyle));
            _oPdfPCell.Colspan = 14;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 14;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
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
                _oPdfPCell.Colspan = 14;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                _oEmployeeBonuss.ForEach(x => _oTempEmployeeBonuss.Add(x));
                _oEmployeeBonuss = _oEmployeeBonuss.OrderBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x=>x.BlockName).ToList();
                while (_oEmployeeBonuss.Count > 0)
                {
                    var oResults = new List<EmployeeBonus>();
                    if (!string.IsNullOrEmpty(_sBlockIDs))
                    {
                        oResults = _oEmployeeBonuss.Where(x => x.BusinessUnitName == _oEmployeeBonuss[0].BusinessUnitName && x.LocationName == _oEmployeeBonuss[0].LocationName && x.DepartmentName == _oEmployeeBonuss[0].DepartmentName && x.BlockName == _oEmployeeBonuss[0].BlockName).ToList();
                    }
                    else
                    {
                        oResults = _oEmployeeBonuss.Where(x => x.BusinessUnitName == _oEmployeeBonuss[0].BusinessUnitName && x.LocationName == _oEmployeeBonuss[0].LocationName && x.DepartmentName == _oEmployeeBonuss[0].DepartmentName).ToList();
                    }
                    

                    //this.PrintHeader(oResults[0], oResults[0].BusinessUnitID);
                    //this.PrintHeadRow(oResults[0]);
                    this.PrintBody(oResults, oResults[0].DepartmentID);
                    _nRow = 0;
                    if (!string.IsNullOrEmpty(_sBlockIDs))
                    {
                        _oEmployeeBonuss.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName && x.BlockName == oResults[0].BlockName); 
                    }
                    else
                    {
                        _oEmployeeBonuss.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName); 
                    }
                }
                //PrintGT();
            }
        }
        private void PrintHeadRow(EmployeeBonus oEmployeeBonus)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Unit-" + oEmployeeBonus.LocationName, _oFontStyle)); _oPdfPCell.Colspan = 5; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            string sDeptBlock = "Department- "+ oEmployeeBonus.DepartmentName;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                if (!string.IsNullOrEmpty(oEmployeeBonus.BlockName))
                {
                    sDeptBlock += ", Block- " + oEmployeeBonus.BlockName;
                }
            }

            _oPdfPCell = new PdfPCell(new Phrase(sDeptBlock, _oFontStyle)); _oPdfPCell.Colspan = 9; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Employee ID", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Employee Type", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Joining", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Salary", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Declaration Date", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(oEmployeeBonus.SalaryHeadName + " " + oEmployeeBonus.Note + " " + oEmployeeBonus.Year, _oFontStyle)); _oPdfPCell.Colspan = 4;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
           
            _oPdfPCell = new PdfPCell(new Phrase("Net Payable", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Rcv. Sign", _oFontStyle)); _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
           
            _oPdfPCell = new PdfPCell(new Phrase("Service Year", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("%", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Payable", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Stamp", _oFontStyle));
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }

        int RowHeight = 38;
        private void PrintBody(List<EmployeeBonus> oEmployeeBonuss, int nDepartmentID)
        {
            this.PrintHeader(oEmployeeBonuss[0], oEmployeeBonuss[0].BusinessUnitID);
            this.PrintHeadRow(oEmployeeBonuss[0]);

            int nCount = 0;
            foreach (EmployeeBonus oItem in oEmployeeBonuss)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                nCount++;
                _nRow++;
                _nRowCount++;
                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.EmployeeCode, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                _oPdfPCell = new PdfPCell(new Phrase(oItem.EmployeeName, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DesignationName, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.EmployeeTypeName, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.JoiningDateInString, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.GrossAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BonusDeclarationDateInString, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                DateTime DeclarationDate = oItem.BonusDeclarationDate.AddDays(1);
                DateDifference oService = new DateDifference(oItem.JoiningDate, DeclarationDate);

                double nTotalDayCount = (DeclarationDate - oItem.JoiningDate).TotalDays;

                _oPdfPCell = new PdfPCell(new Phrase((nTotalDayCount < 365)?nTotalDayCount.ToString() + "d":oService.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                double Percent = oItem.BonusAmount / oItem.GrossAmount * 100;
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(Percent, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.BonusAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                //For BaseAddress Golden<=500
                _oPdfPCell = new PdfPCell(new Phrase((_oCompany.BaseAddress == "golden" && (oItem.BonusAmount <=500)) ? this.GetAmountInStr(0, true, true) : this.GetAmountInStr(10, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase((_oCompany.BaseAddress == "golden" && (oItem.BonusAmount <= 500)) ? this.GetAmountInStr(oItem.BonusAmount, true, true) : this.GetAmountInStr(oItem.BonusAmount - 10, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                int nModBy = 10;

                if ((_nRow % nModBy == 0 || oEmployeeBonuss.Count == nCount))// && _oTempEmployeeBonuss.Count != _nRowCount)
                {
                    if (oEmployeeBonuss.Count == nCount)
                    {
                        PrintDepartmentTotal(nDepartmentID, oItem.BlockName);
                    }

                    if (_oTempEmployeeBonuss.Count == _nRowCount)
                    {
                        PrintGT();
                    }
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();

                    if (_oEmployeeBonuss.Count <= 0)
                    {
                        _oEmployeeBonus = _oTempEmployeeBonuss.Last();
                    }
                    else _oEmployeeBonus = _oEmployeeBonuss[0];


                    if (oEmployeeBonuss.Count != nCount)
                    {
                        this.PrintHeader(_oEmployeeBonus, _oEmployeeBonus.BusinessUnitID);
                        this.PrintHeadRow(_oEmployeeBonus);
                    }
                }
            }
        }
        private void PrintDepartmentTotal(int nDepartmentID, string blockName)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("DEPARTMENT TOTAL", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double TotalGross;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                TotalGross = _oTempEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID && (x.BlockName == blockName)).Sum(x => x.GrossAmount);
            }
            else
            {
                TotalGross = _oTempEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID).Sum(x => x.GrossAmount);
            }

            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalGross, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalPayable;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                nTotalPayable = _oTempEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID && (x.BlockName == blockName)).Sum(x => x.BonusAmount);
            }
            else
            {
                nTotalPayable = _oTempEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID).Sum(x => x.BonusAmount);
            }


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalPayable, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double TotalStamp;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                TotalStamp = _oTempEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID && (x.BlockName == blockName)).ToList().Count * 10;
            }
            else
            {
                TotalStamp = _oTempEmployeeBonuss.Where(x => x.DepartmentID == nDepartmentID).ToList().Count * 10;
            }
            
            //For Golden

            int EmpCountForBelow500;
            if (!string.IsNullOrEmpty(_sBlockIDs))
            {
                EmpCountForBelow500 = _oTempEmployeeBonuss.Where(x => (x.DepartmentID == nDepartmentID && (x.BlockName == blockName)) && (x.BonusAmount <= 500)).ToList().Count();
            }
            else
            {
                EmpCountForBelow500 = _oTempEmployeeBonuss.Where(x => (x.DepartmentID == nDepartmentID) && (x.BonusAmount <= 500)).ToList().Count();
            }

            double TotalStampForGolden = EmpCountForBelow500 * 10;
            
            _oPdfPCell = new PdfPCell(new Phrase((_oCompany.BaseAddress == "golden")?this.GetAmountInStr(TotalStamp - TotalStampForGolden, true, true):this.GetAmountInStr(TotalStamp, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oCompany.BaseAddress == "golden") ? this.GetAmountInStr(nTotalPayable - (TotalStamp - TotalStampForGolden), true, true) : this.GetAmountInStr(nTotalPayable - TotalStamp, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        private void PrintGT()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("GRAND TOTAL", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double TotalGross = _oTempEmployeeBonuss.Sum(x => x.GrossAmount);

            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalGross, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight; _oPdfPCell.Colspan = 3;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalPayable = _oTempEmployeeBonuss.Sum(x => x.BonusAmount);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalPayable, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double TotalStamp = _oTempEmployeeBonuss.Count * 10;
            //For Golden
            int EmpCountForBelow500 = _oTempEmployeeBonuss.Where(x => (x.BonusAmount <= 500)).ToList().Count();
            double TotalStampForGolden = EmpCountForBelow500 * 10;


            _oPdfPCell = new PdfPCell(new Phrase((_oCompany.BaseAddress == "golden") ? this.GetAmountInStr(TotalStamp - TotalStampForGolden, true, true) : this.GetAmountInStr(TotalStamp, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase((_oCompany.BaseAddress == "golden") ? this.GetAmountInStr(nTotalPayable - (TotalStamp - TotalStampForGolden), true, true) : this.GetAmountInStr(nTotalPayable - TotalStamp, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }

        #endregion

        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount) : amount;
            //return (bWithPrecision) ? Global.MillionFormat(amount) : Global.MillionFormat(amount).Split('.')[0];
            return (bWithPrecision) ? Global.MillionFormatActualDigit(amount) : Global.MillionFormatActualDigit(amount).Split('.')[0];
        }
    }

}
