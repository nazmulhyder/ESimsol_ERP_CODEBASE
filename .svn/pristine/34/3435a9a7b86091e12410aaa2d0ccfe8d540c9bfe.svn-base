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
	public class LotMixingDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, LotMixingDetail oLotMixingDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_LotMixingDetail]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%b,%n,%s, %n,%n,%s",
									oLotMixingDetail.LotMixingDetailID,oLotMixingDetail.LotMixingID,oLotMixingDetail.ProductID,oLotMixingDetail.LotID,oLotMixingDetail.Qty,oLotMixingDetail.Qty_Percentage ,oLotMixingDetail.MUnitID,oLotMixingDetail.BagCount,oLotMixingDetail.UnitPrice,oLotMixingDetail.CurrencyID,oLotMixingDetail.Remarks,oLotMixingDetail.IsLotMendatory,oLotMixingDetail.InOutTypeInt, oLotMixingDetail.LotNo, nUserID, (int)eEnumDBOperation, sDetailIDs);
		}

		public static void Delete(TransactionContext tc, LotMixingDetail oLotMixingDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_LotMixingDetail]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%b,%n,%s, %n,%n,%s",
                                    oLotMixingDetail.LotMixingDetailID, oLotMixingDetail.LotMixingID, oLotMixingDetail.ProductID, oLotMixingDetail.LotID, oLotMixingDetail.Qty, oLotMixingDetail.Qty_Percentage, oLotMixingDetail.MUnitID, oLotMixingDetail.BagCount, oLotMixingDetail.UnitPrice, oLotMixingDetail.CurrencyID, oLotMixingDetail.Remarks, oLotMixingDetail.IsLotMendatory, oLotMixingDetail.InOutTypeInt, oLotMixingDetail.LotNo, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_LotMixingDetail WHERE LotMixingDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM LotMixingDetail");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
