using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ReceivedChequeDA
    {
        public ReceivedChequeDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ReceivedCheque oReceivedCheque, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ReceivedCheque]"
                                    + "%n, %n, %d, %n, %s, %n, %d, %n, %s, %s, %s, %n, %n, %n, %s, %s, %d, %s, %s, %n, %n",
                                    oReceivedCheque.ReceivedChequeID, oReceivedCheque.ContractorID, oReceivedCheque.IssueDate, (int)oReceivedCheque.ChequeStatus, oReceivedCheque.ChequeNo, (int)oReceivedCheque.TransactionType, oReceivedCheque.ChequeDate, oReceivedCheque.Amount, oReceivedCheque.BankName, oReceivedCheque.BranchName, oReceivedCheque.AccountNo, oReceivedCheque.ReceivedAccontID, oReceivedCheque.SubLedgerID, oReceivedCheque.AuthorizedBy, oReceivedCheque.Remarks, oReceivedCheque.MoneyReceiptNo, oReceivedCheque.MoneyReceiptDate, oReceivedCheque.BillNumber, oReceivedCheque.ProductDetails, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ReceivedCheque oReceivedCheque, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ReceivedCheque]"
                                    + "%n, %n, %d, %n, %s, %n, %d, %n, %s, %s, %s, %n, %n, %n, %s, %s, %d, %s, %s, %n, %n",
                                    oReceivedCheque.ReceivedChequeID, oReceivedCheque.ContractorID, oReceivedCheque.IssueDate, (int)oReceivedCheque.ChequeStatus, oReceivedCheque.ChequeNo, (int)oReceivedCheque.TransactionType, oReceivedCheque.ChequeDate, oReceivedCheque.Amount, oReceivedCheque.BankName, oReceivedCheque.BranchName, oReceivedCheque.AccountNo, oReceivedCheque.ReceivedAccontID, oReceivedCheque.SubLedgerID, oReceivedCheque.AuthorizedBy, oReceivedCheque.Remarks, oReceivedCheque.MoneyReceiptNo, oReceivedCheque.MoneyReceiptDate, oReceivedCheque.BillNumber, oReceivedCheque.ProductDetails, (int)eEnumDBOperation, nUserID);
        }

        public static IDataReader UpdateReceivedChequeStatus(TransactionContext tc, ReceivedCheque oReceivedCheque, ReceivedChequeHistory oReceivedChequeHistory, int nCurrentUserID)
        {
            return tc.ExecuteReader("EXEC [SP_UpdateReceivedChequeStatus] "
                                    + "%n, %n, %n, %s, %s, %n",
                                    oReceivedChequeHistory.ReceivedChequeID,
                                    (int)oReceivedChequeHistory.PreviousStatus,
                                    (int)oReceivedChequeHistory.CurrentStatus,
                                    oReceivedChequeHistory.Note,
                                    oReceivedChequeHistory.ChangeLog,
                                    nCurrentUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ReceivedCheque WHERE ReceivedChequeID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ReceivedCheque");
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      

       

       
        #endregion
    }  
}
