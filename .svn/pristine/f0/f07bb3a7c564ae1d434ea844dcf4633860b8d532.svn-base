using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using System.Net.Mail;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing.Imaging;
using System.Drawing;


namespace ESimSolFinancial.Controllers
{
    public class AccountsBookSetupController : Controller
    {
        #region Declaration
        AccountsBookSetup _oAccountsBookSetup = new AccountsBookSetup();
        List<AccountsBookSetup> _oAccountsBookSetups = new List<AccountsBookSetup>();
        #endregion

        #region Actions
        public ActionResult ViewAccountsBookSetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AccountsBookSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oAccountsBookSetups = new List<AccountsBookSetup>();
            _oAccountsBookSetups = AccountsBookSetup.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oAccountsBookSetups);
        }

        public ActionResult ViewAccountsBookSetup(int id, double ts)
        {
            _oAccountsBookSetup = new AccountsBookSetup();            
            if (id > 0)
            {                
                _oAccountsBookSetup = _oAccountsBookSetup.Get(id, (int)Session[SessionInfo.currentUserID]);                
                _oAccountsBookSetup.AccountsBookSetupDetails = AccountsBookSetupDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.MappingTypes = EnumObject.jGets(typeof(EnumACMappingType));
            ViewBag.ComponentTypes = EnumObject.jGets(typeof(EnumComponentType));
            return View(_oAccountsBookSetup);
        }

        [HttpPost]
        public JsonResult Save(AccountsBookSetup oAccountsBookSetup)
        {
            _oAccountsBookSetup = new AccountsBookSetup();
            try
            {
                _oAccountsBookSetup = oAccountsBookSetup;
                _oAccountsBookSetup = _oAccountsBookSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oAccountsBookSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAccountsBookSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(AccountsBookSetup oAccountsBookSetup)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oAccountsBookSetup.Delete(oAccountsBookSetup.AccountsBookSetupID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsCOAByCodeOrName(ChartsOfAccount oChartsOfAccount)
        {
            VoucherType oVoucherType = new VoucherType();
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();

            oChartsOfAccounts = ChartsOfAccount.GetsByCodeOrName(oChartsOfAccount, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
