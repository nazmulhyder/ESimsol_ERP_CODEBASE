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
    public class FabricBatchLoomDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricBatchLoom oFabricBatchLoom, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchLoom]"
                                    + "%n,%n,%n,%n,%D,%D,%n,%n,%s,%n,%n,%b,%n,%s,%n,%n,%n,%n,%n,%n,%b,%n,%n,%n",
                                    oFabricBatchLoom.FabricBatchLoomID, oFabricBatchLoom.FBID, (int)oFabricBatchLoom.Status, oFabricBatchLoom.FMID, NullHandler.GetNullValue(oFabricBatchLoom.StartTime), NullHandler.GetNullValue(oFabricBatchLoom.EndTime), oFabricBatchLoom.Qty, oFabricBatchLoom.RPM, oFabricBatchLoom.Texture, oFabricBatchLoom.FBPriviousStatus, oFabricBatchLoom.FabricBatchStatus, oFabricBatchLoom.IsFromRunOut, oFabricBatchLoom.ReedCount, oFabricBatchLoom.Dent, oFabricBatchLoom.TSUID, (int)eEnumDBOperation, nUserID, oFabricBatchLoom.ShiftID, oFabricBatchLoom.BeamID, oFabricBatchLoom.FBPBeamID, oFabricBatchLoom.IsHold, oFabricBatchLoom.Efficiency, oFabricBatchLoom.Pick, oFabricBatchLoom.FLPID);
        }
        public static void Delete(TransactionContext tc, FabricBatchLoom oFabricBatchLoom, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchLoom]"
                                    + "%n,%n,%n,%n,%D,%D,%n,%n,%s,%n,%n,%b,%n,%s,%n,%n,%n,%n,%n,%n,%b,%n,%n,%n",
                                    oFabricBatchLoom.FabricBatchLoomID, oFabricBatchLoom.FBID, (int)oFabricBatchLoom.Status, oFabricBatchLoom.FMID, NullHandler.GetNullValue(oFabricBatchLoom.StartTime), NullHandler.GetNullValue(oFabricBatchLoom.EndTime), oFabricBatchLoom.Qty, oFabricBatchLoom.RPM, oFabricBatchLoom.Texture, oFabricBatchLoom.FBPriviousStatus, oFabricBatchLoom.FabricBatchStatus, oFabricBatchLoom.IsFromRunOut, oFabricBatchLoom.ReedCount, oFabricBatchLoom.Dent, oFabricBatchLoom.TSUID, (int)eEnumDBOperation, nUserID, oFabricBatchLoom.ShiftID, oFabricBatchLoom.BeamID, oFabricBatchLoom.FBPBeamID, oFabricBatchLoom.IsHold, oFabricBatchLoom.Efficiency, oFabricBatchLoom.Pick, oFabricBatchLoom.FLPID);
        }
        public static IDataReader InsertUpdateSizing(TransactionContext tc, FabricBatchLoom oFabricBatchLoom, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchLoomSizing]"
                                    + "%n,%n,%n,%n,%D,%D,%n,%n,%s,%n,%n,%b,%n,%b,%n,%n,%n",
                                    oFabricBatchLoom.FabricBatchLoomID, oFabricBatchLoom.FBID, (int)oFabricBatchLoom.WeavingProcess, oFabricBatchLoom.FMID, NullHandler.GetNullValue(oFabricBatchLoom.StartTime),  NullHandler.GetNullValue(oFabricBatchLoom.EndTime), oFabricBatchLoom.Qty, oFabricBatchLoom.RPM, oFabricBatchLoom.Texture, oFabricBatchLoom.FBPriviousStatus, oFabricBatchLoom.FabricBatchStatus, oFabricBatchLoom.IsFromRunOut, oFabricBatchLoom.BeamID, (int)eEnumDBOperation, nUserID,oFabricBatchLoom.ShiftID);
        }

        public static void UpdateWeaving(TransactionContext tc, FabricBatchLoom oFBP, Int64 nUserID)
        {
             tc.ExecuteNonQuery("Update FabricBatchLoom Set RPM = " + oFBP.RPM + ", ReedCount = " + oFBP.ReedCount + ",  Dent = '" + oFBP.Dent + "' Where FabricBatchLoomID = " + oFBP.FabricBatchLoomID + "");
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchLoom WHERE FabricBatchLoomID=%n", nID);
        }
        
        public static IDataReader GetByBatchAndWeavingType(TransactionContext tc, int nFBID, int nEProcess)
        {
            return tc.ExecuteReader("SELECT top 1 * FROM View_FabricBatchLoom WHERE FBID=%n AND WeavingProcess = %n", nFBID, nEProcess);
        }
        public static IDataReader Gets(int nFBID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchLoom WHERE FBID = " + nFBID);
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
        public static IDataReader ImportFabricBatchLoomDA(TransactionContext tc, FabricBatchLoom oFBP, Int64 nUserID)
        {
           

            return tc.ExecuteReader("EXEC [SP_Process_FabricBatchLoomUpload]"
                                   + "%n, %n, %n, %n, %n, %D, %D, %n, %n, %n, %n ,%n",
                                  oFBP.FabricBatchLoomID, oFBP.FSCDID, oFBP.FBID, oFBP.FMID, oFBP.BeamID, oFBP.StartTime, oFBP.EndTime, oFBP.RPM, oFBP.ReedCount, oFBP.TSUID,nUserID ,oFBP.ShiftID);
        }

        #endregion
    }
}
