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
	public class RSDetailAdditonalDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, RSDetailAdditonal oRSDetailAdditonal, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_RSDetailAdditonal]"
									+"%n,%n,%n,%n,%n,%n,%s,%n,%d,%n,%n",
									oRSDetailAdditonal.RSDetailAdditonalID,oRSDetailAdditonal.RouteSheetDetailID,oRSDetailAdditonal.SequenceNo,oRSDetailAdditonal.InOutType,oRSDetailAdditonal.Qty,oRSDetailAdditonal.LotID,oRSDetailAdditonal.Note,oRSDetailAdditonal.IssuedByID,oRSDetailAdditonal.IssueDate,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, RSDetailAdditonal oRSDetailAdditonal, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_RSDetailAdditonal]"
									+"%n,%n,%n,%n,%n,%n,%s,%n,%d,%n,%n",
									oRSDetailAdditonal.RSDetailAdditonalID,oRSDetailAdditonal.RouteSheetDetailID,oRSDetailAdditonal.SequenceNo,oRSDetailAdditonal.InOutType,oRSDetailAdditonal.Qty,oRSDetailAdditonal.LotID,oRSDetailAdditonal.Note,oRSDetailAdditonal.IssuedByID,oRSDetailAdditonal.IssueDate,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_RSDetailAdditonal WHERE RSDetailAdditonalID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM RSDetailAdditonal");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
