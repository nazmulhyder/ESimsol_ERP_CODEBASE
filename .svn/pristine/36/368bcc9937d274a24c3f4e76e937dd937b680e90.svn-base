using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class MgtDBACSetupController : Controller
    {
        #region Declaration
        MgtDBACSetup _oMgtDBACSetup = new MgtDBACSetup();
        List<MgtDBACSetup> _oMgtDBACSetups = new List<MgtDBACSetup>();
        #endregion

        #region Actions
        public ActionResult ViewMgtDBACSetup(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.MgtDBACSetup).ToString() + "," + ((int)EnumModuleName.TAP).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oMgtDBACSetups = new List<MgtDBACSetup>();
            _oMgtDBACSetups = MgtDBACSetup.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.MgtDBACTypes = EnumObject.jGets(typeof(EnumMgtDBACType));
            return View(_oMgtDBACSetups);
        }

        [HttpPost]
        public JsonResult Save(MgtDBACSetup oMgtDBACSetup)
        {
            _oMgtDBACSetup = new MgtDBACSetup();
            try
            {
                _oMgtDBACSetup = oMgtDBACSetup;
                _oMgtDBACSetup = oMgtDBACSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMgtDBACSetup = new MgtDBACSetup();
                _oMgtDBACSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMgtDBACSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult Delete(MgtDBACSetup oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                MgtDBACSetup oMgtDBACSetup = new MgtDBACSetup();
                sFeedBackMessage = oMgtDBACSetup.Delete(oJB.MgtDBACSetupID, (int)Session[SessionInfo.currentUserID]);
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


        #region Get
        [HttpPost]
        public JsonResult GetAccountHead(ChartsOfAccount oChartsOfAccount)//Id=ContractorID
        {
            string sAccountHeadName = oChartsOfAccount.ErrorMessage;
            List<ChartsOfAccount> _oChartsOfAccounts = new List<ChartsOfAccount>();
            string Ssql = "SELECT * FROM View_ChartsOfAccount WHERE AccountHeadName LIKE '%" + sAccountHeadName + "%' ";
            _oChartsOfAccounts = new List<ChartsOfAccount>();
            _oChartsOfAccounts = ChartsOfAccount.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChartsOfAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
