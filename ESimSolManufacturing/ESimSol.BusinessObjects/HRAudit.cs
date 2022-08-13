using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;

namespace ESimSol.BusinessObjects
{
    #region HRAudit
    public class HRAudit
    {
        public HRAudit()
        {

            EmployeeID = 0;
            ESID = 0;
            TotalDays = 0;
            NoRecord = 0;
            TotalOSD = 0;
            TotalPresent = 0;
            TotalAbsent = 0;
            TotalDayoff = 0;
            TotalHoliday = 0;
            TotalLeave = 0;
            EarlyDays = 0;
            AvgEarly = 0;
            LateDays = 0;
            LateMin = 0;
            AvgLate = 0;
            FirstOT = 0;
            SecondOT = 0;
            OTHours = 0;
            AvgOT = 0;
            OTRate = 0;
            OTAmount = 0;
            EWD = 0;
            EmployeeName = "";
            Code = "";
            JoiningDate = DateTime.MinValue;
            ConfirmationDate = DateTime.MinValue;
            BUName = "";
            LocationName = "";
            DepartmentName = "";
            DesignationName = "";
            EmployeeTypeName = "";
            Gender = "";
            Gross = 0;
            NetAmount = 0;
            LastGross = 0;
            IncrementAmount = 0;
            LastIncrement = DateTime.MinValue;
            TotalPunch = 0;
            PunchNotFound = 0;
            OnePunchFound = 0;
            ExpectedWorkingHour = 0;
            ExpectedWorkingHourWithOT = 0;
            TotalWorkingHour = 0;
            TotalWorkingHourWithOT = 0;
            ExpectedAvgWorkingHour = 0;
            WDPM = 0;
            PPM = 0;
            APM= 0;
            LWPPM = 0;
            LatePM = 0;
            EarlyPM = 0;
            ManualPM = 0;
            PNFPM = 0;
            ODPFPM = 0;
            AvgBOAPM = 0;
            EarlyMin = 0;
            LPM = 0;
            TotalManual = 0;
            ErrorMessage = "";
            UploadedFinancial = 0;
            EmployeeSalaryDetails = new List<EmployeeSalaryDetail>();
            LeaveLedgerReports = new List<LeaveLedgerReport>();
            BenefitOnAttendanceEmployeeLedgers = new List<BenefitOnAttendanceEmployeeLedger>();
        }

