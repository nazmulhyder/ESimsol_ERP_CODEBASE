using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class TempObjectDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nStatementSetupID, DateTime dstartDate, DateTime dendDate,int nBUID)
        {
            return tc.ExecuteReader("EXECUTE [SP_YetToAccountHeadConfigure] %n, %d, %d, %n", nStatementSetupID, dstartDate, dendDate, nBUID);
        }
        #endregion
    }
}
