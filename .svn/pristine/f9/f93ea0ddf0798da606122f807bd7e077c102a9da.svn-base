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
	public class LoanDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, Loan oLoan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_Loan]"
                                    + "%n,%n,%n,%n,%n,%s,%d,%n,%d, %d,%n,%n,%n,%n,%n,%n,%d,%n,%d,%s,%n,%n,%d,%n,%n, %n, %n,%n",
									oLoan.LoanID,
                                    oLoan.BUID,
                                    oLoan.LoanStatusInt,
                                    oLoan.LoanRefID,
                                    oLoan.LoanTypeInt,
                                    oLoan.LoanNo,
                                    oLoan.LoanStartDate,
                                    oLoan.LoanRefTypeInt,
                                    oLoan.IssueDate,
                                    oLoan.StlmtStartDate,
                                    oLoan.CRate,
                                    oLoan.LoanCurencyID,
                                    oLoan.LoanAmount,
                                    oLoan.InterestRate,
                                    oLoan.LiborRate,
                                    oLoan.CompoundTypeInt,
                                    oLoan.ApproxSettlement,
                                    oLoan.RcvBankAccountID,
                                    oLoan.RcvDate,
                                    oLoan.LoanRemarks,
                                    oLoan.NoOfInstallment,
                                    oLoan.InstallmentCycleInt,
                                    oLoan.InstallmentStartDate,
                                    oLoan.InstallmentAmount,
                                    oLoan.ProcessFeePercent,
                                    oLoan.ProcessFeeAmount,
                                    nUserID,
                                    (int)eEnumDBOperation);
		}

     

		public static void Delete(TransactionContext tc, Loan oLoan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_Loan]"
                                    + "%n,%n,%n,%n,%n,%s,%d,%n,%d, %d,%n,%n,%n,%n,%n,%n,%d,%n,%d,%s,%n,%n,%d,%n,%n, %n, %n,%n",
                                    oLoan.LoanID,
                                    oLoan.BUID,
                                    oLoan.LoanStatusInt,
                                    oLoan.LoanRefID,
                                    oLoan.LoanTypeInt,
                                    oLoan.LoanNo,
                                    oLoan.LoanStartDate,
                                    oLoan.LoanRefTypeInt,
                                    oLoan.IssueDate,
                                    oLoan.StlmtStartDate,
                                    oLoan.CRate,
                                    oLoan.LoanCurencyID,
                                    oLoan.LoanAmount,
                                    oLoan.InterestRate,
                                    oLoan.LiborRate,
                                    oLoan.CompoundTypeInt,
                                    oLoan.ApproxSettlement,
                                    oLoan.RcvBankAccountID,
                                    oLoan.RcvDate,
                                    oLoan.LoanRemarks,
                                    oLoan.NoOfInstallment,
                                    oLoan.InstallmentCycleInt,
                                    oLoan.InstallmentStartDate,
                                    oLoan.InstallmentAmount,
                                    oLoan.ProcessFeePercent,
                                    oLoan.ProcessFeeAmount,
                                    nUserID,
                                    (int)eEnumDBOperation);
        }


        public static void UpdateStmlStartDate(TransactionContext tc, Loan oLoan, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE Loan SET StlmtStartDate = %d WHERE LoanID =%n", oLoan.StlmtStartDate, oLoan.LoanID);
        }

		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_Loan WHERE LoanID=%n", nID);
		}
		public static IDataReader Gets(int buid, TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_Loan WHERE BUID=%n", buid);
		}
        public static IDataReader Gets(EnumLoanRefType eLoanRefType, int nRefID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Loan AS HH WHERE HH.LoanRefType=%n AND HH.LoanRefID = %n ORDER BY HH.LoanID ASC", (int)eLoanRefType, nRefID);
        } 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
