using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class AccountingActivityDA
    {
        public AccountingActivityDA() { }

        

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, int nRoleUserID, DateTime dStartDate, DateTime dEnddate)
        {
            return tc.ExecuteReader("EXEC [dbo].[SP_AccountingActivity] %n,%d,%d", nRoleUserID, dStartDate, dEnddate);
        }
       
       
        #endregion
    }  
}
