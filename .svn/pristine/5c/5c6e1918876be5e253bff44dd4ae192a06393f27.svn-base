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
    public class FabricLoomPlanDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FabricLoomPlan oFabricLoomPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricLoomPlan]"
                                   + " %n,%n,%n,%n,	%n,%s,%D,%D, %b,%n,%n,%n,%n",
                                   oFabricLoomPlan.FLPID, oFabricLoomPlan.FSCDID, oFabricLoomPlan.FBID, oFabricLoomPlan.FMID, (int)oFabricLoomPlan.PlanStatus, oFabricLoomPlan.Note, oFabricLoomPlan.StartTime, oFabricLoomPlan.EndTime, oFabricLoomPlan.IsIncreaseTime, (int)oFabricLoomPlan.FabricPrograme, oFabricLoomPlan.ShiftID, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FabricLoomPlan oFabricLoomPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricLoomPlan]"
                                   + " %n,%n,%n,%n,	%n,%s,%D,%D, %b,%n,%n,%n,%n",
                                   oFabricLoomPlan.FLPID, oFabricLoomPlan.FSCDID, oFabricLoomPlan.FBID, oFabricLoomPlan.FMID, (int)oFabricLoomPlan.PlanStatus, oFabricLoomPlan.Note, oFabricLoomPlan.StartTime, oFabricLoomPlan.EndTime, oFabricLoomPlan.IsIncreaseTime, (int)oFabricLoomPlan.FabricPrograme, oFabricLoomPlan.ShiftID, nUserID, (int)eEnumDBOperation);
        }
        public static IDataReader Swap(TransactionContext tc, FabricLoomPlan oFabricLoomPlan, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricLoomPlan_Swap]"
                               + "%n,%n,%D,%D,%n",
                               oFabricLoomPlan.FLPID, oFabricLoomPlan.FMID,  oFabricLoomPlan.StartTime.ToString("dd MMM yyyy HH:mm"), oFabricLoomPlan.EndTime.ToString("dd MMM yyyy HH:mm"), nUserId);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricLoomPlan WHERE FLPID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricLoomPlan ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader UpdatePlanStatus(TransactionContext tc, FabricLoomPlan oFabricLoomPlan)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricLoomPlan Set PlanStatus=%n WHERE FLPID=%n", (int)oFabricLoomPlan.PlanStatus, oFabricLoomPlan.FLPID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FabricLoomPlan WHERE FLPID=%n", oFabricLoomPlan.FLPID);
        }
        #endregion
    }

}
