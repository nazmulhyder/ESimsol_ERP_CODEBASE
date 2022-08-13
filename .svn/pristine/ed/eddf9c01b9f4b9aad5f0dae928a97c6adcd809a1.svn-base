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
using System.Threading;
using ESimSolFinancial.Hubs;
using System.Threading.Tasks;

namespace ESimSolFinancial.Controllers
{
    public class AccountingSessionController : Controller
    {
        #region Declaration
        AccountingSession _oAccountingSession = new AccountingSession();
        List<AccountingSession> _oAccountingSessions = new List<AccountingSession>();
        List<AccountingSession> _oSelectedAccountingSessions = new List<AccountingSession>();
        TAccountingSession _oTAccountingSession = new TAccountingSession();
        List<TAccountingSession> _oTAccountingSessions = new List<TAccountingSession>();
        string[] _aSessionHierarchy = null;
        string _sErrorMessage = "";
        #endregion

        #region Functions
        private AccountingSession GetRootAccountingSession()
        {
            AccountingSession oAccountingSession = new AccountingSession();
            foreach (AccountingSession oItem in _oAccountingSessions)
            {
                if (oItem.ParentSessionID == -1)
                {
                    return oItem;
                }
            }
            return oAccountingSession;
        }

        private List<AccountingSession> GetChildAccountingSessions(int nAccountHeadID)
        {
            List<AccountingSession> oAccountingSessions = new List<AccountingSession>();
            foreach (AccountingSession oItem in _oAccountingSessions)
            {
                if (oItem.ParentSessionID == nAccountHeadID)
                {
                    oAccountingSessions.Add(oItem);
                }
            }
            return oAccountingSessions;
        }

