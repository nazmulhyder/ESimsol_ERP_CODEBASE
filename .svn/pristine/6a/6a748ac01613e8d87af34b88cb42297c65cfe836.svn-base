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
	public class PurchaseReturnDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, PurchaseReturn oPurchaseReturn, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_PurchaseReturn]"
                                    + "%n,%s,%n,%d,%n,%n,%n,%n,%s,%n,%n",
                                    oPurchaseReturn.PurchaseReturnID, oPurchaseReturn.ReturnNo, oPurchaseReturn.BUID, oPurchaseReturn.ReturnDate, oPurchaseReturn.RefTypeInt, oPurchaseReturn.SupplierID, oPurchaseReturn.RefObjectID, oPurchaseReturn.WorkingUnitID, oPurchaseReturn.Remarks, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, PurchaseReturn oPurchaseReturn, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_PurchaseReturn]"
                                    + "%n,%s,%n,%d,%n,%n,%n,%n,%s,%n,%n",
                                    oPurchaseReturn.PurchaseReturnID, oPurchaseReturn.ReturnNo, oPurchaseReturn.BUID, oPurchaseReturn.ReturnDate, oPurchaseReturn.RefTypeInt, oPurchaseReturn.SupplierID, oPurchaseReturn.RefObjectID, oPurchaseReturn.WorkingUnitID, oPurchaseReturn.Remarks, nUserID, (int)eEnumDBOperation);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_PurchaseReturn WHERE PurchaseReturnID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM PurchaseReturn");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
