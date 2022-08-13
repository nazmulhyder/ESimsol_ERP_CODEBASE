using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeLoanAmountDA
    {
        public EmployeeLoanAmountDA() { }


        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM EmployeeLoanAmount WHERE EmployeeLoanAmountID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static Int32 GetEmployeeLoanID(TransactionContext tc, int nLoanRequestID)
        {
            string sSQL = "Select EmployeeLoanID from EmployeeLoanAmount Where LoanRequestID=" + nLoanRequestID;
            object obj = tc.ExecuteScalar(sSQL);
            return Convert.ToInt32(obj);
        }
        

        #endregion
    }
}