        private void AddDayEndTreeNodes(AccountingSession oAccountingSession)
        {
            int nSessionID = oAccountingSession.AccountingSessionID;
            _oSelectedAccountingSessions = _oAccountingSessions;
            List<AccountingSession> oLastUpdate = new List<AccountingSession>();
            List<AccountingSession> oYEndList = new List<AccountingSession>();
            List<AccountingSession> oHYEndList = new List<AccountingSession>();
            List<AccountingSession> oQEndList = new List<AccountingSession>();
            List<AccountingSession> oMEndList = new List<AccountingSession>();
            List<AccountingSession> oWEndList = new List<AccountingSession>();
            List<AccountingSession> oDEndList = new List<AccountingSession>();

            _aSessionHierarchy = oAccountingSession.SessionHierarchy.Split(',');
            oYEndList = GetChild(1, EnumSessionType.YearEnd);
            oHYEndList = GetChild(nSessionID, EnumSessionType.HalfYearEnd);
            oQEndList = GetChild(nSessionID, EnumSessionType.QuarterEnd);
            oMEndList = GetChild(nSessionID, EnumSessionType.MonthEnd);
            oWEndList = GetChild(nSessionID, EnumSessionType.WeekEnd);
            oDEndList = GetChild(nSessionID, EnumSessionType.DayEnd);

            if (DisplayNode(EnumSessionType.YearEnd.ToString()))
            {
                foreach (AccountingSession oYItem in oYEndList)
                {
                    UpdateParentID(oYItem.AccountingSessionID, 1, ref _oSelectedAccountingSessions);
                }
            }

            if (DisplayNode(EnumSessionType.HalfYearEnd.ToString()))
            {
                foreach (AccountingSession oHItem in oHYEndList)
                {
                    UpdateParentID(oHItem.AccountingSessionID, nSessionID, ref _oSelectedAccountingSessions);
                }
                oLastUpdate = oHYEndList;
            }

            if (DisplayNode(EnumSessionType.QuarterEnd.ToString()))
            {
                if (oLastUpdate.Count <= 0)
                {
                    foreach (AccountingSession oQItem in oQEndList)
                    {
                        UpdateParentID(oQItem.AccountingSessionID, nSessionID, ref _oSelectedAccountingSessions);
                    }
                }
                else
                {
                    foreach (AccountingSession oLQItem in oLastUpdate)
                    {
                        foreach (AccountingSession oQItem in oQEndList)
                        {
                            if (oLQItem.StartDate <= oQItem.StartDate && oLQItem.EndDate >= oQItem.EndDate)
                            {
                                UpdateParentID(oQItem.AccountingSessionID, oLQItem.AccountingSessionID, ref _oSelectedAccountingSessions);
                            }
                        }
                    }
                }
                oLastUpdate = oQEndList;
            }

            if (DisplayNode(EnumSessionType.MonthEnd.ToString()))
            {
                if (oLastUpdate.Count <= 0)
                {
                    foreach (AccountingSession oMItem in oMEndList)
                    {
                        UpdateParentID(oMItem.AccountingSessionID, nSessionID, ref _oSelectedAccountingSessions);
                    }
                }
                else
                {
                    foreach (AccountingSession oLQItem in oLastUpdate)
                    {
                        foreach (AccountingSession oMItem in oMEndList)
                        {
                            if (oLQItem.StartDate <= oMItem.StartDate && oLQItem.EndDate >= oMItem.EndDate)
                            {
                                UpdateParentID(oMItem.AccountingSessionID, oLQItem.AccountingSessionID, ref _oSelectedAccountingSessions);
                            }
                        }
                    }
                }
                oLastUpdate = oMEndList;
            }


            if (DisplayNode(EnumSessionType.WeekEnd.ToString()))
            {
                if (oLastUpdate.Count <= 0)
                {
                    foreach (AccountingSession oWItem in oWEndList)
                    {
                        UpdateParentID(oWItem.AccountingSessionID, nSessionID, ref _oSelectedAccountingSessions);
                    }
                }
                else
                {
                    foreach (AccountingSession oLQItem in oLastUpdate)
                    {
                        foreach (AccountingSession oWItem in oWEndList)
                        {
                            if (oLQItem.StartDate <= oWItem.StartDate && oLQItem.EndDate >= oWItem.EndDate)
                            {
                                UpdateParentID(oWItem.AccountingSessionID, oLQItem.AccountingSessionID, ref _oSelectedAccountingSessions);
                            }
                        }
                    }
                }
                oLastUpdate = oWEndList;
            }

            if (DisplayNode(EnumSessionType.DayEnd.ToString()))
            {
                if (oLastUpdate.Count <= 0)
                {
                    foreach (AccountingSession oDItem in oDEndList)
                    {
                        UpdateParentID(oDItem.AccountingSessionID, nSessionID, ref _oSelectedAccountingSessions);
                    }
                }
                else
                {
                    foreach (AccountingSession oLQItem in oLastUpdate)
                    {
                        foreach (AccountingSession oDItem in oDEndList)
                        {
                            if (oLQItem.StartDate <= oDItem.StartDate && oLQItem.EndDate >= oDItem.EndDate)
                            {
                                UpdateParentID(oDItem.AccountingSessionID, oLQItem.AccountingSessionID, ref _oSelectedAccountingSessions);
                            }
                        }
                    }
                }
            }
        }

        public void UpdateParentID(int nID, int nParentID, ref List<AccountingSession> oAccountingSessions)
        {
            foreach (AccountingSession oItem in oAccountingSessions)
            {
                if (oItem.AccountingSessionID == nID)
                {
                    oItem.ParentID = nParentID;
                }
            }
        }

        private List<AccountingSession> GetChild(int nSessionID, EnumSessionType eEnumSessionType)
        {
            List<AccountingSession> oAccountingSessions = new List<AccountingSession>();
            foreach (AccountingSession oItem in _oAccountingSessions)
            {
                if (oItem.SessionID == nSessionID && oItem.SessionType == eEnumSessionType)
                {
                    oAccountingSessions.Add(oItem);
                }
            }
            return oAccountingSessions;
        }

        private bool DisplayNode(string sNodeType)
        {
            foreach (string sItem in _aSessionHierarchy)
            {
                if (sNodeType == sItem)
                {
                    return true;
                }
            }
            return false;
        }

