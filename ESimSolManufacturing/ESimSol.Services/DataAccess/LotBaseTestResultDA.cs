using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{
    public class LotBaseTestResultDA
    {
        public LotBaseTestResultDA() { }
        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, LotBaseTestResult oLotBaseTestResult, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LotBaseTestResult] " + "%n, %n ,%n ,%n ,%s ,%n ,%n ", oLotBaseTestResult.LotBaseTestResultID, oLotBaseTestResult.LotBaseTestID, oLotBaseTestResult.LotID, oLotBaseTestResult.Qty, oLotBaseTestResult.Note, nUserID, nDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nLotBaseTestResultID)
        {
            return tc.ExecuteReader("SELECT * FROM LotBaseTestResult WHERE LotBaseTestResultID=%n", nLotBaseTestResultID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
