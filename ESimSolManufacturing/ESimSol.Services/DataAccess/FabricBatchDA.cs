using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class FabricBatchDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricBatch oFabricBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatch]"
                                    + "%n, %s, %n, %n, %n, %n, %n, %d, %n, %n, %n, %n, %n, %n, %n",
                                    oFabricBatch.FBID, oFabricBatch.BatchNo, oFabricBatch.FEOID, oFabricBatch.Qty, oFabricBatch.Status, oFabricBatch.FEOSID, oFabricBatch.FMID, oFabricBatch.IssueDate, oFabricBatch.TotalEnds, oFabricBatch.NoOfSection, oFabricBatch.WarpCount, oFabricBatch.FWPDID, oFabricBatch.PlanType, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, FabricBatch oFabricBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatch]"
                                    + "%n, %s, %n, %n, %n, %n,%n, %d, %n, %n, %n, %n, %n, %n, %n",
                                    oFabricBatch.FBID, oFabricBatch.BatchNo, oFabricBatch.FabricSalesContractDetailID, oFabricBatch.Qty, oFabricBatch.Status, oFabricBatch.FEOSID, oFabricBatch.FMID, oFabricBatch.IssueDate, oFabricBatch.TotalEnds, oFabricBatch.NoOfSection, oFabricBatch.WarpCount, oFabricBatch.FWPDID, oFabricBatch.PlanType, (int)eEnumDBOperation, nUserID);
        }
      
        public static IDataReader FabricProductionQCDone(TransactionContext tc, FabricBatch oFabricBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_FabricProductionQCDone]"
                                    + "%n, %n, %d, %n, %n",
                                    oFabricBatch.FBID, oFabricBatch.Qty, oFabricBatch.IssueDate,  nUserID);
                                    // Here IssueDate = Only date
                                    // Qty = QC Qty
        }
        public static IDataReader Finish(TransactionContext tc, FabricBatch oFabricBatch)
        {
            return tc.ExecuteReader("EXEC [SP_Process_FBMachineAndBeamFree] %n", oFabricBatch.FBID);
        }
        public static IDataReader BatchFinish(TransactionContext tc, FabricBatch oFabricBatch)
        {
            return tc.ExecuteReader("EXEC [SP_Process_FabricBatchFinish] %n", oFabricBatch.FBID);
        }
        public static IDataReader UpdateBatchNo(TransactionContext tc, FabricBatch oFabricBatch, Int64 nUserID, EnumDBOperation eEnumDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchNo]" + "%n, %s,%n", oFabricBatch.FBID, oFabricBatch.BatchNo, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatch WHERE FBID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatch");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsByFabricSalesContractDetailID(TransactionContext tc, int nFabricSalesContractDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatch WHERE FabricSalesContractDetailID=%n", nFabricSalesContractDetailID);
        }
        public static IDataReader GetByBatchNo(TransactionContext tc, string sBatchNo)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatch WHERE BatchNo=%s", sBatchNo);
        }
        #endregion
    }
}
