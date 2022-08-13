using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class FNProductionBatchTransferDA
    {
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FNProductionBatchTransfer oFNPBT, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNProductionBatchTransfer] "
                                    + "%n,%s ,%n ,%n , %n, %n ,%n", oFNPBT.FNPBTransferID, oFNPBT.TransferNote, (int)oFNPBT.FNTreatment,  nUserID,nUserID,nUserID, nDBOperation);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFNPBTID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNProductionBatchTransfer WHERE FNPBTransferID=%n", nFNPBTID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
