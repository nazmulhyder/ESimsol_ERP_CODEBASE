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

    public class AttendanceSchemeLeaveDA
    {

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, AttendanceSchemeLeave oAttendanceSchemeLeave, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceSchemeLeave] %n,%n,%n,%n,%n,%n,%b,%n,%b,%n,%n,%n,%b", oAttendanceSchemeLeave.AttendanceSchemeLeaveID, oAttendanceSchemeLeave.AttendanceSchemeID, oAttendanceSchemeLeave.LeaveID, oAttendanceSchemeLeave.TotalDay, oAttendanceSchemeLeave.DeferredDay, oAttendanceSchemeLeave.ActivationAfter,
                oAttendanceSchemeLeave.IsLeaveOnPresence, oAttendanceSchemeLeave.PresencePerLeave, oAttendanceSchemeLeave.IsCarryForward, oAttendanceSchemeLeave.MaxCarryDays, nUserID, nDBOperation,oAttendanceSchemeLeave.IsComp);
        }


        public static void Delete(TransactionContext tc, int nAttendanceSchemeID, string sAttendanceSchemeLeaveIDs)
        {
            tc.ExecuteNonQuery(
                "DELETE FROM AttendanceSchemeLeave WHERE AttendanceSchemeID= %n AND AttendanceSchemeLeaveID NOT IN (%q)", nAttendanceSchemeID, sAttendanceSchemeLeaveIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceSchemeLeave WHERE AttendanceSchemeLeaveID=%n", nID);
        }

        public static IDataReader Gets(int nAttendanceSchemeID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_AttendanceSchemeLeave where AttendanceSchemeID=%n", nAttendanceSchemeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
}
