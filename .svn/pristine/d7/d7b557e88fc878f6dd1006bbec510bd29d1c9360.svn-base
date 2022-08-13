using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;

namespace ESimSolFinancial.Controllers
{
    public class GeneralWorkingDayController : Controller
    {
        #region Declaration
        GeneralWorkingDay _oGeneralWorkingDay = new GeneralWorkingDay();
        List<GeneralWorkingDay> _oGeneralWorkingDays = new List<GeneralWorkingDay>();
        #endregion

        public ActionResult ViewGeneralWorkingDays(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AccountsBookSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oGeneralWorkingDays = new List<GeneralWorkingDay>();
            return View(_oGeneralWorkingDays);
        }

        public ActionResult ViewGeneralWorkingDay(int id)
        {
            GeneralWorkingDay _oGeneralWorkingDay = new GeneralWorkingDay();
            if (id > 0)
            {
                _oGeneralWorkingDay = _oGeneralWorkingDay.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oGeneralWorkingDay.GeneralWorkingDayDetails = GeneralWorkingDayDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oGeneralWorkingDay.GeneralWorkingDayShifts = GeneralWorkingDayShift.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.GWDApplyOns = EnumObject.jGets(typeof(EnumGWDApplyOn));
            return View(_oGeneralWorkingDay);
        }

        [HttpPost]
        public JsonResult Save(GeneralWorkingDay oGeneralWorkingDay)
        {
            _oGeneralWorkingDay = new GeneralWorkingDay();
            try
            {
                _oGeneralWorkingDay = oGeneralWorkingDay;
                _oGeneralWorkingDay = _oGeneralWorkingDay.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGeneralWorkingDay = new GeneralWorkingDay();
                _oGeneralWorkingDay.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGeneralWorkingDay);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(GeneralWorkingDay oGeneralWorkingDay)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oGeneralWorkingDay.Delete(oGeneralWorkingDay.GWDID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AdvSearch(string sParam)
        {
            DateTime dtStartDate = Convert.ToDateTime(sParam.Split('~')[0]);
            DateTime dtEndDate = Convert.ToDateTime(sParam.Split('~')[1]);

            List<GeneralWorkingDay> oGeneralWorkingDays = new List<GeneralWorkingDay>();            
            try
            {
                string sSQL = "SELECT * FROM View_GeneralWorkingDay AS HH WHERE CONVERT(DATE,CONVERT(VARCHAR(12),HH.AttendanceDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtEndDate.ToString("dd MMM yyyy") + "',106)) ORDER BY HH.AttendanceDate ASC";
                oGeneralWorkingDays = GeneralWorkingDay.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                GeneralWorkingDay oGeneralWorkingDay = new GeneralWorkingDay();
                oGeneralWorkingDay.ErrorMessage = ex.Message;
                oGeneralWorkingDays.Add(oGeneralWorkingDay);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGeneralWorkingDays);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        #region detail

        [HttpPost]
        public JsonResult GetsGeneralWorkingDayDetail()
        {
            List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
            try
            {
                string sSql = "SELECT * FROM View_DepartmentRequirementPolicy AS DRP ORDER BY DRP.BUName, DRP.Location, DRP.Department ASC";
                oDepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                DepartmentRequirementPolicy oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
                oDepartmentRequirementPolicy.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDepartmentRequirementPolicys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion detail

        #region shift
        [HttpPost]
        public JsonResult GetsGeneralWorkingDayShift()
        {
            List<HRMShift> oHRMShifts = new List<HRMShift>();
            try
            {
                string sSql = "SELECT * FROM HRM_Shift";
                oHRMShifts = HRMShift.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                HRMShift oHRMShift = new HRMShift();
                oHRMShift.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oHRMShifts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }

}

