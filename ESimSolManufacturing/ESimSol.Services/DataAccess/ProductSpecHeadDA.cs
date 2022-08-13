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
	public class ProductSpecHeadDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, ProductSpecHead oProductSpecHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_ProductSpecHead]"
									+"%n,%n,%n,%n,%n",
									oProductSpecHead.ProductSpecHeadID,oProductSpecHead.ProductID,oProductSpecHead.SpecHeadID,oProductSpecHead.Sequence, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, ProductSpecHead oProductSpecHead, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_ProductSpecHead]"
									+"%n,%n,%n,%n,%n",
									oProductSpecHead.ProductSpecHeadID,oProductSpecHead.ProductID,oProductSpecHead.SpecHeadID,oProductSpecHead.Sequence, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_ProductSpecHead WHERE ProductSpecHeadID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM ProductSpecHead");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}

        public static IDataReader UpDown(TransactionContext tc, ProductSpecHead oProductSpecHead, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_ProductSpecHeadUpDown]" + "%n, %n, %n"
                , oProductSpecHead.ProductID
                , oProductSpecHead.ProductSpecHeadID
                , oProductSpecHead.IsUp
            );
        }

		#endregion
	}

}
