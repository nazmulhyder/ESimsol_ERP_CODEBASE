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
	public class RMConsumptionDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, RMConsumption oRMConsumption, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_RMConsumption]"
									+"%n,%s,%d,%n,%s,%n,%n",
									oRMConsumption.RMConsumptionID,oRMConsumption.ConsumptionNo,oRMConsumption.ConsumptionDate,oRMConsumption.BUID,oRMConsumption.Remarks,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, RMConsumption oRMConsumption, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_RMConsumption]"
                                    + "%n,%s,%d,%n,%s,%n,%n",
                                    oRMConsumption.RMConsumptionID, oRMConsumption.ConsumptionNo, oRMConsumption.ConsumptionDate, oRMConsumption.BUID, oRMConsumption.Remarks, nUserID, (int)eEnumDBOperation);
        }
        public static void Approved(TransactionContext tc, RMConsumption oRMConsumption, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE RMConsumption SET ApprovedBy = %n WHERE RMConsumptionID = %n", nUserID, oRMConsumption.RMConsumptionID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_RMConsumption WHERE RMConsumptionID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_RMConsumption");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
