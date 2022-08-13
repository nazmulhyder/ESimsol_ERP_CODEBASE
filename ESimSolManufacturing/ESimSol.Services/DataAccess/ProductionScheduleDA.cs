using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
   
    public class ProductionScheduleDA
    {
        public ProductionScheduleDA() { }

        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, ProductionSchedule oProductionSchedule, EnumDBOperation eEnumDBProductionSchedule, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionSchedule]"
                                    + "%n,%n,%n,%s,%n,%n,%n,%n,%b,%D,%D,%n,%n,%n,%b,%n,%b,%D,%D,%n,%n",
                                   oProductionSchedule.ProductionScheduleID, oProductionSchedule.BatchGroup, oProductionSchedule.ScheduleStatus, oProductionSchedule.ScheduleStability, oProductionSchedule.MachineID, oProductionSchedule.BatchNo, oProductionSchedule.LocationID, oProductionSchedule.ProductionQty, oProductionSchedule.ScheduleType, oProductionSchedule.StartTime, oProductionSchedule.EndTime, nUserId, (int)eEnumDBProductionSchedule, oProductionSchedule.ChangeGrid, oProductionSchedule.bIncreaseTime, oProductionSchedule.SwapScheduleID, oProductionSchedule.bEffectIncDec, oProductionSchedule.CheckTime, oProductionSchedule.IncDecTime, oProductionSchedule.IncDecMachineID, oProductionSchedule.BUID);
        }

                                                                                                                                                                                                                                          

        public static void Delete(TransactionContext tc, ProductionSchedule oProductionSchedule, EnumDBOperation eEnumDBProductionSchedule, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionSchedule]"
                                    + "%n,%n,%n,%s,%n,%n,%n,%n,%b,%D,%D,%n,%n,%n,%b,%n,%b,%D,%D,%n,%n",
                                   oProductionSchedule.ProductionScheduleID, oProductionSchedule.BatchGroup, oProductionSchedule.ScheduleStatus, oProductionSchedule.ScheduleStability, oProductionSchedule.MachineID, oProductionSchedule.BatchNo, oProductionSchedule.LocationID, oProductionSchedule.ProductionQty, oProductionSchedule.ScheduleType, oProductionSchedule.StartTime, oProductionSchedule.EndTime, nUserId, (int)eEnumDBProductionSchedule, oProductionSchedule.ChangeGrid, oProductionSchedule.bIncreaseTime, oProductionSchedule.SwapScheduleID, oProductionSchedule.bEffectIncDec, oProductionSchedule.CheckTime, oProductionSchedule.IncDecTime, oProductionSchedule.IncDecMachineID, oProductionSchedule.BUID);
        }

                                           

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            //return tc.ExecuteReader("SELECT * FROM View_ProductionSchedule Order By ProductionScheduleID DESC");
            return tc.ExecuteReader("SELECT * FROM View_ProductionSchedule where Activity='1'  order by StartTime ASC");
        }

        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sLocationIDs)
        {
            return tc.ExecuteReader(" SELECT * FROM View_ProductionSchedule  where (StartTime >=%D and  StartTime<%D) or (EndTime >=%D and  EndTime<%D) and Activity='1' and LocationID in (%q)  order by StartTime ASC", dStartDate, dEndDate, dStartDate, dEndDate, sLocationIDs);
        }

        public static IDataReader Gets(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sLocationIDs, string sDyeMachineIDs)
        {
            return tc.ExecuteReader(" SELECT * FROM View_ProductionSchedule  where Activity='1' and StartTime >=%D and  EndTime<%D and LocationID in (%q) and PSSID in (%q) order by StartTime ASC", dStartDate, dEndDate, sLocationIDs, sDyeMachineIDs);
        }

        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionSchedule WHERE ProductionScheduleID=%n and Activity='1'", nID);
        }

        public static int GetsMax(TransactionContext tc,string sSql)
        {
            object obj = tc.ExecuteScalar(sSql);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }

        public static IDataReader Refresh(TransactionContext tc, string sSql)
        {
            return tc.ExecuteReader(sSql);
        }

        public static void Update(TransactionContext tc, long nId1, long nId2, double ProductionQtyFirst, double ProductionQtySecond)
        {
            tc.ExecuteNonQuery("Update ProductionScheduleDetail set ProductionScheduleID=%n where  ProductionScheduleID=%n ", 0, nId1);
            tc.ExecuteNonQuery("Update ProductionScheduleDetail set ProductionScheduleID=%n where  ProductionScheduleID=%n ", nId1, nId2);
            tc.ExecuteNonQuery("Update ProductionScheduleDetail set ProductionScheduleID=%n where  ProductionScheduleID=%n ", nId2, 0);

            tc.ExecuteNonQuery("Update ProductionSchedule set ProductionQty=%n where  ProductionScheduleID=%n ", ProductionQtySecond, nId1);
            tc.ExecuteNonQuery("Update ProductionSchedule set ProductionQty=%n where  ProductionScheduleID=%n ", ProductionQtyFirst, nId2);
        }

        public static IDataReader GetsGroupBy(TransactionContext tc, string sSql)
        {
            return tc.ExecuteReader(sSql);
        }

        public static Double GetWaitingProductionQuantity(TransactionContext tc, string sSql)
        {

            object obj = tc.ExecuteScalar(sSql);
            if (obj == null) return 0;
            double nWaitingTotalQuantity = 0;
            nWaitingTotalQuantity = Convert.ToDouble(obj);
            return nWaitingTotalQuantity;
        }



        public static int GetUnpublishProductionSchedule(TransactionContext tc, string sSql)
        {

            object obj = tc.ExecuteScalar(sSql);
            if (obj == null) return 0;
            int nUnpublishProductionSchedule = 0;
            nUnpublishProductionSchedule = Convert.ToInt32(obj);
            return nUnpublishProductionSchedule;
        }
        #endregion


    }
}
