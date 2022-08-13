using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ChequeHistoryDA
    {
        public ChequeHistoryDA() { }



        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeHistory ORDER BY ChequeHistoryID ASC");
        }
        public static IDataReader Gets(TransactionContext tc, int nChequeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeHistory WHERE ChequeID=%n ORDER BY ChequeHistoryID DESC", nChequeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Get(TransactionContext tc, int nChequeID, int nStatus)
        {
            return tc.ExecuteReader("SELECT * FROM View_ChequeHistory WHERE ChequeID = %n AND CurrentStatus = %n", nChequeID, nStatus);
        }

        #endregion
    }  
}
