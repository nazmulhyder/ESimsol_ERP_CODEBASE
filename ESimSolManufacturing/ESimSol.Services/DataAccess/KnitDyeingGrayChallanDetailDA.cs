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
	public class KnitDyeingGrayChallanDetailDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingGrayChallanDetail oKnitDyeingGrayChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnitDyeingGrayChallanDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingGrayChallanDetail]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
									oKnitDyeingGrayChallanDetail.KnitDyeingGrayChallanDetailID,oKnitDyeingGrayChallanDetail.KnitDyeingGrayChallanID,
                                    oKnitDyeingGrayChallanDetail.KnitDyeingBatchDetailID,oKnitDyeingGrayChallanDetail.GrayFabricID,oKnitDyeingGrayChallanDetail.StoreID,
                                    oKnitDyeingGrayChallanDetail.LotID,oKnitDyeingGrayChallanDetail.MUnitID,oKnitDyeingGrayChallanDetail.Qty,
                                    oKnitDyeingGrayChallanDetail.Remarks,nUserID, (int)eEnumDBOperation, sKnitDyeingGrayChallanDetailIDs);
		}

        public static void Delete(TransactionContext tc, KnitDyeingGrayChallanDetail oKnitDyeingGrayChallanDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sKnitDyeingGrayChallanDetailIDs)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingGrayChallanDetail]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
									oKnitDyeingGrayChallanDetail.KnitDyeingGrayChallanDetailID,oKnitDyeingGrayChallanDetail.KnitDyeingGrayChallanID,
                                    oKnitDyeingGrayChallanDetail.KnitDyeingBatchDetailID,oKnitDyeingGrayChallanDetail.GrayFabricID,oKnitDyeingGrayChallanDetail.StoreID,
                                    oKnitDyeingGrayChallanDetail.LotID,oKnitDyeingGrayChallanDetail.MUnitID,oKnitDyeingGrayChallanDetail.Qty,
                                    oKnitDyeingGrayChallanDetail.Remarks, nUserID, (int)eEnumDBOperation, sKnitDyeingGrayChallanDetailIDs);
		}
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM KnitDyeingGrayChallanDetail WHERE KnitDyeingGrayChallanDetailID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM KnitDyeingGrayChallanDetail");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
