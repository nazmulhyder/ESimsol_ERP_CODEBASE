using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
   public class LotBaseTestDA
    {
       public LotBaseTestDA() { }
        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, LotBaseTest oLotBaseTest, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LotBaseTest] " + " %n, %n ,%s ,%b ,%b ,%n ,%n  ", oLotBaseTest.LotBaseTestID, oLotBaseTest.BUID, oLotBaseTest.Name, oLotBaseTest.Activity,oLotBaseTest.IsRaw ,nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nLotBaseTestID)
        {
            return tc.ExecuteReader("SELECT * FROM LotBaseTest WHERE LotBaseTestID=%n", nLotBaseTestID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
