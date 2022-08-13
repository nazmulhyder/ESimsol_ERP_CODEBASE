using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class FabricBatchProductionBatchManDA
    {
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchProductionBatchMan oFabricBatchProductionBatchMan, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchProductionBatchMan]"
                                    + "%n,%n,%n,%n,%n,%d,%s,%n,%n,%n,%n",
                                    oFabricBatchProductionBatchMan.FBPBID, oFabricBatchProductionBatchMan.FBPID, oFabricBatchProductionBatchMan.EmployeeID, oFabricBatchProductionBatchMan.ShiftID, oFabricBatchProductionBatchMan.Qty, oFabricBatchProductionBatchMan.FinishDate.ToString("dd MMM yyyy"), oFabricBatchProductionBatchMan.Note, oFabricBatchProductionBatchMan.RPM,oFabricBatchProductionBatchMan.Efficiency, nUserID, nEnumDBOperation);
        }

        public static void  Delete(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchProductionBatchMan oFabricBatchProductionBatchMan, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchProductionBatchMan]"
                                   + "%n,%n,%n,%n,%n,%d,%s,%n,%n,%n,%n",
                                   oFabricBatchProductionBatchMan.FBPBID, oFabricBatchProductionBatchMan.FBPID, oFabricBatchProductionBatchMan.EmployeeID, oFabricBatchProductionBatchMan.ShiftID, oFabricBatchProductionBatchMan.Qty, oFabricBatchProductionBatchMan.FinishDate, oFabricBatchProductionBatchMan.Note, oFabricBatchProductionBatchMan.RPM,oFabricBatchProductionBatchMan.Efficiency, nUserID, nEnumDBOperation);
        }
        public static void MultipleApprove(TransactionContext tc, String FBPBIDS, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE FabricBatchProductionBatchMan SET ApproveBy="+nUserID +", ApproveDate=GETDATE() WHERE FBPBID IN("+ FBPBIDS+")");
        }
        public static void MultipleDelete(TransactionContext tc, String FBPBIDS, Int64 nUserID)
        {
            tc.ExecuteNonQuery("DELETE FabricBatchProductionBatchMan WHERE  FBPBID IN(" + FBPBIDS + ")");
        }
    
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProductionBatchMan WHERE FBPBID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFBPID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProductionBatchMan WHERE FBPID=%n ", nFBPID);
        }
        public static IDataReader GetsBySql(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
   
}
