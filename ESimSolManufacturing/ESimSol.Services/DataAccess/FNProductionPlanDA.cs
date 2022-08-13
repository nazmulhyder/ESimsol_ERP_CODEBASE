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
	public class FNProductionPlanDA 
	{
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, FNProductionPlan oFNProductionPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNProductionPlan]"
                                   + "%n,%s,%n,%n,      %n,%D,%D,%n,        %D,%s,%b,       %n,%n,%n",
                                   oFNProductionPlan.FNPPID, oFNProductionPlan.PlanNo, oFNProductionPlan.FSCDID, oFNProductionPlan.FNMachineID, 
                                   oFNProductionPlan.Qty, oFNProductionPlan.StartTime, oFNProductionPlan.EndTime, oFNProductionPlan.FNTreatment, 
                                   oFNProductionPlan.ReceiveDate, oFNProductionPlan.Note, oFNProductionPlan.IsIncreaseTime, 
                                   (int)oFNProductionPlan.PlanStatus, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, FNProductionPlan oFNProductionPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNProductionPlan]"
                                   + "%n,%s,%n,%n,      %n,%D,%D,%n,        %D,%s,%b,       %n,%n,%n",
                                   oFNProductionPlan.FNPPID, oFNProductionPlan.PlanNo, oFNProductionPlan.FSCDID, oFNProductionPlan.FNMachineID,
                                   oFNProductionPlan.Qty, oFNProductionPlan.StartTime, oFNProductionPlan.EndTime, oFNProductionPlan.FNTreatment,
                                   oFNProductionPlan.ReceiveDate, oFNProductionPlan.Note, oFNProductionPlan.IsIncreaseTime,
                                   (int)oFNProductionPlan.PlanStatus, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNProductionPlan WHERE FNPPID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNProductionPlan");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
	}

}
