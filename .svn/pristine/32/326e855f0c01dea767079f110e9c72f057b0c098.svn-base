using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class TradingSaleReturnDetailDA
    {
        public TradingSaleReturnDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TradingSaleReturnDetail oTradingSaleReturnDetail, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TradingSaleReturnDetail]" + "%n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %s",
                                         oTradingSaleReturnDetail.TradingSaleReturnDetailID, oTradingSaleReturnDetail.TradingSaleReturnID, oTradingSaleReturnDetail.ProductID, oTradingSaleReturnDetail.ItemDescription, oTradingSaleReturnDetail.MeasurementUnitID, oTradingSaleReturnDetail.ReturnQty, oTradingSaleReturnDetail.UnitPrice, oTradingSaleReturnDetail.Amount, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, TradingSaleReturnDetail oTradingSaleReturnDetail, EnumDBOperation eEnumDBOperation, int nUserID, string sTradingSaleReturnDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TradingSaleReturnDetail]" + "%n, %n, %n, %s, %n, %n, %n, %n, %n, %n, %s",
                                         oTradingSaleReturnDetail.TradingSaleReturnDetailID, oTradingSaleReturnDetail.TradingSaleReturnID, oTradingSaleReturnDetail.ProductID, oTradingSaleReturnDetail.ItemDescription, oTradingSaleReturnDetail.MeasurementUnitID, oTradingSaleReturnDetail.ReturnQty, oTradingSaleReturnDetail.UnitPrice, oTradingSaleReturnDetail.Amount, nUserID, (int)eEnumDBOperation, sTradingSaleReturnDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleReturnDetail WHERE TradingSaleReturnDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleReturnDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nSaleReturnID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleReturnDetail WHERE SaleReturnID=%n", nSaleReturnID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

