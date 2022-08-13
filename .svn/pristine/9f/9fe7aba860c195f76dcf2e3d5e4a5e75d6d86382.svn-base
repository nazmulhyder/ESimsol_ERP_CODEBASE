using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;




namespace ESimSol.Services.DataAccess
{
    public class PurchaseInvoiceHistoryDA
    {
        public PurchaseInvoiceHistoryDA() { }

        #region Insert Function
        public static void Insert(TransactionContext tc, PurchaseInvoiceHistory oPurchaseInvoiceHistory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_PurchaseInvoiceHistory]" + "%n, %n, %n, %n, %s, %n, %n",
                                oPurchaseInvoiceHistory.PurchaseInvoiceHistoryID, oPurchaseInvoiceHistory.PurchaseInvoiceID, oPurchaseInvoiceHistory.CurrentStatusInt,  oPurchaseInvoiceHistory.PrevoiusStatusInt, oPurchaseInvoiceHistory.Note, nUserID, (int)eEnumDBOperation);
        }        
        #endregion

        

        #region Delete Function
        public static void Delete(TransactionContext tc, PurchaseInvoiceHistory oPurchaseInvoiceHistory)
        {
            tc.ExecuteNonQuery("DELETE FROM PurchaseInvoiceHistory WHERE PurchaseInvoiceHistoryID=%n", oPurchaseInvoiceHistory.PurchaseInvoiceHistoryID);
        }
        public static void DeleteByBill(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM PurchaseInvoiceHistory WHERE PurchaseInvoiceID=%n", nID);
        }
        #endregion
        #region Generation Function
     
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT *,(SELECT UserName FROM Users WHERE UserID=LH.TrgDBUserID) AS EmployeeName FROM PurchaseInvoiceHistory LH WHERE LH.PurchaseInvoiceHistoryID=%n", nID);
        }
        public static IDataReader Get(TransactionContext tc, int nInvoiceStatus, int nBankStatus, int nInvID)
        {
            return tc.ExecuteReader("SELECT *,(SELECT UserName FROM Users WHERE UserID=LH.DBUserID) AS UserName FROM PurchaseInvoiceHistory LH WHERE LH.InvoiceEvent=%n and LH.InvoiceBankStatus=%n and LH.PurchaseInvoiceID=%n", nInvoiceStatus, nBankStatus, nInvID);
        }
        public static IDataReader GetbyPurchaseInvoice(TransactionContext tc, int nPurchaseInvoiceID, int eEvent)
        {
            return tc.ExecuteReader("SELECT *,(SELECT UserName FROM Users WHERE UserID=LH.TrgDBUserID) AS EmployeeName FROM PurchaseInvoiceHistory LH WHERE LH.PurchaseInvoiceID=%n AND LH.BillEvent=%n", nPurchaseInvoiceID, eEvent);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT *,(SELECT UserName FROM Users WHERE UserID=LH.TrgDBUserID) AS EmployeeName FROM PurchaseInvoiceHistory LH ");
        }
        public static IDataReader Gets(TransactionContext tc, int nPurchaseInvoiceID)
        {
            return tc.ExecuteReader("SELECT *,(SELECT UserName FROM Users WHERE UserID=LH.TrgDBUserID) AS EmployeeName FROM PurchaseInvoiceHistory LH WHERE LH.PurchaseInvoiceID=%n", nPurchaseInvoiceID);
        }
        public static int GetBillHisEven(TransactionContext tc, int nLBillID, int eBillEvent)
        {
            object obj = tc.ExecuteScalar("SELECT isnull(BillEvent,0) FROM PurchaseInvoiceHistory WHERE PurchaseInvoiceID=%n and BillEvent=%n", nLBillID, eBillEvent);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }

        public static IDataReader Get(TransactionContext tc, int nPurchaseInvoiceID, int nEvent)
        {
            return tc.ExecuteReader("SELECT * FROM PurchaseInvoiceHistory WHERE PurchaseInvoiceID=%n AND BillEvent=%n", nPurchaseInvoiceID, nEvent);
        }
      
        #endregion
    }
}
