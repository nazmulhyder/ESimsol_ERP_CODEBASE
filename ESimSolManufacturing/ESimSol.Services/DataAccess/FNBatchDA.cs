using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNBatchDA
    {
        public FNBatchDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FNBatch oFNBatch, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNBatch] %n, %n, %n, %n, %n, %n, %d, %d, %n, %s, %n, %n", oFNBatch.FNBatchID, oFNBatch.FNExOID, oFNBatch.Qty, (int)oFNBatch.FNBatchStatus, oFNBatch.GLM, oFNBatch.FNPPID, oFNBatch.IssueDate, oFNBatch.ExpectedDeliveryDate, oFNBatch.GreyGSM, oFNBatch.Note, nUserID, nDBOperation);
        }
        public static IDataReader SaveNote(TransactionContext tc, string sSQL, Int64 nUserID)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader TransferFNBatchCard(TransactionContext tc, FNBatch oFNBatch, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_TransferFNBatchCard]" + "%n,%n,%b,%n,%n", oFNBatch.FNBatchID, oFNBatch.FNPPID, oFNBatch.IsFullTransfer, nUserID, oFNBatch.OutQty);
        }
        public static IDataReader Get(TransactionContext tc, int nFNBatchID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNBatch WHERE FNBatchID=%n", nFNBatchID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
