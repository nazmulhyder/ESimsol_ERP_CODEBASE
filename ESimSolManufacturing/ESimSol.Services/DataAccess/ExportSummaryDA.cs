using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ExportSummaryDA
    {
        public static IDataReader GetsExportSummary(TransactionContext tc, ExportSummary oExportSummary)
        {
            return tc.ExecuteReader("EXEC [SP_ExportSummery]" + "%n, %n, %n, %n, %d, %d, %s, %s",
                                   oExportSummary.BUID, oExportSummary.ReportNameInt, oExportSummary.ReportTypeInt, oExportSummary.ReportlayoutInt, oExportSummary.StartDate, oExportSummary.EndDate, oExportSummary.CompareFromMonths, oExportSummary.CompareWithMonths);
        }
    }
}
