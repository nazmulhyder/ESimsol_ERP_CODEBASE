using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportPaymentRequestDA
    {
        public ImportPaymentRequestDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportPaymentRequest oImportPaymentRequest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPaymentRequest]" + "%n, %s, %n, %d, %n, %s, %n, %n, %n,%n,%n, %n",
                                    oImportPaymentRequest.ImportPaymentRequestID, oImportPaymentRequest.RefNo, oImportPaymentRequest.BankAccountID, oImportPaymentRequest.LetterIssueDate, (int)oImportPaymentRequest.LiabilityType, oImportPaymentRequest.Note, oImportPaymentRequest.CRate, oImportPaymentRequest.CurrencyType, oImportPaymentRequest.Paymentthrough, oImportPaymentRequest.BUID, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ImportPaymentRequest oImportPaymentRequest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPaymentRequest]" + "%n, %s, %n, %d, %n, %s, %n, %n, %n,%n,%n, %n",
                                    oImportPaymentRequest.ImportPaymentRequestID, oImportPaymentRequest.RefNo, oImportPaymentRequest.BankAccountID, oImportPaymentRequest.LetterIssueDate, (int)oImportPaymentRequest.LiabilityType, oImportPaymentRequest.Note, oImportPaymentRequest.CRate, oImportPaymentRequest.CurrencyType, oImportPaymentRequest.Paymentthrough, oImportPaymentRequest.BUID, nUserID, (int)eEnumDBOperation);
        }
       
        public static IDataReader Approved(TransactionContext tc, ImportPaymentRequest oImportPaymentRequest, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Approved_ImportPaymentRequest]" + "%n,  %n", oImportPaymentRequest.ImportPaymentRequestID, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPaymentRequest WHERE ImportPaymentRequestID=%n", nID);
        }
        public static IDataReader GetByPInvoice(TransactionContext tc, long nPInvoiceID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPaymentRequest WHERE ImportPaymentRequestID in (Select ImportPaymentRequestID from ImportPaymentRequestDetail where ImportInvoiceID=%n)", nPInvoiceID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPaymentRequest ORDER BY RefNo");
        }
        public static IDataReader WaitForApproval(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPaymentRequest WHERE ISNULL(RequestBy,0)!=0 AND ISNULL(ApprovedBy,0)=0 and BUID=%n  ORDER BY RefNo", nBUID);
        }        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
