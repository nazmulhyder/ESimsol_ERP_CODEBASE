using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ReportManagement;
using System.Reflection;

namespace ESimSolFinancial.Controllers
{
    public class AttendanceReportController : Controller
    {
        #region Declaration
        AttendanceMonitoring _oAttendanceMonitoring;
        private List<AttendanceMonitoring> _oAttendanceMonitorings;
        #endregion

        #region Views
        public ActionResult View_AttendanceMonitorings(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oAttendanceMonitorings = new List<AttendanceMonitoring>();

            string sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

            return View(_oAttendanceMonitorings);
        }

        #endregion

        //[HttpGet]
        //public JsonResult AttendanceMonitoringSearch(int nLocationID, string sDepartmentIDs, int nShiftID, string sDate, double nts)
        //{
        //    _oAttendanceMonitorings = new List<AttendanceMonitoring>();
        //    string sSql = "";
        //    //sSql="SELECT * FROM (SELECT DRPD.DepartmentID,(SELECT Name From Department WHERE DepartmentID=DRPD.DepartmentID) AS Department"+

        //    //",DRPD.DesignationID, DRPD.Designation, DRPD.Shift, DRPD.RequiredPerson, DRPD.ShiftID"+

        //    //",(SELECT LocationID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID=DRPD.DepartmentRequirementPolicyID) AS LocationID"+

        //    //",(SELECT COUNT(EmployeeID) FROM View_EmployeeOfficial WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1) AS ExistPerson"+

        //    //",(SELECT Gender FROM View_Employee WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1) AS Gender"+

        //    //",(SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='"+sDate+"' AND InTime!='"+sDate+" 00:00:000') AS Present" +

        //    //",(SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='"+sDate+"' AND InTime='"+sDate+" 00:00:000' AND OutTime='"+sDate+" 00:00:000') AS [Absent]"+

        //    //",(SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='"+sDate+"' AND IsDayOff=1) AS [DayOff]" +

        //    //",(SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='"+sDate+"' AND IsLeave=1) AS [Leave]" +

        //    //" FROM View_DepartmentRequirementDesignation AS DRPD) aa WHERE DepartmentID IN (" + sDepartmentIDs + ")";

        //    sSql = "SELECT * FROM (SELECT DRPD.DepartmentID,(SELECT Name From Department WHERE DepartmentID=DRPD.DepartmentID) AS Department"+

        //           ", DRPD.DesignationID , DRPD.Designation , DRPD.Shift , DRPD.RequiredPerson, DRPD.ShiftID"+

