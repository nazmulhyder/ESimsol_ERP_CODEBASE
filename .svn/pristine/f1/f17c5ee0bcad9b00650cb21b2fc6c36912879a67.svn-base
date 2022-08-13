using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNExecutionOrderNoteDA
    {
        public FNExecutionOrderNoteDA() { }

        #region Insert Update Delete Function
        //public static IDataReader IUD(TransactionContext tc, FNExecutionOrderNote oFNEO, int nDBOperation, Int64 nUserId)
        //{
        //    return tc.ExecuteReader("EXEC [SP_IUD_FNExecutionOrderNote]" + "%n, %n, %s, %n, %n",
        //                            oFNEO.FNExONoteID, oFNEO.FNExOID, oFNEO.Note, nUserId, nDBOperation);
        //}

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM FNExecutionOrderNote WHERE FNExONoteID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int FNExOID)
        {
            return tc.ExecuteReader("SELECT * FROM FNExecutionOrderNote WHERE FNExOID=%n", FNExOID);
        }
        #endregion
    }
}
