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
	public class CommercialEncashmentDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, CommercialEncashment oCommercialEncashment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_CommercialEncashment]"
									+"%n,%n,%d,%n,%n,%n,%n,%n,%n,%n,%n,%b,%n,%n,%s,%n,%n",
									oCommercialEncashment.CommercialEncashmentID,oCommercialEncashment.CommercialBSID,oCommercialEncashment.EncashmentDate,oCommercialEncashment.ApprovedBy,oCommercialEncashment.AmountInCurrency,oCommercialEncashment.AmountBC,oCommercialEncashment.EncashRate,oCommercialEncashment.OverDueInCurrency,oCommercialEncashment.OverDueBC,oCommercialEncashment.GainLossCurrencyID,oCommercialEncashment.GainLossAmount,oCommercialEncashment.IsGain,oCommercialEncashment.PDeductionInCurrency,oCommercialEncashment.PDeductionBC,oCommercialEncashment.Remarks,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, CommercialEncashment oCommercialEncashment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_CommercialEncashment]"
									+"%n,%n,%d,%n,%n,%n,%n,%n,%n,%n,%n,%b,%n,%n,%s,%n,%n",
									oCommercialEncashment.CommercialEncashmentID,oCommercialEncashment.CommercialBSID,oCommercialEncashment.EncashmentDate,oCommercialEncashment.ApprovedBy,oCommercialEncashment.AmountInCurrency,oCommercialEncashment.AmountBC,oCommercialEncashment.EncashRate,oCommercialEncashment.OverDueInCurrency,oCommercialEncashment.OverDueBC,oCommercialEncashment.GainLossCurrencyID,oCommercialEncashment.GainLossAmount,oCommercialEncashment.IsGain,oCommercialEncashment.PDeductionInCurrency,oCommercialEncashment.PDeductionBC,oCommercialEncashment.Remarks,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_CommercialEncashment WHERE CommercialEncashmentID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM CommercialEncashment");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
