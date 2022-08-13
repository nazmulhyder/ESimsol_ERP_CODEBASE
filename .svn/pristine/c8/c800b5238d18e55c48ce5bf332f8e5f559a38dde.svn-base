using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class CurrencyConversionDA
    {
        public CurrencyConversionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CurrencyConversion oCurrencyConversion, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CurrencyConversion]"
                                    + "%n, %n, %n, %n, %n, %n, %d, %b, %s, %n, %n",
                                    oCurrencyConversion.CurrencyConversionID, oCurrencyConversion.CurrencyID, oCurrencyConversion.ToCurrencyID, oCurrencyConversion.BankID, oCurrencyConversion.RateSale, oCurrencyConversion.RatePurchase, oCurrencyConversion.Date,oCurrencyConversion.IsOpen,oCurrencyConversion.Note, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, CurrencyConversion oCurrencyConversion, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CurrencyConversion]"
                                    + "%n, %n, %n, %n, %n, %n, %d, %b, %s, %n, %n",
                                    oCurrencyConversion.CurrencyConversionID, oCurrencyConversion.CurrencyID, oCurrencyConversion.ToCurrencyID, oCurrencyConversion.BankID, oCurrencyConversion.RateSale, oCurrencyConversion.RatePurchase, oCurrencyConversion.Date, oCurrencyConversion.IsOpen, oCurrencyConversion.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CurrencyConversion WHERE CurrencyConversionID=%n", nID);
        }

        public static IDataReader GetsLastConversionRate(TransactionContext tc, int nFromCurrencyID, int nToCurrencyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CurrencyConversion WHERE CurrencyID=%n AND ToCurrencyID=%n AND Date =(SELECT MAX(Date) FROM View_CurrencyConversion WHERE CurrencyID=%n AND ToCurrencyID=%n)", nFromCurrencyID, nToCurrencyID, nFromCurrencyID, nToCurrencyID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CurrencyConversion");
        }

        public static IDataReader GetsByFromCurrency(TransactionContext tc, int nCurrencyID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CurrencyConversion WHERE CurrencyID=%n", nCurrencyID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
