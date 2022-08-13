using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;



namespace ESimSol.Services.DataAccess
{
    public class ImportInvoiceHistoryDA
    {
        public ImportInvoiceHistoryDA() { }

        #region Insert Function
        public static void Insert(TransactionContext tc, ImportInvoiceHistory oLCBH, Int64 nUserId)
        {
            tc.ExecuteNonQuery("INSERT INTO ImportInvoiceHistory(ImportInvoiceHistoryID, PurchaseInvoiceID, BillEvent, BillEvent_Prevoius, Note, DBServerDateTime, DBUserID)"
                + " VALUES(%n, %n, %n, %n, %s, %s, %q, %n)",
                oLCBH.ImportInvoiceHistoryID, oLCBH.ImportInvoiceID, (int)oLCBH.InvoiceEvent, (int)oLCBH.BillEvent_Prevoius, oLCBH.Note, Global.DBDateTime, oLCBH.DBUserID);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, ImportInvoiceHistory oImportInvoiceHistory, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE ImportInvoiceHistory SET PurchaseInvoiceID=%n, BillEvent=%n, BillEvent_Prevoius=%n,  Note=%s, DBServerDateTime=%q, DBUserID=%n "
                + " WHERE ImportInvoiceHistoryID=%n", oImportInvoiceHistory.ImportInvoiceID, (int)oImportInvoiceHistory.InvoiceEvent, (int)oImportInvoiceHistory.BillEvent_Prevoius, oImportInvoiceHistory.Note, Global.DBDateTime, oImportInvoiceHistory.DBUserID, oImportInvoiceHistory.ImportInvoiceHistoryID);
        }
        public static void UpdateBillState(TransactionContext tc, ImportInvoiceHistory oImportInvoiceHistory, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE ImportInvoiceHistory SET BillEvent=%n, BillEvent_Prevoius=%n,  Note=%s, DBServerDateTime=%q, DBUserID=%n "
                + " WHERE BillEvent=%n and PurchaseInvoiceID=%n", (int)oImportInvoiceHistory.InvoiceEvent, (int)oImportInvoiceHistory.BillEvent_Prevoius, oImportInvoiceHistory.Note, Global.DBDateTime, oImportInvoiceHistory.DBUserID, (int)oImportInvoiceHistory.InvoiceEvent, oImportInvoiceHistory.ImportInvoiceID);
        }
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, ImportInvoiceHistory oImportInvoiceHistory)
        {
            tc.ExecuteNonQuery("DELETE FROM ImportInvoiceHistory WHERE ImportInvoiceHistoryID=%n", oImportInvoiceHistory.ImportInvoiceHistoryID);
        }
        public static void DeleteByBill(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM ImportInvoiceHistory WHERE PurchaseInvoiceID=%n", nID);
        }
        #endregion
        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ImportInvoiceHistory", "ImportInvoiceHistoryID");
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoiceHistory as LH WHERE LH.ImportInvoiceHistoryID=%n", nID);
        }
        public static IDataReader Get(TransactionContext tc, int nInvoiceStatus, int nBankStatus, int nInvID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoiceHistory as LH WHERE LH.InvoiceEvent=%n and LH.InvoiceBankStatus=%n and LH.ImportInvoiceID=%n", nInvoiceStatus, nBankStatus, nInvID);
        }
        public static IDataReader GetbyPurchaseInvoice(TransactionContext tc, int nPurchaseInvoiceID, int eEvent)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoiceHistory  ImportInvoiceID=%n AND BillEvent=%n", nPurchaseInvoiceID, eEvent);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT *,(SELECT UserName FROM Users WHERE UserID=LH.TrgDBUserID) AS EmployeeName FROM ImportInvoiceHistory LH ");
        }
        public static IDataReader Gets(TransactionContext tc, int nImportInvoiceID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoiceHistory WHERE ImportInvoiceID=%n", nImportInvoiceID);
        }


        public static IDataReader GetsByInvoiceEvent(TransactionContext tc, int nImportInvoiceID, string sInvoiceEvent)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportInvoiceHistory WHERE ImportInvoiceID=%n AND InvoiceEvent in (%q) and ImportInvoiceHistoryID in (Select Top(2)(ImportInvoiceHistoryID) from ImportInvoiceHistory where ImportInvoiceID=%n AND InvoiceEvent in (%q) order by ImportInvoiceHistoryID DESC)", nImportInvoiceID, sInvoiceEvent, nImportInvoiceID, sInvoiceEvent);
        }
     
        #endregion
    }
}
