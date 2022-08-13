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
	public class FabricPlanDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FabricPlan oFP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FabricPlan]"
                                    + "%n,  %n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n",
                                    oFP.FabricPlanID, oFP.FabricPlanOrderID, oFP.FabricID, oFP.RefID, (EnumFabricPlanRefType)oFP.RefType, oFP.ProductID, oFP.Value, oFP.SLNo, oFP.LabdipDetailID, (int)oFP.WarpWeftType, oFP.RGB, oFP.Color, oFP.TwistedGroup, oFP.EndsCount, nUserID, (int)eEnumDBOperation);
		}

        public static void Delete(TransactionContext tc, FabricPlan oFP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FabricPlan]"
                                     + "%n,  %n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%n,%n,%n,%n",
                                    oFP.FabricPlanID, oFP.FabricPlanOrderID,oFP.FabricID, oFP.RefID, (EnumFabricPlanRefType)oFP.RefType, oFP.ProductID, oFP.Value, oFP.SLNo, oFP.LabdipDetailID, (int)oFP.WarpWeftType, oFP.RGB, oFP.Color, oFP.TwistedGroup, oFP.EndsCount, nUserID, (int)eEnumDBOperation);
		}

       
        public static void UpdateYarn(TransactionContext tc, int id, int nProductID, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update FabricPlan SET ProductID =%n  WHERE FabricPlanID = %n", nProductID, id);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FabricPlan WHERE FabricPlanID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM FabricPlan");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static IDataReader MakeCombo(TransactionContext tc, string sFabricPlanID, int nFabricPlanOrderID, int nCombo, int nDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricPlan_Twisted] %s, %n, %n, %n, %n", sFabricPlanID, nFabricPlanOrderID, nCombo, nUserID, nDBOperation);
        }
        public static void UpdateSequence(TransactionContext tc, FabricPlan oFabricPlan)
        {
            tc.ExecuteNonQuery("Update FabricPlan SET SLNo = %n WHERE FabricPlanID = %n", oFabricPlan.SLNo, oFabricPlan.FabricPlanID);
        }

        public static IDataReader UpdateLDDetailID(TransactionContext tc, FabricPlan oFabricPlan)
        {
            string sSQL1 = SQLParser.MakeSQL("Update FabricPlan Set LabdipDetailID=%n WHERE FabricPlanID=%n", oFabricPlan.LabdipDetailID, oFabricPlan.FabricPlanID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FabricPlan WHERE FabricPlanID=%n", oFabricPlan.FabricPlanID);

        }

		#endregion
	}

}
