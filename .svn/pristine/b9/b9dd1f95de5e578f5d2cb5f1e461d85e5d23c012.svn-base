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
	public class KnitDyeingBatchDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, KnitDyeingBatch oKnitDyeingBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_KnitDyeingBatch]"
									+"%n,%n,%n,%d,%s,%n,%n,%n,%n,%n,%n,%D,%D,%n,%n,%n,%n,%n,%s,%n,%n",
                                    oKnitDyeingBatch.KnitDyeingBatchID, oKnitDyeingBatch.BUID, oKnitDyeingBatch.KnitDyingProgramID, oKnitDyeingBatch.BatchIssueDate, oKnitDyeingBatch.BatchNo, oKnitDyeingBatch.RefObjectID, oKnitDyeingBatch.ColorID, oKnitDyeingBatch.FabricID, oKnitDyeingBatch.WashTypeID, oKnitDyeingBatch.FinishedGSMID, oKnitDyeingBatch.MachineID, oKnitDyeingBatch.LoadTime, oKnitDyeingBatch.UnloadTime, oKnitDyeingBatch.MUnitID, oKnitDyeingBatch.TotalGrayQty, oKnitDyeingBatch.TotalFinishQty, oKnitDyeingBatch.ProcessLoss, oKnitDyeingBatch.ApprovedBy, oKnitDyeingBatch.Remarks, nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, KnitDyeingBatch oKnitDyeingBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_KnitDyeingBatch]"
									+"%n,%n,%n,%d,%s,%n,%n,%n,%n,%n,%n,%D,%D,%n,%n,%n,%n,%n,%s,%n,%n",
                                    oKnitDyeingBatch.KnitDyeingBatchID, oKnitDyeingBatch.BUID, oKnitDyeingBatch.KnitDyingProgramID, oKnitDyeingBatch.BatchIssueDate, oKnitDyeingBatch.BatchNo, oKnitDyeingBatch.RefObjectID, oKnitDyeingBatch.ColorID, oKnitDyeingBatch.FabricID, oKnitDyeingBatch.WashTypeID, oKnitDyeingBatch.FinishedGSMID, oKnitDyeingBatch.MachineID, oKnitDyeingBatch.LoadTime, oKnitDyeingBatch.UnloadTime, oKnitDyeingBatch.MUnitID, oKnitDyeingBatch.TotalGrayQty, oKnitDyeingBatch.TotalFinishQty, oKnitDyeingBatch.ProcessLoss, oKnitDyeingBatch.ApprovedBy, oKnitDyeingBatch.Remarks, nUserID, (int)eEnumDBOperation);
		}

        public static void Approved(TransactionContext tc, KnitDyeingBatch oKnitDyeingBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE KnitDyeingBatch SET ApprovedBy = %n WHERE KnitDyeingBatchID = %n", nUserID,  oKnitDyeingBatch.KnitDyeingBatchID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_KnitDyeingBatch WHERE KnitDyeingBatchID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_KnitDyeingBatch");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
