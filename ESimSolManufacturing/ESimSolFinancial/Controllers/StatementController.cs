using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Linq;

namespace ESimSolFinancial.Controllers
{
    public class StatementController : Controller
    {
        #region Declaration
        List<Statement> _oStatements = new List<Statement>();
        Statement _oStatement = new Statement();
        List<OperationCategorySetup> _oOperationCategorySetups = new List<OperationCategorySetup>();
        OperationCategorySetup _oOperationCategorySetup = new OperationCategorySetup();
        List<LedgerGroupSetup> _oLedgerGroupSetups = new List<LedgerGroupSetup>();
        LedgerGroupSetup _oLedgerGroupSetup = new LedgerGroupSetup();
        List<LedgerBreakDown> _oLedgerBreakDowns = new List<LedgerBreakDown>();
        LedgerBreakDown _oLedgerBreakDown = new LedgerBreakDown();
        #endregion

        #region Statements
        public ActionResult ViewStatements(int buid, int smid, int dct, string sdt, string edt, int menuid)
        {
            //buid means businessunitid
            //smid means statementsetupid
            //dct means datecomparetype
            //sdt means startdate
            //edt means enddate           

            if (sdt == null || sdt == "") 
            {
                DateTime now = DateTime.Now;
                sdt = (new DateTime(now.Year, now.Month, 1)).ToString("dd MMM yyyy");
            }
            if (edt == null || edt == "") { edt = DateTime.Today.ToString("dd MMM yyyy"); }


            DateTime dStartDate = Convert.ToDateTime(sdt);
            DateTime dEndDate = Convert.ToDateTime(sdt);
            if ((EnumCompareOperator)dct == EnumCompareOperator.Between)
            {
                dEndDate = Convert.ToDateTime(edt);
            }

            Statement oStatement = new Statement();
            oStatement.StatementSetupID = smid;
            oStatement.BUID = buid;
            oStatement.StartDate = dStartDate;
            oStatement.EndDate = dEndDate;
            oStatement.DCO = dct;

            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            double nOpeningBalance = 0;
            double nClosingBalance = 0;
            double nPeriodClosingBalance = 0;
            List<OperationCategorySetup> oOperationCategorySetups = new List<OperationCategorySetup>();
            oOperationCategorySetups = OperationCategorySetup.Gets(oStatement.StatementSetupID, (int)Session[SessionInfo.currentUserID]);

            List<Statement> oStatements = new List<Statement>();            
            _oStatements = Statement.Gets(oStatement.StatementSetupID, oStatement.StartDate, oStatement.EndDate, oStatement.BUID, (int)Session[SessionInfo.currentUserID]);
            int nOperationCategorySetupindex = -1;
            int countLedger = 0;
            foreach (OperationCategorySetup oItem in oOperationCategorySetups)
            {
                double nTotalAmount = 0;
                #region OperationCategorySetup
                _oStatement = new Statement();
                _oStatement.GroupType = 1; // 1 = Operation Group , 2 = Ledger Group , 3 = total
                _oStatement.GroupName = oItem.CategorySetupName;
                _oStatement.BalanceAmountInString = "";
                countLedger = 0;
                nOperationCategorySetupindex++;
                oStatements.Add(_oStatement);
                #endregion

                #region LedgerGroupSetup
                foreach (Statement oItemStatement in _oStatements)
                {
                    nOpeningBalance = oItemStatement.OpeningBalance;
                    if (oItemStatement.IsDr == oItem.IsDebit)
                    {
                        _oStatement = new Statement();
                        _oStatement.GroupType = 2; // 1 = Operation Group , 2 = Ledger Group , 3 = total
                        _oStatement.LedgerGroupSetupID = oItemStatement.OCSID;
                        _oStatement.GroupName = oItemStatement.GroupName;
                        _oStatement.IsDr = oItemStatement.IsDr;
                        _oStatement.BalanceAmount = oItemStatement.BalanceAmount;

                        if (_oStatement.IsDr)
                        {
                            _oStatement.BalanceAmountInString = Global.MillionFormat(oItemStatement.BalanceAmount);
                        }
                        else
                        {
                            _oStatement.BalanceAmountInString = "(" + Global.MillionFormat(oItemStatement.BalanceAmount) + ")";

                        }

                        if (_oStatement.IsDr)
                        {
                            nTotalAmount = nTotalAmount + _oStatement.BalanceAmount;
                            nClosingBalance = nClosingBalance + _oStatement.BalanceAmount;
                            nPeriodClosingBalance = nPeriodClosingBalance + _oStatement.BalanceAmount;
                        }
                        else
                        {
                            nTotalAmount = nTotalAmount - _oStatement.BalanceAmount;
                            nClosingBalance = nClosingBalance - _oStatement.BalanceAmount;
                            nPeriodClosingBalance = nPeriodClosingBalance - _oStatement.BalanceAmount;
                        }
                        oStatements.Add(_oStatement);
                    }


                    #region Total AMount
                    countLedger++;
                    if (countLedger == _oStatements.Count)
                    {
                        _oStatement = new Statement();
                        _oStatement.GroupType = 3; // 1 = Operation Group , 2 = Ledger Group , 3 = total
                        _oStatement.GroupName = "Net " + oOperationCategorySetups[nOperationCategorySetupindex].CategorySetupName;
                        _oStatement.BalanceAmount = nTotalAmount;
                        if (_oStatement.BalanceAmount < 0)
                        {
                            _oStatement.BalanceAmountInString = "(" + Global.MillionFormat(_oStatement.BalanceAmount * (-1)) + ")";
                        }
                        else
                        {
                            _oStatement.BalanceAmountInString = Global.MillionFormat(_oStatement.BalanceAmount);
                        }
                        oStatements.Add(_oStatement);
                    }
                    #endregion
                }
                #endregion
            }

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oStatement.BUID, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0)
            {
                oCompany.Name = oBusinessUnit.Name;
            }

