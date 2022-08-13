using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VPPermissionDA
    {
        public VPPermissionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, VPPermission oVPPermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_VPPermission]" + "%n, %n, %n, %n, %n",
                                    oVPPermission.VPPermissionID, oVPPermission.UserID, oVPPermission.IntegrationSetupID, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, VPPermission oVPPermission, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_VPPermission]" + "%n, %n, %n, %n, %n",
                                    oVPPermission.VPPermissionID, oVPPermission.UserID, oVPPermission.IntegrationSetupID, (int)eEnumDBOperation, nUserID);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VPPermission WHERE VPPermissionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VPPermission");
        }
        public static IDataReader GetsByUser(TransactionContext tc, int nPermittedUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VPPermission WHERE UserID=%n ORder BY VPPermissionID", nPermittedUserID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
