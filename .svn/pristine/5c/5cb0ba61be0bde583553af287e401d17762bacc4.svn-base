using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportOutstandingDA
    {
        public ImportOutstandingDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, DateTime dFromDODate, DateTime dToDODate, int nBUID, int nCurrencyID,int nDate)
        {
            return tc.ExecuteReader("EXEC [sp_ImportOutstanding]" + "%d, %d, %n,%n,%n", dFromDODate, dToDODate, nBUID, nCurrencyID, nDate);
        }
        public static IDataReader GetsImportOutstandingReport(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
