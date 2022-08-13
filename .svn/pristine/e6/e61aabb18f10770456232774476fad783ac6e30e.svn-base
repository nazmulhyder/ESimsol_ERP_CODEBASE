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
	public class OrderRecapCommentDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, OrderRecapComment oOrderRecapComment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_OrderRecapComment]"
									+"%n,%n,%s,%s,%n,%n",
									oOrderRecapComment.OrderRecapCommentID,oOrderRecapComment.OrderRecapID,oOrderRecapComment.CommentsBy,oOrderRecapComment.CommentsText,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, OrderRecapComment oOrderRecapComment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_OrderRecapComment]"
									+"%n,%n,%s,%s,%n,%n",
									oOrderRecapComment.OrderRecapCommentID,oOrderRecapComment.OrderRecapID,oOrderRecapComment.CommentsBy,oOrderRecapComment.CommentsText,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM OrderRecapComment WHERE OrderRecapCommentID=%n", nID);
		}
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM OrderRecapComment where OrderRecapID=%n", id);
        } 
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM OrderRecapComment");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
