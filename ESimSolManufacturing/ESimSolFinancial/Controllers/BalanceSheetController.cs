using System;
using System.Linq;
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
    public class BalanceSheetController : Controller
    {
        #region Declaration
        List<BalanceSheet> _oBalanceSheets = new List<BalanceSheet>();
        List<BalanceSheet> _oBalanceSheetTemps = new List<BalanceSheet>();
        BalanceSheet _oBalanceSheet = new BalanceSheet();
        List<BalanceSheet> _oAssets = new List<BalanceSheet>();
        List<BalanceSheet> _oLiability = new List<BalanceSheet>();
        List<BalanceSheet> _oOwnersEquity = new List<BalanceSheet>();
        List<BalanceSheet> _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

        List<BalanceSheet> _oAssetsTemp = new List<BalanceSheet>();
        List<BalanceSheet> _oLiabilityTemp = new List<BalanceSheet>();
        List<BalanceSheet> _oOwnersEquityTemp = new List<BalanceSheet>();
        List<BalanceSheet> _oLiabilityAndOwnersEquityTemp = new List<BalanceSheet>();

        BalanceSheet _oBS = new BalanceSheet();
        TChartsOfAccount _oTChartsOfAccount = new TChartsOfAccount();
        List<TChartsOfAccount> _oTChartsOfAccounts = new List<TChartsOfAccount>();
        FinancialPositionSetup _oFinancialPositionSetup = new FinancialPositionSetup();
        List<FinancialPositionSetup> _oFinancialPositionSetups = new List<FinancialPositionSetup>();
        #endregion
        #region Function
        private TChartsOfAccount GetRoot(int nParentID)
        {
            TChartsOfAccount oTChartsOfAccount = new TChartsOfAccount();
            foreach (TChartsOfAccount oItem in _oTChartsOfAccounts)
            {
                if (oItem.parentid == nParentID)
                {
                    return oItem;
                }
            }
            return _oTChartsOfAccount;
        }
        private TChartsOfAccount GetRootByID(int nID)
        {
            TChartsOfAccount oTChartsOfAccount = new TChartsOfAccount();
            foreach (TChartsOfAccount oItem in _oTChartsOfAccounts)
            {
                if (oItem.id == nID)
                {
                    return oItem;
                }
            }
            return _oTChartsOfAccount;
        }
        private void AddTreeNodes(ref TChartsOfAccount oTChartsOfAccount)
        {
            List<TChartsOfAccount> oChildNodes;
            oChildNodes = GetChild(oTChartsOfAccount.id);
            oTChartsOfAccount.children = oChildNodes;

            foreach (TChartsOfAccount oItem in oChildNodes)
            {
                TChartsOfAccount oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        private List<TChartsOfAccount> GetChild(int nAccountHeadID)
        {
            List<TChartsOfAccount> oTChartsOfAccounts = new List<TChartsOfAccount>();
            foreach (TChartsOfAccount oItem in _oTChartsOfAccounts)
            {
                if (oItem.parentid == nAccountHeadID)
                {
                    oTChartsOfAccounts.Add(oItem);
                }
            }
            return oTChartsOfAccounts;
        }
        #endregion

        public ActionResult RptBalanceSheet(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oAssets = new List<BalanceSheet>();
            _oLiability = new List<BalanceSheet>();
            _oOwnersEquity = new List<BalanceSheet>();
            _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);

            _oBalanceSheet = new BalanceSheet();
            _oBalanceSheets = BalanceSheet.Gets(0, (int)EnumAccountType.Ledger, oAccountingSession.StartDate, DateTime.Today, 0, false, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
            _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();

            BalanceSheet oBS = new BalanceSheet();
            oBS.AccountHeadID = 1;
            this.AddNodes(oBS);

            #region Asset Tree
            foreach (BalanceSheet oItem in _oAssets)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentAccountHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.CGSGBalanceInString = oItem.CGSGBalanceInString;
                _oTChartsOfAccount.LadgerBalanceInString = oItem.LedgerBalanceInString;
                //if (oItem.AccountType == EnumAccountType.Group || oItem.AccountType == EnumAccountType.SubGroup)
                //{
                //    _oTChartsOfAccount.state = "closed";
                //}
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRoot(1);
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oBalanceSheet.TAsset = _oTChartsOfAccount;
            #endregion

            #region Liability Tree
            _oTChartsOfAccounts = new List<TChartsOfAccount>();

            foreach (BalanceSheet oItem in _oLiabilityAndOwnersEquity)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentAccountHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.CGSGBalanceInString = oItem.CGSGBalanceInString;
                _oTChartsOfAccount.LadgerBalanceInString = oItem.LedgerBalanceInString;
                //if (oItem.AccountType == EnumAccountType.Group || oItem.AccountType == EnumAccountType.SubGroup)
                //{
                //    _oTChartsOfAccount.state = "closed";
                //}
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }

            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRootByID(4);//Owners Equity
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oBalanceSheet.TLiablityAndOwnersEquitys.AddRange(_oTChartsOfAccount.children);

            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRootByID(3);//Libility
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oBalanceSheet.TLiablityAndOwnersEquitys.AddRange(_oTChartsOfAccount.children);
            #endregion
            _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
            _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);

            oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(DateTime.Today, (int)Session[SessionInfo.currentUserID]);
            _oBalanceSheet.SessionDate = "Balance Sheet @ " + DateTime.Today.ToString("dd MMM yyyy");
            _oBalanceSheet.AccountTypeObjs = EnumObject.jGets(typeof(EnumAccountType));

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            return View(_oBalanceSheet);
        }

        #region New Version
        #region Set BalaceSheet SessionData
        [HttpPost]
        public ActionResult SetBSSessionData(BalanceSheet oBalanceSheet)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oBalanceSheet);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Financial Position
        public ActionResult ViewFinancialPosition(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oAssets = new List<BalanceSheet>();
            _oLiability = new List<BalanceSheet>();
            _oOwnersEquity = new List<BalanceSheet>();
            _oLiabilityAndOwnersEquity = new List<BalanceSheet>();
            _oBalanceSheet = new BalanceSheet();
            AccountingSession oAccountingSession = new AccountingSession();

            try
            {
                _oBalanceSheet = (BalanceSheet)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                _oBalanceSheet = null;
            }
            oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);

            if (_oBalanceSheet != null)
            {                
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(_oBalanceSheet.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                if (_oBalanceSheet.IsForCurrentDate)
                {                    
                    _oBalanceSheet.TempDate = oAccountingSession.EndDate;
                }
                _oBalanceSheet.AccountType = EnumAccountType.SubGroup != _oBalanceSheet.AccountType ? EnumAccountType.SubGroup : _oBalanceSheet.AccountType;

                _oBalanceSheet.TempDate = Convert.ToDateTime(_oBalanceSheet.Param.Split('~')[0]);
                _oBalanceSheet.StartDate = Convert.ToDateTime(_oBalanceSheet.Param.Split('~')[1]);
                _oBalanceSheets = BalanceSheet.Gets(_oBalanceSheet.BUID, (int)_oBalanceSheet.AccountType, _oBalanceSheet.StartDate, _oBalanceSheet.TempDate, _oBalanceSheet.ParentAccountHeadID, _oBalanceSheet.IsApproved, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
                _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();
       
                if (_oBalanceSheet.nSearcingCriteria == 1)//Multiple Date
                {
                    _oBalanceSheet.StartDate = Convert.ToDateTime(_oBalanceSheet.Param.Split('~')[1]);
                    DateTime dTempDate = Convert.ToDateTime(_oBalanceSheet.Param.Split('~')[2]);//for Second Date
                    _oBalanceSheetTemps = BalanceSheet.Gets(_oBalanceSheet.BUID, (int)_oBalanceSheet.AccountType, _oBalanceSheet.StartDate, dTempDate, _oBalanceSheet.ParentAccountHeadID, _oBalanceSheet.IsApproved, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
                    _oBalanceSheetTemps = _oBalanceSheetTemps.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();
                    foreach (BalanceSheet oTempItem in _oBalanceSheetTemps)
                    {
                        if (_oBalanceSheets.Where(x => x.AccountHeadID == oTempItem.AccountHeadID).Count() <= 0)
                        {
                            BalanceSheet oNewBalanceSheet = new BalanceSheet();
                            oNewBalanceSheet = oTempItem;
                            oNewBalanceSheet.CGSGBalanceTemp = oTempItem.CGSGBalance;
                            oNewBalanceSheet.ClosingBalance = 0; 
                            oNewBalanceSheet.TotalLiabilitysOwnerEquitysTemp = oTempItem.TotalLiabilitysOwnerEquitys ;
                            oNewBalanceSheet.TotalLiabilitysOwnerEquitys = 0;
                            oNewBalanceSheet.TotalAssetsTemp = oTempItem.TotalAssets;
                            oNewBalanceSheet.TotalAssets = 0;
                            _oBalanceSheets.Add(oNewBalanceSheet);
                        }
                    }
                    foreach (BalanceSheet oItem in _oBalanceSheets)
                    {
                        BalanceSheet oTempBalanceSheet = new BalanceSheet();
                        oTempBalanceSheet = _oBalanceSheetTemps.Where(x => x.AccountHeadID == oItem.AccountHeadID).FirstOrDefault();
                        oItem.CGSGBalanceTemp = oTempBalanceSheet == null ? 0 : oTempBalanceSheet.CGSGBalance;
                        oItem.TotalLiabilitysOwnerEquitysTemp = oTempBalanceSheet == null ? 0 : oTempBalanceSheet.TotalLiabilitysOwnerEquitys;
                        oItem.TotalAssetsTemp = oTempBalanceSheet == null ? 0 : oTempBalanceSheet.TotalAssets;
                    }
                }
                _oBalanceSheet.BalanceSheets = new List<BalanceSheet>();
                _oBalanceSheet.BalanceSheets = _oBalanceSheets;
            }
            else
            {
                _oBalanceSheet = new BalanceSheet();
                _oBalanceSheet.BalanceSheets = new List<BalanceSheet>();
                _oBalanceSheet.AccountType = EnumAccountType.SubGroup;
                _oBalanceSheet.StartDate = oAccountingSession.StartDate;
                _oBalanceSheets = BalanceSheet.Gets(1, (int)EnumAccountType.SubGroup, _oBalanceSheet.StartDate, DateTime.Today, 0, false, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
                _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();
                _oBalanceSheet.BalanceSheets = _oBalanceSheets;
            }
            BalanceSheet oBS = new BalanceSheet();
            oBS.AccountHeadID = 1;
            this.AddNodes(oBS);
            _oBalanceSheet.Assets = _oAssets;
            _oBalanceSheet.LiabilitysOwnerEquitys = _oOwnersEquity;
            _oBalanceSheet.LiabilitysOwnerEquitys.AddRange(_oLiability);
            _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
            _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);
            _oBalanceSheet.TotalAssetsTemp = BalanceSheet.ComponentBalanceTemp(EnumComponentType.Asset, _oAssetsTemp);
            _oBalanceSheet.TotalLiabilitysOwnerEquitysTemp = BalanceSheet.ComponentBalanceTemp(EnumComponentType.Liability, _oLiabilityAndOwnersEquityTemp);

            oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(DateTime.Today, (int)Session[SessionInfo.currentUserID]);
            _oBalanceSheet.SessionDate = "Balance Sheet @ " + DateTime.Today.ToString("dd MMM yyyy");
            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUs = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null; //GetCompanyLogo(oCompany);
            _oBalanceSheet.Company = oCompany;
            //_oBalanceSheet.TempDate = DateTime.Today;
            ViewBag.MonthEnum = EnumObject.jGets(typeof(EnumMonthName));
            int yearNow = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            List<int> YearList = new List<int>();
            for (int i = yearNow - 10; i <= yearNow + 15; i++)
            {
                YearList.Add(i);
            }
            ViewBag.ListOfYear = YearList;
            ViewBag.AccountingSessions = AccountingSession.Gets((int)Session[SessionInfo.currentUserID]);
            this.Session.Remove(SessionInfo.ParamObj);
            return View(_oBalanceSheet);
        }

        public ActionResult PrintFinancialPosition(string sParameter)
        {
            _oAssets = new List<BalanceSheet>();
            _oLiability = new List<BalanceSheet>();
            _oOwnersEquity = new List<BalanceSheet>();
            _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

            int nBUID = sParameter.Split('~')[0] == null ? 0 : sParameter.Split('~')[0] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[0]);
            DateTime dBalanceSheetDate = sParameter.Split('~')[1] == null ? DateTime.Now : sParameter.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[1]);
            DateTime dBalanceSheetDateTemp = sParameter.Split('~')[2] == null ? DateTime.Now : sParameter.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[2]);
            bool bIsApproved = sParameter.Split('~')[3] == null ? false : sParameter.Split('~')[3] == "" ? false :  Convert.ToBoolean(sParameter.Split('~')[3]);
            int nSearcingCriteria = sParameter.Split('~')[4] == null ? 0 : sParameter.Split('~')[4] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[4]);
            DateTime dBalanceSheetStartDate = sParameter.Split('~')[5] == null ? DateTime.Now : sParameter.Split('~')[5] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[5]);


            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion


            _oBalanceSheet = new BalanceSheet();
            _oBalanceSheets = BalanceSheet.Gets(nBUID, (int)EnumAccountType.SubGroup, dBalanceSheetStartDate, dBalanceSheetDate, 0, bIsApproved, (int)Session[SessionInfo.currentUserID]);
            _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();

            if (nSearcingCriteria == 1)//Multiple Date
            {

                _oBalanceSheetTemps = BalanceSheet.Gets(nBUID, (int)EnumAccountType.SubGroup, dBalanceSheetStartDate, dBalanceSheetDateTemp, 0, bIsApproved, (int)Session[SessionInfo.currentUserID]);
                _oBalanceSheetTemps = _oBalanceSheetTemps.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();
                foreach (BalanceSheet oTempItem in _oBalanceSheetTemps)
                {
                    if (_oBalanceSheets.Where(x => x.AccountHeadID == oTempItem.AccountHeadID).Count() <= 0)
                    {
                        BalanceSheet oNewBalanceSheet = new BalanceSheet();
                        oNewBalanceSheet = oTempItem;
                        oNewBalanceSheet.CGSGBalanceTemp = oTempItem.CGSGBalance;
                        oNewBalanceSheet.ClosingBalance = 0;
                        oNewBalanceSheet.TotalLiabilitysOwnerEquitysTemp = oTempItem.TotalLiabilitysOwnerEquitys;
                        oNewBalanceSheet.TotalLiabilitysOwnerEquitys = 0;
                        oNewBalanceSheet.TotalAssetsTemp = oTempItem.TotalAssets;
                        oNewBalanceSheet.TotalAssets = 0;
                        _oBalanceSheets.Add(oNewBalanceSheet);
                    }
                }
                foreach (BalanceSheet oItem in _oBalanceSheets)
                {
                    BalanceSheet oTempBalanceSheet = new BalanceSheet();
                    oTempBalanceSheet = _oBalanceSheetTemps.Where(x => x.AccountHeadID == oItem.AccountHeadID).FirstOrDefault();
                    oItem.CGSGBalanceTemp = oTempBalanceSheet == null ? 0 : oTempBalanceSheet.CGSGBalance;
                    oItem.TotalLiabilitysOwnerEquitysTemp = oTempBalanceSheet == null ? 0 : oTempBalanceSheet.TotalLiabilitysOwnerEquitys;
                    oItem.TotalAssetsTemp = oTempBalanceSheet == null ? 0 : oTempBalanceSheet.TotalAssets;
                }
            }


            BalanceSheet oBS = new BalanceSheet();
            oBS.AccountHeadID = 1;
            this.AddNodes(oBS);

            _oBalanceSheet.Assets = _oAssets;
            _oBalanceSheet.LiabilitysOwnerEquitys = _oOwnersEquity;
            _oBalanceSheet.LiabilitysOwnerEquitys.AddRange(_oLiability);
            _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
            _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);
            _oBalanceSheet.TotalAssetsTemp = BalanceSheet.ComponentBalanceTemp(EnumComponentType.Asset, _oAssetsTemp);
            _oBalanceSheet.TotalLiabilitysOwnerEquitysTemp = BalanceSheet.ComponentBalanceTemp(EnumComponentType.Liability, _oLiabilityAndOwnersEquityTemp);

            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(dBalanceSheetDate, (int)Session[SessionInfo.currentUserID]);
            _oBalanceSheet.TempDate = dBalanceSheetDate;
            //for Temporary DAte
            _oBalanceSheet.nSearcingCriteria = nSearcingCriteria;
            _oBalanceSheet.Param = "~"+dBalanceSheetDateTemp;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null; //GetCompanyLogo(oCompany);
            _oBalanceSheet.Company = oCompany;

            TempData["message"] = "";
            byte[] abytes = null;
            if (_oBalanceSheet.nSearcingCriteria == 0)
            {
                rptFinancialPosition oReport = new rptFinancialPosition();
                abytes = oReport.PrepareReport(_oBalanceSheet, oBusinessUnit);
            }
            else //for Multiple date
            {
                rptFinancialPositionMultipleDate oReport = new rptFinancialPositionMultipleDate();
                abytes = oReport.PrepareReport(_oBalanceSheet, oBusinessUnit);
            }
            return File(abytes, "application/pdf");
        }
        public void FinancialPositionExpotToExcel(string sParameter)
        {
            _oAssets = new List<BalanceSheet>();
            _oLiability = new List<BalanceSheet>();
            _oOwnersEquity = new List<BalanceSheet>();
            _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

            int nBUID = sParameter.Split('~')[0] == null ? 0 : sParameter.Split('~')[0] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[0]);
            DateTime dBalanceSheetDate = sParameter.Split('~')[1] == null ? DateTime.Now : sParameter.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[1]);
            DateTime dBalanceSheetDateTemp = sParameter.Split('~')[2] == null ? DateTime.Now : sParameter.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[2]);
            bool bIsApproved = sParameter.Split('~')[3] == null ? false : sParameter.Split('~')[3] == "" ? false : Convert.ToBoolean(sParameter.Split('~')[3]);
            int nSearcingCriteria = sParameter.Split('~')[4] == null ? 0 : sParameter.Split('~')[4] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[4]);
            DateTime dBalanceSheetStartDate = sParameter.Split('~')[5] == null ? DateTime.Now : sParameter.Split('~')[5] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[5]);

            _oBalanceSheet = new BalanceSheet();
            _oBalanceSheets = BalanceSheet.Gets(nBUID, (int)EnumAccountType.SubGroup, dBalanceSheetStartDate, dBalanceSheetDate, 0, bIsApproved, (int)Session[SessionInfo.currentUserID]);
            _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();

            if (nSearcingCriteria == 1)//Multiple Date
            {
                _oBalanceSheetTemps = BalanceSheet.Gets(nBUID, (int)EnumAccountType.SubGroup, dBalanceSheetStartDate, dBalanceSheetDateTemp, 0, bIsApproved, (int)Session[SessionInfo.currentUserID]);
                _oBalanceSheetTemps = _oBalanceSheetTemps.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();
                foreach (BalanceSheet oTempItem in _oBalanceSheetTemps)
                {
                    if (_oBalanceSheets.Where(x => x.AccountHeadID == oTempItem.AccountHeadID).Count() <= 0)
                    {
                        BalanceSheet oNewBalanceSheet = new BalanceSheet();
                        oNewBalanceSheet = oTempItem;
                        oNewBalanceSheet.CGSGBalanceTemp = oTempItem.CGSGBalance;
                        oNewBalanceSheet.ClosingBalance = 0;
                        oNewBalanceSheet.TotalLiabilitysOwnerEquitysTemp = oTempItem.TotalLiabilitysOwnerEquitys;
                        oNewBalanceSheet.TotalLiabilitysOwnerEquitys = 0;
                        oNewBalanceSheet.TotalAssetsTemp = oTempItem.TotalAssets;
                        oNewBalanceSheet.TotalAssets = 0;
                        _oBalanceSheets.Add(oNewBalanceSheet);
                    }
                }
                foreach (BalanceSheet oItem in _oBalanceSheets)
                {
                    BalanceSheet oTempBalanceSheet = new BalanceSheet();
                    oTempBalanceSheet = _oBalanceSheetTemps.Where(x => x.AccountHeadID == oItem.AccountHeadID).FirstOrDefault();
                    oItem.CGSGBalanceTemp = oTempBalanceSheet == null ? 0 : oTempBalanceSheet.CGSGBalance;
                    oItem.TotalLiabilitysOwnerEquitysTemp = oTempBalanceSheet == null ? 0 : oTempBalanceSheet.TotalLiabilitysOwnerEquitys;
                    oItem.TotalAssetsTemp = oTempBalanceSheet == null ? 0 : oTempBalanceSheet.TotalAssets;
                }
            }

            BalanceSheet oBS = new BalanceSheet();
            oBS.AccountHeadID = 1;
            this.AddNodes(oBS);

            _oBalanceSheet.Assets = _oAssets;
            _oBalanceSheet.LiabilitysOwnerEquitys = _oOwnersEquity;
            _oBalanceSheet.LiabilitysOwnerEquitys.AddRange(_oLiability);


            _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
            _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);
            _oBalanceSheet.TotalAssetsTemp = BalanceSheet.ComponentBalanceTemp(EnumComponentType.Asset, _oAssetsTemp);
            _oBalanceSheet.TotalLiabilitysOwnerEquitysTemp = BalanceSheet.ComponentBalanceTemp(EnumComponentType.Liability, _oLiabilityAndOwnersEquityTemp);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null; //GetCompanyLogo(oCompany);

            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany.Name = oBusinessUnit.Name;
            }
            _oBalanceSheet.Company = oCompany;
            _oBalanceSheet.TempDate = dBalanceSheetDate;
            //for Temporary DAte
            _oBalanceSheet.nSearcingCriteria = nSearcingCriteria;
            _oBalanceSheet.Param = "~" + dBalanceSheetDateTemp;

            #region Fit to Excel
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            bool bIsFistSegment = false;
            bool bIsImmediateSegment = false;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Statement of Financial Position");
                sheet.Name = "Statement of Financial Position";
                sheet.Column(2).Width = 3;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 3;
                if(nSearcingCriteria==1)//for Monthly
                {
                    sheet.Column(7).Width = 20;
                    sheet.Column(8).Width = 3;
                }
                int nLastColIndex = nSearcingCriteria==1?8:6;
                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, nLastColIndex]; cell.Merge = true;
                cell.Value = _oBalanceSheet.Company.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nLastColIndex]; cell.Merge = true;
                cell.Value = "Statement of Financial Position"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nLastColIndex]; cell.Merge = true;
                cell.Value = nSearcingCriteria == 0 ? "As at " + _oBalanceSheet.BalanceSheetDate : "As at " + _oBalanceSheet.BalanceSheetDate + " And " + _oBalanceSheet.BalanceSheetDateTemp; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2];
                border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 4]; cell.Value = "Notes"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = _oBalanceSheet.BalanceSheetDate + "-" + _oBalanceSheet.Company.CurrencyName;
                cell.Style.Font.Bold = true; cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 6];
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;

                if (nSearcingCriteria == 1)//for Monthly
                {

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = _oBalanceSheet.BalanceSheetDateTemp + "-" + _oBalanceSheet.Company.CurrencyName;
                    cell.Style.Font.Bold = true; cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8];
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;

                }

                rowIndex = rowIndex + 1;
                #endregion

                #region Asstes Part
                cell = sheet.Cells[rowIndex, 2, rowIndex, nLastColIndex]; cell.Merge = true;
                cell.Value = "ASSETS"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                int i = 0;
                foreach (BalanceSheet oItem in _oBalanceSheet.Assets)
                {
                    if (oItem.AccountType == EnumAccountType.Segment)
                    {
                        if (bIsFistSegment == true)
                        {
                            #region Blank Row
                            cell = sheet.Cells[rowIndex, 2];
                            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, nLastColIndex];
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex = rowIndex + 1;
                            #endregion
                        }
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true;
                        cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.CGSGBalance; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[rowIndex, 6];
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        if (nSearcingCriteria == 1)//for Monthly
                        {
                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.CGSGBalanceTemp; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[rowIndex, 8];
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        
                        rowIndex = rowIndex + 1;
                        bIsFistSegment = true;
                        bIsImmediateSegment = true;
                    }
                    else if (oItem.AccountType == EnumAccountType.SubGroup)
                    {
                        cell = sheet.Cells[rowIndex, 2];
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;


                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.AccountCode; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.CGSGBalance; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        if (bIsImmediateSegment == true)
                        {
                            border.Top.Style = ExcelBorderStyle.Thin;
                        }
                        border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        if ((i + 1) < _oBalanceSheet.Assets.Count)
                        {
                            if (_oBalanceSheet.Assets[i + 1].AccountType == EnumAccountType.Segment)
                            {
                                border.Bottom.Style = ExcelBorderStyle.Thin;
                            }
                        }
                        else
                        {
                            border.Bottom.Style = ExcelBorderStyle.Thin;
                        }

                        cell = sheet.Cells[rowIndex, 6];
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        if (nSearcingCriteria == 1)//for Monthly
                        {

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.CGSGBalanceTemp; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                            if (bIsImmediateSegment == true)
                            {
                                border.Top.Style = ExcelBorderStyle.Thin;
                            }
                            border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if ((i + 1) < _oBalanceSheet.Assets.Count)
                            {
                                if (_oBalanceSheet.Assets[i + 1].AccountType == EnumAccountType.Segment)
                                {
                                    border.Bottom.Style = ExcelBorderStyle.Thin;
                                }
                            }
                            else
                            {
                                border.Bottom.Style = ExcelBorderStyle.Thin;
                            }

                            cell = sheet.Cells[rowIndex, 8];
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;

                        }

                        rowIndex = rowIndex + 1;
                        bIsImmediateSegment = false;
                    }
                    i = i + 1;
                }

                #region Asset Summery
                cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true;
                cell.Value = "TOTAL ASSETS"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Font.Size = 14; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = _oBalanceSheet.TotalAssets; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Font.Size = 14; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, 6];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                if (nSearcingCriteria == 1)//for Monthly
                {

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = _oBalanceSheet.TotalAssetsTemp; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Font.Size = 14; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, 8];
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                }
                rowIndex = rowIndex + 1;

                #region Blank Row
                cell = sheet.Cells[rowIndex, 2];
                border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, nLastColIndex];
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;
                #endregion


                #endregion

                #endregion

                #region EQUITY AND LIABILITIES PARTS
                bIsFistSegment = false;
                bIsImmediateSegment = false;

                cell = sheet.Cells[rowIndex, 2, rowIndex, nLastColIndex]; cell.Merge = true;
                cell.Value = "EQUITY AND LIABILITIES"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 14; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                i = 0;
                foreach (BalanceSheet oItem in _oBalanceSheet.LiabilitysOwnerEquitys)
                {
                    if (oItem.AccountType == EnumAccountType.Segment)
                    {
                        if (bIsFistSegment == true)
                        {
                            #region Blank Row
                            cell = sheet.Cells[rowIndex, 2];
                            border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, nLastColIndex];
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex = rowIndex + 1;
                            #endregion
                        }
                        cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true;
                        cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                        border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.CGSGBalance; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[rowIndex, 6];
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        if (nSearcingCriteria == 1)//for Monthly
                        {

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.CGSGBalanceTemp; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            cell = sheet.Cells[rowIndex, 8];
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        rowIndex = rowIndex + 1;
                        bIsFistSegment = true;
                        bIsImmediateSegment = true;
                    }
                    else if (oItem.AccountType == EnumAccountType.SubGroup)
                    {
                        cell = sheet.Cells[rowIndex, 2];
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.AccountCode; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.CGSGBalance; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        if (bIsImmediateSegment == true)
                        {
                            border.Top.Style = ExcelBorderStyle.Thin;
                        }
                        border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        if ((i + 1) < _oBalanceSheet.LiabilitysOwnerEquitys.Count)
                        {
                            if (_oBalanceSheet.LiabilitysOwnerEquitys[i + 1].AccountType == EnumAccountType.Segment || _oBalanceSheet.LiabilitysOwnerEquitys[i + 1].AccountType == EnumAccountType.Component)
                            {
                                border.Bottom.Style = ExcelBorderStyle.Thin;
                            }
                        }
                        else
                        {
                            border.Bottom.Style = ExcelBorderStyle.Thin;
                        }

                        cell = sheet.Cells[rowIndex, 6];
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                        if (nSearcingCriteria == 1)//for Monthly
                        {

                            cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.CGSGBalanceTemp; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            if (bIsImmediateSegment == true)
                            {
                                border.Top.Style = ExcelBorderStyle.Thin;
                            }
                            border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if ((i + 1) < _oBalanceSheet.LiabilitysOwnerEquitys.Count)
                            {
                                if (_oBalanceSheet.LiabilitysOwnerEquitys[i + 1].AccountType == EnumAccountType.Segment || _oBalanceSheet.LiabilitysOwnerEquitys[i + 1].AccountType == EnumAccountType.Component)
                                {
                                    border.Bottom.Style = ExcelBorderStyle.Thin;
                                }
                            }
                            else
                            {
                                border.Bottom.Style = ExcelBorderStyle.Thin;
                            }

                            cell = sheet.Cells[rowIndex, 8];
                            border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        rowIndex = rowIndex + 1;
                        bIsImmediateSegment = false;
                    }
                    i = i + 1;
                }

                #region EQUITY AND LIABILITIES SUMMERY
                cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true;
                cell.Value = "TOTAL EQUITY AND LIABILITIES"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell.Style.Font.Size = 14; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = _oBalanceSheet.TotalLiabilitysOwnerEquitys; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                cell.Style.Font.Size = 14; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, 6];
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                if (nSearcingCriteria == 1)//for Monthly
                {

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = _oBalanceSheet.TotalLiabilitysOwnerEquitysTemp; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Font.Size = 14; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, 8];
                    border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                }

                rowIndex = rowIndex + 1;
                #endregion

                #endregion

                #region Blank Row
                cell = sheet.Cells[rowIndex, 2, rowIndex, nLastColIndex]; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;
                #endregion

                cell = sheet.Cells[1, 1, rowIndex, nLastColIndex+2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=StatementOfFinancialPosition.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public ActionResult PrintNotesOfStatement(string sParameter)
        {
            _oAssets = new List<BalanceSheet>();
            _oLiability = new List<BalanceSheet>();
            _oOwnersEquity = new List<BalanceSheet>();
            _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

            int nBUID = sParameter.Split('~')[0] == null ? 0 : sParameter.Split('~')[0] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[0]);
            DateTime dBalanceSheetDate = sParameter.Split('~')[1] == null ? DateTime.Now : sParameter.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[1]);
            int nParentAccountHeadID = sParameter.Split('~')[2] == null ? 0 : sParameter.Split('~')[2] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[2]);
            bool bIsApproved = sParameter.Split('~')[3] == null ? false : sParameter.Split('~')[3] == "" ? false : Convert.ToBoolean(sParameter.Split('~')[3]);
            DateTime dBalanceSheetStartDate = sParameter.Split('~')[4] == null ? DateTime.Now : sParameter.Split('~')[4] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[4]);

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            _oBalanceSheet = new BalanceSheet();
            _oBalanceSheets = BalanceSheet.Gets(nBUID, (int)EnumAccountType.Ledger, dBalanceSheetStartDate, dBalanceSheetDate, nParentAccountHeadID, bIsApproved, (int)Session[SessionInfo.currentUserID]);
            BalanceSheet oBS = new BalanceSheet();
            oBS.AccountHeadID = _oBalanceSheets[0].ParentAccountHeadID;
            this.AddNodes(oBS);

            _oBalanceSheet.Assets = _oAssets;
            _oBalanceSheet.LiabilitysOwnerEquitys = _oOwnersEquity;
            _oBalanceSheet.LiabilitysOwnerEquitys.AddRange(_oLiability);
            _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
            _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);

            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(dBalanceSheetDate, (int)Session[SessionInfo.currentUserID]);
            _oBalanceSheet.TempDate = dBalanceSheetDate;

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null; //GetCompanyLogo(oCompany);
            _oBalanceSheet.Company = oCompany;

            TempData["message"] = "";
            rptNotesOfFinancialStatements oReport = new rptNotesOfFinancialStatements();
            byte[] abytes = oReport.PrepareReport(_oBalanceSheet, oBusinessUnit);
            return File(abytes, "application/pdf");
        }

        public void NotesOfStatementExpotToExcel(string sParameter)
        {
            _oAssets = new List<BalanceSheet>();
            _oLiability = new List<BalanceSheet>();
            _oOwnersEquity = new List<BalanceSheet>();
            _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

            int nBUID = sParameter.Split('~')[0] == null ? 0 : sParameter.Split('~')[0] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[0]);
            DateTime dBalanceSheetDate = sParameter.Split('~')[1] == null ? DateTime.Now : sParameter.Split('~')[1] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[1]);
            int nParentAccountHeadID = sParameter.Split('~')[2] == null ? 0 : sParameter.Split('~')[2] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[2]);
            bool bIsApproved = sParameter.Split('~')[3] == null ? false : sParameter.Split('~')[3] == "" ? false : Convert.ToBoolean(sParameter.Split('~')[3]);
            DateTime dBalanceSheetStartDate = sParameter.Split('~')[4] == null ? DateTime.Now : sParameter.Split('~')[4] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[4]);

            _oBalanceSheet = new BalanceSheet();
            _oBalanceSheets = BalanceSheet.Gets(nBUID, (int)EnumAccountType.Ledger, dBalanceSheetStartDate, dBalanceSheetDate, nParentAccountHeadID, bIsApproved, (int)Session[SessionInfo.currentUserID]);
            _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();

            BalanceSheet oBS = new BalanceSheet();
            oBS.AccountHeadID = _oBalanceSheets[0].ParentAccountHeadID;
            this.AddNodes(oBS);

            _oBalanceSheet.Assets = _oAssets;
            _oBalanceSheet.LiabilitysOwnerEquitys = _oOwnersEquity;
            _oBalanceSheet.LiabilitysOwnerEquitys.AddRange(_oLiability);


            _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
            _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null; //GetCompanyLogo(oCompany);

            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany.Name = oBusinessUnit.Name;
            }
            _oBalanceSheet.Company = oCompany;
            _oBalanceSheet.TempDate = dBalanceSheetDate;

            #region Fit to Excel
            int rowIndex = 2, startRow = 0, endRow = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            bool bIsFistSegment = false;
            bool bIsImmediateSegment = false;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Notes to the Financial Statements");
                sheet.Name = "Notes to the Financial Statements";
                sheet.Column(2).Width = 3;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 20;
                sheet.Column(6).Width = 3;

                #region Report Header
                cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true;
                cell.Value = _oBalanceSheet.Company.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                startRow = rowIndex;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true;
                cell.Value = "Notes to the Financial Statements"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rowIndex = rowIndex + 1;


                cell = sheet.Cells[rowIndex, 2, rowIndex, 4]; cell.Merge = true; 
                cell.Value = "For the Year Ended "+_oBalanceSheet.TempDate.ToString("dd MMMM yyyy"); 
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                cell = sheet.Cells[rowIndex, 5]; cell.Value = _oBalanceSheet.TempDate.ToString("dd-MM-yyyy");
                cell.Style.Font.Bold = true; cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 2;
                #endregion

                #region Asstes Part
                

                int i = 0,ncount=0;
                Double nPriviousSG = 0;
                foreach (BalanceSheet oItem in _oBalanceSheet.Assets)
                {
                    if (oItem.AccountType == EnumAccountType.SubGroup || oItem.AccountType == EnumAccountType.Ledger)
                    {
                        if (oItem.AccountType == EnumAccountType.SubGroup)
                        {

                            if (ncount != 0)
                            {
                                #region Total print of previous Subgroup
                                cell = sheet.Cells[rowIndex, 5]; cell.Value = nPriviousSG; 
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true;
                                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                                rowIndex = rowIndex + 3;
                                #endregion
                                #region Date
                                cell = sheet.Cells[rowIndex, 5]; cell.Value = _oBalanceSheet.TempDate.ToString("dd-MM-yyyy");
                                cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                rowIndex = rowIndex + 1;
                                #endregion
                            }
                            #region subgroup Print
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true;
                            cell.Value = oItem.AccountCode + " " + oItem.AccountHeadName + "  " + oCompany.CurrencyName + " " + oItem.CGSGBalanceInString;
                            cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            rowIndex++;
                            #endregion
                            #region Hard Code Text
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true;
                            cell.Value = "Details of " + oItem.AccountHeadName + " as at " + _oBalanceSheet.TempDate.ToString("dd MMMM yyyy") + " Are Shown in given :";
                            cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            rowIndex++;
                            #endregion
                            ncount++;
                            nPriviousSG = oItem.CGSGBalance;
                        }
                        else
                        {

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;



                            cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.LedgerBalance;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                            rowIndex = rowIndex + 1;
                        }
                    }
                    
                }

                #region Total print of previous Subgroup
                if (_oBalanceSheet.Assets != null && _oBalanceSheet.Assets.Count > 0)
                {
                    cell = sheet.Cells[rowIndex, 5]; cell.Value = nPriviousSG;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 3;
                }
                #endregion

                #endregion

                #region EQUITY AND LIABILITIES PARTS
               
               

                foreach (BalanceSheet oItem in _oBalanceSheet.LiabilitysOwnerEquitys)
                {
                    if (oItem.AccountType == EnumAccountType.SubGroup || oItem.AccountType == EnumAccountType.Ledger)
                    {
                        if (oItem.AccountType == EnumAccountType.SubGroup)
                        {

                            if (ncount != 0)
                            {
                                #region Total print of previous Subgroup
                                cell = sheet.Cells[rowIndex, 5]; cell.Value = nPriviousSG;
                                cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true;
                                cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                                rowIndex = rowIndex + 3;
                                #endregion
                                #region Date
                                cell = sheet.Cells[rowIndex, 5]; cell.Value = _oBalanceSheet.TempDate.ToString("dd-MM-yyyy");
                                cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                rowIndex = rowIndex + 1;
                                #endregion
                            }
                            #region subgroup Print
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true;
                            cell.Value = oItem.AccountCode + " " + oItem.AccountHeadName + "  " + oCompany.CurrencyName + " " + oItem.CGSGBalanceInString;
                            cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            rowIndex++;
                            #endregion
                            #region Hard Code Text
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true;
                            cell.Value = "Details of " + oItem.AccountHeadName + " as at " + _oBalanceSheet.TempDate.ToString("dd MMMM yyyy") + " Are Shown in given :";
                            cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                            rowIndex++;
                            #endregion
                            ncount++;
                            nPriviousSG = oItem.CGSGBalance;
                        }
                        else
                        {

                            cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.AccountHeadName; cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            cell.Style.Font.Size = 11; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;



                            cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.LedgerBalance;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            cell.Style.Font.Bold = false; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                            endRow = rowIndex;
                            rowIndex = rowIndex + 3;
                        }
                    }
                    
                }

                #region Total print of previous Subgroup
                if (_oBalanceSheet.LiabilitysOwnerEquitys != null && _oBalanceSheet.LiabilitysOwnerEquitys.Count > 0)
                {
                    cell = sheet.Cells[rowIndex, 5]; cell.Value = nPriviousSG;
                    cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.Font.Italic = true;
                    cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;


                    rowIndex = rowIndex + 3;
                }
                endRow = rowIndex; 
                #endregion
                

                #endregion



                cell = sheet.Cells[1, 1, rowIndex + 1, 7];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                cell = sheet.Cells[startRow, 2, startRow, 6];
                border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[startRow, 2, endRow, 2];
                border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[startRow, 6, endRow, 6];
                border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[endRow, 2, endRow, 6];
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=StatementOfFinancialPosition.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion


        #region Notes of FP
        public ActionResult ViewNotesOfFinancialPosition(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AccountingSession oAccountingSession = new AccountingSession();
            try
            {
                _oBalanceSheet = (BalanceSheet)Session[SessionInfo.ParamObj];
            }
            catch (Exception ex)
            {
                _oBalanceSheet = null;
            }
            if (_oBalanceSheet != null)
            {
                #region Check Authorize Business Unit
                if (!BusinessUnit.IsPermittedBU(_oBalanceSheet.BUID, (int)Session[SessionInfo.currentUserID]))
                {
                    rptErrorMessage oErrorReport = new rptErrorMessage();
                    byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                    return File(aErrorMessagebytes, "application/pdf");
                }
                #endregion

                if (_oBalanceSheet.IsForCurrentDate)
                {
                    oAccountingSession = AccountingSession.GetRunningAccountingYear((int)Session[SessionInfo.currentUserID]);
                    _oBalanceSheet.TempDate = oAccountingSession.EndDate;

                }
                _oBalanceSheet.AccountType = EnumAccountType.Ledger != _oBalanceSheet.AccountType ? EnumAccountType.Ledger : _oBalanceSheet.AccountType;
                _oBalanceSheets = BalanceSheet.Gets(_oBalanceSheet.BUID, (int)_oBalanceSheet.AccountType, _oBalanceSheet.StartDate, _oBalanceSheet.TempDate, _oBalanceSheet.ParentAccountHeadID, _oBalanceSheet.IsApproved, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
                _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();
                _oBalanceSheet.BalanceSheets = new List<BalanceSheet>();
                _oBalanceSheet.BalanceSheets = _oBalanceSheets;
            }
            else
            {
                _oBalanceSheet = new BalanceSheet();
                _oBalanceSheet.BalanceSheets = new List<BalanceSheet>();
                _oBalanceSheet.AccountType = EnumAccountType.Ledger;
                _oBalanceSheets = BalanceSheet.Gets(0, (int)EnumAccountType.Ledger, oAccountingSession.StartDate, DateTime.Today, 0, false, (int)Session[SessionInfo.currentUserID]); //5 = Ledger
                _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();
                _oBalanceSheet.BalanceSheets = _oBalanceSheets;
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = null; //GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (_oBalanceSheet.BUID > 0)
            {
                oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(_oBalanceSheet.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany.Name = oBusinessUnit.Name;

            }
            //_oBalanceSheet.Company = oCompany;
            //_oBalanceSheet.TempDate = DateTime.Today;

            _oBalanceSheet.Company = oCompany;
            //_oBalanceSheet.TempDate = DateTime.Today;
            ViewBag.BU = oBusinessUnit;
            this.Session.Remove(SessionInfo.ParamObj);
            return View(_oBalanceSheet);
        }
        #endregion
        #endregion



        [HttpGet]
        public JsonResult GetsFinancialPositionData(string sParameter, double ts)
        {
            _oAssets = new List<BalanceSheet>();
            _oLiability = new List<BalanceSheet>();
            _oOwnersEquity = new List<BalanceSheet>();
            _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

            int nBUID = sParameter.Split('~')[0] == null ? 0 : sParameter.Split('~')[0] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[0]);
            int nAccountType = sParameter.Split('~')[1] == null ? 0 : sParameter.Split('~')[1] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[1]);
            DateTime dBalanceSheetDate = sParameter.Split('~')[2] == null ? DateTime.Now : sParameter.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[2]);
            bool bIsApproved = sParameter.Split('~')[3] == null ? false : sParameter.Split('~')[3] == "" ? false : Convert.ToBoolean(sParameter.Split('~')[3]);
            DateTime dBalanceSheetStartDate = sParameter.Split('~')[4] == null ? DateTime.Now : sParameter.Split('~')[4] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[4]);

            _oBalanceSheet = new BalanceSheet();
            _oBalanceSheets = BalanceSheet.Gets(nBUID, (int)EnumAccountType.SubGroup, dBalanceSheetStartDate, dBalanceSheetDate, 0, bIsApproved, (int)Session[SessionInfo.currentUserID]); //4 = Ledger
            _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();

            BalanceSheet oBS = new BalanceSheet();
            oBS.AccountHeadID = 1;
            this.AddNodes(oBS);

            _oBalanceSheet.Assets = _oAssets;
            _oBalanceSheet.LiabilitysOwnerEquitys = _oOwnersEquity;
            _oBalanceSheet.LiabilitysOwnerEquitys.AddRange(_oLiability);


            _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
            _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = null; //GetCompanyLogo(oCompany);

            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany.Name = oBusinessUnit.Name;
            }
            _oBalanceSheet.Company = oCompany;
            _oBalanceSheet.TempDate = dBalanceSheetDate;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBalanceSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RefreshBalanceSheet(string sParameter, double ts)
        {
            _oAssets = new List<BalanceSheet>();
            _oLiability = new List<BalanceSheet>();
            _oOwnersEquity = new List<BalanceSheet>();
            _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

            int nBUID = sParameter.Split('~')[0] == null ? 0 : sParameter.Split('~')[0] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[0]);
            int nAccountType = sParameter.Split('~')[1] == null ? 0 : sParameter.Split('~')[1] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[1]);
            DateTime dBalanceSheetDate = sParameter.Split('~')[2] == null ? DateTime.Now : sParameter.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[2]);
            bool bIsApproved = sParameter.Split('~')[3] == null ? false : sParameter.Split('~')[3] == "" ? false : Convert.ToBoolean(sParameter.Split('~')[3]);
            DateTime dBalanceSheetStartDate = sParameter.Split('~')[4] == null ? DateTime.Now : sParameter.Split('~')[4] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[4]);

            _oBalanceSheet = new BalanceSheet();
            _oBalanceSheets = BalanceSheet.Gets(nBUID, nAccountType, dBalanceSheetStartDate, dBalanceSheetDate, 0, bIsApproved, (int)Session[SessionInfo.currentUserID]); //4 = Ledger
            _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();

            BalanceSheet oBS = new BalanceSheet();
            oBS.AccountHeadID = 1;
            this.AddNodes(oBS);

            _oBalanceSheet.Assets = _oAssets;
            _oBalanceSheet.LiabilitysOwnerEquitys = _oLiability;
            _oBalanceSheet.LiabilitysOwnerEquitys.AddRange(_oOwnersEquity);
            #region Asset Tree
            foreach (BalanceSheet oItem in _oAssets)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentAccountHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.CGSGBalanceInString = oItem.CGSGBalanceInString;
                _oTChartsOfAccount.LadgerBalanceInString = oItem.LedgerBalanceInString;
                //if ((EnumAccountType)nAccountType == EnumAccountType.Ledger)
                //{
                //    if (oItem.AccountType == EnumAccountType.Group || oItem.AccountType == EnumAccountType.SubGroup)
                //    {
                //        _oTChartsOfAccount.state = "closed";
                //    }
                //}
                //else if ((EnumAccountType)nAccountType == EnumAccountType.SubGroup)
                //{
                //    if (oItem.AccountType == EnumAccountType.Group)
                //    {
                //        _oTChartsOfAccount.state = "closed";
                //    }
                //}               
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }
            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRoot(1);
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oBalanceSheet.TAsset = _oTChartsOfAccount;
            #endregion

            #region Liability & OwnersEquity Tree
            _oTChartsOfAccounts = new List<TChartsOfAccount>();
            foreach (BalanceSheet oItem in _oLiabilityAndOwnersEquity)
            {
                _oTChartsOfAccount = new TChartsOfAccount();
                _oTChartsOfAccount.id = oItem.AccountHeadID;
                _oTChartsOfAccount.parentid = oItem.ParentAccountHeadID;
                _oTChartsOfAccount.text = oItem.AccountHeadName;
                _oTChartsOfAccount.CGSGBalanceInString = oItem.CGSGBalanceInString;
                _oTChartsOfAccount.LadgerBalanceInString = oItem.LedgerBalanceInString;
                //if ((EnumAccountType)nAccountType == EnumAccountType.Ledger)
                //{
                //    if (oItem.AccountType == EnumAccountType.Group || oItem.AccountType == EnumAccountType.SubGroup)
                //    {
                //        _oTChartsOfAccount.state = "closed";
                //    }
                //}
                //else if ((EnumAccountType)nAccountType == EnumAccountType.SubGroup)
                //{
                //    if (oItem.AccountType == EnumAccountType.Group)
                //    {
                //        _oTChartsOfAccount.state = "closed";
                //    }
                //}      
                _oTChartsOfAccounts.Add(_oTChartsOfAccount);
            }

            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRootByID(4);//Owners Equity
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oBalanceSheet.TLiablityAndOwnersEquitys.AddRange(_oTChartsOfAccount.children);

            _oTChartsOfAccount = new TChartsOfAccount();
            _oTChartsOfAccount = GetRootByID(3);//Libility
            this.AddTreeNodes(ref _oTChartsOfAccount);
            _oBalanceSheet.TLiablityAndOwnersEquitys.AddRange(_oTChartsOfAccount.children);
            #endregion

            _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
            _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);


            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(dBalanceSheetDate, (int)Session[SessionInfo.currentUserID]);
            _oBalanceSheet.SessionDate = "Balance Sheet @ " + dBalanceSheetDate.ToString("dd MMM yyyy");

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBalanceSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrepareBalanceSheet(string sParameter)
        {
            _oAssets = new List<BalanceSheet>();
            _oLiability = new List<BalanceSheet>();
            _oOwnersEquity = new List<BalanceSheet>();
            _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

            int nBUID = sParameter.Split('~')[0] == null ? 0 : sParameter.Split('~')[0] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[0]);
            int nAccountType = sParameter.Split('~')[1] == null ? 0 : sParameter.Split('~')[1] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[1]);
            DateTime dBalanceSheetDate = sParameter.Split('~')[2] == null ? DateTime.Now : sParameter.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[2]);
            bool bIsApproved = sParameter.Split('~')[3] == null ? false : sParameter.Split('~')[3] == "" ? false : Convert.ToBoolean(sParameter.Split('~')[3]);
            DateTime dBalanceSheetStartDate = sParameter.Split('~')[4] == null ? DateTime.Now : sParameter.Split('~')[4] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[4]);

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion


            _oBalanceSheet = new BalanceSheet();
            _oBalanceSheets = BalanceSheet.Gets(0, nAccountType, dBalanceSheetStartDate, dBalanceSheetDate, 0, bIsApproved, (int)Session[SessionInfo.currentUserID]); //4 = Ledger
            _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();

            BalanceSheet oBS = new BalanceSheet();
            oBS.AccountHeadID = 1;
            this.AddNodes(oBS);

            _oBalanceSheet.Assets = _oAssets;
            _oBalanceSheet.LiabilitysOwnerEquitys = _oLiability;
            _oBalanceSheet.LiabilitysOwnerEquitys.AddRange(_oOwnersEquity);
            _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
            _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);

            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(dBalanceSheetDate, (int)Session[SessionInfo.currentUserID]);
            _oBalanceSheet.SessionDate = "As At " + dBalanceSheetDate.ToString("dd MMM yyyy");

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oBalanceSheet.Company = oCompany;

            TempData["message"] = "";
            rptBalanceSheet oReport = new rptBalanceSheet();
            byte[] abytes = oReport.PrepareReportShort(_oBalanceSheet, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrepareBalanceSheetInXL(string sParameter)
        {
            _oAssets = new List<BalanceSheet>();
            _oLiability = new List<BalanceSheet>();
            _oOwnersEquity = new List<BalanceSheet>();
            _oLiabilityAndOwnersEquity = new List<BalanceSheet>();

            int nBUID = sParameter.Split('~')[0] == null ? 0 : sParameter.Split('~')[0] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[0]);
            int nAccountType = sParameter.Split('~')[1] == null ? 0 : sParameter.Split('~')[1] == "" ? 0 : Convert.ToInt32(sParameter.Split('~')[1]);
            DateTime dBalanceSheetDate = sParameter.Split('~')[2] == null ? DateTime.Now : sParameter.Split('~')[2] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[2]);
            bool bIsApproved = sParameter.Split('~')[3] == null ? false : sParameter.Split('~')[3] == "" ? false : Convert.ToBoolean(sParameter.Split('~')[3]);
            DateTime dBalanceSheetStartDate = sParameter.Split('~')[4] == null ? DateTime.Now : sParameter.Split('~')[4] == "" ? DateTime.Now : Convert.ToDateTime(sParameter.Split('~')[4]);

            AccountingSession oAccountingSession = new AccountingSession();

            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(nBUID, (int)Session[SessionInfo.currentUserID]))
            {
                rptErrorMessage oErrorReport = new rptErrorMessage();
                byte[] aErrorMessagebytes = oErrorReport.PrepareReport("You are not authorize for selected business unit!");
                return File(aErrorMessagebytes, "application/pdf");
            }
            #endregion

            _oBalanceSheet = new BalanceSheet();
            _oBalanceSheets = BalanceSheet.Gets(nBUID, nAccountType, dBalanceSheetStartDate, dBalanceSheetDate, 0, bIsApproved, (int)Session[SessionInfo.currentUserID]); //4 = Ledger
            _oBalanceSheets = _oBalanceSheets.OrderBy(a => a.AccountType).ThenBy(a => a.Sequence).ToList();

            BalanceSheet oBS = new BalanceSheet();
            oBS.AccountHeadID = 1;
            this.AddNodes(oBS);

            _oBalanceSheet.Assets = _oAssets;
            _oBalanceSheet.LiabilitysOwnerEquitys = _oLiability;
            _oBalanceSheet.LiabilitysOwnerEquitys.AddRange(_oOwnersEquity);
            _oBalanceSheet.TotalAssets = BalanceSheet.ComponentBalance(EnumComponentType.Asset, _oAssets);
            _oBalanceSheet.TotalLiabilitysOwnerEquitys = BalanceSheet.ComponentBalance(EnumComponentType.Liability, _oLiabilityAndOwnersEquity);
            var stream = new MemoryStream();
            #region Balance Sheet Short

            var serializer = new XmlSerializer(typeof(List<BalanceSheetShortXL>));

            //We load the data           
            BalanceSheetShortXL oBalanceSheetShortXL = new BalanceSheetShortXL();
            List<BalanceSheetShortXL> oBalanceSheetShortXLs = new List<BalanceSheetShortXL>();

            oAccountingSession = new AccountingSession();
            oAccountingSession = oAccountingSession.GetSessionByDate(dBalanceSheetDate, (int)Session[SessionInfo.currentUserID]);

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
        private void AddNodes(BalanceSheet oIS)
        {
            List<BalanceSheet> oChildNodes = new List<BalanceSheet>();
            oChildNodes = GetChildren(oIS.AccountHeadID);
            foreach (BalanceSheet oItem in oChildNodes)
            {
                BalanceSheet oTemp = oItem;
                if (oItem.ComponentType == EnumComponentType.Asset)
                {
                    _oAssets.Add(oTemp);
                    if (oItem.CGSGBalanceTemp != 0) { _oAssetsTemp.Add(oTemp); }
                }
                else if (oItem.ComponentType == EnumComponentType.OwnersEquity)
                {
                    _oOwnersEquity.Add(oTemp);
                    _oLiabilityAndOwnersEquity.Add(oTemp);
                    if (oItem.CGSGBalanceTemp != 0) { _oLiabilityAndOwnersEquityTemp.Add(oTemp); }
                }
                else if (oItem.ComponentType == EnumComponentType.Liability)
                {
                    _oLiability.Add(oTemp);
                    _oLiabilityAndOwnersEquity.Add(oTemp);
                    if (oItem.CGSGBalanceTemp != 0) { _oLiabilityAndOwnersEquityTemp.Add(oTemp); }
                }
                AddNodes(oTemp);
            }
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

        #region Comprehensive Income Statement Setup
        public ActionResult ViewFinancialPositionSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFinancialPositionSetup = new FinancialPositionSetup();
            _oFinancialPositionSetups = new List<FinancialPositionSetup>();
            _oFinancialPositionSetups = FinancialPositionSetup.Gets((int)Session[SessionInfo.currentUserID]);
            if (_oFinancialPositionSetups.Count > 0)
            {
                _oFinancialPositionSetup.AssetSetups = _oFinancialPositionSetups.Where(x => x.ComponentID == 2).OrderBy(x => x.Sequence).ToList();//Assets list
                _oFinancialPositionSetup.LiabilityWithOwnersEquitySetups = _oFinancialPositionSetups.Where(x => x.ComponentID == 3 || x.ComponentID == 4).OrderBy(x => x.Sequence).ToList();//Owners Equity and Libility list
            }
            return View(_oFinancialPositionSetup);
        }
        [HttpPost]
        public JsonResult Save(FinancialPositionSetup oFinancialPositionSetup)
        {
            _oFinancialPositionSetups = new List<FinancialPositionSetup>();
            _oFinancialPositionSetup = new FinancialPositionSetup();
            try
            {
                _oFinancialPositionSetups = oFinancialPositionSetup.Save((int)Session[SessionInfo.currentUserID]);
                if (_oFinancialPositionSetups.Count > 0)
                {
                    _oFinancialPositionSetup.AssetSetups = _oFinancialPositionSetups.Where(x => x.ComponentID == 2).OrderBy(x => x.Sequence).ToList();//Assets list
                    _oFinancialPositionSetup.LiabilityWithOwnersEquitySetups = _oFinancialPositionSetups.Where(x => x.ComponentID == 3 || x.ComponentID == 4).OrderBy(x => x.Sequence).ToList();//Owners Equity and Libility list
                }
            }
            catch (Exception ex)
            {
                _oFinancialPositionSetup = new FinancialPositionSetup();
                _oFinancialPositionSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFinancialPositionSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            _oFinancialPositionSetup = new FinancialPositionSetup();
            try
            {
                sFeedBackMessage = _oFinancialPositionSetup.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}