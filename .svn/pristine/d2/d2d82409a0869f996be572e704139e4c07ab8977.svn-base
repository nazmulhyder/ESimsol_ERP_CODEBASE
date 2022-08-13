using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PTUTrackerDA
    {
        public PTUTrackerDA() { }      

        #region Get & Exist Function
      

        #region Get & Exist Function
        public static double GetRawMatrialUseCapacity(TransactionContext tc, int nPTUTrackerID)
        {
            object obj = tc.ExecuteScalar("select Isnull(((JobOrderQtyApproved+ReturnQty+ShortclaimQty)-(ProductionPipeLineQty+ReadyStockInhand+ActualDeliveryQty)),0) from ProductionTracingUnit where ProductionTracingUnitID=%n", nPTUTrackerID);
            if (obj == null) return -1;
            return Convert.ToDouble(obj);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL, int nReportType, int nViewType, int nOrderType)
        {
            if (nOrderType == (int)(EnumOrderType.SampleOrder))
            {
                return tc.ExecuteReader("EXEC [RPT_PTUTracker_Sample] '" + sSQL + "'," + nReportType + "," + nViewType);
            }
            else
            {
                return tc.ExecuteReader("EXEC [RPT_PTUTracker] '" + sSQL + "'," + nReportType + "," + nViewType);
            }

        }

        public static IDataReader GetsYetTOPro(TransactionContext tc, int nReportType, int nViewType)
        {
            return tc.ExecuteReader("SELECT * FROM [View_PTUTrackers] WHERE  OrderID IN (SELECT DEOID FROM DyeingExecutionOrder WHERE ContractorID=%n)  ORDER BY OrderID, ProductID", nReportType, nViewType);
        }

        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_PTUTrackers] WHERE PTUTrackerID=%n", nID);
        }

        public static IDataReader GetRunningOrderPTUTracker(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_ExtendedPTUTracker] '" + sSQL + "'");
        }

        public static IDataReader JobTracker(TransactionContext tc, string sSQL, int bIsSearchWithDeyingOrderNotIssue, bool bIncludingAdjustment)
        {
            return tc.ExecuteReader("EXEC [JobTracker]" + "%s,%n,%b", sSQL, bIsSearchWithDeyingOrderNotIssue,  bIncludingAdjustment);
        }
        public static IDataReader JobTracker_Mkt(TransactionContext tc, string sSQL, int bIsSearchWithDeyingOrderNotIssue, bool bIncludingAdjustment)
        {
            return tc.ExecuteReader("EXEC [JobTracker_MKT]" + "%s,%n,%b", sSQL, bIsSearchWithDeyingOrderNotIssue, bIncludingAdjustment);
        }
        public static IDataReader SampleTracker(TransactionContext tc, string sSQL, bool bIsInlabe)
        {
            return tc.ExecuteReader("EXEC [SampleTracker]" + "%s,%b", sSQL, bIsInlabe);
        }
        public static IDataReader GetSampleOrderDetails(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_SampleOrderDetails]" + sSQL);
        }

        #endregion

     
        #endregion
    }
}
