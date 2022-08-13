using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AHOpeningBreakdownDA
    {
        public AHOpeningBreakdownDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nBUID, int nAccountHeadID, int nCurrencyID, DateTime StartDate, bool bIsApproved)
        {
            return tc.ExecuteReader("EXEC [SP_AHOpeningBreakdown]" + "%n,%d,%n,%b,%n", nAccountHeadID, StartDate, nCurrencyID, bIsApproved, nBUID);
        }

       
        #endregion
    }
    

}
