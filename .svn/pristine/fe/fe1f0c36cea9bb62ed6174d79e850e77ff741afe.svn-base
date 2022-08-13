using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{
    public class BenefitOnAttendanceController : Controller
    {
        #region Declaration
        BenefitOnAttendance _oBenefitOnAttendance;
        List<BenefitOnAttendance> _oBenefitOnAttendances;
        BenefitOnAttendanceEmployee _oBenefitOnAttendanceEmployee;
        List<BenefitOnAttendanceEmployee> _oBenefitOnAttendanceEmployees;
        #endregion

        #region Views
        public ActionResult View_BenefitOnAttendances(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oBenefitOnAttendances = new List<BenefitOnAttendance>();
            string sSql = "SELECT * FROM View_BenefitOnAttendance ";
            _oBenefitOnAttendances = BenefitOnAttendance.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            return View(_oBenefitOnAttendances);
        }

        public ActionResult View_BenefitOnAttendance(string sid, string sMsg)//BOAID
        {
            int nId = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oBenefitOnAttendance = new BenefitOnAttendance();
            if (nId > 0)
            {
                _oBenefitOnAttendance = BenefitOnAttendance.Get(nId, (int)Session[SessionInfo.currentUserID]);
            }
            if (sMsg != "N/A")
            {
                _oBenefitOnAttendance.ErrorMessage = sMsg;
            }
            ViewBag.SalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType=2", (int)Session[SessionInfo.currentUserID]);
            ViewBag.LeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead", (int)Session[SessionInfo.currentUserID]);
            ViewBag.Holidays = Holiday.Gets("SELECT * FROM Holiday", (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            ViewBag.Company = oCompany.GetCompanyLogo(1, 0);
            return View(_oBenefitOnAttendance);
        }

        public ActionResult View_BenefitOnAttendanceEmployee(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<BenefitOnAttendanceEmployee> oBenefitOnAttendanceEmployees = new List<BenefitOnAttendanceEmployee>();

            List<BenefitOnAttendance> oBenefitOnAttendances = new List<BenefitOnAttendance>();
            string sSql = "Select * from View_BenefitOnAttendance Where ApproveBy<>0 And InactiveBy=0";
            oBenefitOnAttendances = BenefitOnAttendance.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            ViewBag.BenefitOnAttendances = oBenefitOnAttendances;
            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Shifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));

            return View(oBenefitOnAttendanceEmployees);
        }
        #endregion

        #region oBenefitOnAttendance_IUD
        [HttpPost]
        public JsonResult BenefitOnAttendance_IU(BenefitOnAttendance oBenefitOnAttendance)
        {
            try
            {
                if (oBenefitOnAttendance.BOAID > 0)
                {
                    oBenefitOnAttendance = oBenefitOnAttendance.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oBenefitOnAttendance = oBenefitOnAttendance.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oBenefitOnAttendance = new BenefitOnAttendance();
                oBenefitOnAttendance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBenefitOnAttendance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult GetBenefitOnAttendance(BenefitOnAttendance oBenefitOnAttendance)
        {
            if (oBenefitOnAttendance.BOAID > 0)
            {
                oBenefitOnAttendance = BenefitOnAttendance.Get("SELECT * FROM View_BenefitOnAttendance WHERE BOAID=" + oBenefitOnAttendance.BOAID, (int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBenefitOnAttendance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BenefitOnAttendance_Delete(BenefitOnAttendance oBenefitOnAttendance)
        {
            try
            {
                oBenefitOnAttendance = oBenefitOnAttendance.IUD((int)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBenefitOnAttendance = new BenefitOnAttendance();
                oBenefitOnAttendance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBenefitOnAttendance.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion oBenefitOnAttendance_IUD

        #region Approve
        [HttpPost]
        public JsonResult BenefitOnAttendance_Approve(BenefitOnAttendance oBenefitOnAttendance)
        {
            try
            {
                oBenefitOnAttendance.ApproveBy = (int)Session[SessionInfo.currentUserID];
                oBenefitOnAttendance = oBenefitOnAttendance.IUD((int)EnumDBOperation.Approval, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBenefitOnAttendance = new BenefitOnAttendance();
                oBenefitOnAttendance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBenefitOnAttendance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Approve

        #region Inactive
        [HttpPost]
        public JsonResult BenefitOnAttendance_Inactive(BenefitOnAttendance oBenefitOnAttendance)
        {
            try
            {
                oBenefitOnAttendance.InactiveBy = (int)Session[SessionInfo.currentUserID];
                oBenefitOnAttendance = oBenefitOnAttendance.IUD((int)EnumDBOperation.InActive, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBenefitOnAttendance = new BenefitOnAttendance();
                oBenefitOnAttendance.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBenefitOnAttendance);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Approve

        #region Search

        [HttpPost]
        public JsonResult GetsBenefitOnAttendance(BenefitOnAttendance oBenefitOnAttendance)
        {
            BenefitOnAttendance oBOA = new BenefitOnAttendance();
            oBOA.BOAs = BenefitOnAttendance.Gets("SELECT * FROM View_BenefitOnAttendance WHERE  ApproveBy>0  AND (InactiveBy<=0 OR InactiveBy IS NULL)", (int)Session[SessionInfo.currentUserID]);
            oBOA.BOAEmployees = BenefitOnAttendanceEmployee.Gets("SELECT * FROM View_BenefitOnAttendanceEmployee WHERE EmployeeID=" + oBenefitOnAttendance.nID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBOA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsBenefitOnAttendanceEmployee(Employee oEmployee)
        {
            List<BenefitOnAttendanceEmployee> _oBenefitOnAttendanceEmployees = new List<BenefitOnAttendanceEmployee>();
            _oBenefitOnAttendanceEmployees = BenefitOnAttendanceEmployee.Gets(oEmployee.EmployeeID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBenefitOnAttendanceEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsBenefitOnAttendanceEmployeeStopped(BenefitOnAttendanceEmployee oBenefitOnAttendanceEmployee)
        {
            BenefitOnAttendanceEmployeeStopped oBenefitOnAttendanceEmployeeStopped = new BenefitOnAttendanceEmployeeStopped();
            oBenefitOnAttendanceEmployeeStopped = oBenefitOnAttendanceEmployeeStopped.GetBy(oBenefitOnAttendanceEmployee.EmployeeID, oBenefitOnAttendanceEmployee.BOAID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBenefitOnAttendanceEmployeeStopped);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region oBenefitOnAttendance_Emp_IUD
        [HttpPost]
        public JsonResult BenefitOnAttendanceEmployee_IU(BenefitOnAttendanceEmployee oBOAEmp)
        {
            try
            {
                if (oBOAEmp.BOAEmployeeID > 0)
                {
                    oBOAEmp = oBOAEmp.IUD((int)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oBOAEmp = oBOAEmp.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oBOAEmp = new BenefitOnAttendanceEmployee();
                oBOAEmp.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBOAEmp);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion oBenefitOnAttendance_Emp_IUD

        #region Report
        public ActionResult View_BenefitOnAttendances_Report(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oBenefitOnAttendances = new List<BenefitOnAttendance>();
            string sSql = "SELECT * FROM View_BenefitOnAttendance ";
            _oBenefitOnAttendances = BenefitOnAttendance.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            string SsQL = "SELECT * FROM View_EmployeeOfficial WHERE EmployeeID =" + ((User)(Session[SessionInfo.CurrentUser])).EmployeeID;
            ViewBag.EmployeeOfficial = EmployeeOfficial.Get(SsQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.EmployeeTypes = EmployeeType.Gets("SELECT * FROM EmployeeType WHERE EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, (int)Session[SessionInfo.currentUserID]);
            ViewBag.StaffWorkers = EmployeeType.Gets("SELECT * FROM EmployeeType WHERE EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Shifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            sSql = "";
            sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            return View(_oBenefitOnAttendances);
        }
        #endregion Report

        #region BOAEL
        [HttpPost]
        public JsonResult GetsBOAEL(string sParam, double nts)
        {
            List<BenefitOnAttendanceReport> oBOARs = new List<BenefitOnAttendanceReport>();
            List<BenefitOnAttendanceReport> oTempBOARs = new List<BenefitOnAttendanceReport>();
            BenefitOnAttendanceReport oBOAR = new BenefitOnAttendanceReport();

            oBOARs = GetsBOALists(sParam);
            oBOARs.ForEach(x => oTempBOARs.Add(x));

            if (oBOARs.Count() > 0)
            {
                oBOARs[0].CellRowSpans = this.RowMerge(oTempBOARs);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBOARs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public List<BenefitOnAttendanceReport> GetsBOALists(string sParam)
        {
            List<BenefitOnAttendanceReport> oBOARs = new List<BenefitOnAttendanceReport>();
            BenefitOnAttendanceReport oBOAR = new BenefitOnAttendanceReport();
            try
            {
                string sDateFrom = sParam.Split('~')[0];
                string sDateTo = sParam.Split('~')[1];
                string BOAIDs = sParam.Split('~')[2];
                string sEmployeeIDs = sParam.Split('~')[3];
                string sLocationID = sParam.Split('~')[4];
                string sDepartmentIDs = sParam.Split('~')[5];
                string sBusinessUnitIDs = sParam.Split('~')[6];
                double nStartSalaryRange = Convert.ToDouble(sParam.Split('~')[7]);
                double nEndSalaryRange = Convert.ToDouble(sParam.Split('~')[8]);

                //string sSql = "SELECT * FROM View_BenefitOnAttendanceEmployeeLedger WHERE AttendanceDate BETWEEN '" + sDateFrom + "' AND '" + sDateTo + "'";
                //if (BOAIDs !="")
                //{
                //    sSql += " AND BOAID IN(" + BOAIDs+")";
                //}
                //if (sEmployeeIDs != "")
                //{
                //    sSql += " AND EmployeeID IN(" + sEmployeeIDs + ")";
                //}
                //if (sDepartmentIDs != "")
                //{
                //    sSql += " AND DepartmentID IN (" + sDepartmentIDs + ")";
                //}
                //BOAELs = BenefitOnAttendanceEmployeeLedger.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

                oBOARs = BenefitOnAttendanceReport.Gets(Convert.ToDateTime(sDateFrom), Convert.ToDateTime(sDateTo), BOAIDs, sEmployeeIDs, sLocationID, sDepartmentIDs, sBusinessUnitIDs, nStartSalaryRange, nEndSalaryRange,(int)Session[SessionInfo.currentUserID]);
                if (oBOARs.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oBOARs = new List<BenefitOnAttendanceReport>();
                oBOAR = new BenefitOnAttendanceReport();
                oBOAR.ErrorMessage = ex.Message;
                oBOARs.Add(oBOAR);
            }
            return oBOARs;
        }
         
        public ActionResult PrintBOAEL(string sParam, double nts)
        {
            BenefitOnAttendanceEmployeeLedger BOAEL = new BenefitOnAttendanceEmployeeLedger();
            string sDateFrom = sParam.Split('~')[0];
            string sDateTo = sParam.Split('~')[1];
            string BOAIDs = sParam.Split('~')[2];
            string sEmployeeIDs = sParam.Split('~')[3];
            string sLocationID = sParam.Split('~')[4];
            string sDepartmentIDs= sParam.Split('~')[5];
            string sBusinessUnitIds = sParam.Split('~')[6];

            string sSql = "SELECT * FROM View_BenefitOnAttendanceEmployeeLedger WHERE AttendanceDate BETWEEN '" + sDateFrom + "' AND '" + sDateTo + "'";
            if (BOAIDs !="")
            {
                sSql += " AND BOAID IN(" + BOAIDs+")";
            }
            if (sEmployeeIDs != "")
            {
                sSql += " AND EmployeeID IN(" + sEmployeeIDs + ")";
            }
            if (sDepartmentIDs != "")
            {
                sSql += " AND DepartmentID IN (" + sDepartmentIDs + ")";
            }
            if (sLocationID != "")
            {
                sSql += " AND EmployeeID IN(SELECT  EmployeeID FROM View_EmployeeOfficial WHERE LocationID IN(" + sLocationID + "))";
            }
            if (sBusinessUnitIds != "")
            {
                sSql += " AND EmployeeID IN(SELECT EmployeeID FROM View_Employee WHERE BusinessUnitID IN(" + sBusinessUnitIds + "))";
            }
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + "AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            BOAEL.BOAELs = BenefitOnAttendanceEmployeeLedger.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
            BOAEL.Company = oCompanys.First();
            BOAEL.ErrorMessage = sDateFrom + "~" + sDateTo;

            rptBOAEL oReport = new rptBOAEL();
            byte[] abytes = oReport.PrepareReport(BOAEL);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintFormat2(string sParam, double nts)
        {
            List<BenefitOnAttendanceReport> oBOAELs = new List<BenefitOnAttendanceReport>();
            List<BenefitOnAttendanceReport> oTempBOAELs = new List<BenefitOnAttendanceReport>();
            List<BenefitOnAttendanceReport> oDistinctBOAs = new List<BenefitOnAttendanceReport>();
            BenefitOnAttendanceReport oBOAEL = new BenefitOnAttendanceReport();

            oBOAELs = GetsBOALists(sParam);
            oBOAELs.ForEach(x => oTempBOAELs.Add(x));
            oDistinctBOAs = oBOAELs.GroupBy(x => x.BOAName).Select(grp => grp.First()).ToList();


            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);

            rptBOAEL_Format2 oReport = new rptBOAEL_Format2();
            byte[] abytes = oReport.PrepareReport(oBOAELs, oDistinctBOAs, oCompanys.First());
            return File(abytes, "application/pdf");
        }

        public void PrintBOAEL_XL(string sParam, double nts)
        {
            List<BenefitOnAttendanceReport> oBOAELs = new List<BenefitOnAttendanceReport>();
            List<BenefitOnAttendanceReport> oTempBOAELs = new List<BenefitOnAttendanceReport>();
            List<BenefitOnAttendanceReport> oDistinctBOAs = new List<BenefitOnAttendanceReport>();
            BenefitOnAttendanceReport oBOAEL = new BenefitOnAttendanceReport();

            oBOAELs = GetsBOALists(sParam);
            oBOAELs.ForEach(x => oTempBOAELs.Add(x));
            oDistinctBOAs = oBOAELs.GroupBy(x => x.BOAName).Select(grp => grp.First()).ToList();

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
                var sheet = excelPackage.Workbook.Worksheets.Add("Benefit On Attendance");
                sheet.Name = "Benefit On Attendance";

                nMaxColumn = oDistinctBOAs.Count * 3 + 6;

                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 30; //CODE
                sheet.Column(4).Width = 30; //NAME
                sheet.Column(5).Width = 30; //W Status
                int i = 0;
                for (i = 6; i < nMaxColumn; i++)
                {
                    sheet.Column(i).Width = 18; //Addition Salary Heads
                }
                sheet.Column(i).Width = 18; //Total Amount

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Benefit On Attendance"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Merge = true;
                cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 1, colIndex]; cell.Merge = true;
                cell.Value = "CODE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 1, colIndex]; cell.Merge = true;
                cell.Value = "NAME"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 1, colIndex]; cell.Merge = true;
                cell.Value = "W. STATUS"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                foreach (BenefitOnAttendanceReport oitem in oDistinctBOAs)
                {

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + 2]; cell.Merge = true;
                    cell.Value = oitem.BOAName; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 1, colIndex]; cell.Merge = true; cell.Value = "TOTAL AMOUNT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 6;

                foreach (BenefitOnAttendanceReport oitem in oDistinctBOAs)
                {
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "TOTAL DAY "; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BENEFIT"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "AMOUNT"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }

                #endregion

                #region Table Body

                int nSL = 0;
                while (oBOAELs.Count > 0)
                {
                    List<BenefitOnAttendanceReport> oTempBOAREmps = new List<BenefitOnAttendanceReport>();
                    oTempBOAREmps = oBOAELs.Where(x => x.EmployeeID == oBOAELs.First().EmployeeID).OrderBy(x => x.EmployeeID).ToList();
                    oBOAELs.RemoveAll(x => x.EmployeeID == oTempBOAREmps.First().EmployeeID);

                    nSL++;
                    colIndex = 2;
                    rowIndex++;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempBOAREmps[0].EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempBOAREmps[0].EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempBOAREmps[0].WorkingStatusInST; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    Double nTotalAmount = 0;

                    foreach (BenefitOnAttendanceReport oitm in oDistinctBOAs)
                    {
                        List<BenefitOnAttendanceReport> oTempBOARs = new List<BenefitOnAttendanceReport>();
                        oTempBOARs = oTempBOAREmps.Where(x => x.BOAName == oitm.BOAName).ToList();

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempBOARs.Count > 0 ? oTempBOARs[0].TotalDay : 0; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempBOARs.Count > 0 ? oTempBOARs[0].Benefit : ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oTempBOARs.Count > 0 ? oTempBOARs[0].Amount : 0); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nTotalAmount += oTempBOARs.Count > 0 ? oTempBOARs[0].Amount : 0;
                    }

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalAmount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                }

                rowIndex++;
                colIndex = 2;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = 5]; cell.Merge = true;
                cell.Value = "TOTAL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                foreach (BenefitOnAttendanceReport oitm in oDistinctBOAs)
                {
                    colIndex++;

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, ++colIndex]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    Double nTotalAmount = 0;
                    nTotalAmount = oTempBOAELs.Where(x => x.BOAName == oitm.BOAName).Sum(x => x.Amount);
                    cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = nTotalAmount > 0 ? Global.MillionFormat(nTotalAmount) : "-"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                }


                cell = sheet.Cells[rowIndex, nMaxColumn]; cell.Value = Global.MillionFormat(oTempBOAELs.Sum(x => x.Amount)); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BENEFIT.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }



        public void PrintExtraBenefit_XLFormat2(string sParam, double nts)
        {
            List<BenefitOnAttendanceReport> oBOAELs = new List<BenefitOnAttendanceReport>();
            List<BenefitOnAttendanceReport> oTempBOAELs = new List<BenefitOnAttendanceReport>();
            List<BenefitOnAttendanceReport> oDistinctBOAs = new List<BenefitOnAttendanceReport>();
            List<EmployeeSalaryStructure> oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
            List<EmployeeSalaryStructureDetail> oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();

            BenefitOnAttendanceReport oBOAEL = new BenefitOnAttendanceReport();

            oBOAELs = GetsBOALists(sParam);
            oBOAELs.ForEach(x => oTempBOAELs.Add(x));
            oBOAELs = oBOAELs.OrderBy(x => x.DepartmentName).ThenBy(x => x.EmployeeID).ToList();
            oDistinctBOAs = oBOAELs.GroupBy(x => x.BOAName).Select(grp => grp.First()).ToList();



            string EmpIDs = "";
            string sSql = "";
            if (oBOAELs.Count > 0)
            {
                string TempEmpIDs = "";
                int nCount = 0;
                //oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
                foreach (BenefitOnAttendanceReport oItem in oBOAELs)
                {
                    TempEmpIDs += oItem.EmployeeID + ",";
                    EmpIDs += oItem.EmployeeID + ",";
                    nCount++;

                    if (nCount % 100 == 0 || nCount == oBOAELs.Count)
                    {
                        TempEmpIDs = TempEmpIDs.Remove(TempEmpIDs.Length - 1, 1);
                        sSql = "";
                        sSql = "SELECT * FROM EmployeeSalaryStructure WHERE EmployeeID IN (" + TempEmpIDs + ")";
                        List<EmployeeSalaryStructure> oEmployeeSalaryStructuresT = new List<EmployeeSalaryStructure>();
                        oEmployeeSalaryStructuresT = EmployeeSalaryStructure.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                        oEmployeeSalaryStructures.AddRange(oEmployeeSalaryStructuresT);


                        sSql = "";
                        sSql = "select MAX(Amount) As Amount, ESSID from EmployeeSalaryStructureDetail WHERE ESSID IN(SELECT ESSID FROM EmployeeSalaryStructure WHERE EmployeeID IN(" + TempEmpIDs + ")) Group By ESSID";
                        List<EmployeeSalaryStructureDetail> ooEmployeeSalaryStructureDetailsT = new List<EmployeeSalaryStructureDetail>();
                        ooEmployeeSalaryStructureDetailsT = EmployeeSalaryStructureDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                        oEmployeeSalaryStructureDetails.AddRange(ooEmployeeSalaryStructureDetailsT);

                        TempEmpIDs = "";
                    }
                }
                EmpIDs = EmpIDs.Remove(EmpIDs.Length - 1, 1);
            }
            else
            {
                oEmployeeSalaryStructures = new List<EmployeeSalaryStructure>();
                oEmployeeSalaryStructureDetails = new List<EmployeeSalaryStructureDetail>();
            }

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
                var sheet = excelPackage.Workbook.Worksheets.Add("Benefit On Attendance");
                sheet.Name = "Benefit On Attendance";


                sheet.Column(2).Width = 8; 
                sheet.Column(3).Width = 30; 
                sheet.Column(4).Width = 30; 
                sheet.Column(5).Width = 30; 
                sheet.Column(6).Width = 20; 
                sheet.Column(7).Width = 20; 
                sheet.Column(8).Width = 15; 
                sheet.Column(9).Width = 30; 
                sheet.Column(10).Width = 15; 
                sheet.Column(11).Width = 15;
                nMaxColumn = 11;
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Benefit On Attendance"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "CODE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "NAME"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "DEPARTMENT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "JOINING DATE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "TOTAL DAY"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "BENEFIT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "GROSS"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "BASIC"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 2;

                #endregion

                #region Table Body

                int nSL = 0;
                while (oBOAELs.Count > 0)
                {

                    var oResults = new List<BenefitOnAttendanceReport>();
                    oResults = oBOAELs.Where(x => x.DepartmentName == oBOAELs[0].DepartmentName).OrderBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();


                    //List<BenefitOnAttendanceReport> oTempBOAREmps = new List<BenefitOnAttendanceReport>();
                    //oTempBOAREmps = oBOAELs.Where(x=>x.DepartmentName == oBOAELs[0].DepartmentName &&  x.EmployeeID == oBOAELs[0].EmployeeID).OrderBy(x=>x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();
                    oBOAELs.RemoveAll(x => x.DepartmentName == oResults[0].DepartmentName);

                    nSL++;
                    colIndex = 2;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department : " + oResults.FirstOrDefault().DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                    colIndex = 2;
                    foreach(BenefitOnAttendanceReport oListItem in oResults)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL++; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.EmployeeName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.DepartmentName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.DesignationName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.JoiningDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        foreach (BenefitOnAttendanceReport oitm in oDistinctBOAs)
                        {
                            List<BenefitOnAttendanceReport> oTempBOARs = new List<BenefitOnAttendanceReport>();
                            oTempBOARs = oResults.Where(x => x.BOAName == oitm.BOAName).ToList();

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempBOARs.Count > 0 ? oTempBOARs[0].TotalDay : 0; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oTempBOARs.Count > 0 ? oTempBOARs[0].Benefit : ""; cell.Style.Font.Bold = false;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oTempBOARs.Count > 0 ? oTempBOARs[0].Amount : 0); cell.Style.Font.Bold = false;
                            //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            //nTotalAmount += oTempBOARs.Count > 0 ? oTempBOARs[0].Amount : 0;
                        }

                        double Gross = 0.0;
                        double Basic = 0.0;
                        int ESSID = 0;

                        var oResult = oEmployeeSalaryStructures.Where(x => x.EmployeeID == oListItem.EmployeeID).ToList();

                        Gross = (oResult.Count > 0 ? oResult.FirstOrDefault().GrossAmount : 0);
                        ESSID = (oResult.Count > 0 ? oResult.FirstOrDefault().ESSID : 0);

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Gross, 2); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        var oResult1 = oEmployeeSalaryStructureDetails.Where(x => x.ESSID == ESSID).ToList();
                        Basic = (oResult1.Count > 0 ? oResult1.FirstOrDefault().Amount : 0);

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(Basic, 2); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                        colIndex = 2;
                    }
                    nSL = 0;
                }


                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BENEFIT.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        public ActionResult PrintCompDayAllowance(string sParam, double nts)
        {
            List<ExtraBenefit> oExtraBenefits = new List<ExtraBenefit>();
            string sDateFrom = sParam.Split('~')[0];
            string sDateEnd = sParam.Split('~')[1];
            string BOAIDs = sParam.Split('~')[2];
            string sEmployeeIDs = sParam.Split('~')[3];
            string sLocationID = sParam.Split('~')[4];
            string sDepartmentIDs = sParam.Split('~')[5];
            string sBusinessUnitIDs = sParam.Split('~')[6];
            double nStartSalaryRange = Convert.ToDouble(sParam.Split('~')[7]);
            double nEndSalaryRange = Convert.ToDouble(sParam.Split('~')[8]);

            oExtraBenefits = ExtraBenefit.Gets(Convert.ToDateTime(sDateFrom), Convert.ToDateTime(sDateEnd), BOAIDs, sEmployeeIDs, sLocationID, sDepartmentIDs, sBusinessUnitIDs, nStartSalaryRange, nEndSalaryRange, (int)Session[SessionInfo.currentUserID]);
            List<ExtraBenefit> oTempExtraBenefits = new List<ExtraBenefit>();
            oExtraBenefits.ForEach(x => oTempExtraBenefits.Add(x));

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<SalarySheetSignature> oSalarySheetSignatures = new List<SalarySheetSignature>();
            List<SalarySheetSignature> oTempSalarySheetSignatures = new List<SalarySheetSignature>();

            SalarySheetSignature oSalarySheetSignature = new SalarySheetSignature();
            oSalarySheetSignature.SignatureID = 1; oSalarySheetSignature.SignatureName = "Prepared By"; oSalarySheetSignatures.Add(oSalarySheetSignature); oSalarySheetSignature = new SalarySheetSignature();
            oSalarySheetSignature.SignatureID = 2; oSalarySheetSignature.SignatureName = "Checked by"; oSalarySheetSignatures.Add(oSalarySheetSignature); oSalarySheetSignature = new SalarySheetSignature();
            oSalarySheetSignature.SignatureID = 3; oSalarySheetSignature.SignatureName = "Chief Accountant"; oSalarySheetSignatures.Add(oSalarySheetSignature); oSalarySheetSignature = new SalarySheetSignature();
            oSalarySheetSignature.SignatureID = 4; oSalarySheetSignature.SignatureName = "Director"; oSalarySheetSignatures.Add(oSalarySheetSignature);

            rptCompDayAllowance oReport = new rptCompDayAllowance();
            byte[] abytes = oReport.PrepareReport(oExtraBenefits, oCompany, oBusinessUnit, oSalarySheetSignatures, "(" + sDateFrom + " to " + sDateEnd + ")");
            return File(abytes, "application/pdf");

        }

        public void ExcelCompDayAllowance(string sParam, double nts)
        {
            List<ExtraBenefit> oExtraBenefits = new List<ExtraBenefit>();
            string sDateFrom = sParam.Split('~')[0];
            string sDateEnd = sParam.Split('~')[1];
            string BOAIDs = sParam.Split('~')[2];
            string sEmployeeIDs = sParam.Split('~')[3];
            string sLocationID = sParam.Split('~')[4];
            string sDepartmentIDs = sParam.Split('~')[5];
            string sBusinessUnitIDs = sParam.Split('~')[6];
            double nStartSalaryRange = Convert.ToDouble(sParam.Split('~')[7]);
            double nEndSalaryRange = Convert.ToDouble(sParam.Split('~')[8]);

            oExtraBenefits = ExtraBenefit.Gets(Convert.ToDateTime(sDateFrom), Convert.ToDateTime(sDateEnd), BOAIDs, sEmployeeIDs, sLocationID, sDepartmentIDs, sBusinessUnitIDs, nStartSalaryRange, nEndSalaryRange, (int)Session[SessionInfo.currentUserID]);

            List<ExtraBenefit> oTempExtraBenefits = new List<ExtraBenefit>();
            oExtraBenefits.ForEach(x => oTempExtraBenefits.Add(x));

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
                var sheet = excelPackage.Workbook.Worksheets.Add("Benefit On Attendance");
                sheet.Name = "Benefit On Attendance";


                sheet.Column(2).Width = 8;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 15;
                sheet.Column(9).Width = 15;
                sheet.Column(10).Width = 15;
                sheet.Column(11).Width = 15;
                nMaxColumn = 11;
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Dayoff Allowance"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Sl"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Employee ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Date of join"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Per Day"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Signature"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 2;

                #endregion

                #region Table Body
                oExtraBenefits = oExtraBenefits.OrderBy(x => x.AttendanceDateInString).ThenBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
                int nSL = 0;
                while (oExtraBenefits.Count > 0)
                {

                    var oResults = new List<ExtraBenefit>();
                    oResults = oExtraBenefits.Where(x => x.BusinessUnitName == oExtraBenefits[0].BusinessUnitName && x.LocationName == oExtraBenefits[0].LocationName && x.DepartmentName == oExtraBenefits[0].DepartmentName).ToList();

                    var oResultGroup = oResults.GroupBy(x => new { x.BusinessUnitName, x.LocationName, x.DepartmentName, x.EmployeeCode}, (key, grp) => new ExtraBenefit
                    {
                        EmployeeCode = grp.First().EmployeeCode,
                        EmployeeName = grp.First().EmployeeName,
                        Days = grp.Count(),
                        BusinessUnitName = key.BusinessUnitName,
                        LocationName = grp.First().LocationName,
                        DepartmentName = grp.First().DepartmentName,
                        DesignationName = grp.First().DesignationName,
                        JoiningDate = grp.First().JoiningDate,
                        Salary = grp.First().Salary,
                        PerDayAmount = grp.First().PerDayAmount,
                        PayableAmount = grp.First().PerDayAmount * grp.Count()

                    }).OrderBy(x => x.BusinessUnitName).ThenBy(x=>x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();

                    oExtraBenefits.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName);

                    nSL++;
                    colIndex = 2;
                    //rowIndex++;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Unit : " + oResultGroup.FirstOrDefault().LocationName + ", Department : " + oResultGroup.FirstOrDefault().DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                    colIndex = 2;
                    oResults = oResults.OrderBy(x => x.EmployeeCode).ToList();
                    foreach (ExtraBenefit oListItem in oResultGroup)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL++; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.EmployeeName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.DesignationName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.JoiningDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oListItem.Salary); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Dayoff
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.Days; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oListItem.PerDayAmount); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oListItem.PayableAmount); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                        rowIndex++;
                        colIndex = 2;
                    }
                    nSL = 0;

                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oResultGroup.Sum(x => x.Salary)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Dayoff
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oResultGroup.Sum(x => x.PerDayAmount)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oResultGroup.Sum(x => x.PayableAmount)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex += 1;

                }


                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oTempExtraBenefits.Sum(x => x.Salary)); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Dayoff
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oTempExtraBenefits.Sum(x => x.PerDayAmount)); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oTempExtraBenefits.Sum(x => x.PayableAmount)); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BENEFIT.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }

        public void ExcelExportBenefitOfAttendance(string sParam, double nts)
        {
            List<ExtraBenefit> oExtraBenefits = new List<ExtraBenefit>();
            string sDateFrom = sParam.Split('~')[0];
            string sDateEnd = sParam.Split('~')[1];
            string BOAIDs = sParam.Split('~')[2];
            string sEmployeeIDs = sParam.Split('~')[3];
            string sLocationID = sParam.Split('~')[4];
            string sDepartmentIDs = sParam.Split('~')[5];
            string sBusinessUnitIDs = sParam.Split('~')[6];
            double nStartSalaryRange = Convert.ToDouble(sParam.Split('~')[7]);
            double nEndSalaryRange = Convert.ToDouble(sParam.Split('~')[8]);
            int nEmployeeType = Convert.ToInt32(sParam.Split('~')[9]);
            bool bIsComp = Convert.ToBoolean(sParam.Split('~')[10]);

            string sSQL = " AND HH.AttendanceDate BETWEEN '" + sDateFrom + "' AND '" + sDateEnd+"'";
            if (sEmployeeIDs != null && sEmployeeIDs != "")
            {
                sSQL = sSQL + " AND HH.EmployeeID IN (" + sEmployeeIDs + ")";
            }
            if (sLocationID != null && sLocationID != "")
            {
                sSQL = sSQL + " AND HH.LocationID IN (" + sLocationID + ")";
            }
            if (sDepartmentIDs != null && sDepartmentIDs != "")
            {
                sSQL = sSQL + " AND HH.DepartmentID IN (" + sDepartmentIDs + ")";
            }
            if (sBusinessUnitIDs != null && sBusinessUnitIDs != "")
            {
                sSQL = sSQL + " AND HH.BusinessUnitID IN (" + sBusinessUnitIDs + ")";
            }
            if (nStartSalaryRange != 0 && nEndSalaryRange != 0)
            {
                sSQL = sSQL + " AND HH.EmployeeID IN (SELECT ESS.EmployeeID FROM EmployeeSalaryStructure AS ESS WHERE ESS.GrossAmount BETWEEN " + nStartSalaryRange.ToString() + " AND " + nEndSalaryRange.ToString() + ")";
            }
            if (nEmployeeType != 0)
            {
                sSQL = sSQL + " AND HH.EmployeeID IN (SELECT EG.EmployeeID FROM EmployeeGroup AS EG WHERE EG.EmployeeTypeID = " + nEmployeeType.ToString() + ")";
            }

            oExtraBenefits = ExtraBenefit.Gets(sSQL, bIsComp, (int)(Session[SessionInfo.currentUserID]));
            List<ExtraBenefit> oTempExtraBenefits = new List<ExtraBenefit>();
            oExtraBenefits.ForEach(x => oTempExtraBenefits.Add(x));

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
                var sheet = excelPackage.Workbook.Worksheets.Add("Benefit On Attendance");
                sheet.Name = "Benefit On Attendance";

                sheet.Column(2).Width = 8;
                sheet.Column(3).Width = 30;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 30;
                sheet.Column(6).Width = 20;
                sheet.Column(7).Width = 20;
                sheet.Column(8).Width = 15;
                sheet.Column(9).Width = 15;
                sheet.Column(10).Width = 15;
                sheet.Column(11).Width = 15;
                nMaxColumn = 11;
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Report Header
                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = "Dayoff Allowance"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Sl"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Employee ID"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Name"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Designation"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Date of join"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Days"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Per Day"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Payable"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "Signature"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
                colIndex = 2;

                #endregion

                #region Table Body
                oExtraBenefits = oExtraBenefits.OrderBy(x => x.AttendanceDateInString).ThenBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
                int nSL = 0;
                while (oExtraBenefits.Count > 0)
                {
                    var oResults = new List<ExtraBenefit>();
                    oResults = oExtraBenefits.Where(x => x.BusinessUnitName == oExtraBenefits[0].BusinessUnitName && x.LocationName == oExtraBenefits[0].LocationName && x.DepartmentName == oExtraBenefits[0].DepartmentName).ToList();

                    var oResultGroup = oResults.GroupBy(x => new { x.BusinessUnitName, x.LocationName, x.DepartmentName, x.EmployeeCode }, (key, grp) => new ExtraBenefit
                    {
                        EmployeeCode = grp.First().EmployeeCode,
                        EmployeeName = grp.First().EmployeeName,
                        BusinessUnitName = key.BusinessUnitName,
                        LocationName = grp.First().LocationName,
                        DepartmentName = grp.First().DepartmentName,
                        DesignationName = grp.First().DesignationName,
                        JoiningDate = grp.First().JoiningDate,
                        Salary = grp.First().Salary,
                        DaysCount = grp.First().DaysCount,
                        PerDayAmount = grp.First().PerDayAmount,
                        PayableAmount = grp.First().PayableAmount

                    }).OrderBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ThenBy(x => x.EmployeeCode).ToList();

                    oExtraBenefits.RemoveAll(x => x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName);

                    nSL++;
                    colIndex = 2;
                    //rowIndex++;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Unit : " + oResultGroup.FirstOrDefault().LocationName + ", Department : " + oResultGroup.FirstOrDefault().DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex++;
                    colIndex = 2;
                    oResults = oResults.OrderBy(x => x.EmployeeCode).ToList();
                    foreach (ExtraBenefit oListItem in oResultGroup)
                    {
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL++; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.EmployeeCode; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.EmployeeName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.DesignationName; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.JoiningDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oListItem.Salary); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        //Dayoff
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oListItem.DaysCount; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oListItem.PerDayAmount); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oListItem.PayableAmount); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        rowIndex++;
                        colIndex = 2;
                    }
                    nSL = 0;

                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oResultGroup.Sum(x => x.Salary)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //Dayoff
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oResultGroup.Sum(x => x.PerDayAmount)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oResultGroup.Sum(x => x.PayableAmount)); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex += 1;

                }


                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Grand Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oTempExtraBenefits.Sum(x => x.Salary)); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                //Dayoff
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oTempExtraBenefits.Sum(x => x.PerDayAmount)); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(oTempExtraBenefits.Sum(x => x.PayableAmount)); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex += 1;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BENEFIT.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }

        }
        #endregion BOAEL

        #region Search Benefits of the Employee

        [HttpPost]
        public JsonResult GetsAllBenefitsofEmployee(BenefitOnAttendanceEmployee oBOAE)
        {

            List<BenefitOnAttendanceEmployee> oBOAEs = new List<BenefitOnAttendanceEmployee>();
            string sSQL = "SELECT * FROM View_BenefitOnAttendanceEmployee Where EmployeeID=" + oBOAE.EmployeeID + " And InactiveDate IS NULL ";
            oBOAEs = BenefitOnAttendanceEmployee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBOAEs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Search BOA Employee Or Not


        [HttpPost]
        public JsonResult GetBOAEmpsByType(BenefitOnAttendance oBenefitOnAttendance)
        {
            int nType = 0, nBOAID = 0, nPreviousRecord = 0;
            string sEmpNameCode = "";

            int.TryParse(oBenefitOnAttendance.Params.Split('~')[0], out nType);
            int.TryParse(oBenefitOnAttendance.Params.Split('~')[1], out nBOAID);
            sEmpNameCode = oBenefitOnAttendance.Params.Split('~')[2];
            int.TryParse(oBenefitOnAttendance.Params.Split('~')[3], out nPreviousRecord);
            double nStartSalaryRange = Convert.ToDouble(oBenefitOnAttendance.Params.Split('~')[4]);
            double nEndSalaryRange = Convert.ToDouble(oBenefitOnAttendance.Params.Split('~')[5]);

            List<BenefitOnAttendanceEmployee> oBOAEs = new List<BenefitOnAttendanceEmployee>();
            string sSQL = "Select * from View_BenefitOnAttendanceEmployee  Where BOAID=" + nBOAID + "";

            if (nType == 1) // All Benifited
                sSQL += " And InactiveDate IS NULL And ISNULL(InactiveBy,0)=0";
            if (nType == 2) // Permanently Benifited
                sSQL += " And InactiveDate IS NULL And ISNULL(InactiveBy,0)=0 And ISNULL(IsTemporaryAssign,0)=0";
            if (nType == 3) // Templorarily Benifited
                sSQL += " And InactiveDate IS NULL And ISNULL(InactiveBy,0)=0 And ISNULL(IsTemporaryAssign,0)=1";
            if (nType == 4) // Templorarily Stopped
                sSQL += " And InactiveDate IS NULL And BOAEmployeeID In (Select BOAEmployeeID from BenefitOnAttendanceEmployeeStopped Where EndDate>= '" + DateTime.Now.ToString("dd MMM yyyy") + "')";


            if (!string.IsNullOrEmpty(sEmpNameCode))
                sSQL += " And (EmployeeName Like '%" + sEmpNameCode.Trim() + "%' OR EmployeeCode Like '%" + sEmpNameCode + "%')";
            if (nStartSalaryRange > 0 && nEndSalaryRange > 0)
            {
                sSQL += " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE GrossAmount BETWEEN " + nStartSalaryRange + " AND " + nEndSalaryRange + ")";
            }
            sSQL += " Order By BOAEmployeeID ASC";

            oBOAEs = BenefitOnAttendanceEmployee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            if (oBOAEs.Count() > 0)
            {
                int nTotalRecord = oBOAEs.Count();
                int nUpto = ((nTotalRecord - nPreviousRecord) > 100) ? 100 : (nTotalRecord - nPreviousRecord);
                oBOAEs = oBOAEs.GetRange(nPreviousRecord, nUpto);
                oBOAEs.FirstOrDefault().Params = nTotalRecord.ToString();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBOAEs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNotBenifitedEmployees(BenefitOnAttendance oBenefitOnAttendance)
        {
            int nType = 0, nBOAID = 0, nPreviousRecord = 0;
            string sEmpNameCode = "";

            int.TryParse(oBenefitOnAttendance.Params.Split('~')[0], out nType);
            int.TryParse(oBenefitOnAttendance.Params.Split('~')[1], out nBOAID);
            sEmpNameCode = oBenefitOnAttendance.Params.Split('~')[2];
            int.TryParse(oBenefitOnAttendance.Params.Split('~')[3], out nPreviousRecord);
            double nStartSalaryRange = Convert.ToDouble(oBenefitOnAttendance.Params.Split('~')[4]);
            double nEndSalaryRange = Convert.ToDouble(oBenefitOnAttendance.Params.Split('~')[5]);


            List<Employee> oEmployees = new List<Employee>();
            string sSQL = "Select * from View_Employee Where IsActive=1 AND (DepartmentID>0 OR DepartmentID IS NOT NULL) And EmployeeID In (Select EmployeeID From EmployeeOfficial Where IsActive=1)"
                           +" AND EmployeeID Not In (Select EmployeeID from BenefitOnAttendanceEmployee Where BOAID =" + nBOAID + " And InactiveDate IS NULL)";
            if (!string.IsNullOrEmpty(sEmpNameCode))
                sSQL += " And (Name Like '%" + sEmpNameCode.Trim() + "%' OR Code Like '%" + sEmpNameCode + "%')";

            if (nStartSalaryRange > 0 && nEndSalaryRange>0)
            {
                sSQL += " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE GrossAmount BETWEEN "+nStartSalaryRange+" AND "+ nEndSalaryRange+")";
            }

            sSQL += "Order By EmployeeID ASC";

            oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            if (oEmployees.Count() > 0)
            {
                int nTotalRecord = oEmployees.Count();
                int nUpto = ((nTotalRecord - nPreviousRecord) > 100) ? 100 : (nTotalRecord - nPreviousRecord);
                oEmployees = oEmployees.GetRange(nPreviousRecord, nUpto);
                oEmployees.FirstOrDefault().Params = nTotalRecord.ToString();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsEmployeeWithBenefitOrNot(BenefitOnAttendance oBenefitOnAttendance)
        {
            int nBOAID = 0;
            string sEmpNameCode = "", sEmpID = "";

            int.TryParse(oBenefitOnAttendance.Params.Split('~')[0], out nBOAID);
            sEmpNameCode = oBenefitOnAttendance.Params.Split('~')[1];
            sEmpID = oBenefitOnAttendance.Params.Split('~')[2];


            List<Employee> oEmployees = new List<Employee>();
            string sSQL = " Select * from View_Employee Where EmployeeID <> 0 ";

            if (nBOAID>0)
                sSQL += " And EmployeeID Not In( Select DISTINCT(EmployeeID) from BenefitOnAttendanceEmployee Where ISNULL(InactiveBy,0)=0 " +
                        " And InactiveDate IS NULL And BOAID=" + nBOAID + ") ";

            if (!string.IsNullOrEmpty(sEmpID))
                sSQL += " And EmployeeID In (" + sEmpID.Trim() + ")";

            if (!string.IsNullOrEmpty(sEmpNameCode))
                sSQL += " And (Name Like '%" + sEmpNameCode.Trim() + "%' OR Code Like '%" + sEmpNameCode.Trim() + "%')";
            
            sSQL += " Order By EmployeeID ASC";

            oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            List<BenefitOnAttendanceEmployee> oBOAEs = new List<BenefitOnAttendanceEmployee>();
            sSQL="Select * from View_BenefitOnAttendanceEmployee Where ISNULL(InactiveBy,0)=0  And InactiveDate IS NULL And BOAID=" + nBOAID + "";
            if (!string.IsNullOrEmpty(sEmpID))
                sSQL += " And EmployeeID In (" + sEmpID.Trim() + ")";

            if (!string.IsNullOrEmpty(sEmpNameCode))
                sSQL += " And (EmployeeName Like '%" + sEmpNameCode.Trim() + "%' OR EmployeeCode Like '%" + sEmpNameCode.Trim() + "%')";

            sSQL += " Order By EmployeeID ASC";

            oBOAEs = BenefitOnAttendanceEmployee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            if (oEmployees.Any() && oEmployees.FirstOrDefault().EmployeeID > 0)
            {
                BenefitOnAttendanceEmployee oBOAE = new BenefitOnAttendanceEmployee();
                oEmployees.ForEach(x =>
                    {
                        oBOAE = new BenefitOnAttendanceEmployee();
                        oBOAE.BOAEmployeeID = 0;
                        oBOAE.BOAID = 0;
                        oBOAE.EmployeeID = x.EmployeeID;
                        oBOAE.InactiveDate = DateTime.MinValue;
                        oBOAE.InactiveBy = 0;
                        oBOAE.IsTemporaryAssign = false;
                        oBOAE.EmployeeName = x.Name;
                        oBOAE.EmployeeCode = x.Code;
                        oBOAE.DepartmentName = x.DepartmentName;
                        oBOAE.DesignationName = x.DesignationName;
                        oBOAEs.Add(oBOAE);
                    });
            }

            oBOAEs = oBOAEs.OrderBy(x => x.EmployeeID).ToList();

         
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBOAEs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        

        #endregion

        #region Gets BOAE History

        [HttpPost]
        public JsonResult GetsBOAStoppedHistory(BenefitOnAttendanceEmployee oBOAE)
        {

            List<BenefitOnAttendanceEmployeeStopped> oBOAESs = new List<BenefitOnAttendanceEmployeeStopped>();
            string sSQL =  "Select * from BenefitOnAttendanceEmployeeStopped Where BOAEmployeeID In (" + oBOAE.BOAEmployeeID + ")";
            oBOAESs = BenefitOnAttendanceEmployeeStopped.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            List<BenefitOnAttendanceEmployeeAssign> oBOAEAs = new List<BenefitOnAttendanceEmployeeAssign>();
            sSQL = "Select * from BenefitOnAttendanceEmployeeAssign Where BOAEmployeeID In (" + oBOAE.BOAEmployeeID + ")";
            oBOAEAs = BenefitOnAttendanceEmployeeAssign.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            BenefitOnAttendanceHistory oBOAH=new BenefitOnAttendanceHistory();
            oBOAH.TempStopHistorys = oBOAESs;
            oBOAH.TempAssignHistorys = oBOAEAs;


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBOAH);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        
        #endregion

        #region Single Stop Benefits
        [HttpPost]
        public JsonResult Save_BOAEStopped(BenefitOnAttendanceEmployeeStopped oBenefitOnAttendanceEmployeeStopped)
        {
            try
            {
                if (oBenefitOnAttendanceEmployeeStopped.BOAESID <= 0)
                {
                    oBenefitOnAttendanceEmployeeStopped = oBenefitOnAttendanceEmployeeStopped.IUD((short)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oBenefitOnAttendanceEmployeeStopped = oBenefitOnAttendanceEmployeeStopped.IUD((short)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }
               
            }
            catch (Exception ex)
            {
                oBenefitOnAttendanceEmployeeStopped = new BenefitOnAttendanceEmployeeStopped();
                oBenefitOnAttendanceEmployeeStopped.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBenefitOnAttendanceEmployeeStopped);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete_BOAEStopped(BenefitOnAttendanceEmployeeStopped oBenefitOnAttendanceEmployeeStopped)
        {
            try
            {
                if (oBenefitOnAttendanceEmployeeStopped.BOAESID <= 0)
                    throw new Exception("Select a valid item.");
                oBenefitOnAttendanceEmployeeStopped = oBenefitOnAttendanceEmployeeStopped.IUD((short)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oBenefitOnAttendanceEmployeeStopped = new BenefitOnAttendanceEmployeeStopped();
                oBenefitOnAttendanceEmployeeStopped.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBenefitOnAttendanceEmployeeStopped.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Single Temp Assign Benefits
        [HttpPost]
        public JsonResult SaveTempAssignBenefit(BenefitOnAttendanceEmployeeAssign oBenefitOnAttendanceEmployeeAssign)
        {
            try
            {
                if (oBenefitOnAttendanceEmployeeAssign.BOAEAID <= 0)
                {
                    oBenefitOnAttendanceEmployeeAssign = oBenefitOnAttendanceEmployeeAssign.IUD((short)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    oBenefitOnAttendanceEmployeeAssign = oBenefitOnAttendanceEmployeeAssign.IUD((short)EnumDBOperation.Update, (int)Session[SessionInfo.currentUserID]);
                }

            }
            catch (Exception ex)
            {
                oBenefitOnAttendanceEmployeeAssign = new BenefitOnAttendanceEmployeeAssign();
                oBenefitOnAttendanceEmployeeAssign.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBenefitOnAttendanceEmployeeAssign);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteTempAssignBenefit(BenefitOnAttendanceEmployeeAssign oBenefitOnAttendanceEmployeeAssign)
        {
            try
            {
                if (oBenefitOnAttendanceEmployeeAssign.BOAEAID <= 0)
                    throw new Exception("Select a valid item.");
                oBenefitOnAttendanceEmployeeAssign = oBenefitOnAttendanceEmployeeAssign.IUD((short)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oBenefitOnAttendanceEmployeeAssign = new BenefitOnAttendanceEmployeeAssign();
                oBenefitOnAttendanceEmployeeAssign.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBenefitOnAttendanceEmployeeAssign.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Multi Assign

        [HttpPost]
        public JsonResult MultiAssign(BenefitOnAttendance oBenefitOnAttendance)
        {
            BenefitOnAttendanceEmployee oBOAE = new BenefitOnAttendanceEmployee();
            List<BenefitOnAttendanceEmployee> oBOAEs = new List<BenefitOnAttendanceEmployee>();
            try
            {
                if (oBenefitOnAttendance.BOAID <= 0)
                    throw new Exception("Select benefit on attendance.");

                if (string.IsNullOrEmpty(oBenefitOnAttendance.Params))
                    throw new Exception("No employee found");

                bool IsTemporary = false;
                DateTime dtStartFrom, dtStartTo = DateTime.Today;


                string sEmpIds = oBenefitOnAttendance.Params.Split('~')[0];
                bool.TryParse(oBenefitOnAttendance.Params.Split('~')[1], out IsTemporary);
                DateTime.TryParse(oBenefitOnAttendance.Params.Split('~')[2], out dtStartFrom);
                DateTime.TryParse(oBenefitOnAttendance.Params.Split('~')[3], out dtStartTo);
                List<BenefitOnAttendanceEmployee> oBOAEmps = new List<BenefitOnAttendanceEmployee>();

                List<string> EmpIDs = new List<string>();
                EmpIDs = sEmpIds.Split(',').Distinct().ToList();

                string sSQL = "";
                if (IsTemporary)
                {
                    sSQL = "Select * from BenefitOnAttendanceEmployeeAssign Where (('" + dtStartFrom.ToString("dd MMM yyyy") + "' Between StartDate And EndDate) OR ('" + dtStartTo.ToString("dd MMM yyyy") + "' Between StartDate And EndDate ))  And BOAEmployeeID In (Select BOAEmployeeID from BenefitOnAttendanceEmployee Where BOAID=" + oBenefitOnAttendance.BOAID + " And  EmployeeID In (" + sEmpIds + ") And InactiveDate IS NULL And IsTemporaryAssign=1)";
                    var oBOAEmpAssigns = BenefitOnAttendanceEmployeeAssign.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                    if (oBOAEmpAssigns.Count() > 0 && oBOAEmpAssigns.FirstOrDefault().BOAEmployeeID > 0)
                    {
                        oBOAE = BenefitOnAttendanceEmployee.Get(oBOAEmpAssigns.FirstOrDefault().BOAEmployeeID, (int)Session[SessionInfo.currentUserID]);
                        throw new Exception(oBOAE.EmployeeName+ " already has the benefits in this date range.");
                    }
                        

                    sSQL = "Select * from View_BenefitOnAttendanceEmployee Where BOAID=" + oBenefitOnAttendance.BOAID + " And  EmployeeID In (" + sEmpIds + ") And InactiveDate IS NULL And IsTemporaryAssign=1" +
                            " And BOAEmployeeID Not In (Select BOAEmployeeID from BenefitOnAttendanceEmployeeAssign Where EndDate>='" + dtStartFrom.ToString("dd MMM yyyy") + "')";
                    oBOAEmps = BenefitOnAttendanceEmployee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                }
                else
                {
                    sSQL = "Select * from View_BenefitOnAttendanceEmployee Where BOAID=" + oBenefitOnAttendance.BOAID + " And  EmployeeID In (" + sEmpIds + ") And InactiveDate IS NULL";
                    oBOAEmps = BenefitOnAttendanceEmployee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                    if (oBOAEmps.Any(x=>!x.IsTemporaryAssign) && oBOAEmps.Where(x=>!x.IsTemporaryAssign).FirstOrDefault().BOAEmployeeID > 0)
                        throw new Exception(oBOAEmps.Where(x => !x.IsTemporaryAssign).FirstOrDefault().EmployeeName + " already has the benefits.");
                }

               EmpIDs.ForEach(x=> {
                   oBOAE=new BenefitOnAttendanceEmployee();
                   oBOAE.BOAEmployeeID = (oBOAEmps.Where(p => p.EmployeeID == Convert.ToInt32(x)).Any()) ? oBOAEmps.Where(p => p.EmployeeID == Convert.ToInt32(x)).FirstOrDefault().BOAEmployeeID : 0;
                   oBOAE.BOAID=oBenefitOnAttendance.BOAID;
                   oBOAE.EmployeeID=Convert.ToInt32(x);
                   oBOAE.IsTemporaryAssign = IsTemporary;
                   oBOAEs.Add(oBOAE);
               });

               oBOAEs = BenefitOnAttendanceEmployee.MultiAssign(oBOAEs, dtStartFrom, dtStartTo, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBOAEs = new List<BenefitOnAttendanceEmployee>();
                oBOAE = new BenefitOnAttendanceEmployee();
                oBOAE.ErrorMessage = ex.Message;
                oBOAEs.Add(oBOAE);
            }
            oBOAE = (oBOAEs.Count()>0) ? oBOAEs.FirstOrDefault() : new BenefitOnAttendanceEmployee();
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBOAE);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #endregion

        #region Multi Stopped

        [HttpPost]
        public JsonResult MultiStopped(BenefitOnAttendanceEmployeeStopped oBOAEStopped)
        {
            BenefitOnAttendanceEmployeeStopped oBOAEmpS = new BenefitOnAttendanceEmployeeStopped();
            List<BenefitOnAttendanceEmployeeStopped> oBOAEmpSs = new List<BenefitOnAttendanceEmployeeStopped>();

            List<BenefitOnAttendanceEmployee> oBOAEs = new List<BenefitOnAttendanceEmployee>();
            BenefitOnAttendanceEmployee oBOAE = new BenefitOnAttendanceEmployee();
            try
            {
                if (oBOAEStopped.BOAID <= 0)
                    throw new Exception("Select benefit on attendance.");

                if (string.IsNullOrEmpty(oBOAEStopped.Params))
                    throw new Exception("No employee found");

                List<int> EmpIDs = new List<int>();
                EmpIDs = oBOAEStopped.Params.Split(',').Select(Int32.Parse).Distinct().ToList();

                string sSQL = "Select * from View_BenefitOnAttendanceEmployee Where BOAID=" + oBOAEStopped.BOAID + " And  EmployeeID In (" + oBOAEStopped.Params.Trim() + ") And InactiveDate IS NULL";

                oBOAEs = BenefitOnAttendanceEmployee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                bool IsAllFound= EmpIDs.Select(p => !oBOAEs.Select(x => x.EmployeeID).ToList().Contains(p)).Any();
                if ((oBOAEs.Select(x => x.EmployeeID).ToList().Count() != EmpIDs.Count()) || !IsAllFound)
                    throw new Exception("Invalid employee found who have not attendance benifit.");


                EmpIDs.ForEach(x =>
                {
                    oBOAEmpS = new BenefitOnAttendanceEmployeeStopped();
                    oBOAEmpS.BOAESID = 0;
                    oBOAEmpS.BOAEmployeeID = oBOAEs.Where(p => p.EmployeeID == Convert.ToInt32(x)).FirstOrDefault().BOAEmployeeID;
                    oBOAEmpS.BOAID = oBOAEStopped.BOAID;
                    oBOAEmpS.EmployeeID = Convert.ToInt32(x);
                    oBOAEmpS.StartDate = oBOAEStopped.StartDate;
                    oBOAEmpS.EndDate = oBOAEStopped.EndDate;
                    oBOAEmpS.InactiveDate = oBOAEStopped.InactiveDate;
                    oBOAEmpS.IsPermanent = oBOAEStopped.IsPermanent;
                    oBOAEmpSs.Add(oBOAEmpS);
                });
                oBOAEmpSs = BenefitOnAttendanceEmployeeStopped.MultiStopped(oBOAEmpSs, (int)Session[SessionInfo.currentUserID]);

                if (oBOAEmpSs.Any() && !string.IsNullOrEmpty(oBOAEmpSs.FirstOrDefault().ErrorMessage))
                {
                    throw new Exception(oBOAEmpSs.FirstOrDefault().ErrorMessage);
                }
                sSQL = "Select * from View_BenefitOnAttendanceEmployee Where BOAEmployeeID In (" + string.Join(",", oBOAEs.Select(x => x.BOAEmployeeID).ToArray()) + ")";
                oBOAEs = BenefitOnAttendanceEmployee.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBOAEs = new List<BenefitOnAttendanceEmployee>();
                oBOAE = new BenefitOnAttendanceEmployee();
                oBOAE.ErrorMessage = ex.Message;
                oBOAEs.Add(oBOAE);
            }
            oBOAE = (oBOAEs.Count() > 0) ? oBOAEs.FirstOrDefault() : new BenefitOnAttendanceEmployee();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBOAE);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        private List<CellRowSpan> RowMerge(List<BenefitOnAttendanceReport> oBOAELs)
        {
            List<BenefitOnAttendanceReport> oTempBOAELs = new List<BenefitOnAttendanceReport>();
            List<CellRowSpan> oSaleRowSpans = new List<CellRowSpan>();
            int[,] mergerCell2D = new int[1, 2];
            int[] rowIndex = new int[15];
            int[] rowSpan = new int[15];

            bool IsBuyerSpan = false;
            int nIteration = 0;
            while (oBOAELs.Count() > 0)
            {
                oTempBOAELs = oBOAELs.Where(x => x.EmployeeID == oBOAELs.FirstOrDefault().EmployeeID).ToList();
                oBOAELs.RemoveAll(x => x.EmployeeID == oTempBOAELs.FirstOrDefault().EmployeeID);

                IsBuyerSpan = true;
                nIteration = 0;
                foreach (BenefitOnAttendanceReport OItem in oTempBOAELs)
                {
                    ++nIteration;
                    if (IsBuyerSpan)
                    {
                        IsBuyerSpan = false;
                        rowIndex[0] = rowIndex[0] + rowSpan[0];
                        rowSpan[0] = oTempBOAELs.Count();
                        oSaleRowSpans.Add(MakeSpan.GenerateRowSpan("EmployeeCode", rowIndex[0], rowSpan[0]));
                        oSaleRowSpans.Add(MakeSpan.GenerateRowSpan("EmployeeName", rowIndex[0], rowSpan[0]));
                    }
                }
            }
            return oSaleRowSpans;
        }

        #region Extra Benefit
        public void PrintExtraBenefit_XL(string sParam, double nts)
        {
            List<ExtraBenefit> oExtraBenefits = new List<ExtraBenefit>();
            string sDateFrom = sParam.Split('~')[0];
            string sDateEnd = sParam.Split('~')[1];
            string BOAIDs = sParam.Split('~')[2];
            string sEmployeeIDs = sParam.Split('~')[3];
            string sLocationID = sParam.Split('~')[4];
            string sDepartmentIDs = sParam.Split('~')[5];
            string sBusinessUnitIDs = sParam.Split('~')[6];
            double nStartSalary = Convert.ToDouble(sParam.Split('~')[7]);
            double nEndSalary = Convert.ToDouble(sParam.Split('~')[8]);

            oExtraBenefits = ExtraBenefit.Gets(Convert.ToDateTime(sDateFrom), Convert.ToDateTime(sDateEnd), BOAIDs, sEmployeeIDs, sLocationID, sDepartmentIDs, sBusinessUnitIDs, nStartSalary, nEndSalary, (int)Session[SessionInfo.currentUserID]);

            List<ExtraBenefit> oTempExtraBenefits = new List<ExtraBenefit>();
            oExtraBenefits.ForEach(x => oTempExtraBenefits.Add(x));

            int nMaxColumn = 0;
            int colIndex = 2;
            int rowIndex = 1;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("EXTRA BENEFITS");
                sheet.Name = "EXTRA BENEFITS";

                sheet.Column(2).Width = 6; //SL
                sheet.Column(3).Width = 20; //EmployeeCode
                sheet.Column(4).Width = 20; //EmployeeName
                sheet.Column(5).Width = 20; //DesignationName
                sheet.Column(6).Width = 20; // W. STATUS
                sheet.Column(7).Width = 20; //JOINING
                sheet.Column(8).Width = 20; //GROSS
                sheet.Column(9).Width = 20; //INTIME
                sheet.Column(10).Width = 20; //OUTTIME
                sheet.Column(11).Width = 20; //DURATION
                sheet.Column(12).Width = 20; //PERCENT
                sheet.Column(13).Width = 20; //PERDAY
                sheet.Column(14).Width = 20; //PAYABLE
                sheet.Column(15).Width = 20; //SIGN

                nMaxColumn = 15;

                sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
                cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                rowIndex = rowIndex + 1;

                bool flag = true;
                bool flagDate = true;
                oExtraBenefits = oExtraBenefits.OrderBy(x => x.AttendanceDateInString).ThenBy(x => x.BusinessUnitName).ThenBy(x => x.LocationName).ThenBy(x => x.DepartmentName).ToList();
                List<ExtraBenefit> oEBs = new List<ExtraBenefit>();
                //if (oExtraBenefits.Count > 0)
                //{
                //    oEBs = oExtraBenefits.GroupBy(x => new { x.AttendanceDateInString }, (key, grp) => new ExtraBenefit
                //    {
                //        AttendanceDateInString = key.AttendanceDateInString,
                //        List = grp

                //    }).OrderBy(x => x.AttendanceDateInString).ToList();

                //    foreach (var oItem in oEBs)
                //    {

                //        if (flag)
                //        {
                //            this.EBReportHeaderXL(ref  sheet, ref  cell, ref  rowIndex, ref  colIndex, nMaxColumn, oItem, sDateFrom, sDateEnd);
                //            flag = false;
                //        }
                //    }
                //}
                string CurrentPrintDate = string.Empty;
               
                
                while (oExtraBenefits.Count > 0)
                {
                    var oResults = oExtraBenefits.Where(x => x.AttendanceDateInString == oExtraBenefits[0].AttendanceDateInString && x.BusinessUnitName == oExtraBenefits[0].BusinessUnitName && x.LocationName == oExtraBenefits[0].LocationName && x.DepartmentName == oExtraBenefits[0].DepartmentName).ToList();
                   // CurrentPrintDate = oExtraBenefits[0].AttendanceDateInString;
                    if (flag)
                    {
                        this.EBReportHeaderXL(ref  sheet, ref  cell, ref  rowIndex, ref  colIndex, nMaxColumn, oExtraBenefits[0], sDateFrom, sDateEnd);
                        flag = false;
                    }
                    if (oExtraBenefits[0].AttendanceDateInString != CurrentPrintDate)
                    {
                        rowIndex = rowIndex + 1;
                        cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Value = oResults[0].AttendanceDateInString; cell.Style.Font.Bold = true; cell.Merge = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        rowIndex = rowIndex + 1;
                        flagDate = false;
                    }
                    this.EBColumnSetup(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nMaxColumn, oExtraBenefits[0]);
                    this.EBBodySetup(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nMaxColumn, oResults, ref CurrentPrintDate);
                    this.TotalEB(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nMaxColumn, oResults,"Total");
                    oExtraBenefits.RemoveAll(x => x.AttendanceDateInString == oExtraBenefits[0].AttendanceDateInString && x.BusinessUnitName == oResults[0].BusinessUnitName && x.LocationName == oResults[0].LocationName && x.DepartmentName == oResults[0].DepartmentName);
                    flagDate = true;
                }

                #region Total
                this.TotalEB(ref  sheet, ref  cell, ref fill, ref border, ref rowIndex, ref colIndex, nMaxColumn, oTempExtraBenefits, "Grand Total");
                //signature

                rowIndex = rowIndex + 8;
                colIndex = 1;

                int Colspan = 0;

                Colspan = (nMaxColumn - nMaxColumn % 4) / 4 - 1;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_________________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_______________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "__________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "_____________"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                rowIndex = rowIndex + 1;
                colIndex = 0;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Prepared By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Checked By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Reviewed By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, colIndex = colIndex + Colspan]; cell.Merge = true; cell.Value = "Approved By"; cell.Style.Font.Bold = true; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=EBList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void EBReportHeaderXL(ref ExcelWorksheet sheet, ref ExcelRange cell, ref int rowIndex, ref int colIndex, int nMaxColumn,ExtraBenefit oExtraBenefit, string dFrom, string dEnd)
        {
            rowIndex = rowIndex + 1;

            sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
            cell = sheet.Cells[rowIndex, 2]; cell.Value = oExtraBenefit.BusinessUnitName; cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rowIndex = rowIndex + 1;

            sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
            cell = sheet.Cells[rowIndex, 2]; cell.Value = oExtraBenefit.BusinessUnitAddress; cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rowIndex = rowIndex + 1;

            sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn].Merge = true;
            cell = sheet.Cells[rowIndex, 2]; cell.Value = oExtraBenefit.BOAName + " for the date " + dFrom + " To " + dEnd; cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            rowIndex = rowIndex + 1;
        }

        public void EBColumnSetup(ref ExcelWorksheet sheet, ref ExcelRange cell, ref ExcelFill fill, ref OfficeOpenXml.Style.Border border, ref int rowIndex, ref int colIndex, int nMaxColumn, ExtraBenefit oExtraBenefit)
        {
            colIndex = 2;

            cell = sheet.Cells[rowIndex, 2, rowIndex, 5]; cell.Merge = true;
            cell.Value = "Unit-" + oExtraBenefit.LocationName; cell.Style.Font.Bold = false;
            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            cell = sheet.Cells[rowIndex, nMaxColumn - 4, rowIndex, nMaxColumn]; cell.Merge = true;
            cell.Value = "Department-" + oExtraBenefit.DepartmentName; cell.Style.Font.Bold = false;
            cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            rowIndex++;

            colIndex = 2;
            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Employee ID"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "Designation"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = true; cell.Value = "W. Status"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "In Time"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Out Time"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Duration"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Entitle"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Per Day"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payable"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Signature"; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


            rowIndex++;

        }

        public void EBBodySetup(ref ExcelWorksheet sheet, ref ExcelRange cell, ref ExcelFill fill, ref OfficeOpenXml.Style.Border border, ref int rowIndex, ref int colIndex, int nMaxColumn, List<ExtraBenefit> oExtraBenefits, ref string CurrentPrintDate)
        {
            int nSL = 0;
            oExtraBenefits = oExtraBenefits.OrderBy(x=>x.EmployeeCode).ToList();
            foreach (ExtraBenefit EB in oExtraBenefits)
            {
                nSL++;
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeCode; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.EmployeeName; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.DesignationName; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.WorkingStatusInST; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.JoiningDateInString; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(EB.Salary,0); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.InTimeInString; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = EB.OutTimeInString; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                TimeSpan duration = EB.InTime - EB.OutTime;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = duration.ToString(@"hh\:mm"); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(EB.Percent, 0); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(EB.PerDayAmount, 0); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Math.Round(EB.PayableAmount,0); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ""; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex++;
            }
            if (oExtraBenefits.Any())
                CurrentPrintDate = oExtraBenefits.First().AttendanceDateInString;
        }

        public void TotalEB(ref ExcelWorksheet sheet, ref ExcelRange cell, ref ExcelFill fill, ref OfficeOpenXml.Style.Border border, ref int rowIndex, ref int colIndex, int nMaxColumn, List<ExtraBenefit> oExtraBenefits, string sHeader)
        {
            cell = sheet.Cells[rowIndex, 2, rowIndex, 7]; cell.Merge = true;
            cell.Value = sHeader; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            double nGrossAmount = oExtraBenefits.Sum(x => x.Salary);
            cell = sheet.Cells[rowIndex, 8]; cell.Value = Math.Round(nGrossAmount, 0); cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 9, rowIndex, 12]; cell.Merge = true;
            cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            double nPerDayAmount = oExtraBenefits.Sum(x => x.PerDayAmount);
            cell = sheet.Cells[rowIndex, 13]; cell.Value = Math.Round(nPerDayAmount, 0); cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            double nPayableAmount = oExtraBenefits.Sum(x => x.PayableAmount);
            cell = sheet.Cells[rowIndex, 14]; cell.Value = Math.Round(nPayableAmount, 0); cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

            rowIndex++;
        }

        #endregion Extra Benefit

        [HttpPost]
        public JsonResult SearchByNameOrCode(BenefitOnAttendanceEmployee oEmp)
        {
            _oBenefitOnAttendanceEmployees = new List<BenefitOnAttendanceEmployee>();
            string Ssql = "SELECT * FROM View_BenefitOnAttendanceEmployee WHERE EmployeeName LIKE '%" + oEmp.EmployeeName + "%'" + " OR EmployeeCode LIKE '%" + oEmp.EmployeeName + "%'";
            try
            {
                _oBenefitOnAttendanceEmployees = BenefitOnAttendanceEmployee.Gets(Ssql, (int)Session[SessionInfo.currentUserID]);
                if (_oBenefitOnAttendanceEmployees.Count <= 0)
                {
                    throw new Exception("record not found");
                }
            }
            catch (Exception ex)
            {
                _oBenefitOnAttendanceEmployee = new BenefitOnAttendanceEmployee();
                _oBenefitOnAttendanceEmployees.Add(_oBenefitOnAttendanceEmployee);
                _oBenefitOnAttendanceEmployees[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBenefitOnAttendanceEmployees);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Excel
        public void GetBOAEmpsByTypeExcel(string sParam)
        {
            int nType = 0, nBOAID = 0, nPreviousRecord = 0;
            string sEmpNameCode = "";

            int.TryParse(sParam.Split('~')[0], out nType);
            int.TryParse(sParam.Split('~')[1], out nBOAID);
            sEmpNameCode = sParam.Split('~')[2];
            double nStartSalaryRange = Convert.ToDouble(sParam.Split('~')[3]);
            double nEndSalaryRange = Convert.ToDouble(sParam.Split('~')[4]);

            List<BenefitOnAttendanceEmployee> oBOAEs = new List<BenefitOnAttendanceEmployee>();
            string sSQL = "Select * from View_BenefitOnAttendanceEmployee  Where BOAID=" + nBOAID + "";

            if (nType == 1) // All Benifited
                sSQL += " And InactiveDate IS NULL And ISNULL(InactiveBy,0)=0";
            if (nType == 2) // Permanently Benifited
                sSQL += " And InactiveDate IS NULL And ISNULL(InactiveBy,0)=0 And ISNULL(IsTemporaryAssign,0)=0";
            if (nType == 3) // Templorarily Benifited
                sSQL += " And InactiveDate IS NULL And ISNULL(InactiveBy,0)=0 And ISNULL(IsTemporaryAssign,0)=1";
            if (nType == 4) // Templorarily Stopped
                sSQL += " And InactiveDate IS NULL And BOAEmployeeID In (Select BOAEmployeeID from BenefitOnAttendanceEmployeeStopped Where EndDate>= '" + DateTime.Now.ToString("dd MMM yyyy") + "')";


            if (!string.IsNullOrEmpty(sEmpNameCode))
                sSQL += " And (EmployeeName Like '%" + sEmpNameCode.Trim() + "%' OR EmployeeCode Like '%" + sEmpNameCode + "%')";
            if (nStartSalaryRange > 0 && nEndSalaryRange > 0)
            {
                sSQL += " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE GrossAmount BETWEEN " + nStartSalaryRange + " AND " + nEndSalaryRange + ")";
            }
            sSQL += " Order By BOAEmployeeID ASC";

            oBOAEs = BenefitOnAttendanceEmployee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            int nDays = 0;
            int nColumns = 0;
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
                var sheet = excelPackage.Workbook.Worksheets.Add("BENEFITED EMPLOYEE");
                sheet.Name = "BENEFITED EMPLOYEE";

                
                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 30; //NAME
                sheet.Column(4).Width = 20; //CODE
                sheet.Column(5).Width = 28; //DESIGNATION
                sheet.Column(6).Width = 28; //DEPARTMENT
                sheet.Column(7).Width = 28;//Benefit Type
                sheet.Column(8).Width = 28;//Inactive Date

                nMaxColumn = nColumns;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, 2]; cell.Value =  oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "BENEFITED EMPLOYEE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DEPARTMENT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "BENEFIT TYPE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "INACTIVE DATE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                int nCount = 0;
                foreach ( BenefitOnAttendanceEmployee oItem in oBOAEs)
                {
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = (++nCount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.IsTemporaryAssignStr; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.InactiveDateSt; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                }

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=BenefitedEmployee.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        public void GetNotBenifitedEmployeesExcel(string sParam)
        {
            int nType = 0, nBOAID = 0, nPreviousRecord = 0;
            string sEmpNameCode = "";

            int.TryParse(sParam.Split('~')[0], out nType);
            int.TryParse(sParam.Split('~')[1], out nBOAID);
            sEmpNameCode = sParam.Split('~')[2];
            double nStartSalaryRange = Convert.ToDouble(sParam.Split('~')[3]);
            double nEndSalaryRange = Convert.ToDouble(sParam.Split('~')[4]);

            List<Employee> oEmployees = new List<Employee>();
            string sSQL = "Select * from View_Employee Where IsActive=1 AND (DepartmentID>0 OR DepartmentID IS NOT NULL) And EmployeeID In (Select EmployeeID From EmployeeOfficial Where IsActive=1)"
                           + " AND EmployeeID Not In (Select EmployeeID from BenefitOnAttendanceEmployee Where BOAID =" + nBOAID + " And InactiveDate IS NULL)";
            if (!string.IsNullOrEmpty(sEmpNameCode))
                sSQL += " And (Name Like '%" + sEmpNameCode.Trim() + "%' OR Code Like '%" + sEmpNameCode + "%')";

            if (nStartSalaryRange > 0 && nEndSalaryRange > 0)
            {
                sSQL += " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE GrossAmount BETWEEN " + nStartSalaryRange + " AND " + nEndSalaryRange + ")";
            } 
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND DRPID "
                            + "IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + ")";
            }

            sSQL += "Order By EmployeeID ASC";

            oEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);


            int nDays = 0;
            int nColumns = 0;
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
                var sheet = excelPackage.Workbook.Worksheets.Add("NOT YET BENEFITED EMPLOYEE");
                sheet.Name = "NOT YET BENEFITED EMPLOYEE";


                sheet.Column(2).Width = 8; //SL
                sheet.Column(3).Width = 30; //NAME
                sheet.Column(4).Width = 20; //CODE
                sheet.Column(5).Width = 28; //DESIGNATION
                sheet.Column(6).Width = 28; //DEPARTMENT

                nMaxColumn = nColumns;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                #region Report Header
                cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 1;

                cell = sheet.Cells[rowIndex, 2]; cell.Value = "BENEFITED EMPLOYEE"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rowIndex = rowIndex + 2;
                #endregion

                #region Table Header 02
                colIndex = 2;
                cell = sheet.Cells[rowIndex, colIndex++];
                cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NAME"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "CODE"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DESIGNATION"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "DEPARTMENT"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                int nCount = 0;
                foreach (Employee oItem in oEmployees)
                {
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex++];
                    cell.Value = (++nCount); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Name; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.Code; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                }

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=NotYetBenefitedEmployee.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
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
