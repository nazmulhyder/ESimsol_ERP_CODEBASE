using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;


namespace ESimSol.BusinessObjects
{
    #region LeaveLedgerReport
    [DataContract]
    public class LeaveLedgerReport : BusinessObject
    {
        public LeaveLedgerReport()
        {
            EmployeeID = 0;
            EmployeeName = "";
            EmployeeCode = "";
            DepartmentName = "";
            DesignationName = "";
            LocationName = "";
            JoiningDate = DateTime.Now;
            LeaveHeadID = 0;
            LeaveShortName = "";
            TotalLeave = 0;
            //Enjoyed = 0;
            ErrorMessage = "";
            BusinessUnitID = 0;
            BUName = "";
            Full_Enjoyed = 0;
            Half_Enjoyed = 0;
            Short_Enjoyed = 0;
            Full_Balance = 0;
            Half_Balance = 0;
            Short_Balance = 0;
            LeaveDuration = 0;

            LeaveName = "";
            EnjoyedLeaveSalaryMonth = 0;
            EmpLeaveLedgerID = 0;
            ApplicationDate = DateTime.MinValue;
            LeaveType = EnumLeaveType.None;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
            BusinessUnits = new List<BusinessUnit>();
        }


        #region Properties

        public double Full_Enjoyed { get; set; }
        public double Half_Enjoyed { get; set; }
        public double Short_Enjoyed { get; set; }
        public double Full_Balance { get; set; }
        public double Half_Balance { get; set; }
        public double Short_Balance { get; set; }
        public int EmployeeID { get; set; }
        public int EmpLeaveLedgerID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EnjoyedLeaveSalaryMonth { get; set; }
        public int LeaveDuration { get; set; }
        
        public string LocationName { get; set; }
        public string LeaveName { get; set; }
        public string EmployeeName { get; set; }

        public EnumLeaveType LeaveType { get; set; }
        public int BusinessUnitID { get; set; }

        public string BUName { get; set; }
        
        public string EmployeeCode { get; set; }
        
        public string DepartmentName { get; set; }
        
        public string DesignationName { get; set; }
        
        public DateTime JoiningDate { get; set; }
        
        public int LeaveHeadID { get; set; }
        public string LeaveShortName { get; set; }
        
        public double TotalLeave { get; set; }
        
        //public double Enjoyed { get; set; }
        
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public List<LeaveLedgerReport> LeaveLedgerReports { get; set; }
        public List<BusinessUnit> BusinessUnits { get; set; }
        public AttendanceCalendarSession AttendanceCalendarSession { get; set; }
        public List<LeaveHead> LeaveHeads { get; set; }

        public Company Company { get; set; }

        public string LeaveTypeSt { get { return EnumObject.jGet(this.LeaveType); } }
        public double Enjoyed
        {
            get
            {
                double dEnjoyed=this.Full_Enjoyed;
                if (this.Half_Enjoyed>0)
                {
                    dEnjoyed = dEnjoyed + this.Half_Enjoyed / 2;
                }
                if (this.Short_Enjoyed > 0)
                {
                    dEnjoyed = dEnjoyed + this.Short_Enjoyed / 4;
                }
                return dEnjoyed;
            }
        }
        public string EnjoyedInfo
        {

            get
            {
                //string retVal = "";
                //if (this.Full_Enjoyed > 0)
                //{
                //    retVal += "F:" + this.Full_Enjoyed.ToString();
                //}
                //if (this.Half_Enjoyed > 0)
                //{
                //    retVal += "-H:" + this.Half_Enjoyed.ToString();
                //}
                //if (this.Short_Enjoyed > 0)
                //{
                //    retVal += "-S:" + this.Short_Enjoyed.ToString();
                //}


                return this.Enjoyed.ToString();//retVal;// "F:" + this.Full_Enjoyed + "-H:" + this.Half_Enjoyed + "-S:" + this.Short_Enjoyed;
            }
        }

        public string JoiningDateInString
        {
            get
            {
                return JoiningDate.ToString("dd MMM yyyy");
            }
        }

        public string EmployeeNameCode
        {
            get
            {
                return EmployeeName + "[" + EmployeeCode + "]";
            }
        }

