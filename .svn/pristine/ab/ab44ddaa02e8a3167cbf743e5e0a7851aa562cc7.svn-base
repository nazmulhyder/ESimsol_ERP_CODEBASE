using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FNOrderFabricReceiveDA
    {
        public FNOrderFabricReceiveDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUDInsertUpdate(TransactionContext tc, FNOrderFabricReceive oFNExeDetail, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNOrderFabricReceive]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                                    oFNExeDetail.FNOrderFabricReceiveID, oFNExeDetail.FSCDID, oFNExeDetail.LotID, oFNExeDetail.Qty, oFNExeDetail.QtyTrIn, oFNExeDetail.QtyTrOut, oFNExeDetail.QtyReturn, oFNExeDetail.QtyCon, oFNExeDetail.Grade, oFNExeDetail.FabricReqRollID, oFNExeDetail.WUID, oFNExeDetail.LotNo, nUserId, nDBOperation, oFNExeDetail.FabricSource);
        }


        #endregion
        //#region Reeive
        //public static IDataReader Receive(TransactionContext tc, int FNOrderFabricReceiveID, Int64 nUserId)
        //{
        //    return tc.ExecuteReader("EXEC [SP_Process_FNExeOrderFabricReceive]" + "%n, %n", FNOrderFabricReceiveID, nUserId);
        //}

        //#endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nFNExODetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNOrderFabricReceive WHERE FNOrderFabricReceiveID=%n", nFNExODetailID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
