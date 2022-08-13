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
	public class CashFlowSetupDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, CashFlowSetup oCashFlowSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_CashFlowSetup]" +"%n, %n, %n, %n, %n, %s, %s, %n, %n",
									oCashFlowSetup.CashFlowSetupID,oCashFlowSetup.CFTransactionCategoryInInt,oCashFlowSetup.CFTransactionGroupInInt,oCashFlowSetup.CFDataTypeInInt,oCashFlowSetup.SubGroupID,oCashFlowSetup.DisplayCaption,oCashFlowSetup.Remarks,nUserID, (int)eEnumDBOperation);
		}

		public static void Delete(TransactionContext tc, CashFlowSetup oCashFlowSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_CashFlowSetup]" + "%n, %n, %n, %n, %n, %s, %s, %n, %n",
                                    oCashFlowSetup.CashFlowSetupID, oCashFlowSetup.CFTransactionCategoryInInt, oCashFlowSetup.CFTransactionGroupInInt, oCashFlowSetup.CFDataTypeInInt, oCashFlowSetup.SubGroupID, oCashFlowSetup.DisplayCaption, oCashFlowSetup.Remarks, nUserID, (int)eEnumDBOperation);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_CashFlowSetup WHERE CashFlowSetupID=%n", nID);
		}
        public static IDataReader Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, bool bIsDetails, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC[SP_CashFlowStatement]" + "%n,%d,%d,%b", nBUID, dStartDate, dEndDate, bIsDetails);
        } 
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_CashFlowSetup");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
