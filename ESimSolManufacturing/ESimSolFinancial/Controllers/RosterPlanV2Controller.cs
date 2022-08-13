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
    public class RosterPlanV2Controller : Controller
    {

        #region Actions
        public ActionResult View_RosterPlan(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);


            RosterPlanEmployeeV2 oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            string sSql = "SELECT * FROM HRM_Shift WHERE IsActive=1";
            ViewBag.HRMShifts = HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oRosterPlanEmployeeV2);
        }

        #endregion

        #region Utility Functions
      
        [HttpPost]
        public JsonResult GetCount(HCMSearchObj oHCMSearchObj)
        {
            RosterPlanEmployeeV2 oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
            string sSQL = this.GetSQL(oHCMSearchObj);
            string sQuery = "SELECT COUNT(*) FROM View_RosterPlanEmployee AS RPE WHERE  RPE.AttendanceDate BETWEEN '" + oHCMSearchObj.StartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.EndDate.ToString("dd MMM yyyy") + "'" + sSQL;
            oRosterPlanEmployeeV2 = RosterPlanEmployeeV2.GetTotalCount(sQuery, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRosterPlanEmployeeV2);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(HCMSearchObj oHCMSearchObj)
        {
            string sSQL = "";
           
            if (oHCMSearchObj.EmployeeIDs != "" && oHCMSearchObj.EmployeeIDs != null)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT Emp.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.EmployeeIDs + "', ',') AS Emp)";
            }
            if (oHCMSearchObj.LocationIDs != "" && oHCMSearchObj.LocationIDs != null)
            {
                sSQL = sSQL + " AND RPE.LocationID IN (SELECT LOC.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.LocationIDs + "', ',') AS LOC)";
            }
            if (oHCMSearchObj.DepartmentIDs != "" && oHCMSearchObj.DepartmentIDs != null)
            {
                sSQL = sSQL + " AND RPE.DepartmentID IN (SELECT DEPT.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.DepartmentIDs + "', ',') AS DEPT)";
            }
            if (oHCMSearchObj.DesignationIDs != "" && oHCMSearchObj.DesignationIDs != null)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.DesignationID IN (" + oHCMSearchObj.DesignationIDs + "))";
            }
            if (oHCMSearchObj.BUIDs != "" && oHCMSearchObj.BUIDs != null)
            {
                sSQL = sSQL + " AND RPE.BusinessUnitID IN (SELECT BU.items FROM dbo.SplitInToDataSet('" + oHCMSearchObj.BUIDs + "', ',') AS BU)";
            }
            if(oHCMSearchObj.PIMSRoaster!=2)//Here 2 for Select One,1 for PIMS Roster and 0 for User Define
            {
               sSQL=sSQL+" AND RPE.IsPIMSRoster=" + oHCMSearchObj.PIMSRoaster;
            }
            if (oHCMSearchObj.BlockIDs != "" && oHCMSearchObj.BlockIDs != null)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.BlockIDs + "))";
            }
            if (oHCMSearchObj.GroupIDs != "" && oHCMSearchObj.GroupIDs != null)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.GroupIDs + "))";
            }
            if (oHCMSearchObj.EmployeeTypeIDs != "" && oHCMSearchObj.EmployeeTypeIDs != null)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID IN (" + oHCMSearchObj.EmployeeTypeIDs + "))";
            }
            if (oHCMSearchObj.ShiftIDs != "" && oHCMSearchObj.ShiftIDs != null)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.CurrentShiftID IN (" + oHCMSearchObj.ShiftIDs + "))";
            }
            if (oHCMSearchObj.SalarySchemeIDs != "" && oHCMSearchObj.SalarySchemeIDs != null)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.SalarySchemeID IN (" + oHCMSearchObj.SalarySchemeIDs + "))";
            }
            if (oHCMSearchObj.AttendanceSchemeIDs != "" && oHCMSearchObj.AttendanceSchemeIDs != null)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.AttendanceSchemeID IN (" + oHCMSearchObj.AttendanceSchemeIDs + "))";
            }
            if (oHCMSearchObj.CategoryID != 0)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT EC.EmployeeID FROM EmployeeConfirmation AS EC WHERE EO.EmployeeCategory=" + oHCMSearchObj.CategoryID + ")";
            }
            if (oHCMSearchObj.AuthenticationNo != "" && oHCMSearchObj.AuthenticationNo != null)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT EA.EmployeeID FROM EmployeeAuthentication AS EA WHERE EA.IsActive =1 AND EA.CardNo LIKE '%" + oHCMSearchObj.AuthenticationNo + "%')";
            }
            if (oHCMSearchObj.StartSalaryRange != 0 && oHCMSearchObj.EndSalaryRange != 0)
            {
                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.GrossAmount BETWEEN " + oHCMSearchObj.StartSalaryRange.ToString() + " AND " + oHCMSearchObj.EndSalaryRange.ToString() + ")";
            }

            if (oHCMSearchObj.IsJoiningDate == true)
            {

                sSQL = sSQL + " AND RPE.EmployeeID IN (SELECT EO.EmployeeID FROM EmployeeOfficial AS EO WHERE EO.DateOfJoin BETWEEN '" + oHCMSearchObj.JoiningStartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.JoiningEndDate.ToString("dd MMM yyyy") + "')";
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
        public JsonResult AdvanceSearch(HCMSearchObj oHCMSearchObj)
        {
            List<RosterPlanEmployeeV2> oRosterPlanEmployeeV2s = new List<RosterPlanEmployeeV2>();
            try
            {
                string sSQL = "SELECT top(" + oHCMSearchObj.LoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeCode) Row,* FROM  View_RosterPlanEmployee AS RPE WHERE  RPE.AttendanceDate BETWEEN '" + oHCMSearchObj.StartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.EndDate.ToString("dd MMM yyyy") + "'";
                sSQL = sSQL + this.GetSQL(oHCMSearchObj) + ") aa WHERE Row >" + oHCMSearchObj.RowLength + " ORDER BY EmployeeCode,AttendanceDate";
                oRosterPlanEmployeeV2s = RosterPlanEmployeeV2.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
                if (oRosterPlanEmployeeV2s.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                RosterPlanEmployeeV2 oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
                oRosterPlanEmployeeV2s = new List<RosterPlanEmployeeV2>();
                oRosterPlanEmployeeV2.ErrorMessage = ex.Message;
                oRosterPlanEmployeeV2s.Add(oRosterPlanEmployeeV2);
            }

            var jsonResult = Json(oRosterPlanEmployeeV2s, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetsByEmpCode(Employee oEmployee)
        {
            List<Employee> _oEmployees = new List<Employee>();

            string sSQL = "Select * from VIEW_Employee as P Where P.EmployeeID<>0 ";
            if (!String.IsNullOrEmpty(oEmployee.Name))
            {
                sSQL = sSQL + " AND P.Name Like '%" + oEmployee.Name + "%'";
                sSQL = sSQL + " OR P.Code Like '%" + oEmployee.Name + "%'";
            }


            _oEmployees = new List<Employee>();
            _oEmployees = Employee.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var jsonResult = Json(_oEmployees, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oEmployees);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetsEmployeeBatch(EmployeeBatch oEmployeeBatch)
        {
            List<EmployeeBatch> _oEmployeeBatchs = new List<EmployeeBatch>();

            string sSQL = "Select * from VIEW_EmployeeBatch as P Where P.EmployeeBatchID<>0 ";
            if (!String.IsNullOrEmpty(oEmployeeBatch.BatchName))
            {
                sSQL = sSQL + " AND P.BatchNo Like '%" + oEmployeeBatch.BatchName + "%'";
                sSQL = sSQL + " OR P.BatchName Like '%" + oEmployeeBatch.BatchName + "%'";
            }


            _oEmployeeBatchs = new List<EmployeeBatch>();
            _oEmployeeBatchs = EmployeeBatch.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            var jsonResult = Json(_oEmployeeBatchs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oEmployees);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

        }

       

        [HttpPost]
        public JsonResult UpdateRosterPlanEmployee(RosterPlanEmployeeV2 oRosterPlanEmployeeV2)
        {
            try
            {
                oRosterPlanEmployeeV2 = oRosterPlanEmployeeV2.UpdateRosterPlanEmployee(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
                oRosterPlanEmployeeV2.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRosterPlanEmployeeV2);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CommitRosterPlanEmployee(RosterPlanEmployeeV2 oRosterPlanEmployeeV2)
        {
            List<RosterPlanEmployeeV2> oRosterPlanEmployeeV2s = new List<RosterPlanEmployeeV2>();
            try
            {
                oRosterPlanEmployeeV2s = oRosterPlanEmployeeV2.CommitRosterPlanEmployee(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {

                 oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
                oRosterPlanEmployeeV2s = new List<RosterPlanEmployeeV2>();
                oRosterPlanEmployeeV2.ErrorMessage = ex.Message;
                oRosterPlanEmployeeV2s.Add(oRosterPlanEmployeeV2);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRosterPlanEmployeeV2s);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsRosterPlanEmployeeHistory(RosterPlanEmployeeV2 oRosterPlanEmployeeV2)
        {
            string sSQL = "";
            sSQL = "SELECT * FROM view_RosterPlanEmployeeLog AS RPEL WHERE RPEL.RPEID=" + oRosterPlanEmployeeV2.RPEID + " ORDER BY RPEL.RosterPlanEmployeeLogID";


            List<RosterPlanEmployeeV2> oRosterPlanEmployeeV2s = new List<RosterPlanEmployeeV2>();
            try
            {
                oRosterPlanEmployeeV2s = RosterPlanEmployeeV2.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oRosterPlanEmployeeV2s.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }

            }
            catch (Exception ex)
            {
                oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
                oRosterPlanEmployeeV2s = new List<RosterPlanEmployeeV2>();
                oRosterPlanEmployeeV2.ErrorMessage = ex.Message;
                oRosterPlanEmployeeV2s.Add(oRosterPlanEmployeeV2);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRosterPlanEmployeeV2s);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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

        #region RosterPlanEmployeeExcel
        public void ExcelRosterPlanEmployee(double ts)
        {
            HCMSearchObj oHCMSearchObj = new HCMSearchObj();
            RosterPlanEmployeeV2 oRosterPlanEmployeeV2 = new RosterPlanEmployeeV2();
            List<RosterPlanEmployeeV2> oRosterPlanEmployeeV2s = new List<RosterPlanEmployeeV2>();
            try
            {
                oHCMSearchObj = (HCMSearchObj)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_RosterPlanEmployee AS RPE WHERE  RPE.AttendanceDate BETWEEN '" + oHCMSearchObj.StartDate.ToString("dd MMM yyyy") + "' AND '" + oHCMSearchObj.EndDate.ToString("dd MMM yyyy") + "'" + this.GetSQL(oHCMSearchObj) + " ORDER BY EmployeeCode,AttendanceDate";
                DateTime StartDate = oHCMSearchObj.StartDate;
                DateTime EndDate = oHCMSearchObj.EndDate;
                DateTime StartDateN = oHCMSearchObj.StartDate;
                DateTime EndDateN = oHCMSearchObj.EndDate;



                oRosterPlanEmployeeV2s = RosterPlanEmployeeV2.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oRosterPlanEmployeeV2s.Count <= 0)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }
                ExcelRange cell;
                OfficeOpenXml.Style.Border border;
                ExcelFill fill;
                int colIndex = 1;
                int rowIndex = 1;

                int nDays = (int)(oHCMSearchObj.EndDate - oHCMSearchObj.StartDate).TotalDays + 1;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Employee Roster Plan");
                    sheet.Name = "Employee Roster Plan";

                    int n = 1;
                    sheet.Column(n++).Width = 15;//code
                    for (int i = 1; i <= nDays; i++)
                    {
                        sheet.Column(n++).Width = 15;//date
                    }

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    oHCMSearchObj.EndDate = oHCMSearchObj.EndDate.AddDays(1);
                    while (oHCMSearchObj.StartDate.ToString("dd MMM yyyy") != oHCMSearchObj.EndDate.ToString("dd MMM yyyy"))
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oHCMSearchObj.StartDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        oHCMSearchObj.StartDate = oHCMSearchObj.StartDate.AddDays(1);
                    }
                    rowIndex += 1;

                    bool flag = false;
                    int EmpID = 0;
                    foreach (RosterPlanEmployeeV2 oItem in oRosterPlanEmployeeV2s)
                    {
                        if (EmpID == oItem.EmployeeID)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                        EmpID = oItem.EmployeeID;
                        if (!flag)
                        {

                            colIndex = 1;
                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = true;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            EndDate = EndDate.AddDays(1);
                            while (StartDate.ToString("dd MMM yyyy") != EndDate.ToString("dd MMM yyyy"))
                            {
                                oRosterPlanEmployeeV2 = oRosterPlanEmployeeV2s.Where(x => x.EmployeeID == oItem.EmployeeID && x.AttendanceDate.ToString("dd MMM yyyy") == StartDate.ToString("dd MMM yyyy")).FirstOrDefault();

                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oRosterPlanEmployeeV2 == null) ? "" : oRosterPlanEmployeeV2.ShiftName; cell.Style.Font.Bold = true;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                StartDate = StartDate.AddDays(1);
                            }
                            rowIndex += 1;

                            StartDate = StartDateN;
                            EndDate = EndDateN;
                        }
                    }



                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=EmployeeRosterPlan.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
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
                    var sheet = excelPackage.Workbook.Worksheets.Add("Employee Roster Plan");
                    sheet.Name = "Employee Roster Plan";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=EmployeeRosterPlan.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
           
        }



        #endregion
    }
}