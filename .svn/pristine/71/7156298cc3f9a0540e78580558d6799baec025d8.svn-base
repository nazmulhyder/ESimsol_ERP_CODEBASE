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
    public class GUProductionOrderDA
    {
        public GUProductionOrderDA() { }
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, GUProductionOrder oGUProductionOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_GUProductionOrder]"
                                    + "%n, %s, %n, %n, %n, %n, %s, %s, %n, %n, %n, %d, %d, %s,%n,%n,%d, %n, %n",
                                    oGUProductionOrder.GUProductionOrderID, oGUProductionOrder.GUProductionOrderNo, oGUProductionOrder.OrderRecapID, 
                                    oGUProductionOrder.BuyerID, oGUProductionOrder.ProductID, oGUProductionOrder.FabricID, oGUProductionOrder.GG, oGUProductionOrder.Count,
                                    oGUProductionOrder.ProductionUnitID, oGUProductionOrder.MerchandiserID, (int)oGUProductionOrder.OrderStatus, oGUProductionOrder.OrderDate, oGUProductionOrder.FactoryShipmentDate, oGUProductionOrder.Note, oGUProductionOrder.BUID, oGUProductionOrder.ShipmentScheduleID, oGUProductionOrder.InputDate,  (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, GUProductionOrder oGUProductionOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_GUProductionOrder]"
                                    + "%n, %s, %n, %n, %n, %n, %s, %s, %n, %n, %n, %d, %d, %s,%n,%n,%d, %n, %n",
                                    oGUProductionOrder.GUProductionOrderID, oGUProductionOrder.GUProductionOrderNo, oGUProductionOrder.OrderRecapID,
                                    oGUProductionOrder.BuyerID, oGUProductionOrder.ProductID, oGUProductionOrder.FabricID, oGUProductionOrder.GG, oGUProductionOrder.Count,
                                    oGUProductionOrder.ProductionUnitID, oGUProductionOrder.MerchandiserID, (int)oGUProductionOrder.OrderStatus, oGUProductionOrder.OrderDate, oGUProductionOrder.FactoryShipmentDate, oGUProductionOrder.Note, oGUProductionOrder.BUID, oGUProductionOrder.ShipmentScheduleID, oGUProductionOrder.InputDate, (int)eEnumDBOperation, nUserID);
        }
        public static IDataReader ChangeStatus(TransactionContext tc, GUProductionOrder oGUProductionOrder, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_GUProductionOrderHistory]"
                                    + "%n, %n, %n, %n, %s, %n, %n", oGUProductionOrder.GUProductionOrderHistoryID, oGUProductionOrder.GUProductionOrderID, (int)oGUProductionOrder.OrderStatus, (int)oGUProductionOrder.ActionType, oGUProductionOrder.Note, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader SendToProducton(TransactionContext tc, int nGUProductionOrderID,  Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [Sp_SendToGUProduction]" + "%n, %n", nGUProductionOrderID, nUserID);
        }
        public static void UpdateToleranceWithStatus(TransactionContext tc, GUProductionOrder oGUProductionOrder)
        {
            tc.ExecuteNonQuery("Update GUProductionOrder SET ToleranceInPercent = %n , WindingStatus = %n WHERE GUProductionOrderID = %n",oGUProductionOrder.ToleranceInPercent, (int)oGUProductionOrder.WindingStatus, oGUProductionOrder.GUProductionOrderID);
        }
        #endregion
        #region Get & Exist Function
        public static IDataReader GetbyGUProductionOrderNo(TransactionContext tc, string  GUProductionOrderno)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionOrder WHERE GUProductionOrderNo=%s", GUProductionOrderno);
        }
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionOrder WHERE GUProductionOrderID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionOrder");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets_bySalorderID(TransactionContext tc, long nSalorderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionOrder WHERE OrderRecapID=%n", nSalorderID);
        }
        public static IDataReader Gets_byPOIDs(TransactionContext tc, string sPOIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_GUProductionOrder WHERE GUProductionOrderID IN (" + sPOIDs + ") Order By BuyerID");
        }
        public static IDataReader ProductionProgresReport(TransactionContext tc, string sRecapIDs)
        {
            return tc.ExecuteReader("EXEC[SP_ProductionProgressReport]" + "%s", sRecapIDs);
        }
        #endregion
    }
}
