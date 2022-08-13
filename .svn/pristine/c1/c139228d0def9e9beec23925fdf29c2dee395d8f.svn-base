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

namespace ESimSolFinancial.Controllers
{
    public class PayrollProcessController : Controller
    {
        #region Actual Salary
        #region Declaration
        PayrollProcessManagement _oPayrollProcessManagement;
        private List<PayrollProcessManagement> _oPayrollProcessManagements;
        SalaryCorrection _oSalaryCorrection;
        private List<SalaryCorrection> _oSalaryCorrections;
        private List<PayrollProcessManagementObject> _oPayrollProcessManagementObjects;
        //string ConnStringRT = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\RTAttendanceData\\UNIS.mdb; Jet OLEDB:Database Password=unisamho;";
        #endregion

        #region Views
        public ActionResult View_PayrollProcesss(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPayrollProcessManagements = new List<PayrollProcessManagement>();
            _oPayrollProcessManagementObjects = new List<PayrollProcessManagementObject>();
            List<PayrollProcessManagementObject> _oPayrollProcessManagementObjectTemps = new List<PayrollProcessManagementObject>();
            string sSQL = "select * from View_PayrollProcessManagement WHERE Status=1";
            _oPayrollProcessManagements = PayrollProcessManagement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oPayrollProcessManagements = _oPayrollProcessManagements.OrderByDescending(x => x.ProcessDate).ToList();
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();

            return View(_oPayrollProcessManagements);
        }
        public ActionResult View_PayrollProcess(string sid, string sMsg)
        {
            int nPPMID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oPayrollProcessManagement = new PayrollProcessManagement();
            if (nPPMID > 0)
            {
                _oPayrollProcessManagement = PayrollProcessManagement.Get(nPPMID, (int)Session[SessionInfo.currentUserID]);
            }

            //string sSQL_DRP = "SELECT * FROM View_DepartmentRequirementPolicy WHERE DepartmentID IN (SELECT DepartmentID FROM Department WHERE IsActive=1)";
            //_oPayrollProcessManagement.DepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets(sSQL_DRP,(int)Session[SessionInfo.currentUserID]);
            //_oPayrollProcessManagement.DepartmentRequirementPolicys = _oPayrollProcessManagement.DepartmentRequirementPolicys.GroupBy(x => x.DepartmentID).Select(grp => grp.First()).ToList();

            string sSQL_SS = "SELECT * FROM SalaryScheme WHERE IsActive=1";
            _oPayrollProcessManagement.SalarySchemes = SalaryScheme.Gets(sSQL_SS, (int)Session[SessionInfo.currentUserID]);

            string sSQL_SH = "SELECT * FROM SalaryHead WHERE IsActive=1 AND IsProcessDependent=1";
            _oPayrollProcessManagement.SalaryHeads = SalaryHead.Gets(sSQL_SH, (int)Session[SessionInfo.currentUserID]);

            _oPayrollProcessManagement.EmployeeGroups = EmployeeGroup.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, (int)Session[SessionInfo.currentUserID]);

            //_oPayrollProcessManagement.Locations = Location.Gets((int)Session[SessionInfo.currentUserID]);

            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            List<DepartmentRequirementPolicy> oDRPs = new List<DepartmentRequirementPolicy>();
            oDRPs = DepartmentRequirementPolicy.Gets((int)Session[SessionInfo.currentUserID]);
            oDRPs = oDRPs.GroupBy(x => x.LocationID).Select(x => x.First()).ToList();
            ViewBag.Locations = oDRPs;

            ViewBag.EmployeeGroups = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BankAccounts = BankAccount.Gets((int)Session[SessionInfo.currentUserID]);

            List<PayrollProcessManagementObject> oPPMOs = new List<PayrollProcessManagementObject>();
            oPPMOs = PayrollProcessManagementObject.Gets("SELECT * FROM PayrollProcessManagementObject WHERE PPMID=" + nPPMID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.PPMobjects = oPPMOs;

            return View(_oPayrollProcessManagement);
        }


        public ActionResult View_SalaryCorrection(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oSalaryCorrections = new List<SalaryCorrection>();
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();
            
            ViewBag.EmployeeTypes = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.EmployeeType, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Shifts = HRMShift.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.EmployeeCSs = Enum.GetValues(typeof(EnumEmployeeCardStatus)).Cast<EnumEmployeeCardStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeWSs = Enum.GetValues(typeof(EnumEmployeeWorkigStatus)).Cast<EnumEmployeeWorkigStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.EmployeeCategorys = Enum.GetValues(typeof(EnumEmployeeCategory)).Cast<EnumEmployeeCategory>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            string sSql = "";
            sSql = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSql = sSql + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + (int)Session[SessionInfo.currentUserID] + " )";
            }
            sSql = sSql + ")";

