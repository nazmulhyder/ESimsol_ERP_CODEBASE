using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class RptProductionMonthlyInspectionDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, DateTime dFromDate, DateTime dToDate, string sFEOIDs, string sBuyerIDs, string  sFMIDs)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_ProductionMonthlyInspection]" + "%d, %d, %s, %s, %s", dFromDate, dToDate, sFEOIDs, sBuyerIDs, sFMIDs);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
