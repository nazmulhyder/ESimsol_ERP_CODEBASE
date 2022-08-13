using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class HolidayCalendar : BusinessObject
    {
        public HolidayCalendar()
        {
            HolidayCalendarID = 0;
            CalendarNo = "";
            CalendarName = "";
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            Remarks = "";
            TotalHolidays = 0;
            ErrorMessage = "";
            //EmployeeBatchDetails = new List<EmployeeBatchDetail>();
        }
        #region Properties

        public int HolidayCalendarID { get; set; }
        public string CalendarNo { get; set; }
        public string CalendarName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalHolidays { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        //public List<EmployeeBatchDetail> EmployeeBatchDetails { get; set; }
        #endregion
        #region derived properties
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
        public HolidayCalendar Save(long nUserID)
        {
            return HolidayCalendar.Service.Save(this, nUserID);
        }
        public static List<HolidayCalendar> Gets(string sSQL, long nUserID)
        {
            return HolidayCalendar.Service.Gets(sSQL, nUserID);
        }
        public HolidayCalendar Get(int id, long nUserID)
        {
            return HolidayCalendar.Service.Get(id, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return HolidayCalendar.Service.Delete(id, nUserID);
        }

     
        #region ServiceFactory
        internal static IHolidayCalendarService Service
        {
            get { return (IHolidayCalendarService)Services.Factory.CreateService(typeof(IHolidayCalendarService)); }
        }
        #endregion
    }
    #region IHolidayCalendarService interface

    public interface IHolidayCalendarService
    {
        HolidayCalendar Save(HolidayCalendar oHolidayCalendar, Int64 nUserID);
        List<HolidayCalendar> Gets(string sSQL, long nUserID);
        HolidayCalendar Get(int id, long nUserID);
        string Delete(int id, long nUserID);
      
    }
    #endregion
}