        public double Balance
        {
            get
            {
                return this.TotalLeave - this.Enjoyed;
            }
        }

        public string BalanceInfo
        {

            get
            {
                //string retVal = "";
                //if (this.Full_Balance > 0)
                //{
                //    retVal += "F:" + this.Full_Balance.ToString();
                //}
                //if (this.Half_Balance > 0)
                //{
                //    retVal += "-H:" + this.Half_Balance.ToString();
                //}
                //if (this.Short_Balance > 0)
                //{
                //    retVal += "-S:" + this.Short_Balance.ToString();
                //}
                //return retVal;
                return this.Balance.ToString();
            }
        }
        //public double BalanceFull
        //{
        //    get
        //    {
        //        return this.TotalLeave - this.Full_Enjoyed;
        //    }
        //}
        //public double BalanceHalf
        //{
        //    get
        //    {
        //        return this.TotalLeave - this.Half_Enjoyed;
        //    }
        //}
        //public double BalanceShort
        //{
        //    get
        //    {
        //        return this.TotalLeave - this.Short_Enjoyed;
        //    }
        //}
        #endregion

        #region User Define Functions
        #region Object Mapping
        private static LeaveLedgerReport MappingObject(DataRow oDataRow)
        {
            LeaveLedgerReport oLeaveLedgerReport = new LeaveLedgerReport();
            oLeaveLedgerReport.EmployeeID = (oDataRow["EmployeeID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["EmployeeID"]);
            oLeaveLedgerReport.EnjoyedLeaveSalaryMonth = (oDataRow["EnjoyedLeaveSalaryMonth"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["EnjoyedLeaveSalaryMonth"]);
            oLeaveLedgerReport.LeaveHeadID = (oDataRow["LeaveHeadID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["LeaveHeadID"]);
            oLeaveLedgerReport.LeaveName = (oDataRow["LeaveName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["LeaveName"]);
            oLeaveLedgerReport.LeaveShortName = (oDataRow["LeaveShortName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["LeaveShortName"]);
            oLeaveLedgerReport.Full_Enjoyed = (oDataRow["Full_Enjoyed"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["Full_Enjoyed"]);
            oLeaveLedgerReport.Half_Enjoyed = (oDataRow["Half_Enjoyed"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["Half_Enjoyed"]);
            oLeaveLedgerReport.Short_Enjoyed = (oDataRow["Short_Enjoyed"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["Short_Enjoyed"]);
            oLeaveLedgerReport.Full_Balance = (oDataRow["Full_Balance"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["Full_Balance"]);
            oLeaveLedgerReport.Half_Balance = (oDataRow["Half_Balance"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["Half_Balance"]);
            oLeaveLedgerReport.Short_Balance = (oDataRow["Short_Balance"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["Short_Balance"]);
            oLeaveLedgerReport.TotalLeave = (oDataRow["TotalLeave"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["TotalLeave"]);
            oLeaveLedgerReport.EmpLeaveLedgerID = (oDataRow["EmpLeaveLedgerID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["EmpLeaveLedgerID"]);
            return oLeaveLedgerReport;
        }
        #endregion

        #region CreateObject
        public static LeaveLedgerReport CreateObject(DataRow oDataRow)
        {
            LeaveLedgerReport oLeaveLedgerReport = new LeaveLedgerReport();
            oLeaveLedgerReport = MappingObject(oDataRow);
            return oLeaveLedgerReport;
        }
        #endregion

        #region CreateObjects
        public static List<LeaveLedgerReport> CreateObjects(DataTable oDataTable)
        {
            List<LeaveLedgerReport> oLeaveLedgerReports = new List<LeaveLedgerReport>();
            foreach (DataRow oDataRow in oDataTable.Rows)
            {
                LeaveLedgerReport oItem = CreateObject(oDataRow);
                oLeaveLedgerReports.Add(oItem);
            }
            return oLeaveLedgerReports;
        }
        public static List<LeaveLedgerReport> CreateObjects(DataRow[] oDataRows)
        {
            List<LeaveLedgerReport> oLeaveLedgerReports = new List<LeaveLedgerReport>();
            foreach (DataRow oDataRow in oDataRows)
            {
                LeaveLedgerReport oItem = CreateObject(oDataRow);
                oLeaveLedgerReports.Add(oItem);
            }
            return oLeaveLedgerReports;
        }
        #endregion
        #endregion
        #region Functions
        public static List<LeaveLedgerReport> Gets(string sEmployeeIDs, string sDepartmentIds, string sDesignationIds, int ACSID, int nLeaveHeadID, double nBalanceAmount, int nBalanceType, bool bReportingPerson, DateTime dtFrom, DateTime dtTo, bool bDate, bool IsActive, bool IsInActive, string sBUnit, string sLocationID, long nUserID)
        {
            return LeaveLedgerReport.Service.Gets(sEmployeeIDs, sDepartmentIds, sDesignationIds, ACSID, nLeaveHeadID, nBalanceAmount, nBalanceType, bReportingPerson, dtFrom, dtTo, bDate, IsActive, IsInActive, sBUnit, sLocationID, nUserID);
        }
        public static List<LeaveLedgerReport> GetsComp(string sEmployeeIDs, string sDepartmentIds, string sDesignationIds, int ACSID, int nLeaveHeadID, double nBalanceAmount, int nBalanceType, bool bReportingPerson, DateTime dtFrom, DateTime dtTo, bool bDate, bool IsActive, bool IsInActive, string sBUnit, string sLocationID, long nUserID)
        {
            return LeaveLedgerReport.Service.GetsComp(sEmployeeIDs, sDepartmentIds, sDesignationIds, ACSID, nLeaveHeadID, nBalanceAmount, nBalanceType, bReportingPerson, dtFrom, dtTo, bDate, IsActive, IsInActive, sBUnit, sLocationID, nUserID);
        }
        public static List<LeaveLedgerReport> GetLeaveWithEnjoyBalance(string sBUIDs, string sLocIDs, string sDeptIDs, string sDesgIDs, string sEmployeeIDs, DateTime sStartDate, DateTime sEndDate,
                int nApplicationNature, int nLeaveHeadId, int nLeaveType, int nLeaveStatus, int nIsPaid, int nIsUnPaid , long nUserID)
        {
            return LeaveLedgerReport.Service.GetLeaveWithEnjoyBalance(sBUIDs, sLocIDs, sDeptIDs, sDesgIDs, sEmployeeIDs, sStartDate, sEndDate,
                nApplicationNature, nLeaveHeadId, nLeaveType, nLeaveStatus, nIsPaid, nIsUnPaid, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILeaveLedgerReportService Service
        {
            get { return (ILeaveLedgerReportService)Services.Factory.CreateService(typeof(ILeaveLedgerReportService)); }
        }

        #endregion
    }

    #endregion

    #region ILeaveLedgerReport interface
    [ServiceContract]
    public interface ILeaveLedgerReportService
    {
        List<LeaveLedgerReport> GetLeaveWithEnjoyBalance(string sBUIDs, string sLocIDs, string sDeptIDs, string sDesgIDs, string sEmployeeIDs, DateTime sStartDate, DateTime sEndDate,
                   int nApplicationNature, int nLeaveHeadId, int nLeaveType, int nLeaveStatus, int nIsPaid, int nIsUnPaid, Int64 nUserID);
        
        List<LeaveLedgerReport> Gets(string sEmployeeIDs, string sDepartmentIds, string sDesignationIds, int ACSID, int nLeaveHeadID, double nBalanceAmount, int nBalanceType, bool bReportingPerson, DateTime dtFrom, DateTime dtTo, bool bDate, bool IsActive, bool IsInActive, string sBUnit, string sLocationID, Int64 nUserID);
        List<LeaveLedgerReport> GetsComp(string sEmployeeIDs, string sDepartmentIds, string sDesignationIds, int ACSID, int nLeaveHeadID, double nBalanceAmount, int nBalanceType, bool bReportingPerson, DateTime dtFrom, DateTime dtTo, bool bDate, bool IsActive, bool IsInActive, string sBUnit, string sLocationID, Int64 nUserID);
    }
    #endregion
}
