using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class TradingSaleInvoiceDetailDA
    {
        public TradingSaleInvoiceDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TradingSaleInvoiceDetail oTradingSaleInvoiceDetail, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TradingSaleInvoiceDetail]" + "%n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                                         oTradingSaleInvoiceDetail.TradingSaleInvoiceDetailID, oTradingSaleInvoiceDetail.TradingSaleInvoiceID, oTradingSaleInvoiceDetail.ProductID, oTradingSaleInvoiceDetail.ItemDescription, oTradingSaleInvoiceDetail.MeasurementUnitID, oTradingSaleInvoiceDetail.InvoiceQty, oTradingSaleInvoiceDetail.UnitPrice, oTradingSaleInvoiceDetail.Amount, oTradingSaleInvoiceDetail.Discount, oTradingSaleInvoiceDetail.NetAmount, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, TradingSaleInvoiceDetail oTradingSaleInvoiceDetail, EnumDBOperation eEnumDBOperation, int nUserID, string sTradingSaleInvoiceDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TradingSaleInvoiceDetail]" + "%n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %n, %n, %s",
                                         oTradingSaleInvoiceDetail.TradingSaleInvoiceDetailID, oTradingSaleInvoiceDetail.TradingSaleInvoiceID, oTradingSaleInvoiceDetail.ProductID, oTradingSaleInvoiceDetail.ItemDescription, oTradingSaleInvoiceDetail.MeasurementUnitID, oTradingSaleInvoiceDetail.InvoiceQty, oTradingSaleInvoiceDetail.UnitPrice, oTradingSaleInvoiceDetail.Amount, oTradingSaleInvoiceDetail.Discount, oTradingSaleInvoiceDetail.NetAmount, nUserID, (int)eEnumDBOperation, sTradingSaleInvoiceDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleInvoiceDetail WHERE TradingSaleInvoiceDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleInvoiceDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nTradingSaleInvoiceID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleInvoiceDetail WHERE TradingSaleInvoiceID=%n", nTradingSaleInvoiceID);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nTradingSaleInvoiceLogID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleInvoiceDetailLog WHERE TradingSaleInvoiceLogID=%n", nTradingSaleInvoiceLogID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
