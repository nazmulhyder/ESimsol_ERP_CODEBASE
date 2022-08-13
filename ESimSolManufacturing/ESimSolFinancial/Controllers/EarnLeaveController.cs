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
using System.Net.Mail;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class EarnLeaveController : Controller
    {
        #region Declaration
        EarnLeave _oEarnLeave;
        ELEncashCompliance _oELEncashCompliance;
        ELEncashComplianceDetail _oELEncashComplianceDetail;
        private List<EarnLeave> _oEarnLeaves;
        private List<ELEncashCompliance> _oELEncashCompliances;
        private List<ELEncashComplianceDetail> _oELEncashComplianceDetails;
        #endregion

        #region View
        public ActionResult View_EarnLeaves(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<EarnLeave> oEarnLeaves = new List<EarnLeave>();

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ELProcess).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            string sSQL = "";
            sSQL = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSQL = sSQL + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));

            return View(oEarnLeaves);
        }
        #endregion

        #region Search
        [HttpPost]
        public JsonResult ELSearch(string sEmployeeIDs, string sLocationID, string sDepartmentIds, string sDesignationIDs, DateTime dtUptodate, string sBUnit, int nLoadRecordsS, int nRowLengthS, double ts)
        {
            List<EarnLeave> oEarnLeaves = new List<EarnLeave>();
            try
            {
                oEarnLeaves = EarnLeave.ELGets(sEmployeeIDs, sLocationID, sDepartmentIds, sDesignationIDs, dtUptodate, sBUnit, nLoadRecordsS, nRowLengthS, 0,((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oEarnLeaves.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oEarnLeaves = new List<EarnLeave>();
                EarnLeave oEarnLeave = new EarnLeave();
                oEarnLeave.ErrorMessage = ex.Message;
                oEarnLeaves.Add(oEarnLeave);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEarnLeaves);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
            
        [HttpPost]
        public JsonResult ELSearchClassified(string sEmployeeIDs, string sLocationID, string sDepartmentIds, string sDesignationIDs, string sBUnit, int nLoadRecordsS, int nRowLengthS, double ts)
        {
            List<EarnLeave> oEarnLeaves = new List<EarnLeave>();
            try
            {
                string sSQL = "SELECT top(" + nLoadRecordsS + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeID) Row,* FROM View_ELProcess WHERE EmployeeID<>0 ";
                if (!string.IsNullOrEmpty(sEmployeeIDs))
                {
                    sSQL += "AND EmployeeID IN(" + sEmployeeIDs + ")";
                }
                if (!string.IsNullOrEmpty(sLocationID))
                {
                    sSQL += "AND LocationID IN(" + sLocationID + ")";
                }
                if (!string.IsNullOrEmpty(sDepartmentIds))
                {
                    sSQL += "AND DepartmentID IN(" + sDepartmentIds + ")";
                }
                if (!string.IsNullOrEmpty(sDesignationIDs))
                {
                    sSQL += "AND DesignationID IN(" + sDesignationIDs + ")";
                }
                if (!string.IsNullOrEmpty(sBUnit))
                {
                    sSQL += "AND BusinessUnitID IN(" + sBUnit + ")";
                }
                sSQL = sSQL + ") aa WHERE Row >" + nRowLengthS;

                oEarnLeaves = EarnLeave.ELGetsClassified(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oEarnLeaves.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oEarnLeaves = new List<EarnLeave>();
                EarnLeave oEarnLeave = new EarnLeave();
                oEarnLeave.ErrorMessage = ex.Message;
                oEarnLeaves.Add(oEarnLeave);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEarnLeaves);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Search

        #region Process
        [HttpPost]
        public JsonResult ELProcess(int nEmpLeaveLedgerID, DateTime LastProcessDate)
        {
            EarnLeave oEarnLeave = new EarnLeave();
            try
            {
                oEarnLeave = oEarnLeave.ELProcess(nEmpLeaveLedgerID, LastProcessDate, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oEarnLeave = new EarnLeave();
                oEarnLeave.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEarnLeave);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MultipleELProcess(string sEmployeeIDs, string sBUnit, string sLocationID, string sDepartmentIds, string sDesignationIDs, DateTime dtUptodate, string sEmpLeaveLedgerIDs)
        {
            EarnLeave oEarnLeave = new EarnLeave();
            List<EmployeeLeaveLedger> oEmployeeLeaveLedgers = new List<EmployeeLeaveLedger>();
            try
            {
                if(sEmpLeaveLedgerIDs=="")
                {
                    string sSql="";
                    sSql="SELECT * FROM View_EmployeeLeaveLedger WHERE IsLeaveOnPresence=1 AND ACSID IN ( "
                        +"SELECT MAX(ACSID) FROM EmployeeLeaveLedger)";
                    if (sEmployeeIDs != "")
                    {
                        sSql = sSql + " AND EmployeeID IN(" + sEmployeeIDs + ")";
                    }
                    if (sBUnit != "")
                    {
                        sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID  FROM Employeeofficial WHERE DRPID IN(SELECT DepartmentRequirementPolicyID FROm DepartmentRequirementPolicy WHERE BUSINESSUNITID IN(" + sBUnit + ")))";
                    }
                    if (sLocationID!="")
                    {
                        sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM View_EmployeeOfficialALL WHERE locationID IN( " + sLocationID+ "))";
                    }
                    if (sDepartmentIds != "")
                    {
                        sSql = sSql + " AND  EmployeeID IN(SELECT EmployeeID FROM View_Employee WHERE DepartmentID IN(" + sDepartmentIds + "))";
                    }
                    if (sDesignationIDs != "")
                    {
                        sSql = sSql + " AND  EmployeeID IN(SELECT EmployeeID FROM View_Employee WHERE DesignationID IN(" + sDesignationIDs + "))";
                    }
                    if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                    {
                        sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID  FROM Employeeofficial WHERE DRPID  " 
                                    + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
                    }
                    oEmployeeLeaveLedgers = EmployeeLeaveLedger.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    sEmpLeaveLedgerIDs = string.Join(",", oEmployeeLeaveLedgers.Select(x => x.EmpLeaveLedgerID));
                }
                string[] sEmpLLIDs;
                sEmpLLIDs = sEmpLeaveLedgerIDs.Split(',');
                foreach (string sEmpLeaveLedgerID in sEmpLLIDs)
                {
                    int nEmpLLID = Convert.ToInt32(sEmpLeaveLedgerID);
                    oEarnLeave = oEarnLeave.ELProcess(nEmpLLID, dtUptodate, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oEarnLeave = new EarnLeave();
                oEarnLeave.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEarnLeave);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Process

        #region XL
        public void EL_XL(string sParam)
        {
            string sEmployeeIDs = sParam.Split('~')[0];
            string sLocationID = sParam.Split('~')[1];
            string  sDepartmentIds = sParam.Split('~')[2];
            string sDesignationIds = sParam.Split('~')[3];
            DateTime dtUptodate = Convert.ToDateTime(sParam.Split('~')[4]);
            string sBUnit = sParam.Split('~')[5];

            EarnLeave oEarnLeave = new EarnLeave();
            List<EarnLeave> oEarnLeaves = new List<EarnLeave>();
            oEarnLeaves = EarnLeave.ELGets(sEmployeeIDs, sLocationID, sDepartmentIds, sDesignationIds, dtUptodate, sBUnit, 0, 0, 1,((User)(Session[SessionInfo.CurrentUser])).UserID);

            oEarnLeaves = oEarnLeaves.OrderBy(x=>x.Code).ToList();

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
                var sheet = excelPackage.Workbook.Worksheets.Add("Earn Leave");
                sheet.Name = "Earn Leave";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //CODE
                sheet.Column(4).Width = 20; //NAME
                sheet.Column(5).Width = 20; //DEPARTEMNT
                sheet.Column(6).Width = 20; //DSIGNATION
                sheet.Column(7).Width = 15; //JOINING
                sheet.Column(7).Width = 15; //JOINING
                sheet.Column(8).Width = 12; //CLASIFIED EL
                sheet.Column(9).Width = 12; //CLASIFIED EL
                sheet.Column(10).Width = 12; //CLASIFIED EL
                sheet.Column(11).Width = 12; //CLASIFIED EL
                sheet.Column(12).Width = 12; //CLASIFIED EL
                sheet.Column(13).Width = 12; //CLASIFIED EL
                sheet.Column(14).Width = 12; //ENJOYED
                sheet.Column(15).Width = 12; //RUNNING
                sheet.Column(16).Width = 12; //BALANCE
                
                nMaxColumn = 16;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "EL"; cell.Style.Font.Bold = true;
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

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "StartDate"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PresentONAtt"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "AbsentOnAtt"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DayoffOnAtt"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "HolidayOnAtt"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "LeaveOnAtt"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CLASSIFIED EL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ENJOYED"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "RUNNING"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BALANCE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion

                #region Table Body
                int nSL = 0;

                int nTotalClassifiedEL = 0;
                int nTotalEnjoyed = 0;
                int nTotalRunning = 0;
                int nTotalBalance = 0;
                int nPOA = 0;
                int nAOA = 0;
                int nDOA = 0;
                int nHOA = 0;
                int nLOA = 0;

                foreach( EarnLeave oItem in oEarnLeaves)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DPTName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DSGName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LastProcessDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.PresentONAtt; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.AbsentOnAtt; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DayoffOnAtt; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.HolidayOnAtt; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LeaveOnAtt; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ClassifiedEL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Enjoyed; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.RunningEL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Balance; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;

                    nTotalClassifiedEL += oItem.ClassifiedEL;
                    nTotalEnjoyed += oItem.Enjoyed;
                    nTotalRunning += oItem.RunningEL;
                    nTotalBalance += oItem.Balance;

                    nPOA += oItem.PresentONAtt;
                    nAOA += oItem.AbsentOnAtt;
                    nDOA += oItem.DayoffOnAtt;
                    nHOA += oItem.HolidayOnAtt;
                    nLOA += oItem.LeaveOnAtt;
                    
                }
                #endregion

                #region Total

                sheet.Cells[rowIndex, 2, rowIndex, 7].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Total:"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                colIndex = 8;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nPOA.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nAOA.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nDOA.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nHOA.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nLOA.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalClassifiedEL.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalEnjoyed.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalRunning.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalBalance.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EL.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }


        public void EL_XLClassified(string sParam)
        {
            string sEmployeeIDs = sParam.Split('~')[0];
            string sLocationID = sParam.Split('~')[1];
            string  sDepartmentIds = sParam.Split('~')[2];
            string sDesignationIds = sParam.Split('~')[3];
            string sBUnit = sParam.Split('~')[4];

            EarnLeave oEarnLeave = new EarnLeave();
            List<EarnLeave> oEarnLeaves = new List<EarnLeave>();
            string sSQL = "SELECT * FROM View_ELProcess WHERE EmployeeID<>0 ";
            if (!string.IsNullOrEmpty(sEmployeeIDs))
            {
                sSQL += "AND EmployeeID IN(" + sEmployeeIDs + ")";
            }
            if (!string.IsNullOrEmpty(sLocationID))
            {
                sSQL += "AND LocationID IN(" + sLocationID + ")";
            }
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                sSQL += "AND DepartmentID IN(" + sDepartmentIds + ")";
            }
            if (!string.IsNullOrEmpty(sDesignationIds))
            {
                sSQL += "AND DesignationID IN(" + sDesignationIds + ")";
            }
            if (!string.IsNullOrEmpty(sBUnit))
            {
                sSQL += "AND BusinessUnitID IN(" + sBUnit + ")";
            }

            oEarnLeaves = EarnLeave.ELGetsClassified(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            oEarnLeaves = oEarnLeaves.OrderBy(x=>x.Code).ToList();

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
                var sheet = excelPackage.Workbook.Worksheets.Add("Earn Leave");
                sheet.Name = "Earn Leave";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //CODE
                sheet.Column(4).Width = 20; //NAME
                sheet.Column(5).Width = 20; //DEPARTEMNT
                sheet.Column(6).Width = 20; //DSIGNATION
                sheet.Column(7).Width = 15; //JOINING
                sheet.Column(7).Width = 15; //JOINING
                sheet.Column(8).Width = 12; //CLASIFIED EL
                sheet.Column(9).Width = 12; //CLASIFIED EL
                sheet.Column(10).Width = 12; //CLASIFIED EL
                sheet.Column(11).Width = 12; //CLASIFIED EL
                sheet.Column(12).Width = 12; //CLASIFIED EL
                sheet.Column(13).Width = 12; //CLASIFIED EL
                sheet.Column(14).Width = 12; //ENJOYED
                sheet.Column(15).Width = 12; //RUNNING
                sheet.Column(16).Width = 12; //BALANCE
                
                nMaxColumn = 16;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "EL"; cell.Style.Font.Bold = true;
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

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "StartDate"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TotalPresent"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TotalAbsent"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TotalDayoff"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TotalHoliday"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TotalLeavOnAtt"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ClassifiedLeave"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                #endregion

                #region Table Body
                int nSL = 0;

                int nTotalClassifiedEL = 0;
                double nTotalLeave = 0.0;
                int nTotalEnjoyed = 0;
                int nTotalRunning = 0;
                int nTotalBalance = 0;
                int nPOA = 0;
                int nAOA = 0;
                int nDOA = 0;
                int nHOA = 0;
                int nLOA = 0;

                int nTotalPresent = 0;
                int nTotalAbsent = 0;
                int nTotalDayoff = 0;
                int nTotalHoliday = 0;

                foreach( EarnLeave oItem in oEarnLeaves)
                {
                    nSL++;
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DPTName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DSGName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LastProcessDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalPresent; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalAbsent; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalDayOff; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalHoliday; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalLeaveOnAttendance; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.TotalLeave; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;

                    nTotalLeave += oItem.TotalLeave;
                    nTotalClassifiedEL += oItem.ClassifiedEL;
                    nTotalEnjoyed += oItem.Enjoyed;
                    nTotalRunning += oItem.RunningEL;
                    nTotalBalance += oItem.Balance;

                    nPOA += oItem.PresentONAtt;
                    nAOA += oItem.AbsentOnAtt;
                    nDOA += oItem.DayoffOnAtt;
                    nHOA += oItem.HolidayOnAtt;
                    nLOA += oItem.LeaveOnAtt;

                    nTotalPresent += oItem.TotalPresent;
                    nTotalAbsent += oItem.TotalAbsent;
                    nTotalDayoff += oItem.TotalDayOff;
                    nTotalHoliday += oItem.TotalHoliday;
                    
                }
                #endregion

                #region Total

                sheet.Cells[rowIndex, 2, rowIndex, 8].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Total:"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                colIndex = 9;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalPresent.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalAbsent.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalDayoff.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalHoliday.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nLOA.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalClassifiedEL.ToString("###,##0"); cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalEnjoyed.ToString("###,##0"); cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalRunning.ToString("###,##0"); cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalBalance.ToString("###,##0"); cell.Style.Font.Bold = true;
                //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nTotalLeave.ToString("###,##0"); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Green); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
               
                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EL.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        

        #endregion XL

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

        #region EarnLeave Encashment
        public ActionResult View_EarnLeaveEncashments(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<EarnLeave> oEarnLeaves = new List<EarnLeave>();

            ViewBag.oACSs = AttendanceCalendarSession.Gets("SELECT * FROM AttendanceCalendarSession", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            string sSQL = "";
            sSQL = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSQL = sSQL + ")";
            ViewBag.BusinessUnits = BusinessUnit.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));

            return View(oEarnLeaves);
        }

        [HttpPost]
        public JsonResult ELGetsToEncash(string sBusinessUnitIds, string sEmployeeIDs, string sLocationID, string sDepartmentIds, string sDesignationIDs, int nACSID, double nStartSalaryRange, double nEndSalaryRange, int nLoadRecordsS, int nRowLengthS, double ts)
        {
            List<EarnLeave> oEarnLeaves = new List<EarnLeave>();
            try
            {
                oEarnLeaves = EarnLeave.ELGetsToEncash(sBusinessUnitIds, sEmployeeIDs, sLocationID, sDepartmentIds, sDesignationIDs, nACSID, nStartSalaryRange, nEndSalaryRange, nLoadRecordsS, nRowLengthS, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oEarnLeaves.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oEarnLeaves = new List<EarnLeave>();
                EarnLeave oEarnLeave = new EarnLeave();
                oEarnLeave.ErrorMessage = ex.Message;
                oEarnLeaves.Add(oEarnLeave);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEarnLeaves);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EncashEL(string sEmpLeaveLedgerIDs, DateTime DeclarationDate, int nACSID, int nConsiderEL, bool IsApplyforallemployee, bool IsEncashPresentBalance, double ts)
        {
            EarnLeave oEarnLeave = new EarnLeave();
            try
            {
                int nIndex = 0;
                int nNewIndex = 1;
                string sMessage = "";
                while (nNewIndex != 0)
                {
                    nNewIndex = oEarnLeave.EncashEL(nIndex, sEmpLeaveLedgerIDs, DeclarationDate, nACSID, nConsiderEL, IsApplyforallemployee, IsEncashPresentBalance, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    nIndex = nNewIndex;
                }


                //sMessage = EarnLeave.EncashEL(sEmpLeaveLedgerIDs, DeclarationDate, nACSID, nConsiderEL, IsApplyforallemployee, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                //if (sMessage != "" && sMessage!=null)
                //{
                //    throw new Exception(sMessage);
                //}
            }
            catch (Exception ex)
            {
                oEarnLeave = new EarnLeave();
                oEarnLeave.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEarnLeave);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EncashedELSearch(string sParams,double ts)
        {
            List<ELEncash> oELEncashs = new List<ELEncash>();
            string sBusinessUnitIds=sParams.Split('~')[0];
            string sEmployeeIDs=sParams.Split('~')[1];
            string sLocationID=sParams.Split('~')[2]; 
            string sDepartmentIds=sParams.Split('~')[3];
            string sDesignationIDs=sParams.Split('~')[4];
            int nACSID=Convert.ToInt32(sParams.Split('~')[5]);
            bool IsDeclarationDate= Convert.ToBoolean(sParams.Split('~')[6]);
            DateTime dtDeclarationDate = Convert.ToDateTime(sParams.Split('~')[7]);
            int nLoadRecordsS = Convert.ToInt32(sParams.Split('~')[8]);
            int nRowLengthS = Convert.ToInt32(sParams.Split('~')[9]);
            double nStartSalary = Convert.ToDouble(sParams.Split('~')[10]);
            double nEndSalary = Convert.ToDouble(sParams.Split('~')[11]);
            try
            {
                string sSql = "SELECT top(" + nLoadRecordsS + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY ELEncashID) Row,* FROM View_ELEncash WHERE ELEncashID <>0";
                if (sBusinessUnitIds!="")
                {
                    sSql+=" AND EmpLeaveLedgerID IN(SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger WHERE EmployeeID IN("
                        + " SELECT EmployeeID FROM View_Employee WHERE BusinessUnitID IN(" + sBusinessUnitIds + ")))";
                }
                if (sEmployeeIDs != "")
                {
                    sSql += " AND EmpLeaveLedgerID IN(SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger WHERE EmployeeID IN(" + sEmployeeIDs + "))";
                }
                if (sLocationID!="")
                {
                    sSql += " AND EmpLeaveLedgerID IN(SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger WHERE EmployeeID IN("
                        + "SELECT EmployeeID FROM View_Employee WHERE LocationID IN (" + sLocationID + ")))";
                }
                if (sDepartmentIds != "")
                {
                    sSql += " AND EmpLeaveLedgerID IN(SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger WHERE EmployeeID IN("
                        + "SELECT EmployeeID FROM View_Employee WHERE DepartmentID IN (" + sDepartmentIds + ")))";
                }
                if (sDesignationIDs != "")
                {
                    sSql += " AND EmpLeaveLedgerID IN(SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger WHERE EmployeeID IN("
                        + "SELECT EmployeeID FROM View_Employee WHERE DesignationID IN (" + sDesignationIDs + ")))";
                }
                if (nACSID >0)
                {
                    sSql += " AND EmpLeaveLedgerID IN(SELECT EmpLeaveLedgerID FROM EmployeeLeaveLedger WHERE ACSID = " + nACSID + ")";
                }
                if (nStartSalary > 0 && nEndSalary>0)
                {
                    sSql += " AND EmployeeID IN (SELECT top(1)EmployeeID FROM EmployeeSalaryStructure WITH (NOLOCK) WHERE ActualGrossAmount BETWEEN "+nStartSalary+" AND "+nEndSalary+")";
                }
                if (IsDeclarationDate)
                {
                    sSql += " AND DeclarationDate='" + dtDeclarationDate.ToString("dd MMM yyyy")+"'";
                }
                sSql = sSql + ") aa WHERE Row >" + nRowLengthS;
                oELEncashs = ELEncash.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oELEncashs.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oELEncashs = new List<ELEncash>();
                ELEncash oELEncash = new ELEncash();
                oELEncash.ErrorMessage = ex.Message;
                oELEncashs.Add(oELEncash);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oELEncashs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RollBackEncashedEL(string sELEncashIDs, DateTime dtDeclarationDate, double ts)
        {
            ELEncash oELEncash = new ELEncash();
            try
            {
                string sMessage = "";
                sMessage = ELEncash.RollBackEncashedEL(sELEncashIDs, dtDeclarationDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (sMessage != "" && sMessage != null)
                {
                    throw new Exception(sMessage);
                }
            }
            catch (Exception ex)
            {
                oELEncash = new ELEncash();
                oELEncash.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oELEncash.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveEncashedEL(string sELEncashIDs, DateTime dtDeclarationDate, double ts)
        {
            List<ELEncash> oELEncashs = new List<ELEncash>();
            try
            {
                oELEncashs = ELEncash.ApproveEncashedEL(sELEncashIDs, dtDeclarationDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oELEncashs.Count > 0 && oELEncashs[0].ErrorMessage!="")
                {
                    throw new Exception(oELEncashs[0].ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                oELEncashs = new List<ELEncash>();
                ELEncash oELEncash = new ELEncash();
                oELEncash.ErrorMessage = ex.Message;
                oELEncashs.Add(oELEncash);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oELEncashs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Report
        public ActionResult PrintEncashedEL(string sParams, double ts)
        {
            ELEncashReport oELEncashReport = new ELEncashReport();
            string sBusinessUnitIds = sParams.Split('~')[0];
            string sEmployeeIDs = sParams.Split('~')[1];
            string sLocationID = sParams.Split('~')[2];
            string sDepartmentIds = sParams.Split('~')[3];
            string sDesignationIds = sParams.Split('~')[4]; 
            int nACSID = Convert.ToInt32(sParams.Split('~')[5]);
            bool IsDeclarationDate = Convert.ToBoolean(sParams.Split('~')[6]);
            DateTime dtDeclarationDate = Convert.ToDateTime(sParams.Split('~')[7]);
            string sELEncashIDs = sParams.Split('~')[8];
            string sFormat = sParams.Split('~')[9];
            double nStartSalary = Convert.ToDouble(sParams.Split('~')[10]);
            double nEndSalary = Convert.ToDouble(sParams.Split('~')[11]);

            oELEncashReport.ELEncahsreports = ELEncashReport.Gets(sELEncashIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sEmployeeIDs, nACSID, IsDeclarationDate, dtDeclarationDate, nStartSalary,nEndSalary,((User)(Session[SessionInfo.CurrentUser])).UserID);

            rptEncashedEL oReport = new rptEncashedEL();
            byte[] abytes = oReport.PrepareReport(oELEncashReport, sFormat);
            return File(abytes, "application/pdf");

        }
        #endregion Report

        #endregion

        #region ELProcessEditHistory
        [HttpPost]
        public JsonResult ELProcessEditHistory_IUD(ELProcessEditHistory oELProcessEditHistory)
        {
            ELProcessEditHistory oELPEHistory = new ELProcessEditHistory();
            try
            {
                oELPEHistory = oELProcessEditHistory.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oELPEHistory = new ELProcessEditHistory();
                oELPEHistory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oELPEHistory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsELProcessEditHistory(ELProcessEditHistory oELProcessEditHistory)
        {
            List<ELProcessEditHistory> oELPEHistorys = new List<ELProcessEditHistory>();
            try
            {
                string sSql = "SELECT * FROM View_ELProcessEditHistory WHERE ELPID IN(SELECT ELProcessID FROM ELProcess WHERE EmployeeID=" + oELProcessEditHistory.EmployeeID+")";
                oELPEHistorys = oELProcessEditHistory.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if(oELPEHistorys.Count<=0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oELPEHistorys = new List<ELProcessEditHistory>();
                oELProcessEditHistory = new ELProcessEditHistory();
                oELProcessEditHistory.ErrorMessage = ex.Message;
                oELPEHistorys.Add(oELProcessEditHistory);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oELPEHistorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion ELProcessEditHistory



        public void Print_ReportXL(string sParams)
        {
            ELEncashReport oELEncashReport = new ELEncashReport();
            string sBusinessUnitIds = sParams.Split('~')[0];
            string sEmployeeIDs = sParams.Split('~')[1];
            string sLocationID = sParams.Split('~')[2];
            string sDepartmentIds = sParams.Split('~')[3];
            string sDesignationIds = sParams.Split('~')[4];
            int nACSID = Convert.ToInt32(sParams.Split('~')[5]);
            bool IsDeclarationDate = Convert.ToBoolean(sParams.Split('~')[6]);
            DateTime dtDeclarationDate = Convert.ToDateTime(sParams.Split('~')[7]);
            string sELEncashIDs = sParams.Split('~')[8];
            string sFormat = sParams.Split('~')[9];
            double nStartSalary = Convert.ToDouble(sParams.Split('~')[10]);
            double nEndSalary = Convert.ToDouble(sParams.Split('~')[11]);

            oELEncashReport.ELEncahsreports = ELEncashReport.Gets(sELEncashIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sEmployeeIDs, nACSID, IsDeclarationDate, dtDeclarationDate, nStartSalary, nEndSalary, ((User)(Session[SessionInfo.CurrentUser])).UserID);


            List<ELEncashReport> _oELEncashReports = new List<ELEncashReport>();

            List<ELEncashReport> _oTempELEncashReports = new List<ELEncashReport>();
            oELEncashReport.ELEncahsreports.ForEach(x => _oTempELEncashReports.Add(x));

            string Format = "";
            int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = 16;
            int nSL = 0;
            int span = 0;
            int nColumn = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            Format = sFormat;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("EL Encash");
                sheet.Name = "EL Encash";

                sheet.Column(nColumn++).Width = 8;
                sheet.Column(nColumn++).Width = 10;
                sheet.Column(nColumn++).Width = 20;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;

                if (Format == "withbasic")
                {
                    sheet.Column(nColumn++).Width = 15;
                }

                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;



                nColumn = 1;
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);


                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; cell.Style.Font.Size = 10;
                //cell.Value = oCompany.Address + "\n" + oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oELEncashReport.ELEncahsreports[0].BusinessUnitName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oELEncashReport.ELEncahsreports[0].BusinessUnitAddress; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Earn Leave Encashment till-" + oELEncashReport.ELEncahsreports[0].DeclarationDateInString; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 3;

                span = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#####"; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nColumn += 1;
                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                if (Format == "withbasic")
                {
                    cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Basic"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;
                }

                cell = sheet.Cells[nRowIndex, nColumn, nRowIndex, nColumn+7]; cell.Value = "Earn Leave Encashment(" + oELEncashReport.ELEncahsreports[0].DeclarationDateInString + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                

                //
                cell = sheet.Cells[nRowIndex+1, nColumn]; cell.Value = "Declaration Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "Total Days"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "Service Year"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "T/EL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "Enj.EL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "En.EL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Net Pay"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;
                //


                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Rcv. Sign"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                nRowIndex += 2;
                _oELEncashReports = oELEncashReport.ELEncahsreports;

                _oELEncashReports = _oELEncashReports.OrderBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
                _oELEncashReports.ForEach(x => _oTempELEncashReports.Add(x));
                
                double TotalGrossG = 0.0;
                double nTotalDaysG = 0.0;
                double TotalELG = 0.0;
                double TotalEnjoyedG = 0.0;
                double TotalEncashedELG = 0.0;
                double TotalEncashedELAmountG = 0.0;
                double TotalStampG = 0.0;
                double TotalNetPayableG = 0.0;

                while (_oELEncashReports.Count > 0)
                {
                    var oResults = _oELEncashReports.Where(x => x.BusinessUnitName == _oELEncashReports[0].BusinessUnitName && x.LocationName == _oELEncashReports[0].LocationName && x.DepartmentName == _oELEncashReports[0].DepartmentName).ToList();
                    _oELEncashReports.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName);

                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = "Unit-" + oResults[0].LocationName + ", Department-" + oResults[0].DepartmentName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;
                    nRowIndex += 1;
                    foreach (var oItem in oResults)
                    {
                        nSL++;
                        nColumn = 1;

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.JoiningInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(oItem.Gross); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalGrossG += Math.Round(oItem.Gross);

                        if (Format == "withbasic")
                        {

                            cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(oItem.Amount); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            nColumn += 1;
                        }

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.DeclarationDateInString; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;

                        //Total Days
                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.TotalDays; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        nTotalDaysG += oItem.TotalDays;

                        DateTime DeclarationDate = oItem.DeclarationDate.AddDays(1);
                        DateDifference oService = new DateDifference(oItem.Joining, DeclarationDate);

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oService.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.TotalEL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalELG += oItem.TotalEL;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.Enjoyed; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalEnjoyedG += Convert.ToDouble(oItem.Enjoyed);


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.EncashedEL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalEncashedELG += oItem.EncashedEL;

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.EncashedELAmount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalEncashedELAmountG += oItem.EncashedELAmount;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.Stamp; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalStampG += oItem.Stamp;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(oItem.NetPayable); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalNetPayableG += Math.Round(oItem.NetPayable);

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;



                        nEndRow = nRowIndex;
                        nRowIndex++;


                    }
                    nColumn = 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;
                    double TotalGross = oResults.Sum(x => x.Gross);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(TotalGross); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;


                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    double nTotalDays = oResults.Sum(x => x.TotalDays);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    if (Format == "withbasic")
                    {
                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                    }


                    double TotalEL = oResults.Sum(x => x.TotalEL);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEL; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    double TotalEnjoyed = oResults.Sum(x => x.Enjoyed);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEnjoyed; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    double TotalEncashedEL = oResults.Sum(x => x.EncashedEL);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEncashedEL; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    double TotalEncashedELAmount = oResults.Sum(x => x.EncashedELAmount);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEncashedELAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    double TotalStamp = oResults.Sum(x => x.Stamp);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalStamp; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;


                    double TotalNetPayable = oResults.Sum(x => x.NetPayable);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(TotalNetPayable); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;
                    nRowIndex += 1;
                }

                nRowIndex += 1;
                nColumn = 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;
                //double TotalGrossG = _oTempELEncashReports.Sum(x => x.Gross);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(TotalGrossG); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;


                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                //double nTotalDaysG = _oTempELEncashReports.Sum(x => x.TotalDays);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                if (Format == "withbasic")
                {
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;
                }


                //double TotalELG = _oTempELEncashReports.Sum(x => x.TotalEL);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalELG; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                //double TotalEnjoyedG = _oELEncashReports.Sum(x => x.Enjoyed);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEnjoyedG; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                //double TotalEncashedELG = _oTempELEncashReports.Sum(x => x.EncashedEL);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEncashedELG; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                //double TotalEncashedELAmountG = _oTempELEncashReports.Sum(x => x.EncashedELAmount);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEncashedELAmountG; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                //double TotalStampG = _oTempELEncashReports.Sum(x => x.Stamp);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalStampG; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;


                //double TotalNetPayableG = _oTempELEncashReports.Sum(x => x.NetPayable);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(TotalNetPayableG); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;
                nRowIndex += 1;

                


                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EL ENCASH.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();


            }

        }


        #region Summary

        public void Summary_ReportXL(string sParams)
        {
            ELEncashReport oELEncashReport = new ELEncashReport();
            string sBusinessUnitIds = sParams.Split('~')[0];
            string sEmployeeIDs = sParams.Split('~')[1];
            string sLocationID = sParams.Split('~')[2];
            string sDepartmentIds = sParams.Split('~')[3];
            string sDesignationIds = sParams.Split('~')[4];
            int nACSID = Convert.ToInt32(sParams.Split('~')[5]);
            bool IsDeclarationDate = Convert.ToBoolean(sParams.Split('~')[6]);
            DateTime dtDeclarationDate = Convert.ToDateTime(sParams.Split('~')[7]);
            string sELEncashIDs = sParams.Split('~')[8];
            string sFormat = sParams.Split('~')[9];
            double nStartSalary = Convert.ToDouble(sParams.Split('~')[10]);
            double nEndSalary = Convert.ToDouble(sParams.Split('~')[11]);

            oELEncashReport.ELEncahsreports = ELEncashReport.Gets(sELEncashIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sEmployeeIDs, nACSID, IsDeclarationDate, dtDeclarationDate, nStartSalary, nEndSalary, ((User)(Session[SessionInfo.CurrentUser])).UserID);



            Company oCompany = new Company();
            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Employee Bonus Summary");
                sheet.Name = "Employee Bonus Summary";

                sheet.Column(1).Width = 25;//SL
                sheet.Column(2).Width = 25;//Dept Name
                sheet.Column(3).Width = 20;//ManPower
                sheet.Column(4).Width = 20;//Gross
                sheet.Column(5).Width = 20;//Payable
                sheet.Column(6).Width = 20;//Stamp
                sheet.Column(7).Width = 20;//NetPayable
                sheet.Column(8).Width = 20;

                nMaxColumn = 8;

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Table Body

                int rowIndex = 1;
                int nPS = 0;
                int colIndex = 1;


                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string BUIDs = string.Join(",", oELEncashReport.ELEncahsreports.Select(p => p.BusinessUnitID).Distinct().ToList());
                if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Name : oCompany.Name; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 30; cell.Style.Font.SetFromFont(new Font("Century Gothic", 22));
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 22; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                rowIndex = rowIndex + 1;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Address : oCompany.Address; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colIndex = 1;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = (oELEncashReport.ELEncahsreports.Count <= 0)?"":"Earn Leave Encashment till-" + oELEncashReport.ELEncahsreports[0].DeclarationDateInString; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colIndex = 1;
                rowIndex = rowIndex + 1;


                var grpEBSummary = oELEncashReport.ELEncahsreports.GroupBy(x => new {x.BusinessUnitName, x.LocationName }, (key, grp) => new
                {
                    BusinessUnitName = key.BusinessUnitName,
                    LocationName = grp.First().LocationName,
                    ELCount = grp.Count(),
                    ELList = grp,
                    DepartmentName = grp.First().DepartmentName,

                }).OrderBy(x => x.BusinessUnitName).ThenBy(x=>x.LocationName).ToList();


                foreach (var oItem in grpEBSummary)
                {
                    colIndex = 1;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BU"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BusinessUnitName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    rowIndex += 1;
                    colIndex = 1;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    rowIndex += 2;
                    colIndex = 1;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex += 1;
                    colIndex = 1;

                    var List = oItem.ELList.GroupBy(x => new { x.DepartmentID }, (key, grp) => new ELEncashReport
                    {
                        DepartmentName = grp.First().DepartmentName,
                        EncashedELAmount = grp.Sum(x => x.EncashedELAmount),
                        Stamp = grp.Sum(x => x.Stamp),
                        Gross = grp.Sum(x => x.Gross),
                        NetPayable = grp.Sum(x => x.NetPayable),

                        ManPower = grp.Count()

                    }).OrderBy(x => x.DepartmentName).ToList();
                    nPS = 1;
                    double total = 0.0;
                    double nManPower = 0.0;
                    double nGross = 0.0;
                    int totalStamp = 0;
                    double totalNetPayable = 0.0;
                    foreach (var data in List)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nPS++;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.DepartmentName;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.ManPower;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(data.Gross);
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(data.EncashedELAmount);
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        int nStamp = data.ManPower * 10;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nStamp;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(data.NetPayable);
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        rowIndex += 1;
                        nManPower += data.ManPower;
                        nGross += data.Gross;
                        total += data.EncashedELAmount;
                        totalStamp += nStamp;
                        totalNetPayable += data.NetPayable;
                    }
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nManPower; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(nGross); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(total); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = totalStamp; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(totalNetPayable); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    rowIndex += 3;

                }

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Group Summary Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;
                colIndex = 1;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BUName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;
                colIndex = 1;

                var grpEBGrandSummary = oELEncashReport.ELEncahsreports.GroupBy(x => new { x.BusinessUnitID, x.BusinessUnitName }, (key, grp) => new
                {
                    BusinessUnitID = key.BusinessUnitID,
                    BusinessUnitName = key.BusinessUnitName,
                    LocationName = grp.First().LocationName,
                    EBSCount = grp.Count(),
                    EBSList = grp,
                    DepartmentName = grp.First().DepartmentName,
                    EncashedELAmount = grp.Sum(x => x.EncashedELAmount),
                    Gross = grp.Sum(x => x.Gross),
                    NetPayable = grp.Sum(x => x.NetPayable),
                    ManPower = grp.Count()


                }).OrderBy(x => x.BusinessUnitName).ToList();

                foreach (var oItem in grpEBGrandSummary)
                {
                    //var List = oItem.EBSList.GroupBy(x => new { x.LocationName }, (key, grp) => new EmployeeBonus
                    //{
                    //    LocationName = grp.First().LocationName,
                    //    BonusAmount = grp.Sum(x => x.BonusAmount),
                    //    GrossAmount = grp.Sum(x => x.GrossAmount),

                    //    ManPower = grp.Count()

                    //}).OrderBy(x => x.LocationName).ToList();

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.BusinessUnitName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;


                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.ManPower; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(oItem.Gross); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(oItem.EncashedELAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    int nStamp = oItem.ManPower * 10;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nStamp; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(oItem.NetPayable); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;


                    rowIndex += 1;
                }


                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ELSummary.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }

        #endregion

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

        #region Compliance EL 

        public ActionResult View_ELEncashCompliance(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.AccountsBookSetup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oELEncashCompliances = new List<ELEncashCompliance>();
            //_oEmployeeAdvanceSalarys = EmployeeAdvanceSalary.Gets((int)Session[SessionInfo.currentUserID]);

            _oELEncashCompliances = ELEncashCompliance.Gets((int)Session[SessionInfo.currentUserID]);

            ViewBag.EmployeeGroups = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeBlocks = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.Block, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oELEncashCompliances);
        }
        public ActionResult View_ELEncashComplianceProcess(int menuid)
        {
            _oELEncashCompliance = new ELEncashCompliance();


            return View(_oELEncashCompliance);
        }

        [HttpPost]
        public JsonResult SaveProcess(ELEncashCompliance oELEncashCompliance)
        {
            _oELEncashCompliance = new ELEncashCompliance();
            try
            {
                int nIndex = 0;
                int nNewIndex = 1;

                _oELEncashCompliance = oELEncashCompliance;
                _oELEncashCompliance = _oELEncashCompliance.Save((int)Session[SessionInfo.currentUserID]);

                if (_oELEncashCompliance.ELEncashCompID > 0)
                {
                    while (nNewIndex != 0)
                    {
                        nNewIndex = _oELEncashCompliance.ELEncashComplianceSave(nIndex, _oELEncashCompliance.ELEncashCompID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        nIndex = nNewIndex;
                    }
                }
            }
            catch (Exception ex)
            {
                _oELEncashCompliance.ErrorMessage = ex.Message;

                _oELEncashCompliance = new ELEncashCompliance();
                _oELEncashCompliance.ErrorMessage = ex.Message.Split('~')[0];
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oELEncashCompliance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteProcess(ELEncashCompliance oELEncashCompliance)
        {
            _oELEncashCompliance = new ELEncashCompliance();
            string sErrorMease = "";
            try
            {
                _oELEncashCompliance = oELEncashCompliance;
                _oELEncashCompliance = _oELEncashCompliance.Delete((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oELEncashCompliance = new ELEncashCompliance();
                _oELEncashCompliance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oELEncashCompliance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveProcess(ELEncashCompliance oELEncashCompliance)
        {
            _oELEncashCompliance = new ELEncashCompliance();
            try
            {
                _oELEncashCompliance = oELEncashCompliance.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oELEncashCompliance = new ELEncashCompliance();
                _oELEncashCompliance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oELEncashCompliance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchELComp(ELEncashComplianceDetail oELEncashComplianceDetail)
        {

            List<ELEncashComplianceDetail> oELEncashComplianceDetails = new List<ELEncashComplianceDetail>();
            try
            {
                DateTime dtStart = Convert.ToDateTime(oELEncashComplianceDetail.Params.Split('~')[0]);
                DateTime dtEnd = Convert.ToDateTime(oELEncashComplianceDetail.Params.Split('~')[1]);
                int BusinessunitID = Convert.ToInt32(oELEncashComplianceDetail.Params.Split('~')[2]);
                string LocationIDs = Convert.ToString(oELEncashComplianceDetail.Params.Split('~')[3]);
                string DepartmentIDs = Convert.ToString(oELEncashComplianceDetail.Params.Split('~')[4]);
                int sSalary = Convert.ToInt32(oELEncashComplianceDetail.Params.Split('~')[5]);
                int eSalary = Convert.ToInt32(oELEncashComplianceDetail.Params.Split('~')[6]);
                string GroupIDs = Convert.ToString(oELEncashComplianceDetail.Params.Split('~')[7]);
                string BlockIDs = Convert.ToString(oELEncashComplianceDetail.Params.Split('~')[8]);
                string DesignationIDs = Convert.ToString(oELEncashComplianceDetail.Params.Split('~')[9]);
                bool bDeclaration = Convert.ToBoolean(oELEncashComplianceDetail.Params.Split('~')[10]);
                string sDeclarationDate = oELEncashComplianceDetail.Params.Split('~')[11];
                bool bDateBetween = Convert.ToBoolean(oELEncashComplianceDetail.Params.Split('~')[12]);



                string sSQL = "SELECT * FROM View_ELEncashComplianceDetail WHERE ELEncashCompID IN(SELECT ELEncashCompID FROM ELEncashCompliance  WITH (NOLOCK) WHERE ELEncashCompID <> 0";
                if (bDateBetween)
                {
                    sSQL += "  StartDate='" + dtStart.ToString("dd MMM yyyy") + "' AND EndDate='" + dtEnd.ToString("dd MMM yyyy") + "'";
                }
                if (bDeclaration)
                {
                    sSQL += " AND DeclarationDate='" + sDeclarationDate + "'";
                }
                if (BusinessunitID != 0)
                {
                    sSQL += " AND BUID=" + BusinessunitID;
                }
                if (!string.IsNullOrEmpty(LocationIDs))
                {
                    sSQL += " AND LocationID IN(" + LocationIDs + ")";
                }
                sSQL = sSQL + ")";
                if (!string.IsNullOrEmpty(DepartmentIDs))
                {
                    sSQL += " AND DepartmentID IN(" + DepartmentIDs + ")";
                }
                if (!string.IsNullOrEmpty(DesignationIDs))
                {
                    sSQL += " AND DesignationID IN(" + DesignationIDs + ")";
                }
                if ((sSalary > 0 && eSalary > 0) && (sSalary <= eSalary))
                {
                    sSQL += " AND CompGrossSalary BETWEEN '" + sSalary + "' AND '" + eSalary + "'";
                }
                if (BlockIDs != "")
                {
                    sSQL += "AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + BlockIDs + "))";
                }
                if (GroupIDs != "")
                {
                    sSQL += " AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + GroupIDs + "))";
                }

                oELEncashComplianceDetails = ELEncashComplianceDetail.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oELEncashComplianceDetails = new List<ELEncashComplianceDetail>();
                oELEncashComplianceDetail = new ELEncashComplianceDetail();
                oELEncashComplianceDetail.ErrorMessage = ex.Message;
                oELEncashComplianceDetails.Add(oELEncashComplianceDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oELEncashComplianceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void Print_ReportCompXL(string sParam)
        {
            List<ELEncashComplianceDetail> oELEncashComplianceDetails = new List<ELEncashComplianceDetail>();
            DateTime dtStart = Convert.ToDateTime(sParam.Split('~')[0]);
            DateTime dtEnd = Convert.ToDateTime(sParam.Split('~')[1]);
            int BusinessunitID = Convert.ToInt32(sParam.Split('~')[2]);
            string LocationIDs = Convert.ToString(sParam.Split('~')[3]);
            string DepartmentIDs = Convert.ToString(sParam.Split('~')[4]);
            int sSalary = Convert.ToInt32(sParam.Split('~')[5]);
            int eSalary = Convert.ToInt32(sParam.Split('~')[6]);
            string GroupIDs = Convert.ToString(sParam.Split('~')[7]);
            string BlockIDs = Convert.ToString(sParam.Split('~')[8]);
            string DesignationIDs = Convert.ToString(sParam.Split('~')[9]);
            bool bDeclaration = Convert.ToBoolean(sParam.Split('~')[10]);
            string sDeclarationDate = sParam.Split('~')[11];
            bool bDateBetween = Convert.ToBoolean(sParam.Split('~')[12]);
            string sFormat = sParam.Split('~')[13];



            string sSQL = "SELECT * FROM View_ELEncashComplianceDetail WHERE ELEncashCompID IN(SELECT ELEncashCompID FROM ELEncashCompliance  WITH (NOLOCK) WHERE ELEncashCompID <> 0";
            if (bDateBetween)
            {
                sSQL += " AND StartDate='" + dtStart.ToString("dd MMM yyyy") + "' AND EndDate='" + dtEnd.ToString("dd MMM yyyy") + "'";
            }
            if (bDeclaration)
            {
                sSQL += " AND DeclarationDate='" + sDeclarationDate + "'";
            }
            if (BusinessunitID != 0)
            {
                sSQL += " AND BUID=" + BusinessunitID;
            }
            if (!string.IsNullOrEmpty(LocationIDs))
            {
                sSQL += " AND LocationID IN(" + LocationIDs + ")";
            }
            sSQL = sSQL + ")";
            if (!string.IsNullOrEmpty(DepartmentIDs))
            {
                sSQL += " AND DepartmentID IN(" + DepartmentIDs + ")";
            }
            if (!string.IsNullOrEmpty(DesignationIDs))
            {
                sSQL += " AND DesignationID IN(" + DesignationIDs + ")";
            }
            if ((sSalary > 0 && eSalary > 0) && (sSalary <= eSalary))
            {
                sSQL += " AND CompGrossSalary BETWEEN '" + sSalary + "' AND '" + eSalary + "'";
            }
            if (BlockIDs != "")
            {
                sSQL += "AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + BlockIDs + "))";
            }
            if (GroupIDs != "")
            {
                sSQL += " AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + GroupIDs + "))";
            }

            oELEncashComplianceDetails = ELEncashComplianceDetail.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            oELEncashComplianceDetails = oELEncashComplianceDetails.GroupBy(test => test.EmployeeID)
                   .Select(grp => grp.First())
                   .ToList();

            oELEncashComplianceDetails = oELEncashComplianceDetails.Where(x => x.EncashAmount > 0).ToList();

            List<ELEncashComplianceDetail> _oELEncashReports = new List<ELEncashComplianceDetail>();

            List<ELEncashComplianceDetail> _oTempELEncashReports = new List<ELEncashComplianceDetail>();
            oELEncashComplianceDetails.ForEach(x => _oTempELEncashReports.Add(x));

            string Format = "";
            int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = 16;
            int nSL = 0;
            int span = 0;
            int nColumn = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            Format = sFormat;
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("EL Encash");
                sheet.Name = "EL Encash";

                sheet.Column(nColumn++).Width = 8;
                sheet.Column(nColumn++).Width = 10;
                sheet.Column(nColumn++).Width = 20;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;

                if (Format == "withbasic")
                {
                    sheet.Column(nColumn++).Width = 15;
                }

                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;
                sheet.Column(nColumn++).Width = 15;



                nColumn = 1;
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);


                //cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true; cell.Style.Font.Size = 10;
                //cell.Value = oCompany.Address + "\n" + oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = true;
                //cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oELEncashComplianceDetails[0].BusinessUnitName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oELEncashComplianceDetails[0].BusinessUnitAddress; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Earn Leave Encashment till-" + oELEncashComplianceDetails[0].DeclarationDateInStr; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 3;

                span = nRowIndex + 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Numberformat.Format = "#####"; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Name"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                nColumn += 1;
                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Designation"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Joining Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Gross"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                if (Format == "withbasic")
                {
                    cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Basic"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;
                }

                cell = sheet.Cells[nRowIndex, nColumn, nRowIndex, nColumn + 4]; cell.Value = "Earn Leave Encashment(" + oELEncashComplianceDetails[0].DeclarationDateInStr + ")"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                //
                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "Declaration Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "Service Year"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "En.EL"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex + 1, nColumn]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Net Pay"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;
                //


                cell = sheet.Cells[nRowIndex, nColumn, span, nColumn]; cell.Value = "Rcv. Sign"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Merge = true;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                nRowIndex += 2;
                _oELEncashReports = oELEncashComplianceDetails;

                _oELEncashReports = _oELEncashReports.OrderBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
                _oELEncashReports.ForEach(x => _oTempELEncashReports.Add(x));

                double TotalGrossG = 0.0;
                double nTotalDaysG = 0.0;
                double TotalELG = 0.0;
                double TotalEnjoyedG = 0.0;
                double TotalEncashedELG = 0.0;
                double TotalEncashedELAmountG = 0.0;
                double TotalStampG = 0.0;
                double TotalNetPayableG = 0.0;

                while (_oELEncashReports.Count > 0)
                {
                    var oResults = _oELEncashReports.Where(x => x.BusinessUnitName == _oELEncashReports[0].BusinessUnitName && x.LocationName == _oELEncashReports[0].LocationName && x.DepartmentName == _oELEncashReports[0].DepartmentName).ToList();
                    _oELEncashReports.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName);

                    nColumn = 1;
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = "Unit-" + oResults[0].LocationName + ", Department-" + oResults[0].DepartmentName; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;
                    nRowIndex += 1;
                    foreach (var oItem in oResults)
                    {
                        nSL++;
                        nColumn = 1;

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = "" + nSL; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        cell.Style.Numberformat.Format = "#####";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.JoiningDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(oItem.CompGrossSalary); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalGrossG += Math.Round(oItem.CompGrossSalary);

                        if (Format == "withbasic")
                        {

                            cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(oItem.CompBasicAmount); cell.Style.Font.Bold = false;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            nColumn += 1;
                        }

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.DeclarationDateInStr; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;

                        //nTotalDaysG += oItem.TotalDays;

                        DateTime DeclarationDate = oItem.DeclarationDate.AddDays(1);
                        DateDifference oService = new DateDifference(oItem.JoiningDate, DeclarationDate);

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oService.ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.EncashELCount; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalEncashedELG += oItem.EncashELCount;

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(oItem.EncashAmount); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalEncashedELAmountG += oItem.EncashAmount;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = oItem.Stamp; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalStampG += oItem.Stamp;


                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round((oItem.EncashAmount - oItem.Stamp)); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                        TotalNetPayableG += Math.Round((oItem.EncashAmount - oItem.Stamp));

                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;



                        nEndRow = nRowIndex;
                        nRowIndex++;


                    }
                    nColumn = 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;
                    double TotalGross = oResults.Sum(x => x.CompGrossSalary);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(TotalGross); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;


                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;


                    if (Format == "withbasic")
                    {
                        cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nColumn += 1;
                    }

                    double TotalEncashedEL = oResults.Sum(x => x.EncashELCount);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEncashedEL; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    double TotalEncashedELAmount = oResults.Sum(x => Math.Round(x.EncashAmount));
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEncashedELAmount; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    double TotalStamp = oResults.Sum(x => x.Stamp);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalStamp; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;


                    double TotalNetPayable = oResults.Sum(x => Math.Round(x.EncashAmount)) - oResults.Sum(x => x.Stamp);
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(TotalNetPayable); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;

                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;
                    nRowIndex += 1;
                }

                nRowIndex += 1;
                nColumn = 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;
                //double TotalGrossG = _oTempELEncashReports.Sum(x => x.Gross);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(TotalGrossG); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;


                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                if (Format == "withbasic")
                {
                    cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nColumn += 1;
                }


                //double TotalEncashedELG = _oTempELEncashReports.Sum(x => x.EncashedEL);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEncashedELG; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                //double TotalEncashedELAmountG = _oTempELEncashReports.Sum(x => x.EncashedELAmount);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalEncashedELAmountG; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                //double TotalStampG = _oTempELEncashReports.Sum(x => x.Stamp);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = TotalStampG; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;


                //double TotalNetPayableG = _oTempELEncashReports.Sum(x => x.NetPayable);
                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = Math.Round(TotalNetPayableG); cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;

                cell = sheet.Cells[nRowIndex, nColumn]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                nColumn += 1;
                nRowIndex += 1;




                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EL ENCASH.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();


            }

        }

        public void Summary_ReportCompXL(string sParam)
        {
            List<ELEncashComplianceDetail> oELEncashComplianceDetails = new List<ELEncashComplianceDetail>();
            DateTime dtStart = Convert.ToDateTime(sParam.Split('~')[0]);
            DateTime dtEnd = Convert.ToDateTime(sParam.Split('~')[1]);
            int BusinessunitID = Convert.ToInt32(sParam.Split('~')[2]);
            string LocationIDs = Convert.ToString(sParam.Split('~')[3]);
            string DepartmentIDs = Convert.ToString(sParam.Split('~')[4]);
            int sSalary = Convert.ToInt32(sParam.Split('~')[5]);
            int eSalary = Convert.ToInt32(sParam.Split('~')[6]);
            string GroupIDs = Convert.ToString(sParam.Split('~')[7]);
            string BlockIDs = Convert.ToString(sParam.Split('~')[8]);
            string DesignationIDs = Convert.ToString(sParam.Split('~')[9]);
            bool bDeclaration = Convert.ToBoolean(sParam.Split('~')[10]);
            string sDeclarationDate = sParam.Split('~')[11];
            bool bDateBetween = Convert.ToBoolean(sParam.Split('~')[12]);
            string sFormat = sParam.Split('~')[13];



            string sSQL = "SELECT * FROM View_ELEncashComplianceDetail WHERE ELEncashCompID IN(SELECT ELEncashCompID FROM ELEncashCompliance  WITH (NOLOCK) WHERE ELEncashCompID <> 0";
            if (bDateBetween)
            {
                sSQL += " AND StartDate='" + dtStart.ToString("dd MMM yyyy") + "' AND EndDate='" + dtEnd.ToString("dd MMM yyyy") + "'";
            }
            if (bDeclaration)
            {
                sSQL += " AND DeclarationDate='" + sDeclarationDate + "'";
            }
            if (BusinessunitID != 0)
            {
                sSQL += " AND BUID=" + BusinessunitID;
            }
            if (!string.IsNullOrEmpty(LocationIDs))
            {
                sSQL += " AND LocationID IN(" + LocationIDs + ")";
            }
            sSQL = sSQL + ")";
            if (!string.IsNullOrEmpty(DepartmentIDs))
            {
                sSQL += " AND DepartmentID IN(" + DepartmentIDs + ")";
            }
            if (!string.IsNullOrEmpty(DesignationIDs))
            {
                sSQL += " AND DesignationID IN(" + DesignationIDs + ")";
            }
            if ((sSalary > 0 && eSalary > 0) && (sSalary <= eSalary))
            {
                sSQL += " AND CompGrossSalary BETWEEN '" + sSalary + "' AND '" + eSalary + "'";
            }
            if (BlockIDs != "")
            {
                sSQL += "AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + BlockIDs + "))";
            }
            if (GroupIDs != "")
            {
                sSQL += " AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + GroupIDs + "))";
            }

            oELEncashComplianceDetails = ELEncashComplianceDetail.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            oELEncashComplianceDetails = oELEncashComplianceDetails.GroupBy(test => test.EmployeeID)
                   .Select(grp => grp.First())
                   .ToList();

            oELEncashComplianceDetails = oELEncashComplianceDetails.Where(x => x.EncashAmount > 0).ToList();

            List<ELEncashComplianceDetail> _oELEncashReports = new List<ELEncashComplianceDetail>();

            List<ELEncashComplianceDetail> _oTempELEncashReports = new List<ELEncashComplianceDetail>();
            oELEncashComplianceDetails.ForEach(x => _oTempELEncashReports.Add(x));


            Company oCompany = new Company();
            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Employee Bonus Summary");
                sheet.Name = "Employee Bonus Summary";

                sheet.Column(1).Width = 25;//SL
                sheet.Column(2).Width = 25;//Dept Name
                sheet.Column(3).Width = 20;//ManPower
                sheet.Column(4).Width = 20;//Gross
                sheet.Column(5).Width = 20;//Payable
                sheet.Column(6).Width = 20;//Stamp
                sheet.Column(7).Width = 20;//NetPayable
                sheet.Column(8).Width = 20;

                nMaxColumn = 8;

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Table Body

                int rowIndex = 1;
                int nPS = 0;
                int colIndex = 1;


                List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
                string BUIDs = string.Join(",", oELEncashComplianceDetails.Select(p => p.BUID).Distinct().ToList());
                if (BUIDs != "") { oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + BUIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID); }


                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Name : oCompany.Name; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 30; cell.Style.Font.SetFromFont(new Font("Century Gothic", 22));
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 22; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                rowIndex = rowIndex + 1;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = oBusinessUnits.Count == 1 ? oBusinessUnits[0].Address : oCompany.Address; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colIndex = 1;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, colIndex]; cell.Merge = true;
                cell.Value = (oELEncashComplianceDetails.Count <= 0) ? "" : "Earn Leave Encashment till-" + oELEncashComplianceDetails[0].DeclarationDateInStr; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colIndex = 1;
                rowIndex = rowIndex + 1;


                var grpEBSummary = oELEncashComplianceDetails.GroupBy(x => new { x.BusinessUnitName, x.LocationName }, (key, grp) => new
                {
                    BusinessUnitName = key.BusinessUnitName,
                    LocationName = grp.First().LocationName,
                    ELCount = grp.Count(),
                    ELList = grp,
                    DepartmentName = grp.First().DepartmentName,

                }).OrderBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ToList();


                foreach (var oItem in grpEBSummary)
                {
                    colIndex = 1;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BU"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BusinessUnitName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    rowIndex += 1;
                    colIndex = 1;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Location"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.LocationName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    rowIndex += 2;
                    colIndex = 1;


                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex += 1;
                    colIndex = 1;

                    var List = oItem.ELList.GroupBy(x => new { x.DepartmentID }, (key, grp) => new ELEncashComplianceDetail
                    {
                        DepartmentName = grp.First().DepartmentName,
                        EncashAmount = grp.Sum(x => Math.Round(x.EncashAmount)),
                        Stamp = grp.Sum(x => x.Stamp),
                        CompGrossSalary = grp.Sum(x => x.CompGrossSalary),
                        //NetPayable = grp.Sum(x => x.NetPayable),

                        ManPower = grp.Count()

                    }).OrderBy(x => x.DepartmentName).ToList();
                    nPS = 1;
                    double total = 0.0;
                    double nManPower = 0.0;
                    double nGross = 0.0;
                    int totalStamp = 0;
                    double totalNetPayable = 0.0;
                    foreach (var data in List)
                    {
                        colIndex = 1;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nPS++;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.DepartmentName;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = data.ManPower;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(data.CompGrossSalary);
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(data.EncashAmount);
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        int nStamp = data.ManPower * 10;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nStamp;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(data.EncashAmount - data.Stamp);
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        colIndex += 1;

                        rowIndex += 1;
                        nManPower += data.ManPower;
                        nGross += data.CompGrossSalary;
                        total += Math.Round(data.EncashAmount);
                        totalStamp += nStamp;
                        totalNetPayable += (data.EncashAmount - data.Stamp);
                    }
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nManPower; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(nGross); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(total); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = totalStamp; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(totalNetPayable); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    rowIndex += 3;

                }

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Group Summary Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;
                colIndex = 1;


                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BUName"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ManPower"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;
                colIndex = 1;

                var grpEBGrandSummary = oELEncashComplianceDetails.GroupBy(x => new { x.BUID, x.BusinessUnitName }, (key, grp) => new
                {
                    BUID = key.BUID,
                    BusinessUnitName = key.BusinessUnitName,
                    LocationName = grp.First().LocationName,
                    EBSCount = grp.Count(),
                    EBSList = grp,
                    DepartmentName = grp.First().DepartmentName,
                    EncashAmount = grp.Sum(x => Math.Round(x.EncashAmount)),
                    Stamp = grp.Sum(x => x.Stamp),
                    CompGrossSalary = grp.Sum(x => x.CompGrossSalary),
                    //NetPayable = grp.Sum(x => x.NetPayable),
                    ManPower = grp.Count()


                }).OrderBy(x => x.BusinessUnitName).ToList();

                foreach (var oItem in grpEBGrandSummary)
                {
                    //var List = oItem.EBSList.GroupBy(x => new { x.LocationName }, (key, grp) => new EmployeeBonus
                    //{
                    //    LocationName = grp.First().LocationName,
                    //    BonusAmount = grp.Sum(x => x.BonusAmount),
                    //    GrossAmount = grp.Sum(x => x.GrossAmount),

                    //    ManPower = grp.Count()

                    //}).OrderBy(x => x.LocationName).ToList();

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.BusinessUnitName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;


                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.ManPower; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(oItem.CompGrossSalary); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(oItem.EncashAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    int nStamp = oItem.ManPower * 10;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = nStamp; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = Math.Round(oItem.EncashAmount - oItem.Stamp); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Yellow); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    colIndex += 1;


                    rowIndex += 1;
                }


                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=ELSummary.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }

        public ActionResult PrintEncashedELComp(string sParam)
        {
            List<ELEncashComplianceDetail> oELEncashComplianceDetails = new List<ELEncashComplianceDetail>();
            DateTime dtStart = Convert.ToDateTime(sParam.Split('~')[0]);
            DateTime dtEnd = Convert.ToDateTime(sParam.Split('~')[1]);
            int BusinessunitID = Convert.ToInt32(sParam.Split('~')[2]);
            string LocationIDs = Convert.ToString(sParam.Split('~')[3]);
            string DepartmentIDs = Convert.ToString(sParam.Split('~')[4]);
            int sSalary = Convert.ToInt32(sParam.Split('~')[5]);
            int eSalary = Convert.ToInt32(sParam.Split('~')[6]);
            string GroupIDs = Convert.ToString(sParam.Split('~')[7]);
            string BlockIDs = Convert.ToString(sParam.Split('~')[8]);
            string DesignationIDs = Convert.ToString(sParam.Split('~')[9]);
            bool bDeclaration = Convert.ToBoolean(sParam.Split('~')[10]);
            string sDeclarationDate = sParam.Split('~')[11];
            bool bDateBetween = Convert.ToBoolean(sParam.Split('~')[12]);
            string sFormat = sParam.Split('~')[13];



            string sSQL = "SELECT * FROM View_ELEncashComplianceDetail WHERE ELEncashCompID IN(SELECT ELEncashCompID FROM ELEncashCompliance  WITH (NOLOCK) WHERE ELEncashCompID <> 0";
            if (bDateBetween)
            {
                sSQL += " AND StartDate='" + dtStart.ToString("dd MMM yyyy") + "' AND EndDate='" + dtEnd.ToString("dd MMM yyyy") + "'";
            }
            if (bDeclaration)
            {
                sSQL += " AND DeclarationDate='" + sDeclarationDate + "'";
            }
            if (BusinessunitID != 0)
            {
                sSQL += " AND BUID=" + BusinessunitID;
            }
            if (!string.IsNullOrEmpty(LocationIDs))
            {
                sSQL += " AND LocationID IN(" + LocationIDs + ")";
            }
            sSQL = sSQL + ")";
            if (!string.IsNullOrEmpty(DepartmentIDs))
            {
                sSQL += " AND DepartmentID IN(" + DepartmentIDs + ")";
            }
            if (!string.IsNullOrEmpty(DesignationIDs))
            {
                sSQL += " AND DesignationID IN(" + DesignationIDs + ")";
            }
            if ((sSalary > 0 && eSalary > 0) && (sSalary <= eSalary))
            {
                sSQL += " AND CompGrossSalary BETWEEN '" + sSalary + "' AND '" + eSalary + "'";
            }
            if (BlockIDs != "")
            {
                sSQL += "AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + BlockIDs + "))";
            }
            if (GroupIDs != "")
            {
                sSQL += " AND EmployeeID IN( SELECT EmployeeID From EmployeeGroup WHERE EmployeeTypeID IN(" + GroupIDs + "))";
            }

            oELEncashComplianceDetails = ELEncashComplianceDetail.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            oELEncashComplianceDetails = oELEncashComplianceDetails.GroupBy(test => test.EmployeeID)
                   .Select(grp => grp.First())
                   .ToList();

            oELEncashComplianceDetails = oELEncashComplianceDetails.Where(x => x.EncashAmount > 0).ToList();
            oELEncashComplianceDetails = oELEncashComplianceDetails.OrderBy(x => x.EmployeeCode).ToList();

            sSQL = "Select * from SalarySheetSignature";
            var oSalarySheetSignature = SalarySheetSignature.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptEncashedELCompliance oReport = new rptEncashedELCompliance();
            byte[] abytes = oReport.PrepareReport(oELEncashComplianceDetails, sFormat, oSalarySheetSignature);
            return File(abytes, "application/pdf");

        }

        #endregion




    }
}
