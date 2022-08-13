using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class TrailBalanceDA
    {
        public TrailBalanceDA() { }

        #region Insert Update Delete Function
       
        #endregion

        #region Get & Exist Function       
      
        public static IDataReader Gets(TransactionContext tc, int nAccountHead, int AccountTypeInInt, DateTime dStartDate, DateTime dEndDate, int nBusinessUnitID)
        {
            return tc.ExecuteReader("EXEC [SP_TrailBalance]" + "%n ,%n ,%d ,%d ,%n", nAccountHead, AccountTypeInInt, dStartDate, dEndDate, nBusinessUnitID);
        }

        public static IDataReader ProcessTrialBalance(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sStartLedgerCode, string sEndLedgerCode, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_Process_TrailBalance]" + "%d, %d, %s, %s, %s", dStartDate, dEndDate, sStartLedgerCode, sEndLedgerCode, sSQL);
        }   
        #endregion
    }
}