        #region Properties
        public DateTime JoiningDate { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public DateTime LastIncrement { get; set; }
        public string EmployeeTypeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string LocationName { get; set; }
        public string BUName { get; set; }
        public string EmployeeName { get; set; }
        public string Code { get; set; }
        public int TotalManual { get; set; }
        public int EWD { get; set; }
        public int EarlyMin { get; set; }
        public int ESID { get; set; }
        public int TotalDays { get; set; }
        public int NoRecord { get; set; }
        public int TotalOSD { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalDayoff { get; set; }
        public int TotalHoliday { get; set; }
        public int TotalLeave { get; set; }
        public int EarlyDays { get; set; }
        public int LateDays { get; set; }
        public int LateMin { get; set; }
        public double AvgBOAPM { get; set; }
        public double ODPFPM { get; set; }
        public double PNFPM { get; set; }
        public double ManualPM { get; set; }
        public double EarlyPM { get; set; }
        public double LatePM { get; set; }
        public double LWPPM { get; set; }
        public double APM { get; set; }
        public double PPM { get; set; }
        public double LPM { get; set; }
        public double WDPM { get; set; }
        public double UploadedFinancial { get; set; }
        public double ExpectedAvgWorkingHour { get; set; }
        public double TotalWorkingHourWithOT { get; set; }
        public double TotalWorkingHour { get; set; }
        public double ExpectedWorkingHourWithOT { get; set; }
        public double ExpectedWorkingHour { get; set; }
        public double OnePunchFound { get; set; }
        public double PunchNotFound { get; set; }
        public double TotalPunch { get; set; }
        public double IncrementAmount { get; set; }
        public double LastGross { get; set; }
        public double NetAmount { get; set; }
        public double Gross { get; set; }
        public string Gender { get; set; }
        public double AvgEarly { get; set; }
        public double AvgLate { get; set; }
        public double OTAmount { get; set; }
        public double OTRate { get; set; }
        public double AvgOT { get; set; }
        public double OTHours { get; set; }
        public double SecondOT { get; set; }
        public double FirstOT { get; set; }
        public int EmployeeID { get; set; }
        public string ErrorMessage { get; set; }
        public List<EmployeeSalaryDetail> EmployeeSalaryDetails { get; set; }
        public List<LeaveLedgerReport> LeaveLedgerReports { get; set; }
        public List<BenefitOnAttendanceEmployeeLedger> BenefitOnAttendanceEmployeeLedgers { get; set; }
        #endregion

        public string JoiningDateSt
        {
            get
            {
                return (this.JoiningDate == DateTime.MinValue) ? "-" : this.JoiningDate.ToString("dd MMM yyyy");
            }
        }
        public string ConfirmationDateSt
        {
            get
            {
                return (this.ConfirmationDate == DateTime.MinValue) ? "-" : this.ConfirmationDate.ToString("dd MMM yyyy");
            }
        }
        public string LastIncrementSt
        {
            get
            {
                return (this.LastIncrement == DateTime.MinValue) ? "-" : this.LastIncrement.ToString("dd MMM yyyy");
            }
        }
        #region Functions


        public static List<HRAudit> Gets(string sSQL, long nUserID)
        {
            return HRAudit.Service.Gets(sSQL, nUserID);
        }
        public static HRAudit Get(string sSQL, long nUserID)
        {
            return HRAudit.Service.Get(sSQL, nUserID);
        }
        public static DataSet GetAuditReport(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet, double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance, int nPayType)
        {
            return HRAudit.Service.GetAuditReport(sBU, sLocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, sBlockIDs, sGroupIDs, sEmpIDs, nMonthID, nYear, bNewJoin, IsOutSheet, nStartSalaryRange, nEndSalaryRange, IsCompliance, nPayType);
        }
        #endregion



        #region User Define Functions
        #region Object Mapping
        private static HRAudit MappingObject(DataRow oDataRow)
        {
            HRAudit oHRAudit = new HRAudit();
            oHRAudit.EmployeeID = (oDataRow["EmployeeID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["EmployeeID"]);
            oHRAudit.TotalManual = (oDataRow["TotalManual"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["TotalManual"]);
            oHRAudit.EWD = (oDataRow["EWD"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["EWD"]);
            oHRAudit.ESID = (oDataRow["ESID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["ESID"]);
            oHRAudit.TotalDays = (oDataRow["TotalDays"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["TotalDays"]);
            oHRAudit.NoRecord = (oDataRow["NoRecord"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["NoRecord"]);
            oHRAudit.TotalOSD = (oDataRow["TotalOSD"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["TotalOSD"]);
            oHRAudit.TotalPresent = (oDataRow["TotalPresent"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["TotalPresent"]);
            oHRAudit.TotalAbsent = (oDataRow["TotalAbsent"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["TotalAbsent"]);
            oHRAudit.TotalDayoff = (oDataRow["TotalDayoff"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["TotalDayoff"]);
            oHRAudit.TotalHoliday = (oDataRow["TotalHoliday"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["TotalHoliday"]);
            oHRAudit.TotalLeave = (oDataRow["TotalLeave"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["TotalLeave"]);
            oHRAudit.EarlyDays = (oDataRow["EarlyDays"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["EarlyDays"]);
            oHRAudit.LateDays = (oDataRow["LateDays"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["LateDays"]);
            oHRAudit.LateMin = (oDataRow["LateMin"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["LateMin"]);
            oHRAudit.EarlyMin = (oDataRow["EarlyMin"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["EarlyMin"]);
            oHRAudit.JoiningDate = (oDataRow["JoiningDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oDataRow["JoiningDate"]);
            oHRAudit.ConfirmationDate = (oDataRow["ConfirmationDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oDataRow["ConfirmationDate"]);
            oHRAudit.LastIncrement = (oDataRow["LastIncrement"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(oDataRow["LastIncrement"]);


            oHRAudit.UploadedFinancial = (oDataRow["UploadedFinancial"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["UploadedFinancial"]);
            oHRAudit.LPM = (oDataRow["LPM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["LPM"]);
            oHRAudit.FirstOT = (oDataRow["FirstOT"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["FirstOT"]);
            oHRAudit.SecondOT = (oDataRow["SecondOT"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["SecondOT"]);
            oHRAudit.OTHours = (oDataRow["OTHours"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["OTHours"]);
            oHRAudit.AvgOT = (oDataRow["AvgOT"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["AvgOT"]);
            oHRAudit.OTRate = (oDataRow["OTRate"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["OTRate"]);
            oHRAudit.OTAmount = (oDataRow["OTAmount"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["OTAmount"]);
            oHRAudit.AvgLate = (oDataRow["AvgLate"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["AvgLate"]);
            oHRAudit.AvgEarly = (oDataRow["AvgEarly"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["AvgEarly"]);
            oHRAudit.Gross = (oDataRow["Gross"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["Gross"]);
            oHRAudit.NetAmount = (oDataRow["NetAmount"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["NetAmount"]);
            oHRAudit.LastGross = (oDataRow["LastGross"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["LastGross"]);
            oHRAudit.IncrementAmount = (oDataRow["IncrementAmount"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["IncrementAmount"]);
            oHRAudit.TotalPunch = (oDataRow["TotalPunch"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["TotalPunch"]);
            oHRAudit.PunchNotFound = (oDataRow["PunchNotFound"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["PunchNotFound"]);
            oHRAudit.OnePunchFound = (oDataRow["OnePunchFound"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["OnePunchFound"]);
            oHRAudit.ExpectedWorkingHour = (oDataRow["ExpectedWorkingHour"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["ExpectedWorkingHour"]);
            oHRAudit.ExpectedWorkingHourWithOT = (oDataRow["ExpectedWorkingHourWithOT"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["ExpectedWorkingHourWithOT"]);
            oHRAudit.TotalWorkingHour = (oDataRow["TotalWorkingHour"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["TotalWorkingHour"]);
            oHRAudit.TotalWorkingHourWithOT = (oDataRow["TotalWorkingHourWithOT"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["TotalWorkingHourWithOT"]);
            oHRAudit.ExpectedAvgWorkingHour = (oDataRow["ExpectedAvgWorkingHour"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["ExpectedAvgWorkingHour"]);
            oHRAudit.WDPM = (oDataRow["WDPM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["WDPM"]);
            oHRAudit.PPM = (oDataRow["PPM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["PPM"]);
            oHRAudit.APM = (oDataRow["APM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["APM"]);
            oHRAudit.LWPPM = (oDataRow["LWPPM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["LWPPM"]);
            oHRAudit.LatePM = (oDataRow["LatePM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["LatePM"]);
            oHRAudit.EarlyPM = (oDataRow["EarlyPM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["EarlyPM"]);
            oHRAudit.ManualPM = (oDataRow["ManualPM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["ManualPM"]);
            oHRAudit.PNFPM = (oDataRow["PNFPM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["PNFPM"]);
            oHRAudit.ODPFPM = (oDataRow["ODPFPM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["ODPFPM"]);
            oHRAudit.AvgBOAPM = (oDataRow["AvgBOAPM"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["AvgBOAPM"]);



            oHRAudit.EmployeeTypeName = (oDataRow["EmployeeTypeName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["EmployeeTypeName"]);
            oHRAudit.DesignationName = (oDataRow["DesignationName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["DesignationName"]);
            oHRAudit.DepartmentName = (oDataRow["DepartmentName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["DepartmentName"]);
            oHRAudit.LocationName = (oDataRow["LocationName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["LocationName"]);
            oHRAudit.BUName = (oDataRow["BUName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["BUName"]);
            oHRAudit.EmployeeName = (oDataRow["EmployeeName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["EmployeeName"]);
            oHRAudit.Code = (oDataRow["Code"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["Code"]);
            oHRAudit.Gender = (oDataRow["Gender"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["Gender"]);


            return oHRAudit;
        }
        #endregion

        #region CreateObject
        public static HRAudit CreateObject(DataRow oDataRow)
        {
            HRAudit oHRAudit = new HRAudit();
            oHRAudit = MappingObject(oDataRow);
            return oHRAudit;
        }
        #endregion

        #region CreateObjects
        public static List<HRAudit> CreateObjects(DataTable oDataTable)
        {
            List<HRAudit> oHRAudits = new List<HRAudit>();
            foreach (DataRow oDataRow in oDataTable.Rows)
            {
                HRAudit oItem = CreateObject(oDataRow);
                oHRAudits.Add(oItem);
            }
            return oHRAudits;
        }
        public static List<HRAudit> CreateObjects(DataRow[] oDataRows)
        {
            List<HRAudit> oHRAudits = new List<HRAudit>();
            foreach (DataRow oDataRow in oDataRows)
            {
                HRAudit oItem = CreateObject(oDataRow);
                oHRAudits.Add(oItem);
            }
            return oHRAudits;
        }
        #endregion
        #endregion


        #region ServiceFactory

        internal static IHRAuditService Service
        {
            get { return (IHRAuditService)Services.Factory.CreateService(typeof(IHRAuditService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceDaily interface

    public interface IHRAuditService
    {
        DataSet GetAuditReport(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet, double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance, int nPayType);
        List<HRAudit> Gets(string sSQL, Int64 nUserID);
        HRAudit Get(string sSQL, Int64 nUserID);
      
    }
    #endregion
}

