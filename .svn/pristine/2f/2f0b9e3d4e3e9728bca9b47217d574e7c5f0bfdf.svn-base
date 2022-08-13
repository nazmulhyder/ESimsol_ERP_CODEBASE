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
	public class BUWiseConsumptionUnitDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, BUWiseConsumptionUnit oBUWiseConsumptionUnit, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_BUWiseConsumptionUnit]"
									+"%n,%n,%n,%n,%n",
									oBUWiseConsumptionUnit.BUWiseConsumptionUnitID,oBUWiseConsumptionUnit.BUID,oBUWiseConsumptionUnit.ConsumptionUnitID,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, BUWiseConsumptionUnit oBUWiseConsumptionUnit, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_BUWiseConsumptionUnit]"
                                    + "%n,%n,%n,%n,%n",
                                    oBUWiseConsumptionUnit.BUWiseConsumptionUnitID, oBUWiseConsumptionUnit.BUID, oBUWiseConsumptionUnit.ConsumptionUnitID, nUserID, (int)eEnumDBOperation);
        }


		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_BUWiseConsumptionUnit WHERE BUWiseConsumptionUnitID=%n", nID);
		}
        public static IDataReader Gets(TransactionContext tc, int nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_BUWiseConsumptionUnit WHERE ConsumptionUnitID=%n", nID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
