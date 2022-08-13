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
	public class FNProductionConsumptionDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FNProductionConsumption oFNProductionConsumption, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FNProductionConsumption]"
                                    + "%n,%n,%n, %n,%n,%n,%n,%n,%n,%n",
									oFNProductionConsumption.FNPConsumptionID,oFNProductionConsumption.FNProductionID, oFNProductionConsumption.FNPBatchID, oFNProductionConsumption.ProductID,oFNProductionConsumption.LotID,oFNProductionConsumption.Qty,oFNProductionConsumption.MUID, oFNProductionConsumption.FNRDetailID, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FNProductionConsumption oFNProductionConsumption, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FNProductionConsumption]"
                                    + "%n,%n,%n, %n,%n,%n,%n,%n,%n,%n",
                                    oFNProductionConsumption.FNPConsumptionID, oFNProductionConsumption.FNProductionID, oFNProductionConsumption.FNPBatchID, oFNProductionConsumption.ProductID, oFNProductionConsumption.LotID, oFNProductionConsumption.Qty, oFNProductionConsumption.MUID, oFNProductionConsumption.FNRDetailID, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FNProductionConsumption WHERE FNPConsumptionID=%n", nID);
		}
		public static IDataReader Gets(int id, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNProductionConsumption WHERE  FNProductionID = %n",id);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
