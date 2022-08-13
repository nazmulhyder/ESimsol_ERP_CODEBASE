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
    public class rptSalarySummary_F2
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
        bool _bHasOTAllowance = false;
        bool isRound = true;
        #endregion

        public byte[] PrepareReport(List<SalarySummary_F2> oSalarySummary_F2s, List<SalarySummaryDetail_F2> oSalarySummaryDetail_F2s, List<SalarySheetSignature> oSalarySheetSignatures, List<SalarySheetProperty> oSalarySheetPropertys, bool bRound)
        {
            _oSalarySummary_F2s = oSalarySummary_F2s;
            _oSalarySummaryDetail_F2s = oSalarySummaryDetail_F2s;
            if (oSalarySheetPropertys.Any(x => x.PropertyFor == 2 && x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.OTAllowance))
            {
                _bHasOTAllowance = true;
            }
            isRound = bRound;
            _oAdditionSalaryHeads = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadIDType == EnumSalaryHeadType.Addition).GroupBy(x => x.SalaryHeadID).Select(x => x.First()).OrderBy(x=>x.SalaryHeadSequence).ToList();
            _oDeductionSalaryHeads = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadIDType == EnumSalaryHeadType.Deduction).GroupBy(x => x.SalaryHeadID).Select(x => x.First()).OrderBy(x=>x.SalaryHeadSequence).ToList();
            _oSalarySummary_F2s_Location = oSalarySummary_F2s.GroupBy(x => x.LocationID).Select(x => x.First()).ToList();
            #region Page Setup

            int nCol = 6;
            if (_bHasOTAllowance)
            {
                nCol = 8;
            }
            _nColumns = nCol + _oAdditionSalaryHeads.Count + _oDeductionSalaryHeads.Count + 3;

            float[] tablecolumns = new float[_nColumns];
            int nCount = 0;
            tablecolumns[nCount++] = 15f;
            tablecolumns[nCount++] = 90f;
            tablecolumns[nCount++] = 40f;
            if (_bHasOTAllowance)
            {
                tablecolumns[nCount++] = 40f;
            }
            tablecolumns[nCount++] = 60f;
            if (_bHasOTAllowance)
            {
                tablecolumns[nCount++] = 50f;
            }
            tablecolumns[nCount++] = 60f;
            tablecolumns[nCount++] = 60f;
            int i = 0;
            for (i = nCol; i < _nColumns - 3; i++)
            {
                tablecolumns[i] = 50f;
            }
            //tablecolumns[i++] = 40f;
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
                _oSalarySummary_F2s = _oSalarySummary_F2s.OrderBy(x=>x.LocationName).ThenBy(x => x.DepartmentName).ToList();
                _oSalarySummary_F2s.ForEach(x => _oTempSalarySummary_F2s.Add(x));


                while (_oSalarySummary_F2s.Count > 0)
                {
                    List<SalarySummary_F2> oTempSS = new List<SalarySummary_F2>();
                    oTempSS = _oSalarySummary_F2s.Where(x => x.BusinessUnitID == _oSalarySummary_F2s[0].BusinessUnitID && x.LocationID == _oSalarySummary_F2s[0].LocationID).ToList();

                    this.PrintHeader(oTempSS[0]);
                    this.PrintHeaddRow(oTempSS[0]);
                    
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

            _oPdfPCell = new PdfPCell(new Phrase("BusinessUnit-" + oSS.BusinessUnitName + ", Location-" + oSS.LocationName, _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
           
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 22f, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ManPower", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            
            if (_bHasOTAllowance)
            {
                _oPdfPCell = new PdfPCell(new Phrase("OTHr", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("Salary/Wages", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            if (_bHasOTAllowance)
            {
                _oPdfPCell = new PdfPCell(new Phrase("OT Payable", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPCell = new PdfPCell(new Phrase("Total Payable", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Earnings On Att.", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
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

            //_oPdfPCell = new PdfPCell(new Phrase("Stamp", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

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

                if (_bHasOTAllowance)
                {
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.OTHr.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.GrossSalary, true, true), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                if (_bHasOTAllowance)
                {

                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.OTAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.TotalPayable, true, true), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.EOA, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                List<SalarySummaryDetail_F2> oTempDetails = new List<SalarySummaryDetail_F2>();

                foreach (SalarySummaryDetail_F2 oDetailAddItem in _oAdditionSalaryHeads)
                {
                    oTempDetails = new List<SalarySummaryDetail_F2>();
                    oTempDetails = _oSalarySummaryDetail_F2s.Where(x => x.BusinessUnitID == oItem.BusinessUnitID && x.LocationID == oItem.LocationID && x.DepartmentID == oItem.DepartmentID && x.SalaryHeadID == oDetailAddItem.SalaryHeadID).ToList();
                    _oPdfPCell = new PdfPCell(new Phrase(oTempDetails.Count > 0 ? this.GetAmountInStr(oTempDetails[0].Amount, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }
                foreach (SalarySummaryDetail_F2 oDeductionAddItem in _oDeductionSalaryHeads)
                {
                    oTempDetails = new List<SalarySummaryDetail_F2>();
                    oTempDetails = _oSalarySummaryDetail_F2s.Where(x => x.BusinessUnitID == oItem.BusinessUnitID && x.LocationID == oItem.LocationID && x.DepartmentID == oItem.DepartmentID && x.SalaryHeadID == oDeductionAddItem.SalaryHeadID).ToList();
                    _oPdfPCell = new PdfPCell(new Phrase(oTempDetails.Count > 0 ? this.GetAmountInStr(oTempDetails[0].Amount, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                //_oPdfPCell = new PdfPCell(new Phrase(oItem.Stamp>0?this.GetAmountInStr(oItem.Stamp, true, true):"-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                //double nNetPay = oItem.NetPay;
                //if (_bHasOTAllowance == false)
                //{
                //    double nTotalOTAmount = oItem.OTAmount;
                //    nNetPay -= nTotalOTAmount;
                //}

                _oPdfPCell = new PdfPCell(new Phrase(oItem.NetPay > 0 ? this.GetAmountInStr(Math.Round(oItem.NetPay,0), true, true) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
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

                //this.PrintHeader(oSSs[0]);
                //this.PrintHeaddRow(oSSs[0]);
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
            if (_bHasOTAllowance)
            {

                double TotalOTHour = oSalarySummary_F2s.Sum(x => x.OTHr);
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalOTHour, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            double nTotalGrossSalary = oSalarySummary_F2s.Sum(x => x.GrossSalary);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalGrossSalary, true, true), _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            if (_bHasOTAllowance)
            {

                double nTotalOTAmount = oSalarySummary_F2s.Sum(x => x.OTAmount);
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalOTAmount, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            double nTotalPayable = oSalarySummary_F2s.Sum(x => x.TotalPayable);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalPayable, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nEOA = oSalarySummary_F2s.Sum(x => x.EOA);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nEOA, true, true), _oFontStyle)); _oPdfPCell.FixedHeight = RowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nHAmount = 0;
            foreach (SalarySummaryDetail_F2 oDetailAddItem in _oAdditionSalaryHeads)
            {
                if (sTHead == "GRAND TOTAL")
                {
                    nHAmount = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadID == oDetailAddItem.SalaryHeadID).Sum(x => x.Amount);
                }
                else
                {
                    nHAmount = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadID == oDetailAddItem.SalaryHeadID && x.BusinessUnitID == oSalarySummary_F2s[0].BusinessUnitID && x.LocationID == oSalarySummary_F2s[0].LocationID).Sum(x => x.Amount);
                }
                _oPdfPCell = new PdfPCell(new Phrase(nHAmount > 0 ? this.GetAmountInStr(nHAmount, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            foreach (SalarySummaryDetail_F2 oDeductionAddItem in _oDeductionSalaryHeads)
            {
                if (sTHead == "GRAND TOTAL")
                {
                    nHAmount = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadID == oDeductionAddItem.SalaryHeadID).Sum(x => x.Amount);
                }
                else
                {
                    nHAmount = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadID == oDeductionAddItem.SalaryHeadID && x.BusinessUnitID == oSalarySummary_F2s[0].BusinessUnitID && x.LocationID == oSalarySummary_F2s[0].LocationID).Sum(x => x.Amount);
                }
                _oPdfPCell = new PdfPCell(new Phrase(nHAmount > 0 ? this.GetAmountInStr(nHAmount, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            //double nTotalStamp = oSalarySummary_F2s.Sum(x => x.Stamp);
            //_oPdfPCell = new PdfPCell(new Phrase(nTotalStamp > 0 ? this.GetAmountInStr(nTotalStamp, true, true) : "-", _oFontStyle));  _oPdfPCell.FixedHeight = RowHeight;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalNetPay = oSalarySummary_F2s.Sum(x => x.NetPay);
            //if (_bHasOTAllowance == false)
            //{
            //    double nTotalOTAmount = oSalarySummary_F2s.Sum(x => x.OTAmount);
            //    nTotalNetPay -= nTotalOTAmount;
            //}
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
            amount = (isRound) ? Math.Round(amount, 0) : Math.Round(amount, 2);
            return amount.ToString();
            //return (bWithPrecision) ? Global.MillionFormat(amount).Split('.')[0] : Global.MillionFormat(amount);
        }
    }

}
