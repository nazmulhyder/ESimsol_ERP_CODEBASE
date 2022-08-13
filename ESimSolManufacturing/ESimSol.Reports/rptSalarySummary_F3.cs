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
    public class rptSalarySummary_F3
    {
        #region Declaration
        int _nColumns = 0;
        int _nPageWidth = 2800;
        int _npageHeight = 1700;
        int RowHeight = 80;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<SalarySummary_F2> _oSalarySummary_F2s = new List<SalarySummary_F2>();
        List<SalarySummary_F2> _oTempSalarySummary_F2s = new List<SalarySummary_F2>();
        List<SalarySummaryDetail_F2> _oSalarySummaryDetail_F2s = new List<SalarySummaryDetail_F2>();
        List<SalarySummaryDetail_F2> _oAdditionSalaryHeads = new List<SalarySummaryDetail_F2>();
        List<SalarySummaryDetail_F2> _oDeductionSalaryHeads = new List<SalarySummaryDetail_F2>();
        List<SalarySummary_F2> _oSalarySummary_F2s_Location = new List<SalarySummary_F2>();
        Company _oCompany = new Company();

        #endregion

        public byte[] PrepareReport(List<SalarySummary_F2> oSalarySummary_F2s, List<SalarySummaryDetail_F2> oSalarySummaryDetail_F2s,List<SalarySheetSignature> oSalarySheetSignatures)
        {
            _oSalarySummary_F2s = oSalarySummary_F2s;
            _oSalarySummaryDetail_F2s = oSalarySummaryDetail_F2s;

            _oAdditionSalaryHeads = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadIDType == EnumSalaryHeadType.Addition).GroupBy(x => x.SalaryHeadID).Select(x => x.First()).ToList();
            _oDeductionSalaryHeads = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadIDType == EnumSalaryHeadType.Deduction).GroupBy(x => x.SalaryHeadID).Select(x => x.First()).ToList();
            _oSalarySummary_F2s_Location = oSalarySummary_F2s.GroupBy(x => x.LocationID).Select(x => x.First()).ToList();
            #region Page Setup

            _nColumns = 7 + _oAdditionSalaryHeads.Count + _oDeductionSalaryHeads.Count+4;

            float[] tablecolumns = new float[_nColumns];

            tablecolumns[0] = 15f;
            tablecolumns[1] = 90f;
            tablecolumns[2] = 40f;
            tablecolumns[3] = 40f;
            tablecolumns[4] = 60f;
            tablecolumns[5] = 50f;
            tablecolumns[6] = 60f;
            int i = 0;
            for (i = 7; i < _nColumns-4; i++)
            {
                tablecolumns[i] = 50f;
            }
            tablecolumns[i++] = 40f;
            tablecolumns[i++] = 60f;
            tablecolumns[i++] = 50f;
            tablecolumns[i++] = 60f;

            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);
            _oDocument.SetMargins(10f, 10f, 5f, 35f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.nFontSize = 30;
            oPDFWriter.PageEvent = PageEventHandler;

            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);
            #endregion

            //this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 5;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header
        private void PrintHeader(SalarySummary_F2 oSalarySummary_F2)
        {
            #region CompanyHeader

            _oFontStyle = FontFactory.GetFont("Tahoma", 40f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oSalarySummary_F2.BusinessUnitName, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oSalarySummary_F2.BusinessUnitAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 35f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SALARY SUMMARY", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 25f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + oSalarySummary_F2.StartDateInString + " To " + oSalarySummary_F2.EndDateInString, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oPdfPCell = new PdfPCell(new Phrase(" "));
            _oPdfPCell.Colspan = _nColumns;
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

            if (_oSalarySummary_F2s.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
                _oPdfPCell = new PdfPCell(new Phrase("There is no data to print !!", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

            }
            else
            {
                _oSalarySummary_F2s = _oSalarySummary_F2s.OrderBy(x => x.DepartmentName).ToList();
                _oSalarySummary_F2s.ForEach(x => _oTempSalarySummary_F2s.Add(x));

                this.PrintHeader(_oSalarySummary_F2s[0]);
                this.PrintHeaddRow(_oSalarySummary_F2s[0]);

                while (_oSalarySummary_F2s.Count > 0)
                {
                    List<SalarySummary_F2> oTempSS = new List<SalarySummary_F2>();
                    oTempSS = _oSalarySummary_F2s.Where(x => x.BusinessUnitID == _oSalarySummary_F2s[0].BusinessUnitID && x.LocationID == _oSalarySummary_F2s[0].LocationID).ToList();
                    PrintSalarySummary_F2(oTempSS);
                    _oSalarySummary_F2s.RemoveAll(x => x.BusinessUnitID == oTempSS[0].BusinessUnitID && x.LocationID == oTempSS[0].LocationID);
                }
            }

            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        #endregion
        private void PrintHeaddRow(SalarySummary_F2 oSS)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 25f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("Unit-" + oSS.LocationName, _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 22f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ManPower", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OTHr", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Salary/Wages", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OT Payable", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Total Payable", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            foreach(SalarySummaryDetail_F2 oItem in _oAdditionSalaryHeads)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            foreach (SalarySummaryDetail_F2 oItem in _oDeductionSalaryHeads)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }

            _oPdfPCell = new PdfPCell(new Phrase("Stamp", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Net Payable", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bank", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cash", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
         
        }
        int _nRowCount = 0;
        private void PrintSalarySummary_F2(List<SalarySummary_F2> oSSs)
        {
            int nCount = 0;
            foreach (SalarySummary_F2 oItem in oSSs)
            {
                nCount++;
                _nRowCount++;
                _oFontStyle = FontFactory.GetFont("Tahoma", 23f, iTextSharp.text.Font.NORMAL);

                _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.DepartmentName, _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.NoOfEmp.ToString(), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CompOTHr.ToString(), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.CompGrossSalary, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.CompOTAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.CompTotalPayable, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                List<SalarySummaryDetail_F2> oTempDetails = new List<SalarySummaryDetail_F2>();

                foreach (SalarySummaryDetail_F2 oDetailAddItem in _oAdditionSalaryHeads)
                {
                    oTempDetails = new List<SalarySummaryDetail_F2>();
                    oTempDetails = _oSalarySummaryDetail_F2s.Where(x => x.BusinessUnitID == oItem.BusinessUnitID && x.LocationID == oItem.LocationID && x.DepartmentID == oItem.DepartmentID && x.SalaryHeadID == oDetailAddItem.SalaryHeadID).ToList();
                    _oPdfPCell = new PdfPCell(new Phrase(oTempDetails.Count > 0 ? this.GetAmountInStr(oTempDetails[0].CompAmount, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                foreach (SalarySummaryDetail_F2 oDeductionAddItem in _oDeductionSalaryHeads)
                {
                    oTempDetails = new List<SalarySummaryDetail_F2>();
                    oTempDetails = _oSalarySummaryDetail_F2s.Where(x => x.BusinessUnitID == oItem.BusinessUnitID && x.LocationID == oItem.LocationID && x.DepartmentID == oItem.DepartmentID && x.SalaryHeadID == oDeductionAddItem.SalaryHeadID).ToList();
                    _oPdfPCell = new PdfPCell(new Phrase(oTempDetails.Count > 0 ? this.GetAmountInStr(oTempDetails[0].CompAmount, true, true) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                _oPdfPCell = new PdfPCell(new Phrase(oItem.Stamp>0?this.GetAmountInStr(oItem.Stamp, true, true):"-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.NetPay>0?this.GetAmountInStr(oItem.CompNetPay, true, true):"-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.BankPay>0?this.GetAmountInStr(oItem.BankPay, true, true):"-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(oItem.CashPay>0?this.GetAmountInStr(oItem.CashPay, true, true):"-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPTable.CompleteRow();

                if (nCount % 14 == 0)
                {
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();

                    this.PrintHeader(oSSs[0]);
                    this.PrintHeaddRow(oSSs[0]);
                }
               
            }
           
            if (_oTempSalarySummary_F2s.Count != _nRowCount)
            {
                this.PrintGT(oSSs,"TOTAL");
                _oDocument.Add(_oPdfPTable);
                _oDocument.NewPage();
                _oPdfPTable.DeleteBodyRows();

                this.PrintHeader(oSSs[0]);
                this.PrintHeaddRow(oSSs[0]);
            }
            else
            {
                if (_oSalarySummary_F2s_Location.Count > 1) { this.PrintGT(oSSs,"TOTAL"); }
                this.PrintGT(_oTempSalarySummary_F2s,"GRAND TOTAL");
            }
        }
        private void PrintGT(List<SalarySummary_F2> oSalarySummary_F2s, string sTHead)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 23f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase(sTHead, _oFontStyle)); _oPdfPCell.Colspan = 2;  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double TotalNoOfEmp = oSalarySummary_F2s.Sum(x => x.NoOfEmp);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalNoOfEmp, true, false), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double TotalOTHour = oSalarySummary_F2s.Sum(x => x.CompOTHr);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalOTHour, true, true), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalGrossSalary = oSalarySummary_F2s.Sum(x => x.CompGrossSalary);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalGrossSalary, true, true), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalOTAmount = oSalarySummary_F2s.Sum(x => x.CompOTAmount);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalOTAmount, true, true), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalPayable = oSalarySummary_F2s.Sum(x => x.CompTotalPayable);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalPayable, true, true), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nHAmount = 0;
            foreach (SalarySummaryDetail_F2 oDetailAddItem in _oAdditionSalaryHeads)
            {
                nHAmount = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadID == oDetailAddItem.SalaryHeadID && x.BusinessUnitID == oSalarySummary_F2s[0].BusinessUnitID && x.LocationID == oSalarySummary_F2s[0].LocationID).Sum(x => x.CompAmount);
                _oPdfPCell = new PdfPCell(new Phrase(nHAmount > 0 ? this.GetAmountInStr(nHAmount, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            foreach (SalarySummaryDetail_F2 oDeductionAddItem in _oDeductionSalaryHeads)
            {
                nHAmount = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadID == oDeductionAddItem.SalaryHeadID && x.BusinessUnitID == oSalarySummary_F2s[0].BusinessUnitID && x.LocationID == oSalarySummary_F2s[0].LocationID).Sum(x => x.CompAmount);
                _oPdfPCell = new PdfPCell(new Phrase(nHAmount > 0 ? this.GetAmountInStr(nHAmount, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            double nTotalStamp = oSalarySummary_F2s.Sum(x => x.Stamp);
            _oPdfPCell = new PdfPCell(new Phrase(nTotalStamp > 0 ? this.GetAmountInStr(nTotalStamp, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalNetPay = oSalarySummary_F2s.Sum(x => x.CompNetPay);
            _oPdfPCell = new PdfPCell(new Phrase(nTotalNetPay > 0 ? this.GetAmountInStr(nTotalNetPay, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalBankPay = oSalarySummary_F2s.Sum(x => x.BankPay);
            _oPdfPCell = new PdfPCell(new Phrase(nTotalBankPay > 0 ? this.GetAmountInStr(nTotalBankPay, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalCashPay = oSalarySummary_F2s.Sum(x => x.CashPay);
            _oPdfPCell = new PdfPCell(new Phrase(nTotalCashPay > 0 ? this.GetAmountInStr(nTotalCashPay, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

        }
        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount) : amount;
            return (bWithPrecision) ? Global.MillionFormat(amount) : Global.MillionFormat(amount).Split('.')[0];
        }
    }

}

