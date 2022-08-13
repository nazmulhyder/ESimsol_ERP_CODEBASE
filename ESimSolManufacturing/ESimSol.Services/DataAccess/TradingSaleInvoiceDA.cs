using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class TradingSaleInvoiceDA
    {
        public TradingSaleInvoiceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TradingSaleInvoice oTradingSaleInvoice, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TradingSaleInvoice]" + "%n, %n, %n, %s, %d, %n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %n, %n",
                                         oTradingSaleInvoice.TradingSaleInvoiceID, oTradingSaleInvoice.BUID, oTradingSaleInvoice.SalesTypeInt, oTradingSaleInvoice.InvoiceNo, oTradingSaleInvoice.InvoiceDate, oTradingSaleInvoice.BuyerID, oTradingSaleInvoice.ContactPersonID, oTradingSaleInvoice.CurrencyID, oTradingSaleInvoice.Note, oTradingSaleInvoice.ApprovedBy, oTradingSaleInvoice.GrossAmount, oTradingSaleInvoice.DiscountAmount, oTradingSaleInvoice.VatAmount, oTradingSaleInvoice.ServiceCharge, oTradingSaleInvoice.CommissionAmount, oTradingSaleInvoice.NetAmount, oTradingSaleInvoice.CardNo, oTradingSaleInvoice.CardPaid, oTradingSaleInvoice.CashPaid, oTradingSaleInvoice.ATMCardID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, TradingSaleInvoice oTradingSaleInvoice, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TradingSaleInvoice]" + "%n, %n, %n, %s, %d, %n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %n, %n",
                                         oTradingSaleInvoice.TradingSaleInvoiceID, oTradingSaleInvoice.BUID, oTradingSaleInvoice.SalesTypeInt, oTradingSaleInvoice.InvoiceNo, oTradingSaleInvoice.InvoiceDate, oTradingSaleInvoice.BuyerID, oTradingSaleInvoice.ContactPersonID, oTradingSaleInvoice.CurrencyID, oTradingSaleInvoice.Note, oTradingSaleInvoice.ApprovedBy, oTradingSaleInvoice.GrossAmount, oTradingSaleInvoice.DiscountAmount, oTradingSaleInvoice.VatAmount, oTradingSaleInvoice.ServiceCharge, oTradingSaleInvoice.CommissionAmount, oTradingSaleInvoice.NetAmount, oTradingSaleInvoice.CardNo, oTradingSaleInvoice.CardPaid, oTradingSaleInvoice.CashPaid, oTradingSaleInvoice.ATMCardID, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader AcceptRevise(TransactionContext tc, TradingSaleInvoice oTradingSaleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptTradingSaleInvoiceRevise]"
                                   + "%n, %n, %n, %s, %d, %n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %n, %n",
                                         oTradingSaleInvoice.TradingSaleInvoiceID, oTradingSaleInvoice.BUID, oTradingSaleInvoice.SalesTypeInt, oTradingSaleInvoice.InvoiceNo, oTradingSaleInvoice.InvoiceDate, oTradingSaleInvoice.BuyerID, oTradingSaleInvoice.ContactPersonID, oTradingSaleInvoice.CurrencyID, oTradingSaleInvoice.Note, oTradingSaleInvoice.ApprovedBy, oTradingSaleInvoice.GrossAmount, oTradingSaleInvoice.DiscountAmount, oTradingSaleInvoice.VatAmount, oTradingSaleInvoice.ServiceCharge, oTradingSaleInvoice.CommissionAmount, oTradingSaleInvoice.NetAmount, oTradingSaleInvoice.CardNo, oTradingSaleInvoice.CardPaid, oTradingSaleInvoice.CashPaid, oTradingSaleInvoice.ATMCardID, nUserID, (int)eEnumDBOperation);
        }
        public static void Approved(TransactionContext tc, TradingSaleInvoice oTradingSaleInvoice, int nUserID)
        {
            tc.ExecuteNonQuery("UPDATE TradingSaleInvoice SET ApprovedBy =%n,TradingSaleInvoiceStatus=%n WHERE TradingSaleInvoiceID =%n", nUserID, (int)EnumTradingSaleInvoiceStatus.Approved,oTradingSaleInvoice.TradingSaleInvoiceID);
        }

        public static void UndoApproved(TransactionContext tc, TradingSaleInvoice oTradingSaleInvoice, int nUserID)
        {
            tc.ExecuteNonQuery("UPDATE TradingSaleInvoice SET ApprovedBy =0,TradingSaleInvoiceStatus=0 WHERE TradingSaleInvoiceID =%n", oTradingSaleInvoice.TradingSaleInvoiceID);
        }
        public static IDataReader ApprovedCashSale(TransactionContext tc, int nTradingSaleInvoiceID, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ApprovedTradingSaleInvoice]" + "%n, %n", nTradingSaleInvoiceID, nUserID);
        }
        #endregion
        public static IDataReader RequestRevise(TransactionContext tc, TradingSaleInvoice oTradingSaleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_TradingSaleInvoiceOperation]"
                                    + "%n,%n,%n,%s,%n,%n",
                                    0, oTradingSaleInvoice.TradingSaleInvoiceID, (int)oTradingSaleInvoice.TradingSaleInvoiceStatus, oTradingSaleInvoice.Note, nUserID, (int)eEnumDBOperation);
        }
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleInvoice WHERE TradingSaleInvoiceID=%n", nID);
        }
        public static IDataReader GetLog(TransactionContext tc, int nLogID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleInvoiceLog WHERE TradingSaleInvoiceLogID=%n", nLogID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleInvoice");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsInitialInvoices(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleInvoice WHERE BUID = %n AND ISNULL(ApprovedBy,0)=0 ORDER BY TradingSaleInvoiceID ASC", nBUID);
        }
        #endregion
    }
}
