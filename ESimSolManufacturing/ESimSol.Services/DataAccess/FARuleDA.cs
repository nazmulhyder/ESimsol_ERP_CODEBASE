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
	public class FARuleDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, FARule oFARule, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_FARule]"
                                    + "%n, %n,%n,%n,%n,%n,%n, %n,%n, %n,%n,%n ,%s,%n, %n, %n,%n",
                                    oFARule.FARuleID, oFARule.FAMethodInt, oFARule.ProductID, oFARule.DEPCalculateOnInt, oFARule.DefaultSalvage, oFARule.UseFullLife, oFARule.DefaultDepRate,  oFARule.CurrencyID, oFARule.MUnitID,
                                    oFARule.DefaultCostPrice,       oFARule.CostPriceEffectOn,       oFARule.DepEffectFormOn,       oFARule.Remarks,       oFARule.RegisterApplyOn, oFARule.IsApplyForProductBaseInt,
                                    nUserID, (int)eEnumDBOperation);
		}

        public static void Delete(TransactionContext tc, FARule oFARule, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FARule]"
                                    + "%n, %n,%n,%n,%n,%n,%n, %n,%n, %n,%n,%n ,%s,%n, %n, %n,%n",
                                    oFARule.FARuleID, oFARule.FAMethodInt, oFARule.ProductID, oFARule.DEPCalculateOnInt, oFARule.DefaultSalvage, oFARule.UseFullLife, oFARule.DefaultDepRate, oFARule.CurrencyID, oFARule.MUnitID,
                                    oFARule.DefaultCostPrice, oFARule.CostPriceEffectOn, oFARule.DepEffectFormOn, oFARule.Remarks, oFARule.RegisterApplyOn, oFARule.IsApplyForProductBaseInt,
                                    nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader GetFARegister(TransactionContext tc, FARule oFARule)
        {
            return tc.ExecuteReader("SELECT * FROM  FARegister WHERE ProductID =%n", oFARule.ProductID);
        }
        public static IDataReader Remove_FACode(TransactionContext tc, FARule oFARule)
        {
            string sSQL1 = SQLParser.MakeSQL("DELETE FACode WHERE ProductID=%n", oFARule.ProductID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_FARule WHERE ProductID=%n", oFARule.ProductID);

        }
        public static IDataReader Remove_FARule(TransactionContext tc, FARule oFARule)
        {
            return tc.ExecuteReader("EXEC [SP_Remove_FARule]" + "%n,%b,%b", oFARule.ProductID, oFARule.IsApplyForProductBase, oFARule.IsApplyForFACode);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_FARule WHERE FARuleID=%n", nID);
		}
        public static IDataReader GetByProduct(TransactionContext tc, long nPID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FARule WHERE ProductID=%n", nPID);
        }
		public static IDataReader Gets(TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_FARule");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 


       
		#endregion
	}

}
