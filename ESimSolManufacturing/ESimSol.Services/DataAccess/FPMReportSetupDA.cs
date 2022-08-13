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
	public class FPMReportSetupDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FPMReportSetup oFPMReportSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FPMReportSetup]"
									+"%n,%n,%n,%n,%n,%n",
									oFPMReportSetup.FPMReportSetupID,oFPMReportSetup.SetUpType,oFPMReportSetup.AccountHeadID,oFPMReportSetup.SubSetup,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FPMReportSetup oFPMReportSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FPMReportSetup]"
									+"%n,%n,%n,%n,%n,%n",
									oFPMReportSetup.FPMReportSetupID,oFPMReportSetup.SetUpType,oFPMReportSetup.AccountHeadID,oFPMReportSetup.SubSetup,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FPMReportSetup WHERE FPMReportSetupID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FPMReportSetup AS HH ORDER BY HH.SetUpType ASC");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
