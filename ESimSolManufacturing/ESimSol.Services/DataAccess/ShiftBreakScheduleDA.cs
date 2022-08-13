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
    public class ShiftBreakScheduleDA
    {
        public ShiftBreakScheduleDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, ShiftBreakSchedule oShiftBreakSchedule, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShiftBreakSchedule] %n,%n,%n,%D,%D,%b,%n,%n",
                   oShiftBreakSchedule.ShiftBScID, oShiftBreakSchedule.ShiftID, oShiftBreakSchedule.ShiftBNID,
                   oShiftBreakSchedule.StartTime, oShiftBreakSchedule.EndTime, oShiftBreakSchedule.IsActive,
                   nUserID, nDBOperation);

        }
        public static IDataReader Activity(int nShiftBScID, TransactionContext tc)
        {
            return tc.ExecuteReader("UPDATE ShiftBreakSchedule SET IsActive=~IsActive WHERE ShiftBScID=%n;SELECT * FROM View_ShiftBreakSchedule WHERE ShiftBScID=%n", nShiftBScID, nShiftBScID);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nShiftBScID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShiftBreakSchedule WHERE ShiftBScID=%n", nShiftBScID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShiftBreakSchedule");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
