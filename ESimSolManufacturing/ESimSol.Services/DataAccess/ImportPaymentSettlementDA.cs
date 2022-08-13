using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ImportPaymentSettlementDA
    {
        public ImportPaymentSettlementDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImportPaymentSettlement oImportPaymentSettlement, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sImportPaymentSettlementIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPaymentSettlement]" + "%n, %n, %n, %n, %n, %s",
                                    oImportPaymentSettlement.PIPRDetailID, oImportPaymentSettlement.ImportPaymentRequestID, oImportPaymentSettlement.ImportInvoiceID, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ImportPaymentSettlement oImportPaymentSettlement, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sImportPaymentSettlementIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPaymentSettlement]" + "%n, %n, %n, %n, %n, %s",
                                    oImportPaymentSettlement.PIPRDetailID, oImportPaymentSettlement.ImportPaymentRequestID, oImportPaymentSettlement.ImportInvoiceID, nUserID, (int)eEnumDBOperation, sImportPaymentSettlementIDs);
        }
    
      
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPaymentSettlement WHERE ImportPaymentSettlementID=%n", nID);
        }
    
        public static IDataReader Gets(int nbuid, int nBankStatus, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPaymentSettlement where ISNULL(ApprovedBy,0)!=0 AND  BUID=%n and BankStatus=%n", nbuid, nBankStatus); //where BankStatus =%n
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
