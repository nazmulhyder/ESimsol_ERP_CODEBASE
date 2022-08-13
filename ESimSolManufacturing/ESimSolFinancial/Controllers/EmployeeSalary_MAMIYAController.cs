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
using OfficeOpenXml.Drawing;


namespace ESimSolFinancial.Controllers
{
    public class EmployeeSalary_MAMIYAController : Controller
    {
        
        #region Declaration
        EmployeeSalary _oEmployeeSalary;
        private List<EmployeeSalary> _oEmployeeSalarys;
        //List<AttendanceDaily> _oAttendanceDailys;
        List<EmployeeSalaryDetailDisciplinaryAction> _oEmployeeSalaryDetailDisciplinaryActions;
        #endregion

        #region Views
        public ActionResult View_EmployeeSalarys_MAMIYA(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployeeSalarys = new List<EmployeeSalary>();
            
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            
            return View(_oEmployeeSalarys);
        }

        public ActionResult View_PrintMultiplePaySlip(string EmployeeSalaryIDs, string sDate, double ts)
        {
            _oEmployeeSalary = new EmployeeSalary();
            string sSql = "SELECT * FROM VIEW_EmployeeSalary WHERE  EmployeeSalaryID IN(" + EmployeeSalaryIDs + ") Order by EmployeeCode";
            string sSqlDetail = "SELECT *FROM VIEW_EmployeeSalaryDetail WHERE EmployeeSalaryID IN(" + EmployeeSalaryIDs + ")";
            string sSqlDAction = "SELECT * FROM EmployeeSalaryDetailDisciplinaryAction WHERE EmployeeSalaryID IN(" + EmployeeSalaryIDs + ")";
            _oEmployeeSalary.EmployeeSalarys = EmployeeSalary.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSqlDetail, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = EmployeeSalaryDetailDisciplinaryAction.Gets(sSqlDAction, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeSalary.ErrorMessage = sDate;
            return PartialView(_oEmployeeSalary);
        }

        public ActionResult PrintPaySlip_MAMIYA_Bangla(string sDate, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs,int nPayType, double ts)
        {
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);

            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_PaySlip(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            oEmployeeSalary.ErrorMessage = sDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalary.Company = oCompanys.First();
            oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);
            return PartialView(oEmployeeSalary);
        }

        #endregion

        #region Salary Search

