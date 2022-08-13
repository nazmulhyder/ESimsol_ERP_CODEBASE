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
	public class KnitDyeingGrayChallanDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingGrayChallan oKnitDyeingGrayChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingGrayChallan]"
									+"%n,%n,%n,%s,%d,%n,%s,%s,%s,%n,%n",
									oKnitDyeingGrayChallan.KnitDyeingGrayChallanID,oKnitDyeingGrayChallan.KnitDyeingBatchID,oKnitDyeingGrayChallan.BUID,
                                    oKnitDyeingGrayChallan.ChallanNo,oKnitDyeingGrayChallan.ChallanDate,oKnitDyeingGrayChallan.DisburseBy,oKnitDyeingGrayChallan.Remarks,
                                    oKnitDyeingGrayChallan.TruckNo,oKnitDyeingGrayChallan.DriverName,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, KnitDyeingGrayChallan oKnitDyeingGrayChallan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingGrayChallan]"
									+"%n,%n,%n,%s,%d,%n,%s,%s,%s,%n,%n",
									oKnitDyeingGrayChallan.KnitDyeingGrayChallanID,oKnitDyeingGrayChallan.KnitDyeingBatchID,oKnitDyeingGrayChallan.BUID,
                                    oKnitDyeingGrayChallan.ChallanNo,oKnitDyeingGrayChallan.ChallanDate,oKnitDyeingGrayChallan.DisburseBy,oKnitDyeingGrayChallan.Remarks,
                                    oKnitDyeingGrayChallan.TruckNo,oKnitDyeingGrayChallan.DriverName,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM KnitDyeingGrayChallan WHERE KnitDyeingGrayChallanID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM KnitDyeingGrayChallan");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
