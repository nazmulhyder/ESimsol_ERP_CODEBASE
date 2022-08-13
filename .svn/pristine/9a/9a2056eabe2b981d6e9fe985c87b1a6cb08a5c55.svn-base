using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LoanSummaryDA
    {
        public LoanSummaryDA() { }


        #region Insert Update Delete Function
        public static IDataReader Gets(TransactionContext tc, DateTime dtFrom, DateTime dtTo, string sDeptID, int nSalaryMonth, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_LoanSummary]" + " %d, %d, %s, %n", dtFrom, dtTo, sDeptID, nSalaryMonth);
        }
        #endregion

    }
}
