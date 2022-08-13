using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    class SP_GeneralJournalDA
    {
        public SP_GeneralJournalDA() { }

        #region Inseret Update Delete
        public static IDataReader InsertUpdate(TransactionContext tc, SP_GeneralJournal oSP_GeneralJournal, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("Not Required");
        }

        public static void Delete(TransactionContext tc, SP_GeneralJournal oSP_GeneralJournal, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Not Required"); ;
        }

        #endregion

        #region Get & Exist Function

        public static IDataReader GetsGeneralJournal(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("Exec [SP_GeneralJournal] %s", sSQL);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL, int nCompanyID)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets_SuspendALog(TransactionContext tc, string sSQL, int nCompanyID)
        {
            sSQL = "SELECT COA.AccountCode,COA.AccountHeadName ,SuspendAccountLog.* FROM SuspendAccountLog INNER JOIN ChartsOfAccount AS COA ON COA.AccountHeadID=SuspendAccountLog.AccountHeadID ORDER BY SuspendAccountLog.DBServerDateTime DESC";
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
