using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class AttendanceSchemeController : Controller
    {



        #region Declartion
        AttendanceScheme _oAttendanceScheme = new AttendanceScheme();
        List<AttendanceScheme> _oAttendanceSchemes = new List<AttendanceScheme>();
        #endregion

        #region Old Version

        [HttpPost]
        public JsonResult AttendanceScheme_IU(AttendanceScheme oAttendanceScheme)// DRP Insert
        {
            _oAttendanceScheme = new AttendanceScheme();
            try
            {
                _oAttendanceScheme = oAttendanceScheme;
                if (_oAttendanceScheme.AttendanceSchemeID <= 0)
                {
                    _oAttendanceScheme = _oAttendanceScheme.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oAttendanceScheme = _oAttendanceScheme.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oAttendanceScheme = new AttendanceScheme();
                _oAttendanceScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AttendanceSchemeDelete(AttendanceScheme oAttendanceScheme)//Department Requirement Policy Delete
        {
            try
            {
                if (oAttendanceScheme.AttendanceSchemeID <= 0)
                {
                    throw new Exception("Please select a valid attendance scheme from list.");
                }
                oAttendanceScheme = oAttendanceScheme.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oAttendanceScheme = new AttendanceScheme();
                oAttendanceScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceScheme.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AttendanceSchemeSearch()
        {
            _oAttendanceScheme = new AttendanceScheme();
            _oAttendanceScheme.RosterPlans = RosterPlan.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            //_oAttendanceSchemes = AttendanceScheme.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceScheme.AttendanceCalendars = AttendanceCalendar.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            //_oAttendanceScheme.DepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            //_oAttendanceScheme.AttendanceSchemes = _oAttendanceSchemes;
            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return PartialView(_oAttendanceScheme);
        }

        [HttpPost]
        public JsonResult AttendanceSchemeSearch(string sParam)
        {
            List<AttendanceScheme> oAS = new List<AttendanceScheme>();
            string sSchemeName = Convert.ToString(sParam.Split('~')[0]);

            int nRosterPlanID = Convert.ToInt32(sParam.Split('~')[1]);
            int nAttendanceCalenderID = Convert.ToInt32(sParam.Split('~')[2]);

            string sSQL = "SELECT * FROM View_AttendanceScheme WHERE AttendanceSchemeID <>0";
            if (sSchemeName != " ")
            {
                sSQL = sSQL + " AND Name like'" + "%" + sSchemeName + "%" + "'";
            }
            if (nRosterPlanID > 0)
            {
                sSQL = sSQL + " AND RosterPlanID=" + nRosterPlanID + "";
            }

            if (nAttendanceCalenderID > 0)
            {
                sSQL = sSQL + " AND AttendanceCalenderID=" + nAttendanceCalenderID + "";
            }
            try
            {
                oAS = AttendanceScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oAttendanceScheme = new AttendanceScheme();
                _oAttendanceScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Attendance Scheme Holiday
        [HttpPost]
        public JsonResult ASH_Delete(AttendanceSchemeHoliday oASH)// ACH Delete
        {
            AttendanceSchemeHoliday oAttendanceSchemeHoliday = new AttendanceSchemeHoliday();
            try
            {
                oAttendanceSchemeHoliday = oASH.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oAttendanceSchemeHoliday = new AttendanceSchemeHoliday();
                oAttendanceSchemeHoliday.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceSchemeHoliday.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSchemeHolidays(int nASID)// AttendanceScheme.AttendanceSchemeID
        {
            List<AttendanceSchemeHoliday> oACHs = new List<AttendanceSchemeHoliday>();
            string sSql = "";
            if (nASID > 0)
            {
                sSql = "SELECT * FROm View_AttendanceSchemeHoliDay WHERE AttendanceSchemeID=" + nASID;
                oACHs = AttendanceSchemeHoliday.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oACHs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Attendance Scheme Leave
        [HttpPost]
        public JsonResult ASL_Delete(AttendanceSchemeLeave oASL)// ACL Delete
        {
            AttendanceSchemeLeave oAttendanceSchemeLeave = new AttendanceSchemeLeave();
            try
            {
                oAttendanceSchemeLeave = oASL.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oAttendanceSchemeLeave = new AttendanceSchemeLeave();
                oAttendanceSchemeLeave.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceSchemeLeave.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSchemeLeavs(int nASID)// AttendanceScheme.AttendanceSchemeID
        {
            List<AttendanceSchemeLeave> oASLs = new List<AttendanceSchemeLeave>();
            string sSql = "";
            if (nASID > 0)
            {
                sSql = "SELECT * FROm View_AttendanceSchemeLeave WHERE AttendanceSchemeID=" + nASID;
                oASLs = AttendanceSchemeLeave.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oASLs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Copy
        [HttpPost]
        public JsonResult AttendanceScheme_Copy(AttendanceScheme oAttendanceScheme)//Attendance Scheme ID
        {
            _oAttendanceScheme = new AttendanceScheme();
            List<AttendanceSchemeLeave> oASLs = new List<AttendanceSchemeLeave>();
            List<AttendanceSchemeHoliday> oASHs = new List<AttendanceSchemeHoliday>();
            List<AttendanceSchemeDayOff> oASDs = new List<AttendanceSchemeDayOff>();
            string sSql = "";
            try
            {
                _oAttendanceScheme = _oAttendanceScheme.Get(oAttendanceScheme.AttendanceSchemeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                sSql = "select * from View_AttendanceSchemeLeave where AttendanceSchemeID=" +_oAttendanceScheme.AttendanceSchemeID;
                oASLs = AttendanceSchemeLeave.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                foreach (AttendanceSchemeLeave oItem in oASLs)
                {
                    oItem.AttendanceSchemeLeaveID = 0;
                    oItem.AttendanceSchemeID = 0;
                    _oAttendanceScheme.AttendanceSchemeLeaves.Add(oItem);
                }
                sSql = "select * from View_AttendanceSchemeHoliDay where AttendanceSchemeID=" + _oAttendanceScheme.AttendanceSchemeID;
                oASHs = AttendanceSchemeHoliday.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                foreach (AttendanceSchemeHoliday oItem in oASHs)
                {
                    oItem.AttendanceSchemeHolidayID = 0;
                    oItem.AttendanceSchemeID = 0;
                    _oAttendanceScheme.AttendanceSchemeHolidays.Add(oItem);
                }
                sSql = "select * from AttendanceSchemeDayOff where AttendanceSchemeID=" + _oAttendanceScheme.AttendanceSchemeID;
                oASDs = AttendanceSchemeDayOff.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                foreach (AttendanceSchemeDayOff oItem in oASDs)
                {
                    oItem.AttendanceSchemeDayOffID = 0;
                    oItem.AttendanceSchemeID = 0;
                    _oAttendanceScheme.AttendanceSchemeDayOffs.Add(oItem);
                }
            }
            catch (Exception ex)
            {
                _oAttendanceScheme = new AttendanceScheme();
                _oAttendanceScheme.ErrorMessage = ex.Message;
            }
            //_oAttendanceScheme.AttendanceCalendars = AttendanceCalendar.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            //sSql = "select * from View_RosterPlanDetail";
            //_oAttendanceScheme.RosterPlanDetails = RosterPlanDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //_oAttendanceScheme.RosterPlans = RosterPlan.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult AttendanceSchemeSearchByName(string sASName, double nts)
        {
            _oAttendanceSchemes = new List<AttendanceScheme>();
            _oAttendanceScheme = new AttendanceScheme();
            try
            {
                string sSql = "";
                if (sASName == "")
                {
                    sSql = "SELECT * FROM View_AttendanceScheme";
                }
                else
                {
                    sSql = "SELECT * FROM View_AttendanceScheme WHERE Name LIKE '%" + sASName + "%' ";
                }
                _oAttendanceSchemes = AttendanceScheme.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oAttendanceSchemes.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oAttendanceSchemes = new List<AttendanceScheme>();
                _oAttendanceScheme.ErrorMessage = ex.Message;
                _oAttendanceSchemes.Add(_oAttendanceScheme);
            }
            return PartialView(_oAttendanceSchemes);
        }

        #region Att. Scheme Search for Emp Basic
        [HttpPost]
        public JsonResult AttSchemePick(string sASName)
        {

            _oAttendanceSchemes = new List<AttendanceScheme>();
            _oAttendanceScheme = new AttendanceScheme();
            try
            {
                string sSql = "";
                if (sASName == "")
                {
                    sSql = "SELECT * FROM View_AttendanceScheme";
                }
                else
                {
                    sSql = "SELECT * FROM View_AttendanceScheme WHERE Name LIKE '%" + sASName + "%' ";
                }
                _oAttendanceSchemes = AttendanceScheme.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oAttendanceSchemes.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oAttendanceSchemes = new List<AttendanceScheme>();
                _oAttendanceScheme.ErrorMessage = ex.Message;
                _oAttendanceSchemes.Add(_oAttendanceScheme);

            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceSchemes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Att. Search Search for Emp Basic
        #endregion

        #region New Version

        

        public ActionResult View_AttendanceSchemes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oAttendanceSchemes = new List<AttendanceScheme>();
            _oAttendanceSchemes = AttendanceScheme.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.AttendanceCalendars = AttendanceCalendar.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.RosterPlans = RosterPlan.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);


            return View(_oAttendanceSchemes);
        }

        public ActionResult View_AttendanceScheme(int id)//Attendance Scheme ID
        {
            _oAttendanceScheme = new AttendanceScheme();
            string sSql = "";
            if (id > 0)
            {
                _oAttendanceScheme = _oAttendanceScheme.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                sSql = "select * from View_AttendanceSchemeLeave where AttendanceSchemeID=" +_oAttendanceScheme.AttendanceSchemeID;
                _oAttendanceScheme.AttendanceSchemeLeaves = AttendanceSchemeLeave.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sSql = "select * from View_AttendanceSchemeHoliDay where AttendanceSchemeID=" + _oAttendanceScheme.AttendanceSchemeID;
                _oAttendanceScheme.AttendanceSchemeHolidays = AttendanceSchemeHoliday.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                sSql = "select * from AttendanceSchemeDayOff where AttendanceSchemeID=" + _oAttendanceScheme.AttendanceSchemeID;
                _oAttendanceScheme.AttendanceSchemeDayOffs = AttendanceSchemeDayOff.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                _oAttendanceScheme.AttendanceCalendars = AttendanceCalendar.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oAttendanceScheme.RosterPlans = RosterPlan.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
                sSql = "Select * from View_RosterPlanDetail";
                _oAttendanceScheme.RosterPlanDetails = RosterPlanDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            else
            {
                //sSql = "Select * FROM AttendanceCalendar Where IsActive=1  And AttendanceCalendarID In (Select AttendanceCalendarID from AttendanceCalendarSession Where ACSID In (Select Distinct(ACSID) from AttendanceCalendarSessionHoliday Where IsActive=1 And ACSID In (Select Max(ACSID) from AttendanceCalendarSession Where IsActive=1 And AttendanceCalendarID In (SELECT AttendanceCalendarID FROM AttendanceCalendar Where IsActive=1 ) Group By AttendanceCalendarID)))";
                sSql = "SELECT * FROM AttendanceCalendar WHERE IsActive=1 AND AttendanceCalendarID IN ("
                      + "  SELECT AttendanceCalendarID FROM AttendanceCalendarSession WHERE IsActive=1 AND ACSID IN("
                      + "  SELECT ACSID FROM AttendanceCalendarSessionHoliday WHERE IsActive=1))";

                _oAttendanceScheme.AttendanceCalendars = AttendanceCalendar.Gets(sSql,((User)(Session[SessionInfo.CurrentUser])).UserID);

                sSql = "Select * from View_RosterPlan Where IsActive=1 And RosterPlanID In (Select Distinct(RosterPlanID) from RosterPlanDetail)";
                _oAttendanceScheme.RosterPlans = RosterPlan.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                sSql = "Select * from View_RosterPlanDetail Where RosterPlanID In (Select RosterPlanID from RosterPlan Where IsActive=1)";
                _oAttendanceScheme.RosterPlanDetails = RosterPlanDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }

            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            sSql = "Select * from LeaveHead Where IsActive=1";
            ViewBag.LeaveHeads = LeaveHead.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.PaymentCycles = Enum.GetValues(typeof(EnumPaymentCycle)).Cast<EnumPaymentCycle>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.RecruitmentEvents = Enum.GetValues(typeof(EnumRecruitmentEvent)).Cast<EnumRecruitmentEvent>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.DayOffType = Enum.GetValues(typeof(EnumDayOffType)).Cast<EnumDayOffType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            return View(_oAttendanceScheme);
        }

        [HttpPost]
        public JsonResult GetsAttendanceScheme(AttendanceScheme oAttendanceScheme)
        {
            List<AttendanceScheme> oAttendanceSchemes= new List<AttendanceScheme>();
            string sSchemeName = Convert.ToString(oAttendanceScheme.Params.Split('~')[0]);
            int nAttendanceCalenderID = Convert.ToInt32(oAttendanceScheme.Params.Split('~')[1]);
            int nRosterPlanID = Convert.ToInt32(oAttendanceScheme.Params.Split('~')[2]);
            
            string sSQL = "SELECT * FROM View_AttendanceScheme WHERE AttendanceSchemeID <>0";
            if (sSchemeName != " ")
            {
                sSQL = sSQL + " AND Name like'" + "%" + sSchemeName + "%" + "'";
            }
            if (nRosterPlanID > 0)
            {
                sSQL = sSQL + " AND RosterPlanID=" + nRosterPlanID + "";
            }
            if (nAttendanceCalenderID > 0)
            {
                sSQL = sSQL + " AND AttendanceCalenderID=" + nAttendanceCalenderID + "";
            }
            try
            {
                oAttendanceSchemes = AttendanceScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oAttendanceScheme = new AttendanceScheme();
                _oAttendanceScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceSchemes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsAttendanceSchemeCurrentSession(AttendanceScheme oAttendanceScheme)
        {
            List<AttendanceScheme> oAttendanceSchemes = new List<AttendanceScheme>();
            try
            {
                string sSQL = "Select * from View_AttendanceScheme Where AttendanceCalenderID=(Select max(AttendanceCalendarID) from AttendanceCalendar)";
                oAttendanceSchemes = AttendanceScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if(oAttendanceSchemes.Count<=0)
                {
                    throw new Exception("There is no Attendance Scheme!");
                }
            }
            catch (Exception ex)
            {
                _oAttendanceScheme = new AttendanceScheme();
                _oAttendanceScheme.ErrorMessage = ex.Message;
                oAttendanceSchemes = new List<AttendanceScheme>();
                oAttendanceSchemes.Add(_oAttendanceScheme);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceSchemes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AttendanceSchemeActiveInActive(AttendanceScheme oAttendanceScheme)
        {
            try
            {
                if (oAttendanceScheme.AttendanceSchemeID <= 0) { throw new Exception ("Please select a valid attendance scheme."); }
                oAttendanceScheme.IsActive = !oAttendanceScheme.IsActive;
                oAttendanceScheme = oAttendanceScheme.ActiveInActive(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oAttendanceScheme = new AttendanceScheme();
                oAttendanceScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


         [HttpPost]
        public JsonResult DeleteAttendanceSchemeDayOff(AttendanceSchemeDayOff oASDO)
        {
            try
            {
                if (oASDO.AttendanceSchemeDayOffID <= 0) { throw new Exception("Please select a valid attendance scheme."); }
                oASDO = oASDO.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oASDO = new AttendanceSchemeDayOff();
                oASDO.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oASDO.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        

        #endregion

    }
}
