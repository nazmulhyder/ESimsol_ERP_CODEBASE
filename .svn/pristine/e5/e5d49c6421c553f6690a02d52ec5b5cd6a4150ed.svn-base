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


namespace ESimSolFinancial.Controllers
{
    public class StatementSetupController : Controller
    {
        #region Declaration
        List<StatementSetup> _oStatementSetups = new List<StatementSetup>();
        StatementSetup _oStatementSetup = new StatementSetup();
        List<OperationCategorySetup> _oOperationCategorySetups = new List<OperationCategorySetup>();
        OperationCategorySetup _oOperationCategorySetup = new OperationCategorySetup();
        List<LedgerGroupSetup> _oLedgerGroupSetups = new List<LedgerGroupSetup>();
        LedgerGroupSetup _oLedgerGroupSetup = new LedgerGroupSetup();
        List<LedgerBreakDown> _oLedgerBreakDowns = new List<LedgerBreakDown>();
        LedgerBreakDown _oLedgerBreakDown = new LedgerBreakDown();
        ChartsOfAccount _oChartsOfAccount = new ChartsOfAccount();
        List<ChartsOfAccount> _oChartsOfAccounts = new List<ChartsOfAccount>();
        #endregion
        #region Functions
        private List<LedgerGroupSetup> GetLedgerGroups(int nOperationCategorySetupID, List<LedgerGroupSetup> oLedgerGroupSetups, List<LedgerBreakDown> oLedgerBreakDowns)
        {
            List<LedgerGroupSetup> oTempLedgerGroupSetups = new List<LedgerGroupSetup>();
            foreach (LedgerGroupSetup oItem in oLedgerGroupSetups)
            {
                if (oItem.OCSID == nOperationCategorySetupID)
                {
                    oItem.LedgerBreakDowns = GetBreakDowns(oItem.LedgerGroupSetupID, oLedgerBreakDowns);
                    oTempLedgerGroupSetups.Add(oItem);
                }
            }
            return oTempLedgerGroupSetups;
        }
        private List<LedgerBreakDown> GetBreakDowns(int id, List<LedgerBreakDown> oLedgerBreakDowns)
        {
            List<LedgerBreakDown> oTempLedgerBreakDowns = new List<LedgerBreakDown>();
            foreach (LedgerBreakDown oItem in oLedgerBreakDowns)
            {
                if (oItem.ReferenceID == id)
                {
                    oTempLedgerBreakDowns.Add(oItem);
                }
            }

            return oTempLedgerBreakDowns;
        }
        #endregion

