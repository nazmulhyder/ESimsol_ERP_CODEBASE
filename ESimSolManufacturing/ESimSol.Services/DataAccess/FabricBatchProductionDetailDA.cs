using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class FabricBatchProductionDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchProductionDetail oFabricBatchProductionDetail, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchProductionDetail]"
                                    + "%n,%n,%n,%n,%D,%n,%s,%n,%n,%n",
                                    oFabricBatchProductionDetail.FBPDetailID, oFabricBatchProductionDetail.FBPID, oFabricBatchProductionDetail.EmployeeID, oFabricBatchProductionDetail.ShiftID, oFabricBatchProductionDetail.ProductionDate, oFabricBatchProductionDetail.NoOfBreakage, oFabricBatchProductionDetail.Note, oFabricBatchProductionDetail.NoOfFrame, nUserID, nEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchProductionDetail oFabricBatchProductionDetail, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchProductionDetail]"
                                   + "%n,%n,%n,%n,%D,%n,%s,%n,%n,%n",
                                   oFabricBatchProductionDetail.FBPDetailID, oFabricBatchProductionDetail.FBPID, oFabricBatchProductionDetail.EmployeeID, oFabricBatchProductionDetail.ShiftID, oFabricBatchProductionDetail.ProductionDate, oFabricBatchProductionDetail.NoOfBreakage, oFabricBatchProductionDetail.Note, oFabricBatchProductionDetail.NoOfFrame, nUserID, nEnumDBOperation);
        }
        public static void MultipleApprove(TransactionContext tc, String FBPDetailIDS, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE FabricBatchProductionDetail SET ApproveBy=" + nUserID + ", ApproveDate=GETDATE() WHERE FBPDetailID IN(" + FBPDetailIDS + ")");
        }
        public static void MultipleDelete(TransactionContext tc, String FBPDetailIDS, Int64 nUserID)
        {
            tc.ExecuteNonQuery("DELETE FabricBatchProductionDetail WHERE  FBPDetailID IN(" + FBPDetailIDS + ")");
        }

        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProductionDetail WHERE FBPDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFBPID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchProductionDetail WHERE FBPID=%n ", nFBPID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }


}
