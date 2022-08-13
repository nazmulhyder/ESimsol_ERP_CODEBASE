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
	public class KnitDyeingBatchDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingBatchDetail oKnitDyeingBatchDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingBatchDetail]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n",
									oKnitDyeingBatchDetail.KnitDyeingBatchDetailID,oKnitDyeingBatchDetail.KnitDyeingBatchID,oKnitDyeingBatchDetail.KnitDyeingPTUID,oKnitDyeingBatchDetail.FabricTypeID,oKnitDyeingBatchDetail.GrayDiaID,oKnitDyeingBatchDetail.RollQty,oKnitDyeingBatchDetail.GrayQty,oKnitDyeingBatchDetail.ProcessLoss,oKnitDyeingBatchDetail.FinishQty,oKnitDyeingBatchDetail.FinishDiaID,oKnitDyeingBatchDetail.FinishGSMID,oKnitDyeingBatchDetail.Remarks,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, KnitDyeingBatchDetail oKnitDyeingBatchDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingBatchDetail]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n",
									oKnitDyeingBatchDetail.KnitDyeingBatchDetailID,oKnitDyeingBatchDetail.KnitDyeingBatchID,oKnitDyeingBatchDetail.KnitDyeingPTUID,oKnitDyeingBatchDetail.FabricTypeID,oKnitDyeingBatchDetail.GrayDiaID,oKnitDyeingBatchDetail.RollQty,oKnitDyeingBatchDetail.GrayQty,oKnitDyeingBatchDetail.ProcessLoss,oKnitDyeingBatchDetail.FinishQty,oKnitDyeingBatchDetail.FinishDiaID,oKnitDyeingBatchDetail.FinishGSMID,oKnitDyeingBatchDetail.Remarks,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_KnitDyeingBatchDetail WHERE KnitDyeingBatchDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM KnitDyeingBatchDetail");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static void DeleteList(TransactionContext tc, string sKnitDyeingBatchDetailIDs, int nKnitDyeingBatchID, Int64 nUserID)
        {
            string sSQL = "DELETE FROM KnitDyeingBatchDetail WHERE KnitDyeingBatchID = " + nKnitDyeingBatchID;
            if (sKnitDyeingBatchDetailIDs.Length > 0)
            {
                sSQL = sSQL + " AND KnitDyeingBatchDetailID NOT IN (" + sKnitDyeingBatchDetailIDs + ")";
            }
            tc.ExecuteNonQuery(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingBatchDetail WHERE KnitDyeingBatchID = %n Order By KnitDyeingBatchDetailID", id);
        } 

		#endregion
	}

}
