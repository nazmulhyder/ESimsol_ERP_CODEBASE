using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class CommentsHistoryDA
    {
        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CommentsHistory oCommentsHistory, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CommentsHistory]"
                                    + "%n, %n, %n,%s,%s, %D, %n,%n",
                                     oCommentsHistory.CommentsHistoryID, oCommentsHistory.ModuleID, oCommentsHistory.ModuleObjID, oCommentsHistory.CommentsBy, oCommentsHistory.CommentsText, oCommentsHistory.CommentsDateSt, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, CommentsHistory oCommentsHistory, EnumDBOperation eEnumDBOperation, int nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CommentsHistory]"
                                    + "%n, %n, %n,%s,%s, %D, %n,%n",
                                     oCommentsHistory.CommentsHistoryID, oCommentsHistory.ModuleID, oCommentsHistory.ModuleObjID, oCommentsHistory.CommentsBy, oCommentsHistory.CommentsText, oCommentsHistory.CommentsDateSt, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM CommentsHistory");
        }
        public static IDataReader Get(TransactionContext tc, int nCommentsHistoryID)
        {
            return tc.ExecuteReader("SELECT * FROM CommentsHistory WHERE CommentsHistoryID=%n", nCommentsHistoryID);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}