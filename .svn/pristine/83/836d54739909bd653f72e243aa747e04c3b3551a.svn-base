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
	public class CostHeadDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, CostHead oCostHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_CostHead]"
									+"%n,%s,%s,%n,%n,%n",
									oCostHead.CostHeadID,oCostHead.Name,oCostHead.Note,(int)oCostHead.CostHeadType, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, CostHead oCostHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_CostHead]"
									+"%n,%s,%s,%n,%n,%n",
									oCostHead.CostHeadID,oCostHead.Name,oCostHead.Note,(int)oCostHead.CostHeadType,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_CostHead WHERE CostHeadID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM CostHead");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
