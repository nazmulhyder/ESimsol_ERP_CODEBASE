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

    public class RosterPlanEmployeeV2 : BusinessObject
    {
        public RosterPlanEmployeeV2()
        {
            RPEID = 0;
            EmployeeID = 0;
            ShiftID = 0;
            BusinessUnitID = 0;
            LocationID = 0;
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
            BUName = "";
            LocationName = "";
            Department = "";
            ErrorMessage = "";
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            DepartmentID = 0;
            Remarks = "";
            IsPIMSRoaster = false;
            TotalAttendanceCount = 0;
            UserName = "";
            DBServerDateTime = DateTime.Now;
            EmployeeBatchID = 0;
            EmployeeIDs = "";
        }

        #region Properties
        public int RPEID { get; set; }
        public int TrsShiftID { get; set; }
        public int EmployeeID { get; set; }
        public int ShiftID { get; set; }
        public int BusinessUnitID { get; set; }
        public int LocationID { get; set; }
        public bool IsDayOff { get; set; }
        public bool IsPIMSRoaster { get; set; }
        public string Remarks { get; set; }
        public string BUName { get; set; }
        public string LocationName { get; set; }
        public string Department { get; set; }
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
        public int TotalAttendanceCount { get; set; }
        public string UserName { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public int EmployeeBatchID { get; set; }
        public string EmployeeIDs { get; set; }
        #endregion

        #region Derived Property
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

        public string DateTimeInST
        {
            get
            {
                return this.DBServerDateTime.ToString("dd MMM yyyy") + ", " + this.DBServerDateTime.ToString("H:mm");
            }
        }
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
                    return "DayOff";
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
                    return "Holiday";
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
        #endregion

        #region Functions
        public static List<RosterPlanEmployeeV2> Gets(string sSQL, long nUserID)
        {
            return RosterPlanEmployeeV2.Service.Gets(sSQL, nUserID);
        }

        public static RosterPlanEmployeeV2 GetTotalCount(string ssql, long nUserID)
        {
            return RosterPlanEmployeeV2.Service.GetTotalCount(ssql, nUserID);
        }

        public RosterPlanEmployeeV2 UpdateRosterPlanEmployee(long nUserID)
        {
            return RosterPlanEmployeeV2.Service.UpdateRosterPlanEmployee(this, nUserID);
        }
        public   List<RosterPlanEmployeeV2> CommitRosterPlanEmployee(long nUserID)
        {
            return RosterPlanEmployeeV2.Service.CommitRosterPlanEmployee(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IRosterPlanEmployeeV2Service Service
        {
            get { return (IRosterPlanEmployeeV2Service)Services.Factory.CreateService(typeof(IRosterPlanEmployeeV2Service)); }
        }

        #endregion
    }
    #endregion

    #region IRosterPlanEmployeeV2 interface

    public interface IRosterPlanEmployeeV2Service
    {
        List<RosterPlanEmployeeV2> Gets(string sSQL, Int64 nUserID);
        RosterPlanEmployeeV2 GetTotalCount(string ssql, Int64 nUserID);
        RosterPlanEmployeeV2 UpdateRosterPlanEmployee(RosterPlanEmployeeV2 oRosterPlanEmployeeV2, Int64 nUserID);
        List<RosterPlanEmployeeV2> CommitRosterPlanEmployee(RosterPlanEmployeeV2 oRosterPlanEmployeeV2, Int64 nUserID);
    }
    #endregion

   
}
