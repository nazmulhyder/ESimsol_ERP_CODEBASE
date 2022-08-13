using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class PTUDA
    {
        public PTUDA() { }      

        #region Get & Exist Function
        #region approve PTU
     


        #endregion

        #region Get & Exist Function
        public static double GetRawMatrialUseCapacity(TransactionContext tc, int nPTUID)
        {
            object obj = tc.ExecuteScalar("select Isnull(((JobOrderQtyApproved+ReturnQty+ShortclaimQty)-(ProductionPipeLineQty+ReadyStockInhand+ActualDeliveryQty)),0) from ProductionTracingUnit where ProductionTracingUnitID=%n", nPTUID);
            if (obj == null) return -1;
            return Convert.ToDouble(obj);
        }
        public static IDataReader GetsforWeb(TransactionContext tc, string sClause)
        {
            return tc.ExecuteReader("SELECT * FROM [View_PTUs] %q", sClause);
        }
        public static IDataReader GetsByOrder(TransactionContext tc, int nOrderID, int eOrderType)
        {
            return tc.ExecuteReader("SELECT * FROM [View_PTUs] Where OrderID=%n and OrderType=%n",  nOrderID,  (int)eOrderType);
        }
        public static IDataReader GetsByPaymentContract(TransactionContext tc, int nPaymentContractID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_PTUs]  where OrderType=2 and OrderID in  (select SampleOrder.SampleOrderID from SampleOrder  where SampleOrder.ContractID=%n)", nPaymentContractID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader( sSQL);
        }

        public static IDataReader GetsRunningPTUByBuyer(TransactionContext tc, int nContractorID)
        {
            //Job=DyeingExecutionOrder
            //return tc.ExecuteReader("SELECT * FROM [View_PTUs] WHERE OrderType=3 AND OrderID IN (SELECT JobID FROM View_Job WHERE IsRunning=1 AND ContractorID=%n)  ORDER BY OrderID, ProductID", nContractorID);
            return tc.ExecuteReader("SELECT * FROM [View_PTUs] WHERE  OrderID IN (SELECT DEOID FROM DyeingExecutionOrder WHERE ContractorID=%n)  ORDER BY OrderID, ProductID", nContractorID);
        }

        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM [View_PTUs] WHERE PTUID=%n", nID);
        }

        public static IDataReader GetRunningOrderPTU(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_ExtendedPTU] '" + sSQL + "'");
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


        public static IDataReader GetsByIDs(TransactionContext tc, string sFactIDs, string sBuyIDs, string sProdIDs)
        {
            //return tc.ExecuteReader("SELECT * FROM View_PTUs WHERE OrderType=3 and OrderID in (SELECT JobID FROM Job WHERE IsRunning=1) and  (FactoryID in (%q) or BuyerID in (%q)) AND ProductID IN (%q) Order by BuyerID,FactoryID,EWYDLColorNo", sFactIDs, sBuyIDs, sProdIDs);
            return tc.ExecuteReader("SELECT * FROM View_PTUs WHERE OrderID in (SELECT DEOID FROM DyeingExecutionOrder) and  (FactoryID in (%q) or BuyerID in (%q)) AND ProductID IN (%q) Order by BuyerID,FactoryID,EWYDLColorNo", sFactIDs, sBuyIDs, sProdIDs);
        }

        //Added by Fauzul on 4 June 2013
        public static IDataReader GetSampleOrderDetails(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("EXEC [SP_SampleOrderDetails]" + sSQL);
        }

        // Added By Sagor on 20 Jul 2014 to get order
        public static int GetOrder(TransactionContext tc, int PTUID)
        {
            object obj = tc.ExecuteScalar("Select OrderID from ProductionTracingUnit Where ProductionTracingUnitID="+PTUID+"");
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        #endregion

        #region Update for JobOpening
        // Added By Sagor on 20 Jul 2014 to get order
        public static IDataReader UpdateJobOpening(TransactionContext tc, int nPTUID, double nRSInHand, double nActualDeliveryQty, string sPTUDs, Int64 nUserID)
        {
            return tc.ExecuteReader("exec [dbo].[SP_Process_JobOpening]" + "%n,%n,%n,%s,%n", nPTUID, nRSInHand, nActualDeliveryQty, sPTUDs, nUserID);
        }
        #endregion
        #endregion
    }
}
