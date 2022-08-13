using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricPatternDetailDA
    {
        public FabricPatternDetailDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricPatternDetail oFPDetail, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricPatternDetail] %n,%n,%b,%n,%n,%n, %s,%n,%s,%n,%n,%n,%n,%n,%n",
            oFPDetail.FPDID, oFPDetail.FPID, oFPDetail.IsWarp, oFPDetail.ProductID, oFPDetail.LabDipDetailID, oFPDetail.TwistedGroup, oFPDetail.ColorName, oFPDetail.EndsCount, oFPDetail.FPDetailIds, oFPDetail.GroupNo, oFPDetail.SetNo, oFPDetail.Value, oFPDetail.SLNo, nUserId, nDBOperation);
        }

        public static void SavePatternRepeat(TransactionContext tc, FabricPatternDetail oFPDetail, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_Fabric_PatternRepeat] %n,%s,%n,%n", oFPDetail.RepeatNo, oFPDetail.Params, oFPDetail.Count, nUserId);
        }
        public static void CopyFromWarp(TransactionContext tc, FabricPatternDetail oFPDetail, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_CopyFromWarp] %n, %s ,%n", oFPDetail.FPID, oFPDetail.FPDetailIds, nUserId);
        }
        public static void UpdateSequence(TransactionContext tc, FabricPatternDetail oFPDetail)
        {
            tc.ExecuteNonQuery("Update FabricPatternDetail SET SLNo = %n WHERE FPDID = %n", oFPDetail.SLNo, oFPDetail.FPDID);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFPDID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPatternDetail WHERE FPDID=%n", nFPDID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFPID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPatternDetail WHERE FPID=%n order by SLNo"  , nFPID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader MakeTwistedGroup(TransactionContext tc, string sFPDIDs, int nFPID, int nTwistedGroup, int nDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricPatternDetail_Twisted] %s, %n, %n, %n, %n", sFPDIDs, nFPID, nTwistedGroup, nUserID, nDBOperation);
        }
        #endregion

    }
}
