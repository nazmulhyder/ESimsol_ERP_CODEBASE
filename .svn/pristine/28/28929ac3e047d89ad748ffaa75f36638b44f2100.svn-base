using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class HolidayCalendarDRPDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, HolidayCalendarDRP oHolidayCalendarDRP, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_HolidayCalendarDRP]"
                                   + "%n, %n, %n, %n,%n,%s",
                                  oHolidayCalendarDRP.HolidayCalendarDRPID, oHolidayCalendarDRP.HolidayCalendarDetailID, oHolidayCalendarDRP.DRPID,(int)eEnumDBOperation, nUserID, sIDs);
        }

        public static void Delete(TransactionContext tc, HolidayCalendarDRP oHolidayCalendarDRP, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_HolidayCalendarDRP]"
                                  + "%n, %n, %n, %n,%n,%s",
                                  oHolidayCalendarDRP.HolidayCalendarDRPID, oHolidayCalendarDRP.HolidayCalendarDetailID, oHolidayCalendarDRP.DRPID, (int)eEnumDBOperation, nUserID, sIDs);
        }

        #endregion

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_HolidayCalendarDRP WHERE HolidayCalendarDetailID = %n Order By HolidayCalendarDRPID", id);
        }

      
    }
}
