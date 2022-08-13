using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class RptStockReportSimplifiedDA
    {
        public RptStockReportSimplifiedDA() { }

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, string sSQL, DateTime StartTime, DateTime EndTime, int StoreID)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_StockReportSimplified]" + "%s,%D,%D,%n", sSQL, StartTime, EndTime, StoreID);
        }

        #endregion
    }
}
