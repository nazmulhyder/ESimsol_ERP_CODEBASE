using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class ExportLCReportDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportLCReport ORDER BY ExportLCID, VersionNo, ExportPIID");
        }
        public static IDataReader Get(TransactionContext tc, long nExportPIID)
        {
          return tc.ExecuteReader("SELECT * FROM View_ExportLCReport where ExportPIID=%n  ORDER BY ExportLCID, VersionNo, ExportPIID", nExportPIID);
        }      
        public static IDataReader Gets(TransactionContext tc, string sSQL, EnumLCReportLevel eLCReportLevel)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_ExportLCReport]" + " %s, %n", sSQL, (int)eLCReportLevel);
        }
        public static IDataReader GetsReportProduct(TransactionContext tc, ExportLCReport oExportLCReport)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_ExportLCProduct]" + " %n, %n, %n, %n", oExportLCReport.BUID, oExportLCReport.LCReportTypeInt, oExportLCReport.LCReportLevelInt, oExportLCReport.DateYear);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsReport(TransactionContext tc, string sSQL, int nReportType, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_ExportLCReportTwo] '" + sSQL + "','" + nReportType + "'");
        }
        #endregion
    }
}
