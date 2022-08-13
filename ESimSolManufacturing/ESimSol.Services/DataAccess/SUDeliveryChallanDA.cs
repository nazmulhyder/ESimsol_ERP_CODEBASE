using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class SUDeliveryChallanDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, SUDeliveryChallan oSUDeliveryChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SUDeliveryChallan]"
                                    + "%n, %n, %s, %n, %d, %n, %n, %n, %n, %s, %s, %s, %s, %s, %s, %n, %n, %n, %n",
                                    oSUDeliveryChallan.SUDeliveryChallanID, oSUDeliveryChallan.SUDeliveryOrderID, oSUDeliveryChallan.ChallanNo, oSUDeliveryChallan.ChallanStatus, oSUDeliveryChallan.ChallanDate, oSUDeliveryChallan.BuyerID, oSUDeliveryChallan.DeliveryTo, oSUDeliveryChallan.CheckedBy, oSUDeliveryChallan.StoreID, oSUDeliveryChallan.VehicleNo, oSUDeliveryChallan.Remarks, oSUDeliveryChallan.DriverName, oSUDeliveryChallan.DriverContactNo, oSUDeliveryChallan.GatePassNo, oSUDeliveryChallan.StorePhoneNo, oSUDeliveryChallan.ApprovedBy, oSUDeliveryChallan.DeliveredBy, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, SUDeliveryChallan oSUDeliveryChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SUDeliveryChallan]"
                                    + "%n, %n, %s, %n, %d, %n, %n, %n, %n, %s, %s, %s, %s, %s, %s, %n, %n, %n, %n",
                                    oSUDeliveryChallan.SUDeliveryChallanID, oSUDeliveryChallan.SUDeliveryOrderID, oSUDeliveryChallan.ChallanNo, oSUDeliveryChallan.ChallanStatus, oSUDeliveryChallan.ChallanDate, oSUDeliveryChallan.BuyerID, oSUDeliveryChallan.DeliveryTo, oSUDeliveryChallan.CheckedBy, oSUDeliveryChallan.StoreID, oSUDeliveryChallan.VehicleNo, oSUDeliveryChallan.Remarks, oSUDeliveryChallan.DriverName, oSUDeliveryChallan.DriverContactNo, oSUDeliveryChallan.GatePassNo, oSUDeliveryChallan.StorePhoneNo, oSUDeliveryChallan.ApprovedBy, oSUDeliveryChallan.DeliveredBy, (int)eEnumDBOperation, nUserID);
        }

        public static IDataReader SUDeliveryChallanDisburse(TransactionContext tc, int nSUDeliveryChallanID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_SUDeliveryChallanDisburse]"
                                    + "%n, %n",
                                    nSUDeliveryChallanID, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SUDeliveryChallan WHERE SUDeliveryChallanID=%n", nID);
        }
        /// <summary>
        /// added by fahim0abir on date: 4 AUG 2015
        /// for geting list of challans of a specific SUDeliveryOrder
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="nSUDOID"></param>
        /// <returns></returns>
        public static IDataReader GetsBySUDeliveryOrder(TransactionContext tc,int nSUDOID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SUDeliveryChallan AS SUDC WHERE SUDC.SUDeliveryOrderID=" + nSUDOID);
        }
        /// <summary>
        /// added by fahim0abir on date: 30 JUL 2015
        /// for geting list of challans that have ChallanStatus=EnumDeliveryChallan.Approve(1) aka pending challan
        /// </summary>
        /// <returns></returns>
        public static IDataReader GetsPendingChallan(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SUDeliveryChallan AS SUDC WHERE SUDC.ChallanStatus=1");
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SUDeliveryChallan");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static void UpdateStatus(TransactionContext tc, int nSUDeliveryChallanID, int nNewStatus)
        {
            if (nNewStatus == 0)
            {
                tc.ExecuteNonQuery("UPDATE SUDeliveryChallan SET ChallanStatus = %n, ApprovedBy=0 WHERE SUDeliveryChallanID = %n", nNewStatus, nSUDeliveryChallanID);
            }
            else 
            {
                tc.ExecuteNonQuery("UPDATE SUDeliveryChallan SET ChallanStatus = %n WHERE SUDeliveryChallanID = %n", nNewStatus, nSUDeliveryChallanID);
            }
        }
        #endregion
    }
}
