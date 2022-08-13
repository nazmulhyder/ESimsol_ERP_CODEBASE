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
	public class SalesQuotationDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, SalesQuotationDetail oSalesQuotationDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_SalesQuotationDetail]"
                                    + "%n,%n,%n,%s,%s,%n,%n,%s,%n,%n,%s",
                                        oSalesQuotationDetail.SalesQuotationDetailID, oSalesQuotationDetail.SalesQuotationID, oSalesQuotationDetail.FeatureID,  oSalesQuotationDetail.FeatureCode, oSalesQuotationDetail.FeatureName,  oSalesQuotationDetail.CurrencyID, oSalesQuotationDetail.Price, oSalesQuotationDetail.Remarks,   nUserID, (int)eEnumDBOperation, sIDs);
		}

        public static void Delete(TransactionContext tc, SalesQuotationDetail oSalesQuotationDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SalesQuotationDetail]"
                                    + "%n,%n,%n,%s,%s,%n,%n,%s,%n,%n,%s",
                                        oSalesQuotationDetail.SalesQuotationDetailID, oSalesQuotationDetail.SalesQuotationID, oSalesQuotationDetail.FeatureID, oSalesQuotationDetail.FeatureCode, oSalesQuotationDetail.FeatureName, oSalesQuotationDetail.CurrencyID, oSalesQuotationDetail.Price, oSalesQuotationDetail.Remarks, nUserID, (int)eEnumDBOperation, sIDs);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_SalesQuotationDetail WHERE SalesQuotationDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc, int id)
		{
            return tc.ExecuteReader("SELECT * FROM View_SalesQuotationDetail WHERE SalesQuotationID = %n", id);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
