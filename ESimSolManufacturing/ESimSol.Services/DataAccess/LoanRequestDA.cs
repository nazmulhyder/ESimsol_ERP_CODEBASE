using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LoanRequestDA
    {
        public LoanRequestDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, LoanRequest oLoanRequest, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LoanRequest]" + "%n, %n, %n, %d, %d, %s, %n, %n, %n, %n, %n, %s, %n, %b, %n, %n", oLoanRequest.LoanRequestID, oLoanRequest.EmployeeID, oLoanRequest.LoanType, oLoanRequest.RequestDate, oLoanRequest.ExpectDate, oLoanRequest.Purpose, oLoanRequest.RequestStatus, oLoanRequest.LoanAmount, oLoanRequest.NoOfInstallment, oLoanRequest.InstallmentAmount, oLoanRequest.InterestRate, oLoanRequest.Remarks, oLoanRequest.EmployeeLoanID, oLoanRequest.IsPFLoan, nUserID, nDBOperation);
        }

        public static IDataReader Approval(TransactionContext tc, int nLoanRequestID, short nRequestStatus, string sNote, int nEmployeeLoanID, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LoanRequest_Approval]" + "%n, %n, %s, %n, %n", nLoanRequestID, nRequestStatus, sNote, nEmployeeLoanID, nUserID);
        }

        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LoanRequest WHERE LoanRequestID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
