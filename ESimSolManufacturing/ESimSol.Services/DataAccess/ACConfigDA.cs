using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ACConfigDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ACConfig oACConfig, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ACConfig]"
                                    + "%n, %n, %n, %s, %n, %n",
                                    oACConfig.ACConfigID, oACConfig.ConfigureType, oACConfig.ConfigureValueType, oACConfig.ConfigureValue, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ACConfig oACConfig, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ACConfig]"
                                    + "%n, %n, %n, %s, %n, %n",
                                    oACConfig.ACConfigID, oACConfig.ConfigureType, oACConfig.ConfigureValueType, oACConfig.ConfigureValue, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ACConfig WHERE AccountHeadID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ACConfig");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
