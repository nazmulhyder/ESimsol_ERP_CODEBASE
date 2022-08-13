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
    public class rptOTSummary
    {
        #region Declaration
        int _nColumns = 0;
        int _nPageWidth = 2800;
        int _npageHeight = 1700;
        int RowHeight = 45;
        PdfPTable _oPdfPTable = new PdfPTable(6);
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<OTExceptCompliance> _oOTExceptCompliances = new List<OTExceptCompliance>();
        List<OTExceptCompliance> _oTempOTExceptCompliances = new List<OTExceptCompliance>();
        Company _oCompany = new Company();
        List<PayrollProcessManagement> _oPayrollProcessManagements = new List<PayrollProcessManagement>();
        string _sStartDate = "";
        string _sEndDate = "";
        bool _bExceptComp = false;
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();
        bool _isComp;

        #endregion

        public byte[] PrepareReport(List<OTExceptCompliance> oOTExceptCompliances, Company oCompany, List<PayrollProcessManagement> oPayrollProcessManagements, string sDate, List<SalarySheetSignature> oSalarySheetSignatures, bool bExceptComp, List<BusinessUnit> oBusinessUnits, bool IsComp)
        {
            _isComp = IsComp;
            _oOTExceptCompliances = oOTExceptCompliances;
            _oCompany = oCompany;
            _oPayrollProcessManagements = oPayrollProcessManagements;
            DateTime sStartDate = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime sEndDate = Convert.ToDateTime(sDate.Split(',')[1]);
            _sStartDate = sStartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");
            _bExceptComp = bExceptComp;
            _oBusinessUnits = oBusinessUnits;
            #region Page Setup

            _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
            _oDocument.SetMargins(30f, 30f, 10f, 70f);
            _oPdfPTable.WidthPercentage = 97;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.nFontSize = 7;
            oPDFWriter.PageEvent = PageEventHandler;

            _oDocument.Open();
            _oPdfPTable.SetWidths(new float[] { 28f, 150f, 100f, 97f, 80f, 80f});
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(OTExceptCompliance oOTExceptCompliance)
        {
            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oOTExceptCompliance.BusinessUnitName, _oFontStyle));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oOTExceptCompliance.BusinessUnitAddress, _oFontStyle));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("OT SUMMARY", _oFontStyle));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = 11;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Salary Month-" + (_oPayrollProcessManagements.Count > 0 ? _oPayrollProcessManagements[0].MonthIDInString : ""), _oFontStyle));
            _oPdfPCell.Colspan = 11;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = 6;
            _oPdfPCell.Border = 0;
            _oPdfPCell.FixedHeight = 7;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body
        //float 50f = 50f;
        private void PrintBody()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);

            if (_oOTExceptCompliances.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
                _oPdfPCell = new PdfPCell(new Phrase("There is no data to print !!", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            else
            {
                _oOTExceptCompliances = _oOTExceptCompliances.OrderBy(x=>x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
                //_oOTExceptCompliances.ForEach(x => _oTempOTExceptCompliances.Add(x));
                _oTempOTExceptCompliances = _oOTExceptCompliances;
                var OTList = _oOTExceptCompliances.GroupBy(x => new { x.BusinessUnitName, x.DepartmentName }, (key, grp) => new OTExceptCompliance
                {
                    LocationName = grp.First().LocationName,
                    BusinessUnitID = grp.First().BusinessUnitID,
                    LocationID = grp.First().LocationID,
                    BusinessUnitName = grp.First().BusinessUnitName,
                    DepartmentName = grp.First().DepartmentName,
                    OTGroupList = grp.ToList()
                }).OrderBy(x => x.BusinessUnitName).ThenBy(x=>x.DepartmentName).ToList();

                while (_oOTExceptCompliances.Count > 0)
                {
                    List<OTExceptCompliance> oTempSS = new List<OTExceptCompliance>();
                    oTempSS = OTList.Where(x => x.BusinessUnitID == _oOTExceptCompliances[0].BusinessUnitID && x.LocationID == _oOTExceptCompliances[0].LocationID).ToList();

                    this.PrintHeader(oTempSS[0]);
                    this.PrintHeaddRow(oTempSS[0]);

                    PrintOTSummary_F2(oTempSS);

                    _oOTExceptCompliances.RemoveAll(x => x.BusinessUnitID == oTempSS[0].BusinessUnitID && x.LocationID == oTempSS[0].LocationID);
                }
            }

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        #endregion
        private void PrintHeaddRow(OTExceptCompliance oSS)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("BusinessUnit-" + oSS.BusinessUnitName + ", Location-" + oSS.LocationName, _oFontStyle)); _oPdfPCell.Colspan = 6; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ManPower For OT", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Salary/Wages", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            
            _oPdfPCell = new PdfPCell(new Phrase("OTHr", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OT Payable", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPTable.CompleteRow();

        }
        int _nRowCount = 0;
        private void PrintOTSummary_F2(List<OTExceptCompliance> oSSs)
        {
            int nCount = 0;
            foreach (OTExceptCompliance oItem in oSSs)
            {
                nCount++;
                _nRowCount++;
                _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.NORMAL);

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DepartmentName, _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.OTGroupList.Count().ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                double nGrossAmount = 0.0; ;
                if(_isComp) {
                   nGrossAmount = Convert.ToDouble(oItem.OTGroupList.Sum(x => x.CompGrossAmount));
                }
                else
                {
                    nGrossAmount = Convert.ToDouble(oItem.OTGroupList.Sum(x => x.GrossAmount));
                }

                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nGrossAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                double nOTHr = 0.0;
                if (_isComp)
                {
                    nOTHr = Convert.ToDouble(oItem.OTGroupList.Sum(x => x.CompOTHour));
                }
                else
                {
                    nOTHr = Convert.ToDouble(oItem.OTGroupList.Sum(x => x.OTHour));
                }
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nOTHr, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                double nOTAmount = 0.0;
                if (_isComp)
                {
                    nOTAmount = Convert.ToDouble(oItem.OTGroupList.Sum(x => x.CompOTAmount));
                }
                else
                {
                    nOTAmount = Convert.ToDouble(oItem.OTGroupList.Sum(x => x.OTAmount));
                }

                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nOTAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                if (nCount % 11 == 0)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();

                    this.PrintHeader(oSSs[0]);
                    this.PrintHeaddRow(oSSs[0]);
                }

            }

            if (_oTempOTExceptCompliances.Count != _nRowCount)
            {
                this.PrintGT(oSSs, "TOTAL");
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();

                //this.PrintHeader(oSSs[0]);
                //this.PrintHeaddRow(oSSs[0]);
            }
            //else
            //{
            //    if (_oSalarySummary_F2s_Location.Count > 1) { this.PrintGT(oSSs, "TOTAL"); }
            //    this.PrintGT(_oTempSalarySummary_F2s, "GRAND TOTAL");
            //}
        }
        private void PrintGT(List<OTExceptCompliance> oOTExceptCompliances, string sTHead)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase(sTHead, _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           
            double TotalNoOfEmp = oOTExceptCompliances.Sum(x => x.OTGroupList.Count());
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalNoOfEmp, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double TotalSalary = 0.0;
            if (_isComp)
            {
                TotalSalary = oOTExceptCompliances.Sum(x => x.OTGroupList.Sum(p => p.CompGrossAmount));
            }
            else
            {
                TotalSalary = oOTExceptCompliances.Sum(x => x.OTGroupList.Sum(p => p.GrossAmount));
            }
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalSalary, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double TotalOTHR = 0.0;
            if (_isComp)
            {
                TotalOTHR = oOTExceptCompliances.Sum(x => x.OTGroupList.Sum(p => p.CompOTHour));
            }
            else
            {
                TotalOTHR = oOTExceptCompliances.Sum(x => x.OTGroupList.Sum(p => p.OTHour));
            }
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalOTHR, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double TotalOTAmount = 0.0;
            if (_isComp)
            {
                TotalOTAmount = oOTExceptCompliances.Sum(x => x.OTGroupList.Sum(p => p.CompOTAmount));
            }
            else
            {
                TotalOTAmount = oOTExceptCompliances.Sum(x => x.OTGroupList.Sum(p => p.OTAmount));
            }
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalOTAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }
        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = Math.Round(amount, 0);
            return amount.ToString();
            //return (bWithPrecision) ? Global.MillionFormat(amount).Split('.')[0] : Global.MillionFormat(amount);
        }
    }

}
