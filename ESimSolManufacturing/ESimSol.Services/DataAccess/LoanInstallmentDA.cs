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
	public class LoanInstallmentDA 
	{
		#region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, LoanInstallment oLoanInstallment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
			 return tc.ExecuteReader("EXEC [SP_IUD_LoanInstallment]"
                                    + "%n, %n, %s, %d, %d, %n, %n, %d, %n, %n, %n, %d, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n",
                                    oLoanInstallment.LoanInstallmentID, oLoanInstallment.LoanID, oLoanInstallment.InstallmentNo, oLoanInstallment.InstallmentStartDate, oLoanInstallment.InstallmentDate, oLoanInstallment.PrincipalAmount, oLoanInstallment.LoanTransferTypeInt, oLoanInstallment.TransferDate, oLoanInstallment.TransferDays, oLoanInstallment.TransferInterestRate, oLoanInstallment.TransferInterestAmount, oLoanInstallment.SettlementDate, oLoanInstallment.InterestDays, oLoanInstallment.InterestRate, oLoanInstallment.InterestAmount, oLoanInstallment.LiborRate, oLoanInstallment.LiborInterestAmount, oLoanInstallment.ChargeAmount, oLoanInstallment.DiscountPaidAmount, oLoanInstallment.DiscountRcvAmount, oLoanInstallment.TotalPayableAmount, oLoanInstallment.PaidAmount, oLoanInstallment.PrincipalDeduct, oLoanInstallment.PrincipalBalance, oLoanInstallment.Remarks, nUserID, nUserID, (int)eEnumDBOperation);
		}

        public static void Delete(TransactionContext tc, LoanInstallment oLoanInstallment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
		{
            tc.ExecuteNonQuery("EXEC [SP_IUD_LoanInstallment]"
                                    + "%n, %n, %s, %d, %d, %n, %n, %d, %n, %n, %n, %d, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n",
                                    oLoanInstallment.LoanInstallmentID, oLoanInstallment.LoanID, oLoanInstallment.InstallmentNo, oLoanInstallment.InstallmentStartDate, oLoanInstallment.InstallmentDate, oLoanInstallment.PrincipalAmount, oLoanInstallment.LoanTransferTypeInt, oLoanInstallment.TransferDate, oLoanInstallment.TransferDays, oLoanInstallment.TransferInterestRate, oLoanInstallment.TransferInterestAmount, oLoanInstallment.SettlementDate, oLoanInstallment.InterestDays, oLoanInstallment.InterestRate, oLoanInstallment.InterestAmount, oLoanInstallment.LiborRate, oLoanInstallment.LiborInterestAmount, oLoanInstallment.ChargeAmount, oLoanInstallment.DiscountPaidAmount, oLoanInstallment.DiscountRcvAmount, oLoanInstallment.TotalPayableAmount, oLoanInstallment.PaidAmount, oLoanInstallment.PrincipalDeduct, oLoanInstallment.PrincipalBalance, oLoanInstallment.Remarks, nUserID, nUserID, (int)eEnumDBOperation);
        }

        public static IDataReader Approved(TransactionContext tc, LoanInstallment oLoanInstallment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LoanInstallment]"
                                   + "%n, %n, %s, %d, %d, %n, %n, %d, %n, %n, %n, %d, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n",
                                   oLoanInstallment.LoanInstallmentID, oLoanInstallment.LoanID, oLoanInstallment.InstallmentNo, oLoanInstallment.InstallmentStartDate, oLoanInstallment.InstallmentDate, oLoanInstallment.PrincipalAmount, oLoanInstallment.LoanTransferTypeInt, oLoanInstallment.TransferDate, oLoanInstallment.TransferDays, oLoanInstallment.TransferInterestRate, oLoanInstallment.TransferInterestAmount, oLoanInstallment.SettlementDate, oLoanInstallment.InterestDays, oLoanInstallment.InterestRate, oLoanInstallment.InterestAmount, oLoanInstallment.LiborRate, oLoanInstallment.LiborInterestAmount, oLoanInstallment.ChargeAmount, oLoanInstallment.DiscountPaidAmount, oLoanInstallment.DiscountRcvAmount, oLoanInstallment.TotalPayableAmount, oLoanInstallment.PaidAmount, oLoanInstallment.PrincipalDeduct, oLoanInstallment.PrincipalBalance, oLoanInstallment.Remarks, nUserID, nUserID, (int)eEnumDBOperation);
        }
		#endregion

		#region Get & Exist Function
		public static IDataReader Get(TransactionContext tc, long nID)
		{
			return tc.ExecuteReader("SELECT * FROM View_LoanInstallment WHERE LoanInstallmentID=%n", nID);
		}
		public static IDataReader Gets(int LoanID,  TransactionContext tc)
		{
            return tc.ExecuteReader("SELECT * FROM View_LoanInstallment WHERE LoanID =%n", LoanID);
		} 
		public static IDataReader Gets(TransactionContext tc, string sSQL)
		{
			return tc.ExecuteReader(sSQL);
		} 

		#endregion
	}

}