        [HttpPost]
        public JsonResult SearchSalary(string sIDs, string dtDateFrom, string dtDateTo, double ts)
        {
            try
            {
                string sSql = "";
                sSql = "SELECT * FROM View_EmployeeSalary WHERE EmployeeID IN(" + sIDs + ") AND StartDate='" + dtDateFrom + "' AND EndDate='" + dtDateTo + "' ORDER BY EmployeeCode";
                _oEmployeeSalarys = new List<EmployeeSalary>();
                _oEmployeeSalarys = EmployeeSalary.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployeeSalarys.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployeeSalarys = new List<EmployeeSalary>();
                _oEmployeeSalary = new EmployeeSalary();
                _oEmployeeSalary.ErrorMessage = ex.Message;
                _oEmployeeSalarys.Add(_oEmployeeSalary);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalarys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchSalaryByDepartmentAndDateRange(string sEmployeeIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, string dtDateFrom, string dtDateTo, int nLoadRecords, int nRowLength, int nPayType, double ts)
        {
            try
            {
                string sSql = "";
                sSql = "SELECT * FROM (SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeSalaryID) Row,* FROM View_EmployeeSalary WHERE StartDate='" + dtDateFrom + "' AND EndDate='" + dtDateTo + "'";
                if (sEmployeeIDs != "")
                {
                    sSql = sSql + " AND EmployeeID IN(" + sEmployeeIDs + ")";
                }
                if (nLocationID>0)
                {
                    sSql = sSql + " AND LocationID =" + nLocationID;
                }
                if (sDepartmentIds != "")
                {
                    sSql = sSql + " AND DepartmentID IN(" + sDepartmentIds + ")";
                }
                if (sDesignationIDs != "")
                {
                    sSql = sSql + " AND DesignationID IN(" + sDesignationIDs + ")";
                }
                if (sSalarySchemeIDs != "")
                {
                    sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN(" + sSalarySchemeIDs + "))";
                }
                if(nPayType>0)
                {
                    if (nPayType == 1)
                    {
                        sSql = sSql + " AND IsAllowBankAccount=1";
                    }
                    if (nPayType == 2)
                    {
                        sSql = sSql + " AND IsAllowBankAccount=0";
                    }
                }
                sSql = sSql + ") aa WHERE Row >" + nRowLength + ") aaa ORDER BY EmployeeCode";
                //sSql = "SELECT * FROM View_EmployeeSalary WHERE DepartmentID =" + nID + " AND StartDate='" + dtDateFrom + "' AND EndDate='" + dtDateTo + "'";
                _oEmployeeSalarys = new List<EmployeeSalary>();
                _oEmployeeSalarys = EmployeeSalary.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oEmployeeSalarys.Count == 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oEmployeeSalarys = new List<EmployeeSalary>();
                _oEmployeeSalary = new EmployeeSalary();
                _oEmployeeSalary.ErrorMessage = ex.Message;
                _oEmployeeSalarys.Add(_oEmployeeSalary);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oEmployeeSalarys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion Salary Search

        #region Report
        //public ActionResult PrintSalarySheet(string sEmpIDs, string sDate, int nLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, double ts)
        //{
        //    EmployeeSalary oEmployeeSalary = new EmployeeSalary();
        //    string sStartDate = sDate.Split(',')[0];
        //    string sEndDate = sDate.Split(',')[1];
        //    string sSql = "";
        //    string sSql2 = "";
        //    string sSql3 = "";
        //    string sSql4 = "";

        //    if (sEmpIDs != "")
        //    {
        //        sSql2 = "SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN (" + sEmpIDs + ") AND StartDate='" + sStartDate + "' AND EndDate='" + sEndDate + "') ORDER BY SalaryHeadID";
        //        sSql3 = "SELECT * FROM View_EmployeeSalary WHERE  EmployeeID IN (" + sEmpIDs + ") AND StartDate='" + sStartDate + "' AND EndDate='" + sEndDate + "' ORDER BY EmployeeCode";
        //        sSql4 = "SELECT * FROM EmployeeSalaryDetailDisciplinaryAction WHERE EmployeeSalaryID IN(SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN(" + sEmpIDs + "))";
        //        oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql2, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //        oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = EmployeeSalaryDetailDisciplinaryAction.Gets(sSql4, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //        oEmployeeSalary.EmployeeSalarys = EmployeeSalary.Gets(sSql3, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    }
        //    else
        //    {
        //        sSql3 = "SELECT * FROM View_EmployeeSalary WHERE StartDate='" + sStartDate + "' AND EndDate='" + sEndDate + "' ";
        //        if (nLocationID > 0)
        //        {
        //            sSql = sSql + " AND LocationID=" + nLocationID;
        //        }
        //        if (sDepartmentIDs != "")
        //        {
        //            sSql3 += " AND DepartmentID IN(" + sDepartmentIDs + ")";
        //        }
        //        if (sDesignationIDs != "")
        //        {
        //            sSql3 += " AND DesignationID IN(" + sDesignationIDs + ")";
        //        }
        //        if (sSalarySchemeIDs != "")
        //        {
        //            sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN(" + sSalarySchemeIDs + "))";
        //        }
        //        sSql3 += " ORDER BY EmployeeCode";

        //        oEmployeeSalary.EmployeeSalarys = EmployeeSalary.Gets(sSql3, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //        //List<int> EmpIDs = new List<int>();
        //        //EmpIDs = oEmployeeSalary.EmployeeSalarys.Select(x => x.EmployeeID).ToList();

        //        string EmpIDs = string.Join(",", oEmployeeSalary.EmployeeSalarys.Select(x => x.EmployeeID));
        //        sSql2 = "SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN (" + EmpIDs + ") AND StartDate='" + sStartDate + "' AND EndDate='" + sEndDate + "') ORDER BY SalaryHeadID";
        //        sSql4 = "SELECT * FROM EmployeeSalaryDetailDisciplinaryAction WHERE EmployeeSalaryID IN(SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN(" + EmpIDs + "))";
        //        oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql2, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //        oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = EmployeeSalaryDetailDisciplinaryAction.Gets(sSql4, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    }
        //    string sSql_SalaryHead = "SELECT * FROM SalaryHead";
        //    oEmployeeSalary.SalaryHeads = SalaryHead.Gets(sSql_SalaryHead, ((User)(Session[SessionInfo.CurrentUser])).UserID);

        //    oEmployeeSalary.ErrorMessage = sDate;
        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oEmployeeSalary.Company = oCompanys.First();
        //    oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);

        //    rptSalarySheet_MAMIYA oReport = new rptSalarySheet_MAMIYA();
        //    byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
        //    return File(abytes, "application/pdf");
        //}

        public ActionResult PrintSalarySheet_MAMIYA(string sDate, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, int nPayType, double ts)
        {
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);
            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_MAMIYA(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType,((User)(Session[SessionInfo.CurrentUser])).UserID);
               
            oEmployeeSalary.ErrorMessage = sDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalary.Company = oCompanys.First();
            oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);

            rptSalarySheet_MAMIYAFormat oReport = new rptSalarySheet_MAMIYAFormat();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintSalarySheet_OT_MAMIYA(string sDate, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, double ts)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);
            string sSql = "";
            string sSql2 = "";
            string sSql3 = "";
            string sSql4 = "";

            if (sEmpIDs != "")
            {
                sSql = "SELECT * FROM View_Employee_WithImage WHERE EmployeeID IN ( " + sEmpIDs + ")";
                //string sSql1 = "SELECT * FROM View_AttendanceDaily  WHERE EmployeeID IN (" + sEmpIDs + ") AND AttendanceDate  BETWEEN '" + sStartDate + "' AND '" + sEndDate + "'";
                sSql2 = "SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN (" + sEmpIDs + ") AND StartDate='" + dtDateFrom.ToString("dd MMM yyyy") + "' AND EndDate='" + dtDateTo.ToString("dd MMM yyyy") + "') ORDER BY SalaryHeadID";
                sSql3 = "SELECT * FROM View_EmployeeSalary WHERE  EmployeeID IN (" + sEmpIDs + ") AND StartDate='" + dtDateFrom.ToString("dd MMM yyyy") + "' AND EndDate='" + dtDateTo.ToString("dd MMM yyyy") + "' ORDER BY EmployeeCode";
                sSql4 = "SELECT * FROM EmployeeSalaryDetailDisciplinaryAction WHERE EmployeeSalaryID IN(SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN(" + sEmpIDs + "))";
                oEmployeeSalary.Employees = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                //oEmployeeSalary.AttendanceDailys = AttendanceDaily.Gets(sSql1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql2, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = EmployeeSalaryDetailDisciplinaryAction.Gets(sSql4, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalary.EmployeeSalarys = EmployeeSalary.Gets(sSql3, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            else
            {
                sSql3 = "SELECT * FROM View_EmployeeSalary WHERE StartDate='" + dtDateFrom.ToString("dd MMM yyyy") + "' AND EndDate='" + dtDateTo.ToString("dd MMM yyyy") + "' ";
                if (nLocationID > 0)
                {
                    sSql = sSql + " AND LocationID=" + nLocationID;
                }
                if (sDepartmentIds != "")
                {
                    sSql3 += " AND DepartmentID IN(" + sDepartmentIds + ")";
                }
                if (sDesignationIDs != "")
                {
                    sSql3 += " AND DesignationID IN(" + sDesignationIDs + ")";
                }
                if (sSalarySchemeIDs != "")
                {
                    sSql = sSql + " AND EmployeeID IN(SELECT EmployeeID FROM EmployeeSalaryStructure WHERE SalarySchemeID IN(" + sSalarySchemeIDs + "))";
                }
                sSql3 += " ORDER BY EmployeeCode";

                oEmployeeSalary.EmployeeSalarys = EmployeeSalary.Gets(sSql3, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                //List<int> EmpIDs = new List<int>();
                //EmpIDs = oEmployeeSalary.EmployeeSalarys.Select(x => x.EmployeeID).ToList();

                string EmpIDs = string.Join(",", oEmployeeSalary.EmployeeSalarys.Select(x => x.EmployeeID));
                sSql = "SELECT * FROM View_Employee_WithImage WHERE EmployeeID IN ( " + EmpIDs + ")";
                sSql2 = "SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN (" + EmpIDs + ") AND StartDate='" + dtDateFrom.ToString("dd MMM yyyy") + "' AND EndDate='" + dtDateTo.ToString("dd MMM yyyy") + "') ORDER BY SalaryHeadID";
                sSql4 = "SELECT * FROM EmployeeSalaryDetailDisciplinaryAction WHERE EmployeeSalaryID IN(SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN(" + EmpIDs + "))";
                oEmployeeSalary.Employees = Employee.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                //oEmployeeSalary.AttendanceDailys = AttendanceDaily.Gets(sSql1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql2, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = EmployeeSalaryDetailDisciplinaryAction.Gets(sSql4, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }

            string sSql_SalaryHead = "SELECT * FROM SalaryHead";
            oEmployeeSalary.SalaryHeads = SalaryHead.Gets(sSql_SalaryHead, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            oEmployeeSalary.ErrorMessage = sDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalary.Company = oCompanys.First();
            oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);

            bool bViewESS = false;
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            if (oARMs.Count > 0) { bViewESS = true; }

            rptSalarySheet_OT_MAMIYA oReport = new rptSalarySheet_OT_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary, bViewESS);
            return File(abytes, "application/pdf");
        }


        //public ActionResult PrintMultiplePaySlip(string EmployeeSalaryIDs, string sDate, double ts)
        //{
        //    EmployeeSalary oEmployeeSalary = new EmployeeSalary();
        //    string sSql = "SELECT * FROM VIEW_EmployeeSalary WHERE  EmployeeSalaryID IN(" + EmployeeSalaryIDs + ")";
        //    string sSql_ESDDA = "SELECT * FROM EmployeeSalaryDetailDisciplinaryAction WHERE EmployeeSalaryID IN(" + EmployeeSalaryIDs + ")";

        //    oEmployeeSalary.EmployeeSalarys = EmployeeSalary.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.GetsForPaySlip(EmployeeSalaryIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = EmployeeSalaryDetailDisciplinaryAction.Gets(sSql_ESDDA, ((User)(Session[SessionInfo.CurrentUser])).UserID);

        //    oEmployeeSalary.ErrorMessage = sDate;
        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oEmployeeSalary.Company = oCompanys.First();
        //    oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);

        //    rptPaySlip_DetailFormat oReport = new rptPaySlip_DetailFormat();
        //    byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
        //    return File(abytes, "application/pdf");
        //}

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
       
        public ActionResult PrintMultiplePaySlip_MAMIYA(string sEmpIDs, string sDate, int nLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID,int nPayType, double ts)
        {
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);

            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_PaySlip(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDesignationIDs, sDesignationIDs, sSalarySchemeIDs, nPayType,((User)(Session[SessionInfo.CurrentUser])).UserID);

            oEmployeeSalary.ErrorMessage = sDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalary.Company = oCompanys.First();
            oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);

            rptPaySlip_MAMIYA oReport = new rptPaySlip_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintFinalSettlementOfResig_MAMIYA(int nEmpID, double ts)
        {
            DateTime dtDateFrom= DateTime.Now;
            DateTime dtDateTo = DateTime.Now; ;

            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_PaySlip(dtDateFrom, dtDateTo, nEmpID.ToString(), 0, "", "", "",0, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            //oEmployeeSalary.ErrorMessage = sDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalary.Company = oCompanys.First();
            oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);

            rptFinalSettlementOfResig_MAMIYA oReport = new rptFinalSettlementOfResig_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintConsolidatedPFDeduction(string sDate, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID,int nPayType, double ts)
        {
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);
            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_SalarySummery_MAMIYA(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs,nPayType, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            oEmployeeSalary.ErrorMessage = sDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalary.Company = oCompanys.First();
            oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);

            rptConsolidatedLoanANDPFDeduction oReport = new rptConsolidatedLoanANDPFDeduction();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintMiscDeductionReport(string sDate, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, int nPayType, double ts)
        {
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);
            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_SalarySummery_MAMIYA(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType,((User)(Session[SessionInfo.CurrentUser])).UserID);

            oEmployeeSalary.ErrorMessage = sDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalary.Company = oCompanys.First();
            oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);

            rptMiscDeductionReport oReport = new rptMiscDeductionReport();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
            return File(abytes, "application/pdf");
        }

