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
	public class BUWiseProductCategoryDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, BUWiseProductCategory oBUWiseProductCategory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_BUWiseProductCategory]"
									+"%n,%n,%n,%n,%n",
									oBUWiseProductCategory.BUWiseProductCategoryID,oBUWiseProductCategory.BUID,oBUWiseProductCategory.ProductCategoryID,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, BUWiseProductCategory oBUWiseProductCategory, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_BUWiseProductCategory]"
                                    + "%n,%n,%n,%n,%n",
                                    oBUWiseProductCategory.BUWiseProductCategoryID, oBUWiseProductCategory.BUID, oBUWiseProductCategory.ProductCategoryID, nUserID, (int)eEnumDBOperation);
        }


		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_BUWiseProductCategory WHERE BUWiseProductCategoryID=%n", nID);
		}
        public static IDataReader Gets(TransactionContext tc, int nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_BUWiseProductCategory WHERE ProductCategoryID=%n", nID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
