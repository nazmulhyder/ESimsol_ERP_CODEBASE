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
	public class JobDetailDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, JobDetail oJobDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sJobDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_JobDetail]"
									+"%n,%n,%n,%n,%n,%s",
									oJobDetail.JobDetailID,oJobDetail.JobID,oJobDetail.OrderRecapID,nUserID, (int)eEnumDBOperation, sJobDetailIDs);
		}

        public static void Delete(TransactionContext tc, JobDetail oJobDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sJobDetailIDs)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_JobDetail]"
									+"%n,%n,%n,%n,%n,%s",
                                    oJobDetail.JobDetailID, oJobDetail.JobID, oJobDetail.OrderRecapID, nUserID, (int)eEnumDBOperation, sJobDetailIDs);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_JobDetail WHERE JobDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc, int nJobID)
		{
			return tc.ExecuteReader("SELECT * FROM View_JobDetail WHERE JobID =%n",nJobID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
