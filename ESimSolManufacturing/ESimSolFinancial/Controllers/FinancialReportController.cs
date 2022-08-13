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
using System.Threading;


namespace ESimSolFinancial.Controllers
{
    public class FinancialReportController : Controller
    {
        #region Declaration
        //General Ledger
        List<SP_GeneralLedger> _oSP_GeneralLedgers = new List<SP_GeneralLedger>();
        SP_GeneralLedger _oSP_GeneralLedger = new SP_GeneralLedger();
        ChartsOfAccount _oCOA_ChartsOfAccount = new ChartsOfAccount();


        //Trail Balance
        List<TrailBalance> _oTrailBalances = new List<TrailBalance>();
        TrailBalance _oTrailBalance = new TrailBalance();
        TTrailBalance _oTTrailBalance = new TTrailBalance();
        List<TTrailBalance> _oTTrailBalances = new List<TTrailBalance>();

        //Balance Sheet
        List<BalanceSheet> _oBalanceSheets = new List<BalanceSheet>();
        BalanceSheet _oBalanceSheet = new BalanceSheet();
        List<BalanceSheet> _oAssets = new List<BalanceSheet>();
        List<BalanceSheet> _oLiability = new List<BalanceSheet>();
        List<BalanceSheet> _oOwnersEquity = new List<BalanceSheet>();
        List<BalanceSheet> _oLiabilityAndOwnersEquity = new List<BalanceSheet>();
        BalanceSheet _oBS = new BalanceSheet();

        //Income Statement
        List<IncomeStatement> _oIncomeStatements = new List<IncomeStatement>();
        IncomeStatement _oIncomeStatement = new IncomeStatement();
        List<IncomeStatement> _oRevenues = new List<IncomeStatement>();
        List<IncomeStatement> _oExpenses = new List<IncomeStatement>();
        IncomeStatement _oIS = new IncomeStatement();

        List<FinancialReport> _oFinancialReports = new List<FinancialReport>();
        FinancialReport _oFinancialReport = new FinancialReport();

        Company _oCompany = new Company();

        #endregion

        #region Function

        #region Trial Balance
        private List<TTrailBalance> GetTrialBalanceRoot()
        {
            List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
            foreach (TTrailBalance oItem in _oTTrailBalances)
            {
                if (oItem.ParentHeadID == 1)
                {
                    oTTrailBalances.Add(oItem);
                }
            }
            return oTTrailBalances;
        }
        private void AddTrialBalanceTreeNodes(ref TTrailBalance oTTrailBalance)
        {
            List<TTrailBalance> oChildNodes;
            oChildNodes = GetTrialBalanceChild(oTTrailBalance.AccountHeadID);
            oTTrailBalance.children = oChildNodes;

            foreach (TTrailBalance oItem in oChildNodes)
            {
                oTTrailBalance.state = "closed";
                TTrailBalance oTemp = oItem;
                AddTrialBalanceTreeNodes(ref oTemp);
            }
        }
        private List<TTrailBalance> GetTrialBalanceChild(int nAccountHeadID)
        {
            List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
            foreach (TTrailBalance oItem in _oTTrailBalances)
            {
                if (oItem.ParentHeadID == nAccountHeadID)
                {
                    oTTrailBalances.Add(oItem);
                }
            }
            return oTTrailBalances;
        }

        private List<TrailBalance> DeconstructTBTree(List<TTrailBalance> oTTBs)
        {
            List<TrailBalance> oTBs = new List<TrailBalance>();
            foreach (TTrailBalance oItem in oTTBs)
            {
                _oTrailBalance = new TrailBalance();
                _oTrailBalance.AccountHeadID = oItem.AccountHeadID;
                _oTrailBalance.ParentAccountHeadID = oItem.ParentHeadID;
                _oTrailBalance.AccountHeadName = oItem.AccountHeadName;
                _oTrailBalance.AccountCode = oItem.AccountCode;
                _oTrailBalance.OpenningBalance =oItem.OpenningBalanceDbl;
                _oTrailBalance.ClosingBalance = oItem.ClosingBalanceDbl;
                _oTrailBalance.DebitAmount= oItem.DebitAmountDbl;
                _oTrailBalance.CreditAmount= oItem.CreditAmountDbl;
                _oTrailBalance.AccountTypeSt = oItem.AccountTypeInString;
                _oTrailBalance.AccountType = oItem.AccountType;
                oTBs.Add(_oTrailBalance);
                if (oItem.children != null && oItem.children.Count > 0)
                {
                  oTBs.AddRange( DeconstructTBTree(oItem.children));
                }
            }
            return oTBs;
        }
        #endregion

        #region Balance Sheet
        private List<BalanceSheet> GetChildren(int nAccountHeadID)
        {
            List<BalanceSheet> oBalanceSheets = new List<BalanceSheet>();
            foreach (BalanceSheet oItem in _oBalanceSheets)
            {
                if (oItem.ParentAccountHeadID == nAccountHeadID)
                {
                    oBalanceSheets.Add(oItem);
                }
            }
            return oBalanceSheets;
        }
        private void AddBalanceSheetNodes(BalanceSheet oIS)
        {
            List<BalanceSheet> oChildNodes = new List<BalanceSheet>();
            oChildNodes = GetChildren(oIS.AccountHeadID);
            foreach (BalanceSheet oItem in oChildNodes)
            {
                BalanceSheet oTemp = oItem;
                if (oItem.ComponentType == EnumComponentType.Asset)
                {
                    _oAssets.Add(oTemp);
                }
                else if (oItem.ComponentType == EnumComponentType.Liability)
                {
                    _oLiability.Add(oTemp);
                    _oLiabilityAndOwnersEquity.Add(oTemp);
                }
                else if (oItem.ComponentType == EnumComponentType.OwnersEquity)
                {
                    _oOwnersEquity.Add(oTemp);
                    _oLiabilityAndOwnersEquity.Add(oTemp);
                }
                AddBalanceSheetNodes(oTemp);
            }
        }
        #endregion

        #region IncomeStatement
        private List<IncomeStatement> GetIncomeStatementChildren(int nAccountHeadID)
        {
            List<IncomeStatement> oIncomeStatements = new List<IncomeStatement>();
            foreach (IncomeStatement oItem in _oIncomeStatements)
            {
                if (oItem.ParentAccountHeadID == nAccountHeadID)
                {
                    oIncomeStatements.Add(oItem);
                }
            }
            return oIncomeStatements;
        }
        private void AddIncomeStatementNodes(IncomeStatement oIS)
        {
            List<IncomeStatement> oChildNodes = new List<IncomeStatement>();
            oChildNodes = GetIncomeStatementChildren(oIS.AccountHeadID);
            foreach (IncomeStatement oItem in oChildNodes)
            {
                IncomeStatement oTemp = oItem;
                if (oItem.ComponentType == EnumComponentType.Income)
                {
                    _oRevenues.Add(oTemp);
                }
                else if (oItem.ComponentType == EnumComponentType.Expenditure)
                {
                    _oExpenses.Add(oTemp);
                }
                AddIncomeStatementNodes(oTemp);
            }
        }
        #endregion


