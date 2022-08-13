using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class CountryDA
    {
        public CountryDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Country oCountry, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_Country]"
                                    + "%n, %s, %s,  %s,  %s, %n, %n",
                                    oCountry.CountryID, oCountry.Code, oCountry.Name, oCountry.ShortName, oCountry.Note, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, Country oCountry, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_Country]"
                              + "%n, %s, %s,  %s,  %s, %n, %n",
                                    oCountry.CountryID, oCountry.Code, oCountry.Name, oCountry.ShortName, oCountry.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM Country WHERE CountryID=%n", nID);
        }

        public static IDataReader GetByType(TransactionContext tc, int nInOutType)
        {
            return tc.ExecuteReader("SELECT * FROM Country ");
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Country Order BY Name");
        }
        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM Country Order BY Name");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}