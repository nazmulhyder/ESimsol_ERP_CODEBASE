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
	public class FabricPlanCountDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FabricPlanCount oFabricPlanCount, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FabricPlanCount]"
									+"%n,%n,%n,%s,%n,%n,%n",
									oFabricPlanCount.FabricPlanCountID,oFabricPlanCount.FabricPlanningID,oFabricPlanCount.SLNo,oFabricPlanCount.Repeat,oFabricPlanCount.RepeatCount,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FabricPlanCount oFabricPlanCount, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FabricPlanCount]"
									+"%n,%n,%n,%s,%n,%n,%n",
									oFabricPlanCount.FabricPlanCountID,oFabricPlanCount.FabricPlanningID,oFabricPlanCount.SLNo,oFabricPlanCount.Repeat,oFabricPlanCount.RepeatCount,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FabricPlanCount WHERE FabricPlanCountID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FabricPlanCount");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
