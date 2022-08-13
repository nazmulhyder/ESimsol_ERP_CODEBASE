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
    public class AttendanceSchemeDayOffDA
    {

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, AttendanceSchemeDayOff oAttendanceSchemeDayOff, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceSchemeDayOff] %n,%n,%s,%n,%b,%n,%D,%D,%n,%n ", oAttendanceSchemeDayOff.AttendanceSchemeDayOffID, oAttendanceSchemeDayOff.AttendanceSchemeID, oAttendanceSchemeDayOff.WeekDay, oAttendanceSchemeDayOff.DayOffType, oAttendanceSchemeDayOff.IsAlternateFromFirstWeek, oAttendanceSchemeDayOff.NoOfRandomDayOff, oAttendanceSchemeDayOff.InTime,oAttendanceSchemeDayOff.OutTime,  nUserID, nDBOperation);
        }


        public static void Delete(TransactionContext tc, int nAttendanceSchemeID, string sAttendanceSchemeDayOffIDs)
        {
            tc.ExecuteNonQuery(
                "DELETE FROM AttendanceSchemeDayOff WHERE AttendanceSchemeID= %n AND AttendanceSchemeDayOffID NOT IN (%q)", nAttendanceSchemeID, sAttendanceSchemeDayOffIDs);
        }



        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM AttendanceSchemeDayOff WHERE AttendanceSchemeDayOffID=%n", nID);
        }

        public static IDataReader Gets(int nAttendanceSchemeID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from AttendanceSchemeDayOff where AttendanceSchemeID==%n", nAttendanceSchemeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
