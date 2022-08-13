using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricDeliveryScheduleDA
    {
        public FabricDeliveryScheduleDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricDeliverySchedule oFDSchedule, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricDeliverySchedule]"
                                    + "%n, %n, %n,%n,  %D, %s, %s,%n,%b, %n, %n",
                                    oFDSchedule.FabricDeliveryScheduleID, oFDSchedule.DeliveryOrderNameID, oFDSchedule.FabricSalesContractID,oFDSchedule.Qty, oFDSchedule.DeliveryDate, oFDSchedule.Note, oFDSchedule.DeliveryAddress,oFDSchedule.DeliveryToID,oFDSchedule.IsOwn, nUserID ,(int)eEnumDBOperation );
        }

        public static void Delete(TransactionContext tc, FabricDeliverySchedule oFDSchedule, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricDeliverySchedule]"
                                    + "%n, %n, %n,%n,  %D, %s, %s,%n,%b,%n, %n",
                                    oFDSchedule.FabricDeliveryScheduleID, oFDSchedule.DeliveryOrderNameID, oFDSchedule.FabricSalesContractID, oFDSchedule.Qty, oFDSchedule.DeliveryDate, oFDSchedule.Note, oFDSchedule.DeliveryAddress,oFDSchedule.DeliveryToID,oFDSchedule.IsOwn, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricDeliverySchedule WHERE FabricDeliveryScheduleID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricDeliverySchedule ");
        }
        public static IDataReader GetsFSCID(TransactionContext tc, int nFSCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricDeliverySchedule WHERE FabricSalesContractID=%n order by DeliveryDate ASC", nFSCID);
        }
        public static IDataReader GetsFSCIDLog(TransactionContext tc, int nFSCID)
        {
            return tc.ExecuteReader("SELECT * FROM FabricDeliveryScheduleLog WHERE FabricSalesContractLogID=%n order by DeliveryDate ASC", nFSCID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
      
        #endregion
    }
}