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

    #region AttendanceSchemeLeave

    public class AttendanceSchemeLeave : BusinessObject
    {
        public AttendanceSchemeLeave()
        {
            AttendanceSchemeLeaveID = 0;
            AttendanceSchemeID = 0;
            LeaveID = 0;
            TotalDay = 0;
            DeferredDay = 0;
            ActivationAfter = EnumRecruitmentEvent.None;
            IsLeaveOnPresence = false;
            PresencePerLeave = 0;
            IsCarryForward = false;
            MaxCarryDays = 0;
            ErrorMessage = "";
            IsComp = true;
        }

        #region Properties

        public int AttendanceSchemeLeaveID { get; set; }
        public int AttendanceSchemeID { get; set; }
        public int LeaveID { get; set; }
        public int TotalDay { get; set; }
        public int DeferredDay { get; set; }
        public EnumRecruitmentEvent ActivationAfter { get; set; }
        public bool IsLeaveOnPresence { get; set; }
        public bool IsComp { get; set; }
        public int PresencePerLeave  { get; set; }
        public bool IsCarryForward { get; set; }
        public int MaxCarryDays { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string LeaveName { get; set; }
        public string ActivationAfterInString
        {
            get
            {
                return ActivationAfter.ToString();
            }
        }

        public double YearsToCarry { get { return (this.MaxCarryDays > 0) ? (this.MaxCarryDays/360) : 0; } }
        public string PresenceEarnLeave { get { return (this.IsLeaveOnPresence) ? "Yes" : "No"; } }
        public string CarryForward { get { return (this.IsCarryForward) ? "Yes" : "No"; } }

        public string EarnLeaveStatus { get { return (this.IsLeaveOnPresence) ? "By " + this.PresencePerLeave.ToString() + " days of presence" : ""; } }
        public string CarryForwardStatus { get { return (this.IsCarryForward && this.MaxCarryDays > 0) ? "Max. " + (this.MaxCarryDays/360).ToString() + " years" : ""; } }
        public string ActivationStatus { get { return ((int)this.ActivationAfter > 0) ? this.DeferredDay.ToString() + " days of " + this.ActivationAfter.ToString() : ""; } }

        public string IsCompInStr { get { return (this.IsComp == true) ? "Yes" : "No"; } }

        #endregion

        #region Functions
        public static List<AttendanceSchemeLeave> Gets(int nAttendanceSchemeID, long nUserID)
        {
            return AttendanceSchemeLeave.Service.Gets(nAttendanceSchemeID, nUserID);
        }
        public static List<AttendanceSchemeLeave> Gets(string sSQL, long nUserID)
        {
            return AttendanceSchemeLeave.Service.Gets(sSQL, nUserID);
        }
        public AttendanceSchemeLeave Get(int id, long nUserID)
        {
            return AttendanceSchemeLeave.Service.Get(id, nUserID);
        }
        public AttendanceSchemeLeave IUD(int nDBOperation, long nUserID)
        {
            return AttendanceSchemeLeave.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IAttendanceSchemeLeaveService Service
        {
            get { return (IAttendanceSchemeLeaveService)Services.Factory.CreateService(typeof(IAttendanceSchemeLeaveService)); }
        }

        #endregion
    }
    #endregion

    #region IAttendanceSchemeLeave interface

    public interface IAttendanceSchemeLeaveService
    {
        AttendanceSchemeLeave Get(int id, Int64 nUserID);
        List<AttendanceSchemeLeave> Gets(int attendanceSchemeID, Int64 nUserID);
        List<AttendanceSchemeLeave> Gets(string sSQL, Int64 nUserID);
        AttendanceSchemeLeave IUD(AttendanceSchemeLeave oAttendanceSchemeLeave, int nDBOperation, Int64 nUserID);
    }
    #endregion

}
