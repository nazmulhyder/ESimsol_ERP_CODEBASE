using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ProductReconciliationReportDA
    {
        public ProductReconciliationReportDA() { }



        public static IDataReader GetsPR(TransactionContext tc, int nBUID, string sProductIDs, DateTime dStartDate, DateTime dEndDate, int nReportType, int nSortType)
        {
            return tc.ExecuteReader("EXEC [sp_ProductReconciliationReport]%s, %s,%d,%d, %n,%n", nBUID, sProductIDs, dStartDate, dEndDate.AddDays(1), nReportType, nSortType);
        }
        public static IDataReader GetsImport(TransactionContext tc, int nBUID, string sProductIDs,  int nReportType)
        {
            return tc.ExecuteReader("EXEC [sp_ProductReconciliationReport_Import] %n, %s, %n", nBUID, sProductIDs,  nReportType);
        }
        
    }
}
