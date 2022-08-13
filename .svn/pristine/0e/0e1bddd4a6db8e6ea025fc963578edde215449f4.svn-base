using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class SalesStatementDA
    {
        public SalesStatementDA() { }
        public static IDataReader GetsSalesStatement(TransactionContext tc, int BUID, DateTime StartDate, DateTime EndDate)
        {
            return tc.ExecuteReader("EXEC [SP_SalesStatement] " + " %n, %d, %d  ",  BUID, StartDate, EndDate);
        }
        public static IDataReader Gets_Summary(TransactionContext tc,int nBUID)
        {
            return tc.ExecuteReader("EXEC [SP_SalesStatement_ExportBill] " + " %n ", nBUID);
        }

        public static IDataReader Gets_BillRealize(TransactionContext tc, int nBUID, DateTime dStartDate, DateTime dEndDate)
        {
            return tc.ExecuteReader("EXEC [SP_SalesStatement_BillRealize] " + "%n, %d, %d  ", nBUID, dStartDate, dEndDate);
        }
        public static IDataReader Gets_BillMaturity(TransactionContext tc, int nBUID, DateTime dStartDate, DateTime dEndDate)
        {
            return tc.ExecuteReader("EXEC [SP_SalesStatement_BillMaturity] " + "%n, %d, %d  ", nBUID, dStartDate, dEndDate);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
    }
}
