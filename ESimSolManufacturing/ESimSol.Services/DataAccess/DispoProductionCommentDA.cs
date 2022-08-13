using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{
    public class DispoProductionCommentDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DispoProductionComment oDispoProductionComment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DispoProductionComment]"
                                   + "%n,%n,%s,%b,%n,%n", oDispoProductionComment.DispoProductionCommentID, oDispoProductionComment.FSCDID, oDispoProductionComment.Comment, oDispoProductionComment.IsOwn, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DispoProductionComment oDispoProductionComment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DispoProductionComment]"
                                   + "%n,%n,%s,%b,%n,%n", oDispoProductionComment.DispoProductionCommentID, oDispoProductionComment.FSCDID, oDispoProductionComment.Comment, oDispoProductionComment.IsOwn, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DispoProductionComment WHERE DispoProductionCommentID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DispoProductionComment ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsBySP(TransactionContext tc, int nFSCDID, long nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_DispoProductionCommentViewer]" + "%n, %n", nFSCDID, nUserID);
        }
        #endregion
    }

}
