using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricPatternDA
    {
        public FabricPatternDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricPattern oFP, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricPattern] %n,%s,%n,%n,%n,%n,%n,%n,%n,%n,%s,%s,%s,%d,%b,%n,%n",
            oFP.FPID, oFP.PatternNo, oFP.FabricID, oFP.Weave, oFP.Reed, oFP.Pick, oFP.GSM, oFP.Warp, oFP.Weft, oFP.Dent, oFP.Note, oFP.Ratio, oFP.RepeatSize, oFP.ApproveDate, oFP.IsActive, nUserId, nDBOperation);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFPID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPattern WHERE FPID=%n", nFPID);
        }
        public static IDataReader GetActiveFP(TransactionContext tc, int nFabricId, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricPattern WHERE IsActive=1 And FabricID=%n", nFabricId);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

    }
}
