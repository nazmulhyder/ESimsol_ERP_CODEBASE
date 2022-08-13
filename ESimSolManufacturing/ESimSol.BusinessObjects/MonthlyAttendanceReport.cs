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
    #region MonthlyAttendanceReport
    [DataContract]
    public class MonthlyAttendanceReport : MonthlyAttendanceReport_Extend
    {
        public MonthlyAttendanceReport()
        {
            EmployeeID = 0;
            EmployeeName = "";
            EmployeeCode = "";
            BusinessUnitID = 0;
            LocationName = "";
            DepartmentName = "";
            DesignationName = "";
            BUName = "";
            JoiningDate = DateTime.Now;
            TotalWorkingDay = 0;
            PresentDay = 0;
            AbsentDay = 0;
            DayOFF = 0;
            HoliDay = 0;
            Leave = 0;
            LeaveHalfShort = 0;
            NoWork = 0;
            OvertimeInhour = 0;
            IsActive = false;
            Education = "";
            DisAction = 0;
            ErrorMessage = "";
            RegularOT_Hr = 0;
            ExtraOT_Hr = 0;
            OT_Hr = 0;
            OT_Rate = 0;
            OT_Amount = 0;
            PaymentDay = 0;
            GrossSalary = 0;
            PerDaySalary = 0;
            PerDaySalaryBasic = 0;
            SearchingDay = 0;
            GrossSalaryBasedOnPresent = 0;
            TotalAmount = 0;
            RemarkWithCount = "";
            EmpCount = 0;
            AbsentAmount = 0;
            SalaryAmount = 0;
            OTAmount = 0;
            PayableGrossAmount = 0;
            RegularOT_Amount = 0;
        }

        public double RegularOT_Hr { get; set; }
        public double ExtraOT_Hr { get; set; }
        public double RegularOT_Amount { get; set; }
        public double ExtraOT_Amount { get; set; }
        public double OT_Hr { get; set; }
        public double OT_Rate { get; set; }
        public double OT_Amount { get; set; }
        public int PaymentDay { get; set; }
        public double GrossSalary { get; set; }
        public double PerDaySalary { get; set; }
        public double PerDaySalaryBasic { get; set; }
        public int SearchingDay { get; set; }
        public double GrossSalaryBasedOnPresent { get; set; }
        public double TotalAmount { get; set; }


        #region Properties
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string RemarkWithCount { get; set; }
        public string BUName { get; set; }
        public string EmployeeCode { get; set; }
        public int BusinessUnitID { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Education { get; set; }
        public DateTime JoiningDate { get; set; }
        public int TotalWorkingDay { get; set; }//Company Working Day
        public int PresentDay { get; set; }
        public int AbsentDay { get; set; }
        public int DayOFF { get; set; }
        public int HoliDay { get; set; }
        public int Leave { get; set; }
        public int LeaveHalfShort { get; set; }
        public int NoWork { get; set; }
        public double OvertimeInhour { get; set; }
        public int EarlyOutDays { get; set; }
        public int EarlyOutMins { get; set; }
        public int LateDays { get; set; }
        public int LateMins { get; set; }
        public int ExcessMins { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        public int DisAction { get; set; }
        public int EmpCount { get; set; }
        public double AbsentAmount { get; set; }
        public double SalaryAmount { get; set; }
        public double PayableGrossAmount { get; set; }
        public double OTAmount { get; set; }
        #endregion

        #region Derived Property

        public List<MonthlyAttendanceReport> MonthlyAttendanceReports { get; set; }
        public Company Company { get; set; }
        public string PresentDaySt
        {
            get
            {
                if (PresentDay > 0)
                    return PresentDay.ToString();
                else
                    return "-";
            }
        }
        public string AbsentDaySt
        {
            get
            {
                if (AbsentDay > 0)
                    return AbsentDay.ToString();
                else
                    return "-";
            }
        }
        public string DayOFFSt
        {
            get
            {
                if (DayOFF > 0)
                    return DayOFF.ToString();
                else
                    return "-";
            }
        }
        public string HoliDaySt
        {
            get
            {
                if (this.HoliDay > 0)
                    return this.HoliDay.ToString();
                else
                    return "-";
            }
        }

        public string LeaveSt
        {
            get
            {
                if (Leave > 0)
                    return Leave.ToString();
                else
                    return "-";
            }
        }
        public string LeaveHalfShortSt
        {
            get
            {
                if (LeaveHalfShort > 0)
                    return LeaveHalfShort.ToString();
                else
                    return "-";
            }
        }

        
        public string NoWorkSt
        {
            get
            {
                if (NoWork > 0)
                    return NoWork.ToString();
                else
                    return "-";
            }
        }
        public string OvertimeInhourSt
        {
            get
            {
                if (OvertimeInhour > 0)
                    return OvertimeInhour.ToString();
                else
                    return "-";
            }
        }
        public string TotalWorkingDaySt
        {
            get
            {
                if (TotalWorkingDay > 0)
                    return TotalWorkingDay.ToString();
                else
                    return "-";
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

        public string CWD
        {
            get
            {
                if (TotalWorkingDay > 0)
                    return TotalWorkingDay.ToString();
                else
                    return "-";
            }
        }

        public string WorkingStatusInString
        {
            get
            {
                if (IsActive) return "Continued";
                else return "Discontinued";
                //return this.IsActive.ToString();
            }
        }
        public string DisActionSt
        {
            get
            {
                if (this.DisAction > 0)
                    return DisAction.ToString();
                else
                    return "-";
            }
        }
        #endregion

        #region Functions
        public static List<MonthlyAttendanceReport> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, long nUserID)
        {
            return MonthlyAttendanceReport.Service.Gets(sEmployeeIDs, sBusinessUnitIds,sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nStartSalaryRange, nEndSalaryRange, ShiftIds, sRemarks, nUserID);
        }
        public static List<MonthlyAttendanceReport> Gets_F3(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, long nUserID)
        {
            return MonthlyAttendanceReport.Service.Gets_F3(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nUserID);
        }
        public static List<MonthlyAttendanceReport> Gets_F3_Comp(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, long nUserID)
        {
            return MonthlyAttendanceReport.Service.Gets_F3_Comp(sEmployeeIDs, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sSalarySchemeIDs, DateFrom, DateTo, sWorkingStatus, sGroupIDs, sBlockIDs, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IMonthlyAttendanceReportService Service
        {
            get { return (IMonthlyAttendanceReportService)Services.Factory.CreateService(typeof(IMonthlyAttendanceReportService)); }
        }
        #endregion
    }
    #endregion

    #region IMonthlyAttendanceReport interface
    
    public interface IMonthlyAttendanceReportService
    {
        List<MonthlyAttendanceReport> Gets(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, double nStartSalaryRange, double nEndSalaryRange, string ShiftIds, string sRemarks, Int64 nUserID);
        List<MonthlyAttendanceReport> Gets_F3(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, Int64 nUserID);
        List<MonthlyAttendanceReport> Gets_F3_Comp(string sEmployeeIDs, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sSalarySchemeIDs, DateTime DateFrom, DateTime DateTo, string sWorkingStatus, string sGroupIDs, string sBlockIDs, Int64 nUserID);

    }
    #endregion
}
