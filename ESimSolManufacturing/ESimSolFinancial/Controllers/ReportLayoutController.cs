using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
 


namespace ESimSolFinancial.Controllers
{

    public class ReportLayoutController : Controller
    {
        #region Declaration
        ReportLayout _oReportLayout = new ReportLayout();
        List<ReportLayout> _oReportLayouts = new List<ReportLayout>();
        string _sErrorMessage = "";
        #endregion

        public ActionResult ViewReportLayouts(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(EnumModuleName.ReportLayout.ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oReportLayouts = new List<ReportLayout>();
            _oReportLayouts = ReportLayout.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ReportTypes = EnumObject.jGets(typeof(EnumReportLayout));
            ViewBag.OperationTypes = EnumObject.jGets(typeof(EnumModuleName));
            return View(_oReportLayouts);
        }

        public ActionResult ViewReportLayout(int id, double ts)
        {
            _oReportLayout = new ReportLayout();
            if (id > 0)
            {
                _oReportLayout = _oReportLayout.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            //_oReportLayout.ReportTypes = ReportType.Gets();
            //_oReportLayout.OperationTypes = OperationType.Gets();
            return PartialView(_oReportLayout);
        }

        [HttpPost]
        public JsonResult GetReportLayout(ReportLayout oReportLayout)
        {
            _oReportLayout = new ReportLayout();
            try
            {
                _oReportLayout = _oReportLayout.Get(oReportLayout.ReportLayoutID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oReportLayout = new ReportLayout();
                _oReportLayout.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReportLayout);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(ReportLayout oReportLayout)
        {
            _oReportLayout = new ReportLayout();
            try
            {
                oReportLayout.ReportType = (EnumReportLayout)oReportLayout.ReportTypeInInt;
                oReportLayout.OperationType = (EnumModuleName)oReportLayout.OperationTypeInInt;
                _oReportLayout = oReportLayout.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oReportLayout = new ReportLayout();
                _oReportLayout.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oReportLayout);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ReportLayout oReportLayout)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oReportLayout.Delete(oReportLayout.ReportLayoutID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     

    }
    

}
