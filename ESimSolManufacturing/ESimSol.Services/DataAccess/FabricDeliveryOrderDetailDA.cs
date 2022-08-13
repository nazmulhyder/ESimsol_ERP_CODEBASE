using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricDeliveryOrderDetailDA
    {
        public FabricDeliveryOrderDetailDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricDeliveryOrderDetail oFDODetail, int nDBOperation, Int64 nUserId,string sFDODDIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricDeliveryOrderDetail] %n, %n, %n, %n, %n, %n,%n,%n, %n, %n,%n, %s",
            oFDODetail.FDODID, oFDODetail.FDOID, oFDODetail.FabricID, oFDODetail.Qty, oFDODetail.UnitPrice, oFDODetail.FEOID, oFDODetail.ExportPIID, oFDODetail.ExportPIDetailID, nUserId, nDBOperation, (int)oFDODetail.DOPriceType, sFDODDIDs);
        }

        public static void Delete(TransactionContext tc, FabricDeliveryOrderDetail oFDODetail, int nDBOperation, Int64 nUserId, string sFDODDIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricDeliveryOrderDetail]%n, %n, %n, %n, %n, %n,%n,%n, %n, %n,%n, %s",
            oFDODetail.FDODID, oFDODetail.FDOID, oFDODetail.FabricID, oFDODetail.Qty, oFDODetail.UnitPrice, oFDODetail.FEOID, oFDODetail.ExportPIID, oFDODetail.ExportPIDetailID,nUserId, nDBOperation, (int)oFDODetail.DOPriceType, sFDODDIDs);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFDODID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricDeliveryOrderDetail WHERE FDODID=%n", nFDODID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFDOID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricDeliveryOrderDetail WHERE FDOID=%n", nFDOID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

    }
}
