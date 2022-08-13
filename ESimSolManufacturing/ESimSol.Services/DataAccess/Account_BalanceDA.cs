using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    class Account_BalanceDA
    {
        public Account_BalanceDA() { }

        #region Insert Function
        public static void Insert(TransactionContext tc, Account_Balance oAccount_Balance)
        {
            tc.ExecuteNonQuery("INSERT INTO Account_Balance(Account_Balance_ID, Bank_ID, ERQAC, FCAC, LTR, PAD, Saleable_Amount, Date)"
                + " VALUES(%n, %n, %n, %n, %n, %n,%n,%D)",
                oAccount_Balance.Account_Balance_ID, oAccount_Balance.Bank_ID, oAccount_Balance.ERQAC, oAccount_Balance.FCAC, oAccount_Balance.LTR, oAccount_Balance.PAD, oAccount_Balance.Saleable_Amount, oAccount_Balance.Date);
        }
        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, Account_Balance oAccount_Balance)
        {
            tc.ExecuteNonQuery("UPDATE Account_Balance SET Bank_ID=%n, ERQAC=%n,  FCAC=%n, LTR=%n, PAD=%n,Saleable_Amount=%n, Date=%D"
                + " WHERE Account_Balance_ID=%n", oAccount_Balance.Bank_ID, oAccount_Balance.ERQAC, oAccount_Balance.FCAC, oAccount_Balance.LTR, oAccount_Balance.PAD, oAccount_Balance.Saleable_Amount, oAccount_Balance.Date, oAccount_Balance.Account_Balance_ID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("Account_Balance", "Account_Balance_ID");
        }

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM Account_Balance WHERE Account_Balance_ID=%n", nID);
        }
        #endregion

        #region gets
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC sp_GetCurrentAccountBalance");
        }
        #endregion
    }
}
