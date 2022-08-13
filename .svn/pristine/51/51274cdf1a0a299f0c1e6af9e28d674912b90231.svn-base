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
	public class OrderSheetDetailDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, OrderSheetDetail oOrderSheetDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_OrderSheetDetail]"
                                    + "%n,%n,%n,%n,%s,%s,%n,%n,%n,%d,%s,%n,%n,%s,%n,%n, %n,%n,%s",
                                    oOrderSheetDetail.OrderSheetDetailID, oOrderSheetDetail.OrderSheetID, oOrderSheetDetail.ProductID, oOrderSheetDetail.ColorID, oOrderSheetDetail.ProductDescription, oOrderSheetDetail.StyleDescription, oOrderSheetDetail.PolyMUnitID, oOrderSheetDetail.Qty, oOrderSheetDetail.UnitPrice, oOrderSheetDetail.DeliveryDate, oOrderSheetDetail.Note, oOrderSheetDetail.ModelReferenceID, oOrderSheetDetail.UnitID, oOrderSheetDetail.BuyerRef, oOrderSheetDetail.ColorQty,oOrderSheetDetail.RecipeID,  nUserID, (int)eEnumDBOperation, sDetailIDs);
		}

		public static void Delete(TransactionContext tc, OrderSheetDetail oOrderSheetDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_OrderSheetDetail]"
                                    + "%n,%n,%n,%n,%s,%s,%n,%n,%n,%d,%s,%n,%n,%s,%n,%n, %n,%n,%s",
                                    oOrderSheetDetail.OrderSheetDetailID, oOrderSheetDetail.OrderSheetID, oOrderSheetDetail.ProductID, oOrderSheetDetail.ColorID, oOrderSheetDetail.ProductDescription, oOrderSheetDetail.StyleDescription, oOrderSheetDetail.PolyMUnitID, oOrderSheetDetail.Qty, oOrderSheetDetail.UnitPrice, oOrderSheetDetail.DeliveryDate, oOrderSheetDetail.Note, oOrderSheetDetail.ModelReferenceID, oOrderSheetDetail.UnitID, oOrderSheetDetail.BuyerRef, oOrderSheetDetail.ColorQty, oOrderSheetDetail.RecipeID, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }


		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_OrderSheetDetail WHERE OrderSheetDetailID=%n", nID);
		}
		public static IDataReader Gets(int nORSID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_OrderSheetDetail WHERE OrderSheetID = %n ORDER BY OrderSheetDetailID", nORSID);
		}

        public static IDataReader GetsByLog(int nORSLogID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_OrderSheetDetailLog WHERE OrderSheetLogID = %n",nORSLogID);
		} 
        
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
