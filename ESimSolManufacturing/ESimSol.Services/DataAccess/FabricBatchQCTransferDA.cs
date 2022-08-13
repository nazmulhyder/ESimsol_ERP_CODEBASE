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
	public class FabricBatchQCTransferDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FabricBatchQCTransfer oFabricBatchQCTransfer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchQCTransfer]"
									+"%n,%n,%d,%n",
									oFabricBatchQCTransfer.FBQCTransferID,nUserID,oFabricBatchQCTransfer.IssueDate, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FabricBatchQCTransfer oFabricBatchQCTransfer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchQCTransfer]"
									+"%n,%s,%d,%n,%n,%n",
									oFabricBatchQCTransfer.FBQCTransferID,oFabricBatchQCTransfer.TransferNo,oFabricBatchQCTransfer.IssueDate,oFabricBatchQCTransfer.IssueBy,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchQCTransfer WHERE FBQCTransferID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchQCTransfer");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
