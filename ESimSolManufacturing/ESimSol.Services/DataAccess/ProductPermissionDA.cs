using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductPermissionDA
    {
        public ProductPermissionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductPermission oProductPermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductPermission]" + "%n, %n, %n, %n, %n, %s, %n, %n",
                                    oProductPermission.ProductPermissionID, oProductPermission.UserID, oProductPermission.ModuleNameInt, oProductPermission.ProductUsagesInt, oProductPermission.ProductCategoryID, oProductPermission.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ProductPermission oProductPermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductPermission]" + "%n, %n, %n, %n, %n, %s, %n, %n",
                                    oProductPermission.ProductPermissionID, oProductPermission.UserID, oProductPermission.ModuleNameInt, oProductPermission.ProductUsagesInt, oProductPermission.ProductCategoryID, oProductPermission.Remarks, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductPermission WHERE ProductPermissionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductPermission");
        }
        public static IDataReader GetsByUser(TransactionContext tc, int nPermittedUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductPermission WHERE UserID=%n ORder BY ModuleName", nPermittedUserID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
