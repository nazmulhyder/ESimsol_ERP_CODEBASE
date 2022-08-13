using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FPMgtReportDA
    {
        public FPMgtReportDA() { }

        #region Get & Exist Function

        public static IDataReader GetsFPMgtReports(TransactionContext tc, DateTime PositionDate, int CurrencyID, bool IsApproved, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_FPMgtReport]" + " %d,%n,%b", PositionDate, CurrencyID, IsApproved);
        }
       
        
        #endregion
    }
}
