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

    public class ProductionExecutionPlanDA
    {
        public ProductionExecutionPlanDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionExecutionPlan oProductionExecutionPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionExecutionPlan]"
                                    + "%n, %s,%n,  %d,%d,%n,%s,%n,%n,%n,%n",
                                    oProductionExecutionPlan.ProductionExecutionPlanID, oProductionExecutionPlan.RefNo, oProductionExecutionPlan.OrderRecapID, 
                                    oProductionExecutionPlan.PlanDate, oProductionExecutionPlan.ShipmentDate, oProductionExecutionPlan.PlanExecutionQty, oProductionExecutionPlan.Note, (int)oProductionExecutionPlan.PlanStatus, oProductionExecutionPlan.SMV, (int)eEnumDBOperation, nUserID);
        }

        public static void Delete(TransactionContext tc, ProductionExecutionPlan oProductionExecutionPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionExecutionPlan]"
                                    + "%n, %s,%n,  %d,%d,%n,%s,%n,%n,%n,%n",
                                    oProductionExecutionPlan.ProductionExecutionPlanID, oProductionExecutionPlan.RefNo, oProductionExecutionPlan.OrderRecapID,
                                    oProductionExecutionPlan.PlanDate, oProductionExecutionPlan.ShipmentDate, oProductionExecutionPlan.PlanExecutionQty, oProductionExecutionPlan.Note, (int)oProductionExecutionPlan.PlanStatus, oProductionExecutionPlan.SMV, (int)eEnumDBOperation, nUserID);
        }


        public static IDataReader AcceptRevise(TransactionContext tc, ProductionExecutionPlan oProductionExecutionPlan, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_AcceptProductionExecutionPlanRevise]"
                                    + "%n, %s,%n,%d,%d,%n,%s,%n,%n",
                                    oProductionExecutionPlan.ProductionExecutionPlanID, oProductionExecutionPlan.RefNo, oProductionExecutionPlan.OrderRecapID,
                                    oProductionExecutionPlan.PlanDate, oProductionExecutionPlan.ShipmentDate, oProductionExecutionPlan.PlanExecutionQty, oProductionExecutionPlan.Note, (int)oProductionExecutionPlan.PlanStatus,  nUserID);
        }


        public static void Approve(TransactionContext tc, int id, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update ProductionExecutionPlan SET ApproveBy = %n, PlanStatus=%n  WHERE ProductionExecutionPlanID = %n", nUserID, (int)EnumPlanStatus.Approved,  id);
        }
        public static void RequestForRevise(TransactionContext tc, int id, Int64 nUserID)
        {
            tc.ExecuteNonQuery("Update ProductionExecutionPlan SET ApproveBy = %n, PlanStatus=%n  WHERE ProductionExecutionPlanID = %n", nUserID, (int)EnumPlanStatus.Requst_For_Revise, id);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionExecutionPlan WHERE ProductionExecutionPlanID=%n", nID);
        }
        public static IDataReader GetByOrderRecap(TransactionContext tc, long nOrderRecapID)
        {
            return tc.ExecuteReader("SELECT TOP 1 * FROM View_ProductionExecutionPlan WHERE OrderRecapID=%n", nOrderRecapID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionExecutionPlan ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }  

}
