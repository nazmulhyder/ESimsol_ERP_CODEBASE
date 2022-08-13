using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
   public class FabricYarnDeliveryOrderDA
    {
        #region Insert Update Delete Function

       public static IDataReader IUD(TransactionContext tc, FabricYarnDeliveryOrder oFabricYarnDeliveryOrder, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricYarnDeliveryOrder] " + "%n , %n, %n , %n ,%D, %D, %n ,%n ,%s", oFabricYarnDeliveryOrder.FYDOID, oFabricYarnDeliveryOrder.FEOID, (int)oFabricYarnDeliveryOrder.DeliveryUnit, oFabricYarnDeliveryOrder.ApproveBy, oFabricYarnDeliveryOrder.ApproveDate, oFabricYarnDeliveryOrder.ExpectedDeliveryDate, nUserID, nDBOperation, oFabricYarnDeliveryOrder.Remark);
        }
      
        #endregion

        #region Get & Exist Function
       public static IDataReader Get(TransactionContext tc, int nFYDOID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricYarnDeliveryOrder WHERE FYDOID=%n", nFYDOID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
