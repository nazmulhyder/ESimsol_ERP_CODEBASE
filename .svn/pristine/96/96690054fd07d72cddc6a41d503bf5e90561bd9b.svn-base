using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricSeekingDateDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricSeekingDate oFabricSeekingDate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSeekingDate]"
                                    + "%n, %n, %D, %n, %n, %n",
                                    oFabricSeekingDate.FabricID, oFabricSeekingDate.FabricRequestTypeInt, oFabricSeekingDate.SeekingDate,  oFabricSeekingDate.NoOfSets, nUserID,(int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricSeekingDate oFabricSeekingDate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricSeekingDate]"
                                    + "%n, %n, %D, %n, %n, %n",
                                    oFabricSeekingDate.FabricID, oFabricSeekingDate.FabricRequestTypeInt, NullHandler.GetNullValue(oFabricSeekingDate.SeekingDate), oFabricSeekingDate.NoOfSets, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricSeekingDate WHERE FabricSeekingDateID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, int nFabricID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricSeekingDate WHERE FabricID = %n", nFabricID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
