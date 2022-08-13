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

    public class rptCompDayAllowance
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        iTextSharp.text.Font _oFontStyleBold;
        PdfPTable _oPdfPTable = new PdfPTable(1);
        PdfPCell _oPdfPCell;
        iTextSharp.text.Image _oImag;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<ExtraBenefit> _oExtraBenefits = new List<ExtraBenefit>();
        List<SalarySheetSignature> _oSalarySheetSignatures = new List<SalarySheetSignature>();
        #endregion

        public byte[] PrepareReport(List<ExtraBenefit> oExtraBenefits, Company oCompany, BusinessUnit oBusinessUnit, List<SalarySheetSignature> oSalarySheetSignatures, string sDateRange)
        {
            _oExtraBenefits = oExtraBenefits;
            _oCompany = oCompany;
            _oBusinessUnit = oBusinessUnit;
            _oSalarySheetSignatures = oSalarySheetSignatures;

            #region Page Setup
            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(15f, 8f, 8f, 90f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, 1);
            //PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });



            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = _oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.nFontSize = 8;
            oPDFWriter.PageEvent = PageEventHandler;
            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 595f });


            #endregion

            ESimSolPdfHelper.PrintHeader_Baly(ref _oPdfPTable, _oBusinessUnit, _oCompany, "Offday Duty Allowance " + sDateRange, 1);
            this.PrintEmptyRow("", 20);
            this.DataList();
            _oPdfPTable.HeaderRows = 3;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        public void DataList()
        {
            PdfPTable oPdfPTable = new PdfPTable(10);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(GetWidth());
            _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
            _oFontStyleBold = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.BOLD);
            int nMinHeight = 22;

            if (_oExtraBenefits.Count > 0)
            {
                _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Employee ID", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Date Of Join", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Salary", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Days", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Per Day", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Payable", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyleBold));
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(oPdfPTable);
                _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            int nCount = 1;
            string sBUName = "";
            string sDepartName = "";
            int nSL = 1;
            //_oExtraBenefits = _oExtraBenefits.OrderBy(x => x.BusinessUnitName).ThenBy(x => x.DepartmentName).ToList();
            var oResultGroup = _oExtraBenefits.GroupBy(x => new { x.BusinessUnitName, x.LocationName, x.DepartmentName, x.EmployeeCode }, (key, grp) => new ExtraBenefit
            {
                EmployeeCode = grp.First().EmployeeCode,
                EmployeeName = grp.First().EmployeeName,
                Days = grp.Count(),
                BusinessUnitName = key.BusinessUnitName,
                LocationName = grp.First().LocationName,
                DepartmentName = grp.First().DepartmentName,
                DesignationName = grp.First().DesignationName,
                JoiningDate = grp.First().JoiningDate,
                Salary = grp.First().Salary,
                PerDayAmount = grp.First().PerDayAmount,
                PayableAmount = grp.First().PerDayAmount * grp.Count()

            }).OrderBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();

            oResultGroup = oResultGroup.OrderBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ThenBy(x => x.EmployeeName).ToList();
            foreach (ExtraBenefit oItem in oResultGroup)
                {

                    PdfPTable oCPdfPTable = new PdfPTable(10);
                    oCPdfPTable.WidthPercentage = 100;
                    oCPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
                    oCPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    oCPdfPTable.SetWidths(GetWidth());

                    if (sBUName != oItem.BusinessUnitName || sDepartName != oItem.DepartmentName)
                    {
                        if (nCount > 1)
                        {
                            PdfPTable oCPdfPTableNew = new PdfPTable(10);
                            oCPdfPTableNew.WidthPercentage = 100;
                            oCPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                            oCPdfPTableNew.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            oCPdfPTableNew.SetWidths(GetWidth());

                            _oPdfPCell = new PdfPCell(new Phrase("Sub Total:", _oFontStyleBold)); _oPdfPCell.Colspan = 5;
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName && x.DepartmentName == sDepartName).Sum(x => x.Salary)).ToString(), _oFontStyle));
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName && x.DepartmentName == sDepartName).Sum(x => x.PerDayAmount)).ToString(), _oFontStyle));
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName && x.DepartmentName == sDepartName).Sum(x => x.PayableAmount)).ToString(), _oFontStyle));
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(oCPdfPTableNew);
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.Colspan = 1;
                            _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                        }
                    }
                    if (sBUName != oItem.BusinessUnitName)
                    {
                        if (nCount > 1)
                        {
                            PdfPTable oCPdfPTableNew = new PdfPTable(10);
                            oCPdfPTableNew.WidthPercentage = 100;
                            oCPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                            oCPdfPTableNew.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            oCPdfPTableNew.SetWidths(GetWidth());


                            _oPdfPCell = new PdfPCell(new Phrase("Unit Total:", _oFontStyleBold)); _oPdfPCell.Colspan = 5;
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName).Sum(x => x.Salary)).ToString(), _oFontStyle));
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName).Sum(x => x.PerDayAmount)).ToString(), _oFontStyle));
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName).Sum(x => x.PayableAmount)).ToString(), _oFontStyle));
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
                            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableNew.AddCell(_oPdfPCell);

                            _oPdfPCell = new PdfPCell(oCPdfPTableNew);
                            _oPdfPCell.Border = 0;
                            _oPdfPCell.Colspan = 1;
                            _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                            _oPdfPTable.CompleteRow();
                        }
                    }
                    if (sBUName != oItem.BusinessUnitName || sDepartName != oItem.DepartmentName)
                    {
                        PdfPTable oCPdfPTableNew = new PdfPTable(10);
                        oCPdfPTableNew.WidthPercentage = 100;
                        oCPdfPTableNew.HorizontalAlignment = Element.ALIGN_LEFT;
                        oCPdfPTableNew.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        oCPdfPTableNew.SetWidths(GetWidth());

                        _oPdfPCell = new PdfPCell(new Phrase("Unit : " + oItem.BusinessUnitName + ", Department : " + oItem.DepartmentName, _oFontStyleBold)); _oPdfPCell.Colspan = 10;
                        _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; oCPdfPTableNew.AddCell(_oPdfPCell);

                        _oPdfPCell = new PdfPCell(oCPdfPTableNew);
                        _oPdfPCell.Border = 0;
                        _oPdfPCell.Colspan = 1;
                        _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                        _oPdfPTable.CompleteRow();
                        nSL = 1;
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(nSL++.ToString(), _oFontStyle)); _oPdfPCell.MinimumHeight = nMinHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.EmployeeCode, _oFontStyle)); _oPdfPCell.MinimumHeight = nMinHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.EmployeeName, _oFontStyle)); _oPdfPCell.MinimumHeight = nMinHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DesignationName, _oFontStyle)); _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.JoiningDateInString, _oFontStyle)); _oPdfPCell.MinimumHeight = nMinHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oItem.Salary).ToString(), _oFontStyle)); _oPdfPCell.MinimumHeight = nMinHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.Days.ToString(), _oFontStyle)); _oPdfPCell.MinimumHeight = nMinHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oItem.PerDayAmount, 0).ToString(), _oFontStyle)); _oPdfPCell.MinimumHeight = nMinHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oItem.PayableAmount, 0).ToString(), _oFontStyle)); _oPdfPCell.MinimumHeight = nMinHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.MinimumHeight = nMinHeight; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(oCPdfPTable);
                    _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    sBUName = oItem.BusinessUnitName;
                    sDepartName = oItem.DepartmentName;
                    nCount = 2;
                }
            #region Last Sub Total
            PdfPTable oCPdfPTableSub = new PdfPTable(10);
            oCPdfPTableSub.WidthPercentage = 100;
            oCPdfPTableSub.HorizontalAlignment = Element.ALIGN_LEFT;
            oCPdfPTableSub.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oCPdfPTableSub.SetWidths(GetWidth());

            _oPdfPCell = new PdfPCell(new Phrase("Sub Total:", _oFontStyleBold)); _oPdfPCell.Colspan = 5;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableSub.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName && x.DepartmentName == sDepartName).Sum(x => x.Salary)).ToString(), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableSub.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableSub.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName && x.DepartmentName == sDepartName).Sum(x => x.PerDayAmount), 0).ToString(), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableSub.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName && x.DepartmentName == sDepartName).Sum(x => x.PayableAmount), 0).ToString(), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableSub.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableSub.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oCPdfPTableSub);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Last Unit Total
            PdfPTable oCPdfPTableUnit = new PdfPTable(10);
            oCPdfPTableUnit.WidthPercentage = 100;
            oCPdfPTableUnit.HorizontalAlignment = Element.ALIGN_LEFT;
            oCPdfPTableUnit.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oCPdfPTableUnit.SetWidths(GetWidth());

            _oPdfPCell = new PdfPCell(new Phrase("Unit Total:", _oFontStyleBold)); _oPdfPCell.Colspan = 5;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableUnit.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName).Sum(x => x.Salary)).ToString(), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableUnit.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableUnit.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName).Sum(x => x.PerDayAmount), 0).ToString(), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableUnit.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Math.Round(oResultGroup.Where(x => x.BusinessUnitName == sBUName).Sum(x => x.PayableAmount), 0).ToString(), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableUnit.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableUnit.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oCPdfPTableUnit);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion

            #region Grand Total
            PdfPTable oCPdfPTableGrand = new PdfPTable(10);
            oCPdfPTableGrand.WidthPercentage = 100;
            oCPdfPTableGrand.HorizontalAlignment = Element.ALIGN_LEFT;
            oCPdfPTableGrand.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oCPdfPTableGrand.SetWidths(GetWidth());

            _oPdfPCell = new PdfPCell(new Phrase("Grand Total:", _oFontStyleBold)); _oPdfPCell.Colspan = 5;
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableGrand.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oExtraBenefits.Sum(x => x.Salary)).ToString(), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableGrand.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableGrand.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oExtraBenefits.Sum(x => x.PerDayAmount)).ToString(), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableGrand.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat_Round(_oExtraBenefits.Sum(x => x.PayableAmount)).ToString(), _oFontStyle));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableGrand.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyleBold));
            _oPdfPCell.Border = iTextSharp.text.Rectangle.BOX; _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oCPdfPTableGrand.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(oCPdfPTableGrand);
            _oPdfPCell.Border = 0;
            _oPdfPCell.Colspan = 1;
            _oPdfPCell.ExtraParagraphSpace = 7f; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            #endregion
        }

        public void PrintEmptyRow(string sString, int nHeight)
        {
            PdfPTable oPdfPTable = new PdfPTable(1);
            oPdfPTable.WidthPercentage = 100;
            oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            oPdfPTable.SetWidths(new float[] { 595f });
            _oFontStyle = FontFactory.GetFont("Tahoma", 11f, iTextSharp.text.Font.NORMAL);

            #region Row
            _oPdfPCell = new PdfPCell(new Phrase(sString, _oFontStyle)); _oPdfPCell.FixedHeight = nHeight;
            _oPdfPCell.Border = 0; _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; oPdfPTable.AddCell(_oPdfPCell);

            oPdfPTable.CompleteRow();
            #endregion

            _oPdfPCell = new PdfPCell(oPdfPTable);
            _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        public float[] GetWidth()
        {
            return new float[] { 5, 13, 18, 15, 12, 10, 7, 8, 8, 21 };
        }
    }
}

