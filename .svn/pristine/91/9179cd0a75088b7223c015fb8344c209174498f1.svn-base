using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ProductReconciliationReportDetailDA
    {
        public ProductReconciliationReportDetailDA() { }
                
        public static IDataReader Gets_ProductReconciliationReportDetail(TransactionContext tc, int nProductID, DateTime dStartDate, DateTime dEndDate, int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_ProductReconciliationReportDetail] '" + nProductID + "','" + dStartDate + "','" + dEndDate.AddDays(1) + "','" + nReportType + "'");
        }
      
        public static IDataReader Gets_PRDetail(TransactionContext tc, int nBUID, int nProductID, DateTime dStartDate, DateTime dEndDate, int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_ProductReconciliationReportDetail] %n, %n, %d, %d,%n", nBUID, nProductID, dStartDate, dEndDate.AddDays(1), nReportType);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        
    }
}
