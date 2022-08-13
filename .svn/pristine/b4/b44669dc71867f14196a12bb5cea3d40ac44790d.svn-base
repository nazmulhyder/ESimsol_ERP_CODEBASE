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
	public class FNMachineProcessDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FNMachineProcess oFNMachineProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FNMachineProcess]"
									+"%n,%n,%n,%n,%d,%n,%n",
									oFNMachineProcess.FNMProcessID,oFNMachineProcess.FNMachineID,oFNMachineProcess.FNTPID,oFNMachineProcess.InActiveBy,oFNMachineProcess.InActiveDate,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FNMachineProcess oFNMachineProcess, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FNMachineProcess]"
									+"%n,%n,%n,%n,%d,%n,%n",
									oFNMachineProcess.FNMProcessID,oFNMachineProcess.FNMachineID,oFNMachineProcess.FNTPID,oFNMachineProcess.InActiveBy,oFNMachineProcess.InActiveDate,nUserID, (int)eEnumDBOperation);
		}
        public static void ChangeActivety(TransactionContext tc, FNMachineProcess oFNMachineProcess, Int64 nUserID)
        {
            if (oFNMachineProcess.InActiveBy==0)
            {
                tc.ExecuteNonQuery("Update FNMachineProcess SET InActiveBy=%n, InActiveDate = %d WHERE FNMProcessID=%n", nUserID,DateTime.Now,  oFNMachineProcess.FNMProcessID);
            }
            else
            {
                tc.ExecuteNonQuery("Update FNMachineProcess SET InActiveBy=0, InActiveDate = null WHERE FNMProcessID=%n", oFNMachineProcess.FNMProcessID);
            }
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNMachineProcess WHERE FNMProcessID=%n", nID);
		}
		public static IDataReader Gets(int nFNMachineID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNMachineProcess WHERE FNMachineID=%n Order BY FNTreatment", nFNMachineID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
