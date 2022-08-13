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
using CrystalDecisions.CrystalReports.Engine;
using System.Data;

namespace ESimSolFinancial.Controllers
{
    public class AttendanceDaily_ZNController : PdfViewController
    {
        #region Declaration
        AttendanceDaily_ZN _oAttendanceDaily_ZN;
        private List<AttendanceDaily_ZN> _oAttendanceDaily_ZNs;
        private List<MaxOTConfiguration> _oMaxOTConfiguration;

        #endregion

        #region view
        public ActionResult View_Rpt_EmpWiseAttendance(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Attendance).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.AuthorizationRolesMapping_ESS = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.EmployeeSalaryStructure).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            _oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            string sSql = "SELECT * FROM HRM_Shift WHERE IsActive=1";
            _oAttendanceDaily_ZN.HRMShifts = HRMShift.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            sSql = "";
            sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            ViewBag.AuthorizationRolesMappingPermission = GetPermissionList();
            return View(_oAttendanceDaily_ZN);
        }
        #endregion view
        public List<EnumObject> GetPermissionList()
        {
            List<EnumObject> oEnumObjects = new List<EnumObject>();
            List<EnumObject> objEnumObjects = new List<EnumObject>();

            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Attendance).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            foreach (AuthorizationRoleMapping oItem in oAuthorizationRoleMappings)
            {
                if (oItem.OperationType == EnumRoleOperationType.Time_Card_F1)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumEmployeeTimeCard.Time_Card_F1;
                    oEnumObject.Value = EnumObject.jGet(EnumEmployeeTimeCard.Time_Card_F1);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.Time_Card_F2)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumEmployeeTimeCard.Time_Card_F2;
                    oEnumObject.Value = EnumObject.jGet(EnumEmployeeTimeCard.Time_Card_F2);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.Time_Card_F2_1)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumEmployeeTimeCard.Time_Card_F2_1;
                    oEnumObject.Value = EnumObject.jGet(EnumEmployeeTimeCard.Time_Card_F2_1);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.Time_Card_F3)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumEmployeeTimeCard.Time_Card_F3;
                    oEnumObject.Value = EnumObject.jGet(EnumEmployeeTimeCard.Time_Card_F3);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.Time_Card_F4)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumEmployeeTimeCard.Time_Card_F4;
                    oEnumObject.Value = EnumObject.jGet(EnumEmployeeTimeCard.Time_Card_F4);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.Time_Card_F5)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumEmployeeTimeCard.Time_Card_F5;
                    oEnumObject.Value = EnumObject.jGet(EnumEmployeeTimeCard.Time_Card_F5);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.Time_Card_F6)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumEmployeeTimeCard.Time_Card_F6;
                    oEnumObject.Value = EnumObject.jGet(EnumEmployeeTimeCard.Time_Card_F6);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.Time_Card_FC7)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumEmployeeTimeCard.Time_Card_FC7;
                    oEnumObject.Value = EnumObject.jGet(EnumEmployeeTimeCard.Time_Card_FC7);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.Time_Card_Worker)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumEmployeeTimeCard.Time_Card_Worker;
                    oEnumObject.Value = EnumObject.jGet(EnumEmployeeTimeCard.Time_Card_Worker);
                    objEnumObjects.Add(oEnumObject);
                }
                if (oItem.OperationType == EnumRoleOperationType.Job_Card)
                {
                    EnumObject oEnumObject = new EnumObject();
                    oEnumObject.id = (int)EnumEmployeeTimeCard.Job_Card;
                    oEnumObject.Value = EnumObject.jGet(EnumEmployeeTimeCard.Job_Card);
                    objEnumObjects.Add(oEnumObject);
                }
            }
            return objEnumObjects;
        }

        #region Search
        [HttpPost]
        public JsonResult EmployeeWiseAttendanceSearch(string sTemp)
        {
            _oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            try
            {
                string sEmployeeIDs = sTemp.Split('~')[0];
                DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
                DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
                string sLocationID = sTemp.Split('~')[3];
                string sDepartmentIds = sTemp.Split('~')[4];
                string sBUnitIDs = sTemp.Split('~')[5];
                double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
                double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
                string sBlockIDs = sTemp.Split('~')[8];
                string sGroupIDs = sTemp.Split('~')[9];

                _oAttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oAttendanceDaily_ZNs.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oAttendanceDaily_ZN = new AttendanceDaily_ZN();
                _oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
                _oAttendanceDaily_ZN.ErrorMessage = ex.Message;
                _oAttendanceDaily_ZNs.Add(_oAttendanceDaily_ZN);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceDaily_ZNs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EmployeeWiseAttendanceSearchComp(string sTemp)
        {
            _oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            try
            {
                string sEmployeeIDs = sTemp.Split('~')[0];
                DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
                DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
                string sLocationID = sTemp.Split('~')[3];
                string sDepartmentIds = sTemp.Split('~')[4];
                string sBUnitIDs = sTemp.Split('~')[5];
                double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
                double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
                string sBlockIDs = sTemp.Split('~')[8];
                string sGroupIDs = sTemp.Split('~')[9];

                _oAttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCardComp(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oAttendanceDaily_ZNs.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oAttendanceDaily_ZN = new AttendanceDaily_ZN();
                _oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
                _oAttendanceDaily_ZN.ErrorMessage = ex.Message;
                _oAttendanceDaily_ZNs.Add(_oAttendanceDaily_ZN);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceDaily_ZNs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion search

        #region TimeCard
        public ActionResult PrintTimeCard(string sTemp, int nType, string sVersion)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();

            #region Searching And Get Data
            string sEmployeeIDs = sTemp.Split('~')[0];
            DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLocationID = sTemp.Split('~')[3];
            string sDepartmentIds = sTemp.Split('~')[4];
            string sBUnitIDs = sTemp.Split('~')[5];
            double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
            double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
            string sBMMIDs = sTemp.Split('~')[8];
            string sGroupIDs = sTemp.Split('~')[9];

            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F2 || nType == (int)EnumEmployeeTimeCard.Time_Card_F3)
            { 
            
            
            }
            AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sVersion, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;
            #endregion

            #region Basic Information
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.Company = oCompanys.First();
            oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
            oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
            #endregion

            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F1)
            {
                rptMamiyaTimeCard oReport = new rptMamiyaTimeCard();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F2 || nType == (int)EnumEmployeeTimeCard.Time_Card_F3)
            {
                oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;
                if (sBUnitIDs != "")
                {
                    oAttendanceDaily_ZN.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + sBUnitIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                 rptTimeCard_F2 oReport = new rptTimeCard_F2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F4)
            {
                rptTimeCard_F4 oReport = new rptTimeCard_F4();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F5)
            {
                rptTimeCard_F6 oReport = new rptTimeCard_F6();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F6)
            {
                rptTimeCard_F7 oReport = new rptTimeCard_F7();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_FC7)
            {
                rptTimeCard_FC7 oReport = new rptTimeCard_FC7();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_AMG_Worker)
            {
                oAttendanceDaily_ZN.LeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                rptTimeCard_F6_1 oReport = new rptTimeCard_F6_1();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_Worker)
            {
                oAttendanceDaily_ZN.LeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                rptTimeCard_Worker oReport = new rptTimeCard_Worker();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Job_Card)
            {
                oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;
                if (sBUnitIDs != "")
                {
                    oAttendanceDaily_ZN.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + sBUnitIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                rptJobCard oReport = new rptJobCard();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            return RedirectToAction("~/blank");
        }

        public ActionResult PrintTimeCardComp(string sTemp, int nType, string sVersion)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();

            #region Searching And Get Data
            string sEmployeeIDs = sTemp.Split('~')[0];
            DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLocationID = sTemp.Split('~')[3];
            string sDepartmentIds = sTemp.Split('~')[4];
            string sBUnitIDs = sTemp.Split('~')[5];
            double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
            double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
            string sBMMIDs = sTemp.Split('~')[8];
            string sGroupIDs = sTemp.Split('~')[9];

            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCardComp(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sVersion, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;
            #endregion

            #region Basic Information
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.Company = oCompanys.First();
            oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
            oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
            #endregion

            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F6)
            {
                rptTimeCard_F7 oReport = new rptTimeCard_F7();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }           
            return RedirectToAction("~/blank");
        }
        public ActionResult PrintFirstTimeCard(string sTemp)
        {
            _oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            //string sEmployeeIDs = sTemp.Split('~')[0];
            //DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            //DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            //int nLocationID = Convert.ToInt32(sTemp.Split('~')[3]);
            //string sDepartmentIds = sTemp.Split('~')[4];  

            string sEmployeeIDs = sTemp.Split('~')[0];
            DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLocationID = sTemp.Split('~')[3];
            string sDepartmentIds = sTemp.Split('~')[4];
            string sBUnitIDs = sTemp.Split('~')[5];
            double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
            double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);

            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, "", "", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;


            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceDaily_ZN.Company = oCompanys.First();
            _oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(_oAttendanceDaily_ZN.Company.OrganizationLogo);
            _oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
            rptFirstTimeCard oReport = new rptFirstTimeCard();
            byte[] abytes = oReport.PrepareReport(_oAttendanceDaily_ZN);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintOTTimeCard(string sTemp)
        {
            _oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            //string sEmployeeIDs = sTemp.Split('~')[0];
            //DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            //DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            //int nLocationID = Convert.ToInt32(sTemp.Split('~')[3]);
            //string sDepartmentIds = sTemp.Split('~')[4];

            string sEmployeeIDs = sTemp.Split('~')[0];
            DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLocationID = sTemp.Split('~')[3];
            string sDepartmentIds = sTemp.Split('~')[4];
            string sBUnitIDs = sTemp.Split('~')[5];
            double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
            double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);

            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, "", "", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceDaily_ZN.Company = oCompanys.First();
            _oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(_oAttendanceDaily_ZN.Company.OrganizationLogo);
            _oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
            rptOTTimeCard oReport = new rptOTTimeCard();
            byte[] abytes = oReport.PrepareReport(_oAttendanceDaily_ZN);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintFinalTimeCard(string sTemp)
        {
            _oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            //string sEmployeeIDs = sTemp.Split('~')[0];
            //DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            //DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            //int nLocationID = Convert.ToInt32(sTemp.Split('~')[3]);
            //string sDepartmentIds = sTemp.Split('~')[4];

            string sEmployeeIDs = sTemp.Split('~')[0];
            DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLocationID = sTemp.Split('~')[3];
            string sDepartmentIds = sTemp.Split('~')[4];
            string sBUnitIDs = sTemp.Split('~')[5];
            double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
            double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);

            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, "", "", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceDaily_ZN.Company = oCompanys.First();
            _oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(_oAttendanceDaily_ZN.Company.OrganizationLogo);
            _oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
            rptFinalTimeCard oReport = new rptFinalTimeCard();
            byte[] abytes = oReport.PrepareReport(_oAttendanceDaily_ZN);
            return File(abytes, "application/pdf");
        }
        //public ActionResult PrintTimeCard_F2(string sTemp, string sType)
        //{
        //    AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
        //    //string sEmployeeIDs = sTemp.Split('~')[0];
        //    //DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    //DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    //int nLocationID = Convert.ToInt32(sTemp.Split('~')[3]);
        //    //string sDepartmentIds = sTemp.Split('~')[4];

        //    string sEmployeeIDs = sTemp.Split('~')[0];
        //    DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    string sLocationID = sTemp.Split('~')[3];
        //    string sDepartmentIds = sTemp.Split('~')[4];
        //    string sBUnitIDs = sTemp.Split('~')[5];
        //    double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
        //    double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
        //    string sBMMIDs = sTemp.Split('~')[8];
        //    string sGroupIDs = sTemp.Split('~')[9];

        //    List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        //    AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

        //    if (sBUnitIDs != "")
        //    {
        //        oAttendanceDaily_ZN.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + sBUnitIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    }


        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.Company = oCompanys.First();
        //    oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
        //    oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
        //    rptTimeCard_F2 oReport = new rptTimeCard_F2();
        //    byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
        //    return File(abytes, "application/pdf");
        //}
        //public ActionResult PrintTimeCard_F4(string sTemp, string sType)
        //{
        //    AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
        //    //string sEmployeeIDs = sTemp.Split('~')[0];
        //    //DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    //DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    //int nLocationID = Convert.ToInt32(sTemp.Split('~')[3]);
        //    //string sDepartmentIds = sTemp.Split('~')[4];
        //    string sEmployeeIDs = sTemp.Split('~')[0];
        //    DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    string sLocationID = sTemp.Split('~')[3];
        //    string sDepartmentIds = sTemp.Split('~')[4];
        //    string sBUnitIDs = sTemp.Split('~')[5];
        //    double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
        //    double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
        //    string sBMMIDs = sTemp.Split('~')[8];
        //    string sGroupIDs = sTemp.Split('~')[9];

        //    List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        //    AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.Company = oCompanys.First();
        //    oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
        //    oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
        //    rptTimeCard_F4 oReport = new rptTimeCard_F4();
        //    byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
        //    return File(abytes, "application/pdf");
        //}
        //public ActionResult PrintTimeCard_F6(string sTemp)
        //{
        //    AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
        //    //string sEmployeeIDs = sTemp.Split('~')[0];
        //    //DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    //DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    //int nLocationID = Convert.ToInt32(sTemp.Split('~')[3]);
        //    //string sDepartmentIds = sTemp.Split('~')[4];

        //    string sEmployeeIDs = sTemp.Split('~')[0];
        //    DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    string sLocationID = sTemp.Split('~')[3];
        //    string sDepartmentIds = sTemp.Split('~')[4];
        //    string sBUnitIDs = sTemp.Split('~')[5];
        //    double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
        //    double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
        //    string sBMMIDs = sTemp.Split('~')[8];
        //    string sGroupIDs = sTemp.Split('~')[9];

        //    List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        //    AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.Company = oCompanys.First();
        //    oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
        //    oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
        //    rptTimeCard_F6 oReport = new rptTimeCard_F6();
        //    byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
        //    return File(abytes, "application/pdf");
        //}
        //public ActionResult PrintTimeCard_F7(string sTemp)
        //{
        //    AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();

        //    string sEmployeeIDs = sTemp.Split('~')[0];
        //    DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    string sLocationID = sTemp.Split('~')[3];
        //    string sDepartmentIds = sTemp.Split('~')[4];
        //    string sBUnitIDs = sTemp.Split('~')[5];
        //    double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
        //    double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
        //    string sBMMIDs = sTemp.Split('~')[8];
        //    string sGroupIDs = sTemp.Split('~')[9];

        //    List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        //    AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.Company = oCompanys.First();
        //    oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
        //    oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
        //    rptTimeCard_F7 oReport = new rptTimeCard_F7();
        //    byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
        //    return File(abytes, "application/pdf");
        //}

        //public ActionResult PrintTimeCard_FC7(string sTemp, string sType)
        //{
        //    AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
        //    //string sEmployeeIDs = sTemp.Split('~')[0];
        //    //DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    //DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    //int nLocationID = Convert.ToInt32(sTemp.Split('~')[3]);
        //    //string sDepartmentIds = sTemp.Split('~')[4];

        //    string sEmployeeIDs = sTemp.Split('~')[0];
        //    DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    string sLocationID = sTemp.Split('~')[3];
        //    string sDepartmentIds = sTemp.Split('~')[4];
        //    string sBUnitIDs = sTemp.Split('~')[5];
        //    double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
        //    double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
        //    string sBMMIDs = sTemp.Split('~')[8];
        //    string sGroupIDs = sTemp.Split('~')[9];

        //    List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        //    AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

        //    _oMaxOTConfiguration = MaxOTConfiguration.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.MaxOTConfiguration = _oMaxOTConfiguration;

        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.Company = oCompanys.First();
        //    oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
        //    oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
        //    rptTimeCard_FC7 oReport = new rptTimeCard_FC7();
        //    byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
        //    return File(abytes, "application/pdf");
        //}


        #endregion TimeCard
        private string MakeSQL(string sSelectString, string sEmployeeIDs, DateTime Startdate, DateTime EndDate, string sLocationID, string sDepartmentIds, string sTemp, string sBUnitIDs, double nStartSalaryRange, double nEndSalaryRange, string sBMMIDs, string sGroupIDs, int nLastID, string sOrderBySQL)
        {
            //string sSQL = "SELECT TOP (100) * FROM AttendanceDaily";
            string sSQL1 = "";

            #region Employee ID
            if (!string.IsNullOrEmpty(sEmployeeIDs) && sEmployeeIDs != "0")
            {
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " EmployeeID IN(" + sEmployeeIDs + ")";
            }
            #endregion

            #region Date
            Global.TagSQL(ref sSQL1);
            sSQL1 = sSQL1 + " (AttendanceDate>= '" + Startdate.ToString("dd MMM yyyy") + "' AND AttendanceDate < '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
            #endregion

            #region Department ID
            if (!string.IsNullOrEmpty(sDepartmentIds))
            {
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " DepartmentID IN(" + sDepartmentIds + ")";
            }
            #endregion

            #region Location ID
            if (!string.IsNullOrEmpty(sLocationID))
            {
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " LocationID IN(" + sLocationID + ")";
            }
            #endregion

            #region BusinessUnit ID
            if (!string.IsNullOrEmpty(sBUnitIDs))
            {
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " BusinessUnitID IN(" + sBUnitIDs + ")";
            }
            #endregion

            #region Salary Range
            if (nStartSalaryRange > 0 && nEndSalaryRange > 0)
            {
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " EmployeeID IN (SELECT EmployeeID FROM EmployeeSalary WHERE CompNetAmount BETWEEN " + nStartSalaryRange + " AND " + nEndSalaryRange + ")";
            }
            #endregion

            #region EmployeeGroup
            if (!string.IsNullOrEmpty(sGroupIDs))
            {
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " EmployeeID IN (SELECT EmployeeID FROM EmployeeGroup WHERE EGID IN(" + sGroupIDs + ")";
            }
            #endregion

            #region EmployeeBlock
            if (!string.IsNullOrEmpty(sBMMIDs))
            {
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " EmployeeID IN (SELECT EmployeeID FROM EmployeeGroup WHERE EmployeeTypeID IN(" + sBMMIDs + ")";
            }
            #endregion

            #region Indexing
            if (nLastID > 0)
            {
                Global.TagSQL(ref sSQL1);
                sSQL1 = sSQL1 + " AttendanceID>" + nLastID;
            }
            #endregion

            return sSelectString + sSQL1 + sOrderBySQL;
        }
        //public void Print_ReportXL(string sTemp)
        //{

        //    Company _oCompany = new Company();
        //    DateTime _dstartDate = DateTime.Now;
        //    DateTime _dEndDate = DateTime.Now;

        //    AttendanceDaily oAttendanceDaily = new AttendanceDaily();
        //    string sEmployeeIDs = sTemp.Split('~')[0];
        //    DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    string sLocationID = sTemp.Split('~')[3];
        //    string sDepartmentIds = sTemp.Split('~')[4];
        //    string sBUnitIDs = sTemp.Split('~')[5];
        //    double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
        //    double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
        //    string sBMMIDs = sTemp.Split('~')[8];
        //    string sGroupIDs = sTemp.Split('~')[9];
        //    //SELECT TOP (100) * FROM AttendanceDaily
        //    List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
        //    string sSQL = MakeSQL("SELECT COUNT(AttendanceID) AS AttendanceID FROM AttendanceDaily ", sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, 0, "");
        //    oAttendanceDailys = AttendanceDaily.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

        //    int nLoopCount = oAttendanceDailys[0].AttendanceID / 50000;
        //    double dLoopCount = oAttendanceDailys[0].AttendanceID % 50000;

        //    if (dLoopCount > 0) nLoopCount++;

        //    List<AttendanceDaily> oListAttendanceDailys = new List<AttendanceDaily>();
        //    int nLastID = 0;
        //    for (int i = 0; i < nLoopCount; i++)
        //    {
        //        oAttendanceDailys = new List<AttendanceDaily>();
        //        sSQL = MakeSQL("SELECT TOP(50000) AttendanceID, Code, AttendanceDate, CompInTime, CompOutTime, CompTotalWorkingHourInMinute, CompOverTimeInMinute, HRM_Shift FROM View_AttendanceDaily  ", sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, nLastID, " ORDER BY AttendanceID");
        //        oAttendanceDailys = AttendanceDaily.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //        nLastID = oAttendanceDailys[oAttendanceDailys.Count() - 1].AttendanceID;
        //        oListAttendanceDailys.AddRange(oAttendanceDailys);
        //    }

        //    #region Excel

        //    int rowIndex = 2;
        //    ExcelRange cell;
        //    ExcelFill fill;
        //    OfficeOpenXml.Style.Border border;
        //    ExcelPackage excelPackage = new ExcelPackage();

        //    Company oCompany = new Company();

        //    using (excelPackage = new ExcelPackage())
        //    {
        //        int nColIndex = 2;
        //        excelPackage.Workbook.Properties.Author = "ESimSol";
        //        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
        //        var sheet = excelPackage.Workbook.Worksheets.Add("TImeCard");
        //        sheet.Name = "Time Card";

        //        sheet.Column(nColIndex++).Width = 8;  //SL
        //        sheet.Column(nColIndex++).Width = 20; //Emp_ID
        //        sheet.Column(nColIndex++).Width = 20; //Date
        //        sheet.Column(nColIndex++).Width = 20; //InTime
        //        sheet.Column(nColIndex++).Width = 20; //OutTime
        //        sheet.Column(nColIndex++).Width = 20; //Duration
        //        sheet.Column(nColIndex++).Width = 20; //OverTime
        //        sheet.Column(nColIndex++).Width = 20; //Status
        //        sheet.Column(nColIndex++).Width = 20; //Shift


        //        oCompany = new Company();
        //        oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //        oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //        #region Report Header
        //        sheet.Cells[rowIndex, 2, rowIndex, nColIndex].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex++;

        //        sheet.Cells[rowIndex, 2, rowIndex, nColIndex].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Time Card"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex++;

        //        sheet.Cells[rowIndex, 2, rowIndex, nColIndex].Merge = true;
        //        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Time Card"; cell.Style.Font.Bold = true;
        //        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        rowIndex++;
        //        #endregion

        //        #region Column Header
        //        nColIndex = 2;
        //        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = "SL"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = "Employee ID"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = "Date"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = "Entry Time"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = "Exit Time"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = "Duration"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = "Over Time"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = "Status"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //        cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = "Shift"; cell.Style.Font.Bold = true; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
        //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //        #endregion

        //        #region Report Data
        //        if (oListAttendanceDailys.Count > 0)
        //        {
        //            int nSL = 1;
        //            oListAttendanceDailys = oListAttendanceDailys.OrderBy(x => x.AttendanceDate).ThenBy(x => x.EmployeeCode).ToList();

        //            foreach (AttendanceDaily oItem in oListAttendanceDailys)
        //            {
        //                nColIndex = 2;
        //                rowIndex++;

        //                cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = nSL++; cell.Style.Font.Bold = false; cell.Merge = false;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = oItem.EmployeeCode; cell.Style.Font.Bold = false; cell.Merge = false;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = oItem.AttendanceDateInString; cell.Style.Font.Bold = false; cell.Merge = false;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = oItem.CompInTimeInStringAMPM; cell.Style.Font.Bold = false; cell.Merge = false;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = oItem.CompOutTimeInStringAMPM; cell.Style.Font.Bold = false; cell.Merge = false;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = oItem.CompTotalWorkingHourSt; cell.Style.Font.Bold = false; cell.Merge = false;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = oItem.CompOverTimeInMinuteHourSt; cell.Style.Font.Bold = false; cell.Merge = false;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = "P"; cell.Style.Font.Bold = false; cell.Merge = false;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        //                cell = sheet.Cells[rowIndex, nColIndex++]; cell.Value = oItem.HRM_ShiftName; cell.Style.Font.Bold = false; cell.Merge = false;
        //                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
        //                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        //            }
        //        }
        //        #endregion

        //        Response.ClearContent();
        //        Response.BinaryWrite(excelPackage.GetAsByteArray());
        //        Response.AddHeader("content-disposition", "attachment; filename=TimeCard.xlsx");
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Flush();
        //        Response.End();
        //    }
        //    #endregion
        //}

        public void Print_ReportXL(string sTemp)
        {

            Company _oCompany = new Company();
            DateTime _dstartDate = DateTime.Now;
            DateTime _dEndDate = DateTime.Now;

            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            string sEmployeeIDs = sTemp.Split('~')[0];
            DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLocationID = sTemp.Split('~')[3];
            string sDepartmentIds = sTemp.Split('~')[4];
            string sBUnitIDs = sTemp.Split('~')[5];
            double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
            double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
            string sBMMIDs = sTemp.Split('~')[8];
            string sGroupIDs = sTemp.Split('~')[9];

            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.Company = oCompanys.First();
            oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
            oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");

            _oAttendanceDaily_ZN = oAttendanceDaily_ZN;
            _oAttendanceDaily_ZNs = oAttendanceDaily_ZN.AttendanceDaily_ZNs;
            _oCompany = oAttendanceDaily_ZN.Company;

            _dstartDate = Convert.ToDateTime(_oAttendanceDaily_ZN.ErrorMessage.Split('~')[0]);
            _dEndDate = Convert.ToDateTime(_oAttendanceDaily_ZN.ErrorMessage.Split('~')[1]);




            var results = AttendanceDaily_ZNs.GroupBy(x => new { x.EmployeeID }, (key, grp) => new
            {
                EmployeeID = key.EmployeeID,
                EmployeeCode = grp.First().EmployeeCode,
                EmployeeName = grp.First().EmployeeName,
                timeCardCount = grp.Count(),
                BusinessUnitName = grp.First().BUName,
                LocationID = grp.First().LocationID,
                LocationName = grp.First().LocationName,
                DepartmentID = grp.First().DepartmentID,
                DepartmentName = grp.First().DepartmentName,
                JoiningDateInString = grp.First().JoiningDateInString,
                DesignationName = grp.First().DesignationName,
                JoiningDate = grp.First().JoiningDate,
                timeCardList = grp

            }).OrderBy(x => x.EmployeeID).ToList();








            int nRowIndex = 2, nEndRow = 0, nStartCol = 1, nEndCol = 8;
            ExcelRange cell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;
            using (var excelPackage = new ExcelPackage())
            {

                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("TIME-CARD-F5");
                sheet.Name = "TIME-CARD-F5";

                sheet.Column(2).Width = 15;
                sheet.Column(3).Width = 15;
                sheet.Column(4).Width = 15;
                sheet.Column(5).Width = 15;
                sheet.Column(6).Width = 15;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 15;
                sheet.Column(9).Width = 15;
                sheet.Column(10).Width = 15;



                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = AttendanceDaily_ZNs[0].BUName; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = AttendanceDaily_ZNs[0].BUAddress; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;


                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "JOB CARD REPORT FROM " + _dstartDate.ToString("dd MMM yyyy") + " TO " + _dEndDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 2;




                foreach (var oItem in results)
                {

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Employee Code";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 4]; cell.Merge = true; cell.Value = oItem.EmployeeCode;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Joining Date";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    //New Join
                    string sNewOrNot = "";
                    if ((oItem.JoiningDate >= Startdate) && (oItem.JoiningDate <= EndDate))
                    {
                        sNewOrNot = " (New Join)";
                    }

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true; cell.Value = oItem.JoiningDateInString + sNewOrNot;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;


                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Name";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 4]; cell.Merge = true; ; cell.Value = oItem.EmployeeName;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Department";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true; ; cell.Value = oItem.DepartmentName;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Designation";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 4]; cell.Merge = true; cell.Value = oItem.DesignationName;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Sec";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true; cell.Value = oItem.DepartmentName;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex += 2;

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "In"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Out"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Duration"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Late"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Early"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Day"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Remark"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;


                    int colIn = 1;
                    double nPresent = 0;
                    double nAbsent = 0;
                    double nLeave = 0;
                    double nDayOff = 0;
                    double nHoliDay = 0;
                    double nCount = 0;
                    double nLateDays = 0;
                    int nLateHrs = 0;
                    double nTotalOT = 0;
                    double nEarlyDays = 0;
                    int nEarlyHrs = 0;
                    string LeaveStatusDetails = "";
                    double nAttCount = 0;
                    int nPaidLeave = 0;




                    foreach (AttendanceDaily_ZN data in oItem.timeCardList)
                    {
                        if ((data.AttStatusInString == "A"))
                            nAbsent++;
                        if ((data.InTimeInString_12hr != "-" || data.OutTimeInString_12hr != "-") && (data.IsDayOff == false))
                        {
                            nPresent++;
                        }
                        if (data.EarlyDepartureMinuteInString != "" && (data.IsDayOff == false))
                            nEarlyDays++;
                        if (data.EarlyDepartureHrSt != "-" && (data.IsDayOff == false))
                            nEarlyHrs += data.EarlyDepartureMinute;
                        if (data.LateArrivalMinuteInString != "" && (data.IsDayOff == false))
                            nLateDays++;
                        if (data.LateArrivalHourSt != "-" && (data.IsDayOff == false))
                            nLateHrs += data.LateArrivalMinute;
                        if (data.IsLeaveInString != "")
                        {
                            nLeave++;

                            if (data.IsUnPaid == false) { nPaidLeave++; }
                        }
                        if (data.IsDayOffInString != "")
                            nDayOff++;
                        if (data.IsHoliDayInString != "")
                            nHoliDay++;
                        nAttCount = oItem.timeCardList.Count();

                        string nameOfDay = data.AttendanceDate.ToString("ddd");

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = data.AttendanceDateInString; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = data.InTimeInString; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = data.OutTimeInString; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = data.TotalWorkingHourSt; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = Global.MinInHourMin(data.LateArrivalMinute); cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = data.EarlyDepartureMinuteSt; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = nameOfDay; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = data.AttStatusInString; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, colIn++]; cell.Value = data.Remark; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nRowIndex++;
                        colIn = 1;
                    }

                    nRowIndex += 1;

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Present Days"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = nPresent > 0 ? nPresent.ToString() : "-"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Absent Days"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = (nAbsent > 0) ? nAbsent.ToString() : "-"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Late Days"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = (nLateDays > 0) ? nLateDays.ToString() : "-"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Early Out Days"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = (nEarlyDays > 0) ? nEarlyDays.ToString() : "-"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 1]; cell.Value = "Leave Days"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = (nLeave > 0) ? nLeave.ToString() : "-"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Off day"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = (nDayOff > 0) ? nDayOff.ToString() : "-"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Late Hrs."; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = (nLateHrs > 0) ? Global.MinInHourMin(nLateHrs).ToString() : "-"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Early Out Mins."; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = (nEarlyHrs > 0) ? nEarlyHrs.ToString() : "-"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Holidays"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = (nHoliDay > 0) ? nHoliDay.ToString() : "-"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Total Att."; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = (nPresent + nPaidLeave + nDayOff + nHoliDay).ToString(); cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;



                    nRowIndex += 6;
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

        #region GetImage

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

        #endregion GetImage

        #region Crystal Report Time Card F2
        public ActionResult PrintTimeCardBangla_F2(string sTemp, string sType)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            string sEmployeeIDs = sTemp.Split('~')[0];
            DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLocationID = sTemp.Split('~')[3];
            string sDepartmentIds = sTemp.Split('~')[4];
            string sBUnitIDs = sTemp.Split('~')[5];
            double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
            double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
            string sBMMIDs = sTemp.Split('~')[8];
            string sGroupIDs = sTemp.Split('~')[9];


            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sType, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (sBUnitIDs != "")
            {
                oBusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + sBUnitIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (oBusinessUnits.Count > 1 || oBusinessUnits.Count <= 0)
            {
                if (AttendanceDaily_ZNs.Count > 0)
                {
                    for (int i = 0; i < AttendanceDaily_ZNs.Count - 1; i++)
                    {
                        AttendanceDaily_ZNs[i].UnitName = oCompany.Name;
                        AttendanceDaily_ZNs[i].UnitAddress = oCompany.Address;
                    }
                }
            }
            else
            {
                if (AttendanceDaily_ZNs.Count > 0)
                {
                    for (int i = 0; i < AttendanceDaily_ZNs.Count - 1; i++)
                    {
                        AttendanceDaily_ZNs[i].UnitName = oBusinessUnits[0].Name;
                        AttendanceDaily_ZNs[i].UnitAddress = oBusinessUnits[0].Address;
                    }
                }
            }

            AttendanceDaily_ZNs = AttendanceDaily_ZNs.OrderBy(x => x.EmployeeCode).ToList();
            int nSl = 0;
            int nCount = 0;
            int nTotal = AttendanceDaily_ZNs.Count;
            int loopCounter = 0;
            for (loopCounter = 0; loopCounter < AttendanceDaily_ZNs.Count - 1; loopCounter++)
            {
                ++nSl;
                ++nCount;
                AttendanceDaily_ZNs[loopCounter].SLNo = nSl.ToString();
                if ((AttendanceDaily_ZNs[loopCounter].EmployeeCode != AttendanceDaily_ZNs[loopCounter + 1].EmployeeCode) && (nTotal != nCount))
                {
                    nSl = 0;
                }
            }
            ++nSl;
            AttendanceDaily_ZNs[loopCounter].SLNo = nSl.ToString();

            if (AttendanceDaily_ZNs.Count > 0)
            {
                for (int i = 0; i < AttendanceDaily_ZNs.Count; i = i + 1)
                {
                    AttendanceDaily_ZNs[i].EndDate = EndDate.ToString("dd/MM/yyyy");
                    AttendanceDaily_ZNs[i].StartDate = Startdate.ToString("dd/MM/yyyy");
                    if (AttendanceDaily_ZNs[i].InTimeInString == "-" && AttendanceDaily_ZNs[i].OutTimeInString == "-" && AttendanceDaily_ZNs[i].IsOSD == false)
                    {
                        if (AttendanceDaily_ZNs[i].IsDayOff == true)
                        {
                            AttendanceDaily_ZNs[i].DayoffCounter = 1;
                        }
                        else if (AttendanceDaily_ZNs[i].IsHoliday == true)
                        {
                            AttendanceDaily_ZNs[i].HolidayCounter = 1;
                        }
                        else if (AttendanceDaily_ZNs[i].IsLeave == true)
                        {
                            AttendanceDaily_ZNs[i].LeaveCounter = 1;
                        }
                        else
                        {
                            AttendanceDaily_ZNs[i].AbsentCounter = 1;
                        }
                    }
                    else
                    {
                        if (AttendanceDaily_ZNs[i].IsDayOff == true)
                        {
                            AttendanceDaily_ZNs[i].DayoffCounter = 1;
                        }
                        else if (AttendanceDaily_ZNs[i].IsHoliday == true)
                        {
                            AttendanceDaily_ZNs[i].HolidayCounter = 1;
                        }
                        else if (AttendanceDaily_ZNs[i].IsLeave == true)
                        {
                            AttendanceDaily_ZNs[i].LeaveCounter = 1;
                        }
                        else
                        {
                            AttendanceDaily_ZNs[i].PresentCounter = 1;
                        }
                    }
                    if (AttendanceDaily_ZNs[i].OverTimeInMinute > 0)
                    {
                        AttendanceDaily_ZNs[i].OverTimeCounter = AttendanceDaily_ZNs[i].OverTimeInMinute;
                    }
                    if (AttendanceDaily_ZNs[i].EarlyDepartureMinute > 0)
                    {
                        //AttendanceDaily_ZNs[i].EarlyCounter = AttendanceDaily_ZNs[i].EarlyDepartureMinute;
                        AttendanceDaily_ZNs[i].EarlyCounter = 1;
                    }
                    if (AttendanceDaily_ZNs[i].LateArrivalMinute > 0)
                    {
                        //AttendanceDaily_ZNs[i].LateCounter = AttendanceDaily_ZNs[i].LateArrivalMinute;
                        AttendanceDaily_ZNs[i].LateCounter = 1;
                    }
                }
            }






            oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

            //GET Client OperationSettings
            ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
            ClientOperationSetting oCOS = new ClientOperationSetting();


            oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.BanglaFont, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (Convert.ToInt32(oTempClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Avro)
            {
                //oAttendanceDaily_ZN.AttendanceDaily_ZNs[0].BanglaFont = (int)EnumClientOperationValueFormat.Avro;
                //oCOS.ErrorMessage = Convert.ToString((int)EnumClientOperationValueFormat.Avro);
                oAttendanceDaily_ZN.AttendanceDaily_ZNs.ForEach(x => { x.BanglaFont = ""; });

            }
            else
            {
                //oAttendanceDaily_ZN.AttendanceDaily_ZNs[0].BanglaFont = (int)EnumClientOperationValueFormat.Bijoy;
                //oCOS.ErrorMessage = Convert.ToString((int)EnumClientOperationValueFormat.Bijoy);
                oAttendanceDaily_ZN.AttendanceDaily_ZNs.ForEach(x => { x.BanglaFont = EnumClientOperationValueFormat.Bijoy.ToString(); });
            }

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.Company = oCompanys.First();
            oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
            oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");


            //if (sBUnitIDs != "")
            //{
            //    oAttendanceDaily_ZN.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + sBUnitIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //}
            //if (oAttendanceDaily_ZN.BusinessUnits.Count > 1)
            //{
            //    oAttendanceDaily_ZN.AttendanceDaily_ZNs[0].CompanyName = oAttendanceDaily_ZN.Company.NameInBangla;
            //}
            //else
            //{
            //    oAttendanceDaily_ZN.AttendanceDaily_ZNs[0].CompanyName = oAttendanceDaily_ZN.BusinessUnits[0].NameInBangla;
            //}



            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "TimeCardF2Bangla.rpt"));
            rd.SetDataSource(oAttendanceDaily_ZN.AttendanceDaily_ZNs);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                TextObject txtDays = (TextObject)rd.ReportDefinition.Sections["Section5"].ReportObjects["txtDays"];
                txtDays.Text = ((EndDate - Startdate).TotalDays + 1).ToString();

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf");
            }
            catch { throw; }

        }

        #endregion

        #region AMG Worker TimeCard

        //public ActionResult PrintTimeCard_AMG(string sTemp)
        //{
        //    AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();

        //    string sEmployeeIDs = sTemp.Split('~')[0];
        //    DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    string sLocationID = sTemp.Split('~')[3];
        //    string sDepartmentIds = sTemp.Split('~')[4];
        //    string sBUnitIDs = sTemp.Split('~')[5];
        //    double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
        //    double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
        //    string sBMMIDs = sTemp.Split('~')[8];
        //    string sGroupIDs = sTemp.Split('~')[9];

        //    List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        //    AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

        //    oAttendanceDaily_ZN.LeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead", ((User)(Session[SessionInfo.CurrentUser])).UserID);

        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.Company = oCompanys.First();
        //    oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
        //    oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
        //    rptTimeCard_F6_1 oReport = new rptTimeCard_F6_1();
        //    byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
        //    return File(abytes, "application/pdf");
        //}

        public ActionResult PrintTimeCard_Worker(string sTemp)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();

            string sEmployeeIDs = sTemp.Split('~')[0];
            DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLocationID = sTemp.Split('~')[3];
            string sDepartmentIds = sTemp.Split('~')[4];
            string sBUnitIDs = sTemp.Split('~')[5];
            double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
            double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
            string sBMMIDs = sTemp.Split('~')[8];
            string sGroupIDs = sTemp.Split('~')[9];

            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

            oAttendanceDaily_ZN.LeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.Company = oCompanys.First();
            oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
            oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
            rptTimeCard_Worker oReport = new rptTimeCard_Worker();
            byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
            return File(abytes, "application/pdf");
        }
        #endregion
        private List<AttendanceDaily> GetAttendenceDailyList(DataTable oDataTable)
        {
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();

            foreach (DataRow oDataRow in oDataTable.Rows)
            {
                AttendanceDaily oRPTAttendanceDaily = new AttendanceDaily();
                oRPTAttendanceDaily.EmployeeID = oDataRow.Field<Int32>("EmployeeID");//Int32
                oRPTAttendanceDaily.EmployeeCode = oDataRow.Field<string>("Code");//string
                oRPTAttendanceDaily.EmployeeName = oDataRow.Field<string>("EmployeeName");
                oRPTAttendanceDaily.DepartmentID = oDataRow.Field<Int32>("DepartmentID");
                oRPTAttendanceDaily.DepartmentName = oDataRow.Field<string>("Department");

                //oRPTAttendanceDaily.DesignationName = oDataRow.Field<string>("Designation");
                //oRPTAttendanceDaily.TotalDays = oDataRow.Field<Int32>("TotalDays");
                oAttendanceDailys.Add(oRPTAttendanceDaily);
            }
            return oAttendanceDailys;
        }
        public void DailyAttendanceExcelReport(string sParam, double ts)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<string> columnHead = new List<string>();
            List<int> colWidth = new List<int>();

            int nCount = 0;
            DateTime dStartDate = Convert.ToDateTime(sParam.Split('~')[nCount++]);
            DateTime dEndDate = Convert.ToDateTime(sParam.Split('~')[nCount++]);
            string sBUIDs = sParam.Split('~')[nCount++];
            string sLocationIDs = sParam.Split('~')[nCount++];
            string sDepartmentIDs = sParam.Split('~')[nCount++];

            string sSQL = "SELECT HH.EmployeeID, HH.Code, HH.EmployeeName, HH.DepartmentID, HH.Department, HH.Designation FROM View_AttendanceDaily AS HH " +
                "WHERE CONVERT(DATE,CONVERT(VARCHAR(12),HH.AttendanceDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy") + "',106)) AND BusinessUnitID IN (" + sBUIDs + ") AND LocationID IN (" + sLocationIDs + ") " + (sDepartmentIDs != "" ? " AND DepartmentID IN (" + sDepartmentIDs + ")" : "") +
                          "GROUP BY HH.EmployeeID, HH.Code, HH.EmployeeName, HH.DepartmentID, HH.Department, HH.Designation " +
                          "ORDER BY HH.DepartmentID, HH.Department, HH.EmployeeID, HH.EmployeeName";

            DataSet oLoadDataSets = AttendanceDaily.GetsDataSet(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            DataTable oAttendanceDailys = oLoadDataSets.Tables[0];

            if (oAttendanceDailys.Rows.Count > 0)
            {
                #region
                int rowIndex = 2;
                int colIndex = 2;
                ExcelRange cell;
                OfficeOpenXml.Style.Border border;
                ExcelFill fill;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add(" Exe Wise Production Report");
                    sheet.Name = "  Exe Wise Production Report";

                    sheet.Column(2).Width = 6;  //SL
                    sheet.Column(3).Width = 15; //Employee Code
                    sheet.Column(4).Width = 30; //Employee Name
                    sheet.Column(5).Width = 25; //Designation                    
                    sheet.Column(6).Width = 20; //Shift Duration
                    sheet.Column(7).Width = 20; //Present Hour
                    sheet.Column(8).Width = 20; //Less/OverTime
                    sheet.Column(9).Width = 20; //Salary/Hour
                    sheet.Column(10).Width = 20;  //Gross Salary
                    sheet.Column(11).Width = 20; //Deduction Salary
                    sheet.Column(12).Width = 20; //Net Salary

                    #region Report Header
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 12]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 12]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 15;
                    cell.Value = "Attendence Report"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, 12 - 1]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 12;
                    cell.Value = "Date Range : " + dStartDate.ToString("dd MMM yyyy") + " to " + dEndDate.ToString("dd MMM yyyy"); cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    rowIndex++;
                    rowIndex++;
                    #endregion

                    #region Report Header
                    colIndex = 2;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "#SL"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "Employee Code"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "Employee Name"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "Designation"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "Shift Duration"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "Present Hour"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "Less/OverTime"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "Salary/Hour"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "Gross Salary"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "Deduction Salary"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = "Net Salary"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    rowIndex++;
                    #endregion

                    #region Body
                    nCount = 1;
                    int nDepartmentID = 0;
                    Dictionary<string, string> aGrandTotals = new Dictionary<string, string>();
                    string sStartCell = "", sEndCell = "", sFormula = "";
                    bool bIsPrint = false;
                    int nStartRow = 0, nEndRow = 0;
                    nStartRow = rowIndex;
                    foreach (DataRow oDataRow in oAttendanceDailys.Rows)
                    {
                        if (Convert.ToInt32(oDataRow["DepartmentID"]) != nDepartmentID)
                        {
                            if (bIsPrint == true)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Style.Font.Bold = true;
                                cell.Value = "Sub Total : "; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                                colIndex = 10;
                                aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + nEndRow.ToString()));
                                for (int i = colIndex; i < 13; i++)
                                {
                                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);

                                    cell = sheet.Cells[rowIndex, colIndex++]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                                }
                                rowIndex++;
                            }

                            #region DeptName
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Value = "Dept : " + Convert.ToString(oDataRow["Department"]); cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                            colIndex = 10;
                            for (int i = colIndex; i < 13; i++)
                            {
                                cell = sheet.Cells[rowIndex, colIndex++]; cell.Style.Font.Bold = false;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                            }
                            rowIndex++;
                            #endregion

                            nStartRow = rowIndex;
                        }

                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = (nCount++).ToString(); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = Convert.ToString(oDataRow["Code"]); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = Convert.ToString(oDataRow["EmployeeName"]); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = Convert.ToString(oDataRow["Designation"]); cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;


                        string sTemp = GetEmployeeTotalWorkingHour(Convert.ToInt32(oDataRow["EmployeeID"]), dStartDate, dEndDate);

                        int dEmployeeShiftDurationInMin = Convert.ToInt32(sTemp.Split('~')[0]);
                        int dEmployeeWorkingHour = Convert.ToInt32(sTemp.Split('~')[1]);

                        int dEmployeeShiftDurationInMin_Hour = dEmployeeShiftDurationInMin / 60;
                        float dEmployeeShiftDurationInMin_Min = dEmployeeShiftDurationInMin % 60;
                        double temp1 = Convert.ToDouble(dEmployeeShiftDurationInMin_Hour + "." + dEmployeeShiftDurationInMin_Min);


                        int dEmployeeWorkingHour_Hour = dEmployeeWorkingHour / 60;
                        float dEmployeeWorkingHour_Min = dEmployeeWorkingHour % 60;
                        double temp2 = Convert.ToDouble(dEmployeeWorkingHour_Hour + "." + dEmployeeWorkingHour_Min);


                        string sWorkingHourCell = Global.GetExcelCellName(rowIndex, colIndex);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = temp1; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        string sPresentHourCell = Global.GetExcelCellName(rowIndex, colIndex);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = temp2; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        string sAbsentHourCell = Global.GetExcelCellName(rowIndex, colIndex);
                        //cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Formula = "IF((" + sWorkingHourCell + "- " + sPresentHourCell + ")<0, 0, (" + sWorkingHourCell + "- " + sPresentHourCell + "))"; cell.Style.Font.Bold = false;
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Formula = "(" + sPresentHourCell + "- " + sWorkingHourCell + ")"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        string sSalaryPerHour = Global.GetExcelCellName(rowIndex, colIndex);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Value = 0; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        string sGrossCell = Global.GetExcelCellName(rowIndex, colIndex);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Formula = "(" + sSalaryPerHour + "* " + sWorkingHourCell + ")"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        string sDeductionCell = Global.GetExcelCellName(rowIndex, colIndex);
                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Style.Font.Bold = false; /*cell.Formula = "(" + sSalaryPerHour + "* " + sAbsentHourCell + ")";*/
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Merge = false; cell.Formula = "(" + sGrossCell + "-" + sDeductionCell + ")"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White); cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                        nEndRow = rowIndex;
                        rowIndex++;
                        nDepartmentID = Convert.ToInt32(oDataRow["DepartmentID"]);
                        bIsPrint = true;
                    }

                    #endregion

                    #region Subtotal Last
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Value = "Sub Total : "; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    colIndex = 10;
                    aGrandTotals.Add((aGrandTotals.Count + 1).ToString(), (nStartRow.ToString() + "," + nEndRow.ToString()));
                    for (int i = colIndex; i < 13; i++)
                    {
                        sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                        sEndCell = Global.GetExcelCellName(nEndRow, colIndex);

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    }
                    rowIndex++;
                    colIndex = 2;
                    #endregion

                    #region Grand Total Formula
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 9]; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Value = "Grand Total : "; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    colIndex = 10;
                    for (int x = colIndex; x < 13; x++)
                    {
                        #region Formula
                        sFormula = "";
                        if (aGrandTotals.Count > 0)
                        {
                            sFormula = "SUM(";
                            for (int i = 1; i <= aGrandTotals.Count; i++)
                            {
                                nStartRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[0]);
                                nEndRow = Convert.ToInt32(Convert.ToString(aGrandTotals[i.ToString()]).Split(',')[1]);
                                sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                                sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                                sFormula = sFormula + sStartCell + ":" + sEndCell + ",";
                            }
                            if (sFormula.Length > 0)
                            {
                                sFormula = sFormula.Remove(sFormula.Length - 1, 1);
                            }
                            sFormula = sFormula + ")";
                        }
                        else
                        {
                            sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                            sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                            sFormula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                        }
                        #endregion

                        cell = sheet.Cells[rowIndex, colIndex++]; cell.Style.Font.Bold = false; cell.Formula = sFormula;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    }
                    rowIndex++;
                    //colIndex = 2;

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else
            {
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sales & Marketing");
                    sheet.Name = "Sales & Marketing";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = "No Data found"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Buyer-MonthWiseSalesReport.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }

            }
        }

        private string GetEmployeeTotalWorkingHour(int nEmployeeID, DateTime dStartDate, DateTime dEndDate)
        {
            List<AttendanceDaily_ZN> oAttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            oAttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(nEmployeeID.ToString(), dStartDate, dEndDate, "", "", "", "", 0.00, 0.00, "", "", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            dEndDate = dEndDate.AddDays(1);
            int _nCount = 0;
            int _dEmployeeShiftDurationInMin = 0;
            int _dEmployeeWorkingHour = 0;
            while (dStartDate.ToString("dd MMM yyyy") != dEndDate.ToString("dd MMM yyyy"))
            {
                _nCount++;
                AttendanceDaily_ZN oTempAttendanceDaily_ZN = new AttendanceDaily_ZN();
                oTempAttendanceDaily_ZN = oAttendanceDaily_ZNs.Where(x => x.AttendanceDate.ToString("dd MMM yyyy") == dStartDate.ToString("dd MMM yyyy")).ToList().FirstOrDefault();

                if (oTempAttendanceDaily_ZN != null && oTempAttendanceDaily_ZN.AttendanceID > 0)
                {
                    if (oTempAttendanceDaily_ZN.IsLeave == true)
                    {
                        if (oTempAttendanceDaily_ZN.LeaveType == EnumLeaveType.Full)
                        {
                            _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + 0;
                        }
                        else if (oTempAttendanceDaily_ZN.LeaveType == EnumLeaveType.Short)
                        {
                            TimeSpan tLeaveDuration = TimeSpan.FromMinutes(oTempAttendanceDaily_ZN.LeaveDuration);
                            TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);
                            span = span.Subtract(span - tLeaveDuration);                            
                            if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                            {
                                _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                            }
                        }
                        else if (oTempAttendanceDaily_ZN.LeaveType == EnumLeaveType.Half)
                        {
                            TimeSpan tLeaveDuration = TimeSpan.FromMinutes(oTempAttendanceDaily_ZN.LeaveDuration);
                            TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);
                            span = span - tLeaveDuration;                            
                            if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                            {
                                _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                            }
                        }
                    }
                    else if (oTempAttendanceDaily_ZN.AttStatusInString.Contains("HD") || oTempAttendanceDaily_ZN.AttStatusInString.Contains("Off") || oTempAttendanceDaily_ZN.AttStatusInString.Contains("SL"))
                    {
                        if (oTempAttendanceDaily_ZN.InTimeInString == "-")
                        {
                            _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + 0;
                        }
                        else
                        {
                            if (oTempAttendanceDaily_ZN.ShiftStartTime < oTempAttendanceDaily_ZN.ShiftEndTime)
                            {
                                TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);                                
                                if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                                {
                                    _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                                }
                            }
                            else if (oTempAttendanceDaily_ZN.ShiftStartTime > oTempAttendanceDaily_ZN.ShiftEndTime)
                            {                                
                                TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);                                
                                if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                                {
                                    _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                                }
                            }
                        }
                    }
                    else if (oTempAttendanceDaily_ZN.ShiftStartTime < oTempAttendanceDaily_ZN.ShiftEndTime)
                    {
                        TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);                        
                        if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                        {
                            _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                        }
                    }
                    else if (oTempAttendanceDaily_ZN.ShiftStartTime > oTempAttendanceDaily_ZN.ShiftEndTime)
                    {                        
                        TimeSpan span = GetTimeDiff(oTempAttendanceDaily_ZN);                        
                        if (oTempAttendanceDaily_ZN.IsHoliday == false && oTempAttendanceDaily_ZN.IsDayOff == false)
                        {
                            _dEmployeeShiftDurationInMin = _dEmployeeShiftDurationInMin + span.Hours * 60 + span.Minutes;
                        }
                    }
                }
                dStartDate = dStartDate.AddDays(1);
                if (oTempAttendanceDaily_ZN!=null)
                _dEmployeeWorkingHour = _dEmployeeWorkingHour + oTempAttendanceDaily_ZN.TotalWorkingHourInMinute;
            }
            return _dEmployeeShiftDurationInMin.ToString() + '~' + _dEmployeeWorkingHour.ToString() + '~';
        }

        private TimeSpan GetTimeDiff(AttendanceDaily_ZN obj)
        {
            TimeSpan span = new TimeSpan();
            if (obj.ShiftStartTime < obj.ShiftEndTime)
            {
                span = obj.ShiftEndTime.Subtract(obj.ShiftStartTime);
            }
            else if (obj.ShiftStartTime > obj.ShiftEndTime)
            {
                int tMinuteDifference = (24 * 60) - ((obj.ShiftStartTime.Hour * 60) + obj.ShiftStartTime.Minute) + ((obj.ShiftEndTime.Hour * 60) + obj.ShiftEndTime.Minute);
                span = TimeSpan.FromMinutes(tMinuteDifference);
            }
            return span;
        }
    }
}
