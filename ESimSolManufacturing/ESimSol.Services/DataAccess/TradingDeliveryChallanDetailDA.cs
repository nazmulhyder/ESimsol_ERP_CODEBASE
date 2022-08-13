using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class TradingDeliveryChallanDetailDA
    {
        public TradingDeliveryChallanDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TradingDeliveryChallanDetail oTradingDeliveryChallanDetail, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TradingDeliveryChallanDetail]" + "%n, %n, %n, %n, %s, %n,%n, %n, %n, %n, %n, %n, %s",
                                         oTradingDeliveryChallanDetail.TradingDeliveryChallanDetailID, oTradingDeliveryChallanDetail.TradingDeliveryChallanID, oTradingDeliveryChallanDetail.TradingSaleInvoiceDetailID, oTradingDeliveryChallanDetail.ProductID, oTradingDeliveryChallanDetail.ItemDescription, oTradingDeliveryChallanDetail.UnitID,oTradingDeliveryChallanDetail.LotID, oTradingDeliveryChallanDetail.ChallanQty, oTradingDeliveryChallanDetail.UnitPrice, oTradingDeliveryChallanDetail.Amount, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, TradingDeliveryChallanDetail oTradingDeliveryChallanDetail, EnumDBOperation eEnumDBOperation, int nUserID, string sTradingDeliveryChallanDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TradingDeliveryChallanDetail]" + "%n, %n, %n, %n, %s, %n,%n, %n, %n, %n, %n, %n, %s",
                                         oTradingDeliveryChallanDetail.TradingDeliveryChallanDetailID, oTradingDeliveryChallanDetail.TradingDeliveryChallanID, oTradingDeliveryChallanDetail.TradingSaleInvoiceDetailID, oTradingDeliveryChallanDetail.ProductID, oTradingDeliveryChallanDetail.ItemDescription, oTradingDeliveryChallanDetail.UnitID, oTradingDeliveryChallanDetail.LotID, oTradingDeliveryChallanDetail.ChallanQty, oTradingDeliveryChallanDetail.UnitPrice, oTradingDeliveryChallanDetail.Amount, nUserID, (int)eEnumDBOperation, sTradingDeliveryChallanDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingDeliveryChallanDetail WHERE TradingDeliveryChallanDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingDeliveryChallanDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nDeliveryChallanID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingDeliveryChallanDetail WHERE TradingDeliveryChallanID=%n", nDeliveryChallanID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