        #endregion Report

        #region Salary Summery
        public ActionResult PrintSalarySummery(string sDate, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs,int nMonthID,int nPayType, double ts)
        {
            EmployeeSalary_MAMIYA oSalarySummery = new EmployeeSalary_MAMIYA();
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);
            oSalarySummery.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_SalarySummery_MAMIYA(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oSalarySummery.ErrorMessage = sDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oSalarySummery.Company = oCompanys.First();
            oSalarySummery.Company.CompanyLogo = GetImage(oSalarySummery.Company.OrganizationLogo);
            //oSalarySummery.Company.CompanyLogo = GetCompanyLogo(oSalarySummery.Company);

            rptSalarySummery_MAMIYA oReport = new rptSalarySummery_MAMIYA();
            byte[] abytes = oReport.PrepareReport(oSalarySummery);
            return File(abytes, "application/pdf");
        }
        #endregion Salary Summery

        #region Salary Summery
        public ActionResult PrintSalarySummery_NatureWise(string sDate, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, bool bBankPay,int nMonthID, double ts)
        {
            SalarySummary_MAMIYA_NatureWise oSalarySummary_MAMIYA_NatureWise = new SalarySummary_MAMIYA_NatureWise();
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);
            oSalarySummary_MAMIYA_NatureWise.SalarySummary_MAMIYA_NatureWises = SalarySummary_MAMIYA_NatureWise.Gets(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, bBankPay,((User)(Session[SessionInfo.CurrentUser])).UserID);
            oSalarySummary_MAMIYA_NatureWise.ErrorMessage = sDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oSalarySummary_MAMIYA_NatureWise.Company = oCompanys.First();
            oSalarySummary_MAMIYA_NatureWise.Company.CompanyLogo = GetImage(oSalarySummary_MAMIYA_NatureWise.Company.OrganizationLogo);

            rptSalarySummary_MAMIYA_NatureWise oReport = new rptSalarySummary_MAMIYA_NatureWise();
            byte[] abytes = oReport.PrepareReport(oSalarySummary_MAMIYA_NatureWise);
            return File(abytes, "application/pdf");
        }
        #endregion Salary Summery

