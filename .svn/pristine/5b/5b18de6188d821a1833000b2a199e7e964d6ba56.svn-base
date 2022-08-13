using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;



namespace ESimSol.BusinessObjects
{
    #region AttendanceCalendar

    public class AttendanceCalendar : BusinessObject
    {
        public AttendanceCalendar()
        {
            AttendanceCalendarID = 0;
            Code = 0;
            Name = "";
            Description = "";
            IsActive = true;
            ErrorMessage = "";
        }

        #region Properties

        public int AttendanceCalendarID { get; set; }
        public int Code { get; set; }
        public String Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public string Activity { get { return (this.IsActive) ? "Active" : "Inactive"; } }

        public List<Holiday> oHolidays = new List<Holiday>();
        #endregion

        #region Functions
        public static List<AttendanceCalendar> Gets(long nUserID)
        {
            return AttendanceCalendar.Service.Gets(nUserID);
        }
        public static List<AttendanceCalendar> Gets(string sSQL,long nUserID)
        {
            return AttendanceCalendar.Service.Gets(sSQL,nUserID);
        }
        public static AttendanceCalendar Get(int id, long nUserID)
        {
            return AttendanceCalendar.Service.Get(id, nUserID);
        }

        public AttendanceCalendar IUD(int nDBOperation, long nUserID)
        {
            return AttendanceCalendar.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IAttendanceCalendarService Service
        {
            get { return (IAttendanceCalendarService)Services.Factory.CreateService(typeof(IAttendanceCalendarService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceCalendar interface

    public interface IAttendanceCalendarService
    {
        AttendanceCalendar Get(int id, Int64 nUserID);
        List<AttendanceCalendar> Gets(Int64 nUserID);
        List<AttendanceCalendar> Gets(string sSQL,Int64 nUserID);
        AttendanceCalendar IUD(AttendanceCalendar oAttendanceCalendar, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
