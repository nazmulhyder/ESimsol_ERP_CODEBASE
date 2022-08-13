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

    #region AttendanceSchemeHoliday

    public class AttendanceSchemeHoliday : BusinessObject
    {
        public AttendanceSchemeHoliday()
        {
            AttendanceSchemeHolidayID = 0;
            AttendanceSchemeID = 0;
            HolidayID = 0;
            DayQty = 0;
            DayOfMonth = "";
            ErrorMessage = "";
        }

        #region Properties
        public int AttendanceSchemeHolidayID { get; set; }
        public int AttendanceSchemeID { get; set; }
        public int HolidayID { get; set; }
        public int DayQty { get; set; }
        public string DayOfMonth { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string HoliDayName { get; set; }
        public string DescriptionWithDayOfMonth
        {
            get
            {
                if (DayOfMonth != "")
                    return HoliDayName + "(" + DayOfMonth + ")";
                else
                    return HoliDayName;
            }
        }

        #endregion

        #region Functions
        public static List<AttendanceSchemeHoliday> Gets(int nAttendanceSchemeID, long nUserID)
        {
            return AttendanceSchemeHoliday.Service.Gets(nAttendanceSchemeID, nUserID);
        }
        public static List<AttendanceSchemeHoliday> Gets(string sSQL, long nUserID)
        {
            return AttendanceSchemeHoliday.Service.Gets(sSQL, nUserID);
        }
        public AttendanceSchemeHoliday Get(int id, long nUserID)
        {
            return AttendanceSchemeHoliday.Service.Get(id, nUserID);
        }
        public AttendanceSchemeHoliday IUD(int nDBOperation, long nUserID)
        {
            return AttendanceSchemeHoliday.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IAttendanceSchemeHolidayService Service
        {
            get { return (IAttendanceSchemeHolidayService)Services.Factory.CreateService(typeof(IAttendanceSchemeHolidayService)); }
        }
        #endregion
    }
    #endregion

    #region IAttendanceSchemeHoliday interface

    public interface IAttendanceSchemeHolidayService
    {
        AttendanceSchemeHoliday Get(int id, Int64 nUserID);
        List<AttendanceSchemeHoliday> Gets(int nAttendanceSchemeID, Int64 nUserID);
        List<AttendanceSchemeHoliday> Gets(string sSQL, Int64 nUserID);
        AttendanceSchemeHoliday IUD(AttendanceSchemeHoliday oAttendanceSchemeHoliday, int nDBOperation, Int64 nUserID);
    }
    #endregion

}
