using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AccountsBookSetupDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, AccountsBookSetup oAccountsBookSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_AccountsBookSetup]" + "%n, %s, %n, %s, %n, %n",
                                    oAccountsBookSetup.AccountsBookSetupID,   oAccountsBookSetup.AccountsBookSetupName, oAccountsBookSetup.MappingTypeInt, oAccountsBookSetup.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, AccountsBookSetup oAccountsBookSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_AccountsBookSetup]" + "%n, %s, %n, %s, %n, %n",
                                    oAccountsBookSetup.AccountsBookSetupID, oAccountsBookSetup.AccountsBookSetupName, oAccountsBookSetup.MappingTypeInt, oAccountsBookSetup.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM AccountsBookSetup WHERE AccountsBookSetupID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM AccountsBookSetup");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