            Statement oTempStatement = new Statement();
            if (nPeriodClosingBalance <= 0)
            {
                oTempStatement.NetIncreaseDecreaseText = "Net Decrease at Cash and Cash Equivalents:";
            }
            else
            {
                oTempStatement.NetIncreaseDecreaseText = "Net Increase at Cash and Cash Equivalents:";
            }
            oTempStatement.StartDate = oStatement.StartDate;
            oTempStatement.EndDate = oStatement.EndDate;
            oTempStatement.Company = oCompany;
            oTempStatement.Statements = oStatements;
            oTempStatement.OpeningBalance = nOpeningBalance;
            oTempStatement.ClosingBalance = nClosingBalance + nOpeningBalance;
            oTempStatement.PeriodClosingBalance = nPeriodClosingBalance;
            oTempStatement.BUID = oStatement.BUID;
            oTempStatement.StatementSetupID = oStatement.StatementSetupID;
            oTempStatement.DCO = oStatement.DCO;
            oTempStatement.StartDate = oStatement.StartDate;
            oTempStatement.EndDate = oStatement.EndDate;

            oTempStatement.StatementSetups = StatementSetup.Gets((int)Session[SessionInfo.currentUserID]);
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(oTempStatement);
        }

        public ActionResult PrintStatement(string param)
        {
            int nStatementSetupID = Convert.ToInt32(param.Split('~')[0]);
            int nDateSearch = Convert.ToInt32(param.Split('~')[1]);
            DateTime dStartDate = Convert.ToDateTime(param.Split('~')[2]);
            DateTime dEndDate = Convert.ToDateTime(param.Split('~')[3]);
            int nBUID = Convert.ToInt32(param.Split('~')[4]);

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion


            double nOpeningBalance = 0;
            double nClosingBalance = 0;
            double nPeriodClosingBalance = 0;
            List<OperationCategorySetup> oOperationCategorySetups = new List<OperationCategorySetup>();
            oOperationCategorySetups = OperationCategorySetup.Gets(nStatementSetupID, (int)Session[SessionInfo.currentUserID]);

            List<Statement> oStatements = new List<Statement>();
            if (nDateSearch == 1)
            {
                dEndDate = dStartDate;
            }
            _oStatements = Statement.Gets(nStatementSetupID, dStartDate, dEndDate, nBUID, (int)Session[SessionInfo.currentUserID]);
            int nOperationCategorySetupindex = -1;
            int countLedger = 0;
            foreach (OperationCategorySetup oItem in oOperationCategorySetups)
            {
                double nTotalAmount = 0;
                #region OperationCategorySetup
                _oStatement = new Statement();
                _oStatement.GroupType = 1; // 1 = Operation Group , 2 = Ledger Group , 3 = total
                _oStatement.GroupName = oItem.CategorySetupName;
                _oStatement.BalanceAmountInString = "";
                countLedger = 0;
                nOperationCategorySetupindex++;
                oStatements.Add(_oStatement);
                #endregion

                #region LedgerGroupSetup
                foreach (Statement oItemStatement in _oStatements)
                {
                    nOpeningBalance = oItemStatement.OpeningBalance;
                    if (oItemStatement.IsDr == oItem.IsDebit)
                    {
                        _oStatement = new Statement();
                        _oStatement.GroupType = 2; // 1 = Operation Group , 2 = Ledger Group , 3 = total
                        _oStatement.GroupName = oItemStatement.GroupName;
                        _oStatement.IsDr = oItemStatement.IsDr;
                        _oStatement.BalanceAmount = oItemStatement.BalanceAmount;

                        if (_oStatement.IsDr)
                        {
                            _oStatement.BalanceAmountInString = Global.MillionFormat(oItemStatement.BalanceAmount);
                        }
                        else
                        {
                            _oStatement.BalanceAmountInString = "(" + Global.MillionFormat(oItemStatement.BalanceAmount) + ")";

                        }

                        if (_oStatement.IsDr)
                        {
                            nTotalAmount = nTotalAmount + _oStatement.BalanceAmount;
                            nClosingBalance = nClosingBalance + _oStatement.BalanceAmount;
                            nPeriodClosingBalance = nPeriodClosingBalance + _oStatement.BalanceAmount;
                        }
                        else
                        {
                            nTotalAmount = nTotalAmount - _oStatement.BalanceAmount;
                            nClosingBalance = nClosingBalance - _oStatement.BalanceAmount;
                            nPeriodClosingBalance = nPeriodClosingBalance - _oStatement.BalanceAmount;
                        }
                        oStatements.Add(_oStatement);
                    }


                    #region Total AMount
                    countLedger++;
                    if (countLedger == _oStatements.Count)
                    {
                        _oStatement = new Statement();
                        _oStatement.GroupType = 3; // 1 = Operation Group , 2 = Ledger Group , 3 = total
                        _oStatement.GroupName = "Net " + oOperationCategorySetups[nOperationCategorySetupindex].CategorySetupName;
                        _oStatement.BalanceAmount = nTotalAmount;
                        if (_oStatement.BalanceAmount < 0)
                        {
                            _oStatement.BalanceAmountInString = "(" + Global.MillionFormat(_oStatement.BalanceAmount * (-1)) + ")";
                        }
                        else
                        {
                            _oStatement.BalanceAmountInString = Global.MillionFormat(_oStatement.BalanceAmount);
                        }
                        oStatements.Add(_oStatement);
                    }
                    #endregion
                }
                #endregion
            }

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0)
            {
                oCompany.Name = oBusinessUnit.Name;
            }

