using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class FabricBatchProductionBreakageDA
    {
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchProductionBreakage oFabricBatchProductionBreakage, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchProductionBreakage]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n",
                                    oFabricBatchProductionBreakage.FBPBreakageID, oFabricBatchProductionBreakage.FBLDetailID, oFabricBatchProductionBreakage.FBreakageID, oFabricBatchProductionBreakage.DurationInMin, oFabricBatchProductionBreakage.NoOfBreakage, oFabricBatchProductionBreakage.Note, nUserID, nEnumDBOperation);
        }

        public static void  Delete(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchProductionBreakage oFabricBatchProductionBreakage, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchProductionBreakage]"
                                    + "%n,%n,%n,%n,%n,%s,%n,%n",
                                    oFabricBatchProductionBreakage.FBPBreakageID, oFabricBatchProductionBreakage.FBLDetailID, oFabricBatchProductionBreakage.FBreakageID, oFabricBatchProductionBreakage.DurationInMin, oFabricBatchProductionBreakage.NoOfBreakage, oFabricBatchProductionBreakage.Note, nUserID, nEnumDBOperation);
        }

        
        #endregion


        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nFBPBID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProductionBreakage WHERE FBLDetailID=%n", nFBPBID);
        }
        #endregion
    }
    
   
}
