using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{

    public class BUWiseSubLedgerDA
    {
        public BUWiseSubLedgerDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BUWiseSubLedger oBUWiseSubLedger, EnumDBOperation eEnumDBOperation, Int64 nUserId, string BUWiseSubLedgerIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BUWiseSubLedger]"
                                    + "%n, %n, %n, %n, %n, %s"
                                    , oBUWiseSubLedger.BUWiseSubLedgerID, oBUWiseSubLedger.BusinessUnitID, oBUWiseSubLedger.SubLedgerID, nUserId, (int)eEnumDBOperation, BUWiseSubLedgerIDs);
        }

        public static void Delete(TransactionContext tc, BUWiseSubLedger oBUWiseSubLedger, EnumDBOperation eEnumDBOperation, Int64 nUserId, string BUWiseSubLedgerIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BUWiseSubLedger]"
                                    + "%n, %n, %n, %n, %n, %s"
                                    , oBUWiseSubLedger.BUWiseSubLedgerID, oBUWiseSubLedger.BusinessUnitID, oBUWiseSubLedger.SubLedgerID, nUserId, (int)eEnumDBOperation, BUWiseSubLedgerIDs);
        }

        public static void IUDFromCC(TransactionContext tc, BUWiseSubLedger oBUWiseSubLedger, string sBusinessUnitIDs, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_SubledgerWiseBusinessUnit]" + "%n, %s, %n", oBUWiseSubLedger.SubLedgerID, sBusinessUnitIDs, nUserId);
        }
        public static void CopyBasicChartOfAccount(TransactionContext tc, int nCompanyID, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC[SP_Copy_BasicChartOfAccount]"
                                    + "%n, %n ", nCompanyID, nUserId);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM BUWiseSubLedger WHERE BUWiseSubLedgerID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM BUWiseSubLedger");
        }
        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM BUWiseSubLedger where BusinessUnitID=%n ", nBUID);
        }
        public static IDataReader GetsByCC(TransactionContext tc, int nAHID)
        {
            return tc.ExecuteReader("SELECT * FROM BUWiseSubLedger where SubLedgerID=%n ", nAHID);
        }

        public static IDataReader GetsLeftSelectedBUWiseSubLedger(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
 
}
