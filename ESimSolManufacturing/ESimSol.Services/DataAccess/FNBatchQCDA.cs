using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNBatchQCDA
    {
        public FNBatchQCDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FNBatchQC oFNBatchQC, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNBatchQC] %n, %n, %n, %d, %d, %n, %n, %n, %n", oFNBatchQC.FNBatchQCID, oFNBatchQC.FNBatchID, oFNBatchQC.Qty, oFNBatchQC.StartTime, oFNBatchQC.EndTime, oFNBatchQC.QCInchargeID, oFNBatchQC.ActualWidth, nUserID, nDBOperation);
        }

        public static IDataReader SaveProcess(TransactionContext tc, int nFNBatchID, string sRollNo, int nRollCountStart, int nQCLotItem, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Process_FNBatchQC] %n, %s, %n, %n, %n", nFNBatchID, sRollNo, nRollCountStart, nQCLotItem, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFNBatchQCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNBatchQC WHERE FNBatchQCID=%n", nFNBatchQCID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
