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
    public class ELSetupController : Controller
    {
        #region Declaration
        ELSetup _oELSetup = new ELSetup();
        List<ELSetup> _oELSetups = new List<ELSetup>();
        #endregion

        #region Actions
        public ActionResult View_ELSetup(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AccountsBookSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oELSetups = new List<ELSetup>();
            _oELSetups = ELSetup.Gets((int)Session[SessionInfo.currentUserID]);

            string sSql = "select * from View_ELSetup where InactiveBy = 0";
            _oELSetup = new ELSetup();
            _oELSetup = ELSetup.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ELSetupActive = _oELSetup;

            return View(_oELSetups);
        }

        [HttpPost]
        public JsonResult Save(ELSetup oELSetup)
        {
            _oELSetup = new ELSetup();
            try
            {
                _oELSetup = oELSetup;
                _oELSetup = _oELSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oELSetup.ErrorMessage = ex.Message;

                _oELSetup = new ELSetup();
                _oELSetup.ErrorMessage = ex.Message.Split('~')[0];
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oELSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ELSetup oELSetup)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oELSetup.Delete(oELSetup.ELSetupID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApproveELSetup(ELSetup oELSetup)
        {
            _oELSetup = new ELSetup();
            try
            {
                _oELSetup = oELSetup.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oELSetup = new ELSetup();
                _oELSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oELSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        public JsonResult InactiveELSetup(ELSetup oELSetup)
        {
            _oELSetup = new ELSetup();
            try
            {
                _oELSetup = oELSetup.Inactive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oELSetup = new ELSetup();
                _oELSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oELSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
