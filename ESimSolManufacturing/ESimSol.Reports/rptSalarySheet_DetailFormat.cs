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
using System.Diagnostics;

namespace ESimSol.Reports
{

    public class rptSalarySheet_DetailFormat
    {
        #region Declaration
        iTextSharp.text.Image _oImag;
        Document _oDocument;
        iTextSharp.text.Font _oFontStyle;
        int _nColumns = 0;

        int _nLegalWidth = 1008;
        int _nLegalHeight = 612;
        int _nA4Width = 842;
        int _nA4Height = 595;
        float _widthCounter;
        float nfinalWidth = 0;
        string _sPageName;
        bool flagIndication = true;

        int _nPageWidth = 0;
        int _npageHeight = 612;
        int _nEmpCount;
        int _nGrandEmpCount;
        bool isRound = true;
        PdfPTable _oPdfPTable;
        //= new PdfPTable(27);
        PdfPCell _oPdfPCell;
        MemoryStream _oMemoryStream = new MemoryStream();

        Company _oCompany = new Company();
        List<RPTSalarySheet> _oEmployeeSalarys = new List<RPTSalarySheet>();
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
        List<string> ColDeductions = new List<string>();
        List<string> ColBankDetail = new List<string>();

        string[] ColGross = new string[] { };
        Dictionary<string, object> _gross = new Dictionary<string, object>();
        Dictionary<string, object> _increment = new Dictionary<string, object>();
        Dictionary<string, object> _earnings = new Dictionary<string, object>();
        Dictionary<string, object> _deductions = new Dictionary<string, object>();
        Dictionary<string, object> _banks = new Dictionary<string, object>();

        Dictionary<string, object> _GTgross = new Dictionary<string, object>();
        Dictionary<string, object> _GTincrement = new Dictionary<string, object>();
        Dictionary<string, object> _GTearnings = new Dictionary<string, object>();
        Dictionary<string, object> _GTdeductions = new Dictionary<string, object>();
        Dictionary<string, object> _GTbanks = new Dictionary<string, object>();

        bool _bWithPrecision = true;
        bool _bHasOTAllowance = false;
        bool _bHasParentDept = false;
        string _sStartDate = "";
        string _sEndDate = "";
        bool _bGroupByDept = false;
        bool _bGroupBySerial = false;
        int nTotalCount;
        double gTotal;
        double otTotal;
        #endregion

        public byte[] PrepareReport(EmployeeSalary oEmployeeSalary, List<SalarySheetProperty> oSalarySheetPropertys, List<LeaveHead> oLeaveHeads, bool bWithLeaveHeads, List<EmployeeLeaveOnAttendance> oELOnAttendances, bool bWithPrecision, List<SalarySheetSignature> oSalarySheetSignatures, bool bGroupByDept, bool bGroupBySerial, bool bRound)
        {
            isRound = bRound;
            _bGroupByDept = bGroupByDept;
            _bGroupBySerial = bGroupBySerial;
            _oEmployeeSalarys = oEmployeeSalary.EmployeeSalarySheets.OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeSalaryID).ToList();
            _oEmployeeSalaryDetails = oEmployeeSalary.EmployeeSalarySheetDetails;
            _oEmployeeSalaryDetailDisciplinaryActions = oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions;
            _oSalaryHeads = oEmployeeSalary.SalaryHeads.OrderBy(x => x.Name).ToList();
            _oEmployeeBankAccounts = oEmployeeSalary.EmployeeBankAccounts;
            _oTransferPromotionIncrements = oEmployeeSalary.TransferPromotionIncrements;
            _oCompany = oEmployeeSalary.Company;
            _oBusinessUnits = oEmployeeSalary.BusinessUnits;
            oSalarySheetPropertys = oSalarySheetPropertys.OrderBy(x => x.SalarySheetFormatPropertyInt).ToList();

            _oLeaveHeads = oLeaveHeads.Where(p => oELOnAttendances.Any(p2 => p2.LeaveHeadID == p.LeaveHeadID)).ToList();

            if (_oLeaveHeads.Count <= 0)
            {
                oSalarySheetPropertys.RemoveAll(x => x.SalarySheetFormatPropertyInt == 340);
            }

            _oSalarySheetPropertys = oSalarySheetPropertys;
            //_oLeaveHeads = oLeaveHeads;
            _bWithLeaveHeads = bWithLeaveHeads;
            _oELOnAttendances = oELOnAttendances;
            //_bWithPrecision = bWithPrecision;


            ColEmpInfo = oSalarySheetPropertys.Where(x => x.PropertyFor == 1).Select(p => p.SalarySheetFormatPropertyStr).ToList();
            ColAttDetail = oSalarySheetPropertys.Where(x => x.PropertyFor == 2 && x.SalarySheetFormatProperty != EnumSalarySheetFormatProperty.OTAllowance).Select(p => p.SalarySheetFormatPropertyStr).ToList();
            ColIncrementDetail = oSalarySheetPropertys.Where(x => x.PropertyFor == 3).Select(p => p.SalarySheetFormatPropertyStr).ToList();
            ColBankDetail = oSalarySheetPropertys.Where(x => x.PropertyFor == 4).Select(p => p.SalarySheetFormatPropertyStr).ToList();

