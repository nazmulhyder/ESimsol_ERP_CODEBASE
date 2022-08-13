using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
   public class FabricYarnDeliveryChallanDA
    {
        #region Insert Update Delete Function

       public static IDataReader IUD(TransactionContext tc, FabricYarnDeliveryChallan oFabricYarnDeliveryChallan, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricYarnDeliveryChallan] " + "%n , %n, %n , %n ,%D ,%n ,%n", oFabricYarnDeliveryChallan.FYDChallanID, oFabricYarnDeliveryChallan.FYDOID,  oFabricYarnDeliveryChallan.WUID, oFabricYarnDeliveryChallan.DisburseBy, oFabricYarnDeliveryChallan.DisburseDate, nUserID, nDBOperation);
        }

        public static IDataReader Disburse(TransactionContext tc, FabricYarnDeliveryChallan oFabricYarnDeliveryChallan, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_FabricYarnDelChallanDisburse] " + "%n ,%n", oFabricYarnDeliveryChallan.FYDChallanID,  nUserID);
        }
       

        #endregion

        #region Get & Exist Function
       public static IDataReader Get(TransactionContext tc, int nFYDChallanID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricYarnDeliveryChallan WHERE FYDChallanID=%n", nFYDChallanID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
