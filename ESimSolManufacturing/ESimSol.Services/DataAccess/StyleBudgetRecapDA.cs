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
	public class StyleBudgetRecapDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, StyleBudgetRecap oStyleBudgetRecap, EnumDBOperation eEnumDBOperation,  Int64 nUserID, string sCSRecapIDs)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_StyleBudgetRecap]"
                                    + "%n,%n,%n, %n,%s,%n,%n, %n,%n,%s",
                                    oStyleBudgetRecap.StyleBudgetRecapID, oStyleBudgetRecap.StyleBudgetID, oStyleBudgetRecap.RefType, oStyleBudgetRecap.RefID, oStyleBudgetRecap.Note, oStyleBudgetRecap.UnitPrice, oStyleBudgetRecap.Quantity,  nUserID, (int)eEnumDBOperation, sCSRecapIDs);
		}

        public static void Delete(TransactionContext tc, StyleBudgetRecap oStyleBudgetRecap, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sCSRecapIDs)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_StyleBudgetRecap]"
                                    + "%n,%n,%n, %n,%s,%n,%n, %n,%n,%s",
                                    oStyleBudgetRecap.StyleBudgetRecapID, oStyleBudgetRecap.StyleBudgetID, oStyleBudgetRecap.RefType, oStyleBudgetRecap.RefID, oStyleBudgetRecap.Note, oStyleBudgetRecap.UnitPrice, oStyleBudgetRecap.Quantity, nUserID, (int)eEnumDBOperation, sCSRecapIDs);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_StyleBudgetRecap WHERE StyleBudgetRecapID=%n", nID);
		}
		public static IDataReader Gets(int nCSID, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_StyleBudgetRecap WHERE StyleBudgetID = %n", nCSID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
