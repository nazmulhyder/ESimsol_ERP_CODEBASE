using System;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Drawing;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
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
using System.Collections;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;

namespace ESimSolFinancial.Controllers
{
    public class TimeCardV2Controller : Controller
    {               
        #region Actual Actions
        public ActionResult View_TimeCard(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TimeCard).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            AttendanceDailyV2 oAttendanceDailyV2 = new AttendanceDailyV2();
            List<EnumObject> oTimeCardFormats = this.GetsPermittedTimeCard(oAuthorizationRoleMappings);
            ViewBag.TimeCardFormats = oTimeCardFormats;            
            ViewBag.AuthorizationRolesMappings = oAuthorizationRoleMappings;
            ViewBag.SelectedFormatID = oTimeCardFormats.Select(x => x.id).FirstOrDefault();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM HRM_Shift WHERE IsActive=1";
            ViewBag.HRMShifts = HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oAttendanceDailyV2);
        }

        public ActionResult PrintTimeCard(double ts)
        {
            List<AttendanceDailyV2> oAttendanceDailyV2s = new List<AttendanceDailyV2>();
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            Company oCompany = new Company();
            string _sErrorMesage = "";
            try
            {
                oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oHCMSearchObj);

                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                oAttendanceDailyV2s = AttendanceDailyV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                if (oAttendanceDailyV2s.Count <= 0)
                {
                    rptErrorMessage oReport = new rptErrorMessage();
                    byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                    return File(abytes, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                oAttendanceDailyV2s = new List<AttendanceDailyV2>();
                _sErrorMesage = ex.Message;
            }
            if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_01)
            {
                rptTimeCard_F01V2 oReport = new rptTimeCard_F01V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_02)
            {
                rptTimeCard_F02V2 oReport = new rptTimeCard_F02V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_03)
            {
                rptTimeCard_F03V2 oReport = new rptTimeCard_F03V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_04)
            {
                rptTimeCard_F04V2 oReport = new rptTimeCard_F04V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }

            else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_05)
            {
                rptTimeCard_F05V2 oReport = new rptTimeCard_F05V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }

            else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_06)
            {
                List<LeaveHead> oLeaveHead = LeaveHead.Gets("SELECT * FROM LeaveHead", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                rptTimeCard_F06V2 oReport = new rptTimeCard_F06V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oLeaveHead, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }
            return RedirectToAction("~/blank");
        }

        public void PrintTimeCardExcel(double ts)
        {
            List<AttendanceDailyV2> oAttendanceDailyV2s = new List<AttendanceDailyV2>();
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            Company oCompany = new Company();
            try
            {
                oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oHCMSearchObj);

                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                oAttendanceDailyV2s = AttendanceDailyV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_01)
                {
                    TimeCardFormatOneXL(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_02)
                {
                    TimeCardFormatTwoXL(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_03)
                {
                    TimeCardFormatThreeXL(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_04)
                {
                    TimeCardFormatFourXL(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }

                else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_05)
                {
                    TimeCardFormatFiveXL(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }

                else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_06)
                {
                    List<LeaveHead> oLeaveHead = LeaveHead.Gets("SELECT * FROM LeaveHead", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    TimeCardFormatSixXL(oAttendanceDailyV2s, oCompany, oLeaveHead, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }
            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Time Card");
                    sheet.Name = "Time card";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=TimeCard.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }

        }
        #endregion

        #region Comp Actions
        public ActionResult View_CompTimeCard(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TimeCard).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

          

            List<MaxOTConfiguration> oMaxOTConfiguration = new List<MaxOTConfiguration>();
            AttendanceDailyV2 oAttendanceDailyV2 = new AttendanceDailyV2();
            oMaxOTConfiguration = MaxOTConfiguration.GetsByUser((int)(Session[SessionInfo.currentUserID]));
            List<EnumObject> oTimeCardFormats = this.GetsPermittedTimeCard(oAuthorizationRoleMappings);
            ViewBag.TimeCardFormats = oTimeCardFormats;
            ViewBag.AuthorizationRolesMappings = oAuthorizationRoleMappings;
            ViewBag.TimeCards = oMaxOTConfiguration;
            ViewBag.SelectedFormatID = oTimeCardFormats.Select(x => x.id).FirstOrDefault();
            ViewBag.SelectedTimeCardID = oMaxOTConfiguration.Select(x => x.MOCID).FirstOrDefault();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM HRM_Shift WHERE IsActive=1";
            ViewBag.HRMShifts = HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oAttendanceDailyV2);
        }

        public ActionResult PrintCompTimeCard(double ts)
        {
            List<AttendanceDailyV2> oAttendanceDailyV2s = new List<AttendanceDailyV2>();
            MaxOTConfiguration oMaxOTConfiguration = new MaxOTConfiguration();
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            Company oCompany = new Company();
            string _sErrorMesage = "";
            try
            {
                oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];
                oMaxOTConfiguration = oMaxOTConfiguration.Get(oHCMSearchObj.MOCID, (int)Session[SessionInfo.currentUserID]);
                if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID > 0)
                {
                    oHCMSearchObj.MOCID = oMaxOTConfiguration.SourceTimeCardID;
                    string sSQL = this.CompGetSQL(oHCMSearchObj);
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = AttendanceDailyV2.CompGets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = this.GetsAttendanceWithoutDayOffHoliday(oAttendanceDailyV2s, oMaxOTConfiguration.MaxOTInMin);
                    oAttendanceDailyV2s.OrderBy(x => x.EmployeeID).ThenBy(x => x.AttendanceDate).ToList();
                }
                else if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID == 0)
                {                    
                    string sSQL = this.GetSQL(oHCMSearchObj);
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = AttendanceDailyV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = this.GetsAttendanceWithoutDayOffHoliday(oAttendanceDailyV2s, 0);
                    oAttendanceDailyV2s.OrderBy(x => x.EmployeeID).ThenBy(x => x.AttendanceDate).ToList();
                }
                else
                {
                    string sSQL = this.CompGetSQL(oHCMSearchObj);
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = AttendanceDailyV2.CompGets(sSQL, (int)(Session[SessionInfo.currentUserID]));                    
                }

                if (oAttendanceDailyV2s.Count <= 0)
                {
                    rptErrorMessage oReport = new rptErrorMessage();
                    byte[] abytes = oReport.PrepareReport("No Print Setup Found!!");
                    return File(abytes, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                oAttendanceDailyV2s = new List<AttendanceDailyV2>();
                _sErrorMesage = ex.Message;
            }
            if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_01)
            {
                rptTimeCard_F01V2 oReport = new rptTimeCard_F01V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_02)
            {
                rptTimeCard_F02V2 oReport = new rptTimeCard_F02V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_03)
            {
                rptTimeCard_F03V2 oReport = new rptTimeCard_F03V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }
            else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_04)
            {
                rptTimeCard_F04V2 oReport = new rptTimeCard_F04V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }

            else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_05)
            {
                rptTimeCard_F05V2 oReport = new rptTimeCard_F05V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }

            else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_06)
            {
                List<LeaveHead> oLeaveHead = LeaveHead.Gets("SELECT * FROM LeaveHead", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                rptTimeCard_F06V2 oReport = new rptTimeCard_F06V2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany, oLeaveHead, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                return File(abytes, "application/pdf");
            }
            return RedirectToAction("~/blank");
        }

        public void PrintCompTimeCardExcel(double ts)
        {
            List<AttendanceDailyV2> oAttendanceDailyV2s = new List<AttendanceDailyV2>();
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            MaxOTConfiguration oMaxOTConfiguration = new MaxOTConfiguration();
            Company oCompany = new Company();
            try
            {
                oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];

                oMaxOTConfiguration = oMaxOTConfiguration.Get(oHCMSearchObj.MOCID, (int)Session[SessionInfo.currentUserID]);
                if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID > 0)
                {
                    oHCMSearchObj.MOCID = oMaxOTConfiguration.SourceTimeCardID;
                    string sSQL = this.CompGetSQL(oHCMSearchObj);
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = AttendanceDailyV2.CompGets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = this.GetsAttendanceWithoutDayOffHoliday(oAttendanceDailyV2s, oMaxOTConfiguration.MaxOTInMin);
                    oAttendanceDailyV2s.OrderBy(x => x.EmployeeID).ThenBy(x => x.AttendanceDate).ToList();
                }
                else if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID == 0)
                {
                    string sSQL = this.GetSQL(oHCMSearchObj);
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = AttendanceDailyV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = this.GetsAttendanceWithoutDayOffHoliday(oAttendanceDailyV2s, 0);
                    oAttendanceDailyV2s.OrderBy(x => x.EmployeeID).ThenBy(x => x.AttendanceDate).ToList();
                }
                else
                {
                    string sSQL = this.CompGetSQL(oHCMSearchObj);
                    oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = AttendanceDailyV2.CompGets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                }

                oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));
                if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_01)
                {
                    TimeCardFormatOneXL(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_02)
                {
                    TimeCardFormatTwoXL(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_03)
                {
                    TimeCardFormatThreeXL(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }
                else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_04)
                {
                    TimeCardFormatFourXL(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }

                else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_05)
                {
                    TimeCardFormatFiveXL(oAttendanceDailyV2s, oCompany, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }

                else if (oHCMSearchObj.PrintFormatInt == (int)EnumTimeCardFormat.Time_Card_F_06)
                {
                    List<LeaveHead> oLeaveHead = LeaveHead.Gets("SELECT * FROM LeaveHead", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    TimeCardFormatSixXL(oAttendanceDailyV2s, oCompany, oLeaveHead, oHCMSearchObj.StartDate.ToString("dd MMM yyyy"), oHCMSearchObj.EndDate.ToString("dd MMM yyyy"));
                }
            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Time Card");
                    sheet.Name = "Time card";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=TimeCard.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }

        }
        #endregion

        #region Utility Functions
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
        private List<AttendanceDailyV2> GetsAttendanceWithoutDayOffHoliday(List<AttendanceDailyV2> oAttendanceDailyV2s, int MaxOTInMin)
        {
            List<AttendanceDailyV2> oFinalAttendanceDailyV2s = new List<AttendanceDailyV2>();
            if (oAttendanceDailyV2s.Count > 0)
            {
                while (oAttendanceDailyV2s.Count > 0)
                {
                    List<AttendanceDailyV2> oADs = new List<AttendanceDailyV2>();
                    oADs = oAttendanceDailyV2s.Where(x => x.EmployeeID == oAttendanceDailyV2s[0].EmployeeID).OrderBy(x => x.OverTimeInMin).ToList();
                    oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oADs[0].EmployeeID);
                    int nTotalDayOffHDOT = oADs.Where(x => x.IsHoliday == true || x.IsDayOff == true).ToList().Sum(x => x.OverTimeInMin);

                    int TotalNumRow = oADs.Where(x => x.IsHoliday == false && x.IsDayOff == false && x.OutTimeInString != "-").ToList().Count;                                        
                    int nPerdayApplyOT = nTotalDayOffHDOT / TotalNumRow;
                    if (nPerdayApplyOT < 60)
                    {
                        nPerdayApplyOT = 60;
                    }

                    foreach (AttendanceDailyV2 oItem in oADs)
                    {
                        if (MaxOTInMin > 0)
                        {
                            if (oItem.IsDayOff == false && oItem.IsHoliday == false)
                            {
                                if (oItem.OverTimeInMin < MaxOTInMin && oItem.OverTimeInMin > 0 && oItem.OutTimeInString != "-" && nTotalDayOffHDOT > MaxOTInMin - oItem.OverTimeInMin)
                                {
                                    int nNeededOverTimeInMin = MaxOTInMin - oItem.OverTimeInMin;
                                    oItem.OutTime = oItem.OutTime.AddMinutes(nNeededOverTimeInMin);
                                    oItem.OverTimeInMin = oItem.OverTimeInMin + nNeededOverTimeInMin;
                                    nTotalDayOffHDOT = nTotalDayOffHDOT - nNeededOverTimeInMin;
                                }
                                else
                                {
                                    if (oItem.OverTimeInMin == 0 && oItem.OutTimeInString != "-" && nTotalDayOffHDOT > 0)
                                    {
                                        int ShiftDiff = (int)oItem.ShiftEndTime.Subtract(oItem.OutTime).TotalMinutes;
                                        oItem.OutTime = oItem.OutTime.AddMinutes(ShiftDiff);
                                        int neededOverTime = (nTotalDayOffHDOT >= MaxOTInMin) ? MaxOTInMin : nTotalDayOffHDOT;
                                        oItem.OutTime = oItem.OutTime.AddMinutes(neededOverTime+ this.RandomNumber(1, 10));
                                        oItem.OverTimeInMin = neededOverTime;
                                        nTotalDayOffHDOT = nTotalDayOffHDOT - neededOverTime;
                                    }
                                }
                            }

                            if (oItem.IsDayOff == true || oItem.IsHoliday == true)
                            {
                                oItem.InTime = new DateTime(oItem.InTime.Year, oItem.InTime.Month, oItem.InTime.Day, 0, 0, 0);
                                oItem.OutTime = new DateTime(oItem.OutTime.Year, oItem.OutTime.Month, oItem.OutTime.Day, 0, 0, 0);
                                oItem.OverTimeInMin = 0;
                            }
                        }
                        else
                        {
                            if (oItem.IsDayOff == false && oItem.IsHoliday == false)
                            {
                                if (oItem.OverTimeInMin > 0 && nTotalDayOffHDOT > 0)
                                {                                    
                                    oItem.OutTime = oItem.OutTime.AddMinutes(nPerdayApplyOT);
                                    oItem.OverTimeInMin = oItem.OverTimeInMin + nPerdayApplyOT;
                                    nTotalDayOffHDOT = nTotalDayOffHDOT - nPerdayApplyOT;
                                }
                                else
                                {
                                    if (oItem.OverTimeInMin == 0 && oItem.OutTimeInString != "-" && nTotalDayOffHDOT > 0)
                                    {
                                        int ShiftDiff = (int)oItem.ShiftEndTime.Subtract(oItem.OutTime).TotalMinutes;
                                        oItem.OutTime = oItem.OutTime.AddMinutes(ShiftDiff);                                        
                                        if (nTotalDayOffHDOT >= (nPerdayApplyOT * 2))
                                        {
                                            int nApplyOutTime = (nPerdayApplyOT * 2) + this.RandomNumber(1, 10);
                                            oItem.OutTime = oItem.OutTime.AddMinutes(nApplyOutTime);
                                            oItem.OverTimeInMin = (nPerdayApplyOT * 2);
                                            nTotalDayOffHDOT = nTotalDayOffHDOT - (nPerdayApplyOT * 2);
                                        }
                                        else
                                        {
                                            int nApplyOutTime = nPerdayApplyOT + this.RandomNumber(1, 10);
                                            oItem.OutTime = oItem.OutTime.AddMinutes(nApplyOutTime);
                                            oItem.OverTimeInMin = nPerdayApplyOT;
                                            nTotalDayOffHDOT = nTotalDayOffHDOT - nPerdayApplyOT;
                                        }
                                    }
                                }
                            }

                            if (oItem.IsDayOff == true || oItem.IsHoliday == true)
                            {
                                oItem.InTime = new DateTime(oItem.InTime.Year, oItem.InTime.Month, oItem.InTime.Day, 0, 0, 0);
                                oItem.OutTime = new DateTime(oItem.OutTime.Year, oItem.OutTime.Month, oItem.OutTime.Day, 0, 0, 0);
                                oItem.OverTimeInMin = 0;
                            }

                        }

                    }
                    oFinalAttendanceDailyV2s.AddRange(oADs);
                }
               
            }
            return oFinalAttendanceDailyV2s;
        }


        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private string CompGetSQL(HCMSearchObj oHCMSearchObj)
        {
            string sSQL = "SELECT * FROM View_MaxOTConfigurationAttendance AS MOCA WHERE MOCA.MOCID = " + oHCMSearchObj.MOCID + " AND MOCA.AttendanceDate BETWEEN '" + oHCMSearchObj.StartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.EndDate.ToString("dd MMM yyyy") + "'";
            if (oHCMSearchObj.EmployeeIDs != "" && oHCMSearchObj.EmployeeIDs != null)
            {
                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT Emp.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.EmployeeIDs + "', ',') AS Emp)";
            }
            if (oHCMSearchObj.LocationIDs != "" && oHCMSearchObj.LocationIDs != null)
            {
                sSQL = sSQL + " AND MOCA.LocationID IN (SELECT LOC.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.LocationIDs + "', ',') AS LOC)";
            }
            if (oHCMSearchObj.DepartmentIDs != "" && oHCMSearchObj.DepartmentIDs != null)
            {
                sSQL = sSQL + " AND MOCA.DepartmentID IN (SELECT DEPT.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DepartmentIDs + "', ',') AS DEPT)";
            }
            if (oHCMSearchObj.DesignationIDs != "" && oHCMSearchObj.DesignationIDs != null)
            {
                sSQL = sSQL + " AND MOCA.DesignationID IN (SELECT Desg.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DesignationIDs + "', ',') AS Desg)";
            }
            if (oHCMSearchObj.BUIDs != "" && oHCMSearchObj.BUIDs != null)
            {
                sSQL = sSQL + " AND MOCA.BusinessUnitID IN (SELECT BU.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.BUIDs + "', ',') AS BU)";
            }
            if (oHCMSearchObj.BlockIDs != "" && oHCMSearchObj.BlockIDs != null)
            {
                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.BlockIDs + "))";
            }
            if (oHCMSearchObj.GroupIDs != "" && oHCMSearchObj.GroupIDs != null)
            {
                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.GroupIDs + "))";
            }
            if (oHCMSearchObj.EmployeeTypeIDs != "" && oHCMSearchObj.EmployeeTypeIDs != null)
            {
                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.EmployeeTypeIDs + "))";
            }
            if (oHCMSearchObj.ShiftIDs != "" && oHCMSearchObj.ShiftIDs != null)
            {
                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.CurrentShiftID IN (" + oHCMSearchObj.ShiftIDs + "))";
            }
            if (oHCMSearchObj.SalarySchemeIDs != "" && oHCMSearchObj.SalarySchemeIDs != null)
            {
                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.SalarySchemeID IN (" + oHCMSearchObj.SalarySchemeIDs + "))";
            }
            if (oHCMSearchObj.AttendanceSchemeIDs != "" && oHCMSearchObj.AttendanceSchemeIDs != null)
            {
                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.AttendanceSchemeID IN (" + oHCMSearchObj.AttendanceSchemeIDs + "))";
            }
            if (oHCMSearchObj.CategoryID != 0 && oHCMSearchObj.CategoryID != null)
            {
                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT EC.EmployeeID FROM EmployeeConfirmation AS EC WHERE EO.EmployeeCategory=" + oHCMSearchObj.CategoryID + ")";
            }
            if (oHCMSearchObj.AuthenticationNo != "" && oHCMSearchObj.AuthenticationNo != null)
            {
                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT EA.EmployeeID FROM EmployeeAuthentication AS EA WHERE EA.IsActive =1 AND EA.CardNo LIKE '%" + oHCMSearchObj.AuthenticationNo + "%')";
            }
            if (oHCMSearchObj.StartSalaryRange != 0 && oHCMSearchObj.EndSalaryRange != 0)
            {
                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.GrossAmount BETWEEN " + oHCMSearchObj.StartSalaryRange.ToString() + " AND " + oHCMSearchObj.EndSalaryRange.ToString() + ")";
            }

            if (oHCMSearchObj.IsJoiningDate == true)
            {

                sSQL = sSQL + " AND MOCA.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.DateOfJoin BETWEEN '" + oHCMSearchObj.JoiningStartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.JoiningEndDate.ToString("dd MMM yyyy") + "')";
            }
            sSQL = sSQL + " ORDER BY MOCA.EmployeeID,MOCA.AttendanceDate";
            return sSQL;
        }
        private string GetSQL(HCMSearchObj oHCMSearchObj)
        {
            string sSQL = "SELECT * FROM View_AttendanceDaily AS DA WHERE DA.AttendanceDate BETWEEN '" + oHCMSearchObj.StartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.EndDate.ToString("dd MMM yyyy") + "'";
            if (oHCMSearchObj.EmployeeIDs != "" && oHCMSearchObj.EmployeeIDs != null)
            {
                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT Emp.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.EmployeeIDs + "', ',') AS Emp)";
            }
            if (oHCMSearchObj.LocationIDs != "" && oHCMSearchObj.LocationIDs != null)
            {
                sSQL = sSQL + " AND DA.LocationID IN (SELECT LOC.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.LocationIDs + "', ',') AS LOC)";
            }
            if (oHCMSearchObj.DepartmentIDs != "" && oHCMSearchObj.DepartmentIDs != null)
            {
                sSQL = sSQL + " AND DA.DepartmentID IN (SELECT DEPT.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DepartmentIDs + "', ',') AS DEPT)";
            }
            if (oHCMSearchObj.DesignationIDs != "" && oHCMSearchObj.DesignationIDs != null)
            {
                sSQL = sSQL + " AND DA.DesignationID IN (SELECT Desg.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DesignationIDs + "', ',') AS Desg)";
            }
            if (oHCMSearchObj.BUIDs != "" && oHCMSearchObj.BUIDs != null)
            {
                sSQL = sSQL + " AND DA.BusinessUnitID IN (SELECT BU.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.BUIDs + "', ',') AS BU)";
            }
            if (oHCMSearchObj.BlockIDs != "" && oHCMSearchObj.BlockIDs != null)
            {
                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.BlockIDs + "))";
            }
            if (oHCMSearchObj.GroupIDs != "" && oHCMSearchObj.GroupIDs != null)
            {
                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.GroupIDs + "))";
            }
            if (oHCMSearchObj.EmployeeTypeIDs != "" && oHCMSearchObj.EmployeeTypeIDs != null)
            {
                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.EmployeeTypeIDs + "))";
            }
            if (oHCMSearchObj.ShiftIDs != "" && oHCMSearchObj.ShiftIDs != null)
            {
                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.CurrentShiftID IN (" + oHCMSearchObj.ShiftIDs + "))";
            }
            if (oHCMSearchObj.SalarySchemeIDs != "" && oHCMSearchObj.SalarySchemeIDs != null)
            {
                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.SalarySchemeID IN (" + oHCMSearchObj.SalarySchemeIDs + "))";
            }

            if (oHCMSearchObj.AttendanceSchemeIDs != "" && oHCMSearchObj.AttendanceSchemeIDs != null)
            {
                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.AttendanceSchemeID IN (" + oHCMSearchObj.AttendanceSchemeIDs + "))";
            }

            if (oHCMSearchObj.CategoryID != 0 && oHCMSearchObj.CategoryID != null)
            {
                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT EC.EmployeeID FROM EmployeeConfirmation AS EC WHERE EO.EmployeeCategory=" + oHCMSearchObj.CategoryID + ")";
            }
            if (oHCMSearchObj.AuthenticationNo != "" && oHCMSearchObj.AuthenticationNo != null)
            {
                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT EA.EmployeeID FROM EmployeeAuthentication AS EA WHERE EA.IsActive =1 AND EA.CardNo LIKE '%" + oHCMSearchObj.AuthenticationNo + "%')";
            }
            if (oHCMSearchObj.StartSalaryRange != 0 && oHCMSearchObj.EndSalaryRange != 0)
            {
                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.GrossAmount BETWEEN " + oHCMSearchObj.StartSalaryRange.ToString() + " AND " + oHCMSearchObj.EndSalaryRange.ToString() + ")";
            }
            if (oHCMSearchObj.IsJoiningDate == true)
            {

                sSQL = sSQL + " AND DA.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.DateOfJoin BETWEEN '" + oHCMSearchObj.JoiningStartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.JoiningEndDate.ToString("dd MMM yyyy") + "')";
            }
            sSQL = sSQL + " ORDER BY DA.EmployeeID,DA.AttendanceDate";
            return sSQL;
        }
        public Image GetImage(byte[] Image)
        {
            if (Image != null)
            {
                string fileDirectory = Server.MapPath("~/Content/Image.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(Image);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;

            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Post Method
        [HttpPost]
        public ActionResult SetSessionSearchCriteria(HCMSearchObj oHCMSearchObj)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oHCMSearchObj);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CompAdvanceSearch(HCMSearchObj oHCMSearchObj)
        {
            MaxOTConfiguration oMaxOTConfiguration = new MaxOTConfiguration();
            List<AttendanceDailyV2> oAttendanceDailyV2s = new List<AttendanceDailyV2>();
            try
            {
                oMaxOTConfiguration = oMaxOTConfiguration.Get(oHCMSearchObj.MOCID, (int)Session[SessionInfo.currentUserID]);
                if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID > 0)
                {
                    oHCMSearchObj.MOCID = oMaxOTConfiguration.SourceTimeCardID;
                    string sSQL = this.CompGetSQL(oHCMSearchObj);
                    oAttendanceDailyV2s = AttendanceDailyV2.CompGets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = this.GetsAttendanceWithoutDayOffHoliday(oAttendanceDailyV2s, oMaxOTConfiguration.MaxOTInMin);
                    oAttendanceDailyV2s.OrderBy(x => x.EmployeeID).ThenBy(x => x.AttendanceDate).ToList();
                }
                else if (oMaxOTConfiguration.IsActive == false && oMaxOTConfiguration.SourceTimeCardID == 0)
                {
                    string sSQL = this.GetSQL(oHCMSearchObj);
                    oAttendanceDailyV2s = AttendanceDailyV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                    oAttendanceDailyV2s = this.GetsAttendanceWithoutDayOffHoliday(oAttendanceDailyV2s, 0);
                    oAttendanceDailyV2s.OrderBy(x => x.EmployeeID).ThenBy(x => x.AttendanceDate).ToList();
                }
                else
                {
                    string sSQL = this.CompGetSQL(oHCMSearchObj);
                    oAttendanceDailyV2s = AttendanceDailyV2.CompGets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                }
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

            var jsonResult = Json(oAttendanceDailyV2s, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
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

            var jsonResult = Json(oAttendanceDailyV2s, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        
        [HttpPost]
        public JsonResult GetsEmployeeType(EmployeeType oEmployeeType)
        {
            List<EmployeeType> _oEmployeeTypes = new List<EmployeeType>();
            try
            {
                string sSql = "";
                if (!string.IsNullOrEmpty(oEmployeeType.Name))
                {
                    sSql = "select * from EmployeeType where IsActive=1 AND Name LIKE'%" + oEmployeeType.Name + "%'";
                }
                else
                    sSql = "select * from EmployeeType Where IsActive=1";
                _oEmployeeTypes = EmployeeType.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
                if (_oEmployeeTypes.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                EmployeeType _oEmployeeGroup = new EmployeeType();
                _oEmployeeGroup.ErrorMessage = ex.Message;
                _oEmployeeTypes.Add(_oEmployeeGroup);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeTypes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsHRMShiftByName(HRMShift oHRMShift)
        {
            List<HRMShift> _oHRMShifts = new List<HRMShift>();
            try
            {
                string sSql = "";
                if (!string.IsNullOrEmpty(oHRMShift.Name))
                {
                    sSql = "select * from HRM_Shift where  IsActive=1 AND Name LIKE'%" + oHRMShift.Name + "%'";
                }
                else
                    sSql = "select * from HRM_Shift WHERE IsActive=1";
                _oHRMShifts = HRMShift.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
                if (_oHRMShifts.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                HRMShift _oHRMShift = new HRMShift();
                _oHRMShift.ErrorMessage = ex.Message;
                _oHRMShifts.Add(_oHRMShift);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oHRMShifts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AttendanceDaily_Manual_Single(AttendanceDailyV2 oAttendanceDailyV2)
        {
            try
            {
                oAttendanceDailyV2 = oAttendanceDailyV2.AttendanceDaily_Manual_Single(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oAttendanceDailyV2 = new AttendanceDailyV2();
                oAttendanceDailyV2.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailyV2);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AttendanceDaily_Manual_Single_Comp(AttendanceDailyV2 oAttendanceDailyV2)
        {
            try
            {
                oAttendanceDailyV2 = oAttendanceDailyV2.AttendanceDaily_Manual_Single_Comp(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oAttendanceDailyV2 = new AttendanceDailyV2();
                oAttendanceDailyV2.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailyV2);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsAttendanceDailyManualHistory(AttendanceDailyManualHistory oAttendanceDailyManualHistory)
        {
            string sSQL = "";
            sSQL = "SELECT * FROM view_AttendanceDailyManualHistory WHERE AttendanceID=" + oAttendanceDailyManualHistory.AttendanceID+" ORDER BY ADMHID DESC";


            List<AttendanceDailyManualHistory> oAttendanceDailyManualHistorys = new List<AttendanceDailyManualHistory>();
            try
            {
                oAttendanceDailyManualHistorys = AttendanceDailyManualHistory.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oAttendanceDailyManualHistorys.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }

            }
            catch (Exception ex)
            {
                oAttendanceDailyManualHistory = new AttendanceDailyManualHistory();
                oAttendanceDailyManualHistorys = new List<AttendanceDailyManualHistory>();
                oAttendanceDailyManualHistory.ErrorMessage = ex.Message;
                oAttendanceDailyManualHistorys.Add(oAttendanceDailyManualHistory);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailyManualHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsMaxOTManualUpdateHistory(AttendanceDailyManualHistory oAttendanceDailyManualHistory)
        {
            string sSQL = "";
            sSQL = "SELECT * FROM View_MaxOTManualUpdateHistory WHERE MOCAID=" + oAttendanceDailyManualHistory.MOCAID + " ORDER BY MOMUHID DESC";


            List<AttendanceDailyManualHistory> oAttendanceDailyManualHistorys = new List<AttendanceDailyManualHistory>();
            try
            {
                oAttendanceDailyManualHistorys = AttendanceDailyManualHistory.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oAttendanceDailyManualHistorys.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }

            }
            catch (Exception ex)
            {
                oAttendanceDailyManualHistory = new AttendanceDailyManualHistory();
                oAttendanceDailyManualHistorys = new List<AttendanceDailyManualHistory>();
                oAttendanceDailyManualHistory.ErrorMessage = ex.Message;
                oAttendanceDailyManualHistorys.Add(oAttendanceDailyManualHistory);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailyManualHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region TimeCard Excel
        #region TimeCardFormat OneXL
        private void TimeCardFormatOneXL(List<AttendanceDailyV2> oAttendanceDailyV2s, Company oCompany, string StartDate, string EndDate)
        {
            int nRowIndex = 1, nstartRow = 1, nEndRow = 0, nStartCol = 1, nEndCol = 6, nCheckEachRowItem = 0;
            int colIn = 0;
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            DateTime _dstartDate = Convert.ToDateTime(StartDate);
            DateTime _dEndDate = Convert.ToDateTime(EndDate);
            dstartDate = _dstartDate;
            dEndDate = _dEndDate;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("TIME-CARD-F1");
                sheet.Name = "TIME-CARD-F1";
                sheet.PrinterSettings.TopMargin = 0.0M;
                sheet.PrinterSettings.LeftMargin = 0.1M;
                sheet.PrinterSettings.BottomMargin = 0.0M;
                sheet.PrinterSettings.RightMargin = 0.1M;
                sheet.PrinterSettings.HeaderMargin = 0.0M;
                sheet.PrinterSettings.FooterMargin = 0.0M;
                sheet.PrinterSettings.Orientation = eOrientation.Landscape;
                sheet.PrinterSettings.PaperSize = ePaperSize.A4;

                int nColCount = 1;
                for (var i = 0; i < 4; i++)
                {
                    sheet.Column(nColCount++).Width = 11;//Date
                    sheet.Column(nColCount++).Width = 6;//In
                    sheet.Column(nColCount++).Width = 6;//Out
                    sheet.Column(nColCount++).Width = 6;//OT/NW
                    sheet.Column(nColCount++).Width = 7;//Status
                    sheet.Column(nColCount++).Width = 10;//Shift
                    sheet.Column(nColCount++).Width = 3;//Gaph
                }


                if (oAttendanceDailyV2s.Count > 0)
                {
                    while (oAttendanceDailyV2s.Count > 0)
                    {
                        int nCount = 0;
                        int nExactWD = 0;
                        dstartDate = _dstartDate;
                        dEndDate = _dEndDate;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUAddress; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Time Card"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        List<AttendanceDailyV2> oADs = new List<AttendanceDailyV2>();
                        oADs = oAttendanceDailyV2s.Where(x => x.EmployeeID == oAttendanceDailyV2s[0].EmployeeID).ToList();
                        oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oADs[0].EmployeeID);


                        colIn = nStartCol;



                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Name";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Font.Size = 10;

                        cell = sheet.Cells[nRowIndex, colIn++, nRowIndex, colIn += 3]; cell.Merge = true; cell.Value = ":" + oADs[0].EmployeeName + "[" + oADs[0].EmployeeCode + "]";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.Size = 8;
                        nRowIndex++;
                        colIn = nStartCol;


                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Designation";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Font.Size = 10;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = ":" + oADs[0].DesignationName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.Size = 8;

                        nRowIndex++;
                        colIn = nStartCol;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Joining";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Font.Size = 10;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].JoiningDateInString;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.Size = 8;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = "Date Range";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Font.Size = 10;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ": " + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy");
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.Font.Size = 8;


                        nRowIndex += 2;
                        colIn = nStartCol;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "In"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Out"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "OT/NW"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Shift"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;

                        colIn = nStartCol;
                        nCount = 0;
                        dEndDate = dEndDate.AddDays(1);
                        while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
                        {
                            nCount++;

                            AttendanceDailyV2 oTempAttendanceDailyV2 = new AttendanceDailyV2();
                            oTempAttendanceDailyV2 = oADs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();


                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = dstartDate.ToString("dd MMM yyyy");
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if (oTempAttendanceDailyV2 != null)
                            {
                                nExactWD++;
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.InTimeInString;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OutTimeInString;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = Global.MinInHourMin(oTempAttendanceDailyV2.OverTimeInMin);
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.AttStatusInString;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.ShiftName;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            }
                            else
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--";
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--";
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--";
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "No Record Found";
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--";
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }


                            nRowIndex++;
                            colIn = nStartCol;
                            dstartDate = dstartDate.AddDays(1);
                        }

                        colIn = nStartCol;

                        int nTotalDayOff = oADs.Where(x => x.AttStatusInString == "Off").ToList().Count();
                        int nTotalHoliday = oADs.Where(x => x.AttStatusInString == "HD").ToList().Count();
                        int nTWD = nExactWD - (nTotalDayOff + nTotalHoliday);
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "WD-" + nTWD.ToString() + ", P-" + AttendanceDailyV2.GetAttendanceSummary(oADs, "P", false) + ", A-" + AttendanceDailyV2.GetAttendanceSummary(oADs, "A", false) + ", L-" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Leave", false) + ", Off-" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Off", false) + ", H-" + AttendanceDailyV2.GetAttendanceSummary(oADs, "HD", false) + ", LT-" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Late", false) + ", E-" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Early Out Days", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "OTNH-" + AttendanceDailyV2.GetAttendanceSummary(oADs, "OT") + ", TOT-" + AttendanceDailyV2.GetAttendanceSummary(oADs, "OT"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nCheckEachRowItem++;
                        if (nCheckEachRowItem == 3)
                        {
                            nRowIndex += 1;
                            nstartRow = nRowIndex;
                            nStartCol = 1;
                            nCheckEachRowItem = 0;
                            nEndCol = 6;
                        }
                        else
                        {
                            nStartCol = nStartCol + 7;
                            nRowIndex = nstartRow;
                            nEndCol = nStartCol + 5;
                        }
                    }
                }

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol * 6];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=TIME-CARD-F1.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region TimeCardFormat TwoXL
        private void TimeCardFormatTwoXL(List<AttendanceDailyV2> oAttendanceDailyV2s, Company oCompany, string StartDate, string EndDate)
        {
            int nRowIndex = 1, nEndRow = 0, nStartCol = 1, nEndCol = 6;
            int colIn = 0;
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            DateTime _dstartDate = Convert.ToDateTime(StartDate);
            DateTime _dEndDate = Convert.ToDateTime(EndDate);
            dstartDate = _dstartDate;
            dEndDate = _dEndDate;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("TIME-CARD-F2");
                sheet.Name = "TIME-CARD-F2";

                sheet.Column(1).Width = 7;//SL
                sheet.Column(2).Width = 12;//Date
                sheet.Column(3).Width = 10;//IN
                sheet.Column(4).Width = 10;//Out
                sheet.Column(5).Width = 13;//OT HR
                sheet.Column(6).Width = 17;//Status





                if (oAttendanceDailyV2s.Count > 0)
                {
                    while (oAttendanceDailyV2s.Count > 0)
                    {
                        int nCount = 0;
                        dstartDate = _dstartDate;
                        dEndDate = _dEndDate;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUAddress; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Time Card"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        List<AttendanceDailyV2> oADs = new List<AttendanceDailyV2>();
                        oADs = oAttendanceDailyV2s.Where(x => x.EmployeeID == oAttendanceDailyV2s[0].EmployeeID).ToList();
                        oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oADs[0].EmployeeID);

                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++, nRowIndex, colIn++]; cell.Merge = true; cell.Value = "UNIT : " + (oADs.Count > 0 ? oADs[0].LocationName : "");
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn++, nRowIndex, colIn++]; cell.Merge = true; cell.Value = "";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn++, nRowIndex, colIn++]; cell.Merge = true; cell.Value = "Print Date :" + DateTime.Now.ToString("dd MMM yyyy");
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        nRowIndex += 2;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Code";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].EmployeeCode;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = "Name";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].EmployeeName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Department";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].DepartmentName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = "Designation";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].DesignationName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Joining";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].JoiningDateInString;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = "Date Range";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy");
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                        nRowIndex += 1;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "In"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Out"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "OT HR"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nRowIndex++;

                        colIn = 1;
                        nCount = 0;
                        dEndDate = dEndDate.AddDays(1);
                        while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
                        {
                            nCount++;

                            AttendanceDailyV2 oTempAttendanceDailyV2 = new AttendanceDailyV2();
                            oTempAttendanceDailyV2 = oADs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();

                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            string nameOfDay = dstartDate.ToString("ddd");
                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = dstartDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if (oTempAttendanceDailyV2 != null)
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.InTimeInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OutTimeInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OverTimeInMin; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.AttStatusInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            }
                            else
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "No Record Found"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }


                            nRowIndex++;
                            colIn = 1;
                            dstartDate = dstartDate.AddDays(1);
                        }

                        nRowIndex += 1;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Days In Month "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + nCount.ToString() + " Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;



                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Leave "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Leave"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Present "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "P"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Off days "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Off"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Late "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Late"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Holi Day "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "HD"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Absent "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "A"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Total Over Time "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "OT"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIn = 0;
                        nRowIndex += 2;
                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Prepared By"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                    }
                }

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=TIME-CARD-F2.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region TimeCardFormat ThreeXL
        private void TimeCardFormatThreeXL(List<AttendanceDailyV2> oAttendanceDailyV2s, Company oCompany, string StartDate, string EndDate)
        {
            int nRowIndex = 1, nEndRow = 0, nStartCol = 1, nEndCol = 8;
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            DateTime _dstartDate = Convert.ToDateTime(StartDate);
            DateTime _dEndDate = Convert.ToDateTime(EndDate);
            dstartDate = _dstartDate;
            dEndDate = _dEndDate;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("TIME-CARD-F3");
                sheet.Name = "TIME-CARD-F3";

                sheet.Column(nStartCol++).Width = 12;//Date
                sheet.Column(nStartCol++).Width = 8;//In
                sheet.Column(nStartCol++).Width = 8;//Out
                sheet.Column(nStartCol++).Width = 12;//Duration
                sheet.Column(nStartCol++).Width = 8;//Late
                sheet.Column(nStartCol++).Width = 8;//Early
                sheet.Column(nStartCol++).Width = 8;//Day
                sheet.Column(nStartCol++).Width = 8;//Status
                sheet.Column(nStartCol++).Width = 13;//Remark




                if (oAttendanceDailyV2s.Count > 0)
                {
                    while (oAttendanceDailyV2s.Count > 0)
                    {
                        int nCount = 0;
                        dstartDate = _dstartDate;
                        dEndDate = _dEndDate;

                        nStartCol = 1;
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUAddress; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "JOB CARD REPORT FROM " + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        List<AttendanceDailyV2> oADs = new List<AttendanceDailyV2>();
                        oADs = oAttendanceDailyV2s.Where(x => x.EmployeeID == oAttendanceDailyV2s[0].EmployeeID).ToList();
                        oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oADs[0].EmployeeID);

                        nStartCol = 1;

                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = "Employee Code";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = oADs[0].EmployeeCode;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nStartCol = 6;
                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = "Joining Date";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //New Join
                        string sNewOrNot = "";
                        if ((oADs[0].JoiningDate >= _dstartDate) && (oADs[0].JoiningDate <= _dEndDate))
                        {
                            sNewOrNot = " (New Join)";
                        }

                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = oADs[0].JoiningDateInString + sNewOrNot;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;

                        nStartCol = 1;
                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = "Name";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = oADs[0].EmployeeName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nStartCol = 6;
                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = "Department";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; ; cell.Value = oADs[0].DepartmentName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nRowIndex++;
                        nStartCol = 1;
                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = "Designation";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = oADs[0].DesignationName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nStartCol = 6;
                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = "Sec";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++, nRowIndex, nStartCol++]; cell.Merge = true; cell.Value = oADs[0].DepartmentName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nRowIndex += 2;
                        nStartCol = 1;
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "In"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Out"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Duration"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Late"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Early"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Day"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;

                        int colIn = 1;

                        dEndDate = dEndDate.AddDays(1);
                        while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
                        {
                            nCount++;

                            AttendanceDailyV2 oTempAttendanceDailyV2 = new AttendanceDailyV2();
                            oTempAttendanceDailyV2 = oADs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();

                            string nameOfDay = dstartDate.ToString("ddd");
                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = dstartDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if (oTempAttendanceDailyV2 != null)
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.InTimeInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OutTimeInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.TotalWorkingHourSt; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = Global.MinInHourMin(oTempAttendanceDailyV2.LateArrivalMinute); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = Global.MinInHourMin(oTempAttendanceDailyV2.EarlyDepartureMinute); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = nameOfDay; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.AttStatusInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.Remark; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            else
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = nameOfDay; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }


                            nRowIndex++;
                            colIn = 1;
                            dstartDate = dstartDate.AddDays(1);
                        }

                        nRowIndex += 1;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Present Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "P", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Absent Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "A", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Late Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "Late", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++, nRowIndex, colIn++]; cell.Merge = true; cell.Value = "Early Out Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "Early Out Days", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Leave Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "Leave", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Off day"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "Off", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Late Hrs."; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "Late Hour"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++, nRowIndex, colIn++]; cell.Merge = true; cell.Value = "Early Out Mins."; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "Early Out Mins"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                        string sLeaveName = "";
                        string LeaveStatusDetails = "";
                        string[] LeaveDetails;
                        List<AttendanceDailyV2> oAttendanceDailyV2S = new List<AttendanceDailyV2>();
                        oAttendanceDailyV2S = oADs.Where(x => x.LeaveName != "").GroupBy(p => p.LeaveName).Select(g => g.First()).ToList();
                        sLeaveName = string.Join(",", oAttendanceDailyV2S.Select(x => x.LeaveName));
                        LeaveDetails = sLeaveName.Split(',');


                        foreach (string sLN in LeaveDetails)
                        {
                            if (sLN != "")
                            {
                                oAttendanceDailyV2S = oADs.Where(x => x.LeaveName != "" && x.LeaveName == sLN).ToList();
                                LeaveStatusDetails += sLN + "=" + oAttendanceDailyV2S.Count + ",";

                            }
                        }
                        if (LeaveStatusDetails != "") { LeaveStatusDetails = LeaveStatusDetails.Remove(LeaveStatusDetails.Length - 1); }


                        cell = sheet.Cells[nRowIndex, 1, nRowIndex, 4]; cell.Merge = true; cell.Value = LeaveStatusDetails; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Holidays"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "HD", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        int nPresent = oADs.Where(x => x.AttStatusInString == "P").ToList().Count();
                        int nUnpaidLeave = oADs.Where(x => x.IsLeave == true && x.IsUnPaid == false).ToList().Count;
                        int nDayOff = oADs.Where(x => x.AttStatusInString == "Off").ToList().Count();
                        int nHoliDay = oADs.Where(x => x.AttStatusInString == "HD").ToList().Count();
                        int GTotal = nUnpaidLeave + nPresent + nDayOff + nHoliDay;
                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 8]; cell.Merge = true; cell.Value = "Total Att."; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = GTotal.ToString(); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        colIn = 0;
                        nRowIndex += 2;
                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, colIn += 2]; cell.Merge = true; cell.Value = "Employee/Worker Signature"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                        colIn += 2;
                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Authority"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;
                        nRowIndex += 2;
                    }
                }

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=TIME-CARD-F3.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region TimeCardFormat FourXL
        private void TimeCardFormatFourXL(List<AttendanceDailyV2> oAttendanceDailyV2s, Company oCompany, string StartDate, string EndDate)
        {
            int nRowIndex = 1, nEndRow = 0, nStartCol = 1, nEndCol = 10;
            int colIn = 0;
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            DateTime _dstartDate = Convert.ToDateTime(StartDate);
            DateTime _dEndDate = Convert.ToDateTime(EndDate);
            dstartDate = _dstartDate;
            dEndDate = _dEndDate;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("TIME-CARD-F4");
                sheet.Name = "TIME-CARD-F4";

                sheet.Column(nStartCol++).Width = 7;//SL
                sheet.Column(nStartCol++).Width = 12;//Date
                sheet.Column(nStartCol++).Width = 8;//In
                sheet.Column(nStartCol++).Width = 8;//Late
                sheet.Column(nStartCol++).Width = 8;//Out
                sheet.Column(nStartCol++).Width = 8;//Early
                sheet.Column(nStartCol++).Width = 8;//TW Hour
                sheet.Column(nStartCol++).Width = 10;//Shift
                sheet.Column(nStartCol++).Width = 8;//Shift Durtaion
                sheet.Column(nStartCol++).Width = 10;//Status




                if (oAttendanceDailyV2s.Count > 0)
                {
                    while (oAttendanceDailyV2s.Count > 0)
                    {
                        int nCount = 0;
                        dstartDate = _dstartDate;
                        dEndDate = _dEndDate;

                        nStartCol = 1;
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;





                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Time Card"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        List<AttendanceDailyV2> oADs = new List<AttendanceDailyV2>();
                        oADs = oAttendanceDailyV2s.Where(x => x.EmployeeID == oAttendanceDailyV2s[0].EmployeeID).ToList();
                        oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oADs[0].EmployeeID);


                        cell = sheet.Cells[nRowIndex, 1, nRowIndex, 2]; cell.Merge = true; cell.Value = "UNIT : " + (oADs.Count > 0 ? oADs[0].LocationName : "");
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, 3, nRowIndex, 6]; cell.Merge = true; cell.Value = "";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true; cell.Value = "Print Date : " + DateTime.Now.ToString("dd MMM yyyy");
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        nRowIndex += 1;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Code";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].EmployeeCode;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Name";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].EmployeeName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Department";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].DepartmentName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Designation";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].DesignationName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Joining";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].JoiningDateInString;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Date Range";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, colIn += 2]; cell.Merge = true; cell.Value = ": " + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy");
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                        nRowIndex += 1;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "In"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Late"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Out"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Early"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "T W Hour"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Shift"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Shift Duration"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;

                        colIn = 1;
                        nCount = 0;
                        dEndDate = dEndDate.AddDays(1);
                        while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
                        {
                            nCount++;

                            AttendanceDailyV2 oTempAttendanceDailyV2 = new AttendanceDailyV2();
                            oTempAttendanceDailyV2 = oADs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();

                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            string nameOfDay = dstartDate.ToString("ddd");
                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = dstartDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if (oTempAttendanceDailyV2 != null)
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.InTimeInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = Global.MinInHourMin(oTempAttendanceDailyV2.LateArrivalMinute); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OutTimeInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = Global.MinInHourMin(oTempAttendanceDailyV2.EarlyDepartureMinute); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.TotalWorkingHourSt; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "(" + oTempAttendanceDailyV2.ShiftStartTime.ToString("HH:mm") + "-" + oTempAttendanceDailyV2.ShiftEndTime.ToString("HH:mm") + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = Global.MinInHourMin(oTempAttendanceDailyV2.ShiftDuration); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.AttStatusInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            }
                            else
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "No Record Found"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }


                            nRowIndex++;
                            colIn = 1;
                            dstartDate = dstartDate.AddDays(1);
                        }


                        colIn = 1;

                        cell = sheet.Cells[nRowIndex, 1, nRowIndex, 6]; cell.Merge = true; cell.Value = "Total Working Hr:  " + AttendanceDailyV2.GetAttendanceSummary(oADs, "Total Working Hour"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        cell = sheet.Cells[nRowIndex, 8, nRowIndex, 10]; cell.Merge = true; cell.Value = "Total Shift Duration: " + AttendanceDailyV2.GetAttendanceSummary(oADs, "SD"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        nRowIndex += 1;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Days In Month "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + nCount.ToString() + " Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Leave "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Leave"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, nEndCol]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Present "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "P"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Off days "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Off"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, nEndCol]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Late "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Late"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Holi Day "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "HD"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, nEndCol]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Number Of Absent "; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "A"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;
                        int nTSDuration = Convert.ToInt32(AttendanceDailyV2.GetAttendanceSummary(oADs, "SD", false));
                        int nTWorkingHour = Convert.ToInt32(AttendanceDailyV2.GetAttendanceSummary(oADs, "Total Working Hour", false));
                        string sWorkingSummaryLabel = (nTSDuration > nTWorkingHour) ? "TOTAL LESS" : "TOTAL OVERTIME";
                        int nOvLessValue = (sWorkingSummaryLabel == "TOTAL LESS") ? nTSDuration - nTWorkingHour : nTWorkingHour - nTSDuration;
                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = sWorkingSummaryLabel; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ": " + Global.MinInHourMin(nOvLessValue); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, nEndCol]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIn = 0;
                        nRowIndex += 2;
                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Prepared By"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = ExcelBorderStyle.Thin;
                        nRowIndex += 1;
                    }
                }

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=TIME-CARD-F4.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region TimeCardFormat FiveXL
        private void TimeCardFormatFiveXL(List<AttendanceDailyV2> oAttendanceDailyV2s, Company oCompany, string StartDate, string EndDate)
        {
            int nRowIndex = 1, nStartCol = 1, nEndCol = 8;
            int colIn = 0;
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            DateTime _dstartDate = Convert.ToDateTime(StartDate);
            DateTime _dEndDate = Convert.ToDateTime(EndDate);
            dstartDate = _dstartDate;
            dEndDate = _dEndDate;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("TIME-CARD-F5");
                sheet.Name = "TIME-CARD-F5";

                sheet.Column(nStartCol++).Width = 12;//Date
                sheet.Column(nStartCol++).Width = 8;//Entry Time
                sheet.Column(nStartCol++).Width = 12;//Exit Date
                sheet.Column(nStartCol++).Width = 8;//Exit Time
                sheet.Column(nStartCol++).Width = 12;//Duration
                sheet.Column(nStartCol++).Width = 8;//Over Time
                sheet.Column(nStartCol++).Width = 10;//Status
                sheet.Column(nStartCol++).Width = 12;//Shift




                if (oAttendanceDailyV2s.Count > 0)
                {
                    while (oAttendanceDailyV2s.Count > 0)
                    {
                        int nCount = 0;
                        dstartDate = _dstartDate;
                        dEndDate = _dEndDate;

                        nStartCol = 1;
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUAddress; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Job Card Report From " + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        List<AttendanceDailyV2> oADs = new List<AttendanceDailyV2>();
                        oADs = oAttendanceDailyV2s.Where(x => x.EmployeeID == oAttendanceDailyV2s[0].EmployeeID).ToList();
                        oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oADs[0].EmployeeID);

                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++, nRowIndex, colIn++]; cell.Merge = true; cell.Value = "Employee ID";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].EmployeeCode;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, colIn += 1]; cell.Merge = true; cell.Value = "";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = "Joining Date";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + oADs[0].JoiningDateInString;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++, nRowIndex, colIn++]; cell.Merge = true; cell.Value = "Name";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].EmployeeName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex, colIn += 1]; cell.Merge = true; cell.Value = "";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = "Floor Name";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + oADs[0].LocationName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++, nRowIndex, colIn++]; cell.Merge = true; cell.Value = "Designation";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = ":" + oADs[0].DesignationName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;



                        nRowIndex += 1;
                        colIn = 1;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Entry Time"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Exit Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Exit Time"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Duration"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Overtime"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Shift"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;

                        colIn = 1;
                        nCount = 0;
                        dEndDate = dEndDate.AddDays(1);
                        while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
                        {
                            nCount++;

                            AttendanceDailyV2 oTempAttendanceDailyV2 = new AttendanceDailyV2();
                            oTempAttendanceDailyV2 = oADs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();

                            string nameOfDay = dstartDate.ToString("ddd");
                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = dstartDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if (oTempAttendanceDailyV2 != null)
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.InTimeInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OutTime.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OutTimeInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.TotalWorkingHourSt; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OverTimeInMinuteHourSt; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.AttStatusInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.ShiftName; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            }
                            else
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "No Record Found"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                            }


                            nRowIndex++;
                            colIn = 1;
                            dstartDate = dstartDate.AddDays(1);
                        }

                        nRowIndex++;
                        colIn = 1;

                        cell = sheet.Cells[nRowIndex, 1, nRowIndex, 3]; cell.Merge = true; cell.Value = "Total Days: " + nCount.ToString(); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        cell = sheet.Cells[nRowIndex, 6, nRowIndex, 8]; cell.Merge = true; cell.Value = "Total Over Time: " + AttendanceDailyV2.GetAttendanceSummary(oADs, "OT"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        nRowIndex += 1;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, colIn += 1]; cell.Merge = true; cell.Value = "Punch Days";
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = border.Left.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "P", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Top.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        string sLeaveName = "";
                        string LeaveStatusDetails = "";
                        string[] LeaveDetails;
                        double nTotalLeave = 0;
                        List<AttendanceDailyV2> oAttendanceDailyV2S = new List<AttendanceDailyV2>();
                        oAttendanceDailyV2S = oADs.Where(x => x.LeaveName != "").GroupBy(p => p.LeaveName).Select(g => g.First()).ToList();
                        sLeaveName = string.Join(",", oAttendanceDailyV2S.Select(x => x.LeaveName));
                        LeaveDetails = sLeaveName.Split(',');


                        foreach (string sLN in LeaveDetails)
                        {
                            if (sLN != "")
                            {
                                oAttendanceDailyV2S = oADs.Where(x => x.LeaveName != "" && x.LeaveName == sLN).ToList();
                                LeaveStatusDetails += sLN + "=" + oAttendanceDailyV2S.Count + ",";
                                nTotalLeave += oAttendanceDailyV2S.Count;
                            }
                        }
                        if (LeaveStatusDetails != "") { LeaveStatusDetails = LeaveStatusDetails.Remove(LeaveStatusDetails.Length - 1); }



                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex + 3, colIn += 2]; cell.Merge = true; cell.Value = LeaveStatusDetails; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;

                        int nTLeave = (int)nTotalLeave;
                        int nPresent = oADs.Where(x => x.AttStatusInString == "P").ToList().Count();
                        int nAbsent = oADs.Where(x => x.AttStatusInString == "A").ToList().Count();
                        int nDayOff = oADs.Where(x => x.AttStatusInString == "Off").ToList().Count();
                        int nHoliDay = oADs.Where(x => x.AttStatusInString == "HD").ToList().Count();
                        int GTotal = nTLeave + nPresent + nAbsent + nDayOff + nHoliDay;

                        cell = sheet.Cells[nRowIndex, ++colIn, nRowIndex + 3, ++colIn]; cell.Merge = true; cell.Value = "Total =" + GTotal.ToString(); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Right.Style = border.Left.Style = border.Top.Style = ExcelBorderStyle.Thin;



                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, colIn += 1]; cell.Merge = true; cell.Value = "Absent days";
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "A", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;


                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, colIn += 1]; cell.Merge = true; cell.Value = "Weekend  Day";
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "Off", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = ExcelBorderStyle.Thin;



                        nRowIndex++;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, colIn += 1]; cell.Merge = true; cell.Value = "HoliDays";
                        cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Left.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = ":" + AttendanceDailyV2.GetAttendanceSummary(oADs, "HD", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;


                        nRowIndex += 3;
                    }
                }

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=TIME-CARD-F5.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region TimeCardFormat SixXL
        private void TimeCardFormatSixXL(List<AttendanceDailyV2> oAttendanceDailyV2s, Company oCompany, List<LeaveHead> oLeaveHeads, string StartDate, string EndDate)
        {
            int nRowIndex = 1, nEndRow = 0, nStartCol = 1, nEndCol = 10;
            int colIn = 0;
            DateTime dstartDate = DateTime.Now;
            DateTime dEndDate = DateTime.Now;
            DateTime _dstartDate = Convert.ToDateTime(StartDate);
            DateTime _dEndDate = Convert.ToDateTime(EndDate);
            dstartDate = _dstartDate;
            dEndDate = _dEndDate;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("TIME-CARD-F6");
                sheet.Name = "TIME-CARD-F6";

                sheet.Column(nStartCol++).Width = 12;//Date
                sheet.Column(nStartCol++).Width = 5;//Day
                sheet.Column(nStartCol++).Width = 7;//In
                sheet.Column(nStartCol++).Width = 12;//Out Date
                sheet.Column(nStartCol++).Width = 7;//Out
                sheet.Column(nStartCol++).Width = 10;//Duration
                sheet.Column(nStartCol++).Width = 8;//Overtime
                sheet.Column(nStartCol++).Width = 10;//Shift
                sheet.Column(nStartCol++).Width = 8;//Status
                sheet.Column(nStartCol++).Width = 10;//Remark



                if (oAttendanceDailyV2s.Count > 0)
                {
                    while (oAttendanceDailyV2s.Count > 0)
                    {
                        int nCount = 0;
                        dstartDate = _dstartDate;
                        dEndDate = _dEndDate;

                        nStartCol = 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUName; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oAttendanceDailyV2s[0].BUAddress; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "JOB CARD REPORT FROM " + dstartDate.ToString("dd MMM yyyy") + " TO " + dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;


                        List<AttendanceDailyV2> oADs = new List<AttendanceDailyV2>();
                        oADs = oAttendanceDailyV2s.Where(x => x.EmployeeID == oAttendanceDailyV2s[0].EmployeeID).ToList();
                        oAttendanceDailyV2s.RemoveAll(x => x.EmployeeID == oADs[0].EmployeeID);


                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Employee ID";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 4]; cell.Merge = true; cell.Value = oADs[0].EmployeeCode;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Joining Date";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true; cell.Value = oADs[0].JoiningDateInString;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Name";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 4]; cell.Merge = true; ; cell.Value = oADs[0].EmployeeName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Department";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true; ; cell.Value = oADs[0].DepartmentName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nRowIndex++;

                        cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Designation";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 2, nRowIndex, 4]; cell.Merge = true; cell.Value = oADs[0].DesignationName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Floor Name";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true; cell.Value = oADs[0].LocationName;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        nRowIndex += 2;
                        colIn = 1;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Day"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "In"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Out Date"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Out"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Duration"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Overtime"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Shift"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                        colIn = 1;
                        dEndDate = dEndDate.AddDays(1);
                        while (dstartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
                        {
                            nCount++;

                            AttendanceDailyV2 oTempAttendanceDailyV2 = new AttendanceDailyV2();
                            oTempAttendanceDailyV2 = oADs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dstartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();

                            string nameOfDay = dstartDate.ToString("ddd");
                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = dstartDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = nameOfDay; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            if (oTempAttendanceDailyV2 != null)
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.InTimeInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OutTime.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OutTimeInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.TotalWorkingHourSt; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.OverTimeInMinuteHourSt; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.ShiftName; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.AttStatusInString; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oTempAttendanceDailyV2.Remark; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }
                            else
                            {
                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "--"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "No Record Found"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            }


                            nRowIndex++;
                            colIn = 1;
                            dstartDate = dstartDate.AddDays(1);
                        }

                        nRowIndex += 1;
                        colIn = 1;

                        int nSummaryStartRow = nRowIndex;
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Present Day"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, colIn]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "P", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        colIn = 1;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Absent Day"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, colIn]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "A", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIn = 1;
                        nRowIndex++;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Holiday"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, colIn]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "HD", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        colIn = 1;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Off day"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, colIn]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "Off", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nSummaryStartRow;
                        colIn += 2;
                        List<AttendanceDailyV2> oAttendanceDs = new List<AttendanceDailyV2>();
                        double nTotalLeave = 0;
                        foreach (LeaveHead item in oLeaveHeads)
                        {
                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = item.ShortName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            oAttendanceDs = oADs.Where(x => x.LeaveName != "" && x.LeaveName == item.Name).ToList();

                            cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = oAttendanceDs.Count > 0 ? oAttendanceDs.Count().ToString() : "0"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nTotalLeave += oAttendanceDs.Count;

                            if (colIn == 8)
                            {
                                nRowIndex++;
                                colIn = 4;
                            }
                        }
                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = "Total Leave"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = nTotalLeave.ToString(); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex = nSummaryStartRow;
                        int nPrevStartCol = colIn;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Late Days"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "Late", false); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        colIn = nPrevStartCol;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Late Hours"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "Late Hour"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        colIn = nPrevStartCol;


                        int nPresent = oADs.Where(x => x.AttStatusInString == "P").ToList().Count();
                        int nAbsent = oADs.Where(x => x.AttStatusInString == "A").ToList().Count();
                        int nHoliDay = oADs.Where(x => x.AttStatusInString == "HD").ToList().Count();
                        int nDayOff = oADs.Where(x => x.AttStatusInString == "Off").ToList().Count();
                        int nTLeave = (int)nTotalLeave;
                        int TotalAtt = nPresent + nAbsent + nHoliDay + nDayOff + nTLeave;
                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Total Attendance"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = TotalAtt.ToString(); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        colIn = nPrevStartCol;

                        cell = sheet.Cells[nRowIndex, colIn, nRowIndex, ++colIn]; cell.Merge = true; cell.Value = "Total OverTime"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[nRowIndex, ++colIn]; cell.Value = AttendanceDailyV2.GetAttendanceSummary(oADs, "OT"); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex += 3;
                    }
                }

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=TIME-CARD-F6.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion
        #endregion
    }
}