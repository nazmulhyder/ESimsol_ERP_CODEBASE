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
	public class CashFlowDmSetupDA 
	{
		#region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CashFlowDmSetup oCashFlowDmSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CashFlowDmSetup]" + "%n, %n, %n, %b, %s, %n, %n",
                                    oCashFlowDmSetup.CashFlowDmSetupID, oCashFlowDmSetup.CashFlowHeadID, oCashFlowDmSetup.SubGroupID, oCashFlowDmSetup.IsDebit, oCashFlowDmSetup.Remarks, nUserID, (int)eEnumDBOperation);
        }
		public static void Delete(TransactionContext tc, CashFlowDmSetup oCashFlowDmSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_CashFlowDmSetup]" + "%n, %n, %n, %b, %s, %n, %n",
                                    oCashFlowDmSetup.CashFlowDmSetupID, oCashFlowDmSetup.CashFlowHeadID, oCashFlowDmSetup.SubGroupID, oCashFlowDmSetup.IsDebit, oCashFlowDmSetup.Remarks, nUserID, (int)eEnumDBOperation);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_CashFlowDmSetup WHERE CashFlowDmSetupID=%n", nID);
		}
        public static IDataReader Gets(int nBUID, DateTime dStartDate, DateTime dEndDate, bool bIsDetails, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC[SP_CashFlowDmStatement]" + "%n,%d,%d,%b", nBUID, dStartDate, dEndDate, bIsDetails);
        } 
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_CashFlowDmSetup ORDER BY Sequence, CashFlowDmSetupID ASC");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 
		#endregion
	}

}
