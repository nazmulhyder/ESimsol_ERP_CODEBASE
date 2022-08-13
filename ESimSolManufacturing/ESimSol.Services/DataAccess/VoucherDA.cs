using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VoucherDA
    {
        public VoucherDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Voucher oVoucher, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Voucher]"
                                    + "%n, %n, %n, %s, %s, %s, %d, %n, %d,  %n, %n, %n, %n, %n",
                                    oVoucher.VoucherID, oVoucher.BUID, oVoucher.VoucherTypeID, oVoucher.VoucherNo, oVoucher.Narration, oVoucher.ReferenceNote, oVoucher.VoucherDate, oVoucher.AuthorizedBy, oVoucher.LastUpdatedDate, (int)oVoucher.OperationType, oVoucher.BatchID, oVoucher.TaxEffectInt, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Voucher oVoucher, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Voucher]"
                                    + "%n, %n, %n, %s, %s, %s, %d, %n, %d,  %n, %n, %n, %n, %n",
                                    oVoucher.VoucherID, oVoucher.BUID, oVoucher.VoucherTypeID, oVoucher.VoucherNo, oVoucher.Narration, oVoucher.ReferenceNote, oVoucher.VoucherDate, oVoucher.AuthorizedBy, oVoucher.LastUpdatedDate, (int)oVoucher.OperationType, oVoucher.BatchID, oVoucher.TaxEffectInt, nUserId, (int)eEnumDBOperation);
        }
        public static void UpdateReconcilationDate(TransactionContext tc, int nVoucherID, DateTime dReconcilationDate)
        {
            tc.ExecuteNonQuery("UPDATE Voucher SET VoucherDate=%d WHERE VoucherID=%n", dReconcilationDate, nVoucherID);
        }
        public static void UnApprovedVoucher(TransactionContext tc, Int64 nVoucherID)
        {
            tc.ExecuteNonQuery("EXEC [SP_Un_Approved_Voucher]" + "%n", nVoucherID);
        }

        public static void CounterVoucher(TransactionContext tc, long nVoucherID, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_CounterVoucher]" + "%n, %n", nVoucherID, nUserId);
        }

        public static void CounterVoucherDelete(TransactionContext tc, long nVoucherID, int nCounterVoucherID)
        {
            tc.ExecuteNonQuery("EXEC [SP_CounterVoucherDelete]" + "%n, %n", nVoucherID, nCounterVoucherID);
        }

        

        public static void UpdatePrintCount(TransactionContext tc, Voucher oVoucher, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE Voucher SET  PrintCount=ISNULL(PrintCount,0)+1 where VoucherID=%n", oVoucher.VoucherID);
        }
        public static IDataReader ApprovedVoucher(TransactionContext tc, Voucher oVoucher, bool bIsAutoProcess)
        {
            return tc.ExecuteReader("EXEC [SP_ApprovedVoucher]" + "%s, %n, %b", oVoucher.Narration, oVoucher.AuthorizedBy, bIsAutoProcess);
        }        
        public static void CounterVoucherApproved(TransactionContext tc, int nUserID, int nCounterVoucherID)
        {
            tc.ExecuteNonQuery("UPDATE Voucher  SET AuthorizedBy=%n WHERE VoucherID=%n", nUserID, nCounterVoucherID);
        }
        public static IDataReader CommitVoucherNo(TransactionContext tc, int nBUID, int nVoucherTypeID, DateTime dVoucherDate)
        {
            return tc.ExecuteReader("EXEC [SP_VoucherNo]" + "%n, %n, %d", nBUID, nVoucherTypeID, dVoucherDate);
        }
        public static IDataReader CommitProfitLossAccounts(TransactionContext tc, int nBUID, int nSessionID, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_CommitProfitLossAccount]" + "%n, %n, %n", nBUID, nSessionID, nUserId);
        }
        public static IDataReader CommitProfitLossAppropriationAccounts(TransactionContext tc, Voucher oVoucher, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_CommitProfitLossAppropriationAccount]" + "%n, %n, %n, %s, %s, %n", oVoucher.BUID, oVoucher.AccountingSessionID, oVoucher.VoucherID, oVoucher.ProfitLossAppropriationAccountsInString, oVoucher.Narration, nUserId);
        }        
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nId)
        {
            return tc.ExecuteReader("SELECT * FROM View_Voucher WHERE VoucherID=%n", nId);
        }
        public static IDataReader LastNarration(TransactionContext tc, long nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Voucher WHERE VoucherID=(SELECT MAX(TT.VoucherID) FROM Voucher AS TT WHERE DBUserID=%n)", nUserID);
        }
        public static IDataReader GetsByBatch(TransactionContext tc, int nVoucherBatchID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Voucher WHERE BatchID=%n ORDER BY VoucherID ASC", nVoucherBatchID);
        }
        public static IDataReader GetsByBatchForApprove(TransactionContext tc, int nVoucherBatchID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Voucher AS V WHERE V.BatchID=%n AND V.AuthorizedBy=0 ORDER BY VoucherID  ASC", nVoucherBatchID);
        }
        public static IDataReader GetProfitLossAppropriationAccountVoucher(TransactionContext tc, int nBUID, DateTime dStartDate, DateTime dEndDate)
        {
            return tc.ExecuteReader("SELECT * FROM View_Voucher WHERE BUID=%n AND OperationType=3 AND CONVERT(DATE,CONVERT(VARCHAR(12),VoucherDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),%d,106))", nBUID, dStartDate, dEndDate);
        }

        public static IDataReader GetMaxDate(TransactionContext tc, int nVType, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Voucher WHERE View_Voucher.VoucherID= (SELECT MAX(MM.VoucherID) FROM Voucher  AS MM WHERE MM.VoucherTypeID=%n AND MM.OperationType=1 AND MM.BUID=%n)", nVType, nBUID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Voucher WHERE OperationType=1 and VoucherDate>DATEADD(day,-1,GETDATE()) and VoucherDate<=GETDATE()  order by VoucherDate");
        }

        public static IDataReader GetsWaitForApproval(TransactionContext tc, int nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Voucher WHERE OperationType=1 AND AuthorizedBy=0 AND  BatchID IN ( SELECT TT.VoucherBatchID FROM View_VoucherBatch AS TT WHERE TT.BatchStatus NOT IN (0,1,2)) order by VoucherDate");
        }

        public static IDataReader GetsByVoucherType(TransactionContext tc, int nVoucherTypeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Voucher WHERE OperationType=1 AND  VoucherTypeID=%n order by VoucherDate", nVoucherTypeID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion

        #region Common Function
        public static void CheckVoucherReference(TransactionContext tc, string TableName, string PKColumnName, int PKValue)
        {
            tc.ExecuteNonQuery("EXEC [SP_HasVoucherReference]" + "%s, %s, %n", TableName, PKColumnName, (int)PKValue);
        }
        #endregion
    }  
}
