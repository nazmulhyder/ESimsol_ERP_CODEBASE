using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class ProcessManagementPermissionController : Controller
    {
        #region Declaration
        ProcessManagementPermission _oProcessManagementPermission;
        private List<ProcessManagementPermission> _oProcessManagementPermissions;
        #endregion

        public ActionResult ViewProcessManagementPermissions(int nid, double ts)//nid=UserID
        {
            string sSQL = "";
            _oProcessManagementPermissions = new List<ProcessManagementPermission>();
            sSQL = "select * from View_ProcessManagementPermission WHERE UserID=" + nid;
            _oProcessManagementPermissions = ProcessManagementPermission.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oProcessManagementPermissions);
        }

        public ActionResult ViewProcessManagementPermission(int nid, double ts, int UID)//nid=PMPID & UID=UserID
        {

            _oProcessManagementPermission = new ProcessManagementPermission();
            if (nid > 0)
            {
                _oProcessManagementPermission = ProcessManagementPermission.Get(nid, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            else { _oProcessManagementPermission.UserID = UID; }

            return PartialView(_oProcessManagementPermission);
        }

        #region IUD
        [HttpPost]
        public JsonResult ProcessManagementPermission_IU(ProcessManagementPermission oProcessManagementPermission)
        {
            _oProcessManagementPermission = new ProcessManagementPermission();
            try
            {
                _oProcessManagementPermission = oProcessManagementPermission;
                _oProcessManagementPermission.ProcessManagementType = (EnumProcessManagementType)oProcessManagementPermission.ProcessManagementTypeInt;
                _oProcessManagementPermission.ProcessType = (EnumProcessType)oProcessManagementPermission.ProcessTypeInt;
                _oProcessManagementPermission.ProcessStatus = (EnumProcessStatus)oProcessManagementPermission.ProcessStatusInt;
                _oProcessManagementPermission.CompanyID = ((User)Session[SessionInfo.CurrentUser]).CompanyID;
                if (_oProcessManagementPermission.PMPID > 0)
                {
                    _oProcessManagementPermission = _oProcessManagementPermission.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oProcessManagementPermission = _oProcessManagementPermission.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oProcessManagementPermission = new ProcessManagementPermission();
                _oProcessManagementPermission.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProcessManagementPermission);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ProcessManagementPermission_Delete(int nid)//nid=PMPID
        {
            _oProcessManagementPermission = new ProcessManagementPermission();
            try
            {

                _oProcessManagementPermission.PMPID = nid;
                _oProcessManagementPermission = _oProcessManagementPermission.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oProcessManagementPermission = new ProcessManagementPermission();
                _oProcessManagementPermission.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProcessManagementPermission.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProcessManagementPermission_Activity(ProcessManagementPermission oProcessManagementPermission)//nid=PMPID
        {
            _oProcessManagementPermission = new ProcessManagementPermission();
            try
            {

                _oProcessManagementPermission = oProcessManagementPermission;
                _oProcessManagementPermission = ProcessManagementPermission.Activite(_oProcessManagementPermission.PMPID, _oProcessManagementPermission.IsActive, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oProcessManagementPermission = new ProcessManagementPermission();
                _oProcessManagementPermission.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProcessManagementPermission);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }

}
