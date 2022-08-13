using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class FabricBatchRawMaterialDA
    {
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchRawMaterial oFabricBatchRawMaterial, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchRawMaterial]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%d,%s,%b,%n,%n,%n",
                                    oFabricBatchRawMaterial.FBRMID, 
                                    oFabricBatchRawMaterial.FBID, 
                                    (int)oFabricBatchRawMaterial.WeavingProcess, 
                                    oFabricBatchRawMaterial.ProductID, 
                                    oFabricBatchRawMaterial.LotID, 
                                    oFabricBatchRawMaterial.Qty, 
                                    oFabricBatchRawMaterial.ReceiveBy, 
                                    oFabricBatchRawMaterial.ReceiveByDate, 
                                    oFabricBatchRawMaterial.ColorName, 
                                    oFabricBatchRawMaterial.IsChemicalOut,
                                    oFabricBatchRawMaterial.FabricChemicalPlanID,
                                    nUserID, 
                                    nEnumDBOperation);
        }
        
        public static void  Delete(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchRawMaterial oFabricBatchRawMaterial, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchRawMaterial]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%d,%s,%b,%n,%n,%n",
                                    oFabricBatchRawMaterial.FBRMID,
                                    oFabricBatchRawMaterial.FBID,
                                    (int)oFabricBatchRawMaterial.WeavingProcess,
                                    oFabricBatchRawMaterial.ProductID,
                                    oFabricBatchRawMaterial.LotID,
                                    oFabricBatchRawMaterial.Qty,
                                    oFabricBatchRawMaterial.ReceiveBy,
                                    oFabricBatchRawMaterial.ReceiveByDate,
                                    oFabricBatchRawMaterial.ColorName,
                                    oFabricBatchRawMaterial.IsChemicalOut,
                                    oFabricBatchRawMaterial.FabricChemicalPlanID,
                                    nUserID,
                                    nEnumDBOperation);
        }
        public static IDataReader YarnOut(TransactionContext tc, FabricBatchRawMaterial oFabricBatchRawMaterial, EnumWeavingProcess WeavingProcess, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_FabricYarnOut]"
                                    + "%n,%n, %n, %n, %n ,%n",
                                    oFabricBatchRawMaterial.FBID, oFabricBatchRawMaterial.FBRMID, oFabricBatchRawMaterial.LotID, oFabricBatchRawMaterial.Qty, nUserID, (int)WeavingProcess);
        }
        public static IDataReader ChemicalOut(TransactionContext tc, FabricBatchRawMaterial oFabricBatchRawMaterial, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_FabricChemicalOut]"
                                    + "%n,%n, %n, %n, %n",
                                    oFabricBatchRawMaterial.FBID, oFabricBatchRawMaterial.FBRMID, oFabricBatchRawMaterial.LotID, oFabricBatchRawMaterial.Qty, nUserID);
        }

        #endregion


        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nFBID, int nWeivingProcess)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchRawMaterial WHERE FBID=%n AND WeavingProcess = %n", nFBID,nWeivingProcess);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
   
}
