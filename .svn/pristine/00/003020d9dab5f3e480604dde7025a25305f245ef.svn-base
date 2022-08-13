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
    public class rptSalarySheetF03
    {
        #region Declaration
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        int _nColumns = 0;
        int _nPageWidth = 2800;
        int _npageHeight = 1800;
        PdfPTable _oPdfPTable;
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();
        Company _oCompany = new Company();
        List<EmployeeSalaryV2> _oEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
        List<EmployeeSalaryV2> _oSummaryEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
        List<EmployeeSalaryV2> _oTempEmployeeSalaryV2s = new List<EmployeeSalaryV2>();
        List<LeaveHead> _oLeaveHeads = new List<LeaveHead>();
        List<EmployeeSalaryDetailV2> _oEmployeeSalaryBasics = new List<EmployeeSalaryDetailV2>();
        List<EmployeeSalaryDetailV2> _oEmployeeSalaryAllowances = new List<EmployeeSalaryDetailV2>();
        List<EmployeeSalaryDetailV2> _oEmployeeSalaryDeductions = new List<EmployeeSalaryDetailV2>();
        SalaryField _oSalaryField = new SalaryField();
        EnumPageOrientation _ePageOrientation = EnumPageOrientation.None;
        string _sStartDate = "";
        string _sEndDate = "";
        bool _IsOT = false, _isDynamic = false, _isPortrait=false;
        bool _bIsSerialWise = false;
        double _SubTGrossEarnings = 0, _GrandTGrossEarnings = 0, _GrandTOTAmt = 0, _SubTOTAmt = 0,_GrandTOTHour = 0, _SubTOTHour = 0, _subTBasic = 0, _GrandTBasic = 0;
        DateTime _StartDate = DateTime.Now;
        float _nHeight = 200f, _nFontSize = 0f, _nFontHeader = 0f, _nFooterFont = 7f, _nBUFont = 15f, _nBUlocafont = 8f, _nSalarySheetFont = 12f, _DateFont = 10f, _UnitDeptFont = 12f;
        int _nTotalColumns = 0;
        int _nCount = 0;
        int _nGroupBySerial = 0;
        #endregion
        public byte[] PrepareReport(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<LeaveHead> oLeaveHeads, List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, List<EmployeeSalaryDetailV2> oEmployeeSalaryAllowances, List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions, Company oCompany, SalaryField oSalaryField, EnumPageOrientation ePageOrientation, List<SalarySheetSignature> oSalarySheetSignatures, string StartDate, string EndDate, bool IsOT)
        {
            _oCompany = oCompany;
            _oEmployeeSalaryV2s = oEmployeeSalaryV2s;
            _oLeaveHeads = oLeaveHeads;
            _oEmployeeSalaryBasics = oEmployeeSalaryBasics;
            _oEmployeeSalaryAllowances = oEmployeeSalaryAllowances;
            _oEmployeeSalaryDeductions = oEmployeeSalaryDeductions;
            _oSalaryField = oSalaryField;
            _ePageOrientation = ePageOrientation;
            _StartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            _bIsSerialWise = nGroupBySerial == 2 ? true : false;
            _IsOT = IsOT;
            _sStartDate = _StartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");
            int nColumn = 0;
            _nGroupBySerial = nGroupBySerial;



            _oSalaryField.FixedColumn = 13;//Employee basic Info,Gross Earnings,Gross Deductions,Net Amount,Signature

            _nTotalColumns = _oEmployeeSalaryBasics.Count() + _oSalaryField.FixedColumn;
            _nColumns = _nTotalColumns;
            float[] tablecolumns = new float[_nTotalColumns];
            tablecolumns[nColumn++] = 40f;


            for (var i = 0; i < 7; i++)//Employee Informations
            {
                tablecolumns[nColumn++] = 90f;
            }

            int nBasic = _oEmployeeSalaryBasics.Count();
            for (var i = 0; i < nBasic; i++)//Basic
            {
                tablecolumns[nColumn++] = 70f;
            }


            for (var i = 0; i < 4; i++)//Gross salary,OT Rate,Extra OT,OT Amount
            {
                tablecolumns[nColumn++] = 60f;
            }
            tablecolumns[nColumn++] = 100f;//Signature
            #region Page Setup
            _oPdfPTable = new PdfPTable(_nTotalColumns);
            if (_ePageOrientation == EnumPageOrientation.A4_Landscape)
            {
                _oDocument = new Document(PageSize.A4.Rotate(), 0f, 0f, 0f, 0f);
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                _nHeight = 25f;
                _nFontSize = 6f;
                _nFontHeader = 6f;
            }


            _oDocument.SetMargins(25f, 25f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.nFontSize = (int)_nFooterFont;
            oPDFWriter.PageEvent = PageEventHandler;
            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);

            #endregion
            this.PrintBody();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Header

        private void PrintHeader(string BUName,bool IsSummary)
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 200f, 230f });
            PdfPCell oPdfPCellHearder;


            _oFontStyle = FontFactory.GetFont("Tahoma", _nBUFont, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(BUName, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);


            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", _nBUlocafont, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            string sTitle = "OT Sheet";
            if(IsSummary==true)
            {
                sTitle = "OT Sheet Summary";
            }
            _oFontStyle = FontFactory.GetFont("Tahoma", _nSalarySheetFont, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(sTitle, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", _DateFont, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body
        private void PrintBody()
        {


            _oEmployeeSalaryV2s.ForEach(x => _oTempEmployeeSalaryV2s.Add(x));
            string sPrevBUName = "";

            while (_oEmployeeSalaryV2s.Count > 0)
            {
                var oResults = new List<EmployeeSalaryV2>();
                if (_bIsSerialWise)
                {
                    _oEmployeeSalaryV2s.ForEach(x => _oSummaryEmployeeSalaryV2s.Add(x));
                    oResults = _oEmployeeSalaryV2s.Where(x => x.BUID == _oEmployeeSalaryV2s[0].BUID).ToList();
                }
                else
                {
                    oResults = _oEmployeeSalaryV2s.Where(x => x.BUID == _oEmployeeSalaryV2s[0].BUID && x.LocationID == _oEmployeeSalaryV2s[0].LocationID && x.DepartmentID == _oEmployeeSalaryV2s[0].DepartmentID).OrderBy(x => x.EmployeeCode).ToList();
                    oResults.ForEach(x => _oSummaryEmployeeSalaryV2s.Add(x));
                }

                PrintHeader(oResults[0].BUName,false);
                _oFontStyle = FontFactory.GetFont("Tahoma", _UnitDeptFont, iTextSharp.text.Font.BOLD);
                int nSpan = 0;
                if (_bIsSerialWise)
                {
                    nSpan = _nTotalColumns;
                }
                else
                {
                    nSpan = 8;

                }
                _oPdfPCell = new PdfPCell(new Phrase("Unit : " + oResults[0].BUShortName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = nSpan; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                if (!_bIsSerialWise)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Department : " + oResults.FirstOrDefault().DepartmentName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = _nTotalColumns - 8; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                }
                else
                {
                    if (_bIsSerialWise)
                    {
                        _oPdfPTable.CompleteRow();
                        sPrevBUName = oResults[0].BUShortName;
                    }
                }



                this.ColumnSetup();

                this.SalarySheet(oResults);
                if (_bIsSerialWise)
                {
                    _oEmployeeSalaryV2s.RemoveAll(x => x.BUID == oResults[0].BUID);
                }
                else
                {
                    _oEmployeeSalaryV2s.RemoveAll(x => x.BUID == oResults[0].BUID && x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                }

                if (((_oEmployeeSalaryV2s.Count <= 0) || (_bIsSerialWise == true && _oEmployeeSalaryV2s[0].BUShortName != sPrevBUName)) || _bIsSerialWise == false)
                {
                    if (nAlredayPrintGrandTotal == 0)
                    {
                        this.GrandTotal();
                        ResetGrandTotal();
                    }

                    //_nCount = 0;
                    nAlredayPrintGrandTotal = 0;
                }

            }
            this.Summary();
        }

        private void SalarySheet(List<EmployeeSalaryV2> oEmpSalarys)
        {
            if (!_bIsSerialWise && _nGroupBySerial!=3)
            {
                _nCount = 0;
            }
            int nCount = 0;

            int nTotalRowPage = 0;

            foreach (EmployeeSalaryV2 oEmpSalaryItem in oEmpSalarys)
            {
                nTotalRowPage++;
                _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
                _nCount++;
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

               
                    _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.EmployeeCode, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                

                     _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.NORMAL);
               
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.EmployeeName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                
               
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.DesignationName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                
               
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.Grade, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                


               
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.JoiningDate.ToString("dd MMM yyyy"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                

                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.EmployeeTypeName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                

               
                    _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.PaymentType, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                


                foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
                {
                    double nBasic = oEmpSalaryItem.EmployeeSalaryBasics.Where(x => x.SalaryHeadID == oEmpBasics.SalaryHeadID).Select(x => x.Amount).FirstOrDefault();
                    oEmpBasics.SubTotalAmount = oEmpBasics.SubTotalAmount + nBasic;
                    oEmpBasics.GrandTotalAmount = oEmpBasics.GrandTotalAmount + nBasic;
                    _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nBasic, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                }
                double TotalEarnings = 0;

                if(_oEmployeeSalaryBasics.Count>0)
                {
                    
                    TotalEarnings = oEmpSalaryItem.EmployeeSalaryBasics.Sum(x => x.Amount);
                    _SubTGrossEarnings += TotalEarnings;
                }

                if (TotalEarnings <= 0)
                {
                    _SubTGrossEarnings += oEmpSalaryItem.GrossAmount;
                }
                   
           

                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.GrossAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.OTRatePerHour, 2), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);




                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.ExtraOTHour, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.ExtraOTHour * oEmpSalaryItem.OTRatePerHour, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _SubTOTAmt += oEmpSalaryItem.ExtraOTHour * oEmpSalaryItem.OTRatePerHour;


                _oPdfPCell = new PdfPCell(new Phrase("--", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;//Signature
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                if ((oEmpSalarys.Count == nCount && nTotalRowPage % 15 != 0 ))
                {
                    this.SubTotal();
                    nAlredayPrintGrandTotal = 1;

                    this.GrandTotal();
                    this.ResetGrandTotal();
                    _SubTOTAmt = 0;
                    _SubTGrossEarnings = 0;
                }
                else if (oEmpSalarys.Count == nCount)
                {
                    this.SubTotal();
                    nAlredayPrintGrandTotal = 1;
                    this.GrandTotal();
                    this.ResetGrandTotal();
                    _SubTOTAmt = 0;
                    _SubTGrossEarnings = 0;
                }

                else if((nTotalRowPage % 15 == 0 ))
                {
                    this.SubTotal();
                    _SubTOTAmt = 0;
                    _SubTGrossEarnings = 0;
                   
                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    this.ResetSubTotal();
                    PrintHeader(oEmpSalaryItem.BUName,false);
                    _oFontStyle = FontFactory.GetFont("Tahoma", _UnitDeptFont, iTextSharp.text.Font.BOLD);

                    int nSpan = 0;
                    if (_bIsSerialWise)
                    {
                        nSpan = _nColumns;
                    }
                    else
                    {
                        nSpan = 8;
                    }
                    _oPdfPCell = new PdfPCell(new Phrase("Unit : " + oEmpSalaryItem.BUShortName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = nSpan; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    if (!_bIsSerialWise)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Department : " + oEmpSalaryItem.DepartmentName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = _nColumns - 8; _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    }

                    _oPdfPTable.CompleteRow();
                    this.ColumnSetup();
                }

            }


            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();

        }
        private void GrandTotal()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("GrandTotal", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.Colspan =8;//SL and Emp Basic Info
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpBasics.GrandTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTGrossEarnings, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTOTAmt, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _GrandTOTAmt = 0;

            _GrandTGrossEarnings = 0;
            _oPdfPTable.CompleteRow();
        }
        private void ResetSubTotal()
        {
            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
            {
                oEmpBasics.SubTotalAmount = 0;
            }
            foreach (EmployeeSalaryDetailV2 oEmpAllowances in _oEmployeeSalaryAllowances)
            {
                oEmpAllowances.SubTotalAmount = 0;
            }
            foreach (EmployeeSalaryDetailV2 oEmpDeductions in _oEmployeeSalaryDeductions)
            {
                oEmpDeductions.SubTotalAmount = 0;
            }
        }
        private void ResetGrandTotal()
        {
            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
            {
                oEmpBasics.GrandTotalAmount = 0;
            }
            foreach (EmployeeSalaryDetailV2 oEmpAllowances in _oEmployeeSalaryAllowances)
            {
                oEmpAllowances.GrandTotalAmount = 0;
            }
            foreach (EmployeeSalaryDetailV2 oEmpDeductions in _oEmployeeSalaryDeductions)
            {
                oEmpDeductions.GrandTotalAmount = 0;
            }
        }
        private void SubTotal()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.Colspan =8;//SL and Employee Basic Info
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            foreach (EmployeeSalaryDetailV2 oEmpBasics in _oEmployeeSalaryBasics)
            {
                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpBasics.SubTotalAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            }


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTGrossEarnings, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _GrandTGrossEarnings += _SubTGrossEarnings;



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTOTAmt, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _GrandTOTAmt += _SubTOTAmt;


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        private void ColumnSetup()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", _nFontHeader, iTextSharp.text.Font.BOLD);
            int nSpan = 2;

            _oPdfPCell = new PdfPCell(new Phrase("SL#", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

          
                _oPdfPCell = new PdfPCell(new Phrase("Employee Information", _oFontStyle)); _oPdfPCell.Colspan = 7; _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            
            _oPdfPCell = new PdfPCell(new Phrase("Total Gross", _oFontStyle)); _oPdfPCell.Colspan = _oEmployeeSalaryBasics.Count() + 4; _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyle)); _oPdfPCell.Rowspan = nSpan; _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();

          
                _oPdfPCell = new PdfPCell(new Phrase("Employee Code", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            

           
                _oPdfPCell = new PdfPCell(new Phrase("Employee Name", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
            

            
                _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            
            
                _oPdfPCell = new PdfPCell(new Phrase("Grade", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            
           
                _oPdfPCell = new PdfPCell(new Phrase("Joining Date", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            

           
                _oPdfPCell = new PdfPCell(new Phrase("Employee Type", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            

          
                _oPdfPCell = new PdfPCell(new Phrase("Payment Type", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            
            foreach (EmployeeSalaryDetailV2 oItem in _oEmployeeSalaryBasics)
            {

                _oPdfPCell = new PdfPCell(new Phrase(oItem.SalaryHeadName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            }
            _oPdfPCell = new PdfPCell(new Phrase("Gross Salary", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("OT Rate", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Extra OT Hour", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("OT Amount", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        public void Summary()
        {
            float FixedHeight = 0f;
            iTextSharp.text.Font _oFontStyle;
            if (_isDynamic)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 25, iTextSharp.text.Font.BOLD);
                FixedHeight = 30f;
            }
            else
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 10, iTextSharp.text.Font.BOLD);
            }
    
            PdfPTable oPdfPTable1 = new PdfPTable(6);
            PdfPCell oPdfPCell1;
            oPdfPTable1.SetWidths(new float[] { 60f, 60f, 80f, 60f, 60f,100f });

            double nGTotalEmpCount = 0, nGTotalOTHour = 0, nGTotalGrossAmount = 0, nGTotalTotalOTAmt = 0;
            while (_oSummaryEmployeeSalaryV2s.Count > 0)
            {

                string BUName = _oSummaryEmployeeSalaryV2s[0].BUName;
                this.PrintHeader(BUName,true);
                List<EmployeeSalaryV2> oEmployeeSalaryV2s = _oSummaryEmployeeSalaryV2s.Where(x => x.BUName == BUName).ToList();



                var oDBMonthlyAttendanceReports = oEmployeeSalaryV2s.GroupBy(x => new { x.DepartmentName }, (key, grp) => new
                {
                    DepartmentName = key.DepartmentName,
                    EmpCount = grp.ToList().Count(),
                    GrossAmount = grp.ToList().Sum(y => y.GrossAmount),
                    OTHour = grp.ToList().Sum(y => y.ExtraOTHour),
                    TotalOT = grp.ToList().Sum(y => y.ExtraOTHour * y.OTRatePerHour),
                    Results = grp.ToList(),
                    TotalGross = grp.ToList().Sum(x => x.EmployeeSalaryBasics.Sum(y => y.Amount))
                });


                oPdfPTable1 = new PdfPTable(6);
                oPdfPTable1.SetWidths(new float[] { 60f, 60f, 80f, 60f, 60f,100f });


             
                oPdfPCell1 = new PdfPCell(new Phrase("Unit : " + BUName, _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = 8;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPTable1.CompleteRow();


                oPdfPCell1 = new PdfPCell(new Phrase("Department", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("ManPower", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Extra OT Hour", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Total OT", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase("Gross", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 5; oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


                oPdfPTable1.CompleteRow();

                _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                double nTotalEmpCount = 0, nTotalOTHour = 0, nTotalGrossAmount = 0, nTotalTotalOTAmt = 0;
                foreach (var oBUEmployeeSalary in oDBMonthlyAttendanceReports)
                {
                    oPdfPTable1 = new PdfPTable(6);
                    oPdfPTable1.SetWidths(new float[] { 60f, 60f, 80f, 60f, 60f,100f });

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.DepartmentName, _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_LEFT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.EmpCount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalEmpCount += oBUEmployeeSalary.EmpCount;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.OTHour.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalOTHour += oBUEmployeeSalary.OTHour;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.TotalOT.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalTotalOTAmt += oBUEmployeeSalary.TotalOT;

                    oPdfPCell1 = new PdfPCell(new Phrase(oBUEmployeeSalary.GrossAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                    nTotalGrossAmount += oBUEmployeeSalary.GrossAmount;


                    oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 5; oPdfPCell1.FixedHeight = FixedHeight;
                    oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                    oPdfPTable1.CompleteRow();

                    _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                }

                #region SubTotal
                oPdfPTable1 = new PdfPTable(6);
                oPdfPTable1.SetWidths(new float[] { 60f, 60f, 80f, 60f, 60f, 100f });

                oPdfPCell1 = new PdfPCell(new Phrase("SubTotal:", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalEmpCount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalEmpCount += nTotalEmpCount;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalOTHour.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalOTHour += nTotalOTHour;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalTotalOTAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalTotalOTAmt += nTotalTotalOTAmt;

                oPdfPCell1 = new PdfPCell(new Phrase(nTotalGrossAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);
                nGTotalGrossAmount += nTotalGrossAmount;

               
                oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 5; oPdfPCell1.FixedHeight = FixedHeight;
                oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

                oPdfPTable1.CompleteRow();
                _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                #endregion
                _oSummaryEmployeeSalaryV2s.RemoveAll(x => x.BUName == BUName);


            }
            #region GrandTotal

            oPdfPTable1 = new PdfPTable(6);
            oPdfPTable1.SetWidths(new float[] { 60f, 60f, 80f, 60f, 60f, 100f });


            oPdfPCell1 = new PdfPCell(new Phrase("GrandTotal:", _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalEmpCount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalOTHour.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);


            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalTotalOTAmt.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase(nGTotalGrossAmount.ToString("#,##0"), _oFontStyle)); oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_RIGHT; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPCell1 = new PdfPCell(new Phrase("", _oFontStyle)); oPdfPCell1.Border = 0; oPdfPCell1.Colspan = _nTotalColumns - 5; oPdfPCell1.FixedHeight = FixedHeight;
            oPdfPCell1.HorizontalAlignment = Element.ALIGN_CENTER; oPdfPCell1.BackgroundColor = BaseColor.WHITE; oPdfPTable1.AddCell(oPdfPCell1);

            oPdfPTable1.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTable1); _oPdfPCell.Border = 0; _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            #endregion

        }


        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount) : amount;
            return (bWithPrecision) ? Global.MillionFormat(amount) : Global.MillionFormat(amount).Split('.')[0];
        }


        #endregion

        public byte[] PrepareReportPortrait(int nGroupBySerial, List<EmployeeSalaryV2> oEmployeeSalaryV2s, List<LeaveHead> oLeaveHeads, List<EmployeeSalaryDetailV2> oEmployeeSalaryBasics, List<EmployeeSalaryDetailV2> oEmployeeSalaryAllowances, List<EmployeeSalaryDetailV2> oEmployeeSalaryDeductions, Company oCompany, SalaryField oSalaryField, EnumPageOrientation ePageOrientation, List<SalarySheetSignature> oSalarySheetSignatures, string StartDate, string EndDate, bool IsOT)
        {
            _oCompany = oCompany;
            _oEmployeeSalaryV2s = oEmployeeSalaryV2s;
            _oLeaveHeads = oLeaveHeads;
            _oEmployeeSalaryBasics = oEmployeeSalaryBasics;
            _oEmployeeSalaryAllowances = oEmployeeSalaryAllowances;
            _oEmployeeSalaryDeductions = oEmployeeSalaryDeductions;
            _oSalaryField = oSalaryField;
            _ePageOrientation = ePageOrientation;
            _StartDate = Convert.ToDateTime(StartDate);
            DateTime sEndDate = Convert.ToDateTime(EndDate);
            _bIsSerialWise = nGroupBySerial == 2? true : false;
            _IsOT = IsOT;
            _sStartDate = _StartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");
            int nColumn = 0;
            _nGroupBySerial = nGroupBySerial;



            _nTotalColumns = 11;
            _nColumns = _nTotalColumns;
            float[] tablecolumns = new float[_nTotalColumns];
            tablecolumns[nColumn++] = 40f;//SL


            for (var i = 0; i < 4; i++)//Code,name,Designation,Joining
            {
                tablecolumns[nColumn++] = 100f;
            }


            for (var i = 0; i < 5; i++)//Gross,Basic,OtRate,OTHour,OT
            {
                tablecolumns[nColumn++] = 80f;
            }

            tablecolumns[nColumn++] = 110f;//Signature
            #region Page Setup
            _oPdfPTable = new PdfPTable(_nTotalColumns);
            if (_ePageOrientation == EnumPageOrientation.A4_Portrait)
            {
                _oDocument = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
                _oDocument.SetPageSize(iTextSharp.text.PageSize.A4);
                _nHeight = 30f;
                _nFontSize = 6f;
                _nFontHeader = 8f;
            }

            _oDocument.SetMargins(25f, 25f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 7f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            ESimSolFooter PageEventHandler = new ESimSolFooter();
            PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
            PageEventHandler.PrintPrintingDateTime = false;
            PageEventHandler.nFontSize = (int)_nFooterFont;
            oPDFWriter.PageEvent = PageEventHandler;
            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);

            #endregion
            this.PrintBodyPortrait();
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }
        #region Report Header Portrait

        private void PrintHeaderPortrait(string BUName)
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 200f, 230f });
            PdfPCell oPdfPCellHearder;


            _oFontStyle = FontFactory.GetFont("Tahoma", _nBUFont, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(BUName, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);


            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", _nBUlocafont, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", _nSalarySheetFont, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("OT Sheet", _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", _DateFont, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = _nTotalColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body Portrait
        int nAlredayPrintGrandTotal = 0;
        private void PrintBodyPortrait()
        {
           

            _oEmployeeSalaryV2s.ForEach(x => _oTempEmployeeSalaryV2s.Add(x));
            string sPrevBUName = "";

            while (_oEmployeeSalaryV2s.Count > 0)
            {
                var oResults = new List<EmployeeSalaryV2>();
                if (_bIsSerialWise)
                {
                    _oEmployeeSalaryV2s.ForEach(x => _oSummaryEmployeeSalaryV2s.Add(x));
                    oResults = _oEmployeeSalaryV2s.Where(x => x.BUID == _oEmployeeSalaryV2s[0].BUID).ToList();
                }
                else
                {
                    oResults = _oEmployeeSalaryV2s.Where(x => x.BUID == _oEmployeeSalaryV2s[0].BUID && x.LocationID == _oEmployeeSalaryV2s[0].LocationID && x.DepartmentID == _oEmployeeSalaryV2s[0].DepartmentID).OrderBy(x => x.EmployeeCode).ToList();
                    oResults.ForEach(x => _oSummaryEmployeeSalaryV2s.Add(x));
                }

                PrintHeaderPortrait(oResults[0].BUName);
                _oFontStyle = FontFactory.GetFont("Tahoma", _UnitDeptFont, iTextSharp.text.Font.BOLD);
                int nSpan = 0;
                if (_bIsSerialWise)
                {
                    nSpan = _nTotalColumns;
                }
                else
                {
                    nSpan = 7;

                }
                _oPdfPCell = new PdfPCell(new Phrase("Unit : " + oResults[0].BUShortName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = nSpan; _oPdfPCell.Border = 0;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                if (!_bIsSerialWise)
                {
                    _oPdfPCell = new PdfPCell(new Phrase("Department : " + oResults.FirstOrDefault().DepartmentName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = _nTotalColumns - 7; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                    _oPdfPTable.CompleteRow();

                }
                else
                {
                    if (_bIsSerialWise)
                    {
                        _oPdfPTable.CompleteRow();
                        sPrevBUName = oResults[0].BUShortName;
                    }
                }



                this.ColumnSetupPortrait();

                this.SalarySheetPortrait(oResults);
                if (_bIsSerialWise)
                {
                    _oEmployeeSalaryV2s.RemoveAll(x => x.BUID == oResults[0].BUID);
                }
                else
                {
                    _oEmployeeSalaryV2s.RemoveAll(x => x.BUID == oResults[0].BUID && x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                }

                if (((_oEmployeeSalaryV2s.Count <= 0) || (_bIsSerialWise == true && _oEmployeeSalaryV2s[0].BUShortName != sPrevBUName)) || _bIsSerialWise == false)
                {
                    if (nAlredayPrintGrandTotal == 0)
                    {
                        this.GrandTotalPortrait();

                    }

                    //_nCount = 0;
                    nAlredayPrintGrandTotal = 0;
                }

            }
            this.Summary();
        }

        private void SalarySheetPortrait(List<EmployeeSalaryV2> oEmpSalarys)
        {
            if (!_bIsSerialWise && _nGroupBySerial!=3)
            {
                _nCount = 0;
            }
            int nCount = 0;

            int nTotalRowPage = 0;

            foreach (EmployeeSalaryV2 oEmpSalaryItem in oEmpSalarys)
            {
                nTotalRowPage++;
                _oFontStyle = FontFactory.GetFont("Tahoma", 8, iTextSharp.text.Font.BOLD);
                _nCount++;
                nCount++;
                _oPdfPCell = new PdfPCell(new Phrase(_nCount.ToString(), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oFontStyle = FontFactory.GetFont("Tahoma", 8, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.EmployeeCode, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



                _oFontStyle = FontFactory.GetFont("Tahoma", 8, iTextSharp.text.Font.NORMAL);

                _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.EmployeeName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



                _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.DesignationName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(oEmpSalaryItem.JoiningDate.ToString("dd MMM yyyy"), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



                double nBasic = oEmpSalaryItem.EmployeeSalaryBasics.Where(x => x.SalaryHeadName == "Basic").Select(x => x.Amount).FirstOrDefault();
                _subTBasic += nBasic;
                _GrandTBasic += nBasic;

                double TotalEarnings = 0;
             if(oEmpSalaryItem.EmployeeSalaryBasics.Count>0)
             {
                 TotalEarnings = oEmpSalaryItem.EmployeeSalaryBasics.Sum(x => x.Amount);
              
                 _SubTGrossEarnings += TotalEarnings;
             }



             if (TotalEarnings <= 0)
               {
                   _SubTGrossEarnings += oEmpSalaryItem.GrossAmount;
               }
                    

                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.GrossAmount, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(nBasic, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);






                _oPdfPCell = new PdfPCell(new Phrase(Global.MillionFormat(oEmpSalaryItem.OTRatePerHour, 2), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);




                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.ExtraOTHour, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _SubTOTHour += oEmpSalaryItem.ExtraOTHour;


                _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(oEmpSalaryItem.ExtraOTHour * oEmpSalaryItem.OTRatePerHour, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _SubTOTAmt += oEmpSalaryItem.ExtraOTHour * oEmpSalaryItem.OTRatePerHour;


                _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = _nHeight;//Signature
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();


                if ((oEmpSalarys.Count == nCount && nTotalRowPage % 22 != 0))
                {
                    this.SubTotalPortrait();
                    nAlredayPrintGrandTotal = 1;
                    this.GrandTotalPortrait();

                    _SubTOTAmt = 0;
                    _SubTGrossEarnings = 0;
                    _SubTOTHour = 0;
                    _subTBasic = 0;

                }
                else if (oEmpSalarys.Count == nCount)
                {
                    this.SubTotalPortrait();
                    nAlredayPrintGrandTotal = 1;
                    this.GrandTotalPortrait();

                    _SubTOTAmt = 0;
                    _SubTGrossEarnings = 0;
                    _SubTOTHour = 0;
                    _subTBasic = 0;
                }

                else if ((nTotalRowPage % 22 == 0))
                {
                    this.SubTotalPortrait();
                    _SubTOTAmt = 0;
                    _SubTGrossEarnings = 0;
                    _SubTOTHour = 0;
                    _subTBasic = 0;

                    _oDocument.Add(_oPdfPTable);
                    _oDocument.NewPage();
                    _oPdfPTable.DeleteBodyRows();
                    PrintHeaderPortrait(oEmpSalaryItem.BUName);
                    _oFontStyle = FontFactory.GetFont("Tahoma", _UnitDeptFont, iTextSharp.text.Font.BOLD);

                    int nSpan = 0;
                    if (_bIsSerialWise)
                    {
                        nSpan = _nColumns;
                    }
                    else
                    {
                        nSpan = 8;
                    }
                    _oPdfPCell = new PdfPCell(new Phrase("Unit : " + oEmpSalaryItem.BUShortName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = nSpan; _oPdfPCell.Border = 0;
                    _oPdfPCell.HorizontalAlignment = Element.ALIGN_LEFT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    if (!_bIsSerialWise)
                    {
                        _oPdfPCell = new PdfPCell(new Phrase("Department : " + oEmpSalaryItem.DepartmentName, _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.FixedHeight = 30f; _oPdfPCell.Colspan = _nColumns - 8; _oPdfPCell.Border = 0;
                        _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

                    }

                    _oPdfPTable.CompleteRow();
                    this.ColumnSetupPortrait();
                }

            }


            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();

        }
        private void GrandTotalPortrait()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("GrandTotal", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.Colspan = 5;//SL and Emp Basic Info
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTGrossEarnings, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTBasic, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);





            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTOTHour, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_GrandTOTAmt, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _GrandTOTAmt = 0;
            _GrandTOTHour = 0;
            _GrandTBasic = 0;

            _GrandTGrossEarnings = 0;
            _oPdfPTable.CompleteRow();
        }

        private void SubTotalPortrait()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", _nFontSize, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Sub Total", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY; _oPdfPCell.Colspan = 5;//SL and Employee Basic Info
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTGrossEarnings, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _GrandTGrossEarnings += _SubTGrossEarnings;



            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_subTBasic, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);





            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTOTHour, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _GrandTOTHour += _SubTOTHour;




            _oPdfPCell = new PdfPCell(new Phrase(this.GetAmountInStr(_SubTOTAmt, true, false), _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
            _GrandTOTAmt += _SubTOTAmt;


            _oPdfPCell = new PdfPCell(new Phrase("", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_RIGHT; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }
        private void ColumnSetupPortrait()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", _nFontHeader, iTextSharp.text.Font.BOLD);

            _oPdfPCell = new PdfPCell(new Phrase("#SL", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);




            _oPdfPCell = new PdfPCell(new Phrase("Code", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("Name", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("Designation", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Joining", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("Gross", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPCell = new PdfPCell(new Phrase("Basic", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("OT Rate", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("OT Hour", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);


            _oPdfPCell = new PdfPCell(new Phrase("OT", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);



            _oPdfPCell = new PdfPCell(new Phrase("Signature", _oFontStyle)); _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);

            _oPdfPTable.CompleteRow();
        }


        #endregion
    }
}
