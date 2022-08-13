using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.Services.DataAccess.ReportingDA
{
    public class LotStockReportDA
    {
        #region Get & Exist Function
        public static IDataReader GetsRPTLot(TransactionContext tc, LotStockReport oLotStockReport)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_Lot] " + " %n, %s", oLotStockReport.BUID, oLotStockReport.SearchingCriteria);
        }
        public static IDataReader GetsAll_RPTLot(TransactionContext tc, LotStockReport oLotStockReport, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC  [SP_RPT_ALL_Lot] " + " %n, %s,%s, %s, %s, %n", oLotStockReport.BUID, oLotStockReport.SearchingCriteria, oLotStockReport.Params, oLotStockReport.LotNo, oLotStockReport.OrderNo, nUserID);
        }
        public static IDataReader Gets_StockWiseLotReport(TransactionContext tc, DateTime startdate, DateTime endate, string WUIDs, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_StoreWiseTransection] " + "%s, %d, %d", WUIDs, startdate, endate);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

    }
}
