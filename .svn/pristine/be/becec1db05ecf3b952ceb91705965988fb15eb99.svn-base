using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class ShipmentScheduleDetailDA
    {
        public ShipmentScheduleDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ShipmentScheduleDetail oShipmentScheduleDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sShipmentScheduleDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ShipmentScheduleDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n,%s",
                                    oShipmentScheduleDetail.ShipmentScheduleDetailID, oShipmentScheduleDetail.ShipmentScheduleID, oShipmentScheduleDetail.ColorID, oShipmentScheduleDetail.SizeID, oShipmentScheduleDetail.Qty, (int)eEnumDBOperation, nUserID, sShipmentScheduleDetailIDs);
        }

        public static void Delete(TransactionContext tc, ShipmentScheduleDetail oShipmentScheduleDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sShipmentScheduleDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ShipmentScheduleDetail]"
                                    + "%n, %n, %n, %n, %n, %n, %n,%s",
                                    oShipmentScheduleDetail.ShipmentScheduleDetailID, oShipmentScheduleDetail.ShipmentScheduleID, oShipmentScheduleDetail.ColorID, oShipmentScheduleDetail.SizeID, oShipmentScheduleDetail.Qty, (int)eEnumDBOperation, nUserID, sShipmentScheduleDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentScheduleDetail WHERE ShipmentScheduleDetailID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentScheduleDetail");
        }

        public static IDataReader GetsByShipmentSchedule(int nid, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentScheduleDetail WHERE ShipmentScheduleID=%n", nid);
        }

        public static IDataReader Gets(int nid, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentScheduleDetail WhERE ShipmentScheduleID = %n", nid);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
