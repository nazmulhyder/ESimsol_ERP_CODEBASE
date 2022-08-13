using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNOrderUpdateStatusDA 
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, string sSQL, int nType, DateTime dtStart, DateTime dtEnd)
        {
            return tc.ExecuteReader("Exec [SP_Rpt_FNOrderUpdateStatusV.2]  %s, %n, %D, %D", sSQL, nType, dtStart, dtEnd);
        }
    
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_FNOrderUpdateStatus] %s", sSQL);
        }
        public static IDataReader GetStockReport(TransactionContext tc, DateTime dtStart, DateTime dtEnd, int nReportType ,int nWorkingUnitID, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Rpt_FNOrderUpdateStatus_FabricStock] %d, %d, %n,%n,%n", dtStart, dtEnd, nReportType, nWorkingUnitID, nUserID);
        }



        
        #endregion
    }
}
