using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DUOrderTrackerDA
    {
        public DUOrderTrackerDA() { }      

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, string sSQL, int nReportType)
        {
                return tc.ExecuteReader("EXEC [RPT_DUOrderTracking]'" + sSQL + "'," + nReportType );
        }
        public static IDataReader Gets_Sample(TransactionContext tc, string sSQL, int nReportType)
        {
            return tc.ExecuteReader("EXEC [RPT_DUOrderTracking_Sample]'" + sSQL + "'," + nReportType);
        }


      
     
        #endregion
    }
}
