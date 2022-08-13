using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeLoanSetupDA
    {
        public EmployeeLoanSetupDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeLoanSetup oELS, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeLoanSetup]" + "%n, %n, %n, %n, %b, %b, %b, %n ,%n, %n", oELS.ELSID, oELS.DeferredDay, oELS.ActivationAfter, oELS.LimitInPercentOfPF, oELS.IsEmployeeContribution, oELS.IsCompanyContribution, oELS.IsPFProfit, oELS.SalaryHeadID, nUserID, nDBOperation);
        }

        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeLoanSetup WHERE ELSID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
