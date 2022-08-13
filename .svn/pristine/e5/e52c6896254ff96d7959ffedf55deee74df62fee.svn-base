using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region RosterPlanEmployee

    public class RosterPlanEmployee : BusinessObject
    {
        public RosterPlanEmployee()
        {
            RPEID = 0;
            EmployeeID = 0;
            ShiftID = 0;
            MaxOTInMin = 0;
            IsDayOff = false;
            IsHoliday = false;
            InTime = DateTime.Now;
            OutTime = DateTime.Now;
            AttendanceDate = DateTime.Today;
            ShiftStartTime = DateTime.Now;
            ShiftEndTime = DateTime.Now;
            EmployeeCode = "";
            EmployeeName = "";
            ShiftName = "";
            ErrorMessage = "";
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            DepartmentID = 0;
            TotalRecord = "";
            RecordCount = 0;
            LastRowNum = 0;
            Remarks = "";
            IsRostered = false;
            EmployeeRosters = new List<EmployeeRoster>();
            TrsShiftID = 0;
        }

        #region Properties
        public int RPEID { get; set; }
        public int TrsShiftID { get; set; }
        public int EmployeeID { get; set; }
        public int ShiftID { get; set; }
        public bool IsDayOff { get; set; }
        public bool IsRostered { get; set; }
        public string Remarks { get; set; }
        public bool IsHoliday { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public int MaxOTInMin { get; set; }

        private DateTime _MaxOTInMinDateTime;
        public DateTime MaxOTInMinDateTime
        {
            get { return _MaxOTInMinDateTime; }
            set
            {
                _MaxOTInMinDateTime = value;
                MaxOTInMin = ((_MaxOTInMinDateTime.Hour * 60) + _MaxOTInMinDateTime.Minute);
            }
        }
        
        public DateTime AttendanceDate { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public List<EmployeeRoster> EmployeeRosters { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string ShiftName { get; set; }

        public bool IsGWD
        {
            get
            {
                if (this.IsHoliday == false && this.IsDayOff == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string DateInString
        {
            get
            {
                return AttendanceDate.ToString("dd MMM yyyy");
            }
        }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
        public string ShiftStartTimeInString
        {
            get
            {
                return ShiftStartTime.ToString("H:mm");
            }
        }
        public string ShiftEndTimeInString
        {
            get
            {
                return ShiftEndTime.ToString("H:mm");
            }
        }
        public string ShiftWithDuration
        {
            get
            {
                if (this.ShiftName != "")
                {
                    return this.ShiftName + "(" + this.ShiftStartTimeInString + "-" + this.ShiftEndTimeInString + ")";
                }
                else { return this.ShiftName; }
            }
        }
        public string InTimeSt
        {
            get
            {
                return this.InTime.ToString("H:mm");
            }
        }
        public string MaxOTInMinSt
        {
            get
            {
                return TimeSpan.FromMinutes(this.MaxOTInMin).ToString();
            }
        }
        public string OutTimeSt
        {
            get
            {
                return this.OutTime.ToString("H:mm");
            }
        }
        public string IsDayOffSt
        {
            get
            {
                if (this.IsDayOff)
                {
                    return "DO";
                }
                else
                {
                    return "";
                }                
            }
        }
        public string IsHolidaySt
        {
            get
            {
                if (this.IsHoliday)
                {
                    return "H";
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region Searching Property
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DepartmentID { get; set; }
        public string TotalRecord { get; set; }
        public int RecordCount { get; set; }
        public int LastRowNum { get; set; }        
        #endregion

        #region Functions
        public static List<RosterPlanEmployee> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, long nUserID)
        {
            return RosterPlanEmployee.Service.Gets(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks, nUserID);
        }
        public static List<RosterPlanEmployee> UploadRosterEmpXL(List<RosterPlanEmployee> oRosterPlanEmployees, long nUserID)
        {
            return RosterPlanEmployee.Service.UploadRosterEmpXL(oRosterPlanEmployees, nUserID);
        }
        public static RosterPlanEmployee Get(int Id, long nUserID)
        {
            return RosterPlanEmployee.Service.Get(Id, nUserID);
        }
        public static RosterPlanEmployee Get(string sSQL, long nUserID)
        {
            return RosterPlanEmployee.Service.Get(sSQL, nUserID);
        }
        public static List<RosterPlanEmployee> Gets(long nUserID)
        {
            return RosterPlanEmployee.Service.Gets(nUserID);
        }
        public static List<RosterPlanEmployee> Gets(string sSQL, long nUserID)
        {
            return RosterPlanEmployee.Service.Gets(sSQL, nUserID);
        }
        public static List<RosterPlanEmployee> Gets(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, long nUserID)
        {
            return RosterPlanEmployee.Service.Gets(EmployeeIDs, ShiftID, StartDate, EndDate, nUserID);
        }

        public RosterPlanEmployee IUD(int nDBOperation, long nUserID)
        {
            return RosterPlanEmployee.Service.IUD(this, nDBOperation, nUserID);
        }
        public List<RosterPlanEmployee> Transfer(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, int nOT_In_Minute, bool IsDayOff, int nDBOperation, long nUserID)
        {
            return RosterPlanEmployee.Service.Transfer(EmployeeIDs, ShiftID, StartDate, EndDate, nOT_In_Minute, IsDayOff, nDBOperation, nUserID);
        }
        public List<RosterPlanEmployee> TransferDept(DateTime StartDate, DateTime EndDate, int BUID, string LocationIDs, string DepartmentIDs, int ShiftID, DateTime InTime, DateTime OutTime, bool IsGWD, bool IsDayOff, string Remarks, int MaxOTDateTime, string EmployeeIDs, string DesignationIDs, bool isOfficial, DateTime RosterDate, string GroupIDs, string BlockIDs, int TrsShiftID, int nDBOperation, long nUserID)
        {
            return RosterPlanEmployee.Service.TransferDept(StartDate, EndDate, BUID, LocationIDs, DepartmentIDs, ShiftID, InTime, OutTime, IsGWD, IsDayOff, Remarks, MaxOTDateTime, EmployeeIDs, DesignationIDs, isOfficial, RosterDate, GroupIDs, BlockIDs,TrsShiftID, nDBOperation, nUserID);
        }
        public List<RosterPlanEmployee> Swap(int RosterPlanID, DateTime StartDate, DateTime EndDate, int nDBOperation, long nUserID)
        {
            return RosterPlanEmployee.Service.Swap(RosterPlanID, StartDate, EndDate, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRosterPlanEmployeeService Service
        {
            get { return (IRosterPlanEmployeeService)Services.Factory.CreateService(typeof(IRosterPlanEmployeeService)); }
        }

        #endregion
    }
    #endregion

    #region IRosterPlanEmployee interface

    public interface IRosterPlanEmployeeService
    {
        List<RosterPlanEmployee> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID);
        
        List<RosterPlanEmployee> UploadRosterEmpXL(List<RosterPlanEmployee> oAttendanceDailys, Int64 nUserID);
        RosterPlanEmployee Get(int id, Int64 nUserID);
        RosterPlanEmployee Get(string sSQL, Int64 nUserID);
        List<RosterPlanEmployee> Gets(Int64 nUserID);
        List<RosterPlanEmployee> Gets(string sSQL, Int64 nUserID);
        List<RosterPlanEmployee> Gets(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, Int64 nUserID);
        RosterPlanEmployee IUD(RosterPlanEmployee oRosterPlanEmployee, int nDBOperation, Int64 nUserID);
        List<RosterPlanEmployee> Transfer(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, int nOT_In_Minute, bool IsDayOff, int nDBOperation, Int64 nUserID);
        List<RosterPlanEmployee> TransferDept(DateTime StartDate, DateTime EndDate, int BUID, string LocationIDs, string DepartmentIDs, int ShiftID, DateTime InTime, DateTime OutTime, bool IsGWD, bool IsDayOff, string Remarks, int MaxOTDateTime, string EmployeeIDs, string DesignationIDs, bool isOfficial, DateTime RosterDate, string GroupIDs, string BlockIDs, int TrsShiftID, int nDBOperation, Int64 nUserID);
        List<RosterPlanEmployee> Swap(int RosterPlanID, DateTime StartDate, DateTime EndDate, int nDBOperation, Int64 nUserID);
    }
    #endregion

    #region EmployeeRoster
    public class EmployeeRoster
    {
        public EmployeeRoster()
        {
            EmployeeID = 0;
            EmployeeCode = "";
            EmployeeName = "";
            RPObj01 = new RosterPlanEmployee();
            Day01 = "";
            RPObj02 = new RosterPlanEmployee();
            Day02 = "";
            RPObj03 = new RosterPlanEmployee();
            Day03 = "";
            RPObj04 = new RosterPlanEmployee();
            Day04 = "";
            RPObj05 = new RosterPlanEmployee();
            Day05 = "";
            RPObj06 = new RosterPlanEmployee();
            Day06 = "";
            RPObj07 = new RosterPlanEmployee();
            Day07 = "";
            RPObj08 = new RosterPlanEmployee();
            Day08 = "";
            RPObj09 = new RosterPlanEmployee();
            Day09 = "";
            RPObj10 = new RosterPlanEmployee();
            Day10 = "";
            RPObj11 = new RosterPlanEmployee();
            Day11 = "";
            RPObj12 = new RosterPlanEmployee();
            Day12 = "";
            RPObj13 = new RosterPlanEmployee();
            Day13 = "";
            RPObj14 = new RosterPlanEmployee();
            Day14 = "";
            RPObj15 = new RosterPlanEmployee();
            Day15 = "";
            RPObj16 = new RosterPlanEmployee();
            Day16 = "";
            RPObj17 = new RosterPlanEmployee();
            Day17 = "";
            RPObj18 = new RosterPlanEmployee();
            Day18 = "";
            RPObj19 = new RosterPlanEmployee();
            Day19 = "";
            RPObj20 = new RosterPlanEmployee();
            Day20 = "";
            RPObj21 = new RosterPlanEmployee();
            Day21 = "";
            RPObj22 = new RosterPlanEmployee();
            Day22 = "";
            RPObj23 = new RosterPlanEmployee();
            Day23 = "";
            RPObj24 = new RosterPlanEmployee();
            Day24 = "";
            RPObj25 = new RosterPlanEmployee();
            Day25 = "";
            RPObj26 = new RosterPlanEmployee();
            Day26 = "";
            RPObj27 = new RosterPlanEmployee();
            Day27 = "";
            RPObj28 = new RosterPlanEmployee();
            Day28 = "";
            RPObj29 = new RosterPlanEmployee();
            Day29 = "";
            RPObj30 = new RosterPlanEmployee();
            Day30 = "";
            RPObj31 = new RosterPlanEmployee();
            Day31 = "";
        }
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public RosterPlanEmployee RPObj01 { get; set; }
        public string Day01 { get; set; }
        public RosterPlanEmployee RPObj02 { get; set; }
        public string Day02 { get; set; }
        public RosterPlanEmployee RPObj03 { get; set; }
        public string Day03 { get; set; }
        public RosterPlanEmployee RPObj04 { get; set; }
        public string Day04 { get; set; }
        public RosterPlanEmployee RPObj05 { get; set; }
        public string Day05 { get; set; }
        public RosterPlanEmployee RPObj06 { get; set; }
        public string Day06 { get; set; }
        public RosterPlanEmployee RPObj07 { get; set; }
        public string Day07 { get; set; }
        public RosterPlanEmployee RPObj08 { get; set; }
        public string Day08 { get; set; }
        public RosterPlanEmployee RPObj09 { get; set; }
        public string Day09 { get; set; }
        public RosterPlanEmployee RPObj10 { get; set; }
        public string Day10 { get; set; }
        public RosterPlanEmployee RPObj11 { get; set; }
        public string Day11 { get; set; }
        public RosterPlanEmployee RPObj12 { get; set; }
        public string Day12 { get; set; }
        public RosterPlanEmployee RPObj13 { get; set; }
        public string Day13 { get; set; }
        public RosterPlanEmployee RPObj14 { get; set; }
        public string Day14 { get; set; }
        public RosterPlanEmployee RPObj15 { get; set; }
        public string Day15 { get; set; }
        public RosterPlanEmployee RPObj16 { get; set; }
        public string Day16 { get; set; }
        public RosterPlanEmployee RPObj17 { get; set; }
        public string Day17 { get; set; }
        public RosterPlanEmployee RPObj18 { get; set; }
        public string Day18 { get; set; }
        public RosterPlanEmployee RPObj19 { get; set; }
        public string Day19 { get; set; }
        public RosterPlanEmployee RPObj20 { get; set; }
        public string Day20 { get; set; }
        public RosterPlanEmployee RPObj21 { get; set; }
        public string Day21 { get; set; }
        public RosterPlanEmployee RPObj22 { get; set; }
        public string Day22 { get; set; }
        public RosterPlanEmployee RPObj23 { get; set; }
        public string Day23 { get; set; }
        public RosterPlanEmployee RPObj24 { get; set; }
        public string Day24 { get; set; }
        public RosterPlanEmployee RPObj25 { get; set; }
        public string Day25 { get; set; }
        public RosterPlanEmployee RPObj26 { get; set; }
        public string Day26 { get; set; }
        public RosterPlanEmployee RPObj27 { get; set; }
        public string Day27 { get; set; }
        public RosterPlanEmployee RPObj28 { get; set; }
        public string Day28 { get; set; }
        public RosterPlanEmployee RPObj29 { get; set; }
        public string Day29 { get; set; }
        public RosterPlanEmployee RPObj30 { get; set; }
        public string Day30 { get; set; }
        public RosterPlanEmployee RPObj31 { get; set; }
        public string Day31 { get; set; }        
    }
    #endregion
}
