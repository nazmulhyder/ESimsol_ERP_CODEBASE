using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ImportPaymentRequestDetailDA
    {
        public ImportPaymentRequestDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportPaymentRequestDetail oImportPaymentRequestDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sImportPaymentRequestDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPaymentRequestDetail]" + "%n, %n, %n, %n, %n, %s",
                                    oImportPaymentRequestDetail.PIPRDetailID, oImportPaymentRequestDetail.PIPRID, oImportPaymentRequestDetail.ImportInvoiceID, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ImportPaymentRequestDetail oImportPaymentRequestDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sImportPaymentRequestDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPaymentRequestDetail]" + "%n, %n, %n, %n, %n, %s",
                                    oImportPaymentRequestDetail.PIPRDetailID, oImportPaymentRequestDetail.PIPRID, oImportPaymentRequestDetail.ImportInvoiceID, nUserID, (int)eEnumDBOperation, sImportPaymentRequestDetailIDs);
        }
    
      
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPaymentRequestDetail WHERE ImportPaymentRequestDetailID=%n", nID);
        }
        public static IDataReader GetBy(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPaymentRequestDetail WHERE PurchaseInvoiceLCID=%n", nID);
        }

        public static IDataReader Gets(int nImportPaymentRequestID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPaymentRequestDetail where ImportPaymentRequestID =%n", nImportPaymentRequestID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
