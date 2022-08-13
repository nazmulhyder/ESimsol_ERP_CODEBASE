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
    public class HolidayCalendarDetailDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, HolidayCalendarDetail oHolidayCalendarDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_HolidayCalendarDetail]"
                                   + "%n,%n,%n,%n,%s,%d,%d,%n,%n",
                                   oHolidayCalendarDetail.HolidayCalendarDetailID, oHolidayCalendarDetail.HolidayCalendarID, oHolidayCalendarDetail.HolidayID,oHolidayCalendarDetail.CalendarApply, oHolidayCalendarDetail.Remarks, oHolidayCalendarDetail.StartDate, oHolidayCalendarDetail.EndDate,  (int)eEnumDBOperation,nUserID);
        }

        public static void Delete(TransactionContext tc, HolidayCalendarDetail oHolidayCalendarDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_HolidayCalendarDetail]"
                                    + "%n,%n,%n,%n,%s,%d,%d,%n,%n",
                                   oHolidayCalendarDetail.HolidayCalendarDetailID, oHolidayCalendarDetail.HolidayCalendarID, oHolidayCalendarDetail.HolidayID,oHolidayCalendarDetail.CalendarApply, oHolidayCalendarDetail.Remarks, oHolidayCalendarDetail.StartDate, oHolidayCalendarDetail.EndDate, (int)eEnumDBOperation, nUserID);
        }


        #endregion
    
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_HolidayCalendarDetail WHERE HolidayCalendarDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
