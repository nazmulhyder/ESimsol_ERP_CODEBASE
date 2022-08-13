using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using iTextSharp.text;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using ESimSol.Services.DataAccess;


namespace ESimSolFinancial.Controllers
{
    public class ComplianceAttendanceDailyController : PdfViewController
    {
        #region Declaration
        AttendanceDaily _oAttendanceDaily = new AttendanceDaily();
        List<AttendanceDaily> _oAttendanceDailys = new List<AttendanceDaily>();
        #endregion

        #region Views
        public ActionResult View_ComplianceAttendanceDaily(int nLayoutType, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oAttendanceDailys = new List<AttendanceDaily>();

            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            ViewBag.LayoutType = nLayoutType;//1=Maternity, 2=All

            return View(_oAttendanceDailys);
        }
        public ActionResult View_ComplianceAttendanceDaily_UpdateAttendance(string sParams)
        {
            DateTime dtDateFrom = Convert.ToDateTime(sParams.Split('~')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sParams.Split('~')[1]);
            int nEmployeeID = Convert.ToInt32(sParams.Split('~')[2]);

            _oAttendanceDailys = new List<AttendanceDaily>();

            string sSql = "SELECT * FROM View_AttendanceDaily WHERE EmployeeID = " + nEmployeeID + " AND CONVERT(DATE,CONVERT(VARCHAR(12),AttendanceDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDateFrom.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dtDateTo.ToString("dd MMM yyyy") + "',106))";            
            _oAttendanceDailys = AttendanceDaily.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            _oAttendanceDailys = _oAttendanceDailys.OrderBy(x => x.AttendanceDate).ToList();

            return View(_oAttendanceDailys);
        }

        #endregion

        #region IUD
        [HttpPost]
        public JsonResult AttendanceDaily_IU(AttendanceDaily oAttendanceDaily)
        {
            _oAttendanceDaily = new AttendanceDaily();
            try
            {
                _oAttendanceDaily = oAttendanceDaily;
                _oAttendanceDaily.WorkingStatus = (EnumEmployeeWorkigStatus)oAttendanceDaily.WorkingStatusInt;
                if (_oAttendanceDaily.AttendanceID > 0)
                {
                    _oAttendanceDaily = _oAttendanceDaily.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oAttendanceDaily = _oAttendanceDaily.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oAttendanceDaily = new AttendanceDaily();
                _oAttendanceDaily.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceDaily);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AttendanceDaily_Delete(int id)
        {
            _oAttendanceDaily = new AttendanceDaily();
            try
            {

                _oAttendanceDaily.AttendanceID = id;
                _oAttendanceDaily = _oAttendanceDaily.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oAttendanceDaily = new AttendanceDaily();
                _oAttendanceDaily.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceDaily.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get
        [HttpGet]
        public JsonResult Gets(string sTemp, double ts)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            try
            {
                string sSql = "SELECT * from View_AttendanceDaily where EmployeeID IN (" + sTemp + ")";
                oAttendanceDailys = AttendanceDaily.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oAttendanceDaily = new AttendanceDaily();
                _oAttendanceDaily.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get(int nEmpID, string sDate, double ts)
        {
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            try
            {
                string sSql = "SELECT top(1)* FROM View_AttendanceDaily WHERE EmployeeID=" + nEmpID + " AND AttendanceDate='" + sDate + "'";
                oAttendanceDaily = AttendanceDaily.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDaily.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDaily);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Search
        [HttpPost]
        public JsonResult MaternityFollowUpSearch(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            try
            {
                oAttendanceDailys = AttendanceDaily.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, "", sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, "", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oAttendanceDailys.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDailys = new List<AttendanceDaily>();
                oAttendanceDaily.ErrorMessage = ex.Message;
                oAttendanceDailys.Add(oAttendanceDaily);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public JsonResult Update_AttendanceDaily_Manual_Single(AttendanceDaily oAttendanceDaily)
        {
            AttendanceDaily oAtt = new AttendanceDaily();
            string sFeedBackMessage = "";

            DateTime dtStartDate = Convert.ToDateTime(oAttendanceDaily.Remark.Split('~')[0]);
            DateTime dtEndDate = Convert.ToDateTime(oAttendanceDaily.Remark.Split('~')[1]);
            int nEmployeeID = Convert.ToInt32(oAttendanceDaily.Remark.Split('~')[2]);
            int nBufferTime = Convert.ToInt32(oAttendanceDaily.Remark.Split('~')[3]);
            bool bIsOverTime = Convert.ToBoolean(oAttendanceDaily.Remark.Split('~')[4]);

            //Random rnd = new Random();

            //string sSql = "SELECT * FROM View_AttendanceDaily WHERE EmployeeID = " + nEmployeeID + " AND AttendanceDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy") + "' AND '" + dtEndDate + "'";
            //_oAttendanceDailys = AttendanceDaily.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            //_oAttendanceDailys = _oAttendanceDailys.OrderBy(x => x.AttendanceDate).ToList();

            try
            {
                oAtt = oAttendanceDaily.Update_AttendanceDaily_Manual_Single(dtStartDate, dtEndDate, nEmployeeID, nBufferTime, bIsOverTime, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (string.IsNullOrEmpty(oAtt.ErrorMessage))
                {
                    sFeedBackMessage = "successful";
                }
            }
            catch (Exception ex)
            {
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDaily.ErrorMessage = ex.Message;
                sFeedBackMessage = "unsuccessful";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Update_AttendanceDaily_Manual_All(AttendanceDaily oAttendanceDaily)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            string sFeedBackMessage = "";
            try
            {
                oAttendanceDailys = oAttendanceDaily.Update_AttendanceDaily_Manual_All(((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oAttendanceDailys.Count <= 0)
                {
                    sFeedBackMessage = "successful";
                }
            }
            catch (Exception ex)
            {
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDaily.ErrorMessage = ex.Message;
                sFeedBackMessage = "unsuccessful";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
