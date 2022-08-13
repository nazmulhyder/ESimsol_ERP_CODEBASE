using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class EmployeeLoanInfoDA
    {
        public EmployeeLoanInfoDA() { }

        #region Get Functions
        public static IDataReader Gets(TransactionContext tc, DateTime dtFrom, DateTime dtTo)
        {
            return tc.ExecuteReader("SP_Rpt_EmployeeLoanInfo %d, %d", dtFrom, dtTo);
        }

        #endregion
    }
}
