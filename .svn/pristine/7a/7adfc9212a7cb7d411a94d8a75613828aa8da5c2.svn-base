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
	public class FPDataDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FPData oFPData, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FPData]"
									+"%n,%d,%n,%n,%s,%n,%n,%n,%n,%n,%n",
									oFPData.FPDataID,oFPData.FPDate, oFPData.OperationalCost,oFPData.BTBCost,oFPData.ExportHMonth,oFPData.ExportHQty,oFPData.EHValue,oFPData.ExportQty,oFPData.ExportValue,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FPData oFPData, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_FPData]"
                                    + "%n,%d,%n,%n,%s,%n,%n,%n,%n,%n,%n",
                                    oFPData.FPDataID, oFPData.FPDate, oFPData.OperationalCost, oFPData.BTBCost, oFPData.ExportHMonth, oFPData.ExportHQty, oFPData.EHValue, oFPData.ExportQty, oFPData.ExportValue, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM FPData WHERE FPDataID=%n", nID);
		}
	
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