        //           ", (SELECT LocationID  FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID=DRPD.DepartmentRequirementPolicyID) AS LocationID"+
        //           ", (SELECT COUNT(EmployeeID) FROM View_EmployeeOfficial WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS ExistPerson"+
        //           ", (SELECT COUNT(EmployeeID) FROM View_EmployeeOfficial WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS MaleExistPerson"+
        //           ", (SELECT COUNT(EmployeeID) FROM View_EmployeeOfficial WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS FemaleExistPerson"+
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND  AttendanceDate='"+sDate+"' AND InTime!='"+sDate+" 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS MalePresent"+
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='"+sDate+"' AND InTime='"+sDate+" 00:00:000' AND OutTime='"+sDate+" 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS FemalePresent"+
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='"+sDate+"' AND InTime!='"+sDate+" 00:00:000') AS Present"+
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime='" + sDate + " 00:00:000' AND OutTime='" + sDate + " 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS [MaleAbsent]" +
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime='" + sDate + " 00:00:000' AND OutTime='" + sDate + " 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [FemaleAbsent]" +
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='"+sDate+"' AND InTime='"+sDate+" 00:00:000' AND OutTime='"+sDate+" 00:00:000') AS [Absent]"+
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='"+sDate+"' AND IsDayOff=1) AS [DayOff]"+
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsHoliday=1) AS [HoliDay]" +
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND  AttendanceDate='"+sDate+"' AND IsDayOff=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS [MaleDayOff]"+
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsDayOff=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [FemaleDayOff] " +
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsLeave=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [MaleLeave]" +
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsLeave=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [FemaleLeave]" +
        //           ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsLeave=1) AS [Leave] " +

        //            "FROM View_DepartmentRequirementDesignation AS DRPD) aa WHERE DepartmentID IN ("+sDepartmentIDs+")";

        //    if (nShiftID > 0)
        //    {

        //        sSql = sSql + "AND ShiftID=" + nShiftID;

        //    }

        //    if(nLocationID > 0)
        //    {

        //        sSql = sSql + "AND LocationID="+nLocationID;

        //    }

        //    try
        //    {
        //        _oAttendanceMonitorings = AttendanceMonitoring.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

        //        if (_oAttendanceMonitorings.Count <= 0)
        //        {
        //            throw new Exception("Data Not Found !");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        _oAttendanceMonitoring = new AttendanceMonitoring();
        //        _oAttendanceMonitorings = new List<AttendanceMonitoring>();
        //        _oAttendanceMonitoring.ErrorMessage = ex.Message;
        //        _oAttendanceMonitorings.Add(_oAttendanceMonitoring);
        //    }

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oAttendanceMonitorings);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);

        //}

        [HttpGet]
        public JsonResult AttendanceMonitoringSearch(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBlockIDs, string sGroupIDs, DateTime dDate, double nts)
        {
            _oAttendanceMonitorings = new List<AttendanceMonitoring>();
            try
            {
                _oAttendanceMonitorings = AttendanceMonitoring.Gets(sBUnit, sDepartmentIDs, sDepartmentIDs, sDesignationIDs, sShiftIds, sBlockIDs, dDate, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oAttendanceMonitorings.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oAttendanceMonitoring = new AttendanceMonitoring();
                _oAttendanceMonitorings = new List<AttendanceMonitoring>();
                _oAttendanceMonitoring.ErrorMessage = ex.Message;
                _oAttendanceMonitorings.Add(_oAttendanceMonitoring);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceMonitorings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Print
        public ActionResult PrintAttendanceMonitoring(int nLocationID, string sDepartmentIDs, int nShiftID, string sDate, double nts)
        {
            _oAttendanceMonitoring = new AttendanceMonitoring();
            string sSql = "";
            sSql = "SELECT * FROM (SELECT DRPD.DepartmentID,(SELECT Name From Department WHERE DepartmentID=DRPD.DepartmentID) AS Department" +
                  ", DRPD.DesignationID , DRPD.Designation , DRPD.Shift , DRPD.RequiredPerson, DRPD.ShiftID" +
                  ", (SELECT LocationID  FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID=DRPD.DepartmentRequirementPolicyID) AS LocationID" +
                  ", (SELECT COUNT(EmployeeID) FROM View_EmployeeOfficial WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS ExistPerson" +
                  ", (SELECT COUNT(EmployeeID) FROM View_EmployeeOfficial WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS MaleExistPerson" +
                  ", (SELECT COUNT(EmployeeID) FROM View_EmployeeOfficial WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS FemaleExistPerson" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND  AttendanceDate='" + sDate + "' AND InTime!='" + sDate + " 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS MalePresent" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime='" + sDate + " 00:00:000' AND OutTime='" + sDate + " 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS FemalePresent" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime!='" + sDate + " 00:00:000') AS Present" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime='" + sDate + " 00:00:000' AND OutTime='" + sDate + " 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS [MaleAbsent]" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime='" + sDate + " 00:00:000' AND OutTime='" + sDate + " 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [FemaleAbsent]" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime='" + sDate + " 00:00:000' AND OutTime='" + sDate + " 00:00:000') AS [Absent]" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsDayOff=1) AS [DayOff]" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsHoliday=1) AS [HoliDay]" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND  AttendanceDate='" + sDate + "' AND IsDayOff=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS [MaleDayOff]" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsDayOff=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [FemaleDayOff] " +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsLeave=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [MaleLeave]" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsLeave=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [FemaleLeave]" +
                  ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsLeave=1) AS [Leave] " +

                   "FROM View_DepartmentRequirementDesignation AS DRPD) aa WHERE DepartmentID IN (" + sDepartmentIDs + ")";

            if (nShiftID > 0)
            {
                sSql = sSql + "AND ShiftID=" + nShiftID;
            }

            if (nLocationID > 0)
            {
                sSql = sSql + "AND LocationID=" + nLocationID;
            }

            _oAttendanceMonitoring.AttendanceMonitorings = AttendanceMonitoring.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceMonitoring.Company = oCompanys.First();

            rptAttendanceMonitoring oReport = new rptAttendanceMonitoring();
            byte[] abytes = oReport.PrepareReport(_oAttendanceMonitoring);
            return File(abytes, "application/pdf");
        }

        //public ActionResult PrintAttendanceMonitoring_Landscape(int nLocationID, string sDepartmentIDs, int nShiftID, string sDate, double nts)
        //{
        //    _oAttendanceMonitoring = new AttendanceMonitoring();
        //    string sSql = "";
        //    sSql = "SELECT * FROM (SELECT DRPD.DepartmentID,(SELECT Name From Department WHERE DepartmentID=DRPD.DepartmentID) AS Department" +

        //          ", DRPD.DesignationID , DRPD.Designation , DRPD.Shift , DRPD.RequiredPerson, DRPD.ShiftID" +
        //          ", (SELECT LocationID  FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID=DRPD.DepartmentRequirementPolicyID) AS LocationID" +
        //          ", (SELECT COUNT(EmployeeID) FROM View_EmployeeOfficial WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS ExistPerson" +
        //          ", (SELECT COUNT(EmployeeID) FROM View_EmployeeOfficial WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS MaleExistPerson" +
        //          ", (SELECT COUNT(EmployeeID) FROM View_EmployeeOfficial WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND IsActive=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS FemaleExistPerson" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND  AttendanceDate='" + sDate + "' AND InTime!='" + sDate + " 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS MalePresent" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime='" + sDate + " 00:00:000' AND OutTime='" + sDate + " 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS FemalePresent" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime!='" + sDate + " 00:00:000') AS Present" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime='" + sDate + " 00:00:000' AND OutTime='" + sDate + " 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS [MaleAbsent]" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime='" + sDate + " 00:00:000' AND OutTime='" + sDate + " 00:00:000' AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [FemaleAbsent]" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND InTime='" + sDate + " 00:00:000' AND OutTime='" + sDate + " 00:00:000') AS [Absent]" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsDayOff=1) AS [DayOff]" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsHoliday=1) AS [HoliDay]" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND  AttendanceDate='" + sDate + "' AND IsDayOff=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Male')) AS [MaleDayOff]" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsDayOff=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [FemaleDayOff] " +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsLeave=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [MaleLeave]" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsLeave=1 AND EmployeeID IN (SELECT EmployeeID FROM Employee WHERE Gender='Female')) AS [FemaleLeave]" +
        //          ", (SELECT COUNT(AttendanceID) FROM AttendanceDaily WHERE DepartmentID=DRPD.DepartmentID AND DesignationID=DRPD.DesignationID AND AttendanceDate='" + sDate + "' AND IsLeave=1) AS [Leave] " +

        //           "FROM View_DepartmentRequirementDesignation AS DRPD) aa WHERE DepartmentID IN (" + sDepartmentIDs + ")";

        //    if (nShiftID > 0)
        //    {
        //        sSql = sSql + "AND ShiftID=" + nShiftID;
        //    }

        //    if (nLocationID > 0)
        //    {
        //        sSql = sSql + "AND LocationID=" + nLocationID;
        //    }

        //    _oAttendanceMonitoring.AttendanceMonitorings = AttendanceMonitoring.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    _oAttendanceMonitoring.Company = oCompanys.First();
        //    _oAttendanceMonitoring.Company.CompanyLogo = GetCompanyLogo(_oAttendanceMonitoring.Company);

        //    rptAttendanceMonitoringInLandscape oReport = new rptAttendanceMonitoringInLandscape();
        //    byte[] abytes = oReport.PrepareReport(_oAttendanceMonitoring);
        //    return File(abytes, "application/pdf");
        //}

        public ActionResult PrintAttendanceMonitoring_Landscape(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBlockIDs, DateTime dDate, double nts)
        {
            _oAttendanceMonitoring = new AttendanceMonitoring();
            _oAttendanceMonitoring.AttendanceMonitorings = AttendanceMonitoring.Gets(sBUnit, sLocationID, sDepartmentIDs, sDesignationIDs, sShiftIds, sBlockIDs, dDate, "", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceMonitoring.Company = oCompanys.First();
            _oAttendanceMonitoring.Company.CompanyLogo = GetCompanyLogo(_oAttendanceMonitoring.Company);
            _oAttendanceMonitoring.ErrorMessage = dDate.ToString("dd MMM yyyy");

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oAttendanceMonitoring.AttendanceMonitorings.Select(p => p.BUID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


            rptAttendanceMonitoringInLandscape oReport = new rptAttendanceMonitoringInLandscape();
            byte[] abytes = oReport.PrepareReport(_oAttendanceMonitoring, oBusinessUnits);
            return File(abytes, "application/pdf");
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #region Attendance Monitoring LINE XL
        public void PrintAttendanceMonitoring_LINE_XL(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, double nts)
        {
           
            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            List<AttendanceMonitoring> oAttendanceMonitorings = new List<AttendanceMonitoring>();
            oAttendanceMonitorings = AttendanceMonitoring.Gets_LINE(sBUnit, sLocationID,sDepartmentIDs, sDesignationIDs, sShiftIds, sBMMIDs, dDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<AttendanceMonitoring> oAttendanceMonitorings_Des = new List<AttendanceMonitoring>();//distinct
            oAttendanceMonitorings_Des = oAttendanceMonitorings.GroupBy(p => p.DesignationID).Select(g => g.First()).ToList();

            Dictionary<int, string> departments = new Dictionary<int, string>();

            Dictionary<int, int> departmentDesingnation = new Dictionary<int, int>();
            oAttendanceMonitorings.GroupBy(x => x.DepartmentID).Select(g => g.First()).ToList().ForEach(x =>
            {
                departments.Add(x.DepartmentID, x.DepartmentName);
                departmentDesingnation.Add(x.DepartmentID, oAttendanceMonitorings_Des.Where(p => p.DepartmentID == x.DepartmentID).Select(o => o.DesignationID).Distinct().Count());
            });


            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ATTENDANCE MONITORING");
                sheet.Name = "ATTENDANCE MONITORING";

                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 30; //DEPARTMENT

                int i = 4;
                for (int n = 0; n < 4; n++)
                {
                    foreach (AttendanceMonitoring oDesignation in oAttendanceMonitorings_Des)
                    {
                        sheet.Column(i++).Width = 8; //P
                    }
                }

                sheet.Column(i++).Width = 12; //T
                nMaxColumn = i - 1;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "ATTENDANCE MONITORING"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                if (oAttendanceMonitorings.Count <= 0)
                {
                    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Nothing to print"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                else
                {
                    
                    int nCount = 0;
                    foreach (int nDeptId in departmentDesingnation.Keys)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, ++colIndex]; cell.Merge = true;
                        cell.Value = "DEPARTMENT NAME"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        ++colIndex;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + departmentDesingnation[nDeptId] - 1]; cell.Merge = true;
                        cell.Value = "PRESENT"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        ++colIndex;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + departmentDesingnation[nDeptId] - 1]; cell.Merge = true;
                        cell.Value = "ABSENT"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        ++colIndex;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + departmentDesingnation[nDeptId] - 1]; cell.Merge = true;
                        cell.Value = "LEAVE"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        ++colIndex;
                        cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + departmentDesingnation[nDeptId] - 1]; cell.Merge = true;
                        cell.Value = "TOTAL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;

                        /*----------------------- Second ----------------------*/

                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = departments[nDeptId]; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        for (int n = 0; n < 4; n++)
                        {
                            foreach (AttendanceMonitoring oDesignation in oAttendanceMonitorings_Des.Where(x => x.DepartmentID == nDeptId).ToList())
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = oDesignation.DesignationName; cell.Style.Font.Bold = true;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                        }
                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = "COMMENT"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;


                        /*-------------- Code For Line -----------------*/

                        List<AttendanceMonitoring> oTempAttendanceMonitorings = new List<AttendanceMonitoring>();
                        oTempAttendanceMonitorings = oAttendanceMonitorings.Where(x => x.DepartmentID == nDeptId).ToList();

                        Dictionary<int, string> blockInfo = new Dictionary<int, string>();
                        Dictionary<string, int> summary = new Dictionary<string, int>();


                        oTempAttendanceMonitorings.GroupBy(x => x.BlockID).Select(g => g.First()).OrderBy(x => x.BlockID).ToList().ForEach(x =>
                        {
                            blockInfo.Add(x.BlockID, x.BlockName);
                        });


                        nCount = 0;
                        foreach (int blockId in blockInfo.Keys)
                        {
                            var attMonitorings = oTempAttendanceMonitorings.Where(x => x.BlockID == blockId).ToList();

                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ++nCount; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = blockInfo[blockId]; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            for (int n = 0; n < 4; n++)
                            {
                                foreach (AttendanceMonitoring oDesignation in oAttendanceMonitorings_Des.Where(x => x.DepartmentID == nDeptId).ToList())
                                {
                                    if (n == 0)// Present
                                    {
                                        int nPresent = attMonitorings.Where(x => x.DesignationID == oDesignation.DesignationID && x.BlockID == blockId).Sum(x => x.Present);
                                        string key = "P" + oDesignation.DesignationID.ToString();
                                        if (summary.ContainsKey(key))
                                            summary[key] = summary[key] + nPresent;
                                        else
                                            summary[key] = nPresent;

                                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nPresent; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    }
                                    if (n == 1)// Absent
                                    {
                                        int nAbsent = attMonitorings.Where(x => x.DesignationID == oDesignation.DesignationID && x.BlockID == blockId).Sum(x => x.Absent);
                                        string key = "A" + oDesignation.DesignationID.ToString();
                                        if (summary.ContainsKey(key))
                                            summary[key] = summary[key] + nAbsent;
                                        else
                                            summary[key] = nAbsent;

                                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nAbsent; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    }
                                    if (n == 2)// Leave
                                    {
                                        int nLeave = attMonitorings.Where(x => x.DesignationID == oDesignation.DesignationID && x.BlockID == blockId).Sum(x => x.Leave);
                                        string key = "L" + oDesignation.DesignationID.ToString();
                                        if (summary.ContainsKey(key))
                                            summary[key] = summary[key] + nLeave;
                                        else
                                            summary[key] = nLeave;

                                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nLeave; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    }
                                    if (n == 3)// Total
                                    {
                                        int nTotal = attMonitorings.Where(x => x.DesignationID == oDesignation.DesignationID && x.BlockID == blockId).Sum(x => x.Present + x.Absent + x.Leave);
                                        string key = "T" + oDesignation.DesignationID.ToString();
                                        if (summary.ContainsKey(key))
                                            summary[key] = summary[key] + nTotal;
                                        else
                                            summary[key] = nTotal;

                                        cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTotal; cell.Style.Font.Bold = true;
                                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    }

                                }
                            }

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                        }


                        if (summary.Keys.Count() > 0)
                        {
                            colIndex = 2;
                            cell = sheet.Cells[rowIndex, colIndex, rowIndex, ++colIndex]; cell.Merge = true;
                            cell.Value = "TOTAL"; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            foreach (string key in summary.Keys)
                            {
                                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = summary[key]; cell.Style.Font.Bold = true;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }

                            cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            rowIndex++;
                        }
                    }
                }
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ATTENDANCE_MONITORING.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion Attendance Monitoring LINE XL

        #region Attendance Monitoring Dept Sec XL
        public void PrintAttendanceMonitoring_DeptSec_XL(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, double nts)
        {
            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            List<AttendanceMonitoring> oAttendanceMonitorings = new List<AttendanceMonitoring>();
            oAttendanceMonitorings = AttendanceMonitoring.Gets_DeptSecWise(sBUnit, sLocationID,sDepartmentIDs, sDesignationIDs, sShiftIds, sBMMIDs, dDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", oAttendanceMonitorings.Select(p => p.BUID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


            int maxSubSection = 0;
            List<int> maxList= new List<int>();
            if (oAttendanceMonitorings.Count > 0)
            {
                //maxSubSection = (from am in oAttendanceMonitorings
                //                     group am by new { am.DepartmentID,am.BlockID  }into grp
                //                     select new
                //                     {
                //                         BlockID = grp.Key,
                //                         DepartmentID=grp.Key,
                //                         SectionCount = oAttendanceMonitorings.Where(x => x.DepartmentID == grp.Key.DepartmentID && x.BlockID == grp.Key.BlockID).ToList().Count()
                //                     }).ToList().Max(x => x.SectionCount);


                oAttendanceMonitorings.GroupBy(x => x.DepartmentID).Select(g => g.First()).ToList().ForEach(x =>
                {
                    maxList.Add(oAttendanceMonitorings.Where(p => p.DepartmentID == x.DepartmentID).Select(o => o.BlockID).Distinct().Count());
                });
            }
            if (maxList.Count>0) maxSubSection = maxList.Max(x => x);

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("ATTENDANCE MONITORING");
                sheet.Name = "ATTENDANCE MONITORING";

                nMaxColumn = 3 + maxSubSection*3+2;
                sheet.Column(2).Width = 10; //SL
                sheet.Column(3).Width = 40; //DEPARTMENT

                int n = 0;
                for (n = 5; n < nMaxColumn-1; n++)
                {
                    for(int p=0; p<maxSubSection;p++)
                    {
                        sheet.Column(n).Width = 8; //P
                    }
                }

                sheet.Column(n).Width = 12; //T

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = (oBusinessUnits.Count > 1 || oBusinessUnits.Count <= 0) ? oCompany.Name : oBusinessUnits[0].Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "ATTENDANCE MONITORING"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                if (oAttendanceMonitorings.Count <= 0)
                {
                    sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Nothing to print"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                else
                {
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Section"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + maxSubSection-1];cell.Merge = true;
                    cell.Value = "Present"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    colIndex = colIndex + maxSubSection;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + maxSubSection-1];cell.Merge = true;
                    cell.Value = "Absent"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    colIndex = colIndex + maxSubSection;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + maxSubSection-1];cell.Merge = true;
                    cell.Value = "Leave"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    colIndex = colIndex + maxSubSection;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex++]; cell.Merge = true;
                    cell.Value = "Total"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                   
                   cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex]; cell.Merge = true;
                   cell.Value = "Remark"; cell.Style.Font.Bold = true;
                   fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                   cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                   border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    List<AttendanceMonitoring> oAM_Depts = new List<AttendanceMonitoring>();
                    oAM_Depts = oAttendanceMonitorings.GroupBy(x => x.DepartmentID).Select(x => x.First()).ToList();

                    rowIndex++;
                    foreach (AttendanceMonitoring oitem in oAM_Depts)
                    {
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oitem.DepartmentName; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        string Sections = string.Join(",", oAttendanceMonitorings.Where(x => x.DepartmentID == oitem.DepartmentID).Select(x => x.BlockName).Distinct());
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Sections; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        List<AttendanceMonitoring> oAM_Secs = new List<AttendanceMonitoring>();
                        oAM_Secs = oAttendanceMonitorings.Where(x => x.DepartmentID == oitem.DepartmentID && x.Status == "P")
                            .OrderBy(x => x.BlockName).GroupBy(x=>x.BlockID).Select(x=>x.First()).ToList();

                        int nBlockCount = 0;
                        foreach (AttendanceMonitoring oAMItem in oAM_Secs)
                        {
                            nBlockCount++;
                            int nCount = oAttendanceMonitorings.Where(x => x.BlockID == oAMItem.BlockID && x.Status == "P").Sum(x => x.Count);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        for (int x = 0; x < maxSubSection - nBlockCount; x++)
                        {
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        oAM_Secs = new List<AttendanceMonitoring>();
                        oAM_Secs = oAttendanceMonitorings.Where(x => x.DepartmentID == oitem.DepartmentID && x.Status == "A")
                              .OrderBy(x => x.BlockName).GroupBy(x => x.BlockID).Select(x => x.First()).ToList();
                        nBlockCount = 0;
                        foreach (AttendanceMonitoring oAMItem in oAM_Secs)
                        {
                            nBlockCount++;
                            int nCount = oAttendanceMonitorings.Where(x => x.BlockID == oAMItem.BlockID && x.Status == "A").Sum(x => x.Count);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        for (int x = 0; x < maxSubSection - nBlockCount; x++)
                        {
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        oAM_Secs = new List<AttendanceMonitoring>();
                        oAM_Secs = oAttendanceMonitorings.Where(x => x.DepartmentID == oitem.DepartmentID && x.Status == "L")
                               .OrderBy(x => x.BlockName).GroupBy(x => x.BlockID).Select(x => x.First()).ToList();

                        nBlockCount = 0;
                        foreach (AttendanceMonitoring oAMItem in oAM_Secs)
                        {
                            nBlockCount++;
                            int nCount = oAttendanceMonitorings.Where(x => x.BlockID == oAMItem.BlockID && x.Status == "L").Sum(x => x.Count);
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nCount; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }
                        for (int x = 0; x < maxSubSection - nBlockCount; x++)
                        {
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        List<AttendanceMonitoring> oAM_SecTotal = new List<AttendanceMonitoring>();
                        int nTotal = oAttendanceMonitorings.Where(x => x.DepartmentID == oitem.DepartmentID).Sum(x => x.Count);

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotal; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        //List<AttendanceMonitoring> oAM_SecTotal = new List<AttendanceMonitoring>();
                        //oAM_SecTotal = oAttendanceMonitorings.Where(x => x.DepartmentID == oitem.DepartmentID).OrderBy(x=>x.BlockName).ToList();
                        //foreach (AttendanceMonitoring oAMItem in oAM_SecTotal)
                        //{
                        //    int nTotal = 0;
                        //    nTotal = oAM_SecTotal.Where(x=>x.BlockID==oAMItem.BlockID).Sum(x => x.Count);
                        //    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotal; cell.Style.Font.Bold = true;
                        //    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //}

                        //if (oAM_SecTotal.Count>0 && oAM_SecTotal.Count < maxSubSection)
                        //{
                        //    for (int i = 0; i < (maxSubSection - oAM_SecTotal.Count); i++)
                        //    {
                        //        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = 0; cell.Style.Font.Bold = true;
                        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        //    }
                        //}

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                    }
                }
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ATTENDANCE_MONITORING.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion Attendance Monitoring Dept Sec XL

        #region Attendance Monitoring LINE 
        public ActionResult PrintAttendanceMonitoring_LINE(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBMMIDs, DateTime dDate, double nts)
        {
            AttendanceMonitoring oAttendanceMonitoring = new AttendanceMonitoring();
            oAttendanceMonitoring.AttendanceMonitorings = AttendanceMonitoring.Gets_LINE(sBUnit, sLocationID,sDepartmentIDs, sDesignationIDs, sShiftIds, sBMMIDs, dDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<AttendanceMonitoring> oAttendanceMonitorings = new List<AttendanceMonitoring>();
            oAttendanceMonitoring.AttendanceMonitorings_DepSec = AttendanceMonitoring.Gets_DeptSecWise(sBUnit, sLocationID,sDepartmentIDs, sDesignationIDs, sShiftIds, sBMMIDs, dDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceMonitoring.Company = oCompanys.First();

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", oAttendanceMonitoring.AttendanceMonitorings.Select(p => p.BUID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


            rptAttendanceMonitoring_LINE oReport = new rptAttendanceMonitoring_LINE();
            byte[] abytes = oReport.PrepareReport(oAttendanceMonitoring, oBusinessUnits);
            return File(abytes, "application/pdf");
        }
        #endregion Attendance Monitoring LINE 

        public ActionResult PrintAttendanceMonitoring_F2(string sBUnit, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sShiftIds, string sBlockIDs, DateTime dDate, double nts)
        {
            _oAttendanceMonitoring = new AttendanceMonitoring();
            _oAttendanceMonitoring.AttendanceMonitorings = AttendanceMonitoring.Gets(sBUnit, sLocationID, sDepartmentIDs, sDesignationIDs, sShiftIds, sBlockIDs, dDate, "", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceMonitoring.Company = oCompanys.First();
            _oAttendanceMonitoring.Company.CompanyLogo = GetCompanyLogo(_oAttendanceMonitoring.Company);
            _oAttendanceMonitoring.ErrorMessage = dDate.ToString("dd MMM yyyy");

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            string BUIDs = string.Join(",", _oAttendanceMonitoring.AttendanceMonitorings.Select(p => p.BUID).Distinct().ToList());
            if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


            rptAttendanceMonitoring_F2 oReport = new rptAttendanceMonitoring_F2();
            byte[] abytes = oReport.PrepareReport(_oAttendanceMonitoring, dDate, oBusinessUnits);
            return File(abytes, "application/pdf");
        }
        #endregion Print
    }
}
