using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class CostCenterBreakdownDA
    {
        public CostCenterBreakdownDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string BUIDs, int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved)
        {
            return tc.ExecuteReader("EXEC [SP_CostCenterBreakdown]" + "%s,%n,%n,%d,%d,%b", BUIDs, nAccountHeadID, nCurrencyID, StartDate, EndDate, bIsApproved);
        }

        public static IDataReader GetsAccountWiseBreakdown(TransactionContext tc, int nAccountHeadID, string BUIDs, int nCCID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, EnumBalanceStatus eBalanceStatus)
        {
            return tc.ExecuteReader("EXEC [SP_CCAccountWiseBreakdown]" + "%n, %s, %n, %n, %d, %d, %b, %n", nAccountHeadID, BUIDs, nCCID, nCurrencyID, StartDate, EndDate, bIsApproved, eBalanceStatus);
        }

        public static IDataReader GetsForCostCenter(TransactionContext tc, string BUIDs, int nAccountHeadID, int nCostCenterID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved)
        {
            return tc.ExecuteReader("EXEC [SP_CostCenterLedger]" + "%s,%n,%n,%n,%d,%d,%b", BUIDs, nAccountHeadID, nCostCenterID, nCurrencyID, StartDate, EndDate, IsApproved);
        }
     
        #endregion
    }
    

}
