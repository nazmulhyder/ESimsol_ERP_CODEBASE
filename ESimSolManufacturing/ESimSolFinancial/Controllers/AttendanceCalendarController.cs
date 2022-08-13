using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Web.Script.Serialization;


namespace ESimSolFinancial.Controllers
{
    public class AttendanceCalendarController : Controller
    {
        #region Declaration
        
        private AttendanceCalendarSessionHoliday _oACSH = new AttendanceCalendarSessionHoliday();
        private List<AttendanceCalendarSessionHoliday> _oACSHs = new List<AttendanceCalendarSessionHoliday>();
        private AttendanceCalendarSession  _oACS = new AttendanceCalendarSession();
        private List<AttendanceCalendarSession> _oACSList = new List<AttendanceCalendarSession>();
        private AttendanceCalendar _oAC = new AttendanceCalendar();
        private List<AttendanceCalendar> _oACList = new List<AttendanceCalendar>();
        private string _sErrorMessage = "";

        #endregion

        #region Attendance Calendar
        public ActionResult ViewAttendanceCalendars(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oACList = new List<AttendanceCalendar>();
            _oACList = AttendanceCalendar.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oACList);
        }

        [HttpPost]
        public JsonResult Save(AttendanceCalendar oAttendanceCalendar)
        {
            try
            {
                if (oAttendanceCalendar.AttendanceCalendarID <= 0)
                {
                    oAttendanceCalendar = oAttendanceCalendar.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oAttendanceCalendar = oAttendanceCalendar.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }

            }
            catch (Exception ex)
            {
                oAttendanceCalendar = new AttendanceCalendar();
                oAttendanceCalendar.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceCalendar);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(AttendanceCalendar oAttendanceCalendar)
        {
            try
            {
                if (oAttendanceCalendar.AttendanceCalendarID <= 0) { throw new Exception("Please select a valid item."); }
                oAttendanceCalendar = oAttendanceCalendar.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oAttendanceCalendar = new AttendanceCalendar();
                oAttendanceCalendar.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceCalendar.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetAttendanceCalendar(AttendanceCalendar oAttendanceCalendar)
        {
            try
            {
                if (oAttendanceCalendar.AttendanceCalendarID <= 0) { throw new Exception("Please select a valid item."); }
                oAttendanceCalendar = AttendanceCalendar.Get(oAttendanceCalendar.AttendanceCalendarID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oAttendanceCalendar = new AttendanceCalendar();
                oAttendanceCalendar.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceCalendar);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #endregion AttendanceCalendar

        #region AttendanceCalendarSession
        public ActionResult ViewAttendanceCalendarSessions(int nACID, double nts)
        {
            _oACSList = new List<AttendanceCalendarSession>();

            AttendanceCalendar oAttendanceCalendar = new AttendanceCalendar();
            oAttendanceCalendar = AttendanceCalendar.Get(nACID, (int)Session[SessionInfo.currentUserID]);

            if (oAttendanceCalendar.AttendanceCalendarID > 0)
            {
                _oACSList = AttendanceCalendarSession.Gets(nACID, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.AttendanceCalendar = oAttendanceCalendar;

            return View(_oACSList);
        }

        [HttpPost]
        public JsonResult SaveACS(AttendanceCalendarSession oAttendanceCalendarSession)
        {
            try
            {
                if (oAttendanceCalendarSession.ACSID <= 0)
                {
                    oAttendanceCalendarSession = oAttendanceCalendarSession.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oAttendanceCalendarSession = oAttendanceCalendarSession.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oAttendanceCalendarSession = new AttendanceCalendarSession();
                oAttendanceCalendarSession.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceCalendarSession);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteACS(AttendanceCalendarSession oAttendanceCalendarSession)
        {
            try
            {
                if (oAttendanceCalendarSession.ACSID <= 0) { throw new Exception("Please select a valid attendance claendar session."); }
                oAttendanceCalendarSession = oAttendanceCalendarSession.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oAttendanceCalendarSession = new AttendanceCalendarSession();
                oAttendanceCalendarSession.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceCalendarSession.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteACSHoliday(AttendanceCalendarSessionHoliday oAttendanceCalendarSessionHoliday)
        {
            try
            {
                if (oAttendanceCalendarSessionHoliday.ACSHID <= 0) { throw new Exception("Please select a valid holiday."); }
                oAttendanceCalendarSessionHoliday = oAttendanceCalendarSessionHoliday.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oAttendanceCalendarSessionHoliday = new AttendanceCalendarSessionHoliday();
                oAttendanceCalendarSessionHoliday.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceCalendarSessionHoliday.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ChangeActiveStatus(AttendanceCalendarSession oACS)
        {
            _oACS = new AttendanceCalendarSession();
            _oACSList = new List<AttendanceCalendarSession>();

            _oACSList = _oACS.ChangeActiveStatus(oACS, (int)Session[SessionInfo.currentUserID]);            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oACSList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAttendanceCalendarSession(AttendanceCalendarSession oAttendanceCalendarSession)
        {
            try
            {
                if (oAttendanceCalendarSession.ACSID <= 0) { throw new Exception("Please select a valid item."); }
                oAttendanceCalendarSession = AttendanceCalendarSession.Get(oAttendanceCalendarSession.ACSID, (int)Session[SessionInfo.currentUserID]);
                if (oAttendanceCalendarSession.ACSID > 0)
                {
                    oAttendanceCalendarSession.AttendanceCalendarSessionHolidays = AttendanceCalendarSessionHoliday.Gets(oAttendanceCalendarSession.ACSID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oAttendanceCalendarSession = new AttendanceCalendarSession();
                oAttendanceCalendarSession.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceCalendarSession);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsACSHoliday(AttendanceCalendar oAttendanceCalendar)
        {
            List<AttendanceCalendarSessionHoliday> oACSHs = new List<AttendanceCalendarSessionHoliday>();
            try
            {
                string sSQL = "Select * from View_AttendanceCalendarSessionHoliday Where ACSID=(Select Max(ACSID) from AttendanceCalendarSession Where IsActive=1 And AttendanceCalendarID=" + oAttendanceCalendar.AttendanceCalendarID + ")";
               
                oACSHs = AttendanceCalendarSessionHoliday.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (oACSHs.Count() <= 0) { throw new Exception("No attendance session found."); }
            }
            catch (Exception ex)
            {
                oACSHs = new List<AttendanceCalendarSessionHoliday>();
                AttendanceCalendarSessionHoliday oACSH = new AttendanceCalendarSessionHoliday();
                oACSH.ErrorMessage = ex.Message;
                oACSHs.Add(oACSH);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oACSHs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      

        #endregion AttendanceCalendarSession

        [HttpPost]
        public JsonResult GetACSList()
        {
            List<AttendanceCalendarSession> oACSs = new List<AttendanceCalendarSession>();
            string sSQL = "SELECT * FROM AttendanceCalendarSession";
            oACSs = AttendanceCalendarSession.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oACSs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
    }
}