using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
using System.Collections.Generic;

namespace ESimSol.Services.DataAccess
{
    public class ExportOutstandingDetailDA
    {
        public ExportOutstandingDetailDA() { }

        #region Get & Exist Function
      

     
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportBillMaturityReport] as ExportOutstandingDetail where State=5 Order By [BankBranchID_Negotiation]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(" %q ", sSQL);
        }
        public static IDataReader GetsReport(TransactionContext tc, int nReportType, int nBUID, DateTime dFromDate, DateTime dToDate, int nDiscountType)
        {
            return tc.ExecuteReader("EXEC [SP_ExportOutStandingDetail] %n, %n, %d, %d,%n", nReportType, nBUID, dFromDate, dToDate, nDiscountType);
        }

        #endregion

    }
}
