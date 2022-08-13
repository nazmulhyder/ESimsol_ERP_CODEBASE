using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class ProformaInvoiceHistoryDA
    {
        public ProformaInvoiceHistoryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProformaInvoiceHistory oProformaInvoiceHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sProformaInvoiceHistoryIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProformaInvoiceHistory]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s", oProformaInvoiceHistory.ProformaInvoiceHistoryID, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ProformaInvoiceHistory oProformaInvoiceHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sProformaInvoiceHistoryIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProformaInvoiceHistory]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s", oProformaInvoiceHistory.ProformaInvoiceHistoryID, nUserID, (int)eEnumDBOperation, sProformaInvoiceHistoryIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceHistory WHERE ProformaInvoiceHistoryID=%n", nID);
        }

        public static IDataReader Gets(int ProformaInvoicID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProformaInvoiceHistory where ProformaInvoiceID =%n", ProformaInvoicID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
}
