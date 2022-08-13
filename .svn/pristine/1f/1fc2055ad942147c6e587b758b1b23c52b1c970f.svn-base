using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FabricExecutionOrderFabricDA
    {
        public FabricExecutionOrderFabricDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, FabricExecutionOrderFabric oFabricExecutionOrderFabric, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFabricID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrderFabric]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %s",
                                    oFabricExecutionOrderFabric.FEOFID, oFabricExecutionOrderFabric.FEOID, oFabricExecutionOrderFabric.ExportPIDetailID, oFabricExecutionOrderFabric.Qty, oFabricExecutionOrderFabric.FabricID, oFabricExecutionOrderFabric.FactoryID, (int)eEnumDBOperation, sFabricID, oFabricExecutionOrderFabric.OrderType, oFabricExecutionOrderFabric.FEO_BuyerID, oFabricExecutionOrderFabric.FEO_FabricID, oFabricExecutionOrderFabric.ExportPIIDs);
        }

        public static void Delete(TransactionContext tc, FabricExecutionOrderFabric oFabricExecutionOrderFabric, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sFabricID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricExecutionOrderFabric]"
                                    + "%n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %s",
                                    oFabricExecutionOrderFabric.FEOFID, oFabricExecutionOrderFabric.FEOID, oFabricExecutionOrderFabric.ExportPIDetailID, oFabricExecutionOrderFabric.Qty, oFabricExecutionOrderFabric.FabricID, oFabricExecutionOrderFabric.FactoryID, (int)eEnumDBOperation, sFabricID, oFabricExecutionOrderFabric.OrderType, oFabricExecutionOrderFabric.FEO_BuyerID, oFabricExecutionOrderFabric.FEO_FabricID, oFabricExecutionOrderFabric.ExportPIIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrderFabric WHERE FEOFID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrderFabric");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int nFEOID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricExecutionOrderFabric WHERE FEOID=%n", nFEOID);
        }

        #endregion
    }
}
