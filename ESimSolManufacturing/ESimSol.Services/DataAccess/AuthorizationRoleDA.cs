using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AuthorizationRoleDA
    {        
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AuthorizationRole oAuthorizationRole, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AuthorizationRole]" + "%n, %s, %n, %n, %s, %n, %n",
                                    oAuthorizationRole.AuthorizationRoleID, oAuthorizationRole.RoleNo, oAuthorizationRole.ModuleNameInt, oAuthorizationRole.OperationTypeInt, oAuthorizationRole.Note, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, AuthorizationRole oAuthorizationRole, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AuthorizationRole]" + "%n, %s, %n, %n, %s, %n, %n",
                                    oAuthorizationRole.AuthorizationRoleID, oAuthorizationRole.RoleNo, oAuthorizationRole.ModuleNameInt, oAuthorizationRole.OperationTypeInt, oAuthorizationRole.Note, (int)eEnumDBOperation, nUserID);
        }

        //
        public static void CopyAuthorization(TransactionContext tc, int nAssignUserID, int nCopyFromUserID, bool bUserPermission, bool bAuthorizationRule, bool bStorePermission, bool bProductPermission, bool bBUPermission, bool bTimeCardPermission, bool bAutoVoucharPermission, bool bDashBoardPermission, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_CopyAuthorizationRole]"
                                    + "%n,%n,%b,%b,%b,%b,%b,%b,%b,%b,%n", nAssignUserID, nCopyFromUserID, bUserPermission, bAuthorizationRule, bStorePermission, bProductPermission, bBUPermission, bTimeCardPermission, bAutoVoucharPermission, bDashBoardPermission, nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM AuthorizationRole WHERE AuthorizationRoleID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM AuthorizationRole order by AuthorizationRoleID DESC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void CheckUserPermission(TransactionContext tc, long nUserID, EnumModuleName eModuleName, EnumRoleOperationType enumRoleOperationType)
        {
            tc.ExecuteNonQuery("EXEC [SP_HasAuthorizationRole]" + "%n, %n, %n", nUserID, (int)eModuleName, (int)enumRoleOperationType);
        }

        #endregion

        
    }  
    
  
}
