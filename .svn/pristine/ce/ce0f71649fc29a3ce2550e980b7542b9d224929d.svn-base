using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class AttendanceCalendarSessionHolidayDA
    {
        public AttendanceCalendarSessionHolidayDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, AttendanceCalendarSessionHoliday oAttendanceCalendarSessionHoliday, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceCalendarSessionHoliday]"
                + "%n, %n, %n, %d, %d, %b, %n, %n",
                oAttendanceCalendarSessionHoliday.ACSHID, oAttendanceCalendarSessionHoliday.ACSID, oAttendanceCalendarSessionHoliday.HolidayID, oAttendanceCalendarSessionHoliday.StartDate, oAttendanceCalendarSessionHoliday.EndDate, oAttendanceCalendarSessionHoliday.IsActive, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceCalendarSessionHoliday WHERE AttendanceCalendarSessionHolidayID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nACSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceCalendarSessionHoliday WHERE ACSID=%n", nACSID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
