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

    public class ProductionExecutionPlanDetailBreakdownDA
    {
        public ProductionExecutionPlanDetailBreakdownDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionExecutionPlanDetailBreakdown oProductionExecutionPlanDetailBreakdown, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sProductionExecutionPlanDetailBreakdownIDs)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_ProductionExecutionPlanDetailBreakdown]"
                                    + "%n,%n,%d,%n,%n,%n,%n,%n,%n,%s", oProductionExecutionPlanDetailBreakdown.ProductionExecutionPlanDetailBreakdownID, oProductionExecutionPlanDetailBreakdown.ProductionExecutionPlanDetailID, oProductionExecutionPlanDetailBreakdown.WorkingDate, oProductionExecutionPlanDetailBreakdown.DailyProduction, oProductionExecutionPlanDetailBreakdown.RegularTime, oProductionExecutionPlanDetailBreakdown.OverTime,  oProductionExecutionPlanDetailBreakdown.EfficencyInParcent,  (int)eEnumDBOperation, nUserID, sProductionExecutionPlanDetailBreakdownIDs);
        }

        public static void Delete(TransactionContext tc, ProductionExecutionPlanDetailBreakdown oProductionExecutionPlanDetailBreakdown, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sProductionExecutionPlanDetailBreakdownIDs)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_ProductionExecutionPlanDetailBreakdown]"
                                    + "%n,%n,%d,%n,%n,%n,%n,%n,%n,%s", oProductionExecutionPlanDetailBreakdown.ProductionExecutionPlanDetailBreakdownID, oProductionExecutionPlanDetailBreakdown.ProductionExecutionPlanDetailID, oProductionExecutionPlanDetailBreakdown.WorkingDate, oProductionExecutionPlanDetailBreakdown.DailyProduction, oProductionExecutionPlanDetailBreakdown.RegularTime, oProductionExecutionPlanDetailBreakdown.OverTime, oProductionExecutionPlanDetailBreakdown.EfficencyInParcent, (int)eEnumDBOperation, nUserID, sProductionExecutionPlanDetailBreakdownIDs);
        }

        public static void Paste(TransactionContext tc, ProductionExecutionPlanDetailBreakdown oEPDB, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_PlanPaste]"
                                    + "%n,%d,%s,%n", oEPDB.PLineConfigureID, oEPDB.PlanDate, oEPDB.BrekDownIDs, nUserID);
        }
        

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ProductionExecutionPlanDetailBreakdown WHERE ProductionExecutionPlanDetailBreakdownID=%n", nID);
        }

        public static IDataReader Gets(int nProductionExecutionPlanDetailID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ProductionExecutionPlanDetailBreakdown where ProductionExecutionPlanDetailID =%n ", nProductionExecutionPlanDetailID);
        }
        public static IDataReader GetsByPEPPlanID(int nProductionExecutionPlanID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ProductionExecutionPlanDetailBreakdown where ProductionExecutionPlanDetailID IN (SELECT ProductionExecutionPlanDetailID FROM ProductionExecutionPlanDetail WHERE ProductionExecutionPlanID = %n) ", nProductionExecutionPlanID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }

}
