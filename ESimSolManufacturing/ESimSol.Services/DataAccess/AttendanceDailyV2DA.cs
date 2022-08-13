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
    public class AttendanceDailyV2DA
    {


        public static IDataReader AttendanceDaily_Manual_Single(TransactionContext tc, AttendanceDailyV2 oAttendanceDailyV2, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AttendanceDaily_Manual_Single] %n,%D,%D,%n,%b,%b,%b,%n,%n,%s,%b,%n,%n",
                   oAttendanceDailyV2.AttendanceID,
                   oAttendanceDailyV2.InTime,
                   oAttendanceDailyV2.OutTime,
                   oAttendanceDailyV2.ShiftID,
                   oAttendanceDailyV2.IsOSD,
                   oAttendanceDailyV2.IsDayOff,
                   oAttendanceDailyV2.IsAbsent,
                   oAttendanceDailyV2.LateArrivalMinute,
                   oAttendanceDailyV2.EarlyDepartureMinute,
                   oAttendanceDailyV2.Remark,
                   oAttendanceDailyV2.IsManualOT,
                   oAttendanceDailyV2.OverTimeInMin,
                   nUserID
                   );
        }

        public static IDataReader AttendanceDaily_Manual_Single_Comp(TransactionContext tc, AttendanceDailyV2 oAttendanceDailyV2, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_MaxOTManualAttendanceUpdate] %n,%n,%D,%D,%b,%b,%b,%b,%b,%s,%s,%n",
                   oAttendanceDailyV2.MOCAID,
                   oAttendanceDailyV2.AttendanceID,
                   oAttendanceDailyV2.InTime,
                   oAttendanceDailyV2.OutTime,
                   oAttendanceDailyV2.IsOSD,
                   oAttendanceDailyV2.IsDayOff,
                   oAttendanceDailyV2.IsAbsent,
                   oAttendanceDailyV2.IsNoLate,
                   oAttendanceDailyV2.IsNoEarly,
                   oAttendanceDailyV2.Remark,
                   oAttendanceDailyV2.ManualUpdateHistory,
                   nUserID
                   );
        }

        public static void AssignLeave(TransactionContext tc, AttendanceDailyV2 oAttendanceDailyV2, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_AssignCompensatoryLeave]" + "%d, %n, %s,%n, %n",
                                    oAttendanceDailyV2.LeaveAssignDate, oAttendanceDailyV2.LeaveHeadID, oAttendanceDailyV2.MOCAIDs, oAttendanceDailyV2.CompensatoryLeaveType, nUserID);
        }
        public static IDataReader CompGets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static int GetTotalCount(TransactionContext tc, string sSql, Int64 nUserID)
        {
            int nTotalCount = 0;
            object oIndex = tc.ExecuteScalar(sSql);
            if (oIndex != null)
            {
                nTotalCount = Convert.ToInt32(oIndex);
            }
            return nTotalCount;
        }
    }
}
