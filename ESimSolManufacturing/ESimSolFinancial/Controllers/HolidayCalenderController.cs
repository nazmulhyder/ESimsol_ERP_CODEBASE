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
using System.Data;
using System.Data.OleDb;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class HolidayCalendarController : Controller
    {
        public ActionResult ViewHolidayCalendars(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<HolidayCalendar> _oHolidayCalendar = new List<HolidayCalendar>();
            string sSQL = "SELECT * FROM View_HolidayCalendar  AS HH ORDER BY HolidayCalendarID ASC";
            _oHolidayCalendar = HolidayCalendar.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oHolidayCalendar);

        }

        public ActionResult ViewHolidayDefine(int id)
        {

            List<HolidayCalendarDetail> _oHolidayCalendarDetails = new List<HolidayCalendarDetail>();
            if (id > 0)
            {
                string SSQL = "SELECT * FROM VIEW_HolidayCalendarDetail AS HCD WHERE HCD.HolidayCalendarID=" + id;
                _oHolidayCalendarDetails = HolidayCalendarDetail.Gets(SSQL, (int)Session[SessionInfo.currentUserID]);
                foreach (HolidayCalendarDetail oItem in _oHolidayCalendarDetails)
                {
                    oItem.HolidayCalendarDRPs = HolidayCalendarDRP.Gets(oItem.HolidayCalendarDetailID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            ViewBag.ApplyFor = EnumObject.jGets(typeof(EnumCalendarApply));
            return View(_oHolidayCalendarDetails);
        }
    
        [HttpPost]
        public JsonResult Save(HolidayCalendar oHolidayCalendar)
        {
            HolidayCalendar _oHolidayCalendar = new HolidayCalendar();
            try
            {

                _oHolidayCalendar = oHolidayCalendar.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oHolidayCalendar = new HolidayCalendar();
                _oHolidayCalendar.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHolidayCalendar);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(HolidayCalendar oHolidayCalendar)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oHolidayCalendar.Delete(oHolidayCalendar.HolidayCalendarID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetsHoliday()
        {
            List<Holiday> oHolidays = new List<Holiday>();
            try
            {
                string sSql = "SELECT * FROM Holiday AS HC ORDER BY HC.Code, HC.Description ASC";
                oHolidays = Holiday.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                Holiday oHoliday = new Holiday();
                oHoliday.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oHolidays);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult HolidayDetailSave(HolidayCalendarDetail oHolidayCalendarDetail)
        {
            HolidayCalendarDetail _oHolidayCalendarDetail = new HolidayCalendarDetail();
           List< HolidayCalendarDRP> _oHolidayCalendarDRPs = new List<HolidayCalendarDRP>();
            try
            {
                _oHolidayCalendarDetail = oHolidayCalendarDetail.Save((int)Session[SessionInfo.currentUserID]);
                _oHolidayCalendarDRPs = HolidayCalendarDRP.Gets(_oHolidayCalendarDetail.HolidayCalendarDetailID, (int)Session[SessionInfo.currentUserID]);
                _oHolidayCalendarDetail.HolidayCalendarDRPs = _oHolidayCalendarDRPs;
            }
            catch (Exception ex)
            {
                _oHolidayCalendarDetail = new HolidayCalendarDetail();
                _oHolidayCalendarDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHolidayCalendarDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult HolidayDetailDelete(HolidayCalendarDetail oHolidayCalendarDetail)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oHolidayCalendarDetail.Delete(oHolidayCalendarDetail.HolidayCalendarDetailID, (int)Session[SessionInfo.currentUserID]);
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