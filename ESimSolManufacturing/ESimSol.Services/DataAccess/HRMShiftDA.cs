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
    class HRMShiftDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, HRMShift oHRMShift, EnumDBOperation eEnumDBHRMShift, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_HRMShift]"
                                    + "%n,%s,%s,%s,%D,%D,%D,%n,%D,%D,%D,%b,%n,%b,%D,%D,%b,%n,%n,%b,%n,%b,%D,%b,%n,%b,%D,%D  ",
                                    oHRMShift.ShiftID, oHRMShift.Code, oHRMShift.Name, oHRMShift.NameBangla, oHRMShift.ReportTime, oHRMShift.StartTime, oHRMShift.EndTime, oHRMShift.TotalWorkingTime, oHRMShift.ToleranceTime, oHRMShift.DayStartTime, oHRMShift.DayEndTime, oHRMShift.IsActive, nUserId, oHRMShift.IsOT, oHRMShift.OTStartTime, oHRMShift.OTEndTime, oHRMShift.IsOTOnActual, oHRMShift.OTCalculateAfterInMinute, oHRMShift.MaxOTComplianceInMin, oHRMShift.IsLeaveOnOFFHoliday, (int)eEnumDBHRMShift, oHRMShift.IsOutOrOT, oHRMShift.CompMaxEndTime,oHRMShift.IsWithComp, oHRMShift.ToleranceForEarlyInMin,oHRMShift.IsHalfDayOff, oHRMShift.PStart, oHRMShift.PEnd);
        }

        public static void Delete(TransactionContext tc, HRMShift oHRMShift, EnumDBOperation eEnumDBHRMShift, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_HRMShift]"
                                   + "%n,%s,%s,%s,%D,%D,%D,%n,%D,%D,%D,%b,%n,%b,%D,%D,%b,%n,%n,%b,%n,%b,%D,%b,%n,%b,%D,%D   ",
                                   oHRMShift.ShiftID, oHRMShift.Code, oHRMShift.Name, oHRMShift.NameBangla, oHRMShift.ReportTime, oHRMShift.StartTime, oHRMShift.EndTime, oHRMShift.TotalWorkingTime, oHRMShift.ToleranceTime, oHRMShift.DayStartTime, oHRMShift.DayEndTime, oHRMShift.IsActive, nUserId, oHRMShift.IsOT, oHRMShift.OTStartTime, oHRMShift.OTEndTime, oHRMShift.IsOTOnActual, oHRMShift.OTCalculateAfterInMinute, oHRMShift.MaxOTComplianceInMin, oHRMShift.IsLeaveOnOFFHoliday, (int)eEnumDBHRMShift, oHRMShift.IsOutOrOT, oHRMShift.CompMaxEndTime, oHRMShift.IsWithComp, oHRMShift.ToleranceForEarlyInMin, oHRMShift.IsHalfDayOff, oHRMShift.PStart, oHRMShift.PEnd);
        }


        public static IDataReader ShiftInActive(int nShiftID, int ntRShiftID, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_Process_HRMShiftShiftInActive]%n,%n",nShiftID, ntRShiftID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM HRM_Shift WHERE ShiftID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM HRM_Shift");
        }
        public static IDataReader BUWiseGets(int BUID,  TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM HRM_Shift WHERE ShiftID IN (SELECT ShiftID FROM BUWiseShift WHERE BUID = "+BUID+") AND ISNULL(IsActive,0) = 1 ");
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion

        //internal static IDataReader ShiftInActive(TransactionContext tc,int nShiftID,int ntRShiftID,long nUserId)
        //{
        //    throw new NotImplementedException();
        //}}

    }
}
