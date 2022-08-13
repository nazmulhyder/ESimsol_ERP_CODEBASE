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
	public class CommercialEncashmentDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, CommercialEncashmentDetail oCommercialEncashmentDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDEtailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_CommercialEncashmentDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s",
                                    oCommercialEncashmentDetail.CommercialEncashmentDetailID, oCommercialEncashmentDetail.CommercialEncashmentID, oCommercialEncashmentDetail.BankAccountID, oCommercialEncashmentDetail.ExpenditureHeadID, oCommercialEncashmentDetail.CurrencyID, oCommercialEncashmentDetail.AmountInCurrency, oCommercialEncashmentDetail.CRate, oCommercialEncashmentDetail.AmountBC, nUserID, (int)eEnumDBOperation, sDEtailIDs);
		}

        public static void Delete(TransactionContext tc, CommercialEncashmentDetail oCommercialEncashmentDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDEtailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_CommercialEncashmentDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s",
                                    oCommercialEncashmentDetail.CommercialEncashmentDetailID, oCommercialEncashmentDetail.CommercialEncashmentID, oCommercialEncashmentDetail.BankAccountID, oCommercialEncashmentDetail.ExpenditureHeadID, oCommercialEncashmentDetail.CurrencyID, oCommercialEncashmentDetail.AmountInCurrency, oCommercialEncashmentDetail.CRate, oCommercialEncashmentDetail.AmountBC, nUserID, (int)eEnumDBOperation, sDEtailIDs);
        }

		#endregion

		#region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, int CommercialEncashmentID)
		{
            return tc.ExecuteReader("SELECT * FROM View_CommercialEncashmentDetail WHERE CommercialEncashmentID = %n", CommercialEncashmentID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
