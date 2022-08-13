using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class DeliveryChallanDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DeliveryChallanDetail oDeliveryChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DeliveryChallanDetail]"
                                   + "%n,%n,%n,%n, %n,%n,%n,%n,%n,%s,%s, %n,%n,%s",
                                   oDeliveryChallanDetail.DeliveryChallanDetailID, oDeliveryChallanDetail.DeliveryChallanID, oDeliveryChallanDetail.DODetailID, oDeliveryChallanDetail.PTUUnit2DistributionID,  oDeliveryChallanDetail.LotID, oDeliveryChallanDetail.ProductID, oDeliveryChallanDetail.MUnitID, oDeliveryChallanDetail.Qty, oDeliveryChallanDetail.BagQty, oDeliveryChallanDetail.Note,oDeliveryChallanDetail.StyleNo,  nUserID, (int)eEnumDBOperation, sDetailIDs);
        }

        public static void Delete(TransactionContext tc, DeliveryChallanDetail oDeliveryChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DeliveryChallanDetail]"
                                   + "%n,%n,%n,%n, %n,%n,%n,%n,%n,%s,%s, %n,%n,%s",
                                   oDeliveryChallanDetail.DeliveryChallanDetailID, oDeliveryChallanDetail.DeliveryChallanID, oDeliveryChallanDetail.DODetailID, oDeliveryChallanDetail.PTUUnit2DistributionID, oDeliveryChallanDetail.LotID, oDeliveryChallanDetail.ProductID, oDeliveryChallanDetail.MUnitID, oDeliveryChallanDetail.Qty, oDeliveryChallanDetail.BagQty, oDeliveryChallanDetail.Note, oDeliveryChallanDetail.StyleNo, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }
        public static void UpdateVehicleTime(TransactionContext tc, DeliveryChallan oDeliveryChallan, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update DeliveryChallan SET VehicleDateTime = %D WHERE DeliveryChallanID =%n",oDeliveryChallan.VehicleDateTime,oDeliveryChallan.DeliveryChallanID);
        }
        //
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DeliveryChallanDetail WHERE DeliveryChallanDetailID=%n", nID);
        }
        public static IDataReader Gets(int nDOID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DeliveryChallanDetail WHERE DeliveryChallanID = %n Order By ProductID", nDOID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }

}
