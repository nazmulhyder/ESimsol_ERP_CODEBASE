using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class FabricYarnOrderDA
    {
        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, FabricYarnOrder oFYO, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_FabricYarnOrder] " + " %n,  %n , %n , %n , %n , %n , %n", oFYO.FYOID, oFYO.FEOID, oFYO.ProductID, oFYO.Qty, oFYO.UnitPrice, nUserID, nDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nFYOID)
        {
            return tc.ExecuteReader("SELECT * FROM View_FabricYarnOrder WHERE FYOID=%n", nFYOID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
