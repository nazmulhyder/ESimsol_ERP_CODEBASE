using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects.ReportingObject;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class WUStockReportDA
    {
        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc, short orderType, int processType, string buyerIds, string feoIds, bool bIsCurrentStock)
        {
            return tc.ExecuteReader("Exec SP_Rpt_WeavingStock  %n, %n, %s, %s, %b", orderType, processType, buyerIds, feoIds, bIsCurrentStock);
        }
        #endregion
    }
}
