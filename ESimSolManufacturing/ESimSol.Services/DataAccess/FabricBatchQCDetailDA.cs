using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSol.Services.DataAccess
{

    public class FabricBatchQCDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchQCDetail oFabricBatchQCDetail, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchQCDetail]"
                                    + "%n,%n,%n,%s,%n,%s,%n,%n,%n,%n,%n",
                                    oFabricBatchQCDetail.FBQCDetailID, oFabricBatchQCDetail.FBQCID, (int)oFabricBatchQCDetail.Grade, oFabricBatchQCDetail.LotNo, oFabricBatchQCDetail.Qty, oFabricBatchQCDetail.Remark, oFabricBatchQCDetail.ShiftID, oFabricBatchQCDetail.FabricQCGradeID, oFabricBatchQCDetail.Width, nUserID, nEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchQCDetail oFabricBatchQCDetail, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchQCDetail]"
                                    + "%n,%n,%n,%s,%n,%s,%n, %n,%n,%n,%n",
                                    oFabricBatchQCDetail.FBQCDetailID, oFabricBatchQCDetail.FBQCID, (int)oFabricBatchQCDetail.Grade, oFabricBatchQCDetail.LotNo, oFabricBatchQCDetail.Qty, oFabricBatchQCDetail.Remark, oFabricBatchQCDetail.ShiftID, oFabricBatchQCDetail.FabricQCGradeID, oFabricBatchQCDetail.Width, nUserID, nEnumDBOperation);
        }
        public static IDataReader ReceiveInDelivery(TransactionContext tc, FabricBatchQCDetail oFabricBatchQCDetail, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchQCLot]"
                                    + "%n,%s,%n,%D,%s,%n", oFabricBatchQCDetail.FBQCID,oFabricBatchQCDetail.FBQCDetailIDs, oFabricBatchQCDetail.WorkingUnitID, oFabricBatchQCDetail.StoreRcvDate,oFabricBatchQCDetail.Remark, nUserID);
        }
        public static void Lock(TransactionContext tc, FabricBatchQCDetail oFabricBatchQCDetail, Int64 nUserID)
        {

            tc.ExecuteNonQuery("Update FabricBatchQCDetail SET   DeliveryBy = %n , ProDate=%D, DeliveryDate=%D WHERE FBQCDetailID = %n", nUserID, oFabricBatchQCDetail.ProDate, oFabricBatchQCDetail.DeliveryDate, oFabricBatchQCDetail.FBQCDetailID);
        }
        public static void LockFabricBatchQCDetail(TransactionContext tc, FabricBatchQCDetail oFabricBatchQCDetail, Int64 nUserID)
        {
            //string sFBQCDetailID = string.Join(",", oFabricBatchQCDetail.FabricBatchQCDetails.Select(o => o.FBQCDetailID).Distinct());

            tc.ExecuteNonQuery("Update FabricBatchQCDetail SET DeliveryBy = %n ,ProDate=%D,  DeliveryDate=%D WHERE FBQCDetailID IN (" + oFabricBatchQCDetail.FBQCDetailIDs + ")", nUserID, oFabricBatchQCDetail.ProDate, oFabricBatchQCDetail.DeliveryDate);
        }

        #endregion


        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nFBQCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchQCDetail WHERE FBQCID=%n ", nFBQCID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Get(TransactionContext tc, int nFBQCDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchQCDetail WHERE FBQCDetailID=%n ", nFBQCDetailID);
        }

        #endregion
    }
    
   
}
