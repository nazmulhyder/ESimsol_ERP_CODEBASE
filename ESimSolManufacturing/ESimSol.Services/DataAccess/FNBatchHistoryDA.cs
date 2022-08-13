using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNBatchHistoryDA
    {
        public FNBatchHistoryDA() { }

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFNBatchHistoryID)
        {
            return tc.ExecuteReader("SELECT * FROM FNBatchHistory WHERE FNBatchHistoryID=%n", nFNBatchHistoryID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
