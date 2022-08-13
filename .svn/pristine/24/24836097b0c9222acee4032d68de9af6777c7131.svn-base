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
	public class CapacityAllocationDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, CapacityAllocation oCapacityAllocation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_CapacityAllocation]"
									+"%n,%s,%n,%n,%n,%s,%n,%n",
									oCapacityAllocation.CapacityAllocationID,oCapacityAllocation.Code,oCapacityAllocation.BuyerID,oCapacityAllocation.Quantity,oCapacityAllocation.MUnitID,oCapacityAllocation.Remarks,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, CapacityAllocation oCapacityAllocation, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_CapacityAllocation]"
									+"%n,%s,%n,%n,%n,%s,%n,%n",
									oCapacityAllocation.CapacityAllocationID,oCapacityAllocation.Code,oCapacityAllocation.BuyerID,oCapacityAllocation.Quantity,oCapacityAllocation.MUnitID,oCapacityAllocation.Remarks,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_CapacityAllocation WHERE CapacityAllocationID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_CapacityAllocation");
		}
        public static IDataReader GetsBookingStatus(DateTime dStartDate, DateTime dEndDate, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_GetCapacityAllocation]"+"%d,%d",dStartDate, dEndDate);
        }

		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
