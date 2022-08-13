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
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using ESimSolFinancial.Hubs;

namespace ESimSolFinancial.Controllers
{
    public class ProcessManagementV2Controller : Controller
    {
        #region Declaration
        AttendanceProcessManagement _oAttendanceProcessManagement = new AttendanceProcessManagement();
        List<AttendanceProcessManagement> _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
        EmployeeBonusProcess _oEmployeeBonusProcess = new EmployeeBonusProcess();
        List<EmployeeBonusProcess> _oEmployeeBonusProcesss = new List<EmployeeBonusProcess>();
        EmployeeBonusProcessObject _oEmployeeBonusProcessObject = new EmployeeBonusProcessObject();
        List<EmployeeBonusProcessObject> _oEmployeeBonusProcessObjects = new List<EmployeeBonusProcessObject>();
        //string ConnStringRT = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\RTAttendanceData\\UNIS.mdb; Jet OLEDB:Database Password=unisamho;";
        #endregion

        #region Actual Attendance Process
        public ActionResult ViewAttendanceProcessManagements(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            ViewBag.EnumPunchFormats = EnumObject.jGets(typeof(EnumPunchFormat));
            ViewBag.MenuID = menuid.ToString();
            ViewBag.FeedBack = "";
            return View(_oAttendanceProcessManagements);
        }

        public ActionResult ViewManualAttendanceProcess(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            AttendanceProcessManagement oAPM = new AttendanceProcessManagement();

            List<Location> oLocations = new List<Location>();
            List<Department> oDepartments = new List<Department>();
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();

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
            oAPM = new AttendanceProcessManagement();
            oAPM.BusinessUnits = oBusinessUnits;
            oAPM.Locations = oLocations;
            oAPM.Departments = oDepartments;
            return View(oAPM);
        }

        public ActionResult ViewManualCompAttendanceProcess(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
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
            oAPM = new AttendanceProcessManagement();
            oAPM.BusinessUnits = oBusinessUnits;
            oAPM.Locations = oLocations;
            oAPM.Departments = oDepartments;
            oAPM.TimeCards = oMaxOTConfigurations;
            return View(oAPM);
        }

