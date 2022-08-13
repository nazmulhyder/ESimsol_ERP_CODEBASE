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
	public class CommercialBSDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, CommercialBSDetail oCommercialBSDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_CommercialBSDetail]"
									+"%n,%n,%n,%n,%s,%n,%n,%s",
                                    oCommercialBSDetail.CommercialBSDetailID, oCommercialBSDetail.CommercialBSID, oCommercialBSDetail.CommercialInvoiceID, oCommercialBSDetail.InvoiceAmount, oCommercialBSDetail.Remarks, nUserID, (int)eEnumDBOperation, sDetailIDs);
		}

        public static void Delete(TransactionContext tc, CommercialBSDetail oCommercialBSDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_CommercialBSDetail]"
                                    + "%n,%n,%n,%n,%s,%n,%n,%s",
                                    oCommercialBSDetail.CommercialBSDetailID, oCommercialBSDetail.CommercialBSID, oCommercialBSDetail.CommercialInvoiceID, oCommercialBSDetail.InvoiceAmount, oCommercialBSDetail.Remarks, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_CommercialBSDetail WHERE CommercialBSDetailID=%n", nID);
		}
		public static IDataReader Gets(int nCommercialBSID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_CommercialBSDetail WHERE CommercialBSID=%n", nCommercialBSID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
