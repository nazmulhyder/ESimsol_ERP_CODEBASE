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
	public class RMConsumptionDetailDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, RMConsumptionDetail oRMConsumptionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sRMConsumptionDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_RMConsumptionDetail]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s",
                                    oRMConsumptionDetail.RMConsumptionDetailID, oRMConsumptionDetail.RMConsumptionID, oRMConsumptionDetail.ITransactionID, oRMConsumptionDetail.ProductID, oRMConsumptionDetail.LotID, oRMConsumptionDetail.WUID, oRMConsumptionDetail.MUnitID, oRMConsumptionDetail.Qty, oRMConsumptionDetail.UnitPrice, oRMConsumptionDetail.Amount, nUserID, (int)eEnumDBOperation, sRMConsumptionDetailIDs);
		}

        public static void Delete(TransactionContext tc, RMConsumptionDetail oRMConsumptionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sRMConsumptionDetailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_RMConsumptionDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s",
                                    oRMConsumptionDetail.RMConsumptionDetailID, oRMConsumptionDetail.RMConsumptionID, oRMConsumptionDetail.ITransactionID, oRMConsumptionDetail.ProductID, oRMConsumptionDetail.LotID, oRMConsumptionDetail.WUID, oRMConsumptionDetail.MUnitID, oRMConsumptionDetail.Qty, oRMConsumptionDetail.UnitPrice, oRMConsumptionDetail.Amount, nUserID, (int)eEnumDBOperation, sRMConsumptionDetailIDs);
        }


		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_RMConsumptionDetail WHERE RMConsumptionDetailID=%n", nID);
		}
		public static IDataReader Gets(int id, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_RMConsumptionDetail WHERE RMConsumptionID = %n",id);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
