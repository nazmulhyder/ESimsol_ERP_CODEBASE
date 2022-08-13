using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportOutStandingDetailDA
    {
        public ImportOutStandingDetailDA() { }
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nBUID, int nLCPaymentType, int nBankBranchID, int nCurrencyID, DateTime stratDate, DateTime endDate,int nSPRptType,int nDate)
        {
            return tc.ExecuteReader("EXEC [sp_ImportOutstandingDetail]" + "%n, %n, %n,%n ,%d ,%d ,%n,%n", nBUID, nLCPaymentType, nBankBranchID, nCurrencyID, stratDate, endDate, nSPRptType,nDate);
        }
        #endregion
    }
}
