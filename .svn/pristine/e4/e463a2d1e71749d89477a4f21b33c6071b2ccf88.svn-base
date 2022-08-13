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

    public class ProductionExecutionPlanDetailDA
    {
        public ProductionExecutionPlanDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionExecutionPlanDetail oProductionExecutionPlanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sProductionExecutionPlanDetailIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ProductionExecutionPlanDetail]"
                                    + "%n,%n, %d,%d,%n,%n,%n,%n,%n,%n, %n,%n,%s", 
                                    oProductionExecutionPlanDetail.ProductionExecutionPlanDetailID, oProductionExecutionPlanDetail.ProductionExecutionPlanID, 
                                    oProductionExecutionPlanDetail.StartDate, oProductionExecutionPlanDetail.EndDate, oProductionExecutionPlanDetail.WorkingDays, oProductionExecutionPlanDetail.MachineQty, oProductionExecutionPlanDetail.ProductionPerHour, oProductionExecutionPlanDetail.AvgDailyProduction, oProductionExecutionPlanDetail.TotalProduction, oProductionExecutionPlanDetail.PLineConfigureID, (int)eEnumDBOperation, nUserID, sProductionExecutionPlanDetailIDs);
        }

        public static void Delete(TransactionContext tc, ProductionExecutionPlanDetail oProductionExecutionPlanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sProductionExecutionPlanDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ProductionExecutionPlanDetail]"
                                    + "%n,%n, %d,%d,%n,%n,%n,%n,%n,%n, %n,%n,%s",
                                    oProductionExecutionPlanDetail.ProductionExecutionPlanDetailID, oProductionExecutionPlanDetail.ProductionExecutionPlanID,
                                    oProductionExecutionPlanDetail.StartDate, oProductionExecutionPlanDetail.EndDate, oProductionExecutionPlanDetail.WorkingDays, oProductionExecutionPlanDetail.MachineQty, oProductionExecutionPlanDetail.ProductionPerHour, oProductionExecutionPlanDetail.AvgDailyProduction, oProductionExecutionPlanDetail.TotalProduction, oProductionExecutionPlanDetail.PLineConfigureID, (int)eEnumDBOperation, nUserID, sProductionExecutionPlanDetailIDs);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionExecutionPlanDetail WHERE ProductionExecutionPlanDetailID=%n", nID);
        }

        public static IDataReader Gets(int nProductionExecutionPlanID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionExecutionPlanDetail where ProductionExecutionPlanID =%n ", nProductionExecutionPlanID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
 
}
