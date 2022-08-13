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
    #region AttendanceCalendarSessionHoliday

    public class AttendanceCalendarSessionHoliday : BusinessObject
    {
        public AttendanceCalendarSessionHoliday()
        {
            ACSHID = 0;
            ACSID = 0;
            HolidayID = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            IsActive = true;
            ErrorMessage = "";
        }

        #region Properties
        public int ACSHID { get; set; }
        public int ACSID { get; set; }
        public int HolidayID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string HolidayDescription { get; set; }
        public List<Holiday> Holidays { get; set; }
        public List<AttendanceCalendarSessionHoliday> AttendanceCalendarSessionHolidays { get; set; }
        public string StartDateInString
        {
            get
            {
                return this.StartDate.ToString("dd MMM yyyy");
            }
        }
        public string EndDateInString
        {
            get
            {
                return this.EndDate.ToString("dd MMM yyyy");
            }
        }
        #endregion

        #region Functions
        public AttendanceCalendarSessionHoliday Get(int id, long nUserID)
        {
            return AttendanceCalendarSessionHoliday.Service.Get(id, nUserID);
        }
        public static List<AttendanceCalendarSessionHoliday> Gets(int nACSID, long nUserID)
        {
            return AttendanceCalendarSessionHoliday.Service.Gets(nACSID, nUserID);
        }
        public static List<AttendanceCalendarSessionHoliday> Gets(string sSQL, long nUserID)
        {
            return AttendanceCalendarSessionHoliday.Service.Gets(sSQL, nUserID);
        }
        public AttendanceCalendarSessionHoliday IUD(int nDBOperation, long nUserID)
        {
            return AttendanceCalendarSessionHoliday.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IAttendanceCalendarSessionHolidayService Service
        {
            get { return (IAttendanceCalendarSessionHolidayService)Services.Factory.CreateService(typeof(IAttendanceCalendarSessionHolidayService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceCalendarSessionHoliday interface

    public interface IAttendanceCalendarSessionHolidayService
    {
        AttendanceCalendarSessionHoliday Get(int id, Int64 nUserID);
        List<AttendanceCalendarSessionHoliday> Gets(int nACSID, Int64 nUserID);
        List<AttendanceCalendarSessionHoliday> Gets(string sSQL, Int64 nUserID);
        AttendanceCalendarSessionHoliday IUD(AttendanceCalendarSessionHoliday oAttendanceCalendarSessionHoliday, int nDBOperation, Int64 nUserID);
    }
    #endregion
}
