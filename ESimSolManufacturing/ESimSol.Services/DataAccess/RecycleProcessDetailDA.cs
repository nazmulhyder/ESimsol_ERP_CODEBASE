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
	public class RecycleProcessDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, RecycleProcessDetail oRecycleProcessDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_RecycleProcessDetail]"
                                    + "%n, %n, %n, %n, %n,%n,%n,%n, %n, %n,%s",
                                    oRecycleProcessDetail.RecycleProcessDetailID, oRecycleProcessDetail.RecycleProcessID, oRecycleProcessDetail.ProductID, oRecycleProcessDetail.UnitID, oRecycleProcessDetail.Qty, oRecycleProcessDetail.ProcessProductType, oRecycleProcessDetail.LotID, oRecycleProcessDetail.WorkingUnitID, (int)eEnumDBOperation, nUserID, sIDs);
		}

        public static void Delete(TransactionContext tc, RecycleProcessDetail oRecycleProcessDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_RecycleProcessDetail]"
                                    + "%n, %n, %n, %n, %n,%n,%n,%n, %n, %n,%s",
                                    oRecycleProcessDetail.RecycleProcessDetailID, oRecycleProcessDetail.RecycleProcessID, oRecycleProcessDetail.ProductID, oRecycleProcessDetail.UnitID, oRecycleProcessDetail.Qty,oRecycleProcessDetail.ProcessProductType,oRecycleProcessDetail.LotID,oRecycleProcessDetail.WorkingUnitID, (int)eEnumDBOperation, nUserID, sIDs);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_RecycleProcessDetail WHERE RecycleProcessDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc, int id)
		{
            return tc.ExecuteReader("SELECT * FROM View_RecycleProcessDetail WHERE RecycleProcessID = %n Order By RecycleProcessDetailID", id);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
