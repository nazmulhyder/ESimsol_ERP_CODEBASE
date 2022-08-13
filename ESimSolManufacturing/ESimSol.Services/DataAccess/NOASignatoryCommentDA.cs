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
	public class NOASignatoryCommentDA 
	{
		#region Insert Update Delete Function
		public static IDataReader InsertUpdate(TransactionContext tc, NOASignatoryComment oNOASignatoryComment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_NOASignatoryComment]"
									+"%n,%n,%n,%s,%s,%n,%b,%n,%n,%n",
                                    oNOASignatoryComment.NOASignatoryCommentID, oNOASignatoryComment.NOADetailID, oNOASignatoryComment.PQDetailID, oNOASignatoryComment.Comment,oNOASignatoryComment.Note, oNOASignatoryComment.NOASignatoryID, oNOASignatoryComment.IsAllSave,oNOASignatoryComment.PurchaseQty, nUserID, (int)eEnumDBOperation);
		}
		public static void Delete(TransactionContext tc, NOASignatoryComment oNOASignatoryComment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_NOASignatoryComment]"
                                        + "%n,%n,%n,%s,%s,%n,%b,%n,%n,%n",
                                        oNOASignatoryComment.NOASignatoryCommentID, oNOASignatoryComment.NOADetailID, oNOASignatoryComment.PQDetailID, oNOASignatoryComment.Comment, oNOASignatoryComment.Note, oNOASignatoryComment.NOASignatoryID, oNOASignatoryComment.IsAllSave, oNOASignatoryComment.PurchaseQty,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_NOASignatoryComment WHERE NOASignatoryCommentID=%n", nID);
		}
        public static IDataReader Gets(TransactionContext tc, int nNOADetailID)
		{
            return tc.ExecuteReader("SELECT * FROM View_NOASignatoryComment where NOADetailID=%n", nNOADetailID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
