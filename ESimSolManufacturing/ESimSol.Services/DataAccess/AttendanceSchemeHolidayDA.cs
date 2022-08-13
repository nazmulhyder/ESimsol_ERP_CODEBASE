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
    public class AttendanceSchemeHolidayDA
    {

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, AttendanceSchemeHoliday oAttendanceSchemeHoliday, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceSchemeHoliday] %n,%n,%n,%n,%n,%n", oAttendanceSchemeHoliday.AttendanceSchemeHolidayID, oAttendanceSchemeHoliday.AttendanceSchemeID, oAttendanceSchemeHoliday.HolidayID, oAttendanceSchemeHoliday.DayQty, nUserID, nDBOperation);
        }

        public static void Delete(TransactionContext tc, int nAttendanceSchemeID, string sAttendanceSchemeHolidayIDs)
        {
            tc.ExecuteNonQuery(
                "DELETE FROM AttendanceSchemeHoliday WHERE AttendanceSchemeID= %n AND AttendanceSchemeHolidayID NOT IN (%q)", nAttendanceSchemeID, sAttendanceSchemeHolidayIDs);
        }



        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceSchemeHoliday WHERE AttendanceSchemeHolidayID=%n", nID);
        }

        public static IDataReader Gets(int nAttendanceSchemeID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_AttendanceSchemeHoliday where AttendanceSchemeID=%n", nAttendanceSchemeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