            ColEarnings = _oSalaryHeads.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Basic).Select(x => x.Name).ToList();
            ColEarnings.AddRange(_oSalaryHeads.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Addition).Select(x => x.Name).ToList());
            ColEarnings.AddRange(_oSalaryHeads.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Reimbursement).Select(x => x.Name).ToList());

            if (oSalarySheetPropertys.Any(x => x.PropertyFor == 2 && x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.OTAllowance))
            {
                ColEarnings.AddRange(oSalarySheetPropertys.Where(x => x.PropertyFor == 2 && x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.OTAllowance).Select(x => x.SalarySheetFormatPropertyStr));
                _bHasOTAllowance = true;
            }

            ColDeductions = _oSalaryHeads.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction).Select(x => x.Name).ToList();

            var oEarnings = _oSalaryHeads.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Basic) || (x.SalaryHeadType == EnumSalaryHeadType.Addition) || (x.SalaryHeadType == EnumSalaryHeadType.Reimbursement)).ToList();
            var oDeductions = _oSalaryHeads.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction).ToList();



            ColumnHeader = new List<string>() { "SL#", 
            (ColEmpInfo.Count() > 0 ? "Employee Information" : ""), 
            (ColAttDetail.Count() > 0 ? "Att. Detail" : ""),
            (ColIncrementDetail.Count>0 ? "Increment Detail" : ""),
            "Gross Salary", 
            ((ColEarnings.Count() > 0) ? "Earnings" : ""),
            ((ColEarnings.Count() > 0) ? "Gross Earnings" : ""),

            ((ColDeductions.Count() > 0) ? "Deduction" : ""),
            ((ColDeductions.Count() > 0) ? "Gross Deductions" : ""),

            "Net Amount", 

            (ColBankDetail.Count>0?"Bank Information":""),

            //(ColBankDetail.Count>0?"Bank":""),
            //(ColBankDetail.Count>0?"Cash":""),
            //(ColBankDetail.Count>0?"Account No":""),
            //(ColBankDetail.Count>0?"Bank Name":""), 
            "Signature" };
            ColumnHeader.RemoveAll(x => x == "");


            ColGross = new string[] { "Gross Salary", ((ColEarnings.Count() > 0) ? "Gross Earnings" : ""), ((ColDeductions.Count() > 0) ? "Gross Deductions" : ""), "Net Amount" };
            ColGross = ColGross.Where(x => x != "").ToArray();

            foreach (string property in ColGross)
            {
                _gross.Add(property, 0);
                _GTgross.Add(property, 0);
            }
            foreach (string property in ColIncrementDetail)
            {
                if (property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LastGross.ToString()) || property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LastIncrement.ToString()))
                {
                    _increment.Add(property, 0);
                    _GTincrement.Add(property, 0); 
                }
            }
            foreach (string property in ColEarnings)
            {
                _earnings.Add(property, 0);
                _GTearnings.Add(property, 0);
            }
            foreach (string property in ColDeductions)
            {
                _deductions.Add(property, 0);
                _GTdeductions.Add(property, 0);
            }
            foreach (string property in ColBankDetail)
            {
                if (property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.BankAmount.ToString()) || property == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.CashAmount.ToString()))
                {
                    _banks.Add(property, 0);
                    _GTbanks.Add(property, 0); 
                }
            }
            DateTime sStartDate = Convert.ToDateTime(oEmployeeSalary.ErrorMessage.Split(',')[0]);
            DateTime sEndDate = Convert.ToDateTime(oEmployeeSalary.ErrorMessage.Split(',')[1]);
            _sStartDate = sStartDate.ToString("dd MMM yyyy");
            _sEndDate = sEndDate.ToString("dd MMM yyyy");

            #region Page Setup

            _nColumns = 1 +
                        ColEmpInfo.Count() +
                        ColAttDetail.Count() +
                        ((bWithLeaveHeads) ? _oLeaveHeads.Count > 0 ? _oLeaveHeads.Count() - 1 : _oLeaveHeads.Count() : 0)
                        + 1 +
                        ColEarnings.Count() +
                        ((ColEarnings.Count() > 0) ? 1 : 0) +
                        ColDeductions.Count() +
                        ((ColDeductions.Count() > 0) ? 1 : 0) +
                        ColIncrementDetail.Count() +
                        ColBankDetail.Count() +
                        2;

            int nColumn = 0;
            float[] tablecolumns = new float[_nColumns];

            _nPageWidth = 25;
            tablecolumns[nColumn++] = 25f;
            foreach (string sColumn in ColEmpInfo)
            {
                if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeCode.ToString()))
                {
                    _nPageWidth += 60;
                    tablecolumns[nColumn++] = 60f;
                }

                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeName.ToString()))
                {
                    _nPageWidth += 90;
                    tablecolumns[nColumn++] = 90f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.ParentDepartment.ToString()))
                {
                    _nPageWidth += 90;
                    tablecolumns[nColumn++] = 90f;
                    _bHasParentDept = true;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.Department.ToString()))
                {
                    _nPageWidth += 90;
                    tablecolumns[nColumn++] = 90f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.Designation.ToString()))
                {
                    _nPageWidth += 90;
                    tablecolumns[nColumn++] = 90f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.JoiningDate.ToString()))
                {
                    _nPageWidth += 70;
                    tablecolumns[nColumn++] = 70f;
                }

                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.ConfirmationDate.ToString()))
                {
                    _nPageWidth += 70;
                    tablecolumns[nColumn++] = 70f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeType.ToString()))
                {
                    _nPageWidth += 70;
                    tablecolumns[nColumn++] = 70f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.Gender.ToString()))
                {
                    _nPageWidth += 60;
                    tablecolumns[nColumn++] = 60f;
                }
                else if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeContactNo.ToString()))
                {
                    _nPageWidth += 60;
                    tablecolumns[nColumn++] = 70f;
                }
            }

            foreach (string sColumn in ColAttDetail)
            {
                if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))
                {
                    foreach (LeaveHead oItem in _oLeaveHeads)
                    {
                        _nPageWidth += 30;
                        tablecolumns[nColumn++] = 30f;
                    }
                }
                else
                {
                    _nPageWidth += 45;
                    tablecolumns[nColumn++] = 45f;
                }
            }

            foreach (string sColumn in ColIncrementDetail)
            {
                _nPageWidth += 60;
                tablecolumns[nColumn++] = 60f;
            }

            _nPageWidth += 60;
            tablecolumns[nColumn++] = 60f;

            foreach (string sColumn in ColEarnings)
            {
                _nPageWidth += 60;
                tablecolumns[nColumn++] = 60f;
            }
            if (ColEarnings.Count() > 0)
            {
                _nPageWidth += 63;
                tablecolumns[nColumn++] = 63f;
            }
            //if (_bHasOTAllowance)
            //{
            //    _nPageWidth += 63;
            //    tablecolumns[nColumn++] = 63f;
            //}
            foreach (string sColumn in ColDeductions)
            {
                _nPageWidth += 50;
                tablecolumns[nColumn++] = 50f;
            }
            if (ColDeductions.Count() > 0)
            {
                _nPageWidth += 60;
                tablecolumns[nColumn++] = 60f;
            }
            _nPageWidth += 65;
            tablecolumns[nColumn++] = 65f;

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

            _nPageWidth += 60;
            tablecolumns[nColumn++] = 85f;

            //_nPageWidth += 60;
            //tablecolumns[nColumn++] = 85f;

            for (int i = 0; i < tablecolumns.Count(); i++)
            {
                _widthCounter += tablecolumns[i];
            }

            float[] tablecolumnsTemp = new float[_nColumns];
            for (int i = 0; i < tablecolumns.Count(); i++)
            {
                tablecolumnsTemp[i] = tablecolumns[i];
            }
            float difference = 0;
            float pixelPerCell = 0;
            if ((_widthCounter) <= _nA4Width)
            {
                _nPageWidth = _nA4Width;
                _npageHeight = _nA4Height;
                _sPageName = "A4";
                difference = Math.Abs(_widthCounter - _nA4Width);
                pixelPerCell = difference / tablecolumns.Count();
                for (int i = 0; i < tablecolumns.Count(); i++)
                {
                    tablecolumns[i] += pixelPerCell;
                }
            }
            else if (((_widthCounter) > _nA4Width) && (Math.Abs(_widthCounter - _nA4Width) <= 100))
            {
                _nPageWidth = _nA4Width;
                _npageHeight = _nA4Height;
                _sPageName = "A4";
                difference = Math.Abs(_widthCounter - _nA4Width);
                pixelPerCell = difference / tablecolumns.Count();
                for (int i = 0; i < tablecolumns.Count(); i++)
                {
                    tablecolumns[i] -= pixelPerCell;
                }
            }

            else if ((_widthCounter <= _nLegalWidth))
            {
                _nPageWidth = _nLegalWidth;
                _npageHeight = _nLegalHeight;
                _sPageName = "Legal";
                difference = Math.Abs(_widthCounter - _nA4Width);
                pixelPerCell = difference / tablecolumns.Count();
                for (int i = 0; i < tablecolumns.Count(); i++)
                {
                    tablecolumns[i] += pixelPerCell;
                }
            }
            else if (((_widthCounter) > _nLegalWidth) && (Math.Abs(_widthCounter - _nLegalWidth) <= 850))
            {
                _nPageWidth = _nLegalWidth;
                _npageHeight = _nLegalHeight;
                _sPageName = "Legal";
                difference = Math.Abs(_widthCounter - _nLegalWidth);
                pixelPerCell = difference / tablecolumns.Count();

                double nPerPixel = difference / _widthCounter;
                for (int i = 0; i < tablecolumns.Count(); i++)
                {
                    tablecolumns[i] *= (float)nPerPixel;
                    tablecolumnsTemp[i] -= tablecolumns[i];
                    nfinalWidth += tablecolumnsTemp[i];
                    //tablecolumns[i] -= pixelPerCell;
                }
            }
            else
            {
                _sPageName = "Legal";
                nfinalWidth = _widthCounter;
            }


            _oPdfPTable = new PdfPTable(_nColumns);
            _oDocument = new Document(new iTextSharp.text.Rectangle(nfinalWidth, _npageHeight), 0f, 0f, 0f, 0f);
            //_oDocument = new Document(new iTextSharp.text.Rectangle(633.13f, 942.50f).Rotate(), 35.97f, 133.10f, 35.97f, 35.97f); //A4 Size Paper = height:842   width:595 that means 1 Inch = 71.94679564691657 pixel

            _oDocument.SetMargins(25f, 25f, 10f, 10f);
            _oPdfPTable.WidthPercentage = 100;
            _oPdfPTable.HorizontalAlignment = Element.ALIGN_CENTER;

            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter oPDFWriter = PdfWriter.GetInstance(_oDocument, _oMemoryStream);

            if (_oCompany.BaseAddress == "dachser")
            {
                ESimSolFooter_Dachser_Detailformat PageEventHandler = new ESimSolFooter_Dachser_Detailformat();
                oPDFWriter.PageEvent = PageEventHandler;
            }
            else
            {
                //ESimSolFooter_Signature PageEventHandler = new ESimSolFooter_Signature();
                //oPDFWriter.PageEvent = PageEventHandler;

                ESimSolFooter PageEventHandler = new ESimSolFooter();
                PageEventHandler.signatures = oSalarySheetSignatures.Select(x => x.SignatureName).ToList();
                PageEventHandler.PrintPrintingDateTime = false;
                //PageEventHandler.nFontSize = 15;
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

        private void PrintHeader(int nBusinessUnitID, bool bFlag)
        {
            #region CompanyHeader

            PdfPTable oPdfPTableHeader = new PdfPTable(3);
            oPdfPTableHeader.SetWidths(new float[] { nfinalWidth / 3 - 100, nfinalWidth / 3 + 100, nfinalWidth / 3 });
            PdfPCell oPdfPCellHearder;

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = _oBusinessUnits.Where(x => x.BusinessUnitID == nBusinessUnitID).ToList();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            //oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            _oFontStyle = FontFactory.GetFont("Tahoma", 20f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oBusinessUnits.Count > 0 ? oBusinessUnits[0].Name : "", _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            //oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            _oFontStyle = FontFactory.GetFont("Tahoma", 15f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase("Salary Sheet" + "\n" + "From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_RIGHT;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            //oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);


            oPdfPTableHeader.CompleteRow();

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            //oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            _oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase(oBusinessUnits.Count > 0 ? oBusinessUnits[0].Address : "", _oFontStyle));
            //oPdfPCellHearder.Colspan = 2;
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_CENTER;
            oPdfPCellHearder.Border = 0;
            oPdfPCellHearder.BackgroundColor = BaseColor.WHITE;
            oPdfPCellHearder.ExtraParagraphSpace = 0;
            oPdfPTableHeader.AddCell(oPdfPCellHearder);

            _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
            oPdfPCellHearder = new PdfPCell(new Phrase("", _oFontStyle));
            oPdfPCellHearder.HorizontalAlignment = Element.ALIGN_LEFT;
            oPdfPCellHearder.Border = 0;
            //oPdfPCellHearder.FixedHeight = 15;
            //oPdfPCellHearder.Colspan = 2;
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

            if (bFlag)
            {
                _sPageName += " Page Needed";
            }
            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("Salary Sheet", _oFontStyle));//(" + _sPageName +")"
            //_oPdfPCell.Colspan = _nColumns;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

            //_oFontStyle = FontFactory.GetFont("Tahoma", 10f, iTextSharp.text.Font.BOLD);
            //_oPdfPCell = new PdfPCell(new Phrase("From " + _sStartDate + " To " + _sEndDate, _oFontStyle));
            //_oPdfPCell.Colspan = _nColumns;
            //_oPdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_oPdfPCell.Border = 0;
            //_oPdfPCell.BackgroundColor = BaseColor.WHITE;
            //_oPdfPCell.ExtraParagraphSpace = 0;
            //_oPdfPTable.AddCell(_oPdfPCell);
            //_oPdfPTable.CompleteRow();

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

        private void PrintBody()
        {
            if (_oEmployeeSalarys.Count <= 0)
            {
                _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
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
                List<RPTSalarySheet> oEmployeeSs = new List<RPTSalarySheet>();
                _oEmployeeSalarys.ForEach(x => oEmployeeSs.Add(x));
                //Int32[] locations = _oEmployeeSalarys.Select(x => x.LocationID).Distinct().ToArray();
                var EmpInfoPropertys = _oSalarySheetPropertys.Where(x => x.PropertyFor == 1).ToList();
                var AttDetailPropertys = _oSalarySheetPropertys.Where(x => x.PropertyFor == 2).ToList();
                var IncrementPropertys = _oSalarySheetPropertys.Where(x => x.PropertyFor == 3).ToList();
                var BankPropertys = _oSalarySheetPropertys.Where(x => x.PropertyFor == 4).ToList();

                //foreach (Int32 locationId in locations)
                //{
                while (oEmployeeSs.Count > 0)
                {
                    //List<EmployeeSalary> oTempEmployeeSs = new List<EmployeeSalary>();
                    var oResults = new List<RPTSalarySheet>();
                    if (_bGroupByDept && _bGroupBySerial)
                    {
                        oResults = oEmployeeSs.Where(x => x.LocationID == oEmployeeSs[0].LocationID && x.DepartmentID == oEmployeeSs[0].DepartmentID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();


                    }
                    else if (_bGroupByDept && (_bGroupBySerial == false))
                    {
                        oResults = oEmployeeSs.Where(x => x.LocationID == oEmployeeSs[0].LocationID && x.DepartmentID == oEmployeeSs[0].DepartmentID).OrderBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();


                    }
                    else
                    {
                        oResults = oEmployeeSs.Where(x => x.LocationID == oEmployeeSs[0].LocationID).OrderBy(x => x.LocationName).ThenBy(x => x.EmployeeCode).ToList();

                    }

                    PrintHeader(oResults[0].BusinessUnitID, flagIndication);
                    flagIndication = false;

                    _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                    if (_bGroupByDept)
                        this.SetCellValue("Location Name: " + oResults.FirstOrDefault().LocationName + ", Department Name : " + oResults.FirstOrDefault().DepartmentName, 0, _nColumns, Element.ALIGN_LEFT, 0, 30f);
                    else
                        this.SetCellValue("Location Name: " + oResults.FirstOrDefault().LocationName, 0, _nColumns, Element.ALIGN_LEFT, 0, 30f);
                    _oPdfPTable.CompleteRow();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
                    this.ColumnSetup();

                    _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                    this.SalarySheet(oResults, EmpInfoPropertys, AttDetailPropertys, IncrementPropertys, BankPropertys);
                    if (_bGroupByDept)
                        oEmployeeSs.RemoveAll(x => x.LocationID == oResults[0].LocationID && x.DepartmentID == oResults[0].DepartmentID);
                    else
                        oEmployeeSs.RemoveAll(x => x.LocationID == oResults[0].LocationID);

                    //}
                }
            }
        }

        private void SalarySheet(List<RPTSalarySheet> oEmpSalarys, List<SalarySheetProperty> EmpInfoPropertys, List<SalarySheetProperty> AttDetailPropertys, List<SalarySheetProperty> IncrementPropertys, List<SalarySheetProperty> BankPropertys)
        {
            int nTotalRowCount = 0;
            int UnitWiseRowCount = 0;
            int nRow = 0;
            int nCount = 0;
            double nAddAmount = 0, nDeductionAmount = 0;
            var oELOnAtts = new List<EmployeeLeaveOnAttendance>();
            var oEarnings = _oSalaryHeads.Where(x => (x.SalaryHeadType == EnumSalaryHeadType.Basic) || (x.SalaryHeadType == EnumSalaryHeadType.Addition) || (x.SalaryHeadType == EnumSalaryHeadType.Reimbursement)).ToList();
            var oDeductions = _oSalaryHeads.Where(x => x.SalaryHeadType == EnumSalaryHeadType.Deduction).ToList();

            Dictionary<string, object> gross = new Dictionary<string, object>();
            Dictionary<string, object> increment = new Dictionary<string, object>();
            Dictionary<string, object> earnings = new Dictionary<string, object>();
            Dictionary<string, object> deductions = new Dictionary<string, object>();
            Dictionary<string, object> banks = new Dictionary<string, object>();

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
            foreach (string property in _deductions.Select(x => x.Key).ToArray())
            {
                deductions.Add(property, 0);
            }
            foreach (string property in _banks.Select(x => x.Key).ToArray())
            {
                banks.Add(property, 0);
            }
            float nHeight = 72f;

            foreach (RPTSalarySheet oEmpSalaryItem in oEmpSalarys)
            {
                nCount++; nAddAmount = 0; nDeductionAmount = 0; ++nRow; UnitWiseRowCount++; nTotalRowCount++; _nEmpCount += 1; _nGrandEmpCount += 1; nTotalCount++;
                oELOnAtts = _oELOnAttendances.Where(x => x.EmployeeID == oEmpSalaryItem.EmployeeID).ToList();
                this.SetCellValue((_bGroupByDept && (_bGroupBySerial == false)) ? nTotalCount.ToString() : nCount.ToString(), 0, 0, Element.ALIGN_CENTER, 1, nHeight);

                if (ColEmpInfo.Count() > 0)
                {
                    if (EmpInfoPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.EmployeeCode).Any())
                    {
                        this.SetCellValue(oEmpSalaryItem.EmployeeCode, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
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
                        var sValue = ((oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave) > 0) ? (oEmpSalaryItem.TotalPLeave + oEmpSalaryItem.TotalUpLeave).ToString() : "-";
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
                if (ColIncrementDetail.Count() > 0)
                {
                    string returnedValue = "";
                    returnedValue = (oEmpSalaryItem.LastGross > 0 ? this.GetAmountInStr(oEmpSalaryItem.LastGross, true, _bWithPrecision) : "-");
                    this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    _increment["Last Gross"] = Convert.ToDouble(_increment["Last Gross"]) + (oEmpSalaryItem.LastGross > 0 ? oEmpSalaryItem.LastGross : 0);
                    _GTincrement["Last Gross"] = Convert.ToDouble(_GTincrement["Last Gross"]) + (oEmpSalaryItem.LastGross > 0 ? oEmpSalaryItem.LastGross : 0);


                    returnedValue = (oEmpSalaryItem.IncrementAmount > 0 ? this.GetAmountInStr(oEmpSalaryItem.IncrementAmount, true, _bWithPrecision) : "-");
                    this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    _increment["Last Increment"] = Convert.ToDouble(_increment["Last Increment"]) + (oEmpSalaryItem.IncrementAmount > 0 ? oEmpSalaryItem.IncrementAmount : 0);
                    _GTincrement["Last Increment"] = Convert.ToDouble(_GTincrement["Last Increment"]) + (oEmpSalaryItem.IncrementAmount > 0 ? oEmpSalaryItem.IncrementAmount : 0);

                    this.SetCellValue(oEmpSalaryItem.EffectedDateInStr, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                }


                this.SetCellValue(((oEmpSalaryItem.GrossAmount > 0) ? this.GetAmountInStr(oEmpSalaryItem.GrossAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_RIGHT, 1, nHeight);
                gross["Gross Salary"] = Convert.ToDouble(gross["Gross Salary"]) + oEmpSalaryItem.GrossAmount;
                _GTgross["Gross Salary"] = Convert.ToDouble(_GTgross["Gross Salary"]) + oEmpSalaryItem.GrossAmount;

                foreach (SalaryHead oItem in oEarnings.OrderBy(x => x.SalaryHeadType))
                {
                    double nAmount = GetAmount(oItem.SalaryHeadID, oEmpSalaryItem.EmployeeSalaryID);

                    //nAmount = (oItem.SalaryHeadType == EnumSalaryHeadType.Basic) ? nAmount : Math.Round(nAmount);
                    nAmount = Math.Round(nAmount);

                    earnings[oItem.Name] = Convert.ToDouble(earnings[oItem.Name]) + nAmount;
                    _GTearnings[oItem.Name] = Convert.ToDouble(_GTearnings[oItem.Name]) + nAmount;
                    nAddAmount += (oItem.SalaryHeadType == EnumSalaryHeadType.Addition || oItem.SalaryHeadType == EnumSalaryHeadType.Reimbursement) ? nAmount : 0;
                    //this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, (oItem.SalaryHeadType == EnumSalaryHeadType.Basic) ? true : _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, 25f);
                    this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, 25f);
                }

                double nOTAllowance = Math.Round(oEmpSalaryItem.OTAmount);

                if (_bHasOTAllowance)
                {
                    earnings[Global.CapitalSpilitor(EnumSalarySheetFormatProperty.OTAllowance.ToString())] = Convert.ToDouble(earnings[Global.CapitalSpilitor(EnumSalarySheetFormatProperty.OTAllowance.ToString())]) + nOTAllowance;
                    this.SetCellValue(((nOTAllowance > 0) ? this.GetAmountInStr(nOTAllowance, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, 25f);
                }

                double nEarnings = Math.Round(oEmpSalaryItem.GrossAmount + ((_bHasOTAllowance) ? nOTAllowance : 0) + nAddAmount);
                this.SetCellValue(((nEarnings > 0) ? this.GetAmountInStr(nEarnings, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_RIGHT, 1, 25f);
                gross["Gross Earnings"] = Convert.ToDouble(gross["Gross Earnings"]) + nEarnings;
                _GTgross["Gross Earnings"] = Convert.ToDouble(_GTgross["Gross Earnings"]) + nEarnings;

                foreach (SalaryHead oItem in oDeductions)
                {
                    double nAmount = Math.Round(GetAmount(oItem.SalaryHeadID, oEmpSalaryItem.EmployeeSalaryID));
                    deductions[oItem.Name] = Convert.ToDouble(deductions[oItem.Name]) + nAmount;
                    _GTdeductions[oItem.Name] = Convert.ToDouble(_GTdeductions[oItem.Name]) + nAmount;
                    nDeductionAmount += nAmount;
                    this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, 25f);
                }
                if (oDeductions.Count() > 0)
                {
                    gross["Gross Deductions"] = Convert.ToDouble(gross["Gross Deductions"]) + nDeductionAmount;
                    _GTgross["Gross Deductions"] = Convert.ToDouble(_GTgross["Gross Deductions"]) + nDeductionAmount;
                    this.SetCellValue(((nDeductionAmount > 0) ? this.GetAmountInStr(nDeductionAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_RIGHT, 1, 25f);
                }
                //otTotal += oEmpSalaryItem.OTAmount;
                //gTotal += oEmpSalaryItem.NetAmount;
                double nNetAmount = oEmpSalaryItem.NetAmount;// + ((_bHasOTAllowance) ? 0 : -oEmpSalaryItem.OTAmount);
                if (_bHasOTAllowance == false)
                {
                    nNetAmount -= oEmpSalaryItem.OTAmount;
                }
                this.SetCellValue(((nNetAmount > 0) ? this.GetAmountInStr(nNetAmount, true, true) : "-"), 0, 0, Element.ALIGN_RIGHT, 1, 25f);
                gross["Net Amount"] = Convert.ToDouble(gross["Net Amount"]) + nNetAmount;
                _GTgross["Net Amount"] = Convert.ToDouble(_GTgross["Net Amount"]) + nNetAmount;

                if (ColBankDetail.Count() > 0)
                {
                    string returnedValue = "";

                    if (BankPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.BankAmount).Any())
                    {
                        if (_bHasOTAllowance == false && oEmpSalaryItem.BankAmount > 0)
                        {
                            oEmpSalaryItem.BankAmount -= oEmpSalaryItem.OTAmount;
                        }
                        returnedValue = (oEmpSalaryItem.BankAmount > 0 ? this.GetAmountInStr(oEmpSalaryItem.BankAmount, true, _bWithPrecision) : "-");
                        this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                        banks["Bank Amount"] = Convert.ToDouble(banks["Bank Amount"]) + (oEmpSalaryItem.BankAmount > 0 ? oEmpSalaryItem.BankAmount : 0);
                        _GTbanks["Bank Amount"] = Convert.ToDouble(_GTbanks["Bank Amount"]) + (oEmpSalaryItem.BankAmount > 0 ? oEmpSalaryItem.BankAmount : 0);
                    }

                    if (BankPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.CashAmount).Any())
                    {
                        if (_bHasOTAllowance == false && oEmpSalaryItem.BankAmount <= 0 && oEmpSalaryItem.CashAmount > 0)
                        {
                            oEmpSalaryItem.CashAmount -= oEmpSalaryItem.OTAmount;
                        }

                        returnedValue = (oEmpSalaryItem.CashAmount > 0 ? this.GetAmountInStr(oEmpSalaryItem.CashAmount, true, _bWithPrecision) : "-");
                        this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                        banks["Cash Amount"] = Convert.ToDouble(banks["Cash Amount"]) + (oEmpSalaryItem.CashAmount > 0 ? oEmpSalaryItem.CashAmount : 0);
                        _GTbanks["Cash Amount"] = Convert.ToDouble(_GTbanks["Cash Amount"]) + (oEmpSalaryItem.CashAmount > 0 ? oEmpSalaryItem.CashAmount : 0);
                    }

                    if (BankPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.AccountNo).Any())
                    {
                        returnedValue = (oEmpSalaryItem.AccountNo != "" ? oEmpSalaryItem.AccountNo : "-");
                        this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }

                    if (BankPropertys.Where(x => x.SalarySheetFormatProperty == EnumSalarySheetFormatProperty.BankName).Any())
                    {
                        returnedValue = (oEmpSalaryItem.BankName != "" ? oEmpSalaryItem.BankName : "-");
                        this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    }


                    //if (oEmpSalaryItem.BankAmount > 0)
                    //{
                    //    oEmpSalaryItem.BankAmount -= ((_bHasOTAllowance) ? 0 : oEmpSalaryItem.OTAmount);
                    //}
                    //if (_bHasOTAllowance == false && oEmpSalaryItem.BankAmount > 0)
                    //{
                    //    oEmpSalaryItem.BankAmount -= oEmpSalaryItem.OTAmount;
                    //}
                    //returnedValue = (oEmpSalaryItem.BankAmount > 0 ? this.GetAmountInStr(oEmpSalaryItem.BankAmount, true, _bWithPrecision) : "-");
                    //this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    //banks["Bank Amount"] = Convert.ToDouble(banks["Bank Amount"]) + (oEmpSalaryItem.BankAmount > 0 ? oEmpSalaryItem.BankAmount : 0);

                    ////if (oEmpSalaryItem.BankAmount <= 0 && oEmpSalaryItem.CashAmount > 0)
                    ////{
                    ////    oEmpSalaryItem.CashAmount -= ((_bHasOTAllowance) ? 0 : oEmpSalaryItem.OTAmount);
                    ////}
                    //if (_bHasOTAllowance == false && oEmpSalaryItem.BankAmount <= 0 && oEmpSalaryItem.CashAmount > 0)
                    //{
                    //    oEmpSalaryItem.CashAmount -= oEmpSalaryItem.OTAmount;
                    //}

                    //returnedValue = (oEmpSalaryItem.CashAmount > 0 ? this.GetAmountInStr(oEmpSalaryItem.CashAmount, true, _bWithPrecision) : "-");
                    //this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);
                    //banks["Cash Amount"] = Convert.ToDouble(banks["Cash Amount"]) + (oEmpSalaryItem.CashAmount > 0 ? oEmpSalaryItem.CashAmount : 0);

                    //returnedValue = (oEmpSalaryItem.AccountNo != "" ? oEmpSalaryItem.AccountNo : "-");
                    //this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);

                    //returnedValue = (oEmpSalaryItem.BankName != "" ? oEmpSalaryItem.BankName : "-");
                    //this.SetCellValue(returnedValue, 0, 0, Element.ALIGN_CENTER, 1, nHeight);

                }

                this.SetCellValue("", 0, 0, Element.ALIGN_CENTER, 1, 25f);
                _oPdfPTable.CompleteRow();

                //foreach (string key in ColGross)
                //{
                //    _GTgross[key] = Convert.ToDouble(_GTgross[key]) + Convert.ToDouble(gross[key]);
                //}
                //foreach (string key in earnings.Select(x => x.Key).ToList())
                //{
                //    _GTearnings[key] = Convert.ToDouble(_GTearnings[key]) + Convert.ToDouble(earnings[key]);
                //}
                //foreach (string key in deductions.Select(x => x.Key).ToList())
                //{
                //    _GTdeductions[key] = Convert.ToDouble(_GTdeductions[key]) + Convert.ToDouble(deductions[key]);
                //}
                //foreach (string key in increment.Select(x => x.Key).ToList())
                //{
                //    _GTincrement[key] = Convert.ToDouble(_GTincrement[key]) + Convert.ToDouble(increment[key]);
                //}
                //foreach (string key in banks.Select(x => x.Key).ToList())
                //{
                //    _GTbanks[key] = Convert.ToDouble(_GTbanks[key]) + Convert.ToDouble(banks[key]);
                //}

                int nModBy = 5;
                if (_oCompany.BaseAddress == "B007")
                {
                    nModBy = 5;
                }
                if (UnitWiseRowCount % nModBy == 0)
                {
                    if (UnitWiseRowCount != oEmpSalarys.Count)
                    {

                        Summary(ref gross, ref increment, ref earnings, ref deductions, ColGross, ref banks, false);
                        //if (nTotalRowCount > nModBy)
                        //{
                        //    Summary(ref _gross, ref _increment, ref _earnings, ref _deductions, ColGross, ref _banks, true);
                        //}



                        _oDocument.Add(_oPdfPTable);
                        _oDocument.NewPage();
                        _oPdfPTable.DeleteBodyRows();
                        this.PrintHeader(oEmpSalaryItem.BusinessUnitID, flagIndication);
                        _oFontStyle = FontFactory.GetFont("Tahoma", 12f, iTextSharp.text.Font.BOLD);
                        if (_bGroupByDept)
                        {
                            this.SetCellValue("Location Name: " + oEmpSalaryItem.LocationName + ", Department Name: " + oEmpSalaryItem.DepartmentName, 0, _nColumns, Element.ALIGN_LEFT, 0, 30f);
                        }
                        else
                        {
                            this.SetCellValue("Location Name: " + oEmpSalaryItem.LocationName, 0, _nColumns, Element.ALIGN_LEFT, 0, 30f);
                        }
                        _oPdfPTable.CompleteRow();
                        _oFontStyle = FontFactory.GetFont("Tahoma", 9f, iTextSharp.text.Font.NORMAL);
                        this.ColumnSetup();
                    }
                }

            }
            int empCount = oEmpSalarys.Count();
            bool flag = false;
            if (nCount == empCount) flag = true;
            int nCountForSummery = 5;
            if (_oCompany.BaseAddress == "B007")
            {
                nCountForSummery = 5;
            }
            Summary(ref gross, ref increment, ref earnings, ref deductions, ColGross, ref banks, false);
            if (nTotalRowCount > nCountForSummery || flag)
            {
                Summary(ref _gross, ref _increment, ref _earnings, ref _deductions, ColGross, ref _banks, true);
            }
            if (_oEmployeeSalarys.Count == _nGrandEmpCount)
            {
                GTSummary();
            }
            _oDocument.Add(_oPdfPTable);
            _oDocument.NewPage();
            _oPdfPTable.DeleteBodyRows();
        }

        private void ColumnSetup()
        {
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
                else if (sColumn == "Gross Earnings")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_RIGHT);
                else if (sColumn == "Deduction")
                    this.SetColumnProperty(sColumn, 0, ColDeductions.Count(), Element.ALIGN_CENTER);
                else if (sColumn == "Gross Deductions")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_RIGHT);
                else if (sColumn == "Net Amount")
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_RIGHT);
                else if (sColumn == "Bank Information")
                    this.SetColumnProperty(sColumn, 0, ColBankDetail.Count(), Element.ALIGN_CENTER);
                //else if (sColumn == "Bank")
                //    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                //else if (sColumn == "Cash")
                //    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                //else if (sColumn == "Account No")
                //    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                //else if (sColumn == "Bank Name")
                //    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
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

                this.SetColumnProperty(sColName, nSpan, 0, (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.EmployeeCode.ToString()) == sColumn || Global.CapitalSpilitor(EnumSalarySheetFormatProperty.ConfirmationDate.ToString()) == sColumn) ? Element.ALIGN_CENTER : Element.ALIGN_LEFT);
            }
            foreach (string sColumn in ColAttDetail)
            {
                if (sColumn == (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString())) && (_oLeaveHeads.Count > 0))
                {
                    this.SetColumnProperty(sColumn, 0, _oLeaveHeads.Count(), Element.ALIGN_CENTER);
                }
                else
                {
                    this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
                }
            }
            foreach (string sColumn in ColIncrementDetail)
            {
                string sColName = "";
                sColName = sColumn;
                if (sColumn == Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LastGross.ToString())) { sColName = "Before Increment"; }
                this.SetColumnProperty(sColName, nSpan, 0, Element.ALIGN_CENTER);
            }
            foreach (string sColumn in ColEarnings)
            {
                this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
            }
            foreach (string sColumn in ColDeductions)
            {
                this.SetColumnProperty(sColumn, nSpan, 0, Element.ALIGN_CENTER);
            }
            foreach (string sColumn in ColBankDetail)
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
            _oPdfPCell.HorizontalAlignment = align; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY; _oPdfPTable.AddCell(_oPdfPCell);
        }

        private void SetCellValue(string sName, int nRowSpan, int nColumnSpan, int align, int border, float height)
        {
            _oPdfPCell = new PdfPCell(new Phrase(sName, _oFontStyle));
            _oPdfPCell.Rowspan = (nRowSpan > 0) ? nRowSpan : 1;
            _oPdfPCell.Colspan = (nColumnSpan > 0) ? nColumnSpan : 1;
            if (height > 0)
                _oPdfPCell.FixedHeight = height;
            if (border == 0)
                _oPdfPCell.Border = 0;
            _oPdfPCell.HorizontalAlignment = align; _oPdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE; _oPdfPCell.BackgroundColor = BaseColor.WHITE; _oPdfPTable.AddCell(_oPdfPCell);
        }

        private void Summary(ref Dictionary<string, object> gross, ref Dictionary<string, object> increment, ref Dictionary<string, object> earnings, ref Dictionary<string, object> deductions, string[] ColGross, ref Dictionary<string, object> banks, bool bIsGtotal)
        {
            var value = "";
            float nHeight = 26f;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            int nEmpCountSpan = 1 + ColEmpInfo.Count();
            this.SetCellValue((bIsGtotal) ? "Total Employee : " + _nGrandEmpCount : "Total Employee : " + _nEmpCount, 0, nEmpCountSpan, Element.ALIGN_LEFT, 1, nHeight);
            _nEmpCount = 0;
            //int initialSpan = 1 + ColEmpInfo.Count() + ((ColAttDetail.Where(x => x == (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))).Any()) ? ColAttDetail.Count() + _oLeaveHeads.Count() - 1 : ColAttDetail.Count());
            int initialSpan = ((ColAttDetail.Where(x => x == (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))).Any()) ? ColAttDetail.Count() + _oLeaveHeads.Count() - 1 : ColAttDetail.Count());
            this.SetCellValue((bIsGtotal) ? "Grand Total" : "Total", 0, initialSpan, Element.ALIGN_RIGHT, 1, nHeight);

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
                value = (Convert.ToDouble(gross["Gross Earnings"]) > 0) ? this.GetAmountInStr(Convert.ToDouble(gross["Gross Earnings"]), true, _bWithPrecision) : "-";
                this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);
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


            foreach (string key in ColGross)
            {
                _gross[key] = Convert.ToDouble(_gross[key]) + Convert.ToDouble(gross[key]);
                //_GTgross[key] = Convert.ToDouble(_GTgross[key]) + Convert.ToDouble(gross[key]);
                gross[key] = 0;
            }
            foreach (string key in earnings.Select(x => x.Key).ToList())
            {
                _earnings[key] = Convert.ToDouble(_earnings[key]) + Convert.ToDouble(earnings[key]);
                //_GTearnings[key] = Convert.ToDouble(_GTearnings[key]) + Convert.ToDouble(earnings[key]);
                earnings[key] = 0;
            }
            foreach (string key in deductions.Select(x => x.Key).ToList())
            {
                _deductions[key] = Convert.ToDouble(_deductions[key]) + Convert.ToDouble(deductions[key]);
                //_GTdeductions[key] = Convert.ToDouble(_GTdeductions[key]) + Convert.ToDouble(deductions[key]);
                deductions[key] = 0;
            }
            foreach (string key in increment.Select(x => x.Key).ToList())
            {
                _increment[key] = Convert.ToDouble(_increment[key]) + Convert.ToDouble(increment[key]);
                //_GTincrement[key] = Convert.ToDouble(_GTincrement[key]) + Convert.ToDouble(increment[key]);
                increment[key] = 0;
            }
            foreach (string key in banks.Select(x => x.Key).ToList())
            {
                _banks[key] = Convert.ToDouble(_banks[key]) + Convert.ToDouble(banks[key]);
                //_GTbanks[key] = Convert.ToDouble(_GTbanks[key]) + Convert.ToDouble(banks[key]);
                banks[key] = 0;
            }
        }

        private void GTSummary()
        {
            var value = "";
            float nHeight = 26f;
            _oFontStyle = FontFactory.GetFont("Tahoma", 8f, iTextSharp.text.Font.BOLD);
            int nEmpCountSpan = 1 + ColEmpInfo.Count();
            this.SetCellValue("Total Employee : " + _nGrandEmpCount, 0, nEmpCountSpan, Element.ALIGN_LEFT, 1, nHeight);
            _nEmpCount = 0;
            //int initialSpan = 1 + ColEmpInfo.Count() + ((ColAttDetail.Where(x => x == (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))).Any()) ? ColAttDetail.Count() + _oLeaveHeads.Count() - 1 : ColAttDetail.Count());
            int initialSpan = ((ColAttDetail.Where(x => x == (Global.CapitalSpilitor(EnumSalarySheetFormatProperty.LeaveHead.ToString()))).Any()) ? ColAttDetail.Count() + _oLeaveHeads.Count() - 1 : ColAttDetail.Count());
            this.SetCellValue("Grand Total", 0, initialSpan, Element.ALIGN_RIGHT, 1, nHeight);

            foreach (KeyValuePair<string, object> kvp in _GTincrement)
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

            value = (Convert.ToDouble(_GTgross["Gross Salary"]) > 0) ? this.GetAmountInStr(Convert.ToDouble(_GTgross["Gross Salary"]), true, _bWithPrecision) : "-";
            this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);

            foreach (KeyValuePair<string, object> kvp in _GTearnings)
            {
                double nAmount = Convert.ToDouble(kvp.Value);
                this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, nHeight);
            }
            if (_GTearnings.Any())
            {
                value = (Convert.ToDouble(_GTgross["Gross Earnings"]) > 0) ? this.GetAmountInStr(Convert.ToDouble(_GTgross["Gross Earnings"]), true, _bWithPrecision) : "-";
                this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);
            }

            foreach (KeyValuePair<string, object> kvp in _GTdeductions)
            {
                double nAmount = Convert.ToDouble(kvp.Value);
                this.SetCellValue(((nAmount > 0) ? this.GetAmountInStr(nAmount, true, _bWithPrecision) : "-"), 0, 0, Element.ALIGN_CENTER, 1, nHeight);
            }
            if (_GTdeductions.Any())
            {
                value = (Convert.ToDouble(_GTgross["Gross Deductions"]) > 0) ? this.GetAmountInStr(Convert.ToDouble(_GTgross["Gross Deductions"]), true, _bWithPrecision) : "-";
                this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);
            }
            value = (Convert.ToDouble(_GTgross["Net Amount"]) > 0) ? this.GetAmountInStr(Convert.ToDouble(_GTgross["Net Amount"]), true, _bWithPrecision) : "-";
            this.SetCellValue(value, 0, 0, Element.ALIGN_RIGHT, 1, nHeight);

            foreach (KeyValuePair<string, object> kvp in _GTbanks)
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
        }


        private string GetAmountInStr(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (isRound) ? Math.Round(amount, 0) : Math.Round(amount, 2);
            return amount.ToString();
            //return (bWithPrecision) ? Global.MillionFormat(amount).Split('.')[0] : Global.MillionFormat(amount);
        }
        private string GetAmountInStrGrandTotal(double amount, bool bIsround, bool bWithPrecision)
        {
            amount = (isRound) ? Math.Round(amount, 0) : Math.Round(amount, 2);
            return amount.ToString();
            //return (bWithPrecision) ? Global.MillionFormat(amount).Split('.')[0] : Global.MillionFormat(amount);
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

    public class ESimSolFooter_Dachser_Detailformat : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;
        // we will put the final number of pages in a template
        PdfTemplate template;
        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;
        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;
        #region Properties
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        private string _HeaderLeft;
        public string HeaderLeft
        {
            get { return _HeaderLeft; }
            set { _HeaderLeft = value; }
        }
        private string _HeaderRight;
        public string HeaderRight
        {
            get { return _HeaderRight; }
            set { _HeaderRight = value; }
        }
        private iTextSharp.text.Font _HeaderFont;
        public iTextSharp.text.Font HeaderFont
        {
            get { return _HeaderFont; }
            set { _HeaderFont = value; }
        }
        private iTextSharp.text.Font _FooterFont;
        public iTextSharp.text.Font FooterFont
        {
            get { return _FooterFont; }
            set { _FooterFont = value; }
        }
        #endregion
        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            catch (DocumentException de)
            {
            }
            catch (System.IO.IOException ioe)
            {
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {

            base.OnEndPage(writer, document);
            int pageN = writer.PageNumber;
            String text = "Page " + pageN + " of ";
            float len = bf.GetWidthPoint(text, 8);
            iTextSharp.text.Rectangle pageSize = document.PageSize;
            cb.SetRGBColorFill(100, 100, 100);


            cb.BeginText();
            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Checked By", pageSize.GetLeft((125)), pageSize.GetBottom(37), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Manager-Human Resource", pageSize.GetLeft((125)), pageSize.GetBottom(25), 0);
            cb.EndText();


            cb.BeginText();
            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Approved By", pageSize.GetRight(125), pageSize.GetBottom(37), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 11);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Managing Director", pageSize.GetRight(125), pageSize.GetBottom(25), 0);
            cb.EndText();

            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(10));
            cb.ShowText(text);
            cb.EndText();
            cb.AddTemplate(template, pageSize.GetLeft(40) + len, pageSize.GetBottom(10));


        }
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            template.BeginText();
            template.SetFontAndSize(bf, 8);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber - 1));
            template.EndText();
        }
    }

}