using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ReceivedChequeHistoryDA
    {
        public ReceivedChequeHistoryDA() { }



        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ReceivedChequeHistory ORDER BY ReceivedChequeHistoryID ASC");
        }
        public static IDataReader Gets(TransactionContext tc, int nReceivedChequeID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ReceivedChequeHistory WHERE ReceivedChequeID=%n ORDER BY ReceivedChequeHistoryID DESC", nReceivedChequeID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Get(TransactionContext tc, int nReceivedChequeID, int nStatus)
        {
            return tc.ExecuteReader("SELECT * FROM View_ReceivedChequeHistory WHERE ReceivedChequeID = %n AND CurrentStatus = %n", nReceivedChequeID, nStatus);
        }

        #endregion
    }  
}
