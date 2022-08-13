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
	public class FNExecutionOrderProcessDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FNExecutionOrderProcess oFNExecutionOrderProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FNExecutionOrderProcess]"
									+"%n,%n,%s,%n,%n,%n,%n",
									oFNExecutionOrderProcess.FNExOProcessID,oFNExecutionOrderProcess.FNExOID,oFNExecutionOrderProcess.Remark,oFNExecutionOrderProcess.Sequence,oFNExecutionOrderProcess.FNTPID,nUserID, (int)eEnumDBOperation);
		}
		public static void Delete(TransactionContext tc, FNExecutionOrderProcess oFNExecutionOrderProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FNExecutionOrderProcess]"
									+"%n,%n,%s,%n,%n,%n,%n",
									oFNExecutionOrderProcess.FNExOProcessID,oFNExecutionOrderProcess.FNExOID,oFNExecutionOrderProcess.Remark,oFNExecutionOrderProcess.Sequence,oFNExecutionOrderProcess.FNTPID,nUserID, (int)eEnumDBOperation);
		}


        public static IDataReader UpDown(TransactionContext tc, FNExecutionOrderProcess oFNExecutionOrderProcess, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_FNExOrderProcessUpDown]" + "%n, %n, %n"
                , oFNExecutionOrderProcess.FNExOID
                , oFNExecutionOrderProcess.FNExOProcessID
                , oFNExecutionOrderProcess.IsUp
            );
        }
        public static void UpdateSequence(TransactionContext tc, FNExecutionOrderProcess oFNExecutionOrderProcess)
        {
            tc.ExecuteNonQuery("Update FNExecutionOrderProcess SET Sequence = %n WHERE FNExOProcessID = %n", oFNExecutionOrderProcess.Sequence, oFNExecutionOrderProcess.FNExOProcessID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNExecutionOrderProcess WHERE FNExOProcessID=%n", nID);
		}
		public static IDataReader Gets(int nID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNExecutionOrderProcess WHERE FNExOID=%n ORDER BY Sequence  ", nID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
