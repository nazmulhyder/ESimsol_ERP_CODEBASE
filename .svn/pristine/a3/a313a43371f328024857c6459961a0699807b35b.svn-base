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
    public class DispoProductionCommentViewerDA
    {
        #region Insert Update Delete Function

        public static IDataReader InsertUpdate(TransactionContext tc, DispoProductionCommentViewer oDispoProductionCommentViewer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DispoProductionCommentViewer]"
                                   + "%n,%n,%n,%n", oDispoProductionCommentViewer.DispoProductionCommentViewerID, oDispoProductionCommentViewer.DispoProductionCommentID, nUserID, (int)eEnumDBOperation);
        }

        //public static void Delete(TransactionContext tc, DispoProductionCommentViewer oDispoProductionCommentViewer, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        //{
        //    tc.ExecuteNonQuery("EXEC [SP_IUD_DispoProductionCommentViewer]"
        //                           + "%n,%n,%s,%n,%n", oDispoProductionCommentViewer.DispoProductionCommentViewerID, oDispoProductionCommentViewer.FSCDID, oDispoProductionCommentViewer.Comment, nUserID, (int)eEnumDBOperation);
        //}

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DispoProductionCommentViewer WHERE DispoProductionCommentViewerID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DispoProductionCommentViewer ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }

}
