using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VoucherBillBreakDownDA
    {
        public VoucherBillBreakDownDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nAccountHeadID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool bIsApproved, int nCompanyID)
        {
            return tc.ExecuteReader("EXEC [SP_VoucherBillBreakdown]" + "%n,%n,%d,%d,%b,%n", nAccountHeadID, nCurrencyID, StartDate, EndDate, bIsApproved, nCompanyID);
        }
        public static IDataReader GetsForVoucherBill(TransactionContext tc, int nAccountHeadID, int nVoucherBillID, int nCurrencyID, DateTime StartDate, DateTime EndDate, bool IsApproved, int nCompanyID)
        {
            return tc.ExecuteReader("EXEC [SP_VoucherBillLedger]" + "%n,%n,%n,%d,%d,%b,%n", nAccountHeadID, nVoucherBillID, nCurrencyID, StartDate, EndDate, IsApproved, nCompanyID);
        }
        #endregion
    }
}