            ViewBag.BusinessUnits = BusinessUnit.Gets(sSql, (int)(Session[SessionInfo.currentUserID]));
            
            return View(_oSalaryCorrections);
        }

        #endregion

        #region IUD
        [HttpPost]
        public JsonResult PayrollProcess_IUD_V1(PayrollProcessManagement oPayrollProcessManagement)
        {
            try
            {
                string sBaseAddress = "";
                string sSQL = "SELECT top(1)* FROM View_Company";// WHERE CompanyID=" + ((User)Session[SessionInfo.CurrentUser]).CompanyID;
                List<Company> oCompanys = new List<Company>();
                oCompanys = Company.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sBaseAddress = oCompanys[0].BaseAddress;

                int nIndex = 0;
                int nNewIndex = 1;
                string sEmployeeIDs = "";
                oPayrollProcessManagement.MonthID = (EnumMonth)oPayrollProcessManagement.MonthIDInt;
                oPayrollProcessManagement.CompanyID = 1;
                oPayrollProcessManagement.Status = EnumProcessStatus.Processed;
                oPayrollProcessManagement.PaymentCycle = EnumPaymentCycle.Monthly;
                oPayrollProcessManagement.ProcessDate = DateTime.Now;
                oPayrollProcessManagement = oPayrollProcessManagement.IUD_V1((int)Session[SessionInfo.currentUserID]);

                if (oPayrollProcessManagement.PPMID > 0)
                {
                    if (sEmployeeIDs == null) { sEmployeeIDs = ""; }
                    if (oPayrollProcessManagement.Status == EnumProcessStatus.Processed || oPayrollProcessManagement.Status == EnumProcessStatus.ReProcessed)
                    {
                        if (sBaseAddress == "zn")
                        {
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Production(nIndex, oPayrollProcessManagement.PPMID, sEmployeeIDs, (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }
                        }
                        else if (sBaseAddress == "mamiya")
                        {
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Mamiya(nIndex, oPayrollProcessManagement.PPMID, "", (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }
                        }
                        else
                        {
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Corporate(nIndex, oPayrollProcessManagement.PPMID, "", (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }

                            //For Discontinue
                            nNewIndex = 1;
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Corporate_Discontinue(nIndex, oPayrollProcessManagement.PPMID, "", (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oPayrollProcessManagement = new PayrollProcessManagement();
                oPayrollProcessManagement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPayrollProcessManagement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ContinueProcess(PayrollProcessManagement oPayrollProcessManagement)
        {
            string sBaseAddress = "";
            string sSQL = "SELECT top(1)* FROM View_Company";// WHERE CompanyID=" + ((User)Session[SessionInfo.CurrentUser]).CompanyID;
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sBaseAddress = oCompanys[0].BaseAddress;

            string sEmployeeIDs = "";

            try
            {
                int nNewIndex = 1;

                string sSql = "SELECT COUNT(EmployeeSalaryID) FROM EmployeeSalary WHERE PayrollProcessID=" + oPayrollProcessManagement.PPMID;
                int nIndex = PayrollProcessManagement.CheckPayrollProcess(sSql, (int)Session[SessionInfo.currentUserID]);


                if (oPayrollProcessManagement.PPMID > 0)
                {
                    if (sEmployeeIDs == null) { sEmployeeIDs = ""; }
                    if (oPayrollProcessManagement.Status == EnumProcessStatus.Processed || oPayrollProcessManagement.Status == EnumProcessStatus.ReProcessed)
                    {
                        if (sBaseAddress == "zn")
                        {
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Production(nIndex, oPayrollProcessManagement.PPMID, sEmployeeIDs, (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }
                        }
                        else if (sBaseAddress == "mamiya")
                        {
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Mamiya(nIndex, oPayrollProcessManagement.PPMID, "", (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }
                        }
                        else
                        {
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Corporate(nIndex, oPayrollProcessManagement.PPMID, "", (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }

                            //For Discontinue
                            nNewIndex = 1;
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Corporate_Discontinue(nIndex, oPayrollProcessManagement.PPMID, "", (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oPayrollProcessManagement = new PayrollProcessManagement();
                oPayrollProcessManagement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPayrollProcessManagement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Salary Correction

        [HttpPost]
        public JsonResult SearchSalaryCorrection(string sBU, string sLocationID, int nMonthID, int nYear, int nRowLength, int nLoadRecords, string sEmployeeIDs)
        {
            List<SalaryCorrection> oSalaryCorrections = new List<SalaryCorrection>();
            try
            {
                oSalaryCorrections = SalaryCorrection.GetsReason(sBU, sLocationID, nMonthID, nYear, nRowLength, nLoadRecords, sEmployeeIDs, false, (int)Session[SessionInfo.currentUserID]);
                if (oSalaryCorrections.Count <= 0)
                {
                    oSalaryCorrections = new List<SalaryCorrection>();
                    SalaryCorrection oSalaryCorrection = new SalaryCorrection();
                    oSalaryCorrections.Add(oSalaryCorrection);
                    oSalaryCorrections[0].ErrorMessage = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                oSalaryCorrections = new List<SalaryCorrection>();
                SalaryCorrection oSalaryCorrection = new SalaryCorrection();
                oSalaryCorrections.Add(oSalaryCorrection);
                oSalaryCorrections[0].ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalaryCorrections);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SingleProcess(string sEmployeeIDs, int nMonthID, int nYear)
        {
            _oSalaryCorrection = new SalaryCorrection();
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            PayrollProcessManagement oTempPayrollProcessManagement = new PayrollProcessManagement();
            List<PayrollProcessManagement> oPayrollProcessManagements = new List<PayrollProcessManagement>();
            try
            {
                string sBaseAddress = "";
                string sSQL = "SELECT top(1)* FROM View_Company";// WHERE CompanyID=" + ((User)Session[SessionInfo.CurrentUser]).CompanyID;
                List<Company> oCompanys = new List<Company>();
                oCompanys = Company.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sBaseAddress = oCompanys[0].BaseAddress;

                int nIndex = 0;
                int nNewIndex = 1;
                string sEmployeeIDsForProcess = "";
                bool bFlag = true;
                string[] sEmpIDs = sEmployeeIDs.Split(',');

                foreach (string oitem in sEmpIDs)
                {
                    int nEmpID = Convert.ToInt32(oitem);
                    oTempPayrollProcessManagement = PayrollProcessManagement.CheckPPM(nEmpID, nMonthID, nYear, (int)Session[SessionInfo.currentUserID]);
                    if (oTempPayrollProcessManagement.ErrorMessage == "")
                    {
                        oPayrollProcessManagements.Add(oTempPayrollProcessManagement);
                        sEmployeeIDsForProcess += oitem + ",";
                    }
                }
                if (oPayrollProcessManagements.Count > 0)
                {
                    oPayrollProcessManagement.PPMID = oPayrollProcessManagements[0].PPMID;
                    oPayrollProcessManagement.Status = oPayrollProcessManagements[0].Status;
                    sEmployeeIDsForProcess = sEmployeeIDsForProcess.Remove(sEmployeeIDsForProcess.Length - 1, 1);
                }

                if (oPayrollProcessManagement.PPMID > 0)
                {
                    if (sEmployeeIDs == null) { sEmployeeIDs = ""; }
                    if (oPayrollProcessManagement.Status == EnumProcessStatus.Processed || oPayrollProcessManagement.Status == EnumProcessStatus.ReProcessed)
                    {
                        if (sBaseAddress == "zn")
                        {
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Production(nIndex, oPayrollProcessManagement.PPMID, sEmployeeIDs, (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }
                        }
                        else if (sBaseAddress == "mamiya")
                        {
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Mamiya(nIndex, oPayrollProcessManagement.PPMID, "", (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }
                        }
                        else
                        {
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Corporate(nIndex, oPayrollProcessManagement.PPMID, sEmployeeIDsForProcess, (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }

                            //For Discontinue
                            nNewIndex = 1;
                            while (nNewIndex != 0)
                            {
                                nNewIndex = oPayrollProcessManagement.ProcessPayroll_Corporate_Discontinue(nIndex, oPayrollProcessManagement.PPMID, sEmployeeIDsForProcess, (int)Session[SessionInfo.currentUserID]);
                                nIndex = nNewIndex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oPayrollProcessManagement.ErrorMessage = ex.Message;
            }
            _oSalaryCorrection.PayrollProcessManagement = oPayrollProcessManagement;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryCorrection);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion
        #endregion



        #region Compliance Salary

        public ActionResult View_CompPayrollProcesss(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oPayrollProcessManagements = new List<PayrollProcessManagement>();
            _oPayrollProcessManagementObjects = new List<PayrollProcessManagementObject>();
            List<PayrollProcessManagementObject> _oPayrollProcessManagementObjectTemps = new List<PayrollProcessManagementObject>();
            string sSQL = "select * from View_CompliancePayrollProcessManagement WHERE Status=1";
            _oPayrollProcessManagements = PayrollProcessManagement.GetsComp(sSQL, (int)Session[SessionInfo.currentUserID]);
            _oPayrollProcessManagements = _oPayrollProcessManagements.OrderByDescending(x => x.ProcessDate).ToList();
            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).Where(x => x.Value != 0.ToString()).ToList();


            return View(_oPayrollProcessManagements);
        }
        public ActionResult View_CompPayrollProcess(string sid, string sMsg)
        {
            int nPPMID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oPayrollProcessManagement = new PayrollProcessManagement();
            if (nPPMID > 0)
            {
                _oPayrollProcessManagement = PayrollProcessManagement.GetComp(nPPMID, (int)Session[SessionInfo.currentUserID]);
            }

            //string sSQL_DRP = "SELECT * FROM View_DepartmentRequirementPolicy WHERE DepartmentID IN (SELECT DepartmentID FROM Department WHERE IsActive=1)";
            //_oPayrollProcessManagement.DepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets(sSQL_DRP,(int)Session[SessionInfo.currentUserID]);
            //_oPayrollProcessManagement.DepartmentRequirementPolicys = _oPayrollProcessManagement.DepartmentRequirementPolicys.GroupBy(x => x.DepartmentID).Select(grp => grp.First()).ToList();

            string sSQL_SS = "SELECT * FROM SalaryScheme WHERE IsActive=1";
            _oPayrollProcessManagement.SalarySchemes = SalaryScheme.Gets(sSQL_SS, (int)Session[SessionInfo.currentUserID]);

            string sSQL_SH = "SELECT * FROM SalaryHead WHERE IsActive=1 AND IsProcessDependent=1";
            _oPayrollProcessManagement.SalaryHeads = SalaryHead.Gets(sSQL_SH, (int)Session[SessionInfo.currentUserID]);

            _oPayrollProcessManagement.EmployeeGroups = EmployeeGroup.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, (int)Session[SessionInfo.currentUserID]);

            //_oPayrollProcessManagement.Locations = Location.Gets((int)Session[SessionInfo.currentUserID]);

            ViewBag.Months = Enum.GetValues(typeof(EnumMonth)).Cast<EnumMonth>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.BusinessUnits = BusinessUnit.Gets((int)Session[SessionInfo.currentUserID]);
            List<DepartmentRequirementPolicy> oDRPs = new List<DepartmentRequirementPolicy>();
            oDRPs = DepartmentRequirementPolicy.Gets((int)Session[SessionInfo.currentUserID]);
            oDRPs = oDRPs.GroupBy(x => x.LocationID).Select(x => x.First()).ToList();
            ViewBag.Locations = oDRPs;

            ViewBag.EmployeeGroups = EmployeeType.Gets("select * from EmployeeType where EmployeeGrouping=" + (int)EnumEmployeeGrouping.StaffWorker, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BankAccounts = BankAccount.Gets((int)Session[SessionInfo.currentUserID]);

            List<PayrollProcessManagementObject> oPPMOs = new List<PayrollProcessManagementObject>();
            oPPMOs = PayrollProcessManagementObject.Gets("SELECT * FROM CompliancePayrollProcessManagementObject WHERE PPMID=" + nPPMID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.PPMobjects = oPPMOs;


            MaxOTConfiguration oMaxOTConfiguration = new MaxOTConfiguration();
            List<MaxOTConfiguration> oMaxOTConfigurationUsers = new List<MaxOTConfiguration>();
            List<MaxOTConfiguration> oMaxOTConfigurationTemps = new List<MaxOTConfiguration>();
            List<MaxOTConfiguration> oMaxOTConfigurations = MaxOTConfiguration.Gets((int)(Session[SessionInfo.currentUserID]));
            oMaxOTConfigurationUsers = MaxOTConfiguration.GetsByUser((int)(Session[SessionInfo.currentUserID]));

            foreach (MaxOTConfiguration oItem1 in oMaxOTConfigurations)
            {
                foreach (MaxOTConfiguration oItem2 in oMaxOTConfigurationUsers)
                {
                    if (oItem1.MOCID == oItem2.MOCID)
                    {
                        oMaxOTConfigurationTemps.Add(oItem1);
                    }
                }
            }
            ViewBag.TimeCards = oMaxOTConfigurationTemps;

            return View(_oPayrollProcessManagement);
        }


        public JsonResult PayrollProcess_IUD_V1Comp(PayrollProcessManagement oPayrollProcessManagement)
        {
            try
            {
                string sBaseAddress = "";
                string sSQL = "SELECT top(1)* FROM View_Company";// WHERE CompanyID=" + ((User)Session[SessionInfo.CurrentUser]).CompanyID;
                List<Company> oCompanys = new List<Company>();
                oCompanys = Company.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sBaseAddress = oCompanys[0].BaseAddress;

                int nIndex = 0;
                int nNewIndex = 1;
                string sEmployeeIDs = "";
                oPayrollProcessManagement.MonthID = (EnumMonth)oPayrollProcessManagement.MonthIDInt;
                oPayrollProcessManagement.CompanyID = 1;
                oPayrollProcessManagement.Status = EnumProcessStatus.Processed;
                oPayrollProcessManagement.PaymentCycle = EnumPaymentCycle.Monthly;
                oPayrollProcessManagement.ProcessDate = DateTime.Now;
                oPayrollProcessManagement = oPayrollProcessManagement.IUD_V1Comp((int)Session[SessionInfo.currentUserID]);

                if (oPayrollProcessManagement.PPMID > 0)
                {
                    if (sEmployeeIDs == null) { sEmployeeIDs = ""; }
                    if (oPayrollProcessManagement.Status == EnumProcessStatus.Processed || oPayrollProcessManagement.Status == EnumProcessStatus.ReProcessed)
                    {
                        while (nNewIndex != 0)
                        {
                            nNewIndex = oPayrollProcessManagement.ProcessPayroll_CorporateComp(nIndex, oPayrollProcessManagement.PPMID, "", (int)Session[SessionInfo.currentUserID]);
                            nIndex = nNewIndex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oPayrollProcessManagement = new PayrollProcessManagement();
                oPayrollProcessManagement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPayrollProcessManagement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Excel
        public void PrintExcel(string sString)
        {
            List<SalaryCorrection> oSalaryCorrections = new List<SalaryCorrection>();

            string sBusinessUnitIds = "";
            string sLocationID = "";
            int nMonthID = 0;
            int nYear = 0;
            int nRowLength = 0;
            int nLoadRecords = 0;
            string sEmployeeIDs = "";

            if (!String.IsNullOrEmpty(sString))
            {
                int nCount = 0;
                sBusinessUnitIds = sString.Split('~')[nCount++];
                sLocationID = sString.Split('~')[nCount++];
                nMonthID = Convert.ToInt32(sString.Split('~')[nCount++]);
                nYear = Convert.ToInt32(sString.Split('~')[nCount++]);
                nRowLength = Convert.ToInt32(sString.Split('~')[nCount++]);
                nLoadRecords = Convert.ToInt32(sString.Split('~')[nCount++]);
                sEmployeeIDs = sString.Split('~')[nCount++];

                oSalaryCorrections = SalaryCorrection.GetsReason(sBusinessUnitIds, sLocationID, nMonthID, nYear, 50, 2000, sEmployeeIDs, true, (int)Session[SessionInfo.currentUserID]);
                if (oSalaryCorrections.Count <= 0)
                {
                    oSalaryCorrections = new List<SalaryCorrection>();
                    SalaryCorrection oSalaryCorrection = new SalaryCorrection();
                    oSalaryCorrections.Add(oSalaryCorrection);
                    oSalaryCorrections[0].ErrorMessage = "No Data Found";
                }
            }
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Code", Width = 18f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Name", Width = 40f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "BusinessUnit", Width = 45f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Location", Width = 25f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Reason", Width = 70f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Salary Correction Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Salary Correction Report");
                sheet.Name = "Salary Correction Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = table_header.Count+1;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Salary Correction Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                #region Data
                nRowIndex++;
                nStartCol = 2;

                int nCount = 0; nEndCol = table_header.Count() + nStartCol;
                nEndCol = table_header.Count() + nStartCol;

                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 11, true, true);
                foreach (SalaryCorrection obj in oSalaryCorrections)
                {
                    nStartCol = 2;
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Code, false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Name, false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.BUName, false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LocationName, false, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Reason, false, false);
                    nRowIndex++;
                }

                #endregion
                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=SalaryCorrectionReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion

        }
        #endregion



        #region mail
        private void PaySlipMailSend(LeaveApplication oLeaveApplication, int nEmployeeID, string sApproveOrNot)
        {
            Employee oEmployee = new Employee();
            oEmployee = oEmployee.Get(nEmployeeID, (int)Session[SessionInfo.currentUserID]);

            if (Global.IsValidMail(oEmployee.Email))
            {
                List<string> emialTos = new List<string>();
                emialTos.Add(oEmployee.Email);

                string subject = "Leave Application Sent from " + oLeaveApplication.EmployeeName;
                string message = "";
                if (sApproveOrNot == "Approve")
                {
                    message = "Your Leave is approved which is " + oLeaveApplication.LeaveTypeInString + " leave from " + oLeaveApplication.StartDateTimeInString + " to " + oLeaveApplication.EndDateTimeInString + "."
                                 + " Which is a " + oLeaveApplication.LeaveHeadName + ".";
                }
                else
                {
                    message = "I am applying for a " + oLeaveApplication.LeaveTypeInString + " leave from " + oLeaveApplication.StartDateTimeInString + " to " + oLeaveApplication.EndDateTimeInString + "."
                                 + " Which should be a " + oLeaveApplication.LeaveHeadName + ".";
                }

                string bodyInfo = "";

                List<User> oUsers=new List<User>();
                string sSQL = "Select * from View_User Where EmployeeID=" + oEmployee.EmployeeID;
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]);

                if (oUsers.Any() && oUsers.FirstOrDefault().UserID > 0)
                {
                   
                    var oUser= oUsers.FirstOrDefault();
                    if (sApproveOrNot == "Approve")
                    {
                        bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div> <div style='padding-top:5px;'>{3}</div>" +
                                                 "<div style='padding-top:5px;'>{4}</div> <div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{5}</div> <div style='padding-top:20px;'>Mail sent at time {6}</div>",
                                                 oEmployee.Name, message, oLeaveApplication.ApproveByName, oLeaveApplication.ApprovedByDesignation, oLeaveApplication.ApprovedByDepartment,
                            //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                                 Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                    }
                    else
                    {
                        bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div> <div style='padding-top:5px;'>{3}</div>" +
                                                 "<div style='padding-top:5px;'>{4}</div> <div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{5}</div> <div style='padding-top:20px;'>Mail sent at time {6}</div>",
                                                 oEmployee.Name, message, oLeaveApplication.EmployeeNameCode, oLeaveApplication.DesignationName, oLeaveApplication.DepartmentName,
                                                 //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                                 Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                    }
                }
                else
                {
                    if (sApproveOrNot == "Approve")
                    {
                        bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div> <div style='padding-top:5px;'>{3}</div>" +
                                                 "<div style='padding-top:5px;'>{4}</div> <div style='padding-top:10px;'>Please click the below link to sign in</div> <div style='padding-top:5px;'>{5}</div> <div style='padding-top:20px;'>Mail sent at time {6}</div>",
                                                 oEmployee.Name, message, oLeaveApplication.ApproveByName, oLeaveApplication.ApprovedByDesignation, oLeaveApplication.ApprovedByDepartment,
                            //Url.Action("ExternalLogin", "User", new { id = Global.GetEncodeValue(oUser.LogInID), token = Global.GetEncodeValue(Global.Decrypt(oUser.Password)) }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                                                 Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                    }
                    else
                    {
                        bodyInfo = string.Format("<div> Mr./ Mrs. {0}, </div> <div style='padding-top:15px;'>{1}</div> <div style='padding-top:20px;'>Sincerely yours</div> <div style='padding-top:5px;'> {2} </div> <div style='padding-top:5px;'>{3}</div>" +
                                              "<div style='padding-top:5px;'>{4}</div> <div style='padding-top:20px;'>Mail sent at time {5}</div> <div style='padding-top:20px;'>Mail sent at time {6}</div>",
                                              oEmployee.Name, message, oLeaveApplication.EmployeeNameCode, oLeaveApplication.DesignationName, oLeaveApplication.DepartmentName
                                              , Url.Action("Login", "User", new { sMessage = "" }, Request.Url.Scheme), DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                    }
                 }

                #region Email Credential
                EmailConfig oEmailConfig = new EmailConfig();
                oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);
                #endregion

                Global.MailSend(subject, bodyInfo, emialTos, new List<string>(), new List<Attachment>(), oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
            }
        }
        [HttpPost]
        public JsonResult PaySlipMailSend(PayrollProcessManagement oPayrollProcessManagement)
        {
            oPayrollProcessManagement = PayrollProcessManagement.Get(oPayrollProcessManagement.PPMID, (int)Session[SessionInfo.currentUserID]);
            List<Employee> oEmployees = new List<Employee>();
            Employee oEmployee = new Employee();
            oEmployees = Employee.Gets("SELECT * FROM View_Employee WHERE EmployeeID IN (SELECT EmployeeID FROM EmployeeSalary WHERE PayrollProcessID = " + oPayrollProcessManagement.PPMID + " AND EmployeeID NOT IN(SELECT EmployeeID FROM EmployeeProcessMailHistory Where PPMID = " + oPayrollProcessManagement.PPMID + " AND IsStatus = 'true'))", (int)Session[SessionInfo.currentUserID]);

            string sFeedBackMessage = "";
            bool bIsSend = false;
            foreach (Employee oItem in oEmployees)
            {
                oEmployee = oItem;
                if (!string.IsNullOrEmpty(oEmployee.Email))
                {
                    if (Global.IsValidMail(oEmployee.Email) == true)
                    {
                        EmployeeSalary oEmployeeSalary = new EmployeeSalary();
                        oEmployeeSalary = GetEmployeeSalary(oEmployee.EmployeeID, oPayrollProcessManagement.SalaryTo, false);
                        List<EmployeeSalaryDetail> oTempESDetailAdditions = new List<EmployeeSalaryDetail>();
                        List<EmployeeSalaryDetail> oTempESDetailDeductions = new List<EmployeeSalaryDetail>();
                        oTempESDetailAdditions.AddRange(oEmployeeSalary.EmployeeSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Basic)).ToList());
                        oTempESDetailAdditions.AddRange(oEmployeeSalary.EmployeeSalaryDetails.Where(x => (x.SalaryHeadType == (int)EnumSalaryHeadType.Addition)).ToList());
                        oTempESDetailDeductions.AddRange(oEmployeeSalary.EmployeeSalaryDetails.Where(x => x.SalaryHeadType == (int)EnumSalaryHeadType.Deduction).ToList());
                    
                        int nRowCount = oTempESDetailAdditions.Count;
                        if (oTempESDetailDeductions.Count > oTempESDetailAdditions.Count) nRowCount = oTempESDetailDeductions.Count;

                        List<string> emialTos = new List<string>();
                        emialTos.Add(oEmployee.Email);

                        string subject = "Pay Slip Of " + oEmployee.Name;
                        string message = "For The Month Of " + oPayrollProcessManagement.SalaryTo.ToString("MMM") + ", " + oPayrollProcessManagement.SalaryTo.Year;
                        string sFullBodyStr = string.Format("<div> Mr./ Mrs. {0},</div>" +
                                                        "<div style='padding-top:15px;'>{1}</div>" +
                                                        "<div style='padding-top:20px;'>Mail sent at time {2}</div>"
                                                        , oEmployee.Name
                                                        , message
                                                        , DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));

                        List<Company> oCompanys = new List<Company>();
                        oCompanys = Company.Gets((int)Session[SessionInfo.currentUserID]);
                        oEmployeeSalary.Company = oCompanys.First();
                        oEmployeeSalary.Company.CompanyLogo = GetCompanyLogo(oEmployeeSalary.Company);
                        oEmployeeSalary.Params = message;
                        rptPaySlip_DetailFormat oReport = new rptPaySlip_DetailFormat();
                        byte[] abytes = oReport.PrepareReport(oEmployeeSalary, true, true);

                        MemoryStream stream = new MemoryStream(abytes);
                        Attachment oAttachment = new Attachment(stream, "ESimSol_PaySlip.pdf");
                        //string sFileName = "D:/_Asad/B007/ESimSolManufacturing/ESimSolFinancial/Content/Images/PrintMultiplePaySlip_DetailFormat.pdf";
                        //Attachment oAttachment = new Attachment(abytes, sFileName);
                        List<Attachment> oAttachments = new List<Attachment>();
                        oAttachments.Add(oAttachment);

                        #region Email Credential
                        EmailConfig oEmailConfig = new EmailConfig();
                        oEmailConfig = oEmailConfig.GetByBU(1, (int)Session[SessionInfo.currentUserID]);
                        #endregion

                        bIsSend = Global.MailSend(subject, sFullBodyStr, emialTos, new List<string>(), oAttachments, oEmailConfig.EmailAddress, oEmailConfig.EmailPassword, oEmailConfig.EmailDisplayName, oEmailConfig.HostName, oEmailConfig.PortNumber, oEmailConfig.SSLRequired);
                        if (bIsSend) sFeedBackMessage = "Mail Send Successfully"; else sFeedBackMessage = "Mail Sending Error";
                    }
                    else
                    {
                        sFeedBackMessage = "Invalid Email Address";
                        bIsSend = false;
                    }
                }
                else
                {
                    sFeedBackMessage = "This Employee Has No Email Address";
                    bIsSend = false;
                }
                EmployeeProcessMailHistory oEPMHistory = new EmployeeProcessMailHistory();
                oEPMHistory.EPMHID = 0;
                oEPMHistory.PPMID = oPayrollProcessManagement.PPMID;
                oEPMHistory.EmployeeID = oEmployee.EmployeeID;
                oEPMHistory.IsStatus = bIsSend;
                oEPMHistory.FeedBackMessage = sFeedBackMessage;
                oEPMHistory.OperatedBy = (int)Session[SessionInfo.currentUserID];
                oEPMHistory.Save((int)Session[SessionInfo.currentUserID]);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPayrollProcessManagement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public void MailHistoryReport(int nID)
        {
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            oPayrollProcessManagement = PayrollProcessManagement.Get(nID, (int)Session[SessionInfo.currentUserID]);
            List<EmployeeProcessMailHistory> oEmployeeProcessMailHistorys = new List<EmployeeProcessMailHistory>();
            oEmployeeProcessMailHistorys = EmployeeProcessMailHistory.Gets("SELECT * FROM View_EmployeeProcessMailHistory WHERE PPMID = " + nID + " ORDER BY EmployeeID, SendingTime", (int)Session[SessionInfo.currentUserID]);
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Center, IsBold = true});
            table_header.Add(new TableHeader { Header = "Employee Code", Width = 15f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Employee Name", Width = 30f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Email", Width = 40f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Feedback Message", Width = 35f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Operated By", Width = 25f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            table_header.Add(new TableHeader { Header = "Sending Time", Width = 25f, IsRotate = false, Align = TextAlign.Center, IsBold = true });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("Mail History");
                sheet.Name = "Mail History";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = table_header.Count;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Mail History"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;

                string sProcessInformation = "Process Date : " + oPayrollProcessManagement.ProcessDateInString + ", Salary of " + oPayrollProcessManagement.SalaryForInString;
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = sProcessInformation; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                #region Data
                nRowIndex++;
                int nCount = 0; nEndCol = table_header.Count() + nStartCol;
                nEndCol = table_header.Count() + nStartCol;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                foreach (EmployeeProcessMailHistory obj in oEmployeeProcessMailHistorys)
                {
                    nStartCol = 2;
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), true, true, ExcelHorizontalAlignment.Center, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.EmployeeCode, false, false, ExcelHorizontalAlignment.Center, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.EmployeeName, false, false, ExcelHorizontalAlignment.Left, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Email, false, false, ExcelHorizontalAlignment.Left, true);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FeedBackMessage, false, false, ExcelHorizontalAlignment.Left, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.OperatedByName, false, false, ExcelHorizontalAlignment.Left, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SendingTimeSt, false, false, ExcelHorizontalAlignment.Center, false);
                    nRowIndex++;
                }
                #endregion
                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=MailHistory Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion
        public Image GetCompanyLogo(Company oCompany)
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
        public EmployeeSalary GetEmployeeSalary(int nEmployeeID, DateTime oProdessDate, bool IsCompliance)
        {
            EmployeeSalary oEmployeeSalary = new EmployeeSalary();
            string sSql = "";
            //nEmployeeID = 77595;
            int nMonth = oProdessDate.Month;
            int nYear = oProdessDate.Year;
            if (nEmployeeID != 0)
            {
                sSql = "SELECT * FROM View_EmployeeSalary WHERE  EmployeeID IN (" + nEmployeeID + ") AND MonthID=" + nMonth + " AND DATEPART(YYYY,EndDate)=" + nYear;
                sSql += " ORDER BY EmployeeCode";
                oEmployeeSalary.EmployeeSalarys = EmployeeSalary.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                sSql = "";
                sSql = "SELECT * FROM View_EmployeeSalaryDetail WHERE EmployeeSalaryID IN (SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN (" + nEmployeeID + ") AND PayrollProcessID IN (SELECT   PPMID FROM PayrollProcessManagement WHERE  MonthID=" + nMonth + ") AND DATEPART(YYYY,EndDate)=" + nYear + ") ORDER BY SalaryHeadID";
                oEmployeeSalary.EmployeeSalaryDetails = EmployeeSalaryDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                sSql = "";
                sSql = "SELECT * FROM EmployeeSalaryDetailDisciplinaryAction WHERE EmployeeSalaryID IN(SELECT EmployeeSalaryID FROM EmployeeSalary WHERE EmployeeID IN(" + nEmployeeID + ") AND PayrollProcessID IN (SELECT   PPMID FROM PayrollProcessManagement WHERE  MonthID=" + nMonth + ")  AND DATEPART(YYYY,EndDate)=" + nYear + ")";
                oEmployeeSalary.EmployeeSalaryDetailDisciplinaryActions = EmployeeSalaryDetailDisciplinaryAction.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                oEmployeeSalary.EmployeeBankAccounts = EmployeeBankAccount.Gets("SELECT * FROM VIEW_EmployeeBankAccount WHERE EmployeeID IN(" + nEmployeeID + ") AND IsActive=1", (int)Session[SessionInfo.currentUserID]);
            }

            string sSql_SalaryHead = "";
            if (IsCompliance)
            {
                sSql_SalaryHead = "SELECT * FROM SalaryHead WHERE SalaryHeadID IN(SELECT SalaryHeadID  FROM EmployeeSalaryDetail WHERE "
                                        + " EmployeeSalaryID IN(SELECT EmployeeSalaryID FROM View_EmployeeSalary WHERE MonthID=" + nMonth + " AND  DATEPART(YYYY,EndDate)=" + nYear + ") AND CompAmount>0)";
            }
            else
            {
                sSql_SalaryHead = "SELECT * FROM SalaryHead WHERE SalaryHeadID IN(SELECT SalaryHeadID  FROM EmployeeSalaryDetail WHERE "
                                        + " EmployeeSalaryID IN(SELECT EmployeeSalaryID FROM View_EmployeeSalary WHERE MonthID=" + nMonth + " AND  DATEPART(YYYY,EndDate)=" + nYear + ") AND Amount>0)";
            }
            oEmployeeSalary.SalaryHeads = SalaryHead.Gets(sSql_SalaryHead, (int)Session[SessionInfo.currentUserID]);
            return oEmployeeSalary;
        }
    }
}
