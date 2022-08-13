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
	public class DyeingOrderFabricDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, DyeingOrderFabricDetail oDyeingOrderFabricDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_DyeingOrderFabricDetail]"
									+"%n,%n,%n,%n,%n,%n,%s,%n,%n,%n,%s,%s,%s,%n,%n,%s,%s,%s,%b,%s,%n,%s,%n,%n",
									oDyeingOrderFabricDetail.DyeingOrderFabricDetailID,oDyeingOrderFabricDetail.DyeingOrderID,oDyeingOrderFabricDetail.DyeingOrderDetailID,oDyeingOrderFabricDetail.FSCDetailID,oDyeingOrderFabricDetail.FEOSID,oDyeingOrderFabricDetail.FEOSDID,oDyeingOrderFabricDetail.SLNo,oDyeingOrderFabricDetail.Qty,oDyeingOrderFabricDetail.WarpWeftType,oDyeingOrderFabricDetail.ProductID,oDyeingOrderFabricDetail.BuyerReference,oDyeingOrderFabricDetail.ColorInfo,oDyeingOrderFabricDetail.StyleNo,oDyeingOrderFabricDetail.FinishType,oDyeingOrderFabricDetail.ProcessType,oDyeingOrderFabricDetail.Construction,oDyeingOrderFabricDetail.FabricWidth,oDyeingOrderFabricDetail.ExeNo,oDyeingOrderFabricDetail.IsWarp,oDyeingOrderFabricDetail.ColorName,oDyeingOrderFabricDetail.EndsCount,oDyeingOrderFabricDetail.BatchNo,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, DyeingOrderFabricDetail oDyeingOrderFabricDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_DyeingOrderFabricDetail]"
									+"%n,%n,%n,%n,%n,%n,%s,%n,%n,%n,%s,%s,%s,%n,%n,%s,%s,%s,%b,%s,%n,%s,%n,%n",
									oDyeingOrderFabricDetail.DyeingOrderFabricDetailID,oDyeingOrderFabricDetail.DyeingOrderID,oDyeingOrderFabricDetail.DyeingOrderDetailID,oDyeingOrderFabricDetail.FSCDetailID,oDyeingOrderFabricDetail.FEOSID,oDyeingOrderFabricDetail.FEOSDID,oDyeingOrderFabricDetail.SLNo,oDyeingOrderFabricDetail.Qty,oDyeingOrderFabricDetail.WarpWeftType,oDyeingOrderFabricDetail.ProductID,oDyeingOrderFabricDetail.BuyerReference,oDyeingOrderFabricDetail.ColorInfo,oDyeingOrderFabricDetail.StyleNo,oDyeingOrderFabricDetail.FinishType,oDyeingOrderFabricDetail.ProcessType,oDyeingOrderFabricDetail.Construction,oDyeingOrderFabricDetail.FabricWidth,oDyeingOrderFabricDetail.ExeNo,oDyeingOrderFabricDetail.IsWarp,oDyeingOrderFabricDetail.ColorName,oDyeingOrderFabricDetail.EndsCount,oDyeingOrderFabricDetail.BatchNo,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_DyeingOrderFabricDetail WHERE DyeingOrderFabricDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM DyeingOrderFabricDetail");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
