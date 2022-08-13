using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AccountsBookSetupDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AccountsBookSetupDetail oAccountsBookSetupDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AccountsBookSetupDetail]"
                                    + "%n, %n, %n, %n, %n, %n",
                                    oAccountsBookSetupDetail.AccountsBookSetupDetailID, oAccountsBookSetupDetail.AccountsBookSetupID,oAccountsBookSetupDetail.AccountHeadID, oAccountsBookSetupDetail.ComponentTypeInt, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, AccountsBookSetupDetail oAccountsBookSetupDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AccountsBookSetupDetail]"
                                    + "%n, %n, %n, %n, %n, %n",
                                    oAccountsBookSetupDetail.AccountsBookSetupDetailID, oAccountsBookSetupDetail.AccountsBookSetupID, oAccountsBookSetupDetail.AccountHeadID, oAccountsBookSetupDetail.ComponentTypeInt, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountsBookSetupDetail WHERE AccountsBookSetupDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_AccountsBookSetupDetail WHERE AccountsBookSetupID = %n", id);
        }
        public static IDataReader GetsSQL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
