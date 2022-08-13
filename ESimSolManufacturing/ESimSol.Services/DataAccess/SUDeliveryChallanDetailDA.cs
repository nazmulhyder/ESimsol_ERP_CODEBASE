using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class SUDeliveryChallanDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SUDeliveryChallanDetail oSUDeliveryChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sSUDeliveryChallanDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SUDeliveryChallanDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %s",
                                    oSUDeliveryChallanDetail.SUDeliveryChallanDetailID, oSUDeliveryChallanDetail.SUDeliveryChallanID, oSUDeliveryChallanDetail.SUDeliveryOrderDetailID, oSUDeliveryChallanDetail.ProductID, oSUDeliveryChallanDetail.LotID, oSUDeliveryChallanDetail.MUnitID, oSUDeliveryChallanDetail.Qty, oSUDeliveryChallanDetail.SUDeliveryProgramID, oSUDeliveryChallanDetail.Bags, (int)eEnumDBOperation, nUserID, sSUDeliveryChallanDetailIDs, oSUDeliveryChallanDetail.DCDRemark);
        }

        public static void Delete(TransactionContext tc, SUDeliveryChallanDetail oSUDeliveryChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sSUDeliveryChallanDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SUDeliveryChallanDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %s",
                                    oSUDeliveryChallanDetail.SUDeliveryChallanDetailID, oSUDeliveryChallanDetail.SUDeliveryChallanID, oSUDeliveryChallanDetail.SUDeliveryOrderDetailID, oSUDeliveryChallanDetail.ProductID, oSUDeliveryChallanDetail.LotID, oSUDeliveryChallanDetail.MUnitID, oSUDeliveryChallanDetail.Qty, oSUDeliveryChallanDetail.SUDeliveryProgramID, oSUDeliveryChallanDetail.Bags, (int)eEnumDBOperation, nUserID, sSUDeliveryChallanDetailIDs, oSUDeliveryChallanDetail.DCDRemark);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SUDeliveryChallanDetail WHERE SUDeliveryChallanDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nSUDeliveryChallanID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SUDeliveryChallanDetail WHERE SUDeliveryChallanID=%n", nSUDeliveryChallanID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsBySUDeliveryChallan(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SUDeliveryChallanDetail WHERE SUDeliveryChallanID=%n", nID);
        }
        #endregion
    }
}
