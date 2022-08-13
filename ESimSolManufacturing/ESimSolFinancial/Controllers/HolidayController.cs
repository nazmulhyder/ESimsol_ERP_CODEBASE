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
    public class HolidayController : Controller
    {
        #region Declaration

        private Holiday _oHoliday = new Holiday();
        private List<Holiday> _oHolidays = new List<Holiday>();
        private string _sErrorMessage = "";

        #endregion

        public ActionResult ViewHolidays(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oHolidays = new List<Holiday>();
            _oHolidays = Holiday.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            //_oHolidays[0].oHolidayInfo = _oHoliday;
            return View(_oHolidays);
        }

        public ActionResult ViewHoliday(int id, double ts)
        {
            _oHoliday = new Holiday();
            if (id > 0)
            {
                _oHoliday = _oHoliday.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return PartialView(_oHoliday);
        }

        [HttpPost]
        public JsonResult Save(Holiday oHoliday)
        {
            _oHoliday = new Holiday();
            try
            {
                _oHoliday = oHoliday;
                _oHoliday.TypeOfHoliday = (EnumHolidayType)oHoliday.TypeOfHolidayInt;
                _oHoliday = _oHoliday.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oHoliday.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHoliday);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Holiday oHoliday)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oHoliday.Delete(oHoliday.HolidayID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PickHoliday(double ts)
        {
            _oHoliday = new Holiday();
            _oHoliday.oHolidays = Holiday.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            return PartialView(_oHoliday);
        }
        [HttpPost]
        public JsonResult ChangeActiveStatus(Holiday oHoliday)
        {
            _oHoliday = new Holiday();
            string sMsg;

            sMsg = _oHoliday.ChangeActiveStatus(oHoliday, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oHoliday = _oHoliday.Get(oHoliday.HolidayID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHoliday);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetHoliday(Holiday oHoliday)
        {
            _oHoliday = new Holiday();
            try
            {
                if (oHoliday.HolidayID <= 0) { throw new Exception("Please select a valid item."); }

                _oHoliday = _oHoliday.Get(oHoliday.HolidayID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
                if (_oHoliday.HolidayID <= 0)
                {
                    throw new Exception("No information found.");
                }

            }
            catch (Exception ex)
            {
                _oHoliday = new Holiday();
                _oHoliday.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHoliday);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsHoliday(Holiday oHoliday)
        {
            _oHolidays = new List<Holiday>();
            try
            {
                string sSQL = "SELECT * FROM Holiday Where IsActive=1 And HolidayID In(Select HolidayID from AttendanceCalendarSessionHoliday " +
                              "Where ACSID=(Select Max(ACSID) from AttendanceCalendarSession Where AttendanceCalendarID=" + oHoliday.AttendanceCalendarID + "))";
                _oHolidays = Holiday.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oHolidays = new List<Holiday>();
                _oHoliday = new Holiday();
                _oHoliday.ErrorMessage = ex.Message;
                _oHolidays.Add(_oHoliday);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHolidays);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(Holiday oHoliday)
        {
            _oHolidays = new List<Holiday>();
            try
            {
                string sSQL = "SELECT * FROM Holiday Where IsActive=1";
                _oHolidays = Holiday.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oHolidays = new List<Holiday>();
                _oHoliday = new Holiday();
                _oHoliday.ErrorMessage = ex.Message;
                _oHolidays.Add(_oHoliday);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHolidays);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

    }
}