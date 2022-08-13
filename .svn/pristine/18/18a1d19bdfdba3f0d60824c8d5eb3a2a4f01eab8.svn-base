using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ReportCommentsDA
    {
        public ReportCommentsDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ReportComments oRC, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ReportComments]"
                                    + "%n, %d, %s, %n, %n",
                                    oRC.RCID, oRC.CommentDate, oRC.Note, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, ReportComments oRC, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ReportComments]"
                                    + "%n, %d, %s, %n, %n",
                                    oRC.RCID, oRC.CommentDate, oRC.Note, nUserId, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nRCID)
        {
            return tc.ExecuteReader("SELECT * FROM ReportComments WHERE RCID=%n", nRCID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
