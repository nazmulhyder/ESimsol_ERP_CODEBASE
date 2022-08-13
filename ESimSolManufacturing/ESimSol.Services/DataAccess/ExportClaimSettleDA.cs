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
	public class ExportClaimSettleDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, ExportClaimSettle oExportClaimSettle, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_ExportClaimSettle]"
									+"%n,%n,%n,%s,%n,%n,%n,%n",
									oExportClaimSettle.ExportClaimSettleID,oExportClaimSettle.ExportBillID,oExportClaimSettle.InoutTypeInt,oExportClaimSettle.SettleName ,oExportClaimSettle.Amount,oExportClaimSettle.CurrencyID,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, ExportClaimSettle oExportClaimSettle, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_ExportClaimSettle]"
									+"%n,%n,%n,%s,%n,%n,%n,%n",
                                    oExportClaimSettle.ExportClaimSettleID, oExportClaimSettle.ExportBillID, oExportClaimSettle.InoutTypeInt, oExportClaimSettle.SettleName, oExportClaimSettle.Amount, oExportClaimSettle.CurrencyID, nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_ExportClaimSettle WHERE ExportClaimSettleID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_ExportClaimSettle");
		}
        public static IDataReader GetsByBillID(int nExportBillID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportClaimSettle WHERE ExportBillID=" + nExportBillID);
        } 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
