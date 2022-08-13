using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ConsumptionReportDA
    {
        public ConsumptionReportDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_ConsumptionReport] %s", sSQL);
        }

        public static IDataReader GetsConsumptionSummary(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_ConsumptionReport_CUGWise] %s", sSQL);
        }
        #endregion
    }
}
