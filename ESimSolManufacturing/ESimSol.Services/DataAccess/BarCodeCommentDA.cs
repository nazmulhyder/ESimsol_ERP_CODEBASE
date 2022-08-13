using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class BarCodeCommentDA
    {
        public BarCodeCommentDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, BarCodeComment oBarCodeComment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_BarCodeComment]" + "%n, %n, %s, %n, %n, %s",
                                    oBarCodeComment.BarCodeCommentID, oBarCodeComment.OrderRecapID, oBarCodeComment.Comments, nUserID,  (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, BarCodeComment oBarCodeComment, EnumDBOperation eEnumDBOperation, string sBarCodeCommentIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_BarCodeComment]" + "%n, %n, %s, %n, %n, %s",
                                    oBarCodeComment.BarCodeCommentID, oBarCodeComment.OrderRecapID, oBarCodeComment.Comments, nUserID, (int)eEnumDBOperation, sBarCodeCommentIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BarCodeComment WHERE BarCodeCommentID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_BarCodeComment");
        }
        public static IDataReader Gets(TransactionContext tc, int nOrderRecapID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BarCodeComment WHERE OrderRecapID=%n", nOrderRecapID);
        }
        public static IDataReader GetsForLog(TransactionContext tc, int nOrderRecapLogID)
        {
            return tc.ExecuteReader("SELECT * FROM View_BarCodeCommentLog WHERE OrderRecapLogID=%n", nOrderRecapLogID);
        }
        
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
