using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;



namespace ESimSol.Services.DataAccess
{
    public class InventoryTrakingDA
    {
        public InventoryTrakingDA() { }

        #region Update Function
        
        public static IDataReader Gets_WU(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, int nBUID, int nTParentType, int nValueType)
        {
            return tc.ExecuteReader("EXEC [sp_InventoryTraking_WU ]" + "%D, %D, %n,%n,%n", dStartDate, dEndDate, nBUID, nTParentType, nValueType);
        }
        public static IDataReader Gets_Qty_Value(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, int nBUID, string nWorkingUnitIDs, int nCurrencyID, int nProductCategoryID)
        {
            return tc.ExecuteReader("EXEC [sp_InventoryTraking_QtyValue]" + "%D, %D, %n, %s, %n, %n", dStartDate, dEndDate, nBUID, nWorkingUnitIDs, 0, nProductCategoryID);
        }
        internal static IDataReader Gets_ITransactions(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_InventoryTransaction]" + "%D, %D, %s, %n", dStartDate, dEndDate, sSQL, 0);
        }
        public static IDataReader Gets_Product(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID)
        {
            return tc.ExecuteReader("EXEC [sp_InventoryTraking_Product]" + "%D, %D, %n,%n,%n,%n,%n,%n", dStartDate, dEndDate, nBUID, nWorkingUnitID, nTParentType, nValueType, nMUnitID,  nCurrencyID);
        }
        public static IDataReader Gets_ProductDyeing(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID)
        {
            return tc.ExecuteReader("EXEC [sp_InventoryTraking_ProductDyeing]" + "%D, %D, %n,%n,%n,%n,%n,%n", dStartDate, dEndDate, nBUID, nWorkingUnitID, nTParentType, nValueType, nMUnitID, nCurrencyID);
        }
        public static IDataReader Gets_Lot(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nProductID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID)
        {
            return tc.ExecuteReader("EXEC [sp_InventoryTraking_Lot]" + "%D, %D, %n,%n,%n,%n,%n,%n,%n", dStartDate, dEndDate, nBUID, nWorkingUnitID, nProductID, nTParentType, nValueType, nMUnitID, nCurrencyID);
        }
        public static IDataReader Gets_ForItemMovement(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut)
        {
            return tc.ExecuteReader("EXEC [SP_ItemTraking]" + "%D,%D,%s,%n", dStartDate, dEndDate, sSQL, nReportLayOut);
            
        }
        public static IDataReader Gets_ForInventorySummary(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut)
        {
            return tc.ExecuteReader("EXEC [SP_InventorySummeryReport]" + "%D,%D,%s,%n", dStartDate, dEndDate, sSQL, nReportLayOut);
        }
        public static IDataReader Gets_ForItemMovementDetails(TransactionContext tc, DateTime dStartDate, DateTime dEndDate, string sSQL, string sSQL_FindLot)
        {
            return tc.ExecuteReader("EXEC [SP_ItemTrakingDetails ]" + "%D,%D,%s,%s", dStartDate, dEndDate, sSQL, sSQL_FindLot);
        }
        public static IDataReader Gets_ProductWise(TransactionContext tc, string sProductIDs, int nBUID, DateTime dStartDate, DateTime dEndDate)
        {
            return tc.ExecuteReader("EXEC [sp_InventoryTraking_Product_Wise]" + "%s,%n,%D,%D", sProductIDs, nBUID, dStartDate, dEndDate);
        }
        #endregion

      
    }
}