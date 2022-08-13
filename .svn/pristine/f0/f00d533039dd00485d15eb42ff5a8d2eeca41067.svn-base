using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class FabricYarnOrderAllocateDA
    {
        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, FabricYarnOrderAllocate oFYOA, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricYarnOrderAllocate] " + " %n,  %n , %n , %n , %n, %n , %n", oFYOA.FYOAID, oFYOA.FYOID, oFYOA.WUID, oFYOA.LotID, oFYOA.Qty, nUserID, nDBOperation);
       

        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFYOAID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricYarnOrderAllocate WHERE FYOAID=%n", nFYOAID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
