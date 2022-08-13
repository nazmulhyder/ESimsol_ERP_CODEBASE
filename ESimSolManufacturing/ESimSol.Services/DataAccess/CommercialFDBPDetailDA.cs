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
	public class CommercialFDBPDetailDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, CommercialFDBPDetail oCommercialFDBPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_CommercialFDBPDetail]"
									+"%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                    oCommercialFDBPDetail.CommercialFDBPDetailID, oCommercialFDBPDetail.CommercialFDBPID, oCommercialFDBPDetail.BankAccountID, oCommercialFDBPDetail.AmountInCurrency, oCommercialFDBPDetail.CRate, oCommercialFDBPDetail.AmountBC, oCommercialFDBPDetail.Remarks, nUserID, (int)eEnumDBOperation, sDetailIDs);
		}

        public static void Delete(TransactionContext tc, CommercialFDBPDetail oCommercialFDBPDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_CommercialFDBPDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                    oCommercialFDBPDetail.CommercialFDBPDetailID, oCommercialFDBPDetail.CommercialFDBPID, oCommercialFDBPDetail.BankAccountID, oCommercialFDBPDetail.AmountInCurrency, oCommercialFDBPDetail.CRate, oCommercialFDBPDetail.AmountBC, oCommercialFDBPDetail.Remarks, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }

		#endregion

		#region Get & Exist Function
	
        public static IDataReader Gets(TransactionContext tc, int CommercialFDBPID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CommercialFDBPDetail WHERE CommercialFDBPID = %n", CommercialFDBPID);
        } 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