        #endregion

        #region Actions
        public ActionResult ViewFinancialReport(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.FinancialReportTypes = EnumObject.jGets(typeof(EnumFinancialReportType));
            return View();
        }

        [HttpPost]
        public JsonResult ProcessFinancialReport(FinancialReport oFinancialReport)
        {
            string sjson = "";
            if ((EnumFinancialReportType)oFinancialReport.FinancialReportTypeInt == EnumFinancialReportType.GeneralLedger)
            {
                #region General Ledger
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

                ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
                oChartsOfAccount = oChartsOfAccount.GetByCode(oFinancialReport.StartLedgerCode, (int)Session[SessionInfo.currentUserID]);

                string sSQL = this.GetSQLForGeneralLedger(oFinancialReport, oChartsOfAccount);

                List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
                oSP_GeneralLedgers = SP_GeneralLedger.ProcessGeneralLedger(oChartsOfAccount.AccountHeadID, oFinancialReport.ReportStartDate, oFinancialReport.ReportEndDate, oCompany.BaseCurrencyID, true, sSQL, (int)Session[SessionInfo.currentUserID]);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                sjson = serializer.Serialize(oSP_GeneralLedgers);
                #endregion
            }
            else if ((EnumFinancialReportType)oFinancialReport.FinancialReportTypeInt == EnumFinancialReportType.TrialBalance)
            {
                #region Trail Balance
                string sSQL = this.GetSQLForTrialBalance(oFinancialReport);
                List<TrailBalance> oTrailBalances = new List<TrailBalance>();
                List<TTrailBalance> oTTrailBalances = new List<TTrailBalance>();
                List<TTrailBalance> oTempTTrailBalances = new List<TTrailBalance>();
                oTrailBalances = TrailBalance.ProcessTrialBalance(oFinancialReport.ReportStartDate, oFinancialReport.ReportEndDate, oFinancialReport.StartLedgerCode, oFinancialReport.EndLedgerCode, sSQL, (int)Session[SessionInfo.currentUserID]);

                foreach (TrailBalance oItem in oTrailBalances)
                {
                    _oTTrailBalance = new TTrailBalance();
                    _oTTrailBalance.AccountHeadID = oItem.AccountHeadID;
                    _oTTrailBalance.ParentHeadID = oItem.ParentAccountHeadID;
                    _oTTrailBalance.AccountHeadName = oItem.AccountHeadName;
                    _oTTrailBalance.AccountCode = oItem.AccountCode;
                    _oTTrailBalance.OpenningBalance = oItem.OpeningBalanceSt;
                    _oTTrailBalance.ClosingBalance = oItem.ClosingBalanceSt;
                    _oTTrailBalance.DebitAmount = oItem.DebitAmountInString;
                    _oTTrailBalance.CreditAmount = oItem.CreditAmountInString;
                    _oTTrailBalance.AccountTypeInString = oItem.AccountType.ToString();
                    _oTTrailBalance.OpenningBalanceDbl = oItem.OpenningBalance;
                    _oTTrailBalance.ClosingBalanceDbl = oItem.ClosingBalance;
                    _oTTrailBalance.DebitAmountDbl = oItem.DebitAmount;
                    _oTTrailBalance.CreditAmountDbl = oItem.CreditAmount;
                    _oTTrailBalance.AccountType = oItem.AccountType;
                    _oTTrailBalances.Add(_oTTrailBalance);
                }
                _oTTrailBalance = new TTrailBalance();
                oTTrailBalances = new List<TTrailBalance>();
                oTempTTrailBalances = new List<TTrailBalance>();

                oTempTTrailBalances = GetTrialBalanceRoot();
                foreach (TTrailBalance oItem in oTempTTrailBalances)
                {
                    TTrailBalance oTTrailBalance = new TTrailBalance();
                    oTTrailBalance = oItem;
                    oTTrailBalance.state = "closed";
                    this.AddTrialBalanceTreeNodes(ref  oTTrailBalance);
                    oTTrailBalances.Add(oTTrailBalance);
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                sjson = serializer.Serialize(oTTrailBalances);
                #endregion
            }
            else if ((EnumFinancialReportType)oFinancialReport.FinancialReportTypeInt == EnumFinancialReportType.BalanceSheet)
            {
                #region Balance Sheet
                _oAssets = new List<BalanceSheet>();
                _oLiability = new List<BalanceSheet>();
                _oOwnersEquity = new List<BalanceSheet>();
                _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

                _oBalanceSheet = new BalanceSheet();
                _oBalanceSheets = BalanceSheet.ProcessBalanceSheet(oFinancialReport.ReportStartDate,oFinancialReport.StartBusinessCode,oFinancialReport.EndBusinessCode, ((User)Session[SessionInfo.CurrentUser]).UserID); //4 = Ledger

                BalanceSheet oBS = new BalanceSheet();
                oBS.AccountHeadID = 1;
                this.AddBalanceSheetNodes(oBS);

                _oBalanceSheet.Assets = _oAssets;
                _oBalanceSheet.LiabilitysOwnerEquitys = _oLiability;
                _oBalanceSheet.LiabilitysOwnerEquitys.AddRange(_oOwnersEquity);
                _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
                _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);


                AccountingSession oAccountingSession = new AccountingSession();
                oAccountingSession = oAccountingSession.GetSessionByDate(oFinancialReport.ReportStartDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oBalanceSheet.SessionDate = "Balance Sheet @ " + oFinancialReport.ReportStartDate.ToString("dd MMM yyyy");
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                sjson = serializer.Serialize(_oBalanceSheet);
                #endregion
            }
            else if ((EnumFinancialReportType)oFinancialReport.FinancialReportTypeInt == EnumFinancialReportType.IncomeStatement)
            {
                #region IncomeStatement
                _oRevenues = new List<IncomeStatement>();
                _oExpenses = new List<IncomeStatement>();


                _oIncomeStatement = new IncomeStatement();
                _oIncomeStatements = IncomeStatement.ProcessIncomeStatement(oFinancialReport.ReportStartDate, oFinancialReport.ReportEndDate, ((User)Session[SessionInfo.CurrentUser]).UserID);

                IncomeStatement oIS = new IncomeStatement();
                oIS.AccountHeadID = 1;
                this.AddIncomeStatementNodes(oIS);

                _oIncomeStatement.Revenues = _oRevenues;
                _oIncomeStatement.Expenses = _oExpenses;
                _oIncomeStatement.TotalRevenues = IncomeStatement.ComponentBalance(EnumComponentType.Income, _oRevenues);
                _oIncomeStatement.TotalExpenses = IncomeStatement.ComponentBalance(EnumComponentType.Expenditure, _oExpenses);

                _oIncomeStatement.SessionDate = oFinancialReport.ReportStartDate.ToString("dd MMM yyyy") + " to " + oFinancialReport.ReportEndDate.ToString("dd MMM yyyy");
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                sjson = serializer.Serialize(_oIncomeStatement);
                #endregion
            }
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQLForGeneralLedger(FinancialReport oFinancialReport, ChartsOfAccount oChartsOfAccount)
        {
            string sReturn1 = "SELECT VD.VoucherID, VD.AccountHeadID FROM View_VoucherDetail AS VD ";
            string sReturn = "";

            #region Authorized By
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " ISNULL(VD.AuthorizedBy,0) !=0 ";
            #endregion

            #region Ledger Date
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oFinancialReport.ReportStartDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oFinancialReport.ReportEndDate.ToString("dd MMM yyyy") + "',106)) ";
            #endregion

            #region Ledger Code
            if (oChartsOfAccount.AccountHeadID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VD.AccountHeadID =" + oChartsOfAccount.AccountHeadID.ToString();
            }
            #endregion

            #region Sub Ledger Code
            if (oFinancialReport.StartSubLedgerCode != "0000")
            {
                if (oFinancialReport.EndSubLedgerCode != "0000")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.CCCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartSubLedgerCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndSubLedgerCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.CCCode) = CONVERT(INT, '" + oFinancialReport.StartSubLedgerCode + "') ";
                }
            }
            #endregion

            #region Business Code
            if (oFinancialReport.StartBusinessCode != "00")
            {
                if (oFinancialReport.EndBusinessCode != "00")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.BUCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartBusinessCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndBusinessCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.BUCode) = CONVERT(INT, '" + oFinancialReport.StartBusinessCode + "') ";
                }
            }
            #endregion

