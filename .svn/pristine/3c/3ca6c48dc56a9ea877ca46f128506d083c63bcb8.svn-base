using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricBatchProductionBeamDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricBatchProductionBeam oFabricBatchProductionBeam, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchProductionBeam]"
                                    + "%n, %n, %n, %n,%n, %b, %n, %n, %n, %n",
                                    oFabricBatchProductionBeam.FBPBeamID, oFabricBatchProductionBeam.FBPID, oFabricBatchProductionBeam.BeamID, oFabricBatchProductionBeam.Qty, oFabricBatchProductionBeam.QtyM, oFabricBatchProductionBeam.IsFinish, oFabricBatchProductionBeam.FBPDetailID, oFabricBatchProductionBeam.IsDrawing, (int)eEnumDBOperation, nUserID);
        }
        public static void Delete(TransactionContext tc, FabricBatchProductionBeam oFabricBatchProductionBeam, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchProductionBeam]"
                                    + "%n, %n, %n, %n,%n, %b, %n, %n, %n, %n",
                                    oFabricBatchProductionBeam.FBPBeamID, oFabricBatchProductionBeam.FBPID, oFabricBatchProductionBeam.BeamID, oFabricBatchProductionBeam.Qty, oFabricBatchProductionBeam.QtyM, oFabricBatchProductionBeam.IsFinish, oFabricBatchProductionBeam.FBPDetailID, oFabricBatchProductionBeam.IsDrawing, (int)eEnumDBOperation, nUserID);
        }
        
        #endregion

        #region Get & Exist Function 
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProductionBeam");
        }
        public static IDataReader GetsFinishedBeams(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProductionBeam WHERE IsFinish=1 AND WeavingProcessType=" + (int)EnumWeavingProcess.Sizing);
        }
        public static IDataReader Get(TransactionContext tc, long nFBPID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProductionBeam WHERE FBPID=%n", nFBPID);
        }
        public static IDataReader GetsByFabricBatchProduction(TransactionContext tc, int nFBPID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProductionBeam WHERE FBPID=%n", nFBPID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void Finish(TransactionContext tc, int nFBPBeamID, bool bFinish)
        {
            tc.ExecuteNonQuery("UPDATE FabricBatchProductionBeam SET IsFinish=%b WHERE FBPBeamID=%n", bFinish, nFBPBeamID);
        }
        public static IDataReader TransferFinishBeam(TransactionContext tc, int FBPBeamID, int BeamID)
        {
            return tc.ExecuteReader("EXEC [SP_TransferSizingBeam]"
                                    + "%n, %n",
                                   FBPBeamID,BeamID);
        }
        #endregion
    }
}
