using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class FabricExecutionOrderNoteDA
    {
        public FabricExecutionOrderNoteDA() { }

        #region IUD
        public static IDataReader IUD(TransactionContext tc, FabricExecutionOrderNote oFEONote, int nDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricExecutionOrderNote] %n,%n,%s,%n,%n",
            oFEONote.FEONID, oFEONote.FEOID, oFEONote.Note, nUserId, nDBOperation);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFEONID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM FabricExecutionOrderNote WHERE FEONID=%n", nFEONID);
        }
        public static IDataReader Gets(TransactionContext tc, int nFEOID, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM FabricExecutionOrderNote WHERE FEOID=%n", nFEOID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL, Int64 nUserId)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

    }
}