            #region Area Code
            if (oFinancialReport.StartAreaCode != "00")
            {
                if (oFinancialReport.EndAreaCode != "00")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.AreaCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartAreaCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndAreaCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.AreaCode) = CONVERT(INT, '" + oFinancialReport.StartAreaCode + "') ";
                }
            }
            #endregion

            #region Zonal Code
            if (oFinancialReport.StartZonalCode != "00")
            {
                if (oFinancialReport.EndZonalCode != "00")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.ZoneCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartZonalCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndZonalCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.ZoneCode) = CONVERT(INT, '" + oFinancialReport.StartZonalCode + "') ";
                }
            }
            #endregion

            #region Site Code
            if (oFinancialReport.StartSiteCode != "0000")
            {
                if (oFinancialReport.EndSiteCode != "0000")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.SiteCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartSiteCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndSiteCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.SiteCode) = CONVERT(INT, '" + oFinancialReport.StartSiteCode + "') ";
                }
            }
            #endregion

            #region Product Code
            if (oFinancialReport.StartProductCode != "00000")
            {
                if (oFinancialReport.EndProductCode != "00000")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.PCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartProductCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndProductCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.PCode) = CONVERT(INT, '" + oFinancialReport.StartProductCode + "') ";
                }
            }
            #endregion

            #region Dept Code
            if (oFinancialReport.StartDeptCode != "00")
            {
                if (oFinancialReport.EndDeptCode != "00")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.DeptCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartDeptCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndDeptCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.DeptCode) = CONVERT(INT, '" + oFinancialReport.StartDeptCode + "') ";
                }
            }
            #endregion

            sReturn = sReturn1 + sReturn + "  GROUP BY VoucherID, AccountHeadID ";
            return sReturn;
        }

        private string GetSQLForTrialBalance(FinancialReport oFinancialReport)
        {
            string sReturn = " ";

            #region Sub Ledger Code
            if (oFinancialReport.StartSubLedgerCode != "0000")
            {
                if (oFinancialReport.EndSubLedgerCode != "0000")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.CCCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartSubLedgerCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndSubLedgerCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.CCCode) = CONVERT(INT, '" + oFinancialReport.StartSubLedgerCode + "') ";
                }
            }
            #endregion

            #region Business Code
            if (oFinancialReport.StartBusinessCode != "00")
            {
                if (oFinancialReport.EndBusinessCode != "00")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.BUCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartBusinessCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndBusinessCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.BUCode) = CONVERT(INT, '" + oFinancialReport.StartBusinessCode + "') ";
                }
            }
            #endregion

            #region Area Code
            if (oFinancialReport.StartAreaCode != "00")
            {
                if (oFinancialReport.EndAreaCode != "00")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.AreaCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartAreaCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndAreaCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.AreaCode) = CONVERT(INT, '" + oFinancialReport.StartAreaCode + "') ";
                }
            }
            #endregion

            #region Zonal Code
            if (oFinancialReport.StartZonalCode != "00")
            {
                if (oFinancialReport.EndZonalCode != "00")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.ZoneCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartZonalCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndZonalCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.ZoneCode) = CONVERT(INT, '" + oFinancialReport.StartZonalCode + "') ";
                }
            }
            #endregion

            #region Site Code
            if (oFinancialReport.StartSiteCode != "0000")
            {
                if (oFinancialReport.EndSiteCode != "0000")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.SiteCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartSiteCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndSiteCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.SiteCode) = CONVERT(INT, '" + oFinancialReport.StartSiteCode + "') ";
                }
            }
            #endregion

            #region Product Code
            if (oFinancialReport.StartProductCode != "00000")
            {
                if (oFinancialReport.EndProductCode != "00000")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.PCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartProductCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndProductCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.PCode) = CONVERT(INT, '" + oFinancialReport.StartProductCode + "') ";
                }
            }
            #endregion

            #region Dept Code
            if (oFinancialReport.StartDeptCode != "00")
            {
                if (oFinancialReport.EndDeptCode != "00")
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.DeptCode) BETWEEN  CONVERT(INT, '" + oFinancialReport.StartDeptCode + "') AND  CONVERT(INT, '" + oFinancialReport.EndDeptCode + "') ";
                }
                else
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(INT, VD.DeptCode) = CONVERT(INT, '" + oFinancialReport.StartDeptCode + "') ";
                }
            }
            #endregion

            return sReturn;
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Print
        #region GenaralLedger
        public ActionResult PrintGeneralLedger(FormCollection DataCollection)
        {
            _oSP_GeneralLedger = new SP_GeneralLedger();
            Currency oCurrency = new Currency();
            _oFinancialReport = new JavaScriptSerializer().Deserialize<FinancialReport>(DataCollection["txtFinancialReport"]);
            _oSP_GeneralLedger.SP_GeneralLedgerList = new JavaScriptSerializer().Deserialize<List<SP_GeneralLedger>>(DataCollection["txtPrintGeneralLedger"]);
            _oCOA_ChartsOfAccount = _oCOA_ChartsOfAccount.GetByCode(_oFinancialReport.StartLedgerCode, ((User)Session[SessionInfo.CurrentUser]).UserID);

            bool bCheckACConfing = false;
            List<ACConfig> oACConfigs = new List<ACConfig>();
            string sSQLACConfig = "SELECT * FROM ACConfig WHERE ConfigureType >= " + (int)EnumConfigureType.GLAccHeadWiseNarration + " AND ConfigureType <= " + (int)EnumConfigureType.GLVC;
            oACConfigs = ACConfig.Gets(sSQLACConfig, ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (ACConfig oItem in oACConfigs)
            {
                if (oItem.ConfigureType != EnumConfigureType.GLVoucherNarration)
                {
                    bool bConfigureValue = (oItem.ConfigureValue == "1" ? true : false);
                    if (bConfigureValue == true)
                    {
                        bCheckACConfing = true;
                        break;
                    }
                }
            }
            double CurrentBalance = _oSP_GeneralLedger.SP_GeneralLedgerList[_oSP_GeneralLedger.SP_GeneralLedgerList.Count - 1].CurrentBalance;
            _oCompany = new Company();
            _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oCompany.CompanyLogo = GetCompanyLogo(_oCompany);
            _oSP_GeneralLedger.Company = _oCompany;
            _oSP_GeneralLedger.Currency = oCurrency.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (bCheckACConfing == true)
            {
                _oSP_GeneralLedger.IsApproved = true;
                _oSP_GeneralLedger.SP_GeneralLedgerList = GetsListAccordingToConfig(_oSP_GeneralLedger, oACConfigs);
            }

            rptGeneralLedgersTwo orptGeneralLedgers = new rptGeneralLedgersTwo();
            byte[] abytes = orptGeneralLedgers.PrepareReport(_oSP_GeneralLedger, _oFinancialReport.ReportStartDate, _oFinancialReport.ReportEndDate, _oCOA_ChartsOfAccount.AccountCode, _oCOA_ChartsOfAccount.AccountHeadName, CurrentBalance, true);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintGeneralLedgerInXL(FormCollection DataCollection)
        {
            _oSP_GeneralLedger = new SP_GeneralLedger();
            Currency oCurrency = new Currency();
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<GeneralLedgerXL>));
            _oFinancialReport = new FinancialReport();

            _oFinancialReport = new JavaScriptSerializer().Deserialize<FinancialReport>(DataCollection["txtXLFinancialReport"]);
            _oSP_GeneralLedger.SP_GeneralLedgerList = new JavaScriptSerializer().Deserialize<List<SP_GeneralLedger>>(DataCollection["txtPrintXLGeneralLedger"]);
            _oCOA_ChartsOfAccount = _oCOA_ChartsOfAccount.GetByCode(_oFinancialReport.StartLedgerCode, ((User)Session[SessionInfo.CurrentUser]).UserID);

            bool bACConfing = false;
            List<ACConfig> oACConfigs = new List<ACConfig>();
            string sSQLACConfig = "SELECT * FROM ACConfig WHERE ConfigureType >= " + (int)EnumConfigureType.GLAccHeadWiseNarration + " AND ConfigureType <= " + (int)EnumConfigureType.GLVC;
            oACConfigs = ACConfig.Gets(sSQLACConfig, ((User)Session[SessionInfo.CurrentUser]).UserID);
            foreach (ACConfig oItem in oACConfigs)
            {
                if (oItem.ConfigureType != EnumConfigureType.GLVoucherNarration)
                {
                    bool bConfigureValue = (oItem.ConfigureValue == "1" ? true : false);
                    if (bConfigureValue == true)
                    {
                        bACConfing = true;
                        break;
                    }
                }
            }


            double CurrentBalance = _oSP_GeneralLedger.SP_GeneralLedgerList[_oSP_GeneralLedger.SP_GeneralLedgerList.Count - 1].CurrentBalance;
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oSP_GeneralLedger.Company = oCompany;
            _oSP_GeneralLedger.Currency = oCurrency.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (bACConfing == true)
            {
                _oSP_GeneralLedger.IsApproved = true;
                _oSP_GeneralLedger.SP_GeneralLedgerList = GetsListAccordingToConfig(_oSP_GeneralLedger, oACConfigs);
            }

            //We load the data

            int nCount = 0; double nTotalDebit = 0; double nTotalCredit = 0;
            GeneralLedgerXL oGeneralLedgerXL = new GeneralLedgerXL();
            List<GeneralLedgerXL> oGeneralLedgerXLs = new List<GeneralLedgerXL>();
            foreach (SP_GeneralLedger oItem in _oSP_GeneralLedger.SP_GeneralLedgerList)
            {
                nCount++;
                oGeneralLedgerXL = new GeneralLedgerXL();
                oGeneralLedgerXL.SLNo = nCount.ToString();
                oGeneralLedgerXL.VoucherDate = oItem.VoucherDateInString;
                oGeneralLedgerXL.VoucherNo = oItem.VoucherNo;
                oGeneralLedgerXL.Particulars = oItem.Particulars;
                oGeneralLedgerXL.DebitAmount = oItem.DebitAmount;
                oGeneralLedgerXL.CreditAmount = oItem.CreditAmount;
                oGeneralLedgerXL.CurrentBalance = oItem.CurrentBalance;
                oGeneralLedgerXLs.Add(oGeneralLedgerXL);
                nTotalDebit = nTotalDebit + oItem.DebitAmount;
                nTotalCredit = nTotalCredit + oItem.CreditAmount;
            }

            #region Total
            oGeneralLedgerXL = new GeneralLedgerXL();
            oGeneralLedgerXL.SLNo = "";
            oGeneralLedgerXL.VoucherDate = "";
            oGeneralLedgerXL.VoucherNo = "";
            oGeneralLedgerXL.Particulars = "Total :";
            oGeneralLedgerXL.DebitAmount = nTotalDebit;
            oGeneralLedgerXL.CreditAmount = nTotalCredit;
            oGeneralLedgerXL.CurrentBalance = 0;
            oGeneralLedgerXLs.Add(oGeneralLedgerXL);
            #endregion

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oGeneralLedgerXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "General Ledger.xls");
        }
        #endregion
        public ActionResult PrintTrialBal_DateRange(FormCollection DataCollection)
        {
            
            _oCompany = new Company();
            _oTrailBalances = new List<TrailBalance>();
            
            try
            {
                _oFinancialReport = new JavaScriptSerializer().Deserialize<FinancialReport>(DataCollection["txtFinancialReportTrailBalance"]);
                _oTTrailBalances = new JavaScriptSerializer().Deserialize<List<TTrailBalance>>(DataCollection["txtPrintTrailBalance"]);
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCompany.CompanyLogo = GetCompanyLogo(_oCompany);
                _oTrailBalances = this.DeconstructTBTree(_oTTrailBalances);
            }
            catch (Exception ex)
            {
                _oTTrailBalance.ErrorMessage = ex.Message;
            }
            rptTrailBalances_DateRange oReport = new rptTrailBalances_DateRange();
            byte[] abytes = null;// oReport.PrepareReport(_oTrailBalances, _oCompany, _oFinancialReport.ReportStartDate.ToString("dd MMM yyyy") + " To " + _oFinancialReport.ReportEndDate.ToString("dd MMM yyyy"));
            return File(abytes, "application/pdf");
        }
        #region BalanceSheet
        public ActionResult PrepareBalanceSheet(FormCollection DataCollection)
        {
            _oBalanceSheet = new BalanceSheet();
            _oFinancialReport = new FinancialReport();
            _oFinancialReport = new JavaScriptSerializer().Deserialize<FinancialReport>(DataCollection["txtFinancialReportBalanceSheet"]);
            _oBalanceSheet = new JavaScriptSerializer().Deserialize<BalanceSheet>(DataCollection["txtPrintBalanceSheet"]);

            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(_oFinancialReport.ReportStartDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oBalanceSheet.SessionDate = "Balance Sheet @ " + _oFinancialReport.ReportStartDate.ToString("dd MMM yyyy");

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oBalanceSheet.Company = oCompany;

            TempData["message"] = "";
            rptBalanceSheet oReport = new rptBalanceSheet();
            byte[] abytes = oReport.PrepareReportShort(_oBalanceSheet,new BusinessUnit());
            return File(abytes, "application/pdf");
        }

        public ActionResult PrepareBalanceSheetInXL(FormCollection DataCollection)
        {
            _oBalanceSheet = new BalanceSheet();
            _oFinancialReport = new FinancialReport();
            _oFinancialReport = new JavaScriptSerializer().Deserialize<FinancialReport>(DataCollection["txtFinancialReportBalanceSheetXL"]);
            _oBalanceSheet = new JavaScriptSerializer().Deserialize<BalanceSheet>(DataCollection["txtPrintBalanceSheetXL"]);

            var stream = new MemoryStream();
            #region Balance Sheet Short

            var serializer = new XmlSerializer(typeof(List<BalanceSheetShortXL>));

            //We load the data           
            BalanceSheetShortXL oBalanceSheetShortXL = new BalanceSheetShortXL();
            List<BalanceSheetShortXL> oBalanceSheetShortXLs = new List<BalanceSheetShortXL>();

            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(_oFinancialReport.ReportStartDate, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Assets
            oBalanceSheetShortXL = new BalanceSheetShortXL();
            oBalanceSheetShortXL.Group = "Assets";
            oBalanceSheetShortXLs.Add(oBalanceSheetShortXL);
            foreach (BalanceSheet oItem in _oBalanceSheet.Assets)
            {
                oBalanceSheetShortXL = new BalanceSheetShortXL();
                if (oItem.AccountType == EnumAccountType.Group)
                {
                    oBalanceSheetShortXL.Group = oItem.AccountHeadName;
                    oBalanceSheetShortXL.SubGroup = "";
                    oBalanceSheetShortXL.Ledger = "";
                    oBalanceSheetShortXL.LedgerBalance = 0.00;
                    oBalanceSheetShortXL.GroupBalance = oItem.CGSGBalance;
                }
                else if (oItem.AccountType == EnumAccountType.SubGroup)
                {
                    oBalanceSheetShortXL.Group = "";
                    oBalanceSheetShortXL.SubGroup = oItem.AccountHeadName;
                    oBalanceSheetShortXL.Ledger = "";
                    oBalanceSheetShortXL.LedgerBalance = oItem.CGSGBalance;
                    oBalanceSheetShortXL.GroupBalance = 0.00;
                }
                else if (oItem.AccountType == EnumAccountType.Ledger)
                {
                    oBalanceSheetShortXL.Group = "";
                    oBalanceSheetShortXL.SubGroup = "";
                    oBalanceSheetShortXL.Ledger = oItem.AccountHeadName;
                    oBalanceSheetShortXL.LedgerBalance = oItem.CGSGBalance;
                    oBalanceSheetShortXL.GroupBalance = 0.00;
                }
                oBalanceSheetShortXLs.Add(oBalanceSheetShortXL);
            }

            #region Total Assets
            oBalanceSheetShortXL = new BalanceSheetShortXL();
            oBalanceSheetShortXL.Group = "";
            oBalanceSheetShortXL.SubGroup = "Total Assets=";
            oBalanceSheetShortXL.LedgerBalance = 0.00;
            oBalanceSheetShortXL.GroupBalance = _oBalanceSheet.TotalAssets;
            oBalanceSheetShortXLs.Add(oBalanceSheetShortXL);
            #endregion

            #endregion

            #region Blank
            oBalanceSheetShortXL = new BalanceSheetShortXL();
            oBalanceSheetShortXL.Group = "";
            oBalanceSheetShortXL.SubGroup = "";
            oBalanceSheetShortXL.LedgerBalance = 0;
            oBalanceSheetShortXL.GroupBalance = 0;
            oBalanceSheetShortXLs.Add(oBalanceSheetShortXL);

            oBalanceSheetShortXL = new BalanceSheetShortXL();
            oBalanceSheetShortXL.Group = "";
            oBalanceSheetShortXL.SubGroup = "";
            oBalanceSheetShortXL.LedgerBalance = 0;
            oBalanceSheetShortXL.GroupBalance = 0;
            oBalanceSheetShortXLs.Add(oBalanceSheetShortXL);
            #endregion

            #region Liabilitys & Owner Equitys
            oBalanceSheetShortXL = new BalanceSheetShortXL();
            oBalanceSheetShortXL.Group = "Liabilitys & Owner Equitys";
            oBalanceSheetShortXLs.Add(oBalanceSheetShortXL);
            foreach (BalanceSheet oItem in _oBalanceSheet.LiabilitysOwnerEquitys)
            {
                oBalanceSheetShortXL = new BalanceSheetShortXL();
                if (oItem.AccountType == EnumAccountType.Group)
                {
                    oBalanceSheetShortXL.Group = oItem.AccountHeadName;
                    oBalanceSheetShortXL.SubGroup = "";
                    oBalanceSheetShortXL.Ledger = "";
                    oBalanceSheetShortXL.LedgerBalance = 0.00;
                    oBalanceSheetShortXL.GroupBalance = oItem.CGSGBalance;
                }
                else if (oItem.AccountType == EnumAccountType.SubGroup)
                {
                    oBalanceSheetShortXL.Group = "";
                    oBalanceSheetShortXL.SubGroup = oItem.AccountHeadName;
                    oBalanceSheetShortXL.Ledger = "";
                    oBalanceSheetShortXL.LedgerBalance = oItem.CGSGBalance;
                    oBalanceSheetShortXL.GroupBalance = 0.00;
                }
                else if (oItem.AccountType == EnumAccountType.Ledger)
                {
                    oBalanceSheetShortXL.Group = "";
                    oBalanceSheetShortXL.SubGroup = "";
                    oBalanceSheetShortXL.Ledger = oItem.AccountHeadName;
                    oBalanceSheetShortXL.LedgerBalance = oItem.CGSGBalance;
                    oBalanceSheetShortXL.GroupBalance = 0.00;
                }

                oBalanceSheetShortXLs.Add(oBalanceSheetShortXL);
            }

            #region Total Liabilitys
            oBalanceSheetShortXL = new BalanceSheetShortXL();
            oBalanceSheetShortXL.Group = "";
            oBalanceSheetShortXL.SubGroup = "Total Liabilitys=";
            oBalanceSheetShortXL.LedgerBalance = 0;
            oBalanceSheetShortXL.GroupBalance = _oBalanceSheet.TotalLiabilitysOwnerEquitys;
            oBalanceSheetShortXLs.Add(oBalanceSheetShortXL);
            #endregion

            #endregion

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oBalanceSheetShortXLs);
            #endregion

            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Balance Sheet.xls");
        }
        #endregion
        #region IncomeStatement
        public ActionResult PrepareIncomeStatement(FormCollection DataCollection)
        {
            _oIncomeStatement = new IncomeStatement();
            _oFinancialReport = new FinancialReport();
            _oFinancialReport = new JavaScriptSerializer().Deserialize<FinancialReport>(DataCollection["txtFinancialReportIncomeStatement"]);
            _oIncomeStatement = new JavaScriptSerializer().Deserialize<IncomeStatement>(DataCollection["txtPrintIncomeStatement"]);

            if (_oIncomeStatement.TotalExpenses < _oIncomeStatement.TotalRevenues)
            {
                _oIncomeStatement.ProfiteLossAmount = " Net Income = " + Global.MillionFormat(_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses) + " BDT";
            }
            else
            {
                _oIncomeStatement.ProfiteLossAmount = " Net Loss = " + Global.MillionFormat(_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues) + " BDT";
            }
            _oIncomeStatement.SessionDate = _oFinancialReport.ReportStartDate.ToString("dd MMM yyyy") + " -to- " + _oFinancialReport.ReportEndDate.ToString("dd MMM yyyy");
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oIncomeStatement.Company = oCompany;

            rptIncomeStatement oReport = new rptIncomeStatement();
            byte[] abytes = oReport.PrepareReportShort(_oIncomeStatement,new BusinessUnit());
            return File(abytes, "application/pdf");
        }

        public ActionResult PrepareIncomeStatementInXL(FormCollection DataCollection)
        {
            _oIncomeStatement = new IncomeStatement();
            _oFinancialReport = new FinancialReport();
            _oFinancialReport = new JavaScriptSerializer().Deserialize<FinancialReport>(DataCollection["txtFinancialReportIncomeStatementXL"]);
            _oIncomeStatement = new JavaScriptSerializer().Deserialize<IncomeStatement>(DataCollection["txtPrintIncomeStatementXL"]);
            var stream = new MemoryStream();
            #region Income Statement Short

            var serializer = new XmlSerializer(typeof(List<IncomeStatementShortXL>));

            //We load the data           
            IncomeStatementShortXL oIncomeStatementShortXL = new IncomeStatementShortXL();
            List<IncomeStatementShortXL> oIncomeStatementShortXLs = new List<IncomeStatementShortXL>();

            #region Revenues
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "Revenues";
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            foreach (IncomeStatement oItem in _oIncomeStatement.Revenues)
            {
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                if (oItem.AccountType == EnumAccountType.Group)
                {
                    oIncomeStatementShortXL.Group = oItem.AccountHeadName;
                    oIncomeStatementShortXL.SubGroup = "";
                    oIncomeStatementShortXL.Ledger = "";
                    oIncomeStatementShortXL.LedgerBalance = 0.00;
                    oIncomeStatementShortXL.GroupBalance = oItem.CGSGBalance;
                }
                else if (oItem.AccountType == EnumAccountType.SubGroup)
                {
                    oIncomeStatementShortXL.Group = "";
                    oIncomeStatementShortXL.SubGroup = oItem.AccountHeadName;
                    oIncomeStatementShortXL.Ledger = "";
                    oIncomeStatementShortXL.LedgerBalance = oItem.CGSGBalance;
                    oIncomeStatementShortXL.GroupBalance = 0.00;
                }
                else if (oItem.AccountType == EnumAccountType.Ledger)
                {
                    oIncomeStatementShortXL.Group = "";
                    oIncomeStatementShortXL.SubGroup = "";
                    oIncomeStatementShortXL.Ledger = oItem.AccountHeadName;
                    oIncomeStatementShortXL.LedgerBalance = oItem.CGSGBalance;
                    oIncomeStatementShortXL.GroupBalance = 0.00;
                }
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            }

            #region Total Revenues
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "";
            oIncomeStatementShortXL.SubGroup = "Total Revenues=";
            oIncomeStatementShortXL.LedgerBalance = 0.00;
            oIncomeStatementShortXL.GroupBalance = _oIncomeStatement.TotalRevenues;
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            #endregion

            #endregion

            #region Blank
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "";
            oIncomeStatementShortXL.SubGroup = "";
            oIncomeStatementShortXL.LedgerBalance = 0;
            oIncomeStatementShortXL.GroupBalance = 0;
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);

            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "";
            oIncomeStatementShortXL.SubGroup = "";
            oIncomeStatementShortXL.LedgerBalance = 0;
            oIncomeStatementShortXL.GroupBalance = 0;
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            #endregion

            #region Expenses
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "Expenses ";
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            foreach (IncomeStatement oItem in _oIncomeStatement.Expenses)
            {
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                if (oItem.AccountType == EnumAccountType.Group)
                {
                    oIncomeStatementShortXL.Group = oItem.AccountHeadName;
                    oIncomeStatementShortXL.SubGroup = "";
                    oIncomeStatementShortXL.Ledger = "";
                    oIncomeStatementShortXL.LedgerBalance = 0.00;
                    oIncomeStatementShortXL.GroupBalance = oItem.CGSGBalance;
                }
                else if (oItem.AccountType == EnumAccountType.SubGroup)
                {
                    oIncomeStatementShortXL.Group = "";
                    oIncomeStatementShortXL.SubGroup = oItem.AccountHeadName;
                    oIncomeStatementShortXL.Ledger = "";
                    oIncomeStatementShortXL.LedgerBalance = oItem.CGSGBalance;
                    oIncomeStatementShortXL.GroupBalance = 0.00;
                }
                else if (oItem.AccountType == EnumAccountType.Ledger)
                {
                    oIncomeStatementShortXL.Group = "";
                    oIncomeStatementShortXL.SubGroup = "";
                    oIncomeStatementShortXL.Ledger = oItem.AccountHeadName;
                    oIncomeStatementShortXL.LedgerBalance = oItem.CGSGBalance;
                    oIncomeStatementShortXL.GroupBalance = 0.00;
                }
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            }

            #region Total Expenses
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "";
            oIncomeStatementShortXL.SubGroup = "Total Expenses =";
            oIncomeStatementShortXL.LedgerBalance = 0;
            oIncomeStatementShortXL.GroupBalance = _oIncomeStatement.TotalExpenses;
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            #endregion

            #endregion

            #region Blank
            oIncomeStatementShortXL = new IncomeStatementShortXL();
            oIncomeStatementShortXL.Group = "";
            oIncomeStatementShortXL.SubGroup = "";
            oIncomeStatementShortXL.LedgerBalance = 0;
            oIncomeStatementShortXL.GroupBalance = 0;
            oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
            #endregion

            #region Net Income or Loss
            if (_oIncomeStatement.TotalExpenses < _oIncomeStatement.TotalRevenues)
            {
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = " Net Income =";
                oIncomeStatementShortXL.SubGroup = (_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses).ToString();
                oIncomeStatementShortXL.LedgerBalance = 0.00;
                oIncomeStatementShortXL.GroupBalance = 0;
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                //_oIncomeStatement.ProfiteLossAmount = " Net Income = " + Global.MillionFormat(_oIncomeStatement.TotalRevenues - _oIncomeStatement.TotalExpenses) + " BDT";
            }
            else
            {
                oIncomeStatementShortXL = new IncomeStatementShortXL();
                oIncomeStatementShortXL.Group = "  Net Loss =";
                oIncomeStatementShortXL.SubGroup = (_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues).ToString();
                oIncomeStatementShortXL.LedgerBalance = 0.00;
                oIncomeStatementShortXL.GroupBalance = 0;
                oIncomeStatementShortXLs.Add(oIncomeStatementShortXL);
                // _oIncomeStatement.ProfiteLossAmount = " Net Loss = " + Global.MillionFormat(_oIncomeStatement.TotalExpenses - _oIncomeStatement.TotalRevenues) + " BDT";
            }
            #endregion

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oIncomeStatementShortXLs);
            #endregion

            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Income Statement.xls");

        }
        #endregion
        #endregion

        #region Get Data According to Configuration
        private List<SP_GeneralLedger> GetsListAccordingToConfig(SP_GeneralLedger oSP_GeneralLedger, List<ACConfig> oACConfigs)
        {
            int nCompanyID = (int)Session[SessionInfo.CurrentCompanyID];
            List<SP_GeneralLedger> oNewSP_GeneralLedgers = new List<SP_GeneralLedger>();
            SP_GeneralLedger oNewSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();

            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<CostCenterTransaction> oCostCenterTransactions = new List<CostCenterTransaction>();
            List<VoucherBillTransaction> oVoucherBillTransactions = new List<VoucherBillTransaction>();
            List<VPTransaction> oVPTransactions = new List<VPTransaction>();
            List<VoucherReference> oVoucherReferences = new List<VoucherReference>();
            string sSQL = "";
            if (oSP_GeneralLedger.IsApproved == true)
            {
                sSQL = "SELECT * FROM View_CostCenterTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oCostCenterTransactions = CostCenterTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVoucherBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VPTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVPTransactions = VPTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherReference AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND VD.VoucherID IN (SELECT V.VoucherID FROM Voucher AS V WHERE ISNULL(V.AuthorizedBy,0) != 0) AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVoucherReferences = VoucherReference.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                sSQL = "SELECT * FROM View_CostCenterTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oCostCenterTransactions = CostCenterTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherBillTransaction AS TT WHERE  TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVoucherBillTransactions = VoucherBillTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VPTransaction AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVPTransactions = VPTransaction.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "SELECT * FROM View_VoucherReference AS TT WHERE TT.VoucherDetailID IN (SELECT VD.VoucherDetailID FROM View_VoucherDetail AS VD WHERE VD.AccountHeadID=" + oSP_GeneralLedger.AccountHeadID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),VD.VoucherDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oSP_GeneralLedger.EndDate.ToString("dd MMM yyyy") + "',106)))";
                oVoucherReferences = VoucherReference.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }


            foreach (SP_GeneralLedger oSPGL in oSP_GeneralLedger.SP_GeneralLedgerList)
            {
                oNewSP_GeneralLedgers.Add(oSPGL);
                foreach (ACConfig oItem in oACConfigs)
                {
                    bool bConfigureValue = (oItem.ConfigureValue == "1" ? true : false);
                    #region Account Head Narration
                    if (oItem.ConfigureType == EnumConfigureType.GLAccHeadWiseNarration && bConfigureValue == true)
                    {
                        oNewSP_GeneralLedger = GetNarration(oItem, oSPGL);
                        oNewSP_GeneralLedger.VoucherDate = oSPGL.VoucherDate;
                        if (oNewSP_GeneralLedger.IsNullOrNot == false)
                        {
                            oNewSP_GeneralLedgers.Add(oNewSP_GeneralLedger);
                        }
                    }
                    #endregion
                    #region Voucher Narration
                    else if (oItem.ConfigureType == EnumConfigureType.GLVoucherNarration && bConfigureValue == true)
                    {
                        oNewSP_GeneralLedger = GetNarration(oItem, oSPGL);
                        oNewSP_GeneralLedger.VoucherDate = oSPGL.VoucherDate;
                        if (oNewSP_GeneralLedger.IsNullOrNot == false)
                        {
                            oNewSP_GeneralLedgers.Add(oNewSP_GeneralLedger);
                        }
                    }
                    #endregion
                    #region Cost Center Transaction
                    else if (oItem.ConfigureType == EnumConfigureType.GLCC && bConfigureValue == true)
                    {
                        if (oCostCenterTransactions.Count > 0)
                        {
                            oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                            oTempSP_GeneralLedgers = GetCC(oCostCenterTransactions, oSPGL.VoucherDetailID, oCompany, oSPGL.VoucherDate);
                            oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                        }
                    }
                    #endregion
                    #region Voucher Bill Transaction
                    else if (oItem.ConfigureType == EnumConfigureType.GLBT && bConfigureValue == true)
                    {
                        if (oVoucherBillTransactions.Count > 0)
                        {
                            oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                            oTempSP_GeneralLedgers = GetBT(oVoucherBillTransactions, oSPGL.VoucherDetailID, oCompany, oSPGL.VoucherDate);
                            oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                        }
                    }
                    #endregion
                    #region VP (IR) Transaction
                    else if (oItem.ConfigureType == EnumConfigureType.GLIR && bConfigureValue == true)
                    {
                        if (oVPTransactions.Count > 0)
                        {
                            oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                            oTempSP_GeneralLedgers = GetIR(oVPTransactions, oSPGL.VoucherDetailID, oCompany, oSPGL.VoucherDate);
                            oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                        }
                    }
                    #endregion
                    #region Voucher Reference
                    else if (oItem.ConfigureType == EnumConfigureType.GLVC && bConfigureValue == true)
                    {
                        if (oVoucherReferences.Count > 0)
                        {
                            oTempSP_GeneralLedgers = new List<SP_GeneralLedger>();
                            oTempSP_GeneralLedgers = GetVR(oVoucherReferences, oSPGL.VoucherDetailID, oCompany, oSPGL.VoucherDate);
                            oNewSP_GeneralLedgers.AddRange(oTempSP_GeneralLedgers);
                        }
                    }
                    #endregion
                }
            }
            return oNewSP_GeneralLedgers;
        }
        private List<SP_GeneralLedger> GetCC(List<CostCenterTransaction> oCostCenterTransactions, int nVoucherDetailID, Company oCompany, DateTime dVoucherDate)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            foreach (CostCenterTransaction oItem in oCostCenterTransactions)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID)
                {
                    string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                    oSP_GeneralLedger = new SP_GeneralLedger();
                    oSP_GeneralLedger.ConfigTitle = "CC ";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLCC;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = " Name : " + oItem.CostCenterName + " Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount) + bIsDr;
                    oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                }
            }
            return oSP_GeneralLedgers;
        }
        private List<SP_GeneralLedger> GetBT(List<VoucherBillTransaction> oVoucherBillTransactions, int nVoucherDetailID, Company oCompany, DateTime dVoucherDate)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            foreach (VoucherBillTransaction oItem in oVoucherBillTransactions)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID)
                {
                    string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                    oSP_GeneralLedger = new SP_GeneralLedger();
                    oSP_GeneralLedger.ConfigTitle = "BT ";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLBT;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = " Bill No : " + oItem.BillNo + "Bill Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.BillAmount) + " Amount : " + oItem.CurrencySymbol + " " + Global.MillionFormat(oItem.Amount) + bIsDr;
                    oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                }
            }
            return oSP_GeneralLedgers;
        }
        private List<SP_GeneralLedger> GetIR(List<VPTransaction> oVPTransactions, int nVoucherDetailID, Company oCompany, DateTime dVoucherDate)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            foreach (VPTransaction oItem in oVPTransactions)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID)
                {
                    string bIsDr = (oItem.IsDr == true ? " Dr " : " Cr ");
                    oSP_GeneralLedger = new SP_GeneralLedger();
                    oSP_GeneralLedger.ConfigTitle = "IR ";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLIR;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = " Product Name :" + oItem.ProductName + " WU : " + oItem.WorkingUnitName + " MUnit : " + oItem.MUnitName + " Qty : " + Global.MillionFormat(oItem.Qty) + " Unit Price : " + Global.MillionFormat(oItem.UnitPrice) + " Amount : " + Global.MillionFormat(oItem.Amount) + bIsDr;
                    oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                }
            }
            return oSP_GeneralLedgers;
        }
        private List<SP_GeneralLedger> GetVR(List<VoucherReference> oVoucherReferences, int nVoucherDetailID, Company oCompany, DateTime dVoucherDate)
        {
            SP_GeneralLedger oSP_GeneralLedger = new SP_GeneralLedger();
            List<SP_GeneralLedger> oSP_GeneralLedgers = new List<SP_GeneralLedger>();
            foreach (VoucherReference oItem in oVoucherReferences)
            {
                if (oItem.VoucherDetailID == nVoucherDetailID)
                {
                    oSP_GeneralLedger = new SP_GeneralLedger();
                    oSP_GeneralLedger.ConfigTitle = "VR ";
                    oSP_GeneralLedger.ConfigType = EnumConfigureType.GLVC;
                    oSP_GeneralLedger.VoucherDate = dVoucherDate;
                    oSP_GeneralLedger.VoucherNo = " Description :" + oItem.Description + " Amount : " + Global.MillionFormat(oItem.Amount);
                    oSP_GeneralLedgers.Add(oSP_GeneralLedger);
                }
            }
            return oSP_GeneralLedgers;
        }
        private SP_GeneralLedger GetNarration(ACConfig oACConfig, SP_GeneralLedger oSP_GeneralLedger)
        {
            SP_GeneralLedger oNewSP_GeneralLedger = new SP_GeneralLedger();
            if (oACConfig.ConfigureType == EnumConfigureType.GLAccHeadWiseNarration)
            {
                oNewSP_GeneralLedger.ConfigTitle = " Head Wise Narration ";
                oNewSP_GeneralLedger.VoucherNo = oSP_GeneralLedger.Narration;
                if (oNewSP_GeneralLedger.VoucherNo == "")
                {
                    oNewSP_GeneralLedger.IsNullOrNot = true;
                }
            }
            else if (oACConfig.ConfigureType == EnumConfigureType.GLVoucherNarration)
            {
                oNewSP_GeneralLedger.ConfigTitle = " Voucher Narration ";
                oNewSP_GeneralLedger.VoucherNo = oSP_GeneralLedger.VoucherNarration;
                if (oNewSP_GeneralLedger.VoucherNo == "")
                {
                    oNewSP_GeneralLedger.IsNullOrNot = true;
                }
            }
            return oNewSP_GeneralLedger;
        }
        #endregion
    }
}