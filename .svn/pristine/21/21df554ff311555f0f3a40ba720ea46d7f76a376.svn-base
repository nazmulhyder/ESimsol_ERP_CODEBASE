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
	public class FNProductionBatchQualityDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FNProductionBatchQuality oFNProductionBatchQuality, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FNProductionBatchQuality]"
									+"%n,%n,%b,%s,%n,%n",
									oFNProductionBatchQuality.FNPBQualityID,oFNProductionBatchQuality.FNPBatchID,oFNProductionBatchQuality.IsOk,oFNProductionBatchQuality.Remark,nUserID, (int)eEnumDBOperation);
		}
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNProductionBatchQuality WHERE FNPBQualityID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNProductionBatchQuality");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
