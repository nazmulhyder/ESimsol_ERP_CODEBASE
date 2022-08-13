using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeLoanInstallmentDA
    {
        public EmployeeLoanInstallmentDA() { }


        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EmployeeLoanInstallment oELI, int nDBOperation, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_EmployeeLoanInstallment]" + "%n, %n, %n, %n, %d, %n, %n, %n",
                oELI.ELInstallmentID, oELI.EmployeeLoanID, oELI.InstallmentAmount, oELI.InterestPerInstallment, oELI.InstallmentDate, oELI.ESDetailID, nUserID, nDBOperation);
        }

        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_EmployeeLoanInstallment WHERE EmployeeLoanInstallmentID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
