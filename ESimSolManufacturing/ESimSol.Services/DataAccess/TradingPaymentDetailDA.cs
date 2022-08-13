using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class TradingPaymentDetailDA
    {
        public TradingPaymentDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TradingPaymentDetail oTradingPaymentDetail, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TradingPaymentDetail]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                                         oTradingPaymentDetail.TradingPaymentDetailID, oTradingPaymentDetail.TradingPaymentID, oTradingPaymentDetail.ReferenceID, oTradingPaymentDetail.Amount, oTradingPaymentDetail.Remarks, nUserID, (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, TradingPaymentDetail oTradingPaymentDetail, EnumDBOperation eEnumDBOperation, int nUserID, string sTradingPaymentDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TradingPaymentDetail]" + "%n, %n, %n, %n, %s, %n, %n, %s",
                                         oTradingPaymentDetail.TradingPaymentDetailID, oTradingPaymentDetail.TradingPaymentID, oTradingPaymentDetail.ReferenceID, oTradingPaymentDetail.Amount, oTradingPaymentDetail.Remarks, nUserID, (int)eEnumDBOperation, sTradingPaymentDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingPaymentDetail WHERE TradingPaymentDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingPaymentDetail");
        }
        public static IDataReader Gets(TransactionContext tc, int nPaymentID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingPaymentDetail WHERE TradingPaymentID=%n", nPaymentID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}

