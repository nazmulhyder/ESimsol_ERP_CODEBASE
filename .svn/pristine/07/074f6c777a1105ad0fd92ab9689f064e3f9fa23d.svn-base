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
    public class ShipmentDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, Shipment oShipment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Shipment]"
                                   + "%n,%n,%s,%n,%n,%d,   %n,%s,%s,%s,%s,     %s,%s,%s,%n,%n,     %s, %n,%n",
                                   oShipment.ShipmentID, oShipment.BUID, oShipment.ChallanNo, oShipment.BuyerID, oShipment.StoreID, oShipment.ShipmentDate,
                                   oShipment.ShipmentMode, oShipment.TruckNo, oShipment.DriverName, oShipment.DriverMobileNo, oShipment.Depo,
                                   oShipment.Escord, oShipment.FactoryName, oShipment.SecurityLock, oShipment.EmptyCTNQty, oShipment.GumTapeQty,
                                   oShipment.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Shipment oShipment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Shipment]"
                                   + "%n,%n,%s,%n,%n,%d,   %n,%s,%s,%s,%s,     %s,%s,%s,%n,%n,     %s, %n,%n",
                                   oShipment.ShipmentID, oShipment.BUID, oShipment.ChallanNo, oShipment.BuyerID, oShipment.StoreID, oShipment.ShipmentDate,
                                   oShipment.ShipmentMode, oShipment.TruckNo, oShipment.DriverName, oShipment.DriverMobileNo, oShipment.Depo,
                                   oShipment.Escord, oShipment.FactoryName, oShipment.SecurityLock, oShipment.EmptyCTNQty, oShipment.GumTapeQty,
                                   oShipment.Remarks, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Shipment WHERE ShipmentID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Shipment ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Approve(TransactionContext tc, Shipment oShipment, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_CommitShipment]" + "%n,%n", oShipment.ShipmentID, nUserID);
        }

        #endregion
    }

}
