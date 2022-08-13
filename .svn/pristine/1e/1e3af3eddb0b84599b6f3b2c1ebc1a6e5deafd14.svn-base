using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
namespace ESimSol.Services.DataAccess
{


    public class LedgerGroupSetupDA
    {
        public LedgerGroupSetupDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, LedgerGroupSetup oLedgerGroupSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sLedgerGroupSetupIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LedgerGroupSetup]"
                                   + "%n, %n, %s, %s, %b, %n, %n, %s",
                                   oLedgerGroupSetup.LedgerGroupSetupID,oLedgerGroupSetup.OCSID,oLedgerGroupSetup.LedgerGroupSetupName,oLedgerGroupSetup.Note,oLedgerGroupSetup.IsDr, nUserID,(int)eEnumDBOperation,sLedgerGroupSetupIDs);
        }

        #endregion
        #region Update Function
        public static void Delete(TransactionContext tc, LedgerGroupSetup oLedgerGroupSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sLedgerGroupSetupIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_LedgerGroupSetup]"
                                   + "%n, %n, %s, %s, %b, %n, %n, %s",
                                   oLedgerGroupSetup.LedgerGroupSetupID, oLedgerGroupSetup.OCSID, oLedgerGroupSetup.LedgerGroupSetupName, oLedgerGroupSetup.Note, oLedgerGroupSetup.IsDr, nUserID, (int)eEnumDBOperation, sLedgerGroupSetupIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, Int64 nID)
        {
            return tc.ExecuteReader("SELECT * FROM LedgerGroupSetup WHERE LedgerGroupSetupID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM LedgerGroupSetup WHERE OCSID IN (SELECT OperationCategorySetupID FROM OperationCategorySetup)");
        }
        public static IDataReader Gets(TransactionContext tc, int nid)
        {
            return tc.ExecuteReader("SELECT * FROM LedgerGroupSetup WHERE OCSID=%n", nid);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }

}
