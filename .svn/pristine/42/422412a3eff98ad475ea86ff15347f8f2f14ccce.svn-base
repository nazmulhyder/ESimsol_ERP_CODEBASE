using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class CompensatoryLeaveController : Controller
    {
        public ActionResult ViewCompensatoryLeaves(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
     
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.CompensatoryLeave).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            AttendanceDailyV2 oAttendanceDailyV2 = new AttendanceDailyV2();
            List<MaxOTConfiguration> oMaxOTConfiguration = new List<MaxOTConfiguration>();
            oMaxOTConfiguration = MaxOTConfiguration.GetsByUser((int)(Session[SessionInfo.currentUserID]));
            ViewBag.TimeCards = oMaxOTConfiguration;
            ViewBag.AuthorizationRolesMappings = oAuthorizationRoleMappings;
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM HRM_Shift WHERE IsActive=1";
            ViewBag.HRMShifts = HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();
            oLeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead ORDER BY LeaveHeadID ASC", (int)(Session[SessionInfo.currentUserID]));
            ViewBag.LeaveHeads = oLeaveHeads;
            return View(oAttendanceDailyV2);

        }
        #region Gets Permitted TimeCard
        private List<EnumObject> GetsPermittedTimeCard(List<AuthorizationRoleMapping> oAuthorizationRoleMappings)
        {
            EnumObject oTimeCardFormat = new EnumObject();
            List<EnumObject> oTimeCardFormats = new List<EnumObject>();

            foreach (AuthorizationRoleMapping oItem in oAuthorizationRoleMappings)
            {
                if (oItem.OperationType == EnumRoleOperationType.Time_Card_F_01)
                {
                    oTimeCardFormat = new EnumObject();
                    oTimeCardFormat.id = (int)EnumTimeCardFormat.Time_Card_F_01;
                    oTimeCardFormat.Value = EnumObject.jGet(EnumTimeCardFormat.Time_Card_F_01);
                    oTimeCardFormats.Add(oTimeCardFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Time_Card_F_02)
                {
                    oTimeCardFormat = new EnumObject();
                    oTimeCardFormat.id = (int)EnumTimeCardFormat.Time_Card_F_02;
                    oTimeCardFormat.Value = EnumObject.jGet(EnumTimeCardFormat.Time_Card_F_02);
                    oTimeCardFormats.Add(oTimeCardFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Time_Card_F_03)
                {
                    oTimeCardFormat = new EnumObject();
                    oTimeCardFormat.id = (int)EnumTimeCardFormat.Time_Card_F_03;
                    oTimeCardFormat.Value = EnumObject.jGet(EnumTimeCardFormat.Time_Card_F_03);
                    oTimeCardFormats.Add(oTimeCardFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Time_Card_F_04)
                {
                    oTimeCardFormat = new EnumObject();
                    oTimeCardFormat.id = (int)EnumTimeCardFormat.Time_Card_F_04;
                    oTimeCardFormat.Value = EnumObject.jGet(EnumTimeCardFormat.Time_Card_F_04);
                    oTimeCardFormats.Add(oTimeCardFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Time_Card_F_05)
                {
                    oTimeCardFormat = new EnumObject();
                    oTimeCardFormat.id = (int)EnumTimeCardFormat.Time_Card_F_05;
                    oTimeCardFormat.Value = EnumObject.jGet(EnumTimeCardFormat.Time_Card_F_05);
                    oTimeCardFormats.Add(oTimeCardFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Time_Card_F_06)
                {
                    oTimeCardFormat = new EnumObject();
                    oTimeCardFormat.id = (int)EnumTimeCardFormat.Time_Card_F_06;
                    oTimeCardFormat.Value = EnumObject.jGet(EnumTimeCardFormat.Time_Card_F_06);
                    oTimeCardFormats.Add(oTimeCardFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Time_Card_F_07)
                {
                    oTimeCardFormat = new EnumObject();
                    oTimeCardFormat.id = (int)EnumTimeCardFormat.Time_Card_F_07;
                    oTimeCardFormat.Value = EnumObject.jGet(EnumTimeCardFormat.Time_Card_F_07);
                    oTimeCardFormats.Add(oTimeCardFormat);
                }
                else if (oItem.OperationType == EnumRoleOperationType.Time_Card_F_08)
                {
                    oTimeCardFormat = new EnumObject();
                    oTimeCardFormat.id = (int)EnumTimeCardFormat.Time_Card_F_08;
                    oTimeCardFormat.Value = EnumObject.jGet(EnumTimeCardFormat.Time_Card_F_08);
                    oTimeCardFormats.Add(oTimeCardFormat);
                }
            }

            return oTimeCardFormats;
        }
        #endregion

        public JsonResult AssignLeave(AttendanceDailyV2 oAttendanceDailyV2)
        {
            string FeedbackMessage="";
            try
            {
                FeedbackMessage = oAttendanceDailyV2.AssignLeave((int)(Session[SessionInfo.currentUserID]));
               
            }
            catch (Exception ex)
            {

                FeedbackMessage = ex.Message;
               
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(FeedbackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AdvanceSearch(HCMSearchObj oHCMSearchObj)
        {
            List<AttendanceDailyV2> oAttendanceDailyV2s = new List<AttendanceDailyV2>();
            try
            {
                string sSQL = this.GetSQL(oHCMSearchObj);
                oAttendanceDailyV2s = AttendanceDailyV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                if (oAttendanceDailyV2s.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                AttendanceDailyV2 AttendanceDailyV2 = new AttendanceDailyV2();
                oAttendanceDailyV2s = new List<AttendanceDailyV2>();
                AttendanceDailyV2.ErrorMessage = ex.Message;
                oAttendanceDailyV2s.Add(AttendanceDailyV2);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailyV2s);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(HCMSearchObj oHCMSearchObj)
        {
            string sSQL = "";
            if (oHCMSearchObj.CompensatoryLeaveType == 1)
            {
                sSQL = "SELECT * FROM View_MaxOTConfigurationAttendance AS MOTAD  WHERE MOTAD.MOCID = " + oHCMSearchObj.MOCID + " AND MOTAD.IsDayOff=1 AND CAST(MOTAD.InTime AS TIME(0))!='00:00' AND MOTAD.AttendanceDate = '" + oHCMSearchObj.EndDate.ToString("dd MMM yyyy") + "'";
            }
            else
            {
                sSQL = "SELECT * FROM View_MaxOTConfigurationAttendance AS MOTAD  WHERE MOTAD.MOCID = " + oHCMSearchObj.MOCID + " AND MOTAD.IsCompensatoryLeave=1  AND MOTAD.AttendanceDate = '" + oHCMSearchObj.EndDate.ToString("dd MMM yyyy") + "'";
            }
            if (oHCMSearchObj.EmployeeIDs != "" && oHCMSearchObj.EmployeeIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT Emp.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.EmployeeIDs + "', ',') AS Emp)";
            }
            if (oHCMSearchObj.LocationIDs != "" && oHCMSearchObj.LocationIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.LocationID IN (SELECT LOC.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.LocationIDs + "', ',') AS LOC)";
            }
            if (oHCMSearchObj.DepartmentIDs != "" && oHCMSearchObj.DepartmentIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.DepartmentID IN (SELECT DEPT.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DepartmentIDs + "', ',') AS DEPT)";
            }
            if (oHCMSearchObj.DesignationIDs != "" && oHCMSearchObj.DesignationIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.DesignationID IN (SELECT Desg.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DesignationIDs + "', ',') AS Desg)";
            }
            if (oHCMSearchObj.BUIDs != "" && oHCMSearchObj.BUIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.BusinessUnitID IN (SELECT BU.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.BUIDs + "', ',') AS BU)";
            }
            if (oHCMSearchObj.BlockIDs != "" && oHCMSearchObj.BlockIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.BlockIDs + "))";
            }
            if (oHCMSearchObj.GroupIDs != "" && oHCMSearchObj.GroupIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.GroupIDs + "))";
            }
            if (oHCMSearchObj.EmployeeTypeIDs != "" && oHCMSearchObj.EmployeeTypeIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.EmployeeTypeIDs + "))";
            }
            if (oHCMSearchObj.ShiftIDs != "" && oHCMSearchObj.ShiftIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.CurrentShiftID IN (" + oHCMSearchObj.ShiftIDs + "))";
            }
            if (oHCMSearchObj.SalarySchemeIDs != "" && oHCMSearchObj.SalarySchemeIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.SalarySchemeID IN (" + oHCMSearchObj.SalarySchemeIDs + "))";
            }

            if (oHCMSearchObj.AttendanceSchemeIDs != "" && oHCMSearchObj.AttendanceSchemeIDs != null)
            {
                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.AttendanceSchemeID IN (" + oHCMSearchObj.AttendanceSchemeIDs + "))";
            }

            if (oHCMSearchObj.CategoryID != 0)
            {
                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT EC.EmployeeID FROM EmployeeConfirmation AS EC WHERE EO.EmployeeCategory=" + oHCMSearchObj.CategoryID + ")";
            }
            if (oHCMSearchObj.AuthenticationNo != "" && oHCMSearchObj.AuthenticationNo != null)
            {
                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT EA.EmployeeID FROM EmployeeAuthentication AS EA WHERE EA.IsActive =1 AND EA.CardNo LIKE '%" + oHCMSearchObj.AuthenticationNo + "%')";
            }
            if (oHCMSearchObj.StartSalaryRange != 0 && oHCMSearchObj.EndSalaryRange != 0)
            {
                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.GrossAmount BETWEEN " + oHCMSearchObj.StartSalaryRange.ToString() + " AND " + oHCMSearchObj.EndSalaryRange.ToString() + ")";
            }
            if (oHCMSearchObj.IsJoiningDate == true)
            {

                sSQL = sSQL + " AND MOTAD.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.DateOfJoin BETWEEN '" + oHCMSearchObj.JoiningStartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.JoiningEndDate.ToString("dd MMM yyyy") + "')";
            }
            sSQL = sSQL + " ORDER BY MOTAD.Code";
            return sSQL;
        }

    }
}