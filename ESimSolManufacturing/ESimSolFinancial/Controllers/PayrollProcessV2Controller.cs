using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Data;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.Reports;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net.Mail;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using ESimSolFinancial.Hubs;
using System.Threading;

namespace ESimSolFinancial.Controllers
{
    public class PayrollProcessV2Controller : Controller
    {

        #region Declaration
        PayrollProcessManagement _oPayrollProcessManagement = new PayrollProcessManagement();
        List<PayrollProcessManagement> _oPayrollProcessManagements = new List<PayrollProcessManagement>();

        CompliancePayrollProcessManagement _oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
        List<CompliancePayrollProcessManagement> _oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
        #endregion

        #region Views PayrollProcess
        public ActionResult View_PayrollProcesss(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPayrollProcessManagements = new List<PayrollProcessManagement>();
            List<PayrollProcessManagementObject> _oPayrollProcessManagementObjectTemps = new List<PayrollProcessManagementObject>();
            string sSQL = "select * from View_PayrollProcessManagement WHERE Status=1";
            _oPayrollProcessManagements = PayrollProcessManagement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oPayrollProcessManagements = _oPayrollProcessManagements.OrderByDescending(x => x.ProcessDate).ToList();
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();

            return View(_oPayrollProcessManagements);
        }
        public ActionResult View_PayrollProcess(string sid, string sMsg)
        {
            AttendanceProcessManagement oAPM = new AttendanceProcessManagement();

            List<Location> oLocations = new List<Location>();
            List<Department> oDepartments = new List<Department>();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            List<MaxOTConfiguration> oMaxOTConfigurations = new List<MaxOTConfiguration>();
            oMaxOTConfigurations = MaxOTConfiguration.Gets(true, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "";
            if (((ESimSol.BusinessObjects.User)Session[SessionInfo.CurrentUser]).FinancialUserType == EnumFinancialUserType.GroupAccounts)
            {
                sSQL = "SELECT * FROM View_Location AS LOC WHERE LOC.LocationID !=1 AND  LOC.LocationID IN (SELECT DRP.LocationID FROM DepartmentRequirementPolicy AS DRP) ORDER BY LOC.Name ASC";
                oLocations = Location.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM Department AS Dept WHERE Dept.DepartmentID !=1 AND  Dept.DepartmentID IN (SELECT DRP.DepartmentID FROM DepartmentRequirementPolicy AS DRP) ORDER BY Dept.Name ASC";
                oDepartments = Department.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_BusinessUnit AS BU WHERE BU.BusinessUnitID IN (SELECT DRP.BusinessUnitID FROM DepartmentRequirementPolicy AS DRP) ORDER BY BU.BusinessUnitID ASC";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                sSQL = "SELECT * FROM View_Location AS LOC WHERE LOC.LocationID !=1 AND LOC.LocationID IN (SELECT DRP.LocationID FROM DepartmentRequirementPolicy AS DRP WHERE DRP.DepartmentRequirementPolicyID IN (SELECT DRPP.DRPID FROM DepartmentRequirementPolicyPermission AS DRPP WHERE DRPP.UserID =" + ((int)Session[SessionInfo.currentUserID]).ToString() + "))";
                oLocations = Location.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM Department AS Dept WHERE Dept.DepartmentID !=1 AND Dept.DepartmentID IN (SELECT DRP.DepartmentID FROM DepartmentRequirementPolicy AS DRP WHERE DRP.DepartmentRequirementPolicyID IN (SELECT DRPP.DRPID FROM DepartmentRequirementPolicyPermission AS DRPP WHERE DRPP.UserID =" + ((int)Session[SessionInfo.currentUserID]).ToString() + "))";
                oDepartments = Department.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_BusinessUnit AS BU WHERE BU.BusinessUnitID IN (SELECT DRP.BusinessUnitID FROM DepartmentRequirementPolicy AS DRP WHERE DRP.DepartmentRequirementPolicyID IN (SELECT DRPP.DRPID FROM DepartmentRequirementPolicyPermission AS DRPP WHERE DRPP.UserID =" + ((int)Session[SessionInfo.currentUserID]).ToString() + "))";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            oAPM = new AttendanceProcessManagement();
            oAPM.BusinessUnits = oBusinessUnits;
            oAPM.Locations = oLocations;
            oAPM.Departments = oDepartments;
            ViewBag.TimeCards = oMaxOTConfigurations;
            return View(oAPM);

        }


        [HttpPost]
        public JsonResult PayrollProcess(CompliancePayrollProcessManagement oCompliancePayrollProcessManagement)
        {
            List<CompliancePayrollProcessManagement> _oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCompliancePayrollProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Common Functions
        private List<EnumObject> GetYears()
        {
            List<EnumObject> oYears = new List<EnumObject>();
            EnumObject oYear = new EnumObject();
            oYear.id = 2014;
            oYear.Value = "2014";
            oYears.Add(oYear);

            for (int nYear = 2015; nYear <= (Convert.ToInt32(DateTime.Today.ToString("yyyy")) + 2); nYear++)
            {
                oYear = new EnumObject();
                oYear.id = nYear;
                oYear.Value = nYear.ToString();
                oYears.Add(oYear);
            }
            return oYears;
        }
        #endregion

        #region Views CompliancePayrollProcess
        public ActionResult View_CompPayrollProcesss(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            ViewBag.MenuID = menuid.ToString();
            List<MaxOTConfiguration> oMaxOTConfigurations = new List<MaxOTConfiguration>();
            oMaxOTConfigurations = MaxOTConfiguration.Gets(true, (int)Session[SessionInfo.currentUserID]);
            _oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
            ViewBag.TimeCards = oMaxOTConfigurations;
            ViewBag.Months = EnumObject.jGets(typeof(EnumMonth));
            ViewBag.Years = GetYears();
            return View(_oCompliancePayrollProcessManagements);
        }
        public ActionResult View_CompPayrollProcess(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            CompliancePayrollProcessManagement oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();

            List<Location> oLocations = new List<Location>();
            List<Department> oDepartments = new List<Department>();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            List<MaxOTConfiguration> oMaxOTConfigurations = new List<MaxOTConfiguration>();
            oMaxOTConfigurations = MaxOTConfiguration.Gets(true, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "";
            if (((ESimSol.BusinessObjects.User)Session[SessionInfo.CurrentUser]).FinancialUserType == EnumFinancialUserType.GroupAccounts)
            {
                sSQL = "SELECT * FROM View_Location AS LOC WHERE LOC.LocationID !=1 AND  LOC.LocationID IN (SELECT DRP.LocationID FROM DepartmentRequirementPolicy AS DRP) ORDER BY LOC.Name ASC";
                oLocations = Location.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM Department AS Dept WHERE Dept.DepartmentID !=1 AND  Dept.DepartmentID IN (SELECT DRP.DepartmentID FROM DepartmentRequirementPolicy AS DRP) ORDER BY Dept.Name ASC";
                oDepartments = Department.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_BusinessUnit AS BU WHERE BU.BusinessUnitID IN (SELECT DRP.BusinessUnitID FROM DepartmentRequirementPolicy AS DRP) ORDER BY BU.BusinessUnitID ASC";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                sSQL = "SELECT * FROM View_Location AS LOC WHERE LOC.LocationID !=1 AND LOC.LocationID IN (SELECT DRP.LocationID FROM DepartmentRequirementPolicy AS DRP WHERE DRP.DepartmentRequirementPolicyID IN (SELECT DRPP.DRPID FROM DepartmentRequirementPolicyPermission AS DRPP WHERE DRPP.UserID =" + ((int)Session[SessionInfo.currentUserID]).ToString() + "))";
                oLocations = Location.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM Department AS Dept WHERE Dept.DepartmentID !=1 AND Dept.DepartmentID IN (SELECT DRP.DepartmentID FROM DepartmentRequirementPolicy AS DRP WHERE DRP.DepartmentRequirementPolicyID IN (SELECT DRPP.DRPID FROM DepartmentRequirementPolicyPermission AS DRPP WHERE DRPP.UserID =" + ((int)Session[SessionInfo.currentUserID]).ToString() + "))";
                oDepartments = Department.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_BusinessUnit AS BU WHERE BU.BusinessUnitID IN (SELECT DRP.BusinessUnitID FROM DepartmentRequirementPolicy AS DRP WHERE DRP.DepartmentRequirementPolicyID IN (SELECT DRPP.DRPID FROM DepartmentRequirementPolicyPermission AS DRPP WHERE DRPP.UserID =" + ((int)Session[SessionInfo.currentUserID]).ToString() + "))";
                oBusinessUnits = BusinessUnit.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
            oCompliancePayrollProcessManagement.BusinessUnits = oBusinessUnits;
            oCompliancePayrollProcessManagement.Locations = oLocations;
            oCompliancePayrollProcessManagement.Departments = oDepartments;
            ViewBag.TimeCards = oMaxOTConfigurations;
            ViewBag.Months = EnumObject.jGets(typeof(EnumMonth));
            ViewBag.Years = GetYears();
            return View(oCompliancePayrollProcessManagement);

        }



        [HttpPost]
        public JsonResult SearchCompPayroll(CompliancePayrollProcessManagement oCPPM)
        {
            int nCount = 0;
            int nMonthID = Convert.ToInt32(oCPPM.ErrorMessage.Split('~')[nCount++]);
            string sYearID = Convert.ToString(oCPPM.ErrorMessage.Split('~')[nCount++]);
            int nTimeCard = Convert.ToInt32(oCPPM.ErrorMessage.Split('~')[nCount++]);
            string sBUIDs = Convert.ToString(oCPPM.ErrorMessage.Split('~')[nCount++]);
            string sLocIDs = Convert.ToString(oCPPM.ErrorMessage.Split('~')[nCount++]);
            string sDeptIDs = Convert.ToString(oCPPM.ErrorMessage.Split('~')[nCount++]);

            string sSQL = "SELECT * FROM View_CompliancePayrollProcessManagement AS CPPM WITH(NOLOCK) WHERE CPPM.MonthID = " + nMonthID + " AND  CPPM.YearID=" + sYearID + " AND CPPM.MOCID=" + nTimeCard;
            if (!string.IsNullOrEmpty(sBUIDs))
            {
                sSQL += " AND CPPM.BusinessUnitID IN(" + sBUIDs + ")";
            }
            if (!string.IsNullOrEmpty(sLocIDs))
            {
                sSQL += " AND CPPM.LocationID IN(" + sLocIDs + ")";
            }
            if (!string.IsNullOrEmpty(sDeptIDs))
            {
                sSQL += " AND CPPM.DepartmentID IN(" + sDeptIDs + ")";
            }

            sSQL = sSQL + " ORDER BY  CPPM.BusinessUnitID, CPPM.LocName, CPPM.DeptName ASC";

            List<CompliancePayrollProcessManagement> oCPPMs = new List<CompliancePayrollProcessManagement>();
            try
            {
                oCPPMs = CompliancePayrollProcessManagement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oCPPMs[0] = new CompliancePayrollProcessManagement();
                oCPPMs[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCPPMs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<CompliancePayrollProcessManagement> GetsDepartmentWiseEmpBatchs(List<CompliancePayrollProcessManagement> oCompliancePayrollProcessManagements, int nBUID, int nLocationID, bool bIsSingleDept)
        {
            List<CompliancePayrollProcessManagement> oTempCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
            foreach (CompliancePayrollProcessManagement oItem in oCompliancePayrollProcessManagements)
            {
                if (oItem.BusinessUnitID == nBUID && oItem.LocationID == nLocationID)
                {
                    if (bIsSingleDept == true)
                    {
                        if (oItem.EmpCount >= 100)
                        {
                            oTempCompliancePayrollProcessManagements.Add(oItem);
                        }
                    }
                    else
                    {
                        if (oItem.EmpCount < 100 && oItem.EmpCount > 0)
                        {
                            oTempCompliancePayrollProcessManagements.Add(oItem);
                        }
                    }
                }
            }
            return oTempCompliancePayrollProcessManagements;
        }

        [HttpPost]
        public JsonResult CompPayrollProcess(CompliancePayrollProcessManagement oCPPM)
        {
            _oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
            _oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
            List<CompliancePayrollProcessManagement> oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
            try
            {

                double nTotalBatchCount = 0;
                oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
                oCompliancePayrollProcessManagements = CompliancePayrollProcessManagement.GetsArchiveEmployeeBatchs(oCPPM, (int)Session[SessionInfo.currentUserID]);
                if (oCompliancePayrollProcessManagements.Count <= 0)
                {
                    oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
                    oCompliancePayrollProcessManagements = CompliancePayrollProcessManagement.GetsRunningEmployeeBatchs(oCPPM, (int)Session[SessionInfo.currentUserID]);
                }

                #region Manage Process Batch
                string sDepartmentIDs = ""; int nEmpCount = 0; int nDeptCount = 0;
                List<CompliancePayrollProcessManagement> oTempCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
                foreach (BusinessUnit oBU in oCPPM.BusinessUnits)
                {
                    foreach (Location oLocation in oCPPM.Locations)
                    {
                        #region Single Department Employee Batchs In-Case of Emp Count >= 100
                        oTempCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
                        oTempCompliancePayrollProcessManagements = this.GetsDepartmentWiseEmpBatchs(oCompliancePayrollProcessManagements, oBU.BusinessUnitID, oLocation.LocationID, true);
                        foreach (CompliancePayrollProcessManagement oItem in oTempCompliancePayrollProcessManagements)
                        {
                            _oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
                            _oCompliancePayrollProcessManagement.BusinessUnitID = oItem.BusinessUnitID;
                            _oCompliancePayrollProcessManagement.BUName = oItem.BUName;
                            _oCompliancePayrollProcessManagement.LocationID = oItem.LocationID;
                            _oCompliancePayrollProcessManagement.LocName = oItem.LocName;
                            _oCompliancePayrollProcessManagement.DepartmentID = oItem.DepartmentID;
                            _oCompliancePayrollProcessManagement.DeptName = oItem.DeptName + "(" + oItem.EmpCount + ")";
                            _oCompliancePayrollProcessManagement.EmpCount = oItem.EmpCount;
                            _oCompliancePayrollProcessManagements.Add(_oCompliancePayrollProcessManagement);
                        }
                        #endregion
                        
                        #region Multi Departments Employee Batchs In-Case of Emp Count < 100
                        sDepartmentIDs = "";
                        nEmpCount = 0;
                        nDeptCount = 0;
                        oTempCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
                        oTempCompliancePayrollProcessManagements = this.GetsDepartmentWiseEmpBatchs(oCompliancePayrollProcessManagements, oBU.BusinessUnitID, oLocation.LocationID, false);

                        if (oTempCompliancePayrollProcessManagements.Count > 0)
                        {
                            foreach (CompliancePayrollProcessManagement oItem in oTempCompliancePayrollProcessManagements)
                            {
                                sDepartmentIDs = sDepartmentIDs + oItem.DepartmentID.ToString() + ",";
                                nEmpCount = nEmpCount + oItem.EmpCount;
                                nDeptCount = nDeptCount + 1;
                            }

                            if (sDepartmentIDs.Length > 0)
                            {
                                sDepartmentIDs = sDepartmentIDs.Remove(sDepartmentIDs.Length - 1, 1);
                            }
                            _oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
                            _oCompliancePayrollProcessManagement.BusinessUnitID = oBU.BusinessUnitID;
                            _oCompliancePayrollProcessManagement.BUName = oBU.ShortName;
                            _oCompliancePayrollProcessManagement.LocationID = oLocation.LocationID;
                            _oCompliancePayrollProcessManagement.LocName = oLocation.Name;
                            _oCompliancePayrollProcessManagement.DeptCode = sDepartmentIDs;
                            _oCompliancePayrollProcessManagement.DeptName = "Others " + nDeptCount.ToString() + " Departments(" + nEmpCount + ")";
                            _oCompliancePayrollProcessManagement.EmpCount = nEmpCount;
                            _oCompliancePayrollProcessManagements.Add(_oCompliancePayrollProcessManagement);
                        }
                        #endregion
                    }
                }
                #endregion

                nTotalBatchCount = _oCompliancePayrollProcessManagements.Count;
                if (nTotalBatchCount <= 0)
                {
                    nTotalBatchCount = 1;
                }


                double nPercentCount = 5;
                Thread.Sleep(100);
                ProgressHub.SendMessage("Process : ", nPercentCount, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(500);

                double nPerUnitPercentCount = (95.00 / nTotalBatchCount);

                string sDeptIDs = "";
                oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
                foreach (CompliancePayrollProcessManagement oItem in _oCompliancePayrollProcessManagements)
                {
                    sDeptIDs = "";
                    if (oItem.DepartmentID > 0)
                    {
                        sDeptIDs = oItem.DepartmentID.ToString();
                    }
                    else
                    {
                        sDeptIDs = oItem.DeptCode;
                    }

                    _oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
                    nPercentCount = nPercentCount + nPerUnitPercentCount;
                    ProgressHub.SendMessage("Salary Process  BU : " + oItem.BUName + ", Location : " + oItem.LocName + ", Dept : " + oItem.DeptName, nPercentCount, (int)Session[SessionInfo.currentUserID]);
                    _oCompliancePayrollProcessManagement = CompliancePayrollProcessManagement.CompPayRollProcess(oCPPM, oItem.BusinessUnitID.ToString(), oItem.LocationID.ToString(), sDeptIDs, (int)Session[SessionInfo.currentUserID]);
                    Thread.Sleep(100);
                    if (_oCompliancePayrollProcessManagement.ErrorMessage != "")
                    {
                        _oCompliancePayrollProcessManagement.ErrorMessage = "BU Name :" + oItem.BUName + ", Location : " + oItem.LocName + ", Dept : " + oItem.DeptName + ", Invalid Operation for " + _oCompliancePayrollProcessManagement.ErrorMessage;
                        oCompliancePayrollProcessManagements.Add(_oCompliancePayrollProcessManagement);
                    }
                }

                ProgressHub.SendMessage("Finishing Payroll process", 100, (int)Session[SessionInfo.currentUserID]);
                ProgressHub.SendMessage("Finishing Payroll process", 100, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                oCompliancePayrollProcessManagements = new List<CompliancePayrollProcessManagement>();
                _oCompliancePayrollProcessManagement = new CompliancePayrollProcessManagement();
                _oCompliancePayrollProcessManagement.ErrorMessage = ex.Message;
                oCompliancePayrollProcessManagements.Add(_oCompliancePayrollProcessManagement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oCompliancePayrollProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteCompPayrollProcess(CompliancePayrollProcessManagement oCPPM)
        {
            string sFeedBackMessage = ""; 
            try
            {
                sFeedBackMessage = CompliancePayrollProcessManagement.DeleteCompPayRollProcess(oCPPM.PPMIDs, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {   
                sFeedBackMessage = ex.Message;                
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}