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
    public class AttendanceCalendarDA
    {
        public AttendanceCalendarDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, AttendanceCalendar oAttendanceCalendar, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceCalendar]"
                                    + "%n, %s, %s, %b, %n, %n", oAttendanceCalendar.AttendanceCalendarID, oAttendanceCalendar.Name, oAttendanceCalendar.Description, oAttendanceCalendar.IsActive, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM AttendanceCalendar WHERE AttendanceCalendarID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM AttendanceCalendar");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
