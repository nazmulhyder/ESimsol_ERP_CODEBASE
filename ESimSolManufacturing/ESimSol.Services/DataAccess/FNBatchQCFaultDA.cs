using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class FNBatchQCFaultDA
    {
        public FNBatchQCFaultDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, FNBatchQCFault oFNBatchQCFault, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FNBatchQCFault] %n, %n, %n, %n, %n, %n, %n", oFNBatchQCFault.FNBQCFaultID, oFNBatchQCFault.FNBatchQCDetailID, oFNBatchQCFault.FPFID, oFNBatchQCFault.FaultPoint, oFNBatchQCFault.NoOfFault, nUserID, nDBOperation);

        }
        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFNBQCFaultID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FNBatchQCFault WHERE FNBQCFaultID=%n", nFNBQCFaultID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }


        #endregion
    }
}
