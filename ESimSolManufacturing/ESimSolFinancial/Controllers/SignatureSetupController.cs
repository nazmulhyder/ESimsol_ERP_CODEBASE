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


namespace ESimSolFinancial.Controllers
{
    public class SignatureSetupController : Controller
    {
        #region Declaration        
        SignatureSetup _oSignatureSetup = new SignatureSetup();
        List<SignatureSetup> _oSignatureSetups = new List<SignatureSetup>();        
        #endregion

        #region SignatureSetup Actions
        public ActionResult ViewSignatureSetup(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.SignatureSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oSignatureSetups = new List<SignatureSetup>();
            _oSignatureSetups = SignatureSetup.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ReportModules = EnumObject.jGets(typeof(EnumReportModule));
            return View(_oSignatureSetups);
        }
             
        [HttpPost]
        public JsonResult SaveSignatureSetup(SignatureSetup oSignatureSetup)
        {
            _oSignatureSetup = new SignatureSetup();            
            try
            {
                _oSignatureSetup = oSignatureSetup;
                if (_oSignatureSetup.DisplayDataColumn == null) { _oSignatureSetup.DisplayDataColumn = ""; }
                _oSignatureSetup = _oSignatureSetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSignatureSetup = new SignatureSetup();
                _oSignatureSetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSignatureSetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSignatureSetup(SignatureSetup oSignatureSetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oSignatureSetup.Delete((int)Session[SessionInfo.currentUserID]);
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
