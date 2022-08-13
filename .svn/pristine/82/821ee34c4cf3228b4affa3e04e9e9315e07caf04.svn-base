using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricExecutionOrderYarnReceiveDA
    {
        public FabricExecutionOrderYarnReceiveDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FabricExecutionOrderYarnReceive oFEOYR, EnumDBOperation nDBOperation, Int64 nUserId, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrderYarnReceive]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s, %n, %s,%n, %s, %n,%n, %n,%n,%s",
                                    oFEOYR.FEOYID, oFEOYR.WYRequisitionID, oFEOYR.FSCDID, oFEOYR.IssueLotID, oFEOYR.ReqQty, oFEOYR.ReceiveQty, oFEOYR.BagQty, oFEOYR.DestinationLotID, oFEOYR.WarpWeftType, oFEOYR.DyeingOrderDetailID, oFEOYR.Remarks, oFEOYR.FEOSDID, oFEOYR.Dia, oFEOYR.TFLength, oFEOYR.BeamNo, oFEOYR.NumberOfCone, oFEOYR.TFLengthLB, nUserId, nDBOperation, sDetailIDs);
        }
        public static void Delete(TransactionContext tc, FabricExecutionOrderYarnReceive oFEOYR, EnumDBOperation nDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricExecutionOrderYarnReceive]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s, %n, %s,%n,  %s, %n,%n, %n,%n,%s",
                                    oFEOYR.FEOYID, oFEOYR.WYRequisitionID, oFEOYR.FSCDID, oFEOYR.IssueLotID, oFEOYR.ReqQty, oFEOYR.ReceiveQty, oFEOYR.BagQty, oFEOYR.DestinationLotID, oFEOYR.WarpWeftType, oFEOYR.DyeingOrderDetailID, oFEOYR.Remarks, oFEOYR.FEOSDID, oFEOYR.Dia, oFEOYR.TFLength, oFEOYR.BeamNo, oFEOYR.NumberOfCone, oFEOYR.TFLengthLB, nUserID, nDBOperation, sDetailIDs);
        }
        #endregion

        #region Reeive
        public static IDataReader Receive(TransactionContext tc, FabricExecutionOrderYarnReceive oFEOYR, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrderYarnReceive]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s, %n, %s,%n, %s, %n, %n,%n,%s",
                                    oFEOYR.FEOYID, oFEOYR.WYRequisitionID, oFEOYR.FSCDID, oFEOYR.IssueLotID, oFEOYR.ReqQty, oFEOYR.ReceiveQty, oFEOYR.BagQty, oFEOYR.DestinationLotID, oFEOYR.WarpWeftType, oFEOYR.DyeingOrderDetailID, oFEOYR.Remarks, oFEOYR.FEOSDID, oFEOYR.Dia, oFEOYR.TFLength, oFEOYR.BeamNo, oFEOYR.NumberOfCone, nUserId, EnumDBOperation.Receive, "");

        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nFEOYID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrderYarnReceive WHERE FEOYID=%n", nFEOYID);
        }

        public static IDataReader Gets(TransactionContext tc, int nWYID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrderYarnReceive WHERE WYRequisitionID=%n", nWYID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader UpdateDetail(TransactionContext tc, FabricExecutionOrderYarnReceive oFEOYR)
        {
            tc.ExecuteNonQuery("UPDATE FabricExecutionOrderYarnReceive SET ReceiveQty = %n WHERE FEOYID = %n", oFEOYR.ReceiveQty, oFEOYR.FEOYID);
            return tc.ExecuteReader("SELECT * FROM [View_WYRequisitionDetail] WHERE FEOYID=%n", oFEOYR.FEOYID);
        }

        public static void UpdateObj(TransactionContext tc, FabricExecutionOrderYarnReceive oFEOYR)
        {
            tc.ExecuteNonQuery("UPDATE FabricExecutionOrderYarnReceive SET ReceiveQty = %n, NumberOfCone = %n, BeamNo = %s WHERE FEOYID = %n", oFEOYR.ReceiveQty, oFEOYR.NumberOfCone, oFEOYR.BeamNo, oFEOYR.FEOYID);
        }
        public static IDataReader GetRequisitionDetail(TransactionContext tc, long nFEOYID)
        {
            return tc.ExecuteReader("SELECT * FROM View_WYRequisitionDetail WHERE FEOYID=%n", nFEOYID);
        }

        #endregion
    }
}
