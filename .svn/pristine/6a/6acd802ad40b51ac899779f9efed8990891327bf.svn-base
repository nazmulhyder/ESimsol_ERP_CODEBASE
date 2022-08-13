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
	public class S2SLotTransferDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, S2SLotTransfer oS2SLotTransfer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_S2SLotTransfer]"
									+"%n,%n,%n,%n,%d,%n,%n,%n,%s,%n,%s,%n,%n",
                                    oS2SLotTransfer.S2SLotTransferID, oS2SLotTransfer.StoreID, oS2SLotTransfer.BUID, oS2SLotTransfer.ProductID, oS2SLotTransfer.TransferDate, oS2SLotTransfer.IssueStyleID, oS2SLotTransfer.ReceiveStyleID, oS2SLotTransfer.IssueLotID, oS2SLotTransfer.ReceiveLotNo, oS2SLotTransfer.TransferQty, oS2SLotTransfer.Remarks, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, S2SLotTransfer oS2SLotTransfer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_S2SLotTransfer]"
                                    + "%n,%n,%n,%n,%d,%n,%n,%n,%s,%n,%s,%n,%n",
                                    oS2SLotTransfer.S2SLotTransferID, oS2SLotTransfer.StoreID, oS2SLotTransfer.BUID, oS2SLotTransfer.ProductID, oS2SLotTransfer.TransferDate, oS2SLotTransfer.IssueStyleID, oS2SLotTransfer.ReceiveStyleID, oS2SLotTransfer.IssueLotID, oS2SLotTransfer.ReceiveLotNo, oS2SLotTransfer.TransferQty, oS2SLotTransfer.Remarks, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_S2SLotTransfer WHERE S2SLotTransferID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_S2SLotTransfer");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
