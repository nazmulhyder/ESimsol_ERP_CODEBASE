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
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class AccountOpenningController : Controller
    {

        #region Declaration
        AccountOpenning _oAccountOpenning = new AccountOpenning();
        List<AccountOpenning> _oAccountOpennings = new List<AccountOpenning>();
        AccountingSession _oAccountingSession = new AccountingSession();
        AccountOpenningBreakdown _oAccountOpenningBreakdown = new AccountOpenningBreakdown();
        List<AccountOpenningBreakdown> _oAccountOpenningBreakdowns = new List<AccountOpenningBreakdown>();
        #endregion

        public ActionResult SetOpeningBalance(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = AccountingSession.GetOpenningAccountingYear((int)Session[SessionInfo.currentUserID]);


            _oAccountOpennings = new List<AccountOpenning>();
            AccountOpenning oAccountOpenning = new AccountOpenning();
            oAccountOpenning.OpenningDate = oAccountingSession.StartDate;
            oAccountOpenning.LstCurrency = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oAccountOpenning.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            oAccountOpenning.CurrencyID = oCompany.BaseCurrencyID;
            oAccountOpenning.BaseCurrencyId = oCompany.BaseCurrencyID;
            oAccountOpenning.BaseCurrencySymbol = oCompany.CurrencySymbol;
            oAccountOpenning.BusinessUnitID = buid;
            return View(oAccountOpenning);
        }

        [HttpPost]
        public JsonResult GetOpenningBalance(AccountOpenning oAccountOpenning)
        {
            _oAccountOpenning = new AccountOpenning();
            _oAccountingSession = new AccountingSession();
            try
            {
                AccountingSession oAccountingSession = new AccountingSession();
                oAccountingSession = AccountingSession.GetOpenningAccountingYear((int)Session[SessionInfo.currentUserID]);

                oAccountOpenning.OpenningDate = oAccountingSession.StartDate;
                _oAccountingSession = _oAccountingSession.GetSessionByDate(oAccountOpenning.OpenningDate, (int)Session[SessionInfo.currentUserID]);
                _oAccountOpenning = _oAccountOpenning.Get(oAccountOpenning.AccountHeadID, _oAccountingSession.AccountingSessionID, oAccountOpenning.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                _oAccountOpenningBreakdowns = AccountOpenningBreakdown.GetsByAccountAndSession(oAccountOpenning.AccountHeadID, _oAccountingSession.AccountingSessionID, oAccountOpenning.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                _oAccountOpenning.AccountingSessionID = _oAccountingSession.AccountingSessionID;
                _oAccountOpenning.AHOBs = GetAHOBs(_oAccountOpenningBreakdowns);
            }
            catch (Exception ex)
            {
                _oAccountOpenning = new AccountOpenning();
                _oAccountOpenning.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(_oAccountOpenning, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SetOpenningBalance(AccountOpenning oAccountOpenning)
        {
            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(oAccountOpenning.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
            {
                _oAccountOpenning = new AccountOpenning();
                _oAccountOpenning.ErrorMessage = "You are not authorize for selected business unit!";

                JavaScriptSerializer oJavaScriptSerializer = new JavaScriptSerializer();
                string sjsonString = oJavaScriptSerializer.Serialize(_oAccountOpenning);
                return Json(sjsonString, JsonRequestBehavior.AllowGet);
            }
            #endregion

            _oAccountOpenning = new AccountOpenning();
            try
            {
                _oAccountOpenning = oAccountOpenning;
                _oAccountOpenning.AccountOpenningBreakdowns = GetAccountOpenningBreakdowns(oAccountOpenning);
                _oAccountOpenning = _oAccountOpenning.SetOpenningBalance((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oAccountOpenning = new AccountOpenning();
                _oAccountOpenning.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAccountOpenning);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<AHOB> GetAHOBs(List<AccountOpenningBreakdown> oAccountOpenningBreakdowns)
        {
            AHOB oAHOB = new AHOB();
            List<AHOB> oAHOBs = new List<AHOB>();
            foreach (AccountOpenningBreakdown oItem in oAccountOpenningBreakdowns)
            {
                if (oItem.CCID <= 0)
                {
                    oAHOB = new AHOB();
                    oAHOB.AHOBID = oItem.AccountOpenningBreakdownID;
                    oAHOB.AHOBT = oItem.BreakdownType;
                    oAHOB.AHOBTInt = (int)oItem.BreakdownType;

                    if (oItem.BreakdownType == EnumBreakdownType.CostCenter)
                    {
                        oAHOB.AHOBTStr = "Subledger";
                    }
                    else if (oItem.BreakdownType == EnumBreakdownType.BillReference)
                    {
                        oAHOB.AHOBTStr = "Bill";
                    }
                    else if (oItem.BreakdownType == EnumBreakdownType.SubledgerBill)
                    {
                        oAHOB.AHOBTStr = "SL Bill";
                    }
                    else
                    {
                        oAHOB.AHOBTStr = "Inventory";
                    }

                    oAHOB.BObjID = oItem.BreakdownObjID;
                    oAHOB.Name = oItem.BreakdownName;
                    oAHOB.Code = "";
                    oAHOB.UnitID = oItem.MUnitID;
                    oAHOB.UName = oItem.UnitName;
                    oAHOB.WUID = oItem.WUnitID;
                    oAHOB.WUName = oItem.WUName;
                    oAHOB.Qty = oItem.Qty;
                    oAHOB.UPrice = oItem.UnitPrice;
                    oAHOB.Amount = oItem.OpenningBalance;
                    oAHOB.DrAmount = oItem.IsDr ? oItem.OpenningBalance : 0.00;
                    oAHOB.CrAmount = !oItem.IsDr ? oItem.OpenningBalance : 0.00; ;

                    if (oItem.CurrencyID == 1)
                    {
                        oAHOB.CFormat = oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.AmountInCurrency);
                    }
                    else
                    {
                        oAHOB.CFormat = oItem.CurrencySymbol + Global.MillionFormat(oItem.AmountInCurrency) + "@" + Global.MillionFormat(oItem.ConversionRate);
                    }

                    oAHOB.CID = oItem.CurrencyID;
                    oAHOB.CRate = oItem.ConversionRate;
                    oAHOB.CAmount = oItem.AmountInCurrency;
                    oAHOB.CSymbol = oItem.CurrencySymbol;
                    oAHOB.DR_CR = oItem.IsDr ? "Debit" : "Credit";
                    oAHOB.CCID = oItem.CCID;
                    oAHOB.CCName = oItem.CCName;
                    oAHOB.CCCode = oItem.CCCode;
                    oAHOB.IsBTAply = oItem.IsBTAply;
                    oAHOBs.Add(oAHOB);

                    if (oItem.BreakdownType == EnumBreakdownType.CostCenter)
                    {
                        List<AHOB> oTempAHOBs = new List<AHOB>();
                        oTempAHOBs = GetSubledgerBreakdown(oItem.BreakdownObjID, oAccountOpenningBreakdowns);
                        foreach (AHOB oTemp in oTempAHOBs)
                        {
                            oAHOBs.Add(oTemp);
                        }
                    }

                }
            }
            return oAHOBs;
        }

        private List<AHOB> GetSubledgerBreakdown(int nCCID, List<AccountOpenningBreakdown> oAccountOpenningBreakdowns)
        {
            List<AHOB> oAHOBs = new List<AHOB>();
            AHOB oAHOB = new AHOB();
            foreach (AccountOpenningBreakdown oItem in oAccountOpenningBreakdowns)
            {
                if (oItem.CCID == nCCID && oItem.BreakdownType == EnumBreakdownType.BillReference)
                {
                    oAHOB = new AHOB();
                    oAHOB.AHOBID = oItem.AccountOpenningBreakdownID;
                    oAHOB.AHOBT = oItem.BreakdownType;
                    oAHOB.AHOBTInt = (int)EnumBreakdownType.SubledgerBill;
                    oAHOB.AHOBTStr = "SL Bill";
                    oAHOB.BObjID = oItem.BreakdownObjID;
                    oAHOB.Name = oItem.BreakdownName;
                    oAHOB.Code = "";
                    oAHOB.UnitID = oItem.MUnitID;
                    oAHOB.UName = oItem.UnitName;
                    oAHOB.WUID = oItem.WUnitID;
                    oAHOB.WUName = oItem.WUName;
                    oAHOB.Qty = oItem.Qty;
                    oAHOB.UPrice = oItem.UnitPrice;
                    oAHOB.Amount = oItem.OpenningBalance;
                    oAHOB.DrAmount = oItem.IsDr ? oItem.OpenningBalance : 0.00;
                    oAHOB.CrAmount = !oItem.IsDr ? oItem.OpenningBalance : 0.00; ;

                    if (oItem.CurrencyID == 1)
                    {
                        oAHOB.CFormat = oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.AmountInCurrency);
                    }
                    else
                    {
                        oAHOB.CFormat = oItem.CurrencySymbol + Global.MillionFormat(oItem.AmountInCurrency) + "@" + Global.MillionFormat(oItem.ConversionRate);
                    }

                    oAHOB.CID = oItem.CurrencyID;
                    oAHOB.CRate = oItem.ConversionRate;
                    oAHOB.CAmount = oItem.AmountInCurrency;
                    oAHOB.CSymbol = oItem.CurrencySymbol;
                    oAHOB.DR_CR = oItem.IsDr ? "Debit" : "Credit";
                    oAHOB.CCID = oItem.CCID;
                    oAHOB.CCName = oItem.CCName;
                    oAHOB.CCCode = oItem.CCCode;
                    oAHOB.IsBTAply = oItem.IsBTAply;
                    oAHOBs.Add(oAHOB);
                }
            }
            return oAHOBs;
        }

        private List<AccountOpenningBreakdown> GetAccountOpenningBreakdowns(AccountOpenning oAccountOpenning)
        {
            List<AHOB> oAHOBs = new List<AHOB>();
            AccountOpenningBreakdown oAccountOpenningBreakdown = new AccountOpenningBreakdown();
            List<AccountOpenningBreakdown> oAccountOpenningBreakdowns = new List<AccountOpenningBreakdown>();
            oAHOBs = oAccountOpenning.AHOBs;
            foreach (AHOB oItem in oAHOBs)
            {
                if (oItem.BObjID > 0)
                {
                    oAccountOpenningBreakdown = new AccountOpenningBreakdown();
                    oAccountOpenningBreakdown.AccountOpenningBreakdownID = oItem.AHOBID;
                    oAccountOpenningBreakdown.AccountingSessionID = oAccountOpenning.AccountingSessionID;
                    oAccountOpenningBreakdown.BusinessUnitID = oAccountOpenning.BusinessUnitID;
                    oAccountOpenningBreakdown.AccountHeadID = oAccountOpenning.AccountHeadID;
                    oAccountOpenningBreakdown.BreakdownObjID = oItem.BObjID;
                    oAccountOpenningBreakdown.IsDr = oItem.DR_CR == "Debit" ? true : false;
                    if ((EnumBreakdownType)oItem.AHOBTInt == EnumBreakdownType.SubledgerBill)
                    {
                        oAccountOpenningBreakdown.BreakdownType = EnumBreakdownType.BillReference;
                    }
                    else
                    {
                        oAccountOpenningBreakdown.BreakdownType = (EnumBreakdownType)oItem.AHOBTInt;
                    }
                    oAccountOpenningBreakdown.MUnitID = oItem.UnitID;
                    oAccountOpenningBreakdown.WUnitID = oItem.WUID;
                    oAccountOpenningBreakdown.UnitPrice = oItem.UPrice;
                    oAccountOpenningBreakdown.Qty = oItem.Qty;
                    oAccountOpenningBreakdown.CurrencyID = oItem.CID;
                    oAccountOpenningBreakdown.ConversionRate = oItem.CRate;
                    oAccountOpenningBreakdown.AmountInCurrency = oItem.CAmount;
                    oAccountOpenningBreakdown.OpenningBalance = oItem.DR_CR == "Debit" ? oItem.DrAmount : oItem.CrAmount;
                    oAccountOpenningBreakdown.CCID = oItem.CCID;
                    oAccountOpenningBreakdowns.Add(oAccountOpenningBreakdown);
                }
            }
            return oAccountOpenningBreakdowns;
        }

        public void ExportToExcel(int id, int buid)
        {
            _oAccountOpenning = new AccountOpenning();
            _oAccountingSession = new AccountingSession();
            AccountOpenning oAccountOpenning = new AccountOpenning();
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();

            oAccountOpenning.AccountHeadID = id;
            oAccountOpenning.BusinessUnitID = buid;
            oAccountOpenning.OpenningDate = Convert.ToDateTime("05 Jul 2015");
            _oAccountingSession = _oAccountingSession.GetSessionByDate(oAccountOpenning.OpenningDate, (int)Session[SessionInfo.currentUserID]);
            _oAccountOpenning = _oAccountOpenning.Get(oAccountOpenning.AccountHeadID, _oAccountingSession.AccountingSessionID, oAccountOpenning.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            _oAccountOpenningBreakdowns = AccountOpenningBreakdown.GetsByAccountAndSession(oAccountOpenning.AccountHeadID, _oAccountingSession.AccountingSessionID, oAccountOpenning.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
            _oAccountOpenning.AccountingSessionID = _oAccountingSession.AccountingSessionID;
            _oAccountOpenning.AHOBs = GetAHOBs(_oAccountOpenningBreakdowns);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            _oAccountOpenning.BaseCurrencyId = oCompany.BaseCurrencyID;
            _oAccountOpenning.BaseCurrencySymbol = oCompany.CurrencySymbol;
            oChartsOfAccount = oChartsOfAccount.Get(id, (int)Session[SessionInfo.currentUserID]);

            #region Fit to Excel
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Openning Balance of " + _oAccountOpenning.AccountHeadName);
                sheet.Name = "Openning Balance of " + _oAccountOpenning.AccountHeadName;
                sheet.Column(2).Width = 10;
                sheet.Column(3).Width = 10;
                sheet.Column(4).Width = 20;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 15;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 20;
                sheet.Column(9).Width = 20;
                sheet.Column(10).Width = 20;
                sheet.Column(11).Width = 20;

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true;
                cell.Value = _oAccountOpenning.BUName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true;
                cell.Value = "Openning Balance of " + _oAccountOpenning.AccountHeadName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;


                string makeString = "";
                if (_oAccountOpenning.CurrencyID == _oAccountOpenning.BaseCurrencyId)
                {
                    makeString = _oAccountOpenning.DR_CR + " " + _oAccountOpenning.CSymbol + " " + Global.MillionFormat(_oAccountOpenning.AmountInCurrency);
                }
                else
                {
                    makeString = _oAccountOpenning.DR_CR + " " + _oAccountOpenning.CSymbol + " " + Global.MillionFormat(_oAccountOpenning.AmountInCurrency) + "@" + Global.MillionFormat(_oAccountOpenning.OpenningBalance);
                }
                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true;
                cell.Value = "Opening Balance : " + makeString; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;
                #endregion


                if (_oAccountOpenning.AHOBs.Count > 0)
                {
                    #region Column Header
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Ref Type"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Dr/Cr"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4, rowIndex, 8]; cell.Merge = true;
                    cell.Value = "Particular"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Amount In Currency"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Debit Amount"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "Credit Amount"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;
                    #endregion

                    foreach (AHOB oItem in _oAccountOpenning.AHOBs)
                    {
                        if ((EnumBreakdownType)oItem.AHOBTInt == EnumBreakdownType.CostCenter)
                        {
                            #region Subledger
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = oItem.AHOBTStr; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.DR_CR; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 4, rowIndex, 8]; cell.Value = oItem.Name; cell.Merge = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.CAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.DrAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.CrAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            rowIndex = rowIndex + 1;
                            #endregion
                        }
                        else if ((EnumBreakdownType)oItem.AHOBTInt == EnumBreakdownType.BillReference || (EnumBreakdownType)oItem.AHOBTInt == EnumBreakdownType.SubledgerBill)
                        {
                            #region Bill & Subledger Bill
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = oItem.AHOBTStr; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.DR_CR; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 4, rowIndex, 7]; cell.Value = oItem.Name; cell.Merge = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.BillDateInString; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.CAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.DrAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.CrAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            rowIndex = rowIndex + 1;
                            #endregion
                        }
                        else if ((EnumBreakdownType)oItem.AHOBTInt == EnumBreakdownType.InventoryReference)
                        {
                            #region Inventory
                            cell = sheet.Cells[rowIndex, 2]; cell.Value = oItem.AHOBTStr; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.DR_CR; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.Name; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.WUName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.UName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.UPrice; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.CAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.DrAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.CrAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.Thin;

                            rowIndex = rowIndex + 1;
                            #endregion
                        }
                    }

                    #region Total Summery
                    double nTotalDebitCC = 0, nTotalCreditCC = 0;
                    double nTotalDebitBT = 0, nTotalCreditBT = 0;
                    double nTotalDebitIR = 0, nTotalCreditIR = 0;


                    string AHOBTStr = "", DrCr = "";
                    double nDrAmount = 0, nCrAmount = 0;
                    foreach (AHOB oItem in _oAccountOpenning.AHOBs)
                    {
                        AHOBTStr = oItem.AHOBTStr;
                        DrCr = oItem.DR_CR;
                        nDrAmount = oItem.DrAmount;
                        nCrAmount = oItem.CrAmount;
                        if (AHOBTStr == "Subledger" && oChartsOfAccount.IsCostCenterApply == true)
                        {
                            if (DrCr == "Debit")
                            {
                                nTotalDebitCC = nDrAmount + nTotalDebitCC;
                            }
                            else
                            {
                                nTotalCreditCC = nCrAmount + nTotalCreditCC;
                            }
                        }
                        else if (AHOBTStr == "Bill" && oChartsOfAccount.IsBillRefApply == true)
                        {
                            if (DrCr == "Debit")
                            {
                                nTotalDebitBT = nDrAmount + nTotalDebitBT;
                            }
                            else
                            {
                                nTotalCreditBT = nCrAmount + nTotalCreditBT;
                            }
                        }
                        else if (AHOBTStr == "Inventory" && oChartsOfAccount.IsInventoryApply == true)
                        {
                            if (DrCr == "Debit")
                            {
                                nTotalDebitIR = nDrAmount + nTotalDebitIR;
                            }
                            else
                            {
                                nTotalCreditIR = nCrAmount + nTotalCreditIR;
                            }
                        }
                    }

                    var nMaxDrAmount = Math.Max(nTotalDebitCC, Math.Max(nTotalDebitBT, nTotalDebitIR));
                    var nMaxCrAmount = Math.Max(nTotalCreditCC, Math.Max(nTotalCreditBT, nTotalCreditIR));

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Value = ""; cell.Merge = true; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.Font.Size = 11; border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin; border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = nMaxDrAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Font.Size = 11; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = nMaxCrAmount; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Font.Size = 11; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; cell.Style.Font.Bold = true;
                    border = cell.Style.Border; border.Left.Style = border.Top.Style = border.Bottom.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex = rowIndex + 1;
                    #endregion
                }

                #region EQUITY AND LIABILITIES PARTS
                //bIsFistSegment = false;
                //bIsImmediateSegment = false;

                //cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true;
                //cell.Value = "EQUITY AND LIABILITIES"; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //rowIndex = rowIndex + 1;

                //i = 0;
                //foreach (BalanceSheet oItem in _oBalanceSheet.LiabilitysOwnerEquitys)
                //{
                //    if (oItem.AccountType == EnumAccountType.Segment)
                //    {
                //        if (bIsFistSegment == true)
                //        {
                //            #region Blank Row
                //            cell = sheet.Cells[rowIndex, 2];
                //            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                //            cell = sheet.Cells[rowIndex, 6];
                //            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                //            rowIndex = rowIndex + 1;
                //            #endregion
                //        }
                //        cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true;
                //        cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                //        border.Left.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.CGSGBalance; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //        cell = sheet.Cells[rowIndex, 6];
                //        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;

                //        rowIndex = rowIndex + 1;
                //        bIsFistSegment = true;
                //        bIsImmediateSegment = true;
                //    }
                //    else if (oItem.AccountType == EnumAccountType.SubGroup)
                //    {
                //        cell = sheet.Cells[rowIndex, 2];
                //        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                //        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.AccountCode; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.CGSGBalance; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //        if (bIsImmediateSegment == true)
                //        {
                //            border.Top.Style = ExcelBorderStyle.Thin;
                //        }
                //        border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                //        if ((i + 1) < _oBalanceSheet.LiabilitysOwnerEquitys.Count)
                //        {
                //            if (_oBalanceSheet.LiabilitysOwnerEquitys[i + 1].AccountType == EnumAccountType.Segment || _oBalanceSheet.LiabilitysOwnerEquitys[i + 1].AccountType == EnumAccountType.Component)
                //            {
                //                border.Bottom.Style = ExcelBorderStyle.Thin;
                //            }
                //        }
                //        else
                //        {
                //            border.Bottom.Style = ExcelBorderStyle.Thin;
                //        }

                //        cell = sheet.Cells[rowIndex, 6];
                //        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;

                //        rowIndex = rowIndex + 1;
                //        bIsImmediateSegment = false;
                //    }
                //    i = i + 1;
                //}

                //#region EQUITY AND LIABILITIES SUMMERY
                //cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true;
                //cell.Value = "TOTAL EQUITY AND LIABILITIES"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                //cell.Style.Font.Size = 14; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                //border.Left.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, 5]; cell.Value = _oBalanceSheet.TotalLiabilitysOwnerEquitys; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //cell.Style.Font.Size = 14; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                //cell = sheet.Cells[rowIndex, 6];
                //border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                //rowIndex = rowIndex + 1;
                //#endregion

                #endregion

                #region Blank Row
                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;
                #endregion

                cell = sheet.Cells[1, 1, rowIndex, 13];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=OpenningBalance.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        public ActionResult ViewSubLedgerWiseOpening(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = AccountingSession.GetOpenningAccountingYear((int)Session[SessionInfo.currentUserID]);

            _oAccountOpennings = new List<AccountOpenning>();
            AccountOpenning oAccountOpenning = new AccountOpenning();
            oAccountOpenning.OpenningDate = oAccountingSession.StartDate;
            oAccountOpenning.LstCurrency = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oAccountOpenning.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            oAccountOpenning.CurrencyID = oCompany.BaseCurrencyID;
            oAccountOpenning.BaseCurrencyId = oCompany.BaseCurrencyID;
            oAccountOpenning.BaseCurrencySymbol = oCompany.CurrencySymbol;
            oAccountOpenning.BusinessUnitID = buid;
            return View(oAccountOpenning);
        }

        [HttpPost]
        public JsonResult GetSubledgerOpenningBalance(AccountOpenningBreakdown oAccountOpenningBreakdown)
        {
            _oAccountOpenning = new AccountOpenning();
            _oAccountingSession = new AccountingSession();
            _oAccountOpenningBreakdown = new AccountOpenningBreakdown();
            try
            {
                AccountingSession oAccountingSession = new AccountingSession();
                oAccountingSession = AccountingSession.GetOpenningAccountingYear((int)Session[SessionInfo.currentUserID]);
                               
                _oAccountingSession = _oAccountingSession.GetSessionByDate(oAccountingSession.StartDate, (int)Session[SessionInfo.currentUserID]);               
                _oAccountOpenningBreakdown = _oAccountOpenningBreakdown.GetsByAccountAndSubledgerAndSession(oAccountOpenningBreakdown.AccountHeadID, _oAccountingSession.AccountingSessionID, oAccountOpenningBreakdown.BusinessUnitID, oAccountOpenningBreakdown.CCID, (int)Session[SessionInfo.currentUserID]);
                _oAccountOpenningBreakdowns = AccountOpenningBreakdown.GetsSubLedgerWiseBills(oAccountOpenningBreakdown.AccountHeadID, _oAccountingSession.AccountingSessionID, oAccountOpenningBreakdown.BusinessUnitID, oAccountOpenningBreakdown.CCID, (int)Session[SessionInfo.currentUserID]);
                _oAccountOpenningBreakdown.AccountingSessionID = _oAccountingSession.AccountingSessionID;
                _oAccountOpenningBreakdown.AHOBs = this.GetSubledgerBreakdown(_oAccountOpenningBreakdown.BreakdownObjID, _oAccountOpenningBreakdowns);
            }
            catch (Exception ex)
            {
                _oAccountOpenningBreakdown = new AccountOpenningBreakdown();
                _oAccountOpenningBreakdown.ErrorMessage = ex.Message;
            }

            var jsonResult = Json(_oAccountOpenningBreakdown, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SetSubLedgerWiseOpenningBalance(AccountOpenningBreakdown oAccountOpenningBreakdown)
        {
            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(oAccountOpenningBreakdown.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
            {
                _oAccountOpenning = new AccountOpenning();
                _oAccountOpenning.ErrorMessage = "You are not authorize for selected business unit!";

                JavaScriptSerializer oJavaScriptSerializer = new JavaScriptSerializer();
                string sjsonString = oJavaScriptSerializer.Serialize(_oAccountOpenning);
                return Json(sjsonString, JsonRequestBehavior.AllowGet);
            }
            #endregion

            _oAccountOpenning = new AccountOpenning();
            List<AHOB> oAHOBs = new List<AHOB>();
            List<AHOB> oTempAHOBs = new List<AHOB>();
            try
            {
                _oAccountingSession = _oAccountingSession.GetSessionByDate(Convert.ToDateTime("05 Jul 2016"), (int)Session[SessionInfo.currentUserID]);
                _oAccountOpenning = _oAccountOpenning.Get(oAccountOpenningBreakdown.AccountHeadID, _oAccountingSession.AccountingSessionID, oAccountOpenningBreakdown.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);
                _oAccountOpenning.BusinessUnitID = oAccountOpenningBreakdown.BusinessUnitID;
                _oAccountOpenning.CurrencyID = oAccountOpenningBreakdown.CurrencyID;
                _oAccountOpenning.AccountHeadID = oAccountOpenningBreakdown.AccountHeadID;
                _oAccountOpenning.AccountingSessionID = _oAccountingSession.AccountingSessionID;
                _oAccountOpenning.CostCenterID = oAccountOpenningBreakdown.BreakdownObjID;
                _oAccountOpenning.ConversionRate = oAccountOpenningBreakdown.ConversionRate;
                _oAccountOpenning.OpenningBalance = oAccountOpenningBreakdown.OpenningBalance;
                _oAccountOpenning.AmountInCurrency = oAccountOpenningBreakdown.AmountInCurrency;
                _oAccountOpenning.IsDebit = oAccountOpenningBreakdown.IsDr;
                if (oAccountOpenningBreakdown.AHOBs != null && oAccountOpenningBreakdown.AHOBs.Count > 0)
                {
                    foreach (AHOB oItem in oAccountOpenningBreakdown.AHOBs)
                    {
                        oItem.CCID = oAccountOpenningBreakdown.BreakdownObjID;
                        oTempAHOBs.Add(oItem);
                    }
                }
                _oAccountOpenning.AHOBs = oTempAHOBs;
                _oAccountOpenning.AccountOpenningBreakdowns = GetAccountOpenningBreakdowns(_oAccountOpenning);
                _oAccountOpenning = _oAccountOpenning.SetOpenningBalanceSubledger((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oAccountOpenning = new AccountOpenning();
                _oAccountOpenning.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAccountOpenning);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
