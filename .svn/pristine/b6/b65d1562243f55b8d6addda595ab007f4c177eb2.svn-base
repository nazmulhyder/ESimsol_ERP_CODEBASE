using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class GLMonthlySummaryDA
    {
        public GLMonthlySummaryDA() { }

        #region Insert Update Delete Function
       
        #endregion

        #region Get & Exist Function       
      
        public static IDataReader Gets(TransactionContext tc, int nAccountHead, DateTime dStartDate, DateTime dEndDate, int nCurrencyID, bool bISApproved, string nBusinessUnitIDs)
        {
            return tc.ExecuteReader("EXEC [SP_GLMonthlySummary]" + "%n ,%d ,%d ,%n, %b, %s", nAccountHead, dStartDate, dEndDate, nCurrencyID, bISApproved, nBusinessUnitIDs);
        }

        #endregion
    }
}
