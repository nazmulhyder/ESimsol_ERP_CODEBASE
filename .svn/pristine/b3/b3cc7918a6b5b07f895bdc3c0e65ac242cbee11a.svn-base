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
	public class CostSheetCMDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, CostSheetCM oCostSheetCM, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sCMDIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CostSheetCM]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                                   oCostSheetCM.CostSheetCMID, oCostSheetCM.CostSheetID, oCostSheetCM.CMTypeInt, oCostSheetCM.NumberOfMachine, oCostSheetCM.MachineCost, oCostSheetCM.ProductionPerDay, oCostSheetCM.BufferDays, oCostSheetCM.TotalRequiredDays, oCostSheetCM.CMAdditionalPerent, oCostSheetCM.CMPart, nUserID, (int)eEnumDBOperation, sCMDIDs);
        }

        public static void Delete(TransactionContext tc, CostSheetCM oCostSheetCM, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sCMDIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CostSheetCM]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %s",
                                   oCostSheetCM.CostSheetCMID, oCostSheetCM.CostSheetID, oCostSheetCM.CMTypeInt, oCostSheetCM.NumberOfMachine, oCostSheetCM.MachineCost, oCostSheetCM.ProductionPerDay, oCostSheetCM.BufferDays, oCostSheetCM.TotalRequiredDays, oCostSheetCM.CMAdditionalPerent, oCostSheetCM.CMPart, nUserID, (int)eEnumDBOperation, sCMDIDs);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_CostSheetCM WHERE CostSheetCMID=%n", nID);
		}
		public static IDataReader Gets(int CSID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_CostSheetCM  WHERE CostSheetID=%n ORDER BY CMType ASC", CSID);
		}

        public static IDataReader GetsByLog(int CSLogID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_CostSheetCMLog  WHERE CostSheetLogID=%n", CSLogID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
