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
    public class DispoProductionAttachmentDA
    {
        public DispoProductionAttachmentDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DispoProductionAttachment oDispoProductionAttachment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DispoProductionAttachment]" + "%n, %n, %s, %n, %n",
                                    oDispoProductionAttachment.DispoProductionAttachmentID, oDispoProductionAttachment.DispoProductionCommentID, oDispoProductionAttachment.FileName, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DispoProductionAttachment oDispoProductionAttachment, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DispoProductionAttachment]" + "%n, %n, %s, %n, %n",
                                    oDispoProductionAttachment.DispoProductionAttachmentID, oDispoProductionAttachment.DispoProductionCommentID, oDispoProductionAttachment.FileName, nUserID, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM DispoProductionAttachment WHERE DispoProductionAttachmentID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM DispoProductionAttachment ORDER BY DispoProductionAttachmentID ASC");
        }
        public static IDataReader GetsByDispoProductionAttachment(TransactionContext tc, string sDispoProductionAttachment)
        {
            return tc.ExecuteReader("SELECT * FROM DispoProductionAttachment WHERE DispoProductionAttachmentName LIKE ('%" + sDispoProductionAttachment + "%') ORDER BY DispoProductionAttachmentID ASC");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
