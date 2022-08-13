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
	public class FNProductionBatchDA 
	{
		#region Insert Update Delete Function
		public static IDataReader InsertUpdate(TransactionContext tc, FNProductionBatch oFNProductionBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FNProductionBatch]"
                                     + "%n,%n,%n,%n,%n,  %s,%s,  %n,%n,%n, %n,%n, %n,%n,%n,%n,%s,%n,%n,%n,%n,%n,%n,%n, %n, %n, %b, %n",
                                    oFNProductionBatch.FNPBatchID, oFNProductionBatch.FNProductionID, oFNProductionBatch.FNBatchID, oFNProductionBatch.StartQty, oFNProductionBatch.EndQty,
                                    oFNProductionBatch.StartDateTime.ToString("dd MMM yyyy HH:mm:ss"), oFNProductionBatch.EndDateTime.ToString("dd MMM yyyy HH:mm:ss"),
                                    oFNProductionBatch.StartBatcherID, oFNProductionBatch.EndBatcherID, oFNProductionBatch.MachineSpeed, oFNProductionBatch.StartWidth, oFNProductionBatch.EndWidth, oFNProductionBatch.FlameIntensity, oFNProductionBatch.FlamePosition, oFNProductionBatch.Pressure_Bar, oFNProductionBatch.Temp_C, oFNProductionBatch.Remark, oFNProductionBatch.Ref_FNPBatchID, nUserID, (int)eEnumDBOperation, oFNProductionBatch.ShadeID, oFNProductionBatch.ShiftID, oFNProductionBatch.PH, oFNProductionBatch.DeriveFNBatchID, oFNProductionBatch.FNBatchCardID, oFNProductionBatch.FNMachineID, oFNProductionBatch.IsProduction, oFNProductionBatch.FNTreatmentSubProcessID);
        }

		public static void Delete(TransactionContext tc, FNProductionBatch oFNProductionBatch, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_FNProductionBatch]"
                                   + "%n,%n,%n,%n,%n,  %s,%s,  %n,%n,%n, %n,%n, %n,%n,%n,%n,%s,%n,%n,%n,%n,%n,%n,%n, %n, %n, %b, %n",
                                    oFNProductionBatch.FNPBatchID, oFNProductionBatch.FNProductionID, oFNProductionBatch.FNBatchID, oFNProductionBatch.StartQty, oFNProductionBatch.EndQty,
                                    oFNProductionBatch.StartDateTime.ToString("dd MMM yyyy HH:mm:ss"), oFNProductionBatch.EndDateTime.ToString("dd MMM yyyy HH:mm:ss"),
                                    oFNProductionBatch.StartBatcherID, oFNProductionBatch.EndBatcherID, oFNProductionBatch.MachineSpeed, oFNProductionBatch.StartWidth, oFNProductionBatch.EndWidth, oFNProductionBatch.FlameIntensity, oFNProductionBatch.FlamePosition, oFNProductionBatch.Pressure_Bar, oFNProductionBatch.Temp_C, oFNProductionBatch.Remark, oFNProductionBatch.Ref_FNPBatchID, nUserID, (int)eEnumDBOperation, oFNProductionBatch.ShadeID, oFNProductionBatch.ShiftID, oFNProductionBatch.PH, oFNProductionBatch.DeriveFNBatchID, oFNProductionBatch.FNBatchCardID, oFNProductionBatch.FNMachineID, oFNProductionBatch.IsProduction, oFNProductionBatch.FNTreatmentSubProcessID);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNProductionBatch WHERE FNPBatchID = %n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM FNProductionBatch");
		} 
        public static IDataReader Gets(int id, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FNProductionBatch WHERE FNProductionID=%n",id);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
