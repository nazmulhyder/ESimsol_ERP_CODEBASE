using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class BankReconciliationDA
    {
        public BankReconciliationDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BankReconciliation oBankReconciliation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BankReconciliation]" + "%n, %n, %n, %n, %n, %n, %d, %s, %b, %n, %n, %n, %n, %n",
                                    oBankReconciliation.BankReconciliationID, oBankReconciliation.SubledgerID, oBankReconciliation.VoucherDetailID, oBankReconciliation.CCTID, oBankReconciliation.AccountHeadID, oBankReconciliation.ReconcilHeadID, oBankReconciliation.ReconcilDate, oBankReconciliation.ReconcilRemarks, oBankReconciliation.IsDebit, oBankReconciliation.Amount, oBankReconciliation.ReconcilStatusInt, oBankReconciliation.ReconcilBy,  (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, BankReconciliation oBankReconciliation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BankReconciliation]" + "%n, %n, %n, %n, %n, %n, %d, %s, %b, %n, %n, %n, %n, %n",
                                    oBankReconciliation.BankReconciliationID, oBankReconciliation.SubledgerID, oBankReconciliation.VoucherDetailID, oBankReconciliation.CCTID, oBankReconciliation.AccountHeadID, oBankReconciliation.ReconcilHeadID, oBankReconciliation.ReconcilDate, oBankReconciliation.ReconcilRemarks, oBankReconciliation.IsDebit, oBankReconciliation.Amount, oBankReconciliation.ReconcilStatusInt, oBankReconciliation.ReconcilBy, (int)eEnumDBOperation, nUserID);
        }
        public static void ApprovedReconciliation(TransactionContext tc, int nReconciliationID, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE BankReconciliation SET ReconcilStatus=1, ReconcilBy=%n WHERE BankReconciliationID=%n", nUserID, nReconciliationID);
        }
        public static void UpdateRemarks(TransactionContext tc, BankReconciliation oBankReconciliation)
        {
            tc.ExecuteNonQuery("UPDATE BankReconciliation SET ReconcilRemarks=%s WHERE BankReconciliationID=%n", oBankReconciliation.ReconcilRemarks, oBankReconciliation.BankReconciliationID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankReconciliation WHERE BankReconciliationID=%n", nID);
        }
        public static IDataReader GetLastTransaction(TransactionContext tc, int nSubledger)
        {
            return tc.ExecuteReader("SELECT TOP 1 * FROM View_BankReconciliation AS MM WHERE MM.ReconcilDate = ISNULL((SELECT MAX(HH.ReconcilDate) FROM View_BankReconciliation AS HH WHERE HH.SubledgerID=%n),GETDATE())", nSubledger);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BankReconciliation");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader PrepareBankReconciliation(TransactionContext tc, BankReconciliation oBankReconciliation)
        {
            return tc.ExecuteReader("EXEC [SP_BankReconciliationPrepare] %n, %n, %n, %d, %d",
                                    oBankReconciliation.BUID, oBankReconciliation.SubledgerID, oBankReconciliation.CurrencyID, oBankReconciliation.StartDate, oBankReconciliation.EndDate);
        }
        #endregion
    }  
}
