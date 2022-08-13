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
	public class PurchaseReturnDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, PurchaseReturnDetail oPurchaseReturnDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_PurchaseReturnDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s",
									oPurchaseReturnDetail.PurchaseReturnDetailID,oPurchaseReturnDetail.PurchaseReturnID, oPurchaseReturnDetail.RefTypeInt, oPurchaseReturnDetail.ProductID,oPurchaseReturnDetail.MUnitID,oPurchaseReturnDetail.LotID,oPurchaseReturnDetail.StyleID,oPurchaseReturnDetail.RefObjectDetailID,oPurchaseReturnDetail.ColorID,oPurchaseReturnDetail.SizeID,oPurchaseReturnDetail.ReturnQty,nUserID, (int)eEnumDBOperation, sDetailIDs);
		}

        public static void Delete(TransactionContext tc, PurchaseReturnDetail oPurchaseReturnDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_PurchaseReturnDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s",
                                    oPurchaseReturnDetail.PurchaseReturnDetailID, oPurchaseReturnDetail.PurchaseReturnID, oPurchaseReturnDetail.RefTypeInt, oPurchaseReturnDetail.ProductID, oPurchaseReturnDetail.MUnitID, oPurchaseReturnDetail.LotID, oPurchaseReturnDetail.StyleID, oPurchaseReturnDetail.RefObjectDetailID, oPurchaseReturnDetail.ColorID, oPurchaseReturnDetail.SizeID, oPurchaseReturnDetail.ReturnQty, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_PurchaseReturnDetail WHERE PurchaseReturnDetailID=%n", nID);
		}
		public static IDataReader Gets(int id, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_PurchaseReturnDetail WHERE PurchaseReturnID = %n", id);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
         
		#endregion
	}

}
