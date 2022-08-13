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

namespace ESimSolFinancial.Controllers
{
    public class ProcessManagementController : Controller
    {
        #region Declaration
        AttendanceProcessManagement _oAttendanceProcessManagement;
        private List<AttendanceProcessManagement> _oAttendanceProcessManagements;
        EmployeeBonusProcess _oEmployeeBonusProcess;
        private List<EmployeeBonusProcess> _oEmployeeBonusProcesss;
        EmployeeBonusProcessObject _oEmployeeBonusProcessObject;
        private List<EmployeeBonusProcessObject> _oEmployeeBonusProcessObjects;
        //string ConnStringRT = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\RTAttendanceData\\UNIS.mdb; Jet OLEDB:Database Password=unisamho;";
        #endregion

        #region Views
        public ActionResult ViewAttendanceProcessManagements(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            //sSQL = "select * from View_AttendanceProcessManagement";
            //_oAttendanceProcessManagements = AttendanceProcessManagement.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.AAPs = AttendanceAccessPoint.Gets("Select * FROM AttendanceAccessPoint Where AAPID<>0", ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<PunchLogImportFormat> oPunchLogImportFormats = new List<PunchLogImportFormat>();
            oPunchLogImportFormats = PunchLogImportFormat.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (oPunchLogImportFormats.Count > 0) { ViewBag.PunchLogImportFormats = oPunchLogImportFormats[0]; }
            ViewBag.EnumPunchFormats = Enum.GetValues(typeof(EnumPunchFormat)).Cast<EnumPunchFormat>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            //HRMShift.Gets("SELECT * FROM HRM_Shift WHERE IsActive=1", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.ProcessTypes = Enum.GetValues(typeof(EnumProcessType)).Cast<EnumProcessType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.BusinessUnits = BusinessUnit.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            List<DepartmentRequirementPolicy> oDRPs = new List<DepartmentRequirementPolicy>();
            oDRPs = DepartmentRequirementPolicy.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oDRPs = oDRPs.GroupBy(x => x.LocationID).Select(x => x.First()).ToList();
            ViewBag.Locations = oDRPs;

            return View(_oAttendanceProcessManagements);
        }

        public ActionResult ViewAttendanceProcessManagement(int nId, double ts)
        {
            _oAttendanceProcessManagement = new AttendanceProcessManagement();
            if (nId > 0)
            {
                _oAttendanceProcessManagement = AttendanceProcessManagement.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            _oAttendanceProcessManagement.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oAttendanceProcessManagement.Locations = Location.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oAttendanceProcessManagement);
        }

        public ActionResult ViewAttendanceProcessManagementsALL()
        {
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            //sSQL = "select * from View_AttendanceProcessManagement";
            //_oAttendanceProcessManagements = AttendanceProcessManagement.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oAttendanceProcessManagements);
        }

        public ActionResult ViewAttendanceProcessManagementALL(int nId, double ts)
        {
            _oAttendanceProcessManagement = new AttendanceProcessManagement();
            _oAttendanceProcessManagement.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oAttendanceProcessManagement);
        }


        #endregion

