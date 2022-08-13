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
	public class CommercialFDBPDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, CommercialFDBP oCommercialFDBP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_CommercialFDBP]"
									+"%n,%n,%d,%n,%n,%n,%n,%s,%n,%n",
									oCommercialFDBP.CommercialFDBPID,oCommercialFDBP.CommercialBSID,oCommercialFDBP.PurchaseDate,oCommercialFDBP.PurchaseAmount,oCommercialFDBP.BankCharge,oCommercialFDBP.CRate,oCommercialFDBP.CurrencyID,oCommercialFDBP.Remarks,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, CommercialFDBP oCommercialFDBP, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_CommercialFDBP]"
									+"%n,%n,%d,%n,%n,%n,%n,%s,%n,%n",
									oCommercialFDBP.CommercialFDBPID,oCommercialFDBP.CommercialBSID,oCommercialFDBP.PurchaseDate,oCommercialFDBP.PurchaseAmount,oCommercialFDBP.BankCharge,oCommercialFDBP.CRate,oCommercialFDBP.CurrencyID,oCommercialFDBP.Remarks,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_CommercialFDBP WHERE CommercialFDBPID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM CommercialFDBP");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
