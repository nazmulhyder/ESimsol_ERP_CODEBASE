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
	public class JobDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, Job oJob, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_Job]"
                                    + "%n,%s,%d,%n,%s,%n,%n,%n",
                                    oJob.JobID, oJob.JobNo, oJob.IssueDate, oJob.TechnicalSheetID, oJob.Remarks, oJob.BUID, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, Job oJob, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_Job]"
                                    + "%n,%s,%d,%n,%s,%n,%n,%n",
                                    oJob.JobID, oJob.JobNo, oJob.IssueDate, oJob.TechnicalSheetID, oJob.Remarks, oJob.BUID, nUserID, (int)eEnumDBOperation);
		}

        public static void UndoApprove(TransactionContext tc, int JobID )
        {
            tc.ExecuteNonQuery("");
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_Job WHERE JobID=%n", nID);
		}
		public static IDataReader Gets(int buid, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_Job Where ISNULL(ApprovedBy,0)=0 AND BUID =" + buid);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
