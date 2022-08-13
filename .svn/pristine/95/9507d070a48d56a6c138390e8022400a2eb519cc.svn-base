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
    #region Holiday

    public class Holiday : BusinessObject
    {
        public Holiday()
        {
            HolidayID = 0;
            Code = 0;
            Description = "";
            DayOfMonth = "";
            TypeOfHoliday = EnumHolidayType.None;
            IsActive = true;
            ErrorMessage = "";
        }

        #region Properties

        public int HolidayID { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public EnumHolidayType TypeOfHoliday { get; set; }
        public string DayOfMonth { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        public int TypeOfHolidayInt { get; set; }
        public string TypeOfHolidayInString
        {
            get
            {
                return TypeOfHoliday.ToString();
            }
        }
         public string DescriptionWithDayOfMonth
        {
            get
            {
                if (DayOfMonth != "")
                    return Description + "(" + DayOfMonth + ")";
                else
                    return Description;
            }
        }

        public Holiday oHolidayInfo { get; set; }
        public List<Holiday> oHolidays = new List<Holiday>();

        public int AttendanceCalendarID { get; set; }
        #endregion

        #region Functions
        public static List<Holiday> Gets(long nUserID)
        {
            return Holiday.Service.Gets(nUserID);
        }

        public static List<Holiday> Gets(string sSQL, long nUserID)
        {
            return Holiday.Service.Gets(sSQL, nUserID);
        }

        public Holiday Get(int id, long nUserID)
        {
            return Holiday.Service.Get(id, nUserID);
        }

        public Holiday Save(long nUserID)
        {
            return Holiday.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return Holiday.Service.Delete(id, nUserID);
        }

        public string ChangeActiveStatus(Holiday oHoliday, long nUserID)
        {
            return Holiday.Service.ChangeActiveStatus(oHoliday, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IHolidayService Service
        {
            get { return (IHolidayService)Services.Factory.CreateService(typeof(IHolidayService)); }
        }

        #endregion
    }
    #endregion

    #region IHoliday interface

    public interface IHolidayService
    {

        Holiday Get(int id, Int64 nUserID);
        List<Holiday> Gets(Int64 nUserID);
        List<Holiday> Gets(string sSQL,Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        Holiday Save(Holiday oHoliday, Int64 nUserID);
        string ChangeActiveStatus(Holiday oHoliday, Int64 nUserID);
    }
    #endregion
}
