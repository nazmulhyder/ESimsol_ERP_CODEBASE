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
	public class LoanSettlementDA 
	{
		#region Insert Update Delete Function

		public static IDataReader InsertUpdate(TransactionContext tc, LoanSettlement oLoanSettlement, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sSQL)
		{
            return tc.ExecuteReader("EXEC [SP_IUD_LoanSettlement]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                    oLoanSettlement.LoanSettlementID, oLoanSettlement.LoanInstallmentID, oLoanSettlement.BankAccountID, oLoanSettlement.ExpenseHeadID, oLoanSettlement.CurrencyID, oLoanSettlement.Amount, oLoanSettlement.CRate, oLoanSettlement.AmountBC, oLoanSettlement.Remarks, nUserID, (int)eEnumDBOperation, sSQL);
		}

        public static void Delete(TransactionContext tc, LoanSettlement oLoanSettlement, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sSQL)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_LoanSettlement]"
                                    + "%n,%n,%n,%n,%n,%n,%n,%n,%s,%n,%n,%s",
                                    oLoanSettlement.LoanSettlementID, oLoanSettlement.LoanInstallmentID, oLoanSettlement.BankAccountID, oLoanSettlement.ExpenseHeadID, oLoanSettlement.CurrencyID, oLoanSettlement.Amount, oLoanSettlement.CRate, oLoanSettlement.AmountBC, oLoanSettlement.Remarks, nUserID, (int)eEnumDBOperation, sSQL);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_LoanSettlement WHERE LoanSettlementID=%n", nID);
		}
		public static IDataReader Gets(TransactionContext tc)
		{
			return tc.ExecuteReader("SELECT * FROM LoanSettlement");
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
