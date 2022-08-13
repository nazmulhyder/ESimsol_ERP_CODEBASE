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
    public class rptSalarySummaryNew_F5
    {
        #region Declaration
        int _nColumns = 0;
        float _nRowHeight = 10;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        PdfPTable _oPdfPTable;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        List<SalarySummary_F2> _oSalarySummarys = new List<SalarySummary_F2>();
        List<SalarySummary_F2> _oTempSalarySummary_F2s = new List<SalarySummary_F2>();
        List<SalarySummaryDetail_F2> _oSalarySummaryDetail_F2s = new List<SalarySummaryDetail_F2>();
        List<SalarySummaryDetail_F2> _oAdditionSalaryHeads = new List<SalarySummaryDetail_F2>();
        List<SalarySummaryDetail_F2> _oDeductionSalaryHeads = new List<SalarySummaryDetail_F2>();
        List<SalarySummary_F2> _oSalarySummary_F2s_Location = new List<SalarySummary_F2>();
        Company _oCompany = new Company();
        bool _bHasOTAllowance = false;
        bool isRound = true;
        #endregion

        public byte[] PrepareReport(List<SalarySummary_F2> oSalarySummarys, List<SalarySummaryDetail_F2> oSalarySummaryDetail_F2s, List<SalarySheetSignature> oSalarySheetSignatures, List<SalarySheetProperty> oSalarySheetPropertys, bool bRound)
        {
            _oSalarySummarys = oSalarySummarys;
            _oSalarySummaryDetail_F2s = oSalarySummaryDetail_F2s;
            if (oSalarySheetPropertys.Any(x => x.PropertyFor == 2 && x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.OTAllowance))
            {
                _bHasOTAllowance = true;
            }
            isRound = bRound;
            _oAdditionSalaryHeads = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadIDType == EnumSalaryHeadType.Addition).GroupBy(x => x.SalaryHeadID).Select(x => x.First()).OrderBy(x=>x.SalaryHeadSequence).ToList();
            _oDeductionSalaryHeads = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadIDType == EnumSalaryHeadType.Deduction && (x.SalaryHeadID != 8 && x.SalaryHeadID != 20 && x.SalaryHeadID != 25 && x.SalaryHeadID != 26)).GroupBy(x => x.SalaryHeadID).Select(x => x.First()).OrderBy(x => x.SalaryHeadSequence).ToList();            
            _oSalarySummary_F2s_Location = oSalarySummarys.GroupBy(x => x.LocationID).Select(x => x.First()).ToList();
            #region Page Setup

            int nCol = 6;
            if (_bHasOTAllowance)
            {
                nCol = 8;
            }
            _nColumns = nCol + _oAdditionSalaryHeads.Count + _oDeductionSalaryHeads.Count + 3;

            float[] tablecolumns = new float[_nColumns];
            int nCount = 0;
            tablecolumns[nCount++] = 25f;
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
            tablecolumns[i++] = 60f;
            tablecolumns[i++] = 50f;
            tablecolumns[i++] = 60f;

            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(iTextSharp.text.PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);//842*595 for A4 Size 
            _oDocument.SetMargins(10f, 10f, 5f, 35f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);
            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.nFontSize = 10;
            oPDFWriter.PageEvent = PageEventHandler;

            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);
            #endregion
                        
            this.PrintBody();            
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report & Column Header 
        private void PrintHeader(SalarySummary_F2 oSalarySummary_F2)
        {
            #region CompanyHeader
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oSalarySummary_F2.BusinessUnitName, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(oSalarySummary_F2.BusinessUnitAddress, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SALARY SUMMARY", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
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
        private void PrintHeaddRow(SalarySummary_F2 oSalarySummary)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("BusinessUnit-" + oSalarySummary.BusinessUnitName + ", Location-" + oSalarySummary.LocationName, _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("SL", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Department", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("ManPower", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            if (_bHasOTAllowance)
            {
                _oPdfPCell = new PdfPCell(new Phrase("OTHr", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPCell = new PdfPCell(new Phrase("Salary/Wages", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            if (_bHasOTAllowance)
            {
                _oPdfPCell = new PdfPCell(new Phrase("OT Payable", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPCell = new PdfPCell(new Phrase("Total Payable", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Earnings", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Colspan = _oAdditionSalaryHeads.Count;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Earnings On Att.", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Deduction", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Colspan = _oDeductionSalaryHeads.Count;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Net Payable", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Bank", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Cash", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight; _oPdfPCell.Rowspan = 2;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
            
            foreach (SalarySummaryDetail_F2 oItem in _oAdditionSalaryHeads)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            foreach (SalarySummaryDetail_F2 oItem in _oDeductionSalaryHeads)
            {
                _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            }
            _oPdfPTable.CompleteRow();
        }
        #endregion

        #region Report Body
        private void PrintBody()
        {
            _nRowHeight = 12f;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            if (_oSalarySummarys.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
                _oPdfPCell = new PdfPCell(new Phrase("There is no data to print !!", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {                
                int nBUID = 0; int nLocationID = 0;
                bool bIsPrintSubTotal = false; int nCount = 0;
                List<SalarySummary_F2> oTempSalarySummarys = new List<SalarySummary_F2>();
                foreach (SalarySummary_F2 oItem in _oSalarySummarys)
                {
                    if (oItem.BusinessUnitID != nBUID || oItem.LocationID != nLocationID)
                    {
                        if (bIsPrintSubTotal)
                        {
                            oTempSalarySummarys = new List<SalarySummary_F2>();
                            oTempSalarySummarys = _oSalarySummarys.Where(x => x.BusinessUnitID == nBUID && x.LocationID == nLocationID).ToList();
                            this.PrintGT(oTempSalarySummarys, "TOTAL");

                            _oDocument.Add(_oPdfPTable);
                            _oDocument.NewPage();
                            _oPdfPTable.DeleteBodyRows();
                        }
                        this.PrintHeader(oItem);
                        this.PrintHeaddRow(oItem);                        
                        nCount = 0;
                    }

                    nCount++;
                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.NORMAL);
                    _oPdfPCell = new PdfPCell(new Phrase(nCount.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.DepartmentName, _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.NoOfEmp.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    if (_bHasOTAllowance)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase(oItem.OTHr.ToString(), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.GrossSalary, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    if (_bHasOTAllowance)
                    {

                        _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.OTAmount, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.TotalPayable, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    List<SalarySummaryDetail_F2> oTempDetails = new List<SalarySummaryDetail_F2>();
                    foreach (SalarySummaryDetail_F2 oDetailAddItem in _oAdditionSalaryHeads)
                    {
                        oTempDetails = new List<SalarySummaryDetail_F2>();
                        oTempDetails = _oSalarySummaryDetail_F2s.Where(x => x.BusinessUnitID == oItem.BusinessUnitID && x.LocationID == oItem.LocationID && x.DepartmentID == oItem.DepartmentID && x.SalaryHeadID == oDetailAddItem.SalaryHeadID).ToList();
                        _oPdfPCell = new PdfPCell(new Phrase(oTempDetails.Count > 0 ? this.GetAmountInStr(oTempDetails[0].Amount, true, false) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }

                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oItem.EOA, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    foreach (SalarySummaryDetail_F2 oDeductionAddItem in _oDeductionSalaryHeads)
                    {
                        oTempDetails = new List<SalarySummaryDetail_F2>();
                        oTempDetails = _oSalarySummaryDetail_F2s.Where(x => x.BusinessUnitID == oItem.BusinessUnitID && x.LocationID == oItem.LocationID && x.DepartmentID == oItem.DepartmentID && x.SalaryHeadID == oDeductionAddItem.SalaryHeadID).ToList();
                        _oPdfPCell = new PdfPCell(new Phrase(oTempDetails.Count > 0 ? this.GetAmountInStr(oTempDetails[0].Amount, true, false) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    }
                    _oPdfPCell = new PdfPCell(new Phrase(oItem.NetPay > 0 ? this.GetAmountInStr(Math.Round(oItem.NetPay, 0), true, false) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.BankPay > 0 ? this.GetAmountInStr(oItem.BankPay, true, false) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    _oPdfPCell = new PdfPCell(new Phrase(oItem.CashPay > 0 ? this.GetAmountInStr(oItem.CashPay, true, false) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                    if (nCount % 30 == 0)
                    {
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();

                        this.PrintHeader(oItem);
                        this.PrintHeaddRow(oItem);
                    }

                    nBUID = oItem.BusinessUnitID;
                    nLocationID = oItem.LocationID;
                    bIsPrintSubTotal = true;
                }
                oTempSalarySummarys = new List<SalarySummary_F2>();
                oTempSalarySummarys = _oSalarySummarys.Where(x => x.BusinessUnitID == nBUID && x.LocationID == nLocationID).ToList();
                this.PrintGT(oTempSalarySummarys, "TOTAL");
                this.PrintGT(_oSalarySummarys, "GRAND TOTAL");
            }
            



            _oPdfPCell = new PdfPCell(new Phrase(" ", _oFontStyle)); _oPdfPCell.Colspan = _nColumns; _oPdfPCell.Border = 0; _oPdfPCell.FixedHeight = 30;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();
        }
        private void PrintGT(List<SalarySummary_F2> oSalarySummarys, string sTHead)
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 6.5f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sTHead, _oFontStyle)); _oPdfPCell.Colspan = 2; _oPdfPCell.FixedHeight = _nRowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double TotalNoOfEmp = oSalarySummarys.Sum(x => x.NoOfEmp);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalNoOfEmp, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            if (_bHasOTAllowance)
            {

                double TotalOTHour = oSalarySummarys.Sum(x => x.OTHr);
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(TotalOTHour, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            double nTotalGrossSalary = oSalarySummarys.Sum(x => x.GrossSalary);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalGrossSalary, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            if (_bHasOTAllowance)
            {

                double nTotalOTAmount = oSalarySummarys.Sum(x => x.OTAmount);
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalOTAmount, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }
            double nTotalPayable = oSalarySummarys.Sum(x => x.TotalPayable);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nTotalPayable, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nHAmount = 0;
            foreach (SalarySummaryDetail_F2 oAdditionItem in _oAdditionSalaryHeads)
            {
                if (sTHead == "GRAND TOTAL")
                {
                    nHAmount = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadID == oAdditionItem.SalaryHeadID).Sum(x => x.Amount);
                }
                else
                {
                    nHAmount = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadID == oAdditionItem.SalaryHeadID && x.BusinessUnitID == oSalarySummarys[0].BusinessUnitID && x.LocationID == oSalarySummarys[0].LocationID).Sum(x => x.Amount);
                }
                _oPdfPCell = new PdfPCell(new Phrase(nHAmount > 0 ? this.GetAmountInStr(nHAmount, true, false) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            double nEOA = oSalarySummarys.Sum(x => x.EOA);
            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nEOA, true, false), _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            foreach (SalarySummaryDetail_F2 oDeductionItem in _oDeductionSalaryHeads)
            {                
                if (sTHead == "GRAND TOTAL")
                {
                    nHAmount = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadID == oDeductionItem.SalaryHeadID).Sum(x => x.Amount);
                }
                else
                {
                    nHAmount = _oSalarySummaryDetail_F2s.Where(x => x.SalaryHeadID == oDeductionItem.SalaryHeadID && x.BusinessUnitID == oSalarySummarys[0].BusinessUnitID && x.LocationID == oSalarySummarys[0].LocationID).Sum(x => x.Amount);
                }
                _oPdfPCell = new PdfPCell(new Phrase(nHAmount > 0 ? this.GetAmountInStr(nHAmount, true, false) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            }

            double nTotalNetPay = oSalarySummarys.Sum(x => x.NetPay);
            _oPdfPCell = new PdfPCell(new Phrase(nTotalNetPay > 0 ? this.GetAmountInStr(nTotalNetPay, true, false) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalBankPay = oSalarySummarys.Sum(x => x.BankPay);
            _oPdfPCell = new PdfPCell(new Phrase(nTotalBankPay > 0 ? this.GetAmountInStr(nTotalBankPay, true, false) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            double nTotalCashPay = oSalarySummarys.Sum(x => x.CashPay);
            _oPdfPCell = new PdfPCell(new Phrase(nTotalCashPay > 0 ? this.GetAmountInStr(nTotalCashPay, true, false) : "-", _oFontStyle)); _oPdfPCell.FixedHeight = _nRowHeight;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

        }
        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount) : amount;
            return (bWithPrecision) ? Global.MillionFormat(amount) : Global.MillionFormat(amount).Split('.')[0];
        }
        #endregion
        
    }

}
