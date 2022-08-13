using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNBatchQCDetailDA
    {
        public FNBatchQCDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FNBatchQCDetail oFNBatchQCDetail, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNBatchQCDetail] %n, %n, %n, %s, %n, %b ,%n,%n,%n, %n,  %n, %D, %D, %n, %n", oFNBatchQCDetail.FNBatchQCDetailID, oFNBatchQCDetail.FNBatchQCID, oFNBatchQCDetail.Grade, oFNBatchQCDetail.LotNo, oFNBatchQCDetail.Qty, oFNBatchQCDetail.IsLock, oFNBatchQCDetail.ShadeID, oFNBatchQCDetail.GSM, oFNBatchQCDetail.Bowl_Skew, oFNBatchQCDetail.PointsYard, oFNBatchQCDetail.IsPassed, oFNBatchQCDetail.ProDate, oFNBatchQCDetail.DeliveryDate, nUserID, nDBOperation);
        }
        #endregion
        #region
        public static IDataReader ReceiveInDelivery(TransactionContext tc, FNBatchQCDetail oFNBatchQCDetail, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_FNBatch_ReceiveInDeliveryStore]"
                                    + "%n,%n,%n", oFNBatchQCDetail.FNBatchQCDetailID, oFNBatchQCDetail.WorkingUnitID, nUserID);
        }
        public static IDataReader ReceiveInDeliveryNew(TransactionContext tc, FNBatchQCDetail oFNBatchQCDetail, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_FNBatch_ReceiveInDeliveryStoreTwo]"
                                    + "%n,%s,%n,%n", oFNBatchQCDetail.FNBatchQCID, oFNBatchQCDetail.FNBatchQCDetailIDs, oFNBatchQCDetail.WorkingUnitID, nUserID);
        }
        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFNBatchQCDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM FNBatchQCDetail WHERE FNBatchQCDetailID=%n", nFNBatchQCDetailID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

 
        public static IDataReader ExcessQtyUpdate(TransactionContext tc, FNBatchQCDetail oFNBatchQCDetail, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNBatchQCDetailExcess]  %n, %n, %n", oFNBatchQCDetail.FNBatchQCDetailID, oFNBatchQCDetail.QtyExe, nUserID);
        }
        #endregion
    }
}
