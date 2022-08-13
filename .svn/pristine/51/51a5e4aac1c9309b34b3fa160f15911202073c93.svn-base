using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportBillPendingReportDA
    {
        public ExportBillPendingReportDA() { }

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportBillMaturityReport] as ExportBillPendingReport  WHERE ExportBillPendingReportID=%n", nID);
        }
       
        public static IDataReader Gets(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportBillMaturityReport] as ExportBillPendingReport  WHERE  ExportLCID=%n ", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nID, EnumLCBillEvent eExportBillPendingReportEvent)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportBillMaturityReport] as ExportBillPendingReport  WHERE  ExportLCID=%n AND State=%n", nID, (int)eExportBillPendingReportEvent);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportBillMaturityReport] as ExportBillPendingReport where State=5 Order By [BankBranchID_Negotiation]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(" %q ", sSQL);
        }
        public static IDataReader GetsReport(TransactionContext tc, int nReportType, int nDiscountType)
        {
            return tc.ExecuteReader("EXEC [sp_ExportBillPendingReport] %n, %n", nReportType, nDiscountType);

        }
    
        #endregion

    }
}
