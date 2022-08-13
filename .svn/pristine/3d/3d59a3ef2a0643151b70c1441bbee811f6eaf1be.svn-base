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
    public class LoanInterestDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, LoanInterest oLoanInterest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_LoanInterest]", nUserID);
		}

        public static void Delete(TransactionContext tc, LoanInterest oLoanInterest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_LoanInterest]", nUserID);
        }

        public static IDataReader Approved(TransactionContext tc, LoanInterest oLoanInterest, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LoanInterest]", nUserID);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
            return tc.ExecuteReader("SELECT * FROM View_LoanInterest WHERE LoanInterestID=%n", nID);
		}
        public static IDataReader GetsByLoan(TransactionContext tc, int nLoanID)
		{
            return tc.ExecuteReader("SELECT * FROM View_LoanInterest WHERE LoanID =%n ORDER BY InterestEffectDate ASC", nLoanID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		}
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_LoanInterest");
        } 
		#endregion
	}

}
