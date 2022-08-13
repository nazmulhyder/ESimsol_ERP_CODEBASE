using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricDeliveryOrderDA
    {
        public FabricDeliveryOrderDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricDeliveryOrder oFDO, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricDeliveryOrder] %n,%s, %n, %n, %d, %n,%n, %n, %d, %n, %s, %n, %n, %n, %n, %n, %n",
                                    oFDO.FDOID, 
                                    oFDO.DONo,
                                    oFDO.FDOTypeInInt,
                                    (int)oFDO.DOStatus,  
                                    oFDO.DODate,
                                    oFDO.ContractorID,
                                    oFDO.BuyerID,  
                                    oFDO.DeliveryZoneID, 
                                    oFDO.ExpDeliveryDate,  
                                    oFDO.CurrencyID, 
                                    oFDO.Note,  
                                    oFDO.BCPID,
                                    oFDO.MKTPersonID,
                                    oFDO.DeliveryFrom_BUID,
                                    oFDO.BUID,
                                    nUserId, 
                                    nDBOperation);
        }
        public static void Delete(TransactionContext tc, FabricDeliveryOrder oFDO, int nDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricDeliveryOrder] %n,%s, %n, %n, %d, %n,%n, %n, %d, %n, %s, %n, %n, %n, %n, %n, %n",
                                        oFDO.FDOID,
                                    oFDO.DONo,
                                    oFDO.FDOTypeInInt,
                                    (int)oFDO.DOStatus,
                                    oFDO.DODate,
                                    oFDO.ContractorID,
                                    oFDO.BuyerID,
                                    oFDO.DeliveryZoneID,
                                    oFDO.ExpDeliveryDate,
                                    oFDO.CurrencyID,
                                    oFDO.Note,
                                    oFDO.BCPID,
                                    oFDO.MKTPersonID,
                                    oFDO.DeliveryFrom_BUID,
                                    oFDO.BUID,
                                    nUserId,
                                    nDBOperation);
        }
        public static IDataReader IUD_Log(TransactionContext tc, FabricDeliveryOrder oFDO, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricDeliveryOrderLog] %n,%s, %n, %n, %d, %n,%n, %n, %d, %n, %s, %n, %n, %n, %n,%b, %n, %n",
                                    oFDO.FDOID,
                                    oFDO.DONo,
                                    oFDO.FDOTypeInInt,
                                    (int)oFDO.DOStatus,
                                    oFDO.DODate,
                                    oFDO.ContractorID,
                                    oFDO.BuyerID,
                                    oFDO.DeliveryZoneID,
                                    oFDO.ExpDeliveryDate,
                                    oFDO.CurrencyID,
                                    oFDO.Note,
                                    oFDO.BCPID,
                                    oFDO.MKTPersonID,
                                    oFDO.DeliveryFrom_BUID,
                                    oFDO.BUID,
                                     oFDO.IsRevise,
                                    nUserId,
                                    nDBOperation);
        }

        public static IDataReader UpdateFDOStatus(TransactionContext tc, int nFDOID, int nStatus,string sDeliveryPoint, Int64 nUserId)
        {
            //return tc.ExecuteReader("EXEC [SP_FabricDeliveryOrderStatus] %n,%n,%s,%n", nFDOID, nStatus, sDeliveryPoint, nUserId);
            return tc.ExecuteReader("EXEC [SP_FabricDeliveryOrderStatus] %n,%n,%n", nFDOID, nStatus, nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFDOID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricDeliveryOrder WHERE FDOID=%n", nFDOID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader UpdateFinish(TransactionContext tc, int nFDOID, bool bIsFinish, Int64 nUserId)
        {
            tc.ExecuteNonQuery("UPDATE FabricDeliveryOrder SET IsFinish=%b WHERE FDOID=%n", bIsFinish, nFDOID);
            return tc.ExecuteReader("SELECT * FROM View_FabricDeliveryOrder WHERE FDOID=%n", nFDOID);
        }
        #endregion

    }
}
