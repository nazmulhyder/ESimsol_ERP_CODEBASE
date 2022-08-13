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
	public class FNMachineDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FNMachine oFNMachine, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FNMachine]"
									+"%n,%n,%s,%s,%s,%n,%b,%n,%n",
									oFNMachine.FNMachineID,oFNMachine.FNMachineType,oFNMachine.Code,oFNMachine.Name,oFNMachine.Note,oFNMachine.NoOfBath,oFNMachine.IsAtive,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FNMachine oFNMachine, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FNMachine]"
									+"%n,%n,%s,%s,%s,%n,%b,%n,%n",
									oFNMachine.FNMachineID,oFNMachine.FNMachineType,oFNMachine.Code,oFNMachine.Name,oFNMachine.Note,oFNMachine.NoOfBath,oFNMachine.IsAtive,nUserID, (int)eEnumDBOperation);
		}

        public static void ChangeActivety(TransactionContext tc, FNMachine oFNMachine)
        {
           if(oFNMachine.IsAtive)
           {
               tc.ExecuteNonQuery("Update FNMachine SET IsAtive=%b WHERE FNMachineID=%n", false, oFNMachine.FNMachineID);
           }
           else
           {
               tc.ExecuteNonQuery("Update FNMachine SET IsAtive=%b WHERE FNMachineID=%n", true, oFNMachine.FNMachineID);
           }
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FNMachine WHERE FNMachineID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNMachine");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
