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
	public class StyleBudgetCMDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, StyleBudgetCM oStyleBudgetCM, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sCMDIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_StyleBudgetCM]"
									+"%n,%n,%n,%n,%n,%n,%n,%n,%s, %n,%n,%s",
									oStyleBudgetCM.StyleBudgetCMID,oStyleBudgetCM.StyleBudgetID,oStyleBudgetCM.NumberOfMachine,oStyleBudgetCM.MachineCost,oStyleBudgetCM.ProductionPerDay,oStyleBudgetCM.BufferDays,oStyleBudgetCM.TotalRequiredDays,oStyleBudgetCM.CMAdditionalPerent, oStyleBudgetCM.CMPart,  nUserID, (int)eEnumDBOperation, sCMDIDs);
		}

        public static void Delete(TransactionContext tc, StyleBudgetCM oStyleBudgetCM, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sCMDIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_StyleBudgetCM]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%s, %n,%n,%s",
                                    oStyleBudgetCM.StyleBudgetCMID, oStyleBudgetCM.StyleBudgetID, oStyleBudgetCM.NumberOfMachine, oStyleBudgetCM.MachineCost, oStyleBudgetCM.ProductionPerDay, oStyleBudgetCM.BufferDays, oStyleBudgetCM.TotalRequiredDays, oStyleBudgetCM.CMAdditionalPerent, oStyleBudgetCM.CMPart, nUserID, (int)eEnumDBOperation, sCMDIDs);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_StyleBudgetCM WHERE StyleBudgetCMID=%n", nID);
		}
		public static IDataReader Gets(int CSID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_StyleBudgetCM  WHERE StyleBudgetID=%n", CSID);
		}

        public static IDataReader GetsByLog(int CSLogID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_StyleBudgetCMLog  WHERE StyleBudgetLogID=%n", CSLogID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