        private TAccountingSession GetRoot()
        {
            TAccountingSession oTAccountingSession = new TAccountingSession();
            foreach (TAccountingSession oItem in _oTAccountingSessions)
            {
                if (oItem.parentid == -1)
                {
                    return oItem;
                }
            }
            return oTAccountingSession;
        }

        private IEnumerable<TAccountingSession> GetChild(int nAccountingSessionID)
        {
            List<TAccountingSession> oTAccountingSessions = new List<TAccountingSession>();
            foreach (TAccountingSession oItem in _oTAccountingSessions)
            {
                if (oItem.parentid == nAccountingSessionID)
                {
                    oTAccountingSessions.Add(oItem);
                }
            }
            return oTAccountingSessions;
        }

        private void AddTreeNodes(ref TAccountingSession oTAccountingSession)
        {
            IEnumerable<TAccountingSession> oChildNodes;
            oChildNodes = GetChild(oTAccountingSession.id);
            oTAccountingSession.children = oChildNodes;

            foreach (TAccountingSession oItem in oChildNodes)
            {
                TAccountingSession oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }


        private bool ValidateInput(AccountingSession oAccountingSession, string[] SessionHierarchy)
        {
            if (oAccountingSession.SessionName == null || oAccountingSession.SessionName == "")
            {
                _sErrorMessage = "Please enter AccountingSession Name";
                return false;
            }           

            if (oAccountingSession.StartDate >= oAccountingSession.EndDate)
            {
                _sErrorMessage = "End Date must be greater than Start Date!";
                return false;
            }

            DateTime dLastDayOfYear = oAccountingSession.StartDate.AddYears(1).AddDays(-1);
            if (dLastDayOfYear != oAccountingSession.EndDate)
            {
                _sErrorMessage = "Please Select exactly 1 year! Expected Year End Date :" + dLastDayOfYear.ToString("dd MMM yyyy");
                return false;
            }

            if (SessionHierarchy == null)
            {
                _sErrorMessage = "Atlist select Year End or Day End  SessionHierarchy";
                return false;
            }

            if (!CheckYearDayEnd(SessionHierarchy, EnumSessionType.YearEnd.ToString()))
            {
                _sErrorMessage = "Please select Year End SessionHierarchy";
                return false;
            }

            if (!CheckYearDayEnd(SessionHierarchy, EnumSessionType.DayEnd.ToString()))
            {
                _sErrorMessage = "Please select Day End SessionHierarchy";
                return false;
            }
            return true;
        }

        private bool CheckYearDayEnd(string[] SessionHierarchy, string sTemp)
        {
            foreach (string oItem in SessionHierarchy)
            {
                if (sTemp == oItem)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region New Task
        public TAccountingSession GetTAccountingSession(string x)
        {
            _oAccountingSessions = new List<AccountingSession>();
            _oAccountingSession = new AccountingSession();
            _oTAccountingSession = new TAccountingSession();
            _oTAccountingSessions = new List<TAccountingSession>();

            if (x == "mgt")
            {
                _oAccountingSessions = AccountingSession.GetsTitleSessions((int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oAccountingSessions = AccountingSession.Gets((int)Session[SessionInfo.currentUserID]);
            }
            _oAccountingSession = GetRootAccountingSession();
            List<AccountingSession> oAccountingSessions = GetChildAccountingSessions(_oAccountingSession.AccountingSessionID);
            foreach (AccountingSession oItem in oAccountingSessions)
            {
                AddDayEndTreeNodes(oItem);
            }

            #region Convert Root
            _oTAccountingSession = new TAccountingSession();
            _oTAccountingSession.id = _oAccountingSession.AccountingSessionID;
            _oTAccountingSession.text = _oAccountingSession.SessionName;
            if (_oAccountingSession.SessionType == EnumSessionType.None || _oAccountingSession.SessionType == EnumSessionType.YearEnd)
            {
                _oTAccountingSession.state = "open";
            }
            else
            {
                _oTAccountingSession.state = "closed";
            }
            _oTAccountingSession.attributes = "";
            _oTAccountingSession.parentid = _oAccountingSession.ParentSessionID;
            _oTAccountingSession.SessionType = _oAccountingSession.SessionType.ToString();
            _oTAccountingSession.SessionCode = _oAccountingSession.SessionCode;
            _oTAccountingSession.YearStatus = _oAccountingSession.YearStatus.ToString();
            _oTAccountingSession.StartDate = _oAccountingSession.StartDate.ToString("dd MMM yyyy");
            _oTAccountingSession.EndDate = _oAccountingSession.EndDate.ToString("dd MMM yyyy");
            _oTAccountingSession.LockDateTime = _oAccountingSession.LockDateTimeString;
            _oTAccountingSession.LockDateInString = _oAccountingSession.LockDateInString;
            _oTAccountingSession.ActivationDateTime = _oAccountingSession.ActivationDateTime.ToString("HH:mm");
            _oTAccountingSession.SessionHierarchy = _oAccountingSession.SessionHierarchy;            
            _oTAccountingSessions.Add(_oTAccountingSession);
            #endregion

            foreach (AccountingSession oItem in _oSelectedAccountingSessions)
            {

                _oTAccountingSession = new TAccountingSession();
                _oTAccountingSession.id = oItem.AccountingSessionID;
                _oTAccountingSession.text = oItem.SessionName;
                if (oItem.SessionType == EnumSessionType.None || oItem.SessionType == EnumSessionType.DayEnd)
                {
                    _oTAccountingSession.state = "open";
                }
                else
                {
                    _oTAccountingSession.state = "closed";
                }
                _oTAccountingSession.attributes = "";
                _oTAccountingSession.parentid = oItem.ParentID;
                _oTAccountingSession.SessionType = oItem.SessionType.ToString();
                _oTAccountingSession.SessionCode = oItem.SessionCode;
                _oTAccountingSession.YearStatus = oItem.YearStatus.ToString();
                _oTAccountingSession.StartDate = oItem.StartDate.ToString("dd MMM yyyy");
                _oTAccountingSession.EndDate = oItem.EndDate.ToString("dd MMM yyyy");
                _oTAccountingSession.LockDateTime = oItem.LockDateTimeString;
                _oTAccountingSession.LockDateInString = oItem.LockDateInString;
                _oTAccountingSession.ActivationDateTime = oItem.ActivationDateTime.ToString("HH:mm");
                _oTAccountingSession.SessionHierarchy = oItem.SessionHierarchy;                
                _oTAccountingSessions.Add(_oTAccountingSession);

            }
            _oTAccountingSession = new TAccountingSession();
            _oTAccountingSession = GetRoot();
            this.AddTreeNodes(ref _oTAccountingSession);
            return _oTAccountingSession;
        }
        
        public ActionResult ViewAccountingSessions(string x,int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AccountingSession).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            try
            {
                TempData["Operation"] = x;
                _oTAccountingSession = new TAccountingSession();
                _oTAccountingSession = GetTAccountingSession(x);

                ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
                oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
                ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

                return View(_oTAccountingSession);
            }
            catch (Exception ex)
            {
                _oTAccountingSession = new TAccountingSession();
                ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
                oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
                ViewBag.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);

                return View(_oTAccountingSession);
            }
        }
        
        public ActionResult ViewAccountingSession(int id, double ts)
        {
            _oAccountingSession = new AccountingSession();
            if (id > 0)
            {
                _oAccountingSession = _oAccountingSession.Get(id, (int)Session[SessionInfo.currentUserID]);

            }
            return PartialView(_oAccountingSession);
        }
        
        [HttpPost]
        public JsonResult Refresh(AccountingSession oAccountingSession)
        {
            _oAccountingSession = new AccountingSession();
            try
            {
                _oTAccountingSession = new TAccountingSession();
                _oTAccountingSession = GetTAccountingSession("issue");
            }
            catch (Exception ex)
            {
                _oTAccountingSession = new TAccountingSession();
                _oTAccountingSession.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAccountingSession);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(AccountingSession oAccountingSession)
        {
            _oAccountingSession = new AccountingSession();
            try
            {                
                if (oAccountingSession.SessionHierarchy.Length > 0)
                {
                    oAccountingSession.SessionHierarchy = oAccountingSession.SessionHierarchy.Remove(oAccountingSession.SessionHierarchy.Length - 1, 1);
                }
                if (oAccountingSession.WeekLyHolidays != null)
                {
                    if (oAccountingSession.WeekLyHolidays.Length > 0)
                    {
                        oAccountingSession.WeekLyHolidays = oAccountingSession.WeekLyHolidays.Remove(oAccountingSession.WeekLyHolidays.Length - 1, 1);
                    }
                }
                _oAccountingSession = oAccountingSession;
                _oAccountingSession = _oAccountingSession.Save((int)Session[SessionInfo.currentUserID]);

                _oTAccountingSession = new TAccountingSession();
                if (_oAccountingSession.ErrorMessage == "")
                {
                    _oTAccountingSession = GetTAccountingSession("issue");
                }
                else
                {
                    _oTAccountingSession.ErrorMessage = _oAccountingSession.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oTAccountingSession = new TAccountingSession();
                _oTAccountingSession.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAccountingSession);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(AccountingSession oAccountingSession)
        {
            string sFeedBackMessage = "";
            try
            {                
                sFeedBackMessage = oAccountingSession.Delete(oAccountingSession.AccountingSessionID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {             
                sFeedBackMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewSessionLockUnLock(int id, double ts)
        {
            _oAccountingSession = new AccountingSession();
            if (id > 0)
            {
                _oAccountingSession = _oAccountingSession.Get(id, (int)Session[SessionInfo.currentUserID]);

            }
            return PartialView(_oAccountingSession);
        }

        [HttpPost]
        public ActionResult LockUnLock(AccountingSession oAccountingSession)
        {
            _oAccountingSession = new AccountingSession();
            try
            {
                oAccountingSession.LockDateTime = Convert.ToDateTime(oAccountingSession.LockDateTimeString);
                _oAccountingSession = oAccountingSession;
                _oAccountingSession = _oAccountingSession.LockUnLock((int)Session[SessionInfo.currentUserID]);

                _oTAccountingSession = new TAccountingSession();
                if (_oAccountingSession.ErrorMessage == "")
                {
                    _oTAccountingSession = GetTAccountingSession("issue");
                }
                else
                {
                    _oTAccountingSession.ErrorMessage = _oAccountingSession.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oTAccountingSession = new TAccountingSession();
                _oTAccountingSession.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAccountingSession);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeclareNewAccountingYear(AccountingSession oAccountingSession)
        {
            List<ACCostCenter> oACCostCenters = new List<ACCostCenter>(); 
            _oAccountingSession = new AccountingSession();
            try
            {
                string sSubledgerSQL = "";
                double nPercentCount = 5;
                Thread.Sleep(100);
                ProgressHub.SendMessage("Transfer Accounts Balance for :" + _oAccountingSession.SessionName, nPercentCount, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(500);

                _oAccountingSession = oAccountingSession;
                _oAccountingSession = _oAccountingSession.DeclareNewAccountingYear((int)Session[SessionInfo.currentUserID]);
                if (_oAccountingSession.IsOpeningTransfer)
                {
                    

                    List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
                    string sSQL = "SELECT * FROM View_ChartsOfAccount AS HH WHERE HH.AccountType=5 AND HH.ComponentID IN (2,3,4) ORDER BY HH.ComponentID, HH.AccountHeadID ASC";
                    oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    double nTotalAccountHeads = Convert.ToDouble(oChartsOfAccounts.Count);
                    if (nTotalAccountHeads <= 0)
                    {
                        nTotalAccountHeads = 1;
                    }
                    double nPerAccountPercentCount = (95.00 / nTotalAccountHeads);
                    foreach (ChartsOfAccount oItem in oChartsOfAccounts)
                    {
                        nPercentCount = nPercentCount + nPerAccountPercentCount;
                        ProgressHub.SendMessage("Transfer Ledger : " + oItem.AccountHeadName, nPercentCount, (int)Session[SessionInfo.currentUserID]);
                        AccountingSession.TransferOpeningBalance(_oAccountingSession.AccountingSessionID, _oAccountingSession.PreRunningSessionID, oAccountingSession.BUID, oItem.AccountHeadID, 0, (int)Session[SessionInfo.currentUserID]);
                        Thread.Sleep(100);

                        sSubledgerSQL = "";
                        oACCostCenters = new List<ACCostCenter>();
                        sSubledgerSQL = "SELECT * FROM View_ACCostCenter AS HH WHERE HH.ParentID != 0 AND (HH.ACCostCenterID IN(SELECT MM.BreakdownObjID FROM AccountOpenningBreakdown AS MM WHERE MM.AccountHeadID=" + oItem.AccountHeadID.ToString() + " AND MM.BreakdownType=1 AND MM.AccountingSessionID=" + _oAccountingSession.PreRunningSessionID.ToString() + " AND MM.BusinessUnitID=" + _oAccountingSession.BUID.ToString() + ") OR HH.ACCostCenterID IN (SELECT WW.CCID FROM View_CostCenterTransaction AS WW WHERE WW.AccountHeadID=" + oItem.AccountHeadID.ToString() + " AND WW.BUID = " + _oAccountingSession.BUID.ToString() + " AND CONVERT(DATE,CONVERT(VARCHAR(12),WW.VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + _oAccountingSession.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + _oAccountingSession.EndDate.ToString("dd MMM yyyy") + "',106)))) ORDER BY Name";
                        oACCostCenters = ACCostCenter.Gets(sSubledgerSQL, (int)Session[SessionInfo.currentUserID]);
                        foreach (ACCostCenter oSubledger in oACCostCenters)
                        {                            
                            ProgressHub.SendMessage("Transfer Ledger: " + oItem.AccountHeadName + " , Subledger : "+oSubledger.Name, nPercentCount, (int)Session[SessionInfo.currentUserID]);
                            AccountingSession.TransferOpeningBalance(_oAccountingSession.AccountingSessionID, _oAccountingSession.PreRunningSessionID, oAccountingSession.BUID, oItem.AccountHeadID, oSubledger.ACCostCenterID, (int)Session[SessionInfo.currentUserID]);
                            Thread.Sleep(100);
                        }
                    }
                }

                _oTAccountingSession = new TAccountingSession();
                if (_oAccountingSession.ErrorMessage == "")
                {
                    _oTAccountingSession = GetTAccountingSession("mgt");
                }
                else
                {
                    _oTAccountingSession.ErrorMessage = _oAccountingSession.ErrorMessage;
                }
                ProgressHub.SendMessage("Finishing Operation", 100, (int)Session[SessionInfo.currentUserID]);
                ProgressHub.SendMessage("Finishing Operation", 100, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                _oTAccountingSession = new TAccountingSession();
                _oTAccountingSession.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAccountingSession);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AccountingYearClose(AccountingSession oAccountingSession)
        {
            _oAccountingSession = new AccountingSession();
            try
            {
                _oAccountingSession = oAccountingSession;
                _oAccountingSession = _oAccountingSession.AccountingYearClose((int)Session[SessionInfo.currentUserID]);

                _oTAccountingSession = new TAccountingSession();
                if (_oAccountingSession.ErrorMessage == "")
                {
                    _oTAccountingSession = GetTAccountingSession("mgt");
                }
                else
                {
                    _oTAccountingSession.ErrorMessage = _oAccountingSession.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                _oTAccountingSession = new TAccountingSession();
                _oTAccountingSession.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTAccountingSession);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Checks(string[] values, string name)
        {
            ViewBag.Name = name;
            return PartialView(values);
        }

        public ActionResult holidays(string[] values, string name)
        {
            ViewBag.Name = name;
            return PartialView(values);
        }

        [HttpPost]
        public JsonResult GetsSession(AccountingSession oAccountingSession)
        {
            List<AccountingSession> oAccountingSessions = new List<AccountingSession>();
            oAccountingSessions = AccountingSession.GetsAccountingYears((int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAccountingSessions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion      
    }
}
