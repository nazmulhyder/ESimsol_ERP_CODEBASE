using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class AccountsBookDA
    {
        #region Insert Update Delete Function
       
        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nAccountsBookSetupID, DateTime dStartDate, DateTime dEndDate, string BUIDs, bool bApprovedOnly)
        {
            return tc.ExecuteReader("EXEC [SP_AccountsBook]" + "%n, %d, %d, %s, %b", nAccountsBookSetupID, dStartDate, dEndDate, BUIDs, bApprovedOnly);
        }     
        #endregion
    }
}
