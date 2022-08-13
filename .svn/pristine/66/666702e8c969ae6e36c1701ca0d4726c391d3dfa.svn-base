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


namespace ESimSolFinancial.Controllers
{
    public class HCMXLController : Controller
    {

        #region Declaration

        #endregion

        #region Views
        public ActionResult View_HCMReportXLs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<AttendanceDaily>  oAttendanceDailys = new List<AttendanceDaily>();
            return View(oAttendanceDailys);
        }

        #endregion

        public ActionResult PrintMonthlyAbsentList_MAMIYA_XL(string dtStartDate, string dtEndDate,int nShiftID, int nLocationID, string sDepartmentIDs, string sDesignationIDs, double ts)
        {
            List<AttendanceDaily>  oAttendanceDailys = new List<AttendanceDaily>();
            string sSql = "SELECT * FROM View_AttendanceDaily WHERE  EmployeeID IN ("
            + " SELECT EmployeeID FROM Employee WHERE IsActive=1)"
            + " AND IsDayOff=0 AND IsLeave=0 AND IsHoliday=0 AND (IsOSD=0 OR IsOSD IS NULL) AND (CAST(InTime AS TIME(0))='00:00:00' AND CAST(OutTime AS TIME(0))='00:00:00') AND AttendanceDate BETWEEN '" + dtStartDate + "' AND '" + dtEndDate + "' ";

            if (nShiftID > 0)
            {
                sSql += " AND ShiftID=" + nShiftID;
            }
            if (nLocationID>0)
            {
                sSql += " AND LocationID=" + nLocationID;
            }
            if (sDepartmentIDs!="" && sDepartmentIDs!= null)
            {
                sSql += " AND DepartmentID IN(" + sDepartmentIDs+")";
            }
            if (sDesignationIDs != "" && sDesignationIDs != null)
            {
                sSql += " AND DesignationID IN(" + sDesignationIDs + ")";
            }
            sSql += " ORDER BY Code";
            oAttendanceDailys = AttendanceDaily.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<MonthlyAbsentListXL>));

            MonthlyAbsentListXL oAbsentList = new MonthlyAbsentListXL();
            List<MonthlyAbsentListXL> oAbsentLists = new List<MonthlyAbsentListXL>();

            int nCount = 0;
            foreach (AttendanceDaily oItem in oAttendanceDailys)
            {
                nCount++;
                oAbsentList = new MonthlyAbsentListXL();
                oAbsentList.SL = nCount.ToString();
                oAbsentList.Code = oItem.EmployeeCode;
                oAbsentList.Name = oItem.EmployeeName;
                oAbsentList.Designation = oItem.DesignationName;
                oAbsentList.Department = oItem.DepartmentName;
                oAbsentList.AttendanceDate = oItem.AttendanceDateInString;
                oAbsentList.Shift = oItem.HRM_ShiftName;
                oAbsentList.InTime = oItem.InTimeInString;
                oAbsentList.OutTime = oItem.OutTimeInString;

                string sStatus = "";
                if(oItem.WorkingStatus==EnumEmployeeWorkigStatus.InWorkPlace){sStatus="Continued";}
                else if(oItem.WorkingStatus==EnumEmployeeWorkigStatus.Discontinued){sStatus="Discontinued";}

                oAbsentList.Status = sStatus;

                oAbsentLists.Add(oAbsentList);
            }

            serializer.Serialize(stream, oAbsentLists);
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "AbsentList.xls");

        }

        public ActionResult PrintOTSheet_MAMIYA_XL(DateTime dtStartDate, DateTime dtEndDate, double ts)
        {
            List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();
            oEmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_OTSheet(dtStartDate, dtEndDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<OTSheetXL>));
            if(!bView){
            serializer= new XmlSerializer(typeof(List<OTSheetWithOutSalaryXL>));}

            OTSheetXL oOTSheet = new OTSheetXL();
            List<OTSheetXL> oOTSheets = new List<OTSheetXL>();
             int nCount = 0;
            if(bView)
            {
            foreach (EmployeeSalary_MAMIYA oItem in oEmployeeSalary_MAMIYAs)
            {
                nCount++;
                oOTSheet = new OTSheetXL();
                oOTSheet.SL = nCount.ToString();
                oOTSheet.Code = oItem.EmployeeCode;
                oOTSheet.Name = oItem.EmployeeName;
                oOTSheet.Designation = oItem.DesignationName;
                oOTSheet.Department = oItem.DepartmentName;
                oOTSheet.DateOfJoin = oItem.DateOfJoinInString;
                oOTSheet.Basic = Global.MillionFormat(oItem.Basic);
                oOTSheet.HRent = Global.MillionFormat(oItem.HRent);
                oOTSheet.Medical = Global.MillionFormat(oItem.Med);
                oOTSheet.Gross = Global.MillionFormat(oItem.GrossSalary);
                oOTSheet.OT_NHR = Global.MillionFormat(oItem.OT_NHR);
                oOTSheet.OT_HHR = Global.MillionFormat(oItem.OT_HHR);
                oOTSheet.FHOT = Global.MillionFormat(oItem.FHOT);
                oOTSheet.OTAmount = Global.MillionFormat(oItem.OTAmount);
                oOTSheet.Status = oItem.EmployeeWorkingStatus;

                oOTSheets.Add(oOTSheet);
            }
            }
         OTSheetWithOutSalaryXL oOTSheet_WithoutSalary = new OTSheetWithOutSalaryXL();
            List<OTSheetWithOutSalaryXL> oOTSheet_WithoutSalarys = new List<OTSheetWithOutSalaryXL>();
            if(!bView)
            {
            foreach (EmployeeSalary_MAMIYA oItem in oEmployeeSalary_MAMIYAs)
            {
                nCount++;
                oOTSheet_WithoutSalary = new OTSheetWithOutSalaryXL();
                oOTSheet_WithoutSalary.SL = nCount.ToString();
                oOTSheet_WithoutSalary.Code = oItem.EmployeeCode;
                oOTSheet_WithoutSalary.Name = oItem.EmployeeName;
                oOTSheet_WithoutSalary.Designation = oItem.DesignationName;
                oOTSheet_WithoutSalary.Department = oItem.DepartmentName;
                oOTSheet_WithoutSalary.DateOfJoin = oItem.DateOfJoinInString;
                oOTSheet_WithoutSalary.OT_NHR = Global.MillionFormat(oItem.OT_NHR);
                oOTSheet_WithoutSalary.OT_HHR = Global.MillionFormat(oItem.OT_HHR);
                oOTSheet_WithoutSalary.FHOT = Global.MillionFormat(oItem.FHOT);
                oOTSheet_WithoutSalary.OTAmount = Global.MillionFormat(oItem.OTAmount);
                oOTSheet_WithoutSalary.Status = oItem.EmployeeWorkingStatus;

                oOTSheet_WithoutSalarys.Add(oOTSheet_WithoutSalary);
            }
            }
            if(bView)
            {serializer.Serialize(stream, oOTSheets);}
            else{serializer.Serialize(stream, oOTSheet_WithoutSalarys);}
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "OTSheet.xls");

        }

        public ActionResult PrintLeaveReport_XL(DateTime dtStartDate, DateTime dtEndDate, double ts)
        {
            List<LeaveReport_XL> oLeaveReport_XLs = new List<LeaveReport_XL>();
            oLeaveReport_XLs = LeaveReport_XL.Gets(dtStartDate, dtEndDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<LeaveReportXL>));

            LeaveReportXL oLeaveReportXL = new LeaveReportXL();
            List<LeaveReportXL> oLeaveReportXLs = new List<LeaveReportXL>();

            int nCount = 0;
            foreach (LeaveReport_XL oItem in oLeaveReport_XLs)
            {
                nCount++;
                oLeaveReportXL = new LeaveReportXL();
                oLeaveReportXL.SL = nCount.ToString();
                oLeaveReportXL.Code = oItem.EmployeeCode;
                oLeaveReportXL.Name = oItem.EmployeeName;
                oLeaveReportXL.Designation = oItem.DesignationName;
                oLeaveReportXL.Department = oItem.DepartmentName;
                oLeaveReportXL.DateOfJoin = oItem.DateOfJoinInString;
                oLeaveReportXL.CLDays = Global.MillionFormat(oItem.CL);
                oLeaveReportXL.SLDays = Global.MillionFormat(oItem.SL);
                oLeaveReportXL.ELDays = Global.MillionFormat(oItem.EL);
                oLeaveReportXL.LWPDays = Global.MillionFormat(oItem.LWP);
                oLeaveReportXL.Short_Or_Half = Global.MillionFormat(oItem.shortLeave);
        
                oLeaveReportXLs.Add(oLeaveReportXL);
            }

            serializer.Serialize(stream, oLeaveReportXLs);
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "Leave Report.xls");

        }

        public ActionResult PrintAbsentAmountReport_XL(DateTime dtStartDate, DateTime dtEndDate, double ts)
        {
            List<AbsentAmount_XL> oAbsentAmount_XLs = new List<AbsentAmount_XL>();
            oAbsentAmount_XLs = AbsentAmount_XL.Gets(dtStartDate, dtEndDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<AbsentAmountXL>));
            if (!bView)
            {
                serializer = new XmlSerializer(typeof(List<AbsentAmountWithouTsALARYXL>));
            }

            int nCount = 0;
            AbsentAmountXL oAbsentAmountXL = new AbsentAmountXL();
            List<AbsentAmountXL> oAbsentAmountXLs = new List<AbsentAmountXL>();
            if (bView)
            {
                
                foreach (AbsentAmount_XL oItem in oAbsentAmount_XLs)
                {
                    nCount++;
                    oAbsentAmountXL = new AbsentAmountXL();
                    oAbsentAmountXL.SL = nCount.ToString();
                    oAbsentAmountXL.Code = oItem.EmployeeCode;
                    oAbsentAmountXL.Name = oItem.EmployeeName;
                    oAbsentAmountXL.Designation = oItem.DesignationName;
                    oAbsentAmountXL.Department = oItem.DepartmentName;
                    oAbsentAmountXL.DateOfJoin = oItem.DOJInString;
                    oAbsentAmountXL.Basic = Global.MillionFormat(oItem.Basic);
                    oAbsentAmountXL.HRent = Global.MillionFormat(oItem.HRent);
                    oAbsentAmountXL.Medical = Global.MillionFormat(oItem.Medical);
                    oAbsentAmountXL.GrossAmount = Global.MillionFormat(oItem.GrossAmount);
                    oAbsentAmountXL.AbsentHR_Sick = Global.MillionFormat(oItem.Sick);
                    oAbsentAmountXL.AbsentHR_LWP = Global.MillionFormat(oItem.LWP);
                    oAbsentAmountXL.GrossAmount = Global.MillionFormat(oItem.GrossAmount);
                    oAbsentAmountXL.TotalAbsentAmount = Global.MillionFormat(oItem.TotalAbsentAmount);
                    oAbsentAmountXL.Status = oItem.EmployeeWorkingStatus;
                    oAbsentAmountXLs.Add(oAbsentAmountXL);
                }
            }

            AbsentAmountWithouTsALARYXL oAbsentAmountWithouTsALARYXL = new AbsentAmountWithouTsALARYXL();
            List<AbsentAmountWithouTsALARYXL> oAbsentAmountWithouTsALARYXLs = new List<AbsentAmountWithouTsALARYXL>();

            if(!bView)
            {
                foreach (AbsentAmount_XL oItem in oAbsentAmount_XLs)
                {
                    nCount++;
                    oAbsentAmountWithouTsALARYXL = new AbsentAmountWithouTsALARYXL();
                    oAbsentAmountWithouTsALARYXL.SL = nCount.ToString();
                    oAbsentAmountWithouTsALARYXL.Code = oItem.EmployeeCode;
                    oAbsentAmountWithouTsALARYXL.Name = oItem.EmployeeName;
                    oAbsentAmountWithouTsALARYXL.Designation = oItem.DesignationName;
                    oAbsentAmountWithouTsALARYXL.Department = oItem.DepartmentName;
                    oAbsentAmountWithouTsALARYXL.DateOfJoin = oItem.DOJInString;
                    oAbsentAmountWithouTsALARYXL.AbsentHR_Sick = Global.MillionFormat(oItem.Sick);
                    oAbsentAmountWithouTsALARYXL.AbsentHR_LWP = Global.MillionFormat(oItem.LWP);
                    oAbsentAmountWithouTsALARYXL.TotalAbsentAmount = Global.MillionFormat(oItem.TotalAbsentAmount);
                    oAbsentAmountWithouTsALARYXL.Status = oItem.EmployeeWorkingStatus;
                    oAbsentAmountWithouTsALARYXLs.Add(oAbsentAmountWithouTsALARYXL);
                }
            }
            if (!bView)
            { serializer.Serialize(stream, oAbsentAmountWithouTsALARYXLs); }
            else { serializer.Serialize(stream, oAbsentAmountXLs); }
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "Absent Amont Report.xls");

        }

        public ActionResult PrintShiftAllReport_XL(DateTime dtStartDate, DateTime dtEndDate, double ts)
        {
            List<ShiftAllowance_XL> oShiftAllowance_XLs = new List<ShiftAllowance_XL>();
            oShiftAllowance_XLs = ShiftAllowance_XL.Gets(dtStartDate, dtEndDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            bool bView = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bView = true; }

            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<ShiftAllowanceXL>));
            if (!bView)
            {
                serializer = new XmlSerializer(typeof(List<ShiftAllowanceWithOutSalaryXL>));
            }

            int nCount = 0;

            ShiftAllowanceXL oShiftAllowanceXL = new ShiftAllowanceXL();
            List<ShiftAllowanceXL> oShiftAllowanceXLs = new List<ShiftAllowanceXL>();

            if (bView)
            {
                foreach (ShiftAllowance_XL oItem in oShiftAllowance_XLs)
                {
                    nCount++;
                    oShiftAllowanceXL = new ShiftAllowanceXL();
                    oShiftAllowanceXL.SL = nCount.ToString();
                    oShiftAllowanceXL.Code = oItem.EmployeeCode;
                    oShiftAllowanceXL.Name = oItem.EmployeeName;
                    oShiftAllowanceXL.Designation = oItem.DesignationName;
                    oShiftAllowanceXL.Department = oItem.DepartmentName;
                    oShiftAllowanceXL.DateOfJoin = oItem.DOJInString;
                    oShiftAllowanceXL.Basic = Global.MillionFormat(oItem.Basic);
                    oShiftAllowanceXL.HRent = Global.MillionFormat(oItem.HRent);
                    oShiftAllowanceXL.Medical = Global.MillionFormat(oItem.Medical);
                    oShiftAllowanceXL.GrossAmount = Global.MillionFormat(oItem.GrossAmount);
                    oShiftAllowanceXL.TotalShiftDay = Global.MillionFormat(oItem.TotalShiftDay);
                    oShiftAllowanceXL.Shift_Allowance = Global.MillionFormat(oItem.Value);
                    oShiftAllowanceXL.ShiftAmount = Global.MillionFormat(oItem.ShiftAmount);
                    oShiftAllowanceXL.Status = oItem.EmployeeWorkingStatus;

                    oShiftAllowanceXLs.Add(oShiftAllowanceXL);
                }
            }

            ShiftAllowanceWithOutSalaryXL oShiftAllowanceWithOutSalaryXL = new ShiftAllowanceWithOutSalaryXL();
            List<ShiftAllowanceWithOutSalaryXL> oShiftAllowanceWithOutSalaryXLs = new List<ShiftAllowanceWithOutSalaryXL>();

            if (!bView)
            {
                foreach (ShiftAllowance_XL oItem in oShiftAllowance_XLs)
                {
                    nCount++;
                    oShiftAllowanceWithOutSalaryXL = new ShiftAllowanceWithOutSalaryXL();
                    oShiftAllowanceWithOutSalaryXL.SL = nCount.ToString();
                    oShiftAllowanceWithOutSalaryXL.Code = oItem.EmployeeCode;
                    oShiftAllowanceWithOutSalaryXL.Name = oItem.EmployeeName;
                    oShiftAllowanceWithOutSalaryXL.Designation = oItem.DesignationName;
                    oShiftAllowanceWithOutSalaryXL.Department = oItem.DepartmentName;
                    oShiftAllowanceWithOutSalaryXL.DateOfJoin = oItem.DOJInString;
                    oShiftAllowanceWithOutSalaryXL.TotalShiftDay = Global.MillionFormat(oItem.TotalShiftDay);
                    oShiftAllowanceWithOutSalaryXL.Shift_Allowance = Global.MillionFormat(oItem.Value);
                    oShiftAllowanceWithOutSalaryXL.ShiftAmount = Global.MillionFormat(oItem.ShiftAmount);
                    oShiftAllowanceWithOutSalaryXL.Status = oItem.EmployeeWorkingStatus;

                    oShiftAllowanceWithOutSalaryXLs.Add(oShiftAllowanceWithOutSalaryXL);
                }
            }
            if (!bView)
            { serializer.Serialize(stream, oShiftAllowanceWithOutSalaryXLs); }
            else { serializer.Serialize(stream, oShiftAllowanceXLs); }

           
            stream.Position = 0;
            return File(stream, "application/vnd.ms-excel", "Shift Allowance Report.xls");

        }

        #region Leave Report XL
        public void LeaveReportXL(DateTime dtStartDate, DateTime dtEndDate, double ts)
        {
            //AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            //List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            //List<AttendanceDaily> oADs = new List<AttendanceDaily>();
            //List<AttendanceDaily> oTempADs = new List<AttendanceDaily>();
            //List<LeaveHead> oLeaveHeads = new List<LeaveHead>();

            //string sSql = "SELECT Code, EmployeeName , Department, Designation, JoiningDate , LeaveHeadID , LeaveStatus,"
            //+ "COUNT(AttendanceID) AS TotalLeave   FROM View_AttendanceDaily WHERE  AttendanceDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy")
            //+ "' AND '" + dtStartDate.ToString("dd MMM yyyy") + "' AND  LeaveHeadID>0 GROUP BY  Code, EmployeeName , Department, "
            //+ " Designation, JoiningDate, LeaveHeadID , LeaveStatus ORDER BY Code";

            //oAttendanceDailys = AttendanceDaily.LeaveReportGets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //oLeaveHeads = LeaveHead.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID).OrderBy(x=>x.LeaveHeadID).ToList();

            List<LeaveReport_XL> oLeaveReport_XLs = new List<LeaveReport_XL>();
            oLeaveReport_XLs = LeaveReport_XL.Gets(dtStartDate, dtEndDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);


            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("LEAVE");
                sheet.Name = "LEAVE";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 15; //CODE
                sheet.Column(4).Width = 20; //NAME
                sheet.Column(5).Width = 20; //DEPARTMENT
                sheet.Column(6).Width = 20; //DESIGNATION
                sheet.Column(7).Width = 12; //JOINING
                sheet.Column(8).Width = 10; //CLDays
                sheet.Column(9).Width = 10; //SLDays
                sheet.Column(10).Width = 10; //ELDays
                sheet.Column(11).Width = 10; //LWPDays
                sheet.Column(12).Width = 12; //Short_Or_Half
                sheet.Column(13).Width = 12; //Status
                nMaxColumn = 13;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "LEAVE REPORT"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DEPARTMENT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "JOINING"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "EL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LWP"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Short/Half"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                #endregion

                #region Table Body

                    int nSL = 0;
                    foreach (LeaveReport_XL oLR in oLeaveReport_XLs)
                    {
                        nSL++;
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.EmployeeName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.DepartmentName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.DesignationName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.DateOfJoinInString; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.CL; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.SL; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.EL; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.LWP; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.shortLeave; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oLR.EmployeeWorkingStatus; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                    }
                    
                
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LEAVE.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void LeaveReportXL_F2(DateTime dtStartDate, DateTime dtEndDate, double ts)
        {
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            List<AttendanceDaily> oADs = new List<AttendanceDaily>();
            List<AttendanceDaily> oTempADs = new List<AttendanceDaily>();
            List<LeaveHead> oLeaveHeads = new List<LeaveHead>();

            string sSql = "SELECT Code, EmployeeName , Department, Designation, AttendanceDate ,HRM_Shift , LeaveStatus,WorkingStatus "
            + "  FROM View_AttendanceDaily WHERE  AttendanceDate BETWEEN '" + dtStartDate.ToString("dd MMM yyyy")
            + "' AND '" + dtEndDate.ToString("dd MMM yyyy") + "' AND  LeaveHeadID>0 GROUP BY  Code, EmployeeName , Department, "
            + " Designation, AttendanceDate,HRM_Shift, LeaveStatus,WorkingStatus ORDER BY Code,AttendanceDate";

            oAttendanceDailys = AttendanceDaily.LeaveReportGets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //oLeaveHeads = LeaveHead.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID).OrderBy(x => x.LeaveHeadID).ToList();

            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 2;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("LEAVE");
                sheet.Name = "LEAVE";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 15; //CODE
                sheet.Column(4).Width = 22; //NAME
                sheet.Column(5).Width = 20; //DEPARTMENT
                sheet.Column(6).Width = 22; //DESIGNATION
                sheet.Column(7).Width = 14; //ATTENDANCE DATE
                sheet.Column(8).Width = 10; //SHIFT
                sheet.Column(9).Width = 12; //LEAVE
                sheet.Column(10).Width = 12; //STATUS

                nMaxColumn = 10;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "LEAVE REPORT"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DEPARTMENT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ATTENDANCE DATE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SHIFT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LEAVE TYPE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "STATUS"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                rowIndex++;
                #endregion

                #region Table Body

                int nSL = 0;
                foreach (AttendanceDaily oAD in oAttendanceDailys)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oAD.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oAD.EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oAD.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oAD.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oAD.AttendanceDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oAD.HRM_ShiftName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oAD.LeaveStatus; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oAD.EmployeeWorkingStatusInST; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                }

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LEAVE.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #endregion Leave Report XL
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
    }
}
