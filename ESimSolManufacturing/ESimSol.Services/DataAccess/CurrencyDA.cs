using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class CurrencyDA
    {
        public CurrencyDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Currency oCurrency, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Currency]"
                                    + "%n, %s, %s, %s, %s, %n, %s, %n, %n",
                                    oCurrency.CurrencyID, oCurrency.CurrencyName, oCurrency.IssueFigure, oCurrency.Symbol, oCurrency.SmallerUnit, oCurrency.SmUnitValue, oCurrency.Note, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Currency oCurrency, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Currency]"
                                    + "%n, %s, %s, %s, %s, %n, %s, %n, %n",
                                    oCurrency.CurrencyID, oCurrency.CurrencyName, oCurrency.IssueFigure, oCurrency.Symbol, oCurrency.SmallerUnit, oCurrency.SmUnitValue, oCurrency.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM Currency WHERE CurrencyID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Currency");
        }
        public static IDataReader Gets(TransactionContext tc, string sql)
        {
            return tc.ExecuteReader(sql);
        }
        public static IDataReader GetsLeftSelectedCurrency(TransactionContext tc, int nCurrencyID)
        {
            return tc.ExecuteReader("SELECT * FROM Currency WHERE CurrencyID!=%n", nCurrencyID);
        }
        public static IDataReader GetsLeftSelectedCurrency(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
