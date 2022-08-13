using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class ShipmentScheduleDA
    {
        public ShipmentScheduleDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ShipmentSchedule oShipmentSchedule, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ShipmentSchedule]"
                                    + "%n,%n,%n,%d,%n,%d,%n,%n, %s, %n, %n",
                                    oShipmentSchedule.ShipmentScheduleID, oShipmentSchedule.OrderRecapID, oShipmentSchedule.CountryID,
                                    oShipmentSchedule.ShipmentDate, (int)oShipmentSchedule.CutOffType, oShipmentSchedule.CutOffDate,
                                    oShipmentSchedule.CutOffWeek, (int)oShipmentSchedule.ShipmentMode, oShipmentSchedule.Remarks,
                                    nUserId, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, ShipmentSchedule oShipmentSchedule, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ShipmentSchedule]"
                                    + "%n,%n,%n,%d,%n,%d,%n,%n, %s, %n, %n",
                                    oShipmentSchedule.ShipmentScheduleID, oShipmentSchedule.OrderRecapID, oShipmentSchedule.CountryID,
                                    oShipmentSchedule.ShipmentDate, (int)oShipmentSchedule.CutOffType, oShipmentSchedule.CutOffDate,
                                    oShipmentSchedule.CutOffWeek, (int)oShipmentSchedule.ShipmentMode, oShipmentSchedule.Remarks,
                                    nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentSchedule WHERE ShipmentScheduleID=%n", nID);
        }
        public static IDataReader GetByType(TransactionContext tc, int nCutOffType)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentSchedule WHERE CutOffType=%n", nCutOffType);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentSchedule");
        }
        public static IDataReader Gets(TransactionContext tc, int nORID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ShipmentSchedule WHERE OrderRecapID = "+nORID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}