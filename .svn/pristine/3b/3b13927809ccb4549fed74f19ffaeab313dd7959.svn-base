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


namespace ESimSolFinancial.Controllers
{
    public class EmployeeSalary_ZNController : Controller
    {
        #region Declaration
        EmployeeSalary _oEmployeeSalary;
        private List<EmployeeSalary> _oEmployeeSalarys;
        #endregion

        #region Views
        public ActionResult View_EmployeeSalarys_ZN(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oEmployeeSalarys = new List<EmployeeSalary>();
            return View(_oEmployeeSalarys);
        }

        public ActionResult View_PrintMultiplePaySlip_ZN_First(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, double ts)
        {
            _oEmployeeSalary = new EmployeeSalary();
            //string sSql = "SELECT * FROM VIEW_EmployeeSalary WHERE  EmployeeSalaryID IN(" + EmployeeSalaryIDs + ") Order by EmployeeCode";
            //string sSqlDetail = "SELECT *FROM VIEW_EmployeeSalaryDetail WHERE EmployeeSalaryID IN(" + EmployeeSalaryIDs + ")";
            //string sSqlDAction = "SELECT * FROM EmployeeSalaryDetailDisciplinaryAction WHERE EmployeeSalaryID IN(" + EmployeeSalaryIDs + ")";
            //_oEmployeeSalary.EmployeeSalarys = EmployeeSalary.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //_oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSqlDetail, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //_oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = EmployeeSalaryDetailDisciplinaryAction.Gets(sSqlDAction, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeSalary = GetForSalarySheet_Zn(sEmpIDs, sDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs);
            _oEmployeeSalary.ErrorMessage = sDate;
            return PartialView(_oEmployeeSalary);
        }
        public ActionResult View_PrintMultiplePaySlip_ZN_Final(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, double ts)
        {
            _oEmployeeSalary = new EmployeeSalary();
            //string sSql = "SELECT * FROM VIEW_EmployeeSalary WHERE  EmployeeSalaryID IN(" + EmployeeSalaryIDs + ") Order by EmployeeCode";
            //string sSqlDetail = "SELECT *FROM VIEW_EmployeeSalaryDetail WHERE EmployeeSalaryID IN(" + EmployeeSalaryIDs + ")";
            //string sSqlDAction = "SELECT * FROM EmployeeSalaryDetailDisciplinaryAction WHERE EmployeeSalaryID IN(" + EmployeeSalaryIDs + ")";
            //_oEmployeeSalary.EmployeeSalarys = EmployeeSalary.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //_oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSqlDetail, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //_oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = EmployeeSalaryDetailDisciplinaryAction.Gets(sSqlDAction, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oEmployeeSalary = GetForSalarySheet_Zn(sEmpIDs, sDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs);
            _oEmployeeSalary.ErrorMessage = sDate;
            return PartialView(_oEmployeeSalary);
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
        public JsonResult SearchSalaryByDepartmentAndDateRange(string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, string dtDateFrom, string dtDateTo, int nLoadRecords, int nRowLength, double ts)
        {
            try
            {
                string sSql = "";
                sSql = "SELECT * FROM (SELECT top(" + nLoadRecords + ")* FROM (SELECT ROW_NUMBER() OVER(ORDER BY EmployeeSalaryID) Row,* FROM View_EmployeeSalary WHERE StartDate='" + dtDateFrom + "' AND EndDate='" + dtDateTo + "'";
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
        public ActionResult PrintSalarySheet_First(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, double ts)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            oEmployeeSalary = GetForSalarySheet_Zn(sEmpIDs, sDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs);
            rptSalarySheet_First oReport = new rptSalarySheet_First();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintSalarySheet_OT(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, double ts)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            oEmployeeSalary = GetForSalarySheet_Zn(sEmpIDs, sDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs);
            rptSalarySheet_OT oReport = new rptSalarySheet_OT();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
            return File(abytes, "application/pdf");
        }

        public ActionResult PrintSalarySheet_Final(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, double ts)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            oEmployeeSalary=GetForSalarySheet_Zn(sEmpIDs, sDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs);
            rptSalarySheet_Final oReport = new rptSalarySheet_Final();
            byte[] abytes = oReport.PrepareReport(oEmployeeSalary);
            return File(abytes, "application/pdf");
        }

        public EmployeeSalary GetForSalarySheet_Zn(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            oEmployeeSalary.Employees = new List<Employee>();
            oEmployeeSalary.EmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
            oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = new List<EmployeeSalaryDetailDisciplinaryAction>();
            string sStartDate = sDate.Split(',')[0];
            string sEndDate = sDate.Split(',')[1];
            string sSql_Emp = "";
            string sSql_ESD = "";
            string sSql_ESDDA = "";

            oEmployeeSalary.EmployeeSalarys = EmployeeSalary.Gets_ZN(sEmpIDs, sDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            string EmpIDs = string.Join(",", oEmployeeSalary.EmployeeSalarys.Select(x => x.EmployeeID));
            if (EmpIDs != "" && EmpIDs != null)
            {
                sSql_Emp = "SELECT * FROM View_Employee_WithImage WHERE EmployeeID IN ( " + EmpIDs + ")";
                sSql_ESD = "SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN (" + EmpIDs + ") AND StartDate='" + sStartDate + "' AND EndDate='" + sEndDate + "') ORDER BY SalaryHeadID";
                sSql_ESDDA = "SELECT * FROM EmployeeSalaryDetailDisciplinaryAction WHERE EmployeeSalaryID IN(SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN(" + EmpIDs + "))";
                oEmployeeSalary.Employees = Employee.Gets(sSql_Emp, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql_ESD, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = EmployeeSalaryDetailDisciplinaryAction.Gets(sSql_ESDDA, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            oEmployeeSalary.ErrorMessage = sStartDate + "," + sEndDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oEmployeeSalary.Company = oCompanys.First();
            oEmployeeSalary.Company.CompanyLogo = GetImage(oEmployeeSalary.Company.OrganizationLogo);
            foreach (Employee oItem in oEmployeeSalary.Employees)
            {
                oItem.EmployeePhoto = GetImage(oItem.Photo);
            }
            return oEmployeeSalary;
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

        //public Image GetEmployeePhoto(Employee oEmployee)
        //{
        //    if (oEmployee.Photo != null)
        //    {
        //        MemoryStream m = new MemoryStream(oEmployee.Photo);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(m);
        //        img.Save(Response.OutputStream, ImageFormat.Jpeg);
        //        return img;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public Image GetCompanyLogo(Company oCompany)
        //{
        //    if (oCompany.OrganizationLogo != null)
        //    {
        //        MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(m);
        //        img.Save(Response.OutputStream, ImageFormat.Jpeg);
        //        return img;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
 
        #endregion Report

        #region Salary Summery
        public ActionResult PrintSalarySummery_Ac(string sEmpIDs, string sDate, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, double ts)
        {
            EmpSalarySummery oSalarySummery = new EmpSalarySummery();
            DateTime sStartDate = Convert.ToDateTime(sDate.Split(',')[0]);
            DateTime sEndDate = Convert.ToDateTime(sDate.Split(',')[1]);
            oSalarySummery.SalarySummerys = EmpSalarySummery.Gets(sEmpIDs, sStartDate, sEndDate, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oSalarySummery.ErrorMessage = sDate;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oSalarySummery.Company = oCompanys.First();
            oSalarySummery.Company.CompanyLogo = GetImage(oSalarySummery.Company.OrganizationLogo);
            //oSalarySummery.Company.CompanyLogo = GetCompanyLogo(oSalarySummery.Company);

            rptEMPSalarySummery_AC oReport = new rptEMPSalarySummery_AC();
            byte[] abytes = oReport.PrepareReport(oSalarySummery);
            return File(abytes, "application/pdf");
        }
        #endregion Salary Summery
    }
}
