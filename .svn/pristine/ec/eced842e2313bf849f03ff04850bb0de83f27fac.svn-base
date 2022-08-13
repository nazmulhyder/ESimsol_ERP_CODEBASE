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
    public class AttendanceDailyV2Controller : Controller
    {

        #region Comp Actions
        public ActionResult View_CompAttendanceDaily(int menuid)
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
        [HttpPost]
        public JsonResult GetCount(HCMSearchObj oHCMSearchObj)
        {
            AttendanceDailyV2 oAttendanceDailyV2 = new AttendanceDailyV2();
            string sSQL = this.CompGetSQL(oHCMSearchObj);
            string sQuery = "SELECT COUNT(*) FROM View_MaxOTConfigurationAttendance AS MOCA WHERE MOCA.MOCID = " + oHCMSearchObj.MOCID + " AND MOCA.AttendanceDate = '" + oHCMSearchObj.StartDate.ToString("dd MMM yyyy") + "'" + sSQL;
            if (oHCMSearchObj.IsManual == true)
            {
                sQuery = sQuery + " AND MOCA.IsManual=1 ";
            }
            oAttendanceDailyV2 = AttendanceDailyV2.GetTotalCount(sQuery, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailyV2);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string CompGetSQL(HCMSearchObj oHCMSearchObj)
        {
            string sSQL = "";
           
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
            if (oHCMSearchObj.CategoryID != 0)
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
            List<AttendanceDailyV2> oAttendanceDailyV2s = new List<AttendanceDailyV2>();
            try
            {
                string sSQL = "SELECT top(" + oHCMSearchObj.LoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY Code) Row,* FROM  View_MaxOTConfigurationAttendance AS MOCA WHERE MOCA.MOCID = " + oHCMSearchObj.MOCID + " AND MOCA.AttendanceDate = '" + oHCMSearchObj.StartDate.ToString("dd MMM yyyy") + "'";
                if(oHCMSearchObj.IsManual==true)
                {
                    sSQL = sSQL + " AND MOCA.IsManual=1 ";
                }
                sSQL = sSQL + this.CompGetSQL(oHCMSearchObj) + ") aa WHERE Row >" + oHCMSearchObj.RowLength+ " ORDER BY Code";
                oAttendanceDailyV2s = AttendanceDailyV2.CompGets(sSQL, (int)(Session[SessionInfo.currentUserID]));
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

        #endregion


        #region Excel

        public void ExcelDailyAttendance_LocationWise(double ts)
        {
            DateTime paramDate;
            List<AttendanceDailyV2> oAttendanceDailyV2s = new List<AttendanceDailyV2>();
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];
            int Present=0, Absent=0, DayOff=0, Holiday=0, Late=0, EarlyLeaving=0, OT=0, NoWork=0, Leave=0, NoOutTime=0;
            string sSQL = "SELECT * FROM  View_MaxOTConfigurationAttendance AS MOCA WHERE MOCA.MOCID = " + oHCMSearchObj.MOCID + " AND MOCA.AttendanceDate = '" + oHCMSearchObj.StartDate.ToString("dd MMM yyyy") + "'";
            if (oHCMSearchObj.IsManual == true)
            {
                sSQL = sSQL + " AND MOCA.IsManual=1 ";
            }
            sSQL = sSQL + this.CompGetSQL(oHCMSearchObj) +" ORDER BY Code";
            oAttendanceDailyV2s = AttendanceDailyV2.CompGets(sSQL, (int)(Session[SessionInfo.currentUserID]));





            string EmpIDs = string.Join(",", oAttendanceDailyV2s.Where(x => x.AttendanceID > 0).Select(p => p.EmployeeID).Distinct().ToList());
            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            if (oAttendanceDailyV2s.Count > 0)
            {
                oEmployeeSalaryStructures = EmployeeSalaryStructure.Gets("SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID IN(" + EmpIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

            int nMaxColumn = 0;
            int nStartCol = 1, nEndCol = 13;
            int colIndex = 1;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            string makeFormat = "";


            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Location Wise Daily Attendance");
                sheet.Name = "LocationWiseDailyAttendace";

                int n = 1;
                sheet.Column(n++).Width = 6; //SL
                sheet.Column(n++).Width = 15; //CODE
                sheet.Column(n++).Width = 25; //NAME
                sheet.Column(n++).Width = 20; //DSIGNATION
                sheet.Column(n++).Width = 20; //DOJ
                if (bView) { sheet.Column(n++).Width = 20; } //GROSS
                sheet.Column(n++).Width = 20; //Shift
                sheet.Column(n++).Width = 10; //IN TIME
                sheet.Column(n++).Width = 10; //OUT TIME
                sheet.Column(n++).Width = 10; //Total Hour
                sheet.Column(n++).Width = 10; //LATE(Hr)
                sheet.Column(n++).Width = 10; //EARLY LEAVING(Hr)
                sheet.Column(n++).Width = 15; //DESCTIPTION
                sheet.Column(n++).Width = 10; //OT(Hr)
                sheet.Column(n++).Width = 15; //REMARK

                nMaxColumn = n;

                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 3]; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Font.Bold = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Font.Size = 20;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date : " + Date; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[rowIndex, 3]; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Font.Bold = true; cell.Value = "Date : " + oHCMSearchObj.StartDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Font.Size = 20;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;


                //sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                //cell = sheet.Cells[rowIndex, 2]; cell.Value = "Location : " + _oAttendanceDaily.LocationName; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                //rowIndex = rowIndex + 2;
                #endregion

                #region Table Body
                int nSL = 0;

                var LocWise = oAttendanceDailyV2s.GroupBy(x => x.LocationID).Select(grp => new
                {
                    LocationID = grp.Key,
                    LocationName = grp.First().LocationName,
                    Result = grp
                }).ToList().OrderBy(x => x.LocationName);

                foreach (var data in LocWise)
                {
                    nSL = 0;
                    colIndex = 1;

                    //cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn];
                    cell = sheet.Cells[rowIndex, 2];
                    cell.Style.Font.Bold = true; cell.Value = "Location : " + data.LocationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;

                    #region Table Header 02
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Date Of Join"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    if (bView)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "GROSS"; cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SHIFT"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "IN TIME"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OUT TIME"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total (Hr)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LATE(Hr)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EARLY LEAVING(Hr)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESCRIPTION"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT(Hr)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "REMARKS"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                    #endregion





                    foreach (var oItem in data.Result)
                    {
                        //foreach (AttendanceDaily oItem in _oAttendanceDaily.AttendanceDailys)
                        //{
                        nSL++;
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.JoiningDateInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (bView)
                        {
                            List<EmployeeSalaryStructure> oEmpSSs = new List<EmployeeSalaryStructure>();
                            oEmpSSs = oEmployeeSalaryStructures.Where(x => x.EmployeeID == oItem.EmployeeID).ToList();

                            double nGross = 0;
                            if (oEmpSSs.Count > 0) { nGross = oEmpSSs[0].GrossAmount; }

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nGross; cell.Style.Font.Bold = false;
                            cell.Style.Numberformat.Format = "#,##0.00";
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ShiftName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        makeFormat = oItem.InTimeInString;


                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = makeFormat; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                     
                       makeFormat = oItem.OutTimeInString;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = makeFormat; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalWorkingHourSt; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LateArrivalMinute; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EarlyDepartureMinute; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AttStatusInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.OverTimeInMinuteHourSt; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Remark; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;

                        if (oItem.OverTimeInMin > 0)
                        {
                            OT = OT + oItem.OverTimeInMin;
                        }
                        if (oItem.EarlyDepartureMinute > 0) EarlyLeaving++;
                        if (oItem.LateArrivalMinute > 0) Late++;
                        if (oItem.IsLeave == true) Leave++;
                       // if (oItem. == true) NoWork++;
                        if (oItem.IsDayOff == true) DayOff++;
                        if (oItem.IsHoliday == true) Holiday++;
                        if (oItem.OutTimeInString == "-") NoOutTime++;
                        if ((oItem.InTimeInString != "-" || oItem.OutTimeInString != "-" || oItem.IsOSD == true) && oItem.IsLeave == false && oItem.IsHoliday == false) Present++;
                        if (oItem.InTimeInString == "-" && oItem.OutTimeInString == "-" && oItem.IsLeave == false && oItem.IsHoliday == false && oItem.IsDayOff == false && oItem.IsOSD == false) Absent++;

                    }

                    rowIndex++;
                    rowIndex++;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 3];
                    cell.Style.Font.Bold = true; cell.Value = "Present "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4];
                    cell.Style.Font.Bold = true; cell.Value = Present; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 3];
                    cell.Style.Font.Bold = true; cell.Value = "Absent "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4];
                    cell.Style.Font.Bold = true; cell.Value = Absent; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    cell = sheet.Cells[rowIndex, 3];
                    cell.Style.Font.Bold = true; cell.Value = "Dayoff "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4];
                    cell.Style.Font.Bold = true; cell.Value = DayOff; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    cell = sheet.Cells[rowIndex, 3];
                    cell.Style.Font.Bold = true; cell.Value = "Holiday "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4];
                    cell.Style.Font.Bold = true; cell.Value = Holiday; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 3];
                    cell.Style.Font.Bold = true; cell.Value = "Late "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4];
                    cell.Style.Font.Bold = true; cell.Value = Late; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 3];
                    cell.Style.Font.Bold = true; cell.Value = "EarlyLeaving "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4];
                    cell.Style.Font.Bold = true; cell.Value = EarlyLeaving; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    cell = sheet.Cells[rowIndex, 3];
                    cell.Style.Font.Bold = true; cell.Value = "OT "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    int nHour = (OT / 60);
                    int nMin = (OT % 60);
                    string sOTHourAndMin = "";
                    if (nHour > 0)
                    {
                        sOTHourAndMin = nHour.ToString() + " H,";
                    }
                    if (nMin > 0)
                    {
                        sOTHourAndMin = sOTHourAndMin + nMin.ToString() + " M,";
                    }
                    if (sOTHourAndMin.Length > 0)
                    {
                        sOTHourAndMin = sOTHourAndMin.Remove(sOTHourAndMin.Length - 1, 1);
                    }
                    cell = sheet.Cells[rowIndex, 4];
                    cell.Style.Font.Bold = true; cell.Value = sOTHourAndMin; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    cell = sheet.Cells[rowIndex, 3];
                    cell.Style.Font.Bold = true; cell.Value = "No Work "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4];
                    cell.Style.Font.Bold = true; cell.Value = NoWork; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    cell = sheet.Cells[rowIndex, 3];
                    cell.Style.Font.Bold = true; cell.Value = "Leave "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4];
                    cell.Style.Font.Bold = true; cell.Value = Leave; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;
                    cell = sheet.Cells[rowIndex, 3];
                    cell.Style.Font.Bold = true; cell.Value = "No Out Time "; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4];
                    cell.Style.Font.Bold = true; cell.Value = NoOutTime; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    rowIndex++;

                }
                #endregion


                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LocationWiseDailyAttendance.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion

        #region Print

        public ActionResult PrintDailyAttendance_LocationWise( double ts)
        {
            DateTime paramDate;
            List<AttendanceDailyV2> oAttendanceDailyV2s = new List<AttendanceDailyV2>();
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];
            int Present = 0, Absent = 0, DayOff = 0, Holiday = 0, Late = 0, EarlyLeaving = 0, OT = 0, NoWork = 0, Leave = 0, NoOutTime = 0;
            string sSQL = "SELECT * FROM  View_MaxOTConfigurationAttendance AS MOCA WHERE MOCA.MOCID = " + oHCMSearchObj.MOCID + " AND MOCA.AttendanceDate = '" + oHCMSearchObj.StartDate.ToString("dd MMM yyyy") + "'";
            if (oHCMSearchObj.IsManual == true)
            {
                sSQL = sSQL + " AND MOCA.IsManual=1 ";
            }
            sSQL = sSQL + this.CompGetSQL(oHCMSearchObj) + " ORDER BY Code";
            oAttendanceDailyV2s = AttendanceDailyV2.CompGets(sSQL, (int)(Session[SessionInfo.currentUserID]));



            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)(Session[SessionInfo.currentUserID]));

            rptDailyAttendance_LocationWiseV2 oReport = new rptDailyAttendance_LocationWiseV2();
            byte[] abytes = oReport.PrepareReport(oAttendanceDailyV2s, oCompany);
            return File(abytes, "application/pdf");
        }
        #endregion


    }
}