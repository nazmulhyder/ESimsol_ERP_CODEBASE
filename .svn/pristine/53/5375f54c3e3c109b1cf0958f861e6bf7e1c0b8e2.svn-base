using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class TradingSaleReturnDA
    {
        public TradingSaleReturnDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TradingSaleReturn oTradingSaleReturn, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TradingSaleReturn]" + "%n, %n, %s, %d, %n, %n, %n, %s, %n, %n, %n, %n, %n, %n",
                                         oTradingSaleReturn.TradingSaleReturnID, oTradingSaleReturn.BUID, oTradingSaleReturn.ReturnNo, oTradingSaleReturn.ReturnDate, oTradingSaleReturn.BuyerID, oTradingSaleReturn.ContactPersonID, oTradingSaleReturn.CurrencyID, oTradingSaleReturn.Note, oTradingSaleReturn.ApprovedBy, oTradingSaleReturn.GrossAmount, oTradingSaleReturn.PaymentAmount, oTradingSaleReturn.StoreID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, TradingSaleReturn oTradingSaleReturn, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TradingSaleReturn]" + "%n, %n, %s, %d, %n, %n, %n, %s, %n, %n, %n, %n, %n, %n",
                                         oTradingSaleReturn.TradingSaleReturnID, oTradingSaleReturn.BUID, oTradingSaleReturn.ReturnNo, oTradingSaleReturn.ReturnDate, oTradingSaleReturn.BuyerID, oTradingSaleReturn.ContactPersonID, oTradingSaleReturn.CurrencyID, oTradingSaleReturn.Note, oTradingSaleReturn.ApprovedBy, oTradingSaleReturn.GrossAmount, oTradingSaleReturn.PaymentAmount, oTradingSaleReturn.StoreID, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader Approved(TransactionContext tc, int nTradingSaleReturnID, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ApproveTradingSaleReturn]" + "%n, %n", nTradingSaleReturnID, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleReturn WHERE TradingSaleReturnID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleReturn");
        }      

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsInitialReturns(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TradingSaleReturn WHERE BUID = %n AND ISNULL(ApprovedBy,0)=0 ORDER BY TradingSaleReturnID ASC", nBUID);
        }
        #endregion
    }
}

