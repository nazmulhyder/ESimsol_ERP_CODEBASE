using System;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class FabricBatchProductionDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricBatchProduction oFabricBatchProduction, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchProduction]"
                                    + "%n,%n,%n,%n,%D,%D,%n,%n,%n,%n,%n,%n",
                                    oFabricBatchProduction.FBPID, oFabricBatchProduction.FBID, (int)oFabricBatchProduction.WeavingProcess, oFabricBatchProduction.FMID, NullHandler.GetNullValue(oFabricBatchProduction.StartTime), NullHandler.GetNullValue(oFabricBatchProduction.EndTime), oFabricBatchProduction.Qty,   oFabricBatchProduction.FabricBatchStatus, oFabricBatchProduction.ShiftID, oFabricBatchProduction.ProductionStatus, nUserID,(int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, FabricBatchProduction oFabricBatchProduction, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchProduction]"
                                    + "%n,%n,%n,%n,%D,%D,%n,%n,%n,%n,%n,%n",
                                    oFabricBatchProduction.FBPID, oFabricBatchProduction.FBID, (int)oFabricBatchProduction.WeavingProcess, oFabricBatchProduction.FMID, NullHandler.GetNullValue(oFabricBatchProduction.StartTime), NullHandler.GetNullValue(oFabricBatchProduction.EndTime), oFabricBatchProduction.Qty, oFabricBatchProduction.FabricBatchStatus, oFabricBatchProduction.ShiftID, oFabricBatchProduction.ProductionStatus, nUserID, (int)eEnumDBOperation);
        }
     
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProduction WHERE FBPID=%n", nID);
        }
        
        public static IDataReader GetByBatchAndWeavingType(TransactionContext tc, int nFBID, int nEProcess)
        {
            return tc.ExecuteReader("SELECT top 1 * FROM View_FabricBatchProduction WHERE FBID=%n AND WeavingProcess = %n", nFBID, nEProcess);
        }
        public static IDataReader Gets(int nFBID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProduction WHERE FBID = " + nFBID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsSummery(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion

        #region Import FabricBatch Production
        public static IDataReader ImportFabricBatchProductionDA(TransactionContext tc, FabricBatchProduction oFBP, Int64 nUserID)
        {

            return tc.ExecuteReader("EXEC [SP_Process_FabricBatchProductionUpload]"
                                   + "%n, %n, %n, %n, %n, %D, %D, %n, %n, %n, %n ,%n",
                                  oFBP.FBPID, oFBP.FabricSalesContractDetailID, oFBP.FBID, oFBP.FMID, oFBP.BeamID, oFBP.StartTime, oFBP.EndTime,"",nUserID ,oFBP.ShiftID);
        }

        #endregion
    }
}