        #region XL
        public ActionResult PrintSalarySheet_MAMIYA_XL(string sDate, string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, int nPayType, double ts)//EmployeeSalary_MAMIYA/View_EmployeeSalarys_MAMIYA
        {
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);
            List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();
            oEmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_MAMIYA(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType,((User)(Session[SessionInfo.CurrentUser])).UserID);
               
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<SalarySheet_MAMIAYA_XL>));

            SalarySheet_MAMIAYA_XL oSalarySheet_MAMIAYA_XL = new SalarySheet_MAMIAYA_XL();
            List<SalarySheet_MAMIAYA_XL> oSalarySheet_MAMIAYA_XLs = new List<SalarySheet_MAMIAYA_XL>();

            int nCount = 0;
            foreach (EmployeeSalary_MAMIYA oItem in oEmployeeSalary_MAMIYAs)
            {
                nCount++;
                oSalarySheet_MAMIAYA_XL = new SalarySheet_MAMIAYA_XL();
                oSalarySheet_MAMIAYA_XL.SL = nCount.ToString();
                oSalarySheet_MAMIAYA_XL.Code = oItem.EmployeeCode;
                oSalarySheet_MAMIAYA_XL.Name = oItem.EmployeeName;
                oSalarySheet_MAMIAYA_XL.Department = oItem.DepartmentName;
                oSalarySheet_MAMIAYA_XL.Designation = oItem.DesignationName;
                oSalarySheet_MAMIAYA_XL.DOJ = oItem.DateOfJoinInString;
                oSalarySheet_MAMIAYA_XL.DOC = oItem.DateOfConfirmationInString;
                oSalarySheet_MAMIAYA_XL.Basic = Global.MillionFormat(oItem.Basic);
                oSalarySheet_MAMIAYA_XL.Hrent = Global.MillionFormat(oItem.HRent);
                oSalarySheet_MAMIAYA_XL.Medical = Global.MillionFormat(oItem.Med);
                oSalarySheet_MAMIAYA_XL.Conveyance = Global.MillionFormat(oItem.Conveyance);
                oSalarySheet_MAMIAYA_XL.Gross = Global.MillionFormat(oItem.GrossSalary);
                oSalarySheet_MAMIAYA_XL.AbsentHR_Scick = Global.MillionFormat(oItem.AbsentHr_Sick);
                oSalarySheet_MAMIAYA_XL.AbsentHR_LWP = Global.MillionFormat(oItem.AbsentHr_WOPay);

                oSalarySheet_MAMIAYA_XL.TotalAbsentAmount = Global.MillionFormat(oItem.TotalAbsentAmount);
                oSalarySheet_MAMIAYA_XL.EarnPay = Global.MillionFormat(oItem.EarnedPay);
                oSalarySheet_MAMIAYA_XL.ShiftAmount = Global.MillionFormat(oItem.ShiftAmount);

                oSalarySheet_MAMIAYA_XL.OT_NHR = Global.MillionFormat(oItem.OT_NHR_HR);
                oSalarySheet_MAMIAYA_XL.OT_HHR = Global.MillionFormat(oItem.OT_HHR_HR);
                oSalarySheet_MAMIAYA_XL.OT_FHR = Global.MillionFormat(oItem.FHOT_HR);
                oSalarySheet_MAMIAYA_XL.OTAmount = Global.MillionFormat(oItem.OTAmount);

                oSalarySheet_MAMIAYA_XL.AttBonus = Global.MillionFormat(oItem.AttendanceBonus);
                oSalarySheet_MAMIAYA_XL.ADJCR= Global.MillionFormat(oItem.OTAmount);
                oSalarySheet_MAMIAYA_XL.ADJCR = Global.MillionFormat(oItem.ADJCR);
                oSalarySheet_MAMIAYA_XL.OtherAll = Global.MillionFormat(oItem.OtherAll);
                oSalarySheet_MAMIAYA_XL.FB = Global.MillionFormat(oItem.FB);
                oSalarySheet_MAMIAYA_XL.MobileBill =Global.MillionFormat(oItem.MobileBill);
                oSalarySheet_MAMIAYA_XL.HolidayAll = Global.MillionFormat(oItem.HolidayAll);

                oSalarySheet_MAMIAYA_XL.GrossPay = Global.MillionFormat(oItem.GrossPay);
                oSalarySheet_MAMIAYA_XL.PF = Global.MillionFormat(oItem.PF);
                oSalarySheet_MAMIAYA_XL.TRANS = Global.MillionFormat(oItem.TRNS);

                oSalarySheet_MAMIAYA_XL.DORM = Global.MillionFormat(oItem.DORM);
                oSalarySheet_MAMIAYA_XL.ADV = Global.MillionFormat(oItem.ADV);
                oSalarySheet_MAMIAYA_XL.ADJDR = Global.MillionFormat(oItem.ADJDR);

                oSalarySheet_MAMIAYA_XL.InstallmentAmt = Global.MillionFormat(oItem.InstallmentAmt);
                oSalarySheet_MAMIAYA_XL.LoanInterest = Global.MillionFormat(oItem.InterestAmt);

                oSalarySheet_MAMIAYA_XL.TotalPLOAN = Global.MillionFormat(oItem.PLoan);
                oSalarySheet_MAMIAYA_XL.TAX = Global.MillionFormat(oItem.IncomeTax);
                oSalarySheet_MAMIAYA_XL.DedTotal = Global.MillionFormat(oItem.DeductionTotal);
                oSalarySheet_MAMIAYA_XL.Stamp = Global.MillionFormat(oItem.RS);
                oSalarySheet_MAMIAYA_XL.NetPay = Global.MillionFormat(oItem.NetPay);

                oSalarySheet_MAMIAYA_XL.AccNo = oItem.BankAccountNo;
                oSalarySheet_MAMIAYA_XL.Bank= oItem.BankName;

                oSalarySheet_MAMIAYA_XLs.Add(oSalarySheet_MAMIAYA_XL);
            }

            serializer.Serialize(stream, oSalarySheet_MAMIAYA_XLs);
            stream.Position = 0;
            if(nPayType==1)
            {
                return File(stream, "application/vnd.ms-excel", "SalarySheet_BANK.xls");
            }
            else { return File(stream, "application/vnd.ms-excel", "SalarySheet_CASH.xls"); }
            

        }
        #endregion XL

        #region PaySlip XL
        public void PrintPaySlip_MAMIYA_Bangla_XL(string sEmpIDs, string sDate, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, int nPayType, double ts)
        {
            //EmployeeSalary oEmployeeSalary = this.GetEmployeePaySlip(sEmpIDs, sDate, nLocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, nMonthID, bNewJoin, nYear);

            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);

            EmployeeSalary_MAMIYA oEmployeeSalary = new EmployeeSalary_MAMIYA();
            oEmployeeSalary.EmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_PaySlip(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            this.PrintPaySlipXL(oEmployeeSalary,dtDateFrom, dtDateTo);
        }
        private void PrintPaySlipXL(EmployeeSalary_MAMIYA oEmployeeSalary , DateTime dtDateFrom , DateTime dtDateTo)
        {
            Company oCompany = new Company();
            List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
            List<EmployeeSalary_MAMIYA> oEmployeeSalarys = new List<EmployeeSalary_MAMIYA>();
            List<EmployeeSalaryDetail> oEmployeeSalaryDetails = new List<EmployeeSalaryDetail>();

            oEmployeeSalarys = oEmployeeSalary.EmployeeSalary_MAMIYAs;

            TimeSpan diff = dtDateTo - dtDateFrom;
            int nDays = diff.Days + 1;

            string sDateFrom = "";
            string sDateTo = "";

            sDateFrom = dtDateFrom.ToString("dd") + "/" + dtDateFrom.ToString("MM") + "/" + dtDateFrom.ToString("yy");
            sDateTo = dtDateTo.ToString("dd") + "/" + dtDateTo.ToString("MM") + "/" + dtDateTo.ToString("yy");

            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Pay Slip");
                sheet.PrinterSettings.TopMargin = 0.2M;
                sheet.PrinterSettings.LeftMargin = 0.2M;
                sheet.PrinterSettings.BottomMargin = 0.2M;
                sheet.PrinterSettings.RightMargin = 0.2M;
                //sheet.PrinterSettings.TopMargin = 0;
                //sheet.PrinterSettings.LeftMargin = 0;
                //sheet.PrinterSettings.BottomMargin = 0;
                //sheet.PrinterSettings.RightMargin = 0;
                sheet.PrinterSettings.Orientation = eOrientation.Portrait;
                sheet.PrinterSettings.PaperSize = ePaperSize.A4;

                sheet.Name = "Pay Slip";

                sheet.Column(1).Width = 28;//Caption
                sheet.Column(2).Width = 22;//Value
                sheet.Column(3).Width = 28;//Caption
                sheet.Column(4).Width = 20;//Value

                nMaxColumn = 4;

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Table Body

                int rowIndex = 0;
                int nPS = 0;
                foreach (EmployeeSalary_MAMIYA oItem in oEmployeeSalarys)
                {
                    #region Report Header
                    rowIndex += 1;
                    int colIndex = 1;
                    if (oCompany.CompanyLogo != null)
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        sheet.Row(rowIndex).Height = 30;
                        //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style= border.Right.Style = ExcelBorderStyle.None;

                        ExcelPicture excelImage = null;
                        excelImage = sheet.Drawings.AddPicture(oItem.EmployeeID.ToString(), oCompany.CompanyLogo);
                        excelImage.From.Column = 0;
                        excelImage.From.Row = (rowIndex - 1);
                        excelImage.SetSize(85, 40);
                        excelImage.From.ColumnOff = this.Pixel2MTU(2);
                        excelImage.From.RowOff = this.Pixel2MTU(2);
                        excelImage.SetPosition(rowIndex - 1, 1, colIndex-1, 118);
                    }
                    else
                    {
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //border = cell.Style.Border; border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                        
                    }                   
                    colIndex++;

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 30; cell.Style.Font.SetFromFont(new Font("Century Gothic",22));
                    //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 22; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    
                    rowIndex = rowIndex + 1;
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 16;
                    //border = cell.Style.Border; border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    colIndex = 1;
                    rowIndex = rowIndex + 1;

                    //cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true;
                    //cell.Value = ""; cell.Style.Font.Bold = true;
                    //border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.Thin;
                    //border = cell.Style.Border; border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.Thin;
                    //cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //rowIndex = rowIndex + 1;

                    #endregion

                    #region rows
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, 2]; cell.Merge = true;
                    cell.Value = "Monthly Pay Slip (মাসিক বেতন স্লিপ)"; cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 12;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    border = cell.Style.Border;
                    border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    cell = sheet.Cells[rowIndex, 3]; cell.Merge = true;
                    cell.Value = dtDateTo.ToString("MMMM") + "," + dtDateTo.ToString("yyyy"); cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 12;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    border = cell.Style.Border;
                    border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style = ExcelBorderStyle.Thin;
                    cell = sheet.Cells[rowIndex, 4]; cell.Merge = true;
                    cell.Value = "Pay Slip No - " + oItem.EmployeeSalaryID.ToString(); cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 12;
                    cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    

                    border = cell.Style.Border;
                    border.Left.Style = border.Top.Style = border.Right.Style = border.Bottom.Style =ExcelBorderStyle.Thin; 
                    rowIndex = rowIndex + 1;

                    int nfontSize = 9;
                    double nRH = 13;

                    //1st row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Name(নাম):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary Cycle(বেতন প্রদানের মাস):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(sDateFrom) + "হতে" + NumberInBan(sDateTo); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //2nd row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Code No.(কোড নং.):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Calender(ক্যালেন্ডার দিন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(nDays.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex = rowIndex + 1;

                    //3rd row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Designation(পদবী):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DesignationName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Working Days(মোট কার্যদিবস):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    //NumberInBan(oItem.EarnLeave.ToString())
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.TotalWorkingDay.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //4th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Div(বিভাগ):" ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Present Days(উপস্থিতি দিন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.TotalPresent.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //5th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Sub-Section(সাব-সেকশন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    //
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Weekly Dayoff Days (সাপ্তাহিক ছুটির দিন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.None;

                    //value not from sp static value
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.FriDay.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //6th row
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Joining Date(যোগদানের তাং.):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DateOfJoinInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;
                    //
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Dec. Holidays(ঘোষিত ছুটির দিন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.HoliDay.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;
                    
                    rowIndex = rowIndex + 1;

                    //7th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Confirm Date(স্থায়ীকরনের তারিখ):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.ConfirmationDateInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "C. Leave(নৈমিত্তিক ছুটির কাটানোর দিন)"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.CasualLeave.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //8th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Emp. Category(কর্মকর্তা/শ্রমিকের ধরন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.EmployeeCategoryInString; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "E. Leave(অর্জিত ছুটি কাটানোর দিন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.EarnLeave.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex = rowIndex + 1;
                    //9th row

                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Gross Wages(মোট বেতন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.GrossSalary.ToString("#,###,##0")); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "S. Leave(অসুস্থতা ছুটি কাটানোর ঘণ্টা):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.AbsentHr_Sick.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //10th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Basic(মূল বেতন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.Basic.ToString("#,###,##0")); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "L.W.P(অননুমোদিত অনুপস্থিতি ঘণ্টা):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = @NumberInBan(ICS.Core.Utility.Global.MillionFormat(@oItem.AbsentHr_WOPay)); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //11th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Allowance(অনান্য ভাতা):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "-"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT Hrs.(ওভার টাইম ঘন্টা):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT_HHR-" + NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.OT_HHR)) + ",OT_NHR-" + NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.OT_NHR)) + ",OT_FHR-" + NumberInBan(ICS.Core.Utility.Global.MillionFormat(oItem.FHOT_HR)); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = 8; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //12th row
                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bank Account no.(অ্যাকাউন্ট নং.):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.BankAccountNo; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "NightAllow. Days(রাত্রিকালীন কাজের দিন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.NightAllowDays.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //13th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Last month Loan Bal.(ঋণের ব্যালেন্স):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = ExcelBorderStyle.None; border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.LastMonthLoanBalance.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "L/Month. PF Sub.(পি এফ চাঁদার ব্যালেন্স):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = NumberInBan(oItem.LastMonthPFSub.ToString()); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = ExcelBorderStyle.None; border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //14th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Payble Basic(প্রদেয় মূল):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.Basic > 0 ? NumberInBan(oItem.Basic.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PF Sub. (পি এফ চাঁদা):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.PF > 0 ? NumberInBan(oItem.PF.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //15th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "House Rent(বাড়ি ভাড়া):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.HRent > 0 ? NumberInBan(oItem.HRent.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Loan Inst.(ঋণ কিস্তির পরিমাণ):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.InstallmentAmt > 0 ? NumberInBan(oItem.InstallmentAmt.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //16th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Medical Allow.(চিকিৎসা ভাতা):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.Med > 0 ? NumberInBan(oItem.Med.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Loan Interest(ঋণ সুদের পরিমাণ):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.InterestAmt > 0 ? NumberInBan(oItem.InterestAmt.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    //17th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Conveyance(যাতায়াত ভাতা):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.Conveyance > 0 ? NumberInBan(oItem.Conveyance.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Absent Ded.(অনুপস্থিত কর্তন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.TotalAbsentAmount > 0 ? NumberInBan(oItem.TotalAbsentAmount.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                    //18th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT Allow.(ওটি  ভাতা):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.OTAmount > 0 ? NumberInBan(oItem.OTAmount.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL Ded.(অসুস্থতা ছুটি কর্তন):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.SickLeaveDeduction > 0 ? NumberInBan(oItem.SickLeaveDeduction.ToString("#,###,##0")) : "-"); ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                    //19th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Night Allow.(রাত্রিকালীন কাজের ভাতা):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.NightAllow > 0 ? NumberInBan(oItem.NightAllow.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Office Trans.(অফিস পরিবহন বাবদ):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.TRNS > 0 ? NumberInBan(oItem.TRNS.ToString("#,###,##0")) : "-"); ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                    //20th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Attn. Bonus(উপস্থিতি বোনাস):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.AttendanceBonus > 0 ? NumberInBan(oItem.AttendanceBonus.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Income Tax(আয়কর):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.IncomeTax > 0 ? NumberInBan(oItem.IncomeTax.ToString("#,###,##0")) : "-"); ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                    //21th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ADJCR(ক্রেডিট সমন্বয়):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.ADJCR > 0 ? NumberInBan(oItem.ADJCR.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "ADJDR(ডেবিট সমন্বয়):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.ADJDR > 0 ? NumberInBan(oItem.ADJDR.ToString("#,###,##0")) : "-"); ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                    //22th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Mob. Bill(মোবাইল বিল):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.MobileBill > 0 ? NumberInBan(oItem.MobileBill.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Other Deb.(অনান্য):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "-"; ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                    //23th row
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Holiday All.(হলিডে কাজের ভাতা):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.HolidayAll > 0 ? NumberInBan(oItem.HolidayAll.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Stamp(ষ্ট্যাম্প):"; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.RS > 0 ? NumberInBan(oItem.RS.ToString("#,###,##0")) : "-"); ; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;
                    nRH = 13;
                    nfontSize = 11;
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Earning(মোট উপার্জন):"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = 8;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.EarningsTotal > 0 ? NumberInBan(oItem.EarningsTotal.ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Deduction	(মোট কর্তন)"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = 8;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = (oItem.DeductionTotal > 0 ? NumberInBan(oItem.DeductionTotal.ToString("#,###,##0")) : "-"); ; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    nRH = 16;
                    nfontSize = 12;
                    colIndex = 1;
                    sheet.Row(rowIndex).Height = nRH;
                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Net Payable	(নেট প্রদেয়):"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = 8;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = ((oItem.EarningsTotal - oItem.DeductionTotal) > 0 ? NumberInBan((oItem.EarningsTotal - oItem.DeductionTotal).ToString("#,###,##0")) : "-"); cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nfontSize = 9;
                    cell = sheet.Cells[rowIndex, colIndex++, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "In Word:" + @ICS.Core.Utility.Global.TakaWords(@oItem.EarningsTotal - @oItem.DeductionTotal); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.WrapText = true;
                    cell.Style.Font.Size = nfontSize; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    colIndex = 1;
                    cell = sheet.Cells[rowIndex, colIndex++, rowIndex+1, nMaxColumn]; cell.Merge = true; cell.Style.Font.Size = 6; sheet.Row(rowIndex).Height = 9;
                    cell.Value = "বিঃ দ্রঃ- এটি একটি কম্পিউটার উৎপন্ন পে-স্লিপ, তাই কোনো প্রকার স্বাক্ষর বা সীল এর প্রয়োজন নেই। পে-স্লিপের তথ্য সংক্রান্ত যে কোন ধরনের অনুসন্ধান এবং অভিযোগ থাকলে, পে-স্লিপ প্রাপ্তির পরবর্তী ১০(দশ) কর্ম দিবসের মধ্যে মানব সম্পদ বিভাগে অবগত করার জন্য অনুরোধ করা গেল। উক্ত সময়ের পরে পে-স্লিপ সংক্রান্ত কোনো প্রকার অভিযোগ গ্রহণযোগ্য হবে না। "; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    border = cell.Style.Border; border.Left.Style = border.Right.Style = border.Bottom.Style=ExcelBorderStyle.Thin; border.Top.Style = ExcelBorderStyle.None;

                    #endregion
                    //rowIndex = rowIndex + 1;
                    //nPS++;
                    //if (nPS % 2 != 0)
                    //{
                    //    rowIndex = rowIndex + 2;
                    //    colIndex = 1;
                    //    cell = sheet.Cells[rowIndex, colIndex++, ++rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
                    //    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                    //    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    //}
                    //else if(nPS % 2 == 0)
                    //{
                    //    rowIndex = rowIndex + 1;
                    //    colIndex = 1;
                    //    cell = sheet.Cells[rowIndex, colIndex++, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false;
                    //    sheet.Row(rowIndex).Height = 5;
                    //    //fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); 
                    //    //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                    //}
                    rowIndex += 1;
                    if (rowIndex % 60 == 0)
                    {
                        PageBreak(ref sheet, rowIndex, nMaxColumn);
                    }

                }

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=PAYSLIP.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        public void PageBreak(ref ExcelWorksheet sheet, int nRowIndex, int nEndCol)
        {
            sheet.Row(nRowIndex).PageBreak = true;
            sheet.Row(nEndCol).PageBreak = true;
        }
        #endregion PaySlip XL
        public string NumberInBan(string num)
        {
            string[] Numbers = { "০", "১", "২", "৩", "৪", "৫", "৬", "৭", "৮", "৯", ",", "." };
            var nlength1 = 0;
            var nlength2 = 1;
            var NumberInBangla = "";
            var number = "";
            number = num;

            for (var i = 0; i < number.Length; i++)
            {
                if (number.Substring(nlength1, nlength2) == "0")
                {
                    NumberInBangla += Numbers[0];
                }
                else if (number.Substring(nlength1, nlength2) == "1")
                {
                    NumberInBangla += Numbers[1];
                }
                else if (number.Substring(nlength1, nlength2) == "2")
                {
                    NumberInBangla += Numbers[2];
                }
                else if (number.Substring(nlength1, nlength2) == "3")
                {
                    NumberInBangla += Numbers[3];
                }
                else if (number.Substring(nlength1, nlength2) == "4")
                {
                    NumberInBangla += Numbers[4];
                }
                else if (number.Substring(nlength1, nlength2) == "5")
                {
                    NumberInBangla += Numbers[5];
                }
                else if (number.Substring(nlength1, nlength2) == "6")
                {
                    NumberInBangla += Numbers[6];
                }
                else if (number.Substring(nlength1, nlength2) == "7")
                {
                    NumberInBangla += Numbers[7];
                }
                else if (number.Substring(nlength1, nlength2) == "8")
                {
                    NumberInBangla += Numbers[8];
                }
                else if (number.Substring(nlength1, nlength2) == "9")
                {
                    NumberInBangla += Numbers[9];
                }
                else if (number.Substring(nlength1, nlength2) == ",")
                {
                    NumberInBangla += Numbers[10];
                }
                else if (number.Substring(nlength1, nlength2) == "/" || number.Substring(nlength1, nlength2) == ".")
                {
                    NumberInBangla += Numbers[11];
                }

                nlength1++;
                //nlength2++;
            }
            return NumberInBangla;
        }

        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }

        #region ConsolidatePay XL
        public void PrintConsolidatePayXL(string sEmpIDs, string sDate, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, int nPayType, double ts)
        {
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);

            List<ConsolidatePay_MAMIYA> oConsolidatePays = new List<ConsolidatePay_MAMIYA>();
            oConsolidatePays = ConsolidatePay_MAMIYA.Gets(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs,nMonthID, nPayType, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            this.PrintCPXL(oConsolidatePays, dtDateFrom, dtDateTo);
        }
        private void PrintCPXL(List<ConsolidatePay_MAMIYA> oConsolidatePays, DateTime dtDateFrom, DateTime dtDateTo)
        {
            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Pay Slip");
                sheet.Name = "Pay Slip";

                sheet.Column(1).Width = 8;//SL
                sheet.Column(2).Width = 20;//DepartmentName
                sheet.Column(3).Width = 20;//Wages
                sheet.Column(4).Width = 20;//Salary
                sheet.Column(5).Width = 20;//OTWages
                sheet.Column(6).Width = 20;//OTSalary
                sheet.Column(7).Width = 20;//TotalWages
                sheet.Column(8).Width = 20;//TotalSalary
                sheet.Column(9).Width = 20;//BonusWages
                sheet.Column(10).Width = 20;//BonusSalary

                nMaxColumn = 10;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Table Body

                #region Report Header
                int colIndex = 1;
                int rowIndex = 1;
                int nImage = 0;
                if (oCompany.CompanyLogo != null)
                {
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + 3]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ExcelPicture excelImage = null;
                    excelImage = sheet.Drawings.AddPicture(nImage++.ToString(), oCompany.CompanyLogo);
                    excelImage.From.Column = 0;
                    excelImage.From.Row = (rowIndex - 1);
                    excelImage.SetSize(85, 22);
                    excelImage.From.ColumnOff = this.Pixel2MTU(2);
                    excelImage.From.RowOff = this.Pixel2MTU(2);
                    excelImage.SetPosition(rowIndex - 1, 1, colIndex - 1, 60);
                }
                else
                {
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + 3]; cell.Merge = true; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }
                colIndex++;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;  cell.Style.Font.SetFromFont(new Font("Century Gothic", 22));
                cell.Style.Font.Size = 22; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                rowIndex = rowIndex + 1;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = "Consolidate Pay Report"; cell.Style.Font.Bold = true; 
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = "From " + dtDateFrom.ToString("dd MMM yyyy") + " To " + dtDateTo.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 12;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex = rowIndex + 2;

                int nfontSize = 12;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style =  border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style=border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Wages"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style =  border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT Wages"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "OT Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style =  border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Wages"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style =  border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style =  border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bonus Wages"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style =  border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Bonus Salary"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style =  border.Right.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                #endregion
                int nSL = 0;

           

                double totalWages = 0;
                double totalSalary = 0;
                double totalOTWages = 0;
                double totalOTSalary = 0;
                double totalTotalWages = 0;
                double totalTotalSalary = 0;
                double totalBonusWages = 0;
                double totalBonusSalary = 0;

                foreach (ConsolidatePay_MAMIYA oItem in oConsolidatePays)
                {
                    nSL++;
                    colIndex = 1;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style =border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.Wages); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.Salary); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.OTWages); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.OTSalary); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.TotalWages); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.TotalSalary); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.BonusWages); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style =border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(oItem.BonusSalary); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style =  border.Right.Style = ExcelBorderStyle.Thin;

                    rowIndex = rowIndex + 1;

                    totalWages = totalWages+oItem.Wages;
                    totalSalary = totalSalary +oItem.Salary;
                    totalOTWages=totalOTWages+oItem.OTWages;
                    totalOTSalary = totalOTSalary+oItem.OTSalary;
                    totalTotalWages = totalTotalWages+ oItem.TotalWages;
                    totalTotalSalary = totalTotalSalary+oItem.TotalSalary;
                    totalBonusWages = totalBonusWages+oItem.BonusWages;
                    totalBonusSalary = totalBonusSalary+oItem.BonusSalary;
                }

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + 1]; cell.Merge = true; cell.Value = "Total"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(totalWages); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(totalSalary); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(totalOTWages); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(totalOTSalary); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(totalTotalWages); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(totalTotalSalary); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(totalBonusWages); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex]; cell.Value = Global.MillionFormat(totalBonusSalary); cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Consolidate.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }

        #endregion ConsolidatePay XL

        #region Loan PF and Interest
        public void PrintLoanPFAndInterestXL(string sEmpIDs, string sDate, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, int nPayType, double ts)
        {
            DateTime dtDateFrom = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime dtDateTo = Convert.ToDateTime(sDate.Split(',')[1]);

            List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs = new List<EmployeeSalary_MAMIYA>();
            oEmployeeSalary_MAMIYAs = EmployeeSalary_MAMIYA.Gets_MAMIYA(dtDateFrom, dtDateTo, sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nPayType, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            this.LoanPFAndInterestXL(oEmployeeSalary_MAMIYAs, dtDateFrom, dtDateTo);
        }
        private void LoanPFAndInterestXL(List<EmployeeSalary_MAMIYA> oEmployeeSalary_MAMIYAs, DateTime dtDateFrom, DateTime dtDateTo)
        {
            int nMaxColumn = 0;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Pay Slip");
                sheet.Name = "Pay Slip";

                sheet.Column(1).Width = 5;//SL
                sheet.Column(2).Width = 20;//DepartmentName
                sheet.Column(3).Width = 10;//Type
                sheet.Column(4).Width = 20;//PF Deduction
                sheet.Column(5).Width = 20;//Loan Deduction
                sheet.Column(6).Width = 20;//Interest Deduction
                sheet.Column(7).Width = 20;//Total Amount

                nMaxColumn = 7;

                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetImage(oCompany.OrganizationLogo);

                #region Table Body

                #region Report Header
                int colIndex = 1;
                int rowIndex = 1;
                int nImage = 0;
                if (oCompany.CompanyLogo != null)
                {
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + 2]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ExcelPicture excelImage = null;
                    excelImage = sheet.Drawings.AddPicture(nImage++.ToString(), oCompany.CompanyLogo);
                    excelImage.From.Column = 0;
                    excelImage.From.Row = (rowIndex - 1);
                    excelImage.SetSize(130, 22);
                    excelImage.From.ColumnOff = this.Pixel2MTU(2);
                    excelImage.From.RowOff = this.Pixel2MTU(2);
                    excelImage.SetPosition(rowIndex - 1, 0, colIndex - 2, 80);
                }
                else
                {
                    cell = sheet.Cells[rowIndex, colIndex, rowIndex, colIndex = colIndex + 3]; cell.Merge = true; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                }
                colIndex++;

                cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true; cell.Style.Font.SetFromFont(new Font("Century Gothic", 22));
                cell.Style.Font.Size = 22; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                rowIndex = rowIndex + 1;
                colIndex = 1;
                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 10; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = "Consolidate PF Report"; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, nMaxColumn]; cell.Merge = true;
                cell.Value = "From " + dtDateFrom.ToString("dd MMM yyyy") + " To " + dtDateTo.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true; sheet.Row(rowIndex).Height = 12;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rowIndex = rowIndex + 2;

                int nfontSize = 12;
                colIndex = 1;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Type"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "PF Deduction"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Loan Deduction"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Interest Deduction"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = "Total Amount"; cell.Style.Font.Bold = true;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
              
                rowIndex = rowIndex + 1;

                #endregion
                int nSL = 0;
                List<EmployeeSalary_MAMIYA> oEmployeeSalarys = new List<EmployeeSalary_MAMIYA>();
                oEmployeeSalary_MAMIYAs.ForEach(x => oEmployeeSalarys.Add(x));
                while (oEmployeeSalary_MAMIYAs.Count > 0)
                {
                    colIndex = 1;
                    nSL++;
                    List<EmployeeSalary_MAMIYA> oTempESMs = new List<EmployeeSalary_MAMIYA>();
                    oTempESMs = oEmployeeSalary_MAMIYAs.Where(x => x.DepartmentName == oEmployeeSalary_MAMIYAs[0].DepartmentName).ToList();

                    cell = sheet.Cells[rowIndex, colIndex, rowIndex + 1, colIndex]; cell.Merge = true; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, ++colIndex, rowIndex + 1, colIndex]; cell.Merge = true; cell.Value = oEmployeeSalary_MAMIYAs[0].DepartmentName; cell.Style.Font.Bold = false;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                    string stype = "";
                    for (int i = 0; i < 2; i++)
                    {
                        colIndex = 3;
                        double PF = 0;
                        double Loan = 0;
                        double Interest = 0;
                        if (i == 0) { 
                            stype = "Wages";
                            PF = oTempESMs.Where(x => x.EmployeeTypeID == 1).Sum(x => x.PF);
                            Loan = oTempESMs.Where(x => x.EmployeeTypeID == 1).Sum(x => x.PLoan);
                            Interest = oTempESMs.Where(x => x.EmployeeTypeID == 1).Sum(x => x.InterestAmt);
                        } else { 
                            stype = "Others";
                            PF = oTempESMs.Where(x => x.EmployeeTypeID == 2 && x.EmployeeTypeID == 3).Sum(x => x.PF);
                            Loan = oTempESMs.Where(x => x.EmployeeTypeID == 2 && x.EmployeeTypeID == 3).Sum(x => x.PLoan);
                            Interest = oTempESMs.Where(x => x.EmployeeTypeID == 2 && x.EmployeeTypeID == 3).Sum(x => x.InterestAmt);
                        }
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = stype; cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;
                         
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(PF); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(Loan); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(Interest); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(PF+Loan+Interest); cell.Style.Font.Bold = false;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;
                        
                        rowIndex = rowIndex + 1;
                    }
                    oEmployeeSalary_MAMIYAs.RemoveAll(x => x.DepartmentName == oTempESMs[0].DepartmentName);
                }

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Total Wages"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                double nTotalPFWages = 0;
                nTotalPFWages = oEmployeeSalarys.Where(x => x.EmployeeTypeID == 1).Sum(x => x.PF);

                colIndex = colIndex + 3;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPFWages); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double nTotalLoanWages = 0;
                nTotalLoanWages = oEmployeeSalarys.Where(x => x.EmployeeTypeID == 1).Sum(x => x.PLoan);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalLoanWages); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                double nInterestWages = 0;
                nInterestWages = oEmployeeSalarys.Where(x => x.EmployeeTypeID == 1).Sum(x => x.InterestAmt);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nInterestWages); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPFWages + nTotalLoanWages + nInterestWages); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Total Salary"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                double nTotalPFSalary = 0;
                nTotalPFSalary = oEmployeeSalarys.Where(x => x.EmployeeTypeID == 2 && x.EmployeeTypeID == 3).Sum(x => x.PF);

                colIndex = colIndex + 3;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPFSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                double nTotalLoanSalary = 0;
                nTotalLoanSalary = oEmployeeSalarys.Where(x => x.EmployeeTypeID == 2 && x.EmployeeTypeID == 3).Sum(x => x.PLoan);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalLoanSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                double nInterestSalary = 0;
                nInterestSalary = oEmployeeSalarys.Where(x => x.EmployeeTypeID == 2 && x.EmployeeTypeID == 3).Sum(x => x.InterestAmt);
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nInterestSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPFSalary + nTotalLoanSalary+ nInterestSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Sub Total"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                colIndex = colIndex + 3;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPFWages+nTotalPFSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalLoanWages+nTotalLoanSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nInterestWages+nInterestSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPFWages + nTotalLoanWages + nInterestWages+nTotalPFSalary + nTotalLoanSalary + nInterestSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Employee PF Contribution"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                colIndex = colIndex + 3;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPFWages + nTotalPFSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalLoanWages + nTotalLoanSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nInterestWages + nInterestSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPFWages + nTotalLoanWages + nInterestWages + nTotalPFSalary + nTotalLoanSalary + nInterestSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 1;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Grand Total for payment"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                colIndex = colIndex + 3;
                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPFWages + nTotalPFSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalLoanWages + nTotalLoanSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nInterestWages + nInterestSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, colIndex++]; cell.Value = Global.MillionFormat(nTotalPFWages + nTotalLoanWages + nInterestWages + nTotalPFSalary + nTotalLoanSalary + nInterestSalary); cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                rowIndex = rowIndex + 3;

                colIndex = 1;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, 3]; cell.Merge = true; cell.Value = "Prepared By"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                colIndex = colIndex + 3;
                cell = sheet.Cells[rowIndex, colIndex, rowIndex, ++colIndex]; cell.Merge = true; cell.Value = "Checked By"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[rowIndex, ++colIndex, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = "Approved By"; cell.Style.Font.Bold = false;
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                cell.Style.Font.Size = nfontSize; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;

                #endregion

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=LoanPF.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
        }
        #endregion  Loan PF and Interest
    }
}
