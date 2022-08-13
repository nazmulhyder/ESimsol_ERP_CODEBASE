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
	public class KnitDyeingBatchGrayChallanDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingBatchGrayChallan oKDBGC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingBatchGrayChallan]"
                 +"%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%n",
                 oKDBGC.KnitDyeingBatchGrayChallanID, oKDBGC.KnitDyeingBatchDetailID, oKDBGC.GrayFabricID, oKDBGC.StoreID, oKDBGC.LotID, oKDBGC.MUnitID, oKDBGC.Qty,
                 oKDBGC.Remarks, oKDBGC.DisburseBy, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, KnitDyeingBatchGrayChallan oKDBGC, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingBatchGrayChallan]"
                +"%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%n",
                oKDBGC.KnitDyeingBatchGrayChallanID, oKDBGC.KnitDyeingBatchDetailID, oKDBGC.GrayFabricID, oKDBGC.StoreID, oKDBGC.LotID, oKDBGC.MUnitID, oKDBGC.Qty,
                oKDBGC.Remarks, oKDBGC.DisburseBy, nUserID, (int)eEnumDBOperation);
		}
        public static void DeleteList(TransactionContext tc, string sKnitDyeingBatchGrayChallanIDs, int nKnitDyeingBatchID, Int64 nUserID)
        {
            string sSQL = "DELETE FROM KnitDyeingBatchGrayChallan WHERE KnitDyeingBatchDetailID IN (SELECT KnitDyeingBatchDetailID FROM KnitDyeingBatchDetail WHERE KnitDyeingBatchID = " + nKnitDyeingBatchID + ")";
            if (sKnitDyeingBatchGrayChallanIDs.Length > 0)
            {
                sSQL = sSQL + " AND KnitDyeingBatchGrayChallanID NOT IN (" + sKnitDyeingBatchGrayChallanIDs + ")";
            }
            tc.ExecuteNonQuery(sSQL);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_KnitDyeingBatchGrayChallan WHERE KnitDyeingBatchGrayChallanID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM View_KnitDyeingBatchGrayChallan");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
