using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VPTransactionSummaryDA
    {
        public VPTransactionSummaryDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string BUIDs, int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved)
        {
            return tc.ExecuteReader("EXEC [SP_VPTransactionSummary]" + "%s,%n,%n,%d,%d,%b", BUIDs, nAccountHeadID, nCurrencyID, StartDate, EndDate, bIsApproved);
        }

        public static IDataReader GetsForProduct(TransactionContext tc, string BUIDs, int nAccountHeadID, int nProductID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved)
        {
            return tc.ExecuteReader("EXEC [SP_VPTransactionLedger]" + "%s,%n,%n,%n,%d,%d,%b", BUIDs, nAccountHeadID, nProductID, nCurrencyID, StartDate, EndDate, IsApproved);
        }
     
        #endregion
    }
    

}
