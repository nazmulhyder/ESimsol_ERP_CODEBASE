using ESimSol.BusinessObjects;
using ICS.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.DataAccess
{
    public class NonNegativeLedgerDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, NonNegativeLedger oNonNegativeLedger, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_NonNegativeLedger]"
                                   + "%n, %n, %n, %s, %n, %n",
                                   oNonNegativeLedger.NonNegativeLedgerID, oNonNegativeLedger.BUID, oNonNegativeLedger.AccountHeadID, oNonNegativeLedger.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, NonNegativeLedger oNonNegativeLedger, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_NonNegativeLedger]"
                                   + "%n, %n, %n, %s, %n, %n",
                                   oNonNegativeLedger.NonNegativeLedgerID, oNonNegativeLedger.BUID, oNonNegativeLedger.AccountHeadID, oNonNegativeLedger.Remarks, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function

        //public static IDataReader Get(TransactionContext tc, long nID)
        //{
        //    return tc.ExecuteReader("SELECT * FROM View_HolidayCalendar WHERE HolidayCalendarID=%n", nID);
        //}

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
