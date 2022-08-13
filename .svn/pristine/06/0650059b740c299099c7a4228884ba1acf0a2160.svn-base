using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AuthorizationRoleMappingDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AuthorizationRoleMapping oAuthorizationRoleMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AuthorizationRoleMapping]"
                                    + "%n,%n,%n,%n,%n,%b,%s",
                                    oAuthorizationRoleMapping.AuthorizationRoleMappingID, oAuthorizationRoleMapping.AuthorizationRoleID, oAuthorizationRoleMapping.UserID, nUserID, (int)eEnumDBOperation, false, "");
        }

        public static void Delete(TransactionContext tc, AuthorizationRoleMapping oAuthorizationRoleMapping, EnumDBOperation eEnumDBOperation, Int64 nUserID, bool IsUserBased, string ids)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AuthorizationRoleMapping]"
                                    + "%n,%n,%n,%n,%n,%b,%s",
                                    oAuthorizationRoleMapping.AuthorizationRoleMappingID, oAuthorizationRoleMapping.AuthorizationRoleID, oAuthorizationRoleMapping.UserID,  nUserID, (int)eEnumDBOperation, IsUserBased, ids);
        }

        public static void DisallowMappingRole(TransactionContext tc, string ids)
        {
            tc.ExecuteNonQuery("DELETE FROM AuthorizationRoleMapping WHERE AuthorizationRoleMappingID IN (%s)", ids);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AuthorizationRoleMapping WHERE AuthorizationRoleMappingID=%n", nID);
        }

        public static IDataReader GetsByRole(TransactionContext tc , int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_AuthorizationRoleMapping Where AuthorizationRoleID = " + id);
        }

        public static IDataReader GetsByUser(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_AuthorizationRoleMapping Where UserID = " + id + " order by ModuleName,OperationType");
        }

        public static IDataReader GetsByModuleAndUser(TransactionContext tc, string sModuleNames, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_AuthorizationRoleMapping AS HH WHERE HH.UserID =" + id + " AND  HH.ModuleName IN (" + sModuleNames + ")");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }  


    
}
