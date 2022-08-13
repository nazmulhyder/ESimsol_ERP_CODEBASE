using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class FabricBatchProductionColorDA
    {
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchProductionColor oFabricBatchProductionColor, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchProductionColor]"
                                    + "%n,%n,%s,%n,%s,%n,%n",
                                   oFabricBatchProductionColor.FBPColorID, oFabricBatchProductionColor.FBPBID, oFabricBatchProductionColor.Name, oFabricBatchProductionColor.Qty, oFabricBatchProductionColor.Note,  nUserID, nEnumDBOperation);
        }

        public static void  Delete(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchProductionColor oFabricBatchProductionColor, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchProductionColor]"
                                   + "%n,%n,%s,%n,%s,%n,%n",
                                  oFabricBatchProductionColor.FBPColorID, oFabricBatchProductionColor.FBPBID, oFabricBatchProductionColor.Name, oFabricBatchProductionColor.Qty, oFabricBatchProductionColor.Note, nUserID, nEnumDBOperation);
        }

        #endregion


        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nFBPID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricBatchProductionColor WHERE FBPBID=%n ", nFBPID);
        }
        #endregion
    }
    
   
}
