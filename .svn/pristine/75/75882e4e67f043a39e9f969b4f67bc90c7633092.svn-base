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
    #region AttendanceCalendarSession

    public class AttendanceCalendarSession : BusinessObject
    {
        public AttendanceCalendarSession()
        {
            ACSID = 0;
            AttendanceCalendarID = 0;
            Session = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Description = "";
            IsActive = false;
            ErrorMessage = "";
            AttendanceCalendarSessionHolidays = new List<AttendanceCalendarSessionHoliday>();
        }

        #region Properties
        public int ACSID { get; set; }
        public int AttendanceCalendarID { get; set; }
        public String Session { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string CalendarName { get; set; }
        public string EndDateInString
        {
            get
            {
                return EndDate.ToString("dd MMM yyyy");
            }
        }
        public string StartDateInString
        {
            get
            {
                return StartDate.ToString("dd MMM yyyy");
            }
        }
        public string Activity { get { return (this.IsActive) ? "Active" : "Inactive"; } }
        public List<AttendanceCalendarSessionHoliday> AttendanceCalendarSessionHolidays { get; set; }
        public List<Holiday> Holidays { get; set; }
        #endregion

        #region Functions
        public static List<AttendanceCalendarSession> Gets(int nAttendanceCalendarID, long nUserID)
        {
            return AttendanceCalendarSession.Service.Gets(nAttendanceCalendarID, nUserID);
        }

        public static List<AttendanceCalendarSession> Gets(string sSQL, long nUserID)
        {
            return AttendanceCalendarSession.Service.Gets(sSQL, nUserID);
        }

        public static AttendanceCalendarSession Get(int id, long nUserID)
        {
            return AttendanceCalendarSession.Service.Get(id, nUserID);
        }

        public AttendanceCalendarSession IUD(int nDBOperation, long nUserID)
        {
            return AttendanceCalendarSession.Service.IUD(this, nDBOperation, nUserID);
        }

        public List<AttendanceCalendarSession> ChangeActiveStatus(AttendanceCalendarSession oAttendanceCalendarSession, long nUserID)
        {
            return AttendanceCalendarSession.Service.ChangeActiveStatus(oAttendanceCalendarSession, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IAttendanceCalendarSessionService Service
        {
            get { return (IAttendanceCalendarSessionService)Services.Factory.CreateService(typeof(IAttendanceCalendarSessionService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceCalendarSession interface
    public interface IAttendanceCalendarSessionService
    {
        AttendanceCalendarSession Get(int id, Int64 nUserID);
        List<AttendanceCalendarSession> Gets(int nAttendanceCalendarID, Int64 nUserID);
        List<AttendanceCalendarSession> Gets(string sSQL, Int64 nUserID);
        AttendanceCalendarSession IUD(AttendanceCalendarSession oAttendanceCalendarSession, int nDBOperation, Int64 nUserID);
        List<AttendanceCalendarSession> ChangeActiveStatus(AttendanceCalendarSession oAttendanceCalendarSession, Int64 nUserID);
    }
    #endregion
}
