using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class RptDUOrderStatusDA
    {
          public RptDUOrderStatusDA() { }

        #region Get & Exist Function

          public static IDataReader MailContent(TransactionContext tc,string ProductIDs, int nReportType, DateTime StartTime, DateTime EndTime, int BUID, long nUserID)
        {
            return tc.ExecuteReader("EXEC [sp_RPT_DUOrderStatus]" + "%n,%s, %D, %D,%n", BUID,ProductIDs, StartTime, EndTime,nReportType);
        }
        public static IDataReader GetsGroupBy(TransactionContext tc, string sSql)
        {
            return tc.ExecuteReader(sSql);
        }
        #endregion
    }
}
