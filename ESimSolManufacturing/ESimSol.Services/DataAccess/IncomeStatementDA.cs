using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class IncomeStatementDA
    {
        public IncomeStatementDA() { }

        #region Insert Update Delete Function

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, int nBUID, DateTime dStartDate, DateTime dEndDate, int ParentHeadID, int nAccountTypeInInt)
        {
            return tc.ExecuteReader("EXEC [SP_IncomeStatement]" + "%n, %d, %d, %n, %n", nBUID, dStartDate, dEndDate, ParentHeadID, nAccountTypeInInt);
        }
        public static IDataReader ProcessIncomeStatement(TransactionContext tc, DateTime dStartDate, DateTime dEndDate)
        {
            return tc.ExecuteReader("EXEC [SP_Process_IncomeStatement]" + "%d, %d", dStartDate, dEndDate);
        }
        #endregion
    }
}
