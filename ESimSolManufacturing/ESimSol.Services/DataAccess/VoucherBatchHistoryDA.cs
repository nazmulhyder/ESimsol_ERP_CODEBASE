using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class VoucherBatchHistoryDA
    {
        public VoucherBatchHistoryDA() { }

        #region Insert Update Delete Function
        
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBatchHistory WHERE VoucherBatchHistoryID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBatchHistory");
        }
        public static IDataReader GetsByBatchID(TransactionContext tc,int nVoucherBatchID)
        {
            return tc.ExecuteReader("SELECT * FROM View_VoucherBatchHistory WHERE VoucherBatchID = " + nVoucherBatchID + " ORDER BY DBServerDateTime DESC");
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_VoucherBatchHistory
        }
      

       

       
        #endregion
    }  
}
