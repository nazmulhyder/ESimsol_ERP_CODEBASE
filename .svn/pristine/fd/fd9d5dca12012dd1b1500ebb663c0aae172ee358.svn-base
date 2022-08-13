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
	public class FNTreatmentProcessDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FNTreatmentProcess oFNTreatmentProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FNTreatmentProcess]"
									+"%n,%s,%n,%s,%n,%n,%n,%b",
                                    oFNTreatmentProcess.FNTPID, oFNTreatmentProcess.Description, oFNTreatmentProcess.FNTreatment, oFNTreatmentProcess.FNProcess, nUserID, (int)eEnumDBOperation, oFNTreatmentProcess.Code, oFNTreatmentProcess.IsProduction);
		}

		public static void Delete(TransactionContext tc, FNTreatmentProcess oFNTreatmentProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FNTreatmentProcess]"
                                    + "%n,%s,%n,%s,%n,%n,%n,%b",
                                    oFNTreatmentProcess.FNTPID, oFNTreatmentProcess.Description, oFNTreatmentProcess.FNTreatment, oFNTreatmentProcess.FNProcess, nUserID, (int)eEnumDBOperation, oFNTreatmentProcess.Code, oFNTreatmentProcess.IsProduction);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM FNTreatmentProcess WHERE FNTPID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM FNTreatmentProcess ORDER BY FNTreatment, Code");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
