using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportLCClauseSetupDA
    {
        public ImportLCClauseSetupDA() { }

        #region Insert Function
        public static IDataReader InsertUPdate(TransactionContext tc, ImportLCClauseSetup oILCS, EnumDBOperation eDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCClauseSetup]"
                                    + " %n ,%s, %n ,%n ,%n ,%n ,%n ,%b , %b ,%n ,%n",
                                    oILCS.ImportLCClauseSetupID, oILCS.Clause,oILCS.BUID,oILCS.SL,oILCS.LCPaymentType,oILCS.LCAppType,oILCS.ProductType,oILCS.IsMandatory,oILCS.Activity,  nUserID, (int)eDBOperation);
        }
        public static IDataReader IUD(TransactionContext tc, ImportLCClauseSetup oILCS, int eDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportLCClauseSetup]"
                                    + " %n ,%s, %n ,%n ,%n ,%n ,%n ,%b , %b ,%n ,%n",
                                    oILCS.ImportLCClauseSetupID, oILCS.Clause, oILCS.BUID, oILCS.SL, oILCS.LCPaymentType, oILCS.LCAppType, oILCS.ProductType, oILCS.IsMandatory, oILCS.Activity, nUserID, eDBOperation);
        }
        #endregion

        

        #region Delete Function
        public static void Delete(TransactionContext tc, ImportLCClauseSetup oImportLCClauseSetup, EnumDBOperation eDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportLCClauseSetup]"
                                    + "%n, %s, %b,%n, %n, %n",
                                    oImportLCClauseSetup.ImportLCClauseSetupID, oImportLCClauseSetup.Clause, oImportLCClauseSetup.Activity, oImportLCClauseSetup.BUID, nUserID, (int)eDBOperation);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ImportLCClauseSetup", "ImportLCClauseSetupID");
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM ImportLCClauseSetup WHERE ImportLCClauseSetupID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ImportLCClauseSetup Order By [ImportLCClauseSetupID]");
        }

        public static IDataReader GetsActiveImportLCClauseSetup(TransactionContext tc)
        {
            return tc.ExecuteReader("select * from ImportLCClauseSetup where ImportLCClauseSetup.Activity=1");
        }

        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM ImportLCClauseSetup WHERE BUID=%n", nBUID);
        }
        public static IDataReader GetsWithSQL(TransactionContext tc, String sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }


}
