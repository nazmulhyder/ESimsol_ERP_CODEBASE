using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNExecutionOrderDA
    {
        public FNExecutionOrderDA() { }

        #region Insert Update Delete Function
        //public static IDataReader IUD(TransactionContext tc, FNExecutionOrder oFNEO, int nDBOperation, Int64 nUserId)
        //{
        //    return tc.ExecuteReader("EXEC [SP_IUD_FNExecutionOrder]" + "%n, %n, %d, %s, %s, %s, %s, %s, %s, %n, %n, %n, %n, %n, %n, %n, %s, %n, %n, %n, %d,%s,%n,%s,%s,%s,%s,%n,%n,%b, %n, %n",
        //                            oFNEO.FNExOID, oFNEO.FEOID, oFNEO.IssueDate, oFNEO.Remark, oFNEO.Quality, oFNEO.PeachStandard, oFNEO.StyleNo, oFNEO.BuyerRef, oFNEO.OrderRef,
        //                            (int)oFNEO.OrderType, oFNEO.BuyerID, oFNEO.ContractorPersonalID, oFNEO.FabricID, oFNEO.ProcessType, oFNEO.FinishType, oFNEO.MKTPersonID, oFNEO.FinishWidth, oFNEO.Qty, oFNEO.SampleQty, oFNEO.TestQty, oFNEO.ExpectedDeliveryDate, oFNEO.TestStandard, oFNEO.FactoryID, oFNEO.Emirizing, oFNEO.GarmentsWash, oFNEO.FinalInspection, oFNEO.EndUse, oFNEO.GSM, oFNEO.LightSourceID, oFNEO.bIsRevise, nUserId, nDBOperation);
        //}
        public static IDataReader GetsReport(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
     
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNExecutionOrder WHERE FNExOID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        //public static void UndoApprove(TransactionContext tc, FNExecutionOrder oFNEO, Int64 nUserID)
        //{
        //    tc.ExecuteNonQuery("UPDATE FNExecutionOrder SET ApproveByID = 0 WHERE FNExOID = %n", oFNEO.FNExOID);
        //}
        #endregion
    }
}
