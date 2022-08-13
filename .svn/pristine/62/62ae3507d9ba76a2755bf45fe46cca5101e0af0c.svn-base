using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricExecutionOrderDetailDA
    {
        public FabricExecutionOrderDetailDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricExecutionOrderDetail oFEODetail, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrderDetail] %n,%n,%n,%n,%n,%n,%n",
            oFEODetail.FEODID, oFEODetail.FEOID, oFEODetail.ProductID, oFEODetail.Qty,oFEODetail.SuggestedQty ,nUserId, nDBOperation);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFEODID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrderDetail WHERE FEODID=%n", nFEODID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFEOID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrderDetail WHERE FEOID=%n", nFEOID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

    }
}
