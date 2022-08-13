using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class FNProductionBatchTransferDetailDA 
    {
        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FNProductionBatchTransferDetail oFNPBTD, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNProductionBatchTransferDetail] "
                                    + "%n, %n ,%n ,%n", oFNPBTD.FNPBTransferDetailID, oFNPBTD.FNPBTransferID,oFNPBTD.FNPBatchID, nDBOperation);
        }
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFNPBTDID)
        {
            return tc.ExecuteReader("SELECT * FROM FNProductionBatchTransferDetail WHERE FNPBTransferDetailID=%n", nFNPBTDID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
