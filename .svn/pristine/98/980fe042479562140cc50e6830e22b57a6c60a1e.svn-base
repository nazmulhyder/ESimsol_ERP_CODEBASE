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
	public class FabricBatchSizingSolutionDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FabricBatchSizingSolution oFabricBatchSizingSolution, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchSizingSolution]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
									oFabricBatchSizingSolution.FBID,oFabricBatchSizingSolution.WaterQty,oFabricBatchSizingSolution.Dry,oFabricBatchSizingSolution.Wet,oFabricBatchSizingSolution.RF,oFabricBatchSizingSolution.Viscosity,oFabricBatchSizingSolution.FinalVolume,oFabricBatchSizingSolution.RestQty,oFabricBatchSizingSolution.PreviousRestQty, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FabricBatchSizingSolution oFabricBatchSizingSolution, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchSizingSolution]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n",
									oFabricBatchSizingSolution.FBID,oFabricBatchSizingSolution.WaterQty,oFabricBatchSizingSolution.Dry,oFabricBatchSizingSolution.Wet,oFabricBatchSizingSolution.RF,oFabricBatchSizingSolution.Viscosity,oFabricBatchSizingSolution.FinalVolume,oFabricBatchSizingSolution.RestQty,oFabricBatchSizingSolution.PreviousRestQty,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM FabricBatchSizingSolution WHERE FBID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM FabricBatchSizingSolution");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
