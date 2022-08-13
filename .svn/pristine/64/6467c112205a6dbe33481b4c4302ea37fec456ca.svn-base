using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class TrialBalance_CategorizedDA
    {
        public TrialBalance_CategorizedDA() { }

        #region Insert Update Delete Function
       
        #endregion

        #region Get & Exist Function       
      
        public static IDataReader Gets(TransactionContext tc, int nAccountHead, DateTime dStartDate, DateTime dEndDate, bool bIsApproved,int nCurrencyID, int nBusinessUnitID)
        {
            return tc.ExecuteReader("EXEC [SP_TrialBalance_Categorized]" + "%n ,%d ,%d, %b, %n, %n", nAccountHead, dStartDate, dEndDate, bIsApproved, nCurrencyID, nBusinessUnitID);
        }

        public static IDataReader ProcessTrialBalance(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sStartLedgerCode, string sEndLedgerCode, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_Process_TrialBalance_Categorized]" + "%d, %d, %s, %s, %s", dStartDate, dEndDate, sStartLedgerCode, sEndLedgerCode, sSQL);
        }   
        #endregion
    }
}
