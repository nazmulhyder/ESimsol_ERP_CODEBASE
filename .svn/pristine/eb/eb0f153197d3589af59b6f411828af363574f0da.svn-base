using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BankReconciliationOpenningDA
    {
        public BankReconciliationOpenningDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BankReconciliationOpenning oBankReconciliationOpenning, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BankReconciliationOpenning]" + "%n, %n, %n, %n, %n, %b, %n, %n, %n, %n, %n, %n",
                                    oBankReconciliationOpenning.BankReconciliationOpenningID, oBankReconciliationOpenning.AccountingSessionID, oBankReconciliationOpenning.BusinessUnitID, oBankReconciliationOpenning.AccountHeadID, oBankReconciliationOpenning.SubledgerID, oBankReconciliationOpenning.IsDr, oBankReconciliationOpenning.CurrencyID, oBankReconciliationOpenning.ConversionRate, oBankReconciliationOpenning.AmountInCurrency, oBankReconciliationOpenning.OpenningBalance, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, BankReconciliationOpenning oBankReconciliationOpenning, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BankReconciliationOpenning]" + "%n, %n, %n, %n, %n, %b, %n, %n, %n, %n, %n, %n",
                                    oBankReconciliationOpenning.BankReconciliationOpenningID, oBankReconciliationOpenning.AccountingSessionID, oBankReconciliationOpenning.BusinessUnitID, oBankReconciliationOpenning.AccountHeadID, oBankReconciliationOpenning.SubledgerID, oBankReconciliationOpenning.IsDr, oBankReconciliationOpenning.CurrencyID, oBankReconciliationOpenning.ConversionRate, oBankReconciliationOpenning.AmountInCurrency, oBankReconciliationOpenning.OpenningBalance, nUserID, (int)eEnumDBOperation);
        }
        public static void ResetOpeningBreakdown(TransactionContext tc, int nAccountingSessionID, int nAccountHeadID)
        {
            tc.ExecuteNonQuery("DELETE FROM BankReconciliationOpenning WHERE AccountingSessionID=%n AND AccountHeadID=%n", nAccountingSessionID, nAccountHeadID);
        }

        public static void BRBalanceTranfer(TransactionContext tc, BankReconciliationOpenning oBRO, Int64 nUserID)
        {
             tc.ExecuteNonQuery("EXEC [SP_BankReconciliationBalanceTransfer]" + "%n, %n, %n, %n, %n, %n",
                                    oBRO.AccountingSessionID, oBRO.BusinessUnitID, oBRO.SubledgerID, oBRO.AccountHeadID,  oBRO.CurrencyID, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankReconciliationOpenning WHERE BankReconciliationOpenningID=%n AND CompanyID=%n", nID, nCompanyID);
        }

        public static IDataReader Gets(TransactionContext tc, int nCompanyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankReconciliationOpenning WHERE CompanyID=%n",nCompanyID);
        }
        public static IDataReader GetsByAccountAndSession(TransactionContext tc, int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankReconciliationOpenning WHERE AccountHeadID =%n AND AccountingSessionID=%n AND BusinessUnitID=%n ORDER BY BreakdownType", nAccountHeadID, nAccountingSessionID, nBusinessUnitID);
        }
        public static IDataReader GetsByAccountAndSubledgerAndSession(TransactionContext tc, int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankReconciliationOpenning WHERE AccountHeadID =%n AND AccountingSessionID=%n AND BusinessUnitID=%n AND SubledgerID=%n", nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubLedgerID);
        }
        public static IDataReader GetsSubLedgerWiseBills(TransactionContext tc, int nAccountHeadID, int nAccountingSessionID, int nBusinessUnitID, int nSubLedgerID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankReconciliationOpenning WHERE AccountHeadID =%n AND AccountingSessionID=%n AND BusinessUnitID=%n AND CCID=%n AND BreakdownType=2 ORDER BY BreakdownType", nAccountHeadID, nAccountingSessionID, nBusinessUnitID, nSubLedgerID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }      
        #endregion
    }
}