        public ActionResult ViewStatementSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oStatementSetups = new List<StatementSetup>();
            _oStatementSetups = StatementSetup.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oStatementSetups);
        }
        public ActionResult ViewStatementSetup(int id)
        {
            _oStatementSetup = new StatementSetup();
            _oLedgerGroupSetups = new List<LedgerGroupSetup>();
            _oLedgerBreakDowns = new List<LedgerBreakDown>();
            if (id > 0)
            {
                _oStatementSetup = _oStatementSetup.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oStatementSetup.OperationCategorySetups = OperationCategorySetup.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oStatementSetup.LedgerBreakDowns = LedgerBreakDown.Gets(id, true, ((User)Session[SessionInfo.CurrentUser]).UserID);
                string sSQL = "SELECT * FROM LedgerGroupSetup WHERE OCSID IN (SELECT OperationCategorySetupID FROM OperationCategorySetup WHERE StatementSetupID IN (SELECT StatementSetupID FROM StatementSetup WHERE StatementSetupID = " + id + "))";
                _oLedgerGroupSetups = LedgerGroupSetup.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oLedgerBreakDowns = LedgerBreakDown.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (OperationCategorySetup oItem in _oStatementSetup.OperationCategorySetups)
                {
                    oItem.LedgerGroupSetups = GetLedgerGroups(oItem.OperationCategorySetupID, _oLedgerGroupSetups, _oLedgerBreakDowns);
                }

            }
            else
            {
                _oStatementSetup.OperationCategorySetups = new List<OperationCategorySetup>();
                _oStatementSetup.LedgerBreakDowns = new List<LedgerBreakDown>();
            }

            List<EnumObject> oAccountTypeObjs = EnumObject.jGets(typeof(EnumAccountType));
            ViewBag.AccountTypeObjs  = EnumObject.jGets(typeof(EnumAccountType));// oAccountTypeObjs.Remove(oAccountTypeObjs[5]);//Remove Ledger field
            ViewBag.AccountsHeadDefineNatureObjs = EnumObject.jGets(typeof(EnumAccountsHeadDefineNature));
            ViewBag.DebitCredit = EnumObject.jGets(typeof(EnumDebitCredit));
          
            return View(_oStatementSetup);
        }

        #region Ledger Group
        public ActionResult ViewLedgerGroupSetups(double ts)
        {
            _oLedgerGroupSetups = new List<LedgerGroupSetup>();
            return PartialView(_oLedgerGroupSetups);
        }
        public ActionResult ViewLedgerGroupSetup(double ts)
        {
            _oLedgerGroupSetup = new LedgerGroupSetup();
            return PartialView(_oLedgerGroupSetup);
        }
        #endregion


        [HttpPost]
        public JsonResult Save(StatementSetup oStatementSetup)
        {
            _oStatementSetup = new StatementSetup();
            try
            {
                _oStatementSetup = oStatementSetup;                
                _oStatementSetup = _oStatementSetup.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oStatementSetup = new StatementSetup();
                _oStatementSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oStatementSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                StatementSetup oStatementSetup = new StatementSetup();
                sFeedBackMessage = oStatementSetup.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region View COA chart of Accounts

        public ActionResult ViewChartsOfAccounts()
        {
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oChartsOfAccount.AccountTypeObjs = EnumObject.jGets(typeof(EnumAccountType));
            oChartsOfAccount.AccountTypeObjs.Remove(oChartsOfAccount.AccountTypeObjs[5]); //Remove Ledger field
            return PartialView(oChartsOfAccount);
        }

        [HttpPost]
        public JsonResult Refresh(ChartsOfAccount oChartsOfAccounts)
        {
            if (oChartsOfAccounts.AccountHeadName == null) oChartsOfAccounts.AccountHeadName = "";
            oChartsOfAccounts.AccountType = (EnumAccountType)oChartsOfAccounts.AccountTypeInInt;
            string sSQL = "";
            if (oChartsOfAccounts.AccountType == EnumAccountType.SubGroup)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.SubGroup + " AND AccountHeadName LIKE '%" + oChartsOfAccounts.AccountHeadName + "%')";
            }
            else if (oChartsOfAccounts.AccountType == EnumAccountType.Group)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.Group + " AND AccountHeadName LIKE '%" + oChartsOfAccounts.AccountHeadName + "%'))";
            }
            else if (oChartsOfAccounts.AccountType == EnumAccountType.Segment)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE  ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.Segment + " AND AccountHeadName LIKE '%" + oChartsOfAccounts.AccountHeadName + "%')))";
            }
            else if (oChartsOfAccounts.AccountType == EnumAccountType.Component)
            {
                sSQL = "SELECT * FROM View_ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE ParentHeadID IN (SELECT AccountHeadID FROM ChartsOfAccount WHERE AccountType = " + (int)EnumAccountType.Segment + " AND AccountHeadName LIKE '%" + oChartsOfAccounts.AccountHeadName + "%'))))";
            }
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccounts = ChartsOfAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult ViewYetToAccountConfigure(int smid, string sdt, string edt, int buid, double ts)
        {
            DateTime dStartDate = DateTime.Today;
            DateTime dEndDate = DateTime.Today;
            if (sdt != null && sdt != "")
            {
                dStartDate = Convert.ToDateTime(sdt);
            }
            if (edt != null && edt != "")
            {
                dEndDate = Convert.ToDateTime(edt);
            }

            List<TempObject> oTempObject = new List<TempObject>();
            oTempObject = TempObject.Gets(smid, dStartDate, dEndDate, buid, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oTempObject);
        }
    }



}