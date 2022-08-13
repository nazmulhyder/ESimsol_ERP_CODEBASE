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
    public class rptSalarySheet_DetailFormat_F5
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
        //int _nBorderWidth = 2;
        bool IsCompliance;

        Company _oCompany = new Company();
        List<RPTSalarySheet> _oEmployeeSalarys = new List<RPTSalarySheet>();
        List<RPTSalarySheet> _oTempEmployeeSalarys = new List<RPTSalarySheet>();
        List<RPTSalarySheetDetail> _oEmployeeSalaryDetails = new List<RPTSalarySheetDetail>();
        List<EmployeeSalaryDetailDisciplinaryAction> _oEmployeeSalaryDetailDisciplinaryActions = new List<EmployeeSalaryDetailDisciplinaryAction>();
        List<SalaryHead> _oSalaryHeads = new List<SalaryHead>();
        List<SalaryHead> _oTempSalaryHeads = new List<SalaryHead>();
        List<SalarySheetProperty> _oSalarySheetPropertys = new List<SalarySheetProperty>();
        List<LeaveHead> _oLeaveHeads = new List<LeaveHead>();
        List<EmployeeLeaveOnAttendance> _oELOnAttendances = new List<EmployeeLeaveOnAttendance>();
        List<EmployeeBankAccount> _oEmployeeBankAccounts = new List<EmployeeBankAccount>();
        List<TransferPromotionIncrement> _oTransferPromotionIncrements = new List<TransferPromotionIncrement>();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();

        bool _bWithLeaveHeads = false;
        List<string> ColumnHeader = new List<string>();
        List<string> ColEmpInfo = new List<string>();
        List<string> ColAttDetail = new List<string>();
        List<string> ColIncrementDetail = new List<string>();
        List<string> ColEarnings = new List<string>();
        List<string> ColBasics = new List<string>();
        List<string> ColDeductions = new List<string>();
        List<string> ColBankDetail = new List<string>();

        string[] ColGross = new string[] { };
        Dictionary<string, object> _gross = new Dictionary<string, object>();
        Dictionary<string, object> _increment = new Dictionary<string, object>();
        Dictionary<string, object> _earnings = new Dictionary<string, object>();
        Dictionary<string, object> _basics = new Dictionary<string, object>();
        Dictionary<string, object> _deductions = new Dictionary<string, object>();
        Dictionary<string, object> _banks = new Dictionary<string, object>();

        Dictionary<string, object> gross = new Dictionary<string, object>();
        Dictionary<string, object> increment = new Dictionary<string, object>();
        Dictionary<string, object> earnings = new Dictionary<string, object>();
        Dictionary<string, object> basics = new Dictionary<string, object>();
        Dictionary<string, object> deductions = new Dictionary<string, object>();
        Dictionary<string, object> banks = new Dictionary<string, object>();

        Dictionary<string, object> _Prevgross = new Dictionary<string, object>();
        Dictionary<string, object> _Previncrement = new Dictionary<string, object>();
        Dictionary<string, object> _Prevearnings = new Dictionary<string, object>();
        Dictionary<string, object> _Prevbasics = new Dictionary<string, object>();
        Dictionary<string, object> _Prevdeductions = new Dictionary<string, object>();
        Dictionary<string, object> _Prevbanks = new Dictionary<string, object>();

        bool _bWithPrecision = true;
        bool _bHasOTAllowance = false;
        bool _bHasParentDept = false;
        string _sStartDate = "";
        string _sEndDate = "";
        #endregion

        public byte[] PrepareReport(bool IsComp, EmployeeSalary oEmployeeSalary, List<SalarySheetProperty> oSalarySheetPropertys, List<LeaveHead> oLeaveHeads, bool bWithLeaveHeads, List<EmployeeLeaveOnAttendance> oELOnAttendances, bool bWithPrecision, List<SalarySheetSignature> oSalarySheetSignatures)
        {

            IsCompliance = IsComp;
            _oEmployeeSalarys = oEmployeeSalary.EmployeeSalarySheets.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeSalaryID).ToList();



            _oEmployeeSalaryDetails = oEmployeeSalary.EmployeeSalarySheetDetails;
            _oEmployeeSalaryDetailDisciplinaryActions = oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions;
            //_oSalaryHeads = oEmployeeSalary.SalaryHeads.OrderBy(x => (int)x.SalaryHeadType & x.Sequence).ToList();
            _oSalaryHeads = oEmployeeSalary.SalaryHeads.OrderBy(x => x.Sequence).ToList();
            _oEmployeeBankAccounts = oEmployeeSalary.EmployeeBankAccounts;
            _oTransferPromotionIncrements = oEmployeeSalary.TransferPromotionIncrements;
            _oBusinessUnits = oEmployeeSalary.BusinessUnits;
            _oCompany = oEmployeeSalary.Company;
            oSalarySheetPropertys = oSalarySheetPropertys.OrderBy(x => x.SalarySheetFormatPropertyInt).ToList();
            _oSalarySheetPropertys = oSalarySheetPropertys;
            _oLeaveHeads = oLeaveHeads;
            _bWithLeaveHeads = bWithLeaveHeads;
            _oELOnAttendances = oELOnAttendances;
            _bWithPrecision = bWithPrecision;


            ColEmpInfo = oSalarySheetPropertys.Where(x => x.PropertyFor == 1).Select(p => p.SalarySheetFormatPropertyStr).ToList();
            ColAttDetail = oSalarySheetPropertys.Where(x => x.PropertyFor == 2 && x.SalarySheetFormatProperty != EnumSalarySheetFormatProperty.OTAllowance).Select(p => p.SalarySheetFormatPropertyStr).ToList();
            if (IsCompliance == false)
            {
                ColIncrementDetail = oSalarySheetPropertys.Where(x => x.PropertyFor == 3).Select(p => p.SalarySheetFormatPropertyStr).ToList();
            }
            //if (IsCompliance)
            //{
            //    ColIncrementDetail = oSalarySheetPropertys.Where(x => (x.PropertyFor == 3 && x.SalarySheetFormatPropertyInt != 610)).Select(p => p.SalarySheetFormatPropertyStr).ToList();
            //}
            ColBankDetail = oSalarySheetPropertys.Where(x => x.PropertyFor == 4).Select(p => p.SalarySheetFormatPropertyStr).ToList();

            ColEarnings.AddRange(_oSalaryHeads.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Addition).OrderBy(x=>x.Sequence).Select(x => x.Name).ToList());

            int basicCol = 0;
            if (IsCompliance)
            {
                ColBasics.AddRange(_oSalaryHeads.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic).OrderBy(x=>x.Sequence).Select(x => x.Name).ToList());
                if (ColBasics.Count() > 0) basicCol = ColBasics.Count();
                else basicCol = 0;
            }


            if (oSalarySheetPropertys.Any(x => x.PropertyFor == 2 && x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.OTAllowance))
            {
                ColEarnings.AddRange(oSalarySheetPropertys.Where(x => x.PropertyFor == 2 && x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.OTAllowance).Select(x => x.SalarySheetFormatPropertyStr));
                _bHasOTAllowance = true;
            }

            //List<string> ColDeductions = _oSalaryHeads.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction).Select(x => x.Name).ToList();
            ColDeductions = _oSalaryHeads.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Deduction && (x.SalaryHeadID == 8 || x.SalaryHeadID == 20 || x.SalaryHeadID == 25 || x.SalaryHeadID == 26))).Select(x => x.Name).ToList();
            List<string> allCols = _oSalaryHeads.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Deduction)).Select(x => x.Name).ToList();

            ColDeductions = allCols.Except(_oSalaryHeads.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Deduction && (x.SalaryHeadID == 8 || x.SalaryHeadID == 20 || x.SalaryHeadID == 25 || x.SalaryHeadID == 26))).Select(x => x.Name).ToList()).ToList();
            

            ColumnHeader = new List<string>() { "SL#",
            (ColEmpInfo.Count() > 0 ? "Employee Information" : ""), 
            (ColAttDetail.Count() > 0 ? "Att. Detail" : ""),
            (( IsCompliance == false && ColIncrementDetail.Count() > 0) ? "Increment Detail" : ""), 
            "Gross Salary", ((ColEarnings.Count() > 0) ? "Earnings" : ""), 
            //((ColEarnings.Count() > 0) ? "Gross Earnings" : ""), 


            ((ColEarnings.Count() > 0) ? "Gross Earnings On Att." : ""), 


            (( IsCompliance == true && ColBasics.Count() > 0) ? "Basic Breakdown" : ""), 


            "Deduction", ((ColDeductions.Count() > 0) ? "Gross Deductions" : ""),
            "Net Amount",
            (ColBankDetail.Count>0?"Bank":""),
            (ColBankDetail.Count>0?"Cash":""),
            (ColBankDetail.Count>0?"Account No":""),
            (ColBankDetail.Count>0?"Bank Name":""), 
            "Signature" };

            ColumnHeader.RemoveAll(x => x == "");
            //, ((ColEarnings.Count() > 0 || _bHasOTAllowance) ? "Gross Earnings" : "")
            ColGross = new string[] { "Gross Salary", ((ColEarnings.Count() > 0 || _bHasOTAllowance) ? "Gross Earnings On Att." : ""), ((ColDeductions.Count() > 0) ? "Gross Deductions" : ""), "Net Amount" };
            ColGross = ColGross.Where(x => x != "").ToArray();

            foreach (string property in ColGross)
            {
                _gross.Add(property, 0);
                _Prevgross.Add(property, 0);
            }
            foreach (string property in ColIncrementDetail)
            {
                if (property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LastGross.ToString()) || property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LastIncrement.ToString())) { _increment.Add(property, 0); _Previncrement.Add(property, 0); }
            }
            foreach (string property in ColEarnings)
            {
                _earnings.Add(property, 0);
                _Prevearnings.Add(property, 0);
            }

            if (IsCompliance == true)
            {
                foreach (string property in ColBasics)
                {
                    _basics.Add(property, 0);
                    _Prevbasics.Add(property, 0);
                }
            }

            foreach (string property in ColDeductions)
            {
                _deductions.Add(property, 0);
                _Prevdeductions.Add(property, 0);
            }
            foreach (string property in ColBankDetail)
            {
                if (property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.BankAmount.ToString()) || property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.CashAmount.ToString())) { _banks.Add(property, 0); _Prevbanks.Add(property, 0); }
            }

            DateTime sStartDate = Convert.ToDateTime(oEmployeeSalary.ErrorMessage.Split(',')[0]);
            DateTime sEndDate = Convert.ToDateTime(oEmployeeSalary.ErrorMessage.Split(',')[1]);
            _sStartDate = sStartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");

            #region Page Setup

            _nColumns = 1 + ColEmpInfo.Count() + ColAttDetail.Count() + ((bWithLeaveHeads) ? oLeaveHeads.Count() - 1 : 0) + 1 + ColEarnings.Count() + ((ColEarnings.Count() > 0) ? 1 : 0) + basicCol + ColDeductions.Count() + ((ColDeductions.Count() > 0) ? 1 : 0) + ColIncrementDetail.Count() + ColBankDetail.Count() + 2;

            int nColumn = 0;
            float[] tablecolumns = new float[_nColumns];

            //_nPageWidth = 25;
            tablecolumns[nColumn++] = 25f;
            foreach (string sColumn in ColEmpInfo)
            {
                if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeCode.ToString()))
                {
                    //_nPageWidth += 60;
                    tablecolumns[nColumn++] = 90f;
                }

                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeName.ToString()))
                {
                    //_nPageWidth += 90;
                    tablecolumns[nColumn++] = 110f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.ParentDepartment.ToString()))
                {
                    //_nPageWidth += 90;
                    tablecolumns[nColumn++] = 90f;
                    _bHasParentDept = true;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.Department.ToString()))
                {
                    //_nPageWidth += 90;
                    tablecolumns[nColumn++] = 90f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.Designation.ToString()))
                {
                    //_nPageWidth += 90;
                    tablecolumns[nColumn++] = 90f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.JoiningDate.ToString()))
                {
                    //_nPageWidth += 70;
                    tablecolumns[nColumn++] = 70f;
                }

                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.ConfirmationDate.ToString()))
                {
                    //_nPageWidth += 70;
                    tablecolumns[nColumn++] = 70f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeType.ToString()))
                {
                    //_nPageWidth += 70;
                    tablecolumns[nColumn++] = 70f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.Gender.ToString()))
                {
                    //_nPageWidth += 50;
                    tablecolumns[nColumn++] = 50f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeContactNo.ToString()))
                {
                    //_nPageWidth += 50;
                    tablecolumns[nColumn++] = 70f;
                }
            }

            foreach (string sColumn in ColAttDetail)
            {
                if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))
                {
                    foreach (LeaveHead oItem in oLeaveHeads)
                    {
                        //_nPageWidth += 22;
                        tablecolumns[nColumn++] = 25f;
                    }
                }
                else
                {
                    //_nPageWidth += 40;
                    tablecolumns[nColumn++] = 50f;
                }
            }

            foreach (string sColumn in ColIncrementDetail)
            {
                //_nPageWidth += 60;
                tablecolumns[nColumn++] = 60f;
            }

            //_nPageWidth += 60;
            tablecolumns[nColumn++] = 70f;

            foreach (string sColumn in ColEarnings)
            {
                //_nPageWidth += 60;
                tablecolumns[nColumn++] = 60f;
            }
            if (ColEarnings.Count() > 0)
            {
                //_nPageWidth += 63;
                //tablecolumns[nColumn++] = 70f;// Gross Earnings
                tablecolumns[nColumn++] = 70f;// Gross Earnings on At.
            }

            //for basic types 
            if (IsCompliance == true)
            {
                foreach (string sColumn in ColBasics)
                {
                    //_nPageWidth += 60;
                    tablecolumns[nColumn++] = 70f;
                }
            }
            //if (_bHasOTAllowance)
            //{
            //    _nPageWidth += 63;
            //    tablecolumns[nColumn++] = 63f;
            //}
            foreach (string sColumn in ColDeductions)
            {
                //_nPageWidth += 50;
                tablecolumns[nColumn++] = 70f;
            }
            if (ColDeductions.Count() > 0)
            {
                //_nPageWidth += 60;
                tablecolumns[nColumn++] = 80f;
            }
            //_nPageWidth += 65;
            tablecolumns[nColumn++] = 85f;

            foreach (string sColumn in ColBankDetail)
            {
                if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.AccountNo.ToString()) || sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.BankName.ToString()))
                {
                    //_nPageWidth += 90;
                    tablecolumns[nColumn++] = 110f;
                }
                else
                {
                    //_nPageWidth += 60;
                    tablecolumns[nColumn++] = 60f;
                }
            }

            //_nPageWidth += 85;
            tablecolumns[nColumn++] = 90f;

            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(new iTextSharp.text.Rectangle(_nPageWidth, _npageHeight), 0f, 0f, 0f, 0f);

            _oDocument.SetMargins(25f, 25f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            //ESimSolFooter_Signature PageEventHandler = new ESimSolFooter_Signature();
            //oPDFWriter.PageEvent = PageEventHandler;

            if (IsCompliance)
            {
                ESimSolFooter PageEventHandler = new ESimSolFooter();
                PageEventHandler.signatures = new List<string> { "Prepared By","Checked By","Executive/Managing Director"};
                PageEventHandler.PrintPrintingDateTime = false;
                PageEventHandler.nFontSize = 30;
                oPDFWriter.PageEvent = PageEventHandler;
            }
            else
            {
                ESimSolFooter PageEventHandler = new ESimSolFooter();
                PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
                PageEventHandler.PrintPrintingDateTime = false;
                PageEventHandler.nFontSize = 30;
                oPDFWriter.PageEvent = PageEventHandler;
            }

            _oDocument.Open();
            _oPdfPTable.SetWidths(tablecolumns);

            #endregion

            //this.PrintHeader();
            this.PrintBody();
            //_oPdfPTable.HeaderRows = 7 + ((_oLeaveHeads.Count() > 0) ? 1 : 0);
            //_oPdfPTable.HeaderRows = 4;
            _oDocument.Add(_oPdfPTable);
            _oDocument.Close();
            return _oMemoryStream.ToArray();
        }

        #region Report Header

        private void PrintHeader(int nBusinessUnitID)
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(2);
            oPdfPTableHeader.SetWidths(new float[] { 200f, 230f });
            PdfPCell oPdfPCellHearder;

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = _oBusinessUnits.Where(x => x.BusinessUnitID == nBusinessUnitID).ToList();

            _oFontStyle = FontFactory.GetFont("Tahoma", 40f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oBusinessUnits.Count > 0 ? oBusinessUnits[0].Name : "", _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);


            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oBusinessUnits.Count > 0 ? oBusinessUnits[0].Address : "", _oFontStyle));
            oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);
            oPdfPTableHeader.CompleteRow();

            _oPdfPCell = new PdfPCell(oPdfPTableHeader);
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.FixedHeight = 40;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();


            _oFontStyle = FontFactory.GetFont("Tahoma", 35f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("Salary Sheet", _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 25f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPCell.ExtraParagraphSpace = 0;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            _oPdfPCell = new PdfPCell(new Phrase(""));
            _oPdfPCell.Colspan = _nColumns;
            _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _oPdfPCell.Border = 0;
            _oPdfPCell.BackgroundColor = BaseColor.WHITE;
            _oPdfPTable.AddCell(_oPdfPCell);
            _oPdfPTable.CompleteRow();

            #endregion

        }
        #endregion

        #region Report Body
        int nRow = 0;
        int nTotalRowCount = 0;
        //int nFirstDepartment = 0;
        private void PrintBody()
        {
            if (_oEmployeeSalarys.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase(""));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();

                _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
                _oPdfPCell = new PdfPCell(new Phrase("Nothing to Print!"));
                _oPdfPCell.Colspan = _nColumns;
                _oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _oPdfPCell.Border = 0;
                _oPdfPCell.BackgroundColor = BaseColor.WHITE;
                _oPdfPTable.AddCell(_oPdfPCell);
                _oPdfPTable.CompleteRow();
            }
            else
            {
                //nFirstDepartment = _oEmployeeSalarys.FirstOrDefault().DepartmentID;
                //this.PrintHeader(_oEmployeeSalarys.FirstOrDefault().BusinessUnitID);
                Int32[] locations = _oEmployeeSalarys.Select(x => x.LocationID).Distinct().ToArray();
                var EmpInfoPropertys = _oSalarySheetPropertys.Where(x => x.PropertyFor == 1).ToList();
                var AttDetailPropertys = _oSalarySheetPropertys.Where(x => x.PropertyFor == 2).ToList();
                var IncrementPropertys = _oSalarySheetPropertys.Where(x => x.PropertyFor == 3).ToList();
                var BankPropertys = _oSalarySheetPropertys.Where(x => x.PropertyFor == 4).ToList();


                foreach (string property in _gross.Select(x => x.Key).ToArray())
                {
                    gross.Add(property, 0);
                }
                foreach (string property in _increment.Select(x => x.Key).ToArray())
                {
                    increment.Add(property, 0);
                }
                foreach (string property in _earnings.Select(x => x.Key).ToArray())
                {
                    earnings.Add(property, 0);
                }

                if (IsCompliance == true)
                {
                    foreach (string property in _basics.Select(x => x.Key).ToArray())
                    {
                        basics.Add(property, 0);
                    }
                }

                foreach (string property in _deductions.Select(x => x.Key).ToArray())
                {
                    deductions.Add(property, 0);
                }
                foreach (string property in _banks.Select(x => x.Key).ToArray())
                {
                    banks.Add(property, 0);
                }

                _oEmployeeSalarys.ForEach(x => _oTempEmployeeSalarys.Add(x));
                while (_oEmployeeSalarys.Count > 0)
                {
                    var oResults = _oEmployeeSalarys.Where(x => x.BusinessUnitID == _oEmployeeSalarys[0].BusinessUnitID && x.LocationID == _oEmployeeSalarys[0].LocationID && x.DepartmentID == _oEmployeeSalarys[0].DepartmentID).OrderBy(x => x.EmployeeCode).ToList();
                    PrintHeader(oResults[0].BusinessUnitID);
                    _oFontStyle = FontFactory.GetFont("Tahoma", 25f, iTextSharp.text.Font.BOLD);
                    this.SetCellValue("Unit : " + oResults.FirstOrDefault().LocationName, 0, 8, Element.ALIGN_LEFT, 0, 30f);
                    this.SetCellValue("Department : " + oResults.FirstOrDefault().DepartmentName, 0, _nColumns - 8, Element.ALIGN_RIGHT, 0, 30f);
                    _oPdfPTable.CompleteRow();

                    this.ColumnSetup();

                    this.SalarySheet(oResults, EmpInfoPropertys, AttDetailPropertys, IncrementPropertys, BankPropertys);

                    _oEmployeeSalarys.RemoveAll(x => x.BusinessUnitID == oResults[0].BusinessUnitID && x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                }
            }
        }

        private void SalarySheet(List<RPTSalarySheet> oEmpSalarys, List<SalarySheetProperty> EmpInfoPropertys, List<SalarySheetProperty> AttDetailPropertys, List<SalarySheetProperty> IncrementPropertys, List<SalarySheetProperty> BankPropertys)
        {
            int UnitWiseRowCount = 0;
            double nAddAmount = 0, nDeductionAmount = 0, nBasics = 0; ;
            int nCount = 0;
            var oELOnAtts = new List<EmployeeLeaveOnAttendance>();
            //var oEarnings = _oSalaryHeads.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Basic) || (x.SalaryHeadType == EnumSalaryHeadType.Addition) || (x.SalaryHeadType == EnumSalaryHeadType.Reimbursement)).ToList();
            var oEarnings = _oSalaryHeads.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Addition)).ToList();
            var oBasics = _oSalaryHeads.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Basic)).ToList();
            //var oDeductions = _oSalaryHeads.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction).ToList();

            var oDeductions = _oSalaryHeads.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Deduction && (x.SalaryHeadID != 8 && x.SalaryHeadID != 20 && x.SalaryHeadID != 25 && x.SalaryHeadID != 26))).ToList();
            
            float nHeight = 130f;
            if (IsCompliance)
            {
                nHeight = 180f;
            }

            foreach (RPTSalarySheet oEmpSalaryItem in oEmpSalarys)
            {

                _oFontStyle = FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.NORMAL);
                nCount++; nAddAmount = 0; nDeductionAmount = 0; nBasics = 0; ++nRow; UnitWiseRowCount++; nTotalRowCount++;
                oELOnAtts = _oELOnAttendances.Where(x => x.EmployeeID == oEmpSalaryItem.EmployeeID).ToList();
                this.SetCellValue(nCount.ToString(), 0, 0, Element.ALIGN_CENTER, 1, nHeight);

                if (ColEmpInfo.Count() > 0)
                {
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.EmployeeCode).Any())
                    {
                        _oFontStyle = FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.BOLD);
                        this.SetCellValue(oEmpSalaryItem.EmployeeCode, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                        _oFontStyle = FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.NORMAL);
                    }
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.EmployeeName).Any())
                    {
                        this.SetCellValue(oEmpSalaryItem.EmployeeName, 0, 0, Element.ALIGN_LEFT, 1, nHeight);
                    }
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.ParentDepartment).Any())
                    {
                        this.SetCellValue(oEmpSalaryItem.ParentDepartmentName, 0, 0, Element.ALIGN_LEFT, 1, nHeight);
                    }
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.Department).Any())
                    {
                        this.SetCellValue(oEmpSalaryItem.DepartmentName, 0, 0, Element.ALIGN_LEFT, 1, nHeight);
                    }
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.Designation).Any())
                    {
                        this.SetCellValue(oEmpSalaryItem.DesignationName, 0, 0, Element.ALIGN_LEFT, 1, nHeight);
                    }
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.JoiningDate).Any())
                    {
                        this.SetCellValue(oEmpSalaryItem.JoiningDateInString, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.ConfirmationDate).Any())
                    {
                        this.SetCellValue(oEmpSalaryItem.DateOfConfirmationInString, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.EmployeeType).Any())
                    {
                        this.SetCellValue(oEmpSalaryItem.EmployeeTypeName, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.Gender).Any())
                    {
                        this.SetCellValue(oEmpSalaryItem.Gender, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.EmployeeContactNo).Any())
                    {
                        this.SetCellValue(oEmpSalaryItem.ContactNo, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                }

                if (ColAttDetail.Count() > 0)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.NORMAL);
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.TotalDays).Any())
                    {
                        var sValue = (oEmpSalaryItem.TotalDays > 0) ? (oEmpSalaryItem.TotalDays).ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.PresentDay).Any())
                    {
                        var sValue = (oEmpSalaryItem.Present > 0) ? oEmpSalaryItem.Present.ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.DayOffHolidays).Any())
                    {
                        var sValue = ((oEmpSalaryItem.TotalDayOff) > 0) ? (oEmpSalaryItem.TotalDayOff).ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.AbsentDays).Any())
                    {
                        var sValue = (oEmpSalaryItem.TotalAbsent > 0) ? oEmpSalaryItem.TotalAbsent.ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.LeaveHead).Any())
                    {
                        foreach (LeaveHead oLeaveHead in _oLeaveHeads)
                        {
                            string sValue = (oELOnAtts.Where(x => x.LeaveHeadID == oLeaveHead.LeaveHeadID).Any()) ? oELOnAtts.Where(x => x.LeaveHeadID == oLeaveHead.LeaveHeadID).FirstOrDefault().LeaveDays.ToString() : "-";
                            this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                        }
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.LeaveDays).Any())
                    {
                        var sValue = ((oEmpSalaryItem.TotalLeave) > 0) ? (oEmpSalaryItem.TotalLeave).ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.EmployeeWorkingDays).Any())
                    {
                        var sValue = (oEmpSalaryItem.EWD > 0) ? oEmpSalaryItem.EWD.ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.EarlyOutDays).Any())
                    {
                        var sValue = ((oEmpSalaryItem.TotalEarlyLeaving) > 0) ? (oEmpSalaryItem.TotalEarlyLeaving).ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.EarlyOutMins).Any())
                    {
                        var sValue = ((oEmpSalaryItem.EarlyInMin) > 0) ? (oEmpSalaryItem.EarlyInMin).ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.LateDays).Any())
                    {
                        var sValue = ((oEmpSalaryItem.TotalLate) > 0) ? (oEmpSalaryItem.TotalLate).ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.LateHrs).Any())
                    {
                        var sValue = ((oEmpSalaryItem.LateInMin) > 0) ? Global.MinInHourMin(oEmpSalaryItem.LateInMin) : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.OTHours).Any())
                    {
                        var sValue = (oEmpSalaryItem.OTHour > 0) ? oEmpSalaryItem.OTHour.ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                    if (AttDetailPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.OTRate).Any())
                    {
                        var sValue = (oEmpSalaryItem.OTRatePerHour > 0) ? oEmpSalaryItem.OTRatePerHour.ToString() : "-";
                        this.SetCellValue(sValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }
                }

                //List<TransferPromotionIncrement> oTPIs = new List<TransferPromotionIncrement>();
                //oTPIs = _oTransferPromotionIncrements.Where(x => x.EmployeeID == oEmpSalaryItem.EmployeeID).ToList();
                if (ColIncrementDetail.Count() > 0)
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.NORMAL);

                    string returnedValue = "";
                    returnedValue = (oEmpSalaryItem.LastGross > 0 ? this.GetAmountInStr(oEmpSalaryItem.LastGross, true, _bWithPrecision) : "-");
                    this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    increment["Last Gross"] = Convert.ToDouble(increment["Last Gross"]) + Math.Round(oEmpSalaryItem.LastGross > 0 ? oEmpSalaryItem.LastGross : 0);


                    returnedValue = (oEmpSalaryItem.IncrementAmount > 0 ? this.GetAmountInStr(oEmpSalaryItem.IncrementAmount, true, _bWithPrecision) : "-");
                    this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    increment["Last Increment"] = Convert.ToDouble(increment["Last Increment"]) + Math.Round(oEmpSalaryItem.IncrementAmount > 0 ? oEmpSalaryItem.IncrementAmount : 0);


                    this.SetCellValue(oEmpSalaryItem.EffectedDateInStr, 0, 0, Element.ALIGN_CENTER, 1, nHeight);

                }

                this.SetCellValue(((oEmpSalaryItem.GrossAmount > 0) ? this.GetAmountInStr(oEmpSalaryItem.GrossAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_RIGHT, 1, nHeight);
                gross["Gross Salary"] = Convert.ToDouble(gross["Gross Salary"]) + Math.Round(oEmpSalaryItem.GrossAmount);

                foreach (SalaryHead oItem in oEarnings.OrderBy(x => x.SalaryHeadType))
                {
                    double nAmount = GetAmount(oItem.SalaryHeadID, oEmpSalaryItem.EmployeeSalaryID);
                    nAmount = Math.Round(nAmount);

                    earnings[oItem.Name] = Convert.ToDouble(earnings[oItem.Name]) + nAmount;
                    nAddAmount += (oItem.SalaryHeadType == EnumSalaryHeadType.Addition) ? nAmount : 0;
                    this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, 25f);
                }

                double nOTAllowance = Math.Round(oEmpSalaryItem.OTAmount);

                if (_bHasOTAllowance)
                {
                    earnings[Global.CapitalSpilitor(EnumSalarySheetFormatProperty.OTAllowance.ToString())] = Convert.ToDouble(earnings[Global.CapitalSpilitor(EnumSalarySheetFormatProperty.OTAllowance.ToString())]) + nOTAllowance;
                    this.SetCellValue(((nOTAllowance > 0) ? this.GetAmountInStr(nOTAllowance, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, 25f);
                }

                //if (oEarnings.Count > 0 || _bHasOTAllowance)
                //{

                //    _oFontStyle = FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.BOLD);
                //    double nEarnings = Math.Round(oEmpSalaryItem.GrossAmount + ((_bHasOTAllowance) ? nOTAllowance : 0) + nAddAmount);
                //    this.SetCellValue(((nEarnings > 0) ? this.GetAmountInStr(nEarnings, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_RIGHT, 1, 25f);
                //    gross["Gross Earnings"] = Convert.ToDouble(gross["Gross Earnings"]) + nEarnings;
                //}

                //grossEarningOnAtt = (PD * oneDayGross) + OT+Addition
                if (oEarnings.Count > 0 || _bHasOTAllowance)// gross Earnings on Attendance
                {
                    _oFontStyle = FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.BOLD);
                    double nOnedayGross = 0;
                    if ((oEmpSalaryItem.Present + oEmpSalaryItem.TotalDayOff) > 0)
                    {
                        double dMonthDays = (oEmpSalaryItem.EndDate - oEmpSalaryItem.StartDate).TotalDays + 1;
                        //nOnedayGross = oEmpSalaryItem.GrossAmount / (oEmpSalaryItem.TotalWorkingDay + oEmpSalaryItem.TotalDayOff + oEmpSalaryItem.TotalHoliday); 
                        nOnedayGross = oEmpSalaryItem.GrossAmount / dMonthDays; 
                    }
                    double nEarningsOnAtt = 0.0;
                    if (IsCompliance == true)
                    {
                        nEarningsOnAtt = Math.Round(oEmpSalaryItem.PDComp * nOnedayGross + ((_bHasOTAllowance) ? nOTAllowance : 0) + nAddAmount);
                    }
                    //double nEarningsOnAtt = Math.Round(oEmpSalaryItem.PD * nOnedayGross + ((_bHasOTAllowance) ? nOTAllowance : 0) + nAddAmount);
                    nEarningsOnAtt = Math.Round(oEmpSalaryItem.PD * nOnedayGross + ((_bHasOTAllowance) ? nOTAllowance : 0) + nAddAmount);
                    
                    this.SetCellValue(((nEarningsOnAtt > 0) ? this.GetAmountInStr(nEarningsOnAtt, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_RIGHT, 1, 25f);
                    gross["Gross Earnings On Att."] = Convert.ToDouble(gross["Gross Earnings On Att."]) + nEarningsOnAtt;
                }

                if (IsCompliance == true)
                {
                    foreach (SalaryHead oItem in oBasics.OrderBy(x => x.SalaryHeadType))
                    {
                        double nAmount = GetAmount(oItem.SalaryHeadID, oEmpSalaryItem.EmployeeSalaryID);
                        nAmount = Math.Round(nAmount);


                        basics[oItem.Name] = Convert.ToDouble(basics[oItem.Name]) + nAmount;
                        nAddAmount += (oItem.SalaryHeadType == EnumSalaryHeadType.Basic) ? nAmount : 0;

                        this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, 25f);
                    }
                }


                _oFontStyle = FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.NORMAL);

                foreach (SalaryHead oItem in oDeductions)
                {
                    double nAmount = Math.Round(GetAmount(oItem.SalaryHeadID, oEmpSalaryItem.EmployeeSalaryID));
                    deductions[oItem.Name] = Convert.ToDouble(deductions[oItem.Name]) + nAmount;
                    nDeductionAmount += nAmount;
                    this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, 25f);
                }
                _oFontStyle = FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.BOLD);
                if (oDeductions.Count() > 0)
                {
                    gross["Gross Deductions"] = Convert.ToDouble(gross["Gross Deductions"]) + nDeductionAmount;
                    this.SetCellValue(((nDeductionAmount > 0) ? this.GetAmountInStr(nDeductionAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_RIGHT, 1, 25f);
                }

                _oFontStyle = FontFactory.GetFont("Tahoma", 19f, iTextSharp.text.Font.BOLD);
                double nNetAmount = oEmpSalaryItem.NetAmount + ((_bHasOTAllowance) ? 0 : -nOTAllowance);
                this.SetCellValue(((nNetAmount > 0) ? this.GetAmountInStr(nNetAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_RIGHT, 1, 25f);
                gross["Net Amount"] = Convert.ToDouble(gross["Net Amount"]) + nNetAmount;
                _oFontStyle = FontFactory.GetFont("Tahoma", 18f, iTextSharp.text.Font.NORMAL);

                if (ColBankDetail.Count() > 0)
                {
                    string returnedValue = "";
                    returnedValue = (oEmpSalaryItem.BankAmount > 0 ? this.GetAmountInStr(oEmpSalaryItem.BankAmount, true, _bWithPrecision) : "-");
                    this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    banks["Bank Amount"] = Convert.ToDouble(banks["Bank Amount"]) + (oEmpSalaryItem.BankAmount > 0 ? oEmpSalaryItem.BankAmount : 0);

                    //if (oBanks.Count <= 0) { oEmpSalaryItem.CashAmount = nNetAmount; }
                    returnedValue = (oEmpSalaryItem.CashAmount > 0 ? this.GetAmountInStr(oEmpSalaryItem.CashAmount, true, _bWithPrecision) : "-");
                    this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    banks["Cash Amount"] = Convert.ToDouble(banks["Cash Amount"]) + (oEmpSalaryItem.CashAmount > 0 ? oEmpSalaryItem.CashAmount : 0);

                    returnedValue = (oEmpSalaryItem.AccountNo != "" ? oEmpSalaryItem.AccountNo : "-");
                    this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);

                    returnedValue = (oEmpSalaryItem.BankName != "" ? oEmpSalaryItem.BankName : "-");
                    this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);

                }

                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, 25f);
                _oPdfPTable.CompleteRow();

                int nModBy = 8;
                if (IsCompliance)
                {
                    nModBy = 7;
                }

                if (UnitWiseRowCount % nModBy == 0)
                {
                    if (UnitWiseRowCount != oEmpSalarys.Count)
                    {
                        Summary(gross, increment, earnings, basics, deductions, ColGross, banks, false, false);
                        if (nTotalRowCount > nModBy)
                        {
                            Summary(_Prevgross, _Previncrement, _Prevearnings, _Prevbasics, _Prevdeductions, ColGross, _Prevbanks, false, true);
                            Summary(_gross, _increment, _earnings, basics, _deductions, ColGross, _banks, true, false);
                        }
                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();
                        this.PrintHeader(oEmpSalaryItem.BusinessUnitID);
                        _oFontStyle = FontFactory.GetFont("Tahoma", 25f, iTextSharp.text.Font.BOLD);
                        this.SetCellValue("Unit : " + oEmpSalaryItem.LocationName, 0, 8, Element.ALIGN_LEFT, 0, 30f);
                        this.SetCellValue("Department : " + oEmpSalaryItem.DepartmentName, 0, _nColumns - 8, Element.ALIGN_RIGHT, 0, 30f);
                        _oPdfPTable.CompleteRow();
                        this.ColumnSetup();
                    }
                }
            }
            int nCountForSummery = 8;
            if (IsCompliance)
            {
                nCountForSummery = 7;
            }
            Summary(gross, increment, earnings, basics, deductions, ColGross, banks, false, false);
            if (nTotalRowCount > nCountForSummery)
            {
                Summary(_Prevgross, _Previncrement, _Prevearnings, _Prevbasics, _Prevdeductions, ColGross, _Prevbanks, false, true);
                Summary(_gross, _increment, _earnings, basics, _deductions, ColGross, _banks, true, false);
            }
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();

        }

        private void ColumnSetup()
        {
            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            int nAddSpan = (_oLeaveHeads.Count() > 0) ? 1 : 0; // Leave Head Detail
            int nSpan = 2 + nAddSpan;
            foreach (string sColumn in ColumnHeader)
            {
                if (sColumn == "SL#")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                else if (sColumn == "Employee Information")
                    this.SetColumnProperty(sColumn, 0, ColEmpInfo.Count(), Element.ALIGN_CENTER);
                else if (sColumn == "Att. Detail")
                {
                    var colSpan = ColAttDetail.Count();
                    if (ColAttDetail.Where(x => x == (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))).Any())
                    {
                        colSpan = colSpan + _oLeaveHeads.Count() - 1;
                    }
                    this.SetColumnProperty(sColumn, 0, colSpan, Element.ALIGN_CENTER);
                }
                else if (sColumn == "Increment Detail")
                    this.SetColumnProperty(sColumn, 0, ColIncrementDetail.Count(), Element.ALIGN_CENTER);
                else if (sColumn == "Gross Salary")
                    this.SetColumnProperty(ColIncrementDetail.Count() > 0 ? "Present Salary" : sColumn, nSpan, 0, Element.ALIGN_CENTER);
                else if (sColumn == "Earnings")
                    this.SetColumnProperty(sColumn, 0, ColEarnings.Count(), Element.ALIGN_CENTER);
                //else if (sColumn == "Gross Earnings")
                //    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);


                else if(sColumn == "Basic Breakdown")
                    this.SetColumnProperty(sColumn, 0, ColBasics.Count(), Element.ALIGN_CENTER);


                else if (sColumn == "Gross Earnings On Att.")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                else if (sColumn == "Deduction")
                    this.SetColumnProperty(sColumn, 0, ColDeductions.Count(), Element.ALIGN_CENTER);
                else if (sColumn == "Gross Deductions")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                else if (sColumn == "Net Amount")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                else if (sColumn == "Bank")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                else if (sColumn == "Cash")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                else if (sColumn == "Account No")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                else if (sColumn == "Bank Name")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                else if (sColumn == "Signature")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
            }
            _oPdfPTable.CompleteRow();

            nSpan = 1 + nAddSpan;
            foreach (string sColumn in ColEmpInfo)
            {
                string sColName = "";
                sColName = sColumn;
                if (_bHasParentDept && sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.Department.ToString())) { sColName = "Sub Department"; }
                if (_bHasParentDept && sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.ParentDepartment.ToString())) { sColName = "Department"; }
                if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeCode.ToString())) { sColName = "Employee ID"; }

                this.SetColumnProperty(sColName, nSpan, 0, (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeCode.ToString()) == sColumn || Global.CapitalSpilitor(EnumSalarySheetFormatProperty.ConfirmationDate.ToString()) == sColumn) ? Element.ALIGN_CENTER : Element.ALIGN_LEFT);
            }
            foreach (string sColumn in ColAttDetail)
            {
                if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))
                {
                    this.SetColumnProperty(sColumn, 0, _oLeaveHeads.Count(), Element.ALIGN_CENTER);
                }
                else
                {
                    this.SetColumnProperty(sColumn == "Employee Working Days" ? "EWD" : sColumn, nSpan, 0, Element.ALIGN_CENTER);
                }
            }
            foreach (string sColumn in ColIncrementDetail)
            {
                //this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                string sColName = "";
                sColName = sColumn;
                if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LastGross.ToString())) { sColName = "Before Increment"; }
                this.SetColumnProperty(sColName, nSpan, 0, Element.ALIGN_CENTER);
            }
            foreach (string sColumn in ColEarnings)
            {
                this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
            }
            foreach (string sColumn in ColBasics)
            {
                this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
            }
            
            foreach (string sColumn in ColDeductions)
            {
                this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
            }

            if (ColAttDetail.Where(x => x == (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))).Any())
            {
                foreach (LeaveHead oItem in _oLeaveHeads)
                {
                    this.SetColumnProperty(oItem.ShortName, 0, 0, Element.ALIGN_CENTER);
                }
            }
        }

        private void SetColumnProperty(string sName, int nRowSpan, int nColumnSpan, int align)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            _oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            _oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            _oPdfPCell.BorderColor = BaseColor.GRAY;
            _oPdfPCell.HorizontalAlignment = align; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
        }

        private void SetCellValue(string sName, int nRowSpan, int nColumnSpan, int align, int border, float height)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            _oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            _oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            _oPdfPCell.BorderColor = BaseColor.GRAY;
            if (height > 0)
                _oPdfPCell.FixedHeight = height;
            if (border == 0)
                _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = align; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        }
        private void Summary(Dictionary<string, object> gross, Dictionary<string, object> increment, Dictionary<string, object> earnings, Dictionary<string, object> basics, Dictionary<string, object> deductions, string[] ColGross, Dictionary<string, object> banks, bool bIsGtotal, bool bIsPrev)
        {
            var value = "";
            float nHeight = 28f;
            _oFontStyle = FontFactory.GetFont("Tahoma", 16f, iTextSharp.text.Font.BOLD);
            int initialSpan = 1 + ColEmpInfo.Count() + ((ColAttDetail.Where(x => x == (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))).Any()) ? ColAttDetail.Count() + _oLeaveHeads.Count() - 1 : ColAttDetail.Count());
            if (bIsGtotal) { value = "Grand Total"; } else if (bIsPrev) { value = "Previous Total"; } else { value = "Total"; }
            this.SetCellValue(value, 0, initialSpan, Element.ALIGN_RIGHT, 1, nHeight);

            foreach (KeyValuePair<string, object> kvp in increment)
            {
                double nAmount = Convert.ToDouble(kvp.Value);
                this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, nHeight);
            }
            foreach (string property in ColIncrementDetail)
            {
                if (property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.IncrementEffectDate.ToString()))
                {
                    value = "";
                    this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);
                }
            }

            value = (Convert.ToDouble(gross["Gross Salary"]) > 0) ? this.GetAmountInStr(Convert.ToDouble(gross["Gross Salary"]), true, _bWithPrecision) : "-";
            this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);

            foreach (KeyValuePair<string, object> kvp in earnings)
            {
                double nAmount = Convert.ToDouble(kvp.Value);
                this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, nHeight);
            }
            if (earnings.Any())
            {
                //value = (Convert.ToDouble(gross["Gross Earnings"]) > 0) ? this.GetAmountInStr(Convert.ToDouble(gross["Gross Earnings"]), true, _bWithPrecision) : "-";
                //this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);

                value = (Convert.ToDouble(gross["Gross Earnings On Att."]) > 0) ? this.GetAmountInStr(Convert.ToDouble(gross["Gross Earnings On Att."]), true, _bWithPrecision) : "-";
                this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);
            }

            if (IsCompliance == true)
            {
                foreach (KeyValuePair<string, object> kvp in basics)
                {
                    double nAmount = Convert.ToDouble(kvp.Value);
                    this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                }
            }

            foreach (KeyValuePair<string, object> kvp in deductions)
            {
                double nAmount = Convert.ToDouble(kvp.Value);
                this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, nHeight);
            }
            if (deductions.Any())
            {
                value = (Convert.ToDouble(gross["Gross Deductions"]) > 0) ? this.GetAmountInStr(Convert.ToDouble(gross["Gross Deductions"]), true, _bWithPrecision) : "-";
                this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);
            }
            value = (Convert.ToDouble(gross["Net Amount"]) > 0) ? this.GetAmountInStr(Convert.ToDouble(gross["Net Amount"]), true, _bWithPrecision) : "-";
            this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);

            foreach (KeyValuePair<string, object> kvp in banks)
            {
                double nAmount = Convert.ToDouble(kvp.Value);
                this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, nHeight);
            }

            foreach (string property in ColBankDetail)
            {
                if (property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.AccountNo.ToString()))
                {
                    value = "";
                    this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);
                }
                if (property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.BankName.ToString()))
                {
                    value = "";
                    this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);
                }
            }

            value = "";
            this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);

            _oPdfPTable.CompleteRow();

            if (!bIsGtotal && !bIsPrev)
            {
                foreach (string key in ColGross)
                {
                    _gross[key] = Convert.ToDouble(_gross[key]) + Convert.ToDouble(gross[key]);
                    _Prevgross[key] = Convert.ToDouble(_gross[key]) - Convert.ToDouble(gross[key]);
                    gross[key] = 0;
                }
                foreach (string key in earnings.Select(x => x.Key).ToList())
                {
                    _earnings[key] = Convert.ToDouble(_earnings[key]) + Convert.ToDouble(earnings[key]);
                    _Prevearnings[key] = Convert.ToDouble(_earnings[key]) - Convert.ToDouble(earnings[key]);
                    earnings[key] = 0;
                }

                foreach (string key in basics.Select(x => x.Key).ToList())
                {
                    _basics[key] = Convert.ToDouble(_basics[key]) + Convert.ToDouble(basics[key]);
                    _Prevbasics[key] = Convert.ToDouble(_basics[key]) - Convert.ToDouble(basics[key]);
                    basics[key] = 0;
                }


                foreach (string key in deductions.Select(x => x.Key).ToList())
                {
                    _deductions[key] = Convert.ToDouble(_deductions[key]) + Convert.ToDouble(deductions[key]);
                    _Prevdeductions[key] = Convert.ToDouble(_deductions[key]) - Convert.ToDouble(deductions[key]);
                    deductions[key] = 0;
                }
                foreach (string key in increment.Select(x => x.Key).ToList())
                {
                    _increment[key] = Convert.ToDouble(_increment[key]) + Convert.ToDouble(increment[key]);
                    _Previncrement[key] = Convert.ToDouble(_increment[key]) - Convert.ToDouble(increment[key]);
                    increment[key] = 0;
                }
                foreach (string key in banks.Select(x => x.Key).ToList())
                {
                    _banks[key] = Convert.ToDouble(_banks[key]) + Convert.ToDouble(banks[key]);
                    _Prevbanks[key] = Convert.ToDouble(_banks[key]) - Convert.ToDouble(banks[key]);
                    banks[key] = 0;
                }
            }
        }

        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (bIsround) ? Math.Round(amount) : amount;
            return (bWithPrecision) ? Global.MillionFormat(amount) : Global.MillionFormat(amount).Split('.')[0];
        }

        public double GetAmount(int nSHID, int nESID)
        {
            double nAmount = 0;
            foreach (RPTSalarySheetDetail oESDItem in _oEmployeeSalaryDetails)
            {
                if (oESDItem.SalaryHeadID == nSHID && oESDItem.EmployeeSalaryID == nESID)
                {
                    nAmount += oESDItem.Amount;
                }
            }
            return nAmount;
        }
        #endregion
    }
}
