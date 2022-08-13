using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ITransactionDA
    {
        public ITransactionDA() { }

        #region Insert Update Delete Function
        
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM ITransaction WHERE ITransactionID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ITransaction");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader UpdateTransaction(TransactionContext tc, ITransaction oITransaction, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_UpdateTransactionDate]" + "%n, %d, %n", oITransaction.LotID, oITransaction.TransactionTime, nUserID);
        }
        #endregion
    }
}
