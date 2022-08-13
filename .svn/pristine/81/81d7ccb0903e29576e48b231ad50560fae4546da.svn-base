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
    public class BankReconciliationOpenningController : Controller
    {
        #region Declaration        
        AccountingSession _oAccountingSession = new AccountingSession();
        BankReconciliationOpenning _oBankReconciliationOpenning = new BankReconciliationOpenning();
        List<BankReconciliationOpenning> _oBankReconciliationOpennings = new List<BankReconciliationOpenning>();
        #endregion
       
        public ActionResult ViewBankReconciliationOpenning(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            ESimSol.BusinessObjects.User oUser = new ESimSol.BusinessObjects.User();
            oUser = oUser.Get((int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            AccountingSession oAccountingSession = new AccountingSession();
            oAccountingSession = AccountingSession.GetOpenningAccountingYear((int)Session[SessionInfo.currentUserID]);

            _oBankReconciliationOpennings = new List<BankReconciliationOpenning>();
            BankReconciliationOpenning oBankReconciliationOpenning = new BankReconciliationOpenning();
            oBankReconciliationOpenning.LstCurrency = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            oBankReconciliationOpenning.BusinessUnits = BusinessUnit.GetsPermittedBU(oUser, (int)Session[SessionInfo.currentUserID]);
            oBankReconciliationOpenning.CurrencyID = oCompany.BaseCurrencyID;
            oBankReconciliationOpenning.BaseCurrencyId = oCompany.BaseCurrencyID;
            oBankReconciliationOpenning.BaseCurrencySymbol = oCompany.CurrencySymbol;
            oBankReconciliationOpenning.BusinessUnitID = buid;
            oBankReconciliationOpenning.OpenningDate = oAccountingSession.StartDate;
            return View(oBankReconciliationOpenning);
        }

        [HttpPost]
        public JsonResult GetSubledgerOpenningBalance(BankReconciliationOpenning oBankReconciliationOpenning)
        {            
            _oAccountingSession = new AccountingSession();
            _oBankReconciliationOpenning = new BankReconciliationOpenning();
            try
            {
                _oAccountingSession = _oAccountingSession.GetSessionByDate(DateTime.Today, (int)Session[SessionInfo.currentUserID]);
                _oBankReconciliationOpenning = _oBankReconciliationOpenning.GetsByAccountAndSubledgerAndSession(oBankReconciliationOpenning.AccountHeadID, _oAccountingSession.AccountingSessionID, oBankReconciliationOpenning.BusinessUnitID, oBankReconciliationOpenning.SubledgerID, (int)Session[SessionInfo.currentUserID]);                
                _oBankReconciliationOpenning.AccountingSessionID = _oAccountingSession.AccountingSessionID;                
            }
            catch (Exception ex)
            {
                _oBankReconciliationOpenning = new BankReconciliationOpenning();
                _oBankReconciliationOpenning.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankReconciliationOpenning);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBankReconcilatonOpenning(BankReconciliationOpenning oBankReconciliationOpenning)
        {
            #region Check Authorize Business Unit
            if (!BusinessUnit.IsPermittedBU(oBankReconciliationOpenning.BusinessUnitID, (int)Session[SessionInfo.currentUserID]))
            {
                _oBankReconciliationOpenning = new BankReconciliationOpenning();
                _oBankReconciliationOpenning.ErrorMessage = "You are not authorize for selected business unit!";

                JavaScriptSerializer oJavaScriptSerializer = new JavaScriptSerializer();
                string sjsonString = oJavaScriptSerializer.Serialize(_oBankReconciliationOpenning);
                return Json(sjsonString, JsonRequestBehavior.AllowGet);
            }
            #endregion

            _oBankReconciliationOpenning = new BankReconciliationOpenning();            
            try
            {
                _oBankReconciliationOpenning = oBankReconciliationOpenning.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oBankReconciliationOpenning = new BankReconciliationOpenning();
                _oBankReconciliationOpenning.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBankReconciliationOpenning);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
