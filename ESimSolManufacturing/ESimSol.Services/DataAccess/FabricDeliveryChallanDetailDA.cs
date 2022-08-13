using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricDeliveryChallanDetailDA
    {
        public FabricDeliveryChallanDetailDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricDeliveryChallanDetail oFDCDetail, int nDBOperation, Int64 nUserId,string sFDODDIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricDeliveryChallanDetail] %n,%n,%n,%n,%n,%n,%n,%n,%n,%s",
            oFDCDetail.FDCDID, oFDCDetail.FDCID, oFDCDetail.FDODID, oFDCDetail.LotID, oFDCDetail.Qty, oFDCDetail.FSCDID,oFDCDetail.FNBatchQCDetailID, nUserId, nDBOperation, sFDODDIDs);
        }
        public static void Delete(TransactionContext tc, FabricDeliveryChallanDetail oFDCDetail, int nDBOperation, Int64 nUserId, string sFDODDIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricDeliveryChallanDetail] %n,%n,%n,%n,%n,%n,%n,%n,%n,%s",
           oFDCDetail.FDCDID, oFDCDetail.FDCID, oFDCDetail.FDODID, oFDCDetail.LotID, oFDCDetail.Qty, oFDCDetail.FSCDID,oFDCDetail.FNBatchQCDetailID,nUserId, nDBOperation, sFDODDIDs);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFDCDID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricDeliveryChallanDetail WHERE FDCDID=%n", nFDCDID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFDCID, bool bIsSample, Int64 nUserId)
        {
            string sSQL = "";
            if (!bIsSample) { sSQL = "SELECT * FROM View_FabricDeliveryChallanDetail WHERE FDCID=%n and isnull(IsSample,0)=0 order by CONVERT(int,dbo.udf_GetNumeric(SUBSTRING(LotNo,8,12))) ASC"; }
            else { sSQL = "SELECT * FROM View_FabricDeliveryChallanDetailPPS WHERE FDCID=%n and isnull(IsSample,0)=1"; }
            return tc.ExecuteReader(sSQL, nFDCID);
        }
        public static IDataReader GetsForAdj(TransactionContext tc, int nContractorID, int nParentFDCID,Int64 nUserId)
        {
            return tc.ExecuteReader("Select * from View_FabricDeliveryChallanDetailPPS where isnull(ParentFDCID,0)=%n and isnull(IsSample,0)=1", nParentFDCID);
        }
        public static IDataReader Update_Adj(TransactionContext tc, FabricDeliveryChallanDetail oFabricDeliveryChallanDetail)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricDeliveryChallanDetail Set ParentFDCID=%n WHERE FDCDID=%n", oFabricDeliveryChallanDetail.ParentFDCID, oFabricDeliveryChallanDetail.FDCDID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FabricDeliveryChallanDetailPPS WHERE FDCDID=%n", oFabricDeliveryChallanDetail.ParentFDCID);

        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

    }
}
