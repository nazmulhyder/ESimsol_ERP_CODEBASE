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
	public class FabricPlanningDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FabricPlanning oFP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FabricPlanning]"
                                    + "%n,%n,%n,%n,%b,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                    oFP.FabricPlanningID, oFP.FabricID, oFP.FSCDID, oFP.ProductID, oFP.IsWarp, oFP.RGB, oFP.Color, oFP.Value, oFP.RepeatNo, oFP.Count1, oFP.Count2, oFP.Count3, oFP.Count4, oFP.Count5, oFP.Count6, oFP.Count7, oFP.Count8, oFP.Count9, oFP.Count10, oFP.Count11, oFP.Count12, oFP.Count13, oFP.Count14,oFP.Count15, nUserID, (int)eEnumDBOperation);
		}

        public static void Delete(TransactionContext tc, FabricPlanning oFP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FabricPlanning]"
                                    + "%n,%n,%n,%n,%b,%s,%s,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
                                    oFP.FabricPlanningID, oFP.FabricID, oFP.FSCDID, oFP.ProductID, oFP.IsWarp, oFP.RGB, oFP.Color, oFP.Value, oFP.RepeatNo, oFP.Count1, oFP.Count2, oFP.Count3, oFP.Count4, oFP.Count5, oFP.Count6, oFP.Count7, oFP.Count8, oFP.Count9, oFP.Count10, oFP.Count11, oFP.Count12, oFP.Count13, oFP.Count14, oFP.Count15, nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FabricPlanning WHERE FabricPlanningID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM FabricPlanning");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static IDataReader MakeCombo(TransactionContext tc, string sFabricPlanningID, int nFabricID, int nCombo, int nDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricPlanning_Combo] %s, %n, %n, %n, %n", sFabricPlanningID, nFabricID, nCombo, nUserID, nDBOperation);
        }
		#endregion
	}

}
