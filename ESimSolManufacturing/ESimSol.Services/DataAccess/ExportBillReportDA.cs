using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ExportBillReportDA
    {
        public ExportBillReportDA() { }
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_ExportBill] as ExportBill  WHERE ExportBillID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(" %q ", sSQL);
        }
        public static IDataReader GetByLDBCNo(TransactionContext tc, string sLDBCNo)
        {
            return tc.ExecuteReader("Select * from [View_ExportBill] where ExportBillID in( select ExportBillid from LDBC where LDBCNo=%s)", sLDBCNo);
        }
 
        #endregion

    }
}