            StatementSetup oStatementSetup = new StatementSetup();
            oStatementSetup = oStatementSetup.Get(nStatementSetupID, (int)Session[SessionInfo.currentUserID]);

            Statement oTempStatement = new Statement();
            if (nPeriodClosingBalance <= 0)
            {
                oTempStatement.NetIncreaseDecreaseText = "Net Decrease at Cash and Cash Equivalents:";
            }
            else
            {
                oTempStatement.NetIncreaseDecreaseText = "Net Increase at Cash and Cash Equivalents:";
            }
            oTempStatement.StartDate = dStartDate;
            oTempStatement.EndDate = dEndDate;
            oTempStatement.Company = oCompany;
            oTempStatement.Statements = oStatements;
            oTempStatement.OpeningBalance = nOpeningBalance;
            oTempStatement.ClosingBalance = nClosingBalance + nOpeningBalance;
            oTempStatement.PeriodClosingBalance = nPeriodClosingBalance;
            oTempStatement.StatementName = oStatementSetup.StatementSetupName;

            rptAccountingStatement oReport = new rptAccountingStatement();
            byte[] abytes = oReport.PrepareReport(oTempStatement, oCompany);
            return File(abytes, "application/pdf");
        }

        public void ExportStatementToExcel(string param)
        {

            #region Dataget
            int nStatementSetupID = Convert.ToInt32(param.Split('~')[0]);
            int nDateSearch = Convert.ToInt32(param.Split('~')[1]);
            DateTime dStartDate = Convert.ToDateTime(param.Split('~')[2]);
            DateTime dEndDate = Convert.ToDateTime(param.Split('~')[3]);
            int nBUID = Convert.ToInt32(param.Split('~')[4]);

            double nOpeningBalance = 0;
            double nClosingBalance = 0;
            double nPeriodClosingBalance = 0;
            List<OperationCategorySetup> oOperationCategorySetups = new List<OperationCategorySetup>();
            oOperationCategorySetups = OperationCategorySetup.Gets(nStatementSetupID, (int)Session[SessionInfo.currentUserID]);

            List<Statement> oStatements = new List<Statement>();
            if (nDateSearch == 1)
            {
                dEndDate = dStartDate;
            }
            _oStatements = Statement.Gets(nStatementSetupID, dStartDate, dEndDate, nBUID, (int)Session[SessionInfo.currentUserID]);
            int nOperationCategorySetupindex = -1;
            int countLedger = 0;
            foreach (OperationCategorySetup oItem in oOperationCategorySetups)
            {
                double nTotalAmount = 0;
                #region OperationCategorySetup
                _oStatement = new Statement();
                _oStatement.GroupType = 1; // 1 = Operation Group , 2 = Ledger Group , 3 = total
                _oStatement.GroupName = oItem.CategorySetupName;
                _oStatement.BalanceAmountInString = "";
                countLedger = 0;
                nOperationCategorySetupindex++;
                oStatements.Add(_oStatement);
                #endregion

                #region LedgerGroupSetup
                foreach (Statement oItemStatement in _oStatements)
                {
                    nOpeningBalance = oItemStatement.OpeningBalance;
                    if (oItemStatement.IsDr == oItem.IsDebit)
                    {
                        _oStatement = new Statement();
                        _oStatement.GroupType = 2; // 1 = Operation Group , 2 = Ledger Group , 3 = total
                        _oStatement.GroupName = oItemStatement.GroupName;
                        _oStatement.IsDr = oItemStatement.IsDr;
                        _oStatement.BalanceAmount = oItemStatement.BalanceAmount;

                        if (_oStatement.IsDr)
                        {
                            _oStatement.BalanceAmountInString = Global.MillionFormat(oItemStatement.BalanceAmount);
                        }
                        else
                        {
                            _oStatement.BalanceAmountInString = "(" + Global.MillionFormat(oItemStatement.BalanceAmount) + ")";

                        }

                        if (_oStatement.IsDr)
                        {
                            nTotalAmount = nTotalAmount + _oStatement.BalanceAmount;
                            nClosingBalance = nClosingBalance + _oStatement.BalanceAmount;
                            nPeriodClosingBalance = nPeriodClosingBalance + _oStatement.BalanceAmount;
                        }
                        else
                        {
                            nTotalAmount = nTotalAmount - _oStatement.BalanceAmount;
                            nClosingBalance = nClosingBalance - _oStatement.BalanceAmount;
                            nPeriodClosingBalance = nPeriodClosingBalance - _oStatement.BalanceAmount;
                        }
                        oStatements.Add(_oStatement);
                    }


                    #region Total AMount
                    countLedger++;
                    if (countLedger == _oStatements.Count)
                    {
                        _oStatement = new Statement();
                        _oStatement.GroupType = 3; // 1 = Operation Group , 2 = Ledger Group , 3 = total
                        _oStatement.GroupName = "Net " + oOperationCategorySetups[nOperationCategorySetupindex].CategorySetupName;
                        _oStatement.BalanceAmount = nTotalAmount;
                        if (_oStatement.BalanceAmount < 0)
                        {
                            _oStatement.BalanceAmountInString = "(" + Global.MillionFormat(_oStatement.BalanceAmount * (-1)) + ")";
                        }
                        else
                        {
                            _oStatement.BalanceAmountInString = Global.MillionFormat(_oStatement.BalanceAmount);
                        }
                        oStatements.Add(_oStatement);
                    }
                    #endregion
                }
                #endregion
            }

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0)
            {
                oCompany.Name = oBusinessUnit.Name;
            }

            StatementSetup oStatementSetup = new StatementSetup();
            oStatementSetup = oStatementSetup.Get(nStatementSetupID, (int)Session[SessionInfo.currentUserID]);

            Statement oTempStatement = new Statement();
            if (nPeriodClosingBalance <= 0)
            {
                oTempStatement.NetIncreaseDecreaseText = "Net Decrease at Cash and Cash Equivalents :";
            }
            else
            {
                oTempStatement.NetIncreaseDecreaseText = "Net Increase at Cash and Cash Equivalents :";
            }
            oTempStatement.StartDate = dStartDate;
            oTempStatement.EndDate = dEndDate;
            oTempStatement.Company = oCompany;
            oTempStatement.Statements = oStatements;
            oTempStatement.OpeningBalance = nOpeningBalance;
            oTempStatement.ClosingBalance = nClosingBalance + nOpeningBalance;
            oTempStatement.PeriodClosingBalance = nPeriodClosingBalance;
            oTempStatement.StatementName = oStatementSetup.StatementSetupName;
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Statement");
                sheet.Name = "Statement";
                sheet.Column(2).Width = 50;
                sheet.Column(3).Width = 20;
                sheet.Column(4).Width = 20;
                nEndCol = 4;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oTempStatement.StatementName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;



                #endregion

                #region statement


                #region Report Data
                #region Date
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "For the period of " + oTempStatement.StartDateInString + " To " + oTempStatement.EndDateInString; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;                
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;                
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Data
                foreach (Statement oItem in oTempStatement.Statements)
                {
                    if (oItem.GroupType == 1)
                    {
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = oItem.GroupName; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    else if (oItem.GroupType == 2)
                    {
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.GroupName; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.BalanceAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    else if (oItem.GroupType == 3)
                    {
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = "       " + oItem.GroupName; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.BalanceAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;

                        cell = sheet.Cells[nRowIndex, 2]; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 3]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                    }


                    nEndRow = nRowIndex;
                    nRowIndex++;

                }
                #endregion

                #region Increase or Decrease
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = oTempStatement.NetIncreaseDecreaseText; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 3]; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = oTempStatement.PeriodClosingBalance; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";              
                border = cell.Style.Border; border.Right.Style = border.Top.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region opening
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Cash and Cash Equivalents at Beginning of Period :"; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[nRowIndex, 3]; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = oTempStatement.OpeningBalnceInstring; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";    
                border = cell.Style.Border; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region closing
                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "Cash and Cash Equivalents at Ending of Period :"; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "Text";
                border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;
                
                cell = sheet.Cells[nRowIndex, 3]; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = oTempStatement.ClosingBalanceInstring; cell.Style.Font.Bold = false;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                border = cell.Style.Border; border.Right.Style= ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Blank
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = border.Left.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                #endregion

                #endregion
                                
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Statement.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Statements Notes
        public ActionResult ViewStatementNotes(int buid, int smid, int dct, string sdt, string edt, int ahid, bool isdr, int menuid)
        {
            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(buid, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            //buid means businessunitid
            //smid means statementsetupid
            //dct means datecomparetype
            //sdt means startdate
            //edt means enddate
            //lgsid means ledger group setup id
            //isdr means IsDebit
            DateTime dStartDate = Convert.ToDateTime(sdt);
            DateTime dEndDate = Convert.ToDateTime(sdt);
            if ((EnumCompareOperator)dct == EnumCompareOperator.Between)
            {
                dEndDate = Convert.ToDateTime(edt);
            }
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oStatement = new Statement();
            _oStatement.BUID = buid;
            _oStatement.StatementSetupID = smid;
            _oStatement.LedgerGroupSetupID = ahid;
            _oStatement.StartDate = dStartDate;
            _oStatement.EndDate = dEndDate;
            _oStatement.DCO = dct;
            _oStatement.IsDebit = isdr;      
            _oStatement.StatementNotes = StatementNote.Gets(smid, dStartDate, dEndDate, buid, ahid, isdr, (int)Session[SessionInfo.currentUserID]);
            _oStatement.StatementSetups = StatementSetup.Gets((int)Session[SessionInfo.currentUserID]);

            StatementSetup oStatementSetup = new StatementSetup();
            oStatementSetup = oStatementSetup.Get(smid, (int)Session[SessionInfo.currentUserID]);

            List<OperationCategorySetup> oOperationCategorySetups = new List<OperationCategorySetup>();
            oOperationCategorySetups = OperationCategorySetup.Gets(smid, (int)Session[SessionInfo.currentUserID]);

            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oChartsOfAccount = oChartsOfAccount.Get(ahid, (int)Session[SessionInfo.currentUserID]);
            if (isdr)
            {
                _oStatement.Title = oStatementSetup.StatementSetupName + " => " + oOperationCategorySetups.Where(x => x.DebitCredit == EnumDebitCredit.Debit).FirstOrDefault().CategorySetupName + " from " + oChartsOfAccount.AccountHeadName + " Notes :";
            }
            else
            {
                _oStatement.Title = oStatementSetup.StatementSetupName + " => " + oOperationCategorySetups.Where(x => x.DebitCredit == EnumDebitCredit.Credit).FirstOrDefault().CategorySetupName + " from " + oChartsOfAccount.AccountHeadName + " Notes :";
            }
            
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(_oStatement);
        }

        public ActionResult PrintStatementNote(int buid, int smid, int dct, string sdt, string edt, int ahid, bool isdr)
        {
            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(buid, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            //buid means businessunitid
            //smid means statementsetupid
            //dct means datecomparetype
            //sdt means startdate
            //edt means enddate
            //ahid means AccountHeadID
            DateTime dStartDate = Convert.ToDateTime(sdt);
            DateTime dEndDate = Convert.ToDateTime(sdt);
            if ((EnumCompareOperator)dct == EnumCompareOperator.Between)
            {
                dEndDate = Convert.ToDateTime(edt);
            }

            Statement oStatement = new Statement();
            oStatement.StatementNotes = StatementNote.Gets(smid, dStartDate, dEndDate, buid, ahid, isdr, (int)Session[SessionInfo.currentUserID]);
            oStatement.StartDate = dStartDate;
            oStatement.EndDate = dEndDate;

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0)
            {
                oCompany.Name = oBusinessUnit.Name;
            }

            StatementSetup oStatementSetup = new StatementSetup();
            oStatementSetup = oStatementSetup.Get(smid, (int)Session[SessionInfo.currentUserID]);

            rptAccountingStatementNotes oReport = new rptAccountingStatementNotes();
            byte[] abytes = oReport.PrepareReport(oStatement, oStatementSetup, oCompany);
            return File(abytes, "application/pdf");
        }

        public void PrintStatementNotexl(int buid, int smid, int dct, string sdt, string edt, int ahid, bool isdr)
        {           
            //buid means businessunitid
            //smid means statementsetupid
            //dct means datecomparetype
            //sdt means startdate
            //edt means enddate
            //ahid means AccountHeadID
            //isdr means IsDebit

            #region Data Gets
            DateTime dStartDate = Convert.ToDateTime(sdt);
            DateTime dEndDate = Convert.ToDateTime(sdt);
            if ((EnumCompareOperator)dct == EnumCompareOperator.Between)
            {
                dEndDate = Convert.ToDateTime(edt);
            }

            Statement oStatement = new Statement();
            oStatement.StatementNotes = StatementNote.Gets(smid, dStartDate, dEndDate, buid, ahid, isdr, (int)Session[SessionInfo.currentUserID]);
            oStatement.StartDate = dStartDate;
            oStatement.EndDate = dEndDate;

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (oBusinessUnit.BusinessUnitID > 0)
            {
                oCompany.Name = oBusinessUnit.Name;
            }

            StatementSetup oStatementSetup = new StatementSetup();
            oStatementSetup = oStatementSetup.Get(smid, (int)Session[SessionInfo.currentUserID]);

            LedgerGroupSetup oLedgerGroupSetup = new LedgerGroupSetup();            
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Statement Note");
                sheet.Name = "Statement Note";
                sheet.Column(2).Width = 8; // SL
                sheet.Column(3).Width = 20; // Voucher No
                sheet.Column(4).Width = 15; //Voucher Date
                sheet.Column(5).Width = 10; //Effected Dr/Cr
                sheet.Column(6).Width = 40; //Effected A/C
                sheet.Column(7).Width = 10; //Particular Dr/Cr
                sheet.Column(8).Width = 40; //Particular A/C
                sheet.Column(9).Width = 18; //Amount
                nEndCol = 9;

                #region Report Header
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oStatementSetup.StatementSetupName+ " Note"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol-3]; cell.Merge = true;
                cell.Value = "Note for : "+ oLedgerGroupSetup.LedgerGroupSetupName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol - 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Date Range : " + oStatement.StartDateInString + " to " + oStatement.EndDateInString; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Column Header
                cell = sheet.Cells[nRowIndex, nStartCol]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol + 1]; cell.Value = "Voucher No"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol + 2]; cell.Value = "Voucher Date"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol + 3]; cell.Value = "Dr/CR"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol + 4]; cell.Value = "Effected A/C"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol + 5]; cell.Value = "Dr/Cr"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol + 6]; cell.Value = "Particular A/C"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nStartCol + 7]; cell.Value = "Amount"; cell.Style.Font.Bold = true; 
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Gray);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;                
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Detail Data 
                int nCount = 0; double nTotalAmount = 0; string sCurrency = "";
                foreach (StatementNote oItem in oStatement.StatementNotes)
                {
                    #region Data Part
                    nCount++;
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex + 1, nStartCol]; cell.Merge = true; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Distributed;

                    cell = sheet.Cells[nRowIndex, nStartCol + 1]; cell.Value = oItem.VoucherNo; cell.Style.Font.Bold = false;                    
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol + 2]; cell.Value = oItem.VoucherDateString; cell.Style.Font.Bold = false;                    
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol + 3]; cell.Value = oItem.CashDebitCredit; cell.Style.Font.Bold = false;                    
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol + 4]; cell.Value = oItem.EffectedAccounts; cell.Style.Font.Bold = false;                    
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol + 5]; cell.Value = oItem.ParticularDebitCredit; cell.Style.Font.Bold = false;                    
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol + 6]; cell.Value = oItem.ParticularAccounts; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sCurrency = oItem.CurrencySymbol;
                    nTotalAmount = nTotalAmount + oItem.Amount;
                    cell = sheet.Cells[nRowIndex, nStartCol + 7, nRowIndex + 1, nStartCol + 7]; cell.Merge = true; cell.Value = oItem.AmountInString; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Distributed;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Voucher Narrations
                    cell = sheet.Cells[nRowIndex, nStartCol + 1, nRowIndex, nStartCol + 6]; cell.Merge = true;  cell.Value = oItem.VoucherNarration; cell.Style.Font.Bold = false;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex = nRowIndex + 1;
                    #endregion
                }
                #endregion

                #region Grand Total                                
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol - 1]; cell.Merge = true; cell.Value = "Grand Total :"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, nEndCol]; cell.Value = sCurrency + " " + Global.MillionFormat(nTotalAmount); cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nRowIndex = nRowIndex + 1;
                #endregion



                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Statement.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion
    }
}
