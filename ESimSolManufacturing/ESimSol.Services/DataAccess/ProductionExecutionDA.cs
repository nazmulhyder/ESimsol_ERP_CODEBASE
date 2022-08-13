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
    public class ProductionExecutionDA
    {
        public ProductionExecutionDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ProductionExecution oProductionExecution, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ProductionExecution]"
                                    + "%n,%n, %n, %n, %n, %n, %d, %n, %n",
                                    oProductionExecution.ProductionExecutionID, oProductionExecution.ProductionProcedureID, oProductionExecution.ProductionStepID, oProductionExecution.ExecutionQty, oProductionExecution.YetToExecution, oProductionExecution.ProductID, oProductionExecution.ExecutionDate, (int)eEnumDBOperation, nUserId);
        }

        public static void Delete(TransactionContext tc, ProductionExecution oProductionExecution, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ProductionExecution]"
                                    + "%n,%n, %n, %n, %n, %n, %d, %n, %n",
                                    oProductionExecution.ProductionExecutionID, oProductionExecution.ProductionProcedureID, oProductionExecution.ProductionStepID, oProductionExecution.ExecutionQty, oProductionExecution.YetToExecution, oProductionExecution.ProductID, oProductionExecution.ExecutionDate, (int)eEnumDBOperation, nUserId);
        }
       
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionExecution WHERE ProductionExecutionID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionExecution ORDER BY ProductionExecutionID");
        }
        public static IDataReader Gets(TransactionContext tc, int nPSID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ProductionExecution WHERE ProductionSheetID =%n ORDER BY Sequence ASC", nPSID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
   
}
