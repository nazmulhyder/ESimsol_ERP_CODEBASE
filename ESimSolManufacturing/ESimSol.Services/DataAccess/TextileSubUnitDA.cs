using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class TextileSubUnitDA
    {
        public TextileSubUnitDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, TextileSubUnit oTextileSubUnit, int eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_TextileSubUnit]" + "%n, %n, %s, %s, %n, %n", 
                oTextileSubUnit.TSUID,
                oTextileSubUnit.TextileUnit,
                oTextileSubUnit.Name,
                oTextileSubUnit.Note,
                nUserId,
                (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, TextileSubUnit oTextileSubUnit, int eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_TextileSubUnit]" + "%n, %n, %s, %s, %n, %n",
                oTextileSubUnit.TSUID,
                oTextileSubUnit.TextileUnit,
                oTextileSubUnit.Name,
                oTextileSubUnit.Note,
                nUserId,
                (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_TextileSubUnit WHERE TSUID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_TextileSubUnit");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