        #region IUD
        [HttpPost]
        public JsonResult AttendanceProcessManagement_IU(AttendanceProcessManagement oAttendanceProcessManagement)
        {
            _oAttendanceProcessManagement = new AttendanceProcessManagement();
            try
            {
                string sSQL = "SELECT distinct * FROM Department WHERE DepartmentID IN (SELECT DepartmentID FROM View_EmployeeOfficial WHERE IsActive=1 AND LocationID=" + oAttendanceProcessManagement.LocationID + ") ORDER BY DepartmentID";
                List<Department> oDpts = new List<Department>();
                oDpts = Department.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                _oAttendanceProcessManagement = oAttendanceProcessManagement;
                _oAttendanceProcessManagement.ProcessType = (EnumProcessType)oAttendanceProcessManagement.ProcessTypeInt;
                _oAttendanceProcessManagement.Status = (EnumProcessStatus)oAttendanceProcessManagement.StatusInt;
                _oAttendanceProcessManagement.DepartmentID = 0;//for all departments process together.
                //_oAttendanceProcessManagement.AttendanceDate = Convert.ToDateTime(oAttendanceProcessManagement.AttendanceDateInString);
                if (_oAttendanceProcessManagement.APMID > 0)
                {
                    _oAttendanceProcessManagement = _oAttendanceProcessManagement.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oAttendanceProcessManagement = _oAttendanceProcessManagement.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }

                if (_oAttendanceProcessManagement.APMID > 0)
                {
                    if (_oAttendanceProcessManagement.Status == EnumProcessStatus.Processed || _oAttendanceProcessManagement.Status == EnumProcessStatus.ReProcessed)
                    {
                        foreach (Department oItem in oDpts)
                        {
                            int nIndex = 0;
                            int nNewIndex = 1;
                            _oAttendanceProcessManagement.DepartmentID = oItem.DepartmentID;
                            while (nNewIndex != 0)
                            {
                                nNewIndex = _oAttendanceProcessManagement.ProcessAttendanceDaily_V1(nIndex, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                                nIndex = nNewIndex;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceProcessManagement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AttendanceProcessManagement_IUALL(AttendanceProcessManagement oAttendanceProcessManagement)
        {
            _oAttendanceProcessManagement = new AttendanceProcessManagement();
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            try
            {
                string sSQL = "SELECT distinct * FROM Department WHERE DepartmentID IN (SELECT DepartmentID FROM View_EmployeeOfficial)";
                List<Department> oDpts = new List<Department>();
                oDpts = Department.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oAttendanceProcessManagement = oAttendanceProcessManagement;
                _oAttendanceProcessManagement.ProcessType = (EnumProcessType)oAttendanceProcessManagement.ProcessTypeInt;
                _oAttendanceProcessManagement.Status = (EnumProcessStatus)oAttendanceProcessManagement.StatusInt;

                if (oDpts.Count > 0)
                {
                    foreach (Department oItem in oDpts)
                    {
                        AttendanceProcessManagement oAPM = new AttendanceProcessManagement();
                        oAPM = _oAttendanceProcessManagement;
                        oAPM.DepartmentID = oItem.DepartmentID;
                        if (oAPM.APMID > 0)
                        {
                            oAPM = oAPM.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        }
                        else
                        {
                            oAPM = oAPM.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        }
                        if (oAPM.ErrorMessage != "")
                        {
                            _oAttendanceProcessManagement = new AttendanceProcessManagement();
                            _oAttendanceProcessManagement.ErrorMessage = oAPM.ErrorMessage;
                            _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
                            break;
                        }
                        else
                        {
                            _oAttendanceProcessManagements.Add(oAPM);
                        }
                    }
                }
                else
                {
                    _oAttendanceProcessManagement = new AttendanceProcessManagement();
                    _oAttendanceProcessManagement.ErrorMessage = "Please Configure Employee Official information";
                    _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
                }
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
                _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AttendanceProcessManagement_StatusUpdate(string sDate, int nStatus)
        {
            _oAttendanceProcessManagement = new AttendanceProcessManagement();
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            List<AttendanceProcessManagement> oAPMs = new List<AttendanceProcessManagement>();
            try
            {
                string sSQL = "SELECT * FROM View_AttendanceProcessManagement WHERE AttendanceDate='" + sDate + "'";
                _oAttendanceProcessManagements = AttendanceProcessManagement.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (_oAttendanceProcessManagements.Count > 0)
                {
                    foreach (AttendanceProcessManagement oItem in _oAttendanceProcessManagements)
                    {
                        AttendanceProcessManagement oAPM = new AttendanceProcessManagement();
                        oAPM = oItem;
                        oAPM.Status = (EnumProcessStatus)nStatus;
                        oAPM = oAPM.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        if (oAPM.ErrorMessage != "")
                        {
                            _oAttendanceProcessManagement = new AttendanceProcessManagement();
                            _oAttendanceProcessManagement.ErrorMessage = oAPM.ErrorMessage;
                            _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
                            break;
                        }
                        else
                        {
                            oAPMs.Add(oAPM);
                        }
                    }
                }
                else
                {
                    _oAttendanceProcessManagement = new AttendanceProcessManagement();
                    _oAttendanceProcessManagement.ErrorMessage = "There is no process to " + ((EnumProcessStatus)nStatus).ToString();
                    oAPMs.Add(_oAttendanceProcessManagement);
                }
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
                oAPMs.Add(_oAttendanceProcessManagement);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAPMs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AttendanceProcessManagement_Delete(int id)
        {
            _oAttendanceProcessManagement = new AttendanceProcessManagement();
            try
            {
                _oAttendanceProcessManagement.APMID = id;
                _oAttendanceProcessManagement = _oAttendanceProcessManagement.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceProcessManagement.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Data Collection for RT
        public ActionResult ViewDataCollectionRT(double ts)
        {
            RTPunchLog oRTP = new RTPunchLog();
            return PartialView(oRTP);
        }

        [HttpPost]
        public JsonResult ProcessDataCollectionRT(List<RTPunchLog> oRTPs, string sDate, double ts)
        {
            string sSQl = "";
            //Check processed Data
            List<AttendanceProcessManagement> oAPTs = new List<AttendanceProcessManagement>();
            RTPunchLog oRTPunchLog = new RTPunchLog();
            List<RTPunchLog> oRTPunchLogs = new List<RTPunchLog>();
            try
            {
                sSQl = "SELECT * FROM View_AttendanceProcessManagement WHERE [Status]=" + (int)EnumProcessStatus.Freeze + " AND AttendanceDate='" + sDate + "'";
                oAPTs = AttendanceProcessManagement.Gets(sSQl, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oRTPunchLogs = oRTPs;
                if (oAPTs.Count <= 0)
                {
                    //Insert Data
                    if (oRTPunchLogs.Count > 0)
                    {
                        AttendanceProcessManagement oATPM = new AttendanceProcessManagement();
                        oRTPunchLog = oATPM.ProcessDataCollectionRT(oRTPunchLogs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    }
                    else
                    {
                        oRTPunchLog.ErrorMessage = "There is no data to process for this date.";
                    }
                }
                else
                {
                    oRTPunchLog.ErrorMessage = "Processed data already freezed for this date.\nYou can not collect data for this date.!!";
                }
            }
            catch (Exception ex)
            {
                oRTPunchLog = new RTPunchLog();
                oRTPunchLog.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRTPunchLog.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult ProcessDataCollectionRT(string sDate, double ts)
        //{
        //    string sSQl = "";
        //    //Check processed Data
        //    List<AttendanceProcessManagement> oAPTs = new List<AttendanceProcessManagement>();
        //    RTPunchLog oRTPunchLog = new RTPunchLog();
        //    List<RTPunchLog> oRTPunchLogs = new List<RTPunchLog>();
        //    try
        //    {
        //        sSQl = "SELECT * FROM AttendanceProcessManagement WHERE AttendanceDate='" + sDate + "'";
        //        oAPTs = AttendanceProcessManagement.Gets(sSQl, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //        if (oAPTs.Count <= 0)
        //        {
        //            sDate = sDate.Replace("-", "");
        //            sSQl = "SELECT tEnter.C_Date, tEnter.C_Time, tEnter.C_Unique FROM tEnter WHERE C_Date='" + sDate + "'";
        //            using (OleDbConnection conn = new OleDbConnection(ConnStringRT))
        //            {
        //                using (OleDbCommand cmd = new OleDbCommand(sSQl, conn))
        //                {
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.Parameters.AddWithValue("C_Date", oRTPunchLog.C_Date);
        //                    cmd.Parameters.AddWithValue("C_Time", oRTPunchLog.C_Time);
        //                    cmd.Parameters.AddWithValue("C_Unique", oRTPunchLog.C_Unique);

        //                    conn.Open();
        //                    using (OleDbDataReader reader = cmd.ExecuteReader())
        //                    {
        //                        while (reader.Read())
        //                        {
        //                            RTPunchLog oRTP = new RTPunchLog();
        //                            oRTP.C_Unique = reader["C_Unique"].ToString();
        //                            oRTP.C_Date = reader["C_Date"].ToString();
        //                            oRTP.C_Time = reader["C_Time"].ToString();
        //                            oRTPunchLogs.Add(oRTP);
        //                        }
        //                    }
        //                    conn.Close();
        //                }
        //            }
        //            //Insert Data
        //            if (oRTPunchLogs.Count > 0)
        //            {
        //                AttendanceProcessManagement oATPM = new AttendanceProcessManagement();
        //                oRTPunchLog = oATPM.ProcessDataCollectionRT(oRTPunchLogs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //            }
        //            else
        //            {
        //                oRTPunchLog.ErrorMessage = "There is no data to process for this date.";
        //            }
        //        }
        //        else
        //        {
        //            oRTPunchLog.ErrorMessage = "Already Processed for this date.";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        oRTPunchLog = new RTPunchLog();
        //        oRTPunchLog.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oRTPunchLog.ErrorMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        #endregion

        #region gets
        [HttpGet]
        public JsonResult GetAPMs(string sParam)
        {

            string date = Convert.ToString(sParam.Split('~')[0]);
            string sBUIDs = Convert.ToString(sParam.Split('~')[1]);
            string sLocIDs = Convert.ToString(sParam.Split('~')[2]);
            string sShiftID = Convert.ToString(sParam.Split('~')[3]);

            string sSQL = "SELECT * FROM View_AttendanceProcessManagement WHERE AttendanceDate='" + date + "'";
            if (!string.IsNullOrEmpty(sBUIDs))
            {
                sSQL += " AND BusinessUnitID IN(" + sBUIDs + ")";
            }
            if (!string.IsNullOrEmpty(sLocIDs))
            {
                sSQL += " AND LocationID IN(" + sLocIDs + ")";
            }
            if (!string.IsNullOrEmpty(sShiftID))
            {
                sSQL += " AND ShiftID IN(" + sShiftID + ")";
            }

            List<AttendanceProcessManagement> oAPMs = new List<AttendanceProcessManagement>();
            try
            {
                oAPMs = AttendanceProcessManagement.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
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


        [HttpGet]
        public JsonResult GetsShiftRefresh(string sParam)
        {
            int sBUID = Convert.ToInt32(sParam.Split('~')[0]);
            //int sLocID = Convert.ToInt32(sParam.Split('~')[1]);
            string sLocIDs = Convert.ToString(sParam.Split('~')[1]);

            string sSQL = "SELECT * FROM HRM_Shift WHERE IsActive=1 and ShiftID in(SELECT ShiftID FROM ShiftBULocConfigure where BUID=" +sBUID + "  AND LocationID IN( " + sLocIDs + "))";

            List<HRMShift> oHRSs = new List<HRMShift>();
            try
            {
                oHRSs = HRMShift.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oHRSs[0] = new HRMShift();
                oHRSs[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oHRSs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PayrollProcessManagement
        public ActionResult ViewProcessSalaryStructure()
        {
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            oPayrollProcessManagement.Locations = Location.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(oPayrollProcessManagement);
        }

        public ActionResult ViewPayrollProcessManagements(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<PayrollProcessManagement> oPayrollProcessManagements = new List<PayrollProcessManagement>();
            oPayrollProcessManagements = PayrollProcessManagement.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(oPayrollProcessManagements);
        }
        

        [HttpPost]
        public JsonResult PayrollProcess(PayrollProcessManagement oPayrollProcessManagement)
        {
            try
            {
                string sAllowanceIDs = "";
                string sMessage = "";
                oPayrollProcessManagement.Status = EnumProcessStatus.Processed;
                oPayrollProcessManagement.PaymentCycle = (EnumPaymentCycle)oPayrollProcessManagement.PaymentCycleInt;
                sAllowanceIDs = oPayrollProcessManagement.AllowanceIDs;
                oPayrollProcessManagement = oPayrollProcessManagement.IUD(((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (oPayrollProcessManagement.PPMID > 0)
                {
                    List<SalaryScheme> oSSs = new List<SalaryScheme>();
                    string sSQL = "SELECT * FROM SalaryScheme WHERE IsActive=1 AND SalarySchemeID IN ("
                                + "SELECT SalarySchemeID FROM EmployeeSalaryStructure)";
                    oSSs = SalaryScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    if (sAllowanceIDs == null) { sAllowanceIDs = ""; }
                    foreach (SalaryScheme oItem in oSSs)
                    {
                        PayrollProcessManagement oPPM = new PayrollProcessManagement();
                        sMessage = oPPM.ProcessPayroll(oItem.SalarySchemeID, oPayrollProcessManagement.LocationID, oPayrollProcessManagement.SalaryFrom,
                            oPayrollProcessManagement.SalaryTo, sAllowanceIDs, oPayrollProcessManagement.PPMID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        if (sMessage != "")
                        {
                            throw new Exception(sMessage);
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
        public JsonResult PayrollProcessByEmployee(PayrollProcessManagement oPayrollProcessManagement)
        {
            try
            {
                int nIndex = 0;
                int nNewIndex = 1;
                string sAllowanceIDs = "";
                oPayrollProcessManagement.Status = EnumProcessStatus.Processed;
                oPayrollProcessManagement.PaymentCycle = (EnumPaymentCycle)oPayrollProcessManagement.PaymentCycleInt;
                sAllowanceIDs = oPayrollProcessManagement.AllowanceIDs;
                oPayrollProcessManagement = oPayrollProcessManagement.IUD(((User)(Session[SessionInfo.CurrentUser])).UserID);

                if (oPayrollProcessManagement.PPMID > 0)
                {
                    if (sAllowanceIDs == null) { sAllowanceIDs = ""; }
                    if (oPayrollProcessManagement.Status == EnumProcessStatus.Processed || oPayrollProcessManagement.Status == EnumProcessStatus.ReProcessed)
                    {
                        while (nNewIndex != 0)
                        {
                            nNewIndex = oPayrollProcessManagement.ProcessPayrollByEmployee(nIndex, oPayrollProcessManagement.LocationID, oPayrollProcessManagement.SalaryFrom,
                                        oPayrollProcessManagement.SalaryTo, sAllowanceIDs, oPayrollProcessManagement.PPMID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
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

        [HttpPost]
        public JsonResult SearchPayroll(string sBU, string sLocationID, int nMonthID, int nYear)
        {
            List<PayrollProcessManagement> oPayrollProcessManagements = new List<PayrollProcessManagement>();
            try
            {
                string sSql = "SELECT * FROM View_PayrollProcessManagement WHERE MonthID=" + nMonthID + " AND DATEPART(YYYY,SalaryTo)=" + nYear;
                if (sBU.Trim() != "" && sBU.Trim() != "0")
                {
                    sSql = sSql + " AND BusinessUnitID=" + sBU;
                }
                if (sLocationID.Trim() != "")
                {
                    sSql = sSql + " AND LocationID=" + sLocationID;
                }
                //if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                //{
                //    sSql = sSql + "AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
                //}
                oPayrollProcessManagements = PayrollProcessManagement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oPayrollProcessManagements.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oPayrollProcessManagements = new List<PayrollProcessManagement>();
                PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
                oPayrollProcessManagements.Add(oPayrollProcessManagement);
                oPayrollProcessManagements[0].ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPayrollProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult SearchPayrollByPDate(PayrollProcessManagement oPayrollProcessManagement)
        {
            List<PayrollProcessManagement> oPayrollProcessManagements = new List<PayrollProcessManagement>();
            try
            {
                string sSql = "SELECT * FROM View_PayrollProcessManagement WHERE ProcessDate='" + oPayrollProcessManagement.ProcessDate + "'";
                oPayrollProcessManagements = PayrollProcessManagement.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oPayrollProcessManagements.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oPayrollProcessManagements = new List<PayrollProcessManagement>();
                oPayrollProcessManagement = new PayrollProcessManagement();
                oPayrollProcessManagements.Add(oPayrollProcessManagement);
                oPayrollProcessManagements[0].ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPayrollProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSalaryStructure(int nId, double ts)//nId=EmployeeID 
        {
            EmployeeSalaryStructure oEmployeeSalaryStructure = new EmployeeSalaryStructure();
            try
            {
                string Ssql = "SELECT * FROM View_EmployeeSalaryStructure WHERE EmployeeID=" + nId;
                oEmployeeSalaryStructure = EmployeeSalaryStructure.Get(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oEmployeeSalaryStructure = new EmployeeSalaryStructure();
                oEmployeeSalaryStructure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEmployeeSalaryStructure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult PPM_StatusUpdate(int nPPM, int nStatus)
        {
            PayrollProcessManagement oPPM = new PayrollProcessManagement();
            try
            {
                if (nPPM > 0)
                {
                    oPPM.PPMID = nPPM;
                    oPPM.Status = (EnumProcessStatus)nStatus;
                    oPPM = oPPM.IUD(((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oPPM = new PayrollProcessManagement();
                    oPPM.ErrorMessage = "Invalid Payroll Process.";
                }
            }
            catch (Exception ex)
            {
                oPPM = new PayrollProcessManagement();
                oPPM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPPM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PPM_StatusUnfreeze(int nPPM)
        {
            PayrollProcessManagement oPPM = new PayrollProcessManagement();
            try
            {
                if (nPPM > 0)
                {
                    oPPM.PPMID = nPPM;
                    oPPM = PayrollProcessManagement.PPM_Unfreeze(oPPM.PPMID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oPPM = new PayrollProcessManagement();
                    oPPM.ErrorMessage = "Invalid Payroll Process.";
                }
            }
            catch (Exception ex)
            {
                oPPM = new PayrollProcessManagement();
                oPPM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPPM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult PayrollProcessDelete(int nPPMID)
        {
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            try
            {
                oPayrollProcessManagement = oPayrollProcessManagement.Delete(nPPMID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
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

        #region LoadEnumAllowanceCondition
        public class EnumLoad
        {
            public EnumLoad()
            {
                int Id = 0;
                string Value = "";
            }

            public int Id { get; set; }
            public string Value { get; set; }
        }

        [HttpGet]
        public JsonResult LoadEnumAllowanceCondition()
        {
            List<EnumLoad> oEnumLoads = new List<EnumLoad>();
            EnumLoad oEnumLoad = new EnumLoad();
            try
            {
                foreach (int oItem in Enum.GetValues(typeof(EnumAllowanceCondition)))
                {
                    if (oItem != 0)
                    {
                        oEnumLoad = new EnumLoad();
                        oEnumLoad.Id = oItem;
                        oEnumLoad.Value = ((EnumAllowanceCondition)oItem).ToString();
                        oEnumLoads.Add(oEnumLoad);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEnumLoads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult ProcessDataCollection(string sAAPIDs, string sDate)
        {
            string sSQl_AAP = "";
            string sConnectionString = "";
            string sSQl_APM = "";
            List<AttendanceProcessManagement> oAPMs = new List<AttendanceProcessManagement>();
            AttendanceProcessManagement oAPM = new AttendanceProcessManagement();
            List<AttendanceAccessPoint> oAPPs = new List<AttendanceAccessPoint>();
            RTPunchLog oRTPunchLog = new RTPunchLog();
            List<RTPunchLog> oRTPunchLogs = new List<RTPunchLog>();

            try
            {
                sSQl_APM = "SELECT * FROM View_AttendanceProcessManagement WHERE [Status]=" + (int)EnumProcessStatus.Freeze + " AND AttendanceDate='" + sDate + "'";
                oAPMs = AttendanceProcessManagement.Gets(sSQl_APM, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oAPMs.Count <= 0)
                {
                    if (sAAPIDs != null || sAAPIDs != "")
                    {
                        sSQl_AAP = "SELECT * FROM AttendanceAccessPoint WHERE AAPID IN(" + sAAPIDs + ")";
                        oAPPs = AttendanceAccessPoint.Gets(sSQl_AAP, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    }
                    IDbConnection oSQLConn;
                    foreach (AttendanceAccessPoint oAAP in oAPPs)
                    {
                        string sSqlQuery = "";
                        if (oAAP.Query != "" || oAAP.Query != null)
                        {
                            sSqlQuery = oAAP.Query;
                        }
                        else
                        {
                            sSqlQuery = "select * from  RTPunchLog WHERE C_Date='" + sDate + "'";
                        }
                        sConnectionString = "";
                        sConnectionString = "Data Source=" + oAAP.DataSource + "; User ID=" + oAAP.DBLoginID + "; password=" + oAAP.DBPassword + "; Initial Catalog=" + oAAP.DBName;
                        oSQLConn = new SqlConnection(sConnectionString);
                        IDbCommand cmd = oSQLConn.CreateCommand();
                        IDataReader reader;
                        cmd.CommandText = sSqlQuery;
                        cmd.CommandType = CommandType.Text;
                        oSQLConn.Open();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            DateTime dDAte = DateTime.Now;
                            oRTPunchLog = new RTPunchLog();
                            oRTPunchLog.RTId = (int)reader["Id"];
                            dDAte = Convert.ToDateTime((string)reader["C_Date"]);
                            oRTPunchLog.C_Date = dDAte.ToString("yyyy") + dDAte.ToString("MM") + dDAte.ToString("dd");
                            dDAte = DateTime.Now;
                            dDAte = Convert.ToDateTime((string)reader["C_Time"]);
                            oRTPunchLog.C_Time = dDAte.ToString("hh") + dDAte.ToString("mm") + dDAte.ToString("ss");
                            oRTPunchLog.C_Unique = (string)reader["C_Unique"];
                            oRTPunchLogs.Add(oRTPunchLog);
                        }
                        oSQLConn.Close();

                        if (oRTPunchLogs.Count > 0)
                        {
                            AttendanceProcessManagement oATPM = new AttendanceProcessManagement();
                            oRTPunchLog = oATPM.ProcessDataCollectionRT(oRTPunchLogs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                        }
                        else
                        {
                            oRTPunchLog.ErrorMessage = "There is no data to process for this date.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oRTPunchLog = new RTPunchLog();
                oRTPunchLog.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRTPunchLog.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Employee Wise Reprocess

        [HttpPost]
        public JsonResult EmployeeWiseReprocess(string sTemp)
        {
            AttendanceDaily oAttendanceDaily = new AttendanceDaily();
            List<AttendanceDaily> oAttendanceDailys = new List<AttendanceDaily>();

            try
            {
                int EmployeeID = Convert.ToInt32(sTemp.Split('~')[0]);
                DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
                DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

                oAttendanceDailys = AttendanceDaily.EmployeeWiseReprocess(EmployeeID, Startdate, EndDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oAttendanceDailys.Count <= 0)
                {
                    throw new Exception("Nothing to process !");
                }

            }
            catch (Exception ex)
            {
                oAttendanceDaily = new AttendanceDaily();
                oAttendanceDailys = new List<AttendanceDaily>();
                oAttendanceDaily.ErrorMessage = ex.Message;
                oAttendanceDailys.Add(oAttendanceDaily);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmployeeWiseReprocessComp(string sTemp)
        {
            AttendanceDaily_ZN oAttendanceDaily = new AttendanceDaily_ZN();
            List<AttendanceDaily_ZN> oAttendanceDailys = new List<AttendanceDaily_ZN>();

            try
            {
                int EmployeeID = Convert.ToInt32(sTemp.Split('~')[0]);
                DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
                DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

                oAttendanceDailys = AttendanceDaily_ZN.EmployeeWiseReprocessComp(EmployeeID, Startdate, EndDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oAttendanceDailys.Count <= 0)
                {
                    throw new Exception("Nothing to process !");
                }

            }
            catch (Exception ex)
            {
                oAttendanceDaily = new AttendanceDaily_ZN();
                oAttendanceDailys = new List<AttendanceDaily_ZN>();
                oAttendanceDaily.ErrorMessage = ex.Message;
                oAttendanceDailys.Add(oAttendanceDaily);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oAttendanceDailys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Reprocess

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
                                            sTempDate = Convert.ToString(oRows[i][1] == DBNull.Value ? "" : oRows[i][1]) +" " + sInTime;
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

                //if (oPunchLogXLs.Count > 0)
                //{
                //    oPunchLogXLs[0].ErrorMessage = "Uploaded Successfully!";
                //}
                //else
                //{
                //    oPunchLogXLs = new List<PunchLog>();
                //    oPunchLogXL = new PunchLog();
                //    oPunchLogXL.ErrorMessage = "nothing to upload or these employees alraedy uploaded!";
                //    oPunchLogXLs.Add(oPunchLogXL);
                //}
                ViewBag.FeedBack = (oPunchLogXLs.Any()) ? "Uploaded Successfully!" : "";// oPunchLogXLs[0].ErrorMessage;
            }
            catch (Exception ex)
            {
                ViewBag.FeedBack = ex.Message;
                _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
                //return View(_oAttendanceProcessManagements);
            }

            List<PunchLogImportFormat> oPunchLogImportFormats = new List<PunchLogImportFormat>();
            oPunchLogImportFormats = PunchLogImportFormat.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            if (oPunchLogImportFormats.Count > 0) { ViewBag.PunchLogImportFormats = oPunchLogImportFormats[0]; }
            ViewBag.EnumPunchFormats = Enum.GetValues(typeof(EnumPunchFormat)).Cast<EnumPunchFormat>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            ViewBag.AAPs = AttendanceAccessPoint.Gets("Select * FROM AttendanceAccessPoint Where AAPID<>0", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Shifts = HRMShift.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.Locations = Location.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ProcessTypes = Enum.GetValues(typeof(EnumProcessType)).Cast<EnumProcessType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();

            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            return View(_oAttendanceProcessManagements);
            //return RedirectToAction("ImportAttendanceFromExcel", "AttendanceUpload_XL", new { menuid = (int)Session[SessionInfo.MenuID] });
        }


        #endregion Import & Export

        #region Attendance Process V1(multiple)
        [HttpPost]
        public JsonResult AttendanceProcessManagement_Process_V1(AttendanceProcessManagement oAttendanceProcessManagement)
        {
            _oAttendanceProcessManagement = new AttendanceProcessManagement();
            AttendanceProcessManagement oAttPM = new AttendanceProcessManagement();
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();

            try
            {
                foreach (Location oLocation in oAttendanceProcessManagement.Locations)
                {
                    string sSQL = "SELECT distinct * FROM Department WHERE DepartmentID IN (SELECT DepartmentID FROM View_EmployeeOfficial WHERE IsActive=1 AND LocationID=" + oLocation.LocationID + " AND BusinessUnitID=" + oAttendanceProcessManagement.BusinessUnitID + ") ORDER BY DepartmentID";
                    List<Department> oDpts = new List<Department>();
                    oDpts = Department.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                    _oAttendanceProcessManagement = oAttendanceProcessManagement;
                    _oAttendanceProcessManagement.ProcessType = (EnumProcessType)oAttendanceProcessManagement.ProcessTypeInt;
                    _oAttendanceProcessManagement.Status = (EnumProcessStatus)oAttendanceProcessManagement.StatusInt;
                    _oAttendanceProcessManagement.DepartmentID = 0;//for all departments process together.
                    _oAttendanceProcessManagement.LocationID = oLocation.LocationID;

                    string sSQL_Shift = "SELECT * FROM HRM_Shift WHERE IsActive=1 and ShiftID in(SELECT ShiftID FROM ShiftBULocConfigure where BUID=" + oAttendanceProcessManagement.BusinessUnitID + "  AND LocationID = " + oLocation.LocationID + ")";

                    List<HRMShift> oHRSs = new List<HRMShift>();
                    oHRSs = HRMShift.Gets(sSQL_Shift, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                    List<HRMShift> oHRS_Temp = new List<HRMShift>();

                    for (int i = 0; i < oAttendanceProcessManagement.Shifts.Count; i++)
                    {
                        for (int j = 0; j < oHRSs.Count; j++)
                        {
                            if (oAttendanceProcessManagement.Shifts[i].ShiftID == oHRSs[j].ShiftID)
                            {
                                oHRS_Temp.Add(oAttendanceProcessManagement.Shifts[i]);
                            }
                        }
                    }

                    foreach (HRMShift oHRMShift in oHRS_Temp)
                    {
                        List<EmployeeOfficial> objEmployeeOfficials = new List<EmployeeOfficial>();
                        objEmployeeOfficials = EmployeeOfficial.Gets("SELECT TOP(1)* FROM EmployeeOfficial WHERE CurrentShiftID = " + oHRMShift.ShiftID + " and IsActive = 1", ((User)(Session[SessionInfo.CurrentUser])).UserID);

                        //if (objEmployeeOfficials.Count > 0 && objEmployeeOfficials[0].EmployeeID > 0)
                        //{
                            _oAttendanceProcessManagement.ShiftID = oHRMShift.ShiftID;
                            oAttPM = new AttendanceProcessManagement();
                            oAttPM = _oAttendanceProcessManagement.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                            if (oAttPM.APMID > 0)
                            {
                                _oAttendanceProcessManagements.Add(oAttPM);
                                if (oAttPM.Status == EnumProcessStatus.Processed || oAttPM.Status == EnumProcessStatus.ReProcessed)
                                {
                                    foreach (Department oItem in oDpts)
                                    {
                                        int nIndex = 0;
                                        int nNewIndex = 1;
                                        oAttPM.DepartmentID = oItem.DepartmentID;
                                        while (nNewIndex != 0)
                                        {
                                            nNewIndex = oAttPM.ProcessAttendanceDaily_V1(nIndex, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                                            nIndex = nNewIndex;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (oAttPM.ErrorMessage != "")
                                {
                                    throw new Exception(oAttPM.ErrorMessage);
                                }
                                break;
                            }
                        //}
                        
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
        public JsonResult AttendanceProcessManagement_ReProcess_V1(AttendanceProcessManagement oAttendanceProcessManagement)
        {
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            try
            {
                foreach (AttendanceProcessManagement oItem in oAttendanceProcessManagement.AttendanceProcessManagements)
                {
                    string sSQL = "SELECT distinct * FROM Department WHERE DepartmentID IN (SELECT DepartmentID FROM View_EmployeeOfficial WHERE IsActive=1 AND LocationID=" + oItem.LocationID + " AND BusinessUnitID=" + oItem.BusinessUnitID + ") ORDER BY DepartmentID";
                    List<Department> oDpts = new List<Department>();
                    oDpts = Department.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                    _oAttendanceProcessManagement = new AttendanceProcessManagement();
                    _oAttendanceProcessManagement = oItem;
                    _oAttendanceProcessManagement.ProcessType = EnumProcessType.DailyProcess;
                    _oAttendanceProcessManagement.Status = EnumProcessStatus.ReProcessed;

                    if (_oAttendanceProcessManagement.APMID > 0)
                    {
                        _oAttendanceProcessManagement = _oAttendanceProcessManagement.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    }
                    if (_oAttendanceProcessManagement.APMID > 0)
                    {
                        _oAttendanceProcessManagements.Add(_oAttendanceProcessManagement);
                        if (_oAttendanceProcessManagement.Status == EnumProcessStatus.Processed || _oAttendanceProcessManagement.Status == EnumProcessStatus.ReProcessed)
                        {
                            foreach (Department oDpt in oDpts)
                            {
                                int nIndex = 0;
                                int nNewIndex = 1;
                                _oAttendanceProcessManagement.DepartmentID = oDpt.DepartmentID;
                                while (nNewIndex != 0)
                                {
                                    nNewIndex = _oAttendanceProcessManagement.ProcessAttendanceDaily_V1(nIndex, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                                    nIndex = nNewIndex;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_oAttendanceProcessManagement.ErrorMessage != "") { throw new Exception(_oAttendanceProcessManagement.ErrorMessage); }
                        else { throw new Exception("Nothing to Reprocess !"); }
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
        public JsonResult AttendanceProcessManagement_Rollback_Freeze(AttendanceProcessManagement oAttendanceProcessManagement)
        {
            try
            {
                foreach (AttendanceProcessManagement oItem in oAttendanceProcessManagement.AttendanceProcessManagements)
                {
                    _oAttendanceProcessManagement = new AttendanceProcessManagement();
                    _oAttendanceProcessManagement = oItem;
                    _oAttendanceProcessManagement.ProcessType = EnumProcessType.DailyProcess;
                    _oAttendanceProcessManagement.Status = (EnumProcessStatus)oAttendanceProcessManagement.StatusInt; ;
                    if (_oAttendanceProcessManagement.APMID > 0)
                    {
                        _oAttendanceProcessManagement = _oAttendanceProcessManagement.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    }
                    if (_oAttendanceProcessManagement.ErrorMessage != "" && _oAttendanceProcessManagement.ErrorMessage != null)
                    { throw new Exception(_oAttendanceProcessManagement.ErrorMessage); }
                }
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceProcessManagement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Attendance Process V1(multiple)

        #region ProcessBeforeSalary
        public ActionResult View_ProcessBeforeSalarys(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oAttendanceProcessManagements = new List<AttendanceProcessManagement>();
            return View(_oAttendanceProcessManagements);
        }

        [HttpPost]
        public JsonResult ProcessBreezeAbsent(DateTime StartDate, DateTime EndDate, double ts)
        {
            _oAttendanceProcessManagement = new AttendanceProcessManagement();
            try
            {
                _oAttendanceProcessManagement = _oAttendanceProcessManagement.ProcessBreezeAbsent(StartDate, EndDate, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oAttendanceProcessManagement = new AttendanceProcessManagement();
                _oAttendanceProcessManagement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAttendanceProcessManagement.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion ProcessBeforeSalary



        #region Compliance Payroll Process Management
        [HttpPost]
        public JsonResult PPM_StatusUpdateComp(int nPPM, int nStatus)
        {
            PayrollProcessManagement oPPM = new PayrollProcessManagement();
            try
            {
                if (nPPM > 0)
                {
                    oPPM.PPMID = nPPM;
                    oPPM.Status = (EnumProcessStatus)nStatus;
                    oPPM = oPPM.IUDComp(((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oPPM = new PayrollProcessManagement();
                    oPPM.ErrorMessage = "Invalid Payroll Process.";
                }
            }
            catch (Exception ex)
            {
                oPPM = new PayrollProcessManagement();
                oPPM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPPM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PPM_StatusUnfreezeComp(int nPPM)
        {
            PayrollProcessManagement oPPM = new PayrollProcessManagement();
            try
            {
                if (nPPM > 0)
                {
                    oPPM.PPMID = nPPM;
                    oPPM = PayrollProcessManagement.PPM_UnfreezeComp(oPPM.PPMID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oPPM = new PayrollProcessManagement();
                    oPPM.ErrorMessage = "Invalid Payroll Process.";
                }
            }
            catch (Exception ex)
            {
                oPPM = new PayrollProcessManagement();
                oPPM.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPPM);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult PayrollProcessDeleteComp(int nPPMID)
        {
            PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
            try
            {
                oPayrollProcessManagement = oPayrollProcessManagement.DeleteComp(nPPMID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
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
        public JsonResult SearchPayrollComp(string sBU, string sLocationID, int nMonthID, int nYear)
        {
            List<PayrollProcessManagement> oPayrollProcessManagements = new List<PayrollProcessManagement>();
            try
            {
                string sSql = "SELECT * FROM View_CompliancePayrollProcessManagement WHERE MonthID=" + nMonthID + " AND DATEPART(YYYY,SalaryTo)=" + nYear;
                if (sBU.Trim() != "" && sBU.Trim() != "0")
                {
                    sSql = sSql + " AND BusinessUnitID=" + sBU;
                }
                if (sLocationID.Trim() != "")
                {
                    sSql = sSql + " AND LocationID=" + sLocationID;
                }
                //if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                //{
                //    sSql = sSql + "AND DepartmentID IN(SELECT DepartmentID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + "))";
                //}
                oPayrollProcessManagements = PayrollProcessManagement.GetsComp(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oPayrollProcessManagements.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                oPayrollProcessManagements = new List<PayrollProcessManagement>();
                PayrollProcessManagement oPayrollProcessManagement = new PayrollProcessManagement();
                oPayrollProcessManagements.Add(oPayrollProcessManagement);
                oPayrollProcessManagements[0].ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPayrollProcessManagements);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ProcessAttendanceComplianceAsPerEdit(string sTemp)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            string sMessage = "";

            try
            {
                string EmployeeID = sTemp.Split('~')[0];
                DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
                DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
                int nMOCID = Convert.ToInt32(sTemp.Split('~')[3]);
                string sLocationIDs = sTemp.Split('~')[4];
                string sBUIDs = sTemp.Split('~')[5];
                //EmployeeWiseReprocessComp

                int nIndex = 0;
                int nNewIndex = 1;
                while (nNewIndex != 0)
                {
                    nNewIndex = oAttendanceDaily_ZN.ProcessCompAsPerEdit(EmployeeID, Startdate, EndDate, nMOCID, nIndex, sLocationIDs, sBUIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    nIndex = nNewIndex;
                }
                sMessage = "Success";
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}
