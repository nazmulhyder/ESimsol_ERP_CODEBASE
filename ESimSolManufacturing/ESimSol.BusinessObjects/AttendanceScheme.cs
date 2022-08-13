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

    #region AttendanceScheme

    public class AttendanceScheme : BusinessObject
    {
        public AttendanceScheme()
        {
            AttendanceSchemeID = 0;
            CompanyID = 1;//This is a Hard Qoutted Value
            RosterPlanID = 0;
            Code = "";
            OvertimeCalculateInMinuteAfter = 0;
            Accomodation = true;
            AccommodationDeferredDay = 0;
            AccommodationActivationAfter = EnumRecruitmentEvent.None;
            EnforceMonthClosingRoster = true;
            OverTime = true;
            OverTimeDeferredDay = 0;
            OverTimeActivationAfter = EnumRecruitmentEvent.None;
            IsOTCalTimeStartFromShiftStart = false;
            MaxOTInMinutePerDay = 0;
            BreakageTimeInMinute = 0;
            RosterPlanDescription = "";
            strDepartmentCloseDays = "";
            ErrorMessage = "";

            LeaveList = new List<LeaveHead>();
            Holidays = new List<Holiday>();

            AttendanceCalendars = new List<AttendanceCalendar>();
            AttendanceSchemeLeaves = new List<AttendanceSchemeLeave>();
            AttendanceSchemeHolidays = new List<AttendanceSchemeHoliday>();
            AttendanceSchemeDayOffs = new List<AttendanceSchemeDayOff>();
            RosterPlanDetails = new List<RosterPlanDetail>();
            RosterPlans = new List<RosterPlan>();
            AttendanceSchemes = new List<AttendanceScheme>();
            DayOff = "N/A";
            RosterCycle = 0;
            Params = "";
            IsActive = true;
        }

        #region Properties

        public int AttendanceSchemeID { get; set; }
        public int CompanyID { get; set; }
        public int RosterPlanID { get; set; }
        public int AttendanceCalenderID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int OvertimeCalculateInMinuteAfter { get; set; }
        public bool Accomodation { get; set; }
        public int AccommodationDeferredDay { get; set; }
        public EnumRecruitmentEvent AccommodationActivationAfter { get; set; }
        public bool EnforceMonthClosingRoster { get; set; }
        public bool OverTime { get; set; }
        public int OverTimeDeferredDay { get; set; }
        public EnumRecruitmentEvent OverTimeActivationAfter { get; set; }
        public bool IsOTCalTimeStartFromShiftStart { get; set; }
        public int MaxOTInMinutePerDay { get; set; }
        public int BreakageTimeInMinute { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }


        public double OvertimeCalculateInHourAfter
        {
            get
            {
                return (this.OvertimeCalculateInMinuteAfter > 0) ? Math.Round((double)this.OvertimeCalculateInMinuteAfter / 60, 2) : 0;
            }
        }
        public double MaxOTInHourPerDay
        {
            get
            {
                return (this.MaxOTInMinutePerDay > 0) ? Math.Round((double)this.MaxOTInMinutePerDay / 60, 2) : 0;
            }
        }
        #endregion

        #region Derived Property
        public string CompanyName { get; set; }
        public string AttendanceCalendar { get; set; }
        public string RosterPlanDescription { get; set; }
        public string DayOff { get; set; }
        public double RosterCycle { get; set; }
        public List<LeaveHead> LeaveList { get; set; }
        public List<Holiday> Holidays { get; set; }
        public List<AttendanceCalendar> AttendanceCalendars { get; set; }
        public string strDepartmentCloseDays { get; set; }
        public List<AttendanceSchemeLeave> AttendanceSchemeLeaves { get; set; }
        public List<AttendanceSchemeHoliday> AttendanceSchemeHolidays { get; set; }
        public List<AttendanceSchemeDayOff> AttendanceSchemeDayOffs { get; set; }
        public List<RosterPlanDetail> RosterPlanDetails { get; set; }
        public List<RosterPlan> RosterPlans { get; set; }
        public List<AttendanceScheme> AttendanceSchemes { get; set; }
        public string OverTimeActivationAfterInString { get { return this.OverTimeActivationAfter.ToString(); } }
        public string AccommodationActivationAfterInString { get { return this.AccommodationActivationAfter.ToString(); } }
        public string IsActiveInStr { get { return (this.IsActive)?"Active":"Inactive"; } }
        
        public string Params { get; set; }
        #endregion

        #region Functions
        public static List<AttendanceScheme> Gets(long nUserID)
        {
            return AttendanceScheme.Service.Gets(nUserID);
        }

        public static List<AttendanceScheme> Gets(string sSQL, long nUserID)
        {
            return AttendanceScheme.Service.Gets(sSQL, nUserID);
        }

        public AttendanceScheme Get(int id, long nUserID)
        {
            return AttendanceScheme.Service.Get(id, nUserID);
        }

        public AttendanceScheme IUD(int nDBOperation, long nUserID)
        {
            return AttendanceScheme.Service.IUD(this, nDBOperation, nUserID);
        }
        public AttendanceScheme ActiveInActive(long nUserID)
        {
            return AttendanceScheme.Service.ActiveInActive(this, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IAttendanceSchemeService Service
        {
            get { return (IAttendanceSchemeService)Services.Factory.CreateService(typeof(IAttendanceSchemeService)); }
        }

        #endregion
    }
    #endregion

    #region IAttendanceScheme interface

    public interface IAttendanceSchemeService
    {
        AttendanceScheme Get(int id, Int64 nUserID);
        List<AttendanceScheme> Gets(Int64 nUserID);
        List<AttendanceScheme> Gets(string sSQL, Int64 nUserID);
        AttendanceScheme IUD(AttendanceScheme oAttendanceScheme, int nDBOperation, Int64 nUserID);
        AttendanceScheme ActiveInActive(AttendanceScheme oAttendanceScheme, Int64 nUserID);

        
    }
    #endregion

}
