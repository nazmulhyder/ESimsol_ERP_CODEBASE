using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class StorePermissionDA
    {
        public StorePermissionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, StorePermission oStorePermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_StorePermission]" + "%n, %n, %n, %n, %n, %s, %n, %n",
                                    oStorePermission.StorePermissionID, oStorePermission.UserID, oStorePermission.ModuleNameInt, oStorePermission.WorkingUnitID, oStorePermission.StoreTypeInt, oStorePermission.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, StorePermission oStorePermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_StorePermission]" + "%n, %n, %n, %n, %n, %s, %n, %n",
                                    oStorePermission.StorePermissionID, oStorePermission.UserID, oStorePermission.ModuleNameInt, oStorePermission.WorkingUnitID, oStorePermission.StoreTypeInt, oStorePermission.Remarks, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_StorePermission WHERE StorePermissionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_StorePermission");
        }
        public static IDataReader GetsByUser(TransactionContext tc, int nPermittedUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_StorePermission WHERE UserID=%n", nPermittedUserID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
