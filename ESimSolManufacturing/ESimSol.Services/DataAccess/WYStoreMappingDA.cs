using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class WYStoreMappingDA
    {
        public WYStoreMappingDA() { }

        #region Insert Update Delete Active Function
        public static IDataReader InsertUpdate(TransactionContext tc, WYStoreMapping oWYStoreMapping, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_WYStoreMapping]"
                                    + "%n,%n,%n,%n, %s,%n,%n,%n",
                                    oWYStoreMapping.WYStoreMappingID, oWYStoreMapping.WYarnTypeInt, oWYStoreMapping.WYStoreTypeInt, oWYStoreMapping.WorkingUnitID, oWYStoreMapping.Note, oWYStoreMapping.BUID, nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, WYStoreMapping oWYStoreMapping, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_WYStoreMapping]"
                                      + "%n,%n,%n,%n, %s,%n,%n,%n",
                                    oWYStoreMapping.WYStoreMappingID, oWYStoreMapping.WYarnTypeInt, oWYStoreMapping.WYStoreTypeInt, oWYStoreMapping.WorkingUnitID, oWYStoreMapping.Note, oWYStoreMapping.BUID, nUserId, (int)eEnumDBOperation);
        }
        public static IDataReader ToggleActivity(TransactionContext tc, WYStoreMapping oWYStoreMapping)
        {
            string sSQL1 = SQLParser.MakeSQL("Update WYStoreMapping Set Activity=~Activity WHERE WYStoreMappingID=%n", oWYStoreMapping.WYStoreMappingID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM WYStoreMapping WHERE WYStoreMappingID=%n", oWYStoreMapping.WYStoreMappingID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WYStoreMapping WHERE WYStoreMappingID=%n", nID);
        }
        public static IDataReader GetsActive(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_WYStoreMapping WHERE Activity=1");
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_WYStoreMapping");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByModule(TransactionContext tc, int nBUID, string sModuleIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_WYStoreMapping WHERE ModuleType IN ( " + sModuleIDs + " ) and Activity=1  ORDER BY Name", nBUID);
        }
        #endregion

    }
}