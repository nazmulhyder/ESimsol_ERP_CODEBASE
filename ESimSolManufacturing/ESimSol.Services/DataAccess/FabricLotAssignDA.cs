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
	public class FabricLotAssignDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FabricLotAssign oFabricLotAssign, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FabricLotAssign]"
									+"%n,%n,%n,%n,%n,%n,%n",
									oFabricLotAssign.FabricLotAssignID,oFabricLotAssign.LotID,oFabricLotAssign.FEOSDID,oFabricLotAssign.Qty,oFabricLotAssign.Balance,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FabricLotAssign oFabricLotAssign, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FabricLotAssign]"
									+"%n,%n,%n,%n,%n,%n,%n",
									oFabricLotAssign.FabricLotAssignID,oFabricLotAssign.LotID,oFabricLotAssign.FEOSDID,oFabricLotAssign.Qty,oFabricLotAssign.Balance,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FabricLotAssign WHERE FabricLotAssignID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM FabricLotAssign");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
