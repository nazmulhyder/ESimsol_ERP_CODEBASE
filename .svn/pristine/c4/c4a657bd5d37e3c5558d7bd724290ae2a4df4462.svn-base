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

    public class AttendanceSchemeDA
    {

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, AttendanceScheme oAttendanceScheme, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceScheme] %n,%n,%n,%n,%s,%s,%n,%b,%n,%n,%b,%b,%n,%n,%b,%n,%n,%b,%n,%n",
                oAttendanceScheme.AttendanceSchemeID, oAttendanceScheme.CompanyID, oAttendanceScheme.RosterPlanID, oAttendanceScheme.AttendanceCalenderID, oAttendanceScheme.Name, oAttendanceScheme.Code, oAttendanceScheme.OvertimeCalculateInMinuteAfter, oAttendanceScheme.Accomodation, oAttendanceScheme.AccommodationDeferredDay, oAttendanceScheme.AccommodationActivationAfter, oAttendanceScheme.EnforceMonthClosingRoster, oAttendanceScheme.OverTime, oAttendanceScheme.OverTimeDeferredDay, oAttendanceScheme.OverTimeActivationAfter, oAttendanceScheme.IsOTCalTimeStartFromShiftStart, oAttendanceScheme.MaxOTInMinutePerDay, oAttendanceScheme.BreakageTimeInMinute, oAttendanceScheme.IsActive, nUserID, nDBOperation);
        }



        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceScheme WHERE AttendanceSchemeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_AttendanceScheme");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
}
