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
    public class FabricSizingPlanDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricSizingPlan oFabricSizingPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSizingPlan]"
                                   + " %n,%n,%n,	%n,%d,%n,%s,%D,%D,%b,%s,%n,%n,%n,%n,%n,%n",
                                   oFabricSizingPlan.FabricSizingPlanID, oFabricSizingPlan.FSCDID, oFabricSizingPlan.FEOSID, oFabricSizingPlan.FMID, oFabricSizingPlan.FinishDate,
                                   oFabricSizingPlan.Status, oFabricSizingPlan.Note, oFabricSizingPlan.StartTime, oFabricSizingPlan.EndTime, oFabricSizingPlan.IsIncreaseTime, oFabricSizingPlan.WarpBeam, oFabricSizingPlan.Qty, oFabricSizingPlan.FabricWarpPlanID, oFabricSizingPlan.LFID, oFabricSizingPlan.Priority, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricSizingPlan oFabricSizingPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricSizingPlan]"
                                   + "%n,%n,%n,	%n,%d,%n,%s,%D,%D,%b,%s,%n,%n,%n,%n,%n,%n",
                                   oFabricSizingPlan.FabricSizingPlanID, oFabricSizingPlan.FSCDID, oFabricSizingPlan.FEOSID, oFabricSizingPlan.FMID, oFabricSizingPlan.FinishDate,
                                   oFabricSizingPlan.Status, oFabricSizingPlan.Note, NullHandler.GetNullValue(oFabricSizingPlan.StartTime),NullHandler.GetNullValue( oFabricSizingPlan.EndTime), oFabricSizingPlan.IsIncreaseTime, oFabricSizingPlan.WarpBeam, oFabricSizingPlan.Qty, oFabricSizingPlan.FabricWarpPlanID, oFabricSizingPlan.LFID, oFabricSizingPlan.Priority, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader Swap(TransactionContext tc, FabricSizingPlan oFabricSizingPlan, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricSizingPlan_Swap]"
                               + "%n,%n,%D,%D,%n",
                               oFabricSizingPlan.FabricSizingPlanID, oFabricSizingPlan.FMID, oFabricSizingPlan.StartTime.ToString("dd MMM yyyy HH:mm"), oFabricSizingPlan.EndTime.ToString("dd MMM yyyy HH:mm"), nUserId);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSizingPlan WHERE FabricSizingPlanID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricSizingPlan ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader UpdatePlanStatus(TransactionContext tc, FabricSizingPlan oFabricSizingPlan, Int64 nUserId)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricSizingPlan Set PlanStatus=%n, Priority=%n,LastUpdateBy=%n, LastUpdateDateTime=getdate() WHERE FabricSizingPlanID=%n", (int)oFabricSizingPlan.PlanStatus, oFabricSizingPlan.Priority,nUserId, oFabricSizingPlan.FabricSizingPlanID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FabricSizingPlan WHERE FabricSizingPlanID=%n", oFabricSizingPlan.FabricSizingPlanID);
        }
        public static IDataReader UpdateWaterQty(TransactionContext tc, FabricSizingPlan oFabricSizingPlan)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricSizingPlan Set WaterQty=%n, WarpBeam=%s WHERE FabricSizingPlanID=%n", oFabricSizingPlan.WaterQty, oFabricSizingPlan.WarpBeam, oFabricSizingPlan.FabricSizingPlanID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FabricSizingPlan WHERE FabricSizingPlanID=%n", oFabricSizingPlan.FabricSizingPlanID);
        }

        public static int GetsProStatus(TransactionContext tc, int nFSPID, EnumWeavingProcess eWUPState)
        {
            object obj = tc.ExecuteScalar("SELECT count(*) FROM FabricBatchProduction where WeavingProcess=%n and FBID in (Select FBID from FabricBatch where FWPDID in (Select FabricWarpPlanID from FabricSizingPlan where FabricSizingPlanID=%n))", eWUPState, nFSPID);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }
        #endregion
    }

}
