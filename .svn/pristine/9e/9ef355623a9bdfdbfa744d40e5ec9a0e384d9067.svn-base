using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class OperationCategorySetupDA
    {
        public OperationCategorySetupDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, OperationCategorySetup oOperationCategorySetup, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sStatementIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_OperationCategorySetup]"
                                   + "%n, %n, %s,%n, %s, %n, %n, %s",
                                   oOperationCategorySetup.OperationCategorySetupID, oOperationCategorySetup.StatementSetupID, oOperationCategorySetup.CategorySetupName,oOperationCategorySetup.DebitCredit, oOperationCategorySetup.Note, (int)eEnumDBOperation, nUserID, sStatementIDs);
        }

        #endregion

        #region Update Function
        public static void Delete(TransactionContext tc, OperationCategorySetup oOperationCategorySetup, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sStatementIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_OperationCategorySetup]"
                                   + "%n, %n, %s,%n, %s, %n, %n, %s",
                                   oOperationCategorySetup.OperationCategorySetupID, oOperationCategorySetup.StatementSetupID, oOperationCategorySetup.CategorySetupName,oOperationCategorySetup.DebitCredit, oOperationCategorySetup.Note, (int)eEnumDBOperation, nUserID, sStatementIDs);
        }        
        #endregion


        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, Int64 nID)
        {
            return tc.ExecuteReader("SELECT * FROM [VIEW_OperationCategorySetup] WHERE OperationCategorySetupID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM VIEW_OperationCategorySetup");
        }
        public static IDataReader Gets(TransactionContext tc, int nStatementSetupID)
        {
            return tc.ExecuteReader("select * from VIEW_OperationCategorySetup WHERE StatementSetupID=%n", nStatementSetupID);
        }
        #endregion
    }
   
}
