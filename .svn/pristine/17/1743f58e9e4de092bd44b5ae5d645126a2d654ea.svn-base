using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
   public class FabricYarnDeliveryOrderDetailDA
    {
        #region Insert Update Delete Function
       public static IDataReader IUD(TransactionContext tc, FabricYarnDeliveryOrderDetail oFabricYarnDeliveryOrderDetail, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricYarnDeliveryOrderDetail] " + "%n, %n, %n, %n ,%n, %n, %n ", oFabricYarnDeliveryOrderDetail.FYDODetailID, oFabricYarnDeliveryOrderDetail.FYDOID, oFabricYarnDeliveryOrderDetail.ProductID, oFabricYarnDeliveryOrderDetail.Qty, oFabricYarnDeliveryOrderDetail.UnitPrice, nUserID, nDBOperation);
        }
        #endregion


        #region Get & Exist Function
       public static IDataReader Get(TransactionContext tc, int nFYDODetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricYarnDeliveryOrderDetail WHERE FYDODetailID=%n", nFYDODetailID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
