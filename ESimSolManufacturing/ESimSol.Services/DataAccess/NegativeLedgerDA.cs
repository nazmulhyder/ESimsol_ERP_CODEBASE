using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class NegativeLedgerDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nCompanyID)
        {
            return tc.ExecuteReader("EXEC [SP_NegativeLedger] %n",nCompanyID);
        }
        #endregion
    }
}
