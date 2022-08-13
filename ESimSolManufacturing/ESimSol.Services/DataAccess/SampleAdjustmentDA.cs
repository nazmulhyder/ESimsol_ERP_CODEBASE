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
	public class SampleAdjustmentDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, SampleAdjustment oSampleAdjustment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_SampleAdjustment]"
									+"%n,%s,%n,%n,%n,%n,%d,%n,%s,%n,%n,%n",
									oSampleAdjustment.SampleAdjustmentID,oSampleAdjustment.AdjustmentNo,oSampleAdjustment.BUID,oSampleAdjustment.SANID,oSampleAdjustment.ContractorID,oSampleAdjustment.CurrencyID,oSampleAdjustment.IssueDate,oSampleAdjustment.AdjustAmount,oSampleAdjustment.Remarks,oSampleAdjustment.ApprovedBy,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, SampleAdjustment oSampleAdjustment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			tc.ExecuteNonQuery("EXEC [SP_IUD_SampleAdjustment]"
									+"%n,%s,%n,%n,%n,%n,%d,%n,%s,%n,%n,%n",
									oSampleAdjustment.SampleAdjustmentID,oSampleAdjustment.AdjustmentNo,oSampleAdjustment.BUID,oSampleAdjustment.SANID,oSampleAdjustment.ContractorID,oSampleAdjustment.CurrencyID,oSampleAdjustment.IssueDate,oSampleAdjustment.AdjustAmount,oSampleAdjustment.Remarks,oSampleAdjustment.ApprovedBy,nUserID, (int)eEnumDBOperation);
		}

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_SampleAdjustment WHERE SampleAdjustmentID=%n", nID);
		}
        public static IDataReader Process(int BUID, TransactionContext tc)
		{

          return  tc.ExecuteReader("EXEC [SP_ProcessSampleAdjustment]"+ "%n",BUID);
        }
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_SampleAdjustment WHERE ISNULL(ApprovedBy,0)=0");
        } 
		#endregion
	}

}
