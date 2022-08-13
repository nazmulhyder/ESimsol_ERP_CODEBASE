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
	public class SampleAdjustmentDetailDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, SampleAdjustmentDetail oSampleAdjustmentDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_SampleAdjustmentDetail]"
									+"%n,%n,%n,%n,%s,%n,%n,%s",
									oSampleAdjustmentDetail.SampleAdjustmentID,oSampleAdjustmentDetail.SampleAdjustmentDetailID,oSampleAdjustmentDetail.SampleInvoiceID,oSampleAdjustmentDetail.AdjustAmount,oSampleAdjustmentDetail.Remarks,nUserID, (int)eEnumDBOperation, sDetailIDs);
		}

        public static void Delete(TransactionContext tc, SampleAdjustmentDetail oSampleAdjustmentDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDetailIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_SampleAdjustmentDetail]"
                                    + "%n,%n,%n,%n,%s,%n,%n,%s",
                                    oSampleAdjustmentDetail.SampleAdjustmentID, oSampleAdjustmentDetail.SampleAdjustmentDetailID, oSampleAdjustmentDetail.SampleInvoiceID, oSampleAdjustmentDetail.AdjustAmount, oSampleAdjustmentDetail.Remarks, nUserID, (int)eEnumDBOperation, sDetailIDs);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_SampleAdjustmentDetail WHERE SampleAdjustmentDetailID=%n", nID);
		}
		public static IDataReader Gets(int nID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_SampleAdjustmentDetail Where SampleAdjustmentID=%n", nID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
