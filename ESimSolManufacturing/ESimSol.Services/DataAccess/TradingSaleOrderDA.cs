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

    public class TradingSaleOrderDA
    {
        public TradingSaleOrderDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TradingSaleOrder oTradingSaleOrder, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TradingSaleOrder]" + "%n, %n, %n, %s, %n, %n, %n, %d, %d, %b, %s, %d, %n, %n",
                                    oTradingSaleOrder.TradingSaleOrderID, oTradingSaleOrder.BUID, oTradingSaleOrder.DemandRequsitionID, oTradingSaleOrder.TradingSaleOrderNo, (int)oTradingSaleOrder.OrderType, oTradingSaleOrder.ContractorID, oTradingSaleOrder.ContractorPersonalID, oTradingSaleOrder.OrderCreateDate, oTradingSaleOrder.RequestedDeliveryDate, oTradingSaleOrder.IsActive, oTradingSaleOrder.Note, oTradingSaleOrder.SaleValidatyDate, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, TradingSaleOrder oTradingSaleOrder, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TradingSaleOrder]" + "%n, %n, %n, %s, %n, %n, %n, %d, %d, %b, %s, %d, %n, %n",
                                    oTradingSaleOrder.TradingSaleOrderID, oTradingSaleOrder.BUID, oTradingSaleOrder.DemandRequsitionID, oTradingSaleOrder.TradingSaleOrderNo, (int)oTradingSaleOrder.OrderType, oTradingSaleOrder.ContractorID, oTradingSaleOrder.ContractorPersonalID, oTradingSaleOrder.OrderCreateDate, oTradingSaleOrder.RequestedDeliveryDate, oTradingSaleOrder.IsActive, oTradingSaleOrder.Note, oTradingSaleOrder.SaleValidatyDate, nUserId, (int)eEnumDBOperation);
        }

        public static IDataReader SaveInvoice(TransactionContext tc, SaleInvoice oSaleInvoice, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleOrder WHERE TradingSaleOrderID=%n", nUserId);
            //return tc.ExecuteReader("EXEC [SP_IUD_SaleInvoice]" + "%n, %n, %s, %n, %n, %n, %d, %d,%n, %n, %n,%n, %n, %n, %s,%n, %d, %s, %n, %s, %n, %n",
            //                        oSaleInvoice.SaleInvoiceID, oSaleInvoice.TradingSaleOrderID, oSaleInvoice.SaleInvoiceNo, (int)oSaleInvoice.InvoiceType, oSaleInvoice.ContractorID, oSaleInvoice.ContractorPersonalID, oSaleInvoice.SaleInvoiceDate, oSaleInvoice.DeliveryDate, oSaleInvoice.GrossAmount, oSaleInvoice.Discount, oSaleInvoice.TotalVat, oSaleInvoice.ServiceChargeAmount, oSaleInvoice.CommissionAmount, oSaleInvoice.NetAmount, oSaleInvoice.Note, (int)oSaleInvoice.PaymentMethod, oSaleInvoice.DuePaymentDate, oSaleInvoice.PreviousSummary, oSaleInvoice.ApprovedBy, oSaleInvoice.SaleInvoiceDetailInString, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleOrder WHERE TradingSaleOrderID=%n", nID);
        }
        public static IDataReader GetByTradingSaleOrderNo(TransactionContext tc, string sTradingSaleOrderNo)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleOrder WHERE TradingSaleOrderNo=%s", sTradingSaleOrderNo);
        }



        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleOrder");
        }

        public static IDataReader GetMaxOrderNo(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleOrder WHERE TradingSaleOrderID=(SELECT MAX(TradingSaleOrderID) FROM TradingSaleOrder)");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
