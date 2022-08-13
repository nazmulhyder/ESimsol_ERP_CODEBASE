using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class InvoiceLotDA
    {
        public InvoiceLotDA() { }

        #region Insert, Update, Delete
        public static IDataReader InsertUpdate(TransactionContext tc, InvoiceLot oInvoiceLot, EnumDBOperation eEnumDBInvoiceLot, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_InvoiceLot]"
                                    + "%n, %n, %s, %s, %s, %s, %s, %n, %n, %s",
                                     oInvoiceLot.InvoiceLotID,
                                     oInvoiceLot.PurchaseInvoiceDetailID,
                                     oInvoiceLot.SerialNo,
                                     oInvoiceLot.EngineNo,
                                     oInvoiceLot.AlternatorNo,
                                     oInvoiceLot.ModuleNo,
                                     oInvoiceLot.Others,                                     
                                     nUserId,
                                     (int)eEnumDBInvoiceLot,
                                     ""
                                     );
        }

        public static void Delete(TransactionContext tc, InvoiceLot oInvoiceLot, EnumDBOperation eEnumDBInvoiceLot, Int64 nUserId, string sInvoiceDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_InvoiceLot]"
                                    + "%n, %n, %s, %s, %s, %s, %s, %n, %n, %s",
                                     oInvoiceLot.InvoiceLotID,
                                     oInvoiceLot.PurchaseInvoiceDetailID,
                                     oInvoiceLot.SerialNo,
                                     oInvoiceLot.EngineNo,
                                     oInvoiceLot.AlternatorNo,
                                     oInvoiceLot.ModuleNo,
                                     oInvoiceLot.Others,
                                     nUserId,
                                     (int)eEnumDBInvoiceLot,
                                     sInvoiceDetailIDs
                                     );
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_InvoiceLot WHERE InvoiceLotID=%n", nID);
        }
        public static IDataReader Gets(int nInvoiceDetailID, TransactionContext tc)
        {
            return tc.ExecuteReader("select * from View_InvoiceLot where PurchaseInvoiceDetailID=%n", nInvoiceDetailID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByInvoice(TransactionContext tc, int nPurchaseInvoiceId)
        {
            return tc.ExecuteReader("SELECT * FROM View_InvoiceLot WHERE PurchaseInvoiceDetailID IN (SELECT ImportInvoiceDetailID FROM ImportInvoiceDetail WHERE ImportInvoiceID=%n)", nPurchaseInvoiceId);
        }
        #endregion
    }
}
