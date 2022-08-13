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
	public class BTMADetailDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, BTMADetail oBTMADetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_BTMADetail]"
                                    + "%n,%n,%n, %s,%s,%n,%n,%n, %n,%n,%s",
                                    oBTMADetail.BTMADetailID, oBTMADetail.BTMAID, oBTMADetail.ProductID, oBTMADetail.ProductName, oBTMADetail.PINo, oBTMADetail.Qty, oBTMADetail.MUnitID, oBTMADetail.UnitPrice, nUserID, (int)eEnumDBOperation, sDetailIDs);
		}

        public static void Delete(TransactionContext tc, BTMADetail oBTMADetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_BTMADetail]"
									+"%n,%n,%n, %s,%s,%n,%n,%n, %n,%n,%s",
                                    oBTMADetail.BTMADetailID, oBTMADetail.BTMAID, oBTMADetail.ProductID, oBTMADetail.ProductName, oBTMADetail.PINo, oBTMADetail.Qty, oBTMADetail.MUnitID, oBTMADetail.UnitPrice, nUserID, (int)eEnumDBOperation, sDetailIDs);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_BTMADetail WHERE BTMADetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM BTMADetail");
		}
        public static IDataReader Gets(int nBTMAID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BTMADetail WHERE BTMAID=" + nBTMAID);
        } 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
