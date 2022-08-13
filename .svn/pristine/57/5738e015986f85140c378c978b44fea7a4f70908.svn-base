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
    public class HolidayDA
    {
        public HolidayDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Holiday oHoliday, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Holiday]"
                                    + "%n, %n, %s, %n,%s, %b, %n, %n", oHoliday.HolidayID, oHoliday.Code, oHoliday.Description, (int)oHoliday.TypeOfHoliday,oHoliday.DayOfMonth, oHoliday.IsActive, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Holiday oHoliday, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Holiday]"
                                    + "%n, %n, %s, %n,%s, %b, %n, %n", oHoliday.HolidayID, oHoliday.Code, oHoliday.Description, (int)oHoliday.TypeOfHoliday, oHoliday.DayOfMonth, oHoliday.IsActive, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM Holiday WHERE HolidayID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Holiday");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static void ChangeActiveStatus(TransactionContext tc, Holiday oHoliday)
        {
            tc.ExecuteNonQuery("Update Holiday SET IsActive=%b WHERE HolidayID=%n", oHoliday.IsActive, oHoliday.HolidayID);
        }
        #endregion
    }
}
