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
    public class FabricWarpPlanDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricWarpPlan oFabricWarpPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricWarpPlan]"
                                   + " %n,%n,%n,%n,	%n,%d,%n,%s,%D,%D,%b,%s,%n,%n,%n,%n",
                                   oFabricWarpPlan.FabricWarpPlanID, oFabricWarpPlan.FSCDID, oFabricWarpPlan.FEOSID, oFabricWarpPlan.FMID, oFabricWarpPlan.Qty, oFabricWarpPlan.FinishDate,
                                   oFabricWarpPlan.Status, oFabricWarpPlan.Note, oFabricWarpPlan.StartTime, oFabricWarpPlan.EndTime, oFabricWarpPlan.IsIncreaseTime, oFabricWarpPlan.WarpBeam, oFabricWarpPlan.Priority, oFabricWarpPlan.PlanType, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricWarpPlan oFabricWarpPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricWarpPlan]"
                                   + " %n,%n,%n,%n,	%n,%d,%n,%s,%D,%D,%b,%s,%n,%n,%n,%n",
                                   oFabricWarpPlan.FabricWarpPlanID, oFabricWarpPlan.FSCDID, oFabricWarpPlan.FEOSID, oFabricWarpPlan.FMID, oFabricWarpPlan.Qty, oFabricWarpPlan.FinishDate,
                                   oFabricWarpPlan.Status, oFabricWarpPlan.Note, oFabricWarpPlan.StartTime, oFabricWarpPlan.EndTime, oFabricWarpPlan.IsIncreaseTime, oFabricWarpPlan.WarpBeam, oFabricWarpPlan.Priority, oFabricWarpPlan.PlanType, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader Swap(TransactionContext tc, FabricWarpPlan oFabricWarpPlan, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricWarpPlan_Swap]"
                               + "%n,%n,%D,%D,%n",
                               oFabricWarpPlan.FabricWarpPlanID, oFabricWarpPlan.FMID,  oFabricWarpPlan.StartTime.ToString("dd MMM yyyy HH:mm"), oFabricWarpPlan.EndTime.ToString("dd MMM yyyy HH:mm"), nUserId);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricWarpPlan WHERE FabricWarpPlanID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricWarpPlan ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader UpdatePlanStatus(TransactionContext tc, FabricWarpPlan oFabricWarpPlan, Int64 nUserId)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricWarpPlan Set PlanStatus=%n, Priority=%n,LastUpdateBy=%n, LastUpdateDateTime=getdate() WHERE FabricWarpPlanID=%n", (int)oFabricWarpPlan.PlanStatus, oFabricWarpPlan.Priority,nUserId, oFabricWarpPlan.FabricWarpPlanID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FabricWarpPlan WHERE FabricWarpPlanID=%n", oFabricWarpPlan.FabricWarpPlanID);
        }

        public static int GetsProStatus(TransactionContext tc, int nFWDID, EnumWeavingProcess eWUPState)
        {
            object obj = tc.ExecuteScalar("SELECT count(*) FROM FabricBatchProduction where  FBID in (Select FBID from FabricBatch where FWPDID=%n )",   nFWDID);
            if (obj == null) return 0;
            int aaaa = 0;
            aaaa = Convert.ToInt32(obj);
            return aaaa;
        }


        #endregion
    }

}
