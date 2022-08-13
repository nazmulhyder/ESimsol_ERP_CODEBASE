using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{

    public class StatementSetupDA
    {
        public StatementSetupDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, StatementSetup oStatementSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_StatementSetup]"
                                    + "%n, %s,%n, %s, %n, %n",
                                    oStatementSetup.StatementSetupID, oStatementSetup.StatementSetupName,oStatementSetup.AccountsHeadDefineNature, oStatementSetup.Note, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, StatementSetup oStatementSetup, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_StatementSetup]"
                                    + "%n, %s,%n, %s, %n, %n",
                                    oStatementSetup.StatementSetupID, oStatementSetup.StatementSetupName,oStatementSetup.AccountsHeadDefineNature, oStatementSetup.Note, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM StatementSetup WHERE StatementSetupID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM StatementSetup Order By [StatementSetupName]");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
