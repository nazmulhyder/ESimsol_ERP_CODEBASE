using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LightSourceDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, LightSource oLightSource, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LightSource]"
                                    + "%n, %s, %n, %n",
                                    oLightSource.LightSourceID, oLightSource.Descriptions, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, LightSource oLightSource, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LightSource]"
                                    + "%n, %s, %n, %n",
                                    oLightSource.LightSourceID, oLightSource.Descriptions, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM LightSource order by [Descriptions]");
        }
        public static IDataReader GetsActive(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM LightSource order by [Descriptions]");
        }
        public static IDataReader Get(TransactionContext tc, long nLightSourceID)
        {
            return tc.ExecuteReader("SELECT * FROM LightSource WHERE LightSourceID=%n", nLightSourceID);
        }
        #endregion
    }
}
