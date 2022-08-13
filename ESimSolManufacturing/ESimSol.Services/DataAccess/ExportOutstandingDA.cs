using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportOutstandingDA
    {
        public ExportOutstandingDA() { }

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, DateTime dFromDODate, DateTime dToDODate, int nTextileUnit)
        {
            return tc.ExecuteReader("EXEC [SP_ExportOutStanding]" + "%d, %d, %n", dFromDODate, dToDODate, nTextileUnit);
        }
        public static IDataReader GetsListByGroup(TransactionContext tc, ExportOutstanding oExportOutstanding)
        {
            return tc.ExecuteReader("EXEC [SP_ExportOutstandingGroupWise]" + "%n, %n, %d, %d, %n", oExportOutstanding.OperationStage, oExportOutstanding.BUID, oExportOutstanding.FromDate, oExportOutstanding.ToDate, oExportOutstanding.GroupBy);
        }
        public static IDataReader GetsListChallanDetail(TransactionContext tc, ExportOutstanding oExportOutstanding)
        {
            return tc.ExecuteReader("EXEC [SP_ExportOutstandingDetail]" + "%n, %n, %d, %d, %n, %n", oExportOutstanding.OperationStage, oExportOutstanding.BUID, oExportOutstanding.FromDate, oExportOutstanding.ToDate, oExportOutstanding.GroupBy, oExportOutstanding.TempID); //oExportOutstanding.TempID = ContractorID or BankID
        }
        #endregion
    }
}
