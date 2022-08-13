using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DBPermissionDA
    {
        public DBPermissionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DBPermission oDBPermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DBPermission]" + "%n, %n, %n, %s, %n, %n",
                                    oDBPermission.DBPermissionID, oDBPermission.UserID, oDBPermission.DashBoardTypeInt, oDBPermission.Remarks, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, DBPermission oDBPermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DBPermission]" + "%n, %n, %n, %s, %n, %n",
                                    oDBPermission.DBPermissionID, oDBPermission.UserID, oDBPermission.DashBoardTypeInt, oDBPermission.Remarks, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            //return tc.ExecuteReader("SELECT * FROM View_DBPermission WHERE DBPermissionID=%n", nID);
            return tc.ExecuteReader("SELECT * FROM DBPermission WHERE DBPermissionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            //return tc.ExecuteReader("SELECT * FROM View_DBPermission");
            return tc.ExecuteReader("SELECT * FROM DBPermission");
        }
        public static IDataReader GetsByUser(TransactionContext tc, int nPermittedUserID)
        {
            //return tc.ExecuteReader("SELECT * FROM View_DBPermission WHERE UserID=%n", nPermittedUserID);
            return tc.ExecuteReader("SELECT * FROM DBPermission WHERE UserID=%n", nPermittedUserID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
