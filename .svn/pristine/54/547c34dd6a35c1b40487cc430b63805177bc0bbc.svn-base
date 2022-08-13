using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class CCOpeningBreakdownDA
    {
        public CCOpeningBreakdownDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string BUIDs,int nCCID, int nAccountHeadID, int nCurrencyID, DateTime StartDate, bool bIsApproved)
        {
            return tc.ExecuteReader("EXEC [SP_CCOpeningBreakdown]" + "%n,%n,%d,%n,%b,%s", nAccountHeadID, nCCID, StartDate, nCurrencyID, bIsApproved, BUIDs);
        }

       
        #endregion
    }
    

}
