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
    public class ImportPIAttachmentDA
    {
      

        #region Insert Function

        public static void Insert(TransactionContext tc, ImportPIAttachment oImportPIAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oImportPIAttachment.AttatchFile;

            string sSQL = SQLParser.MakeSQL("INSERT INTO ImportPIAttachment(ImportPIAttachmentID,ImportPIID,AttatchmentName,AttatchFile,FileType,Remarks,DBUserID,DBServerDateTime)"
            + " VALUES(%n, %n, %s, %q, %s, %s, %n, %D)",
            oImportPIAttachment.ImportPIAttachmentID, oImportPIAttachment.ImportPIID, oImportPIAttachment.AttatchmentName, "@file",  oImportPIAttachment.FileType, oImportPIAttachment.Remarks,  nUserID, DateTime.Now);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        #endregion

        #region Update Function
        public static void Update(TransactionContext tc, ImportPIAttachment oImportPIAttachment, Int64 nUserID)
        {
            SqlParameter fileparameter = new SqlParameter();
            fileparameter.SqlDbType = SqlDbType.VarBinary;
            fileparameter.ParameterName = "file";
            fileparameter.Value = oImportPIAttachment.AttatchFile;

            string sSQL = SQLParser.MakeSQL("UPDATE ImportPIAttachment SET ImportPIID=%n,AttatchmentName=%s,AttatchFile=%q,FileType%s,Remarks=%s,DBUserID=%n,DBServerDateTime=%D"
            + " WHERE ImportPIAttachmentID=%n", oImportPIAttachment.ImportPIID, oImportPIAttachment.AttatchmentName, "@file", oImportPIAttachment.FileType, oImportPIAttachment.Remarks, nUserID, DateTime.Now, oImportPIAttachment.ImportPIAttachmentID);
            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, fileparameter);
        }

        public static void ResetApproved(TransactionContext tc, ImportPIAttachment oImportPIAttachment, Int64 nUserID)
        {
            tc.ExecuteNonQuery("UPDATE ImportPIAttachment  SET IsApproved=0 WHERE ImportPIID=%n", oImportPIAttachment.ImportPIID);
        }

        //public static void Approved(TransactionContext tc, ImportPIAttachment oImportPIAttachment, Int64 nUserID)
        //{
        //    tc.ExecuteNonQuery("UPDATE ImportPIAttachment  SET IsApproved=1, DBUserID=%n,DBServerDateTime=%D WHERE ImportPIAttachmentID=%n",nUserID,   DateTime.Now, oImportPIAttachment.ImportPIAttachmentID);
        //}

        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("DELETE FROM ImportPIAttachment WHERE ImportPIAttachmentID=%n", nID);
        }
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ImportPIAttachment", "ImportPIAttachmentID");
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIAttachment WHERE ImportPIID=%n", id);
        }

        public static IDataReader GetWithAttachFile(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM ImportPIAttachment WHERE ImportPIAttachmentID=%n", id);
        }

        public static IDataReader Get(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPIAttachment WHERE ImportPIAttachmentID=%n", id);
        }
        #endregion
    }
}
