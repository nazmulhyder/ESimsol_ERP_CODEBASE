using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class SubledgerRefConfigDA
    {
        #region Insert Update Delete Function
        public static void InsertUpdate(TransactionContext tc, SubledgerRefConfig oSubledgerRefConfig, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SubledgerRefConfig]" + "%n, %n, %n, %b, %b, %n, %n",
                                    oSubledgerRefConfig.SubledgerRefConfigID, oSubledgerRefConfig.AccountHeadID, oSubledgerRefConfig.SubledgerID, oSubledgerRefConfig.IsBillRefApply, oSubledgerRefConfig.IsOrderRefApply, nUserID, (int)eEnumDBOperation);
        }
        public static void Delete(TransactionContext tc, string sAccountHeadIDs, int nSubledgerID)
        {
            tc.ExecuteNonQuery("DELETE FROM SubledgerRefConfig WHERE AccountHeadID NOT IN (SELECT * FROM  dbo.SplitInToDataSet('" + sAccountHeadIDs + "',',')) AND SubLedgerID = " + nSubledgerID.ToString());
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nAccountHeadID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SubledgerRefConfig WHERE AccountHeadID=%n", nAccountHeadID);
        }
        public static IDataReader GetDependencies(TransactionContext tc, int nAccountHeadID, int nSubledgerID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SubledgerRefConfig WHERE AccountHeadID=%n AND SubledgerID=%n", nAccountHeadID, nSubledgerID);
        }
        public static IDataReader Gets(TransactionContext tc, int nSubledgerID)
        {
            return tc.ExecuteReader("SELECT * FROM View_SubledgerRefConfig WHERE SubledgerID=%n", nSubledgerID);
        }        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
