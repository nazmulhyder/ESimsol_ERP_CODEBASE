using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{

    public class FabricBatchLoomDetailDA
    {
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchLoomDetail oFabricBatchLoomDetail, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricBatchLoomDetail]"
                                    + "%n,%n,%n,%n,%n,%d,%s,%n,%n,%n,%n,%n,%n",
                                    oFabricBatchLoomDetail.FBLDetailID, oFabricBatchLoomDetail.FabricBatchLoomID, oFabricBatchLoomDetail.EmployeeID, oFabricBatchLoomDetail.ShiftID, oFabricBatchLoomDetail.Qty, oFabricBatchLoomDetail.FinishDate.ToString("dd MMM yyyy"), oFabricBatchLoomDetail.Note, oFabricBatchLoomDetail.RPM, oFabricBatchLoomDetail.Efficiency, oFabricBatchLoomDetail.Warp, oFabricBatchLoomDetail.Weft, nUserID, nEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, EnumDBOperation nEnumDBOperation, FabricBatchLoomDetail oFabricBatchLoomDetail, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_FabricBatchLoomDetail]"
                                   + "%n,%n,%n,%n,%n,%d,%s,%n,%n,%n,%n,%n,%n",
                                   oFabricBatchLoomDetail.FBLDetailID, oFabricBatchLoomDetail.FabricBatchLoomID, oFabricBatchLoomDetail.EmployeeID, oFabricBatchLoomDetail.ShiftID, oFabricBatchLoomDetail.Qty, oFabricBatchLoomDetail.FinishDate, oFabricBatchLoomDetail.Note, oFabricBatchLoomDetail.RPM, oFabricBatchLoomDetail.Efficiency, oFabricBatchLoomDetail.Warp, oFabricBatchLoomDetail.Weft, nUserID, nEnumDBOperation);
        }
        public static void MultipleApprove(TransactionContext tc, String FBLDetailIDS, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE FabricBatchLoomDetail SET ApproveBy=" + nUserID + ", ApproveDate=GETDATE() WHERE FBLDetailID IN(" + FBLDetailIDS + ")");
        }
        public static void MultipleDelete(TransactionContext tc, String FBLDetailIDS, Int64 nUserID)
        {
            tc.ExecuteNonQuery("DELETE FabricBatchLoomDetail WHERE  FBLDetailID IN(" + FBLDetailIDS + ")");
        }

        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchLoomDetail WHERE FBLDetailID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFabricBatchLoomID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricBatchLoomDetail WHERE FabricBatchLoomID=%n  ORDER BY FinishDate DESC, FBLDetailID DESC", nFabricBatchLoomID);
        }
        public static IDataReader GetsBySql(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }


}
