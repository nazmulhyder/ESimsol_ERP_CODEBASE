using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class FabricYarnDeliveryChallanDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FabricYarnDeliveryChallanDetail oFabricYarnDeliveryChallanDetail, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricYarnDeliveryChallanDetail] " + "%n, %n, %n, %n ,%n, %n, %n ,%s,%s ", oFabricYarnDeliveryChallanDetail.FYDCDetailID, oFabricYarnDeliveryChallanDetail.FYDChallanID, oFabricYarnDeliveryChallanDetail.FYDODetailID, oFabricYarnDeliveryChallanDetail.Qty, oFabricYarnDeliveryChallanDetail.LotID, nUserID, nDBOperation, oFabricYarnDeliveryChallanDetail.Remarks,oFabricYarnDeliveryChallanDetail.BagQty);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFYDCDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricYarnDeliveryChallanDetail WHERE FYDCDetailID=%n", nFYDCDetailID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
