using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web.Script.Serialization;


namespace ESimSolFinancial.Controllers
{
    public class AttendanceAccessPointController : Controller
    {
        #region Declaration
        AttendanceAccessPoint _oAAP = new AttendanceAccessPoint();
        List<AttendanceAccessPoint> _oAAPs = new List<AttendanceAccessPoint>();
        #endregion

        #region New Task
        public ActionResult ViewAttendanceAccessPoints(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oAAPs = new List<AttendanceAccessPoint>();
            string sSQL = "Select * FROM AttendanceAccessPoint Where AAPID<>0";
            _oAAPs = AttendanceAccessPoint.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DataProviders = Enum.GetValues(typeof(EnumDataProvider)).Cast<EnumDataProvider>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oAAPs);
        }

        public ActionResult ViewAttendanceAccessPoint(int? nID)
        {
            _oAAP = new AttendanceAccessPoint();
            //string sSQL = "";
            if (nID != null && nID > 0)
            {
                _oAAP = AttendanceAccessPoint.Get((int)nID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //if (_oAAP.AAPID > 0)
                //{
                //    sSQL = "Select * from View_AttendanceAccessPointEmployee Where AAPID=" + _oAAP.AAPID + "";
                //    _oAAP.AAPEs = AttendanceAccessPointEmployee.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //}
            }

            ViewBag.DataProviders = Enum.GetValues(typeof(EnumDataProvider)).Cast<EnumDataProvider>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oAAP);
        }


        [HttpPost]
        public JsonResult SaveAAP(AttendanceAccessPoint oAAP)
        {
            try
            {
                if (oAAP.AAPID <= 0)
                {
                    oAAP = oAAP.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oAAP = oAAP.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oAAP = new AttendanceAccessPoint();
                oAAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAAP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAAP(AttendanceAccessPoint oAAP)
        {
            try
            {
                oAAP = oAAP.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oAAP = new AttendanceAccessPoint();
                oAAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAAP.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActicationAAP(AttendanceAccessPoint oAAP)
        {
            try
            {
                oAAP.IsActive = !oAAP.IsActive;
                oAAP = oAAP.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oAAP = new AttendanceAccessPoint();
                oAAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAAP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult SaveAAPEmp(AttendanceAccessPointEmployee oAAPDetail)
        {
            try
            {
                if (oAAPDetail.AAPEmployeeID <= 0)
                {
                    oAAPDetail = oAAPDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oAAPDetail = oAAPDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oAAPDetail = new AttendanceAccessPointEmployee();
                oAAPDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAAPDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAAPEmp(AttendanceAccessPointEmployee oAAPDetail)
        {
            try
            {
                oAAPDetail = oAAPDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oAAPDetail = new AttendanceAccessPointEmployee();
                oAAPDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAAPDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ActivityChangeAAPEmp(AttendanceAccessPointEmployee oAAPDetail)
        {
            try
            {
                oAAPDetail.IsActive = !oAAPDetail.IsActive;
                oAAPDetail = oAAPDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oAAPDetail = new AttendanceAccessPointEmployee();
                oAAPDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAAPDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Get(AttendanceAccessPoint oAAP)
        {
            try
            {
                if (oAAP.AAPID <= 0) { throw new Exception("Invalid Attendance Access Point Item."); }
                oAAP = AttendanceAccessPoint.Get(oAAP.AAPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oAAP.AAPID > 0)
                {
                    string sSQL = "Select * from View_AttendanceAccessPointEmployee Where AAPID=" + oAAP.AAPID + "";
                    oAAP.AAPEs = AttendanceAccessPointEmployee.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oAAP = new AttendanceAccessPoint();
                oAAP.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAAP);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(AttendanceAccessPoint oAAP)
        {
            List<AttendanceAccessPoint> oAAPs = new List<AttendanceAccessPoint>();
            try
            {
                string sName = oAAP.Params.Split('~')[0].Trim();
                string sMachineSLNo = oAAP.Params.Split('~')[1].Trim();
                int nDataProvider = Convert.ToInt16(oAAP.Params.Split('~')[2]);

                string sSQL = "Select * from AttendanceAccessPoint Where AAPID<>0 ";

                if (sName != "")
                {
                    sSQL = sSQL + " And Name Like '%" + sName + "%'";
                }
                if (sMachineSLNo != "")
                {
                    sSQL = sSQL + " And MachineSLNo Like '%" + sMachineSLNo + "%'";
                }
                if (nDataProvider>0)
                {
                    sSQL = sSQL + " And DataProvider =" + nDataProvider + "";
                }
                oAAPs = AttendanceAccessPoint.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oAAP = new AttendanceAccessPoint();
                oAAP.ErrorMessage = ex.Message;
                oAAPs = new List<AttendanceAccessPoint>();
                oAAPs.Add(oAAP);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAAPs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region gets with pagination
        [HttpPost]
        public JsonResult AAPESearch(string sParam)
        {
            List<AttendanceAccessPointEmployee> oAAPEs = new List<AttendanceAccessPointEmployee>();
            int nAAPID = Convert.ToInt32(sParam.Split('~')[0]);
            int nRowLength = Convert.ToInt32(sParam.Split('~')[1]);
            int nLoadRecord = Convert.ToInt32(sParam.Split('~')[2]);

            string sSQL = "SELECT top(" + nLoadRecord + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeCode) Row,* FROM View_AttendanceAccessPointEmployee WHERE EmployeeID <>0";
            if (nAAPID>0)
            {
                sSQL = sSQL + " AND AAPID=" + nAAPID;
            }
            sSQL = sSQL + ") aa WHERE Row >" + nRowLength + " Order By EmployeeCode";
   
            try
            {
                oAAPEs = AttendanceAccessPointEmployee.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oAAPEs = new List<AttendanceAccessPointEmployee>();
                AttendanceAccessPointEmployee oAAPE = new AttendanceAccessPointEmployee();
                oAAPE.ErrorMessage = ex.Message;
                oAAPEs.Add(oAAPE);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAAPEs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
