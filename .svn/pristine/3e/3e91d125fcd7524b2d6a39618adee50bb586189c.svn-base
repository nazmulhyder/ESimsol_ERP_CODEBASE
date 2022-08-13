using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class RptDailyLogReportDA
    {
         public RptDailyLogReportDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_RPT_DailyLogReport");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, DateTime dtLoomStart, string sFEOIDs, string sBuyerIDs, int nFabricWeave, int nProcessType, string sConstruction, int nTsuid ,double ReedCount)
        {
            return tc.ExecuteReader("EXEC SP_Rpt_DailyLog %d, %s, %s, %n, %n, %s, %n ,%n", dtLoomStart, sFEOIDs, sBuyerIDs, nFabricWeave, nProcessType, sConstruction, nTsuid, ReedCount);
        }
        #endregion
    }
}
