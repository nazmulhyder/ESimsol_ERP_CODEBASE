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
	public class FNRequisitionDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FNRequisitionDetail oFNRequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FNRequisitionDetail]"
									+"%n,%n,%n,%n,%n,%n,%s,%n,%n",
									oFNRequisitionDetail.FNRDetailID,oFNRequisitionDetail.FNRID,oFNRequisitionDetail.ProductID,oFNRequisitionDetail.LotID,oFNRequisitionDetail.RequiredQty,oFNRequisitionDetail.DisburseQty,oFNRequisitionDetail.Remarks,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, FNRequisitionDetail oFNRequisitionDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_FNRequisitionDetail]"
									+"%n,%n,%n,%n,%n,%n,%s,%n,%n",
									oFNRequisitionDetail.FNRDetailID,oFNRequisitionDetail.FNRID,oFNRequisitionDetail.ProductID,oFNRequisitionDetail.LotID,oFNRequisitionDetail.RequiredQty,oFNRequisitionDetail.DisburseQty,oFNRequisitionDetail.Remarks,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FNRequisitionDetail WHERE FNRDetailID=%n", nID);
		}
		public static IDataReader Gets(int id, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNRequisitionDetail WHERE FNRID=%n",id);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
