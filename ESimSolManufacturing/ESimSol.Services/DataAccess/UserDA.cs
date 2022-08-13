using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class UserDA
    {
        public UserDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, User oUser, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_User]"
                                    + "%n, %s, %s, %s, %n, %b, %s, %b, %s, %n, %n, %n, %s, %n, %b, %n",
                                    oUser.UserID, oUser.LogInID, oUser.UserName, oUser.Password, oUser.OwnerID, oUser.LoggedOn, oUser.LoggedOnMachine, oUser.CanLogin, oUser.DomainUserName, oUser.EmployeeID, oUser.LocationID, oUser.AccountHolderTypeInInt, oUser.EmailAddress, oUser.FinancialUserTypeInt, oUser.IsLocationBindded, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, User oUser, EnumDBOperation eEnumDBOperation)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_User]"
                                    + "%n, %s, %s, %s, %n, %b, %s, %b, %s, %n, %n, %n, %s, %n,  %b, %n",
                                    oUser.UserID, oUser.LogInID, oUser.UserName, oUser.Password, oUser.OwnerID, oUser.LoggedOn, oUser.LoggedOnMachine, oUser.CanLogin, oUser.DomainUserName, oUser.EmployeeID, oUser.LocationID, oUser.AccountHolderTypeInInt, oUser.EmailAddress, oUser.FinancialUserTypeInt, oUser.IsLocationBindded, (int)eEnumDBOperation);
        }

        public static void ConfirmMenuPermission(TransactionContext tc, int nUserID, string sSelectedMenuKeys, Int64 nCurrentUserId, int nApplicationType)
        {
            tc.ExecuteNonQuery("EXEC SP_UserMenuPermissionKeys_Web %n, %n, %s, %n", nUserID, nCurrentUserId, sSelectedMenuKeys, nApplicationType);
        }

        public static void ChangePassword(TransactionContext tc, User oUser)
        {
            tc.ExecuteNonQuery(" UPDATE Users SET [Password]=%s WHERE UserID=%n", oUser.Password, oUser.UserID);
        }
        public static void ToggleShowLedgerBalance(TransactionContext tc, User oUser)
        {
            tc.ExecuteNonQuery("UPDATE Users SET IsShowLedgerBalance=%b WHERE UserID=%n", oUser.IsShowLedgerBalance, oUser.UserID);
        }

        public static IDataReader UpdateCanLogin(TransactionContext tc, int userID, bool canLogin)
        {
            tc.ExecuteNonQuery("UPDATE [Users] SET [Canlogin]=%b WHERE [UserID ]=%n", canLogin, userID);
            return tc.ExecuteReader("SELECT * FROM View_User WHERE [UserID ]=%n", userID);
        }

        #region Buyer As User
        public static IDataReader InsertUpdateBuyerAsUser(TransactionContext tc, User oUser, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_UserBuyerType] %s, %s, %n, %s, %n",
                                    oUser.Password, oUser.DomainUserName, oUser.EmployeeID, oUser.EmailAddress, (int)eEnumDBOperation);
        }
        public static IDataReader GetByBuyerID(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_User where AccountHolderType=%n and EmployeeID=%n", (int)EnumAccountHolderType.Contractor, nID);
        }
        #endregion
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_User WHERE UserID=%n", nID);
        }

        public static IDataReader Get(TransactionContext tc, string sLoginID)
        {
            return tc.ExecuteReader("SELECT * FROM View_User WHERE LogInID=%s", sLoginID);
        }

        public static IDataReader Gets(TransactionContext tc, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_User WHERE UserID NOT IN(%n,%n)  order by LogInID", nUserId, -9);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_User WHERE ([UserID] NOT IN(%n, %n)) AND " + sSQL + " ORDER BY LoginID", -9, nUserID);
        }
        /// <summary>
        /// added by fahim0abir on date : 13 Jul 2015
        /// used to get users including current user
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="sSQL"></param>
        /// <param name="nUserID"></param>
        /// <returns></returns>
        public static IDataReader GetsForProductionExecution(TransactionContext tc, string sSQL, Int64 nUserID)
        {
            return tc.ExecuteReader("SELECT * FROM View_User WHERE ([UserID] NOT IN(%n)) AND " + sSQL + " ORDER BY LoginID", -9);
        }
        public static IDataReader GetsByLogInID(TransactionContext tc, string sLogInID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_User WHERE UserID NOT IN(%n,%n) AND LogInID=%s", nUserId, -9, sLogInID);
        }

        public static int CountPermissionkey(TransactionContext tc, int userID)
        {
            object ob = tc.ExecuteScalar("SELECT COUNT(*) FROM [UserPermissionFinance] WHERE [UserID]=%n", userID);
            return ob == DBNull.Value ? 0 : Convert.ToInt32(ob);
        }

        public static IDataReader GetPermissionKeys(TransactionContext tc, int userID)
        {
            return tc.ExecuteReader("SELECT [MenuID] FROM [UserPermissionFinance] WHERE [UserID]=%n", userID);
        }
        public static int GetEmployeeID(TransactionContext tc, Int64 nUserID)
        {
            object obj = tc.ExecuteScalar("select EmployeeID  from View_User where [UserID]=%n ", nUserID);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }
        public static int GetEmployeeType(TransactionContext tc, Int64 nUserID)
        {
            object obj = tc.ExecuteScalar("select isnull(EmployeeType,0)  from View_User where [UserID]=%n ", nUserID);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }
        public static int GetContractorType(TransactionContext tc, Int64 nUserID)
        {
            object obj = tc.ExecuteScalar("select isnull(ContractorType,0)  from View_User where [UserID]=%n ", nUserID);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }
        public static int GetAccountHolderType(TransactionContext tc, Int64 nUserID)
        {
            object obj = tc.ExecuteScalar("select isnull(AccountHolderType,0)  from View_User where [UserID]=%n ", nUserID);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }
        public static int GetContractorID(TransactionContext tc, Int64 nUserID)
        {
            object obj = tc.ExecuteScalar("select isnull(ContractorID,0)  from View_User where [UserID]=%n ", nUserID);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }
        public static IDataReader GetsBySql(TransactionContext tc, string sSQL, Int64 nUserID)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetUserActionLogs(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetByLogInID(TransactionContext tc, string sLogInID)
        {
            return tc.ExecuteReader("Select * from View_User Where LogInID=%s", sLogInID);
        }

        #endregion
    }  
}
