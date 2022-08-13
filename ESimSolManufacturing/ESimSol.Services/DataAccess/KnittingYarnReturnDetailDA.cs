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
	public class KnittingYarnReturnDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, KnittingYarnReturnDetail oKnittingYarnReturnDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID ,string SIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnittingYarnReturnDetail]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                    oKnittingYarnReturnDetail.KnittingYarnReturnDetailID, oKnittingYarnReturnDetail.KnittingYarnReturnID, oKnittingYarnReturnDetail.KnittingYarnChallanDetailID, oKnittingYarnReturnDetail.YarnID, oKnittingYarnReturnDetail.ReceiveStoreID, oKnittingYarnReturnDetail.LotID, oKnittingYarnReturnDetail.MUnitID, oKnittingYarnReturnDetail.Qty, oKnittingYarnReturnDetail.Remarks, nUserID, (int)eEnumDBOperation, SIDs);
		}

        public static void Delete(TransactionContext tc, KnittingYarnReturnDetail oKnittingYarnReturnDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string SIDs)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_KnittingYarnReturnDetail]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                    oKnittingYarnReturnDetail.KnittingYarnReturnDetailID, oKnittingYarnReturnDetail.KnittingYarnReturnID, oKnittingYarnReturnDetail.KnittingYarnChallanDetailID, oKnittingYarnReturnDetail.YarnID, oKnittingYarnReturnDetail.ReceiveStoreID, oKnittingYarnReturnDetail.LotID, oKnittingYarnReturnDetail.MUnitID, oKnittingYarnReturnDetail.Qty, oKnittingYarnReturnDetail.Remarks, nUserID, (int)eEnumDBOperation, SIDs);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_KnittingYarnReturnDetail WHERE KnittingYarnReturnDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM KnittingYarnReturnDetail");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
