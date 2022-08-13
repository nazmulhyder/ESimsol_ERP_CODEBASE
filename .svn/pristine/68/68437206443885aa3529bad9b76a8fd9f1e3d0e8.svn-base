using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricDeliveryChallanDA
    {
        public FabricDeliveryChallanDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricDeliveryChallan oFDC, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricDeliveryChallan]  %n,%n,%s,%D,%s,%s,%s,%s,%n, %n,%s,%b, %s,%s,%s,  %n,%n",
            oFDC.FDCID, oFDC.FDOID, oFDC.ChallanNo, oFDC.IssueDate, oFDC.DeliveryPoint, oFDC.VehicleNo, oFDC.DriverName, oFDC.Note, oFDC.WorkingUnitID, oFDC.VehicleTypeID, oFDC.DriverMobile, oFDC.IsSample, oFDC.DeliveryMan, oFDC.GatePassNo, oFDC.CPDeliveryMan, nUserId, nDBOperation);
        }
        public static void Delete(TransactionContext tc, FabricDeliveryChallan oFDC, int nDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricDeliveryChallan]  %n,%n,%s,%D,%s,%s,%s,%s,%n, %n,%s,%b, %s,%s,%s,  %n,%n",
            oFDC.FDCID, oFDC.FDOID, oFDC.ChallanNo, oFDC.IssueDate, oFDC.DeliveryPoint, oFDC.VehicleNo, oFDC.DriverName, oFDC.Note, oFDC.WorkingUnitID, oFDC.VehicleTypeID, oFDC.DriverMobile, oFDC.IsSample, oFDC.DeliveryMan, oFDC.GatePassNo, oFDC.CPDeliveryMan, nUserId, nDBOperation);
        }
        public static IDataReader FDCDisburse(TransactionContext tc, int nFDCID , int nTriggerParentType, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_FabricDeliveryChallanDisburse] %n, %n, %n", nFDCID, nTriggerParentType, nUserId);
        }

        
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFDCID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricDeliveryChallan WHERE FDCID=%n", nFDCID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

    }
}
