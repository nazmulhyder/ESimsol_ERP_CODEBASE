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
    public class ImageCommentDA
    {
        public ImageCommentDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, ImageComment oImageComment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImageComment]" + "%n, %n, %s, %n, %n, %s",
                                    oImageComment.ImageCommentID, oImageComment.TechnicalSheetID, oImageComment.Comments, nUserID,  (int)eEnumDBOperation, "");
        }

        public static void Delete(TransactionContext tc, ImageComment oImageComment, EnumDBOperation eEnumDBOperation, string sImageCommentIDs, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImageComment]" + "%n, %n, %s, %n, %n, %s",
                                    oImageComment.ImageCommentID, oImageComment.TechnicalSheetID, oImageComment.Comments, nUserID, (int)eEnumDBOperation, sImageCommentIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM ImageComment WHERE ImageCommentID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM ImageComment");
        }
        public static IDataReader Gets(TransactionContext tc, int nTechnicalSheetID)
        {
            return tc.ExecuteReader("SELECT * FROM ImageComment WHERE TechnicalSheetID=%n", nTechnicalSheetID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
}
