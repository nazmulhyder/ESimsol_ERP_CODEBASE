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
    public class AttendanceCalendarSessionDA
    {
        public AttendanceCalendarSessionDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, AttendanceCalendarSession oAttendanceCalendarSession, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceCalendarSession] %n, %n, %s, %d, %d, %s, %b, %n, %n", oAttendanceCalendarSession.ACSID, oAttendanceCalendarSession.AttendanceCalendarID, oAttendanceCalendarSession.Session, oAttendanceCalendarSession.StartDate, oAttendanceCalendarSession.EndDate, oAttendanceCalendarSession.Description, oAttendanceCalendarSession.IsActive, nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceCalendarSession WHERE ACSID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nAttendanceCalendarID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceCalendarSession WHERE AttendanceCalendarID =%n", nAttendanceCalendarID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader ChangeActiveStatus(TransactionContext tc, AttendanceCalendarSession oACS, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Active_AttendanceCalendarSession] %n, %n", oACS.ACSID, nUserID);
        }
        
        #endregion
    }
}