        [HttpPost]
        public JsonResult Save(AttendanceProcessManagement oAttendanceProcessManagement)
        {
            AttendanceProcessManagement oAttPM = new AttendanceProcessManagement();
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            try
            {
                DepartmentRequirementPolicy oDepartmentRequirementPolicy = new DepartmentRequirementPolicy();
                List<DepartmentRequirementPolicy> oDepartmentRequirementPolicys = new List<DepartmentRequirementPolicy>();
                oDepartmentRequirementPolicys = DepartmentRequirementPolicy.Gets("SELECT * FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN (" + oAttendanceProcessManagement.ErrorMessage + ")", (int)Session[SessionInfo.currentUserID]);

                if (oDepartmentRequirementPolicys.Count > 0)
                {
                    foreach (DepartmentRequirementPolicy oItem in oDepartmentRequirementPolicys)
                    {
                        _oAttendanceProcessManagement = new AttendanceProcessManagement();
                        _oAttendanceProcessManagement.APMID = 0;
                        _oAttendanceProcessManagement.CompanyID = oItem.CompanyID;
                        _oAttendanceProcessManagement.LocationID = oItem.LocationID;
                        _oAttendanceProcessManagement.BusinessUnitID = oItem.BusinessUnitID;
                        _oAttendanceProcessManagement.DepartmentID = oItem.DepartmentID;
                        _oAttendanceProcessManagement.AttendanceDate = oAttendanceProcessManagement.AttendanceDate;
                        _oAttendanceProcessManagement.ProcessType = EnumProcessType.DailyProcess;
                        _oAttendanceProcessManagement.Status = EnumProcessStatus.Initialize;
                        _oAttendanceProcessManagement = _oAttendanceProcessManagement.IUD_V2(EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                        _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
                    }
                }
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
                _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(AttendanceProcessManagement oAttendanceProcessManagement)
        {
            AttendanceProcessManagement oAttPM = new AttendanceProcessManagement();
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            try
            {

                if (oAttendanceProcessManagement.ErrorMessage != null || oAttendanceProcessManagement.ErrorMessage != "")
                {
                    _oAttendanceProcessManagement = oAttendanceProcessManagement.IUD_V2(EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
                }

                if (_oAttendanceProcessManagement.ErrorMessage != "")
                {
                    _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
                    _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
                }
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
                _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RollBack(AttendanceProcessManagement oAttendanceProcessManagement)
        {
            AttendanceProcessManagement oAttPM = new AttendanceProcessManagement();
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            try
            {

                if (oAttendanceProcessManagement.ErrorMessage != null || oAttendanceProcessManagement.ErrorMessage != "")
                {
                    _oAttendanceProcessManagement = oAttendanceProcessManagement.IUD_V2(EnumDBOperation.RollBack, (int)Session[SessionInfo.currentUserID]);
                    _oAttendanceProcessManagements = _oAttendanceProcessManagement.AttendanceProcessManagements;
                }
                if (_oAttendanceProcessManagement.ErrorMessage != "")
                {
                    _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
                    _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
                }
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
                _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ManualAttendanceProcess(AttendanceProcessManagement oAPM)
        {
            _oAttendanceProcessManagement = new AttendanceProcessManagement();
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            try
            {

                #region APM Process
                DateTime dAPMStartDate = DateTime.Today;
                dAPMStartDate = oAPM.StartDate;
                while (dAPMStartDate <= oAPM.EndDate)
                {
                    AttendanceProcessManagement.APMProcess(BusinessUnit.IDInString(oAPM.BusinessUnits), Location.IDInString(oAPM.Locations), Department.IDInString(oAPM.Departments), dAPMStartDate, dAPMStartDate, (int)Session[SessionInfo.currentUserID]);
                    dAPMStartDate = dAPMStartDate.AddDays(1);
                }
                #endregion

                double nTotalBatchCount = 0;
                List<AttendanceProcessManagement> oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
                string sSQL = "SELECT * FROM View_AttendanceProcessManagement AS HH WHERE HH.AttendanceDate BETWEEN '" + oAPM.StartDate.ToString("dd MMM yyyy") + "' AND '" + oAPM.EndDate.ToString("dd MMM yyyy") + "' AND HH.BusinessUnitID IN (" + BusinessUnit.IDInString(oAPM.BusinessUnits) + ") AND HH.LocationID IN (" + Location.IDInString(oAPM.Locations) + ") AND HH.DepartmentID IN (" + Department.IDInString(oAPM.Departments) + ") ORDER BY HH.AttendanceDate, HH.BusinessUnitID, HH.LocationName, HH.DepartmenName ASC";
                oAttendanceProcessManagements = AttendanceProcessManagement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                var oSingleDepts = oAttendanceProcessManagements.Where(x => x.EmpCount > 100).ToList();
                var oGropuDepts = oAttendanceProcessManagements.Where(x => x.EmpCount <= 100).GroupBy(grp => new
                {
                    grp.BusinessUnitID,
                    grp.LocationID,
                    grp.AttendanceDate,
                }).Select(gcs => new AttendanceProcessManagement
                {
                    BusinessUnitID = gcs.Key.BusinessUnitID,
                    LocationID = gcs.Key.LocationID,
                    AttendanceDate = gcs.Key.AttendanceDate,
                });


                if (oSingleDepts != null && oSingleDepts.Count() > 0)
                {
                    nTotalBatchCount = nTotalBatchCount + oSingleDepts.Count();
                }
                if (oGropuDepts != null && oGropuDepts.Count() > 0)
                {
                    nTotalBatchCount = nTotalBatchCount + oGropuDepts.Count();
                }


                double nPercentCount = 5;
                Thread.Sleep(100);
                ProgressHub.SendMessage("Process : ", nPercentCount, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(500);

                //double nDaysCount = (oAPM.EndDate - oAPM.StartDate).TotalDays + 1;
                //nTotalBatchCount = (nTotalBatchCount * nDaysCount);
                double nPerUnitPercentCount = (95.00 / nTotalBatchCount);

                string sDeptIDs = "";
                DateTime dStartDate = DateTime.Today;
                dStartDate = oAPM.StartDate;
                while (dStartDate <= oAPM.EndDate)
                {
                    foreach (BusinessUnit oBusinessUnit in oAPM.BusinessUnits)
                    {
                        foreach (Location oLocation in oAPM.Locations)
                        {
                            List<AttendanceProcessManagement> oAPMs = new List<AttendanceProcessManagement>();
                            oAPMs = this.GetsDepartmens(oBusinessUnit.BusinessUnitID, oLocation.LocationID, dStartDate, oAttendanceProcessManagements);
                            if (oAPMs != null && oAPMs.Count() > 0)
                            {
                                foreach (AttendanceProcessManagement oDepartment in oAPMs)
                                {
                                    sDeptIDs = "";
                                    if (oDepartment.DepartmentID > 0)
                                    {
                                        sDeptIDs = oDepartment.DepartmentID.ToString();
                                    }
                                    else
                                    {
                                        sDeptIDs = oDepartment.ErrorMessage;
                                    }

                                    oAPM.EmployeeID = 0;
                                    _oAttendanceProcessManagement = new AttendanceProcessManagement();
                                    nPercentCount = nPercentCount + nPerUnitPercentCount;
                                    ProgressHub.SendMessage("Process Date : " + dStartDate.ToString("dd MMM yyyy") + ",  BU : " + oBusinessUnit.ShortName + ", Location : " + oLocation.Name + ", Dept : " + oDepartment.DepartmenName + "(" + oDepartment.EmpCount.ToString() + ") ", nPercentCount, (int)Session[SessionInfo.currentUserID]);
                                    _oAttendanceProcessManagement = _oAttendanceProcessManagement.ManualAttendanceProcess(oBusinessUnit.BusinessUnitID.ToString(), oLocation.LocationID.ToString(), sDeptIDs, oAPM.EmployeeID, dStartDate, (int)Session[SessionInfo.currentUserID]);
                                    Thread.Sleep(100);
                                    if (_oAttendanceProcessManagement.ErrorMessage != "")
                                    {
                                        _oAttendanceProcessManagement.ErrorMessage = "Attendance Date : " + dStartDate.ToString("dd MMM yyyy") + ", BU Name :" + oBusinessUnit.ShortName + ", Location : " + oLocation.Name + ", Dept : " + oDepartment.DepartmenName + ", Invalid Operation for " + _oAttendanceProcessManagement.ErrorMessage;
                                        _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
                                    }
                                }
                            }
                        }
                    }
                    dStartDate = dStartDate.AddDays(1);
                }

                ProgressHub.SendMessage("Finishing Attendance process", 100, (int)Session[SessionInfo.currentUserID]);
                ProgressHub.SendMessage("Finishing Attendance process", 100, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
                _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EmployeeWiseManualAttendanceProcess(AttendanceProcessManagement oAPM)
        {
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();
            try
            {
                bool bFlag = true;
                string sBUIDs = "", sLocationIDs = "", sDeptIDs = "";
                DateTime dStartDate = DateTime.Today;
                dStartDate = oAPM.StartDate;
                while (dStartDate <= oAPM.EndDate)
                {
                    _oAttendanceProcessManagement = new AttendanceProcessManagement();
                    _oAttendanceProcessManagement = _oAttendanceProcessManagement.ManualAttendanceProcess(sBUIDs, sLocationIDs, sDeptIDs, oAPM.EmployeeID, dStartDate, (int)Session[SessionInfo.currentUserID]);
                    if (_oAttendanceProcessManagement.ErrorMessage != "")
                    {
                        oAttendanceDaily = new AttendanceDaily();
                        oAttendanceDaily.ErrorMessage = _oAttendanceProcessManagement.ErrorMessage;
                        bFlag = false;
                        break;
                    }
                    dStartDate = dStartDate.AddDays(1);
                }
                if (bFlag)
                {
                    oAttendanceDailys = new List<AttendanceDaily>();
                    string sSQL = "SELECT * FROM View_AttendanceDaily AS HH WHERE HH.EmployeeID =" + oAPM.EmployeeID.ToString() + " AND CONVERT(DATE,CONVERT(VARCHAR(12),HH.AttendanceDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + oAPM.StartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + oAPM.EndDate.ToString("dd MMM yyyy") + "',106)) ORDER BY HH.AttendanceDate ASC";
                    oAttendanceDailys = AttendanceDaily.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }

            }
            catch (Exception ex)
            {
                oAttendanceDailys = new List<AttendanceDaily>();
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDaily.ErrorMessage = ex.Message;
                oAttendanceDailys.Add(oAttendanceDaily);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<AttendanceProcessManagement> GetsDepartmens(int nBUID, int nLocationID, DateTime dAttenDate, List<AttendanceProcessManagement> oAttendanceProcessManagements)
        {
            string sDepartmentIDs = ""; int nEmpCount = 0; int nDeptCount = 0;
            List<AttendanceProcessManagement> oAPMs = new List<AttendanceProcessManagement>();
            foreach (AttendanceProcessManagement oAPM in oAttendanceProcessManagements)
            {
                if (oAPM.BusinessUnitID == nBUID && oAPM.LocationID == nLocationID && oAPM.AttendanceDate.ToString("dd MMM yyyy") == dAttenDate.ToString("dd MMM yyyy"))
                {
                    if (oAPM.EmpCount > 100)
                    {
                        oAPMs.Add(oAPM);
                    }
                    else
                    {
                        nDeptCount = nDeptCount + 1;
                        sDepartmentIDs = sDepartmentIDs + oAPM.DepartmentID.ToString() + ",";
                        nEmpCount = nEmpCount + oAPM.EmpCount;
                    }
                }
            }
            if (sDepartmentIDs != "")
            {
                sDepartmentIDs = sDepartmentIDs.Remove(sDepartmentIDs.Length - 1, 1);
            }
            if (sDepartmentIDs != "")
            {
                AttendanceProcessManagement oAPM = new AttendanceProcessManagement();
                oAPM.DepartmenName = "Others-" + nDeptCount.ToString();
                oAPM.ErrorMessage = sDepartmentIDs;
                oAPM.EmpCount = nEmpCount;
                oAPMs.Add(oAPM);
            }
            return oAPMs;
        }

        #endregion

        #region Actual Attendance Process

        [HttpPost]
        public JsonResult ManualCompAttendanceProcess(AttendanceProcessManagement oAPM)
        {
            _oAttendanceProcessManagement = new AttendanceProcessManagement();
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            try
            {
                double nTotalBatchCount = 0;
                List<AttendanceProcessManagement> oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
                string sSQL = "SELECT * FROM View_AttendanceProcessManagement AS HH WHERE HH.AttendanceDate BETWEEN '" + oAPM.StartDate.ToString("dd MMM yyyy") + "' AND '" + oAPM.EndDate.ToString("dd MMM yyyy") + "' AND HH.BusinessUnitID IN (" + BusinessUnit.IDInString(oAPM.BusinessUnits) + ") AND HH.LocationID IN (" + Location.IDInString(oAPM.Locations) + ") AND HH.DepartmentID IN (" + Department.IDInString(oAPM.Departments) + ") ORDER BY HH.AttendanceDate, HH.BusinessUnitID, HH.LocationName, HH.DepartmenName ASC";
                oAttendanceProcessManagements = AttendanceProcessManagement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                var oGropuDepts = oAttendanceProcessManagements.GroupBy(grp => new
                {
                    grp.BusinessUnitID,
                    grp.LocationID,
                    grp.AttendanceDate,
                }).Select(gcs => new AttendanceProcessManagement
                {
                    BusinessUnitID = gcs.Key.BusinessUnitID,
                    LocationID = gcs.Key.LocationID,
                    AttendanceDate = gcs.Key.AttendanceDate,
                    EmpCount = gcs.Sum(x => x.EmpCount)
                });

                if (oGropuDepts != null && oGropuDepts.Count() > 0)
                {
                    nTotalBatchCount = nTotalBatchCount + oGropuDepts.Count();
                }

                //for Multiple Time Cards 
                nTotalBatchCount = (nTotalBatchCount * oAPM.TimeCards.Count());


                double nPercentCount = 5;
                Thread.Sleep(100);
                ProgressHub.SendMessage("Process : ", nPercentCount, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(500);

                double nPerUnitPercentCount = (95.00 / nTotalBatchCount);

                string sDeptIDs = "";
                DateTime dStartDate = DateTime.Today;
                List<Location> oLocations = new List<Location>();

                dStartDate = oAPM.StartDate;
                while (dStartDate <= oAPM.EndDate)
                {
                    foreach (MaxOTConfiguration oTimeCard in oAPM.TimeCards)
                    {
                        foreach (BusinessUnit oBusinessUnit in oAPM.BusinessUnits)
                        {
                            oLocations = new List<Location>();
                            oLocations = this.GetsCompLocations(oBusinessUnit.BusinessUnitID, dStartDate, oAttendanceProcessManagements);
                            foreach (Location oLocation in oLocations)
                            {
                                sDeptIDs = "";                                
                                sDeptIDs = this.GetsCompDepartments(oBusinessUnit.BusinessUnitID, oLocation.LocationID, dStartDate, oAttendanceProcessManagements);

                                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                                nPercentCount = nPercentCount + nPerUnitPercentCount;
                                ProgressHub.SendMessage("Process Date : " + dStartDate.ToString("dd MMM yyyy") + ", Time Card : " + oTimeCard.TimeCardName + ", BU : " + oBusinessUnit.ShortName + ", Location : " + oLocation.Name, nPercentCount, (int)Session[SessionInfo.currentUserID]);
                                _oAttendanceProcessManagement = _oAttendanceProcessManagement.ManualCompAttendanceProcess(oBusinessUnit.BusinessUnitID.ToString(), oLocation.LocationID.ToString(), sDeptIDs, dStartDate, oTimeCard.MOCID, (int)Session[SessionInfo.currentUserID]);
                                Thread.Sleep(100);
                                if (_oAttendanceProcessManagement.ErrorMessage != "")
                                {
                                    _oAttendanceProcessManagement.ErrorMessage = "Attendance Date : " + dStartDate.ToString("dd MMM yyyy") + ", Time Card : " + oTimeCard.TimeCardName + ",  BU Name :" + oBusinessUnit.ShortName + ", Location :" + oLocation.Name + ", Invalid Operation for " + _oAttendanceProcessManagement.ErrorMessage;
                                    _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
                                }
                            }
                        }
                    }
                    dStartDate = dStartDate.AddDays(1);
                }

                ProgressHub.SendMessage("Finishing Attendance process", 100, (int)Session[SessionInfo.currentUserID]);
                ProgressHub.SendMessage("Finishing Attendance process", 100, (int)Session[SessionInfo.currentUserID]);
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
                _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        private List<Location> GetsCompLocations(int nBUID, DateTime dAttenDate, List<AttendanceProcessManagement> oAttendanceProcessManagements)
        {
            Location oLocation = new Location(); 
            List<Location> oLocations = new List<Location>();            
            List<AttendanceProcessManagement> oAPMs = new List<AttendanceProcessManagement>();
            foreach (AttendanceProcessManagement oAPM in oAttendanceProcessManagements)
            {
                if (oAPM.BusinessUnitID == nBUID && oAPM.AttendanceDate.ToString("dd MMM yyyy") == dAttenDate.ToString("dd MMM yyyy"))
                {
                    if (!IsExists(oAPM.LocationID, oLocations))
                    {
                        oLocation = new Location();
                        oLocation.LocationID = oAPM.LocationID;
                        oLocation.Name = oAPM.LocationName;
                        oLocations.Add(oLocation);
                    }
                }
            }
            return oLocations;
        }

        private bool IsExists(int nLocationID, List<Location> oLocations)
        {
            foreach (Location oItem in oLocations)
            {
                if (oItem.LocationID == nLocationID)
                {
                    return true;
                }
            }
            return false;
        }

        private string GetsCompDepartments(int nBUID,  int nLocationID, DateTime dAttenDate, List<AttendanceProcessManagement> oAttendanceProcessManagements)
        {
            string sDepartmentIDs = "";
            List<AttendanceProcessManagement> oAPMs = new List<AttendanceProcessManagement>();
            foreach (AttendanceProcessManagement oAPM in oAttendanceProcessManagements)
            {
                if (oAPM.BusinessUnitID == nBUID && oAPM.LocationID == nLocationID && oAPM.AttendanceDate.ToString("dd MMM yyyy") == dAttenDate.ToString("dd MMM yyyy"))
                {
                    sDepartmentIDs = sDepartmentIDs + oAPM.DepartmentID.ToString() + ",";
                }
            }
            if (sDepartmentIDs != "")
            {
                sDepartmentIDs = sDepartmentIDs.Remove(sDepartmentIDs.Length - 1, 1);
            }
            return sDepartmentIDs;
        }
        #endregion

        #region Gets
        [HttpPost]
        public JsonResult GetAPMs(AttendanceProcessManagement oAPM)
        {
            int nCount = 0;
            DateTime dStartDate = Convert.ToDateTime(oAPM.ErrorMessage.Split('~')[nCount++]);
            DateTime dEndDate = Convert.ToDateTime(oAPM.ErrorMessage.Split('~')[nCount++]);
            string sBUIDs = Convert.ToString(oAPM.ErrorMessage.Split('~')[nCount++]);
            string sLocIDs = Convert.ToString(oAPM.ErrorMessage.Split('~')[nCount++]);
            string sDeptIDs = Convert.ToString(oAPM.ErrorMessage.Split('~')[nCount++]);

            string sSQL = "SELECT * FROM View_AttendanceProcessManagement AS APM WITH(NOLOCK) WHERE APM.AttendanceDate BETWEEN '" + dStartDate.ToString("dd MMM yyyy") + "' AND '" + dEndDate.ToString("dd MMM yyyy") + "'";
            if (!string.IsNullOrEmpty(sBUIDs))
            {
                sSQL += " AND APM.BusinessUnitID IN(" + sBUIDs + ")";
            }
            if (!string.IsNullOrEmpty(sLocIDs))
            {
                sSQL += " AND APM.LocationID IN(" + sLocIDs + ")";
            }
            if (!string.IsNullOrEmpty(sDeptIDs))
            {
                sSQL += " AND APM.DepartmentID IN(" + sDeptIDs + ")";
            }

            if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
            {
                sSQL = sSQL + " AND APM.DRPID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission AS DRPP WHERE DRPP.UserID = " + ((int)Session[SessionInfo.currentUserID]).ToString() + ")";
            }
            sSQL = sSQL + " ORDER BY APM.AttendanceDate, APM.BusinessUnitID, APM.LocationName, APM.DepartmenName ASC";

            List<AttendanceProcessManagement> oAPMs = new List<AttendanceProcessManagement>();
            try
            {
                oAPMs = AttendanceProcessManagement.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oAPMs[0] = new AttendanceProcessManagement();
                oAPMs[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAPMs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Import & Export
        private List<PunchLog> GetPunchLogFromExcel(HttpPostedFileBase PostedFile, int nPunchFormat)
        {
            try
            {
                DataSet ds = new DataSet();
                DataRowCollection oRows = null;
                string fileExtension = "";
                string fileDirectory = "";
                List<PunchLog> oPunchLogXLs = new List<PunchLog>();
                PunchLog oPunchLogXL = new PunchLog();

                string sBaseAddress = "";
                string sSQL = "SELECT top(1)* FROM View_Company";// WHERE CompanyID=" + ((User)Session[SessionInfo.CurrentUser]).CompanyID;
                List<Company> oCompanys = new List<Company>();
                oCompanys = Company.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sBaseAddress = oCompanys[0].BaseAddress;

                if (PostedFile.ContentLength > 0)
                {
                    fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);
                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                        if (System.IO.File.Exists(fileDirectory))
                        {
                            System.IO.File.Delete(fileDirectory);
                        }
                        PostedFile.SaveAs(fileDirectory);

                        string excelConnectionString = string.Empty;
                        //connection String for xls file format.
                        //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                        ////excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileDirectory + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }
                        excelConnection.Close();
                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                        oRows = ds.Tables[0].Rows;

                        for (int i = 0; i < oRows.Count; i++)
                        {
                            oPunchLogXL = new PunchLog();
                            string sTempDate = "";
                            string pdt = "";
                            DateTime dDate = DateTime.Now;
                            string sInTime = "";
                            string sOutTime = "";

                            if (nPunchFormat == (int)(EnumPunchFormat.WithSeparateTime))
                            {
                                oPunchLogXL.CardNo = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                                if (oPunchLogXL.CardNo != "" && !string.IsNullOrEmpty(oPunchLogXL.CardNo))
                                {
                                    sInTime = Convert.ToString(oRows[i][2] == DBNull.Value ? "" : oRows[i][2]);
                                    if (ds.Tables[0].Columns.Count > 3)
                                    {
                                        sOutTime = Convert.ToString(oRows[i][3] == DBNull.Value ? "" : oRows[i][3]);
                                    }

                                    if (sInTime != "")
                                    {
                                        sTempDate = "";
                                        pdt = "";

                                        if ((Session[SessionInfo.BaseAddress]).ToString().ToUpper() == ("B007").ToUpper())
                                        {
                                            sTempDate = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                                            string sYear = sTempDate.Split('/')[0];
                                            string sMonth = sTempDate.Split('/')[1];
                                            string sDay = sTempDate.Split('/')[2];
                                            string sHour = sInTime.Split(':')[0];
                                            string sMin = sInTime.Split(':')[1];
                                            string sSecond = sInTime.Split(':')[2];
                                            dDate = DateTime.ParseExact(Convert.ToInt32(sMonth).ToString("00") + "/" + Convert.ToInt32(sDay).ToString("00") + "/" + sYear + " " + Convert.ToInt32(sHour).ToString("00") + ":" + Convert.ToInt32(sMin).ToString("00") + ":" + sSecond, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
                                            pdt = dDate.ToString("dd MMM yyyy HH:mm:ss");
                                            oPunchLogXL.PunchDateTime_ST = pdt;
                                            oPunchLogXLs.Add(oPunchLogXL);
                                        }
                                        else
                                        {
                                            sTempDate = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]) + " " + sInTime;
                                            dDate = Convert.ToDateTime(sTempDate);
                                            pdt = dDate.ToString("dd MMM yyyy HH:mm:ss");
                                            oPunchLogXL.PunchDateTime_ST = pdt;
                                            if (Convert.ToDateTime(sTempDate).ToString("HH:mm:ss") != "00:00:00") { oPunchLogXLs.Add(oPunchLogXL); }
                                        }
                                    }
                                    if (sOutTime != "")
                                    {
                                        oPunchLogXL = new PunchLog();
                                        oPunchLogXL.CardNo = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                                        sTempDate = "";
                                        pdt = "";

                                        if ((Session[SessionInfo.BaseAddress]).ToString().ToUpper() == ("/B007").ToUpper())
                                        {
                                            sTempDate = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);
                                            string sYear = sTempDate.Split('/')[0];
                                            string sMonth = sTempDate.Split('/')[1];
                                            string sDay = sTempDate.Split('/')[2];
                                            string sHour = sOutTime.Split(':')[0];
                                            string sMin = sOutTime.Split(':')[1];
                                            string sSecond = sOutTime.Split(':')[2];
                                            dDate = DateTime.ParseExact(Convert.ToInt32(sMonth).ToString("00") + "/" + Convert.ToInt32(sDay).ToString("00") + "/" + sYear + " " + Convert.ToInt32(sHour).ToString("00") + ":" + Convert.ToInt32(sMin).ToString("00") + ":" + sSecond, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
                                            pdt = dDate.ToString("dd MMM yyyy HH:mm:ss");
                                            oPunchLogXL.PunchDateTime_ST = pdt;
                                            oPunchLogXLs.Add(oPunchLogXL);
                                        }
                                        else
                                        {
                                            sTempDate = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]) + " " + sOutTime;
                                            dDate = Convert.ToDateTime(sTempDate);
                                            pdt = dDate.ToString("dd MMM yyyy HH:mm:ss");
                                            oPunchLogXL.PunchDateTime_ST = pdt;
                                            if (Convert.ToDateTime(sTempDate).ToString("HH:mm:ss") != "00:00:00") { oPunchLogXLs.Add(oPunchLogXL); }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                sTempDate = "";
                                pdt = "";
                                oPunchLogXL.CardNo = Convert.ToString(oRows[i][0] == DBNull.Value ? "" : oRows[i][0]);
                                if (oPunchLogXL.CardNo != "" && !string.IsNullOrEmpty(oPunchLogXL.CardNo))
                                {

                                    sTempDate = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]);

                                    #region This Region for Test not any Business Logic By Faruk
                                    var testfileDirectory = Server.MapPath("~/Content/faruk.txt");
                                    if (System.IO.File.Exists(testfileDirectory))
                                    {
                                        System.IO.File.Delete(testfileDirectory);
                                    }
                                    FileStream fs = null;
                                    using (fs = System.IO.File.Create(testfileDirectory))
                                    {

                                    }

                                    //using (StreamWriter sw = new StreamWriter(testfileDirectory))
                                    //{
                                    //    sw.Write(sTempDate);
                                    //    if (nPunchFormat == (int)(EnumPunchFormat.MM_DD_YY))
                                    //    {
                                    //        if ((Session[SessionInfo.BaseAddress]).ToString().ToUpper() == ("/B007").ToUpper())
                                    //        {
                                    //            string sTestMonth = sTempDate.Split('/')[0];
                                    //            string sTestDay = sTempDate.Split('/')[1];
                                    //            string sTestYearAndTime = sTempDate.Split('/')[2];
                                    //            string sTestYear = sTestYearAndTime.Split(' ')[0];
                                    //            string sTestTime = sTestYearAndTime.Split(' ')[1];
                                    //            string sTestHour = sTestTime.Split(':')[0];
                                    //            string sTestMin = sTestTime.Split(':')[1];
                                    //            string sTestAMPM = sTestYearAndTime.Split(' ')[2];
                                    //            sw.Write(Environment.NewLine);
                                    //            sw.Write(sTestMonth);
                                    //            sw.Write(Environment.NewLine);
                                    //            sw.Write(sTestDay);
                                    //            sw.Write(Environment.NewLine);
                                    //            sw.Write(sTestYear);
                                    //            sw.Write(Environment.NewLine);
                                    //            sw.Write(sTestHour);
                                    //            sw.Write(Environment.NewLine);
                                    //            sw.Write(sTestMin);
                                    //            sw.Write(Environment.NewLine);
                                    //            sw.Write(sTestAMPM);
                                    //            DateTime dTestDate = DateTime.ParseExact(Convert.ToInt32(sTestMonth).ToString("00") + "/" + Convert.ToInt32(sTestDay).ToString("00") + "/" + sTestYear + " " + Convert.ToInt32(sTestHour).ToString("00") + ":" + Convert.ToInt32(sTestMin).ToString("00") + " " + sTestAMPM, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None);
                                    //            string testpdt = dTestDate.ToString("dd MMM yyyy HH:mm:ss");
                                    //            sw.Write(Environment.NewLine);
                                    //            sw.Write(testpdt);
                                    //        }
                                    //    }
                                    //}
                                    #endregion

                                    if (nPunchFormat == (int)(EnumPunchFormat.DD_MM_YY))
                                    {
                                        string sDay = "";
                                        string sMonth = "";
                                        string sYearAndTime = "";


                                        if (sTempDate.Contains("-"))
                                        {
                                            sDay = sTempDate.Split('-')[0];
                                            sMonth = sTempDate.Split('-')[1];
                                            sYearAndTime = sTempDate.Split('-')[2];
                                            dDate = Convert.ToDateTime(sDay + "-" + sMonth + "-" + sYearAndTime);
                                        }
                                        else
                                        {
                                            if (sTempDate.Split('/').Count() < 3)
                                            {
                                                throw new Exception("Date Format is Invail For Excel Row Number :" + (i + 2).ToString());
                                            }
                                            else
                                            {
                                                sDay = sTempDate.Split('/')[0];
                                                sMonth = sTempDate.Split('/')[1];
                                                sYearAndTime = sTempDate.Split('/')[2];
                                                dDate = Convert.ToDateTime(sDay + "/" + sMonth + "/" + sYearAndTime);
                                            }
                                        }

                                        pdt = dDate.ToString("dd MMM yyyy HH:mm:ss");
                                    }
                                    else if (nPunchFormat == (int)(EnumPunchFormat.MM_DD_YY))
                                    {
                                        if ((Session[SessionInfo.BaseAddress]).ToString().ToUpper() == ("/B007").ToUpper())
                                        {
                                            string sMonth = sTempDate.Split('/')[0];
                                            string sDay = sTempDate.Split('/')[1];
                                            string sYearAndTime = sTempDate.Split('/')[2];
                                            string sYear = sYearAndTime.Split(' ')[0];
                                            string sTime = sYearAndTime.Split(' ')[1];
                                            string sHour = sTime.Split(':')[0];
                                            string sMin = sTime.Split(':')[1];
                                            string sAMPM = sYearAndTime.Split(' ')[2];
                                            dDate = DateTime.ParseExact(Convert.ToInt32(sMonth).ToString("00") + "/" + Convert.ToInt32(sDay).ToString("00") + "/" + sYear + " " + Convert.ToInt32(sHour).ToString("00") + ":" + Convert.ToInt32(sMin).ToString("00") + " " + sAMPM, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None);
                                            pdt = dDate.ToString("dd MMM yyyy HH:mm:ss");

                                            //using (StreamWriter sw = new StreamWriter(testfileDirectory))
                                            //{
                                            //    sw.Write(Environment.NewLine);
                                            //    sw.Write(pdt);
                                            //    sw.Write(Environment.NewLine);
                                            //    sw.Write(DateTime.Now.ToString("dd MMM yyyy HH:mm tt"));
                                            //}
                                        }
                                        else if (sBaseAddress == "wangs")
                                        {
                                            string sMonth = sTempDate.Split('/')[0];
                                            string sDay = sTempDate.Split('/')[1];
                                            string sYearAndTime = sTempDate.Split('/')[2];
                                            string sYear = sYearAndTime.Split(' ')[0];
                                            string sTime = sYearAndTime.Split(' ')[1];
                                            string sHour = sTime.Split(':')[0];
                                            string sMin = sTime.Split(':')[1];
                                            string sAMPM = sYearAndTime.Split(' ')[2];
                                            dDate = DateTime.ParseExact(Convert.ToInt32(sMonth).ToString("00") + "/" + Convert.ToInt32(sDay).ToString("00") + "/" + sYear + " " + Convert.ToInt32(sHour).ToString("00") + ":" + Convert.ToInt32(sMin).ToString("00") + " " + sAMPM, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None);
                                            pdt = dDate.ToString("dd MMM yyyy HH:mm:ss");
                                        }
                                        else
                                        {

                                            string sMonth = sTempDate.Split('/')[0];
                                            string sDay = sTempDate.Split('/')[1];
                                            string sYearAndTime = sTempDate.Split('/')[2];
                                            dDate = Convert.ToDateTime(sMonth + "/" + sDay + "/" + sYearAndTime);
                                            pdt = dDate.ToString("dd MMM yyyy HH:mm:ss");
                                        }
                                    }
                                    else if (nPunchFormat == (int)(EnumPunchFormat.YY_MM_DD))
                                    {
                                        string sYearAndTime = sTempDate.Split('/')[0];
                                        string sMonth = sTempDate.Split('/')[1];
                                        string sDay = sTempDate.Split('/')[2];
                                        dDate = Convert.ToDateTime(sYearAndTime + "/" + sMonth + "/" + sDay);
                                        pdt = dDate.ToString("dd MMM yyyy HH:mm:ss");
                                    }
                                    oPunchLogXL.PunchDateTime_ST = pdt;
                                    oPunchLogXLs.Add(oPunchLogXL);
                                    //if (Convert.ToDateTime(sTempDate).ToString("HH:mm:ss") != "00:00:00") { oPunchLogXLs.Add(oPunchLogXL); }
                                }
                            }
                        }
                        if (System.IO.File.Exists(fileDirectory))
                        {
                            System.IO.File.Delete(fileDirectory);
                        }
                    }
                    else
                    {
                        throw new Exception("File not supported");
                    }
                }
                return oPunchLogXLs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private Tuple<List<PunchLog>, List<PunchLog>> GetPunchLogFromText(HttpPostedFileBase PostedFile, int nPunchFormat)
        {
            List<PunchLog> oPunchLogXLs = new List<PunchLog>();
            List<PunchLog> oPunchLogXLWithErrors = new List<PunchLog>();
            PunchLog oPunchLogXL = new PunchLog();
            string fileExtension = "", fileDirectory = "";
            if (PostedFile.ContentLength > 0)
            {
                fileExtension = System.IO.Path.GetExtension(PostedFile.FileName);
                if (fileExtension == ".txt")
                {
                    fileDirectory = Server.MapPath("~/Content/") + PostedFile.FileName;
                    if (System.IO.File.Exists(fileDirectory))
                    {
                        System.IO.File.Delete(fileDirectory);
                    }
                    PostedFile.SaveAs(fileDirectory);

                    List<string> lines = System.IO.File.ReadAllLines(fileDirectory).ToList();

                    lines = lines.Where(x => !string.IsNullOrEmpty(x) && x.Trim() != "").Select(x => x = x.Trim()).ToList(); ;

                    int day = 0, month = 0, year = 0, hour = 0, minute = 0, second = 0;
                    string sProximity = "";

                    string sBaseAddress = "";
                    string sSQL = "SELECT top(1)* FROM View_Company";
                    List<Company> oCompanys = new List<Company>();
                    oCompanys = Company.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    sBaseAddress = oCompanys[0].BaseAddress;
                    if (sBaseAddress == "amg")
                    {
                        foreach (string line in lines)
                        {
                            if (line.Length >= 24)
                            {
                                try
                                {
                                    day = Convert.ToInt16(line.Substring(3, 2));
                                    month = Convert.ToInt16(line.Substring(5, 2));
                                    year = Convert.ToInt32((DateTime.Today.Year.ToString()).Substring(0, 2) + line.Substring(7, 2));

                                    hour = Convert.ToInt16(line.Substring(9, 2));
                                    minute = Convert.ToInt16(line.Substring(11, 2));
                                    second = Convert.ToInt16(line.Substring(13, 2));
                                    sProximity = line.Substring(15, 9);

                                    oPunchLogXL = new PunchLog();
                                    oPunchLogXL.PunchDateTime = new DateTime(year, month, day, hour, minute, second);
                                    oPunchLogXL.CardNo = sProximity;
                                    oPunchLogXL.PunchDateTime_ST = oPunchLogXL.PunchDateTime.ToString("dd MMM yyyy HH:mm:ss");
                                    oPunchLogXLs.Add(oPunchLogXL);
                                }
                                catch
                                {
                                    oPunchLogXL = new PunchLog();
                                    oPunchLogXL.CardNo = line;
                                    oPunchLogXLWithErrors.Add(oPunchLogXL);
                                }

                            }
                            else
                            {
                                oPunchLogXL = new PunchLog();
                                oPunchLogXL.CardNo = line;
                                oPunchLogXLWithErrors.Add(oPunchLogXL);
                            }

                        }

                    }

                    if (System.IO.File.Exists(fileDirectory))
                        System.IO.File.Delete(fileDirectory);
                }
                else
                {
                    throw new Exception("File not supported");
                }
            }

            return new Tuple<List<PunchLog>, List<PunchLog>>(oPunchLogXLs, oPunchLogXLWithErrors);
        }

        [HttpPost]
        public ActionResult ViewAttendanceProcessManagements(HttpPostedFileBase filePunchLogs, int txtEnumPunchFormat)
        {
            List<PunchLog> oPunchLogXLs = new List<PunchLog>();
            List<PunchLog> oPunchLogWithErrors = new List<PunchLog>();
            PunchLog oPunchLogXL = new PunchLog();

            try
            {
                if (filePunchLogs == null) { throw new Exception("File not Found"); }

                string extension = Path.GetExtension(filePunchLogs.FileName);

                if (extension == ".xlsx" || extension == ".xls")
                    oPunchLogXLs = this.GetPunchLogFromExcel(filePunchLogs, txtEnumPunchFormat);
                else
                {
                    var tuple = this.GetPunchLogFromText(filePunchLogs, txtEnumPunchFormat);
                    oPunchLogXLs = tuple.Item1;
                    oPunchLogWithErrors = tuple.Item2;
                }

                if (oPunchLogXLs.Count() > 0)
                    oPunchLogXLs = PunchLog.UploadXL(oPunchLogXLs, ((User)Session[SessionInfo.CurrentUser]).UserID);


                oPunchLogWithErrors.AddRange(oPunchLogXLs.Where(x => x.PunchLogID <= 0).ToList());

                if (oPunchLogWithErrors != null && oPunchLogWithErrors.Any())
                {
                    MemoryStream ms = new MemoryStream();
                    TextWriter txtWriter = new StreamWriter(ms);
                    txtWriter.WriteLine("------------- The code given in the file  doesn't found --------------");
                    foreach (PunchLog oItem in oPunchLogWithErrors)
                    {
                        txtWriter.WriteLine(oItem.CardNo);
                    }
                    txtWriter.Flush();
                    byte[] bytes = ms.ToArray();
                    ms.Close();

                    Response.Clear();
                    Response.ContentType = "application/force-download";
                    Response.AddHeader("content-disposition", "attachment;  filename=file.txt");
                    Response.BinaryWrite(bytes);
                    Response.End();
                }
                oPunchLogXLs = oPunchLogXLs.Where(x => x.PunchLogID > 0).ToList();
                ViewBag.FeedBack = (oPunchLogXLs.Any()) ? "Uploaded Successfully!" : "";
            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
            }

            if (Session[SessionInfo.MenuID] != null)
            {
                ViewBag.MenuID = (int)Session[SessionInfo.MenuID];
            }
            else
            {
                ViewBag.MenuID = 0;
            }
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            ViewBag.EnumPunchFormats = EnumObject.jGets(typeof(EnumPunchFormat));            
            return View(_oAttendanceProcessManagements);            
        }


        #endregion Import & Export
    }
}
