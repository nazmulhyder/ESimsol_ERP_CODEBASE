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


    public class TradingSaleOrderDetailDA
    {
        public TradingSaleOrderDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TradingSaleOrderDetail oTradingSaleOrderDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TradingSaleOrderDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n,%n, %n, %n, %s",
                                    oTradingSaleOrderDetail.TradingSaleOrderDetailID, oTradingSaleOrderDetail.TradingSaleOrderID, oTradingSaleOrderDetail.ProductID, oTradingSaleOrderDetail.OrderQty, oTradingSaleOrderDetail.UnitPrice, oTradingSaleOrderDetail.MeasurementUnitID, oTradingSaleOrderDetail.VatInPercent, oTradingSaleOrderDetail.InvoiceQty, (int)oTradingSaleOrderDetail.ProductGrade, nUserId, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, TradingSaleOrderDetail oTradingSaleOrderDetail, EnumDBOperation eEnumDBOperation, Int64 nUserId, string sTradingSaleOrderDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TradingSaleOrderDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n,%n, %n, %n, %s",
                                    oTradingSaleOrderDetail.TradingSaleOrderDetailID, oTradingSaleOrderDetail.TradingSaleOrderID, oTradingSaleOrderDetail.ProductID, oTradingSaleOrderDetail.OrderQty, oTradingSaleOrderDetail.UnitPrice, oTradingSaleOrderDetail.MeasurementUnitID, oTradingSaleOrderDetail.VatInPercent, oTradingSaleOrderDetail.InvoiceQty, (int)oTradingSaleOrderDetail.ProductGrade, nUserId, (int)eEnumDBOperation, sTradingSaleOrderDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleOrderDetail WHERE TradingSaleOrderDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleOrderDetail");
        }

        public static IDataReader Gets(TransactionContext tc, long id)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleOrderDetail WHERE SaleorderID=" + id);
        }

        public static IDataReader GetsForInvoice(TransactionContext tc, long id)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleOrderDetail WHERE YetToInvoice!=0 AND SaleorderID=" + id);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
 
   
}

