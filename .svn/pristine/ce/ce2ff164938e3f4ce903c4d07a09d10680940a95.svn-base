using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AccountOpenningBreakdownDA
    {
        public AccountOpenningBreakdownDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AccountOpenningBreakdown oAccountOpenningBreakdown, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AccountOpenningBreakdown]"
                                    + "%n, %n, %n, %n, %n, %b, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                                    oAccountOpenningBreakdown.AccountOpenningBreakdownID, oAccountOpenningBreakdown.AccountingSessionID, oAccountOpenningBreakdown.BusinessUnitID, oAccountOpenningBreakdown.AccountHeadID, oAccountOpenningBreakdown.BreakdownObjID, oAccountOpenningBreakdown.IsDr, (int)oAccountOpenningBreakdown.BreakdownType, oAccountOpenningBreakdown.MUnitID, oAccountOpenningBreakdown.WUnitID, oAccountOpenningBreakdown.UnitPrice, oAccountOpenningBreakdown.Qty, oAccountOpenningBreakdown.CurrencyID, oAccountOpenningBreakdown.ConversionRate, oAccountOpenningBreakdown.AmountInCurrency, oAccountOpenningBreakdown.OpenningBalance, oAccountOpenningBreakdown.CCID, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, AccountOpenningBreakdown oAccountOpenningBreakdown, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sAccountOpenningBreakdownIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AccountOpenningBreakdown]"
                                    + "%n, %n, %n, %n, %n, %b, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                                    oAccountOpenningBreakdown.AccountOpenningBreakdownID, oAccountOpenningBreakdown.AccountingSessionID, oAccountOpenningBreakdown.BusinessUnitID, oAccountOpenningBreakdown.AccountHeadID, oAccountOpenningBreakdown.BreakdownObjID, oAccountOpenningBreakdown.IsDr, (int)oAccountOpenningBreakdown.BreakdownType, oAccountOpenningBreakdown.MUnitID, oAccountOpenningBreakdown.WUnitID, oAccountOpenningBreakdown.UnitPrice, oAccountOpenningBreakdown.Qty, oAccountOpenningBreakdown.CurrencyID, oAccountOpenningBreakdown.ConversionRate, oAccountOpenningBreakdown.AmountInCurrency, oAccountOpenningBreakdown.OpenningBalance, oAccountOpenningBreakdown.CCID, nUserID, (int)eEnumDBOperation, sAccountOpenningBreakdownIDs);
        }
        public static void ResetOpeningBreakdown(TransactionContext tc, int nAccountingSessionID, int nAccountHeadID)
        {
            tc.ExecuteNonQuery("DELETE FROM AccountOpenningBreakdown WHERE AccountingSessionID=%n AND AccountHeadID=%n", nAccountingSessionID, nAccountHeadID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountOpenningBreakdown WHERE AccountOpenningBreakdownID=%n AND CompanyID=%n", nID, nCompanyID);
        }

        public static IDataReader Gets(TransactionContext tc, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountOpenningBreakdown WHERE CompanyID=%n",nCompanyID);
        }
        public static IDataReader GetsByAccountAndSession(TransactionContext tc, int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountOpenningBreakdown WHERE AccountHeadID =%n AND AccountingSessionID=%n AND BusinessUnitID=%n ORDER BY BreakdownType", nAccountHeadID, nAccountingSessionID, nBusinessUnitID);
        }
        public static IDataReader GetsByAccountAndSubledgerAndSession(TransactionContext tc, int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountOpenningBreakdown WHERE AccountHeadID =%n AND AccountingSessionID=%n AND BusinessUnitID=%n AND BreakdownObjID=%n AND BreakdownType=1 ORDER BY BreakdownType", nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubLedgerID);
        }
        public static IDataReader GetsSubLedgerWiseBills(TransactionContext tc, int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountOpenningBreakdown WHERE AccountHeadID =%n AND AccountingSessionID=%n AND BusinessUnitID=%n AND CCID=%n AND BreakdownType=2 ORDER BY BreakdownType", nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubLedgerID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }      
        #endregion
    }
}
