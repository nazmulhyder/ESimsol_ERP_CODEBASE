using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class PTUTransectionDA
    {
        public PTUTransectionDA() { }

        #region PTU Transection Function
        //
        public static void UpdatePTUTransaction(TransactionContext tc, PTUTransection oPTUTransection)
        {
            tc.ExecuteNonQuery("Update PTUTransaction SET Quantity = %n WHERE PTUTransectionID = %n", oPTUTransection.Quantity, oPTUTransection.PTUTransectionID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader GetsDataSet(TransactionContext tc, DateTime StartDate, DateTime EndDate, int BUID, int ProductionUnitID)
        {
            return tc.ExecuteReader("Exec [SP_DailyGUProductionReport]" + "%d,%d, %n, %n", StartDate, EndDate, BUID, ProductionUnitID);
        }

        public static IDataReader GetPTUTransactionHistory(TransactionContext tc, int nGUProductionOrderID, int nProductionStepID)
        {
            //return tc.ExecuteReader("EXEC [SP_GetPTUTransactionHistory] %n,%n", nGUProductionOrderID, nProductionStepID);
            return tc.ExecuteReader("SELECT SUM(PTUT.Quantity) AS Quantity, 	PTUT.GUProductionTracingUnitDetailID,	 PTUT.ColorID, PTUT.SizeID, PTUT.ColorName		FROM View_PTUTransaction AS PTUT WHERE PTUT.ProductionStepID=" + nProductionStepID + "	AND PTUT.GUProductionOrderID = " + nGUProductionOrderID + " GROUP BY  GUProductionTracingUnitDetailID,  ColorID, SizeID, ColorName");
            
        }

        public static IDataReader GetPTUTransactionBYIDs(TransactionContext tc, string sPTUTransactionIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_PTUTransaction WHERE PTUTransectionID IN(SELECT * FROM dbo.SplitInToDataSet(%s,','))", sPTUTransactionIDs);
        }
        public static IDataReader GetBuyPTUDetailID(TransactionContext tc, int nGUProductionTracingUnitDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PTUTransaction WHERE GUProductionTracingUnitDetailID = %n ", nGUProductionTracingUnitDetailID);
        }
        public static IDataReader GetPTUViewDetails(TransactionContext tc, int nProductionStepID, DateTime dOperationDate, int nExcutionFatoryId, int nGUProductionOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PTUTransaction AS PTUT WHERE PTUT.ProductionStepID=%n AND PTUT.GUProductionOrderID=%n AND CONVERT(DATE,CONVERT(VARCHAR(12),PTUT.OperationDate,106))=CONVERT(DATE,CONVERT(VARCHAR(12),%d,106)) AND PTUT.ExecutionFactoryID=%n ORDER BY PTUTransectionID", nProductionStepID,  nGUProductionOrderID, dOperationDate, nExcutionFatoryId);
        }
        public static IDataReader GetPTUViewDetails(TransactionContext tc, int nPTUDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_PTUTransaction AS PTUT WHERE PTUT.GUProductionTracingUnitDetailID=%n ORDER BY PTUTransectionID", nPTUDetailID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets_byPOIDs(TransactionContext tc, string sPOIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_PTUTransaction AS PTUT WHERE  PTUT.GUProductionOrderID IN(" + sPOIDs + ") ORDER BY PTUTransectionID");
        }
        
        #endregion
    }
}
